using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTurret : Building
{
	[Header("Gun Turret Values")]
	[Tooltip("The prefab representing the bullet.")]
	[SerializeField] private GameObject _bulletPrefab;
	[Tooltip("Power of the spawned projectile, index is the tower's current level.")]
	[SerializeField] private int[] powerLevels = new int[] {1, 2, 3, 4};
	[Tooltip("Speed of the spawned projectile, index is the tower's current level.")]
	[SerializeField] private int[] speedLevels = new int[] {2, 4, 6, 8};
	[Tooltip("Range of the tower, index is the tower's current level.")]
	[SerializeField] private int[] rangeLevels = new int[] {6, 8, 10, 12};
	[Tooltip("Number of times the turret will fire before it breaks.")]
	[SerializeField] private int[] bulletNumberLevels = new int[] {10, 12, 14, 16};
	[Tooltip("Seconds to wait before each bullet fires.")]
	[SerializeField] private float[] bulletDelayLevels = new float[] {5f, 4f, 3f, 1f};

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

	public override void TryUpgradeTower()
	{
		base.TryUpgradeTower();
		//SetCurrentRaidus();
	}

	/*private void SetCurrentRaidus()
	{
		if (col != null)
			col.radius = rangeLevels[towerLevel];
	}*/

	private IEnumerator FireBulletRally()
	{
		if (_bulletPrefab != null)
		{
			for (int i = 0; i < bulletNumberLevels[_towerLevel]; i++)
			{
				var q = CurrentDirection switch
				{
					TowerDirection.TD_Up => Quaternion.Euler(0f, 0f, 0f),
					TowerDirection.TD_Right => Quaternion.Euler(0f, 0f, -90f),
					TowerDirection.TD_Down => Quaternion.Euler(0f, 0f, 180f),
					TowerDirection.TD_Left => Quaternion.Euler(0f, 0f, 90f),
					_ => transform.rotation,
				};
				GameObject newProjectileObj = Instantiate(_bulletPrefab, transform.position, q);
				if (newProjectileObj.TryGetComponent<Projectile>(out var p))
					p.SetupProjectile(gameObject, powerLevels[_towerLevel], speedLevels[_towerLevel], rangeLevels[_towerLevel]);
				yield return new WaitForSeconds(bulletDelayLevels[_towerLevel]);
			}
			BreakTower();
			//StartCoroutine(ResetTower(2.0f));
		}
	}
}
