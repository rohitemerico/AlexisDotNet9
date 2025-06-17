using System.Xml.Serialization;

namespace MDM.iOS.Entities.MDM
{
    /// <summary>
    /// This class object is usually used during enrollment. 
    /// It contains all necessary information to successfully enroll 
    /// an iPad into the MDM server such as push magic, device token etc.
    /// </summary>

    [Serializable]
    public class En_Enrollment
    {
        private Guid mDMIDField;
        [XmlAnyAttribute]
        private string uDIDField;
        [XmlAnyAttribute]
        private string iMEIField;
        [XmlAnyAttribute]
        private string serialNoField;
        [XmlAnyAttribute]
        private string pushMagicField;
        [XmlAnyAttribute]
        private string topicField;
        [XmlAnyAttribute]
        private string tokenField;
        [XmlAnyAttribute]
        private string unlockTokenField;
        [XmlAnyAttribute]
        private int mdmStatusField;
        [XmlAnyAttribute]
        private string mdmPathField;
        [XmlAnyAttribute]
        private int mdmAllowEnrollStatusField;
        [XmlAnyAttribute]
        private string tempGroupIDField;
        [XmlAnyAttribute]
        private DateTime? createdDateTimeField;
        [XmlAnyAttribute]
        private DateTime? lastMdmCheckInDateTimeField;
        [XmlAnyAttribute]
        private DateTime? lastApprovedDateTimeField;
        [XmlAnyAttribute]
        private DateTime? lastModifiedDateTimeField;
        [XmlAnyAttribute]
        private DateTime? lastRejectDateTimeField;
        [XmlAnyAttribute]
        private Guid? lastApprovedUserField;
        [XmlAnyAttribute]
        private Guid? lastRejectedUserField;
        [XmlAnyAttribute]
        private Guid? branchIDField;
        [XmlAnyAttribute]


        /// <remarks/>
        public Guid MDMID
        {
            get
            {
                return mDMIDField;
            }
            set
            {
                mDMIDField = value;
            }
        }

        public Guid? BranchID
        {
            get
            {
                return branchIDField;
            }
            set
            {
                branchIDField = value;
            }
        }

        /// <remarks/>
        public string UDID
        {
            get
            {
                return uDIDField;
            }
            set
            {
                uDIDField = value;
            }
        }

        public string TempGID
        {
            get { return tempGroupIDField; }
            set { tempGroupIDField = value; }
        }

        /// <remarks/>
        public string IMEI
        {
            get
            {
                return iMEIField;
            }
            set
            {
                iMEIField = value;
            }
        }

        /// <remarks/>
        public string SerialNo
        {
            get
            {
                return serialNoField;
            }
            set
            {
                serialNoField = value;
            }
        }


        /// <remarks/>
        public string PushMagic
        {
            get
            {
                return pushMagicField;
            }
            set
            {
                pushMagicField = value;
            }
        }

        /// <remarks/>
        public string Topic
        {
            get
            {
                return topicField;
            }
            set
            {
                topicField = value;
            }
        }

        /// <remarks/>
        public string Token
        {
            get
            {
                return tokenField;
            }
            set
            {
                tokenField = value;
            }
        }

        /// <remarks/>
        public string UnlockToken
        {
            get
            {
                return unlockTokenField;
            }
            set
            {
                unlockTokenField = value;
            }
        }

        /// <remarks/>
        public int MdmStatus
        {
            get
            {
                return mdmStatusField;
            }
            set
            {
                mdmStatusField = value;
            }
        }

        /// <remarks/>
        public string MdmPath
        {
            get
            {
                return mdmPathField;
            }
            set
            {
                mdmPathField = value;
            }
        }

        /// <remarks/>
        public int MdmAllowEnrollStatus
        {
            get
            {
                return mdmAllowEnrollStatusField;
            }
            set
            {
                mdmAllowEnrollStatusField = value;
            }
        }

        /// <remarks/>
        public string TempGroupID
        {
            get
            {
                return tempGroupIDField;
            }
            set
            {
                tempGroupIDField = value;
            }
        }

        /// <remarks/>
        public DateTime? CreatedDateTime
        {
            get
            {
                return createdDateTimeField;
            }
            set
            {
                createdDateTimeField = value;
            }
        }

        /// <remarks/>
        public DateTime? LastMdmCheckInDateTime
        {
            get
            {
                return lastMdmCheckInDateTimeField;
            }
            set
            {
                lastMdmCheckInDateTimeField = value;
            }
        }

        /// <remarks/>
        public DateTime? LastApprovedDateTime
        {
            get
            {
                return lastApprovedDateTimeField;
            }
            set
            {
                lastApprovedDateTimeField = value;
            }
        }

        /// <remarks/>
        public DateTime? LastModifiedDateTime
        {
            get
            {
                return lastModifiedDateTimeField;
            }
            set
            {
                lastModifiedDateTimeField = value;
            }
        }

        /// <remarks/>
        public DateTime? LastRejectDateTime
        {
            get
            {
                return lastRejectDateTimeField;
            }
            set
            {
                lastRejectDateTimeField = value;
            }
        }

        /// <remarks/>
        public Guid? LastApprovedUser
        {
            get
            {
                return lastApprovedUserField;
            }
            set
            {
                lastApprovedUserField = value;
            }
        }

        /// <remarks/>
        public Guid? LastRejectedUser
        {
            get
            {
                return lastRejectedUserField;
            }
            set
            {
                lastRejectedUserField = value;
            }
        }
    }
}
