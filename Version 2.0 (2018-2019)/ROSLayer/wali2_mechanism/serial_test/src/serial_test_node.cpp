/*
 * serial_test.cpp
 *
 *  Created on: 2015年7月3日
 *      Author: johnchen
 */
#include <ros/ros.h>
#include <wali2_robot_serialcon/SerialPort.h>
#include <iostream>
#include <pthread.h>



using namespace std;
using namespace wali2_robot_serialcon;


//
//void* sread(SerialPort* ser) {
//	cout << ser.Read();
//}
//
//int main(int argc, char** argv) {
//	pthread_t tids[2];
//	SerialPort *serialport("/dev/ttyUSB0");
//	serialport.Open();
//
//	int ret = pthread_create(&tids[0], NULL, sread(serialport), NULL); //参数：创建的线程id，线程参数，线程运行函数的起始地址，运行函数的参数
//
//	serialport.Write("asdadasds");
//
//	serialport.Close();
//	return 0;
//}


#define NUM_THREADS 1 //线程数

SerialPort serialport("/dev/ttyUSB0");

void *say_hello(void *args)
{
	cout << "hello..." << endl;
	while (1)
	{
		//		cout << serialport.Read();
	}
} //函数返回的是函数指针，便于后面作为参数



int main()
{
	serialport.Open();
	pthread_t tids[NUM_THREADS]; //线程id

	int ret = pthread_create(&tids[0], NULL, say_hello, NULL); //参数：创建的线程id，线程参数，线程运行函数的起始地址，运行函数的参数
	if (ret != 0)											   //创建线程成功返回0
	{
		cout << "pthread_create error:error_code=" << ret << endl;
	}
	while (1)
	{
		//		serialport.Write("aaaaaaaaaaaaaaaaaaaaaaa");
	}
	pthread_exit(NULL); //等待各个线程退出后，进程才结束，否则进程强制结束，线程处于未终止的状态
	serialport.Close();
}
