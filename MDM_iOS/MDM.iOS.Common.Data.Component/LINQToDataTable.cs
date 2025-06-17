using System.Data;
using System.Reflection;

namespace MDM.iOS.Common.Data.Component;

/// <summary>
/// Method involves conversion from LINQ to DataTable
/// </summary>
public class LINQToDataTable
{

    public static DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
    {
        DataTable dt = new DataTable();


        PropertyInfo[] columns = null;

        if (Linqlist == null) return dt;

        foreach (T Record in Linqlist)
        {

            if (columns == null)
            {
                columns = ((Type)Record.GetType()).GetProperties();
                foreach (PropertyInfo property in columns)
                {
                    Type colType = property.PropertyType;

                    //if user-defined prop,  not part of the standard assembly (mscorlib)
                    if ((colType.IsClass) && (colType.Assembly.FullName != columns.GetType().Assembly.FullName))
                    {

                    }

                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        //colType = colType.GetGenericArguments()[0];
                        colType = Nullable.GetUnderlyingType(colType);
                    }

                    dt.Columns.Add(new DataColumn(property.Name, colType));
                }
            }

            DataRow dr = dt.NewRow();

            foreach (PropertyInfo pinfo in columns)
            {
                dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                (Record, null);
            }

            dt.Rows.Add(dr);
        }
        return dt;
    }

}
