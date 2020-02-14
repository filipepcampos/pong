using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using TMPro;

public class Bola : MonoBehaviour
{
    public float ballSpeed;
    public float maxAngle;
    public float increment;

    [Header ("Game Objects")]
    public GameObject invisibleBall;
    public GameObject playerEsq;
    public GameObject playerDir;
    public GameObject continueText;

    [Header ("Particles")]
    public GameObject EsqParticleSystem;
    public GameObject DirParticleSystem;
    public GameObject EsqWinParticleSystem;
    public GameObject DirWinParticleSystem;  

    [Header ("Scores")]
    public float playerEsqScore;
    public float playerDirScore;
    public TMP_Text playerEsqText;
    public TMP_Text playerDirText;  



    private ParticleSystem dirParticle;
    private ParticleSystem esqParticle;

    private Animator EsqScoreWinAnimation;
    private Animator DirScoreWinAnimation;

    private AudioSource[] audios;
    private AudioSource audioWin;
    private AudioSource audioLose;
    private AudioSource audioHit;
    private AudioSource audioWall;

    private float angle; 
    private float xVel;
    private float yVel; 
    private float yDif;    
    private Rigidbody2D rigBody;

    private bool gameIsOver = false; 
    private bool gameStarted = false;  

    void Start()
    {
        rigBody = gameObject.GetComponent<Rigidbody2D>(); 
        dirParticle = DirParticleSystem.GetComponent<ParticleSystem>();
        esqParticle = EsqParticleSystem.GetComponent<ParticleSystem>();
        EsqScoreWinAnimation = playerEsqText.GetComponent<Animator>();
        DirScoreWinAnimation = playerDirText.GetComponent<Animator>();

        audios = gameObject.GetComponents<AudioSource>();
        audioHit = audios[0];  
        audioWin = audios[1];
        audioLose = audios[2];  
        audioWall = audios[3];                   
    }

    void Update()
    {
        if(!gameStarted)
        {           
            if(CrossPlatformInputManager.GetAxis("Vertical") != 0)
            {
                Debug.Log("got input");
                rigBody.velocity = new Vector2(ballSpeed * Time.deltaTime,0);
                gameStarted = true;
            }

        }

        if(gameIsOver)
        {
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("MenuScene");
            }
        }
    }
    

    void OnTriggerEnter2D(Collider2D col) // * Colisões com raquetes
    {       
        if(col.gameObject.tag == "Esq")
        {   
            audioHit.Play(0);
            esqParticle.Play();
            playerEsq.GetComponent<RaqueteAI>().desiredY = 0;
            yDif = -(col.gameObject.transform.position.y - this.gameObject.transform.position.y);            
            CalculateVel(yDif,1);                 
        }

        if(col.gameObject.tag == "Dir")
        {        
            audioHit.Play(0); 
            dirParticle.Play();    
            yDif = -(col.gameObject.transform.position.y - this.gameObject.transform.position.y);
            CalculateVel(yDif,-1);                                 
        }   
    }

    void OnCollisionEnter2D(Collision2D col) // * Colisões com paredes
    {
        if(col.gameObject.tag == "ParedeEsq")
        {
            playerDirScore += 1;
            playerDirText.text = playerDirScore.ToString();
            audioWin.Play(0);  
            Reset(); 
            VerifyWin();         
        }

        else if(col.gameObject.tag == "ParedeDir")
        {            
            playerEsqScore += 1;
            playerEsqText.text = playerEsqScore.ToString(); 
            audioLose.Play(0);
            Reset();
            VerifyWin();           
        }

        else
        {
            audioWall.Play(0);
        }
    }

    void CalculateVel(float yDif, float a) // * Calculo da velocidade da bola quando entra em contacto com raquete
    {       
        ballSpeed += increment; 

        if(yDif==0)
        {
            xVel = ballSpeed * a * Time.deltaTime;
            yVel = 0;
        }

        else
        {
            angle = yDif * maxAngle;
            xVel = a * ballSpeed * Time.deltaTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            yVel = ballSpeed * Time.deltaTime * Mathf.Sin(angle * Mathf.Deg2Rad);
        }

        if(a == -1)
        {
            invisibleBall.GetComponent<BolaInvisivel>().SendInvisibleBall(a * Mathf.Cos(angle*Mathf.Deg2Rad)
              ,Mathf.Sin(angle*Mathf.Deg2Rad),
              transform.position.x, transform.position.y);
        }

        rigBody.velocity = new Vector2(xVel,yVel);
    }

    

    void Reset() // * Reset posição da bola
    {
        this.gameObject.transform.position = new Vector2(0,0);
        ballSpeed = 300;
        rigBody.velocity = new Vector2(ballSpeed * Time.deltaTime, 0);           
    }

    void VerifyWin() // * Verificar se algum jogador ganhou
    {
        if(playerDirScore == 5)
        {
            DirWinParticleSystem.GetComponent<ParticleSystem>().Play();
            GameOver();                        
        }

        if(playerEsqScore == 5)
        {
            EsqWinParticleSystem.GetComponent<ParticleSystem>().Play();
            GameOver();  
        }
    }
    
    void GameOver()
    {
        Destroy(GameObject.FindWithTag("LinhaCentral"));
        EsqScoreWinAnimation.Play("EsqScoreGameOver");
        DirScoreWinAnimation.Play("DirScoreGameOver");
        playerEsq.GetComponent<Animator>().Play("RaqueteEsqGameOver");
        playerDir.GetComponent<Animator>().Play("RaqueteDirGameOver");
        continueText.GetComponent<Animator>().Play("ContinueText");

        rigBody.velocity = new Vector2(0,0);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        
        gameIsOver = true;          
    }
}
