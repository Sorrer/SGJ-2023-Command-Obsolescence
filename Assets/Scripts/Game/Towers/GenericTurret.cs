using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTurret : Tower
{
	[SerializeField]
	private int[] powerLevels = new int[] {1, 2, 3, 4};	/**< Power of the spawned projectile, index is the tower's current level. */
	[SerializeField]
	private int[] speedLevels = new int[] {2, 4, 6, 8}; /**< Speed of the spawned projectile, index is the tower's current level. */
	[SerializeField]
	private int[] rangeLevels = new int[] {6, 8, 10, 12}; /**< Range of the tower, index is the tower's current level. */
	[SerializeField]
	private int[] bulletNumberLevels = new int[] {10, 12, 14, 16}; /**< Number of times the turret will fire before it breaks. */
	[SerializeField]
	private float[] bulletDelayLevels = new float[] {5f, 4f, 3f, 1f}; /**< Seconds wait before each bullet fires */

	private CircleCollider2D col;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		col = GetComponent<CircleCollider2D>();
		ExecuteTowerAction();
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
		StartCoroutine(FireBulletRally());
	}

	public override void UpgradeTower()
	{
		base.UpgradeTower();
		//SetCurrentRaidus();
	}

	/*private void SetCurrentRaidus()
	{
		if (col != null)
			col.radius = rangeLevels[towerLevel];
	}*/

	private IEnumerator FireBulletRally()
	{
		if (projectileObj != null)
		{
			for (int i = 0; i < bulletNumberLevels[towerLevel]; i++)
			{
				var q = currentDirection switch
				{
					TowerDirection.TD_Up => Quaternion.Euler(0f, 0f, 0f),
					TowerDirection.TD_Right => Quaternion.Euler(0f, 0f, -90f),
					TowerDirection.TD_Down => Quaternion.Euler(0f, 0f, 180f),
					TowerDirection.TD_Left => Quaternion.Euler(0f, 0f, 90f),
					_ => transform.rotation,
				};
				GameObject newProjectileObj = Instantiate(projectileObj, transform.position, q);
				if (newProjectileObj.TryGetComponent<Projectile>(out var p))
					p.SetupProjectile(gameObject, powerLevels[towerLevel], speedLevels[towerLevel]);
				yield return new WaitForSeconds(bulletDelayLevels[towerLevel]);
			}
			BreakTower();
			//StartCoroutine(ResetTower(2.0f));
		}
	}
}
