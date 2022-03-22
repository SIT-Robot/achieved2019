#!/usr/bin/env python
__author__ = 'johnchen'

from sit_robot_srv.srv import *
import rospy
from geometry_msgs.msg import PoseStamped
import socket
import string
import os
import sys


class Nav(object):
    def __init__(self, current_pose=PoseStamped()):
        self.current_pose = current_pose

    def movetogo(self, pose=PoseStamped()):

        try:
            rospy.wait_for_service('/moveToGoal/moveToGoal')
            movetogoal_client = rospy.ServiceProxy('/moveToGoal/moveToGoal', moveToGoal)
            print "Requesting %s" % pose.header.frame_id
            # print pose.header.frame_id
            # print pose.pose.position.x
            # print pose.pose.position.y
            # print pose.pose.position.z
            # print pose.pose.orientation.x
            # print pose.pose.orientation.y
            # print pose.pose.orientation.z
            # print pose.pose.orientation.w
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

    def HandAction(self, type=0):
        try:
            rospy.wait_for_service('/arm_action/hand_action')
            handaction_client = rospy.ServiceProxy('/arm_action/hand_action', ArmAction)
            print 'handaction: ' + str(type)
            resp1 = handaction_client.call(ArmActionRequest(type))
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

    def hubServe(self, PORT, HOST=''):

        BUFSIZ = 2048
        ADDR = (HOST, PORT)

        tcpServSock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        tcpServSock.bind(ADDR)
        tcpServSock.listen(5)

        print('waiting for connection...')
        print PORT
        while not rospy.is_shutdown():

            tcpClientSock, addr = tcpServSock.accept()
            print('...connected from: ', addr)

            while not rospy.is_shutdown():
                # data = str(tcpClientSock.recv(BUFSIZ), encoding='utf-8')
                data = str(tcpClientSock.recv(BUFSIZ))

                if not data:
                    break
                if "#EM#" in data:
                    f = self.readdata(data)
                    l = f.read(1024)
                    while (l):
                        tcpClientSock.send(l)
                        l = f.read(1024)
                elif "#PM#" in data:
                    f = self.readdata(data)
                    l = f.read(1024)
                    while (l):
                        tcpClientSock.send(l)
                        l = f.read(1024)
                else:
                    tcpClientSock.send(bytes('@%s@' % self.readdata(data)))

            tcpClientSock.close()
        tcpServSock.close()
        return data

    def readdata(self, data):

        if data.count('#') is not 2:
            return 0
        # cut
        list = data.split('@', 8)
        list[0] = list[0][1:]
        list[data.count('@')] = list[data.count('@')][0:list[data.count('@')].find('#')]

        print list

        if "movetogo" in list:
            if len(list) is not 9:
                return 0
            pose = PoseStamped()
            pose.header.frame_id = list[1]
            pose.pose.position.x = string.atof(list[2])
            pose.pose.position.y = string.atof(list[3])
            pose.pose.position.z = string.atof(list[4])
            pose.pose.orientation.x = string.atof(list[5])
            pose.pose.orientation.y = string.atof(list[6])
            pose.pose.orientation.z = string.atof(list[7])
            pose.pose.orientation.w = string.atof(list[8])
            return self.movetogo(pose)
        elif "getLocation" in list:
            if len(list) is not 1:
                return 0
            currentpose = self.getLocation()
            poselist = [currentpose.header.frame_id,
                        currentpose.pose.position.x,
                        currentpose.pose.position.y,
                        currentpose.pose.position.z,
                        currentpose.pose.orientation.x,
                        currentpose.pose.orientation.y,
                        currentpose.pose.orientation.z,
                        currentpose.pose.orientation.w]
            currentpose = "getLocation"
            for i in poselist:
                currentpose = currentpose + '@' + str(i)
            print currentpose
            return currentpose
        elif "handaction" in list:
            if len(list) is not 2:
                return 0
            return self.HandAction(int(list[1]))
        elif "EM" in list:
            if len(list) is not 1:
                return 0
            return self.EPimage()
        elif "PM" in list:
            if len(list) is not 1:
                return 0
            return self.PMimage()
        else:
            print "not find mothod"
            return 0

    mappath = os.path.expanduser('~') + "/catkin_ws/src/sit_robot_2dnav/map/map.pgm "
    uplaodpath = os.path.expanduser('~') + "/catkin_ws/src/sit_robot_common/sit_robot_hub/upload/"

    def EPimage(self):

        os.system("convert " + self.mappath + self.uplaodpath + "map.png")
        im = open(os.path.expanduser('~') + "/catkin_ws/src/sit_robot_common/sit_robot_hub/upload/map.png", "rb")
        return im

    def PMimage(self):
        os.system("gnome-screenshot -f " + self.uplaodpath + "screenshot.png")
        im = open(self.uplaodpath + "screenshot.png", "rb")
        return im


