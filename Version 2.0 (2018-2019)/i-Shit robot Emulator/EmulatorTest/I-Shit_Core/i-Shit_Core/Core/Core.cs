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
            String scriptName = System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath);
            coreWin.Text = scriptName + " - " + coreWin.Text;
            coreWin.Show();//显示log窗口
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High;//进行优先级高
            Console.WriteLine("i-SHIT Core Started. Version: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + ". Created by Strawing Lee. Last Built in " + System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location));
            Huaji();
            Console.WriteLine("Init Script: " + scriptName + " ...");
            s.InitScript();
            new Thread(new ThreadStart(delegate
            {
                if (Drivers.Driver.Emulator_Mode == true)
                {
                    Console.WriteLine("Emulator Enabled！");
                    Console.WriteLine("Please config emulator and click START to continue.");
                    while (!Drivers.Driver.Emulator_Ready)
                    {
                        Thread.Sleep(1);//这么做是为了让while true不吃CPU
                    };
                }
                s.Script_Process(); Console.WriteLine("Script Process Finished.烫");
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
