using System.Xml.Serialization;

namespace MDM.iOS.Entities.Dashboard
{
    /// <summary>
    /// The Entity class for branch creations. 
    /// </summary>
    [Serializable]
    public class UIBranchEn : Base_Dashboard
    {
        private Guid bID;
        [XmlAttribute]
        public Guid BranchID
        {
            get
            {
                if (bID == Guid.Empty)
                {
                    bID = Guid.NewGuid();
                }
                return bID;
            }
            set { bID = value; }
        }


        private string bDesc;
        [XmlAttribute]
        public string BranchDesc
        {
            get { return bDesc; }
            set { bDesc = value; }
        }


        private bool monday;
        [XmlAnyAttribute]
        public bool Monday
        {
            get { return monday; }
            set { monday = value; }
        }


        private bool tuesday;
        [XmlAnyAttribute]
        public bool Tuesday
        {
            get { return tuesday; }
            set { tuesday = value; }
        }


        private bool wednesday;
        [XmlAnyAttribute]
        public bool Wednesday
        {
            get { return wednesday; }
            set { wednesday = value; }
        }


        private bool thursday;
        [XmlAnyAttribute]
        public bool Thursday
        {
            get { return thursday; }
            set { thursday = value; }
        }


        private bool friday;
        [XmlAnyAttribute]
        public bool Friday
        {
            get { return friday; }
            set { friday = value; }
        }


        private bool saturday;
        [XmlAnyAttribute]
        public bool Saturday
        {
            get { return saturday; }
            set { saturday = value; }
        }


        private bool sunday;
        [XmlAnyAttribute]
        public bool Sunday
        {
            get { return sunday; }
            set { sunday = value; }
        }


        private TimeSpan openTime;
        [XmlAnyAttribute]
        public TimeSpan OpenTime
        {
            get { return openTime; }
            set { openTime = value; }
        }


        private TimeSpan closeTime;
        [XmlAnyAttribute]
        public TimeSpan CloseTime
        {
            get { return closeTime; }
            set { closeTime = value; }
        }
    }
}
