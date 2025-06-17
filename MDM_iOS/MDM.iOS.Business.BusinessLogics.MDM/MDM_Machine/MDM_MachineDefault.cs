using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.iOS.Common.Data.Component;
using MDM.iOS.Entities;

namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_Machine
{
    /// <summary>
    /// Actual implementation of retrieving or updating database called TBLMACHINE by implementing the MDM_MachineBase abstract class.
    /// </summary>
    public class MDM_MachineDefault : MDM_MachineBase
    {
        /// <summary>
        /// Updates the designated branch of a particular machine. This method is 
        /// called from the MDM Machine Maintenance edit function. 
        /// </summary>
        /// <param name="Maen"></param>
        /// <returns></returns>
        public override bool UpdateMachineBranchID(DeviceEn Maen)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            try
            {
                if (!string.IsNullOrEmpty(Maen.DeviceSerial))
                {
                    sql.AppendLine("UPDATE TBLMACHINE SET BRANCHID=@BID WHERE MACHINESERIAL= @MACHINESERIAL");
                    //sql.AppendLine("UPDATE TBLMACHINE SET NEWLONGITUDE = 5, NEWLATITUDE = 5 WHERE MachineImei ");
                    sql.AppendLine("#SPLIT#");
                    sql.AppendLine("UPDATE MDM_Enrollment SET BRANCHID=@BID WHERE SERIALNO= @MACHINESERIAL");

                    myParams.Add(new Params("@BID", "GUID", Maen.BranchId));
                    myParams.Add(new Params("@MACHINESERIAL", "NVARCHAR", Maen.DeviceSerial));

                    ret = SqlDataControl.Input(sql.ToString(), myParams);
                }

            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                         System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                sql.Clear();
                myParams.Clear();
            }

            return ret;
        }

        /// <summary>
        /// This method updates the machine branch ID from the enrollment page. 
        /// </summary>
        /// <param name="MaEn"></param>
        /// <returns></returns>
        public override bool UpdateMachineBranchIDOnly(DeviceEn MaEn)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            try
            {
                if (!string.IsNullOrEmpty(MaEn.DeviceSerial))
                {
                    sql.AppendLine("UPDATE TBLMACHINE SET BRANCHID=@BID WHERE MACHINESERIAL= @MACHINESERIAL");


                    myParams.Add(new Params("@BID", "GUID", MaEn.BranchId));
                    myParams.Add(new Params("@MACHINESERIAL", "NVARCHAR", MaEn.DeviceSerial));

                    ret = SqlDataControl.Input(sql.ToString(), myParams);
                }
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                         System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                sql.Clear();
                myParams.Clear();
            }

            return ret;
        }

        /// <summary>
        /// Updates the machine status variable based on the machine UDID. 
        /// </summary>
        /// <param name="MaEn"></param>
        /// <returns></returns>
        public override bool UpdateMachineStatus(DeviceEn MaEn)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            try
            {
                sql.AppendLine("UPDATE TBLMACHINE SET MACHINESTATUS = @MSTAT WHERE MACHINEUDID = @MUDID");
                myParams.Add(new Params("@MSTAT", "INT", MaEn.DeviceStatus));
                myParams.Add(new Params("@MUDID", "NVARCHAR", MaEn.DeviceUDID));
                ret = SqlDataControl.Input(sql.ToString(), myParams);
            }

            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                         System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                sql.Clear();
                myParams.Clear();
            }

            return ret;
        }

        /// <summary>
        /// Select the list of machines that are actively enrolled in the MDM server and 
        /// are assigned with a branch. 
        /// </summary>
        /// <param name="My_BranchId"></param>
        /// <returns></returns>
        public override DataTable getMachine_byBranch(Guid My_BranchId)
        {
            DataTable ret = new DataTable();

            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            try
            {

                sql.AppendLine("SELECT * ");
                sql.AppendLine("from tblMachine");
                sql.AppendLine("WHERE MACHINESTATUS = 1 AND BranchId IS NOT NULL");
                //sql.AppendLine("WHERE MachineImei IS NOT NULL AND MACHINESTATUS = 1 AND BranchId IS NOT NULL");

                if (My_BranchId != Guid.Empty)
                {
                    sql.AppendLine("AND BranchId=@BranchId");
                }

                myParams.Add(new Params("@BranchId", "GUID", My_BranchId));

                ret = SqlDataControl.GetResult(sql.ToString(), myParams);


            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                         System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                sql.Clear();
                myParams.Clear();
            }

            return ret;

        }

    }
}
