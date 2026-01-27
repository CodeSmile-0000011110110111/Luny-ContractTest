using Godot;
using Luny.Engine;
using Luny.Godot.Engine;
using Luny.Unity.Engine;
using NUnit.Framework;
using System;

namespace Luny.ContractTest
{
	public abstract class ContractTestBase
	{
		protected abstract NativeEngine Engine { get; }

		protected ILunyEngineNativeAdapter EngineAdapter { get; private set; }
		private bool _didShutdownEngine;

		[SetUp]
		protected void InitializeEngine()
		{
			EngineSimulator.Reset();
			_didShutdownEngine = false;

			Console.WriteLine("[0] SetUp => InitializeEngine() ...");

			switch (Engine)
			{
				case NativeEngine.Godot:
					EngineAdapter = new LunyEngineGodotAdapter();
					SceneTree.Instance.Root.AddChild((Node)EngineAdapter);
					break;
				case NativeEngine.Unity:
					EngineAdapter = LunyEngineUnityAdapter.Initialize();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(Engine), Engine, "unhandled engine type");
			}

			SimulateFrame(); // first frame runs OnEngineStartup

			Console.WriteLine($"[{LunyEngine.Instance.Time.FrameCount}] SetUp => InitializeEngine() complete.");
		}

		[TearDown]
		protected void ShutdownEngine()
		{
			if (_didShutdownEngine)
				return;

			_didShutdownEngine = true;

			var frameCount = LunyEngine.Instance?.Time?.FrameCount ?? -1;
			Console.WriteLine($"[{frameCount}] Teardown => ShutdownEngine() ...");

			var internalAdapter = (ILunyEngineNativeAdapterInternal)EngineAdapter;
			internalAdapter.SimulateQuit_UnitTestOnly();

			Console.WriteLine($"[{frameCount}] Teardown => Disposing engine adapter ...");
			if (EngineAdapter is IDisposable disposable)
				disposable.Dispose();

			Console.WriteLine($"[{frameCount}] Teardown => Resetting engine simulator ...");
			EngineSimulator.Reset();

			Console.WriteLine($"[{frameCount}] Teardown => ShutdownEngine() complete.");
		}

		protected void SimulateFrame(Double delta = 1.0 / 60.0) => EngineSimulator.SimulateFrame(Engine, delta);

		protected void SimulateFrames(Int32 count, Double delta = 1.0 / 60.0)
		{
			for (var i = 0; i < count; i++)
				EngineSimulator.SimulateFrame(Engine, delta);
		}
	}
}
