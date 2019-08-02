using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public void Toggle(GameObject _object)
    {
        _object.SetActive(!_object.activeSelf);
    }
}
