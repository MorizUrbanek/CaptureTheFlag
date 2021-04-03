using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    public ShowWeapon showWeapon;

    int selectedWeapon = 0;
    int weaponcount;


    private void Start()
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
