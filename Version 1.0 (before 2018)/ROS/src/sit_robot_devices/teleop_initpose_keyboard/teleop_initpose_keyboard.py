#!/usr/bin/env python
import roslib;

roslib.load_manifest('teleop_initpose_keyboard')
import rospy

from std_msgs.msg import Header
from geometry_msgs.msg import PoseWithCovarianceStamped, PoseStamped

import math

import sys, select, termios, tty

msg = """
Reading from the keyboard  and Publishing to Twist!
---------------------------
Moving around:
   u    i    o
   j    k    l

q/z : increase/decrease max speeds by 10%
w/x : increase/decrease only linear speed by 10%
e/c : increase/decrease only angular speed by 10%
anything else : stop

CTRL-C to quit
"""

moveBindings = {
    'i': (1, 0, 0),
    'o': (0, 0, -1),
    'j': (0, 1, 0),
    'l': (0, -1, 0),
    'u': (0, 0, 1),
    'k': (-1, 0, 0),
}

speedBindings = {
    'q': (1.1, 1.1),
    'z': (.9, .9),
    'w': (1.1, 1),
    'x': (.9, 1),
    'e': (1, 1.1),
    'c': (1, .9),
}


def getKey():
    tty.setraw(sys.stdin.fileno())
    select.select([sys.stdin], [], [], 0)
    key = sys.stdin.read(1)
    termios.tcsetattr(sys.stdin, termios.TCSADRAIN, settings)
    return key


speed = .1
turn = 1

currentpose = PoseStamped()


def callback(data=PoseStamped()):
    global currentpose
    currentpose = data
    # print data


def getLocation():
    rospy.Subscriber('/current_position/current_position', PoseStamped, callback)
    rate = rospy.Rate(1.0)
    rate.sleep()


def vels(speed, turn):
    return "currently:\tspeed %s\tturn %s " % (speed, turn)


def rotatevel(w, turn):
    # print math.asin(w)
    print '222::: ' + str(math.degrees(math.acos(w)*2))
    print '111::: ' + str(math.degrees(math.acos(w)*2+turn))
    return math.degrees(math.asin(w)*2+turn)


if __name__ == "__main__":
    rospy.init_node('teleop_twist_keyboard', anonymous=True)

    settings = termios.tcgetattr(sys.stdin)

    initpose_pub = rospy.Publisher('initialpose', PoseWithCovarianceStamped, queue_size=10)

    x = 0
    th = 0
    status = 0

    try:
        print msg
        print vels(speed, turn)
        while (1):
            key = getKey()
            if key in moveBindings.keys():
                x = moveBindings[key][0]
                th = moveBindings[key][1]
            elif key in speedBindings.keys():
                speed = speed * speedBindings[key][0]
                turn = turn * speedBindings[key][1]

                print vels(speed, turn)
                if (status == 14):
                    print msg
                status = (status + 1) % 15
            else:
                x = 0
                th = 0
                if (key == '\x03'):
                    break

            getLocation()

            pose = PoseWithCovarianceStamped()
            pose.header = Header(None, None, 'map')

            pose.pose.pose.position.x = currentpose.pose.position.x
            pose.pose.pose.position.y = currentpose.pose.position.y

            pose.pose.pose.orientation.x = currentpose.pose.orientation.x
            pose.pose.pose.orientation.y = currentpose.pose.orientation.y
            pose.pose.pose.orientation.z = currentpose.pose.orientation.z
            pose.pose.pose.orientation.w = currentpose.pose.orientation.w

            if key == 'i':
                pose.pose.pose.position.x = currentpose.pose.position.x + speed
            elif key == 'k':
                pose.pose.pose.position.x = currentpose.pose.position.x - speed
            elif key == 'j':
                pose.pose.pose.position.y = currentpose.pose.position.y + speed
            elif key == 'l':
                pose.pose.pose.position.y = currentpose.pose.position.y - speed
            # elif key == 'u':
                # th = rotatevel(currentpose.pose.orientation.w, turn)
                # pose.pose.pose.orientation.z = math.sin(math.radians(th)/2)
                # pose.pose.pose.orientation.w = math.cos(math.radians(th)/2)
            # elif key == 'o':
            #     pose.pose.pose.orientation.z = math.sin(-math.pi / 2)
            #     pose.pose.pose.orientation.w = math.cos(-math.pi / 2)
            #     print key

            initpose_pub.publish(pose)

            # twist = Twist()
            # twist.linear.x = x * speed
            # twist.linear.y = 0
            # twist.linear.z = 0
            # twist.angular.x = 0
            # twist.angular.y = 0
            # twist.angular.z = th * turn
            # pub.publish(twist)

    except Exception, e:
        print e

    finally:
        pose = PoseWithCovarianceStamped()
        pose.header.frame_id = 'map'

        pose.pose.pose.position.x = currentpose.pose.position.x
        pose.pose.pose.position.y = currentpose.pose.position.y

        pose.pose.pose.orientation.x = currentpose.pose.orientation.x
        pose.pose.pose.orientation.y = currentpose.pose.orientation.y
        pose.pose.pose.orientation.z = currentpose.pose.orientation.z
        pose.pose.pose.orientation.w = currentpose.pose.orientation.w

        initpose_pub.publish(pose)

        termios.tcsetattr(sys.stdin, termios.TCSADRAIN, settings)


