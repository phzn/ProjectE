using System;
using System.Collections.Generic;
using Features.Detection;
using Features.RunModes;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.PoiTravel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.NPC
{
	// Token: 0x02000F06 RID: 3846
	[DefaultExecutionOrder(-200)]
	[DisallowMultipleComponent]
	public sealed class UniversityAmbientNpcProximity : MonoBehaviour
	{
		// Token: 0x170012C5 RID: 4805
		// (get) Token: 0x0600736D RID: 29549 RVA: 0x002F1265 File Offset: 0x002EF465
		public int TrackedCount
		{
			get
			{
				return this._actors.Count;
			}
		}

		// Token: 0x170012C6 RID: 4806
		// (get) Token: 0x0600736E RID: 29550 RVA: 0x002F1274 File Offset: 0x002EF474
		public int SuspendedCount
		{
			get
			{
				int num = 0;
				foreach (NPCAmbientSuspension npcambientSuspension in this._actors)
				{
					if (npcambientSuspension != null && npcambientSuspension.IsSuspended)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x0600736F RID: 29551 RVA: 0x002F12D8 File Offset: 0x002EF4D8
		public static bool SupportsScene(string sceneName)
		{
			return sceneName == "POI - University" || sceneName == "POI - Mall";
		}

		// Token: 0x06007370 RID: 29552 RVA: 0x002F12F4 File Offset: 0x002EF4F4
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Install()
		{
			SceneManager.sceneLoaded -= UniversityAmbientNpcProximity.OnSceneLoaded;
			SceneManager.sceneLoaded += UniversityAmbientNpcProximity.OnSceneLoaded;
		}

		// Token: 0x06007371 RID: 29553 RVA: 0x002F1318 File Offset: 0x002EF518
		private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (!UniversityAmbientNpcProximity.SupportsScene(scene.name))
			{
				return;
			}
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			GameObject[] array = rootGameObjects;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].GetComponent<UniversityAmbientNpcProximity>() != null)
				{
					return;
				}
			}
			GameObject gameObject = new GameObject(scene.name + " Ambient NPC Proximity");
			SceneManager.MoveGameObjectToScene(gameObject, scene);
			UniversityAmbientNpcProximity universityAmbientNpcProximity = gameObject.AddComponent<UniversityAmbientNpcProximity>();
			array = rootGameObjects;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (SeatedRandomReaction seatedRandomReaction in array[i].GetComponentsInChildren<SeatedRandomReaction>(true))
				{
					universityAmbientNpcProximity.Register(seatedRandomReaction);
				}
			}
		}

		// Token: 0x06007372 RID: 29554 RVA: 0x002F13C0 File Offset: 0x002EF5C0
		public void Register(SeatedRandomReaction reaction)
		{
			if (reaction == null || reaction.gameObject.scene != base.gameObject.scene)
			{
				return;
			}
			NPCAmbientSuspension npcambientSuspension = reaction.GetComponent<NPCAmbientSuspension>();
			if (npcambientSuspension == null)
			{
				npcambientSuspension = reaction.gameObject.AddComponent<NPCAmbientSuspension>();
			}
			npcambientSuspension.Initialize(reaction);
			if (!this._actors.Contains(npcambientSuspension))
			{
				this._actors.Add(npcambientSuspension);
			}
		}

		// Token: 0x06007373 RID: 29555 RVA: 0x002F1434 File Offset: 0x002EF634
		private void OnEnable()
		{
			NPCSimulationPause.PauseChanged -= this.OnPauseChanged;
			NPCSimulationPause.PauseChanged += this.OnPauseChanged;
			PlayerActivityGate.ResourcesChanged -= this.OnResourcesChanged;
			PlayerActivityGate.ResourcesChanged += this.OnResourcesChanged;
		}

		// Token: 0x06007374 RID: 29556 RVA: 0x002F1488 File Offset: 0x002EF688
		private void OnDisable()
		{
			NPCSimulationPause.PauseChanged -= this.OnPauseChanged;
			PlayerActivityGate.ResourcesChanged -= this.OnResourcesChanged;
			if (this._director != null)
			{
				this._director.OnEncounterStarted -= this.RestoreAll;
			}
			this._director = null;
			this.RestoreAll();
		}

		// Token: 0x06007375 RID: 29557 RVA: 0x002F14E9 File Offset: 0x002EF6E9
		private void OnPauseChanged(bool paused)
		{
			if (paused)
			{
				this.RestoreAll();
			}
			this._nextCheck = 0f;
		}

		// Token: 0x06007376 RID: 29558 RVA: 0x002F14FF File Offset: 0x002EF6FF
		private void OnResourcesChanged(PlayerContextResource resources)
		{
			if ((resources & (PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation)) == (PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation))
			{
				this.RestoreAll();
			}
			this._nextCheck = 0f;
		}

		// Token: 0x06007377 RID: 29559 RVA: 0x002F151C File Offset: 0x002EF71C
		private bool IsPresentationBusy()
		{
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (NPCSimulationPause.IsPaused || (PlayerActivityGate.ActiveResources & (PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation)) == (PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation) || (SimpleDialogueManager.Instance != null && SimpleDialogueManager.Instance.IsDialogueActive) || (instance != null && (instance.IsEventActive || instance.HasPendingEncounter)) || PoiArrivalPresentation.IsPresentingInScene(base.gameObject.scene))
			{
				return true;
			}
			LongWayHomeUniversitySequence presenterForScene = LongWayHomeUniversitySequence.GetPresenterForScene(base.gameObject.scene);
			return presenterForScene != null && presenterForScene.OwnsDestinationReveal;
		}

		// Token: 0x06007378 RID: 29560 RVA: 0x002F15AC File Offset: 0x002EF7AC
		private void Update()
		{
			if (this._director != ExhibitionEventDirector.Instance)
			{
				if (this._director != null)
				{
					this._director.OnEncounterStarted -= this.RestoreAll;
				}
				this._director = ExhibitionEventDirector.Instance;
				if (this._director != null)
				{
					this._director.OnEncounterStarted += this.RestoreAll;
				}
			}
			if (this._player == null || !this._player.gameObject.activeInHierarchy)
			{
				this._player = ((GameSystemsBootstrapper.Instance != null) ? GameSystemsBootstrapper.Instance.Player : null);
				if (this._player == null)
				{
					GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
					this._player = ((gameObject != null) ? gameObject.transform : null);
				}
			}
			if (this._camera == null || !this._camera.isActiveAndEnabled)
			{
				this._camera = Camera.main;
			}
			Transform transform = ((this._camera != null) ? this._camera.transform : null);
			if (this._player == null || this.IsPresentationBusy())
			{
				this.RestoreAll();
				return;
			}
			StealthActor stealthActor = ((SensoryManager.Instance != null) ? SensoryManager.Instance.GetPlayer() : null);
			float num = ((stealthActor != null) ? stealthActor.FinalNoiseRadius : 0f);
			if (Time.unscaledTime < this._nextCheck && num <= this._lastNoise && (this._player.position - this._lastPlayerPosition).sqrMagnitude < 4f && (transform == null || (transform.position - this._lastCameraPosition).sqrMagnitude < 4f))
			{
				return;
			}
			this._nextCheck = Time.unscaledTime + 0.25f;
			this._lastNoise = num;
			this._lastPlayerPosition = this._player.position;
			this._lastCameraPosition = ((transform != null) ? transform.position : this._lastPlayerPosition);
			this.Evaluate(this._player, transform, num);
		}

		// Token: 0x06007379 RID: 29561 RVA: 0x002F17E0 File Offset: 0x002EF9E0
		public void Evaluate(Transform player, Transform viewCamera, float playerNoiseRadius)
		{
			if (player == null || this.IsPresentationBusy())
			{
				this.RestoreAll();
				return;
			}
			int num = 0;
			Vector3 position = player.position;
			Vector3 vector = ((viewCamera != null) ? viewCamera.position : position);
			foreach (NPCAmbientSuspension npcambientSuspension in this._actors)
			{
				if (!(npcambientSuspension == null))
				{
					float num2 = Mathf.Max(25f, Mathf.Max(npcambientSuspension.AwarenessRadius, playerNoiseRadius) + 3f);
					float num3 = (npcambientSuspension.IsSuspended ? num2 : Mathf.Max(32f, num2 + 32f - 25f));
					Vector3 position2 = npcambientSuspension.transform.position;
					bool flag = (position - position2).sqrMagnitude > num3 * num3 && (vector - position2).sqrMagnitude > num3 * num3 && !npcambientSuspension.HasGameplayOwnership;
					if (flag && !npcambientSuspension.IsSuspended)
					{
						if (num >= 4)
						{
							this._nextCheck = 0f;
						}
						else
						{
							npcambientSuspension.SetSuspended(true);
							if (npcambientSuspension.IsSuspended)
							{
								num++;
							}
						}
					}
					else
					{
						npcambientSuspension.SetSuspended(flag);
					}
				}
			}
		}

		// Token: 0x0600737A RID: 29562 RVA: 0x002F195C File Offset: 0x002EFB5C
		public void RestoreAll()
		{
			foreach (NPCAmbientSuspension npcambientSuspension in this._actors)
			{
				if (npcambientSuspension != null)
				{
					npcambientSuspension.SetSuspended(false);
				}
			}
		}

		// Token: 0x040076AD RID: 30381
		public const string UniversityScene = "POI - University";

		// Token: 0x040076AE RID: 30382
		public const string MallScene = "POI - Mall";

		// Token: 0x040076AF RID: 30383
		public const float WakeDistance = 25f;

		// Token: 0x040076B0 RID: 30384
		public const float SleepDistance = 32f;

		// Token: 0x040076B1 RID: 30385
		private const float CheckInterval = 0.25f;

		// Token: 0x040076B2 RID: 30386
		private const float AwarenessPadding = 3f;

		// Token: 0x040076B3 RID: 30387
		public const int MaxSleepsPerEvaluation = 4;

		// Token: 0x040076B4 RID: 30388
		private readonly List<NPCAmbientSuspension> _actors = new List<NPCAmbientSuspension>(100);

		// Token: 0x040076B5 RID: 30389
		private Transform _player;

		// Token: 0x040076B6 RID: 30390
		private Camera _camera;

		// Token: 0x040076B7 RID: 30391
		private float _nextCheck;

		// Token: 0x040076B8 RID: 30392
		private float _lastNoise;

		// Token: 0x040076B9 RID: 30393
		private Vector3 _lastPlayerPosition;

		// Token: 0x040076BA RID: 30394
		private Vector3 _lastCameraPosition;

		// Token: 0x040076BB RID: 30395
		private ExhibitionEventDirector _director;
	}
}
