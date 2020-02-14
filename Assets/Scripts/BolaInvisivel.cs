using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BolaInvisivel : MonoBehaviour
{
    public float ballSpeed;
    public float maxAngle;
    public float increment;
    public GameObject raqueteEsq;
    
    private Rigidbody2D rigBody;
    
    


    void Start()
    {
        rigBody = gameObject.GetComponent<Rigidbody2D>();         
        rigBody.velocity = new Vector2(ballSpeed * Time.deltaTime,0);                
    }

    void OnTriggerEnter2D(Collider2D col) 
    {   
        if(col.gameObject.tag == "PosTrigger")
        {
            raqueteEsq.GetComponent<RaqueteAI>().desiredY = this.transform.position.y; 
            rigBody.velocity = new Vector2(0,0);            
        }   
    }

    void OnCollisionEnter2D(Collision2D col) // * Colisões com paredes
    {
        if(col.gameObject.tag == "ParedeEsq")
        {
            rigBody.velocity = new Vector2(0,0);       
        }

        if(col.gameObject.tag == "ParedeDir")
        {
            rigBody.velocity = new Vector2(0,0);          
        }
    }

   
    public void SendInvisibleBall(float x,float y, float xPos, float yPos)
    {        
        rigBody.transform.position = new Vector2(xPos,yPos);
        rigBody.velocity = new Vector2(x * Time.deltaTime * ballSpeed, y * Time.deltaTime * ballSpeed);
    }

}
