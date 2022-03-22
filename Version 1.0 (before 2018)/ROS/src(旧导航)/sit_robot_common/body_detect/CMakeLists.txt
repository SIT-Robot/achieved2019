cmake_minimum_required(VERSION 2.8.3)
project(body_detect)

find_package(catkin REQUIRED COMPONENTS
  roscpp
  std_msgs
  message_generation
)

catkin_package(
  INCLUDE_DIRS include
  LIBRARIES body_detect
  CATKIN_DEPENDS message_runtime roscpp rospy std_msgs sit_robot_msgs people_msgs
)

include_directories(
	include 
  ${catkin_INCLUDE_DIRS}
)

include_directories(include ${catkin_INCLUDE_DIRS})

add_executable(body_detect src/body_detect.cpp)
target_link_libraries(body_detect ${catkin_LIBRARIES})
