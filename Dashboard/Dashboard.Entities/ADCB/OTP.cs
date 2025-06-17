//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB
{
    [Serializable]
    public class OTP
    {
        [XmlElement]
        public RespHeader Header { get; set; }

        [XmlElement]
        public OTPRespond OTPRespond { get; set; }
    }

    [Serializable]
    public class OTPRespond
    {
        [XmlElement]
        public string Status { get; set; }
    }
}
