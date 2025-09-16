using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PacStudent/Audio Palette", fileName = "AudioPalette")]
public class AudioPalette : ScriptableObject
{
  
    public AudioClip bgmIntro;
    public AudioClip bgmStartScene;
    public AudioClip bgmGhostsNormal;
    public AudioClip bgmGhostsScared;
    public AudioClip bgmGhostsDead;

  
    public AudioClip sfxMoveNoPellet;
    public AudioClip sfxEatPellet;
    public AudioClip sfxHitWall;
    public AudioClip sfxDeath;
}
