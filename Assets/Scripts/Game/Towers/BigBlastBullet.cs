using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBlastBullet : Projectile
{
	private float scale;

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
		//transform.Translate(Vector3.up * speed * Time.deltaTime);
	}

	public void SetupBigBlastBullet(GameObject _owner, int _power, float _scale, float _timeAlive)
	{
		Debug.Log("Setup big blast");
		owner = _owner;
		power = _power;
		scale = _scale;
		timeAlive = _timeAlive;
		transform.localScale = new Vector3(scale, scale, 1f);
	}
}
