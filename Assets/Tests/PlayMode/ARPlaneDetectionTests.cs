using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.XR.ARFoundation;

namespace ARCoreApp.Tests.PlayMode
{
    /// <summary>
    /// Play Mode tests for AR plane detection and ARFoundation components
    /// Note: These tests verify component creation and basic functionality
    /// without requiring an active AR session or device.
    /// </summary>
    public class ARPlaneDetectionTests
    {
        private GameObject _testGameObject;
        private ARPlaneManager _planeManager;

        [SetUp]
        public void Setup()
        {
            // Setup AR plane manager for testing
            _testGameObject = new GameObject("Test AR Components");
            _planeManager = _testGameObject.AddComponent<ARPlaneManager>();
        }

        [TearDown]
        public void Teardown()
        {
            // Cleanup test objects
            if (_testGameObject != null)
            {
                Object.DestroyImmediate(_testGameObject);
            }
        }

        [UnityTest]
        public IEnumerator ARPlaneManager_Initializes()
        {
            // Wait one frame for initialization
            yield return null;

            Assert.IsNotNull(_planeManager, "ARPlaneManager should be initialized");
            Assert.IsTrue(_planeManager.enabled, "ARPlaneManager should be enabled");
        }

        [Test]
        public void ARPlaneManager_HasCorrectType()
        {
            // Verify the component is of correct type
            Assert.IsInstanceOf<ARPlaneManager>(_planeManager,
                "Component should be instance of ARPlaneManager");
        }

        [Test]
        public void ARPlaneManager_TrackablesInitiallyEmpty()
        {
            // Verify trackables collection is initially empty
            Assert.IsNotNull(_planeManager.trackables,
                "ARPlaneManager trackables collection should not be null");
            Assert.AreEqual(0, _planeManager.trackables.count,
                "ARPlaneManager should start with no detected planes");
        }

        [Test]
        public void ARPlaneManager_CanBeDisabled()
        {
            // Test that plane manager can be disabled
            _planeManager.enabled = false;
            Assert.IsFalse(_planeManager.enabled, "ARPlaneManager should be disabled");

            _planeManager.enabled = true;
            Assert.IsTrue(_planeManager.enabled, "ARPlaneManager should be re-enabled");
        }

        [UnityTest]
        public IEnumerator TestObjectLifecycle_CreatesAndDestroysCorrectly()
        {
            // Test that our test setup and teardown work correctly
            Assert.IsNotNull(_testGameObject, "Test GameObject should exist during test");
            Assert.IsNotNull(_planeManager, "PlaneManager should exist during test");

            yield return null;

            // After yield, objects should still exist until TearDown
            Assert.IsNotNull(_testGameObject, "Test GameObject should persist through yield");
        }

        [Test]
        public void ARPlaneManager_DefaultPlanePrefabIsNull()
        {
            // Verify default state
            Assert.IsNull(_planeManager.planePrefab,
                "Plane prefab should be null by default until configured");
        }

        [UnityTest]
        public IEnumerator GameObject_WithARPlaneManager_ExistsInScene()
        {
            // Verify the test GameObject is properly added to scene
            yield return null;

            var foundObject = GameObject.Find("Test AR Components");
            Assert.IsNotNull(foundObject, "Test GameObject should be findable in scene");
            Assert.AreSame(_testGameObject, foundObject, "Found object should be the same as created object");
        }
    }
}
