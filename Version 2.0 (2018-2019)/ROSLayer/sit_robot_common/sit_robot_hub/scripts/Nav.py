#!/usr/bin/env python
__author__ = 'johnchen'

from sit_robot_srv.srv import *
import rospy
from geometry_msgs.msg import PoseStamped
from geometry_msgs.msg import Twist
from people_msgs.msg import PositionMeasurementArray
from nav_msgs.msg import Odometry
import SocketServer, time
import math
import string
import os


class Nav(object):
    def __init__(self, current_pose=PoseStamped(), peoples=PositionMeasurementArray(), odom=Odometry()):
        self.current_pose = current_pose
        self.velpub = rospy.Publisher('/base_cmd_vel', Twist, queue_size=1)
        self.peoples = peoples
        self.odom = odom

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

    def PeopleCallBack(self, data=PositionMeasurementArray()):
        self.peoples = data

    def getpeople(self):
        rospy.Subscriber('people_tracker_measurements', PositionMeasurementArray, self.PeopleCallBack)
        rate = rospy.Rate(1.0)
        rate.sleep()
        return self.peoples

    def Peopleid(self, objid):
        self.getpeople()
        for peo in self.peoples.people:
            if objid == peo.name:
                return peo.name + "@" + str(peo.pos.x) + "@" + str(peo.pos.y)
        return "null" + "@" + "0" + "@" + "0"

    def Peoplexy(self, x, y):
        self.getpeople()
        for peo in self.peoples.people:
            if abs(math.sqrt(x * x + y * y) - math.sqrt(peo.pos.x * peo.pos.x + peo.pos.y * peo.pos.y)) < 0.2:
                return peo.name
        return "null"

    def movevel(self, vx, vy, vth):

        twist = Twist()
        twist.linear.x = vx
        twist.linear.y = vy
        twist.angular.z = vth
        self.velpub.publish(twist)

    def movevelz(self, z):
        twist = Twist()
        if z > 0:
            twist.angular.z = 0.5
        else:
            twist.angular.z = -0.5
        zs = abs(z / twist.angular.z)
        print zs
        self.movevel(0, 0, twist.angular.z)
        time.sleep(zs)
        self.movevel(0, 0, 0)
        return 1

    def movevels(self, x, y):
        twist = Twist()
        if x > 0:
            twist.linear.x = 0.2
        else:
            twist.linear.x = -0.2
        if y > 0:
            twist.linear.y = 0.2
        else:
            twist.linear.y = -0.2
        xs = abs(x / twist.linear.x)
        ys = abs(y / twist.linear.y)
        if x != 0:
            self.movevel(twist.linear.x, 0, 0)
            time.sleep(xs)
        if y != 0:
            self.movevel(0, twist.linear.y, 0)
            time.sleep(ys)
        self.movevel(0, 0, 0)
        print xs
        return 1

    def GetOdom(self):
        rospy.Subscriber('odom', Odometry, self.OdomCallback)
        rate = rospy.Rate(1.0)
        rate.sleep()
        return self.odom

    def OdomCallback(self, data=Odometry()):
        self.odom = data

    def hubServe(self, PORT, HOST=''):

        print 'Server is started\nwaiting for connection...\n'
        print PORT
        srv = SocketServer.ThreadingTCPServer((HOST, PORT), MyServer)
        srv.serve_forever()

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
        elif "movevel" in list:
            vx = string.atof(list[1])
            vy = string.atof(list[2])
            vth = string.atof(list[3])
            self.movevel(vx, vy, vth)
            return 1
        elif "movevel&s" in list:
            x = string.atof(list[1])
            y = string.atof(list[2])
            print x
            print y
            return self.movevels(x, y)
        elif "movevel&z" in list:
            z = string.atof(list[1])
            print z
            return self.movevelz(z)
        elif "EM" in list:
            if len(list) is not 1:
                return 0
            return self.EPimage()
        elif "PM" in list:
            if len(list) is not 1:
                return 0
            return self.PMimage()
        elif "Peopleid" in list:
            if len(list) is not 2:
                return 0
            return self.Peopleid(list[1])
        elif "Peoplexy" in list:
            if len(list) is not 3:
                return 0
            x = string.atof(list[1])
            y = string.atof(list[2])
            return self.Peoplexy(x, y)
        elif "getOdom" in list:
            odom = self.GetOdom()
            odomlist = [odom.header.frame_id,
                        odom.pose.pose.position.x,
                        odom.pose.pose.position.y,
                        odom.pose.pose.position.z,
                        odom.pose.pose.orientation.x,
                        odom.pose.pose.orientation.y,
                        odom.pose.pose.orientation.z,
                        odom.pose.pose.orientation.w]
            odom = "getodom"
            for i in odomlist:
                odom = odom + '@' + str(i)
            print odom
            return odom
        else:
            print "not find mothod"
            return 0

    mappath = os.path.expanduser('~') + "/catkin_ws/src/sit_robot_2dnav/map/map.pgm "

    uplaodpath = os.path.expanduser('~') + "/catkin_ws/src/sit_robot_common/sit_robot_hub/upload/"

    mapfilepath = os.path.expanduser('~') + "/catkin_ws/src/sit_robot_common/sit_robot_hub/upload/map.png"

    pmfilepath = uplaodpath + "screenshot.png"

    def EPimage(self):

        os.system("convert " + self.mappath + self.uplaodpath + "map.png")
        im = open(self.mapfilepath, "rb")
        return im

    def PMimage(self):
        os.system("gnome-screenshot -f " + self.uplaodpath + "screenshot.png")
        im = open(self.pmfilepath, "rb")
        return im


class MyServer(SocketServer.BaseRequestHandler):
    def handle(self):
        nav = Nav()

        print 'Connected from', self.client_address

        receivedData = self.request.recv(8192)

        print receivedData

        if receivedData.startswith('#EM#'):
            time.sleep(5)
            sfile = nav.readdata(receivedData)

            self.request.sendall(str(os.path.getsize(nav.mapfilepath)))

            print str(os.path.getsize(nav.mapfilepath))

            while True:
                data = sfile.read(1024)
                if not data:
                    break
                while len(data) > 0:
                    intSent = self.request.send(data)
                    data = data[intSent:]

            time.sleep(3)

        elif receivedData.startswith('#PM#'):

            time.sleep(5)
            sfile = nav.readdata(receivedData)

            self.request.sendall(str(os.path.getsize(nav.pmfilepath)))

            print str(os.path.getsize(nav.pmfilepath))

            while True:
                data = sfile.read(1024)
                if not data:
                    break
                while len(data) > 0:
                    intSent = self.request.send(data)
                    data = data[intSent:]

            time.sleep(3)

        else:
            self.request.sendall(bytes('@%s@' % nav.readdata(receivedData)))

        self.request.close()

        print 'Disconnected from', self.client_address


