using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SynSvnLog.Model
{
    public class SvnUser
    {
        public string ID { get; set; }

        public string DomainAccount { get; set; }

        public string RealName { get; set; }

        public string Description { get; set; }
    }
}
