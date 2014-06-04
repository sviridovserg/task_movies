using System.Runtime.Serialization;

namespace Movies.DataContracts
{
    [DataContract]
    public class AuthData
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}