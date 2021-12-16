#pragma once

#include "net_common.h"

namespace tonks
{
	namespace net
	{
		// Message Header is sent at start of all messages. The template allows us 
		// to use "enum class" to ensure that the messages are valid at compile time
		template <typename T>
		struct message_header 
		{
			T id{};
			uint32_t size = 0;
		};

		template <typename T>
		struct message 
		{
			message_header<T> header{};
			std::vector<uint8_t> body;

			// return size of entire message packet in bytes
			size_t size() const
			{
				return sizeof(message_header<T>) + body.size();
			}

			// Override for std::cout compatibility - produces friendly description of message
			friend std::ostream& operator << (std::ostream& os, const message<T>& msg)
			{
				os << "ID:" << int(msg.header.id) << " Size:" << msg.header.size << std::endl;
				os << "Message:" << std::endl;
				for (const auto& element : msg.body) {
					os << element << " ";
				}
				os << std::endl;
				return os;
			}

			// Pushes any POD-like data into the message buffer
			// treat this like a stack
			template<typename DataType>
			friend message<T>& operator << (message<T>& msg, const DataType& data)
			{
				// check that the type of what we want to push into the body is trivially copyale
				static_assert(std::is_standard_layout<DataType>::value, "The type of this data is too complex to be pushed into the message buffer");

				// cache the current size of vector, as this will be the point we insert the data
				size_t ii = msg.body.size();

				// resize the vector of the body message by the size of the data being pushed into it
				msg.body.resize(msg.body.size() + sizeof(DataType));

				// Physically copy the new data into the newly allocated vector space
				std::memcpy(msg.body.data() + ii, &data, sizeof(DataType));

				// recalculate the message size as we have just changed it
				msg.header.size = msg.size();

				// return the target message so it can be "chained" (whatever the fuck that means;
				return msg;
			}

			// retrieve data from the message the same way it is pushed into it. kinda.
			// treat this like a stack
			template<typename DataType>
			friend message<T>& operator >> (message<T>& msg, DataType& data)
			{
				// Check that the type of the data being pushed is trivially copyable
				static_assert(std::is_standard_layout<DataType>::value, "The type of this data is too complex to be pushed into the message buffer");

				// cache the location towards the end of the vector where the pulled data starts
				size_t ii = msg.body.size() - sizeof(DataType);

				// Physically copy the data from the vector into the user variable
				std::memcpy(&data, msg.body.data() + ii, sizeof(DataType));

				// shrink the vector to remove read bytes, and reset end position
				msg.body.resize(ii);

				// return the target message so it can be "changed"
				return msg;
			}
		};

		// forward declare the connection
		template <typename T>
		class connection;

		template <typename T>
		struct owned_message
		{
			std::shared_ptr<connection<T>> remote = nullptr;
			message<T> msg;

			// Override for std::cout compatibility - produces friendly description of message
			friend std::ostream& operator << (std::ostream& os, const owned_message<T>& msg)
			{
				os << msg.msg;
				return os;
			}
		};
	}
}