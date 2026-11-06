using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Features.Inventory;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.InsertableToys
{
	// Token: 0x0200117C RID: 4476
	[DefaultExecutionOrder(-800)]
	public sealed class InsertableToyRuntimeBootstrap : MonoBehaviour
	{
		// Token: 0x060088CE RID: 35022 RVA: 0x0037F97A File Offset: 0x0037DB7A
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			InsertableToyRuntimeBootstrap._instance = null;
		}

		// Token: 0x060088CF RID: 35023 RVA: 0x0037F982 File Offset: 0x0037DB82
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Install()
		{
			if (InsertableToyRuntimeBootstrap._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[InsertableToyRuntime]");
			Object.DontDestroyOnLoad(gameObject);
			InsertableToyRuntimeBootstrap._instance = gameObject.AddComponent<InsertableToyRuntimeBootstrap>();
		}

		// Token: 0x060088D0 RID: 35024 RVA: 0x0037F9AC File Offset: 0x0037DBAC
		private void Awake()
		{
			if (InsertableToyRuntimeBootstrap._instance != null && InsertableToyRuntimeBootstrap._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			InsertableToyRuntimeBootstrap._instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			this.LoadDefinitions();
		}

		// Token: 0x060088D1 RID: 35025 RVA: 0x0037F9EB File Offset: 0x0037DBEB
		private void OnEnable()
		{
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			SceneManager.sceneUnloaded += this.OnSceneUnloaded;
			this.RefreshChunkManagerSubscription();
			this.ScheduleLoadedScenes();
		}

		// Token: 0x060088D2 RID: 35026 RVA: 0x0037FA1C File Offset: 0x0037DC1C
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
			SceneManager.sceneUnloaded -= this.OnSceneUnloaded;
			this.SetChunkManagerSubscription(null);
			if (this._hierarchyScanRoutine != null)
			{
				base.StopCoroutine(this._hierarchyScanRoutine);
				this._hierarchyScanRoutine = null;
			}
			this._pendingHierarchyRoots.Clear();
			this._pendingHierarchyRootIds.Clear();
			this._scheduledSceneHandles.Clear();
			this._scanStack.Clear();
			this._activeHierarchyRootId = 0;
		}

		// Token: 0x060088D3 RID: 35027 RVA: 0x0037FAA0 File Offset: 0x0037DCA0
		private void Start()
		{
			this.RefreshChunkManagerSubscription();
			this.ScheduleLoadedScenes();
			this.EnsurePlayerController();
		}

		// Token: 0x060088D4 RID: 35028 RVA: 0x0037FAB4 File Offset: 0x0037DCB4
		private void Update()
		{
			this.EnsurePlayerController();
		}

		// Token: 0x060088D5 RID: 35029 RVA: 0x0037FABC File Offset: 0x0037DCBC
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			this.RefreshChunkManagerSubscription();
			this.ScheduleSceneScan(scene);
			this.EnsurePlayerController();
		}

		// Token: 0x060088D6 RID: 35030 RVA: 0x0037FAD1 File Offset: 0x0037DCD1
		private void OnSceneUnloaded(Scene scene)
		{
			this._scheduledSceneHandles.Remove(scene.handle);
			this.RefreshChunkManagerSubscription();
		}

		// Token: 0x060088D7 RID: 35031 RVA: 0x0037FAEC File Offset: 0x0037DCEC
		private void LoadDefinitions()
		{
			this._items = Resources.LoadAll<InsertableToyItemSO>("InsertableToys");
		}

		// Token: 0x060088D8 RID: 35032 RVA: 0x0037FAFE File Offset: 0x0037DCFE
		public static void NotifyHierarchyAvailable(GameObject root)
		{
			if (root == null || InsertableToyRuntimeBootstrap._instance == null)
			{
				return;
			}
			InsertableToyRuntimeBootstrap._instance.ScheduleHierarchyScan(root);
		}

		// Token: 0x060088D9 RID: 35033 RVA: 0x0037FB24 File Offset: 0x0037DD24
		private void ScheduleLoadedScenes()
		{
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				this.ScheduleSceneScan(SceneManager.GetSceneAt(i));
			}
		}

		// Token: 0x060088DA RID: 35034 RVA: 0x0037FB50 File Offset: 0x0037DD50
		private void ScheduleSceneScan(Scene scene)
		{
			if (!scene.IsValid() || !scene.isLoaded || !this._scheduledSceneHandles.Add(scene.handle))
			{
				return;
			}
			List<GameObject> list = new List<GameObject>();
			scene.GetRootGameObjects(list);
			for (int i = 0; i < list.Count; i++)
			{
				this.ScheduleHierarchyScan(list[i]);
			}
		}

		// Token: 0x060088DB RID: 35035 RVA: 0x0037FBB0 File Offset: 0x0037DDB0
		private void ScheduleHierarchyScan(GameObject root)
		{
			if (root == null)
			{
				return;
			}
			int instanceID = root.GetInstanceID();
			if (instanceID == this._activeHierarchyRootId || !this._pendingHierarchyRootIds.Add(instanceID))
			{
				return;
			}
			this._pendingHierarchyRoots.Enqueue(root);
			if (this._hierarchyScanRoutine == null && base.isActiveAndEnabled)
			{
				this._hierarchyScanRoutine = base.StartCoroutine(this.ProcessHierarchyScans());
			}
		}

		// Token: 0x060088DC RID: 35036 RVA: 0x0037FC14 File Offset: 0x0037DE14
		private IEnumerator ProcessHierarchyScans()
		{
			yield return null;
			int num = 0;
			long num2 = Stopwatch.GetTimestamp();
			while (this._pendingHierarchyRoots.Count > 0 || this._scanStack.Count > 0)
			{
				if (this._scanStack.Count == 0)
				{
					this._activeHierarchyRootId = 0;
					GameObject gameObject = this._pendingHierarchyRoots.Dequeue();
					num++;
					if (gameObject != null)
					{
						this._activeHierarchyRootId = gameObject.GetInstanceID();
						this._pendingHierarchyRootIds.Remove(this._activeHierarchyRootId);
						this._scanStack.Push(gameObject.transform);
					}
					if (InsertableToyRuntimeBootstrap.ShouldYieldHierarchyScanFrame(num, num2))
					{
						yield return null;
						num = 0;
						num2 = Stopwatch.GetTimestamp();
					}
				}
				else
				{
					using (InsertableToyRuntimeBootstrap.ScanHierarchyMarker.Auto())
					{
						Transform transform = this._scanStack.Pop();
						num++;
						if (transform != null)
						{
							this.TryConfigureSource(transform);
							for (int i = transform.childCount - 1; i >= 0; i--)
							{
								this._scanStack.Push(transform.GetChild(i));
							}
						}
					}
					if (InsertableToyRuntimeBootstrap.ShouldYieldHierarchyScanFrame(num, num2))
					{
						yield return null;
						num = 0;
						num2 = Stopwatch.GetTimestamp();
					}
				}
			}
			this._activeHierarchyRootId = 0;
			this._hierarchyScanRoutine = null;
			if (this._pendingHierarchyRoots.Count > 0 && base.isActiveAndEnabled)
			{
				this._hierarchyScanRoutine = base.StartCoroutine(this.ProcessHierarchyScans());
			}
			yield break;
		}

		// Token: 0x060088DD RID: 35037 RVA: 0x0037FC23 File Offset: 0x0037DE23
		private static bool ShouldYieldHierarchyScanFrame(int processed, long frameStartedAt)
		{
			return processed >= 512 || Stopwatch.GetTimestamp() - frameStartedAt >= InsertableToyRuntimeBootstrap.HierarchyScanBudgetTicks;
		}

		// Token: 0x060088DE RID: 35038 RVA: 0x0037FC40 File Offset: 0x0037DE40
		private void ScanHierarchy(GameObject root)
		{
			if (root == null)
			{
				return;
			}
			if (this._items == null || this._items.Length == 0)
			{
				this.LoadDefinitions();
			}
			if (this._items == null || this._items.Length == 0)
			{
				return;
			}
			using (InsertableToyRuntimeBootstrap.ScanHierarchyMarker.Auto())
			{
				this._scanStack.Clear();
				this._scanStack.Push(root.transform);
				while (this._scanStack.Count > 0)
				{
					Transform transform = this._scanStack.Pop();
					if (!(transform == null))
					{
						this.TryConfigureSource(transform);
						for (int i = transform.childCount - 1; i >= 0; i--)
						{
							this._scanStack.Push(transform.GetChild(i));
						}
					}
				}
			}
		}

		// Token: 0x060088DF RID: 35039 RVA: 0x0037FD1C File Offset: 0x0037DF1C
		private void RefreshChunkManagerSubscription()
		{
			this.SetChunkManagerSubscription(ChunkManager.Instance);
		}

		// Token: 0x060088E0 RID: 35040 RVA: 0x0037FD2C File Offset: 0x0037DF2C
		private void SetChunkManagerSubscription(ChunkManager manager)
		{
			if (this._subscribedChunkManager == manager)
			{
				return;
			}
			if (this._subscribedChunkManager != null)
			{
				this._subscribedChunkManager.OnChunkSpawned -= this.OnChunkSpawned;
				this._subscribedChunkManager.OnChunkZonesReady -= this.OnChunkZonesReady;
			}
			this._subscribedChunkManager = manager;
			if (this._subscribedChunkManager == null)
			{
				return;
			}
			this._subscribedChunkManager.OnChunkSpawned += this.OnChunkSpawned;
			this._subscribedChunkManager.OnChunkZonesReady += this.OnChunkZonesReady;
			foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._subscribedChunkManager.SpawnedChunks)
			{
				ChunkInstance value = keyValuePair.Value;
				if (((value != null) ? value.chunkRoot : null) != null)
				{
					this.ScheduleHierarchyScan(value.chunkRoot);
				}
			}
		}

		// Token: 0x060088E1 RID: 35041 RVA: 0x0037FE30 File Offset: 0x0037E030
		private void OnChunkSpawned(ChunkInstance chunk)
		{
			if (chunk != null)
			{
				this.ScheduleHierarchyScan(chunk.chunkRoot);
			}
		}

		// Token: 0x060088E2 RID: 35042 RVA: 0x0037FE44 File Offset: 0x0037E044
		private void OnChunkZonesReady(ChunkInstance chunk)
		{
			if (((chunk != null) ? chunk.spawnedZones : null) == null)
			{
				return;
			}
			for (int i = 0; i < chunk.spawnedZones.Count; i++)
			{
				this.ScheduleHierarchyScan(chunk.spawnedZones[i]);
			}
		}

		// Token: 0x060088E3 RID: 35043 RVA: 0x0037FE88 File Offset: 0x0037E088
		private void TryConfigureSource(Transform candidate)
		{
			if (candidate == null)
			{
				return;
			}
			InsertableToyItemSO insertableToyItemSO = null;
			int num = -1;
			for (int i = 0; i < this._items.Length; i++)
			{
				InsertableToyItemSO insertableToyItemSO2 = this._items[i];
				if (!(insertableToyItemSO2 == null))
				{
					IReadOnlyList<string> worldSourceNamePrefixes = insertableToyItemSO2.WorldSourceNamePrefixes;
					int num2 = 0;
					while (worldSourceNamePrefixes != null && num2 < worldSourceNamePrefixes.Count)
					{
						string text = worldSourceNamePrefixes[num2];
						if (!string.IsNullOrWhiteSpace(text) && candidate.name.StartsWith(text, StringComparison.OrdinalIgnoreCase))
						{
							bool flag = text.Length == num && insertableToyItemSO != null && string.CompareOrdinal(insertableToyItemSO2.itemId, insertableToyItemSO.itemId) < 0;
							if (text.Length > num || flag)
							{
								insertableToyItemSO = insertableToyItemSO2;
								num = text.Length;
							}
						}
						num2++;
					}
				}
			}
			if (insertableToyItemSO == null)
			{
				return;
			}
			InsertableToyPickupSource insertableToyPickupSource = candidate.GetComponent<InsertableToyPickupSource>();
			if (insertableToyPickupSource == null)
			{
				insertableToyPickupSource = candidate.gameObject.AddComponent<InsertableToyPickupSource>();
			}
			if (insertableToyPickupSource.ItemData != insertableToyItemSO)
			{
				insertableToyPickupSource.Configure(insertableToyItemSO);
			}
		}

		// Token: 0x060088E4 RID: 35044 RVA: 0x0037FF9C File Offset: 0x0037E19C
		private void EnsurePlayerController()
		{
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			if (instance == null)
			{
				this._configuredInventory = null;
				return;
			}
			if (this._configuredInventory == instance)
			{
				return;
			}
			if (instance.GetComponent<InsertableToyController>() == null)
			{
				instance.gameObject.AddComponent<InsertableToyController>();
			}
			this._configuredInventory = instance;
		}

		// Token: 0x04008B75 RID: 35701
		private const int MaxTransformsScannedPerFrame = 512;

		// Token: 0x04008B76 RID: 35702
		private const double HierarchyScanBudgetSeconds = 0.0005;

		// Token: 0x04008B77 RID: 35703
		private static readonly long HierarchyScanBudgetTicks = Math.Max(1L, (long)Math.Ceiling((double)Stopwatch.Frequency * 0.0005));

		// Token: 0x04008B78 RID: 35704
		private static readonly ProfilerMarker ScanHierarchyMarker = new ProfilerMarker("InsertableToyRuntimeBootstrap.ScanHierarchy");

		// Token: 0x04008B79 RID: 35705
		private static InsertableToyRuntimeBootstrap _instance;

		// Token: 0x04008B7A RID: 35706
		private InsertableToyItemSO[] _items;

		// Token: 0x04008B7B RID: 35707
		private PlayerInventoryManager _configuredInventory;

		// Token: 0x04008B7C RID: 35708
		private ChunkManager _subscribedChunkManager;

		// Token: 0x04008B7D RID: 35709
		private readonly Queue<GameObject> _pendingHierarchyRoots = new Queue<GameObject>(32);

		// Token: 0x04008B7E RID: 35710
		private readonly HashSet<int> _pendingHierarchyRootIds = new HashSet<int>();

		// Token: 0x04008B7F RID: 35711
		private readonly HashSet<int> _scheduledSceneHandles = new HashSet<int>();

		// Token: 0x04008B80 RID: 35712
		private readonly Stack<Transform> _scanStack = new Stack<Transform>(4096);

		// Token: 0x04008B81 RID: 35713
		private Coroutine _hierarchyScanRoutine;

		// Token: 0x04008B82 RID: 35714
		private int _activeHierarchyRootId;
	}
}
