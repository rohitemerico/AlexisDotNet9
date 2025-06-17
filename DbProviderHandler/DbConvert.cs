using System.Data;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;

namespace DbProviderHandler
{
    public class ConvertedQuery
    {
        public string StrCommand { get; set; }
        public List<Params> ParameterList { get; set; }
    }

    public class DbConvert
    {
        /// <summary>
        /// If input = :id , method output = @id.
        /// Note: Oracle uses : with parameter name. SQL Server uses @ with parameter name.
        /// Note: Oracle uses Guid, Sql server uses unique identifier, Guid converted to NVARCHAR (using SSMA tool)
        /// Note: Oracle NCLOB converted to NVARCHAR (using SSMA tool)
        /// Original project uses Oracle, now migrated to SQL server
        /// </summary>
        /// <param name="strCommand"></param>
        /// <param name="myParams"></param>
        /// <returns></returns>
        public static ConvertedQuery ConvertOracleParam(string strCommand, List<Params> myParams)
        {
            string strCommandNew = strCommand;
            List<Params> myParamsNew = myParams;

            if (strCommand.Contains(":"))
            {
                strCommandNew = strCommand.Replace(":", "@");
                Logger.LogToFile("OracleDb-SqlServer", "SqlData_Android", "GetResult(...)", $"Found Oracle parameter syntax! Converted: \n{strCommand}");
            }

            for (int i = 0; i < myParams.Count; i++)
            {
                //change to @
                if (myParams[i].dataName.Contains(":"))
                    myParamsNew[i].dataName = myParams[i].dataName.Replace(":", "@");

                //change guid to NVARCHAR
                if (myParams[i].dataValue != null)
                {
                    if (myParams[i].dataValue.GetType().Name.ToLower() == "guid")
                    {
                        myParamsNew[i].dataType = "NVARCHAR";
                        myParamsNew[i].dataValue = myParams[i].dataValue.ToString();
                    }
                }

                //change NCLOB to NVARCHAR
                if (myParams[i].dataType.ToString().ToUpper() == "NCLOB")
                {
                    myParamsNew[i].dataType = "NVARCHAR";
                    myParamsNew[i].dataValue = myParams[i].dataValue.ToString();
                }

            }

            return new ConvertedQuery() { StrCommand = strCommandNew, ParameterList = myParamsNew };
        }

        /// <summary>
        /// Note: Oracle after migrated to SQL server with SSMA tool, ...
        /// all numeric types including ints are normalized to a standard single representation among all platforms.
        /// Thus, int --> numeric type (A synonym for decimal type in C#)
        /// Original project uses Oracle, now migrated to SQL server
        /// </summary>
        /// <param name="ret_dt"></param>
        /// <returns></returns>
        public static DataTable ConvertOracleDataType(DataTable ret_dt)
        {
            return null;
        }
    }
}
