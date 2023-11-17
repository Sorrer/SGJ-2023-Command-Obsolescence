public class Wall : Building
{
    public override void BreakTower()
    {
        base.BreakTower();
        tileComponent.DestroyEntity();
    }
}