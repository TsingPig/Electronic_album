using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BBSPostItem : MonoBehaviour
{
    // Start is called before the first frame update
    public Button BtnEnterPost;

    void Start()
    {
        BtnEnterPost.onClick.AddListener(() =>
        {
            Debug.Log("�����ƻ�");

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
