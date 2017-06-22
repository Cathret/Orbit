using Orbit.Entity;
using UnityEngine;

public class DeviantOpponent : AOpponentController
{
    protected override void Start()
    {
        base.Start();

        Vector2 target2D = Random.insideUnitCircle;
        target2D *= GameGrid.Instance.RealEfficientSide / 2;

        Vector3 target3D = GameGrid.Instance.RealCenter + new Vector3( target2D.x, target2D.y, 0 );

        Vector3 realTarget = target3D + ( target3D - transform.position );

        WayPoints.Add( realTarget );
    }
}