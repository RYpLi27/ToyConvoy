using Sirenix.OdinInspector;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour {
    public static WeaponRecoil instance;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private float recoilY, recoilX;
    [SerializeField] [InfoBox("Between 0(no stop)-1(instant)")]
    private float recoilReturnSpeed;

    private void Update() {
        if (GameManager.gameState == GameManager.GameState.Pause) return;
        HandleRecoil();
    }

    private void HandleRecoil() {
        recoilY = Mathf.Lerp(recoilY, 0, .05f);
        recoilX = Mathf.Lerp(recoilX, 0, .05f);
        
        transform.localRotation = Quaternion.Lerp(transform.localRotation ,Quaternion.Euler(recoilX, recoilY, 0f), recoilReturnSpeed);
    }
    
    public void ApplyRecoil(float verticalRecoil, float horizontalRecoil) {
        recoilX += Random.Range(-verticalRecoil/2, -verticalRecoil);
        recoilY += Random.Range(-horizontalRecoil, horizontalRecoil);
    }
}
