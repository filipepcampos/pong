using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaqueteAI : MonoBehaviour
{  
    public float moveSpeed;
    public GameObject ball;
    public GameObject bolaInvisivel;
    public float offset;

    private Rigidbody2D rigBody;
    private Rigidbody2D ballRigBody;

    [SerializeField]
    public float desiredY;

    private float diffY;
    private float simulatedInput;    
    
    void Start()
    {
        PlayerPrefs.SetString("Difficulty","hard"); // TODO: Implement in-Menu choosing 
        rigBody = gameObject.GetComponent<Rigidbody2D>();
        ballRigBody = ball.GetComponent<Rigidbody2D>();
        string difficulty = PlayerPrefs.GetString("Difficulty");

        if(difficulty == "easy")
        {
            bolaInvisivel.GetComponent<BolaInvisivel>().ballSpeed = 1300;
            offset = 0.1f;
        }             

        if(difficulty == "hard")
        {
            bolaInvisivel.GetComponent<BolaInvisivel>().ballSpeed = 3000;
            offset = 0f;
        }      
    }

    void Update()
    {        
        diffY = -(gameObject.transform.position.y - desiredY);

        if(Mathf.Abs(diffY + offset) < 0.3)
        {
            simulatedInput = 0;
        }

        else
        {
            simulatedInput =  Mathf.Sign(diffY);
        }
        
        rigBody.velocity = new Vector2(rigBody.velocity.x, moveSpeed * Time.deltaTime * 
          simulatedInput);
    }
}

