using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TopInfobar : MonoBehaviour
{

    public static Action<String> OnTopInfobarUpdate;

    private void Awake()
    {
        OnTopInfobarUpdate += HandleInfobarUpdate;
    }

    public void HandleInfobarUpdate(String str)
    {
        StartCoroutine(WriteThenClear(str));
    }

    IEnumerator WriteThenClear(String str)
    {
        TextMeshProUGUI textComp = gameObject.GetComponent<TextMeshProUGUI>();
        textComp.text = str;

        yield return new WaitForSeconds(5);
        textComp.text = "";
    }
}
