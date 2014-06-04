using System.Runtime.Serialization;

namespace Movies.DataContracts.FaultContracts
{
    [DataContract]
    public class InitializationFault
    {
        public InitializationFault(string message)
        {
            Message = message;
        }

        [DataMember]
        public string Message { get; set; }
    }
}