using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYER_STATE { IDLE, MOVE, DASH, JUMP, FALL };

public class Movement : MonoBehaviour
{
    PLAYER_STATE m_state;

    public float runSpeed;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    public float dashCouldown;
    public float dashForce;

    public bool facingRight;
    public bool facingLeft;
    public bool canDoubleJump;

    Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        dashCouldown -= Time.deltaTime;

        switch (m_state)
        {
            case PLAYER_STATE.IDLE:
                {
                    Move(PLAYER_STATE.IDLE);
                    Jump();
                }
                break;
            case PLAYER_STATE.MOVE:
                {
                    Move(PLAYER_STATE.IDLE);
                    Jump();
                }
                break;
            //case PLAYER_STATE.DASH:
            //    {
            //        Dash();
            //    }
            //    break;
            case PLAYER_STATE.JUMP:
                {
                   Move(PLAYER_STATE.JUMP);
                   Jump();
                }
                break;
            case PLAYER_STATE.FALL:
                {
                    Move(PLAYER_STATE.FALL);
                }
                break;
            default:
                break;
        }
    }

    private void Jump()
    {
        if (Input.GetKey("space") && CheckGround.isGrounded)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
            canDoubleJump = true;
            m_state = PLAYER_STATE.JUMP;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (canDoubleJump == true)
                {
                    rb2D.velocity = new Vector2(rb2D.velocity.x, doubleJumpSpeed);
                    canDoubleJump = false;
                }
            }
        }
    }
    private void Move(PLAYER_STATE state)
    { 
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            rb2D.velocity = new Vector2(runSpeed, rb2D.velocity.y);
            facingRight = true;
            facingLeft = false;
            if (CheckGround.isGrounded){
                m_state = PLAYER_STATE.MOVE;
            }
            
        }
        else if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            rb2D.velocity = new Vector2(-runSpeed, rb2D.velocity.y);
            facingRight = false;
            facingLeft = true;
            if (CheckGround.isGrounded)
            {
                m_state = PLAYER_STATE.MOVE;
            }
        }
      
        else
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            m_state = state;
        }
    }
    private void Dash()
    {

        if (Input.GetKey(KeyCode.LeftControl) && dashCouldown <= 0)
        {
            //m_state = PLAYER_STATE.DASH;
        
            if (facingLeft)
            {
                rb2D.AddForce(Vector2.left * dashForce, ForceMode2D.Impulse);
            }

            if (facingRight)
            {
                rb2D.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
            }
        }

        dashCouldown = 2;

    }
    public void SetPlayerState(PLAYER_STATE state)
    {
        m_state = state;
    }
}
