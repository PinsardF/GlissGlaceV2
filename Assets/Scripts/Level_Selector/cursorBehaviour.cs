using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorBehaviour : MonoBehaviour

{
    public Rigidbody2D rb;
    //public SpriteRenderer spriteRenderer;
    //public Transform thisTransform;
    //public UIManager UIManager;
    public GameObject UIManager;
    public GameObject levelSelect;
    //This list doesn't contain the actual levels but their scripts
    private List<pinPointScript> levels = new List<pinPointScript>();
    private bool moving = false;
    private pinPointScript cursorOnLevel; 
    private Vector2 moveDirection;
    public float moveSpeed;
    private Vector2 destination;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < 4; i++)
        {
            pinPointScript script = GameObject.Find("World_" + i).GetComponent<pinPointScript>();
            levels.Add(script);
        }
        // Finding which level we are on
        foreach (pinPointScript level in levels)
        {
            if (level.pintransform.position.x == rb.position.x && level.pintransform.position.y == rb.position.y)
            {
                cursorOnLevel = level;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            ProcessInputs();
        }
    }

    void ProcessInputs()
    {
        if (!moving)
        {
            //Get the axis of the movement (horizontal or vertical)
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            //Checking if there is an arrow input
            if (moveX != 0 || moveY != 0)
            {
                bool gonnaMove = false;
                //Checking if we can go into the direction input
                if (moveX > 0 && cursorOnLevel.right.Length > 0)
                {
                    //print("On peut aller à droite");
                    moveDirection = new Vector2(cursorOnLevel.right[0], cursorOnLevel.right[1]) - rb.position;
                    destination = new Vector2(cursorOnLevel.right[0], cursorOnLevel.right[1]);
                    gonnaMove = true;
                } else if (moveX < 0 && cursorOnLevel.left.Length > 0)
                {
                    //print("On peut aller à gauche");
                    moveDirection = new Vector2(cursorOnLevel.left[0], cursorOnLevel.left[1]) - rb.position;
                    destination = new Vector2(cursorOnLevel.left[0], cursorOnLevel.left[1]);
                    gonnaMove = true;
                }
                else if (moveY > 0 && cursorOnLevel.up.Length > 0)
                {
                    //print("On va en haut");
                    moveDirection = new Vector2(cursorOnLevel.up[0], cursorOnLevel.up[1]) - rb.position;
                    destination = new Vector2(cursorOnLevel.up[0], cursorOnLevel.up[1]);
                    gonnaMove = true;
                }
                else if (moveY < 0 && cursorOnLevel.down.Length > 0)
                {
                    //print("On peut aller en bas");
                    moveDirection = new Vector2(cursorOnLevel.down[0], cursorOnLevel.down[1]) - rb.position;
                    destination = new Vector2(cursorOnLevel.down[0], cursorOnLevel.down[1]);
                    gonnaMove = true;
                }

                //Then we move
                if (gonnaMove)
                {
                    moving = true;
                    Move();
                    cursorOnLevel = null;
                }
            } else if (Input.GetKey("space")) {
                print("On ouvre le menu du monde " + cursorOnLevel.world);
                //Displaying the UI
                Time.timeScale = 0;
                UIManager.GetComponent<UIManager>().setWorldId(cursorOnLevel.world);
                UIManager.GetComponent<UIManager>().showPaused();
            }

        }
    }

    void Move()
    {
        moveDirection.Normalize();
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            CheckDestination();
        }
    }

    void CheckDestination()
    {
        //We can't be sure that the cursor will go exactly on the point so we look for the moment where he is very close to it
        if (Mathf.Abs(rb.position.x - destination.x) < 0.2 && Mathf.Abs(rb.position.y - destination.y) < 0.2)
        {
            rb.velocity = new Vector2(0, 0);
            moving = false;
            rb.position = destination;
            // Finding which level we are on
            foreach (pinPointScript level in levels)
            {
                if (level.pintransform.position.x == rb.position.x && level.pintransform.position.y == rb.position.y)
                {
                    cursorOnLevel = level;
                }
            }
        }
    }
}