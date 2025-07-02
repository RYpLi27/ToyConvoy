using UnityEngine;
using FMODUnity;
using Sirenix.OdinInspector;

public class FMODEvents : MonoBehaviour
{
    //MUSIC
    [field: SerializeField] public EventReference menuMusic { get; private set; }
    [field: SerializeField] public EventReference gameMusic { get; private set; }
    
    //EFFECTS
    [field: SerializeField] public EventReference shotFired { get; private set; }
    [field: SerializeField] public EventReference gameOver { get; private set; }
    [field: SerializeField] public EventReference jump { get; private set; }
    [field: SerializeField] public EventReference lifeLost { get; private set; }
    [field: SerializeField] public EventReference ratCharge { get; private set; }
    [field: SerializeField] public EventReference ballistaShot { get; private set; }
    [field: SerializeField] public EventReference mortarBoom { get; private set; }
    [field: SerializeField] public EventReference cactusShot { get; private set; }
    [field: SerializeField] public EventReference steps { get; private set; }

    public static FMODEvents instance
    { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("too many FMOD Events scripts");
        }
        instance = this;
    }

}
