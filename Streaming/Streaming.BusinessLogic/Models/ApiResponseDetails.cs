
using System.Runtime.Serialization;

namespace Streaming.BusinessLogic.Models
{
    [DataContract]
    public class ApiResponseDetails<T>
    {
        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public int StatusCode { get; set; }

        public bool IsSuccessStatusCode => StatusCode / 200 == 1;

        [DataMember]
        public bool IsError { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ResponseException { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public T Result { get; set; }
    }
}
