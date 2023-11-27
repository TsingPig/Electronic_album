using Sirenix.OdinInspector;
using System;
using System.Data;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.EventSystems;

public class MySQLManager : Singleton<MySQLManager>
{

    //IP地址
    public string host;
    //端口号
    public string port;
    //用户名
    public string userName;
    //密码
    public string password;
    //数据库名称
    public string databaseName;
    //封装好的数据库类
    MySQLAccess _mySQLAccess;


    private void Start()
    {
        _mySQLAccess = new MySQLAccess(host, port, userName, password, databaseName);
    }

    [Button("注册测试")]
    public void Register()
    {
        _mySQLAccess = new MySQLAccess(host, port, userName, password, databaseName);

        string[] columns = { "account", "nick_name", "password" };
        string[] values = { "Jrm", "季老师", "Tsingpig123asd**" };

        _mySQLAccess.Insert("useraccount", columns, values);


        //_mySQLAccess.ShowTables();
    }

    public void Register(string account, string nick_name, string userPassword)
    {
        _mySQLAccess = new MySQLAccess(host, port, userName, password, databaseName);

        string[] columns = { "account", "nick_name", "password" };
        string[] values = { account, nick_name, userPassword };

        _mySQLAccess.Insert("useraccount", columns, values);
    }

    public bool Login(string account, string userPassword)
    {

        _mySQLAccess = new MySQLAccess(host, port, userName, password, databaseName);
        string[] items = { "account", "password" };
        string tablename = "useraccount";
        string[] operation = { "=", "=" };
        string[] whereColumns = { "account", "password" };
        string[] value = { account, userPassword };
        DataSet result = new DataSet();
        result = _mySQLAccess.Select(tablename, items, whereColumns, operation, value);
        // 检查是否返回了任何行
        if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0 && result.Tables[0].Columns.Count > 1)
        {
            // 用户名和密码匹配，登录成功
            return true;
        }
        else
        {
            // 用户名和密码不匹配，登录失败
            //Console.WriteLine("登录失败：用户名或密码不正确。");
            return false;
        }

    }


    [Button("查询全部")]
    public void QueryAll()
    {

        _mySQLAccess = new MySQLAccess(host, port, userName, password, databaseName);
        _mySQLAccess.Select("useraccount", "*");

    }
    private void Init()
    {

    }

    private new void Awake()
    {
        base.Awake();
        Init();
    }
}