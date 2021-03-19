using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController sharedInstance;
    public float runningSpeed;
    public float jumpForce;
    public LayerMask groundLayer; //asignar Tag

    private Rigidbody2D rb;
    private Animator anim;
    private string currentState;
    private float x = 0; //movimiento en eje X

    private void Awake() {
        sharedInstance = this; //Singleton
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y == 0)
            Jump();

        if (x == 0) {
            ChangeAnimationState("Idle");
        } else if (x > 0) {
            //UpdateBodySprites(false);
            ChangeAnimationState("Running");
        } else if (x < 0) {
            //UpdateBodySprites(true);
            ChangeAnimationState("RunningLeft");
        }
    }

    private void FixedUpdate() {

        x = Input.GetAxis("Horizontal"); //movimiento en el eje X

        rb.position += new Vector2(x, 0) * Time.deltaTime * this.runningSpeed;
                
    }

    void Jump() {
        if (IsGrounded())
            rb.AddForce(Vector2.up * this.jumpForce * 100, ForceMode2D.Impulse);
        
    }

    bool IsGrounded() {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 10.2f, groundLayer))
            return true;
        else
            return false;
    }

    public void ChangeAnimationState(string newState) {
        if (this.currentState == newState) return;
        this.anim.Play(newState);
        this.currentState = newState;
    }

    private void UpdateBodySprites(bool lookingLeft) {
        foreach (Transform child in transform) {
            child.gameObject.GetComponent<FlipSprite>().FlipSpriteXAxis(lookingLeft);
        }
    }

}
