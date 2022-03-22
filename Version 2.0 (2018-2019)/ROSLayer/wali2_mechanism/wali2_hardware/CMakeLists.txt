cmake_minimum_required(VERSION 2.8.3)
project(wali2_hardware)

find_package(catkin REQUIRED COMPONENTS
  geometry_msgs
  roscpp
  wali2_mechanism_msgs
  wali2_robot_serialcon
)

catkin_package(
  INCLUDE_DIRS include
  LIBRARIES wali2_hardware
  CATKIN_DEPENDS geometry_msgs roscpp wali2_mechanism_msgs wali2_robot_serialcon
#  DEPENDS system_lib
)

include_directories( 
  include
  ${catkin_INCLUDE_DIRS}
)

 add_library(wali2_hardware
  include/${PROJECT_NAME}/hardware.h
   src/hardware.cpp
 )
 
 target_link_libraries(${PROJECT_NAME} ${catkin_LIBRARIES})



## Declare a cpp executable
 add_executable(wali2_hardware_node src/wali2_hardware_node.cpp)

 add_dependencies(wali2_hardware_node wali2_hardware_generate_messages_cpp ${catkin_EXPORTED_TARGETS})

## Specify libraries to link a library or executable target against
 target_link_libraries(wali2_hardware_node
   ${catkin_LIBRARIES}
 )

 target_link_libraries(wali2_hardware_node wali2_hardware)

 install(TARGETS wali2_hardware wali2_hardware_node
   ARCHIVE DESTINATION ${CATKIN_PACKAGE_LIB_DESTINATION}
   LIBRARY DESTINATION ${CATKIN_PACKAGE_LIB_DESTINATION}
   RUNTIME DESTINATION ${CATKIN_PACKAGE_BIN_DESTINATION}
 )
 
 install(DIRECTORY include/${PROJECT_NAME}/
  DESTINATION ${CATKIN_PACKAGE_INCLUDE_DESTINATION})