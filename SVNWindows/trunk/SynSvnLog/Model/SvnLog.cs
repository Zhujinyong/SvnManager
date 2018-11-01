using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SynSvnLog.Model
{
    public class SvnLog
    {
        public string ID { get; set; }

        public string ProjectID { get; set; }

        public string UserID { get; set; }

        public DateTime CreateTime { get; set; }

        public int Revision { get; set; }

        public string Message { get; set; }

        public List<SvnLogFile> logFileLsit { get; set; }
    }
}
