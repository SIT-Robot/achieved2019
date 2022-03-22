cmake_minimum_required(VERSION 2.8.3)
project(hand_action)

find_package(catkin REQUIRED COMPONENTS
  roscpp
  rospy
  std_msgs
  sit_robot_srv
)

catkin_package(
  INCLUDE_DIRS include
  LIBRARIES hand_action
  CATKIN_DEPENDS 
  roscpp rospy std_msgs
  sit_robot_srv
)

include_directories(
  include
  ${catkin_INCLUDE_DIRS}
)

include_directories(
  ${catkin_INCLUDE_DIRS}
)

include_directories(include ${catkin_INCLUDE_DIRS})
add_executable(hand_action src/hand_action.cpp)
add_dependencies(hand_action hand_action_messages_cpp ${catkin_EXPORTED_TARGETS})
target_link_libraries(hand_action ${catkin_LIBRARIES})

install(TARGETS hand_action
   ARCHIVE DESTINATION ${CATKIN_PACKAGE_LIB_DESTINATION}
   LIBRARY DESTINATION ${CATKIN_PACKAGE_LIB_DESTINATION}
   RUNTIME DESTINATION ${CATKIN_PACKAGE_BIN_DESTINATION}
 )

 install(DIRECTORY include/${PROJECT_NAME}/
   DESTINATION ${CATKIN_PACKAGE_INCLUDE_DESTINATION})
