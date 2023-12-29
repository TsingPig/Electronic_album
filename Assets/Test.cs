using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [Header("Õº∆¨µÿ÷∑")]
    [SerializeField]
    public string url = "";
    // Use this for initialization
    void Start()
    {
        StartCoroutine(DownSprite());

    }

    IEnumerator DownSprite()
    {
        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texD1 = new DownloadHandlerTexture(true);
        wr.downloadHandler = texD1;
        yield return wr.SendWebRequest();
        int width = 1920;
        int high = 1080;
        if(!wr.isNetworkError)
        {
            Texture2D tex = new Texture2D(width, high);
            tex = texD1.texture;

            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            transform.GetComponent<Image>().sprite = sprite;
        }
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}
