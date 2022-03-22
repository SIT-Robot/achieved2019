#include "ros/ros.h"
#include <sit_robot_srv/ArmAction.h>

//heads for sending 
#include <stdio.h>      /*标准输入输出定义*/ 
#include <stdlib.h>     /*标准函数库定义*/
#include <unistd.h>     /*Unix 标准函数定义*/
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>      /*文件控制定义*/
#include <termios.h>    /*PPSIX 终端控制定义*/
#include <errno.h>      /*错误号定义*/
#include <string.h>
#include<iostream>
#include<math.h>
using namespace std;

//*********************************串口
#define FALSE -1
#define TRUE 0
#define LENGTH 8

int speed_arr[] = {B38400,B19200,B9600,B4800,B2400,B1200,B300,B38400,B19200,B9600,B4800,B2400,B1200,B300};
int name_arr[] = {38400,19200,9600,4800,2400,1200,300,38400,19200,9600,4800,2400,1200,300};
int fd;

int OpenDev(char *Dev);
void set_speed(int fd, int speed);
int set_Parity(int fd, int databits, int stopbits, int parity);
//*************初始**************
char zero[8]={0x77,0x59,0x9D,0x55,0x6F,0xA2,0x50,0x88};//初始姿态
/****************握手*********************/
char shake[8]={0x77,0x80,0x7E,0x90,0x70,0xA5,0x80,0x88};//握手

/****************分部-take*********************/
char get14[8]={0x77,0x75,0xA3,0xB4,0xB4,0xA5,0x50,0x88};//一抬
char get24[8]={0x77,0x75,0x6A,0xB4,0xB4,0xA5,0x50,0x88};//二抬
char get34[8]={0x77,0x75,0x6A,0x92,0xB4,0xA5,0x58,0x88};//
char get44[8]={0x77,0x75,0x63,0x7E,0x5D,0xA5,0x58,0x88};//
//char sh[8] = {0x77,0x80,0x63,0x7E,0x5D,0xA5,0x58,0x88 };
char get54[8]={0x77,0x75,0x63,0x92,0x4C,0xA5,0x90,0x88};//抓住
char gup64[8]={0x77,0x75,0x64,0x92,0x50,0xA5,0x90,0x88};//上起

char got14[8]={0x77,0x6A,0x90,0x50,0xB4,0xA5,0x90,0x88};
char got24[8]={0x77,0x6A,0x9F,0x50,0xB4,0xA5,0x90,0x88};
/**********分步-put****/
char put0[8] = {0x77,0x76,0x75,0x98,0x73,0xA5,0x92,0x88 };

char put1[8] = {0x77,0x76,0x75,0x98,0x62,0xA5,0x92,0x88 };
char put2[8] = {0x77,0x76,0x75,0x98,0x62,0xA5,0x50,0x88 };

/****************分部-take-纸*********************/

char get254[8]={0x77,0x75,0x63,0x92,0x4C,0xA5,0x7F,0x88};//抓住
char gup264[8]={0x77,0x75,0x64,0x92,0x50,0xA5,0x7F,0x88};//上起

char got214[8]={0x77,0x6A,0x90,0x50,0xB4,0xA5,0x90,0x88};
char got224[8]={0x77,0x6A,0x9F,0x50,0xB4,0xA5,0x90,0x88};
/**********分步-put纸****/
char put20[8] = {0x77,0x76,0x75,0x98,0x73,0xA5,0x7F,0x88 };

char put21[8] = {0x77,0x76,0x75,0x98,0x62,0xA5,0x7F,0x88 };

//--------------------------------------------------------//
int handcatch(int type);

bool catchObj(	sit_robot_srv::ArmAction::Request  &req,	sit_robot_srv::ArmAction::Response &res);

int main(int argc, char **argv)
{
  ros::init(argc, argv, "hand_action");
 ros::NodeHandle armaction_nh("arm_action");

  ros::ServiceServer service = armaction_nh.advertiseService("hand_action", catchObj);
//****串口初始化
  char dev[]  = "/dev/ttyUSB1"; //USB串口
  fd = OpenDev(dev);
  tcflush(fd, TCIFLUSH);
  set_speed(fd,115200);
  if (set_Parity(fd,8,1,'N') == FALSE)
  {
     printf("HAND=====>>> Set Parity Error\n");
     exit (0);
  }
  ROS_INFO("HAND=====>>> Device is open!");

//***
  ROS_INFO("HAND=====>>> Ready to catch.");

  ros::spin();
  close(fd);
  return 0;
}

bool catchObj(	sit_robot_srv::ArmAction::Request  &req,
         	sit_robot_srv::ArmAction::Response &res)
{
  res.isSuccess=handcatch(req.type);
  return true;
}


int handcatch(int type)
{
  int result=0;
  switch (type){
//case HAND_TYPE_CATCH_TWO_STEPS
	  case 11:
		cout<<"get14"<<endl;	write(fd, get14, LENGTH);	sleep(4);
		cout<<"get24"<<endl;	write(fd, get24, LENGTH);	sleep(4);
		cout<<"get34"<<endl;	write(fd, get34, LENGTH);	sleep(4);
		cout<<"get44"<<endl;	write(fd, get44, LENGTH);	sleep(4);
		cout<<"get54"<<endl;	write(fd, get54, LENGTH);	sleep(4);
		cout<<"gup64"<<endl; 	write(fd, gup64, LENGTH);	sleep(6);
		result=1;
		break;
	  case 12:
		cout<<"got14"<<endl;	write(fd, got14, LENGTH);	sleep(3);
		cout<<"got24"<<endl;	write(fd, got24, LENGTH);	sleep(4);
		result=1;
		break;
 	  case 21:
		cout<<"put0"<<endl;	write(fd, put0, LENGTH);	sleep(5);
		cout<<"HAND=====>>> type 240 complete!"<<endl;
		break;
	  case 22:
		cout<<"put1"<<endl;	write(fd, put1, LENGTH);	sleep(3);
		cout<<"put2"<<endl;	write(fd, put2, LENGTH);	sleep(3);
		result=1;
		cout<<"HAND=====>>> type 241 complete!"<<endl;
		break; 

	case 99:
		cout<<"握手"<<endl;	write(fd, shake, LENGTH);	sleep(8);
		cout<<"HAND=====>>> COMMAND shake complete!"<<endl;
		break;
	case 1000:
		cout<<"初始"<<endl;	write(fd, zero, LENGTH);	sleep(5);
		cout<<"HAND=====>>> COMMAND original complete!"<<endl;
		break;
	  case 31:
		cout<<"get14"<<endl;	write(fd, get14, LENGTH);	sleep(4);
		cout<<"get24"<<endl;	write(fd, get24, LENGTH);	sleep(4);
		cout<<"get34"<<endl;	write(fd, get34, LENGTH);	sleep(4);
		cout<<"get44"<<endl;	write(fd, get44, LENGTH);	sleep(4);
		cout<<"get54"<<endl;	write(fd, get254, LENGTH);	sleep(4);
		cout<<"gup64"<<endl; 	write(fd, gup264, LENGTH);	sleep(6);
		result=1;
		break;
	  case 32:
		cout<<"got14"<<endl;	write(fd, got214, LENGTH);	sleep(3);
		cout<<"got24"<<endl;	write(fd, got224, LENGTH);	sleep(4);
		result=1;
		break;
 	  case 41:
		cout<<"put0"<<endl;	write(fd, put20, LENGTH);	sleep(5);
		cout<<"HAND=====>>> type 240 complete!"<<endl;
		break;
	  case 42:
		cout<<"put1"<<endl;	write(fd, put21, LENGTH);	sleep(3);
		cout<<"put2"<<endl;	write(fd, put2, LENGTH);	sleep(3);
		result=1;
		cout<<"HAND=====>>> type 241 complete!"<<endl;
		break; 

	default:
		break;
  }



  return result;

}

void set_speed(int fd, int speed)
{
	unsigned int   i;
	int   status;
	struct termios   Opt;
	tcgetattr(fd, &Opt);
	for ( i= 0;  i < sizeof(speed_arr) / sizeof(int);  i++)
	{
	      if  (speed == name_arr[i])
	      {
	            tcflush(fd, TCIOFLUSH);
	            cfsetispeed(&Opt, speed_arr[i]);
	            cfsetospeed(&Opt, speed_arr[i]);
	            status = tcsetattr(fd, TCSANOW, &Opt);
	            if  (status != 0)
		    {
		            perror("tcsetattr fd");
		            return;
	            }
	            tcflush(fd,TCIOFLUSH);
	      }
	}
}

/**
 * *@brief   设置串口数据位，停止位和效验位
 * *@param  fd     类型  int  打开的串口文件句柄
 * *@param  databits 类型  int 数据位   取值 为 7 或者8
 * *@param  stopbits 类型  int 停止位   取值为 1 或者2
 * *@param  parity  类型  int  效验类型 取值为N,E,O,S
 * */
int set_Parity(int fd,int databits,int stopbits,int parity)
{
	struct termios options;
	if  ( tcgetattr( fd,&options)  !=  0)
	{
		perror("SetupSerial 1");
		return(FALSE);
	}
	options.c_cflag &= ~CSIZE;
	switch (databits) /*设置数据位数*/
	{
	case 7:
		options.c_cflag |= CS7;
		break;
	case 8:
		options.c_cflag |= CS8;
		break;
	default:
		fprintf(stderr,"Unsupported data size\n");
		return (FALSE);
	}
	switch (parity)
	{
	case 'n':
	case 'N':
		options.c_cflag &= ~PARENB;   /* Clear parity enable */
		options.c_iflag &= ~INPCK;     /* Enable parity checking */
		break;
	case 'o':
	case 'O':
		options.c_cflag |= (PARODD | PARENB); /* 设置为奇效验*/
		options.c_iflag |= INPCK;             /* Disnable parity checking */
		break;
	case 'e':
	case 'E':
		options.c_cflag |= PARENB;     /* Enable parity */
		options.c_cflag &= ~PARODD;   /* 转换为偶效验*/
		options.c_iflag |= INPCK;       /* Disnable parity checking */
		break;
	case 'S':
	case 's':  /*as no parity*/
		options.c_cflag &= ~PARENB;
		options.c_cflag &= ~CSTOPB;break;
	default:
		fprintf(stderr,"Unsupported parity\n");
		return (FALSE);
	}
	/* 设置停止位*/
	switch (stopbits)
	{
	case 1:
		options.c_cflag &= ~CSTOPB;
		break;
	case 2:
		options.c_cflag |= CSTOPB;
		break;
   	default:
     	        fprintf(stderr,"Unsupported stop bits\n");
		return (FALSE);
	}
	/* Set input parity option */
	if (parity != 'n')
		options.c_iflag |= INPCK;
	tcflush(fd,TCIFLUSH);
	options.c_cc[VTIME] = 150; /* 设置超时15 seconds*/
	options.c_cc[VMIN] = 0; /* Update the options and do it NOW */
	if (tcsetattr(fd,TCSANOW,&options) != 0)
	{
		perror("SetupSerial 3");
		return (FALSE);
	}
	return (TRUE);
}

/**********************************************************************
 * 代码说明：使用usb串口测试的，发送的数据是字符，
 * 但是没有发送字符串结束符号，所以接收到后，后面加上了结束符号。
&nbsp; * **********************************************************************/
/*********************************************************************/

int OpenDev(char *Dev)
{
	int	fd = open(Dev, O_RDWR);         //| O_NOCTTY | O_NDELAY
	if (-1 == fd)
	{
		perror("HAND=====>>> Can't Open Serial Port");
		return -1;
	}
	else
		return fd;
}

