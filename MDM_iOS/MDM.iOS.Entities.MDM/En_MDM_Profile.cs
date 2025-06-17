using System.Xml.Serialization;

namespace MDM.iOS.Entities.MDM
{
    [Serializable]
    public class En_MDM_Profile
    {
        public Guid profile_ID;
        [XmlAttribute]
        private Guid Profile_ID
        {
            get { return profile_ID; }
            set { profile_ID = value; }
        }

        public string profile_Name;
        [XmlAttribute]
        private string Profile_Name
        {
            get { return profile_Name; }
            set { profile_Name = value; }
        }


        public string profile_Desc;
        [XmlAttribute]
        private string Profile_Desc
        {
            get { return profile_Desc; }
            set { profile_Desc = value; }
        }


        public Guid profile_GroupID;
        [XmlAttribute]
        private Guid Profile_GroupID
        {
            get { return profile_GroupID; }
            set { profile_GroupID = value; }
        }


        public int profile_Status;
        [XmlAttribute]
        private int Profile_Status
        {
            get { return profile_Status; }
            set { profile_Status = value; }
        }


        public DateTime lastUpdateDate;
        [XmlAttribute]
        private DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }

        public string plistContent;
        [XmlAttribute]
        private string PlistContent
        {
            get { return plistContent; }
            set { plistContent = value; }
        }

        private string checkInUrl;
        [XmlAttribute]
        public string CheckInUrl
        {
            get { return checkInUrl; }
            set { checkInUrl = value; }
        }

        private string checkOutUrl;
        [XmlAttribute]
        public string CheckOutUrl
        {
            get { return checkOutUrl; }
            set { checkOutUrl = value; }
        }

        private string enrollmentUrl;
        [XmlAttribute]
        public string EnrollmentUrl
        {
            get { return enrollmentUrl; }
            set { enrollmentUrl = value; }
        }

        private string certPath;
        [XmlAttribute]
        public string CertPath
        {
            get { return certPath; }
            set { certPath = value; }
        }

        private List<RestrictionEn> restrictionProperties;
        [XmlAttribute]
        public List<RestrictionEn> RestrictionProperties
        {
            get
            {
                if (restrictionProperties == null)
                    restrictionProperties = new List<RestrictionEn>();

                return restrictionProperties;
            }
            set { restrictionProperties = value; }
        }

        private CertificateFile certFile;
        [XmlAttribute]
        public CertificateFile CertFile
        {
            get
            {
                if (certFile == null)
                    certFile = new CertificateFile();
                return certFile;
            }
            set { certFile = value; }
        }


        public class CertificateFile
        {
            private string archived;
            [XmlAttribute]
            public string Archived
            {
                get { return archived; }
                set { archived = value; }
            }

            private string extensions;
            [XmlAttribute]
            public string Extensions
            {
                get { return extensions; }
                set { extensions = value; }
            }

            private string friendlyName;
            [XmlAttribute]
            public string FriendlyName
            {
                get { return friendlyName; }
                set { friendlyName = value; }
            }

            private DateTime effectiveDate;
            [XmlAttribute]
            public DateTime EffectiveDate
            {
                get { return effectiveDate; }
                set { effectiveDate = value; }
            }

            private DateTime expirationDate;
            [XmlAttribute]
            public DateTime ExpirationDate
            {
                get { return expirationDate; }
                set { expirationDate = value; }
            }

            private string format;
            [XmlAttribute]
            public string Format
            {
                get { return format; }
                set { format = value; }
            }

            private string keyAlgorithm;
            [XmlAttribute]
            public string KeyAlgorithm
            {
                get { return keyAlgorithm; }
                set { keyAlgorithm = value; }
            }

            private string keyAlgorithmParametersString;
            [XmlAttribute]
            public string KeyAlgorithmParametersString
            {
                get { return keyAlgorithmParametersString; }
                set { keyAlgorithmParametersString = value; }
            }

            private string nameInfo;
            [XmlAttribute]
            public string NameInfo
            {
                get { return nameInfo; }
                set { nameInfo = value; }
            }

            private string serialNumberString;
            [XmlAttribute]
            public string SerialNumberString
            {
                get { return serialNumberString; }
                set { serialNumberString = value; }
            }

            private string type;
            [XmlAttribute]
            public string Type
            {
                get { return type; }
                set { type = value; }
            }

            private string issuer;
            [XmlAttribute]
            public string Issuer
            {
                get { return issuer; }
                set { issuer = value; }
            }

            private string issuerName;
            [XmlAttribute]
            public string IssuerName
            {
                get { return issuerName; }
                set { issuerName = value; }
            }

            private string serialNumber;
            [XmlAttribute]
            public string SerialNumber
            {
                get { return serialNumber; }
                set { serialNumber = value; }
            }

            private string signatureAlgorithm;
            [XmlAttribute]
            public string SignatureAlgorithm
            {
                get { return signatureAlgorithm; }
                set { signatureAlgorithm = value; }
            }

            private string subject;
            [XmlAttribute]
            public string Subject
            {
                get { return subject; }
                set { subject = value; }
            }

            private string subjectName;
            [XmlAttribute]
            public string SubjectName
            {
                get { return subjectName; }
                set { subjectName = value; }
            }

            private int version;
            [XmlAttribute]
            public int Version
            {
                get { return version; }
                set { version = value; }
            }
        }
    }
}
