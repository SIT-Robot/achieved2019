#include "ros/ros.h"
#include <geometry_msgs/Twist.h>
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
//*********************************
#define FALSE -1
#define TRUE 0
#define LENGTH 32
#define PI 3.14159265358979f
//********************************
int fd;
char Speed[100];

float robotWheelD = 0.15;//大轮子直径
float robotWheelL = 0.071;//大轮子到中心的距离
//float robotWheelL = 0.035;//大轮子到中心的距离(旧公式)


void setSpeed(double vx,double vy,double vth);
int speed_arr[] = {B38400,B19200,B9600,B4800,B2400,B1200,B300,B38400,B19200,B9600,B4800,B2400,B1200,B300};

int name_arr[] = {38400,19200,9600,4800,2400,1200,300,38400,19200,9600,4800,2400,1200,300};

int OpenDev(char *Dev);
void set_speed(int fd, int speed);
int set_Parity(int fd, int databits, int stopbits, int parity);
void velocityCallback(const geometry_msgs::Twist::ConstPtr& vel); 
bool portOpen() ;

void velocityCallback(const geometry_msgs::Twist::ConstPtr& vel)
{
  //****************************************
	//last_command_time_ = ros::WallTime::now();
	double odoX = 0;
	double odoY = 0;
	double odoPhi = 0;
	odoX = vel->linear.x;
	odoY = vel->linear.y;
	odoPhi = vel->angular.z;
        //cout << " vx= " << odoX << " vy=" <<odoY << " vth=" << odoPhi <<endl;
    	setSpeed(odoX,odoY,odoPhi);
    	write(fd, Speed, LENGTH);
	
}

int main(int argc, char **argv)
{
	
  ros::init(argc, argv, "base_controller");
  char dev[]  = "/dev/ttyUSB0"; //USB串口
  fd = OpenDev(dev);
  ROS_INFO("%d",fd);
  tcflush(fd, TCIFLUSH);
  set_speed(fd,115200);
  if (set_Parity(fd,8,1,'N') == FALSE)
  {
     printf("Set Parity Error\n");
     exit (0);
  }
  ROS_INFO("Device is open!");
  ros::NodeHandle nh_;

  ros::Subscriber velocity_sub_ = nh_.subscribe("base_cmd_vel", 1, velocityCallback);
  //velocity_sub_ = nh_.subscribe("cmd_vel", 1, velocityCallback, this);

  ros::spin();

  close(fd);
  return 0;
}

bool portOpen() {  return fd  != -1; }

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

/*
 * 这是文件I/O的常用函数，open函数，open函数用来打开一个设备，他返回的是一个整型变量，如果这个值等于-1，说明打开文件出现错误，如果为大于0的值，那么这个值代表的就是文件描述符。一般的写法是if((fd=open("/dev/ttys0",O_RDWR | O_NOCTTY | O_NDELAY)<0){
perror("open");
}
这个事常用的一种用法fd是设备描述符，linux在操作硬件设备时，屏蔽了硬件的基本细节，只把硬件当做文件来进行操作，而所有的操作都是以open函数来开始，它用来获取fd，然后后期的其他操作全部控制fd来完成对硬件设备的实际操作。你要打开的/dev/ttyS0，代表的是串口1，也就是常说的com1，后面跟的是一些控制字。int open(const char *pathname, int oflag, …/*, mode_t mode * / ) ;这个就是open函数的公式。控制字可以有多种，我现在给你列出来：
O_RDONLY 只读打开。
O_WRONLY 只写打开。
O_RDWR 读、写打开。
O_APPEND 每次写时都加到文件的尾端。
O_CREAT 若此文件不存在则创建它。使用此选择项时，需同时说明第三个参数mode，用其说明该新文件的存取许可权位。
O_EXCL 如果同时指定了O_CREAT，而文件已经存在，则出错。这可测试一个文件是否存在，如果不存在则创建此文件成为一个原子操作。
O_TRUNC 如果此文件存在，而且为只读或只写成功打开，则将其长度截短为0。
O_NOCTTY 如果p a t h n a m e指的是终端设备，则不将此设备分配作为此进程的控制终端。
O_NONBLOCK 如果p a t h n a m e指的是一个F I F O、一个块特殊文件或一个字符特殊文件，则此选择项为此文件的本次打开操作和后续的I / O操作设置非阻塞方式。
O_SYNC 使每次w r i t e都等到物理I / O操作完成。
这些控制字都是通过“或”符号分开（|）
当调用系统调用open时，操作系统会将文件系统对应设备文件的inode中的file_operations安装进用户进程的task_struct中的fil
 *
 * */

int OpenDev(char *Dev)
{
	 if (portOpen()) ::close(fd);            //fd!=-1
	int fd = ::open(Dev, O_RDWR | O_NONBLOCK | O_NOCTTY);         //| O_NOCTTY | O_NDELAY
	if (fd==-1)
	{
		perror("Can't Open Serial Port");
		return -1;
	}
	else
		return fd;
}

void setSpeed(double Vx,double Vy,double Vth)
{
	short int nV1;//机器人大轮子速度（转速）
	short int nV2;
	short int nV3;
	float xx,yy;
//	float L = 0.071;
	xx =-Vy;
	yy = Vx;

	nV1=4300.0f * (cos(Vth) * xx + sin(Vth) * yy + 3.0f*robotWheelL*Vth); //修改4300.0f可改变轮子速度比例
	nV2=4300.0f * (-sin(PI/3.0f+Vth) * yy - cos(PI/3.0f+Vth) * xx + 3.0f*robotWheelL*Vth);
	nV3=4300.0f * ( sin(PI/3.0f-Vth) * yy - cos(PI/3.0f-Vth) * xx + 3.0f*robotWheelL*Vth);
	
	//cout<<"INPUT-XYTh--->Vx:::"<<Vx<<" Vy:::"<<Vy<<" Vth:::"<<Vth<<endl;
	//cout<<"INPUT-XYTh--->#1sv:::"<<nV1<<" #2sv:::"<<nV2<<" 3sv:::"<<nV3<<endl;

	sprintf(Speed,"#1sv:%d;#2sv:%d;#3sv:%d;",nV1,nV2,nV3);
	cout << Speed <<endl;
}
