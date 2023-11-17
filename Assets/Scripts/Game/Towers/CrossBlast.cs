using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBlast : Tower
{
	[SerializeField]
	private int[] powerLevels = new int[] {1, 2, 3, 4};	/**< Power of the spawned projectile, index is the tower's current level. */
	[SerializeField]
	private int[] speedLevels = new int[] {2, 4, 6, 8}; /**< Speed of the spawned projectile, index is the tower's current level. */

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		//col = GetComponent<CircleCollider2D>();
		//SetCurrentRaidus();
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	}

	public override void ExecuteTowerAction()
	{
		base.ExecuteTowerAction();
		StartCoroutine(ExecuteCrossBlast());
	}

	private IEnumerator ExecuteCrossBlast()
	{
		for (int i = 0; i < 3; i++)
		{
			if (projectileObj != null)
			{
				for (int j = 0; j < 4; j++)
				{
					GameObject newProjectileObj = Instantiate(projectileObj, transform.position, transform.rotation);
					if (newProjectileObj.TryGetComponent<Projectile>(out var p))
						p.SetupProjectile(gameObject, powerLevels[towerLevel], speedLevels[towerLevel]);
					newProjectileObj.transform.Rotate(0.0f, 0.0f, 90.0f*j);
				}
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
}
