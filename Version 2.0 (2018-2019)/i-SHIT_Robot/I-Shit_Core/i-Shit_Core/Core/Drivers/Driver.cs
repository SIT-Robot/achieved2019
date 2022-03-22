/*用来测试，不要在开其他文件了*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace i_Shit_Core.Core.Drivers
{
    public static partial class Driver
    {
        internal static SynchronizationContext UIThreadOperator = SynchronizationContext.Current;
    }

    public static partial class Driver
    {
        public static void Test_aaaa()
        {
            System.Console.WriteLine("aaaa");
        }
    }
}
