using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuggestionMusicUIController : MonoBehaviour
{
    bool IsPuse;
    public GameObject playBtn;
    public GameObject PauseBtn;

    // Use this for initialization
    void Start()
    {
        IsPuse = false;

    }
    
    // for stop and run applacation
    public void Pause()
    {
        IsPuse = !IsPuse;
        if (IsPuse)
        {
            Time.timeScale = 0;
            playBtn.SetActive(true);
            PauseBtn.SetActive(false);

        }
        else if (!IsPuse)
        {
            Time.timeScale = 0;
            playBtn.SetActive(false);
            PauseBtn.SetActive(true);
        }
    }
}
