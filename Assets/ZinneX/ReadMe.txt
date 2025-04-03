EnemyBehaviour => pathfinding, moving, OnDisable of enemy (called when enemy dies or gets to its destination)

EnemyPathManager *instance => method that returns nodes to EnemyBehaviour

EnemySO => scriptableobject that contains stats of enemies (can add some more if needed)

HealthManager => taking damage and dying

TurretBehaviour => managing enemies in range and firing at them (this is a script that will work on every turret type. for other types just assign different TurretSO extension (e.g. TurretSniperNest scriptableobject))

TurretSO => scriptable object for turrets that contain base stats, projectilePrefab and virtual method for shooting

TurretSniperNest => extension of TurretSO that contains shooting logic (if something have to happen when SniperNest is shooting then do it here)

SniperNestProjectile => assigning target to projectile, making it move to target and hitting it

/////////////////////////////////////////////////////////////////////////////////////////////////

To add another turret type you have to create new script that extends TurretSO and then assign it to turretSO variable in TurretBehaviour.
Also if the projectile behaviour of the new turret type differs from others then you also have to create MonoBehaviour script that handles it on its own.

As for new enemy type you just have to create new scriptable object (bottom of create asset menu) and assign it to EnemyBehaviour

If this didnt help you solve your problem then dm me @ZinneX.