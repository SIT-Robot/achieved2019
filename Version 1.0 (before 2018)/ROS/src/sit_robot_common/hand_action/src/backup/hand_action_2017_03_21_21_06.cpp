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
//*************初始**************116,
char zero[8]={0x77,0x74,0xB4,0x46,0x37,0xBD,0x2C,0x88 };//初始姿态
//以前GPSR	动作



char begin1[8] = {0x77,0x74,0xBE,0x69,0x96,0xB8,0x4A,0x88 };
char begin2[8] = {0x77,0x74,0xBE,0xA0,0x96,0xB8,0x4A,0x88 };
char begin3[8] = {0x77,0x74,0x69,0xA0,0x96,0xB8,0x4A,0x88 };
char begin4[8] = {0x77,0x51,0x69,0xA0,0x96,0xB8,0x4A,0x88 };
char catchready[8] = {0x77,0x51,0x69,0x5F,0x4B,0xB8,0x4A,0x88 };
char catchlab[8] = {0x77,0x51,0x69,0x5F,0x4B,0xB8,0x7E,0x88 };
char away[8] = {0x77,0x51,0x64,0x5F,0x4B,0xB8,0x7E,0x88 };




char back[8] = {0x77,0x51,0x64,0x64,0x64,0xB8,0x7E,0x88 };



char readygive[8] = {0x77,0x51,0x78,0x82,0x3C,0xB8,0x7E,0x88 };
char give[8] = {0x77,0x51,0x78,0x82,0x3C,0xB8,0x4A,0x88 };








char zoo1[8] = {0x77,0x74,0x87,0x64,0x64,0xAE,0x95,0x88 };
char zoo2[8] = {0x77,0x74,0x91,0x64,0x64,0xAE,0x95,0x88 };
char zoo3[8] = {0x77,0x74,0x87,0x64,0x64,0xAE,0x95,0x88 };
char zoo4[8] = {0x77,0x60,0x87,0x64,0x64,0xAE,0x95,0x88 };
char zoo5[8] = {0x77,0x60,0x87,0x64,0x3C,0xAE,0x95,0x88 };
char zoo6[8] = {0x77,0x60,0x87,0x64,0x64,0xAE,0x95,0x88 };
char zoo7[8] = {0x77,0x4C,0x87,0x64,0x3C,0x5E,0x68,0x88 };
char zoo8[8] = {0x77,0x4C,0x87,0x64,0x64,0xAE,0x95,0x88 };
char zoo9[8] = {0x77,0x60,0x87,0x64,0x3C,0xAE,0x95,0x88 };
char zoo10[8] = {0x77,0x60,0x87,0x64,0x64,0xAE,0x95,0x88 };
char zoo11[8] = {0x77,0x74,0x87,0x64,0x64,0xAE,0x95,0x88 };
char zoo12[8] = {0x77,0x60,0x87,0x64,0x64,0xAE,0x95,0x88 };
char zoo13[8] = {0x77,0x74,0x87,0x64,0x64,0xAE,0x95,0x88 };
char zoo14[8] = {0x77,0x74,0x91,0x64,0x64,0xAE,0x95,0x88 };




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

		case 500:	//拿走桌子上的水瓶
		cout<<"begin1"<<endl;	write(fd, begin1, LENGTH);	sleep(1);
                cout<<"begin2"<<endl;	write(fd, begin2, LENGTH);	sleep(2);
		cout<<"begin3"<<endl;	write(fd, begin3, LENGTH);	sleep(2);
		cout<<"begin4"<<endl;	write(fd, begin4, LENGTH);	sleep(2);
                cout<<"catch ready"<<endl;	write(fd, catchready, LENGTH);	sleep(2);
		cout<<"catch lab"<<endl;	write(fd, catchlab, LENGTH);	sleep(2);
		cout<<"away"<<endl;	write(fd, away, LENGTH);	sleep(1);
		cout<<"HAND=====>>>I am ready to take the bottle away!"<<endl;		
		result=1;
		break;

 		case 501:	//拿走水瓶
		cout<<"back"<<endl;	write(fd, back, LENGTH);	sleep(1);
		cout<<"HAND=====>>>hahahahhaahhahahh"<<endl;		
		result=1;
		break;

                case 502:	//
		cout<<"ready give"<<endl;	write(fd, readygive, LENGTH);	sleep(2);
		cout<<"give"<<endl;	write(fd, give, LENGTH);	sleep(2);
		cout<<"初始"<<endl;	write(fd, zero, LENGTH);	sleep(1);
		cout<<"HAND=====>>>hahahahhaahhahahh"<<endl;		
		result=1;
		break;
		
		case 600:	
		cout<<"zoo1"<<endl;	write(fd, zoo1, LENGTH);	sleep(2);
                cout<<"zoo2"<<endl;	write(fd, zoo2, LENGTH);	sleep(2);
                cout<<"zoo3"<<endl;	write(fd, zoo3, LENGTH);	sleep(2);
		cout<<"zoo4"<<endl;	write(fd, zoo4, LENGTH);	sleep(2);
		cout<<"zoo5"<<endl;	write(fd, zoo5, LENGTH);	sleep(2);
		cout<<"zoo6"<<endl;	write(fd, zoo6, LENGTH);	sleep(2);
		cout<<"zoo7"<<endl;	write(fd, zoo7, LENGTH);	sleep(2);
		cout<<"zoo8"<<endl;	write(fd, zoo8, LENGTH);	sleep(2);
		cout<<"zoo9"<<endl;	write(fd, zoo9, LENGTH);	sleep(2);
                cout<<"zoo10"<<endl;	write(fd, zoo10, LENGTH);	sleep(2);
                cout<<"zoo11"<<endl;	write(fd, zoo11, LENGTH);	sleep(2);
		cout<<"zoo12"<<endl;	write(fd, zoo12, LENGTH);	sleep(2);
		cout<<"zoo13"<<endl;	write(fd, zoo13, LENGTH);	sleep(2);
		cout<<"zoo14"<<endl;	write(fd, zoo14, LENGTH);	sleep(2);
		cout<<"zoo1"<<endl;	write(fd, zoo1, LENGTH);	sleep(2);
                cout<<"zoo2"<<endl;	write(fd, zoo2, LENGTH);	sleep(2);
                cout<<"zoo3"<<endl;	write(fd, zoo3, LENGTH);	sleep(2);
		cout<<"zoo4"<<endl;	write(fd, zoo4, LENGTH);	sleep(2);
		cout<<"zoo5"<<endl;	write(fd, zoo5, LENGTH);	sleep(2);
		cout<<"zoo6"<<endl;	write(fd, zoo6, LENGTH);	sleep(2);
		cout<<"zoo7"<<endl;	write(fd, zoo7, LENGTH);	sleep(2);
		cout<<"zoo8"<<endl;	write(fd, zoo8, LENGTH);	sleep(2);
		cout<<"zoo9"<<endl;	write(fd, zoo9, LENGTH);	sleep(2);
                cout<<"zoo10"<<endl;	write(fd, zoo10, LENGTH);	sleep(2);
                cout<<"zoo11"<<endl;	write(fd, zoo11, LENGTH);	sleep(2);
		cout<<"zoo12"<<endl;	write(fd, zoo12, LENGTH);	sleep(2);
		cout<<"zoo13"<<endl;	write(fd, zoo13, LENGTH);	sleep(2);
		cout<<"zoo14"<<endl;	write(fd, zoo14, LENGTH);	sleep(2);
		cout<<"初始"<<endl;	write(fd, zero,  LENGTH);	sleep(2);
		cout<<"HAND=====>>>I am ready to take the bottle away!"<<endl;		
		result=1;
		break;

	
	case 1000:	//初始
		cout<<"初始"<<endl;	write(fd, zero, LENGTH);	sleep(1);
		cout<<"我已回到初始"<<endl;		
		result=1;
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

