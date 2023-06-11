using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class objective_script : MonoBehaviour
{
    public UnityEvent reachedObjective = new UnityEvent();
    private bool collided = false;

    // Start is called before the first frame update
    void Start()
    {
        print("loaded objective_script");
    }

    // Update is called once per frame
    void Update()
    {
        //Nothing
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Only executes once
        if (!collided)
        {
            //When we reach the objective, we invoke the event reachedObjective so the level_script gets it
            collided = true;
            print("invoke reachedObjective");
            reachedObjective.Invoke();
        }
    }
}
