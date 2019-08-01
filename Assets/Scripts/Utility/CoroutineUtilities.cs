using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineUtilities : MonoBehaviour
{
    public static CoroutineUtilities Instance;

    private void Awake()
    {
        Instance = this;
    }
}
