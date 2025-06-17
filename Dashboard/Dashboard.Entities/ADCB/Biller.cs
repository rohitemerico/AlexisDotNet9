//using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Dashboard.Entities.ADCB
{
    [Serializable]
    public class Biller
    {
        [XmlAttribute]
        public string billerName;
        [XmlAttribute]
        public string billerAccount;
        [XmlAttribute]
        public string orgnCode;

        public Biller()
        {
        }

        public Biller(string Name, string Account, string Code)
        {
            billerName = Name;
            billerAccount = Account;
            orgnCode = Code;
        }
    }

}