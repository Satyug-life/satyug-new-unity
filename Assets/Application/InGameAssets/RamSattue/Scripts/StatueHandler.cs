using Satyug.HardMode;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StatueHandler : MonoBehaviour
{
    [SerializeField] UnityEvent onIntiailizeEvent;
    [SerializeField] UnityEvent onStartEvent;

    [SerializeField] AudioHandler audioHandler;

    public void Initialized()
    {
        onIntiailizeEvent?.Invoke();
        audioHandler.audioSource = this.GetComponent<AudioSource>();
        audioHandler.audioSource.clip = audioHandler.sound[0].clip;
        audioHandler.audioSource.volume = audioHandler.sound[0].volume;
        audioHandler.audioSource.Play();

        StartCoroutine(waitForVoiceEnd(audioHandler.audioSource.clip.length));
        StartCoroutine(playTimeSound());
    }

    IEnumerator waitForVoiceEnd(float timer)
    {
        yield return new WaitForSeconds(timer + 3f);
        
        SetSound(1);
        onStartEvent?.Invoke();
    }

    public void SetSound(int index)
    {
        audioHandler.audioSource.clip = audioHandler.sound[index].clip;
        audioHandler.audioSource.Play();
    }

    public void StopSound()
    {
        audioHandler.audioSource.Stop();
    }

    private float totalTime = 30f;

    private IEnumerator playTimeSound()
    {
        while (StatueManager.StatueManagerInstance.isGameStart)
        {
            yield return null;

            yield return new WaitForSeconds(totalTime);
            if (!audioHandler.audioSource.isPlaying)
                SetSound(6);
        }

        yield break;
    }
}

[System.Serializable]
public struct AudioHandler
{
    public AudioSource audioSource;
    public Sound[] sound;
}

[System.Serializable]
public struct Sound
{
    public string name;
    public AudioClip clip;
    public float volume;
    public bool isLoop;
}