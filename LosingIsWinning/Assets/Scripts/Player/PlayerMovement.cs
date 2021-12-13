﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Data")]
    public float m_walkSpeed = 100.0f;
    public float m_jumpSpeed = 400.0f;
    public float m_fallMultiplier = 2.5f;

    public float m_dashSpeed = 200.0f;
    public float m_dashTime = 2.0f;
    private float m_currDashTime = 0.0f;

    [Header("States")]
    [Tooltip("Standing on ground will reset jump")]
    public Transform m_groundCheckPos; 
    public float m_groundCheckRadius;
    public LayerMask m_groundLayers;

    public int m_maxJumps = 1;
    private int m_currJumps = 0;

    private bool m_startJump = false; //when just press jump
    private bool m_isGrounded = true; //check if on ground
    private bool m_isDashing = false;

    private Vector2 m_dir = Vector2.zero;

    private Rigidbody2D m_rigidBody;


    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();

        m_startJump = false;
        m_isDashing = false;
        m_isGrounded = Physics2D.OverlapCircle(m_groundCheckPos.position, m_groundCheckRadius, m_groundLayers);

        m_currJumps = m_maxJumps;
    }

    // Update is called once per frame
    void Update()
    {
        m_dir.x = Input.GetAxisRaw("Horizontal");
        m_dir.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && m_currJumps > 0)
        {
            --m_currJumps;
            m_startJump = true;
        }

        //for dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && !m_isDashing)
        {
            m_isDashing = true;
            m_currDashTime = m_dashTime;
        }

        if (m_isDashing)
        {
            m_currDashTime -= Time.deltaTime;
            m_isDashing = m_currDashTime > 0.0f; 
        }
}

    private void FixedUpdate()
    {
        //press dash
        if (m_currDashTime > 0.0f)
        {
            //TODO, change to curr dir facing not just direction itself
            Debug.Log("DASH");
            m_rigidBody.velocity = new Vector2(1.0F * m_dashSpeed * Time.fixedDeltaTime, m_rigidBody.velocity.y);
            return;
        }

        //press jump
        if (m_startJump)
        {
            m_rigidBody.velocity = Vector2.up * m_jumpSpeed * Time.fixedDeltaTime;
            m_startJump = false;
        }

        m_isGrounded = Physics2D.OverlapCircle(m_groundCheckPos.position, m_groundCheckRadius, m_groundLayers);
        if (m_isGrounded) //reset number of jumps
        {
            m_currJumps = m_maxJumps;
        }

        //update right left movement
        m_rigidBody.velocity = new Vector2(m_dir.x * m_walkSpeed * Time.fixedDeltaTime, m_rigidBody.velocity.y);

        //if falling
        if (m_rigidBody.velocity.y < 0.0f)
        {
            m_rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (m_fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(m_groundCheckPos.position, m_groundCheckRadius);
    }
}