using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flamethrower : Building
{
	[Header("Flamethrower Values")]
	[Tooltip("The prefab representing the flames being shot out.")]
	[SerializeField] private GameObject _flamePrefab;
	[Tooltip("Power of the spawned projectile, index is the tower's current level.")]
	[SerializeField] private int[] powerLevels = new int[] {1, 2, 3, 4};
	[Tooltip("Speed of the spawned projectile, index is the tower's current level.")]
	[SerializeField] private float[] speedLevels = new float[] {2f, 4f, 6f, 8f};
	[Tooltip("Range of the tower, index is the tower's current level.")]
	[SerializeField] private float[] rangeLevels = new float[] {1f, 2f, 3f, 4f};
	[Tooltip("Number of times the flamethrower will fire before it breaks.")]
	[SerializeField] private int[] bulletNumberLevels = new int[] {10, 12, 14, 16};
	[Tooltip("Seconds to wait before each flame fires.")]
	[SerializeField] private float[] bulletDelayLevels = new float[] {5f, 4f, 3f, 1f};

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		//col = GetComponent<CircleCollider2D>();
		//SetCurrentRaidus();
		ExecuteTowerAction();
	}

	public override void ExecuteTowerAction()
	{
		base.ExecuteTowerAction();
		StartCoroutine(ExecuteCrossBlast());
	}

	private IEnumerator ExecuteCrossBlast()
	{
		for (int i = 0; i < bulletNumberLevels[_towerLevel]; i++)
		{
			if (_flamePrefab != null)
			{
				for (int j = 0; j < 4; j++)
				{
					GameObject newProjectileObj = Instantiate(_flamePrefab, transform.position, transform.rotation);
					if (newProjectileObj.TryGetComponent<Projectile>(out var p))
						p.SetupProjectile(gameObject, powerLevels[_towerLevel], speedLevels[_towerLevel], rangeLevels[_towerLevel]);
					newProjectileObj.transform.Rotate(0.0f, 0.0f, 90.0f*j);
				}
			}
			yield return new WaitForSeconds(bulletDelayLevels[_towerLevel]);
		}

		BreakTower();
	}
}
