using System.Xml.Serialization;

namespace MDM.iOS.Entities.MDM
{
    /// <summary>
    /// This class shows the list of restrictions that are present
    /// in the iPad. 
    /// </summary>

    [Serializable]
    public class En_MDM_Restrictions
    {

        private Guid ipadProfile_ID;
        [XmlAttribute]
        public Guid IpadProfile_ID
        {
            get { return ipadProfile_ID; }
            set { ipadProfile_ID = value; }
        }

        private Int16 safariAcceptCookies;
        [XmlAttribute]
        public Int16 SafariAcceptCookies
        {
            get { return safariAcceptCookies; }
            set { safariAcceptCookies = value; }
        }

        private Boolean allowAppInstallation;
        [XmlAttribute]
        public Boolean AllowAppInstallation
        {
            get { return allowAppInstallation; }
            set { allowAppInstallation = value; }
        }

        private Boolean allowCamera;
        [XmlAttribute]
        public Boolean AllowCamera
        {
            get { return allowCamera; }
            set { allowCamera = value; }
        }

        private Boolean allowVideoConferencing;
        [XmlAttribute]
        public Boolean AllowVideoConferencing
        {
            get { return allowVideoConferencing; }
            set { allowVideoConferencing = value; }
        }

        private Boolean allowScreenShot;
        [XmlAttribute]
        public Boolean AllowScreenShot
        {
            get { return allowScreenShot; }
            set { allowScreenShot = value; }
        }

        private Boolean allowGlobalBackgroundFetchWhenRoaming;
        [XmlAttribute]
        public Boolean AllowGlobalBackgroundFetchWhenRoaming
        {
            get { return allowGlobalBackgroundFetchWhenRoaming; }
            set { allowGlobalBackgroundFetchWhenRoaming = value; }
        }

        private Boolean allowAssistant;
        [XmlAttribute]
        public Boolean AllowAssistant
        {
            get { return allowAssistant; }
            set { allowAssistant = value; }
        }

        private Boolean allowAssistantWhileLocked;
        [XmlAttribute]
        public Boolean AllowAssistantWhileLocked
        {
            get { return allowAssistantWhileLocked; }
            set { allowAssistantWhileLocked = value; }
        }

        private Boolean allowVoiceDialing;
        [XmlAttribute]
        public Boolean AllowVoiceDialing
        {
            get { return allowVoiceDialing; }
            set { allowVoiceDialing = value; }
        }

        private Boolean allowPassbookWhileLocked;
        [XmlAttribute]
        public Boolean AllowPassbookWhileLocked
        {
            get { return allowPassbookWhileLocked; }
            set { allowPassbookWhileLocked = value; }
        }

        private Boolean allowInAppPurchases;
        [XmlAttribute]
        public Boolean AllowInAppPurchases
        {
            get { return allowInAppPurchases; }
            set { allowInAppPurchases = value; }
        }

        private Boolean allowMultiplayerGaming;
        [XmlAttribute]
        public Boolean AllowMultiplayerGaming
        {
            get { return allowMultiplayerGaming; }
            set { allowMultiplayerGaming = value; }
        }

        private Boolean allowAddingGameCenterFriends;
        [XmlAttribute]
        public Boolean AllowAddingGameCenterFriends
        {
            get { return allowAddingGameCenterFriends; }
            set { allowAddingGameCenterFriends = value; }
        }

        private Boolean allowYouTube;
        [XmlAttribute]
        public Boolean AllowYouTube
        {
            get { return allowYouTube; }
            set { allowYouTube = value; }
        }

        private Boolean allowiTunes;
        [XmlAttribute]
        public Boolean AllowiTunes
        {
            get { return allowiTunes; }
            set { allowiTunes = value; }
        }

        private Boolean allowSafari;
        [XmlAttribute]
        public Boolean AllowSafari
        {
            get { return allowSafari; }
            set { allowSafari = value; }
        }

        private Boolean safariAllowAutoFill;
        [XmlAttribute]
        public Boolean SafariAllowAutoFill
        {
            get { return safariAllowAutoFill; }
            set { safariAllowAutoFill = value; }
        }

        private Boolean safariForceFraudWarning;
        [XmlAttribute]
        public Boolean SafariForceFraudWarning
        {
            get { return safariForceFraudWarning; }
            set { safariForceFraudWarning = value; }
        }

        private Boolean safariAllowJavaScript;
        [XmlAttribute]
        public Boolean SafariAllowJavaScript
        {
            get { return safariAllowJavaScript; }
            set { safariAllowJavaScript = value; }
        }


        private Boolean safariAllowPopups;
        [XmlAttribute]
        public Boolean SafariAllowPopups
        {
            get { return safariAllowPopups; }
            set { safariAllowPopups = value; }
        }


        private Boolean allowCloudBackup;
        [XmlAttribute]
        public Boolean AllowCloudBackup
        {
            get { return allowCloudBackup; }
            set { allowCloudBackup = value; }
        }

        private Boolean allowCloudDocumentSync;
        [XmlAttribute]
        public Boolean AllowCloudDocumentSync
        {
            get { return allowCloudDocumentSync; }
            set { allowCloudDocumentSync = value; }
        }

        private Boolean allowPhotoStream;
        [XmlAttribute]
        public Boolean AllowPhotoStream
        {
            get { return allowPhotoStream; }
            set { allowPhotoStream = value; }
        }

        private Boolean allowSharedStream;
        [XmlAttribute]
        public Boolean AllowSharedStream
        {
            get { return allowSharedStream; }
            set { allowSharedStream = value; }
        }

        private Boolean allowDiagnosticSubmission;
        [XmlAttribute]
        public Boolean AllowDiagnosticSubmission
        {
            get { return allowDiagnosticSubmission; }
            set { allowDiagnosticSubmission = value; }
        }

        private Boolean allowUntrustedTLSPrompt;
        [XmlAttribute]
        public Boolean AllowUntrustedTLSPrompt
        {
            get { return allowUntrustedTLSPrompt; }
            set { allowUntrustedTLSPrompt = value; }
        }

        private Boolean forceEncryptedBackup;
        [XmlAttribute]
        public Boolean ForceEncryptedBackup
        {
            get { return forceEncryptedBackup; }
            set { forceEncryptedBackup = value; }
        }

        private Boolean allowExplicitContent;
        [XmlAttribute]
        public Boolean AllowExplicitContent
        {
            get { return allowExplicitContent; }
            set { allowExplicitContent = value; }
        }

        private Boolean allowBookstoreErotica;
        [XmlAttribute]
        public Boolean AllowBookstoreErotica
        {
            get { return allowBookstoreErotica; }
            set { allowBookstoreErotica = value; }
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


    }


    /// <summary>
    /// A restriction object which consists of basic properties such
    /// as ID, name, whether it is checked, presumably to be configured
    /// in the UI dashboard. 
    /// </summary>

    public class RestrictionEn
    {
        private int rid;
        [XmlAttribute]
        public int RID
        {
            get { return rid; }
            set { rid = value; }
        }

        private string restrictionName;
        [XmlAttribute]
        public string RestrictionName
        {
            get { return restrictionName; }
            set { restrictionName = value; }
        }

        private bool isCheck;
        [XmlAttribute]
        public bool IsCheck
        {
            get { return isCheck; }
            set { isCheck = value; }
        }

        private int rGroup;
        [XmlAttribute]
        public int RGroup
        {
            get { return rGroup; }
            set { rGroup = value; }
        }

        private int groupHeader;
        [XmlAttribute]
        public int GroupHeader
        {
            get { return groupHeader; }
            set { groupHeader = value; }
        }
    }
}
