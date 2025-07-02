using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    [SerializeField] private Music music;
    
    private void Start() {
        EventInstance musicEvent = music switch {
            Music.menu => AudioManager.instance.CreateEventInstance(FMODEvents.instance.menuMusic),
            Music.game => AudioManager.instance.CreateEventInstance(FMODEvents.instance.gameMusic),
            _ => AudioManager.instance.CreateEventInstance(FMODEvents.instance.menuMusic)
        };

        musicEvent.start();
        musicEvent.release();
    }

    public enum Music {
        menu,
        game
    }
}
