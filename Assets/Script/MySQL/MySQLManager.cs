using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using TsingPigSDK;

public class MySQLManager : Singleton<MySQLManager>
{
    private MySQLAccess _mySQLAccess;

    private void Init()
    {
        _mySQLAccess = new MySQLAccess("gz-cynosdbmysql-grp-cttmzfob.sql.tencentcdb.com", "25862", "Tsingpig", "123asd**", "electronic_album");
    }

    /// <summary>
    /// 向数据库注册账号
    /// </summary>
    /// <param name="account"></param>
    /// <param name="nick_name"></param>
    /// <param name="userPassword"></param>
    /// <returns>是否成功注册</returns>
    public bool Register(string account, string nick_name, string userPassword)
    {
        string[] columns = { "account", "nick_name", "password" };
        string[] values = { account, nick_name, userPassword };
        return _mySQLAccess.Insert("useraccount", columns, values);
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


    /// <summary>
    /// 向数据库尝试登录管理员账号
    /// </summary>
    /// <param name="account"></param>
    /// <param name="userPassword"></param>
    /// <returns>是否成功登录</returns>
    public bool LoginSuper(string account, string userPassword)
    { 
        string[] items = { "account", "password", "isSuper" };
        string tablename = "useraccount";
        string[] operation = { "=", "=" };
        string[] whereColumns = { "account", "password" };
        string[] value = { account, userPassword };
        DataSet result = new DataSet();
        result = _mySQLAccess.Select(tablename, items, whereColumns, operation, value);
        // 检查是否返回了任何行
        if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0 && result.Tables[0].Columns.Count > 1 )
        {
            object columnValue = result.Tables[0].Rows[0][2];
            bool booleanValue = (bool)columnValue;
            if (booleanValue) { return true; }// 用户名和密码匹配，登录成功
            else { return false; }
        }
        else
        {
            // 用户名和密码不匹配，登录失败
            //Console.WriteLine("登录失败：用户名或密码不正确。");
            return false;
        }
    }

    /// <summary>
    /// 修改指定账号的昵称
    /// </summary>
    /// <param name="account">账号</param>
    /// <param name="newNickName">新昵称</param>
    public void UpdateNickName(string account, string newNickName)
    {
        string tableName = "useraccount";
        string columnToUpdate = "nick_name";
        string conditionColumn = "account";
        string conditionValue = account;

        _mySQLAccess.Update(tableName, columnToUpdate, newNickName, conditionColumn, "=", conditionValue);
    }

    /// <summary>
    /// 查询指定账号的昵称
    /// </summary>
    /// <param name="account">要查询昵称的账号</param>
    /// <returns>账号对应的昵称</returns>
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
            // 返回查询到的昵称
            return result.Tables[0].Rows[0]["nick_name"].ToString();
        }
        else
        {
            // 没有找到对应账号的昵称
            return null;
        }
    }

    private new void Awake()
    {
        base.Awake();
        Init();
    }
}