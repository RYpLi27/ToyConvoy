using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {
    public static List<ObjectPool> objectPools = new();

    private GameObject objectPoolEmptyHolder;
    private static GameObject projectileEmpty;
    private static GameObject effectsEmpty;
    private static GameObject enemyEmpty;
    private static GameObject turretEmpty;

    public enum PoolingParent {
        Projectile,
        Enemy,
        Effect,
        Turret,
        none
    }

    private void Awake() {
        SetupEmpties();
    }

    private void SetupEmpties() {
        objectPoolEmptyHolder = new GameObject("Pooled Objects");
        
        enemyEmpty = new GameObject("Enemies");
        enemyEmpty.transform.SetParent(objectPoolEmptyHolder.transform);
        
        projectileEmpty = new GameObject("Projectiles");
        projectileEmpty.transform.SetParent(objectPoolEmptyHolder.transform);
        
        effectsEmpty = new GameObject("Effects");
        effectsEmpty.transform.SetParent(objectPoolEmptyHolder.transform);
        
        turretEmpty = new GameObject("Turrets");
        turretEmpty.transform.SetParent(objectPoolEmptyHolder.transform);
    }
    
    ///<summary>
    ///Parents object to pooling parent
    ///</summary>
    public static GameObject SpawnObject(GameObject obj, Vector3 spawnPosition, Quaternion spawnRotation, PoolingParent parent = PoolingParent.none) {
        ObjectPool pool = objectPools.Find(p => p.objectName == obj.name) ?? CreatePool(obj.name);

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObj == null) {
            GameObject parentObject = SetParentObject(parent);
            
            spawnableObj = Instantiate(obj, spawnPosition, spawnRotation);
            if(parentObject != null) spawnableObj.transform.SetParent(parentObject.transform);
        }
        else {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }
    
    ///<summary>
    ///Parents object manually
    ///</summary>
    public static GameObject SpawnObject(GameObject obj, Transform parent) {
        string objectName = obj.name.Replace("(Clone)", "");
        ObjectPool pool = objectPools.Find(p => p.objectName == objectName) ?? CreatePool(objectName);

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObj == null) {
            spawnableObj = Instantiate(obj, parent);
        }
        else {
            spawnableObj.transform.SetParent(parent);
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }
    
    public static void ReturnObjectToPool(GameObject obj, float t = 0) {
        string objectName = obj.name.Replace("(Clone)", "");

        ObjectPool pool = objectPools.Find(p => p.objectName == objectName);

        obj.SetActive(false);
        if (pool == null) {
            // Debug.LogWarning($"Object: {obj} is not pooled.");
        } else {
            pool.InactiveObjects.Add(obj);
        }
    }

    private static ObjectPool CreatePool(string objName) {
        ObjectPool pool = new ObjectPool() {objectName = objName};
        objectPools.Add(pool);

        return pool;
    }

    private static GameObject SetParentObject(PoolingParent parent) {
        switch (parent) {
            case PoolingParent.Enemy:
                return enemyEmpty;
            
            case PoolingParent.Projectile:
                return projectileEmpty;
            
            case PoolingParent.Effect:
                return effectsEmpty;
            
            case PoolingParent.Turret:
                return turretEmpty;
            
            case PoolingParent.none:
            default:
                return null;
        }
    }
}


public class ObjectPool {
    public string objectName;
    public List<GameObject> InactiveObjects = new();
}