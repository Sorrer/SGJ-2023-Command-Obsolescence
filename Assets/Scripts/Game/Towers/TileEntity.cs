using UnityEngine;

public class TileEntity : MonoBehaviour
{
	[Header("General Entity Values")]
	[Tooltip("The string name of the entity.")]
	[SerializeField] protected string _entityName;
	[Tooltip("The sprite renderer for this entity.")]
	[SerializeField] protected SpriteRenderer _sr;
	[Tooltip("Whether or not this entity can be destroyed.")]
	[SerializeField] protected bool _canBeDestroyed = true;
	[Tooltip("Whether or not this entity blocks movement.")]
	[SerializeField] protected bool _isWall = false;
	[Tooltip("The name of the building that can be used to replace this entity.")]
	[SerializeField] protected string _replacement;

	[HideInInspector] public TileComponent tileComponent;
	
	/// <summary>
	/// The destroy cost for all entities. 
	/// 
	/// Wasn't sure where to put this; feel free to move this to a more suitable script if need be.
	/// </summary>
	public const int STANDARD_DESTROY_COST = 200;

	/// <summary>
	/// Whether or not this entity can be destroyed.
	/// </summary>
	public bool CanBeDestroyed => _canBeDestroyed;
	/// <summary>
	/// Whether or not this entity acts as a wall.
	/// </summary>
	public bool IsWall => _isWall;
	/// <summary>
	/// The name of the building that can be used to replace this entity.
	/// </summary>
	public string Replacement => _replacement;

	public int weight = 1;

	public bool isInteractable = true;
}
