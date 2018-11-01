using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace SynSvnLog
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //调试使用
            //new SvnLogService().UpdateSvnLogToDatabase();
            //return;
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new SynLogService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
