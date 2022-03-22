cmake_minimum_required(VERSION 2.8.3)
project(sit_robot_hub)

find_package(catkin REQUIRED COMPONENTS
  message_generation
  geometry_msgs
  roscpp
  rospy
  std_msgs
)

catkin_python_setup()

catkin_package(CATKIN_DEPENDS rospy message_runtime)

include_directories(${catkin_INCLUDE_DIRS})

catkin_install_python(PROGRAMS scripts/test.py
  DESTINATION ${CATKIN_PACKAGE_BIN_DESTINATION})

catkin_install_python(PROGRAMS scripts/Nav.py DESTINATION ${CATKIN_PACKAGE_BIN_DESTINATION})

install(PROGRAMS bin/Nav.py DESTINATION ${CATKIN_PACKAGE_BIN_DESTINATION})

