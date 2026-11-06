using System;
using Features.RenderingPrecision;
using Invector.vCamera;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;

namespace ProjectExhibitionist.PoiTravel
{
	// Token: 0x02000963 RID: 2403
	public static class PoiTravelState
	{
		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x060041B1 RID: 16817 RVA: 0x00186C9A File Offset: 0x00184E9A
		// (set) Token: 0x060041B2 RID: 16818 RVA: 0x00186CA1 File Offset: 0x00184EA1
		public static bool HasPendingReturn { get; private set; }

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x060041B3 RID: 16819 RVA: 0x00186CA9 File Offset: 0x00184EA9
		// (set) Token: 0x060041B4 RID: 16820 RVA: 0x00186CB0 File Offset: 0x00184EB0
		public static string SourceSceneName { get; private set; }

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x060041B5 RID: 16821 RVA: 0x00186CB8 File Offset: 0x00184EB8
		// (set) Token: 0x060041B6 RID: 16822 RVA: 0x00186CBF File Offset: 0x00184EBF
		public static PoiTravelOrigin Origin { get; private set; }

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x060041B7 RID: 16823 RVA: 0x00186CC7 File Offset: 0x00184EC7
		public static bool IsFastTravelVisit
		{
			get
			{
				return PoiTravelState.Origin == PoiTravelOrigin.HomeFastTravel;
			}
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x060041B8 RID: 16824 RVA: 0x00186CD1 File Offset: 0x00184ED1
		public static bool HasUsablePendingReturn
		{
			get
			{
				return PoiTravelState.HasPendingReturn && !PoiTravelState.IsCoreSceneName(PoiTravelState.SourceSceneName);
			}
		}

		// Token: 0x060041B9 RID: 16825 RVA: 0x00186CE9 File Offset: 0x00184EE9
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetState()
		{
			PoiTravelState.HasPendingReturn = false;
			PoiTravelState.SourceSceneName = null;
			PoiTravelState.Origin = PoiTravelOrigin.None;
			PoiTravelState._returnPosition = default(Vector3);
			PoiTravelState._returnRotation = Quaternion.identity;
			PoiTravelState._discoveryDestination = null;
			PoiTravelState._discoveryId = null;
		}

		// Token: 0x060041BA RID: 16826 RVA: 0x00186D1E File Offset: 0x00184F1E
		public static void ClearPendingReturn()
		{
			PoiTravelState.ResetState();
		}

		// Token: 0x060041BB RID: 16827 RVA: 0x00186D25 File Offset: 0x00184F25
		public static void NotifySceneArrived(string sceneName)
		{
			if (PoiTravelState.HasPendingReturn && sceneName != PoiTravelState.SourceSceneName && !PoiTravelState.IsPoiSceneName(sceneName))
			{
				PoiTravelState.ClearPendingReturn();
			}
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x00186D48 File Offset: 0x00184F48
		public static void BeginVisit(string sourceScene, Vector3 returnPosition, Quaternion returnRotation, PoiTravelOrigin origin = PoiTravelOrigin.ProceduralMap, string destinationScene = null, string discoveryId = null)
		{
			object obj = (PoiTravelState.IsCoreSceneName(sourceScene) ? null : sourceScene);
			PoiTravelState.HasPendingReturn = !string.IsNullOrWhiteSpace(obj);
			PoiTravelState.SourceSceneName = obj;
			PoiTravelState._returnPosition = ((sourceScene == "Game - Current") ? WorldOrigin.ToLogical(returnPosition) : returnPosition);
			PoiTravelState._returnRotation = returnRotation;
			PoiTravelState.Origin = (PoiTravelState.HasPendingReturn ? origin : PoiTravelOrigin.None);
			PoiTravelState._discoveryDestination = ((PoiTravelState.Origin == PoiTravelOrigin.ProceduralMap) ? destinationScene : null);
			PoiTravelState._discoveryId = ((PoiTravelState.Origin == PoiTravelOrigin.ProceduralMap) ? discoveryId : null);
			if (!PoiTravelState.HasPendingReturn)
			{
				Debug.LogError("[PoiTravelState] Refused an empty/core return scene.");
			}
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x00186DDC File Offset: 0x00184FDC
		public static bool TryConsumeNaturalDiscovery(string loadedSceneName, out string discoveryId)
		{
			discoveryId = null;
			if (!PoiTravelState.HasUsablePendingReturn || PoiTravelState.Origin != PoiTravelOrigin.ProceduralMap || string.IsNullOrWhiteSpace(PoiTravelState._discoveryId) || string.IsNullOrWhiteSpace(PoiTravelState._discoveryDestination) || !string.Equals(loadedSceneName, PoiTravelState._discoveryDestination, StringComparison.Ordinal))
			{
				return false;
			}
			discoveryId = PoiTravelState._discoveryId;
			PoiTravelState._discoveryDestination = null;
			PoiTravelState._discoveryId = null;
			return true;
		}

		// Token: 0x060041BE RID: 16830 RVA: 0x00186E37 File Offset: 0x00185037
		public static string ResolveSourceSceneName(string currentGameplayScene, string activeSceneName, string portalObjectSceneName)
		{
			if (!string.IsNullOrWhiteSpace(currentGameplayScene) && !PoiTravelState.IsCoreSceneName(currentGameplayScene))
			{
				return currentGameplayScene;
			}
			if (!string.IsNullOrWhiteSpace(activeSceneName) && !PoiTravelState.IsCoreSceneName(activeSceneName))
			{
				return activeSceneName;
			}
			if (!string.IsNullOrWhiteSpace(portalObjectSceneName) && !PoiTravelState.IsCoreSceneName(portalObjectSceneName))
			{
				return portalObjectSceneName;
			}
			return null;
		}

		// Token: 0x060041BF RID: 16831 RVA: 0x00186E70 File Offset: 0x00185070
		public static bool IsCoreSceneName(string sceneName)
		{
			return string.Equals(sceneName, "_Core", StringComparison.Ordinal);
		}

		// Token: 0x060041C0 RID: 16832 RVA: 0x00186E7E File Offset: 0x0018507E
		public static bool IsPoiSceneName(string sceneName)
		{
			return !string.IsNullOrWhiteSpace(sceneName) && sceneName.StartsWith("POI - ", StringComparison.Ordinal);
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x00186E98 File Offset: 0x00185098
		public static bool TryApplyReturnOverride(string loadedSceneName)
		{
			if (!PoiTravelState.HasUsablePendingReturn || loadedSceneName != PoiTravelState.SourceSceneName)
			{
				return false;
			}
			GameObject gameObject = PlayerRegistry.PlayerObject;
			if (gameObject == null)
			{
				gameObject = GameObject.FindGameObjectWithTag("Player");
			}
			if (gameObject == null)
			{
				Debug.LogWarning("[PoiTravelState] Return scene loaded but player was not found.");
				return false;
			}
			global::CharacterController component = gameObject.GetComponent<global::CharacterController>();
			bool flag = component != null && component.enabled;
			if (flag)
			{
				component.enabled = false;
			}
			Rigidbody component2 = gameObject.GetComponent<Rigidbody>();
			if (component2 != null)
			{
				component2.velocity = Vector3.zero;
				component2.angularVelocity = Vector3.zero;
			}
			Vector3 vector = ((loadedSceneName == "Game - Current") ? WorldOrigin.ToUnity(PoiTravelState._returnPosition) : PoiTravelState._returnPosition);
			gameObject.transform.SetPositionAndRotation(vector, PoiTravelState._returnRotation);
			if (component2 != null)
			{
				component2.position = vector;
				component2.rotation = PoiTravelState._returnRotation;
			}
			Physics.SyncTransforms();
			if (flag)
			{
				component.enabled = true;
			}
			vThirdPersonCamera vThirdPersonCamera = Object.FindObjectOfType<vThirdPersonCamera>();
			if (vThirdPersonCamera != null)
			{
				vThirdPersonCamera.ResetAngleWithoutSmooth();
			}
			PoiTravelState.ResetState();
			return true;
		}

		// Token: 0x040041A6 RID: 16806
		private static Vector3 _returnPosition;

		// Token: 0x040041A7 RID: 16807
		private static Quaternion _returnRotation;

		// Token: 0x040041A8 RID: 16808
		private static string _discoveryDestination;

		// Token: 0x040041A9 RID: 16809
		private static string _discoveryId;
	}
}
