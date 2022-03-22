/*
 * wali2_hardware_node.cpp
 *
 *  Created on: 2015年7月5日
 *      Author: johnchen
 */
#include <wali2_hardware/hardware.h>
#include <ros/ros.h>


using namespace wali2_hardware;

int main(int argc, char **argv)
{
	ros::init(argc, argv, "wali2_hardware");
	ros::NodeHandle nh_;
	hardware hw_cmd("/dev/ttyUSB0", nh_);
	ros::Subscriber cmd_sub = nh_.subscribe("base_cmd_vel", 1,
											&hardware::cmdCommand, &hw_cmd);
	ros::AsyncSpinner s(1);


	s.start();

	while (ros::ok())
	{
		hw_cmd.getSpeed();
	}
	return 0;
}
