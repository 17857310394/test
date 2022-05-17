using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public List<PickUp> redGems;
    public List<PickUp> blueGems;
    public Door RedDoor;
    public Door BlueDoor;
    public playerController RedMan;
    public playerController BlueMan;
    public int DeathNum;
    public float GameTime;

    bool IsGameOver;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);

        redGems = new List<PickUp>();
        blueGems = new List<PickUp>();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        if (IsGameOver)
        {
            return;
        }
        GameTime += Time.deltaTime;
        UIManager.UpdateTimeText(GameTime);

        TotheEnd();
    }

    public static void RegisterDoor(Door door)
    {
        switch (door.gameObject.tag)
        {
            case "RedItem":
                instance.RedDoor = door;
                break;
            case "BlueItem":
                instance.BlueDoor = door;
                break;
            default:
                Debug.Log("error door");
                break;
        }
    }

    public static void RegisterPlayer(playerController player)
    {
        switch (player.gameObject.tag)
        {
            case "RedMan":
                instance.RedMan = player;
                break;
            case "BlueMan":
                instance.BlueMan = player;
                break;
            default:
                Debug.Log("error player");
                break;
        }
    }

    public static void RegisterGem(PickUp orb)
    {
        switch (orb.gameObject.tag)
        {
            case "RedItem":
                if (!instance.redGems.Contains(orb))
                {
                    instance.redGems.Add(orb);

                    //UIManager.AddOrbText(orb);
                }
                break;
            case "BlueItem":
                if (!instance.blueGems.Contains(orb))
                {
                    instance.blueGems.Add(orb);

                    //UIManager.AddOrbText(orb);
                }
                break;
            default:
                Debug.Log("error orb");
                break;
        } 
    }

    public static void RemoveGem(PickUp orb)
    {
        switch (orb.gameObject.tag)
        {
            case "RedItem":
                if (instance.redGems.Contains(orb))
                {
                    instance.redGems.Remove(orb);
                }
                break;
            case "BlueItem":
                if (instance.blueGems.Contains(orb))
                {
                    instance.blueGems.Remove(orb);
                }
                break;
            default:
                Debug.Log("error orb");
                break;

                //if (instance.orbs.Count == 0)
                //{
                //    instance.door.OpenDoor();
                //}
        }
    }

    public void TotheEnd()
    {
        if (instance.RedDoor.IsOpen && instance.BlueDoor.IsOpen)
        {
            instance.RedMan.IsLeave = true;
            instance.BlueMan.IsLeave = true;
            Invoke("NextLevel", 1.5f);
        }
    }

    public static void DestoryPlayer()
    {
        Destroy(instance.RedMan.gameObject);
        Destroy(instance.BlueMan.gameObject);
        Debug.Log("sss");
    }

    public static void playerDied()
    {
        instance.Invoke("RestartScene", 1.5f);
        instance.DeathNum++;
        //UIManager.UpdateDieText(instance.DeathNum);
    }

    public void NextLevel()
    {
        SaveMsg();
        string nowlevel = PlayerPrefs.GetString("nowselect");
        string nowlevelNum = (int.Parse(nowlevel.Substring(nowlevel.Length - 1, 1)) + 1).ToString();
        PlayerPrefs.SetString("nowselect", "level" + nowlevelNum);
        SceneManager.LoadScene("DemoLevel");
    }

    public void SaveMsg()
    {
        PlayerPrefs.SetFloat(PlayerPrefs.GetString("nowselect")+"Time", GameTime);
        Debug.Log("用时:"+GameTime);

        PlayerPrefs.SetInt(PlayerPrefs.GetString("nowselect") + "RedGem", redGems.Count);
        Debug.Log("剩余红宝石:" + redGems.Count);

        PlayerPrefs.SetInt(PlayerPrefs.GetString("nowselect") + "BlueGem", blueGems.Count);
        Debug.Log("剩余蓝宝石:" + blueGems.Count);

        float score = 10000 - (redGems.Count + blueGems.Count) * 500 - GameTime * 20;
        PlayerPrefs.SetFloat(PlayerPrefs.GetString("nowselect") + "Score", score);
        Debug.Log("分数:" + score);
    }

    private void RestartScene()
    {
        instance.redGems.Clear();
        instance.blueGems.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
