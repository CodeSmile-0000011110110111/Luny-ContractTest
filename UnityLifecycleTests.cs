using Luny.Engine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Luny.ContractTest
{
	public sealed class MockMonoBehaviour : MonoBehaviour
	{
		public List<String> Calls = new();

		private void Awake() => Calls.Add(nameof(Awake));
		private void Start() => Calls.Add(nameof(Start));
		private void FixedUpdate() => Calls.Add(nameof(FixedUpdate));
		private void Update() => Calls.Add(nameof(Update));
		private void LateUpdate() => Calls.Add(nameof(LateUpdate));
		private void OnEnable() => Calls.Add(nameof(OnEnable));
		private void OnDisable() => Calls.Add(nameof(OnDisable));
		private void OnDestroy() => Calls.Add(nameof(OnDestroy));
	}

	[TestFixture]
	public sealed class UnityLifecycleTests : ContractTestBase
	{
		protected override NativeEngine Engine => NativeEngine.Unity;

		[Test]
		public void MonoBehaviour_Lifecycle_Order()
		{
			var go = new GameObject("Test");
			var mb = go.AddComponent<MockMonoBehaviour>();

			SimulateFrame();

			Assert.That(mb.Calls, Is.EqualTo(new[] { "Awake", "OnEnable", "Start", "FixedUpdate", "Update", "LateUpdate" }));
		}

		[Test]
		public void MonoBehaviour_NotActive_NoLifecycle()
		{
			var go = new GameObject("Test");
			go.SetActive(false);
			var mb = go.AddComponent<MockMonoBehaviour>();

			SimulateFrame();

			Assert.That(mb.Calls, Is.EqualTo(new[] { "Awake" }));

			go.SetActive(true);
			SimulateFrame();

			Assert.That(mb.Calls, Is.EqualTo(new[] { "Awake", "OnEnable", "Start", "FixedUpdate", "Update", "LateUpdate" }));
		}
	}
}
