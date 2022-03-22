cmake_minimum_required(VERSION 2.8.3)
project(wali2_hardware_interface)

find_package(catkin REQUIRED COMPONENTS
  roscpp
  geometry_msgs
   wali2_robot_serialcon
)

catkin_package(
  	INCLUDE_DIRS include
   	CATKIN_DEPENDS roscpp geometry_msgs wali2_robot_serialcon
#	CFG_EXTRAS ${PROJECT_NAME}-extras.cmake
)

 install(DIRECTORY include/${PROJECT_NAME}/
    DESTINATION ${CATKIN_PACKAGE_INCLUDE_DESTINATION} )