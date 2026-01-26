using Godot;
using Luny.Godot.Engine;
using NUnit.Framework;
using System.Reflection;
using System.Linq;

namespace Luny.ContractTest
{
	[TestFixture]
	[NonParallelizable]
	public class GodotEngineObserverTests : LunyEngineObserverContractTests
	{
		protected override EngineType Engine => EngineType.Godot;

		protected override void InitializeEngine()
		{
			var adapter = new LunyEngineGodotAdapter();
			SceneTree.Instance.Root.AddChild(adapter);
		}

		protected override void ShutdownEngine()
		{
			var adapters = GodotObject.GetNodes<LunyEngineGodotAdapter>().ToList();
			Assert.That(adapters.Count, Is.EqualTo(1), "Expected exactly one LunyEngineGodotAdapter in the scene tree during shutdown.");
			
			var adapter = adapters[0];
			if (adapter != null)
			{
				var method = typeof(LunyEngineGodotAdapter).GetMethod("Shutdown", BindingFlags.NonPublic | BindingFlags.Instance);
				method?.Invoke(adapter, null);
			}
		}
	}
}
