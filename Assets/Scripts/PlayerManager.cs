using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { NONE, UP, DOWN, LEFT, RIGHT };

public class PlayerManager : MonoBehaviour
{
    private BoxCollider2D box2D;
    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer spr_render;

    private int runningID;
    private int jumpingID;

    private Vector2 spdVector;
    private Vector2 prevSpd;

    private Direction moveDir;
    private Direction jumpDir;

    private bool isRunning;
    private bool isJumping;
    private bool wasJumping;
    private bool jumpPerformed;
    private bool canWallJump;
    private bool canDoubleJump;

    public float moveSpeed = 4;
    public float jumpSpeed = 300;
    public float doubleJumpSpeed = 200;
    public float dashCouldown;
    public float dashForce = 30;

    // Start is called before the first frame update
    void Start()
    {
        box2D = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr_render = GetComponent<SpriteRenderer>();

        runningID = Animator.StringToHash("isMoving");
        jumpingID = Animator.StringToHash("isJumping");

        isRunning = false;
        isJumping = true;
        wasJumping = false;
        jumpPerformed = true;
        canWallJump = false;

        moveDir = Direction.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = Direction.NONE;
        isRunning = false;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            isRunning = true;
            moveDir = Direction.RIGHT;
            spr_render.flipX = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            isRunning = true;
            moveDir = Direction.LEFT;
            spr_render.flipX = true;
        }

        if (!isJumping || canWallJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpPerformed = false;
                isJumping = true;
                canDoubleJump = true;
            }
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



        anim.SetBool(runningID, isRunning);
        if (wasJumping != isJumping)
        {
            anim.SetBool(jumpingID, isJumping);
        }
        wasJumping = isJumping;
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime * 1000;
        dashCouldown -= Time.deltaTime;
        spdVector.y = rb2d.velocity.y;
        switch (moveDir)
        {
            default:
                spdVector.x = 0;
                break;
            case Direction.RIGHT:
                spdVector.x = moveSpeed * delta;
                break;
            case Direction.LEFT:
                spdVector.x = -moveSpeed * delta;
                break;
        }
        rb2d.velocity = spdVector;

        if (isJumping && !jumpPerformed)
        {
            jumpPerformed = true;
            float jumpSpdX = 0;
            if (jumpDir == Direction.LEFT)
            {
                jumpSpdX = -jumpSpeed * delta;
                spr_render.flipX = true;
            }
            else if (jumpDir == Direction.RIGHT)
            {
                jumpSpdX = jumpSpeed * delta;
                spr_render.flipX = false;
            }
            rb2d.AddForce(new Vector2(jumpSpdX, jumpSpeed * delta));
        }

        if (Input.GetKey(KeyCode.LeftControl) && dashCouldown <= 0)
        {
            if (spr_render.flipX == false)
            {
                rb2d.AddForce(Vector2.right * dashForce * delta, ForceMode2D.Impulse);
            }
            else
            {
                rb2d.AddForce(Vector2.left * dashForce * delta, ForceMode2D.Impulse);
            }

            dashCouldown = 2;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb2d.gravityScale = 400;
        }        
    }

    private bool checkRaycastWithScenary(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Floor")
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if (isJumping)
            {
                bool col1 = false;
                bool col2 = false;
                bool col3 = false;

                float center_x = (box2D.bounds.min.x + box2D.bounds.max.x) / 2;
                float center_y = (box2D.bounds.min.y + box2D.bounds.max.y) / 2;
                Vector2 bottomCenterPos = new Vector2(center_x, box2D.bounds.min.y);
                Vector2 bottomLeftPos = new Vector2(box2D.bounds.min.x, box2D.bounds.min.y);
                Vector2 bottomRightPos = new Vector2(box2D.bounds.max.x, box2D.bounds.min.y);

                Vector2 leftPos = new Vector2(box2D.bounds.min.x, center_y);
                Vector2 rightPos = new Vector2(box2D.bounds.max.x, center_y);

                RaycastHit2D[] hits = Physics2D.RaycastAll(bottomCenterPos, -Vector2.up, 2);
                col1 = checkRaycastWithScenary(hits);

                if (!col1)
                {
                    hits = Physics2D.RaycastAll(bottomLeftPos, -Vector2.up, 2);
                    col2 = checkRaycastWithScenary(hits);
                    rb2d.gravityScale = 27;
                }
                if (!col2)
                {
                    hits = Physics2D.RaycastAll(bottomRightPos, -Vector2.up, 2);
                    col3 = checkRaycastWithScenary(hits);
                    rb2d.gravityScale = 27;
                }
                if (col1 || col2 || col3) { isJumping = false; }

                hits = Physics2D.RaycastAll(leftPos, -Vector2.right, 10);
                bool wallLeft = checkRaycastWithScenary(hits);

                hits = Physics2D.RaycastAll(rightPos, Vector2.right, 10);
                bool wallRight = checkRaycastWithScenary(hits);

                if (wallLeft || wallRight)
                {
                    if (wallLeft)
                    {
                        jumpDir = Direction.RIGHT;
                        canDoubleJump = false;
                        spr_render.flipX = false;
                        rb2d.gravityScale = 27;
                    }
                    if (wallRight)
                    {
                        jumpDir = Direction.LEFT;
                        canDoubleJump = false;
                        spr_render.flipX = true;
                        rb2d.gravityScale = 27;
                    }
                    canWallJump = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJumping = true;
            canWallJump = false;
            jumpDir = Direction.NONE;
        }
    }
}