using System.Xml.Serialization;

namespace MDM.iOS.Entities.MDM
{
    /// <summary>
    /// Entity class for the managed device's certificate list. 
    /// Used to store query responses from the iPad device with the 
    /// command "CertificateList". 
    /// </summary>

    [Serializable]
    public class DeviceCertificateEn
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


        private string certName;
        [XmlAnyAttribute]
        public string CertName
        {
            get { return certName; }
            set { certName = value; }
        }

        private bool isIdentityCert;
        [XmlAnyAttribute]
        public bool IsIdentityCert
        {
            get { return isIdentityCert; }
            set { isIdentityCert = value; }
        }
    }
}
