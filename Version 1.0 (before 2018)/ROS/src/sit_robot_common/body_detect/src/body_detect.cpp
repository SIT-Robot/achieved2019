// %Tag(FULLTEXT)%
// %Tag(ROS_HEADER)%

// %EndTag(ROS_HEADER)%
// %Tag(MSG_HEADER)%

// %EndTag(MSG_HEADER)%
#include "ros/ros.h"
#include <string>
#include <iostream>
#include <people_msgs/PositionMeasurementArray.h>
#include "geometry_msgs/Point.h"
#include "std_msgs/Float32MultiArray.h"
using namespace std;
//
////class Leg:
//Leg::Leg(){
//	ID = -1;
//	x=0;
//	y=0;
//	ID=-1;
//	reliability=0;
//}
//
//
//Leg::~Leg(){
//
//}
//void Leg::show(){
//	printf("LEG %d: X: %f  Y: %f  Reliability: %f\n",ID,x,y,reliability);
//}
//
//void Leg::setLeg(int _ID,float _x,float _y,float _reliability){
//	ID=_ID;
//	x=_x;
//	y=_y;
//	reliability=_reliability;
//}
//int Leg::isAviliable(){
//	int res;
//	if(x==0&&y==0&&reliability==0){
//		res=0;
//	}else{
//		res=1;
//	}
//	return res;
//
//}
//
////class Body:
//Body::Body(){
//	x=0;
//	y=0;
//	z=0;
//	locX=0;
//	locY=0;
//	locZ=0;
//	handPose=0;
//}
//Body::~Body(){
//
//}
//
//void Body::bodyTF(){
//	x=locZ / 1000;
//	y=locX / 1000;
//	z=locY / 1000;
//}
//
//void Body::setBody(float _x,float _y,float _z,int _handPose){
//	x=_x;
//	y=_y;
//	z=_z;
//	handPose=_handPose;
//}
//
//void Body::show(){
//	printf("Body: X: %f  Y: %f  Z: %f  HandPose: %d \n",x,y,z,handPose);
//}
//int Body::isAviliable(){
//	int res;
//	if(x==0&&y==0&&z==0){
//		res=0;
//	}else{
//		res=1;
//	}
//	return res;
//}
//
//People::People() {
//	reliability=0;
//}
//People::~People(){
//
//
//}
//
//int People::updateBody(float bodyX,float bodyY,float bodyZ, int bodyPose)
//{
//
//	body.locX=bodyX;
//	body.locY=bodyY;
//	body.locZ=bodyZ;
//	body.handPose=bodyPose;
//	body.bodyTF();
//	return 0;
//}
//
//
//
//
//PeopleDetect::PeopleDetect(){
//	ros::NodeHandle nh;
//	ros::NodeHandle body_nh("body_msg");
//	people_leg = nh.subscribe("people_tracker_measurements", 1,&PeopleDetect::peopleLegsCallback,this);
//	NumOfLeg=0;
//	trackingID=-1;
//}
//
//PeopleDetect::~PeopleDetect(){
//
//}
//
//int  PeopleDetect::updateLegs(){
//	ros::spinOnce();
//	return 0;
//}
//
//void PeopleDetect::peopleLegsCallback(const people_msgs::PositionMeasurementArray & leg_msg) {
//
//	int peopleSize = leg_msg.people.size();
//	NumOfLeg=peopleSize;
//	//cout << peopleSize << endl;
//	for (int i = 0; i < peopleSize; i++) {
//	//	printf("%d\n", leg_msg.people[i].object_id);
//		//printf("X:  %lf\n", leg_msg.people[i].pos.x);
//		//printf("Y:  %lf\n", leg_msg.people[i].pos.y);
//		//printf("reliability:  %lf\n", leg_msg.people[i].reliability);
//		//ID=leg_msg.people[i].object_id;
//		char * name=(char *)leg_msg.people[i].name.c_str();
//		int ID=strtod(name+6*sizeof(char), (char**) NULL);
//		//float x=(leg_msg.people[i].pos.x+0.35);
//		legs[i].setLeg(ID, leg_msg.people[i].pos.x,leg_msg.people[i].pos.y, leg_msg.people[i].reliability);
//		//legs[i].show();
//	}
//}
//
//int PeopleDetect::autoSyncLegBody(){
//	int syncTimes=0;
//	for (int i = 0; i < NumOfLeg; i++) {
//		Leg & ileg = legs[i];
//		//ileg.show();
//		//trackingPeople.body.show();
//		if (abs(ileg.x - trackingPeople.body.x) < 0.150
//				&& abs(ileg.y - trackingPeople.body.y) < 0.15
//				&& ileg.reliability > 0.1) {
//			trackingPeople.leg = ileg;
//			trackingPeople.reliability=ileg.reliability;
//			//ROS_ERROR("ENSURE!!!");
//			syncTimes++;
//		}
//	}
//	if (syncTimes == 0) {
//		cout << "autoSyncLegBody:syncError,can not match any leg" << endl;
//	}
//	return syncTimes;
//}
//int PeopleDetect::syncLegBodyByID(int ID){
//	int syncTimes=0;
//	Leg ileg=findLegID(ID);
//	if(ileg.ID==-1){
//		cout <<"syncLegBodyByID:Can not Find ID"<<endl;
//	} else {
////		if (abs(ileg.x - trackingPeople.body.x) < 0.150
////				&& abs(ileg.y - trackingPeople.body.y) < 0.15
////				&& ileg.reliability > 0.4) {
////			trackingPeople.leg = ileg;
////			syncTimes++;
////		}
//		trackingPeople.leg = ileg;
//		syncTimes++;
////		else{
////			cout<<"syncLegBodyByID:leg doesn't compare this body."<<endl;
////		}
//	}
//	if(syncTimes==0){
//		cout<<"syncLegBodyByID:syncError"<<endl;
//	}
//	return syncTimes;
//}
//
//int PeopleDetect::ensurePeople(int handPose){
//	int res=0;
//	if(trackingPeople.body.handPose==handPose&&trackingPeople.reliability>0.1){
//		trackingID=trackingPeople.leg.ID;
//		//ROS_INFO("BODY_DETECT:::>trackingID:%d",trackingID);
//		res=1;
//	}
//	return res;
//}
//Leg PeopleDetect::findLegID(int ID){
//	Leg resLeg;
//	int res=0;
//	for (int i = 0; i < NumOfLeg; i++) {
//		Leg & iLeg=legs[i];
//		if(iLeg.ID==ID){
//			resLeg=iLeg;
//			res=1;
//		}
//	}
//	if (!res){
//		resLeg.ID=-1;
//	}
//	return resLeg;
//}
//int PeopleDetect::selectPeople(){
//	int resFlag=0;
//	ROS_INFO("selectPeople");
//
//	return resFlag;
//}

#define deleta_x 1
#define deleta_y 1

geometry_msgs::Point msg;

void peopleLegsCallback(const people_msgs::PositionMeasurementArray::ConstPtr & leg_msg){
		int peopleSize = leg_msg->people.size();


		cout <<peopleSize << endl;
		for (int i = 0; i < peopleSize; i++) {
			cout<<i<<leg_msg->people[i].object_id<<" ";
			cout<< i <<leg_msg->people[i].pos.x<<" ";
			cout<<i<<leg_msg->people[i].pos.y<<" ";
			cout<<i<<leg_msg->people[i].reliability<<" ";


//			leg_msg->people[i].name.c_str();
//			leg_msg->people[i].object_id;
//			leg_msg->people[i].pos.x;
//			leg_msg->people[i].pos.y;
//			leg_msg->people[i].pos.z;
//			leg_msg->people[i].reliability;

			//float x=(leg_msg.people[i].pos.x+0.35);
			//legs[i].show();
		}
}

int main(int argc, char **argv) {
	ros::init(argc, argv, "body_detect");

	ros::NodeHandle nh;

	ros::Subscriber people_leg = nh.subscribe("people_tracker_measurements", 1, peopleLegsCallback);

	ros::Publisher sp_pub = nh.advertise<geometry_msgs::Point>("body_detect",1);

	while(ros::ok()){
		ros::spinOnce();

	}
	return 0;
}









