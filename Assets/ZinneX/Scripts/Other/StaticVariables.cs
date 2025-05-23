using UnityEngine;

public static class StaticVariables {
    public static LayerMask whatIsEnemy = 1 << LayerMask.NameToLayer("Enemy");
    public static LayerMask whatIsRoad = 1 << LayerMask.NameToLayer("Road");
    public static LayerMask whatIsBuilding = LayerMask.GetMask("Turret", "EnvironmentalTrap");
    public static LayerMask whatIsGround = 1 << LayerMask.NameToLayer("Ground");
    public static LayerMask whatIsInteractable = 1 << LayerMask.NameToLayer("InteractTrigger");
}
