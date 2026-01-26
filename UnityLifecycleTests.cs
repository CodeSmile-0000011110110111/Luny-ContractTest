using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace Luny.ContractTest
{
    public class MockMonoBehaviour : MonoBehaviour
    {
        public List<string> Calls = new();

        protected override void Awake() => Calls.Add("Awake");
        protected override void Start() => Calls.Add("Start");
        protected override void Update() => Calls.Add("Update");
        protected override void OnEnable() => Calls.Add("OnEnable");
        protected override void OnDisable() => Calls.Add("OnDisable");
        protected override void OnDestroy() => Calls.Add("OnDestroy");
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

            Assert.That(mb.Calls, Is.EqualTo(new[] { "OnEnable", "Awake", "Start", "Update" }));
        }

        [Test]
        public void MonoBehaviour_NotActive_NoLifecycle()
        {
            var go = new GameObject("Test");
            go.SetActive(false);
            var mb = go.AddComponent<MockMonoBehaviour>();

            EngineSimulator.UnityTick();

            Assert.That(mb.Calls, Is.Empty);

            go.SetActive(true);
            EngineSimulator.UnityTick();

            Assert.That(mb.Calls, Is.EqualTo(new[] { "OnEnable", "Awake", "Start", "Update" }));
        }
    }
}
