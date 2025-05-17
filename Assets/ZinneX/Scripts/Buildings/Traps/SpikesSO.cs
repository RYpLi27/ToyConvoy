using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New SpikesSO", menuName = "Scriptable Objects/Traps/Spikes")]
public class SpikesSO : BuildingSO {
    public float damagePerSecond;
    
    [Title("Preferably between 12-36")] 
    public float hitInstancesPerSecond;
    
    public Material objectMaterial;
}
