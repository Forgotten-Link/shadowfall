using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource enemysfx;

    public AudioClip background;
    public AudioClip enemyDeath;
    public AudioClip swordSlash;
    public AudioClip swordStab;
    public AudioClip fireStream;
    public AudioClip fireBall;
    public AudioClip chomp;
    public AudioClip jump;
    public AudioClip victory;
    public AudioClip key;
    public AudioClip hurt;
    
    private void Start() {
        musicSource.clip = background;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip){
        sfxSource.clip = clip;
        sfxSource.Play();
    }
    public void PlayEnemySFX(AudioClip clip){
        enemysfx.clip = clip;
        enemysfx.Play();
    }
}
