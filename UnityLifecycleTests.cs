using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Luny.ContractTest
{
	public class MockMonoBehaviour : MonoBehaviour
	{
		public List<String> Calls = new();

		private void Awake() => Calls.Add(nameof(Awake));
		private void Start() => Calls.Add(nameof(Start));
		private void Update() => Calls.Add(nameof(Update));
		private void OnEnable() => Calls.Add(nameof(OnEnable));
		private void OnDisable() => Calls.Add(nameof(OnDisable));
		private void OnDestroy() => Calls.Add(nameof(OnDestroy));
	}

	[TestFixture]
	public class UnityLifecycleTests
	{
		[SetUp]
		public void Setup() => EngineSimulator.Reset();

		[Test]
		public void MonoBehaviour_Lifecycle_Order()
		{
			var go = new GameObject("Test");
			var mb = go.AddComponent<MockMonoBehaviour>();

			EngineSimulator.UnityTick();

			Assert.That(mb.Calls, Is.EqualTo(new[] { "Awake", "OnEnable", "Start", "Update" }));
		}

		[Test]
		public void MonoBehaviour_NotActive_NoLifecycle()
		{
			var go = new GameObject("Test");
			go.SetActive(false);
			var mb = go.AddComponent<MockMonoBehaviour>();

			EngineSimulator.UnityTick();

			Assert.That(mb.Calls, Is.EqualTo(new[] { "Awake" }));

			go.SetActive(true);
			EngineSimulator.UnityTick();

			Assert.That(mb.Calls, Is.EqualTo(new[] { "Awake", "OnEnable", "Start", "Update" }));
		}
	}
}
