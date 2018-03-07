using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;

namespace Fighting.Orleans
{
    public class OrleansOptions
    {
        public string ClusterId { get; set; }

        public IPAddress ClusterAddress { get; set; }

        public int ClusterPort { get; set; }
    }
}
