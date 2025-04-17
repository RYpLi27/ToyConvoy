using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ObjectPoolManager : MonoBehaviour {
    public static List<ObjectPool> objectPools = new();

    private GameObject objectPoolEmptyHolder;
    private static GameObject projectileEmpty;
    private static GameObject enemyEmpty;

    public enum PoolingParent {
        Projectile,
        Enemy,
        none
    }
    public static PoolingParent poolingParent;

    private void Awake() {
        SetupEmpties();
    }

    private void SetupEmpties() {
        objectPoolEmptyHolder = new GameObject("Pooled Objects");
        
        enemyEmpty = new GameObject("Enemies");
        enemyEmpty.transform.SetParent(objectPoolEmptyHolder.transform);
        
        projectileEmpty = new GameObject("Projectiles");
        projectileEmpty.transform.SetParent(objectPoolEmptyHolder.transform);
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
        ObjectPool pool = objectPools.Find(p => p.objectName == obj.name) ?? CreatePool(obj.name);

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


    public static void ReturnObjectToPool(GameObject obj) {
        string objectName = obj.name.Replace("(Clone)", "");

        ObjectPool pool = objectPools.Find(p => p.objectName == objectName);

        if (pool == null) {
            // Debug.LogWarning($"Object: {obj} is not pooled.");
            obj.SetActive(false);
        } else {
            obj.SetActive(false);
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