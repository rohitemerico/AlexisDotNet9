using System.Xml.Serialization;

namespace MDM.iOS.Entities.Dashboard
{
    [Serializable]
    public class DbEditing
    {

        private Guid createdby;
        [XmlAttribute]
        public Guid Createdby
        {
            get { return createdby; }
            set { createdby = value; }
        }
        /***
        private string cname;
        [XmlAttribute]
        public string Cname
        {
            get { return cname; }
            set { cname = value; }
        }
        /***/

        private string data;
        [XmlAttribute]
        public string Data
        {
            get { return data; }
            set { data = value; }
        }


        private string entityData;
        [XmlAttribute]
        public string EntityData
        {
            get { return entityData; }
            set { entityData = value; }
        }

        private Guid clientid;
        [XmlAttribute]
        public Guid Clientid
        {
            get { return clientid; }
            set { clientid = value; }
        }
        /***
        private Guid countryid;
        [XmlAttribute]
        public Guid Countryid
        {
            get { return countryid; }
            set { countryid = value; }
        }

        private Guid stateid;
        [XmlAttribute]
        public Guid Stateid
        {
            get { return stateid; }
            set { stateid = value; }
        }

        private Guid cityid;
        [XmlAttribute]
        public Guid Cityid
        {
            get { return cityid; }
            set { cityid = value; }
        }

        private Guid districtid;
        [XmlAttribute]
        public Guid Districtid
        {
            get { return districtid; }
            set { districtid = value; }
        }

        private string clientname;
        [XmlAttribute]
        public string Clientname
        {
            get { return clientname; }
            set { clientname = value; }
        }

        private Guid clienttype;
        [XmlAttribute]
        public Guid Clienttype
        {
            get { return clienttype; }
            set { clienttype = value; }
        }

        private string client_address;
        [XmlAttribute]
        public string Client_address
        {
            get { return client_address; }
            set { client_address = value; }
        }

        private string person_in_charge;
        [XmlAttribute]
        public string Person_in_charge
        {
            get { return person_in_charge; }
            set { person_in_charge = value; }
        }

        private string contact_no;
        [XmlAttribute]
        public string Contact_no
        {
            get { return contact_no; }
            set { contact_no = value; }
        }
        private string email;
        [XmlAttribute]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        /***/

        private string moduleName;
        [XmlAttribute]
        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }
        private string editorName;
        [XmlAttribute]
        public string EditorName
        {
            get { return editorName; }
            set { editorName = value; }
        }
        private Guid tblid;
        [XmlAttribute]
        public Guid Tblid
        {
            get { return tblid; }
            set { tblid = value; }
        }

        private int tblStatus;
        [XmlAttribute]
        public int TblStatus
        {
            get { return tblStatus; }
            set { tblStatus = value; }
        }


        private DateTime editDate;
        [XmlAttribute]
        public DateTime EditDate
        {
            get { return editDate; }
            set { editDate = value; }
        }

        private Guid mid;
        [XmlAttribute]
        public Guid Mid
        {
            get { return mid; }
            set { mid = value; }
        }

        private Guid editedBy;
        [XmlAttribute]
        public Guid EditedBy
        {
            get { return editedBy; }
            set { editedBy = value; }
        }
        private Guid declineBy;
        [XmlAttribute]
        public Guid DeclineBy
        {
            get { return declineBy; }
            set { declineBy = value; }
        }
        private Guid approvedBy;
        [XmlAttribute]
        public Guid ApprovedBy
        {
            get { return approvedBy; }
            set { approvedBy = value; }
        }
        private DateTime declineDate;
        [XmlAttribute]
        public DateTime DeclineDate
        {
            get { return declineDate; }
            set { declineDate = value; }
        }
        private DateTime approvedDate;
        [XmlAttribute]
        public DateTime ApprovedDate
        {
            get { return approvedDate; }
            set { approvedDate = value; }
        }

        private List<Table> tables;
        /// <summary>
        /// create more tables if needed to update more than 1 table
        /// </summary>
        [XmlAttribute]
        public List<Table> Tables
        {
            get { return tables; }
            set { tables = value; }
        }
    }
    [Serializable]
    public class Table
    {
        private string mainTableName;
        [XmlAttribute]
        public string MainTableName
        {
            get { return mainTableName; }
            set { mainTableName = value; }
        }

        private string clientidN;
        [XmlAttribute]
        public string ClientN
        {
            get { return clientidN; }
            set { clientidN = value; }
        }


        private Guid clientid;
        [XmlAttribute]
        public Guid Clientid
        {
            get { return clientid; }
            set { clientid = value; }
        }

        private List<rows> rows;
        [XmlAttribute]
        public List<rows> Rows
        {
            get { return rows; }
            set { rows = value; }
        }



        private bool toDelete;
        [XmlAttribute]
        public bool ToDelete
        {
            get { return toDelete; }
            set { toDelete = value; }
        }

        private string deleteWithCondition;
        [XmlAttribute]
        public string DeleteWithCondition
        {
            get { return deleteWithCondition; }
            set { deleteWithCondition = value; }
        }

        private bool isTableWithStatus;
        [XmlAttribute]
        public bool IsTableWithStatus
        {
            get { return isTableWithStatus; }
            set { isTableWithStatus = value; }
        }


        private string tblStatusName;
        [XmlAttribute]
        public string TblStatusName
        {
            get { return tblStatusName; }
            set { tblStatusName = value; }
        }

        private string tblIdName;
        [XmlAttribute]
        public string TblIdName
        {
            get { return tblIdName; }
            set { tblIdName = value; }
        }

        private Guid tblId;
        [XmlAttribute]
        public Guid TblId
        {
            get { return tblId; }
            set { tblId = value; }
        }
    }
    [Serializable]
    public class rows
    {
        private Guid rowId;
        [XmlAttribute]
        public Guid RowId
        {
            get { return rowId; }
            set { rowId = value; }
        }

        private string rowColName;
        /// <summary>
        /// the name of column to pair with db row column ex tid
        /// </summary>
        [XmlAttribute]
        public string RowColName
        {
            get { return rowColName; }
            set { rowColName = value; }
        }


        private List<columns> columns;
        [XmlAttribute]
        public List<columns> Columns
        {
            get { return columns; }
            set { columns = value; }
        }
    }
    [Serializable]
    public class columns
    {
        private string columnName;
        /// <summary>
        /// the name of column that is linked to database column name. The value must tally with db column name
        /// </summary>
        [XmlAttribute]
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        private string columnType;
        /// <summary>
        /// the data type of column from database. eg NVARCHAR / BIT / DATETIME
        /// </summary>
        [XmlAttribute]
        public string ColumnType
        {
            get { return columnType; }
            set { columnType = value; }
        }

        public Guid ColID;
        [XmlAttribute]
        private Guid colID
        {

            get { return ColID; }
            set { ColID = value; }
        }

        private object newValue;
        /// <summary>
        /// the new value to be updated
        /// </summary>
        [XmlAttribute]
        public object NewValue
        {
            get { return newValue; }
            set { newValue = value; }
        }

        private object oldValue;
        /// <summary>
        /// the old value before updating
        /// </summary>
        [XmlAttribute]
        public object OldValue
        {
            get { return oldValue; }
            set { oldValue = value; }
        }

        private bool toDisplay;
        /// <summary>
        /// set to true to display as text on compare page
        /// </summary>
        [XmlAttribute]
        public bool ToDisplay
        {
            get { return toDisplay; }
            set { toDisplay = value; }
        }

        private string newDescriptionText;
        /// <summary>
        /// the text to be displayed when calling compare page. eg ROLE NAME : <descriptionValue>
        /// </summary>
        [XmlAttribute]
        public string NewDescriptionText
        {
            get { return newDescriptionText; }
            set { newDescriptionText = value; }
        }

        private string newDescriptionValue;
        /// <summary>
        /// the value of the description to be displayed. eg. ROLE NAME : ADMINISTRATOR
        /// </summary>
        [XmlAttribute]
        public string NewDescriptionValue
        {
            get { return newDescriptionValue; }
            set { newDescriptionValue = value; }
        }

        private string oldDescriptionText;
        [XmlAttribute]
        public string OldDescriptionText
        {
            get { return oldDescriptionText; }
            set { oldDescriptionText = value; }
        }

        private string oldDescriptionValue;
        [XmlAttribute]
        public string OldDescriptionValue
        {
            get { return oldDescriptionValue; }
            set { oldDescriptionValue = value; }
        }

    }
}
