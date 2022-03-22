cmake_minimum_required(VERSION 2.8.3)
project(teleop_initpose_keyboard)

## Find catkin macros and libraries
## if COMPONENTS list like find_package(catkin REQUIRED COMPONENTS xyz)
## is used, also find other catkin packages
find_package(catkin REQUIRED COMPONENTS
  geometry_msgs
  rospy
)

catkin_package()

## Mark executable scripts (Python etc.) for installation
## in contrast to setup.py, you can choose the destination
catkin_install_python(PROGRAMS
   teleop_initpose_keyboard.py
   DESTINATION ${CATKIN_PACKAGE_BIN_DESTINATION}
)
