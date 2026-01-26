using Luny.Engine;
using Luny.ContractTest.Base;
using NUnit.Framework;
using System;
using System.Linq;

namespace Luny.ContractTest
{
	[NonParallelizable]
	public abstract class LunyEngineObserverContractTests : ContractTestBase
	{
		private readonly String[] _expectedMethodCallOrder =
		{
			nameof(ILunyEngineObserver.OnEngineStartup),
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

		protected abstract void ShutdownEngine();

		[Test]
		public void Observer_LifecycleCallOrder_IsCorrect()
		{
			var observer = LunyEngine.Instance.GetObserver<MockEngineObserver>();
			RunFrames(1);
			ShutdownEngine();

			// Filter out OnSceneLoaded/Unloaded
			var actualOrder = observer.CallOrder.Where(name => _expectedMethodCallOrder.Contains(name)).ToArray();
			if (actualOrder.Length != _expectedMethodCallOrder.Length)
			{
				Console.WriteLine($"[DEBUG_LOG] Actual Call Order: {String.Join(", ", observer.CallOrder)}");
			}
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
			var updateCount = 5;
			var observer = LunyEngine.Instance.GetObserver<MockEngineObserver>();
			RunFrames(updateCount);

			foreach (var methodName in _repeatingMethods)
			{
				Assert.That(observer.CallOrder.Count(name => name == methodName), Is.EqualTo(updateCount),
					$"[{Engine}] {methodName} expected to be called exactly {updateCount} times. Actual calls: {String.Join(", ", observer.CallOrder)}");
			}
		}

		[Test]
		public void ObserverCallbacks_FrameCountInFirstFrame_IsOne()
		{
			var observer = LunyEngine.Instance.GetObserver<MockEngineObserver>();
			RunFrames(1);
			ShutdownEngine();

			var callbackNames = Enum.GetNames(typeof(EngineCallback));
			for (var i = 0; i < observer.FrameCounts.Length; i++)
			{
				var frameCount = observer.FrameCounts[i];
				Assert.That(frameCount, Is.EqualTo(1), $"[{Engine}] FrameCount is {frameCount} in {callbackNames[i]}, expected: 1. Actual calls: {String.Join(", ", observer.CallOrder)}");
			}
		}
	}
}
