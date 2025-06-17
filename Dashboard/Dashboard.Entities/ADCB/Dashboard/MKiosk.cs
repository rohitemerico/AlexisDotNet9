//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class MKiosk : Base_Dashboard
    {
        private Guid machineID;
        [XmlAttribute]
        public Guid MachineID
        {
            get { return machineID; }
            set { machineID = value; }
        }

        private string machineDescription;
        [XmlAttribute]
        public string MachineDescription
        {
            get { return machineDescription; }
            set { machineDescription = value; }
        }

        private string machineSerial;
        [XmlAttribute]
        public string MachineSerial
        {
            get { return machineSerial.ToUpper(); }
            set { machineSerial = value; }
        }

        private string machineKioskID;
        [XmlAttribute]
        public string MachineKioskID
        {
            get { return machineKioskID; }
            set { machineKioskID = value; }
        }

        private string machineStationID;
        [XmlAttribute]
        public string MachineStationID
        {
            get { return machineStationID; }
            set { machineStationID = value; }
        }

        private string machineAddress;
        [XmlAttribute]
        public string MachineAddress
        {
            get { return machineAddress; }
            set { machineAddress = value; }
        }

        private string machineLatitude;
        [XmlAttribute]
        public string MachineLatitude
        {
            get { return machineLatitude; }
            set { machineLatitude = value; }
        }

        private string machineLongtitude;
        [XmlAttribute]
        public string MachineLongtitude
        {
            get { return machineLongtitude; }
            set { machineLongtitude = value; }
        }

        private Guid machineGroupID;
        [XmlAttribute]
        public Guid MachineGroupID
        {
            get { return machineGroupID; }
            set { machineGroupID = value; }
        }

        private string machineGroupDesc;
        [XmlAttribute]
        public string MachineGroupDesc
        {
            get { return machineGroupDesc; }
            set { machineGroupDesc = value; }
        }

        private string cymacIP;
        [XmlAttribute]
        public string MacIP
        {
            get { return cymacIP; }
            set { cymacIP = value; }
        }

        private string cymacPort;
        [XmlAttribute]
        public string MacPort
        {
            get { return cymacPort; }
            set { cymacPort = value; }
        }

        private bool pilot;
        [XmlAttribute]
        public bool MacPilot
        {
            get { return pilot; }
            set { pilot = value; }
        }
    }
}