using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB
{
    [Serializable]
    //[XmlRoot(ElementName = "Resp", Namespace = "")]
    public class ReqDetail
    {
        //[XmlElement]
        //public string CardProductGroup { get; set; }

        [XmlElement]
        public string CardBin { get; set; }

        [XmlElement]
        public string CustomerCategory { get; set; }
    }

    [Serializable]
    public class RespDetail_CC
    {
        [XmlElement]
        public string CardBin { get; set; }

        [XmlElement]
        public List<Kiosk> Kiosk { get; set; }
    }

    [Serializable]
    public class RespDetail_DC
    {
        [XmlElement]
        public string CardProductGroup { get; set; }

        [XmlElement]
        public string CustomerCategory { get; set; }

        [XmlElement]
        public List<Kiosk> Kiosk { get; set; }
    }

    [Serializable]
    public class Kiosk
    {
        [XmlElement]
        public string KioskID { get; set; }

        [XmlElement]
        public string KioskDesc { get; set; }

        [XmlElement]
        public string Location { get; set; }
    }
}
