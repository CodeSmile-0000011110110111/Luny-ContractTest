using Luny.Unity.Engine;
using NUnit.Framework;
using System.Reflection;
using System.Linq;

namespace Luny.ContractTest
{
	[TestFixture]
	[NonParallelizable]
	public class UnityEngineObserverTests : LunyEngineObserverContractTests
	{
		protected override EngineType Engine => EngineType.Unity;

		protected override void InitializeEngine() => LunyEngineUnityAdapter.Initialize();

		protected override void ShutdownEngine()
		{
			// Invoke private Shutdown via reflection or trigger OnApplicationQuit
			var adapter = UnityEngine.Object._allObjects.OfType<LunyEngineUnityAdapter>().FirstOrDefault();
			if (adapter != null)
			{
				var method = typeof(LunyEngineUnityAdapter).GetMethod("Shutdown", BindingFlags.NonPublic | BindingFlags.Instance);
				method?.Invoke(adapter, null);
			}
		}
	}
}
