using System.Collections;
using System.Collections.Generic;
using UnityEngine;



using UnityEngine;
using UnityEngine.UI;

public class PlayMovie : MonoBehaviour
{

    public GameObject videoPlayer;
    public int timeToStop;
    public GameObject PromptCanvas;
    public bool verify = false;
    void Start()
    { 
          PromptCanvas.SetActive(false);
        videoPlayer.SetActive(true);
       timeToStop = 64;
       
        
    }

    void OnTriggerEnter(Collider Player)
    {
        if (Player.gameObject.tag == "Player")
        {
            PromptCanvas.SetActive(true);
            videoPlayer.SetActive(true);
           
            Destroy(videoPlayer, timeToStop);
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        
          //  videoPlayer.SetActive(false);
          
        videoPlayer.SetActive(false);
        PromptCanvas.SetActive(false);
        timeToStop += 10;


    }
}
