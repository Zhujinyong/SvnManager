using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Configuration;

namespace SynSvnLog
{
    partial class SynLogService : ServiceBase
    {
        private static object synLock = new object();

        private static string _serviceName = "同步svn日志";

        private static FileHelper fileHelper = new FileHelper();

        private readonly string _logPath = Application.StartupPath + @"\Log";

        private System.Timers.Timer _timer;

        public SynLogService()
        {
            InitializeComponent();
            InitService();
        }

        private void InitService()
        {
            base.CanShutdown = true;
            base.CanStop = true;
            base.CanPauseAndContinue = true;
            this.ServiceName = _serviceName;
            this.AutoLog = false;//为了使用自定义日志，必须将 AutoLog 设置为 false

            _timer = new System.Timers.Timer();
            _timer.Elapsed += new ElapsedEventHandler(tim_Elapsed);
            //5分钟执行一次
            double interval = 300000;
            double.TryParse( ConfigurationManager.AppSettings.Get("Interval"),out interval);
            _timer.Interval = interval;
            _timer.AutoReset = true;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                this._timer.Enabled = true;
                this._timer.Start();
            }
            catch (Exception ex)
            {
                MessageAdd("OnStart错误：" + ex.Message);
            }
            MessageAdd(_serviceName + "已成功启动!");
        }

        protected override void OnStop()
        {
            try
            {
                this._timer.Stop();
            }
            catch (Exception ex)
            {
                MessageAdd("OnStop错误：" + ex.Message);
            }
            MessageAdd(_serviceName + "已停止!");
        }

        protected override void OnContinue()
        {
            this._timer.Start();
            base.OnContinue();
        }

        protected override void OnPause()
        {
            this._timer.Stop();
            base.OnPause();
        }

        private void tim_Elapsed(object sender, EventArgs e)
        {
            StartThread();
        }

        /// <summary>  
        /// 开始任务 
        /// </summary>  
        private void StartThread()
        {
            new SvnLogService().UpdateSvnLogToDatabase();
            //MessageAdd(ServiceName + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        /// <summary>  
        ///  日志记录
        /// </summary>  
        /// <param name="serviceName">内容</param>  
        public void MessageAdd(string str)
        {
            try
            {
                fileHelper.WriteLogFile(_logPath, str);//写入记录日志
            }
            catch(Exception ex)
            {
                MessageAdd("写入记录日志错误：" + ex.Message);
            }
        }
    }
}
