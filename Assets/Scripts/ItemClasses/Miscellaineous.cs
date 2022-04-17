using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Misc.", fileName = "New Misc.")]
public class Miscellaineous : Item
{
    // Start is called before the first frame update
    enum ItemTypes
    {
        Quest,
        Parcel,
        Other

    }

    public int ItemType;


}
