using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace HelloWeb.DatabaseRelation
{
    public sealed class DatabaseRelation
    {
        private const string NAME_SPACE = "Model";//默认的命名空间名
        private const string STORE_DIRECTORY = @"C:/mysql/Models";
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static MySqlConnection GetMySqlConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);

        }
        /// <summary>
        /// 获取命令行
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static MySqlCommand GetSqlCommand(string sql, MySqlConnection connection)
        {
            return new MySqlCommand(sql, connection);
        }
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public static string getDbConnectionString(string serverAdress, string databaseName, string user, string password)
        {
            string connectionStrings = "server = " + serverAdress + "; Database = " + databaseName + "; User id = " + user + "; Password = " + password + ";allow zero datetime = true;charset=utf8";
            return connectionStrings;
        }

        /// <summary>
        /// 获取在文件配置的数据库连接字符串
        /// </summary>
        /// <param name="name">配置文件的中的name属性值</param>
        /// <returns></returns>
        public static string getDbConnectionString(string name)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;

        }
        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="mySqlCommand"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(MySqlCommand mySqlCommand)
        {
            DataSet dataSet = new DataSet();
            GetDataAdapter(mySqlCommand).Fill(dataSet);
            return dataSet;
        }
        /// <summary>
        /// 读取数据器
        /// </summary>
        /// <param name="mySqlCommand"></param>
        /// <returns></returns>
        public static MySqlDataAdapter GetDataAdapter(MySqlCommand mySqlCommand)
        {
            return new MySqlDataAdapter(mySqlCommand);
        }
        /// <summary>
        /// 数据表
        /// </summary>
        /// <param name="mySqlCommand"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(MySqlCommand mySqlCommand)
        {
            DataTable dataTable = new DataTable();
            GetDataAdapter(mySqlCommand).Fill(dataTable);
            return dataTable;

        }

        /// <summary>
        /// 数据库字段转换成标准属性模式
        /// </summary>
        public static SortedList<String, object> ChangeToStandard(DataTable dataTable)
        {
            SortedList<String, object> lists = new SortedList<string, object>();
            DataColumnCollection list = dataTable.Columns;
            foreach (DataColumn a in list)
            {
                String[] strArr = a.ColumnName.ToLower().Split('_');
                for (int i = 0; i < strArr.Length; i++)
                {
                    Char temp = Char.ToUpper(strArr[i].ElementAt(0));
                    //截取
                    strArr[i] = strArr[i].Substring(1);
                    strArr[i] = String.Concat(temp, strArr[i]);
                }
                lists.Add(String.Concat(strArr), a.DataType.Name);
            }
            return lists;
        }

        /// <summary>
        /// 将数据库table名转换成PASCAl 格式
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<String> ChangeToStandard(List<string> list, char separator)
        {
            List<String> lists = new List<string>();
            foreach (string a in list)
            {
                String[] strArr = a.ToLower().Split(separator);
                for (int i = 0; i < strArr.Length; i++)
                {
                    Char temp = Char.ToUpper(strArr[i].ElementAt(0));
                    //截取
                    strArr[i] = strArr[i].Substring(1);
                    strArr[i] = String.Concat(temp, strArr[i]);

                }
                lists.Add(String.Concat(strArr));
            }
            return lists;
        }

        /// <summary>
        ///  获取数据库的所有表名
        /// </summary>
        /// <param name="dataName">数据库名</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns></returns>
        public static List<string> GetDataTableName(string connectionString)
        {
            List<string> allTableName = new List<string>();
            string sql = "SHOW TABLES;";
            MySqlConnection mySqlConnection = GetMySqlConnection(connectionString);
            mySqlConnection.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            DataTable dataTable = new DataTable();
            GetDataAdapter(mySqlCommand).Fill(dataTable);
            foreach (DataColumn a in dataTable.Columns)
            {
                foreach (DataRow c in dataTable.Rows)
                {
                    allTableName.Add(c[a.ColumnName].ToString());

                }
            }
            mySqlCommand.Dispose();
            mySqlConnection.Close();
            mySqlConnection.Dispose();
            return allTableName;
        }
        /// <summary>
        /// 批量创建数据库 所有表格对应的MODEL
        /// </summary>
        /// <param name="dataName">数据库名称</param>
        /// <param name="storeDirectory">存储的目录</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="nameSpace">设定Model命名空间</param>
        public static void CreateModel( string connectionString, string storeDirectory = STORE_DIRECTORY, string nameSpace = NAME_SPACE)
        {
           
            List<string> tableNameList = GetDataTableName(connectionString);//获取数据库表名
            List<string> tableNameLists = ChangeToStandard(tableNameList, '_');//将数据库表名转化成PASCAL格式的类名
            MySqlConnection mySqlConnection = GetMySqlConnection(connectionString);
            mySqlConnection.Open();
            foreach (string tableName in tableNameLists)
            {
                if (!Directory.Exists(@storeDirectory))
                {
                    Directory.CreateDirectory(@storeDirectory);
                }
                if (!File.Exists(@storeDirectory + "/" + tableName + ".cs"))
                {

                    File.Create(@storeDirectory + "/" + tableName + ".cs").Dispose();

                }
                using (TextWriter tw = File.CreateText(@storeDirectory + "/" + tableName + ".cs"))
                {

                    tw.WriteLine("using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Web;\nusing MyLibrary;\nnamespace " + nameSpace + ".Models\n{");
                    tw.WriteLine("\tpublic class " + tableName + "  \n\t{");

                    foreach (string attr in tableNameList)
                    {
                        List<String> table = new List<string>();
                        table.Add(attr);
                        table = ChangeToStandard(table, '_');
                        if (String.Equals(table[0], tableName))
                        {
                            //读取该表格的所有字段
                            string selectSql = "select * from " + attr;
                            MySqlCommand mySqlCommand = new MySqlCommand(selectSql, mySqlConnection);
                            DataTable dataTable = GetDataTable(mySqlCommand);
                            SortedList<string, object> attributeNameList = ChangeToStandard(dataTable);
                            foreach (string attrName in attributeNameList.Keys)
                                tw.WriteLine("\t\tpublic " + attributeNameList[attrName].ToString() + " " + attrName + " { get; set; }\n");
                            break;
                        }
                        else
                        {
                            continue;
                        }

                    }
                    tw.WriteLine("\t}\n");
                    tw.WriteLine("}");
                }

            }

        }

    }
}