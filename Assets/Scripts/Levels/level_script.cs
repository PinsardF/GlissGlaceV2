using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class level_script : MonoBehaviour
{
    private bool touchedObjective = false;
    public objective_script objectiveScript;
    public PlayerMovement playerScript;

    void Start()
    {
        Time.timeScale = 1;
        print("loaded level_script");

        //We listen for the player to touch the objective
        objectiveScript.reachedObjective.AddListener(() => { print("received event reachedObjective"); touchedObjective = true; });
    }

    // Update is called once per frame
    void Update()
    {
        if (touchedObjective && playerScript.moving)
        {
            print("fini");
        }
    }
}
