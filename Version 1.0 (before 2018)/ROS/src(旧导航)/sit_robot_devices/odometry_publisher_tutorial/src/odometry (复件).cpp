#include <ros/ros.h>
#include <geometry_msgs/Twist.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <termios.h>
#include <errno.h>
#include <string.h>
#include <math.h>
#include <iostream>
using namespace std;
//**************************************
#define TRUE 1 //初始化串口选项：
#define PI 3.14159f
#define BUFSIZE 512
//**************************************
float robotWheelD = 0.15;//大轮子直径
float robotWheelL = 0.210;//大轮子到中心的距离   9-7-20:00 xdz :0.230->0.200
float robotOdoD = 0.050;//里程计轮子直径
float robotOdoL = 0.13;//里程计到中心距离
double pOdometryV[3];
int odoEndFlag = 0;
int odoHeadFlag = 0;

float Vx = 0;//机器人坐标速度
float Vy = 0;
float Vth = 0;


int Odom1=0;//机器人里程计的脉冲数
int Odom2=0;
int Odom3=0;
float OdomHz=0.02;//机器人里程计采样频率

float sV1 = 0;//机器人大轮子速度（m/s）
float sV2 = 0;
float sV3 = 0;	
//***************************************
void decodeOdo(char *odoBuf);
void setTermios(struct termios * pNewtio, int uBaudRate);

void cleandot(float &V);
void setTermios(struct termios * pNewtio, int uBaudRate)
{
	 bzero(pNewtio, sizeof(struct termios)); /* clear struct for new port settings */
	 //8N1
	pNewtio->c_cflag = uBaudRate | CS8 | CREAD | CLOCAL;
	pNewtio->c_iflag = IGNPAR;
	pNewtio->c_oflag = 0;
	pNewtio->c_lflag = 0; //non ICANON
	/* initialize all control characters default values can be found in /usr/include/termios.h, and are given in the comments, but we don't need them here */
	 pNewtio->c_cc[VINTR] = 0; /* Ctrl-c */
	pNewtio->c_cc[VQUIT] = 0; /* Ctrl-\ */
	pNewtio->c_cc[VERASE] = 0; /* del */
	pNewtio->c_cc[VKILL] = 0; /* @ */
	 pNewtio->c_cc[VEOF] = 4; /* Ctrl-d */
	pNewtio->c_cc[VTIME] = 5; /* inter-character timer, timeout VTIME*0.1 */
	pNewtio->c_cc[VMIN] = 0; /* blocking read until VMIN character arrives */
	 pNewtio->c_cc[VSWTC] = 0; /* '\0' */ pNewtio->c_cc[VSTART] = 0; /* Ctrl-q */ pNewtio->c_cc[VSTOP] = 0; /* Ctrl-s */ pNewtio->c_cc[VSUSP] = 0; /* Ctrl-z */
	 pNewtio->c_cc[VEOL] = 0; /* '\0' */
	pNewtio->c_cc[VREPRINT] = 0; /* Ctrl-r */
	pNewtio->c_cc[VDISCARD] = 0; /* Ctrl-u */
	pNewtio->c_cc[VWERASE] = 0; /* Ctrl-w */
	 pNewtio->c_cc[VLNEXT] = 0; /* Ctrl-v */
	 pNewtio->c_cc[VEOL2] = 0; /* '\0' */
 }

int main(int argc, char** argv){
  ros::init(argc, argv, "odometry_computer");
  ros::NodeHandle n;
  ros::Publisher odom_pub = n.advertise<geometry_msgs::Twist>("odom_computer", 50);
  geometry_msgs::Twist msg;
  int fd;
  int nread;
  char buff[BUFSIZE] = "LL0,0,0JJ";

  struct termios oldtio, newtio;
  struct timeval tv;
  const char *dev ="/dev/ttyUSB1";
  fd_set rfds;
  if ((fd = open(dev, O_RDWR | O_NOCTTY))<0)
  {
  	ROS_INFO("err: can't open serial port!\n");
  	return -1;
   }

 tcgetattr(fd, &oldtio); /* save current serial port settings */
 
 setTermios(&newtio, B115200);
  tcflush(fd, TCIFLUSH);
  tcsetattr(fd, TCSANOW, &newtio);
  tv.tv_sec=30;
  tv.tv_usec=0;
 ///"head100,200,300end"

  while(n.ok()){
    //cout<<"wait...\n";
  	FD_ZERO(&rfds);
  	FD_SET(fd, &rfds);
  	if (select(1+fd, &rfds, NULL, NULL, &tv)>0||1)
  	{
  		//cout<<"wait...\n";
  		if (FD_ISSET(fd, &rfds)||1)
  		{
			
  			nread=read(fd, buff, BUFSIZE);
  			//cout << "readlength="<< nread << endl;
  			buff[nread]='\0';
  			//cout <<  buff << endl;
  			decodeOdo(buff);
			
 			if(odoHeadFlag==1 && odoEndFlag==1)
  			{	
				sV1 = Odom1 * PI * robotOdoD / 4000/0.2;
				sV2 = Odom2 * PI * robotOdoD / 4000/0.2*1.011;
				sV3 = Odom3 * PI * robotOdoD / 4000/0.2;
				//cout<<"INPUT-Odom------>Odom1:::"<<Odom1 <<"  Odom2:::"<<Odom2<<"  Odom3:::"<<Odom3<<endl; 
				Vth = (1/( 3.0 * robotOdoL ) * sV1 + 1/( 3.0 * robotOdoL ) * sV2 + 1/( 3.0 * robotOdoL ) * sV3) ;//* //1.0404;//* 1.3336;
				Vy = -(( -1/3.0 * cos(Vth) - 1/sqrt(3.0) * sin(Vth) ) * sV3 +( -1/3.0 * cos(Vth) + 1/sqrt(3.0) * sin(Vth) ) * sV2 + 2/3.0 * cos(Vth) * sV1) * 1.0;//* 0.93215;
				Vx = (( -1/3.0 * sin(Vth) + 1/sqrt(3.0) * cos(Vth) ) * sV3 +( -1/3.0 * sin(Vth) - 1/sqrt(3.0) * cos(Vth) ) * sV2 + 2/3.0 * sin(Vth) * sV1) * 1.0;//* 0.93215;
				
  				//cout<<"AFTER-VbXYTh------>Vx:::"<<Vx <<"  Vy:::"<<Vy<<" Vth:::"<<Vth<<endl;
  			}
  		}
  	}
  	msg.linear.x=Vx;
  	msg.linear.y=Vy;
  	msg.angular.z=Vth;
    odom_pub.publish(msg);
  }
  return 0;
}

void decodeOdo(char *odoBuf)	
{
	int odoflag = 1;
	{
		odoHeadFlag = -1;
		odoEndFlag = -1;
	}
	char *next;
	char *pnext;
	unsigned int idx = 0;

	next = strtok_r(odoBuf, ",",&pnext);

	while(next&&odoflag)
	{
		switch(++idx)
		{
		case 1:
			if(!strcmp(next, "LL"))
			{
				{
					odoHeadFlag = 1;
				}
			}
			else
				odoflag = 0;//if not LL then stop
			break;
		case 2:
			Odom1 = strtod(next,NULL);
			//ROS_INFO("v1 : %f", v1);//num of points
			break;
		case 3:
			Odom2 = strtod(next, NULL);//
			//ROS_INFO("v2 : %f", v2);//num of points
			break;
		case 4 :
			Odom3 = strtod(next, NULL);//
			//ROS_INFO("v3 : %f\n", v3);//num of points
			break;
		case 5 :
			if(!strncmp(next, "JJ",2))
			{
				{
					odoEndFlag = 1;
				}
			}
			break;
		default :
			break;
		}
		next = strtok_r(NULL, ",",&pnext);
	}
}
void cleandot(float &V)
{
	int buf =V*10000;
	V=(float)buf/10000.0;
}
