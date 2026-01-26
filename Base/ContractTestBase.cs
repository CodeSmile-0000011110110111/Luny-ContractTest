using NUnit.Framework;
using System;

namespace Luny.ContractTest.Base
{
	public abstract class ContractTestBase
	{
		protected abstract EngineType Engine { get; }

		[SetUp]
		public virtual void Setup()
		{
			EngineSimulator.Reset();
			InitializeEngine();
		}

		[TearDown]
		public virtual void Teardown() => EngineSimulator.Reset();

		protected abstract void InitializeEngine();

		protected void Tick(Double delta = 1.0 / 60.0) => EngineSimulator.Tick(Engine, delta);

		protected void RunFrames(Int32 count, Double delta = 1.0 / 60.0) => EngineSimulator.RunFrames(Engine, count, delta);

		protected void Quit() => EngineSimulator.Reset();
	}
}
