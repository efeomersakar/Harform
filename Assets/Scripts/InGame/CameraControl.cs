using UnityEngine;
using DG.Tweening; // DOTween kütüphanesi

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private float smoothTime = 0.5f;

    private void LateUpdate()
    {
        if (Target == null) return; 

        transform.DOKill();

        transform.DOMove(
            new Vector3(Target.position.x, Target.position.y, transform.position.z),
            smoothTime 
        );
    }
}
