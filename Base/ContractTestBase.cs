using Godot;
using Luny.Engine;
using Luny.Engine.Bridge;
using Luny.Godot.Engine;
using Luny.Unity.Engine;
using NUnit.Framework;
using System;

namespace Luny.ContractTest
{
	public abstract class ContractTestBase
	{
		private Boolean _didShutdownEngine;
		protected abstract NativeEngine Engine { get; }

		protected ILunyEngineNativeAdapter EngineAdapter { get; private set; }

		[SetUp]
		protected void InitializeEngine()
		{
			EngineSimulator.Reset();
			_didShutdownEngine = false;

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

			Console.WriteLine("=================TEST SETUP DONE=======================");
		}

		[TearDown]
		protected void ShutdownEngine()
		{
			if (_didShutdownEngine)
				return;

			_didShutdownEngine = true;

			var frameCount = LunyEngine.Instance?.Time?.FrameCount ?? Int32.MaxValue;
			Console.WriteLine("==================TEST TEARDOWN========================");

			//Assert.That(frameCount > 0, $"Engine initialization failure frameCount is {frameCount}");
			if (frameCount < 1)
			{
				Console.WriteLine($"[{frameCount}] Teardown => SimulateFrame() to correctly run first frame to end");
				SimulateFrame(); // avoids startup & shutdown without running frame updates - this would never occur in engines
				frameCount = LunyEngine.Instance.Time.FrameCount;
			}

			Console.WriteLine($"[{frameCount}] Teardown => SimulateQuit() ...");
			var internalAdapter = (ILunyEngineNativeAdapterInternal)EngineAdapter;
			internalAdapter.SimulateQuit_UnitTestOnly();

			if (EngineAdapter is IDisposable disposable)
				disposable.Dispose();

			EngineSimulator.Reset();

			Console.WriteLine("Teardown => ShutdownEngine() complete.");
		}

		protected void SimulateFrame(Double delta = 1.0 / 60.0) => EngineSimulator.SimulateFrame(Engine, delta);

		protected void SimulateFrames(Int32 count, Double delta = 1.0 / 60.0)
		{
			for (var i = 0; i < count; i++)
				EngineSimulator.SimulateFrame(Engine, delta);
		}
	}
}
