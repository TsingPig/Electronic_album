using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BBSPostItem : MonoBehaviour
{
    // Start is called before the first frame update
    public Button BtnEnterPost;
    public TMP_Text UserName;
    public TMP_Text Title;
    public TMP_Text Content;
    public List<string> PhotoUrls;

    void Start()
    {
        BtnEnterPost.onClick.AddListener(() =>
        {
            Debug.Log("季神大计基");

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
