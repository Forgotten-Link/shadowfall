using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class PauseMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void SetVolume (float volume){
        audioMixer.SetFloat("Volume", volume);
        Debug.Log(volume);
    }
    public void QuitApp(){
        Application.Quit();
        Debug.Log("Application has quit.");
    }
}
