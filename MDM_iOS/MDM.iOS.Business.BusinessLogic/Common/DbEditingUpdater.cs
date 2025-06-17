using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.iOS.Common.Data.Component;
using MDM.iOS.Entities.Dashboard;
using Newtonsoft.Json;

namespace MDM.iOS.Business.BusinessLogic.Common
{
    public class DbEditingUpdater
    {


        /// <summary>
        /// to retrieve the table column names and the datatype accordingly
        /// returns column_name and data_type
        /// </summary>
        /// <param name="tablename">the table to retrieve</param>
        /// <param name="idName">the column name to exclude, leave null if want all columns, to have more columns to exclude, put string eg 'tblid','custid','custdate'</param>
        /// <returns></returns>
        public static DataTable getColumnAndType(string tablename, string idName)
        {
            DataTable ret = new DataTable();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select column_name,data_type from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and column_name not in(" + idName + ")");
            List<Params> myp = new List<Params>();
            myp.Add(new Params("@tablename", "NVARCHAR", tablename));
            ret = SqlDataControl.GetResult(sql.ToString(), myp);

            for (int i = 0; i < ret.Rows.Count; i++)
            {
                ret.Rows[i]["data_type"] = getDatatype(ret.Rows[i]["data_type"].ToString());
            }
            return ret;
        }

        private static string getDatatype(string dbT)
        {
            string ret = string.Empty;
            switch (dbT)
            {
                case "uniqueidentifier":
                    ret = "GUID";
                    break;
                default:
                    ret = dbT.ToUpper();
                    break;
            }
            return ret;
        }

        public static DataTable retrieveData(Guid rowid)
        {
            DataTable ret = new DataTable();
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT *, um.mDesc,ul.uName FROM DATA_TRAIL TE, user_menu um, user_login ul WHERE TE.TBLID = @ROWID AND TE.TBLSTATUS = 3 and te.mid = um.mid and te.editedby = ul.uid");
                List<Params> myParams = new List<Params>();
                myParams.Add(new Params("@ROWID", "GUID", rowid));
                ret = SqlDataControl.GetResult(sql.ToString(), myParams);
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                              System.Reflection.MethodBase.GetCurrentMethod().Name,
                              ex);
            }
            return ret;
        }


        public static bool insertEditTable(UpdateEn UpdateData)
        {
            bool ret = false;
            try
            {

                StringBuilder sql = new StringBuilder();

                sql.AppendLine("INSERT INTO DATA_TRAIL");
                sql.AppendLine("(TBLID,TBLSTATUS,ENTITYDATA,EDITEDDATE,MID,EDITEDBY)");
                sql.AppendLine("VALUES");
                sql.AppendLine("(@TBLID,@TBLSTATUS,@ENTITYDATA,@CREATEDDATE,@MID,@EDITEDBY)");

                List<Params> MyParams = new List<Params>();
                MyParams.Add(new Params("@TBLID", "GUID", UpdateData.Id));
                MyParams.Add(new Params("@TBLSTATUS", "INT", UpdateData.Status));
                MyParams.Add(new Params("@ENTITYDATA", "NVARCHAR", JsonConvert.SerializeObject(UpdateData)));
                MyParams.Add(new Params("@CREATEDDATE", "DATETIME", UpdateData.EditedDate));
                MyParams.Add(new Params("@MID", "GUID", UpdateData.Mid));
                MyParams.Add(new Params("@EDITEDBY", "GUID", UpdateData.EditPerson));
                ret = SqlDataControl.Input(sql.ToString(), MyParams);
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                              System.Reflection.MethodBase.GetCurrentMethod().Name,
                              ex);
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newValues">rid:b01386c9-c597-4650-bde7-395fc05be377#SPLIT#Role : Administrotor#NXTL#tid:b01386c9-c597-4650-bde7-395fc05be377#SPLIT#Name : Peekaboo</param>
        /// <param name="oldValues">rid:2a0d2c4d-1600-4ef3-8cce-7bdfcbadd546:GUID:USTATUS:UID#SPLIT#Role : Test2#NXTL#tid:2a0d2c4d-1600-4ef3-8cce-7bdfcbadd546:GUID:USTATUS:UID#SPLIT#RoleName : Testing</param>
        /// <param name="tableName">Table Name of the master table</param>
        /// <param name="moduleid">the module id of the edited data</param>
        /// <param name="rowId">the rowid that ties the master and the edited table</param>
        /// <param name="actionPerson">Person that made the changes</param>
        /// <returns></returns>
        public static bool insertEditTable(StringBuilder newValues, StringBuilder oldValues, string tableName, Guid moduleid, Guid rowId, Guid actionPerson)
        {
            bool ret = false;
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("INSERT INTO DATA_TRAIL");
                sql.AppendLine("(TBLID,TBLNAME,TBLSTATUS,OLDVALUES,NEWVALUES,EDITEDDATE,MID,EDITEDBY)");
                sql.AppendLine("VALUES");
                sql.AppendLine("(@TBLID,@TBLNAME,@TBLSTATUS,@OLDVALUES,@NEWVALUES,@CREATEDDATE,@MID,@EDITEDBY)");
                List<Params> MyParams = new List<Params>();
                MyParams.Add(new Params("@TBLID", "GUID", rowId));
                MyParams.Add(new Params("@TBLNAME", "NVARCHAR", tableName));
                MyParams.Add(new Params("@TBLSTATUS", "INT", 3));
                MyParams.Add(new Params("@OLDVALUES", "NVARCHAR", oldValues.ToString()));
                MyParams.Add(new Params("@NEWVALUES", "NVARCHAR", newValues.ToString()));
                MyParams.Add(new Params("@CREATEDDATE", "DATETIME", DateTime.Now));
                MyParams.Add(new Params("@MID", "GUID", moduleid));
                MyParams.Add(new Params("@EDITEDBY", "GUID", actionPerson));
                ret = SqlDataControl.Input(sql.ToString(), MyParams);
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                              System.Reflection.MethodBase.GetCurrentMethod().Name,
                              ex);
            }
            return ret;
        }


        public static DataTable getClientEditedData(Guid rowid)
        {
            DataTable ret = new DataTable();
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT *, cm.mDesc,cl.cName FROM CLIENT_TBLEDITED TE, client_menu cm, client_login cl WHERE TE.TBLID = @ROWID AND TE.TBLSTATUS = 3 and te.mid = cm.mid and te.editedby = cl.cid");
                List<Params> myParams = new List<Params>();
                myParams.Add(new Params("@ROWID", "GUID", rowid));
                ret = SqlDataControl.GetResult(sql.ToString(), myParams);
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                              System.Reflection.MethodBase.GetCurrentMethod().Name,
                              ex);
            }
            return ret;
        }



        public static bool UpdateSync(DbEditing myitems, int index)
        {
            bool ret = false;
            try
            {
                string list = string.Empty;
                string blist = string.Empty;

                string newlist = string.Empty;
                string oldlist = string.Empty;

                string[] newvalue = myitems.Tables[index].Rows[0].Columns[2].NewValue.ToString().ToUpper().Split('#');

                string[] oldvalue = myitems.Tables[index].Rows[0].Columns[2].OldValue.ToString().ToUpper().Split('#');
                //if oldvalue more than new, get the old id only
                if (newvalue.Length < oldvalue.Length)
                {
                    IEnumerable<string> oldOnlyID = oldvalue.Except(newvalue);

                    //Numbers in Old array but not New array.
                    foreach (var n in oldOnlyID)
                    {
                        list += n + '#';
                    }

                }
                else if (oldvalue.Length < newvalue.Length)
                {

                    //if the new is more than old, get the newvalue only
                    IEnumerable<string> newOnlyID = newvalue.Except(oldvalue);

                    foreach (var n in newOnlyID)
                    {
                        list += n + '#';
                    }

                }

                else if (oldvalue.Length == newvalue.Length)
                {
                    //getnewvalue
                    IEnumerable<string> newOnlyID = newvalue.Except(oldvalue);

                    foreach (var n in newOnlyID)
                    {//new added
                        newlist += n + '#';
                    }

                    IEnumerable<string> oldOnlyID = oldvalue.Except(newvalue);

                    foreach (var o in oldOnlyID)
                    {//delete
                        oldlist += o + '#';
                    }

                    string[] splitnew = newlist.Split('#');

                    for (int split_n = 0; split_n < splitnew.Length; split_n++)
                    {
                        if (splitnew[split_n].ToString() == string.Empty)
                        {
                            continue;
                        }

                        DataTable dt = CheckExist(Guid.Parse(splitnew[split_n].ToString()));
                        Guid client = Guid.Parse(myitems.Tblid.ToString().ToUpper());
                        if (dt.Rows.Count > 0)
                        {

                            string rel = dt.Rows[0]["relationship"].ToString();

                            string list_new = string.Empty;
                            if (rel == string.Empty)
                            {
                                list_new += client.ToString().ToUpper() + '#';

                            }
                            else
                            {
                                list_new += rel + client + '#';
                            }

                            if (splitnew[split_n].ToString() != string.Empty)
                            {
                                if (updateFundTransferSync(list_new, Guid.Parse(splitnew[split_n])))
                                {
                                    ret = true;
                                    list_new = null;
                                }
                            }

                        }

                        else
                        {
                            string bank = string.Empty;
                            Guid clientid = Guid.Parse(splitnew[split_n].ToString().ToUpper());
                            DataTable data = null;// ClientBankListDataProcess.GetClientName(clientid);
                            string cname = data.Rows[0]["clientname"].ToString();
                            bank += client.ToString().ToUpper() + "#";

                            //if (ClientBankListDataProcess.Insert_Edit(clientid, cname, bank))
                            {
                                ret = true;

                            }
                        }

                        string[] splitold = oldlist.Split('#');

                        for (int split_o = 0; split_o < splitold.Length; split_o++)
                        {
                            if (splitold[split_o].ToString() == string.Empty)
                            {
                                continue;
                            }
                            //deleted
                            DataTable data = CheckExist(Guid.Parse(splitold[split_o].ToString()));

                            if (data.Rows.Count > 0)
                            {
                                string[] banklist = data.Rows[0]["relationship"].ToString().ToUpper().Split('#');

                                for (int bl = 0; bl < banklist.Length; bl++)
                                {
                                    if (banklist[bl].ToString() != string.Empty)
                                    {
                                        if (Guid.Parse(banklist[bl].ToUpper()) != Guid.Parse(client.ToString().ToUpper()))
                                        {

                                            blist += banklist[bl].ToString().ToUpper() + '#';
                                        }
                                    }

                                }

                                if (updateFundTransferSync(blist, Guid.Parse(splitold[split_o])))
                                {

                                    ret = true;
                                    blist = null;

                                }

                            }
                        }
                    }

                }

                if (newvalue.Length < oldvalue.Length || oldvalue.Length < newvalue.Length)
                {
                    string u_value = string.Empty;

                    string[] splitList = list.Split('#');

                    for (int l = 0; l < splitList.Length; l++)
                    {
                        if (splitList[l].ToString() == string.Empty)
                        {
                            continue;
                        }
                        DataTable dt = CheckExist(Guid.Parse(splitList[l].ToString()));
                        Guid client = Guid.Parse(myitems.Tblid.ToString().ToUpper());
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["relationship"].ToString() != string.Empty)
                            {
                                string[] banklist = dt.Rows[0]["relationship"].ToString().ToUpper().Split('#');

                                if (newvalue.Length < oldvalue.Length)
                                {
                                    for (int bl = 0; bl < banklist.Length; bl++)
                                    {

                                        if (banklist[bl].ToString() != string.Empty)
                                        {

                                            if (Guid.Parse(banklist[bl].ToString().ToUpper()) != Guid.Parse(client.ToString().ToUpper()))
                                            {

                                                blist += banklist[bl].ToString() + '#';
                                            }
                                        }
                                    }
                                }

                                else if (oldvalue.Length < newvalue.Length)
                                {

                                    for (int bl = 0; bl < banklist.Length; bl++)
                                    {

                                        if (banklist[bl].ToString() != string.Empty)
                                        {

                                            blist += banklist[bl].ToString() + '#';

                                        }


                                    }
                                    blist += client.ToString().ToUpper() + "#";
                                }

                            }

                            else
                            {

                                blist += client.ToString().ToUpper() + "#";
                            }





                            if (splitList[l].ToString() != string.Empty)
                            {
                                if (updateFundTransferSync(blist, Guid.Parse(splitList[l])))
                                {


                                    ret = true;
                                    blist = null;

                                }

                            }




                        }
                        else
                        {
                            string bank = string.Empty;
                            Guid clientid = Guid.Parse(splitList[l].ToString());
                            DataTable data = null;// ClientBankListDataProcess.GetClientName(clientid);
                            string cname = data.Rows[0]["clientname"].ToString();
                            bank += client + "#";

                            //if (ClientBankListDataProcess.Insert_Edit(clientid, cname, bank))
                            {
                                ret = true;

                            }

                        }

                    }
                }


                // if the bank dont have agreement ,  add insert first , 
                if (updateFundTransferSync(myitems.Tables[index].Rows[0].Columns[2].NewValue.ToString(), myitems.Tblid))
                {
                    ret = true;
                }

            }

            catch (Exception ex)
            {

                Console.Write(ex);

            }



            return ret;
        }

        private static bool updateFundTransferSync(string list, Guid clientid)
        {

            bool ret = false;
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            try
            {

                DataTable dt = CheckExist(clientid);
                if (list == null)
                {
                    list = string.Empty;

                }

                if (dt.Rows.Count > 0)
                {

                    sql.AppendLine(@"update Client_Fundtrans set relationship = @rel where cid = @clientid");

                    myParams.Add(new Params("@rel", "NVARCHAR", list));
                    myParams.Add(new Params("@clientid", "GUID", clientid));
                    ret = SqlDataControl.Input(sql.ToString(), myParams);
                }

                else
                {
                    //DataTable dt_cname = null;// ClientBankListDataProcess.GetClientName(clientid);

                    //if (ClientBankListDataProcess.Insert_Edit(clientid, dt_cname.Rows[0]["clientname"].ToString(), list))
                    {

                        ret = true;

                    }

                }
            }
            catch (Exception ex)
            {


            }

            finally
            {
                sql.Clear();
                myParams.Clear();
            }
            return ret;





        }

        private static DataTable CheckExist(Guid clientid)
        {
            DataTable ret = new DataTable();
            try
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"select * from client_fundtrans where cid = @cid");
                List<Params> myParams = new List<Params>();
                myParams.Add(new Params("@cid", "GUID", clientid));
                ret = SqlDataControl.GetResult(sql.ToString(), myParams);

            }

            catch (Exception ex)
            {
            }


            return ret;

        }


        public static bool ApproveEdit(UpdateEn MyItems)
        {
            bool ret = false;
            try
            {
                StringBuilder sql = new StringBuilder();
                List<Params> MyParams = new List<Params>();
                for (int t = 0; t < MyItems.Table.Count; t++)
                {

                    sql.AppendLine("update " + MyItems.Table[t].Tablename + " set");
                    for (int c = 0; c < MyItems.Table[t].NewValues.Count; c++)
                    {
                        sql.AppendLine(MyItems.Table[t].NewValues[c].ColumnName + " = @" + MyItems.Table[t].NewValues[c].ColumnName + ",");
                        MyParams.Add(new Params("@" + MyItems.Table[t].NewValues[c].ColumnName, MyItems.Table[t].NewValues[c].ColumnType, MyItems.Table[t].NewValues[c].Value));
                    }
                    sql.AppendLine(MyItems.Table[t].TblStatusDesc + " = 1");
                    sql.AppendLine("where " + MyItems.Table[t].TableidName + " = @" + MyItems.Table[t].TableidName);
                    MyParams.Add(new Params("@" + MyItems.Table[t].TableidName, "GUID", MyItems.Table[t].Tableid));
                    SqlDataControl.Input(sql.ToString(), MyParams);
                    sql.Clear();
                    MyParams.Clear();
                    ret = true;
                }

                sql.AppendLine("UPDATE DATA_TRAIL SET TBLSTATUS = 1,");
                sql.AppendLine("APPROVEDBY = @BY,");
                sql.AppendLine("APPROVEDDATE = @ADATE");
                sql.AppendLine("WHERE TBLID=@TBLID");
                sql.AppendLine("AND TBLSTATUS = 3");
                MyParams.Add(new Params("@BY", "GUID", MyItems.ApprovedEdit));
                MyParams.Add(new Params("@ADATE", "DATETIME", MyItems.ApprovedDate));
                MyParams.Add(new Params("@TBLID", "GUID", MyItems.Id));
                SqlDataControl.Input(sql.ToString(), MyParams);
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                              System.Reflection.MethodBase.GetCurrentMethod().Name,
                              ex);
            }
            return ret;
        }



        public static bool insertEditTable(DbEditing myData)
        {
            bool ret = false;
            try
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("INSERT INTO DATA_TRAIL");
                sql.AppendLine("(TBLID,TBLSTATUS,ENTITYDATA,EDITEDDATE,MID,EDITEDBY)");
                sql.AppendLine("VALUES");
                sql.AppendLine("(@TBLID,@TBLSTATUS,@ENTITYDATA,@CREATEDDATE,@MID,@EDITEDBY)");

                List<Params> MyParams = new List<Params>();
                MyParams.Add(new Params("@TBLID", "GUID", myData.Tblid));
                MyParams.Add(new Params("@TBLSTATUS", "INT", myData.TblStatus));
                MyParams.Add(new Params("@ENTITYDATA", "NVARCHAR", JsonConvert.SerializeObject(myData)));
                MyParams.Add(new Params("@CREATEDDATE", "DATETIME", myData.EditDate));
                MyParams.Add(new Params("@MID", "GUID", myData.Mid));
                MyParams.Add(new Params("@EDITEDBY", "GUID", myData.EditedBy));
                ret = SqlDataControl.Input(sql.ToString(), MyParams);
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                              System.Reflection.MethodBase.GetCurrentMethod().Name,
                              ex);
            }
            return ret;
        }

        public static DataTable getEditedData(Guid rowid)
        {
            DataTable ret = new DataTable();
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT *, um.mDesc,ul.uName FROM DATA_TRAIL TE, menu um, user_login ul WHERE TE.TBLID = @ROWID AND (TE.TBLSTATUS = 2 or TE.TBLSTATUS = 3) and te.mid = um.mid and te.editedby = ul.uid");
                List<Params> myParams = new List<Params>();
                myParams.Add(new Params("@ROWID", "GUID", rowid));
                ret = SqlDataControl.GetResult(sql.ToString(), myParams);
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                              System.Reflection.MethodBase.GetCurrentMethod().Name,
                              ex);
            }
            return ret;
        }

        /// <summary>
        /// to update upon approving the data,
        /// remember data should be 
        /// how many Tables to update or insert
        /// in the table how many rows to insert or update
        /// in the rows how many columns available and include column name and value
        /// </summary>
        /// <param name="myData"></param>
        /// <returns></returns>
        public static bool updateRecords(DbEditing myData)
        {
            bool ret = false;
            try
            {
                StringBuilder sql = new StringBuilder();
                List<Params> myParams = new List<Params>();
                for (int t = 0; t < myData.Tables.Count; t++)
                {
                    if (myData.Tables[t].ToDelete)
                    {
                        ///means need to delete something then insert, not updating anymore
                        if (!string.IsNullOrEmpty(myData.Tables[t].DeleteWithCondition))
                        {
                            myData.Tables[t].DeleteWithCondition += myData.Tables[t].Rows[0].RowColName + " = @" + myData.Tables[t].Rows[0].RowColName;
                            sql.AppendLine(myData.Tables[t].DeleteWithCondition);
                            myParams.Add(new Params("@" + myData.Tables[t].Rows[0].RowColName, "GUID", myData.Tables[t].Rows[0].RowId));
                        }
                        else
                        {
                            sql.AppendLine("delete " + myData.Tables[t].MainTableName + " where " +
                                myData.Tables[t].Rows[0].RowColName + " = @" + myData.Tables[t].Rows[0].RowColName);
                            myParams.Add(new Params("@" + myData.Tables[t].Rows[0].RowColName, "GUID", myData.Tables[t].Rows[0].RowId));
                        }
                        if (SqlDataControl.Input(sql.ToString(), myParams))
                        {
                            insertMultipleRow(myData, t);
                        }
                        sql.Clear();
                        myParams.Clear();
                        ret = true;
                    }
                    else
                    {
                        for (int r = 0; r < myData.Tables[t].Rows.Count; r++)
                        {
                            sql.AppendLine("update " + myData.Tables[t].MainTableName + " set");
                            for (int c = 0; c < myData.Tables[t].Rows[r].Columns.Count; c++)
                            {
                                if (myData.Tables[t].Rows[r].Columns[c].ColumnName == "[DESC]")
                                {
                                    sql.AppendLine("[DESC] = @DESC,");
                                    myParams.Add(new Params("@DESC", myData.Tables[t].Rows[r].Columns[c].ColumnType, myData.Tables[t].Rows[r].Columns[c].NewValue));
                                }

                                else
                                {
                                    sql.AppendLine(myData.Tables[t].Rows[r].Columns[c].ColumnName + " = @" + myData.Tables[t].Rows[r].Columns[c].ColumnName + ",");
                                    myParams.Add(new Params("@" + myData.Tables[t].Rows[r].Columns[c].ColumnName, myData.Tables[t].Rows[r].Columns[c].ColumnType, myData.Tables[t].Rows[r].Columns[c].NewValue));
                                }
                            }
                            string a = sql.ToString().Trim();
                            a = a.Remove(a.Length - 1, 1);
                            sql.Clear();
                            sql.AppendLine(a);
                            sql.AppendLine(" where " + myData.Tables[t].Rows[r].RowColName + " =@" + myData.Tables[t].Rows[r].RowColName);

                            myParams.Add(new Params("@" + myData.Tables[t].Rows[r].RowColName, "GUID", myData.Tables[t].Rows[r].RowId));
                            SqlDataControl.Input(sql.ToString(), myParams);
                            sql.Clear();
                            myParams.Clear();
                        }
                        ret = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                              System.Reflection.MethodBase.GetCurrentMethod().Name,
                              ex);
            }
            return ret;
        }
        public static bool UpdateAdditionalData(DbEditing myData, string status)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            try
            {
                if (status == "APP")
                {
                    sql.AppendLine(@"UPDATE USER_LOGIN SET UAPPROVEDDATE = @TIME, UAPPROVEDBY = @AP, UUPDATEDDATE = @UD ");
                    myParams.Add(new Params("@AP", "GUID", myData.ApprovedBy));

                }

                else if (status == "DEC")
                {
                    sql.AppendLine(@"UPDATE USER_LOGIN SET UDECLINEDBY = @AP, UDECLINEDDATE = @TIME, UUPDATEDDATE = @UD ");
                    myParams.Add(new Params("@AP", "GUID", myData.DeclineBy));
                }

                sql.AppendLine(@"WHERE UID = '" + myData.Tblid + "'");

                myParams.Add(new Params("@TIME", "DATETIME", DateTime.Now));
                myParams.Add(new Params("@UD", "DATETIME", DateTime.Now));

                ret = SqlDataControl.Input(sql.ToString(), myParams);
            }

            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                              System.Reflection.MethodBase.GetCurrentMethod().Name,
                              ex);
            }

            finally
            {

                sql.Clear();
                myParams.Clear();

            }
            return ret;




        }

        private static void insertMultipleRow(DbEditing myData, int t)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                List<Params> myParams = new List<Params>();
                for (int r = 0; r < myData.Tables[t].Rows.Count; r++)
                {

                    sql.Clear();
                    myParams.Clear();
                    sql.AppendLine("insert into " + myData.Tables[t].MainTableName + " (");
                    string colName = string.Empty;
                    string colValue = "values (";
                    for (int c = 0; c < myData.Tables[t].Rows[r].Columns.Count; c++)
                    {
                        colName += myData.Tables[t].Rows[r].Columns[c].ColumnName + ",";
                        colValue += "@" + myData.Tables[t].Rows[r].Columns[c].ColumnName + ",";
                        myParams.Add(new Params("@" + myData.Tables[t].Rows[r].Columns[c].ColumnName,
                            myData.Tables[t].Rows[r].Columns[c].ColumnType,
                            myData.Tables[t].Rows[r].Columns[c].NewValue));
                    }
                    colName = colName.Remove(colName.Length - 1, 1);
                    colName += ")";

                    colValue = colValue.Remove(colValue.Length - 1, 1);
                    colValue += ")";
                    sql.AppendLine(colName);
                    sql.AppendLine(colValue);
                    Console.WriteLine(sql.ToString());
                    SqlDataControl.Input(sql.ToString(), myParams);

                    //exe 
                }
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                              System.Reflection.MethodBase.GetCurrentMethod().Name,
                              ex);
            }
        }



        public static bool UpdateEditTable(Guid rowid, int status, Guid ActionPerson)
        {
            bool ret = false;
            try
            {
                StringBuilder sql = new StringBuilder();
                List<Params> MyParams = new List<Params>();
                sql.AppendLine("UPDATE DATA_TRAIL SET TBLSTATUS = @STAT,");
                MyParams.Add(new Params("@STAT", "INT", status));
                if (status == 1)
                {
                    sql.AppendLine("APPROVEDDATE = @APPROVEDDATE, APPROVEDBY = @APPROVEDBY");
                    MyParams.Add(new Params("@APPROVEDDATE", "DATETIME", DateTime.Now));
                    MyParams.Add(new Params("@APPROVEDBY", "GUID", ActionPerson));
                }
                else if (status == 2)
                {
                    sql.AppendLine("DECLINEDDATE = @DECLINEDDATE, DECLINEDBY = @DECLINEDBY");
                    MyParams.Add(new Params("@DECLINEDDATE", "DATETIME", DateTime.Now));
                    MyParams.Add(new Params("@DECLINEDBY", "GUID", ActionPerson));
                }
                sql.AppendLine("WHERE TBLID = @TBLID AND TBLSTATUS NOT IN (0,1)");
                MyParams.Add(new Params("@TBLID", "GUID", rowid));
                ret = SqlDataControl.Input(sql.ToString(), MyParams);
                sql.Clear();
                MyParams.Clear();
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                              System.Reflection.MethodBase.GetCurrentMethod().Name,
                              ex);
            }
            return ret;
        }

        //problems
        public static bool DeclineEdit(DbEditing myItems)
        {
            bool ret = false;
            try
            {
                StringBuilder sql = new StringBuilder();
                List<Params> MyParams = new List<Params>();

                sql.AppendLine("UPDATE DATA_TRAIL SET TBLSTATUS = 0,");
                sql.AppendLine("DECLINEDBY = @BY,");
                sql.AppendLine("DECLINEDDATE = @ADATE");
                sql.AppendLine("WHERE TBLID=@TBLID");
                sql.AppendLine("AND TBLSTATUS = 3");
                MyParams.Add(new Params("@BY", "GUID", myItems.DeclineBy));
                MyParams.Add(new Params("@ADATE", "DATETIME", myItems.DeclineDate));
                MyParams.Add(new Params("@TBLID", "GUID", myItems.Tblid));
                SqlDataControl.Input(sql.ToString(), MyParams);
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                              System.Reflection.MethodBase.GetCurrentMethod().Name,
                              ex);
            }
            return ret;
        }
    }
}
