using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public Vector3 Direction;

    private Rigidbody rigidBody;
   
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Move()
    {
        Direction.x = Input.GetAxisRaw("Horizontal");
        Direction.z = Input.GetAxisRaw("Vertical");
        Direction.Normalize();
        Direction *= Speed * Time.fixedDeltaTime;
        rigidBody.MovePosition(rigidBody.position + Direction);
    }
}
