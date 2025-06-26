using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MeshMerge : MonoBehaviour
{
    private void Start() {
        CombineInstance[] combine = new CombineInstance[transform.childCount];

        int index = 0;
        foreach (Transform child in gameObject.transform) {
            MeshFilter filter = child.GetComponent<MeshFilter>();
            Debug.Log(filter.mesh);
            combine[index].mesh = filter.sharedMesh;
            combine[index].transform = transform.worldToLocalMatrix * child.localToWorldMatrix;
            child.gameObject.SetActive(false);
            index++;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        transform.gameObject.SetActive(true);
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
