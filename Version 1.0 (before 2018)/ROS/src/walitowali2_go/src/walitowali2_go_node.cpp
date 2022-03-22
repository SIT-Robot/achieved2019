/*
 * walitowali2_go_node.cpp
 *
 *  Created on: 2015年9月10日
 *      Author: stamdordli
 */
#include "ros/ros.h"
#include <geometry_msgs/Twist.h>

int main(int argc ,char ** argv)
{
  ros::init(argc,argv,"walitowali2_go_node");
  ros::NodeHandle nh_;

  ros::Subscriber sub = nh_.("base_cmd_vel", 1000);
  ros::Publisher chatter_pub = nh_.advertise<geometry_msgs/Twist>("walitowali2_go", 1000);

  ros::Rate loop_rate(10);

  ros::spinOnce();
  return 0;
}
