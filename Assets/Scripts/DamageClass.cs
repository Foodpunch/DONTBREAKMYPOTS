using UnityEngine;
using System.Collections;

public struct DamageClass { 
    //Defines what kind of damage type

    int physical;           //physical Damage
    int elemental;          //elemental Damage

    public DamageClass(int a, int b)        //constructor
    {
        this.physical = a;
        this.elemental = b;
    }
}

public struct Element
    //Defines what element
{
    public enum Type    //enum for elements
    {//PLACEHOLDER ELEMENTS!!!
        FIRE,
        WATER,
        EARTH,
        AIR,
        NULL        //incase something has no element
    }
    public Type DamageType;
    public Element(Type _type)
    {
        DamageType = _type;
    }
}

