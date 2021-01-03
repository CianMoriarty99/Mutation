using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    Rigidbody2D rb;
    public Transform pos1, pos2, startPos;
    public float platSpeed;
    public Vector3 nextPos, currentDirection;
    SpriteRenderer spriteRef;
    //public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        nextPos = pos2.position;
        spriteRef = GetComponent<SpriteRenderer>();

        //spriteRef.sprite = sprites[Random.Range(0,sprites.Length - 1)];

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position == pos1.position)
        {
            nextPos = pos2.position;
        }

        if(transform.position == pos2.position)
        {
            nextPos = pos1.position;
        }

        currentDirection = (nextPos - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, nextPos, platSpeed);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player"){
            col.transform.parent = transform;
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player"){
            col.transform.parent = null;
        }
        
    }
}
