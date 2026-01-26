using System;
using System.Linq;
using UnityEngine;
using Godot;

namespace Luny.ContractTest
{
    public static class EngineSimulator
    {
        public static void UnityTick(float deltaTime = 1f / 60f)
        {
            UnityEngine.Time.deltaTime = deltaTime;
            UnityEngine.Time.time += deltaTime;
            UnityEngine.Time.frameCount++;

            var mbs = UnityEngine.Object._allObjects.OfType<MonoBehaviour>().ToList();
            
            // Awake
            foreach (var mb in mbs)
            {
                if (!mb._awakeCalled && mb.gameObject.activeInHierarchy)
                {
                    mb._awakeCalled = true;
                    mb.InternalAwake();
                }
            }

            // Start
            foreach (var mb in mbs)
            {
                if (!mb._startCalled && mb.isActiveAndEnabled)
                {
                    mb._startCalled = true;
                    mb.InternalStart();
                }
            }

            // Update
            foreach (var mb in mbs)
            {
                if (mb.isActiveAndEnabled)
                {
                    mb.InternalUpdate();
                }
            }
        }

        public static void GodotTick(double deltaTime = 1.0 / 60.0)
        {
            Godot.Time.SimulatedFrameCount++;
            Godot.Time.SimulatedTimeMsec += (ulong)(deltaTime * 1000.0);

            var nodes = Godot.GodotObject._allObjects.OfType<Godot.Node>().ToList();
            foreach (var node in nodes)
            {
                if (node.IsInsideTree() && node.CanProcess())
                {
                    node._Process(deltaTime);
                }
            }
        }

        public static void Reset()
        {
            UnityEngine.Object._allObjects.Clear();
            Godot.GodotObject._allObjects.Clear();

            // Re-initialize Godot Root
            var sceneTree = Godot.SceneTree.Instance;
            Godot.GodotObject._allObjects.Add(sceneTree.Root);

            UnityEngine.Time.time = 0;
            UnityEngine.Time.frameCount = 0;
            UnityEngine.Time.deltaTime = 1f / 60f;
            
            Godot.Time.SimulatedFrameCount = 0;
            Godot.Time.SimulatedTimeMsec = 0;
        }
    }
}
