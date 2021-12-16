#pragma once
#include "net_common.h"

namespace tonks
{
	namespace net
	{
		template<typename T>
		class tsqueue
		{
		public:
			tsqueue() = default;
			tsqueue(const tsqueue<T>&) = delete;
			virtual ~tsqueue() { clear(); }

		public:
			// returns and maintains item at front of queue
			const T& front()
			{
				std::scoped_lock lock(muxQueue);
				return deqQueue.front();
			}

			// returns and maintains item at back of queue
			const T& back()
			{
				std::scoped_lock lock(muxQueue);
				return deqQueue.back();
			}

			// adds an item to front of queue
			void push_front(const T& item)
			{
				std::scoped_lock lock(muxQueue);
				deqQueue.emplace_front(std::move(item));
			}

			// adds an item to back of queue
			void push_back(const T& item)
			{
				std::scoped_lock lock(muxQueue);
				deqQueue.emplace_back(std::move(item));
			}

			// returns true if queue is empty
			bool empty()
			{
				std::scoped_lock lock(muxQueue);
				return deqQueue.empty();
			}

			// returns number of items in queue
			size_t count()
			{
				std::scoped_lock lock(muxQueue);
				return deqQueue.size();
			}

			// clears queue
			void clear()
			{
				std::scoped_lock lock(muxQueue);
				deqQueue.clear();
			}

			// removes and returns the first items from front of queue
			T pop_front()
			{
				std::scoped_lock lock(muxQueue);
				auto t = std::move(deqQueue.front());
				deqQueue.pop_front();
				return t;				
			}

			// removes and returns the first items from back of queue
			T pop_front()
			{
				std::scoped_lock lock(muxQueue);
				auto t = std::move(deqQueue.back());
				deqQueue.pop_back();
				return t;
			}


		protected:
			std::mutex muxQueue;  // mutually exclusive lock
			std::deque<T> deqQueue;  // double ended queue = deq
		};
	}
}