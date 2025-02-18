using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WallScript : MonoBehaviour
{
    public float moveDistance = 5f; // Duvarın hareket mesafesi
    public float duration = 1f;     // Hareket süresi
    public float speed = 5;         // Hareket hızı
    private Vector3 startPosition;  // Başlangıç pozisyonu
    private Tween wallTween;        // Tween animasyonunu takip etmek için

    void OnEnable()
    {
        EventManager.Instance.OnPlayerGotDamaged += WallStartPosition;
    }

    void OnDisable()
    {
        EventManager.Instance.OnPlayerGotDamaged -= WallStartPosition;
    }

    void Start()
    {
        startPosition = transform.position;
        MoveWall(); // Başlangıçta hareketi başlat
    }

    private void MoveWall()
    {
        // Önceki tween'i durdur ve yeni hareketi başlat
        wallTween?.Kill();
        wallTween = transform.DOMoveX(startPosition.x + moveDistance, duration * speed);
    }

    private void WallStartPosition()
    {
        wallTween?.Kill(); // Aktif animasyonu durdur
        transform.position = startPosition; // Pozisyonu sıfırla
        MoveWall(); // Hareketi yeniden başlat
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EventManager.Instance.EnemyAttacked();
        }
    }
}