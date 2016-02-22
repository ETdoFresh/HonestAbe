﻿using UnityEngine;
using System.Collections.Generic;

public class PerkManager : MonoBehaviour
{

    [HideInInspector]
    public List<Perk> perkList = new List<Perk>();

    [HideInInspector]
    public Perk activeAxePerk = null;
    [HideInInspector]
    public Perk activeHatPerk = null;
    [HideInInspector]
    public Perk activeTrinketPerk = null;

    public delegate void PerkEffectHandler();
    public event PerkEffectHandler AxePerkEffect = delegate { };
    public event PerkEffectHandler HatPerkEffect = delegate { };
    public event PerkEffectHandler TrinketPerkEffect = delegate { };

    void Start()
    {
        GameObject[] perksInLevel = GameObject.FindGameObjectsWithTag("Perk");
        for (int i = 0; i < perksInLevel.Length; i++)
        {
            Perk p = perksInLevel[i].GetComponent<Perk>();
            p.CheckStatus();
            perkList.Add(p);
        }
    }
}