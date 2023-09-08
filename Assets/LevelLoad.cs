using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelLoad : MonoBehaviour
{
    // build index number
    public int iLevelToLoad;

    // be sure that buildIndexNum is set to largest build index number
    public int buildIndexNum;

    public GameObject levelStart;

    void Update()
    {
        //loads the next level (last level will load into the first level)
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (iLevelToLoad > buildIndexNum)
            {
                iLevelToLoad = 0;
            }
            SceneManager.LoadScene(iLevelToLoad);
            GameObject.DontDestroyOnLoad(this.gameObject);

            levelStart = GameObject.FindGameObjectWithTag("LevelStart");
                this.gameObject.transform.position = levelStart.transform.position;
            iLevelToLoad++;
        }
        }
    }
