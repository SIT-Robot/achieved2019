/*
 * movetogoal_node.cpp
 *
 *  Created on: 2014年8月23日
 *      Author: johnchen
 */

#include <movetogoal/MoveToGoal.h>

int main(int argc, char **argv){
	ros::init(argc, argv, "movetogoal");
	movetogoal::MoveToGoal movetogoal;
	ros::spin();
	return 0;
}


