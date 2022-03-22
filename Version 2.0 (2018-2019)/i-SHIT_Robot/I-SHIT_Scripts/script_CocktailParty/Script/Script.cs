/*
 focus:
 This test focuses on human detection and recognition, 
 safe navigation and human-robot interaction with unknown people.

             /*-----------------------------------Task1-----------------------------------------------------*/
            //1.Entering: 
            //The robot enters the arena and 
            //navigates to the party room and waits for being called. 

            /*-----------------------------------Task2-----------------------------------------------------*/
            //2. Getting called: 
            //The guests call the robot simultaneously, either rising an arm, waving, or shouting. 
            //The robot has to approach one of them. 
            //The calling person introduces themself by name before giving the order of a drink. 
            //The robot leads the dialogue to learn the person and retrieve their drink order.
            /*
                confusion: is guests introduces themself All and then to retrieve their drink order;
                           or one guest introduces him/herself and then to retrieve one drink order;
                                 
             */

                 /*-----------------------------------Task3-----------------------------------------------------*/
                //3.Taking the order: 
                //After the robot has fetched the order of the ﬁrst guest, it can either fetch more orders 
                //(i.e. ﬁnd next calling guest or looking for the sitting one) or proceed to place the order. 
                //In the ﬁrst case, the robot searches for the remaining calling people.
                //During the search process, the robot is allowed to either ask people to call for it again,
                //or to ask people to come to it and to give a new order. In both cases the robot may call into the room. 
                
using i_Shit_Core.Core;
using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using Microsoft.Kinect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace i_Shit_Scirpt.Script
{
    public class MyScript : Scripts
    {
        //using Guest class to keep guest info
        public class Guest
        {
            public  string _guest_name;
            public  string _guest_beverage;   //keep  beverages;
            public  LocationInfo _guest_position; //if the guest stay still. we don't have the face detection;
            public string _characteristics;
        }
        public Guest[] guest_ins = new Guest[5];//how many guest are there
        /// <summary>
        /// Write Init Code Here. 在这里写初始化Script的代码
        /// Run in UI Thread. 在UI线程中运行
        /// Such as Init Kinect. 比如Init Kinect. 
        /// </summary>
        public override void InitScript()
        {

            Driver.Kinect_InitKinect();
            Function.BodyDetect_ShowBodyDetectWindow();//2.1 open body detect window to find guests,

        }

        /// <summary>
        /// 在这里写Script的过程，可随意阻塞。
        /// </summary>
        public override void Script_Process()
        {
 
            LocationInfo party_room = Function.Location_GetLocationInfoByName("party_room");
            LocationInfo barman_bay = Function.Location_GetLocationInfoByName("barman_bay");
            
                    
            for (int i = 0; i < 5; i++)                                                             //how many guests are there
            {
                Function.Move_Navigate(party_room);                                                 //1.1.move to party room
                List<User> fuckers = Function.BodyDetect_GetAllusers();                             //2.1.get the coornation of the people in  fron of robot;
                Function.Move_Distance(fuckers[0].BodyCenter.X - 0.05f, fuckers[0].BodyCenter.Y, 0);//2.2.get close to guest;
                FuckIntroduces(i);                                                                  //3.1.he i-th person introduces him/herself;
                guest_ins[i]._guest_position = Function.Location_GetCurrectLocationFromRos();       //3.2.get the guest position without take picture
                FuckOrders(i);                                                                      //3.3.take order;
                Function.Move_Navigate(barman_bay);                                                 //5.1.move to the barman_bay;
                PlaceOrder(i);                                                                       //5.2.here should place the order that guest asked
            }
        }//end process();

        /// <summary>
        /// This function should remember guests and their names;
        /// </summary>
        public void FuckIntroduces(int guest_num_)
        {
            if(guest_num_  < 0)
                System.Console.WriteLine("!!!!!!!!!!guest number less than 0!!!!!!!");
            Function.Speech_TTS("I am ready", false);
            while (true)
            {
                ArrayList speech = new ArrayList();

                speech.Add(new string[] { "My name is"});
                speech.Add(new string[] { "dog", "fucker", "tom", "sonia" });//add guests' name;


                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                if (resultInt[0] == 0)
                {

                    guest_ins[guest_num_]._guest_name = "";
                    switch (resultInt[1])//Here to judge the name that the guest had said;
                    {
                        case 0:
                            guest_ins[guest_num_]._guest_name = "dog";
                            break;
                        case 1:
                            guest_ins[guest_num_]._guest_name = "fucker";
                            break;
                        default:
                            guest_ins[guest_num_]._guest_name = "tom";
                            break;
                    }

                    Function.Speech_TTS("Are you " + guest_ins[guest_num_]._guest_name + "?" , false);
                    speech = new ArrayList();
                    speech.Add(new string[] { "yes", "no" });
                    if (Function.Speech_Recognize_StartSimpleRecognize(speech)[0] == 0)
                    {
                        Function.Speech_TTS("OK I know, you are " + guest_ins[guest_num_]._guest_name, false);
                        Function.Speech_TTS("hold still, I should momerize you " + guest_ins[guest_num_]._guest_name, true);    
                        //here should remember guest's face                   
                        Console.Write("Successed!");
                        break;
                    }
                    else { Function.Speech_TTS("Maybe I am wrong.", false); }
                }
            }
        }



        //2.3The calling person introduces themself by name before giving the order of a drink. 
        public void FuckOrders(int guest_num_)
        {
            if (guest_num_ < 0)
                System.Console.WriteLine("!!!!!!!!!!guest number less than 0!!!!!!!");

            Function.Speech_TTS("Can i get you anything?", false);
            while (true)
            {
                ArrayList speech = new ArrayList();

                speech.Add(new string[] { "get me the " });
                speech.Add(new string[] { "tea", "biscuit", "tom", "sonia" });

                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                if (resultInt[0] == 0)
                {
                    guest_ins[guest_num_]._guest_beverage = "";
                    switch (resultInt[1])
                    {
                        case 0:
                            guest_ins[guest_num_]._guest_beverage = "tea";
                            break;
                        case 1:
                            guest_ins[guest_num_]._guest_beverage = "biscuit";
                            break;
                        default:
                            guest_ins[guest_num_]._guest_beverage = "tea";
                            break;
                    }                   
                    Function.Speech_TTS("Do you want me take the" + guest_ins[guest_num_]._guest_beverage, false);
                    speech = new ArrayList();
                    speech.Add(new string[] { "yes", "no" });
                    if (Function.Speech_Recognize_StartSimpleRecognize(speech)[0] == 0)
                    {
                        Function.Speech_TTS("OK I know, I will get the" + guest_ins[guest_num_]._guest_beverage, false);
                        Console.Write("Successed");
                    }
                    else { Function.Speech_TTS("Maybe I am wrong.", false); }
                }
            }
        }//end fuckorder

        public void PlaceOrder(int guest_num_)
        {
            string[] _characteristics = { "tall", "strong", "pretty", "cute", "small" };
            Function.Speech_TTS(guest_ins[guest_num_]._guest_name +
                    " need " + guest_ins[guest_num_]._guest_beverage, false);//tell barman who need what to drink;
            Function.Speech_TTS(guest_ins[guest_num_]._guest_name + 
                    "is" + _characteristics[guest_num_], false);            //tell barman guest characteristics;

        }//end PlaceOrder();
    }
}





/*舍不得删的getter and setter
              public void SetGuestName(string guest_name_)
            {
                _guest_name = guest_name_;
            }
            public void SetGuestBeverage(string guest_beverage_)
            {
                _guest_beverage = guest_beverage_;
            }
            public void SetGuestPosition(LocationInfo guest_position_)
            {
                _guest_position = guest_position_;
            }
            public string GetGuestName()
            {
                return _guest_name;
            }
            public string GetGuestBeverage()
            {
                return _guest_beverage;
            }
            public LocationInfo GetGuestPosition()
            {
                return _guest_position;
            }
 */
