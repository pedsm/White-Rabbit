using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public Vector3 offset;
    public float downOffset = 2f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       float y = Input.GetAxisRaw("Vertical");
       float yDiff = downOffset * y;

       transform.position = new Vector3(
           player.position.x + offset.x,
           player.position.y + offset.y + yDiff,
           offset.z
        );
    }
}
