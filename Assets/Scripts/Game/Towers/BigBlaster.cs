using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBlaster : Building
{
	[Header("Big Blaster Turret Values")]
	[Tooltip("The prefab representing the bullet.")]
	[SerializeField] private GameObject _bulletPrefab;
	[Tooltip("Power of the spawned projectile, index is the tower's current level.")]
	[SerializeField] private int[] powerLevels = new int[] {1, 2, 3, 4};
	[Tooltip("Size of the spawned projectile, index is the tower's current level.")]
	[SerializeField] private int[] sizeLevels = new int[] {2, 4, 6, 8};
	[Tooltip("Range of the tower, index is the tower's current level.")]
	[SerializeField] private int[] timeAliveLevels = new int[] {3, 5, 7, 9};

	private BoxCollider2D col;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		col = GetComponentInChildren<BoxCollider2D>();
		ExecuteTowerAction();
		//SetCurrentRaidus();
	}

	public override void ExecuteTowerAction()
	{
		base.ExecuteTowerAction();
		
		if (_bulletPrefab != null)
		{
			var pos = CurrentDirection switch
			{
				TowerDirection.TD_Up => new Vector3(transform.position.x, transform.position.y + LevelGenerator.Instance.GetOffsetY() / 2f, transform.position.z),
				TowerDirection.TD_Right => new Vector3(transform.position.x + LevelGenerator.Instance.GetOffsetX() / 2f, transform.position.y, transform.position.z),
				TowerDirection.TD_Down => new Vector3(transform.position.x, transform.position.y - LevelGenerator.Instance.GetOffsetY() / 2f, transform.position.z),
				TowerDirection.TD_Left => new Vector3(transform.position.x - LevelGenerator.Instance.GetOffsetX() / 2f, transform.position.y, transform.position.z),
				_ => transform.position,
			};
			var rot = CurrentDirection switch
			{
				TowerDirection.TD_Up => Quaternion.Euler(0f, 0f, 0f),
				TowerDirection.TD_Right => Quaternion.Euler(0f, 0f, -90f),
				TowerDirection.TD_Down => Quaternion.Euler(0f, 0f, 180f),
				TowerDirection.TD_Left => Quaternion.Euler(0f, 0f, 90f),
				_ => transform.rotation,
			};
			GameObject newProjectileObj = Instantiate(_bulletPrefab, pos, rot);
			if (newProjectileObj.TryGetComponent<BigBlastBullet>(out var p))
					p.SetupBigBlastBullet(gameObject, powerLevels[_towerLevel], sizeLevels[_towerLevel], timeAliveLevels[_towerLevel]);
			BreakTower();
		}
	}
}
