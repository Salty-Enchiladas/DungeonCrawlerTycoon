using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatLog : MonoBehaviour
{
    public static CombatLog Instance;
    public TextMeshProUGUI log;

    private void Awake()
    {
        Instance = this;
    }

    public static void Log(string message)
    {
        Instance.log.text += "\n" + message;
    }

    public static void ClearLog()
    {
        Instance.log.text = "";
    }
}
