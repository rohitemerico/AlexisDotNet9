using System.Data;

namespace Alexis.Common;

public class DataHelper
{
    public static List<T> ConvertDataTableToList<T>(DataTable dt) where T : new()
    {
        List<T> dataList = new List<T>();

        foreach (DataRow row in dt.Rows)
        {
            T obj = new T();

            foreach (var property in obj.GetType().GetProperties())
            {
                if (dt.Columns.Contains(property.Name))
                {
                    if (row[property.Name] != DBNull.Value)
                    {
                        property.SetValue(obj, row[property.Name]);
                    }
                }
            }

            dataList.Add(obj);
        }

        return dataList;
    }

    public static DataTable ToDataTable<T>(List<T> items)
    {
        DataTable dataTable = new DataTable(typeof(T).Name);

        // Get all the properties
        var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        // Create columns based on property types
        foreach (var prop in properties)
        {
            Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            dataTable.Columns.Add(prop.Name, propType);
        }

        // Add rows
        foreach (var item in items)
        {
            var values = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                var value = properties[i].GetValue(item, null);
                values[i] = value ?? DBNull.Value; // Handle null safely
            }
            dataTable.Rows.Add(values);
        }

        return dataTable;
    }
}
