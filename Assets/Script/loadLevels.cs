using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadLevels : MonoBehaviour
{
    private void Awake()
    {
        Instantiate(Resources.Load(PlayerPrefs.GetString("nowselect")));
    }
}
