using Luny.Unity.Engine;
using NUnit.Framework;

namespace Luny.ContractTest
{
	[TestFixture]
	[NonParallelizable]
	public class UnityObjectLifecycleTests : ObjectLifecycleContractTests
	{
		protected override EngineType Engine => EngineType.Unity;

		protected override void InitializeEngine()
		{
			LunyEngineUnityAdapter.Initialize();
			Tick(); // Trigger Awake/Start
		}
	}
}
