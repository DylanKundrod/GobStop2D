using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField]
    private GameState gameState;
    bool grounded = false;
    bool spaceBarDown = false;
    float jumpSpeed = 13.0f;
    float moveSpeed = 7.0f;
    Rigidbody2D rb;
    Animator animator;
    Collider2D coll;
    float xDirection = 0.0f;
    float xIceDirection = 0.0f;
    bool onIce = false;
    public Color origcolor;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    PhysicsMaterial2D origMaterial;
    [SerializeField]
    PhysicsMaterial2D bouncyMaterial;
    [SerializeField]
    private AudioSource collectionSoundEffect;

    private enum AnimationStateEnum
    {
        Idle = 0,
        Running = 1,
        Jumping = 2,
        Falling = 3
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grounded = true;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        origcolor = spriteRenderer.color;
        coll = GetComponent<Collider2D>();
    }


    IEnumerator MakeBouncy()
    {
        coll.sharedMaterial = bouncyMaterial;
        yield return new WaitForSeconds(6);
        coll.sharedMaterial = origMaterial;
        spriteRenderer.color = origcolor;
        gameState.isBouncy = false;
    }

    IEnumerator Glued()
    {
        animator.speed = 0.5f;
        moveSpeed = 3.5f;
        jumpSpeed = 10f;
        yield return new WaitForSeconds(4);
        animator.speed = 1;
        moveSpeed = 7;
        jumpSpeed = 13;
        gameState.isGlued = false;
        spriteRenderer.color = origcolor;
    }


    IEnumerator Fly()
    {
        rb.gravityScale = 1;
        jumpSpeed = 10f;
        yield return new WaitForSeconds(5);
        rb.gravityScale = 2;
        jumpSpeed = 13f;
        spriteRenderer.color = origcolor;
        gameState.canFly = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (grounded)
            {
                spaceBarDown = true;
                grounded = false;
            }

            if (gameState.canFly)
            {
                spaceBarDown = true;
            }
        }

        xDirection = Input.GetAxisRaw("Horizontal");

        xIceDirection = Input.GetAxis("Horizontal");
        

        if (xDirection < 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 0);
        }

        if (gameState.isBouncy)
        {
            spriteRenderer.color = Color.red;
            StartCoroutine(MakeBouncy());
        }

        if (gameState.isGlued)
        {
            spriteRenderer.color = Color.yellow;
            StartCoroutine(Glued());
        }

        if (gameState.canFly)
        {
            spriteRenderer.color = Color.cyan;
            StartCoroutine(Fly());
        }

        SetAnimationState();
     

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(xDirection * moveSpeed, rb.velocity.y);
        
        if (onIce)
        {
            rb.velocity = new Vector2(xIceDirection* moveSpeed * 1.75f, rb.velocity.y);
        }


        if (spaceBarDown)
        {
            spaceBarDown = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        collectionSoundEffect.Play();
        if (go.tag == "GoldCoin")
        {
            Destroy(go);
            gameState.coins++;
        }

        if (go.tag == "RedCowDrink")
        {
            Destroy(go);
            gameState.isBouncy = true;
        }

        if (go.tag == "Glue")
        {
            Destroy(go);
            gameState.isGlued = true;
        }

        if (go.tag == "Wings")
        {
            Destroy(go);
            gameState.canFly= true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gobj = collision.gameObject;

        if (gobj.tag == "ground")
        {
            grounded = true;
            onIce = false;
        }

        if (gobj.tag == "Ice")
        {
            Debug.Log("Touching ICE");
            grounded = true;
            onIce = true;
            
        }

        if (gobj.tag == "cp")
        {
            if (gameState.coins == gameState.currentCP)
            {
                gobj.GetComponent<Collider2D>().enabled = false;
                gameState.coins = 0;
                gameState.currentCP++;
            }
        }
        /*
        if (gobj.tag == "finalCP")
        {
            if (gameState.coins == gameState.currentCP)
            {
                gobj.GetComponent<Collider2D>().enabled = false;
                gameState.coins = 0;
                gameState.gameOver = true;
            }
        }*/
       
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "Ice")
        {
            grounded = false;
        }

        if (collision.gameObject.tag == "Ice" )
        {
            grounded= false;
        }
    }


    void SetAnimationState()
    {
        AnimationStateEnum playerAnimationState;

        if(grounded)
        {
            if (xDirection == 0f)
            {
                playerAnimationState = AnimationStateEnum.Idle;
            }
            else
            {
                playerAnimationState = AnimationStateEnum.Running;
            }
        }

        else if (rb.velocity.y > 0f) 
        {
            playerAnimationState = AnimationStateEnum.Jumping;

        }

        else 
        {
            playerAnimationState = AnimationStateEnum.Falling;
        }




        animator.SetInteger("playerState", (int)playerAnimationState);


    }

}
