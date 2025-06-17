//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class MApp : Base_Dashboard
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

        private string fPath;
        [XmlAttribute]
        public string FPath
        {
            get { return fPath; }
            set { fPath = value; }
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
        public int FileSize
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

        private bool pilot;
        [XmlAttribute]
        public bool Pilot
        {
            get { return pilot; }
            set { pilot = value; }
        }


    }

}