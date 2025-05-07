using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/Weapon SO")]
public class WeaponSO : ScriptableObject
{
    public float fireRate;
    public float damage;
    public float weaponRange;
    public float verticalRecoil;
    public float horizontalRecoil;
    public int numberOfBullets = 1;
    public GameObject bulletTrialPrefab;
    public GameObject bulletHitPrefab;
}
