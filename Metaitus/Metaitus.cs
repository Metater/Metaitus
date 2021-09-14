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
        public Vector2 position;
        public Vector2 velocity;

        public float drag;
        public bool gravity;
        public float gravityScale;

        public Body(Vector2 position, Vector2 velocity, float drag = 0, bool gravity = false, float gravityScale = 1)
        {
            this.position = position;
            this.velocity = velocity;
            this.drag = drag;
            this.gravity = gravity;
            this.gravityScale = gravityScale;
        }

        public void Tick(float timestep)
        {
            if (gravity) AddForce(Vector2.down * 9.8f * gravityScale * timestep);
            velocity *= 1 - (timestep * drag);
            position += velocity * timestep;
        }

        public void AddForce(Vector2 force)
        {
            velocity += force;
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
