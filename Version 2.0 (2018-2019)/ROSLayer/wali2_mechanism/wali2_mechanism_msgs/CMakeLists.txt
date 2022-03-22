cmake_minimum_required(VERSION 2.8.3)
project(wali2_mechanism_msgs)

find_package(catkin REQUIRED COMPONENTS
  actionlib_msgs
  geometry_msgs
  message_generation
  trajectory_msgs
)

 add_message_files(
   FILES
   JointControllerState.msg
 )

## Generate services in the 'srv' folder
# add_service_files(
#   FILES
#   Service1.srv
#   Service2.srv
# )

## Generate actions in the 'action' folder
# add_action_files(
#   FILES
#   Action1.action
#   Action2.action
# )

 generate_messages(
   DEPENDENCIES
   actionlib_msgs   geometry_msgs   trajectory_msgs
 )

catkin_package(
  CATKIN_DEPENDS actionlib_msgs geometry_msgs message_runtime trajectory_msgs
)

