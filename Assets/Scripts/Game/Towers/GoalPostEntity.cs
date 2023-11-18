namespace DefaultNamespace;

public class GoalPostEntity : TileEntity
{
    public override void OnTowerDestroy()
    {
        SpawnEnemyBehaviour.Instance.mapReference.RemoveGoalPost(LevelGenerator.Instance.GetGridPosition(this.transform.position))
    }
    
    public override void OnTowerPlaced()
    {
        SpawnEnemyBehaviour.Instance.mapReference.AddGoalPost(LevelGenerator.Instance.GetGridPosition(this.transform.position))
    }
    
    
}