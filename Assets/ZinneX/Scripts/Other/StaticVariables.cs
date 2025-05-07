using UnityEngine;

public class StaticVariables : MonoBehaviour {
    public static LayerMask whatIsEnemy = 1 << LayerMask.NameToLayer("Enemy");
}
