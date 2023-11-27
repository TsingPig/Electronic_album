using Sirenix.OdinInspector;
using System;
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
        string[] values = { "Jrm", "����ʦ", "Tsingpig123asd**" };

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
        // ����Ƿ񷵻����κ���
        if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0 && result.Tables[0].Columns.Count > 1)
        {
            // �û���������ƥ�䣬��¼�ɹ�
            return true;
        }
        else
        {
            // �û��������벻ƥ�䣬��¼ʧ��
            //Console.WriteLine("��¼ʧ�ܣ��û��������벻��ȷ��");
            return false;
        }

    }


    [Button("��ѯȫ��")]
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