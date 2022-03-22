/*
 * robot_states_publish_node.cpp
 *
 *  Created on: 2014年10月6日
 *      Author: johnchen
 */
#include <ros/ros.h>
#include <geometry_msgs/PoseStamped.h>
#include <tf/transform_listener.h>

int main(int argc, char** argv) {
	ros::init(argc, argv, "robot_states_publish");
	ros::NodeHandle current_position_nh("current_position");
	tf::TransformListener listener;
	ros::Publisher current_position = current_position_nh.advertise<geometry_msgs::PoseStamped>(
			"current_position", 10);
	geometry_msgs::PoseStamped current_pose;
	ros::Rate r(100.0);
	while (ros::ok()) {
		tf::StampedTransform transform;
		try {
			sleep(1);
			listener.lookupTransform("/map", "/base_link", ros::Time(0),
					transform);
		} catch (tf::TransformException &ex) {
			ROS_ERROR("%s", ex.what());
			ros::Duration(1.0).sleep();
		}
		ROS_INFO("%lf||%lf||%lf", transform.getOrigin().x(),
				transform.getOrigin().y(), transform.getOrigin().z());
		ROS_INFO("%lf||%lf||%lf||%lf", transform.getRotation().x(),
				transform.getRotation().y(), transform.getRotation().z(),
				transform.getRotation().w());
		current_pose.header.frame_id = "map";
		current_pose.pose.position.x = transform.getOrigin().x();
		current_pose.pose.position.y = transform.getOrigin().y();
		current_pose.pose.position.z = transform.getOrigin().z();
		current_pose.pose.orientation.x = transform.getRotation().x();
		current_pose.pose.orientation.y = transform.getRotation().y();
		current_pose.pose.orientation.z = transform.getRotation().z();
		current_pose.pose.orientation.w = transform.getRotation().w();
		current_position.publish(current_pose);
		r.sleep();
	}
	return 0;
}

