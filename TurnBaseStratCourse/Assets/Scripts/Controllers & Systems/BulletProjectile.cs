using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BulletProjectile : MonoBehaviour
{

    private Vector3 _targetPosition;
    [SerializeField] private float          _projectileSpeed = 200f;
    [SerializeField] private TrailRenderer  _trailRenderer;
    [SerializeField] private Transform      _bulletHitVFXPrefab;
    public void Setup(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    private void Update()
    {
        // Calculate the direction from this bullet to the target position
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);

        // Apply that direction to this bullet's current position
        transform.position += moveDirection * _projectileSpeed * Time.deltaTime;

        float distnaceAfterMoving = Vector3.Distance(transform.position, _targetPosition);

        if (distanceBeforeMoving < distnaceAfterMoving) 
        {  
            // Correct the position of the bullet in case it overshoots
            transform.position = _targetPosition;

            // Unparent the trail effect from the parent object right before it is destroyed so 
            // the trail effect can dissipate on its own rather than dissappear instnantly
            _trailRenderer.transform.parent = null;
            Destroy(gameObject);

            Instantiate(_bulletHitVFXPrefab, _targetPosition, Quaternion.identity);
        }
    }
}
