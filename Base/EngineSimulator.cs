using Godot;
using System;
using System.Linq;
using UnityEngine;
using GodotTime = Godot.Time;
using UnityObject = UnityEngine.Object;
using UnityTime = UnityEngine.Time;

namespace Luny.ContractTest
{
	public static class EngineSimulator
	{
		public static void SimulateFrame(NativeEngine type, Double deltaTime = 1.0 / 60.0)
		{
			if (type == NativeEngine.Unity)
				UnityTick((Single)deltaTime);
			else
				GodotTick(deltaTime);
		}

		public static void RunFrames(NativeEngine type, Int32 count, Double deltaTime = 1.0 / 60.0)
		{
			for (var i = 0; i < count; i++)
				SimulateFrame(type, deltaTime);
		}

		public static void UnityTick(Single deltaTime = 1f / 60f)
		{
			UnityTime.frameCount++;
			UnityTime.deltaTime = deltaTime;
			UnityTime.time += deltaTime;

			var mbs = UnityObject._allObjects.OfType<MonoBehaviour>().ToList();

			// Awake => is handled by AddComponent & SetActive

			// Start
			foreach (var mb in mbs)
			{
				if (mb.isActiveAndEnabled)
					mb.InternalStart();
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
			GodotTime.SimulatedFrameCount++;
			GodotTime.SimulatedTimeMsec += (UInt64)(deltaTime * 1000.0);

			// TODO: call _Ready here

			var nodes = GodotObject._allObjects.OfType<Node>().ToList();
			foreach (var node in nodes)
			{
				if (node.IsInsideTree() && node.CanProcess())
				{
					node._PhysicsProcess(deltaTime);
					node._Process(deltaTime);
				}
			}
		}

		public static void Reset()
		{
			LunyEngine.ForceReset_UnityEditorAndUnitTestsOnly();

			// Resetting static IDs
			UnityObject.Reset_UnitTestsOnly();
			GodotObject.Reset_UnitTestsOnly();

			UnityTime.time = 0f;
			UnityTime.frameCount = 0;
			UnityTime.deltaTime = 1f / 60f;

			GodotTime.SimulatedFrameCount = 0;
			GodotTime.SimulatedTimeMsec = 0;
		}
	}
}
