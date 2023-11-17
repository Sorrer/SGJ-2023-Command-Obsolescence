using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BigBlastBullet : Projectile
{
	private float scale;
	private HashSet<EnemyBehaviour> alreadyHit = new HashSet<EnemyBehaviour>();

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

	new protected void OnTriggerEnter2D(Collider2D other)
	{
		// Cancel out the normal projectile behavior
	}

	protected void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.TryGetComponent<EnemyBehaviour>(out var enemy))
		{
			if (!alreadyHit.Contains(enemy))
			{
				Debug.Log("Hit enemy " + other.gameObject.name);
				enemy.Damage(power);
				alreadyHit.Add(enemy);
			}
		}
		if (other != owner && other.gameObject.TryGetComponent<TileEntity>(out var entity))
		{
			Debug.Log("Hit entity " + other.gameObject.name);
			if (entity.CanBeDestroyed) 
			{
				entity.OnTowerDestroy();
				Destroy(entity.gameObject);
			}
		}
	}
}
