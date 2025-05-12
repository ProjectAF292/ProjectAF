using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
    public int weaponId;
    public enum weaponType
    {
        Sword,
        Range,
        None
    }
    public weaponType _weaponType;
}
