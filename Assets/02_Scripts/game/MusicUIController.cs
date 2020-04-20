using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicUIController : MonoBehaviour
{
    bool IsPause;
    public GameObject playButton;
    public GameObject PauseButton;

    // Use this for initialization
    void Start()
    {
        IsPause = false;

    }
    
    // for stop and run applacation
    public void Pause ()
    {
        IsPause = !IsPause;
        if (IsPause)
        {
            Time.timeScale = 0;
            playButton.SetActive(true);
            PauseButton.SetActive(false);

        }
        else if (!IsPause)
        {
            Time.timeScale = 0;
            playButton.SetActive(false);
            PauseButton.SetActive(true);
        }
    }
}
