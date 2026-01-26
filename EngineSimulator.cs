using Godot;
using Luny.Godot.Engine;
using Luny.Unity.Engine;
using System;
using System.Linq;
using UnityEngine;
using Godot_Time = Godot.Time;
using Object = UnityEngine.Object;
using Time = UnityEngine.Time;

namespace Luny.ContractTest
{
	public enum EngineType
	{
		Unity,
		Godot
	}

	public static class EngineSimulator
	{
		public static void Tick(EngineType type, Double deltaTime = 1.0 / 60.0)
		{
			if (type == EngineType.Unity)
			{
				UnityEngine.Time.frameCount++;
				UnityTick((Single)deltaTime);
			}
			else
			{
				Godot_Time.SimulatedFrameCount++;
				GodotTick(deltaTime);
			}
		}

		public static void RunFrames(EngineType type, Int32 count, Double deltaTime = 1.0 / 60.0)
		{
			for (var i = 0; i < count; i++)
				Tick(type, deltaTime);
		}

		public static void UnityTick(Single deltaTime = 1f / 60f)
		{
			UnityEngine.Time.deltaTime = deltaTime;
			UnityEngine.Time.time += deltaTime;
			// Time.frameCount++; // Don't increment here, let's keep it steady for the frame

			var mbs = Object._allObjects.OfType<MonoBehaviour>().ToList();

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

			// FixedUpdate
			foreach (var mb in mbs)
			{
				if (mb.isActiveAndEnabled)
					mb.InternalFixedUpdate();
			}

			// Update
			foreach (var mb in mbs)
			{
				if (mb.isActiveAndEnabled)
					mb.InternalUpdate();
			}

			// LateUpdate
			foreach (var mb in mbs)
			{
				if (mb.isActiveAndEnabled)
					mb.InternalLateUpdate();
			}
		}

		public static void GodotTick(Double deltaTime = 1.0 / 60.0)
		{
			// Godot_Time.SimulatedFrameCount++;
			Godot_Time.SimulatedTimeMsec += (UInt64)(deltaTime * 1000.0);

			var nodes = GodotObject._allObjects.OfType<Node>().ToList();
			foreach (var node in nodes)
			{
				if (node.IsInsideTree() && node.CanProcess())
				{
					Console.WriteLine($"[DEBUG_LOG] Ticking Godot Node: {node.Name}");
					node._PhysicsProcess(deltaTime);
					node._Process(deltaTime);
				}
			}
		}

		public static void Reset()
		{
			LunyEngine.ForceReset_UnityEditorAndUnitTestsOnly();
			LunyEngineUnityAdapter.ForceReset_UnitTestsOnly();
			LunyEngineGodotAdapter.ForceReset_UnitTestsOnly();

			Object.Reset_UnitTestsOnly();
			GodotObject.Reset_UnitTestsOnly();
			SceneTree.ForceReset_UnitTestsOnly(); // Re-creates SceneTree.Instance

			// Re-initialize Godot SceneTree and Root
			var sceneTree = SceneTree.Instance;
			// SceneTree and Root are GodotObjects, they add themselves to _allObjects in ctor
			sceneTree.Root.SetInsideTree(true); // Ensure Root is inside tree

			UnityEngine.Time.time = 0;
			UnityEngine.Time.frameCount = 0; 
			UnityEngine.Time.deltaTime = 1f / 60f;

			Godot_Time.SimulatedFrameCount = 0; 
			Godot_Time.SimulatedTimeMsec = 0;
		}
	}
}
