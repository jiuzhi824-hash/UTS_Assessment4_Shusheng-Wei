using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDirector : MonoBehaviour
{
    public AudioPalette palette;

  
    public AudioSource bgmSource;       
    public AudioSource moveSource;      
    public AudioSource sfxSource;      

    float introTimer = 0f;
    bool switched = false;
    private bool isMoving = false;

    void Reset()
    {
        
        bgmSource = GetOrAddAudioSource("BGM Source");
        moveSource = GetOrAddAudioSource("Move Source");
        sfxSource = GetOrAddAudioSource("SFX Source");
    }

  
    private AudioSource GetOrAddAudioSource(string name)
    {
        AudioSource source = transform.Find(name).GetComponent<AudioSource>();
        if (source == null)
        {
            GameObject obj = new GameObject(name);
            obj.transform.parent = transform;
            source = obj.AddComponent<AudioSource>();
        }
        return source;
    }

    void Start()
    {
        if (palette == null)
        {
            
            return;
        }

        
        PlayBgm();
    }

    void Update()
    {
        if (bgmSource == null || palette == null || switched) return;

        introTimer += Time.deltaTime;

        
        if (!bgmSource.isPlaying || introTimer >= 3f)
        {
            switched = true;
            SwitchToNormalBgm();
        }
    }

   
    public void PlayBgm()
    {
        if (bgmSource != null && palette.bgmIntro != null)
        {
            bgmSource.loop = false;
            bgmSource.clip = palette.bgmIntro;
            bgmSource.Play();
        }
    }

    
    public void SwitchToNormalBgm()
    {
        if (bgmSource != null && palette.bgmGhostsNormal != null)
        {
            bgmSource.Stop();
            bgmSource.loop = true;
            bgmSource.clip = palette.bgmGhostsNormal;
            bgmSource.Play();
        }
    }

   
    public void PlayMoveSound(bool hasPellet = false)
    {
        if (moveSource == null) return;

       
        AudioClip clip = hasPellet ? palette.sfxEatPellet : palette.sfxMoveNoPellet;
        if (clip == null)
        {         
            return;
        }

       
        if (!moveSource.isPlaying || moveSource.clip != clip)
        {
            moveSource.loop = true;
            moveSource.clip = clip;
            moveSource.Play();
        }

        isMoving = true;
    }

   
    public void StopMoveSound()
    {
        if (moveSource != null && moveSource.isPlaying)
        {
            moveSource.Stop();
        }

        isMoving = false;
    }

    
    public void PlayHitWallSound()
    {
        PlayOneShotSound(palette.sfxHitWall);
    }

   
    public void PlayDeathSound()
    {
        PlayOneShotSound(palette.sfxDeath);
        
        StopMoveSound();
    }

    
    public void PlayEatPelletSound()
    {
        PlayOneShotSound(palette.sfxEatPellet);
    }

   
    private void PlayOneShotSound(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
          
        }
    }

   
    public void SwitchToScaredBgm()
    {
        if (bgmSource != null && palette.bgmGhostsScared != null)
        {
            bgmSource.clip = palette.bgmGhostsScared;
            if (!bgmSource.isPlaying) bgmSource.Play();
        }
    }

   
    public void SwitchToGhostDeadBgm()
    {
        if (bgmSource != null && palette.bgmGhostsDead != null)
        {
            bgmSource.clip = palette.bgmGhostsDead;
            if (!bgmSource.isPlaying) bgmSource.Play();
        }
    }
}