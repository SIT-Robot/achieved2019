#! /usr/bin/env python

import tutorial_package.hello
import rospy
from sit_robot_hub.Nav import Nav
from geometry_msgs.msg import PoseStamped

if __name__ == '__main__':
    rospy.init_node('my_pkg')
    nav = Nav()
    pose = nav.getLocation()
    print "%s" % pose.header.frame_id