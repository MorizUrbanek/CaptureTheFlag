using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public Weapon weapon;
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private bool damagePlayer;

    private void OnEnable()
    {
        weapon.SetWeapon(damage, range, shootPoint, damagePlayer);
    }

}
