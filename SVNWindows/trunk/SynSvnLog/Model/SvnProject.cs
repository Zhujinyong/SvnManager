using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SynSvnLog.Model
{
    public class SvnProject
    {
        public string ID { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public int Revision { get; set; }

        public string DomainAccount { get; set; }

        public string Password { get; set; }
    }
}
