using Metaitus.Interfaces;
using Metaitus.Types;
using System.Collections.Generic;

public abstract class MTrigger
{
    // Eventually add non-circle triggers
    // add contains point later

    public bool IsStatic { get; protected set; }
    public float Radius { get; protected set; }
    public MVec2F Offset { get; protected set; }
    public MVec2D Position { get; protected set; }
    public bool HasTriggeredHandlers => triggeredHandlers.Count != 0;

    private readonly List<ITriggeredHandler> triggeredHandlers = new List<ITriggeredHandler>();

    public MTrigger(float radius, MVec2F offset)
    {
        IsStatic = false;
        Radius = radius;
        Offset = offset;
    }

    public MTrigger(float radius, MVec2F offset, MVec2D position)
    {
        IsStatic = false;
        Radius = radius;
        Offset = offset;
        Position = position;
    }

    public void AddTriggeredHandler(ITriggeredHandler triggeredHandler)
    {
        triggeredHandlers.Add(triggeredHandler);
    }

    public void RemoveTriggeredHandler(ITriggeredHandler triggeredHandler)
    {
        triggeredHandlers.Remove(triggeredHandler);
    }

    public bool Intersects(MVec2D position, MTrigger other)
    {
        MVec2F otherPos = ((MVec2F)(other.Position - position)) - Offset + other.Offset;
        return (((otherPos.x - position.x) * (otherPos.x - position.x)) + ((otherPos.y - position.y) * (otherPos.y - position.y))) <= ((Radius + other.Radius) * (Radius + other.Radius));
    }

    public void Triggered(MTrigger triggerer)
    {
        triggeredHandlers.ForEach((h) => h.Triggered(this, triggerer));
    }
}