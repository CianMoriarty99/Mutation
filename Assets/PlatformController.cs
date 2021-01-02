using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    Rigidbody2D rb;
    public Transform pos1, pos2, startPos;
    public float platSpeed;
    public Vector3 nextPos, currentDirection;

    // Start is called before the first frame update
    void Start()
    {
        nextPos = startPos.position;
    }

    // Update is called once per frame
    void Update()
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
}
