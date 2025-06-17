//using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Dashboard.Entities.ADCB
{
    [Serializable]
    public class ServiceRequest
    {
        private string billAccountNo;
        [XmlAttribute]
        public string BillAccountNo
        {
            get { return billAccountNo; }
            set { billAccountNo = value; }
        }
        private string serviceChargeAmt;
        [XmlAttribute]
        public string ServiceChargeAmt
        {
            get { return serviceChargeAmt; }
            set { serviceChargeAmt = value; }
        }

        private string orgCode;
        [XmlAttribute]
        public string OrgCode
        {
            get { return orgCode; }
            set { orgCode = value; }
        }
        private string billAmt;
        [XmlAttribute]
        public string BillAmt
        {
            get { return billAmt; }
            set { billAmt = value; }
        }
        private string billerAccount;
        [XmlAttribute]
        public string BillerAccount
        {
            get { return billerAccount; }
            set { billerAccount = value; }
        }
        private string nameOnCard;
        [XmlAttribute]
        public string NameOnCard
        {
            get { return nameOnCard; }
            set { nameOnCard = value; }
        }
        private string oldNric;
        [XmlAttribute]
        public string OldNric
        {
            get { return oldNric; }
            set { oldNric = value; }
        }
        private string postcode;
        [XmlAttribute]
        public string Postcode
        {
            get { return postcode; }
            set { postcode = value; }
        }
        private string placeOfIssue;
        [XmlAttribute]
        public string PlaceOfIssue
        {
            get { return placeOfIssue; }
            set { placeOfIssue = value; }
        }
        private string bumiStatus;
        [XmlAttribute]
        public string BumiStatus
        {
            get { return bumiStatus; }
            set { bumiStatus = value; }
        }
        private string citizenship;
        [XmlAttribute]
        public string Citizenship
        {
            get { return citizenship; }
            set { citizenship = value; }
        }
        private string machineID;
        [XmlAttribute]
        public string MachineID
        {
            get { return machineID; }
            set { machineID = value; }
        }

        private string agentID;
        [XmlAttribute]
        public string AgentID
        {
            get { return agentID; }
            set { agentID = value; }
        }

        private string transactionDate;
        [XmlAttribute]
        public string TransactionDate
        {
            get { return transactionDate; }
            set { transactionDate = value; }
        }

        private string accountType;
        [XmlAttribute]
        public string AccountType
        {
            get { return accountType; }
            set { accountType = value; }
        }

        private string typeOfID;
        [XmlAttribute]
        public string TypeOfID
        {
            get { return typeOfID; }
            set { typeOfID = value; }
        }

        private string customerName;
        [XmlAttribute]
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        private string address1;
        [XmlAttribute]
        public string Address1
        {
            get { return address1; }
            set { address1 = value; }
        }

        private string address2;
        [XmlAttribute]
        public string Address2
        {
            get { return address2; }
            set { address2 = value; }
        }

        private string address3;
        [XmlAttribute]
        public string Address3
        {
            get { return address3; }
            set { address3 = value; }
        }

        private string email;
        [XmlAttribute]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string mobileNo;
        [XmlAttribute]
        public string MobileNo
        {
            get { return mobileNo; }
            set { mobileNo = value; }
        }

        private string a4Filename;
        [XmlAttribute]
        public string A4Filename
        {
            get { return a4Filename; }
            set { a4Filename = value; }
        }

        private string pictureFileName;
        [XmlAttribute]
        public string PictureFileName
        {
            get { return pictureFileName; }
            set { pictureFileName = value; }
        }

        private string accountNO;
        [XmlAttribute]
        public string AccountNO
        {
            get { return accountNO; }
            set { accountNO = value; }
        }

        private string totalDeposit;
        [XmlAttribute]
        public string TotalDeposit
        {
            get { return totalDeposit; }
            set { totalDeposit = value; }
        }

        private string totalDepositReversal;
        [XmlAttribute]
        public string TotalDepositReversal
        {
            get { return totalDepositReversal; }
            set { totalDepositReversal = value; }
        }

        private string totalWithdrawal;
        [XmlAttribute]
        public string TotalWithdrawal
        {
            get { return totalWithdrawal; }
            set { totalWithdrawal = value; }
        }

        private string totalWithdrawalReversal;
        [XmlAttribute]
        public string TotalWithdrawalReversal
        {
            get { return totalWithdrawalReversal; }
            set { totalWithdrawalReversal = value; }
        }

        private string[] deno;
        [XmlArrayItem]
        public string[] Deno
        {
            get { return deno; }
            set { deno = value; }
        }

        private string cardNo;
        [XmlAttribute]
        public string CardNo
        {
            get { return cardNo; }
            set { cardNo = value; }
        }

        private string nRIC;
        [XmlAttribute]
        public string NRIC
        {
            get { return nRIC; }
            set { nRIC = value; }
        }

        private string password;
        [XmlAttribute]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string totalRemainingCard;
        [XmlAttribute]
        public string TotalRemainingCard
        {
            get { return totalRemainingCard; }
            set { totalRemainingCard = value; }
        }

        private string totalRemainingChequeBook;
        [XmlAttribute]
        public string TotalRemainingChequeBook
        {
            get { return totalRemainingChequeBook; }
            set { totalRemainingChequeBook = value; }
        }

        private string cardType;
        [XmlAttribute]
        public string CardType
        {
            get { return cardType; }
            set { cardType = value; }
        }

        private string cardPin;
        [XmlAttribute]
        public string CardPin
        {
            get { return cardPin; }
            set { cardPin = value; }
        }

        private string refID;
        [XmlAttribute]
        public string RefID
        {
            get { return refID; }
            set { refID = value; }
        }

        private string gender;
        [XmlAttribute]
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        private string dob;
        [XmlAttribute]
        public string Dob
        {
            get { return dob; }
            set { dob = value; }
        }

        private string race;
        [XmlAttribute]
        public string Race
        {
            get { return race; }
            set { race = value; }
        }

        private string birthPlace;
        [XmlAttribute]
        public string BirthPlace
        {
            get { return birthPlace; }
            set { birthPlace = value; }
        }

        private string religion;
        [XmlAttribute]
        public string Religion
        {
            get { return religion; }
            set { religion = value; }
        }

        private string maritialStatus;
        [XmlAttribute]
        public string MaritialStatus
        {
            get { return maritialStatus; }
            set { maritialStatus = value; }
        }

        private string mothersMaiden;
        [XmlAttribute]
        public string MothersMaiden
        {
            get { return mothersMaiden; }
            set { mothersMaiden = value; }
        }

        private string employer;
        [XmlAttribute]
        public string Employer
        {
            get { return employer; }
            set { employer = value; }
        }

        private string designation;
        [XmlAttribute]
        public string Designation
        {
            get { return designation; }
            set { designation = value; }
        }

        private string annualIncome;
        [XmlAttribute]
        public string AnnualIncome
        {
            get { return annualIncome; }
            set { annualIncome = value; }
        }

        private string advertisementID;
        [XmlAttribute]
        public string AdvertisementID
        {
            get { return advertisementID; }
            set { advertisementID = value; }
        }

        private string patchID;
        [XmlAttribute]
        public string PatchID
        {
            get { return patchID; }
            set { patchID = value; }
        }

        private string identifier;
        [XmlAttribute]
        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        private string custNo1;
        [XmlAttribute]
        public string CustNo1
        {
            get { return custNo1; }
            set { custNo1 = value; }
        }

        private int seqNo;
        [XmlAttribute]
        public int SeqNo
        {
            get { return seqNo; }
            set { seqNo = value; }

        }
        private List<Components> componentItems;
        [XmlArray]
        public List<Components> ComponentItems
        {
            get { return componentItems; }
            set { componentItems = value; }
        }
    }
}