﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles attack
public class MeleeEnemyAttack : MonoBehaviour
{
    public bool startAttack = false;
    public List<BoxCollider2D> m_hitboxes;

    // When m_hitboxTimer reaches m_hitboxTime, a new hitbox will be made
    // Need to play test and change values accordingly
    static float HITBOX_START_TIME = 0.1f;
    public float m_hitboxTimer;
    public float m_hitboxTime;

    // Start is called before the first frame update
    void Start()
    {
        m_hitboxTimer = HITBOX_START_TIME;
        m_hitboxTime = HITBOX_START_TIME;
        
        foreach (var hitbox in m_hitboxes)
        {
            hitbox.gameObject.SetActive(false);
        }
    }

    public void ResetAll()
    {
        startAttack = false;

        m_hitboxTimer = HITBOX_START_TIME;
        m_hitboxTime = HITBOX_START_TIME;

        foreach (var hitbox in m_hitboxes)
        {
            hitbox.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (startAttack)
        {
            if (m_hitboxTimer >= m_hitboxTime)
            {
                foreach (var hitbox in m_hitboxes)
                {
                    if (!hitbox.gameObject.activeSelf)
                    {
                        m_hitboxTimer = 0;

                        hitbox.gameObject.SetActive(true);
                        return;
                    }
                }

                // If all hit boxes are generated
                startAttack = false;
                ResetAll();
            }
            else
            {
                m_hitboxTimer += Time.deltaTime;
            }
        }
    }
}
