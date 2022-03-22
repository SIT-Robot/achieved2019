/*
 * MoveToGoal.cpp
 *
 *  Created on: 2014年8月23日
 *      Author: johnchen
 */

#include <movetogoal/MoveToGoal.h>

namespace movetogoal {

MoveToGoal::MoveToGoal() {
	// TODO Auto-generated constructor stub
	ros::NodeHandle nh("moveToGoal");
	movebase_service = nh.advertiseService("moveToGoal", &MoveToGoal::MoveBase, this);
}

MoveToGoal::~MoveToGoal() {
	// TODO Auto-generated destructor stub
}

bool MoveToGoal::MoveBase(sit_robot_srv::moveToGoal::Request& req,
		sit_robot_srv::moveToGoal::Response& res){
	MoveBaseClient ac("move_base", true);
	while(!ac.waitForServer(ros::Duration(5.0))){
	    ROS_INFO("Waiting for the move_base action server to come up");
	  }
	goal.target_pose.header.frame_id = req.frame_id;
	goal.target_pose.header.stamp = ros::Time::now();

	goal.target_pose.pose.position.x = req.positionx;
	goal.target_pose.pose.position.y = req.positiony;
	goal.target_pose.pose.position.z = req.positionz;
	goal.target_pose.pose.orientation.x = req.oritationx;
	goal.target_pose.pose.orientation.y = req.oritationy;
	goal.target_pose.pose.orientation.z = req.oritationz;
	goal.target_pose.pose.orientation.w = req.oritationw;

	ROS_INFO("Sending goal");
	ac.sendGoal(goal);

	ac.waitForResult();

	if(ac.getState() == actionlib::SimpleClientGoalState::SUCCEEDED){
		res.isSuccess = 1;
	    ROS_INFO("Hooray, the base moved the goal!");
	} else {
		res.isSuccess = 0;
		ROS_INFO("The base failed to move the goal for some reason");
	}


	return true;
}

} /* namespace movetogoal */
