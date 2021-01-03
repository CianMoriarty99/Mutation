using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject audioObj1, audioObj2, audioObj3, platform;

    Rigidbody2D rb;
    public float speed, refSpeed, jumpForce, dashSpeed, xRaw, yRaw, x, y, defaultGrav, stepCD, climbCD;

    public bool walk, onGround, onLeftWall, onRightWall, onPlatform, climbing, climbingAquired, dashing, canDash, dashAquired, wallJumpAquired, doubleJump, doubleJumpAquired, sprinting;

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;

    public LayerMask groundLayer;

    SpriteRenderer spriteRef;

    public Animator animator, musicAnim1, musicAnim2, musicAnim3;

    public Vector3 checkpoint;

    public int note;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRef = GetComponent<SpriteRenderer>();
        //dashAquired = false;
        refSpeed = speed;
        defaultGrav = rb.gravityScale;
        checkpoint = this.transform.position;
        climbingAquired = false;
        dashAquired = false;
        wallJumpAquired = false;
        doubleJumpAquired = false;
        note = 0;
        

              
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
       


        if (rb.velocity.y < -0.01 && !onGround && !climbing){
            animator.SetBool("Falling", true);
            animator.SetBool("Jumping", false);
            
        }
        else{
            animator.SetBool("Falling", false);
        }

        if (rb.velocity.x < -0.01){
            spriteRef.flipX = true;

            if(onGround)
            {

                if(stepCD < 0){


                    int r = Random.Range(1,5);
                    FindObjectOfType<AudioManager>().Play("Step"+r);

                    if(sprinting)
                    {
                        stepCD = 0.2f;
                    }
                    else 
                    {
                        stepCD = 0.6f;
                    }   
                        
                }
            }
        }

    
        
        if (rb.velocity.x > 0.01) {
            spriteRef.flipX = false;

            if(onGround)
            {
                if(stepCD < 0){

                    int r = Random.Range(1,5);
                    FindObjectOfType<AudioManager>().Play("Step"+r);

                    if(sprinting)
                    {
                        stepCD = 0.2f;
                    }
                        
                    else
                    {
                        stepCD = 0.6f;
                    }    
                        
                }
            }

        }

        stepCD -= Time.deltaTime;
        


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
            sprinting = true;
            animator.SetBool("Sprinting", true);
            speed = refSpeed * 2;

        }
        else
        {
            sprinting = false;
            speed = refSpeed;
            animator.SetBool("Sprinting", false);
        }


        if(onLeftWall || onRightWall )
        {
            animator.SetBool("OnWall", true);
            canDash = true;
            if(climbingAquired)
            {

                    if(climbCD < 0  && rb.velocity.y > 0){
                        FindObjectOfType<AudioManager>().Play("Wall");

                        if(sprinting)
                        {
                            climbCD = 0.2f;
                        }
                        else 
                        {
                            climbCD = 0.6f;
                        }   
                        
                }

                


                climbing = true;
                animator.SetBool("Climbing", true);
                
                Climb(dir);
            }
            else{

                if(onLeftWall){
                    Debug.Log("LEft");
                    rb.velocity = new Vector2(10, rb.velocity.y);
                }
                else{
                    Debug.Log("Right");
                    rb.velocity = new Vector2(-10, rb.velocity.y);
                }
                
            }


        }

        else 
        {
            animator.SetBool("OnWall", false);
            climbing = false;
            animator.SetBool("Climbing", false);
        }

        climbCD -= Time.deltaTime;
    

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
            int r = Random.Range(1,3);
            FindObjectOfType<AudioManager>().Play("Jump"+r);
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
            animator.SetBool("Jumping", true);
            int r = Random.Range(1,3);
            FindObjectOfType<AudioManager>().Play("Jump"+r);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += jumpForce * Vector2.up;
            doubleJump = false;
        }

    }





    IEnumerator Dash(Vector2 rawDir){
        rb.gravityScale = 0;
        rb.drag = 10f;
        FindObjectOfType<AudioManager>().Play("Dash");
        rb.velocity = rawDir.normalized * new Vector2(dashSpeed, dashSpeed/1.3f);
        yield return new WaitForSeconds(0.3f);
        rb.gravityScale = defaultGrav;
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

    IEnumerator SetAudioDelayed(string music){
        yield return new WaitForSeconds(1f);
        FindObjectOfType<AudioManager>().Play(music);

    }

    void OnTriggerEnter2D(Collider2D col){

        if(col.gameObject.tag == "flag1"){
            checkpoint = this.transform.position;
            StartCoroutine("SetAudioDelayed", "Music2");
            StartCoroutine(FindObjectOfType<AudioManager>().StartFade("Music1", 1f));
            dashAquired = true;
            
            Debug.Log("Flag1");
            Destroy(col.gameObject);
        }

        if(col.gameObject.tag == "flag2"){
            checkpoint = this.transform.position;
            StartCoroutine("SetAudioDelayed", "Music3");
            StartCoroutine(FindObjectOfType<AudioManager>().StartFade("Music2", 1f));
            Debug.Log("Flag2");
            doubleJumpAquired = true;
            Destroy(col.gameObject);
        }

        if(col.gameObject.tag == "flag3"){
            checkpoint = this.transform.position;
            climbingAquired = true;
            wallJumpAquired = true;
            StartCoroutine("SetAudioDelayed", "Music4");
            StartCoroutine(FindObjectOfType<AudioManager>().StartFade("Music3", 1f));
            Debug.Log("Flag3");
            Destroy(col.gameObject);
        }

        if(col.gameObject.tag == "flag4"){
            //END THE GAME STUFF
            checkpoint = this.transform.position;
            StartCoroutine("SetAudioDelayed", "Ending");
            StartCoroutine(FindObjectOfType<AudioManager>().StartFade("Music4", 1f));
            Debug.Log("Flag4");
            Destroy(col.gameObject);
        }

        if(col.gameObject.tag == "Platform"){
            onPlatform = true;
            platform = col.gameObject;
        }

        if(col.gameObject.tag == "Acid")
        {
            FindObjectOfType<AudioManager>().Play("WaterBubble");
            rb.velocity = new Vector2(0,0);
            this.transform.position = checkpoint;
        }

        if(col.gameObject.tag == "Note"){
            checkpoint = this.transform.position;
            Debug.Log("noted");
            note++;
            Destroy(col.gameObject);

        }


    }





}

 

