using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public static bool isGrounded;
    Movement player;

    private void Start()
    {
        player = GetComponentInParent<Movement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        player.SetPlayerState(PLAYER_STATE.IDLE);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isGrounded = false;
    }

}
