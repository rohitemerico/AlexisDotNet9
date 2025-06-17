using System.Xml.Serialization;

namespace MDM.iOS.Entities.MDM
{
    /// <summary>
    /// This class stores the query information returned from 
    /// the iPad devices related to installed application lists
    /// in the form of an entity class. Not to be confused with the 
    /// En_MDMApp class.
    /// </summary>

    [Serializable]
    public class InstalledAppEn
    {
        private Guid id;
        [XmlAnyAttribute]
        public Guid ID
        {
            get { return id; }
            set { id = value; }
        }

        private string udid;
        [XmlAnyAttribute]
        public string UDID
        {
            get { return udid; }
            set { udid = value; }
        }

        private string appName;
        [XmlAnyAttribute]
        public string AppName
        {
            get { return appName; }
            set { appName = value; }
        }


        private string version;
        [XmlAnyAttribute]
        public string Version
        {
            get { return version; }
            set { version = value; }
        }


        private string identifier;
        [XmlAnyAttribute]
        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

    }
}
