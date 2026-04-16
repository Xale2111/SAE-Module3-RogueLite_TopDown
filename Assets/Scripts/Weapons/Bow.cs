using System;
using UnityEngine;

public class Bow : Weapon
{    
    [SerializeField] private GameObject _normalArrowPrefab;
    [SerializeField] private GameObject _specialArrowPrefab;  
    [SerializeField] private Transform _aimingTransform;
    [SerializeField] private bool autoShoot;

    private float _timer = 0;
    public override void LeftClick()
    {
        if (animator)
        { 
            animator.SetTrigger("MainAttack");
        }
        Debug.Log("First attack BOW");        
    }

    public override void RightClick()
    {
        if (animator)
        {
            animator.SetTrigger("SecondAttack");
        }
        Debug.Log("Second attack BOW");
    }

    public void ShootNormalArrow()
    {
        Debug.Log("Release Arrow");
        SpawnArrow(_normalArrowPrefab);
    }

    public void ShootSpecialArrow()
    {
        Debug.Log("Release SPECIAL Arrow");
        SpawnArrow(_specialArrowPrefab);
    }

    private void SpawnArrow(GameObject arrowPrefab)
    {
        Vector3 direction = _aimingTransform.position - transform.position;
        
        // Pour un jeu 2D, calculer l'angle avec Atan2
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg-90;
        Quaternion aimDirection = Quaternion.Euler(0, 0, angle);

        Instantiate(arrowPrefab, transform.position, aimDirection);
    }

    private void Update()
    {
        if (autoShoot)
        {
            if (_timer > _cooldown)
            {
                _timer = 0;
                SpawnArrow(_normalArrowPrefab);
            }
            else
            {
                _timer += Time.deltaTime;
            }
            
        }
    }
}
