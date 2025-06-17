//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class MDocument : Base_Dashboard
    {
        private Guid documentID;
        [XmlAttribute]
        public Guid DID
        {
            get { return documentID; }
            set { documentID = value; }
        }

        private string documentName;
        [XmlAttribute]
        public string DName
        {
            get { return documentName; }
            set { documentName = value; }
        }

        private string componentID;
        [XmlAttribute]
        public string ComponentID
        {
            get { return componentID; }
            set { componentID = value; }
        }

        #region Document_Settings

        private Guid dPassportComponent;
        [XmlAttribute]
        public Guid DPassportComponent
        {
            get { return dPassportComponent; }
            set { dPassportComponent = value; }
        }

        private Guid dEidaComponent;
        [XmlAttribute]
        public Guid DEidaComponent
        {
            get { return dEidaComponent; }
            set { dEidaComponent = value; }
        }

        private Guid dIDComponent;
        [XmlAttribute]
        public Guid DIDComponent
        {
            get { return dIDComponent; }
            set { dIDComponent = value; }
        }

        private Guid dIncomeComponent;
        [XmlAttribute]
        public Guid DIncomeComponent
        {
            get { return dIncomeComponent; }
            set { dIncomeComponent = value; }
        }

        private Guid dAppFormComponent;
        [XmlAttribute]
        public Guid DAppFormComponent
        {
            get { return dAppFormComponent; }
            set { dAppFormComponent = value; }
        }

        private Guid dEmploymentComponent;
        [XmlAttribute]
        public Guid DEmploymentComponent
        {
            get { return dEmploymentComponent; }
            set { dEmploymentComponent = value; }
        }

        private Guid dSupportDocComponent;
        [XmlAttribute]
        public Guid DSupportDocComponent
        {
            get { return dSupportDocComponent; }
            set { dSupportDocComponent = value; }
        }

        private int dPassportSwallow;
        [XmlAttribute]
        public int DPassportSwallow
        {
            get { return dPassportSwallow; }
            set { dPassportSwallow = value; }
        }

        private int dEidaSwallow;
        [XmlAttribute]
        public int DEidaSwallow
        {
            get { return dEidaSwallow; }
            set { dEidaSwallow = value; }
        }

        private int dIDSwallow;
        [XmlAttribute]
        public int DIDSwallow
        {
            get { return dIDSwallow; }
            set { dIDSwallow = value; }
        }

        private int dIncomeSwallow;
        [XmlAttribute]
        public int DIncomeSwallow
        {
            get { return dIncomeSwallow; }
            set { dIncomeSwallow = value; }
        }

        private int dAppFormSwallow;
        [XmlAttribute]
        public int DAppFormSwallow
        {
            get { return dAppFormSwallow; }
            set { dAppFormSwallow = value; }
        }

        private int dEmploymentDocSwallow;
        [XmlAttribute]
        public int DEmploymentDocSwallow
        {
            get { return dEmploymentDocSwallow; }
            set { dEmploymentDocSwallow = value; }
        }

        private int dSupportDocSwallow;
        [XmlAttribute]
        public int DSupportDocSwallow
        {
            get { return dSupportDocSwallow; }
            set { dSupportDocSwallow = value; }
        }

        private int dPassportPrint;
        [XmlAttribute]
        public int DPassportPrint
        {
            get { return dPassportPrint; }
            set { dPassportPrint = value; }
        }

        private int dEidaPrint;
        [XmlAttribute]
        public int DEidaPrint
        {
            get { return dEidaPrint; }
            set { dEidaPrint = value; }
        }

        private int dIDPrint;
        [XmlAttribute]
        public int DIDPrint
        {
            get { return dIDPrint; }
            set { dIDPrint = value; }
        }

        private int dIncomePrint;
        [XmlAttribute]
        public int DIncomePrint
        {
            get { return dIncomePrint; }
            set { dIncomePrint = value; }
        }

        private int dAppFormPrint;
        [XmlAttribute]
        public int DAppFormPrint
        {
            get { return dAppFormPrint; }
            set { dAppFormPrint = value; }
        }

        private int dEmploymentDocPrint;
        [XmlAttribute]
        public int DEmploymentDocPrint
        {
            get { return dEmploymentDocPrint; }
            set { dEmploymentDocPrint = value; }
        }

        private int dSupportDocPrint;
        [XmlAttribute]
        public int DSupportDocPrint
        {
            get { return dSupportDocPrint; }
            set { dSupportDocPrint = value; }
        }

        private List<string> docComponent;
        [XmlAttribute]
        public List<string> DocComponent
        {
            get
            {
                if (docComponent == null)
                    docComponent = new List<string>();
                return docComponent;
            }
            set { docComponent = value; }
        }

        #endregion






        /* CY */

        private Guid cyDocID;
        [XmlAttribute]
        public Guid DocID
        {
            get { return cyDocID; }
            set { cyDocID = value; }
        }


        private string cyDocName;
        [XmlAttribute]
        public string DocName
        {
            get { return cyDocName; }
            set { cyDocName = value; }
        }

        private string cyComponentName;
        [XmlAttribute]
        public string ComponentName
        {
            get { return cyComponentName; }
            set { cyComponentName = value; }
        }

        private int cyDocSwallow;
        [XmlAttribute]
        public int DocSwallow
        {
            get { return cyDocSwallow; }
            set { cyDocSwallow = value; }
        }

        private int cyDocPrint;
        [XmlAttribute]
        public int DocPrint
        {
            get { return cyDocPrint; }
            set { cyDocPrint = value; }
        }

    }

}