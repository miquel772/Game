using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHeath : MonoBehaviour
{

    public float healthPoints = 100;
    Rigidbody2D rb2D;
    void Start()
    {

        rb2D = GetComponent<Rigidbody2D>();


        // Update is called once per frame
        void Update()
        {
            if (healthPoints == 0)
            {
                Destroy(gameObject);
            }


        }

        

       
    }
}
