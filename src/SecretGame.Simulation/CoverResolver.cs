namespace SecretGame.Simulation;

public sealed class CoverResolver
{
    public CoverLevel AgainstAttack(BattleMap map, Cell target, Cell attacker)
    {
        var deltaX = attacker.X - target.X;
        var deltaY = attacker.Y - target.Y;
        Cell neighbour;

        if (Math.Abs(deltaX) >= Math.Abs(deltaY))
            neighbour = new Cell(target.X + Math.Sign(deltaX), target.Y);
        else
            neighbour = new Cell(target.X, target.Y + Math.Sign(deltaY));

        return map.Contains(neighbour) ? map.GetCover(target, neighbour) : CoverLevel.None;
    }
}
