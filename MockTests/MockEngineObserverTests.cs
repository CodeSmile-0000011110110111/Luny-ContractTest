using Luny.Engine;
using NUnit.Framework;
using System;
using System.Linq;

namespace Luny.ContractTest.MockTests
{
	[TestFixture]
	public class GodotEngineObserverTests : MockEngineObserverTests
	{
		protected override NativeEngine Engine => NativeEngine.Godot;
	}

	[TestFixture]
	public class UnityEngineObserverTests : MockEngineObserverTests
	{
		protected override NativeEngine Engine => NativeEngine.Unity;
	}

	public abstract class MockEngineObserverTests : ContractTestBase
	{
		private readonly String[] _expectedMethodCallOrder =
		{
			nameof(ILunyEngineObserver.OnEngineStartup),
			nameof(ILunyEngineObserver.OnSceneLoaded),
			nameof(ILunyEngineObserver.OnEnginePreUpdate),
			nameof(ILunyEngineObserver.OnEngineFixedStep),
			nameof(ILunyEngineObserver.OnEngineUpdate),
			nameof(ILunyEngineObserver.OnEngineLateUpdate),
			nameof(ILunyEngineObserver.OnEnginePostUpdate),
			nameof(ILunyEngineObserver.OnEngineShutdown),
		};

		private readonly String[] _repeatingMethods =
		{
			nameof(ILunyEngineObserver.OnEnginePreUpdate),
			nameof(ILunyEngineObserver.OnEngineFixedStep),
			nameof(ILunyEngineObserver.OnEngineUpdate),
			nameof(ILunyEngineObserver.OnEngineLateUpdate),
			nameof(ILunyEngineObserver.OnEnginePostUpdate),
		};

		[Test]
		public void Observer_LifecycleCallOrder_IsCorrect()
		{
			var observer = LunyEngine.Instance.GetObserver<MockEngineObserver>();
			ShutdownEngine(); // shutdown guarantees one simulated frame

			// Filter out OnSceneLoaded/Unloaded
			var actualOrder = observer.CallOrder.Where(name => _expectedMethodCallOrder.Contains(name)).ToArray();
			if (actualOrder.Length != _expectedMethodCallOrder.Length)
				Console.WriteLine($"[DEBUG_LOG] Actual Call Order:\n{String.Join("\n", observer.CallOrder)}");
			Assert.That(actualOrder, Is.EqualTo(_expectedMethodCallOrder));

			foreach (var methodName in _expectedMethodCallOrder)
			{
				Assert.That(observer.CallOrder.Count(name => name == methodName), Is.EqualTo(1),
					$"{methodName} expected to be called exactly once.");
			}
		}

		[Test]
		public void Observer_UpdateCallCount_IsCorrect()
		{
			var updateCount = 3;
			var observer = LunyEngine.Instance.GetObserver<MockEngineObserver>();
			SimulateFrames(updateCount);

			var expectedUpdateCount = updateCount + 1; // accounts for the initialization frame
			foreach (var methodName in _repeatingMethods)
			{
				Assert.That(observer.RepeatingCallOrder.Count(name => name == methodName), Is.EqualTo(expectedUpdateCount),
					$"[{Engine}] {methodName} expected to be called exactly {updateCount} times. Actual calls:\n" +
					$"{String.Join("\n", observer.RepeatingCallOrder)}");
			}
		}

		[Test]
		public void ObserverCallbacks_FrameCountInFirstFrame_IsOne()
		{
			var observer = LunyEngine.Instance.GetObserver<MockEngineObserver>();
			SimulateFrames(1);
			ShutdownEngine();

			var callbackNames = Enum.GetNames(typeof(MockEngineObserver.CallbackMethod));
			for (var i = 0; i < observer.FrameCounts.Length; i++)
			{
				var frameCount = observer.FrameCounts[i];
				Assert.That(frameCount, Is.EqualTo(1),
					$"[{Engine}] FrameCount is {frameCount} in {callbackNames[i]}, expected: 1. Actual calls: {String.Join(", ", observer.CallOrder)}");
			}
		}
	}
}
