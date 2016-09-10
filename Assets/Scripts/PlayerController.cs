using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private enum State { Normal, OnWallRight, OnWallLeft, InAir }

    private State currentState;

    [SerializeField]
    private float runSpeed = 4f;
    [SerializeField]
    private float jumpForce = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private Vector2 horizontalSpeed;
    private bool canJump;
    private bool shouldJump;

    private GameObject currentPlatform;

    void Start()
    {        
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        currentState = State.Normal;
        horizontalSpeed = Vector2.zero;

        canJump = true;
    }

    void Update()
    {
        if (currentState != State.OnWallLeft && currentState != State.OnWallRight)
            horizontalSpeed = Vector2.right * (Input.GetAxis("Horizontal") * runSpeed);
        else
            horizontalSpeed = Vector2.zero;

        CheckJump();     
    }

    private void CheckJump()
    {
        if (Input.GetButtonDown("Vertical") && canJump)
        {
            shouldJump = true;
        }
    }

    void FixedUpdate()
    {
        // Move horizontally    
        gameObject.transform.Translate(horizontalSpeed * Time.fixedDeltaTime);

        if(shouldJump)
            DoJump();        
    }

    private void DoJump()
    {
        Vector2 jumpVector = Vector2.zero;

        switch (currentState)
        {
            case State.Normal:
                jumpVector = Vector2.up * jumpForce;
                break;
            case State.OnWallLeft:
                jumpVector = new Vector2(-.2f, 1f) * jumpForce;
                break;
            case State.OnWallRight:
                jumpVector = new Vector2(.2f, 1f) * jumpForce;
                break;
        }
        canJump = false;
        shouldJump = false;
        rb.velocity = Vector2.zero;

        currentState = State.InAir;
        sprite.color = Color.white;
        rb.AddForce(jumpVector, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;

        // Landed on top of something.
        if (normal == Vector2.up)
        {
            currentState = State.Normal;
            sprite.color = Color.white;
            canJump = true;
        }
        // Hit the bottom of something.
        else if (normal == Vector2.down)
        {
            sprite.color = Color.white;
            currentState = State.Normal;
        }
        // Hit the right side of something.
        else if (normal == Vector2.right && currentState == State.InAir && collision.gameObject.tag == "StickableWall")
        {
            sprite.color = Color.red;
            currentState = State.OnWallRight;
            canJump = true;
        }
        // Hit the left side of something.
        else if(normal == Vector2.left && currentState == State.InAir && collision.gameObject.tag == "StickableWall")
        {
            currentState = State.OnWallLeft;
            sprite.color = Color.red;
            canJump = true;
        }
        // Hit an angled something.
        else
        {
            Debug.Log("Unknown : " + normal);
        }
        currentPlatform = collision.gameObject;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //if(collision.gameObject == currentPlatform && currentState == State.OnWall)
        //{
        //    gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        //}
    }

}
