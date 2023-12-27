using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlbumItem : MonoBehaviour
{
    public TMP_Text albumTitle;
    private void Start()
    {
        albumTitle.text = gameObject.name;

    }
}
