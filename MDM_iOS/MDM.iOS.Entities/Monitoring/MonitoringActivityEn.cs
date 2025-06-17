using System.Xml.Serialization;

namespace MDM.iOS.Entities.Monitoring
{
    /// <summary>
    /// Monitoring activity entity which contains the details 
    /// such as start time, end time of the specified monitored activity. 
    /// </summary>
    [Serializable]
    public class MonitoringActivityEn
    {
        private string activityID;
        [XmlAnyAttribute]
        public string ActivityID
        {
            get { return activityID; }
            set { activityID = value; }
        }

        private string deviceImei;
        [XmlAnyAttribute]
        public string DeviceIMEI
        {
            get { return deviceImei; }
            set { deviceImei = value; }
        }

        private DateTime startTime;
        [XmlAnyAttribute]
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private DateTime endTime;
        [XmlAnyAttribute]
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
    }
}
