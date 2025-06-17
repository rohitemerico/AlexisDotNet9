//using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Dashboard.Entities.ADCB
{
    [Serializable]
    public class Machine : Components
    {
        public long dataUsage;
        [XmlAttribute]
        private long DataUsage
        {
            get { return dataUsage; }
            set { dataUsage = value; }
        }

        public long wifiUsage;
        [XmlAttribute]
        private long WifiUsage
        {
            get { return wifiUsage; }
            set { wifiUsage = value; }
        }


        public string machineID;
        public bool general;
        public bool cardReader;
        [XmlAttribute]
        private bool CardReader
        {
            get { return cardReader; }
            set { cardReader = value; }
        }
        public bool bioReader;
        [XmlAttribute]
        private bool BioReader
        {
            get { return bioReader; }
            set { bioReader = value; }
        }
        public bool signature;
        [XmlAttribute]
        private bool Signature
        {
            get { return signature; }
            set { signature = value; }
        }
        public bool receipt;
        [XmlAttribute]
        private bool Receipt
        {
            get { return receipt; }
            set { receipt = value; }
        }
        public bool pinPad;
        [XmlAttribute]
        private bool PinPad
        {
            get { return pinPad; }
            set { pinPad = value; }
        }
        public bool scanner;
        [XmlAttribute]
        private bool Scanner
        {
            get { return scanner; }
            set { scanner = value; }
        }
        public int cardDispenser;
        [XmlAttribute]
        private int CardDispenser
        {
            get { return cardDispenser; }
            set { cardDispenser = value; }
        }
        public int cashDeposit;
        [XmlAttribute]
        private int CashDeposit
        {
            get { return cashDeposit; }
            set { cashDeposit = value; }
        }

        //MONEY
        public int rm1;
        [XmlAttribute]
        private int Rm1
        {
            get { return rm1; }
            set { rm1 = value; }
        }
        public int rm2;
        [XmlAttribute]
        private int Rm2
        {
            get { return rm2; }
            set { rm2 = value; }
        }
        public int rm5;
        [XmlAttribute]
        private int Rm5
        {
            get { return rm5; }
            set { rm5 = value; }
        }
        public int rm10;
        [XmlAttribute]
        private int Rm10
        {
            get { return rm10; }
            set { rm10 = value; }
        }
        public int rm20;
        [XmlAttribute]
        private int Rm20
        {
            get { return rm20; }
            set { rm20 = value; }
        }
        public int rm50;
        [XmlAttribute]
        private int Rm50
        {
            get { return rm50; }
            set { rm50 = value; }
        }
        public int rm100;
        [XmlAttribute]
        private int Rm100
        {
            get { return rm100; }
            set { rm100 = value; }
        }



        public int numberOfCards;
        [XmlAttribute]
        private int NumberOfCards
        {
            get { return numberOfCards; }
            set { numberOfCards = value; }
        }

        [XmlAttribute]
        private bool General
        {
            get { return general; }
            set { general = value; }
        }

        [XmlAttribute]
        private string MachineID
        {
            get { return machineID; }
            set { machineID = value; }
        }
        public string kioskID;
        [XmlAttribute]
        private string KioskID
        {
            get { return kioskID; }
            set { kioskID = value; }
        }

        public string tranID;
        [XmlAttribute]
        private string TranID
        {
            get { return tranID; }
            set { tranID = value; }
        }
        public DateTime transDate;
        [XmlAttribute]
        private DateTime TransDate
        {
            get { return transDate; }
            set { transDate = value; }
        }
        public string queuePath;
        [XmlAttribute]
        private string QueuePath
        {
            get { return queuePath; }
            set { queuePath = value; }
        }
        public int cardStatus;
        [XmlAttribute]
        private int CardStatus
        {
            get { return cardStatus; }
            set { cardStatus = value; }
        }
        public int cashStatus;
        [XmlAttribute]
        private int CashStatus
        {
            get { return cashStatus; }
            set { cashStatus = value; }
        }
        public string transactionType;
        [XmlAttribute]
        private string TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }

        public int alarm;
        [XmlAnyAttribute]
        private int Alarm
        {
            get { return alarm; }
            set { alarm = value; }
        }

        public string username;
        [XmlAnyAttribute]
        private string Username
        {
            get { return username; }
            set { username = value; }
        }


        public Guid advID;
        [XmlAnyAttribute]
        private Guid AdvertisementID
        {
            get { return advID; }
            set { advID = value; }
        }

        public int identifier;
        [XmlAttribute]
        private int Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        public string advData;
        [XmlAnyAttribute]
        private string AdvertisementData
        {
            get { return advData; }
            set { advData = value; }
        }

        private bool agentFlag;
        [XmlAttribute]
        public bool AgentFlag
        {
            get { return agentFlag; }
            set { agentFlag = value; }
        }
    }
}