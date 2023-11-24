using Sirenix.OdinInspector;
using System.Data;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.EventSystems;

public class MySQLManager : Singleton<MySQLManager>
{

    //IP��ַ
    public string host;
    //�˿ں�
    public string port;
    //�û���
    public string userName;
    //����
    public string password;
    //���ݿ�����
    public string databaseName;
    //��װ�õ����ݿ���
    MySQLAccess _mySQLAccess;


    private void Start()
    {
        _mySQLAccess = new MySQLAccess(host, port, userName, password, databaseName);
    }

    [Button("ע�����")]
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