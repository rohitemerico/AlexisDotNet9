//using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Dashboard.Entities.ADCB
{
    [Serializable]
    public class Params
    {
        [XmlElement]
        public string dataName { get; set; }
        [XmlElement]
        public string dataType { get; set; }
        [XmlElement]
        public object dataValue { get; set; }

        public Params()
        {
        }
        public Params(string DataName, string DataType, object DataValue)
        {
            dataName = DataName;
            dataType = DataType;
            dataValue = DataValue;
        }
    }
}
