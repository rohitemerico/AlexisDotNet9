//using System.Threading.Tasks;

namespace Dashboard.Entities.ADCB
{
    [Serializable]
    public class RespKioskInfo_DC
    {
        public string errorCode { get; set; }
        public RespHeader Header { get; set; }
        public RespDetail_DC Detail { get; set; }
    }

    public class RespKioskInfo_CC
    {
        public string errorCode { get; set; }
        public RespHeader Header { get; set; }
        public RespDetail_CC Detail { get; set; }
    }

    public class ReqKioskInfo
    {
        public ReqHeader Header { get; set; }
        public ReqDetail Detail { get; set; }
    }
}
