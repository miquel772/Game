using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    Rigidbody2D rb2d;


    public float speed;

    public float jumpSpeed;
    public float doubleJumpSpeed;

    public float dashTotalDuration;
    private float dashSpeed;
    private float dashTimer;
    public float dashDistance;
   


    public bool facingRight;
    public bool facingLeft;
    public bool isDash;
    public bool isGrounded;
    public bool canDoubleJump;


    Time time;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(0.5153801f, -127.968f, 0.0f);
        isGrounded = true;
        facingRight = true;
        dashSpeed = dashDistance / dashTotalDuration;

    }

    // Update is called once per frame
    void Update()
    {

        if (isDash && dashTimer > 0 )
        {
            dashTimer -= Time.deltaTime;
        }
        else
        {
            dashTimer = dashTotalDuration;
            isDash = false;
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);

        }
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded == true)
            {
                canDoubleJump = true;
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
                isGrounded = false;


            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (canDoubleJump == true)
                    {
                        rb2d.velocity = new Vector2(rb2d.velocity.x, doubleJumpSpeed);
                        canDoubleJump = false;
                    }
                }
            }


        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
            if (Input.GetKey(KeyCode.LeftControl) && dashTotalDuration <= 0)
            {
                //Dash();
            }

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();

            if (Input.GetKey(KeyCode.LeftControl) && dashTotalDuration <= 0)
            {
                //Dash();
            }

        }
        else if(dashTotalDuration == 2f)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        if (Input.GetKey(KeyCode.LeftControl) && !isDash)
        {
            Dash();
        }

    }

    void FixedUpdate()
    {
        

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = true;
           
        }
    }


    public void Dash()
    {
        isDash = true;

        if (facingRight)
        {
            rb2d.velocity = new Vector2(dashSpeed, rb2d.velocity.y);

        }
        else if (facingLeft)
        {
            rb2d.velocity = new Vector2(-dashSpeed, rb2d.velocity.y);
        }

        
    }

    public void MoveRight()
    {
        rb2d.velocity = new Vector2(speed , rb2d.velocity.y);
        gameObject.transform.localScale = new Vector3(0.08290367f, 0.09702073f, 1);
        facingRight = true;
        facingLeft = false;
    }
    public void MoveLeft()
    {
        rb2d.velocity = new Vector2(-speed , rb2d.velocity.y);
        gameObject.transform.localScale = new Vector3(-0.08290367f, 0.09702073f, 1);
        facingRight = false;
        facingLeft = true;
    }
}
