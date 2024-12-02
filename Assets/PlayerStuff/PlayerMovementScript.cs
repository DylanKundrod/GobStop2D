using UnityEngine;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents;
using System.Collections;


public class PlayerMovementScript : Agent
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
    public override void OnEpisodeBegin()
    {
        Debug.Log("Episode started. Resetting player state.");
        rb = GetComponent<Rigidbody2D>();
        grounded = true;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        origcolor = spriteRenderer.color;
        coll = GetComponent<Collider2D>();
    }

    IEnumerator MakeBouncy()
    {
        Debug.Log("Bouncy effect activated!");
        coll.sharedMaterial = bouncyMaterial;
        yield return new WaitForSeconds(6);
        Debug.Log("Bouncy effect ended.");
        coll.sharedMaterial = origMaterial;
        spriteRenderer.color = origcolor;
        gameState.isBouncy = false;
    }

    IEnumerator Glued()
    {
        Debug.Log("Glue effect activated! Reducing speed and jump.");
        animator.speed = 0.5f;
        moveSpeed = 3.5f;
        jumpSpeed = 10f;
        yield return new WaitForSeconds(4);
        Debug.Log("Glue effect ended. Restoring speed and jump.");
        animator.speed = 1;
        moveSpeed = 7;
        jumpSpeed = 13;
        gameState.isGlued = false;
        spriteRenderer.color = origcolor;
    }

    IEnumerator Fly()
    {
        Debug.Log("Fly effect activated! Reducing gravity.");
        rb.gravityScale = 1;
        jumpSpeed = 10f;
        yield return new WaitForSeconds(5);
        Debug.Log("Fly effect ended. Restoring gravity.");
        rb.gravityScale = 2;
        jumpSpeed = 13f;
        spriteRenderer.color = origcolor;
        gameState.canFly = false;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int moveDirection = actions.DiscreteActions[0];
        bool isJumping = actions.DiscreteActions[1] == 1;

        Debug.Log($"OnActionReceived - MoveDirection: {moveDirection}, IsJumping: {isJumping}");

        switch (moveDirection)
        {
            case 0:
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                Debug.Log("Moving left.");
                break;
            case 1:
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                Debug.Log("Moving right.");
                AddReward(.01f);
                break;
            default:
                Debug.Log("No movement.");
                break;
        }

        if (isJumping && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            Debug.Log("Jumping.");
        }
    }

    private void FixedUpdate()
    {
        Debug.Log("FixedUpdate called. Requesting decision.");
        RequestDecision();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        Debug.Log($"Trigger entered with: {go.tag}");

        collectionSoundEffect.Play();

        if (go.tag == "GoldCoin")
        {
            Debug.Log("Collected a GoldCoin.");
            Destroy(go);
            gameState.coins++;
            AddReward(1.0f);
        }

        if (go.tag == "RedCowDrink")
        {
            Debug.Log("Collected RedCowDrink. Activating bouncy effect.");
            Destroy(go);
            gameState.isBouncy = true;
            StartCoroutine(MakeBouncy());
        }

        if (go.tag == "Glue")
        {
            Debug.Log("Collected Glue. Activating glue effect.");
            Destroy(go);
            gameState.isGlued = true;
            StartCoroutine(Glued());
        }

        if (go.tag == "Wings")
        {
            Debug.Log("Collected Wings. Activating fly effect.");
            Destroy(go);
            gameState.canFly = true;
            StartCoroutine(Fly());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gobj = collision.gameObject;
        Debug.Log($"Collision entered with: {gobj.tag}");

        if (gobj.tag == "ground")
        {
            grounded = true;
            onIce = false;
            Debug.Log("Touching ground. Grounded set to true.");
        }

        if (gobj.tag == "Ice")
        {
            grounded = true;
            onIce = true;
            Debug.Log("Touching Ice. Grounded set to true. Ice effect activated.");
        }

        if (gobj.tag == "cp")
        {
            if (gameState.coins == gameState.currentCP)
            {
                Debug.Log("Checkpoint reached and coins match. Unlocking checkpoint.");
                gobj.GetComponent<Collider2D>().enabled = false;
                gameState.coins = 0;
                gameState.currentCP++;
                AddReward(1.0f);
            }
        }

        if (gobj.tag == "Trap")
        {
            Debug.Log("Touched a trap. Adding penalty.");
            AddReward(-1.0f);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log($"Collision exited with: {collision.gameObject.tag}");

        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "Ice")
        {
            grounded = false;
            Debug.Log("No longer grounded.");
        }
    }

    void SetAnimationState()
    {
        AnimationStateEnum playerAnimationState;

        if (grounded)
        {
            if (xDirection == 0f)
            {
                playerAnimationState = AnimationStateEnum.Idle;
                Debug.Log("Player state: Idle.");
            }
            else
            {
                playerAnimationState = AnimationStateEnum.Running;
                Debug.Log("Player state: Running.");
            }
        }
        else if (rb.velocity.y > 0f)
        {
            playerAnimationState = AnimationStateEnum.Jumping;
            Debug.Log("Player state: Jumping.");
        }
        else
        {
            playerAnimationState = AnimationStateEnum.Falling;
            Debug.Log("Player state: Falling.");
        }

        animator.SetInteger("playerState", (int)playerAnimationState);
    }

}
