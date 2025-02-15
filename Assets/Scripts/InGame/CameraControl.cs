using UnityEngine;
using DG.Tweening; // DOTween kütüphanesi

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private float smoothTime = 0.5f;
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
      
        Vector3 targetPosition = new Vector3(Target.position.x, Target.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
