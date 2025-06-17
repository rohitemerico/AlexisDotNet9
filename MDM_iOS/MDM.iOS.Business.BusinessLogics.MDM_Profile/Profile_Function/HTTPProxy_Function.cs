using System.Data;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using MDM.iOS.Common.Data.Component;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_Function
{
    /// <summary>
    /// CRUD (Create, Read, Update and Delete) operations on the HTTP Proxy database table. 
    /// </summary>
    public class HTTPProxy_Function
    {
        #region HTTPPRoxy

        public static DataTable HTTP_SelectAll(MDM_HttpProxy my_HttpProxy)
        {
            DataTable ret = new DataTable();
            try
            {
                using (BankIslamEntities db = new BankIslamEntities())
                {
                    var data = from o in db.MDM_HttpProxy
                               where o.Profile_ID == my_HttpProxy.Profile_ID
                               select new
                               {
                                   o.Profile_ID,
                                   o.Port,
                                   o.AllowByPassingProxy,
                                   o.Password,
                                   o.ProxyServer,
                                   o.ProxyType,
                                   o.Username


                               };
                    ret = LINQToDataTable.LINQResultToDataTable(data);
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
        #endregion

        public static Boolean HTTP_Insert(MDM_HttpProxy myProxy)
        {
            bool ret = false;
            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    entities.MDM_HttpProxy.Add(myProxy);
                    if (entities.SaveChanges() != 0)
                    {
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
    }
}
