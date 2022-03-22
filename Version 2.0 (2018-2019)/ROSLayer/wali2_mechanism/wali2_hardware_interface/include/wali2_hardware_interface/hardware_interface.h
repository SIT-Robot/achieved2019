/*
 * hardware_interface.h
 *
 *  Created on: 2015年7月4日
 *      Author: johnchen
 */

#ifndef INCLUDE_WALI2_HARDWARE_INTERFACE_HARDWARE_INTERFACE_H_
#define INCLUDE_WALI2_HARDWARE_INTERFACE_HARDWARE_INTERFACE_H_

#include <hardware_interface/joint_command_interface.h>
#include <hardware_interface/joint_state_interface.h>
#include <hardware_interface/robot_hw.h>

class Wali2Robot: public hardware_interface::RobotHW {
public:
	Wali2Robot() {
			// connect and register the joint state interface
		   hardware_interface::JointStateHandle state_handle_1("1", &pos[0], &vel[0], &eff[0]);
		   jnt_state_interface.registerHandle(state_handle_1);

		   hardware_interface::JointStateHandle state_handle_2("2", &pos[1], &vel[1], &eff[1]);
		   jnt_state_interface.registerHandle(state_handle_2);

		   hardware_interface::JointStateHandle state_handle_3("3", &pos[2], &vel[2], &eff[2]);
		   jnt_state_interface.registerHandle(state_handle_3);

		   registerInterface(&jnt_state_interface);

		   // connect and register the joint position interface
		   hardware_interface::JointHandle pos_handle_1(jnt_state_interface.getHandle("1"), &cmd[0]);
		   jnt_pos_interface.registerHandle(pos_handle_1);

		   hardware_interface::JointHandle pos_handle_2(jnt_state_interface.getHandle("2"), &cmd[1]);
		   jnt_pos_interface.registerHandle(pos_handle_2);

		   hardware_interface::JointHandle pos_handle_3(jnt_state_interface.getHandle("3"), &cmd[2]);
		   jnt_pos_interface.registerHandle(pos_handle_3);

		   registerInterface(&jnt_pos_interface);
	}

private:
	hardware_interface::JointStateInterface jnt_state_interface;
	hardware_interface::PositionJointInterface jnt_pos_interface;
	double cmd[3];
	double pos[3];
	double vel[3];
	double eff[3];
};

#endif /* INCLUDE_WALI2_HARDWARE_INTERFACE_HARDWARE_INTERFACE_H_ */
