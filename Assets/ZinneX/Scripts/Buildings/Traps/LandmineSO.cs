using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New LandmineSO", menuName = "Scriptable Objects/Traps/Landmine")]
public class LandmineSO : BuildingSO {
    public List<TurretStats> turretStats;
    public Material objectMaterial;

    [System.Serializable]
    public class TurretStats {
        public float damage;
        public float explosionRange; // THIS IS RANGE OF DAMAGE HITBOX - ACTIVATION HITBOX IS TRIGGER COLLIDER
        public float timeToRespawn;
        public int upgradePrice;
    }
}
