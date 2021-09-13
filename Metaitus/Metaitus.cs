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

        private Stopwatch clockTicksPerUpdateCounter = new Stopwatch();
        private Stopwatch clockTicksPerWorldTickCounter = new Stopwatch();
        private long clockTicksPerWorldTick;
        private long clockTicksCount = 0;
        public float SecondsPerTick { get; private set; } = 0;
        public float TargetTPS { get; private set; }
        public bool IsAutoTPSEnabled { get; private set; } = false;

        public World(IWorldListener worldListener, float targetTPS)
        {
            this.worldListener = worldListener;
            TargetTPS = targetTPS;
        }

        public void Start()
        {
            clockTicksPerUpdateCounter.Start();
            clockTicksPerWorldTickCounter.Start();
        }

        public void Tick(float timestep)
        {
            foreach (KeyValuePair<int, Body> body in bodies)
            {
                body.Value.Tick(timestep);
                worldListener.UpdateBody(body.Key, body.Value.position);
            }
        }
        public void Update(float deltaTime)
        {
            clockTicksPerUpdateCounter.Stop();
            double secondsPerUpdate = (clockTicksPerUpdateCounter.ElapsedTicks / 10000000);
            //double updatesPerSecond
            //if (secondsPerUpdate != 0)
                //updatesPerSecond = 1 / secondsPerUpdate;
            clockTicksPerUpdateCounter.Restart();
            //if (IsAutoTPSEnabled)
            //{
                //clockTicksPerWorldTick = (long)(1 / (updatesPerSecond * 0.8d)) * 10000000;
            //}
            //else
            //{
                clockTicksPerWorldTick = (1 / ((long)TargetTPS)) * 10000000;
            //}
            clockTicksPerWorldTickCounter.Stop();
            clockTicksCount += clockTicksPerWorldTickCounter.ElapsedTicks;
            clockTicksPerWorldTickCounter.Restart();
            if (clockTicksCount >= clockTicksPerWorldTick)
            {
                SecondsPerTick = clockTicksCount / 10000000;
                clockTicksCount -= clockTicksPerWorldTick;
                //if (clockTicksCount >= clockTicksPerWorldTick)
                //{
                    // add skipping time or get better
                    //IsAutoTPSEnabled = true;
                    //worldListener.Message($"Enabled AutoTPS!");
                //}
                //else
                //{
                    //IsAutoTPSEnabled = false;
                    //worldListener.Message($"Disabled AutoTPS!");
                //}
                Tick(SecondsPerTick);
            }
        }
        public void UpdateTargetTPS(float targetTPS)
        {
            TargetTPS = targetTPS;
        }

        public void AddBody(Body body)
        {
            bodies.Add(nextBodyId, body);
            worldListener.NewBody(nextBodyId);
            nextBodyId++;
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

        public Body(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity;
        }

        public void Tick(float timestep)
        {
            position += velocity * timestep;
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
