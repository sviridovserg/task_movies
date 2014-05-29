using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Movies.DataContracts.FaultContracts
{
    [DataContract]
    public class InitializationFault
    {
        [DataMember]
        public string Message {get;set;}

        public InitializationFault(string message) 
        {
            Message = message;
        }
    }
}