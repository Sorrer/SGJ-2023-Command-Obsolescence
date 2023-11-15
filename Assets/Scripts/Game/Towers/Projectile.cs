using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField]
	private GameObject target;
	private int power;
	private float speed;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (target != null)
		{
			transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
			transform.LookAt(target.transform);
		}
	}

	public void SetupProjectile(GameObject _target, int _power, float _speed)
	{
		target = _target;
		power = _power;
		speed = _speed;
	}
}
