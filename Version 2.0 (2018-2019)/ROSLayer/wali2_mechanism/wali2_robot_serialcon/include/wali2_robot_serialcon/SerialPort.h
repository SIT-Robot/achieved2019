/*
 * SerialPort.h
 *
 *  Created on: 2015年7月3日
 *      Author: johnchen
 */

#ifndef SRC_SERIALPORT_H_
#define SRC_SERIALPORT_H_

#include<stdio.h>      /*标准输入输出定义*/
#include<stdlib.h>     /*标准函数库定义*/
#include<unistd.h>     /*Unix 标准函数定义*/
#include<sys/types.h>
#include<sys/stat.h>
#include<fcntl.h>      /*文件控制定义*/
#include<termios.h>    /*PPSIX 终端控制定义*/
#include<errno.h>      /*错误号定义*/
#include<string.h>

//宏定义
#define FALSE  -1
#define TRUE   0

namespace wali2_robot_serialcon {

class SerialPort {
public:
	SerialPort();
	SerialPort(char* port);
	virtual ~SerialPort();
	void Read(char* rcv_buf);
	void Write(unsigned char* send_buf);
	void Open();
	void Close();
	bool IsPortOpen() {  return this->fd != -1; }
private:
	int fd;                            //文件描述符
	int err;                           //返回调用函数的状态
	int len;
	char* port;
	int UART0_Open(int fd, char* port);
	void UART0_Close(int fd);
	int UART0_Set(int fd, int speed, int flow_ctrl, int databits, int stopbits,
			int parity);
	int UART0_Init(int fd, int speed, int flow_ctrl, int databits, int stopbits,
			int parity);
	int UART0_Recv(int fd, char* rcv_buf, int data_len);
	int UART0_Send(int fd, unsigned char *send_buf, int data_len);
};

} /* namespace wali2_robot_serialcon */

#endif /* SRC_SERIALPORT_H_ */
