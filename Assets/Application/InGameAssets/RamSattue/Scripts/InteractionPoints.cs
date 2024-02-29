using Cinemachine;
using Satyug.HardMode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class InteractionPoints : MonoBehaviour
{
    public List<Areas> InteractionAreas = new List<Areas>();
    public float StepTime;
    [HideInInspector] private int stepIndex = 0;

    [SerializeField] UnityEvent onCompleteEvent;
    [SerializeField] AudioClip completeAudioClip;

    private int totalSpawnElements;

    public void DisableAllInteraction()
    {
        foreach (Areas area in InteractionAreas)
        {
                area.InteractionObjects.GetComponent<Collider>().enabled = false;
        }
    }

    public void InteractionGO(bool value)
    {
        foreach(Areas area in InteractionAreas) 
        {
            if(!area.InteractionObjects.activeSelf)
                area.InteractionObjects.SetActive(value);
        }
    }

    public void SpawnAccessories()
    {
        foreach (Areas area in InteractionAreas)
        {
            if(area.accessoryObject == StatueManager.StatueManagerInstance.GetSelectedAccessoryName())
            {
                totalSpawnElements++;
                area.InteractionObjects.GetComponent<Interactible>().StartBuilding();
                area.InteractionObjects.GetComponent<Collider>().enabled = false;
                area.button.interactable = false;
                //InteractionAreas.Remove(area);
            }
        }
        StatueManager.StatueManagerInstance.currentAttempt = 0;

        if (totalSpawnElements >= InteractionAreas.Count)
        {
            Invoke(nameof(complete), 3f);
        }

        
    }

    public void BuildAllPoints()
    {
        StartCoroutine(performBuildAnimation());
    }

    private IEnumerator performBuildAnimation()
    {
        for (int i = 0; i < InteractionAreas.Count; i++)
        {
            stepIndex = i;
            if (InteractionAreas[i].InteractionObjects.GetComponent<DissolveController>().enabled == false)
            {
                yield return new WaitForSeconds(StepTime);

                InteractionAreas[i].InteractionObjects.GetComponent<Interactible>().StartBuilding();
                //AudioSource.PlayClipAtPoint(completeAudioClip, Camera.main.transform.position,1f);
            }
        }

        if(stepIndex < InteractionAreas.Count) 
        {
            Invoke(nameof(complete), 5f);

            yield break;
        }
    }

    public static string videoURl = "https://dvf7opio6knc7.cloudfront.net/video_v2/Statue_Chakra.mp4";
    [SerializeField] GameObject VideoScreen;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] AudioSource audioPlayer;


    private void complete()
    {
        onCompleteEvent?.Invoke();

        //float audioLength = audioPlayer.clip.length;
        Invoke(nameof(PlayVideo), 2f);
    }

    private void PlayVideo()
    {
        VideoScreen.SetActive(true);

        videoPlayer.url = videoURl;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += PrepareVideo;
        videoPlayer.Play();
    }

    //Logout
    private void PrepareVideo(VideoPlayer videoPlayer)
    {
        float maxTime = (videoPlayer.frameCount / videoPlayer.frameRate);
        Invoke(nameof(Exit), maxTime);   
    }

    private void Exit()
    {
        APIManager.GenerateToken();
    }
}

[System.Serializable]
public struct Areas
{
    public string ID;
    public Accessories accessoryObject;
    public GameObject InteractionObjects;
    public CinemachineVirtualCamera FocusCamera;
    public Button button;
}