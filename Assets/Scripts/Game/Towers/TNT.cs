using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : Building
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
	
	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	public override void ExecuteTowerAction()
	{
		base.ExecuteTowerAction();
	}
}
