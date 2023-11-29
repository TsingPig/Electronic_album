using Sirenix.OdinInspector;
using System.Data;
using TsingPigSDK;

public class MySQLManager : Singleton<MySQLManager>
{
    private MySQLAccess _mySQLAccess;

    private void Init()
    {
        _mySQLAccess = new MySQLAccess("gz-cynosdbmysql-grp-nlpmzwov.sql.tencentcdb.com", "21462", "Tsingpig2", "123asd**", "electronic_album");
    }

    /// <summary>
    /// �����ݿ�ע���˺�
    /// </summary>
    /// <param name="account"></param>
    /// <param name="nick_name"></param>
    /// <param name="userPassword"></param>
    public void Register(string account, string nick_name, string userPassword)
    {
        string[] columns = { "account", "nick_name", "password" };
        string[] values = { account, nick_name, userPassword };
        _mySQLAccess.Insert("useraccount", columns, values);
    }

    /// <summary>
    /// �����ݿⳢ�Ե�¼�˺�
    /// </summary>
    /// <param name="account"></param>
    /// <param name="userPassword"></param>
    /// <returns>�Ƿ�ɹ���¼</returns>
    public bool Login(string account, string userPassword)
    {
        string[] items = { "account", "password" };
        string tablename = "useraccount";
        string[] operation = { "=", "=" };
        string[] whereColumns = { "account", "password" };
        string[] value = { account, userPassword };
        DataSet result = new DataSet();
        result = _mySQLAccess.Select(tablename, items, whereColumns, operation, value);
        // ����Ƿ񷵻����κ���
        if(result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0 && result.Tables[0].Columns.Count > 1)
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

    private new void Awake()
    {
        base.Awake();
        Init();
    }
}