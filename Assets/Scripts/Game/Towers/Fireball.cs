using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile
{
	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		//if (rb != null)
		//	rb.velocity = transform.up * speed;
		transform.Translate(Vector3.up * speed * Time.deltaTime);
	}
}
