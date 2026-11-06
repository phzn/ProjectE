using System;
using System.Collections.Generic;
using Features.Rank;
using Features.RunModes;
using ProjectExhibitionist.PoiTravel;
using UnityEngine;
using UnityEngine.SceneManagement;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000B9D RID: 2973
	[DisallowMultipleComponent]
	public class ChunkManagerVehicleProvider : MonoBehaviour, IChunkProvider
	{
		// Token: 0x1400008F RID: 143
		// (add) Token: 0x06005555 RID: 21845 RVA: 0x00211B14 File Offset: 0x0020FD14
		// (remove) Token: 0x06005556 RID: 21846 RVA: 0x00211B4C File Offset: 0x0020FD4C
		public event Action<ChunkInfo> OnChunkLoaded;

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x06005557 RID: 21847 RVA: 0x00211B84 File Offset: 0x0020FD84
		// (remove) Token: 0x06005558 RID: 21848 RVA: 0x00211BBC File Offset: 0x0020FDBC
		public event Action<ChunkInfo> OnChunkUnloaded;

		// Token: 0x06005559 RID: 21849 RVA: 0x00211BF1 File Offset: 0x0020FDF1
		private void Awake()
		{
			if (this.spawnerToClearOnTeardown == null)
			{
				this.spawnerToClearOnTeardown = base.GetComponent<VehicleSpawner>();
			}
		}

		// Token: 0x0600555A RID: 21850 RVA: 0x00211C10 File Offset: 0x0020FE10
		private void OnEnable()
		{
			SceneManager.sceneUnloaded += this.HandleSceneUnloaded;
			SceneManager.activeSceneChanged += this.HandleActiveSceneChanged;
			RankProgressionService.Changed += this.ApplyRankPolicy;
			RunModeService.Changed += this.ApplyRankPolicy;
			this.ApplyRankPolicy();
		}

		// Token: 0x0600555B RID: 21851 RVA: 0x00211C68 File Offset: 0x0020FE68
		private void OnDisable()
		{
			SceneManager.sceneUnloaded -= this.HandleSceneUnloaded;
			SceneManager.activeSceneChanged -= this.HandleActiveSceneChanged;
			RankProgressionService.Changed -= this.ApplyRankPolicy;
			RunModeService.Changed -= this.ApplyRankPolicy;
			this.ClearTrafficRoutes();
		}

		// Token: 0x0600555C RID: 21852 RVA: 0x00211CBF File Offset: 0x0020FEBF
		private void Update()
		{
			this.ApplyRankPolicy();
			if (this._carSpawningBlocked)
			{
				return;
			}
			if (!this._isBound)
			{
				this.TryBindChunkManager();
			}
		}

		// Token: 0x0600555D RID: 21853 RVA: 0x00211CDE File Offset: 0x0020FEDE
		private void ApplyRankPolicy()
		{
			this.ApplyRankPolicy(ChunkManagerVehicleProvider.IsTrafficBlocked());
		}

		// Token: 0x0600555E RID: 21854 RVA: 0x00211CEC File Offset: 0x0020FEEC
		private static bool IsTrafficBlocked()
		{
			ChunkManager instance = ChunkManager.Instance;
			return ChunkManagerVehicleProvider.ShouldBlockTraffic(SceneManager.GetActiveScene().name, GuidedRankGameplayPolicy.CarSpawningBlocked, instance != null && instance.IsPoiTravelSuspended, instance != null && instance.IsTearingDown);
		}

		// Token: 0x0600555F RID: 21855 RVA: 0x00211D3A File Offset: 0x0020FF3A
		public static bool ShouldBlockTraffic(string sceneName, bool rankBlocked, bool suspended, bool tearingDown)
		{
			return rankBlocked || suspended || tearingDown || PoiTravelState.IsPoiSceneName(sceneName);
		}

		// Token: 0x06005560 RID: 21856 RVA: 0x00211D4B File Offset: 0x0020FF4B
		private void HandleActiveSceneChanged(Scene previous, Scene current)
		{
			this.ApplyRankPolicy();
		}

		// Token: 0x06005561 RID: 21857 RVA: 0x00211D53 File Offset: 0x0020FF53
		private void ApplyRankPolicy(bool blocked)
		{
			if (this._carSpawningBlocked == blocked)
			{
				return;
			}
			this._carSpawningBlocked = blocked;
			if (blocked)
			{
				this.ClearTrafficRoutes();
				return;
			}
			this.TryBindChunkManager();
		}

		// Token: 0x06005562 RID: 21858 RVA: 0x00211D78 File Offset: 0x0020FF78
		private void TryBindChunkManager()
		{
			if (ChunkManagerVehicleProvider.IsTrafficBlocked())
			{
				return;
			}
			if (this._isBound)
			{
				return;
			}
			if (ChunkManager.Instance == null || !ChunkManager.Instance.isActiveAndEnabled)
			{
				return;
			}
			this._chunkManager = ChunkManager.Instance;
			this._chunkManager.OnChunkSpawned += this.HandleChunkLoaded;
			this._chunkManager.OnChunkActivated += this.HandleChunkLoaded;
			this._chunkManager.OnChunkRemoved += this.HandleChunkUnloaded;
			this._chunkManager.OnChunkDeactivated += this.HandleChunkUnloaded;
			this._isBound = true;
			int num = 0;
			foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._chunkManager.SpawnedChunks)
			{
				ChunkInstance value = keyValuePair.Value;
				if (value != null && !(value.chunkRoot == null) && value.chunkRoot.activeInHierarchy)
				{
					int count = this._loadedIds.Count;
					this.HandleChunkLoaded(value);
					if (this._loadedIds.Count > count)
					{
						num++;
					}
				}
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[VSP-Provider] Bind OK. SpawnedChunks={0}, ", this._chunkManager.SpawnedChunks.Count) + string.Format("catch-up con ChunkData={0} chunk segnalati come loaded.", num));
			}
		}

		// Token: 0x06005563 RID: 21859 RVA: 0x00211EEC File Offset: 0x002100EC
		private void UnbindChunkManager()
		{
			if (this._chunkManager != null)
			{
				this._chunkManager.OnChunkSpawned -= this.HandleChunkLoaded;
				this._chunkManager.OnChunkActivated -= this.HandleChunkLoaded;
				this._chunkManager.OnChunkRemoved -= this.HandleChunkUnloaded;
				this._chunkManager.OnChunkDeactivated -= this.HandleChunkUnloaded;
			}
			this._isBound = false;
			this._chunkManager = null;
		}

		// Token: 0x06005564 RID: 21860 RVA: 0x00211F71 File Offset: 0x00210171
		private void OnDestroy()
		{
			this.UnbindChunkManager();
		}

		// Token: 0x06005565 RID: 21861 RVA: 0x00211F79 File Offset: 0x00210179
		public IEnumerable<ChunkInfo> GetActiveChunks()
		{
			if (ChunkManagerVehicleProvider.IsTrafficBlocked())
			{
				yield break;
			}
			if (ChunkManager.Instance == null)
			{
				yield break;
			}
			foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in ChunkManager.Instance.SpawnedChunks)
			{
				ChunkInstance value = keyValuePair.Value;
				ChunkInfo chunkInfo;
				if (value != null && !(value.chunkRoot == null) && value.chunkRoot.activeInHierarchy && ChunkManagerVehicleProvider.TryBuildInfo(value.chunkRoot, out chunkInfo))
				{
					this._loadedIds.Add(chunkInfo.id);
					yield return chunkInfo;
				}
			}
			IEnumerator<KeyValuePair<Vector2Int, ChunkInstance>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06005566 RID: 21862 RVA: 0x00211F8C File Offset: 0x0021018C
		private void HandleChunkLoaded(ChunkInstance chunk)
		{
			if (ChunkManagerVehicleProvider.IsTrafficBlocked())
			{
				return;
			}
			if (chunk == null || chunk.chunkRoot == null)
			{
				return;
			}
			if (!chunk.chunkRoot.activeInHierarchy)
			{
				return;
			}
			ChunkInfo chunkInfo;
			if (!ChunkManagerVehicleProvider.TryBuildInfo(chunk.chunkRoot, out chunkInfo))
			{
				return;
			}
			if (!this._loadedIds.Add(chunkInfo.id))
			{
				return;
			}
			Action<ChunkInfo> onChunkLoaded = this.OnChunkLoaded;
			if (onChunkLoaded == null)
			{
				return;
			}
			onChunkLoaded(chunkInfo);
		}

		// Token: 0x06005567 RID: 21863 RVA: 0x00211FF8 File Offset: 0x002101F8
		private void HandleChunkUnloaded(ChunkInstance chunk)
		{
			if (chunk == null || chunk.chunkRoot == null)
			{
				return;
			}
			string text = chunk.chunkRoot.GetInstanceID().ToString();
			if (!this._loadedIds.Remove(text))
			{
				return;
			}
			Action<ChunkInfo> onChunkUnloaded = this.OnChunkUnloaded;
			if (onChunkUnloaded == null)
			{
				return;
			}
			onChunkUnloaded(new ChunkInfo
			{
				id = text,
				chunkObject = chunk.chunkRoot,
				bounds = new Bounds(chunk.chunkRoot.transform.position, Vector3.one * 50f)
			});
		}

		// Token: 0x06005568 RID: 21864 RVA: 0x00212094 File Offset: 0x00210294
		public bool DebugForceLoadChunkRoot(GameObject chunkRoot)
		{
			if (ChunkManagerVehicleProvider.IsTrafficBlocked())
			{
				return false;
			}
			if (chunkRoot == null)
			{
				return false;
			}
			ChunkInfo chunkInfo;
			if (!ChunkManagerVehicleProvider.TryBuildInfo(chunkRoot, out chunkInfo))
			{
				return false;
			}
			if (!this._loadedIds.Add(chunkInfo.id))
			{
				return false;
			}
			Action<ChunkInfo> onChunkLoaded = this.OnChunkLoaded;
			if (onChunkLoaded != null)
			{
				onChunkLoaded(chunkInfo);
			}
			return true;
		}

		// Token: 0x06005569 RID: 21865 RVA: 0x002120EC File Offset: 0x002102EC
		private static bool TryBuildInfo(GameObject chunkRoot, out ChunkInfo info)
		{
			info = default(ChunkInfo);
			if (chunkRoot.GetComponent<global::VehicleSpawnPlugin.ChunkData>() == null)
			{
				return false;
			}
			Bounds bounds = new Bounds(chunkRoot.transform.position, Vector3.one * 50f);
			Renderer[] componentsInChildren = chunkRoot.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				bounds.Encapsulate(componentsInChildren[i].bounds);
			}
			info = new ChunkInfo
			{
				id = chunkRoot.GetInstanceID().ToString(),
				chunkObject = chunkRoot,
				bounds = bounds
			};
			return true;
		}

		// Token: 0x0600556A RID: 21866 RVA: 0x0021218B File Offset: 0x0021038B
		private void HandleSceneUnloaded(Scene scene)
		{
			this.ClearTrafficRoutes();
			this.ApplyRankPolicy();
		}

		// Token: 0x0600556B RID: 21867 RVA: 0x0021219C File Offset: 0x0021039C
		private void ClearTrafficRoutes()
		{
			this.UnbindChunkManager();
			if (this.spawnerToClearOnTeardown != null)
			{
				this.spawnerToClearOnTeardown.ForceDespawnAll();
			}
			List<string> list = new List<string>(this._loadedIds);
			this._loadedIds.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				Action<ChunkInfo> onChunkUnloaded = this.OnChunkUnloaded;
				if (onChunkUnloaded != null)
				{
					onChunkUnloaded(new ChunkInfo
					{
						id = list[i]
					});
				}
			}
		}

		// Token: 0x04005574 RID: 21876
		[Tooltip("Se true, GetActiveChunks() (snapshot iniziale) include solo i chunk con root attivo. Gli eventi Activated/Deactivated correggono comunque lo stato a regime.")]
		[SerializeField]
		private bool onlyVisibleInInitialSnapshot = true;

		// Token: 0x04005575 RID: 21877
		[Tooltip("VehicleSpawner da svuotare quando la scena di gioco viene scaricata (additive-swap con _Core persistente): senza questo le auto poolate resterebbero orfane. Se lasciato vuoto viene risolto sullo stesso GameObject in Awake.")]
		[SerializeField]
		private VehicleSpawner spawnerToClearOnTeardown;

		// Token: 0x04005576 RID: 21878
		[Tooltip("Log diagnostici (bind, catch-up, load/unload chunk).")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04005577 RID: 21879
		private ChunkManager _chunkManager;

		// Token: 0x04005578 RID: 21880
		private bool _isBound;

		// Token: 0x04005579 RID: 21881
		private bool _carSpawningBlocked;

		// Token: 0x0400557A RID: 21882
		private readonly HashSet<string> _loadedIds = new HashSet<string>();
	}
}
