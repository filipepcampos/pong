using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void Play()
    {   
        SceneManager.LoadScene("GameScene");
    }

    public void About()
    {   
        SceneManager.LoadScene("AboutScene");
    }
    
    public void Options()
    {
        SceneManager.LoadScene("OptionsScene");
    }
}
