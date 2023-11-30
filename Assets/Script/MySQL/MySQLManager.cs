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

    /// <summary>
    /// �޸�ָ���˺ŵ��ǳ�
    /// </summary>
    /// <param name="account">�˺�</param>
    /// <param name="newNickName">���ǳ�</param>
    public void UpdateNickName(string account, string newNickName)
    {
        string tableName = "useraccount";
        string columnToUpdate = "nick_name";
        string conditionColumn = "account";
        string conditionValue = account;

        _mySQLAccess.Update(tableName, columnToUpdate, newNickName, conditionColumn, "=", conditionValue);
    }


    /// <summary>
    /// ��ѯָ���˺ŵ��ǳ�
    /// </summary>
    /// <param name="account">Ҫ��ѯ�ǳƵ��˺�</param>
    /// <returns>�˺Ŷ�Ӧ���ǳ�</returns>
    public string GetNickName(string account)
    {
        string tableName = "useraccount";
        string[] items = { "nick_name" };
        string[] whereColumns = { "account" };
        string[] operation = { "=" };
        string[] values = { account };

        DataSet result = _mySQLAccess.Select(tableName, items, whereColumns, operation, values);

        if(result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
        {
            // ���ز�ѯ�����ǳ�
            return result.Tables[0].Rows[0]["nick_name"].ToString();
        }
        else
        {
            // û���ҵ���Ӧ�˺ŵ��ǳ�
            return null;
        }
    }

    private new void Awake()
    {
        base.Awake();
        Init();
    }
}