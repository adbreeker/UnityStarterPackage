using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public GameObject audioPrefab;
    public AudioClip[] audioClips;


    public void PlaySound(int soundId)
    {
        try
        {
            AudioClip audioClip = audioClips[soundId];
            if(audioClip != null)
            {
                GameObject temp = Instantiate(audioPrefab);
                temp.GetComponent<AudioSource>().clip = audioClip;
                temp.GetComponent<AudioSource>().Play();
                StartCoroutine(destroySource(temp));
            }
        }
        catch
        {
            Debug.Log("SoundManager: Something went wrong");
        }
    }
    IEnumerator destroySource(GameObject temp)
    {
        yield return new WaitForSecondsRealtime(temp.GetComponent<AudioSource>().clip.length);
        Destroy(temp);
    }
}
