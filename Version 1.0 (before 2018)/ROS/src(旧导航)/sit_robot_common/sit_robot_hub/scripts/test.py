#!/usr/bin/env python

from Nav import *
import rospy
from people_msgs.msg import PositionMeasurement

if __name__ == '__main__':
    rospy.init_node("sit_robot_hub_test")
    print "start!!"
    nav = Nav()
    # nav.getLocation()
    # currentpose = nav.getLocation()
    # print "%s" % currentpose.header.frame_id
    # print "%f" % currentpose.pose.position.x
    # print "%f" % currentpose.pose.position.y
    # print "%f" % currentpose.pose.position.z
    # print "%f" % currentpose.pose.orientation.w
    # print "%f" % currentpose.pose.orientation.x
    # print "%f" % currentpose.pose.orientation.y
    # print "%f" % currentpose.pose.orientation.z
    #
    # print "stop"
    #
    # pose = PoseStamped()
    # pose.header.frame_id = "base_link"
    # pose.pose.position.x = 2
    # pose.pose.position.y = 0
    # pose.pose.position.z = 0
    # pose.pose.orientation.x = 0
    # pose.pose.orientation.y = 0
    # pose.pose.orientation.z = 0
    # pose.pose.orientation.w = 1
    #
    # nav.movetogo(pose)

    data = nav.hubServe(8081)
    # print nav.HandAction(41)






