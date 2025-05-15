using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class MyPluginExample : MonoBehaviour
{
    [DllImport("__Internal")]
    static extern void TestEntryPoint();
   
    [DllImport("__Internal")]
    static extern int TestEntryPoint2();

    public TMP_Text text;
    
    private void Start()
    {
        DoTest();
    }

    public void DoTest()
    {
        Debug.Log("About to call TestEntryPoint");
        TestEntryPoint();
        Debug.Log("Called TestEntryPoint");
        int result = TestEntryPoint2();
        Debug.Log($"Called TestEntryPoint2, and got back: {result}");

        text.text = result.ToString();
    }
}
