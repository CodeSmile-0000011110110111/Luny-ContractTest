using Godot;
using Luny.Godot.Engine;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Luny.ContractTest
{
	public class MockNode : Node
	{
		public List<String> Calls = new();

		public override void _Ready() => Calls.Add("_Ready");
		public override void _Process(Double delta) => Calls.Add("_Process");
		public override void _ExitTree() => Calls.Add("_ExitTree");
	}

	[TestFixture]
	public class GodotLifecycleTests : ContractTestBase
	{
		protected override NativeEngine Engine => NativeEngine.Godot;

		[Test]
		public void Node_Lifecycle_Ready_Called_When_Entering_Tree()
		{
			var root = new Node { Name = "Root" };
			root.SetInsideTree(true);

			var child = new MockNode { Name = "Child" };
			Assert.That(child.Calls, Is.Empty);

			root.AddChild(child);
			Assert.That(child.Calls, Contains.Item("_Ready"));
		}

		[Test]
		public void Node_Lifecycle_Process_Called_During_Tick()
		{
			EngineSimulator.SimulateFrame(Engine);

			var root = new Node { Name = "Root" };
			root.SetInsideTree(true);
			var child = new MockNode { Name = "Child" };
			root.AddChild(child);

			EngineSimulator.GodotTick(0.016);
			Assert.That(child.Calls, Contains.Item("_Process"));
		}

		[Test]
		public void Node_Lifecycle_ExitTree_Called_When_Leaving_Tree()
		{
			EngineSimulator.SimulateFrame(Engine);

			var root = new Node { Name = "Root" };
			root.SetInsideTree(true);
			var child = new MockNode { Name = "Child" };
			root.AddChild(child);

			child.Calls.Clear();
			root.SetInsideTree(false);
			Assert.That(child.Calls, Contains.Item("_ExitTree"));
		}
	}
}
