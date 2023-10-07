using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class ChangeAllTextsFont : MonoBehaviour
{
    public TMP_FontAsset font;
    public void Start()
    {
        TMP_Text[] texts = FindObjectsOfType<TMP_Text>();

        foreach (TMP_Text text in texts)
        {
            text.font = font;
        }   
    }
}
