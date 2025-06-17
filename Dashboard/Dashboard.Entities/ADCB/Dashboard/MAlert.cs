//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class MAlert : Base_Dashboard
    {
        private Guid alertID;
        [XmlAttribute]
        public Guid AID
        {
            get { return alertID; }
            set { alertID = value; }
        }

        private string alertDesc;
        [XmlAttribute]
        public string ADesc
        {
            get { return alertDesc; }
            set { alertDesc = value; }
        }

        private int alertMinCardBal;
        [XmlAttribute]
        public int AMinCardBal
        {
            get { return alertMinCardBal; }
            set { alertMinCardBal = value; }
        }

        private int alertMinChequeBal;
        [XmlAttribute]
        public int AMinChequeBal
        {
            get { return alertMinChequeBal; }
            set { alertMinChequeBal = value; }
        }

        private int alertMinPaperBal;
        [XmlAttribute]
        public int AMinPaperBal
        {
            get { return alertMinPaperBal; }
            set { alertMinPaperBal = value; }
        }

        private int alertMinRejCardBal;
        [XmlAttribute]
        public int AMinRejCardBal
        {
            get { return alertMinRejCardBal; }
            set { alertMinRejCardBal = value; }
        }

        private int alertRibFrontBal;
        [XmlAttribute]
        public int ARibFrontBal
        {
            get { return alertRibFrontBal; }
            set { alertRibFrontBal = value; }
        }

        private int alertRibRearBal;
        [XmlAttribute]
        public int ARibRearBal
        {
            get { return alertRibRearBal; }
            set { alertRibRearBal = value; }
        }

        private int alertRibTipBal;
        [XmlAttribute]
        public int ARibTipBal
        {
            get { return alertRibTipBal; }
            set { alertRibTipBal = value; }
        }

        private int alertChequePrintBal;
        [XmlAttribute]
        public int AChequePrintBal
        {
            get { return alertChequePrintBal; }
            set { alertChequePrintBal = value; }
        }

        private int alertChequePrintCatridge;
        [XmlAttribute]
        public int AChequePrintCatridge
        {
            get { return alertChequePrintCatridge; }
            set { alertChequePrintCatridge = value; }
        }

        private int alertCatridgeBal;
        [XmlAttribute]
        public int ACatridgeBal
        {
            get { return alertCatridgeBal; }
            set { alertCatridgeBal = value; }
        }




        private string alertCardEmail;
        [XmlAttribute]
        public string ACardEmail
        {
            get { return alertCardEmail; }
            set { alertCardEmail = value; }
        }

        private string alertCardSMS;
        [XmlAttribute]
        public string ACardSMS
        {
            get { return alertCardSMS; }
            set { alertCardSMS = value; }
        }

        private int alertCardTimeInterval;
        [XmlAttribute]
        public int ACardTimeInterval
        {
            get { return alertCardTimeInterval; }
            set { alertCardTimeInterval = value; }
        }

        private string alertChequeEmail;
        [XmlAttribute]
        public string AChequeEmail
        {
            get { return alertChequeEmail; }
            set { alertChequeEmail = value; }
        }

        private string alertChequeSMS;
        [XmlAttribute]
        public string AChequeSMS
        {
            get { return alertChequeSMS; }
            set { alertChequeSMS = value; }
        }

        private int alertChequeTimeInterval;
        [XmlAttribute]
        public int AChequeTimeInterval
        {
            get { return alertChequeTimeInterval; }
            set { alertChequeTimeInterval = value; }
        }

        private string alertMaintenanceEmail;
        [XmlAttribute]
        public string AMaintenanceEmail
        {
            get { return alertMaintenanceEmail; }
            set { alertMaintenanceEmail = value; }
        }

        private string alertMaintenanceSMS;
        [XmlAttribute]
        public string AMaintenanceSMS
        {
            get { return alertMaintenanceSMS; }
            set { alertMaintenanceSMS = value; }
        }

        private int alertMaintenanceTimeInterval;
        [XmlAttribute]
        public int AMaintenanceTimeInterval
        {
            get { return alertMaintenanceTimeInterval; }
            set { alertMaintenanceTimeInterval = value; }
        }

        private string alertSecurityEmail;
        [XmlAttribute]
        public string ASecurityEmail
        {
            get { return alertSecurityEmail; }
            set { alertSecurityEmail = value; }
        }

        private string alertSecuritySMS;
        [XmlAttribute]
        public string ASecuritySMS
        {
            get { return alertSecuritySMS; }
            set { alertSecuritySMS = value; }
        }

        private int alertSecurityTimeInterval;
        [XmlAttribute]
        public int ASecurityTimeInterval
        {
            get { return alertSecurityTimeInterval; }
            set { alertSecurityTimeInterval = value; }
        }

        private string alertTroubleShootEmail;
        [XmlAttribute]
        public string ATroubleShootEmail
        {
            get { return alertTroubleShootEmail; }
            set { alertTroubleShootEmail = value; }
        }

        private string alertTroubleShootSMS;
        [XmlAttribute]
        public string ATroubleShootSMS
        {
            get { return alertTroubleShootSMS; }
            set { alertTroubleShootSMS = value; }
        }

        private int alertTroubleShootTimeInterval;
        [XmlAttribute]
        public int ATroubleShootTimeInterval
        {
            get { return alertTroubleShootTimeInterval; }
            set { alertTroubleShootTimeInterval = value; }
        }
    }

}