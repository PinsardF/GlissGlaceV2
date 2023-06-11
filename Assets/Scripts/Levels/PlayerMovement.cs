using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;
    public Rigidbody2D rb;
    public GameObject character;
    private Vector2 moveDirection;
    public bool moving = false;
    private List<string> collisionSides = new List<string>();
    public Transform characterTransform;
    private string direction;

    private void Start()
    {
        print("loaded PlayerMovement script");
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        Move();
    }

    //Processing the inputs
    void ProcessInputs()
    {
        
        if (Input.GetKey("p"))
        {
            SceneManager.LoadScene("Level_Selector");
        }
        if (!moving)
        {
            //Get the axis of the movement (horizontal or vertical)
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            //Get the move Vector (if no collision)
            if (moveX != 0)
            {
                //print(characterTransform.position);
                //Checking the collisions
                if ((moveX > 0 && !collisionSides.Contains("right")) || (moveX < 0 && !collisionSides.Contains("left")))
                {
                    moveX = moveX / Mathf.Abs(moveX);
                    moveY = 0;

                    moveDirection = new Vector2(moveX, moveY);
                    moving = true;
                    //print("moving passe à true");

                    collisionSides.Clear();

                    direction = "vertical";
                }

            } else if (moveY != 0)
            {
                //print(rb.position);
                //print(character.transform.position);

                if ((moveY > 0 && !collisionSides.Contains("up")) || (moveY < 0 && !collisionSides.Contains("down")))
                {
                    moveY = moveY / Mathf.Abs(moveY);
                    moveX = 0;

                    moveDirection = new Vector2(moveX, moveY);
                    moving = true;
                    //print("moving passe à true");

                    collisionSides.Clear();

                    direction = "horizontal";
                }
            }
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCircle(rb.position, 0.52f, new ContactFilter2D(), colliders);

        float positionX = Convert.ToSingle(Math.Round(rb.position.x * 2, MidpointRounding.AwayFromZero) / 2);
        float positionY = Convert.ToSingle(Math.Round(rb.position.y * 2, MidpointRounding.AwayFromZero) / 2);

        //print(rb.position);

        bool blocked = false;
        float contactPosition = -100f;
        foreach(ContactPoint2D point in collision.contacts)
        {
            if (contactPosition == -100f)
            {
                if (direction == "vertical")
                {
                    contactPosition = point.point.x;
                } else
                {
                    contactPosition = point.point.y;
                }
            } else
            {
                if (direction == "vertical")
                {
                    if (contactPosition == point.point.x)
                    {
                        blocked = true;
                    }
                } else
                {
                    if (contactPosition == point.point.y)
                    {
                        blocked = true;
                    }
                }
            }
        }

        if (blocked)
        {
            //print("BOOM");
            moveDirection = new Vector2(0, 0);
            rb.velocity = moveDirection;
            transform.position = new Vector3(positionX, positionY, 0f);
            //moving = false;
            //rb.position.Set(positionX, positionY);

            foreach (Collider2D collider in colliders)
            {
                //Up
                if (collider.OverlapPoint(new Vector2(positionX, positionY + 0.6f)))
                {
                    collisionSides.Add("up");
                    //print("up");
                }
                //Down
                else if (collider.OverlapPoint(new Vector2(positionX, positionY - 0.6f)))
                {
                    collisionSides.Add("down");
                    //print("down");
                }
                //Left
                else if (collider.OverlapPoint(new Vector2(positionX - 0.6f, positionY)))
                {
                    collisionSides.Add("left");
                    //print("left");
                }
                //Right
                else if (collider.OverlapPoint(new Vector2(positionX + 0.6f, positionY)))
                {
                    collisionSides.Add("right");
                    //print("right");
                }
            }
            //Lock the character from moving for a few frames
            StartCoroutine(WaitAndUnlock(0.04f));
        }
    }

    //Wait for some time then allow the player to move the character again
    IEnumerator WaitAndUnlock(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        moving = false;
        //print("moving passe à false");
        //print("unlock");
    }
}
