using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : Building
{
	[Header("Big Blaster Turret Values")]
	[Tooltip("The prefab representing the bullet.")]
	[SerializeField] private GameObject _bulletPrefab;
	[Tooltip("Power of the spawned projectile, index is the tower's current level.")]
	[SerializeField] private int[] powerLevels = new int[] {2, 3, 4, 5};
	[Tooltip("Size of the spawned projectile, index is the tower's current level.")]
	[SerializeField] private int[] sizeLevels = new int[] {3, 4, 6, 6};
	[Tooltip("Time the spawned projectile will live for, index is the tower's current level.")]
	[SerializeField] private int[] timeAliveLevels = new int[] {2, 3, 4, 5};
	
	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	public override void ExecuteTowerAction()
	{
		base.ExecuteTowerAction();

		if (_bulletPrefab != null)
		{
			GameObject newProjectileObj = Instantiate(_bulletPrefab, transform.position, transform.rotation);
			if (newProjectileObj.TryGetComponent<BigBlastBullet>(out var p))
					p.SetupBigBlastBullet(gameObject, powerLevels[_towerLevel], sizeLevels[_towerLevel], timeAliveLevels[_towerLevel]);
		}
	}

    public override void OnTowerDestroy()
    {
        base.OnTowerDestroy();
		ExecuteTowerAction();
    }
}
