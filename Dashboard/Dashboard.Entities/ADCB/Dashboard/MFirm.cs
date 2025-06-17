//using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class MFirm : Base_Dashboard
    {
        private Guid sysID;
        [XmlAttribute]
        public Guid SysID
        {
            get { return sysID; }
            set { sysID = value; }
        }

        private Guid terminalTypeID;
        [XmlAttribute]
        public Guid TerminalTypeID
        {
            get { return terminalTypeID; }
            set { terminalTypeID = value; }
        }

        private Guid terminalModelID;
        [XmlAttribute]
        public Guid TerminalModelID
        {
            get { return terminalModelID; }
            set { terminalModelID = value; }
        }

        private string filePath;
        [XmlAttribute]
        public string FPath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        private int countDL;
        [XmlAttribute]
        public int CountDL
        {
            get { return countDL; }
            set { countDL = value; }
        }

        private string ver;
        [XmlAttribute]
        public string Ver
        {
            get { return ver; }
            set { ver = value; }
        }

        private int fileSize;
        [XmlAttribute]
        public int FSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        private string type;
        [XmlAttribute]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private bool agentFlag;
        [XmlAttribute]
        public bool AgentFlag
        {
            get { return agentFlag; }
            set { agentFlag = value; }
        }

        private FirmData dataPack;
        [XmlAttribute]
        public FirmData DataPack
        {
            get
            {
                if (dataPack == null)
                    dataPack = new FirmData();
                return dataPack;
            }
            set { dataPack = value; }
        }

        [Serializable]
        public class FirmData
        {
            private Guid sysID;
            [XmlAttribute]
            public Guid SysID
            {
                get { return sysID; }
                set { sysID = value; }
            }

            private Guid firmwareID;
            [XmlAttribute]
            public Guid FirmwareID
            {
                get { return firmwareID; }
                set { firmwareID = value; }
            }

            private int indicator;
            [XmlAttribute]
            public int Indicator
            {
                get { return indicator; }
                set { indicator = value; }
            }

            private string firmData;
            [XmlAttribute]
            public string Data
            {
                get { return firmData; }
                set { firmData = value; }
            }
        }

    }

}