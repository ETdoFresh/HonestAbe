﻿using UnityEngine;
using System.Collections;
using System;

public class Damage : MonoBehaviour
{
    public float damageAmount;
    public float damageRate = 0.2f;
    public GameObject bloodSplatter;
    public GameObject bloodSwing;
    public GameObject bloodShoot;
    public GameObject bloodMelee;
    public GameObject bloodJab;

    private Health health;
    private BaseCollision collision;
    private SoundPlayer sound;

    void Awake()
    {
        collision = GetComponent<BaseCollision>();
        sound = GetComponent<SoundPlayer>();
    }

    void OnEnable()
    {
        collision.OnCollisionEnter += OnCollision;
    }

    void OnDisable()
    {
        collision.OnCollisionEnter -= OnCollision;
    }

    public void ExecuteDamage(float damageAmount, Collider2D collider)
    {
        if (tag == "Enemy")
            ;
        if (tag == "Boss")
            if (GetComponent<Boss>() != null && GetComponent<Boss>().bossName == "Bear")
                EventHandler.SendEvent(EventHandler.Events.BEAR_HIT, GameObject.Find("Player"));
        if (collider)
            AddBlood(collider);
        if (health = GetComponent<Health>())
            health.Decrease(Convert.ToInt32(damageAmount));
    }

    private void OnCollision(Collider2D collider)
    {
        AttackArea attackArea = collider.GetComponent<AttackArea>();
        if (attackArea && attackArea.IsShootType())
            return;

        if (collider.tag == "Damage")
        {
            damageAmount = collider.transform.GetComponentInParent<Attack>().GetDamageAmount();
            ExecuteDamage(damageAmount, collider);

            if (name.Contains("Bear"))
            {
                if (UnityEngine.Random.value < 0.33f)
                    SoundPlayer.Play("Damage React Bear");
            }
            else if (attackArea)
            {
                if (attackArea.GetAttackState() == Attack.State.Heavy)
                {
                    if (tag != "Player")
                        SoundPlayer.Play("Damage React Heavy");
                    if (attackArea.GetAttackType() == Weapon.AttackType.Swing)
                    {
                        SoundPlayer.Play("Heavy Axe Impact 1");
                        SoundPlayer.Play("Heavy Axe Impact 1.1");
                    }
                    else if (attackArea.GetAttackType() == Weapon.AttackType.Melee)
                    {
                        SoundPlayer.Play("Heavy Punch Impact 1");
                        SoundPlayer.Play("Heavy Punch Impact 1.2");
                        SoundPlayer.Play("Heavy Punch Impact 1.3");
                    }
                    else if (attackArea.GetAttackType() == Weapon.AttackType.Slash)
                        SoundPlayer.Play("Saber Impact");

                    else if (attackArea.GetAttackType() == Weapon.AttackType.Knife)
                        SoundPlayer.Play("Knife Impact");
                }
                else
                {
                    if (tag != "Player")
                        SoundPlayer.Play("Damage React Light");
                    if (attackArea.GetAttackType() == Weapon.AttackType.Swing)
                    {
                        if (attackArea.GetAttackChainNumber() >= 2)
                            SoundPlayer.Play("Light Axe Impact 3");
                        else if (attackArea.GetAttackChainNumber() >= 1)
                            SoundPlayer.Play("Light Axe Impact 2");
                        else
                            SoundPlayer.Play("Light Axe Impact 1");
                    }
                    else if (attackArea.GetAttackType() == Weapon.AttackType.Melee)
                        SoundPlayer.Play("Light Punch Impact");

                    else if (attackArea.GetAttackType() == Weapon.AttackType.Slash)
                        SoundPlayer.Play("Saber Impact");

                    else if (attackArea.GetAttackType() == Weapon.AttackType.Knife)
                        SoundPlayer.Play("Knife Impact");
                }
            }
            else
            {
                if (tag != "Player")
                    SoundPlayer.Play("Damage React Light");
            }
        }
    }

    private void AddBlood(Collider2D collider)
    {
        if (collider.GetComponentInParent<Attack>())
            if (collider.GetComponentInParent<Attack>().weapon)
            {
                Weapon.AttackType attackType = collider.GetComponentInParent<Attack>().weapon.attackType;
                if (attackType == Weapon.AttackType.Swing)
                    InstantiateBlood(bloodSwing);
                else if (attackType == Weapon.AttackType.Shoot)
                    InstantiateBlood(bloodShoot);
                else if (attackType == Weapon.AttackType.Melee)
                    InstantiateBlood(bloodMelee);
                else if (attackType == Weapon.AttackType.Jab)
                    InstantiateBlood(bloodJab);

                return;
            }

        InstantiateBlood(bloodSplatter);

    }

    private void InstantiateBlood(GameObject bloodPrefab)
    {
        if (!bloodPrefab)
            return;

        GameObject blood = Instantiate(bloodPrefab);
        blood.transform.position = transform.position;
        if (transform.localScale.x < 0)
            blood.transform.Rotate(0, 180, 0);
        Destroy(blood, 10);
    }
}
