﻿using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CustomPhysicsObject : MonoBehaviour
{
	protected Rigidbody2D rb;

	private List<CustomForce> currentForces = new List<CustomForce>();
	HashSet<CustomForce> forces = new HashSet<CustomForce>();

	// Start is called before the first frame update
	private void Start()
	{

	}

	// Update is called once per frame
	private void FixedUpdate()
	{

	}

	/*
		* Applies the effects of all current forces
		*/
	private void ApplyForces()
	{

	}

	//
	public void AddCustomForce()
	{

	}


}