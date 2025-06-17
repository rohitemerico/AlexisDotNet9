//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB
{
    [Serializable]
    public class CustKioskAct
    {
        [XmlElement]
        public RespHeader Header { get; set; }
        [XmlElement]
        public string ActDate { get; set; }
        [XmlElement]
        public string ActDetail { get; set; }
        [XmlElement]
        public string CustID { get; set; }
        //[XmlElement]
        //public List<KioskActInfo> KioskActInfo { get; set; }
        [XmlElement]
        public List<ADCBResp> ADCBActInfo { get; set; }
    }

    //[Serializable]
    //public class RespKioskInfo_DC
    //{
    //    public string errorCode { get; set; }
    //    public RespHeader Header { get; set; }
    //    public CustKioskAct Detail { get; set; }
    //}

    [Serializable]
    public class KioskActInfo
    {
        [XmlElement]
        public string KioskID { get; set; }
        [XmlElement]
        public string Location { get; set; }
        [XmlElement]
        public int NoOfCount { get; set; }
        [XmlElement]
        public string errorcode { get; set; }
    }
}
