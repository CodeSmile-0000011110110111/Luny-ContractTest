using NUnit.Framework;
using Godot;
using Luny.Engine;
using Luny.Godot.Engine;
using System.Linq;

namespace Luny.ContractTest
{
    [TestFixture]
    public class GodotBridgeTests
    {
        [SetUp]
        public void Setup() => EngineSimulator.Reset();

        [Test]
        public void GodotObjectService_CreateEmpty_RegistersInLuny()
        {
            // Similar to Unity, we need to initialize the Godot adapter
            // LunyEngineGodotAdapter in real Godot is a Node added to the tree.
            
            var adapter = new LunyEngineGodotAdapter();
            SceneTree.Instance.Root.AddChild(adapter);
            
            // Advance simulation
            EngineSimulator.GodotTick();
            
            var engine = LunyEngine.Instance;
            Assert.That(engine, Is.Not.Null);
            
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
        public void GodotObjectRegistry_GetSingleObject_FindsExistingNativeNode()
        {
            var adapter = new LunyEngineGodotAdapter();
            SceneTree.Instance.Root.AddChild(adapter);
            
            // Initialize the current scene
            SceneTree.Instance.ChangeSceneToFile("res://test.tscn");
            
            EngineSimulator.GodotTick();

            // Create a native node NOT through Luny, must be in CurrentScene to be found by SceneService
            var nativeNode = new Node { Name = "NativeOnly" };
            SceneTree.Instance.CurrentScene.AddChild(nativeNode);
            
            var engine = LunyEngine.Instance;
            
            // GetSingleObject should find it and register it
            var lunyObj = engine.Objects.FindByName("NativeOnly");
            
            Assert.That(lunyObj, Is.Not.Null);
            Assert.That(lunyObj.Name, Is.EqualTo("NativeOnly"));
            Assert.That(engine.Objects.AllObjects.Contains(lunyObj), Is.True);
        }
    }
}
