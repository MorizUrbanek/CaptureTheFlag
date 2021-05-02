using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ChangeWeapon : NetworkBehaviour
{
    public ShowWeapon showWeapon;
    [SerializeField] private const int startWeapon = 1;

    [SyncVar]
    [SerializeField] private int selectedWeapon;

    int weaponcount;

    #region Server
    [Server]
    private void SetWeapon(int value)
    {
        selectedWeapon = value;
    }

    public override void OnStartServer()
    {
        SetWeapon(startWeapon);
    }
    #endregion

    private void OnEnable()
    {
        weaponcount = transform.childCount;
        SwitchWeapon();
        showWeapon.ShowSelectedWeapon(selectedWeapon);
    }

    private void Update()
    {
        int previousSelectedWeopon = selectedWeapon;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= weaponcount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = weaponcount - 1;
            else
                selectedWeapon--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && weaponcount >= 2)
        {
            selectedWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && weaponcount >= 3)
        {
            selectedWeapon = 2;
        }


        if (previousSelectedWeopon != selectedWeapon)
        {
            SwitchWeapon();
            showWeapon.ShowSelectedWeapon(selectedWeapon);
        }
    }

    public void SwitchWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }

}
