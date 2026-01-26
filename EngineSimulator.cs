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
	public static class EngineSimulator
	{
		public static void UnityTick(Single deltaTime = 1f / 60f)
		{
			Time.deltaTime = deltaTime;
			Time.time += deltaTime;
			Time.frameCount++;

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

			// Update
			foreach (var mb in mbs)
			{
				if (mb.isActiveAndEnabled)
					mb.InternalUpdate();
			}
		}

		public static void GodotTick(Double deltaTime = 1.0 / 60.0)
		{
			Godot_Time.SimulatedFrameCount++;
			Godot_Time.SimulatedTimeMsec += (UInt64)(deltaTime * 1000.0);

			var nodes = GodotObject._allObjects.OfType<Node>().ToList();
			foreach (var node in nodes)
			{
				if (node.IsInsideTree() && node.CanProcess())
					node._Process(deltaTime);
			}
		}

		public static void Reset()
		{
			LunyEngine.ForceReset_UnityEditorAndUnitTestsOnly();
			LunyEngineUnityAdapter.ForceReset_UnitTestsOnly();
			LunyEngineGodotAdapter.ForceReset_UnitTestsOnly();

			Object.Reset_UnitTestsOnly();
			GodotObject.Reset_UnitTestsOnly();

			// Re-initialize Godot Root
			var sceneTree = SceneTree.Instance;
			GodotObject._allObjects.Add(sceneTree.Root);

			Time.time = 0;
			Time.frameCount = 0;
			Time.deltaTime = 1f / 60f;

			Godot_Time.SimulatedFrameCount = 0;
			Godot_Time.SimulatedTimeMsec = 0;
		}
	}
}
