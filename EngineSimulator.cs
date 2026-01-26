using System;

namespace Luny.ContractTest
{
    public static class EngineSimulator
    {
        public static void UnityTick(float deltaTime = 1f / 60f)
        {
            UnityEngine.Time.deltaTime = deltaTime;
            UnityEngine.Time.time += deltaTime;
            UnityEngine.Time.frameCount++;
        }

        public static void GodotTick(double deltaTime = 1.0 / 60.0)
        {
            Godot.Time.SimulatedFrameCount++;
            Godot.Time.SimulatedTimeMsec += (ulong)(deltaTime * 1000.0);
        }
        
        public static void Reset()
        {
            UnityEngine.Time.time = 0;
            UnityEngine.Time.frameCount = 0;
            UnityEngine.Time.deltaTime = 1f / 60f;
            
            Godot.Time.SimulatedFrameCount = 0;
            Godot.Time.SimulatedTimeMsec = 0;
        }
    }
}
