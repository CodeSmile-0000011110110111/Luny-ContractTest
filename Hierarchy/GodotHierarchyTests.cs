using Godot;
using NUnit.Framework;

namespace Luny.ContractTest
{
	[TestFixture]
	public class GodotHierarchyTests
	{
		[SetUp]
		public void Setup() => EngineSimulator.Reset();

		[Test]
		public void Node_AddChild_UpdatesParentAndChildren()
		{
			var parent = new Node { Name = "Parent" };
			var child = new Node { Name = "Child" };

			parent.AddChild(child);

			Assert.That(child.GetParent(), Is.EqualTo(parent), "Child should have parent set");
			Assert.That(parent.GetChildren(), Contains.Item(child), "Parent should have child in its children list");
		}

		[Test]
		public void Node3D_IsVisibleInTree_InheritsFromParent()
		{
			var parent = new Node3D { Name = "Parent" };
			var child = new Node3D { Name = "Child" };
			parent.AddChild(child);

			Assert.That(parent.IsVisibleInTree(), Is.True, "Parent should be visible");
			Assert.That(child.IsVisibleInTree(), Is.True, "Child should be visible");

			parent.Visible = false;
			Assert.That(child.IsVisibleInTree(), Is.False, "Child should be invisible because parent is invisible");
		}
	}
}
