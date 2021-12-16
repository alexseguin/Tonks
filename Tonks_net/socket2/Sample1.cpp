#include <iostream>
#include <chrono>

#ifdef _WIN32
#define _WIN32_WINNT 0x0A00
#endif

#define ASIO_STANDALONE
#include <asio.hpp>
#include <asio/ts/buffer.hpp>
#include <asio/ts/internet.hpp>

std::vector<char> vBuffer(20 * 1024);

void GrabSomeData(asio::ip::tcp::socket& socket) {
	socket.async_read_some(asio::buffer(vBuffer.data(), vBuffer.size()), 
		[&](std::error_code ec, std::size_t length)
		{
			if (!ec)
			{
				std::cout << "\n\nRead " << length << " bytes\n\n";

				for (int ii = 0; ii < length; ii++)
					std::cout << vBuffer[ii];

				GrabSomeData(socket);
			}
		}	
	);
}


int main()
{
	asio::error_code error_code;

	// create a context - essentially the platform specific interface to run ASIO related stuff
	asio::io_context context;

	// give some fake work to the context to prevent the run function from exiting right away
	asio::io_context::work idlework(context);

	// start the context in its own thread so the main program can do whatever in the meantime
	std::thread threadContext = std::thread([&]() {context.run(); });

	// Get the address of somewhere we wish to connext to
	// for now, let's just create a a TCP endpoint which is for all intents and purposes right now, just a TCP IP address
	asio::ip::tcp::endpoint endpoint(asio::ip::make_address("51.38.81.49", error_code), 80);

	// create the socket, the context will deliver the implementation, basically the doorway for our networking
	asio::ip::tcp::socket socket(context);

	// tell the socket to try and connect
	socket.connect(endpoint, error_code);

	// handle some basic error handling
	if (!error_code) {
		std::cout << "Connected!" << std::endl;
	}
	else {
		std::cout << "Failed to connect to address:\n" << error_code.message() << std::endl;
	}


	if (socket.is_open()) {

		// prime the async data capture logic
		GrabSomeData(socket);

		std::string sRequest =
			"GET /index.html HTTP/1.1\r\n"
			"Host: david-barr.co.uk\r\n"
			"Connection: close\r\n\r\n";

		socket.write_some(asio::buffer(sRequest.data(), sRequest.size()), error_code);

		// program does something else while asio handles data transfers in the background
		using namespace std::chrono_literals;
		std::this_thread::sleep_for(20000ms);

		context.stop();
		if (threadContext.joinable()) threadContext.join();

	}

	

	return 0;
}