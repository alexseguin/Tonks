#include <iostream>

#include <tonks_net.h>


// constraint the message types
enum class CustomMsgTypes : uint32_t
{
	FireBullet,
	MovePlayer
};

int main()
{
	tonks::net::message<CustomMsgTypes> msg;
	msg.header.id = CustomMsgTypes::FireBullet;

	int a = 1;
	bool b = true;
	float c = 3.14159f;

	struct
	{
		float x;
		float y;
	}d[5];

	msg << a << b << c << d;

	std::cout << msg << std::endl;

	a = 99;
	b = false;
	c = 99.0f;

	msg >> d >> c >> b >> a;

	return 0;
}


