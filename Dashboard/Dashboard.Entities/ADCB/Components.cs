//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB
{
    [Serializable]
    public class Components : Params
    {
        public string queryString;
        [XmlAttribute]
        private string QueryString
        {
            get { return queryString; }
            set { queryString = value; }
        }

        public List<Params> parameters;
        [XmlAttribute]
        private List<Params> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        private List<ComponentItems> items;
        [XmlElement]
        public List<ComponentItems> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
    [Serializable]
    public class ComponentItems
    {
        [XmlElement]
        public int ComponentID;
        [XmlElement]
        public int ComponentStatus;
        [XmlElement]
        public string ComponentMisc;
        [XmlElement]
        public long ComponentValue;

        public ComponentItems()
        {

        }
        public ComponentItems(int componentID, int componentStatus, string componentMisc, long componentValue)
        {
            ComponentID = componentID;
            ComponentStatus = componentStatus;
            ComponentMisc = componentMisc;
            ComponentValue = componentValue;
        }

    }

    public enum ComponentName
    {
        /**
        CardReader = 1,
        BioReader = 2,
        CardDispenser = 3,
        CardEmboser = 4,
        CashDeposit = 5,
        CashDispenser = 6,
        SignaturePad = 7,
        PrinterA4 = 8,
        ThermalReceiptPrinter = 9,
        A4Scanner = 10,
        ChequePrinter = 11,
        ChequeScanner = 12,
        PinPad = 13,
        BarcodeScanner = 14,
        UPS = 15,
        Battery = 16,
        DoorSensor = 17,
        Terminal = 18,
        GPS = 19,
        MobileDataSignal = 20
         */
        CardEmbosser = 1,
        PassportScanner = 2,
        A4Printer = 3,
        A4Scanner = 4,
        CardReader = 5,
        ChequePrinter = 6,
        PinPad = 7,
        WebCam = 8,
        SignaturePad = 9,
        ReceiptPrinter = 10,
        ThumbprintScanner = 11,
        UPS = 12,
        Speaker = 13,
        ChequeShutterGate = 14,
        CardShutterGate = 15,
        TouchScreenMonitor = 16,
        AdsTV = 17,
        CardScanner = 18,
        DoorSensor = 19
    }
}