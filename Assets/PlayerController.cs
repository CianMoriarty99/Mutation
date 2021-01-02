using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject audioObj1, audioObj2, audioObj3, platform;

    Rigidbody2D rb;
    public float speed, refSpeed, jumpForce, dashSpeed, xRaw, yRaw, x, y;

    public bool walk, onGround, onLeftWall, onRightWall, onPlatform, climbing, climbingAquired, dashing, canDash, dashAquired, wallJumpAquired, doubleJump, doubleJumpAquired, sprinting;

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;

    public LayerMask groundLayer;

    SpriteRenderer spriteRef;

    public Animator animator, musicAnim1, musicAnim2, musicAnim3;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRef = GetComponent<SpriteRenderer>();
        //dashAquired = false;
        refSpeed = speed;

              
    }

    // Update is called once per frame
    void Update(){

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        xRaw = Input.GetAxisRaw("Horizontal");
        yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);
        Vector2 rawDir = new Vector2(xRaw, yRaw);

        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));


    
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
       
        if (rb.velocity.y > 0.01){
            //animator.SetBool("isJumping", true);
            //animator.SetBool("isFalling", false);
        }

        if (rb.velocity.y < 0.01){
            //animator.SetBool("isJumping", false);
            //animator.SetBool("isFalling", true);
        }

        if (rb.velocity.x < -0.01){
            spriteRef.flipX = true;
        }
        if (rb.velocity.x > 0.01) {
            spriteRef.flipX = false;
        }
        
        if(onLeftWall || onRightWall)
        {
            climbing = true;
            Climb(dir);

        }
        else 
        {
            climbing = false;
        }

        if(dashing)
        {
            animator.SetBool("Dashing", true);
        }
        else{
            animator.SetBool("Dashing", false);
        }

        if(onPlatform)
        {
            
        }


        if(onGround)
        {
            climbing = false;
            canDash = true;
            doubleJump = true;
        }
        if(!dashing && !climbing)
            Walk(dir);

        if (Input.GetButtonDown("Jump"))
            Jump();

        if(Input.GetButtonDown("Fire1")){
            if(canDash && !onGround && dashAquired)
            {  
                dashing = true;
                StartCoroutine("Dash", rawDir);
                canDash = false;
    

            }

                
        }


        //Left shift = fire3
        if(Input.GetButton("Fire3"))
        {
            animator.SetBool("Sprinting", true);
            speed = refSpeed * 2;

        }
        else
        {
            speed = refSpeed;
            animator.SetBool("Sprinting", false);
        }
    

    }
    

    private void Walk(Vector2 dir){
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }

    private void Climb(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, dir.y * speed);
    }

    private void Jump(){
        if (onGround){
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += jumpForce * Vector2.up;
        }
        
        else if (onRightWall){
            if(wallJumpAquired)
            {
                dashing = true;
                StartCoroutine("Dash", new Vector2(-0.8f, 1.2f));
            }
        }

        else if (onLeftWall){
            if(wallJumpAquired)
            {
                dashing = true;
                StartCoroutine("Dash", new Vector2(0.8f, 1.2f));
            }
        }

        else if(doubleJump && doubleJumpAquired)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += jumpForce * Vector2.up;
            doubleJump = false;
        }

    }





    IEnumerator Dash(Vector2 rawDir){
        rb.gravityScale = 0;
        rb.drag = 10f;
        rb.velocity = rawDir.normalized * new Vector2(dashSpeed, dashSpeed/1.3f);
        yield return new WaitForSeconds(0.3f);
        rb.gravityScale = 6f;
        rb.drag = 0;
        dashing = false;
        animator.SetBool("Dashing", false);
        
        
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] {bottomOffset, rightOffset, leftOffset};

        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }

    void OnTriggerEnter2D(Collider2D col){

        if(col.gameObject.tag == "flag1"){
            //musicAnim1.SetTrigger("fadeOut");
            //StartCoroutine("SetAudioDelayed", audioObj1);
            FindObjectOfType<AudioManager>().Play("Music2");
            FindObjectOfType<AudioManager>().StopPlaying("Music1");
            Debug.Log("Flag1");
            Destroy(col.gameObject);
        }

        if(col.gameObject.tag == "flag2"){
            //musicAnim2.SetTrigger("fadeOut");
            //StartCoroutine("SetAudioDelayed", audioObj2);
            FindObjectOfType<AudioManager>().Play("Music3");
            FindObjectOfType<AudioManager>().StopPlaying("Music2");
            Debug.Log("Flag2");
            Destroy(col.gameObject);
        }

        if(col.gameObject.tag == "flag3"){
            //musicAnim3.SetTrigger("fadeOut");
            //StartCoroutine("SetAudioDelayed", audioObj3);
            //FindObjectOfType<AudioManager>().Play("Music4");
            //FindObjectOfType<AudioManager>().StopPlaying("Music3");
            Debug.Log("Flag1");
        }

        if(col.gameObject.tag == "flag4"){
            //musicAnim3.SetTrigger("fadeOut");
            //StartCoroutine("SetAudioDelayed", audioObj3);
            //FindObjectOfType<AudioManager>().StopPlaying("Music4");
            Debug.Log("Flag1");
        }

        if(col.gameObject.tag == "Platform"){
            onPlatform = true;
            platform = col.gameObject;
        }


    }





}

 

