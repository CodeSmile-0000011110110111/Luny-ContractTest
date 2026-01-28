using Luny.Engine;
using NUnit.Framework;
using UnityEngine;

namespace Luny.ContractTest
{
	[TestFixture]
	public sealed class UnityHierarchyTests : ContractTestBase
	{
		protected override NativeEngine Engine => NativeEngine.Unity;

		[Test]
		public void GameObject_ActiveInHierarchy_InheritsFromParent()
		{
			var parent = new GameObject("Parent");
			var child = new GameObject("Child");
			child.transform.parent = parent.transform;

			Assert.That(parent.activeInHierarchy, Is.True, "Parent should be active initially");
			Assert.That(child.activeInHierarchy, Is.True, "Child should be active initially");

			parent.SetActive(false);
			Assert.That(parent.activeInHierarchy, Is.False, "Parent should be inactive");
			Assert.That(child.activeInHierarchy, Is.False, "Child should be inactive because parent is inactive");

			parent.SetActive(true);
			Assert.That(child.activeInHierarchy, Is.True, "Child should be active again");
		}

		[Test]
		public void Transform_Position_InheritsFromParent()
		{
			var parent = new GameObject("Parent");
			var child = new GameObject("Child");
			child.transform.parent = parent.transform;

			parent.transform.localPosition = new Vector3(10, 0, 0);
			child.transform.localPosition = new Vector3(5, 0, 0);

			Assert.That(child.transform.position.x, Is.EqualTo(15f), "Child world position should be sum of parent and local");
		}
	}
}
