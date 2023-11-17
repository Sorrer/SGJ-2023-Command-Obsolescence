using UnityEngine;

public class TileEntity : MonoBehaviour
{
	[Header("General Entity Values")]
	[Tooltip("The string name of the entity.")]
	[SerializeField] protected string _entityName;
	[Tooltip("The type of the entity.")]
	[SerializeField] protected EntityType _entityType;
	[Tooltip("The sprite renderer for this entity.")]
	[SerializeField] protected SpriteRenderer _sr;
	[Tooltip("Whether or not this entity can be destroyed.")]
	[SerializeField] protected bool _canBeDestroyed = true;

	/// <summary>
	/// The destroy cost for all entities. 
	/// 
	/// Wasn't sure where to put this; feel free to move this to a more suitable script if need be.
	/// </summary>
	public const int STANDARD_DESTROY_COST = 200;

	/// <summary>
	/// Whether or not this tower can be destroyed.
	/// </summary>
	public bool CanBeDestroyed => _canBeDestroyed;
}
