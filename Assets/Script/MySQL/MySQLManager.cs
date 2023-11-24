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
    MySQLAccess mysql;


    private void Start()
    {
        mysql = new MySQLAccess(host, port, userName, password, databaseName);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.pointerPress.name == "loginButton")
        {     //�����ǰ���µİ�ť��ע�ᰴť 
            OnClickedLoginButton();
        }
    }

    /// <summary>
    /// ���µ�¼��ť
    /// </summary>
    private void OnClickedLoginButton()
    {
        mysql.OpenSql();
        string loginMsg = "";
        DataSet ds = mysql.Select
            ("useraccount", new string[] { "level" },
            new string[] { "`" + "account" + "`", "`" + "password" + "`" },
            new string[] { "=", "=" },
            new string[] { });
        if(ds != null)
        {
            DataTable table = ds.Tables[0];
            if(table.Rows.Count > 0)
            {
                loginMsg = "��½�ɹ���";
                Debug.Log("�û�Ȩ�޵ȼ���" + table.Rows[0][0]);
            }
            else
            {
                loginMsg = "�û������������";
            }
        }
        mysql.CloseSql();
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