using SharpSvn;
using System;
using System.Collections.Generic;

namespace Centa.Svn
{
    public class SvnReversionModel
    {
        public long Reversion { get; set; }

        public string Author { get; set; }

        public DateTime Time { get; set; }

        public string LogMessage { get; set; }

        public List<SvnPathMessage> PathList { get; set; }
    }

    public class SvnPathMessage
    {
        public string Path { get; set; }

        public string Action { get; set; }

        public DateTime Time { get; set; }

        public long Reversion { get; set; }

        public string Author { get; set; }

        public string LogMessage { get; set; }
    }

    public class SvnFileMessage
    {
        public string Path { get; set; }

        public string Action { get; set; }

        public DateTime Time { get; set; }

        public long Reversion { get; set; }

        public List<SvnPathMessage> ReversionList { get; set; }
    }


}
