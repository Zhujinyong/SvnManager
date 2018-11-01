using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SynSvnLog.Model
{
    public class SvnLogFile
    {
        public string ID { get; set; }

        public string LogID { get; set; }

        public string Path { get; set; }

        public string Action { get; set; }

    }
}
