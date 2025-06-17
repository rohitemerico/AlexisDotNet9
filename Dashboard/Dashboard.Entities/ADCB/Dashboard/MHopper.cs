//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class MHopper : Base_Dashboard
    {
        private Guid hopperID;
        [XmlAttribute]
        public Guid HID
        {
            get { return hopperID; }
            set { hopperID = value; }
        }

        private string hopperName;
        [XmlAttribute]
        public string HName
        {
            get { return hopperName; }
            set { hopperName = value; }
        }

        private Guid hopper1Temp;
        [XmlAttribute]
        public Guid H1Temp
        {
            get { return hopper1Temp; }
            set { hopper1Temp = value; }
        }

        private string hopper1Range;
        [XmlAttribute]
        public string H1Range
        {
            get { return hopper1Range; }
            set { hopper1Range = value; }
        }

        private string hopper1Mask;
        [XmlAttribute]
        public string H1Mask
        {
            get { return hopper1Mask; }
            set { hopper1Mask = value; }
        }

        private Guid hopper2Temp;
        [XmlAttribute]
        public Guid H2Temp
        {
            get { return hopper2Temp; }
            set { hopper2Temp = value; }
        }

        private string hopper2Range;
        [XmlAttribute]
        public string H2Range
        {
            get { return hopper2Range; }
            set { hopper2Range = value; }
        }

        private string hopper2Mask;
        [XmlAttribute]
        public string H2Mask
        {
            get { return hopper2Mask; }
            set { hopper2Mask = value; }
        }

        private Guid hopper3Temp;
        [XmlAttribute]
        public Guid H3Temp
        {
            get { return hopper3Temp; }
            set { hopper3Temp = value; }
        }

        private string hopper3Range;
        [XmlAttribute]
        public string H3Range
        {
            get { return hopper3Range; }
            set { hopper3Range = value; }
        }

        private string hopper3Mask;
        [XmlAttribute]
        public string H3Mask
        {
            get { return hopper3Mask; }
            set { hopper3Mask = value; }
        }

        private Guid hopper4Temp;
        [XmlAttribute]
        public Guid H4Temp
        {
            get { return hopper4Temp; }
            set { hopper4Temp = value; }
        }

        private string hopper4Range;
        [XmlAttribute]
        public string H4Range
        {
            get { return hopper4Range; }
            set { hopper4Range = value; }
        }

        private string hopper4Mask;
        [XmlAttribute]
        public string H4Mask
        {
            get { return hopper4Mask; }
            set { hopper4Mask = value; }
        }

        private Guid hopper5Temp;
        [XmlAttribute]
        public Guid H5Temp
        {
            get { return hopper5Temp; }
            set { hopper5Temp = value; }
        }

        private string hopper5Range;
        [XmlAttribute]
        public string H5Range
        {
            get { return hopper5Range; }
            set { hopper5Range = value; }
        }

        private string hopper5Mask;
        [XmlAttribute]
        public string H5Mask
        {
            get { return hopper5Mask; }
            set { hopper5Mask = value; }
        }

        private Guid hopper6Temp;
        [XmlAttribute]
        public Guid H6Temp
        {
            get { return hopper6Temp; }
            set { hopper6Temp = value; }
        }

        private string hopper6Range;
        [XmlAttribute]
        public string H6Range
        {
            get { return hopper6Range; }
            set { hopper6Range = value; }
        }

        private string hopper6Mask;
        [XmlAttribute]
        public string H6Mask
        {
            get { return hopper6Mask; }
            set { hopper6Mask = value; }
        }

        private Guid hopper7Temp;
        [XmlAttribute]
        public Guid H7Temp
        {
            get { return hopper7Temp; }
            set { hopper7Temp = value; }
        }

        private string hopper7Range;
        [XmlAttribute]
        public string H7Range
        {
            get { return hopper7Range; }
            set { hopper7Range = value; }
        }

        private string hopper7Mask;
        [XmlAttribute]
        public string H7Mask
        {
            get { return hopper7Mask; }
            set { hopper7Mask = value; }
        }

        private Guid hopper8Temp;
        [XmlAttribute]
        public Guid H8Temp
        {
            get { return hopper8Temp; }
            set { hopper8Temp = value; }
        }

        private string hopper8Range;
        [XmlAttribute]
        public string H8Range
        {
            get { return hopper8Range; }
            set { hopper8Range = value; }
        }

        private string hopper8Mask;
        [XmlAttribute]
        public string H8Mask
        {
            get { return hopper8Mask; }
            set { hopper8Mask = value; }
        }

        private string[,] hopperArray;
        [XmlAttribute]
        public string[,] HopperArray
        {
            get { return hopperArray; }
            set { hopperArray = value; }
        }




        private Guid hopper1Desc;
        [XmlAttribute]
        public Guid H1Desc
        {
            get { return hopper1Desc; }
            set { hopper1Desc = value; }
        }
        private Guid hopper2Desc;
        [XmlAttribute]
        public Guid H2Desc
        {
            get { return hopper2Desc; }
            set { hopper2Desc = value; }
        }
        private Guid hopper3Desc;
        [XmlAttribute]
        public Guid H3Desc
        {
            get { return hopper3Desc; }
            set { hopper3Desc = value; }
        }
        private Guid hopper4Desc;
        [XmlAttribute]
        public Guid H4Desc
        {
            get { return hopper4Desc; }
            set { hopper4Desc = value; }
        }
        private Guid hopper5Desc;
        [XmlAttribute]
        public Guid H5Desc
        {
            get { return hopper5Desc; }
            set { hopper5Desc = value; }
        }
        private Guid hopper6Desc;
        [XmlAttribute]
        public Guid H6Desc
        {
            get { return hopper6Desc; }
            set { hopper6Desc = value; }
        }
        private Guid hopper7Desc;
        [XmlAttribute]
        public Guid H7Desc
        {
            get { return hopper7Desc; }
            set { hopper7Desc = value; }
        }
        private Guid hopper8Desc;
        [XmlAttribute]
        public Guid H8Desc
        {
            get { return hopper8Desc; }
            set { hopper8Desc = value; }
        }
    }

}