using System;
using System.Collections.Generic;
using Features.Detection;
using Features.RunModes;
using Features.Vehicles.Spawning;
using Invector.vCamera;
using Invector.vCharacterController;
using MagicaCloth2;
using ProjectExhibitionist.Core.SceneManagement;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.RenderingPrecision
{
	// Token: 0x02000D80 RID: 3456
	[DefaultExecutionOrder(-9900)]
	public sealed class WorldOrigin : MonoBehaviour
	{
		// Token: 0x170010D5 RID: 4309
		// (get) Token: 0x060065C2 RID: 26050 RVA: 0x00289A3A File Offset: 0x00287C3A
		// (set) Token: 0x060065C3 RID: 26051 RVA: 0x00289A41 File Offset: 0x00287C41
		public static Vector3 Offset { get; private set; }

		// Token: 0x140000AC RID: 172
		// (add) Token: 0x060065C4 RID: 26052 RVA: 0x00289A4C File Offset: 0x00287C4C
		// (remove) Token: 0x060065C5 RID: 26053 RVA: 0x00289A80 File Offset: 0x00287C80
		public static event Action<Vector3> Shifted;

		// Token: 0x060065C6 RID: 26054 RVA: 0x00289AB3 File Offset: 0x00287CB3
		public static Vector3 ToLogical(Vector3 position)
		{
			return position + WorldOrigin.Offset;
		}

		// Token: 0x060065C7 RID: 26055 RVA: 0x00289AC0 File Offset: 0x00287CC0
		public static Vector3 ToUnity(Vector3 position)
		{
			return position - WorldOrigin.Offset;
		}

		// Token: 0x060065C8 RID: 26056 RVA: 0x00289ACD File Offset: 0x00287CCD
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			WorldOrigin.Offset = Vector3.zero;
			WorldOrigin.Shifted = null;
			WorldOrigin._instance = null;
			WorldOrigin._shifting = false;
		}

		// Token: 0x060065C9 RID: 26057 RVA: 0x00289AEB File Offset: 0x00287CEB
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Install()
		{
			if (WorldOrigin._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[WorldOrigin]");
			Object.DontDestroyOnLoad(gameObject);
			WorldOrigin._instance = gameObject.AddComponent<WorldOrigin>();
		}

		// Token: 0x060065CA RID: 26058 RVA: 0x00289B18 File Offset: 0x00287D18
		private void Update()
		{
			if (Time.timeScale <= 0f || SceneManager.GetActiveScene().name != "Game - Current" || (SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.IsTransitioning) || (GameSystemsBootstrapper.Instance != null && !GameSystemsBootstrapper.GameplayReady))
			{
				return;
			}
			Transform player = PlayerRegistry.Player;
			if (player == null)
			{
				return;
			}
			if (DetectionRecoveryGate.IsPlayerRecovering)
			{
				return;
			}
			if (!WorldOriginVehicleBridge.CanShift(player.GetComponent<vThirdPersonController>()))
			{
				return;
			}
			WorldOrigin.TryCenterAround(player.position, false);
		}

		// Token: 0x170010D6 RID: 4310
		// (get) Token: 0x060065CB RID: 26059 RVA: 0x00289BAC File Offset: 0x00287DAC
		public static bool CanShift
		{
			get
			{
				return !WorldOrigin._shifting && (ChunkManager.Instance == null || ChunkManager.Instance.CanShiftWorldOrigin) && (AStarGraphManager.Instance == null || AStarGraphManager.Instance.CanShiftWorldOrigin) && WorldOriginCameraBridge.CanShift && AStarNPCController.CanShiftWorldOrigin();
			}
		}

		// Token: 0x060065CC RID: 26060 RVA: 0x00289C00 File Offset: 0x00287E00
		public static bool TryCenterAround(Vector3 position, bool force = false)
		{
			position.y = 0f;
			if (!force && position.sqrMagnitude <= 16384f)
			{
				return true;
			}
			float num = ((ChunkManager.Instance != null) ? Mathf.Max(1f, ChunkManager.Instance.CellSize) : 60f);
			return WorldOrigin.TryShift(new Vector3(-Mathf.Round(position.x / num) * num, 0f, -Mathf.Round(position.z / num) * num));
		}

		// Token: 0x060065CD RID: 26061 RVA: 0x00289C84 File Offset: 0x00287E84
		public static bool TryResetForSceneExit()
		{
			return WorldOrigin.TryShift(WorldOrigin.Offset);
		}

		// Token: 0x060065CE RID: 26062 RVA: 0x00289C90 File Offset: 0x00287E90
		private static bool TryShift(Vector3 delta)
		{
			if (delta == Vector3.zero)
			{
				return true;
			}
			if (!WorldOrigin.CanShift)
			{
				return false;
			}
			if (WorldOrigin._instance == null)
			{
				WorldOrigin.Install();
			}
			WorldOrigin._shifting = true;
			try
			{
				WorldOrigin._instance.ApplyShift(delta);
			}
			finally
			{
				WorldOrigin._shifting = false;
			}
			return true;
		}

		// Token: 0x060065CF RID: 26063 RVA: 0x00289CF4 File Offset: 0x00287EF4
		private void ApplyShift(Vector3 delta)
		{
			World defaultGameObjectInjectionWorld = World.DefaultGameObjectInjectionWorld;
			if (defaultGameObjectInjectionWorld != null && defaultGameObjectInjectionWorld.IsCreated)
			{
				defaultGameObjectInjectionWorld.EntityManager.CompleteAllTrackedJobs();
			}
			AstarPath active = AstarPath.active;
			if (active != null)
			{
				active.FlushWorkItems();
			}
			WorldOriginInstancing.PrepareForShift();
			this._targets.Clear();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				this.CollectScene(SceneManager.GetSceneAt(i));
			}
			this.CollectScene(base.gameObject.scene);
			Rigidbody[] array = Object.FindObjectsByType<Rigidbody>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			Vector3[] array2 = new Vector3[array.Length];
			for (int j = 0; j < array.Length; j++)
			{
				array2[j] = array[j].position;
			}
			foreach (Transform transform in this._targets)
			{
				transform.position += delta;
			}
			for (int k = 0; k < array.Length; k++)
			{
				array[k].position = array2[k] + delta;
			}
			WorldOrigin.Offset -= delta;
			Physics.SyncTransforms();
			AStarGraphManager instance = AStarGraphManager.Instance;
			if (instance != null)
			{
				instance.ApplyWorldOriginShift(delta);
			}
			AStarNPCController.RebaseFollowers(delta);
			SpawnedVehicleGroundSafety.ApplyWorldOriginShiftToAll(delta);
			vThirdPersonInput vThirdPersonInput = ((PlayerRegistry.Player != null) ? PlayerRegistry.Player.GetComponent<vThirdPersonInput>() : null);
			vThirdPersonCamera vThirdPersonCamera = ((vThirdPersonInput != null) ? vThirdPersonInput.tpCamera : vThirdPersonCamera.instance);
			if (vThirdPersonCamera != null)
			{
				vThirdPersonCamera.ApplyWorldOriginShift(delta);
			}
			WorldOriginCameraBridge.ApplyShift(delta);
			Action<Vector3> shifted = WorldOrigin.Shifted;
			if (shifted != null)
			{
				shifted(delta);
			}
			WorldOrigin.ShiftWorldEffects(delta);
			WorldOriginInstancing.ApplyShift(delta);
			TeamManager team = MagicaManager.Team;
			if (team != null && team.IsValid())
			{
				for (int l = 1; l < team.TeamCount; l++)
				{
					ClothProcess clothProcess = team.GetClothProcess(l);
					MagicaCloth magicaCloth = ((clothProcess != null) ? clothProcess.cloth : null);
					if (magicaCloth != null && magicaCloth.gameObject.activeInHierarchy)
					{
						magicaCloth.ResetCloth(true);
					}
				}
			}
			Physics.SyncTransforms();
		}

		// Token: 0x060065D0 RID: 26064 RVA: 0x00289F20 File Offset: 0x00288120
		private void CollectScene(Scene scene)
		{
			if (!scene.IsValid() || !scene.isLoaded)
			{
				return;
			}
			foreach (GameObject gameObject in scene.GetRootGameObjects())
			{
				this.CollectTargets(gameObject.transform);
			}
		}

		// Token: 0x060065D1 RID: 26065 RVA: 0x00289F68 File Offset: 0x00288168
		private void CollectTargets(Transform root)
		{
			if (root == base.transform)
			{
				return;
			}
			Canvas component = root.GetComponent<Canvas>();
			if (component != null && component.renderMode != RenderMode.WorldSpace)
			{
				return;
			}
			if ((root.name.StartsWith("---", StringComparison.Ordinal) && root.GetComponents<Component>().Length == 1) || root.GetComponent<WorldOriginGroup>() != null || ChunkManager.IsWorldOriginGroupingTransform(root) || root.GetComponent<NPCZoneManager>() != null || root.GetComponent<LongWayHomeRuntimeController>() != null || root.GetComponent<vObjectContainer>() != null || WorldOriginVehicleBridge.IsGroupingTransform(root))
			{
				foreach (object obj in root)
				{
					Transform transform = (Transform)obj;
					this.CollectTargets(transform);
				}
				return;
			}
			this._targets.Add(root);
		}

		// Token: 0x060065D2 RID: 26066 RVA: 0x0028A060 File Offset: 0x00288260
		private static void ShiftWorldEffects(Vector3 delta)
		{
			foreach (ParticleSystem particleSystem in Object.FindObjectsOfType<ParticleSystem>(true))
			{
				if (particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World && particleSystem.particleCount != 0)
				{
					ParticleSystem.Particle[] array2 = new ParticleSystem.Particle[particleSystem.particleCount];
					int particles = particleSystem.GetParticles(array2);
					for (int j = 0; j < particles; j++)
					{
						ParticleSystem.Particle[] array3 = array2;
						int num = j;
						array3[num].position = array3[num].position + delta;
					}
					particleSystem.SetParticles(array2, particles);
				}
			}
			foreach (LineRenderer lineRenderer in Object.FindObjectsOfType<LineRenderer>(true))
			{
				if (lineRenderer.useWorldSpace)
				{
					Vector3[] array5 = new Vector3[lineRenderer.positionCount];
					lineRenderer.GetPositions(array5);
					for (int k = 0; k < array5.Length; k++)
					{
						array5[k] += delta;
					}
					lineRenderer.SetPositions(array5);
				}
			}
			foreach (TrailRenderer trailRenderer in Object.FindObjectsOfType<TrailRenderer>(true))
			{
				Vector3[] array7 = new Vector3[trailRenderer.positionCount];
				trailRenderer.GetPositions(array7);
				for (int l = 0; l < array7.Length; l++)
				{
					array7[l] += delta;
				}
				trailRenderer.SetPositions(array7);
			}
		}

		// Token: 0x0400685F RID: 26719
		public const float RecenterDistance = 128f;

		// Token: 0x04006860 RID: 26720
		private static WorldOrigin _instance;

		// Token: 0x04006861 RID: 26721
		private static bool _shifting;

		// Token: 0x04006862 RID: 26722
		private readonly List<Transform> _targets = new List<Transform>();
	}
}
