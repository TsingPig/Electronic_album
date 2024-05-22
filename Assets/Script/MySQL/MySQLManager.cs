using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using TsingPigSDK;
using Unity.VisualScripting.Antlr3.Runtime;

public class MySQLManager : Singleton<MySQLManager>
{
    private MySQLAccess _mySQLAccess;

    private void Init()
    {
        //_mySQLAccess = new MySQLAccess("gz-cynosdbmysql-grp-cttmzfob.sql.tencentcdb.com", "25862", "Tsingpig", "123asd**", "electronic_album");
        _mySQLAccess = new MySQLAccess("cd-cdb-8py1pna8.sql.tencentcdb.com", "23211", "root", "114514_191980", "database");
    }

    /// <summary>
    /// �����ݿ�ע���˺�
    /// </summary>
    /// <param name="account"></param>
    /// <param name="nick_name"></param>
    /// <param name="userPassword"></param>
    /// <returns>�Ƿ�ɹ�ע��</returns>
    public bool Register(string account, string nick_name, string userPassword)
    {
        string[] columns = { "account", "nick_name", "password" };
        string[] values = { account, nick_name, userPassword };
        return _mySQLAccess.Insert("useraccount", columns, values);
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
    /// �����ݿⳢ�Ե�¼����Ա�˺�
    /// </summary>
    /// <param name="account"></param>
    /// <param name="userPassword"></param>
    /// <returns>�Ƿ�ɹ���¼</returns>
    public bool LoginSuper(string account, string userPassword)
    { 
        string[] items = { "account", "password", "isSuper" };
        string tablename = "useraccount";
        string[] operation = { "=", "=" };
        string[] whereColumns = { "account", "password" };
        string[] value = { account, userPassword };
        DataSet result = new DataSet();
        result = _mySQLAccess.Select(tablename, items, whereColumns, operation, value);
        // ����Ƿ񷵻����κ���
        if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0 && result.Tables[0].Columns.Count > 1 )
        {
            object columnValue = result.Tables[0].Rows[0][2];
            bool booleanValue = (bool)columnValue;
            if (booleanValue) { return true; }// �û���������ƥ�䣬��¼�ɹ�
            else { return false; }
        }
        else
        {
            // �û��������벻ƥ�䣬��¼ʧ��
            //Console.WriteLine("��¼ʧ�ܣ��û��������벻��ȷ��");
            return false;
        }
    }

    /// <summary>
    /// �����ǳ�
    /// </summary>
    /// <param name="account"></param>
    /// <returns>��ѯ�ǳ�</returns>
    public string GetNickname(string account)
    {
        string[] items = { "nick_name" };
        string tablename = "useraccount";
        string[] operation = { "=" };
        string[] whereColumns = { "account" };
        string[] value = { account};
        DataSet result = new DataSet();
        result = _mySQLAccess.Select(tablename, items, whereColumns, operation, value);
        // ����Ƿ񷵻����κ���
        object columnValue = result.Tables[0].Rows[0][0];
        string NickNameValue = (string)columnValue;
        return NickNameValue;   
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

    /// <summary>
    /// ��ѯָ���˺��Ƿ�Ϊ����Ա
    /// </summary>
    /// <param name="account">Ҫ��ѯ�ǳƵ��˺�</param>
    /// <returns>�˺Ŷ�Ӧ���ǳ�</returns>
    public bool GetIsSuper(string account)
    {
        string tableName = "useraccount";
        string[] items = { "is_super" };
        string[] whereColumns = { "account" };
        string[] operation = { "=" };
        string[] values = { account };

        DataSet result = _mySQLAccess.Select(tableName, items, whereColumns, operation, values);

        if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
        {
            // ���ز�ѯ�����ǳ��Ƿ�Ϊ����Ա
            string columnValue = result.Tables[0].Rows[0]["is_super"].ToString();
            bool booleanValue = (bool)(columnValue == "1");
            return booleanValue ? true : false;
        }
        else
        {
            // û���ҵ���Ӧ�˺ŵ��ǳ�
            return false;
        }
    }

    public int GetSectionidBySectionName(string sectionName)
    {
        string tableName = "sectioninfo";
        string[] items = { "sectionid" };
        string[] whereColumns = { "sectionname" };
        string[] operation = { "=" };
        string[] values = { sectionName };

        DataSet result = _mySQLAccess.Select(tableName, items, whereColumns, operation, values);

        if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
        {
            // ���ز�ѯ����id
            return int.Parse(result.Tables[0].Rows[0]["sectionid"].ToString());
        }
        else
        {
            // û���ҵ���Ӧ�˺ŵ�id
            return -1;
        }
    }
    private new void Awake()
    {
        base.Awake();
        Init();
    }
}