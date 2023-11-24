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
        string[] values = { "aaa", "aaa", "aaa" };

        _mySQLAccess.Insert("useraccount", columns, values);
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