cmake_minimum_required(VERSION 2.8.3)
project(wali2_robot_tf)

find_package(catkin REQUIRED COMPONENTS
  geometry_msgs
  roscpp
  tf
)

catkin_package(
#  INCLUDE_DIRS include
#  LIBRARIES wali2_robot_tf
#  CATKIN_DEPENDS geometry_msgs roscpp tf
#  DEPENDS system_lib
)

include_directories(
  ${catkin_INCLUDE_DIRS}
)

add_executable(tf_broadcaster src/tf_broadcaster.cpp)

if(geometry_msgs_EXPORTED_TARGETS)
  add_dependencies(tf_broadcaster ${geometry_msgs_EXPORTED_TARGETS})
endif()

 target_link_libraries(tf_broadcaster
   ${catkin_LIBRARIES}
 )

 install(TARGETS tf_broadcaster
   ARCHIVE DESTINATION ${CATKIN_PACKAGE_LIB_DESTINATION}
   LIBRARY DESTINATION ${CATKIN_PACKAGE_LIB_DESTINATION}
   RUNTIME DESTINATION ${CATKIN_PACKAGE_BIN_DESTINATION}
 )

