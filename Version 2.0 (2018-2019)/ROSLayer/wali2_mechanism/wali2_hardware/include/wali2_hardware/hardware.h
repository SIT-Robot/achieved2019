/*
 * hardware.h
 *
 *  Created on: 2015年7月4日
 *      Author: johnchen
 */

#ifndef SRC_HARDWARE_H_
#define SRC_HARDWARE_H_
#include <stdio.h>
#include <stdlib.h>
#include <iostream>
#include <ros/ros.h>
#include <math.h>
#include <geometry_msgs/Twist.h>
#include <wali2_robot_serialcon/SerialPort.h>
#include <wali2_mechanism_msgs/JointControllerState.h>

#define WHEEL_R 0.127 / 2.0
#define PI M_PI
#define L 0.2215

using namespace std;
using namespace wali2_robot_serialcon;
namespace wali2_hardware
{

class hardware
{
public:
	hardware(char *port, ros::NodeHandle nh_);
	virtual ~hardware();
	unsigned short do_crc(unsigned char *message, int len);
	unsigned char *SetSpeed(double vx, double vy, double vth);
	void decodeOdo(char *odoBuf);
	void getSpeed();
	void cmdCommand(const geometry_msgs::Twist::ConstPtr &msg);

private:
	double wheel_v[3];
	ros::Publisher wheel_pub;
	SerialPort *serialport;
};

} // namespace wali2_hardware

#endif /* SRC_HARDWARE_H_ */
