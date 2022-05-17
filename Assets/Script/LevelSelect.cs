using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public bool CanSelect;
    [SerializeField]private GameObject Lock;

    private void Start()
    {
        Lock = transform.Find("lock").gameObject;
        if (transform.parent.GetChild(0).name == gameObject.name || PlayerPrefs.GetFloat(gameObject.name + "Time") > 0)
        {
            CanSelect = true;
        }

        if (CanSelect)
        {
            Lock.SetActive(false);
        }
    }

    public void OnSelect()
    {
        if (CanSelect)
        {
            PlayerPrefs.SetString("nowselect", gameObject.name);
            SceneManager.LoadScene("Demolevel");
        }
    }
}
