using System.Xml.Serialization;

namespace MDM.iOS.Entities.Dashboard
{
    [Serializable]
    public class IniEntity
    {
        private string NewPassword;
        [XmlAttribute]
        public string newpassword
        {
            get { return NewPassword; }
            set { NewPassword = value; }
        }

        private List<PasswordListing> PasswordList;
        [XmlAttribute]
        public List<PasswordListing> passwordlist
        {
            get { return PasswordList; }
            set { PasswordList = value; }
        }


    }
    [Serializable]
    public class PasswordListing
    {

        private string Password;
        [XmlAttribute]
        public string password
        {
            get { return Password; }
            set { Password = value; }
        }


        private DateTime Date;
        [XmlAttribute]
        public DateTime date
        {
            get { return Date; }
            set { Date = value; }
        }


    }
}