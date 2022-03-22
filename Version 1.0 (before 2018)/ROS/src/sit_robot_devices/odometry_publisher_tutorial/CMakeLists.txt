cmake_minimum_required(VERSION 2.8.3)
project(odometry_publisher_tutorial)

find_package(catkin REQUIRED COMPONENTS nav_msgs roscpp tf rospy std_msgs message_generation)

add_message_files(
    DIRECTORY msg
    FILES
    odom.msg
)

generate_messages(
    DEPENDENCIES
        std_msgs
)

catkin_package(
    CATKIN_DEPENDS
	message_runtime
        roscpp
        rospy
	std_msgs
)

include_directories(${catkin_INCLUDE_DIRS})

# Build the executable
add_executable(odometry_publisher src/odometry_publisher.cpp)
add_executable(odometry_computer src/odometry.cpp)
# Add a build order dependency on nav_msgs
# This ensures that nav_msgs' msg headers are built before your executable
if(nav_msgs_EXPORTED_TARGETS)
  add_dependencies(odometry_publisher ${nav_msgs_EXPORTED_TARGETS})
endif()
# Link against the catkin libraries
target_link_libraries(odometry_publisher ${catkin_LIBRARIES})
target_link_libraries(odometry_computer ${catkin_LIBRARIES})

# Install the executable
install(TARGETS odometry_publisher
  ARCHIVE DESTINATION ${CATKIN_PACKAGE_LIB_DESTINATION}
  LIBRARY DESTINATION ${CATKIN_PACKAGE_LIB_DESTINATION}
  RUNTIME DESTINATION ${CATKIN_PACKAGE_BIN_DESTINATION})
