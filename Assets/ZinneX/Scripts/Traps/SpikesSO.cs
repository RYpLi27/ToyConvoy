using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpikesSO", menuName = "Scriptable Objects/Traps/Spikes")]
public class SpikesSO : ScriptableObject {
    public float damagePerSecond;
    
    [InfoBox("Preferably between 12-36 to avoid calling the method too often but still maintain smooth hp loss")] 
    public float hitInstancesPerSecond;
}
