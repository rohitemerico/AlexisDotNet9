using System.Data;
using System.Reflection;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_Function;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_XML
{
    public class Restriction_XML
    {
        /// <summary>
        /// The helper function to generate the restriction section XML to be included in the mobileconfig file. 
        /// </summary>
        /// <param name="my_List_Restriction"></param>
        /// <param name="my_Restriction_Advance"></param>
        /// <returns></returns>
        public static string Restriction_XMLGenerator(List<MDM_Profile_Restriction> my_List_Restriction, MDM_Profile_Restriction_Advance my_Restriction_Advance)
        {
            string ret = string.Empty;
            try
            {
                ret += @"<dict>
			                <key>PayloadDescription</key>
			                <string>Configures restrictions</string>
			                <key>PayloadDisplayName</key>
			                <string>Restrictions</string>
			                <key>PayloadIdentifier</key>
			                <string>com.apple.applicationaccess." + Guid.NewGuid() + @"</string>
			                <key>PayloadType</key>
			                <string>com.apple.applicationaccess</string>
			                <key>PayloadUUID</key>
			                <string>" + Guid.NewGuid() + @"</string>
			                <key>PayloadVersion</key>
			                <integer>1</integer>";

                my_List_Restriction = my_List_Restriction.OrderBy(x => x.Queue).ToList();

                foreach (MDM_Profile_Restriction my_Restriction in my_List_Restriction)
                {

                    MDM_Restriction_Menu my_Restriction_Menu = new MDM_Restriction_Menu();
                    my_Restriction_Menu.RID = my_Restriction.RID;
                    ret += @"<key>" + Restriction_Function.RestrictionMenu_Functionality(my_Restriction_Menu) + @"</key>
                             <" + (my_Restriction.IsCheck == true ? "true" : "false") + @"/>";

                }

                if (my_Restriction_Advance.AcceptCookies != null)
                {

                    if (my_Restriction_Advance.AcceptCookies.ToUpper() == "NEVER")
                    {
                        ret += @"<key>safariAcceptCookies</key>
			                 <real>2</real>";
                    }
                    else if (my_Restriction_Advance.AcceptCookies.ToUpper() == "FROMCURRENTWEBSITEONLY" && my_Restriction_Advance.AcceptCookies.ToUpper() == "ALWAYS")
                    {
                        ret += @"<key>safariAcceptCookies</key>
			                 <real>1</real>";
                    }
                    else if (my_Restriction_Advance.AcceptCookies.ToUpper() == "FROMWEBSITESIVISIT")
                    {
                        ret += @"<key>safariAcceptCookies</key>
			                 <real>1.5</real>";
                    }
                }

                if (my_Restriction_Advance.RestrictAppUsage != null || my_Restriction_Advance.App_Identify != null)
                {
                    if (my_Restriction_Advance.RestrictAppUsage.ToUpper() == "DONOTSOMEAPPS" && my_Restriction_Advance.App_Identify != null)
                    {

                        string[] App_Idens = my_Restriction_Advance.App_Identify.Split(',');

                        ret += @"<key>blacklistedAppBundleIDs</key>
                                <array>";

                        foreach (string App_Iden in App_Idens)
                        {
                            ret += "<string>" + App_Iden + @"</string>";
                        }

                        ret += @"</array>";

                    }

                    if (my_Restriction_Advance.RestrictAppUsage.ToUpper() == "ONLYALLOWSOMEAPPS" && my_Restriction_Advance.App_Identify != null)
                    {

                        string[] App_Idens = my_Restriction_Advance.App_Identify.Split(',');

                        ret += @"<key>whitelistedAppBundleIDs</key>
                                <array>";

                        foreach (string App_Iden in App_Idens)
                        {
                            ret += "<string>" + App_Iden + @"</string>";
                        }

                        ret += @"</array>";
                    }
                }

                ret += @"</dict>";

            }
            catch (Exception ex)
            {
                ret = @"ERROR";
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                                 ex);
            }

            return ret;
        }





        public static List<MDM_Profile_Restriction> RearrangeRestriction(List<MDM_Profile_Restriction> my_MDM_Profile_Restriction)
        {
            List<MDM_Profile_Restriction> ret = new List<MDM_Profile_Restriction>();

            try
            {
                DataTable dt = ToDataTable<MDM_Profile_Restriction>(my_MDM_Profile_Restriction);

                DataTable tblFiltered = dt.AsEnumerable()
                   .OrderByDescending(row => row.Field<String>("Nachname"))
                   .CopyToDataTable();



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




        public static List<T> ToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName && dr[column.ColumnName] != DBNull.Value)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }



        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
