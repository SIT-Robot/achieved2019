cmake_minimum_required(VERSION 2.8.3)
project(base_controller)

find_package(catkin REQUIRED COMPONENTS
  geometry_msgs
  roscpp
  rospy
)

catkin_package(
#  INCLUDE_DIRS include
#  LIBRARIES base_controller
#  CATKIN_DEPENDS geometry_msgs roscpp rospy
#  DEPENDS system_lib
)

include_directories(${catkin_INCLUDE_DIRS})

include_directories(include ${catkin_INCLUDE_DIRS})

add_executable(base_controller src/base_controller.cpp)
target_link_libraries(base_controller ${catkin_LIBRARIES})
add_dependencies(base_controller base_controller_generate_messages_cpp)
