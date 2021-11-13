using Metaitus.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Metaitus
{
    public class World : ITickable
    {
        private IWorldListener worldListener;

        private int nextBodyId = 0;
        private Dictionary<int, Body> bodies = new Dictionary<int, Body>();

        public float targetTPS;
        public float timeScale = 1;
        private float time = 0;

        public World(IWorldListener worldListener, float targetTPS)
        {
            this.worldListener = worldListener;
            this.targetTPS = targetTPS;
        }

        public void Tick(float timestep)
        {
            foreach (KeyValuePair<int, Body> body in bodies)
            {
                body.Value.Tick(timestep * timeScale);
                worldListener.UpdateBody(body.Key, body.Value.position);
            }
        }
        public void Update(float deltaTime)
        {

            float secondsPerTick = 1 / targetTPS;
            if (deltaTime >= secondsPerTick) worldListener.Message("Target TPS too fast!!!");
            time += deltaTime;
            if (time >= secondsPerTick)
            {
                time -= secondsPerTick;
                if (time >= secondsPerTick)
                {
                    worldListener.Message($"Skipping {time} seconds!");
                    time = 0;
                }
                Tick(secondsPerTick);
            }
        }
        public void UpdateTargetTPS(float targetTPS)
        {
            this.targetTPS = targetTPS;
        }

        public void AddBody(Body body)
        {
            bodies.Add(nextBodyId, body);
            worldListener.NewBody(nextBodyId);
            nextBodyId++;
        }

        public Body GetBody(int bodyId)
        {
            return bodies[bodyId];
        }

        public void RemoveBody(int bodyId)
        {
            bodies.Remove(bodyId);
            worldListener.RemoveBody(bodyId);
        }
    }

    public class Body : ITickable
    {
        public Vector2 position = Vector2.zero;
        public Vector2 velocity = Vector2.zero;

        public bool dragEnabled = false;
        public float dragScale = 0;

        public Dictionary<string, Collider> colliders = new Dictionary<string, Collider>();

        private Queue<Vector2> forceQueue = new Queue<Vector2>();
        private Dictionary<string, Vector2> accelerations = new Dictionary<string, Vector2>();

        #region BuilderMethods
        public Body SetPosition(Vector2 position)
        {
            this.position = position;
            return this;
        }
        public Body SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
            return this;
        }
        public Body SetAcceleration(string name, Vector2 acceleration)
        {
            if (accelerations.ContainsKey(name))
                accelerations[name] = acceleration;
            else
                accelerations.Add(name, acceleration);
            return this;
        }
        public Body SetGravity(bool gravityEnabled, float gravityScale)
        {
            if (gravityEnabled)
                SetAcceleration("gravity", Vector2.down * 9.8f * gravityScale);
            else
                accelerations.Remove("gravity");
                return this;
        }
        public Body SetDrag(bool dragEnabled, float dragScale)
        {
            this.dragEnabled = dragEnabled;
            this.dragScale = dragScale;
            return this;
        }
        #endregion BuilderMethods

        public void Tick(float timestep)
        {
            foreach (Vector2 acceleration in accelerations.Values) AddForce(acceleration * timestep);
            while (forceQueue.Count != 0) velocity += forceQueue.Dequeue();
            if (dragEnabled) velocity *= 1 - (timestep * dragScale);
            position += velocity * timestep;
        }

        public void AddForce(Vector2 force)
        {
            forceQueue.Enqueue(force);
        }

        public override string ToString()
        {
            return $"(Position: {position}, Velocity: {velocity})";
        }
    }

    public struct Vector2
    {
        public float x, y;

        public static Vector2 zero = new Vector2(0, 0);
        public static Vector2 down = new Vector2(0, -1);

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}
