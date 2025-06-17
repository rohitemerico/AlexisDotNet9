using System.Xml.Serialization;

namespace MDM.iOS.Entities.Dashboard
{
    /// <summary>
    /// Entity class for update and return response made by user. 
    /// </summary>

    [Serializable]
    public class UpdateEn
    {
        private string flag;
        [XmlAttribute]
        public string Flag
        {
            get { return flag; }
            set { flag = value; }
        }
        private Guid id;
        [XmlAttribute]
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        private List<updateTables> table;
        [XmlAttribute]
        public List<updateTables> Table
        {
            get { return table; }
            set { table = value; }
        }


        private string status;
        [XmlAttribute]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private Guid editPerson;
        [XmlAttribute]
        public Guid EditPerson
        {
            get { return editPerson; }
            set { editPerson = value; }
        }

        private Guid mid;
        /// <summary>
        /// module id of the editing
        /// </summary>
        [XmlAttribute]
        public Guid Mid
        {
            get { return mid; }
            set { mid = value; }
        }

        private DateTime editedDate;
        [XmlAttribute]
        public DateTime EditedDate
        {
            get { return editedDate; }
            set { editedDate = value; }
        }

        private string editorName;
        [XmlAttribute]
        public string EditorName
        {
            get { return editorName; }
            set { editorName = value; }
        }

        private string moduleName;
        [XmlAttribute]
        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }

        private Guid approvedEdit;
        [XmlAttribute]
        public Guid ApprovedEdit
        {
            get { return approvedEdit; }
            set { approvedEdit = value; }
        }

        private Guid declineEdit;
        [XmlAttribute]
        public Guid DeclineEdit
        {
            get { return declineEdit; }
            set { declineEdit = value; }
        }

        private DateTime approvedDate;
        [XmlAttribute]
        public DateTime ApprovedDate
        {
            get { return approvedDate; }
            set { approvedDate = value; }
        }

        private DateTime declineDate;
        [XmlAttribute]
        public DateTime DeclineDate
        {
            get { return declineDate; }
            set { declineDate = value; }
        }
    }

    [Serializable]
    public class updateTables
    {
        private string tablename;
        [XmlAttribute]
        public string Tablename
        {
            get { return tablename; }
            set { tablename = value; }
        }

        private Guid tableid;
        [XmlAttribute]
        public Guid Tableid
        {
            get { return tableid; }
            set { tableid = value; }
        }

        private string tableidName;
        [XmlAttribute]
        public string TableidName
        {
            get { return tableidName; }
            set { tableidName = value; }
        }

        private string tblStatusDesc;
        [XmlAttribute]
        public string TblStatusDesc
        {
            get { return tblStatusDesc; }
            set { tblStatusDesc = value; }
        }

        private List<compareOldValues> oldValues;
        [XmlAttribute]
        public List<compareOldValues> OldValues
        {
            get { return oldValues; }
            set { oldValues = value; }
        }

        private List<compareNewValues> newValues;
        [XmlAttribute]
        public List<compareNewValues> NewValues
        {
            get { return newValues; }
            set { newValues = value; }
        }

    }

    [Serializable]
    public class compareOldValues
    {
        private string columnName;
        /// <summary>
        /// the exact name of the column name of the main table
        /// </summary>
        [XmlAttribute]
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }
        private string value;
        /// <summary>
        /// the value
        /// </summary>
        [XmlAttribute]
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        private string columnType;
        /// <summary>
        /// column datatype
        /// </summary>
        [XmlAttribute]
        public string ColumnType
        {
            get { return columnType; }
            set { columnType = value; }
        }

        private string desc;
        /// <summary>
        /// display purpose, ex NAME :
        /// </summary>
        [XmlAttribute]
        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }

        private string descValue;
        /// <summary>
        /// the value to be displayed using desc : THE DATA TO DISPLAY
        /// </summary>
        [XmlAttribute]
        public string DescValue
        {
            get { return descValue; }
            set { descValue = value; }
        }
    }

    [Serializable]
    public class compareNewValues
    {
        private string columnName;
        [XmlAttribute]
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }
        private string value;
        [XmlAttribute]
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        private string columnType;
        [XmlAttribute]
        public string ColumnType
        {
            get { return columnType; }
            set { columnType = value; }
        }

        private string desc;
        [XmlAttribute]
        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }

        private string descValue;
        [XmlAttribute]
        public string DescValue
        {
            get { return descValue; }
            set { descValue = value; }
        }
    }
}
