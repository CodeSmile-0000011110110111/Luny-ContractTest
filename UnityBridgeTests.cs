using Luny.Engine;
using Luny.Engine.Bridge.Enums;
using Luny.Unity.Engine;
using NUnit.Framework;
using System.Linq;
using UnityEngine;

namespace Luny.ContractTest
{
	[TestFixture]
	public sealed class UnityBridgeTests : ContractTestBase
	{
		protected override NativeEngine Engine => NativeEngine.Unity;

		[Test]
		public void UnityObjectService_CreateEmpty_RegistersInLuny()
		{
			MonoBehaviour.LogAllMethods(typeof(LunyEngineUnityAdapter));

			var engine = LunyEngine.Instance;
			Assert.That(engine, Is.Not.Null, "LunyEngine.Instance should not be null after initialization");
			Assert.That(engine.Scene.CurrentScene, Is.Not.Null);

			var obj = engine.Object.CreateEmpty("TestObject");
			Assert.That(obj, Is.Not.Null, "engine.Object.CreateEmpty should not return null");
			Assert.That(obj.Name, Is.EqualTo("TestObject"), "Object name should match");

			// Verify it exists in Luny registry
			Assert.That(engine.Objects.AllObjects.Contains(obj), Is.True, "Object should be in registry");

			// Verify it exists in Unity native world
			var nativeGo = Object._allObjects.OfType<GameObject>().FirstOrDefault(g => g.name == "TestObject");
			Assert.That(nativeGo, Is.Not.Null, "Native GameObject should exist");
		}

		[Test]
		public void UnityObjectRegistry_FindByName_FindsExistingNativeObject()
		{
			// Create a native object NOT through Luny
			var nativeGo = new GameObject("NativeOnly");

			var engine = LunyEngine.Instance;
			Assert.That(engine, Is.Not.Null, "LunyEngine.Instance should not be null");

			// FindByName should find it and register it
			var lunyObj = engine.Objects.FindByName("NativeOnly");

			Assert.That(lunyObj, Is.Not.Null, "FindByName should find the native object");
			Assert.That(lunyObj.Name, Is.EqualTo("NativeOnly"), "Object name should match native name");
			Assert.That(engine.Objects.AllObjects.Contains(lunyObj), Is.True, "Found object should be registered");
		}
	}
}
