using Godot;
using Luny.Godot.Engine;
using NUnit.Framework;

namespace Luny.ContractTest
{
	[TestFixture]
	[NonParallelizable]
	public class GodotObjectLifecycleTests : ObjectLifecycleContractTests
	{
		protected override EngineType Engine => EngineType.Godot;

		protected override void InitializeEngine()
		{
			var adapter = new LunyEngineGodotAdapter();
			SceneTree.Instance.Root.AddChild(adapter);
			
			// Initialize a scene so FindObjectByName works (it looks in CurrentScene)
			SceneTree.Instance.ChangeSceneToFile("res://test.tscn");
			
			Tick();
		}
	}
}
