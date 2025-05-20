using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New SpikesSO", menuName = "Scriptable Objects/Traps/Spikes")]
public class SpikesSO : BuildingSO {
    public List<TurretStats> turretStats;
    
    [Title("Preferably between 12-36")] 
    public float hitInstancesPerSecond;
    public Material objectMaterial;

    [System.Serializable]
    public class TurretStats {
        public float damagePerSecond;
        public int upgradePrice;
    }
}
