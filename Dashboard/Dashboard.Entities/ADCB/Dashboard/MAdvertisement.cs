//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class MAdvertisement : Base_Dashboard
    {
        private Guid advertisementID;
        [XmlAttribute]
        public Guid AID
        {
            get { return advertisementID; }
            set { advertisementID = value; }
        }

        private string advertisementName;
        [XmlAttribute]
        public string AName
        {
            get { return advertisementName; }
            set { advertisementName = value; }
        }

        private string advertisementDesc;
        [XmlAttribute]
        public string ADesc
        {
            get { return advertisementDesc; }
            set { advertisementDesc = value; }
        }

        private string advertisementDirectory;
        [XmlAttribute]
        public string ADirectory
        {
            get { return advertisementDirectory; }
            set { advertisementDirectory = value; }
        }

        private int advertisementDuration;
        [XmlAttribute]
        public int ADuration
        {
            get { return advertisementDuration; }
            set { advertisementDuration = value; }
        }

        private int advertisementTotalPack;
        [XmlAttribute]
        public int ATotal
        {
            get { return advertisementTotalPack; }
            set { advertisementTotalPack = value; }
        }

        private int advertisementSequence;
        [XmlAttribute]
        public int Sequence
        {
            get { return advertisementSequence; }
            set { advertisementSequence = value; }
        }

        private bool advertisementIsBackground;
        [XmlAttribute]
        public bool AIsBackgroundIMG
        {
            get { return advertisementIsBackground; }
            set { advertisementIsBackground = value; }
        }

        private string absolutePath;
        [XmlAttribute]
        public string AbsolutePath
        {
            get { return absolutePath; }
            set { absolutePath = value; }
        }

        private string relativePath;
        [XmlAttribute]
        public string RelativePathURL
        {
            get { return relativePath; }
            set { relativePath = value; }
        }

    }

}