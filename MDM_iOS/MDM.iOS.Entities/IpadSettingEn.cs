using System.Xml.Serialization;

namespace MDM.iOS.Entities
{
    // For column SettingValue in [iPadVersionLookUpData] which used during insertion on Dashboard
    [Serializable]
    public class iPadSettings
    {
        private List<Items> item;
        [XmlElement]
        public List<Items> Item
        {
            get
            {
                if (item == null)
                    item = new List<Items>();
                return item;
            }
            set { item = value; }
        }
    }

    [Serializable]
    public class Items
    {
        private string desc;
        [XmlElement]
        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }
        private string value;
        [XmlElement]
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        private string parent_id;
        [XmlElement]
        public string Parent_ID
        {
            get { return this.parent_id; }
            set { this.parent_id = value; }
        }
    }
}
