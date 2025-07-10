using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/Weapon SO")]
public class WeaponSO : ScriptableObject
{
    [LabelText("FireRate (Attacks per second)")] public float fireRate;
    public float damage;
    public float weaponRange;
    public float verticalRecoil;
    public float horizontalRecoil;
    public float cameraVerticalRecoil;
    public float cameraHorizontalRecoil;
    public int numberOfBullets = 1;
    public GameObject bulletTrialPrefab;
    public GameObject bulletHitPrefab;
    public GameObject muzzleFlashPrefab;
    public int soundID;
}
