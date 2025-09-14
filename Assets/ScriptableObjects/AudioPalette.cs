// Assets/Scripts/Audio/AudioPalette.cs
using UnityEngine;

[CreateAssetMenu(menuName = "PacStudent/Audio Palette", fileName = "AudioPalette")]
public class AudioPalette : ScriptableObject
{
    [Header("BGM")]
    public AudioClip bgmIntro;
    public AudioClip bgmStartScene;
    public AudioClip bgmGhostsNormal;
    public AudioClip bgmGhostsScared;
    public AudioClip bgmGhostsDead;

    [Header("SFX")]
    public AudioClip sfxMoveNoPellet;
    public AudioClip sfxEatPellet;
    public AudioClip sfxHitWall;
    public AudioClip sfxDeath;
}
