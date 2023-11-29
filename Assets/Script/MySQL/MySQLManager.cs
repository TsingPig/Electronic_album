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
    /// 向数据库注册账号
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
    /// 向数据库尝试登录账号
    /// </summary>
    /// <param name="account"></param>
    /// <param name="userPassword"></param>
    /// <returns>是否成功登录</returns>
    public bool Login(string account, string userPassword)
    {
        string[] items = { "account", "password" };
        string tablename = "useraccount";
        string[] operation = { "=", "=" };
        string[] whereColumns = { "account", "password" };
        string[] value = { account, userPassword };
        DataSet result = new DataSet();
        result = _mySQLAccess.Select(tablename, items, whereColumns, operation, value);
        // 检查是否返回了任何行
        if(result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0 && result.Tables[0].Columns.Count > 1)
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

    private new void Awake()
    {
        base.Awake();
        Init();
    }
}