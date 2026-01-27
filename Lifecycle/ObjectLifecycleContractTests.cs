using NUnit.Framework;
using System.Linq;

namespace Luny.ContractTest
{
	[TestFixture]
	public class GodotObjectLifecycleTests : ObjectLifecycleContractTests
	{
		protected override NativeEngine Engine => NativeEngine.Godot;
	}

	[TestFixture]
	public class UnityObjectLifecycleTests : ObjectLifecycleContractTests
	{
		protected override NativeEngine Engine => NativeEngine.Unity;
	}

	public abstract class ObjectLifecycleContractTests : ContractTestBase
	{
		[Test]
		public void CreateObject_RegistersInRegistry()
		{
			var obj = LunyEngine.Instance.Object.CreateEmpty("TestObject");
			Assert.That(obj, Is.Not.Null);
			Assert.That(LunyEngine.Instance.Objects.AllObjects.Contains(obj), Is.True);
		}

		[Test]
		public void DestroyObject_RemovesFromRegistry()
		{
			var obj = LunyEngine.Instance.Object.CreateEmpty("ToDestroy");
			Assert.That(LunyEngine.Instance.Objects.AllObjects.Contains(obj), Is.True);

			obj.Destroy();

			// Might need a tick if destruction is deferred
			SimulateFrame();

			Assert.That(LunyEngine.Instance.Objects.AllObjects.Contains(obj), Is.False);
		}

		[Test]
		public void FindByName_ReturnsExistingObject()
		{
			var obj = LunyEngine.Instance.Object.CreateEmpty("UniqueName");
			var found = LunyEngine.Instance.Objects.FindByName("UniqueName");

			Assert.That(found, Is.EqualTo(obj));
		}
	}
}
