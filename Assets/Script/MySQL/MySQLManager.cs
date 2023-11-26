using Sirenix.OdinInspector;
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
        string[] values = { "Tsing", "青猪", "Tsingpig123asd**" };

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