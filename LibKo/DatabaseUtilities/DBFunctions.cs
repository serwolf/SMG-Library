using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Windows.Forms;
namespace DatabaseUtilities.DatabaseUtilities
{
    public abstract class DBFunctions
    {
        private static string GetConnectionString()
        {
            string str = "";

            try
            {
                str = ConfigurationManager.AppSettings["ConnectionString"];
            }
            catch (Exception)
            {

                str = "";
            }

            return str;

        }

        private static object ReadValue(string selectStatement, string field)
        {
            object obj = (object)null;
            SqlConnection connection = new SqlConnection(DBFunctions.GetConnectionString());
            try
            {
                SqlConnection.ClearAllPools();
                SqlCommand sqlCommand = new SqlCommand(selectStatement, connection);
                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.Read())
                    obj = sqlDataReader[field];
                sqlDataReader.Close();
                sqlDataReader.Dispose();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.StartsWith("Timeout"))
                    return DBFunctions.ReadValue(selectStatement, field);
                int num = (int)MessageBox.Show(ex.Message, "Database access error");
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return obj;
        }

        private static ArrayList ReadValues(string selectStatement, string field)
        {
            ArrayList arrayList = new ArrayList();
            SqlConnection connection = new SqlConnection(DBFunctions.GetConnectionString());
            try
            {
                SqlCommand sqlCommand = new SqlCommand(selectStatement, connection);
                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (sqlDataReader.Read())
                    arrayList.Add(sqlDataReader[field]);
                sqlDataReader.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return arrayList;
        }

        public static object DLookup(string field, string table, string criteria)
        {
            if (criteria == null)
                return (object)null;
            return DBFunctions.ReadValue("SELECT [" + field + "] FROM [" + table + "] WHERE (" + criteria + ")", field);
        }

        public static ArrayList DLookupMult(string field, string table)
        {
            return DBFunctions.ReadValues("SELECT [" + field + "] FROM [" + table + "]", field);
        }

        public static ArrayList DLookupMult(string field, string table, string criteria)
        {
            if (criteria == null)
                return (ArrayList)null;
            return DBFunctions.ReadValues("SELECT [" + field + "] FROM [" + table + "] WHERE (" + criteria + ")", field);
        }

        public static ArrayList DLookupMult(string field, string table, string criteria, string sortField)
        {
            if (criteria == null)
                return (ArrayList)null;
            return DBFunctions.ReadValues("SELECT [" + field + "] FROM [" + table + "] WHERE (" + criteria + ") ORDER BY " + sortField, field);
        }

        public static object DLookup(string field, string table, Guid primaryKey)
        {
            if (primaryKey.Equals((object)null))
                return (object)null;
            return DBFunctions.ReadValue("SELECT [" + field + "] FROM [" + table + "] WHERE ([" + DBFunctions.GetKeyField(table) + "] = '" + primaryKey.ToString() + "')", field);
        }

        public static double DMax(string field, string table)
        {
            return DBFunctions.DMax(field, table, (string)null);
        }

        public static double DMax(string field, string table, string criteria)
        {
            double result = -1.0;
            ArrayList arrayList = DBFunctions.DMax(field, table, criteria, 1, "[" + field + "]");
            if (arrayList.Count > 0)
                double.TryParse(arrayList[0].ToString(), out result);
            return result;
        }

        public static ArrayList DMax(string field, string table, string criteria, int howMany, string orderField)
        {
            string str = "SELECT TOP " + howMany.ToString() + " [" + field + "] FROM [" + table + "]";
            if (criteria != null)
                str = str + " WHERE " + criteria;
            return DBFunctions.ReadValues(str + " ORDER BY " + orderField + " DESC", field);
        }

        public static double DMin(string field, string table)
        {
            return DBFunctions.DMin(field, table, (string)null);
        }

        public static double DMin(string field, string table, string criteria)
        {
            double result = -1.0;
            ArrayList arrayList = DBFunctions.DMin(field, table, criteria, 1, "[" + field + "]");
            if (arrayList.Count > 0)
                double.TryParse(arrayList[0].ToString(), out result);
            return result;
        }

        public static ArrayList DMin(string field, string table, string criteria, int howMany, string orderField)
        {
            string str = "SELECT TOP " + (object)howMany + " " + field + " FROM " + table;
            if (criteria != null)
                str = str + " WHERE " + criteria;
            return DBFunctions.ReadValues(str + " ORDER BY " + orderField, field);
        }

        public static Guid DFirst(string table)
        {
            return new Guid(DBFunctions.DFirst((string)null, table, (string)null).ToString());
        }

        public static object DFirst(string field, string table)
        {
            return DBFunctions.DFirst(field, table, (string)null);
        }

        public static object DFirst(string field, string table, string criteria)
        {
            return DBFunctions.DFirst(field, table, criteria, 1)[0];
        }

        public static ArrayList DFirst(string field, string table, string criteria, int howMany)
        {
            if (field == null)
                field = DBFunctions.GetKeyField(table);
            string selectStatement = "SELECT TOP " + howMany.ToString() + " [" + field + "] FROM [" + table + "]";
            if (criteria != null)
                selectStatement = selectStatement + " WHERE " + criteria;
            return DBFunctions.ReadValues(selectStatement, field);
        }

        public static Guid DLast(string table)
        {
            return new Guid(DBFunctions.DLast((string)null, table, (string)null).ToString());
        }

        public static object DLast(string field, string table)
        {
            return DBFunctions.DLast(field, table, (string)null);
        }

        public static object DLast(string field, string table, string criteria)
        {
            return DBFunctions.DLast(field, table, criteria, 1)[0];
        }

        public static ArrayList DLast(string field, string table, string criteria, int howMany)
        {
            if (field == null)
                field = DBFunctions.GetKeyField(table);
            string str = "SELECT TOP " + howMany.ToString() + " [" + field + "] FROM [" + table + "]";
            if (criteria != null)
                str = str + " WHERE " + criteria;
            return DBFunctions.ReadValues(str + " ORDER BY [AutoNumber] DESC", field);
        }

        public static double DSum(string field, string table)
        {
            return DBFunctions.DSum(field, table, (string)null);
        }

        public static double DSum(string field, string table, string criteria)
        {
            string selectStatement = "SELECT ISNULL(SUM([" + field + "]),0) AS SumField FROM [" + table + "]";
            if (criteria != null)
                selectStatement = selectStatement + " WHERE " + criteria;
            return double.Parse(DBFunctions.ReadValue(selectStatement, "SumField").ToString());
        }

        public static double DAverage(string field, string table)
        {
            return DBFunctions.DAverage(field, table, (string)null);
        }

        public static double DAverage(string field, string table, string criteria)
        {
            string selectStatement = "SELECT isnull(AVG([" + field + "]),0) AS AvgField FROM " + table + "]";
            if (criteria != null)
                selectStatement = selectStatement + " WHERE " + criteria;
            return double.Parse(DBFunctions.ReadValue(selectStatement, "AvgField").ToString());
        }

        public static int DCount(string table)
        {
            return DBFunctions.DCount("*", table, (string)null);
        }

        public static int DCount(string field, string table)
        {
            return DBFunctions.DCount(field, table, (string)null);
        }

        public static int DCount(string field, string table, string criteria)
        {
            if (!field.Equals("*"))
                field = "[" + field + "]";
            string selectStatement = "SELECT isnull(COUNT(" + field + "),0) AS CountField FROM [" + table + "]";
            if (criteria != null)
                selectStatement = selectStatement + " WHERE " + criteria;
            return int.Parse(DBFunctions.ReadValue(selectStatement, "CountField").ToString());
        }

        public static int DUpdate(string field, string table, string criteria, string newValue)
        {
            int num1 = -1;
            SqlConnection connection = new SqlConnection(DBFunctions.GetConnectionString());
            string cmdText = "UPDATE [" + table + "] SET [" + field + "] = " + newValue;
            if (criteria != null)
                cmdText = cmdText + " WHERE (" + criteria + ")";
            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmdText, connection);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                int num2 = (int)MessageBox.Show(ex.Message);
            }
            return num1;
        }

        public static Guid DBInsert(string[] fieldNames, string table, object[] values, Guid userID)
        {
            int num = -1;
            if (fieldNames.Length != values.Length)
                return new Guid();
            SqlConnection connection = new SqlConnection(DBFunctions.GetConnectionString());
            string str1 = "";
            string str2 = "";
            for (int index = 0; index < fieldNames.Length; ++index)
            {
                if (fieldNames[index] != "[User]" && fieldNames[index] != "RegDate")
                {
                    str1 = str1 + "[" + fieldNames[index] + "], ";
                    str2 = values[index].GetType() != typeof(string) ? (values[index].GetType() != typeof(Guid) ? str2 + values[index].ToString() + ", " : str2 + "'" + values[index].ToString() + "', ") : str2 + "N'" + values[index].ToString() + "', ";
                }
            }
            string str3 = str1 + "[User]";
            string str4 = str2 + "'" + userID.ToString() + "'";
            string cmdText = "INSERT INTO [" + table + "] (" + str3 + ") VALUES (" + str4 + ")";
            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmdText, connection);
                connection.Open();
                num = sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message + "\nInsert Command was: " + cmdText);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            if (num == 1)
                return DBFunctions.DLast(table);
            else
                return new Guid();
        }

        public static int DBDelete(Guid primaryKey, string table)
        {
            int num1 = -1;
            if (string.IsNullOrEmpty(table))
                return num1;
            SqlConnection connection = new SqlConnection(DBFunctions.GetConnectionString());
            string cmdText = "DELETE FROM [" + table + "] WHERE [" + DBFunctions.GetKeyField(table) + "] = '" + primaryKey.ToString() + "'";
            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmdText, connection);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                int num2 = (int)MessageBox.Show(ex.Message + "\n\nCommand was: " + cmdText);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return num1;
        }

        public static int DBDelete(Guid[] primaryKeys, string table)
        {
            int num1 = -1;
            if (primaryKeys.Length < 1 || string.IsNullOrEmpty(table))
                return num1;
            SqlConnection connection = new SqlConnection(DBFunctions.GetConnectionString());
            string keyField = DBFunctions.GetKeyField(table);
            string str = "DELETE FROM " + table + " WHERE";
            foreach (Guid guid in primaryKeys)
                str = str + " (" + keyField + " = '" + guid.ToString() + "') OR";
            string cmdText = str.Substring(0, str.Length - 3);
            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmdText, connection);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                int num2 = (int)MessageBox.Show(ex.Message + "\n\nCommand was: " + cmdText);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return num1;
        }

        public static int DBDelete(string criteria, string table)
        {
            int num1 = -1;
            if (string.IsNullOrEmpty(criteria) || string.IsNullOrEmpty(table))
                return num1;
            SqlConnection connection = new SqlConnection(DBFunctions.GetConnectionString());
            string cmdText = "DELETE FROM " + table + " WHERE " + criteria;
            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmdText, connection);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                int num2 = (int)MessageBox.Show(ex.Message + "\n\nCommand was: " + cmdText);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return num1;
        }

        public static string GetKeyField(string tableName)
        {
            return DBFunctions.ReadValue("(SELECT c.name AS COLUMN_NAME FROM sys.key_constraints AS k INNER JOIN sys.tables AS t ON t.object_id = k.parent_object_id INNER JOIN sys.index_columns AS ic ON ic.object_id = t.object_id AND ic.index_id = k.unique_index_id INNER JOIN sys.columns AS c ON c.object_id = t.object_id AND c.column_id = ic.column_id WHERE (t.name = N'" + tableName + "'))", "COLUMN_NAME").ToString();
        }

        public static ArrayList ExecuteReaderQuery(string commandText)
        {
            ArrayList arrayList = new ArrayList();
            SqlConnection connection = new SqlConnection(DBFunctions.GetConnectionString());
            try
            {
                SqlCommand sqlCommand = new SqlCommand(commandText, connection);
                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (sqlDataReader.Read())
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>(sqlDataReader.FieldCount);
                    for (int ordinal = 0; ordinal < sqlDataReader.FieldCount; ++ordinal)
                        dictionary.Add(sqlDataReader.GetName(ordinal), sqlDataReader[ordinal].ToString());
                    arrayList.Add((object)dictionary);
                }
                sqlDataReader.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                arrayList = new ArrayList();
            }
            return arrayList;
        }

        public static DataSet GetData(String consult)
        {
            SqlConnection connection = new SqlConnection(DBFunctions.GetConnectionString());

            SqlCommand consultCommand = new SqlCommand(consult, connection);

            DataSet set = new DataSet("Result");

            try
            {
                connection.Open();

                SqlDataAdapter sda = new SqlDataAdapter(consultCommand);

                sda.Fill(set);
            }
            catch (SqlException)
            {
                set = new DataSet("Result");
            }
            finally
            {
                connection.Close();
            }
            return set;
        }

        public static int ExecuteNonReader(string commandText)
        {
            int num1 = -1;
            if (string.IsNullOrEmpty(commandText))
                return num1;
            SqlConnection connection = new SqlConnection(DBFunctions.GetConnectionString());
            try
            {
                SqlCommand sqlCommand = new SqlCommand(commandText, connection);
                connection.Open();
                num1 = sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                int num2 = (int)MessageBox.Show(ex.Message + "\n\nCommand was: " + commandText);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return num1;
        }
    }
}
