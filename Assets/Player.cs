using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float speed = 5f;
    Rigidbody2D rb;
    int coins = 0;
    Vector3 startingPosition; //teleport back to starting position on death

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Get my rigid body
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var input = Input.GetAxis("Horizontal");
        var movement = input * speed;
        rb.velocity = new Vector3(movement, rb.velocity.y, 0);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0, 200, 0)); //Add force straight up
        }
    }

    void OnTriggerEnter2D(Collider2D col) //col is the object we collided with
    {
        if(col.tag == "Coin")
        {
            coins++;
            Destroy(col.gameObject); //remove coin
        }
        else if (col.tag == "Water")
        {
            //Return to start
            transform.position = startingPosition;
        }
        else if (col.tag == "Spike")
        {
            //Return to start
            transform.position = startingPosition;
        }
        else if (col.tag == "End")
        {
            //Go to next level
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
