using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    GameObject[] pauseObjects;
    GameObject[] worldSelect;
    private int world_id = 0;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        //Load every menu to hide
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        worldSelect = GameObject.FindGameObjectsWithTag("World_select");
        //Hide every menu
        hideLevelSelector();
        hidePaused(worldSelect);
    }

    // Update is called once per frame
    void Update()
    {
        //uses the Esc button to leave the level select window
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                hidePaused(worldSelect);
            } else
            {
                Time.timeScale = 0;
                showPaused(worldSelect);
            }
        } else if (Input.GetKeyUp("space") && this.world_id > 0)
        {
            print("TimeScale : " + Time.timeScale);
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                hideLevelSelector();
            } else if (Time.timeScale == 1)
            {
                print("On affiche le menu du monde " + this.world_id);
                Time.timeScale = 0;
                showLevelSelector();
            }
        }
    }

    //hides Level Selector
    public void initWorldId()
    {
        this.world_id = 1;
    }

    public void initTimeScale()
    {
        Time.timeScale = 1;
    }

    //hides Level Selector
    public void hideLevelSelector()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    //Shows Level Selector
    public void showLevelSelector()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }


    //hides objects with ShowOnPause tag
    public void hidePaused(GameObject[] gameObjects)
    {
        foreach (GameObject g in gameObjects)
        {
            g.SetActive(false);
        }
    }

    //shows objects with ShowOnPause tag 
    public void showPaused(GameObject[] gameObjects)
    {
        foreach (GameObject g in gameObjects)
        {
            g.SetActive(true);
        }
    }

    public void setWorldId(int world_id)
    {
        this.world_id = world_id;
    }

    public void startLevel(int level)
    {
        //print("On lance le niveau " + world_id + "-" + level);
        Time.timeScale = 1;
        SceneManager.LoadScene("Level_" + world_id + "-" + level);
    }
}
