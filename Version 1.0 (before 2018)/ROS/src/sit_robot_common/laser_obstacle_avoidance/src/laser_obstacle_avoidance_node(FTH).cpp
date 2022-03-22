/*
 * laser_obstacle_avoidance_node.cpp
 *
 *  Created on: 2014年10月3日
 *      Author: johnchen
 */
#include "ros/ros.h"
#include "sensor_msgs/LaserScan.h"
#include <geometry_msgs/Twist.h>
#include <stdio.h>
#define RVALUE 2.0;
class LaserScan {
public:
	float data[6];
	float min_range;
};

float min_range, min_range_angle;
ros::Publisher vel_pub;
geometry_msgs::Twist cmdvel;
LaserScan laserscan[72];
void chatterCallback(const sensor_msgs::LaserScan::ConstPtr& msg) {
	laserscan[0].data[0] = msg->ranges[45.5];
	float degree = 45.5;
	for (int i = 71; i >= 0; i--) {
		laserscan[i].min_range = msg->ranges[degree];
		for (int j = 1; j < 6; j++) {
			laserscan[i].data[j] = msg->ranges[degree];
			if (laserscan[i].data[j] < laserscan[i].min_range) {
				laserscan[i].min_range = laserscan[i].data[i];
			}
			printf("laserscan[%d].data[%d] = [%f]\n",i, j, laserscan[i].data[j]);
			degree = degree + 0.5;
		}
		printf("%f  laserscan[%d].min = [%f]\n",degree, i, laserscan[i].min_range);
	}
	LaserScan waitlaser[72];
	for(int i = 0;i<72;i++){
		if(laserscan[i].min_range >= 2.0){
			waitlaser[i] = laserscan[i];
		}

	}
}
//void chatterCallback(const sensor_msgs::LaserScan::ConstPtr& msg)
//{
//	printf("Position: [%f] [%f]\n", msg->angle_min,msg->angle_max);
//	printf("Position: [%f] [%f]\n", msg->range_min,msg->range_max);
//		min_range=msg->ranges[0];
//		min_range_angle=0;
//		for(int j=0;j<=180;j++) //increment by one degree
//			{
//			printf("%d = [%f] \n", j, msg->ranges[j]);
//			  	if(msg->ranges[j]<min_range)
//				{
//					min_range=msg->ranges[j];
//					min_range_angle=j/2;
//				}
//			}
//			printf("minimum range is [%f] at an angle of [%f]\n",min_range,min_range_angle);
//		if(min_range<=2.5)  // min_range<=0.5 gave box pushing like behaviour, min_range<=1.2 gave obstacle avoidance
//		{
//			if(min_range_angle<90)
//			{
//				 //cmdvel.angular.z=1.0;
//				//cmdvel.linear.y = 0.1;
//				 cmdvel.linear.x=0;
//				 printf("left\n");
//			}
//			else
//			{
//				 //cmdvel.angular.z=-1.0;
//				//cmdvel.linear.y = -0.1;
//				cmdvel.linear.x=0;
//				 printf("right\n");
//			}
//			//cmdvel.linear.x=-0.1;
//		}
//		else
//		{
//			//cmdvel.linear.x=0.1;
//			cmdvel.angular.z=0;
//			printf("straight\n");
//		}
//
//		 vel_pub.publish(cmdvel);
//
//}

int main(int argc, char **argv) {

	ros::init(argc, argv, "laser_messages");
	ros::NodeHandle n;
	ros::NodeHandle nh;
	vel_pub = nh.advertise<geometry_msgs::Twist>("cmd_vel", 1);
	// instead of pr2, if the node has to be used for roomba use "cmd_vel" instead of "base_controller/command"
	cmdvel.linear.x = 0;
	cmdvel.linear.y = 0;
	cmdvel.linear.z = 0;
	cmdvel.angular.x = 0;
	cmdvel.angular.y = 0;
	cmdvel.angular.z = 0;

	ros::Subscriber sub = n.subscribe("base_scan", 1, chatterCallback);

	ros::spin();

	return 0;
}

