using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Bozo;
using Bozo.AnimeCharacters;
using BoZo.CharacterCreator;
using Features.Detection;
using Features.NPC;
using Features.NPC.StateMachine;
using Pathfinding;
using Pathfinding.ECS.RVO;
using ProjectExhibitionist.CharacterCustomization.Core.Storage;
using Unity.Profiling;
using UnityEngine;

// Token: 0x020000C3 RID: 195
public class NPCRuntimeSpawner : MonoBehaviour
{
	// Token: 0x170001BB RID: 443
	// (get) Token: 0x060007B5 RID: 1973 RVA: 0x000366BC File Offset: 0x000348BC
	private int atlasSize
	{
		get
		{
			return (int)this.atlasResolution;
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x060007B6 RID: 1974 RVA: 0x000366C4 File Offset: 0x000348C4
	public bool IsPrePooling
	{
		get
		{
			return this._isPrePooling;
		}
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x060007B7 RID: 1975 RVA: 0x000366CC File Offset: 0x000348CC
	public bool IsPrePoolComplete
	{
		get
		{
			return this._prePoolComplete;
		}
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x060007B8 RID: 1976 RVA: 0x000366D4 File Offset: 0x000348D4
	public string PrePoolFailure
	{
		get
		{
			return this._prePoolFailure;
		}
	}

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x060007B9 RID: 1977 RVA: 0x000366DC File Offset: 0x000348DC
	public int PrePooledCount
	{
		get
		{
			return this._prePooledCount;
		}
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x060007BA RID: 1978 RVA: 0x000366E4 File Offset: 0x000348E4
	public int PrePoolTarget
	{
		get
		{
			if (this.UsesCivilianRoster)
			{
				return Mathf.Min(this.prePoolCount, this._npcDataList.Count);
			}
			if (!(this._populationBakedRegistry != null) || this._populationBakedRegistry.Count <= 0)
			{
				return this.prePoolCount;
			}
			return Mathf.Min(this.prePoolCount, this._populationBakedRegistry.Count);
		}
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x060007BB RID: 1979 RVA: 0x00036749 File Offset: 0x00034949
	public int AvailablePooledNPCs
	{
		get
		{
			return this._genericPool.Count;
		}
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x060007BC RID: 1980 RVA: 0x00036756 File Offset: 0x00034956
	public bool HasCharacterData
	{
		get
		{
			return this._npcDataList.Count > 0;
		}
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x060007BD RID: 1981 RVA: 0x00036768 File Offset: 0x00034968
	public bool IsPartialPreparationExhausted
	{
		get
		{
			if (this.IsTearingDown || !this._prePoolComplete || this._prePoolFailure != null || this._unavailablePrePoolIdentities.Count == 0)
			{
				return false;
			}
			using (HashSet<GameObject>.Enumerator enumerator = this._prePoolInFlightNpcs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current != null)
					{
						return false;
					}
				}
			}
			using (HashSet<GameObject>.Enumerator enumerator = this._fadingNpcs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current != null)
					{
						return false;
					}
				}
			}
			foreach (GameObject gameObject in this._genericPool)
			{
				if (this.IsUsablePreparedNPC(gameObject))
				{
					return false;
				}
			}
			foreach (CharacterObject characterObject in this._npcDataList)
			{
				if (this.IsAllowedCharacter(characterObject, false) && !this.IsUnavailablePrePoolIdentity(characterObject) && (!this.IsCivilianCharacter(characterObject) || this._residentBudget.CanReserve(this.GetCharacterIdentity(characterObject), this.IsCustomCharacter(characterObject))))
				{
					return false;
				}
			}
			return this.HasUsablePreparedNPC();
		}
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x00036904 File Offset: 0x00034B04
	private void Awake()
	{
		if (this.bakedRegistry == null && this.autoLoadRegistry)
		{
			this.bakedRegistry = Resources.Load<NPCBakedRegistry>("NPCBakedRegistry");
			this.bakedRegistry != null;
		}
		if (this.spawnConfig == null)
		{
			this.spawnConfig = Resources.Load<NPCSpawnConfig>("NPCSpawnConfig");
		}
		if (this.spawnConfig != null)
		{
			this.maxPoolSize = this.spawnConfig.maxTotalNpc;
			this.prePoolCount = Mathf.Clamp(this.spawnConfig.prePoolSize, 0, this.maxPoolSize);
			this.prePoolPerFrame = Mathf.Max(1, this.spawnConfig.prePoolMaxInstancesPerFrame);
			this.prePoolFrameBudgetMs = Mathf.Max(0.1f, this.spawnConfig.prePoolFrameBudgetMs);
		}
		this.BuildNPCDataList();
		if (this._npcDataList.Count == 0)
		{
			Debug.LogWarning("[NPCRuntimeSpawner] Nessun CharacterObject disponibile per gli NPC!");
		}
		if (this.poolContainer == null)
		{
			GameObject gameObject = new GameObject("NPCPoolContainer");
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localPosition = new Vector3(0f, -1000f, 0f);
			this.poolContainer = gameObject.transform;
			return;
		}
		if (!this.poolContainer.gameObject.activeSelf)
		{
			this.poolContainer.gameObject.SetActive(true);
		}
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x00036A68 File Offset: 0x00034C68
	private void BuildNPCDataList()
	{
		this._npcDataList.Clear();
		this.CleanupCustomCharacterObjects();
		this.RefreshPopulationBakedRegistry();
		if (this.UsesCivilianRoster)
		{
			this.BuildCivilianRoster();
			return;
		}
		int num = 0;
		if (this.npcCharacterData != null && this.npcCharacterData.Length != 0)
		{
			foreach (CharacterObject characterObject in this.npcCharacterData)
			{
				if (characterObject != null)
				{
					this._npcDataList.Add(characterObject);
					num++;
				}
			}
		}
		if (this.autoLoadNPCData || num < this.prePoolCount)
		{
			List<CharacterObject> list = null;
			if (this.preferBakedPrefabs && NPCBakedResourceIndex.Names.Count > 0)
			{
				List<string> list2 = new List<string>(NPCBakedResourceIndex.Names.Count);
				foreach (string text in NPCBakedResourceIndex.Names)
				{
					if (text.StartsWith(this.npcPrefix, StringComparison.Ordinal))
					{
						list2.Add(text);
					}
				}
				this.ShuffleList<string>(list2);
				if (list2.Count > 0)
				{
					int num2 = Mathf.Max(0, this.maxPoolSize - num);
					list = new List<CharacterObject>(Mathf.Min(num2, list2.Count));
					int num3 = 0;
					while (num3 < list2.Count && list.Count < num2)
					{
						CharacterObject characterObject2 = NPCBakedResourceIndex.LoadCharacter(list2[num3]);
						if (characterObject2 != null)
						{
							list.Add(characterObject2);
						}
						num3++;
					}
				}
			}
			NPCBakedRegistry npcbakedRegistry = ((this._populationBakedRegistry != null) ? this._populationBakedRegistry : this.bakedRegistry);
			if ((list == null || list.Count == 0) && this.preferBakedPrefabs && npcbakedRegistry != null && npcbakedRegistry.Count > 0)
			{
				list = new List<CharacterObject>(npcbakedRegistry.Count);
				foreach (NPCBakedRegistry.Entry entry in npcbakedRegistry.Entries)
				{
					if (((entry != null) ? entry.characterData : null) != null && entry.characterData.name.StartsWith(this.npcPrefix))
					{
						list.Add(entry.characterData);
					}
				}
			}
			if (list == null || list.Count == 0)
			{
				list = this.LoadNPCsFromResources();
			}
			List<CharacterObject> list3 = new List<CharacterObject>();
			foreach (CharacterObject characterObject3 in list)
			{
				if (!this._npcDataList.Contains(characterObject3))
				{
					list3.Add(characterObject3);
				}
			}
			this.ShuffleList<CharacterObject>(list3);
			int num4 = (this.autoLoadNPCData ? Mathf.Max(0, this.maxPoolSize - num) : Mathf.Max(0, this.prePoolCount - num));
			int num5 = 0;
			int num6 = 0;
			while (num6 < list3.Count && num5 < num4)
			{
				this._npcDataList.Add(list3[num6]);
				num5++;
				num6++;
			}
		}
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x00036DA4 File Offset: 0x00034FA4
	private void RefreshPopulationBakedRegistry()
	{
		if (this._populationRegistryPrefix == this.npcPrefix)
		{
			return;
		}
		this._populationRegistryPrefix = this.npcPrefix;
		this._populationBakedRegistry = null;
		if (string.IsNullOrEmpty(this.npcPrefix))
		{
			return;
		}
		string text = this.npcPrefix.TrimEnd('_');
		if (text.Length == 0)
		{
			return;
		}
		this._populationBakedRegistry = Resources.Load<NPCBakedRegistry>("NPCBakedRegistry_" + text);
		this._populationBakedRegistry != null;
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x00036E20 File Offset: 0x00035020
	private List<CharacterObject> LoadNPCsFromResources()
	{
		List<CharacterObject> list = new List<CharacterObject>();
		foreach (CharacterObject characterObject in Resources.LoadAll<CharacterObject>(""))
		{
			if (characterObject.name.StartsWith(this.npcPrefix))
			{
				list.Add(characterObject);
			}
		}
		return list;
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x00036E6C File Offset: 0x0003506C
	private void ShuffleList<T>(List<T> list)
	{
		int i = list.Count;
		while (i > 1)
		{
			i--;
			int num = Random.Range(0, i + 1);
			T t = list[num];
			list[num] = list[i];
			list[i] = t;
		}
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x00036EB4 File Offset: 0x000350B4
	private bool TryGetBakedPrefab(CharacterObject charData, out GameObject prefab)
	{
		prefab = null;
		if (!this.preferBakedPrefabs || this.IsCustomCharacter(charData))
		{
			return false;
		}
		prefab = NPCBakedResourceIndex.LoadPrefab((charData != null) ? charData.name : null);
		return prefab != null || (this._populationBakedRegistry != null && this._populationBakedRegistry.TryGet(charData, out prefab)) || (this.bakedRegistry != null && this.bakedRegistry.TryGet(charData, out prefab));
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x00036F38 File Offset: 0x00035138
	private async Task<GameObject> LoadBakedPrefabAsync(CharacterObject charData)
	{
		GameObject gameObject;
		if (!this.preferBakedPrefabs || this.IsCustomCharacter(charData))
		{
			gameObject = null;
		}
		else
		{
			GameObject gameObject2 = await NPCBakedResourceIndex.LoadPrefabAsync((charData != null) ? charData.name : null);
			if (gameObject2 != null)
			{
				gameObject = gameObject2;
			}
			else if (this._populationBakedRegistry != null && this._populationBakedRegistry.TryGet(charData, out gameObject2))
			{
				gameObject = gameObject2;
			}
			else
			{
				gameObject = ((this.bakedRegistry != null && this.bakedRegistry.TryGet(charData, out gameObject2)) ? gameObject2 : null);
			}
		}
		return gameObject;
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x00036F84 File Offset: 0x00035184
	private GameObject SpawnFromBaked(GameObject bakedPrefab, CharacterObject charData, Vector3 position, Quaternion rotation, Transform parent, NPCResidentBudget.Reservation reservation = null, bool isRole = false)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(bakedPrefab, position, rotation, parent);
		this.TrackResident(gameObject, charData, reservation, isRole);
		string text = (charData.name.StartsWith(this.npcPrefix) ? charData.name.Substring(this.npcPrefix.Length) : charData.name);
		gameObject.name = "NPC_" + text;
		OutfitSystem component = gameObject.GetComponent<OutfitSystem>();
		if (component != null)
		{
			component.characterData = charData;
		}
		if (gameObject.GetComponent<NPCSpawnFade>() == null)
		{
			gameObject.AddComponent<NPCSpawnFade>();
		}
		try
		{
			this.PreConfigureNPCComponents(gameObject, charData);
		}
		catch
		{
			this.DestroyTrackedNPC(gameObject);
			throw;
		}
		return gameObject;
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x00037040 File Offset: 0x00035240
	private void Start()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.enablePrePooling)
		{
			this.StartPrePooling();
			return;
		}
		this._prePoolComplete = true;
	}

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x060007C7 RID: 1991 RVA: 0x00037064 File Offset: 0x00035264
	// (remove) Token: 0x060007C8 RID: 1992 RVA: 0x0003709C File Offset: 0x0003529C
	public event Action OnPoolReady;

	// Token: 0x060007C9 RID: 1993 RVA: 0x000370D4 File Offset: 0x000352D4
	public void StartPrePooling()
	{
		if (this._isPrePooling || this._prePoolComplete)
		{
			return;
		}
		this._isPrePooling = true;
		this._prePoolFailure = null;
		this._prePooledCount = 0;
		int num = this._prePoolGeneration + 1;
		this._prePoolGeneration = num;
		int num2 = num;
		this.ClearUnavailablePrePoolIdentities();
		this.PrePoolAllNPCsAsync(num2);
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x00037128 File Offset: 0x00035328
	private async Task PrePoolAllNPCsAsync(int generation)
	{
		double prePoolStartedAt = Time.realtimeSinceStartupAsDouble;
		int targetCount = 0;
		List<Task<GameObject>> pendingLoads = new List<Task<GameObject>>();
		bool loggedStart = false;
		object obj = null;
		int num = 0;
		try
		{
			try
			{
				targetCount = this.PrePoolTarget;
				if (this._npcDataList.Count == 0 || targetCount <= 0)
				{
					goto IL_062A;
				}
				int destructionFrame = Time.frameCount;
				for (;;)
				{
					await Task.Yield();
					if (!this.IsPrePoolRunCurrent(generation))
					{
						break;
					}
					if (Time.frameCount != destructionFrame)
					{
						goto Block_9;
					}
				}
				goto IL_062A;
				Block_9:
				double catalogWaitStartedAt = Time.realtimeSinceStartupAsDouble;
				TaskAwaiter<bool> taskAwaiter = this.WaitForMapCatalogAsync(generation).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					goto IL_062A;
				}
				prePoolStartedAt = Time.realtimeSinceStartupAsDouble;
				targetCount = this.PrePoolTarget;
				if (this._npcDataList.Count == 0 || targetCount <= 0)
				{
					goto IL_062A;
				}
				double num2 = prePoolStartedAt - catalogWaitStartedAt;
				Debug.Log(string.Format("[NPCRuntimeSpawner] Pre-pool start: {0} NPC", targetCount) + ((num2 >= 0.05) ? string.Format(" after waiting {0:F1}s for the map catalog", num2) : string.Empty) + string.Format(", t={0:F1}s", Time.realtimeSinceStartup));
				loggedStart = true;
				int processedThisFrame = 0;
				long batchStart = Stopwatch.GetTimestamp();
				for (int i = 0; i < targetCount; i++)
				{
					if (!this.IsPrePoolRunCurrent(generation))
					{
						goto IL_062A;
					}
					CharacterObject characterObject = this._npcDataList[i % this._npcDataList.Count];
					using (NPCRuntimeSpawner.PrePoolBakedBatchMarker.Auto())
					{
						Task<GameObject> task = this.CreateNPCInstanceAsync(characterObject, new Vector3(0f, -1000f, 0f), Quaternion.identity, this.poolContainer, null, true, generation, false, true);
						if (task.IsCompleted)
						{
							TaskAwaiter<GameObject> taskAwaiter3 = task.GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								await taskAwaiter3;
								TaskAwaiter<GameObject> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<GameObject>);
							}
							if (taskAwaiter3.GetResult() != null)
							{
								this._prePooledCount++;
							}
						}
						else
						{
							pendingLoads.Add(task);
						}
					}
					ProfilerMarker.AutoScope autoScope = default(ProfilerMarker.AutoScope);
					double num3 = (double)(Stopwatch.GetTimestamp() - batchStart) * 1000.0 / (double)Stopwatch.Frequency;
					if (++processedThisFrame >= Mathf.Max(1, this.prePoolPerFrame) || num3 >= (double)Mathf.Max(0.1f, this.prePoolFrameBudgetMs))
					{
						await Task.Yield();
						if (!this.IsPrePoolRunCurrent(generation))
						{
							goto IL_062A;
						}
						processedThisFrame = 0;
						batchStart = Stopwatch.GetTimestamp();
					}
					if (pendingLoads.Count >= 4)
					{
						GameObject[] array = await Task.WhenAll<GameObject>(pendingLoads);
						if (!this.IsPrePoolRunCurrent(generation))
						{
							goto IL_062A;
						}
						for (int j = 0; j < array.Length; j++)
						{
							if (array[j] != null)
							{
								this._prePooledCount++;
							}
						}
						pendingLoads.Clear();
					}
				}
				GameObject[] array2 = await Task.WhenAll<GameObject>(pendingLoads);
				if (!this.IsPrePoolRunCurrent(generation))
				{
					goto IL_062A;
				}
				GameObject[] array3 = array2;
				for (int j = 0; j < array3.Length; j++)
				{
					if (array3[j] != null)
					{
						this._prePooledCount++;
					}
				}
				pendingLoads.Clear();
			}
			catch (Exception ex)
			{
				this.RecordPrePoolFailure(generation, ex.Message);
				Debug.LogWarning("[NPCRuntimeSpawner] Pre-pool failed: " + ex.Message);
			}
			goto IL_063F;
			IL_062A:
			num = 1;
		}
		catch (object obj)
		{
		}
		IL_063F:
		await this.DrainPrePoolLoadsAsync(generation, pendingLoads);
		this.CompletePrePoolRun(generation, targetCount, prePoolStartedAt, loggedStart);
		object obj2 = obj;
		if (obj2 != null)
		{
			Exception ex2 = obj2 as Exception;
			if (ex2 == null)
			{
				throw obj2;
			}
			ExceptionDispatchInfo.Capture(ex2).Throw();
		}
		if (num != 1)
		{
			obj = null;
		}
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x00037174 File Offset: 0x00035374
	private async Task DrainPrePoolLoadsAsync(int generation, List<Task<GameObject>> pendingLoads)
	{
		try
		{
			await Task.WhenAll<GameObject>(pendingLoads);
		}
		catch (Exception ex)
		{
			this.RecordPrePoolFailure(generation, ex.Message);
		}
		finally
		{
			pendingLoads.Clear();
		}
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x000371C7 File Offset: 0x000353C7
	private void RecordPrePoolFailure(int generation, string reason)
	{
		if (this.IsPrePoolRunCurrent(generation) && this._prePoolFailure == null)
		{
			this._prePoolFailure = (string.IsNullOrEmpty(reason) ? "NPC preparation failed." : reason);
		}
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x000371F0 File Offset: 0x000353F0
	private void RecordUnavailablePrePoolIdentity(int generation, CharacterObject character, string reason)
	{
		if (!this.IsPrePoolRunCurrent(generation))
		{
			return;
		}
		string characterIdentity = this.GetCharacterIdentity(character);
		if (string.IsNullOrEmpty(characterIdentity))
		{
			this.RecordPrePoolFailure(generation, reason);
			return;
		}
		if (!this._unavailablePrePoolIdentities.Add(characterIdentity))
		{
			return;
		}
		if (this._firstUnavailablePrePoolReason == null)
		{
			this._firstUnavailablePrePoolReason = reason;
		}
		Debug.LogWarning("[NPCRuntimeSpawner] " + reason + " Skipping this identity for the current preparation generation.");
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x00037253 File Offset: 0x00035453
	private bool IsUnavailablePrePoolIdentity(CharacterObject character)
	{
		return this._unavailablePrePoolIdentities.Contains(this.GetCharacterIdentity(character));
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x00037267 File Offset: 0x00035467
	private void ClearUnavailablePrePoolIdentities()
	{
		this._unavailablePrePoolIdentities.Clear();
		this._firstUnavailablePrePoolReason = null;
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x0003727C File Offset: 0x0003547C
	private bool HasUsablePreparedNPC()
	{
		foreach (GameObject gameObject in this._genericPool)
		{
			if (this.IsUsablePreparedNPC(gameObject))
			{
				return true;
			}
		}
		foreach (GameObject gameObject2 in this._activeNpcs)
		{
			if (this.IsUsablePreparedNPC(gameObject2))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00037324 File Offset: 0x00035524
	private bool IsUsablePreparedNPC(GameObject npc)
	{
		if (npc == null || !this._residentNpcs.Contains(npc) || this._roleNpcs.Contains(npc) || this._prePoolInFlightNpcs.Contains(npc) || this._fadingNpcs.Contains(npc))
		{
			return false;
		}
		CharacterObject npcCharacter = this.GetNpcCharacter(npc);
		return npcCharacter != null && this.IsAllowedCharacter(npcCharacter, false);
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00037390 File Offset: 0x00035590
	private void CompletePrePoolRun(int generation, int targetCount, double startedAt, bool loggedStart)
	{
		if (this.IsPrePoolRunCurrent(generation))
		{
			if (this._prePoolFailure == null && this._unavailablePrePoolIdentities.Count > 0 && !this.HasUsablePreparedNPC())
			{
				this._prePoolFailure = "No usable NPCs remain after preparation. " + this._firstUnavailablePrePoolReason;
			}
			this._isPrePooling = false;
			this._prePoolComplete = this._prePoolFailure == null;
			if (loggedStart)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[NPCRuntimeSpawner] Pre-pool ",
					this._prePoolComplete ? "end" : "failed",
					": ",
					string.Format("{0}/{1} NPC in ", this._prePooledCount, targetCount),
					string.Format("{0:F1}s, t={1:F1}s", Time.realtimeSinceStartupAsDouble - startedAt, Time.realtimeSinceStartup)
				}));
			}
			if (this._prePoolComplete)
			{
				Action onPoolReady = this.OnPoolReady;
				if (onPoolReady == null)
				{
					return;
				}
				onPoolReady();
			}
		}
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00037487 File Offset: 0x00035687
	private bool IsPrePoolRunCurrent(int generation)
	{
		return this.IsSpawnRunCurrent(generation) && this._isPrePooling;
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x0003749C File Offset: 0x0003569C
	private async Task<bool> WaitForMapCatalogAsync(int generation)
	{
		ChunkManager catalogOwner = ChunkManager.Instance;
		double warningAt = Time.realtimeSinceStartupAsDouble + 30.0;
		bool hasWaited = false;
		bool warned = false;
		while (this.IsPrePoolRunCurrent(generation))
		{
			if (hasWaited && (catalogOwner == null || ChunkManager.Instance != catalogOwner))
			{
				throw new InvalidOperationException("Map catalog owner changed while NPC preparation was waiting.");
			}
			bool flag;
			if (catalogOwner == null)
			{
				flag = true;
			}
			else
			{
				if (catalogOwner.IsTearingDown || (hasWaited && !catalogOwner.isActiveAndEnabled))
				{
					throw new InvalidOperationException("Map catalog owner stopped while NPC preparation was waiting.");
				}
				if (!catalogOwner.isActiveAndEnabled)
				{
					flag = true;
				}
				else
				{
					if (catalogOwner.CatalogLoadFailed)
					{
						throw new InvalidOperationException("Map catalog failed; NPC preparation cannot begin.");
					}
					if (catalogOwner.IsAwaitingCatalog)
					{
						if (!warned && Time.realtimeSinceStartupAsDouble >= warningAt)
						{
							Debug.LogWarning("[NPCRuntimeSpawner] Map catalog still loading after " + string.Format("{0:0}s; NPC pre-pool remains waiting.", 30f));
							warned = true;
						}
						hasWaited = true;
						await Task.Yield();
						continue;
					}
					flag = true;
				}
			}
			return flag;
		}
		return false;
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x000374E8 File Offset: 0x000356E8
	private void RemoveUIComponents(GameObject npc)
	{
		if (npc == null)
		{
			return;
		}
		CharacterStateManager componentInChildren = npc.GetComponentInChildren<CharacterStateManager>(true);
		if (componentInChildren != null)
		{
			Object.DestroyImmediate(componentInChildren);
		}
		StyleManager componentInChildren2 = npc.GetComponentInChildren<StyleManager>(true);
		if (componentInChildren2 != null)
		{
			Object.DestroyImmediate(componentInChildren2);
		}
		CharacterBlendShapeController componentInChildren3 = npc.GetComponentInChildren<CharacterBlendShapeController>(true);
		if (componentInChildren3 != null)
		{
			Object.DestroyImmediate(componentInChildren3);
		}
		BoZo_Links componentInChildren4 = npc.GetComponentInChildren<BoZo_Links>(true);
		if (componentInChildren4 != null)
		{
			Object.DestroyImmediate(componentInChildren4);
		}
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x0003755C File Offset: 0x0003575C
	private void PreConfigureNPCComponents(GameObject npc, CharacterObject characterData)
	{
		if (npc == null)
		{
			return;
		}
		NPCSpawnFade component = npc.GetComponent<NPCSpawnFade>();
		if (component != null)
		{
			component.RefreshRenderers();
		}
		npc.tag = "NPC";
		NPCRagdollRecovery npcragdollRecovery = npc.GetComponent<NPCRagdollRecovery>();
		NPCStumbleHandler component2 = npc.GetComponent<NPCStumbleHandler>();
		if (npcragdollRecovery == null && component2 != null && component2.enabled)
		{
			npcragdollRecovery = npc.AddComponent<NPCRagdollRecovery>();
		}
		if (npcragdollRecovery != null)
		{
			npcragdollRecovery.ReinitializeAfterRigChange();
		}
		this.InitializeNPCDialogue(npc, characterData);
		FollowerEntity component3 = npc.GetComponent<FollowerEntity>();
		if (component3 != null)
		{
			float num = 0.15f + (float)(NPCRuntimeSpawner._npcPriorityCounter % 11) * 0.01f;
			RVOAgent rvoSettings = component3.rvoSettings;
			rvoSettings.priority = num;
			component3.rvoSettings = rvoSettings;
			NPCRuntimeSpawner._npcPriorityCounter++;
		}
		NPCSenses component4 = npc.GetComponent<NPCSenses>();
		if (component4 != null && component4.EyeTransform == null)
		{
			component4.EyeTransform = npc.transform;
		}
		NPCEquipmentManager component5 = npc.GetComponent<NPCEquipmentManager>();
		if (component5 != null)
		{
			component5.ReInitializeAfterMerge();
		}
		else
		{
			Debug.LogWarning("[NPCRuntimeSpawner] " + npc.name + ": NPCEquipmentManager component NOT FOUND!");
		}
		NPCSimpleLookAt component6 = npc.GetComponent<NPCSimpleLookAt>();
		if (component6 != null)
		{
			component6.ReInitializeAfterMerge();
		}
		else
		{
			Debug.LogWarning("[NPCRuntimeSpawner] " + npc.name + ": NPCSimpleLookAt component NOT FOUND!");
		}
		NPCPropHolder component7 = npc.GetComponent<NPCPropHolder>();
		if (component7 != null)
		{
			component7.ReInitializeAfterMerge();
		}
		this.RunPostSpawnConfigurators(npc, characterData);
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x000376D7 File Offset: 0x000358D7
	public void RegisterConfigurator(INpcPostSpawnConfigurator configurator)
	{
		if (configurator == null || this._postSpawnConfigurators.Contains(configurator))
		{
			return;
		}
		this._postSpawnConfigurators.Add(configurator);
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x000376F7 File Offset: 0x000358F7
	public void UnregisterConfigurator(INpcPostSpawnConfigurator configurator)
	{
		if (configurator == null)
		{
			return;
		}
		this._postSpawnConfigurators.Remove(configurator);
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x0003770C File Offset: 0x0003590C
	private void RunPostSpawnConfigurators(GameObject npc, CharacterObject characterData)
	{
		for (int i = 0; i < this._postSpawnConfigurators.Count; i++)
		{
			try
			{
				INpcPostSpawnConfigurator npcPostSpawnConfigurator = this._postSpawnConfigurators[i];
				if (npcPostSpawnConfigurator != null)
				{
					npcPostSpawnConfigurator.Configure(npc, characterData);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[NPCRuntimeSpawner] Post-spawn configurator threw for '" + ((npc != null) ? npc.name : null) + "': " + ex.Message);
			}
		}
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x00037784 File Offset: 0x00035984
	public void ApplyConfiguratorToExisting(INpcPostSpawnConfigurator configurator)
	{
		NPCRuntimeSpawner.<>c__DisplayClass86_0 CS$<>8__locals1;
		CS$<>8__locals1.configurator = configurator;
		if (CS$<>8__locals1.configurator == null)
		{
			return;
		}
		foreach (GameObject gameObject in this._activeNpcs)
		{
			NPCRuntimeSpawner.<ApplyConfiguratorToExisting>g__Apply|86_0(gameObject, ref CS$<>8__locals1);
		}
		foreach (GameObject gameObject2 in this._genericPool)
		{
			NPCRuntimeSpawner.<ApplyConfiguratorToExisting>g__Apply|86_0(gameObject2, ref CS$<>8__locals1);
		}
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x00037828 File Offset: 0x00035A28
	private void SetNPCLogicEnabled(GameObject npc, bool enabled)
	{
		if (npc == null)
		{
			return;
		}
		AStarNPCController component = npc.GetComponent<AStarNPCController>();
		NPCWanderController component2 = npc.GetComponent<NPCWanderController>();
		NPCSenses component3 = npc.GetComponent<NPCSenses>();
		FollowerEntity component4 = npc.GetComponent<FollowerEntity>();
		if (enabled)
		{
			if (component != null)
			{
				component.enabled = true;
			}
			if (component4 != null)
			{
				component4.enabled = true;
			}
			if (component3 != null)
			{
				component3.enabled = true;
			}
			if (component2 != null)
			{
				component2.enabled = true;
				return;
			}
		}
		else
		{
			if (component2 != null)
			{
				component2.enabled = false;
			}
			if (component3 != null)
			{
				component3.enabled = false;
			}
			if (component4 != null)
			{
				component4.enabled = false;
			}
			if (component != null)
			{
				component.enabled = false;
			}
		}
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x000378DF File Offset: 0x00035ADF
	public GameObject GetPooledNPC(Vector3 position, Quaternion rotation, Transform parent = null)
	{
		return this.TakePooledNPC(null, position, rotation, parent);
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x000378EC File Offset: 0x00035AEC
	private GameObject TakePooledNPC(CharacterObject requested, Vector3 position, Quaternion rotation, Transform parent)
	{
		if (this.IsTearingDown)
		{
			return null;
		}
		int count = this._genericPool.Count;
		while (count-- > 0)
		{
			GameObject gameObject = this._genericPool.Dequeue();
			if (gameObject != null && gameObject)
			{
				CharacterObject npcCharacter = this.GetNpcCharacter(gameObject);
				if (this._roleNpcs.Contains(gameObject) || (npcCharacter != null && !this.IsAllowedCharacter(npcCharacter, false)))
				{
					this.DestroyTrackedNPC(gameObject);
				}
				else
				{
					if (!(requested != null) || !(this.GetCharacterIdentity(npcCharacter) != this.GetCharacterIdentity(requested)))
					{
						gameObject.transform.SetParent(parent);
						gameObject.transform.position = position;
						gameObject.transform.rotation = rotation;
						this.SetNPCLogicEnabled(gameObject, true);
						gameObject.SetActive(true);
						NPCSenses component = gameObject.GetComponent<NPCSenses>();
						if (component != null)
						{
							component.IsPassiveReacting = false;
							component.CurrentAttention = 0f;
							component.IsDetected = false;
						}
						NPCStateMachine component2 = gameObject.GetComponent<NPCStateMachine>();
						if (component2 != null)
						{
							component2.ForceState(NPCStateType.Idle, "Spawner");
						}
						this._activeNpcs.Add(gameObject);
						return gameObject;
					}
					this._genericPool.Enqueue(gameObject);
				}
			}
		}
		return null;
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00037A33 File Offset: 0x00035C33
	public bool HasPooledNPCs()
	{
		return this._genericPool.Count > 0;
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00037A43 File Offset: 0x00035C43
	public List<CharacterObject> GetAvailableCharacterData()
	{
		return new List<CharacterObject>(this._npcDataList);
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00037A50 File Offset: 0x00035C50
	public void SetNPCPrefixOverride(string newPrefix)
	{
		string text = (string.IsNullOrEmpty(newPrefix) ? "NPC_" : newPrefix);
		bool flag = text != "NPC_" || this._rosterSnapshot == NPCRosterSession.GetCurrentSnapshot();
		if (text == this.npcPrefix && flag)
		{
			return;
		}
		object obj = this._prePoolComplete || this._isPrePooling || this._prePoolFailure != null || this._activeNpcs.Count > 0 || this._genericPool.Count > 0 || this._residentNpcs.Count > 0 || this._prePoolInFlightNpcs.Count > 0 || this._fadingNpcs.Count > 0;
		this.npcPrefix = text;
		object obj2 = obj;
		if (obj2 != null)
		{
			this.TeardownAllNPCs();
		}
		this.BuildNPCDataList();
		if (obj2 != null)
		{
			if (this.enablePrePooling)
			{
				this.StartPrePooling();
				return;
			}
			this._prePoolComplete = true;
		}
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00037B2C File Offset: 0x00035D2C
	private void TeardownAllNPCs()
	{
		this._prePoolGeneration++;
		this.ClearUnavailablePrePoolIdentities();
		NPCZoneManager npczoneManager = Object.FindObjectOfType<NPCZoneManager>();
		if (npczoneManager != null)
		{
			npczoneManager.DespawnAllNPCs();
		}
		foreach (GameObject gameObject in new List<GameObject>(this._activeNpcs))
		{
			if (gameObject != null)
			{
				Object.Destroy(gameObject);
			}
		}
		this._activeNpcs.Clear();
		while (this._genericPool.Count > 0)
		{
			GameObject gameObject2 = this._genericPool.Dequeue();
			if (gameObject2 != null)
			{
				Object.Destroy(gameObject2);
			}
		}
		foreach (GameObject gameObject3 in new List<GameObject>(this._prePoolInFlightNpcs))
		{
			if (gameObject3 != null)
			{
				Object.Destroy(gameObject3);
			}
		}
		this._prePoolInFlightNpcs.Clear();
		foreach (GameObject gameObject4 in new List<GameObject>(this._residentNpcs))
		{
			this.DestroyTrackedNPC(gameObject4);
		}
		this._fadingNpcs.Clear();
		this.CleanupCustomCharacterObjects();
		this._isPrePooling = false;
		this._prePoolComplete = false;
		this._prePoolFailure = null;
		this._prePooledCount = 0;
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x060007E2 RID: 2018 RVA: 0x00037CC0 File Offset: 0x00035EC0
	// (set) Token: 0x060007E3 RID: 2019 RVA: 0x00037CC8 File Offset: 0x00035EC8
	public bool IsTearingDown { get; private set; }

	// Token: 0x060007E4 RID: 2020 RVA: 0x00037CD4 File Offset: 0x00035ED4
	public void BeginFastTeardown(List<GameObject> rootsOut)
	{
		if (this.IsTearingDown)
		{
			return;
		}
		this.IsTearingDown = true;
		this._prePoolGeneration++;
		this.ClearUnavailablePrePoolIdentities();
		base.StopAllCoroutines();
		this._isPrePooling = false;
		this._prePoolComplete = false;
		this._prePoolFailure = null;
		base.enabled = false;
		if (rootsOut != null)
		{
			if (this.poolContainer != null)
			{
				rootsOut.Add(this.poolContainer.gameObject);
			}
			foreach (GameObject gameObject in this._activeNpcs)
			{
				if (gameObject != null)
				{
					rootsOut.Add(gameObject);
				}
			}
			foreach (GameObject gameObject2 in this._residentNpcs)
			{
				if (gameObject2 != null)
				{
					rootsOut.Add(gameObject2);
				}
			}
			using (HashSet<GameObject>.Enumerator enumerator = this._prePoolInFlightNpcs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject3 = enumerator.Current;
					if (gameObject3 != null)
					{
						rootsOut.Add(gameObject3);
					}
				}
				goto IL_0169;
			}
		}
		foreach (GameObject gameObject4 in this._prePoolInFlightNpcs)
		{
			if (gameObject4 != null)
			{
				Object.Destroy(gameObject4);
			}
		}
		IL_0169:
		if (rootsOut == null)
		{
			foreach (GameObject gameObject5 in new List<GameObject>(this._residentNpcs))
			{
				this.DestroyTrackedNPC(gameObject5);
			}
		}
		this.CleanupCustomCharacterObjects();
		this._residentNpcs.Clear();
		this._characterByInstance.Clear();
		this._fadingNpcs.Clear();
		this._roleNpcs.Clear();
		this.poolContainer = null;
		this._genericPool.Clear();
		this._activeNpcs.Clear();
		this._prePoolInFlightNpcs.Clear();
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00037F28 File Offset: 0x00036128
	private void OnDestroy()
	{
		this._prePoolGeneration++;
		this.ClearUnavailablePrePoolIdentities();
		this._isPrePooling = false;
		foreach (GameObject gameObject in new List<GameObject>(this._residentNpcs))
		{
			if (gameObject != null)
			{
				Object.Destroy(gameObject);
			}
		}
		this._residentNpcs.Clear();
		this._characterByInstance.Clear();
		this._prePoolInFlightNpcs.Clear();
		this._fadingNpcs.Clear();
		this._roleNpcs.Clear();
		this.CleanupCustomCharacterObjects();
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x00037FE0 File Offset: 0x000361E0
	public async Task<GameObject> SpawnRandomNPC(Vector3 position, Quaternion rotation, Transform parent = null)
	{
		GameObject gameObject;
		if (this._npcDataList.Count == 0)
		{
			Debug.LogError("[NPCRuntimeSpawner] Nessun CharacterObject disponibile!");
			gameObject = null;
		}
		else
		{
			if (this.usePooling && this.HasPooledNPCs())
			{
				GameObject pooledNPC = this.GetPooledNPC(position, rotation, parent);
				if (pooledNPC != null)
				{
					return pooledNPC;
				}
			}
			CharacterObject randomCharacterData = this.GetRandomCharacterData();
			GameObject gameObject2;
			if (randomCharacterData == null)
			{
				gameObject2 = null;
			}
			else
			{
				gameObject2 = await this.SpawnNPC(randomCharacterData, position, rotation, parent, null, false);
			}
			gameObject = gameObject2;
		}
		return gameObject;
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x0003803C File Offset: 0x0003623C
	public async Task<GameObject> SpawnNPC(CharacterObject characterData, Vector3 position, Quaternion rotation, Transform parent = null, OutfitSystem basePrefabOverride = null, bool bypassRoster = false)
	{
		bool flag = bypassRoster || basePrefabOverride != null;
		GameObject gameObject;
		if (!this.IsAllowedCharacter(characterData, flag))
		{
			gameObject = null;
		}
		else
		{
			if (!flag && this.usePooling)
			{
				GameObject gameObject2 = this.TakePooledNPC(characterData, position, rotation, parent);
				if (gameObject2 != null)
				{
					return gameObject2;
				}
			}
			gameObject = await this.CreateNPCInstanceAsync(characterData, position, rotation, parent, basePrefabOverride, false, this._prePoolGeneration, flag, false);
		}
		return gameObject;
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x000380B4 File Offset: 0x000362B4
	public void DespawnNPC(GameObject npc, bool forceDestroy = false)
	{
		if (npc == null)
		{
			return;
		}
		if (forceDestroy || this.IsTearingDown || this._roleNpcs.Contains(npc))
		{
			this.DestroyTrackedNPC(npc);
			return;
		}
		if (this._genericPool.Contains(npc) || this._fadingNpcs.Contains(npc))
		{
			return;
		}
		this._activeNpcs.Remove(npc);
		if (!this.usePooling || !(this.poolContainer != null) || this._genericPool.Count + this._fadingNpcs.Count >= this.maxPoolSize)
		{
			this.DestroyTrackedNPC(npc);
			return;
		}
		int generation = this._prePoolGeneration;
		NPCSpawnFade component = npc.GetComponent<NPCSpawnFade>();
		if (component != null)
		{
			npc.transform.SetParent(this.poolContainer, true);
			this._fadingNpcs.Add(npc);
			component.StartFadeOut(delegate
			{
				this._fadingNpcs.Remove(npc);
				if (npc == null)
				{
					return;
				}
				if (!this.IsSpawnRunCurrent(generation) || this.poolContainer == null || this._genericPool.Count >= this.maxPoolSize)
				{
					this.DestroyTrackedNPC(npc);
					return;
				}
				this.QueueReadyNPC(npc, this.GetNpcCharacter(npc));
			});
			return;
		}
		this.QueueReadyNPC(npc, this.GetNpcCharacter(npc));
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x00038208 File Offset: 0x00036408
	public void DespawnNPCImmediate(GameObject npc, bool forceDestroy = false)
	{
		if (npc == null)
		{
			return;
		}
		this._activeNpcs.Remove(npc);
		this._fadingNpcs.Remove(npc);
		NPCSpawnFade component = npc.GetComponent<NPCSpawnFade>();
		if (component != null)
		{
			component.CancelImmediate();
		}
		if (!forceDestroy && !this.IsTearingDown && !this._roleNpcs.Contains(npc) && this.usePooling && this.poolContainer != null)
		{
			if (this._genericPool.Contains(npc))
			{
				return;
			}
			if (this._genericPool.Count < this.maxPoolSize)
			{
				this.QueueReadyNPC(npc, this.GetNpcCharacter(npc));
				return;
			}
		}
		this.DestroyTrackedNPC(npc);
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x000382B4 File Offset: 0x000364B4
	public CharacterObject GetRandomCharacterData()
	{
		if (this._npcDataList.Count == 0 || this.IsTearingDown)
		{
			return null;
		}
		List<CharacterObject> list = new List<CharacterObject>();
		foreach (GameObject gameObject in this._genericPool)
		{
			CharacterObject characterObject = ((gameObject != null) ? this.GetNpcCharacter(gameObject) : null);
			if (characterObject != null && this.IsAllowedCharacter(characterObject, false) && !list.Contains(characterObject))
			{
				list.Add(characterObject);
			}
		}
		List<CharacterObject> list2 = list.FindAll(new Predicate<CharacterObject>(this.IsCustomCharacter));
		if (list2.Count > 0)
		{
			return list2[Random.Range(0, list2.Count)];
		}
		if (list.Count > 0)
		{
			return list[Random.Range(0, list.Count)];
		}
		foreach (CharacterObject characterObject2 in this._npcDataList)
		{
			if (this.IsAllowedCharacter(characterObject2, false) && !this.IsUnavailablePrePoolIdentity(characterObject2) && (!this.IsCivilianCharacter(characterObject2) || this._residentBudget.CanReserve(this.GetCharacterIdentity(characterObject2), this.IsCustomCharacter(characterObject2))))
			{
				list.Add(characterObject2);
			}
		}
		if (list.Count != 0)
		{
			return list[Random.Range(0, list.Count)];
		}
		return null;
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x060007EB RID: 2027 RVA: 0x00038444 File Offset: 0x00036644
	public int ActiveNPCCount
	{
		get
		{
			return this._activeNpcs.Count;
		}
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x060007EC RID: 2028 RVA: 0x00036749 File Offset: 0x00034949
	public int PooledNPCCount
	{
		get
		{
			return this._genericPool.Count;
		}
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x00038454 File Offset: 0x00036654
	public GameObject SpawnNPCSync(CharacterObject characterData, Vector3 position, Quaternion rotation, Transform parent = null, bool bypassRoster = false)
	{
		if (!this.IsAllowedCharacter(characterData, bypassRoster))
		{
			return null;
		}
		if (!bypassRoster && this.usePooling)
		{
			GameObject gameObject = this.TakePooledNPC(characterData, position, rotation, parent);
			if (gameObject != null)
			{
				return gameObject;
			}
		}
		if (this.IsCustomCharacter(characterData))
		{
			this.CreateNPCInstanceAsync(characterData, new Vector3(0f, -1000f, 0f), Quaternion.identity, this.poolContainer, null, true, this._prePoolGeneration, bypassRoster, false);
			return null;
		}
		NPCResidentBudget.Reservation reservation;
		if (!this.TryReserveCharacter(characterData, bypassRoster, out reservation))
		{
			return null;
		}
		GameObject gameObject2 = null;
		GameObject gameObject4;
		try
		{
			GameObject gameObject3;
			if (this.TryGetBakedPrefab(characterData, out gameObject3))
			{
				gameObject2 = this.SpawnFromBaked(gameObject3, characterData, position, rotation, parent, reservation, bypassRoster);
				this._activeNpcs.Add(gameObject2);
				gameObject4 = gameObject2;
			}
			else if (this.characterBasePrefab == null)
			{
				gameObject4 = null;
			}
			else
			{
				OutfitSystem outfitSystem = this.CreateRuntimeShell(characterData, position, rotation, parent, this.characterBasePrefab, reservation, bypassRoster);
				gameObject2 = outfitSystem.gameObject;
				this.PreConfigureNPCComponents(gameObject2, characterData);
				this._activeNpcs.Add(gameObject2);
				this.LoadCharacterAndOptimizeAsync(outfitSystem, characterData, this._prePoolGeneration);
				gameObject4 = gameObject2;
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning("[NPCRuntimeSpawner] Failed to spawn '" + this.GetCharacterIdentity(characterData) + "': " + ex.Message);
			this.DestroyTrackedNPC(gameObject2);
			gameObject4 = null;
		}
		finally
		{
			this._residentBudget.CancelUnattached(reservation);
		}
		return gameObject4;
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x000385C8 File Offset: 0x000367C8
	private async void LoadCharacterAndOptimizeAsync(OutfitSystem outfit, CharacterObject character, int generation)
	{
		GameObject npc = ((outfit != null) ? outfit.gameObject : null);
		try
		{
			if (!(outfit == null))
			{
				await BMAC_SaveSystem.LoadCharacter(outfit, character.data, true, true);
				if (!this.IsSpawnRunCurrent(generation) || npc == null)
				{
					this.DestroyTrackedNPC(npc);
				}
				else
				{
					if (this.enableMeshOptimization && outfit.mergeMaterial != null)
					{
						await outfit.MergeCharacterAsync();
					}
					if (!this.IsSpawnRunCurrent(generation) || npc == null)
					{
						this.DestroyTrackedNPC(npc);
					}
					else
					{
						this.PreConfigureNPCComponents(npc, character);
						this._prePoolInFlightNpcs.Remove(npc);
						if (this._activeNpcs.Contains(npc))
						{
							this.SetNPCLogicEnabled(npc, true);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning("[NPCRuntimeSpawner] Failed to load '" + this.GetCharacterIdentity(character) + "': " + ex.Message);
			this.DestroyTrackedNPC(npc);
		}
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x00038618 File Offset: 0x00036818
	private void InitializeNPCDialogue(GameObject npcObject, CharacterObject characterData)
	{
		if (!(npcObject == null))
		{
			bool flag;
			if (characterData == null)
			{
				flag = null != null;
			}
			else
			{
				CharacterData data = characterData.data;
				flag = ((data != null) ? data.generalInfo : null) != null;
			}
			if (flag)
			{
				string characterIdentity = this.GetCharacterIdentity(characterData);
				CharacterGeneralInfo characterGeneralInfo = characterData.data.generalInfo;
				CharacterData characterData2 = (this.IsCustomCharacter(characterData) ? null : NpcLookStore.LoadLook(characterIdentity));
				if (characterData2 != null && characterData2.generalInfo != null)
				{
					characterGeneralInfo = characterData2.generalInfo;
				}
				NPCDialogueHolder component = npcObject.GetComponent<NPCDialogueHolder>();
				if (component != null)
				{
					component.Initialize(characterGeneralInfo);
					component.SetNpcIdentity(characterIdentity);
				}
				return;
			}
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x060007F0 RID: 2032 RVA: 0x000386A0 File Offset: 0x000368A0
	public int ResidentCustomNPCCount
	{
		get
		{
			return NPCResidentBudget.CustomCount;
		}
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x060007F1 RID: 2033 RVA: 0x000386A7 File Offset: 0x000368A7
	private bool UsesCivilianRoster
	{
		get
		{
			return this.npcPrefix == "NPC_";
		}
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x000386B9 File Offset: 0x000368B9
	private bool IsCustomCharacter(CharacterObject character)
	{
		return character != null && this._customPresetIds.ContainsKey(character);
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x000386D4 File Offset: 0x000368D4
	private string GetCharacterIdentity(CharacterObject character)
	{
		if (character == null)
		{
			return null;
		}
		string text;
		if (!this._customPresetIds.TryGetValue(character, out text))
		{
			return "sys_" + character.name;
		}
		return text;
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x00038710 File Offset: 0x00036910
	private CharacterObject GetNpcCharacter(GameObject npc)
	{
		if (npc == null)
		{
			return null;
		}
		CharacterObject characterObject;
		if (this._characterByInstance.TryGetValue(npc, out characterObject))
		{
			return characterObject;
		}
		OutfitSystem component = npc.GetComponent<OutfitSystem>();
		if (component == null)
		{
			return null;
		}
		return component.characterData;
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x0003874B File Offset: 0x0003694B
	private bool IsCivilianCharacter(CharacterObject character)
	{
		return this.UsesCivilianRoster && character != null && (this.IsCustomCharacter(character) || character.name.StartsWith("NPC_", StringComparison.Ordinal));
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x0003877C File Offset: 0x0003697C
	private bool IsAllowedCharacter(CharacterObject character, bool bypassRoster = false)
	{
		return !(character == null) && character.data != null && !this.IsTearingDown && (bypassRoster || !this.IsCivilianCharacter(character) || this.IsCustomCharacter(character) || this._eligibleSystemNames.Contains(character.name));
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x000387D0 File Offset: 0x000369D0
	private void BuildCivilianRoster()
	{
		this._rosterSnapshot = NPCRosterSession.GetCurrentSnapshot();
		this._eligibleSystemNames.Clear();
		NPCRosterSnapshot rosterSnapshot = this._rosterSnapshot;
		if (((rosterSnapshot != null) ? rosterSnapshot.entries : null) == null)
		{
			return;
		}
		List<NPCRosterEntry> list = new List<NPCRosterEntry>();
		foreach (NPCRosterEntry npcrosterEntry in this._rosterSnapshot.entries)
		{
			if (npcrosterEntry.isCustom)
			{
				if (this._customPresetIds.Count < 4 && this._npcDataList.Count < this.maxPoolSize)
				{
					CharacterObject characterObject = npcrosterEntry.CreateCustomCharacter();
					if (!(characterObject == null) && characterObject.data != null)
					{
						this._customPresetIds.Add(characterObject, npcrosterEntry.presetId);
						this._npcDataList.Add(characterObject);
					}
				}
			}
			else if (!string.IsNullOrEmpty(npcrosterEntry.resourceName) && this._eligibleSystemNames.Add(npcrosterEntry.resourceName))
			{
				list.Add(npcrosterEntry);
			}
		}
		if (this.npcCharacterData != null)
		{
			foreach (CharacterObject characterObject2 in this.npcCharacterData)
			{
				if (characterObject2 != null && this._npcDataList.Count < this.maxPoolSize && this._eligibleSystemNames.Contains(characterObject2.name) && !this._npcDataList.Contains(characterObject2))
				{
					this._npcDataList.Add(characterObject2);
				}
			}
		}
		this.ShuffleList<NPCRosterEntry>(list);
		foreach (NPCRosterEntry npcrosterEntry2 in list)
		{
			if (this._npcDataList.Count >= this.maxPoolSize)
			{
				break;
			}
			CharacterObject characterObject3 = NPCBakedResourceIndex.LoadCharacter(npcrosterEntry2.resourceName);
			if (characterObject3 != null && !this._npcDataList.Contains(characterObject3))
			{
				this._npcDataList.Add(characterObject3);
			}
		}
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x000389E0 File Offset: 0x00036BE0
	private void CleanupCustomCharacterObjects()
	{
		foreach (CharacterObject characterObject in this._customPresetIds.Keys)
		{
			if (characterObject != null)
			{
				Object.Destroy(characterObject);
			}
		}
		this._customPresetIds.Clear();
		this._eligibleSystemNames.Clear();
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x00038A58 File Offset: 0x00036C58
	private bool TryReserveCharacter(CharacterObject character, bool bypassRoster, out NPCResidentBudget.Reservation reservation)
	{
		reservation = null;
		return this.IsAllowedCharacter(character, bypassRoster) && (bypassRoster || !this.IsUnavailablePrePoolIdentity(character)) && ((!this.IsCustomCharacter(character) && (bypassRoster || !this.IsCivilianCharacter(character))) || this._residentBudget.TryReserve(this.GetCharacterIdentity(character), this.IsCustomCharacter(character), out reservation));
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x00038AB3 File Offset: 0x00036CB3
	private bool IsSpawnRunCurrent(int generation)
	{
		return this != null && !this.IsTearingDown && generation == this._prePoolGeneration;
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00038AD1 File Offset: 0x00036CD1
	private void TrackResident(GameObject npc, CharacterObject character, NPCResidentBudget.Reservation reservation, bool isRole)
	{
		this._residentNpcs.Add(npc);
		this._characterByInstance[npc] = character;
		if (isRole)
		{
			this._roleNpcs.Add(npc);
		}
		this._residentBudget.Attach(reservation, npc);
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x00038B0C File Offset: 0x00036D0C
	private void DestroyTrackedNPC(GameObject npc)
	{
		if (npc == null)
		{
			return;
		}
		this._activeNpcs.Remove(npc);
		this._fadingNpcs.Remove(npc);
		this._prePoolInFlightNpcs.Remove(npc);
		this._residentNpcs.Remove(npc);
		this._characterByInstance.Remove(npc);
		this._roleNpcs.Remove(npc);
		NPCSpawnFade component = npc.GetComponent<NPCSpawnFade>();
		if (component != null)
		{
			component.CancelImmediate();
		}
		npc.SetActive(false);
		Object.Destroy(npc);
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x00038B90 File Offset: 0x00036D90
	private static string GetCustomHierarchyName(CharacterObject character)
	{
		CharacterData data = character.data;
		CharacterGeneralInfo characterGeneralInfo = ((data != null) ? data.generalInfo : null);
		string text = (((characterGeneralInfo != null) ? characterGeneralInfo.characterName : null) + " " + ((characterGeneralInfo != null) ? characterGeneralInfo.surname : null)).Trim();
		if (string.IsNullOrWhiteSpace(text))
		{
			CharacterData data2 = character.data;
			text = ((data2 != null) ? data2.characterName : null);
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			text = character.name;
		}
		return "NPC_Custom_" + string.Join("_", text.Split(null, StringSplitOptions.RemoveEmptyEntries));
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x00038C20 File Offset: 0x00036E20
	private OutfitSystem CreateRuntimeShell(CharacterObject character, Vector3 position, Quaternion rotation, Transform parent, OutfitSystem prefab, NPCResidentBudget.Reservation reservation, bool isRole)
	{
		bool activeSelf = prefab.gameObject.activeSelf;
		OutfitSystem outfitSystem;
		try
		{
			prefab.gameObject.SetActive(false);
			outfitSystem = Object.Instantiate<OutfitSystem>(prefab, position, rotation, parent);
		}
		finally
		{
			prefab.gameObject.SetActive(activeSelf);
		}
		GameObject gameObject = outfitSystem.gameObject;
		this.TrackResident(gameObject, character, reservation, isRole);
		OutfitSystem outfitSystem2;
		try
		{
			this._prePoolInFlightNpcs.Add(gameObject);
			gameObject.name = (this.IsCustomCharacter(character) ? NPCRuntimeSpawner.GetCustomHierarchyName(character) : ("NPC_" + character.name));
			this.RemoveUIComponents(gameObject);
			NPCMeshOptimizer[] componentsInChildren = gameObject.GetComponentsInChildren<NPCMeshOptimizer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Object.DestroyImmediate(componentsInChildren[i]);
			}
			this.SetNPCLogicEnabled(gameObject, false);
			outfitSystem.MuteHeightChange(true);
			outfitSystem.mergedMode = false;
			outfitSystem.mergeOnAwake = false;
			outfitSystem.loadMode = OutfitSystem.LoadMode.Manual;
			outfitSystem.autoUpdate = false;
			outfitSystem.characterData = character;
			outfitSystem.SaveID = this.GetCharacterIdentity(character);
			outfitSystem.atlasSize = this.atlasSize;
			if (outfitSystem.mergeMaterial == null && this.mergeMaterial != null)
			{
				outfitSystem.mergeMaterial = this.mergeMaterial;
			}
			if (gameObject.GetComponent<NPCSpawnFade>() == null)
			{
				gameObject.AddComponent<NPCSpawnFade>();
			}
			if (this.IsCustomCharacter(character))
			{
				gameObject.AddComponent<NPCCustomRuntimeResources>();
			}
			gameObject.SetActive(true);
			NPCCustomRuntimeResources component = gameObject.GetComponent<NPCCustomRuntimeResources>();
			if (component != null)
			{
				component.Capture();
			}
			outfitSystem2 = outfitSystem;
		}
		catch
		{
			this.DestroyTrackedNPC(gameObject);
			throw;
		}
		return outfitSystem2;
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x00038DCC File Offset: 0x00036FCC
	private void QueueReadyNPC(GameObject npc, CharacterObject character)
	{
		this._prePoolInFlightNpcs.Remove(npc);
		this._activeNpcs.Remove(npc);
		this._fadingNpcs.Remove(npc);
		this.SetNPCLogicEnabled(npc, false);
		npc.SetActive(false);
		npc.transform.SetParent(this.poolContainer, false);
		npc.transform.localPosition = Vector3.zero;
		if (this._genericPool.Contains(npc))
		{
			return;
		}
		if (this.IsCustomCharacter(character))
		{
			Queue<GameObject> queue = new Queue<GameObject>();
			queue.Enqueue(npc);
			foreach (GameObject gameObject in this._genericPool)
			{
				queue.Enqueue(gameObject);
			}
			this._genericPool = queue;
			return;
		}
		this._genericPool.Enqueue(npc);
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x00038EB0 File Offset: 0x000370B0
	private async Task<GameObject> CreateNPCInstanceAsync(CharacterObject character, Vector3 position, Quaternion rotation, Transform parent, OutfitSystem prefabOverride, bool forPool, int generation, bool bypassRoster = false, bool readBakedPrefabAsync = false)
	{
		NPCResidentBudget.Reservation reservation;
		GameObject gameObject;
		if (!this.IsSpawnRunCurrent(generation) || !this.TryReserveCharacter(character, bypassRoster, out reservation))
		{
			gameObject = null;
		}
		else
		{
			GameObject npc = null;
			try
			{
				GameObject gameObject2 = null;
				bool flag = false;
				if (prefabOverride == null)
				{
					if (readBakedPrefabAsync)
					{
						gameObject2 = await this.LoadBakedPrefabAsync(character);
						if (!this.IsSpawnRunCurrent(generation))
						{
							return null;
						}
						flag = gameObject2 != null;
					}
					else
					{
						flag = this.TryGetBakedPrefab(character, out gameObject2);
					}
				}
				if (flag)
				{
					npc = this.SpawnFromBaked(gameObject2, character, position, rotation, parent, reservation, bypassRoster);
				}
				else
				{
					OutfitSystem outfitSystem = ((prefabOverride != null) ? prefabOverride : this.characterBasePrefab);
					if (outfitSystem == null)
					{
						if (forPool)
						{
							this.RecordUnavailablePrePoolIdentity(generation, character, "No baked or base prefab is available for '" + this.GetCharacterIdentity(character) + "'.");
						}
						return null;
					}
					OutfitSystem outfit = this.CreateRuntimeShell(character, position, rotation, parent, outfitSystem, reservation, bypassRoster);
					npc = outfit.gameObject;
					bool custom = this.IsCustomCharacter(character);
					await BMAC_SaveSystem.LoadCharacter(outfit, character.data, !custom, true);
					if (!this.IsSpawnRunCurrent(generation) || npc == null)
					{
						this.DestroyTrackedNPC(npc);
						return null;
					}
					NPCCustomRuntimeResources component = npc.GetComponent<NPCCustomRuntimeResources>();
					if (component != null)
					{
						component.Capture();
					}
					if (!custom && this.enableMeshOptimization && outfit.mergeMaterial != null)
					{
						await outfit.MergeCharacterAsync();
					}
					if (!this.IsSpawnRunCurrent(generation) || npc == null)
					{
						this.DestroyTrackedNPC(npc);
						return null;
					}
					this.PreConfigureNPCComponents(npc, character);
					outfit = null;
				}
				if (!this.IsSpawnRunCurrent(generation) || npc == null)
				{
					this.DestroyTrackedNPC(npc);
					gameObject = null;
				}
				else
				{
					this._prePoolInFlightNpcs.Remove(npc);
					if (forPool)
					{
						this.QueueReadyNPC(npc, character);
					}
					else
					{
						this.SetNPCLogicEnabled(npc, true);
						this._activeNpcs.Add(npc);
					}
					gameObject = npc;
				}
			}
			catch (Exception ex)
			{
				if (forPool)
				{
					this.RecordPrePoolFailure(generation, "Failed to load '" + this.GetCharacterIdentity(character) + "': " + ex.Message);
				}
				Debug.LogWarning("[NPCRuntimeSpawner] Failed to load '" + this.GetCharacterIdentity(character) + "': " + ex.Message);
				this.DestroyTrackedNPC(npc);
				gameObject = null;
			}
			finally
			{
				this._residentBudget.CancelUnattached(reservation);
			}
		}
		return gameObject;
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x00039074 File Offset: 0x00037274
	[CompilerGenerated]
	internal static void <ApplyConfiguratorToExisting>g__Apply|86_0(GameObject npc, ref NPCRuntimeSpawner.<>c__DisplayClass86_0 A_1)
	{
		if (npc == null)
		{
			return;
		}
		OutfitSystem component = npc.GetComponent<OutfitSystem>();
		CharacterObject characterObject = ((component != null) ? component.characterData : null);
		if (characterObject == null)
		{
			return;
		}
		try
		{
			A_1.configurator.Configure(npc, characterObject);
		}
		catch (Exception ex)
		{
			Debug.LogWarning("[NPCRuntimeSpawner] ApplyConfiguratorToExisting threw for '" + npc.name + "': " + ex.Message);
		}
	}

	// Token: 0x0400069C RID: 1692
	[Header("Base Character")]
	[Tooltip("Base character prefab con OutfitSystem (es. BMAC_MergedCharacterBase)")]
	[SerializeField]
	private OutfitSystem characterBasePrefab;

	// Token: 0x0400069D RID: 1693
	[Tooltip("Materiale da usare per il merge (es. BMAC_Toon.mat)")]
	[SerializeField]
	private Material mergeMaterial;

	// Token: 0x0400069E RID: 1694
	[Header("NPC Data")]
	[Tooltip("Lista di CharacterObject da usare per gli NPC")]
	[SerializeField]
	private CharacterObject[] npcCharacterData;

	// Token: 0x0400069F RID: 1695
	[Header("Auto-Load Settings")]
	private const string DefaultNPCPrefix = "NPC_";

	// Token: 0x040006A0 RID: 1696
	[Tooltip("Carica automaticamente tutti i CharacterObject con prefisso NPC_ dalla cartella Resources")]
	[SerializeField]
	private bool autoLoadNPCData = true;

	// Token: 0x040006A1 RID: 1697
	[Tooltip("Prefisso per filtrare i CharacterObject (default: NPC_)")]
	[SerializeField]
	private string npcPrefix = "NPC_";

	// Token: 0x040006A2 RID: 1698
	[Header("Performance")]
	[Tooltip("Usa pooling per riutilizzare gli NPC invece di distruggerli")]
	[SerializeField]
	private bool usePooling = true;

	// Token: 0x040006A3 RID: 1699
	[Tooltip("Numero massimo di NPC nel pool. Sovrascritto da NPCSpawnConfig.maxTotalNpc se assegnato.")]
	[SerializeField]
	private int maxPoolSize = 20;

	// Token: 0x040006A4 RID: 1700
	[Header("Spawn Config (centralized)")]
	[Tooltip("Configurazione centralizzata di population, raggi, rate limit. Se assegnato sovrascrive i campi locali. Auto-caricato da Resources/NPCSpawnConfig.asset se null.")]
	[SerializeField]
	private NPCSpawnConfig spawnConfig;

	// Token: 0x040006A5 RID: 1701
	[Header("Mesh Optimization")]
	[Tooltip("Combina le mesh degli NPC per ridurre i draw calls")]
	[SerializeField]
	private bool enableMeshOptimization = true;

	// Token: 0x040006A6 RID: 1702
	[Tooltip("Delay prima dell'ottimizzazione mesh (per permettere il caricamento outfit)")]
	[SerializeField]
	private float meshOptimizationDelay = 0.5f;

	// Token: 0x040006A7 RID: 1703
	[Tooltip("Risoluzione dell'atlas texture generato dal merge. Più basso = merge più veloce + meno RAM, qualità visiva inferiore. 1024 è il default raccomandato.")]
	[SerializeField]
	private NPCRuntimeSpawner.AtlasResolution atlasResolution = NPCRuntimeSpawner.AtlasResolution._1024;

	// Token: 0x040006A8 RID: 1704
	[Header("Pre-Pool Settings")]
	[Tooltip("Abilita pre-caricamento NPC ottimizzati all'avvio")]
	[SerializeField]
	private bool enablePrePooling = true;

	// Token: 0x040006A9 RID: 1705
	[Tooltip("Numero di NPC da pre-caricare e ottimizzare all'avvio")]
	[SerializeField]
	private int prePoolCount = 10;

	// Token: 0x040006AA RID: 1706
	[Tooltip("NPC da pre-caricare per frame durante l'inizializzazione")]
	[SerializeField]
	private int prePoolPerFrame = 1;

	// Token: 0x040006AB RID: 1707
	[Tooltip("Budget CPU massimo indicativo per il batch baked di un singolo frame. Almeno un NPC viene processato per frame, anche se da solo supera il budget.")]
	[SerializeField]
	[Min(0.1f)]
	private float prePoolFrameBudgetMs = 4f;

	// Token: 0x040006AC RID: 1708
	[Tooltip("Container nascosto per gli NPC pre-pooled")]
	[SerializeField]
	private Transform poolContainer;

	// Token: 0x040006AD RID: 1709
	[Header("NPC Component Configuration")]
	[Tooltip("Configurazione centralizzata dei componenti NPC")]
	[SerializeField]
	private NPCComponentConfig npcComponentConfig;

	// Token: 0x040006AE RID: 1710
	[Header("Baked Prefabs (Fast Path)")]
	[Tooltip("Registry che mappa CharacterObject -> prefab baked enriched. Se l'entry esiste, lo spawn salta merge+load runtime (10-100x più veloce).")]
	[SerializeField]
	private NPCBakedRegistry bakedRegistry;

	// Token: 0x040006AF RID: 1711
	[Tooltip("Se attivo, usa il prefab baked dal registry quando disponibile. Disattiva per forzare il merge runtime (debug).")]
	[SerializeField]
	private bool preferBakedPrefabs = true;

	// Token: 0x040006B0 RID: 1712
	[Tooltip("Se attivo e bakedRegistry è null, carica automaticamente Resources/NPCBakedRegistry.asset in Awake.")]
	[SerializeField]
	private bool autoLoadRegistry = true;

	// Token: 0x040006B1 RID: 1713
	private NPCBakedRegistry _populationBakedRegistry;

	// Token: 0x040006B2 RID: 1714
	private string _populationRegistryPrefix;

	// Token: 0x040006B3 RID: 1715
	private List<CharacterObject> _npcDataList = new List<CharacterObject>();

	// Token: 0x040006B4 RID: 1716
	private Queue<GameObject> _genericPool = new Queue<GameObject>();

	// Token: 0x040006B5 RID: 1717
	private HashSet<GameObject> _activeNpcs = new HashSet<GameObject>();

	// Token: 0x040006B6 RID: 1718
	private readonly HashSet<GameObject> _prePoolInFlightNpcs = new HashSet<GameObject>();

	// Token: 0x040006B7 RID: 1719
	private bool _isPrePooling;

	// Token: 0x040006B8 RID: 1720
	private bool _prePoolComplete;

	// Token: 0x040006B9 RID: 1721
	private string _prePoolFailure;

	// Token: 0x040006BA RID: 1722
	private readonly HashSet<string> _unavailablePrePoolIdentities = new HashSet<string>(StringComparer.Ordinal);

	// Token: 0x040006BB RID: 1723
	private string _firstUnavailablePrePoolReason;

	// Token: 0x040006BC RID: 1724
	private int _prePooledCount;

	// Token: 0x040006BD RID: 1725
	private int _prePoolGeneration;

	// Token: 0x040006BE RID: 1726
	private static readonly ProfilerMarker PrePoolBakedBatchMarker = new ProfilerMarker("NPCRuntimeSpawner.PrePoolBakedBatch");

	// Token: 0x040006C0 RID: 1728
	private const float MapCatalogSlowWarningSeconds = 30f;

	// Token: 0x040006C1 RID: 1729
	private static int _npcPriorityCounter = 0;

	// Token: 0x040006C2 RID: 1730
	private readonly List<INpcPostSpawnConfigurator> _postSpawnConfigurators = new List<INpcPostSpawnConfigurator>();

	// Token: 0x040006C4 RID: 1732
	private NPCRosterSnapshot _rosterSnapshot;

	// Token: 0x040006C5 RID: 1733
	private readonly Dictionary<CharacterObject, string> _customPresetIds = new Dictionary<CharacterObject, string>();

	// Token: 0x040006C6 RID: 1734
	private readonly HashSet<string> _eligibleSystemNames = new HashSet<string>(StringComparer.Ordinal);

	// Token: 0x040006C7 RID: 1735
	private readonly HashSet<GameObject> _residentNpcs = new HashSet<GameObject>();

	// Token: 0x040006C8 RID: 1736
	private readonly Dictionary<GameObject, CharacterObject> _characterByInstance = new Dictionary<GameObject, CharacterObject>();

	// Token: 0x040006C9 RID: 1737
	private readonly HashSet<GameObject> _fadingNpcs = new HashSet<GameObject>();

	// Token: 0x040006CA RID: 1738
	private readonly HashSet<GameObject> _roleNpcs = new HashSet<GameObject>();

	// Token: 0x040006CB RID: 1739
	private readonly NPCResidentBudget _residentBudget = new NPCResidentBudget();

	// Token: 0x020000C4 RID: 196
	public enum AtlasResolution
	{
		// Token: 0x040006CD RID: 1741
		_512 = 512,
		// Token: 0x040006CE RID: 1742
		_1024 = 1024,
		// Token: 0x040006CF RID: 1743
		_2048 = 2048,
		// Token: 0x040006D0 RID: 1744
		_4096 = 4096
	}
}
