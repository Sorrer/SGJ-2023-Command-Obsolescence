using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTurret : Tower
{
	[SerializeField]
	private int[] powerLevels = new int[] {1, 2, 3, 4};	/**< Power of the spawned projectile, index is the tower's current level. */
	[SerializeField]
	private int[] speedLevels = new int[] {1, 2, 3, 4}; /**< Speed of the spawned projectile, index is the tower's current level. */

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	}

	public override void ExecuteTowerAction(GameObject target)
	{
		base.ExecuteTowerAction(target);

		if (projectileObj != null)
		{
			GameObject newProjectileObj = Instantiate(projectileObj, transform.position, transform.rotation);
			if (newProjectileObj.TryGetComponent<Projectile>(out var p))
				p.SetupProjectile(target, powerLevels[towerLevel], speedLevels[towerLevel]);
		}
	}
}
