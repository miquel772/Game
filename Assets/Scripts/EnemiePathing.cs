using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiePathing : MonoBehaviour
{
    public GameObject targetTile;
    public float moveSpeed;
    public bool isMoving;
    Rigidbody2D rb2d;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        moveEnemy();
    }

    private void moveEnemy()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "TargetTile")
        {
            isMoving = false;
        }
    }
}