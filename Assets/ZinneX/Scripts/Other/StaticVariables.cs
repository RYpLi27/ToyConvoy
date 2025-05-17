using UnityEngine;

public class StaticVariables : MonoBehaviour {
    public static LayerMask whatIsEnemy = 1 << LayerMask.NameToLayer("Enemy");
    public static LayerMask whatIsRoad = 1 << LayerMask.NameToLayer("Road");
    public static LayerMask whatIsBuilding = 1 << LayerMask.NameToLayer("Turret");
}
