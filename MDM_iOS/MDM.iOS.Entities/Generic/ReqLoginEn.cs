using System.Xml.Serialization;

namespace MDM.iOS.Entities.Generic
{
    /// <summary>
    /// Entity used when the user logs into the system. 
    /// Contains the username and password. 
    /// </summary>
    [Serializable]
    public class ReqLoginEn
    {

        private string username;
        [XmlAnyAttribute]
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string pwd;
        [XmlAnyAttribute]
        public string PWD
        {
            get { return pwd; }
            set { pwd = value; }
        }
    }
}
