using UnityEngine;

[CreateAssetMenu(fileName = "New LandmineSO", menuName = "Scriptable Objects/Traps/Landmine")]
public class LandmineSO : ScriptableObject {
    public float damage;
    public float explosionRange; // THIS IS RANGE OF DAMAGE HITBOX - ACTIVATION HITBOX IS TRIGGER COLLIDER
    public Material objectMaterial;
}
