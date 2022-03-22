#! /usr/bin/env python

import rospy
from geometry_msgs.msg import PoseStamped
from sit_robot_srv.srv import *


class Nav(object):
    def __init__(self, current_pose=PoseStamped()):
        self.current_pose = current_pose

    def movetogo(self, pose=PoseStamped()):

        try:
            rospy.wait_for_service('/moveToGoal/moveToGoal')
            movetogoal_client = rospy.ServiceProxy('/moveToGoal/moveToGoal', moveToGoal)
            print "Requesting %s" % pose.header.frame_id
            resp1 = movetogoal_client.call(moveToGoalRequest(
                pose.header.frame_id,
                pose.pose.position.x,
                pose.pose.position.y,
                pose.pose.position.z,
                pose.pose.orientation.x,
                pose.pose.orientation.y,
                pose.pose.orientation.z,
                pose.pose.orientation.w))
            print "Service call is %s" % resp1.isSuccess
            return resp1.isSuccess

        except rospy.ServiceException, e:
            print "Service call failed: %s" % e

    def LocationCallBack(self, data=PoseStamped()):
        self.current_pose = data

    def getLocation(self):
        rospy.Subscriber('/current_position/current_position', PoseStamped, self.LocationCallBack)
        rate = rospy.Rate(1.0)
        rate.sleep()
        return self.current_pose