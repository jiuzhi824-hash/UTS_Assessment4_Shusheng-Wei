// Assets/Scripts/Audio/AudioDirector.cs
using UnityEngine;

[DisallowMultipleComponent]
public class AudioDirector : MonoBehaviour
{
    public AudioPalette palette;
    public AudioSource source;

    float introTimer = 0f;
    bool switched = false;

    void Reset()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (palette == null || source == null) return;
        source.loop = false;
        source.clip = palette.bgmIntro;
        source.Play();
    }

    void Update()
    {
        if (source == null || palette == null || switched) return;

        introTimer += Time.deltaTime;

        // “3 秒或播放结束，取先到”
        if (!source.isPlaying || introTimer >= 3f)
        {
            switched = true;
            source.Stop();
            source.clip = palette.bgmGhostsNormal;
            source.loop = true;
            source.Play();
        }
    }
}
