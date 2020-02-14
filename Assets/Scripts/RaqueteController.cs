using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class RaqueteController : MonoBehaviour
{   
    public float moveSpeed;
    public GameObject ParticleSystem;

    [SerializeField]
    public float ballY;

    private Rigidbody2D rigBody;
    private float x;

    // Start is called before the first frame update
    void Start()
    {
        rigBody = gameObject.GetComponent<Rigidbody2D>();
        x = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        rigBody.velocity = new Vector2(rigBody.velocity.x, moveSpeed * Time.deltaTime * 
          CrossPlatformInputManager.GetAxisRaw("Vertical"));         
      
    }
}
