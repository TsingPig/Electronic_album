using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace TsingPigSDK
{
    public class MySQLAccess
    {
        //���������
        private static MySqlConnection mySqlConnection;
        //IP��ַ
        private static string host;
        //�˿ں�
        private static string port;
        //�û���
        private static string userName;
        //����
        private static string password;
        //���ݿ�����
        private static string databaseName;

        /// <summary>
        /// ���췽��
        /// </summary>
        /// <param name="_host">ip��ַ</param>
        /// <param name="_userName">�û���</param>
        /// <param name="_password">����</param>
        /// <param name="_databaseName">���ݿ�����</param>
        public MySQLAccess(string _host, string _port, string _userName, string _password, string _databaseName)
        {
            host = _host;
            port = _port;
            userName = _userName;
            password = _password;
            databaseName = _databaseName;
            OpenSql();
        }

        /// <summary>
        /// �����ݿ�
        /// </summary>
        public void OpenSql()
        {
            try
            {
                string mySqlString = string.Format("server = {0};port={1};database = {2};user = {3};password = {4};", host, port, databaseName, userName, password);
                mySqlConnection = new MySqlConnection(mySqlString);
                //if(mySqlConnection.State == ConnectionState.Closed)
                mySqlConnection.Open();

            } catch(Exception e)
            {
                throw new Exception("����������ʧ�ܣ������¼��MySql�����Ƿ�򿪡�" + e.Message.ToString());
            }
                
        }

        /// <summary>
        /// �ر����ݿ�
        /// </summary>
        public void CloseSql()
        {
            if(mySqlConnection != null)
            {
                mySqlConnection.Close();
                mySqlConnection.Dispose();
                mySqlConnection = null;
            }
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="items">Ҫ��ѯ����</param>
        /// <param name="whereColumnName">��ѯ��������</param>
        /// <param name="operation">����������</param>
        /// <param name="value">������ֵ</param>
        /// <returns></returns>
        public DataSet Select(string tableName, string[] items, string[] whereColumnName,
            string[] operation, string[] value)
        {

            if(whereColumnName.Length != operation.Length || operation.Length != value.Length)
            {
                throw new Exception("���벻��ȷ��" + "Ҫ��ѯ������������������������ֵ ��������һ�£�");
            }

            string query = $"SELECT `{items[0]}`";
            for(int i = 1; i < items.Length; i++)
            {
                query += $", `{items[i]}`";
            }

            query += $" FROM `{tableName}` WHERE `{whereColumnName[0]}` {operation[0]} '{value[0]}'";
            for(int i = 1; i < whereColumnName.Length; i++)
            {
                query += $" AND `{whereColumnName[i]}` {operation[i]} '{value[i]}'";
            }
            DataSet result = QuerySet(query);
            Log.LogQueryResult(result);
            return result;
        }

        /// <summary>
        /// ��ѯȫ��
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="wildcard"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public DataSet Select(string tableName, string wildcard)
        {
            if(wildcard != "*")
            {
                throw new ArgumentException("Wildcard must be '*'.", nameof(wildcard));
            }
            string query = $"SELECT * FROM `{tableName}`";
            DataSet result = QuerySet(query);
            Log.LogQueryResult(result);
            return result;
        }

        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="sqlString">sql���</param>
        /// <returns></returns>
        private DataSet QuerySet(string sqlString)
        {
            if(mySqlConnection.State == ConnectionState.Open)
            {
                DataSet ds = new DataSet();
                try
                {
                    MySqlDataAdapter mySqlAdapter = new MySqlDataAdapter(sqlString, mySqlConnection);
                    mySqlAdapter.Fill(ds);
                } catch(Exception e)
                {
                    throw new Exception("SQL:" + sqlString + "/n" + e.Message.ToString());
                } finally
                {
                }
                return ds;
            }
            return null;
        }


        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="columns">Ҫ���������</param>
        /// <param name="values">Ҫ�����ֵ</param>
        public void Insert(string tableName, string[] columns, string[] values)
        {
            if(columns.Length != values.Length)
            {
                throw new Exception("���벻��ȷ��" + "Ҫ������к�ֵ��������һ�£�");
            }

            string query = $"INSERT INTO  `{tableName}`  ({string.Join(",", columns)}) VALUES ('{string.Join("','", values)}')";
            Log.Info($"��������{query}");
            ExecuteNonQuery(query);
        }


        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="columnToUpdate">Ҫ���µ�����</param>
        /// <param name="newValue">�µ�ֵ</param>
        /// <param name="whereColumnName">��������</param>
        /// <param name="operation">����������</param>
        /// <param name="value">����ֵ</param>
        public void Update(string tableName, string columnToUpdate, string newValue,
                           string whereColumnName, string operation, string value)
        {
            string update = $"UPDATE `{tableName}` SET `{columnToUpdate}` = '{newValue}' WHERE `{whereColumnName}` {operation} '{value}'";
            ExecuteNonQuery(update);
            Log.Info($"��������: {update}");
        }


        /// <summary>
        /// ִ�зǲ�ѯSQL���
        /// </summary>
        /// <param name="sqlString">sql���</param>
        private void ExecuteNonQuery(string sqlString)
        {
            if(mySqlConnection.State == ConnectionState.Open)
            {
                try
                {
                    using(MySqlCommand cmd = new MySqlCommand(sqlString, mySqlConnection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                } catch(Exception e)
                {
                    throw new Exception("SQL:" + sqlString + "\n" + e.Message.ToString());
                }
            }
        }


        /// <summary>
        /// ��ʾTable�б�
        /// </summary>
        /// <returns></returns>
        private List<string> ShowTables()
        {
            string query = "SHOW TABLES";
            DataSet result = QuerySet(query);

            if(result != null && result.Tables.Count > 0)
            {
                List<string> tableNames = new List<string>();

                foreach(DataRow row in result.Tables[0].Rows)
                {
                    tableNames.Add(row[0].ToString());
                }

                foreach(string tableName in tableNames)
                {
                    Log.Info(tableName);
                }
                return tableNames;
            }

            return null;
        }

    }

}
