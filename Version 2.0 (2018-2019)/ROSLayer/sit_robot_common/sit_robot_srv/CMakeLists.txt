cmake_minimum_required(VERSION 2.8.3)
project(sit_robot_srv)

find_package(catkin REQUIRED COMPONENTS
  roscpp
  rospy
  std_msgs
  message_generation
)

 add_service_files(
   DIRECTORY srv 
   FILES
   ArmAction.srv
   moveToGoal.srv
   getPosition.srv
   setPosition.srv
   getAngle.srv
   getOritation.srv
   selectpoint.srv
 )

generate_messages(DEPENDENCIES std_msgs)

catkin_package(CATKIN_DEPENDS message_runtime std_msgs)


