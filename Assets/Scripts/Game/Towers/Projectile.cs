using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	private GameObject target = null;
	private int power;
	private float speed;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if (target == null)
			Destroy(gameObject);
		
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
		transform.LookAt(target.transform);
	}

	public void SetupProjectile(GameObject _target, int _power, float _speed)
	{
		target = _target;
		power = _power;
		speed = _speed;
	}
}
