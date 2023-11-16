using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTurret : Tower
{
	[SerializeField]
	private int[] powerLevels = new int[] {1, 2, 3, 4};	/**< Power of the spawned projectile, index is the tower's current level. */
	[SerializeField]
	private int[] speedLevels = new int[] {1, 2, 3, 4}; /**< Speed of the spawned projectile, index is the tower's current level. */
	[SerializeField]
	private int[] rangeLevels = new int[] {6, 8, 10, 12}; /**< Range of the tower, index is the tower's current level. */

	private CircleCollider2D col;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		col = GetComponent<CircleCollider2D>();
		SetCurrentRaidus();
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
			BreakTower();
			StartCoroutine(ResetTower(2.0f));
		}
	}

    public override void UpgradeTower()
    {
        base.UpgradeTower();
		SetCurrentRaidus();
    }

	private void SetCurrentRaidus()
	{
		if (col != null)
			col.radius = rangeLevels[towerLevel];
	}
}
