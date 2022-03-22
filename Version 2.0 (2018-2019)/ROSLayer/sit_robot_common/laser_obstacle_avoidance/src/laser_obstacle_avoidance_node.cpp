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
#include <iostream>
#define RVALUEMAX 0.4 //阈值最大
#define RVALUEMIN 0.0055//阈值最小
class LaserScan {
public:
	float data;
	float min_range;
	bool IsSafe;
};

ros::Publisher vel_pub;
geometry_msgs::Twist cmdvel;
LaserScan laserscan[6];
void chatterCallback(const sensor_msgs::LaserScan::ConstPtr& msg) {

	cmdvel.linear.x = 0;
	cmdvel.linear.y = 0;
	cmdvel.angular.z = 0;
	
	vel_pub.publish(cmdvel);

	int degree = 0;
	for (int i = 0; i < 6; i++) {
		if (msg->ranges[degree] > RVALUEMIN) {
			laserscan[i].min_range = msg->ranges[degree];
		} else {
			laserscan[i].min_range = RVALUEMIN;
		}
		for (int j = 0; j < 180; j++) {
			laserscan[i].data = msg->ranges[degree];
			if (laserscan[i].data > RVALUEMIN) {
				if (laserscan[i].data < laserscan[i].min_range) {
					laserscan[i].min_range = laserscan[i].data;
				}
			}
//			printf("%d laserscan[%d].data = [%f]\n", degree, i,
//					laserscan[i].data);
			degree++;
		}
//		printf("laserscan[%d].min = [%f]\n", i, laserscan[i].min_range);
	}

	for (int i = 0; i < 6; i++) {
		if (laserscan[i].min_range < RVALUEMAX
				&& laserscan[i].min_range > RVALUEMIN) {
			laserscan[i].IsSafe = false;
			ROS_INFO("%d", i);
		} else {
			laserscan[i].IsSafe = true;
		}
	}

//	if(min_range==laserscan[0].min_range   laserscan[0].min_range>RVALUE){min_range = 0; laserscan[0].IsSafe = true;}
	printf("%f",laserscan[0].min_range-laserscan[5].min_range);
	if (!laserscan[0].IsSafe && !laserscan[1].IsSafe && !laserscan[4].IsSafe
			&& !laserscan[5].IsSafe) {
		cmdvel.linear.x = 0.1;
	} else if (!laserscan[0].IsSafe && !laserscan[2].IsSafe) {
		if (laserscan[3].IsSafe) {
			cmdvel.linear.x = 0.1;
			cmdvel.linear.y = 0.1;
		} else if (laserscan[4].IsSafe || laserscan[5].IsSafe) {
			cmdvel.linear.y = 0.1;
		}
	} else if (!laserscan[3].IsSafe && !laserscan[5].IsSafe) {
		if (laserscan[1].IsSafe) {
			cmdvel.linear.x = 0.1;
			cmdvel.linear.y = -0.1;
		} else if (laserscan[0].IsSafe || laserscan[1].IsSafe) {
			cmdvel.linear.y = 0.1;
		}
	} else if ((!laserscan[0].IsSafe && !laserscan[3].IsSafe)
			|| (!laserscan[0].IsSafe && !laserscan[4].IsSafe)) {
		if (laserscan[5].IsSafe) {
			cmdvel.linear.x = 0.1;
		} else if (laserscan[1].IsSafe || laserscan[2].IsSafe) {
			cmdvel.linear.x = 0.1;
			cmdvel.linear.y = -0.1;
		}
	} else if ((!laserscan[5].IsSafe && !laserscan[2].IsSafe)
			|| (!laserscan[5].IsSafe && !laserscan[1].IsSafe)) {
		if (laserscan[0].IsSafe || laserscan[1].IsSafe) {
			cmdvel.linear.x = 0.1;
		} else if (laserscan[3].IsSafe || laserscan[4].IsSafe) {
			cmdvel.linear.x = 0.1;
			cmdvel.linear.y = 0.1;
		}
	} else if (!laserscan[0].IsSafe || !laserscan[1].IsSafe) {
		std::cout << "1";
		if (laserscan[5].IsSafe) {
			cmdvel.linear.y = 0.1;
		} else if (laserscan[4].IsSafe) {
			cmdvel.linear.x = 0.1;
			cmdvel.linear.y = 0.1;
		}
	} else if (!laserscan[4].IsSafe || !laserscan[5].IsSafe) {
		std::cout << "2";
		if (laserscan[0].IsSafe) {
			cmdvel.linear.y = -0.1;
		} else if (laserscan[1].IsSafe) {
			cmdvel.linear.x = 0.1;
			cmdvel.linear.y = -0.1;
		}
	} else if (!laserscan[2].IsSafe || !laserscan[3].IsSafe) {
		std::cout << "3";
		if (laserscan[0].IsSafe) {
			cmdvel.linear.y = -0.1;
		} else if (laserscan[5].IsSafe) {
			cmdvel.linear.y = 0.1;
		} else if ((laserscan[0].min_range - laserscan[5].min_range) < 0) {
			cmdvel.linear.y = 0.1;
		} else if ((laserscan[0].min_range - laserscan[5].min_range) > 0) {
			cmdvel.linear.y = -0.1;
		}
	} else {
		//cmdvel.linear.x = 0;
		//cmdvel.linear.y = 0;
		//cmdvel.angular.z = 0;
	}
	std::cout << cmdvel.linear.x << std::endl;
	std::cout << cmdvel.linear.y << std::endl;
	std::cout << cmdvel.angular.z << std::endl;
	vel_pub.publish(cmdvel);
}

//void chatterCallback(const sensor_msgs::LaserScan::ConstPtr& msg)
//{
//	int degree = 0;
//	for (int i = 0; i < 6; i++) {
//		laserscan[i].min_range = msg->ranges[degree];
//		for (int j = 0; j < 180; j++) {
//			laserscan[i].data = msg->ranges[degree];
//			if(laserscan[i].data < RVALUEMAX && laserscan[i].data > RVALUEMIN){ROS_INFO("laser[%d]=%f",i,laserscan[i].data);}
//			if (laserscan[i].data < laserscan[i].min_range) {
//				laserscan[i].min_range = laserscan[i].data;
//			}
//			degree = degree++;
//		}
//				//printf("%f  laserscan[%d].min = [%f]\n", degree, i,
//				//		laserscan[i].min_range);
//	}
//}

int main(int argc, char **argv) {

	ros::init(argc, argv, "laser_messages");
	ros::NodeHandle n;
	ros::NodeHandle nh;
	vel_pub = nh.advertise<geometry_msgs::Twist>("base_cmd_vel", 1);
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

