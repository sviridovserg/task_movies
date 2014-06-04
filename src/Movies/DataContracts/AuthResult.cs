using System.Runtime.Serialization;

namespace Movies.DataContracts
{
    [DataContract]
    public class AuthResult
    {
        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Login { get; set; }
    }
}