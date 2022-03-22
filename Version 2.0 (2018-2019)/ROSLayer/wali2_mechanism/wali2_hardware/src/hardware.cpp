/*
 * hardware.cpp
 *
 *  Created on: 2015年7月4日
 *      Author: johnchen
 */

#include "wali2_hardware/hardware.h"

namespace wali2_hardware
{

hardware::hardware(char *port, ros::NodeHandle nh_)
{
	// TODO Auto-generated constructor stub
	serialport = new SerialPort(port);
	serialport->Open();
	wheel_pub = nh_.advertise<geometry_msgs::Twist>("/odom_computer", 50);
}

hardware::~hardware()
{
	// TODO Auto-generated destructor stub
	serialport->Close();
}

unsigned short hardware::do_crc(unsigned char *message, int len)
{
	int i, j;
	unsigned short crc_reg = 0;
	unsigned short current;

	for (i = 0; i < len; i++)
	{
		current = message[i] << 8;
		for (j = 0; j < 8; j++)
		{
			if ((short)(crc_reg ^ current) < 0)
			{
				crc_reg = (crc_reg << 1) ^ 0x1021;
			}
			else
			{
				crc_reg <<= 1;
			}
			current <<= 1;
		}
	}
	return crc_reg;
}

unsigned char *hardware::SetSpeed(double vx, double vy, double vth)
{
	unsigned char *buf = new unsigned char[16];
	//	unsigned int x = int(vx * 10638.29787234);

	unsigned int x = int(vx * 10640);
	unsigned int y = int(vy * 10638.29787234);
	unsigned int th = int(vth * 2448);
	unsigned char crc_ccitt_buf[] = {0xAA, 0x40, 0x20, 0x2A, 0x08, 0x0, 0x0,
									 0x0, 0x0, 0x00, 0x00, 0x00, 0x00};
	crc_ccitt_buf[5] = x;
	crc_ccitt_buf[6] = x >> 8;
	crc_ccitt_buf[7] = y;
	crc_ccitt_buf[8] = y >> 8;
	crc_ccitt_buf[9] = th;
	crc_ccitt_buf[10] = th >> 8;
	unsigned int s = do_crc(crc_ccitt_buf, 13);
	for (int i = 0; i < 13; i++)
	{
		buf[i] = crc_ccitt_buf[i];
	}
	buf[13] = s;
	buf[14] = s >> 8;
	buf[15] = 0x0D;
	return buf;
}

void hardware::decodeOdo(char *odoBuf)
{
	int odoEndFlag = 0;
	int odoHeadFlag = 0;
	int odoflag = 1;
	{
		odoHeadFlag = -1;
		odoEndFlag = -1;
	}
	char *next;
	char *pnext;
	unsigned int idx = 0;

	next = strtok_r(odoBuf, ",", &pnext);

	while (next && odoflag)
	{
		switch (++idx)
		{
		case 1:
			if (!strcmp(next, "LL"))
			{
				{
					odoHeadFlag = 1;
				}
			}
			else
				odoflag = 0; //if not LL then stop
			break;
		case 2:
			wheel_v[0] = -strtod(next, NULL);
			//ROS_INFO("v1 : %f", v1);//num of points
			break;
		case 3:
			wheel_v[1] = -strtod(next, NULL); //
			//ROS_INFO("v2 : %f", v2);//num of points
			break;
		case 4:
			wheel_v[2] = -strtod(next, NULL); //
			//ROS_INFO("v3 : %f\n", v3);//num of points
			break;
		case 5:
			if (!strncmp(next, "JJ", 2))
			{
				{
					odoEndFlag = 1;
				}
			}
			break;
		default:
			break;
		}
		next = strtok_r(NULL, ",", &pnext);
	}
}

void hardware::getSpeed()
{
	char *rcv_buf = (char *)malloc(25 * sizeof(char));
	serialport->Read(rcv_buf);
	this->decodeOdo(rcv_buf);
	free(rcv_buf);

	//	double vx = ((sqrt(3) * PI * WHEEL_R) / 1620000) * (wheel_v[1] - wheel_v[2]);
	//	double vy = ((PI * WHEEL_R) / 1620000) * (2 * wheel_v[0] - wheel_v[1] - wheel_v[2]);
	//	double vth = (WHEEL_R / (3 * L)) * (wheel_v[0] + wheel_v[1] + wheel_v[2]);

	double vx = (((sqrt(3) * M_PI * WHEEL_R) / 10800.0) * (wheel_v[1] - wheel_v[0])) / 1.01;
	double vy = ((M_PI * WHEEL_R) / 10800.0) * (2 * wheel_v[2] - wheel_v[1] - wheel_v[0]) / 1.01;
	double vth = -(WHEEL_R * M_PI / (360.0 * 30 * L)) * (wheel_v[0] + wheel_v[1] + wheel_v[2]) / 1.003;

	geometry_msgs::Twist msg;
	msg.linear.x = vx;
	msg.linear.y = vy;
	msg.angular.z = vth;
	wheel_pub.publish(msg);
}

void hardware::cmdCommand(const geometry_msgs::Twist::ConstPtr &msg)
{
	unsigned char *buf = this->SetSpeed(-msg->linear.y, msg->linear.x,
										msg->angular.z);
	serialport->Write(buf);
}

} // namespace wali2_hardware
