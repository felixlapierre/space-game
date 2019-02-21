using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float speed = 10f;
    Rigidbody2D rb;
    new Collider2D collider;
    int coins = 0;
    float radius;
    Vector3 startingPosition; //teleport back to starting position on death

    int grounded = 0; //0 means false
    int airjump = 1;
    Vector2 facing;
    Vector2 normalToSurface;

    private bool Grounded
    {
        get { return grounded > 0; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Get my rigid body
        var col = GetComponent<CircleCollider2D>();
        collider = col;
        radius = col.radius;
        startingPosition = transform.position;
        facing = new Vector3(0, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Grounded: " + (grounded > 0));
        transform.eulerAngles = new Vector3(0, 0, VectorToAngle(normalToSurface) - 90.0f);
        if(Grounded)
        {
            WalkOnWalls();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded > 0)
            {
                Jump();
            }
            else if(airjump > 0)
            {
                airjump--;
                Jump();
            }
        }
    }

    void WalkOnWalls()
    {
        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical");

        if ((inputX != 0 || inputY != 0))
        {
            var input = new Vector2(inputX, inputY).normalized;
            var movement = Vector3.Project(input, Vector2.Perpendicular(normalToSurface)).normalized;
            movement *= speed;
            rb.velocity = movement;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    float VectorToAngle(Vector2 direction)
    {
        return 360 / 2 / Mathf.PI * Mathf.Atan2(direction.y, direction.x);
    }

    void Jump()
    {
        var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        var direction = worldMousePosition - transform.position;
        direction.z = 0;
        direction.Normalize();
        facing = direction;
        rb.velocity = direction * speed;
        normalToSurface = rb.velocity.normalized;
    }

    void OnCollisionEnter2D(Collision2D collision) //col is the object we collided with
    {
        var col = collision.collider;
        
        if(col.tag == "Wall")
        {
            var contactPoint = collision.contacts[0];
            normalToSurface = contactPoint.normal;
            grounded++;
            airjump = 1;
        }
        else if(col.tag == "Coin")
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

    void OnCollisionExit2D(Collision2D collision)
    {
        var col = collision.collider;
        if(col.tag == "Wall")
        {
            grounded--;
            if (!Grounded)
                normalToSurface = rb.velocity.normalized;
        }
    }
}
