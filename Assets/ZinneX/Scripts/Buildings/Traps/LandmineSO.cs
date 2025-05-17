using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New LandmineSO", menuName = "Scriptable Objects/Traps/Landmine")]
public class LandmineSO : BuildingSO {
    public float damage;
    public float explosionRange; // THIS IS RANGE OF DAMAGE HITBOX - ACTIVATION HITBOX IS TRIGGER COLLIDER
    public Material objectMaterial;
    public Material usedMat;
}
