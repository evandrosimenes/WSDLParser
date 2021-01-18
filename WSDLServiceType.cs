using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Six
{
    public class OperationWSDL
    {
        public string name { get; set; }
        public string soapaction { get; set; }
        public string input { get; set; }
        public string output { get; set; }
    }

    public class PortWSDL
    {
        public string name { get; set; }
        public string address { get; set; }
        public List<OperationWSDL> operations { get; set; }
    }

    public class WSDLServiceType
    {
        public string name { get; set; }
        public List<PortWSDL> port { get; set; }
    }
}
