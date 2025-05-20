using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RunTrail : MonoBehaviour {
    [SerializeField] private List<TrailRenderer> trails;
    [SerializeField] private float runTrailTime;
    [SerializeField] private float walkTrailTime;

    public void EnableTrail() {
        DOTween.Kill("DisableTrail");
        foreach (TrailRenderer trail in trails) {
            trail.enabled = true;
            
            Sequence sequence = DOTween.Sequence().SetId("EnableTrail");
            sequence.Append(trail.DOTime(runTrailTime, .5f)).SetEase(Ease.InOutSine);
            sequence.Play();
        }
    }

    public void DisableTrail() {
        DOTween.Kill("EnableTrail");
        foreach (TrailRenderer trail in trails) {
            Sequence sequence = DOTween.Sequence().SetId("DisableTrail");
            sequence.Append(trail.DOTime(walkTrailTime, .5f)).SetEase(Ease.InOutSine);
            sequence.AppendCallback(() => { trail.enabled = false; });
            sequence.Play();
        }
    }
}
