using UnityEngine;

public class TrajectoryProperties
{
    private Vector2 Position;
    private Vector2 Velocity;

    public TrajectoryProperties(Vector2 CurrentPosition, Vector2 CurrentVelocity)
    {
        this.Position = CurrentPosition;
        this.Velocity = CurrentVelocity;
    }

    public Vector2 getPosition() {
        return Position;
    }
    public Vector2 getVelocity()
    {
        return Velocity;
    }
}
