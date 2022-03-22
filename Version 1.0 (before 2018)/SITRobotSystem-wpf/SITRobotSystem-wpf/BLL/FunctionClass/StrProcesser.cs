using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.FunctionClass
{
    static class StrProcesser
    {
        /// <summary>
        /// 顺序截取 开始-终止 之间的字符
        /// </summary>
        /// <param name="strs">源字符串</param>
        /// <param name="str1">开始字符</param>
        /// <param name="str2">终止字符</param>
        /// <returns></returns>
        public static string FindInSequential(string[] strs, string str1, string str2)
        {
            string resStr = "null";
            bool str1Finded = false;
            bool str2Finded = false;
            int str1Index = -1;
            int str2Index = -1;
            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i] == str1 && !str1Finded)
                {
                    str1Finded = true;
                    str1Index = i;
                }
                if (strs[i] == str2 && !str2Finded)
                {
                    str2Finded = true;
                    str2Index = i;
                }
            }
            if (str1Finded&&str2Finded)
            {
                if ((str2Index - str1Index)>=2)
                {
                    resStr = "";
                    for (int i = str1Index+1; i < str2Index; i++)
                    {
                        resStr += strs[i];
                        if (i < str2Index-1)
                        {
                            resStr += " ";
                        }
                    }
                }
            }
            return resStr;
        }

        /// <summary>
        /// 顺序截取 开始（出现次数）-终止（出现次数） 之间的字符
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="str1"></param>
        /// <param name="str1Times"></param>
        /// <param name="str2"></param>
        /// <param name="str2Times"></param>
        /// <returns></returns>
        public static string FindInSequential(string[] strs, string str1, int str1Times, string str2, int str2Times)
        {
            string resStr = "null";
            bool str1Finded = false;
            bool str2Finded = false;
            int str1Index = -1;
            int str2Index = -1;
            int str1foundTimes = 0;
            int str2foundTimes = 0;
            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i] == str1 &&(str1foundTimes != str1Times))
                {
                    str1foundTimes++;
                    str1Finded = true;
                    str1Index = i;
                }
                if (strs[i] == str2 && (str2foundTimes != str2Times))
                {
                    str2foundTimes++;
                    str2Finded = true;
                    str2Index = i;
                }
            }
            if (str1Finded && str2Finded)
            {
                if ((str2Index - str1Index) >= 2)
                {
                    resStr = "";
                    for (int i = str1Index + 1; i < str2Index; i++)
                    {
                        resStr += strs[i];
                        if (i < str2Index - 1)
                        {
                            resStr += " ";
                        }
                    }
                }
            }
            return resStr;
        }
        /// <summary>
        /// 倒序查找
        /// </summary>
        /// <param name="strs">字符组</param>
        /// <param name="str1">结束字符</param>
        /// <param name="str2">开始字符</param>
        /// <returns></returns>
        public static string FindInReverse(string[] strs, string str1, string str2)
        {
            string resStr = "null";
            bool str1Finded = false;
            bool str2Finded = false;
            int str1Index = -1;
            int str2Index = -1;
            for (int i =  strs.Length-1; i >=0; i--)
            {
                if (strs[i] == str1 && !str1Finded)
                {
                    str1Finded = true;
                    str1Index = i;
                }
                if (strs[i] == str2 && !str2Finded)
                {
                    str2Finded = true;
                    str2Index = i;
                }
            }
            if (str1Finded && str2Finded)
            {
                if ((str2Index - str1Index) >= 2)
                {
                    resStr = "";
                    for (int i = str1Index + 1; i < str2Index; i++)
                    {
                        resStr += strs[i];
                        if (i < str2Index - 1)
                        {
                            resStr += " ";
                        }
                    }
                }
            }
            return resStr;   
        }
        /// <summary>
        /// 倒序截取 开始（出现次数）-终止（出现次数） 之间的字符
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="str1"></param>
        /// <param name="str1Times"></param>
        /// <param name="str2"></param>
        /// <param name="str2Times"></param>
        /// <returns></returns>
        public static string FindInReverse(string[] strs, string str1, int str1Times, string str2, int str2Times)
        {
            string resStr = "null";
            bool str1Finded = false;
            bool str2Finded = false;
            int str1Index = -1;
            int str2Index = -1;
            int str1foundTimes = 0;
            int str2foundTimes = 0;
            for (int i = strs.Length - 1; i >= 0; i--)
            {
                if (strs[i] == str1 && (str1foundTimes != str1Times))
                {
                    str1foundTimes++;
                    str1Finded = true;
                    str1Index = i;
                }
                if (strs[i] == str2 && (str2foundTimes != str2Times))
                {
                    str2foundTimes++;
                    str2Finded = true;
                    str2Index = i;
                }
            }
            if (str1Finded && str2Finded)
            {
                if ((str2Index - str1Index) >= 2)
                {
                    resStr = "";
                    for (int i = str1Index + 1; i < str2Index; i++)
                    {
                        resStr += strs[i];
                        if (i < str2Index - 1)
                        {
                            resStr += " ";
                        }
                    }
                }
            }
            return resStr;
        }
    }
}
