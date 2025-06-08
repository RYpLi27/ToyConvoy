using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    [SerializeField] public MeshRenderer skinnedMesh;
    [SerializeField] public float dissolveRate = 0.0125f;
    [SerializeField] public float refreshRate = 0.025f;

    [SerializeField] private Material[] skinnedMaterials;

    void Start()
    {
        if (skinnedMesh != null)
            skinnedMaterials = skinnedMesh.materials;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(DissolveCo());
        }
    }

    IEnumerator DissolveCo()
    {
        if (skinnedMaterials.Length > 0)
        {
            float counter = 0;
            while (skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }

        }
    }
}
