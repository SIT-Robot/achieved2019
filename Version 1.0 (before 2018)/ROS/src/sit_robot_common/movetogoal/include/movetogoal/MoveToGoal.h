/*
 * MoveToGoal.h
 *
 *  Created on: 2014年8月23日
 *      Author: johnchen
 */

#ifndef MOVETOGOAL_H_
#define MOVETOGOAL_H_
#include <ros/ros.h>
#include <sit_robot_srv/moveToGoal.h>
#include <move_base_msgs/MoveBaseAction.h>
#include <actionlib/client/simple_action_client.h>

namespace movetogoal {
	typedef actionlib::SimpleActionClient<move_base_msgs::MoveBaseAction> MoveBaseClient;
class MoveToGoal {
public:
	MoveToGoal();
	virtual ~MoveToGoal();
	bool MoveBase(sit_robot_srv::moveToGoal::Request& req,
			sit_robot_srv::moveToGoal::Response& res);

private:
	ros::ServiceServer movebase_service;
	move_base_msgs::MoveBaseGoal goal;

};
}/* namespace movetogoal */

#endif /* MOVETOGOAL_H_ */
