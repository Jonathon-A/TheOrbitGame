using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTrajectoryProperies
{
    private Vector2 Position;
    private float Mass;

    public NewTrajectoryProperies(Vector2 CurrentPosition, float CurrentMass)
    {
        this.Position = CurrentPosition;
        this.Mass = CurrentMass;
    }

    public Vector2 getPosition()
    {
        return Position;
    }
    public float getMass()
    {
        return Mass;
    }
}
