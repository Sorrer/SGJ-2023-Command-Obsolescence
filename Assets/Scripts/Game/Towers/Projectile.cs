using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	//protected Rigidbody2D rb;

	//[SerializeField]
	//protected GameObject target;
	protected GameObject owner;
	protected int power;
	protected float speed;
	[SerializeField]
	protected float timeAlive;

	// Start is called before the first frame update
	protected virtual void Start()
	{
		//rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	protected virtual void FixedUpdate()
	{
		/*if (target != null)
		{
			transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
			transform.LookAt(target.transform);
		}*/

		timeAlive -= Time.deltaTime;
		if (timeAlive <= 0.0f)
			Destroy(gameObject);
	}

	public void SetupProjectile(GameObject _owner, int _power, float _speed)
	{
		//target = _target;
		owner = _owner;
		power = _power;
		speed = _speed;
	}
}
