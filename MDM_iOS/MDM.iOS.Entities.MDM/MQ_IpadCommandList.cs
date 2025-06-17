using System.Xml.Serialization;

namespace MDM.iOS.Entities.MDM
{
    /// <summary>
    /// The entity class MQ_IpadCommandList is used to store information related 
    /// to the iPad device as well as the query command to be used to query the 
    /// iPad device from the MDM server. MQ means message queue. 
    /// </summary>

    [Serializable]
    public class MQ_IpadCommandList
    {
        /// <summary>
        /// The command name to be fired from the ipad. 
        /// </summary>
        public Enum_CommandName commandName;
        [XmlAttribute]
        private Enum_CommandName CommandName
        {
            get { return commandName; }
            set { commandName = value; }
        }

        /// <summary>
        /// The priority of that particular command. 
        /// </summary>
        public int Mprio;
        [XmlAttribute]
        private int mprio
        {
            get { return Mprio; }
            set { Mprio = value; }
        }

        /// <summary>
        /// The unique identifier which correctly identifiers the apple devices. 
        /// </summary>
        public string uDID;
        [XmlAttribute]
        private string UDID
        {
            get { return uDID; }
            set { uDID = value; }
        }

        public int retryCount;
        [XmlAttribute]
        private int RetryCount
        {
            get { return retryCount; }
            set { retryCount = value; }
        }

        public int queueCount;
        [XmlAttribute]
        private int QueueCount
        {
            get { return queueCount; }
            set { queueCount = value; }
        }


        public Guid apns_ID;
        [XmlAttribute]
        private Guid Apns_ID
        {
            get { return apns_ID; }
            set { apns_ID = value; }
        }

        public string app_identifier;
        [XmlAttribute]
        private string App_identifier
        {
            get { return app_identifier; }
            set { app_identifier = value; }
        }


        public string payloadIdentifier_AddOn;
        [XmlAttribute]
        private string PayloadIdentifier_AddOn
        {
            get { return payloadIdentifier_AddOn; }
            set { payloadIdentifier_AddOn = value; }
        }


        public string filePath;
        [XmlAttribute]
        private string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public Guid ipadProfile_ID;
        [XmlAttribute]
        private Guid IpadProfile_ID
        {
            get { return ipadProfile_ID; }
            set { ipadProfile_ID = value; }
        }

        public string process_ID;
        [XmlAttribute]
        private string Process_ID
        {
            get { return process_ID; }
            set { process_ID = value; }
        }


        public int delayMin;
        [XmlAttribute]
        private int DelayMin
        {
            get { return delayMin; }
            set { delayMin = value; }
        }

        public int firmwareStatus;
        [XmlAttribute]
        private int FirmwareStatus
        {
            get { return firmwareStatus; }
            set { firmwareStatus = value; }
        }


        public string securityType;
        [XmlAttribute]
        private string SecurityType
        {
            get { return securityType; }
            set { securityType = value; }
        }

    }
}
