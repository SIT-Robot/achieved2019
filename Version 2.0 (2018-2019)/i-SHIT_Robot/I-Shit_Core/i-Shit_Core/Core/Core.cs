using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace i_Shit_Core.Core
{
    public class Core
    {



        public Core(Scripts s)
        {
            //最底层（Core）init代码：
            CoreWindow coreWin = new CoreWindow();//显示log窗口
            String scriptName = System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath);//得到EXE的名字
            coreWin.Text = scriptName + " - " + coreWin.Text;//Window title = exe名字
            coreWin.Show();//显示log窗口
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High;//进程优先级高
            Console.WriteLine("i-SHIT Core Started. Version: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + ". Created by Strawing Lee @ March 2018. Last Built in " + System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location));
            Huaji();
            Console.WriteLine("Init Script: " + scriptName + " ...");
            s.InitScript();//调用script中的Init Script（主线程）
            new Thread(new ThreadStart(delegate
            {
                if (Drivers.Driver.Emulator_Mode == true)//判断是否开启了Emulator
                {
                    Console.WriteLine("Emulator Enabled！");
                    Console.WriteLine("Please config emulator and click START to continue.");
                    while (!Drivers.Driver.Emulator_Ready)//开启了Emulator时，等点击Start
                    {
                        Thread.Sleep(1);//这么做是为了让while true不吃CPU
                    };
                }
                s.Script_Process();//开始你的表演（子线程跑script process）
                Console.WriteLine("Script Process Finished.烫");
            })).Start();

        }

        private void Huaji()
        {
            Console.WriteLine(@"");
            Console.WriteLine(@"锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤");
            Console.WriteLine(@"烫                          ########                          烫");
            Console.WriteLine(@"烫                   ######          ######                   烫");
            Console.WriteLine(@"烫             #########                      ####            烫");
            Console.WriteLine(@"烫            ##   #####                 ####   ##            烫");
            Console.WriteLine(@"烫          ##       ###                 ###      ##          烫");
            Console.WriteLine(@"烫        ##                                        ##        烫");
            Console.WriteLine(@"烫      #################             #################       烫");
            Console.WriteLine(@"烫     #######            #          #######            #     烫");
            Console.WriteLine(@"烫     ###############     #         ###############     #    烫");
            Console.WriteLine(@"烫    ##  ###          ####             ###          #####    烫");
            Console.WriteLine(@"烫   ##                                                  ##   烫");
            Console.WriteLine(@"烫   #                                                    #   烫");
            Console.WriteLine(@"烫   #                                                    #   烫");
            Console.WriteLine(@"烫  ##                        #  #                        ##  烫");
            Console.WriteLine(@"烫  ##                                                    ##  烫");
            Console.WriteLine(@"烫  ##                                                    ##  烫");
            Console.WriteLine(@"烫  ##                                                    ##  烫");
            Console.WriteLine(@"烫   #     #                                        #     #   烫");
            Console.WriteLine(@"烫   ##     #                                      #     ##   烫");
            Console.WriteLine(@"烫    #      ##                                  ##      #    烫");
            Console.WriteLine(@"烫     #       ###                            ###       #     烫");
            Console.WriteLine(@"烫      #        ####                      ####        #      烫");
            Console.WriteLine(@"烫       ##          #####            #####          ##       烫");
            Console.WriteLine(@"烫         ##            ##############            ##         烫");
            Console.WriteLine(@"烫           ###                                ###           烫");
            Console.WriteLine(@"烫              ###                          ###              烫");
            Console.WriteLine(@"烫                 #####                #####                 烫");
            Console.WriteLine(@"烫                      ################                      烫");
            Console.WriteLine(@"锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤");
            Console.WriteLine(@"");
        }
    }
    public abstract class Scripts
    {
        public abstract void Script_Process();
        public abstract void InitScript();

    }


}




/*LYF: bets 20181001;
 *YSH: bets 20190101;
 */
