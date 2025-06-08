using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class DeathAnim : MonoBehaviour {
    [SerializeField] private float dissolveTime;
    
    public void PlayAnim() {
        List<Material> materials = GetComponentsInChildren<MeshRenderer>().SelectMany(rend => rend.materials).ToList();

        if (materials.Count == 0) {
            materials = GetComponentsInChildren<SkinnedMeshRenderer>().SelectMany(rend => rend.materials).ToList();
        }
        
        Sequence sequence = DOTween.Sequence();
        
        foreach (Material material in materials) {
            sequence.Join(
                DOTween.To(
                    () => material.GetFloat("_DissolveAmount"),
                    value => material.SetFloat("_DissolveAmount", value),
                    1f,
                    dissolveTime
                )
            );
        }

        sequence.OnComplete(() => {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
            foreach(Material material in materials) material.SetFloat("_DissolveAmount", 0);
        });

        sequence.Play();
    }
}
