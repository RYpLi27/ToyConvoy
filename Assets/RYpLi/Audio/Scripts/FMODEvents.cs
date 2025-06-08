using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{

    [field: Header("Shoot")]
    [field: SerializeField] public EventReference shotFired { get; private set; }

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
