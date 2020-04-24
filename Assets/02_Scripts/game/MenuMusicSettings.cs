using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicSettings : MonoBehaviour
{
    public GameObject buttonIconOn;
    public GameObject buttonIconOff;
     bool isMute;

    public void Start()
    {
                isMute = true;

    }

    public void MusicOnOff()
    {
        isMute = !isMute;
        if(isMute){
            
            Time.timeScale = 0;
            AudioManager.instance.MuteChannel(EAudioChannelType.Music, true);
            buttonIconOn.SetActive(true);
            buttonIconOff.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            AudioManager.instance.MuteChannel(EAudioChannelType.Music, false);
            buttonIconOn.SetActive(false);
            buttonIconOff.SetActive(true);

        }
            
    }
    public void SfxOnOff()
    {
        isMute = !isMute;
        if(isMute){
            
            Time.timeScale = 0;
            AudioManager.instance.MuteChannel(EAudioChannelType.Sfx, true);
            buttonIconOn.SetActive(true);
            buttonIconOff.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            AudioManager.instance.MuteChannel(EAudioChannelType.Sfx, false);
            buttonIconOn.SetActive(false);
            buttonIconOff.SetActive(true);

        }
            
    }
}
