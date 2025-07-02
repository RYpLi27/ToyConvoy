using UnityEngine;
using FMODUnity;

public class PlayOneShot : MonoBehaviour
{
    [SerializeField]
    [EventRef]
    private string soundEvent = null;

    public void PlaySoundEvent()
    {
        if (soundEvent != null)
        {
            RuntimeManager.PlayOneShot(soundEvent);
        }
    }
}
