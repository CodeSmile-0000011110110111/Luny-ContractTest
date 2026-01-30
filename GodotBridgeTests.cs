using Godot;
using Luny.Engine;
using Luny.Engine.Bridge;
using Luny.Engine.Bridge.Enums;
using NUnit.Framework;
using System.Linq;

namespace Luny.ContractTest
{
	[TestFixture]
	public sealed class GodotBridgeTests : ContractTestBase
	{
		protected override NativeEngine Engine => NativeEngine.Godot;

		[Test]
		public void GodotObjectService_CreateEmpty_RegistersInLuny()
		{
			var engine = LunyEngine.Instance;
			Assert.That(engine, Is.Not.Null);
			Assert.That(engine.Scene.CurrentScene, Is.Not.Null);

			var obj = engine.Object.CreateEmpty("TestNode");
			Assert.That(obj, Is.Not.Null);
			Assert.That(obj.Name, Is.EqualTo("TestNode"));

			// Verify in Luny
			Assert.That(engine.Objects.AllObjects.Contains(obj), Is.True);

			// Verify in Godot native world
			var nativeNode = GodotObject._allObjects.OfType<Node>().FirstOrDefault(n => n.Name == "TestNode");
			Assert.That(nativeNode, Is.Not.Null);
		}

		[Test]
		public void GodotObjectRegistry_FindByName_FindsExistingNativeNode()
		{
			// Create a native node NOT through Luny, must be in CurrentScene to be found by SceneService
			var nativeNode = new Node { Name = "NativeOnly" };
			SceneTree.Instance.CurrentScene.AddChild(nativeNode);

			var engine = LunyEngine.Instance;

			// FindByName should find it and register it
			var lunyObj = engine.Objects.FindByName("NativeOnly");

			Assert.That(lunyObj, Is.Not.Null);
			Assert.That(lunyObj.Name, Is.EqualTo("NativeOnly"));
			Assert.That(engine.Objects.AllObjects.Contains(lunyObj), Is.True);
		}

		[Test]
		public void GodotObjectService_CreateFromPrefab_Works()
		{
			var engine = LunyEngine.Instance;
			var prefabPath = "Prefabs/PlayerPrefab";

			var obj = engine.Object.CreateFromPrefab(engine.Asset.Load<ILunyPrefab>(prefabPath));

			Assert.That(obj, Is.Not.Null);
			Assert.That(obj.Name, Is.EqualTo("DefaultMockPrefabRoot"));
			Assert.That(engine.Objects.AllObjects.Contains(obj), Is.True);
			
			var nativeNode = (Node)obj.NativeObject;
			Assert.That(nativeNode.IsInsideTree(), Is.True);
		}
	}
}
