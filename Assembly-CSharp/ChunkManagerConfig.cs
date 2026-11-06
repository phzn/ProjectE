using System;
using UnityEngine;

// Token: 0x02000055 RID: 85
[CreateAssetMenu(fileName = "ChunkManagerConfig", menuName = "MapGeneration/ChunkManagerConfig", order = 1)]
public class ChunkManagerConfig : ScriptableObject
{
	// Token: 0x06000460 RID: 1120 RVA: 0x00022356 File Offset: 0x00020556
	[ContextMenu("Apply Preset")]
	public void ApplyPreset()
	{
		this.ApplyPreset(this.activePreset);
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x00022364 File Offset: 0x00020564
	public void ApplyPreset(ChunkManagerConfig.QualityPreset preset)
	{
		this.activePreset = preset;
		switch (preset)
		{
		case ChunkManagerConfig.QualityPreset.Low:
			this.ApplyLow();
			break;
		case ChunkManagerConfig.QualityPreset.Medium:
			this.ApplyMedium();
			break;
		case ChunkManagerConfig.QualityPreset.High:
			this.ApplyHigh();
			break;
		case ChunkManagerConfig.QualityPreset.Ultra:
			this.ApplyUltra();
			break;
		}
		Debug.Log(string.Format("[ChunkManagerConfig] Preset '{0}' applicato.", preset));
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x000223C8 File Offset: 0x000205C8
	private void ApplyLow()
	{
		this.maxVisibleRadius = 1;
		this.minVisibleRadius = 1;
		this.spawnRadius = 1;
		this.viewAngle = 100f;
		this.viewAngleBuffer = 25f;
		this.deactivationDelay = 4;
		this.visibilityCheckInterval = 0.25f;
		this.maxVisibilityChangesPerFrame = 2;
		this.maxPooledChunksPerPrefab = 3;
		this.maxAutoChunkPrewarmTarget = 12;
		this.prewarmInstancesPerFrame = 1;
		this.prewarmFrameBudgetMs = 0.75f;
		this.maxPooledZonesPerPrefab = 5;
		this.maxAutoZonePrewarmTarget = 64;
		this.enableIncrementalNormalZoneActivation = true;
		this.normalZoneStagingTransformThreshold = 128;
		this.normalZoneStagedMaxOperationsPerFrame = 256;
		this.normalZoneStagedFrameBudgetMs = 0.5f;
		this.runtimeDespawnRadius = 2;
		this.maxChunkDespawnsPerFrame = 1;
		this.maxChunkLifecycleOpsPerTick = 1;
		this.maxZoneSpawnsPerFrame = 1;
		this.zoneSpawnFrameBudgetMs = 0.5f;
		this.maxPostSpawnInitOpsPerFrame = 1;
		this.postSpawnInitFrameBudgetMs = 0.5f;
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x000224AC File Offset: 0x000206AC
	private void ApplyMedium()
	{
		this.maxVisibleRadius = 2;
		this.minVisibleRadius = 1;
		this.spawnRadius = this.maxVisibleRadius + 2;
		this.viewAngle = 110f;
		this.viewAngleBuffer = 20f;
		this.deactivationDelay = 8;
		this.visibilityCheckInterval = 0.15f;
		this.maxVisibilityChangesPerFrame = 3;
		this.maxPooledChunksPerPrefab = 5;
		this.maxAutoChunkPrewarmTarget = 24;
		this.prewarmInstancesPerFrame = 2;
		this.prewarmFrameBudgetMs = 1f;
		this.maxPooledZonesPerPrefab = 8;
		this.maxAutoZonePrewarmTarget = 64;
		this.enableIncrementalNormalZoneActivation = true;
		this.normalZoneStagingTransformThreshold = 128;
		this.normalZoneStagedMaxOperationsPerFrame = 48;
		this.normalZoneStagedFrameBudgetMs = 0.65f;
		this.runtimeDespawnRadius = 5;
		this.maxChunkDespawnsPerFrame = 1;
		this.maxChunkLifecycleOpsPerTick = 1;
		this.maxZoneSpawnsPerFrame = 3;
		this.zoneSpawnFrameBudgetMs = 0.75f;
		this.maxPostSpawnInitOpsPerFrame = 2;
		this.postSpawnInitFrameBudgetMs = 0.6f;
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x00022594 File Offset: 0x00020794
	private void ApplyHigh()
	{
		this.maxVisibleRadius = 3;
		this.minVisibleRadius = 1;
		this.spawnRadius = this.maxVisibleRadius + 2;
		this.viewAngle = 120f;
		this.viewAngleBuffer = 15f;
		this.deactivationDelay = 10;
		this.visibilityCheckInterval = 0.08f;
		this.maxVisibilityChangesPerFrame = 8;
		this.maxPooledChunksPerPrefab = 12;
		this.maxAutoChunkPrewarmTarget = 96;
		this.prewarmInstancesPerFrame = 3;
		this.prewarmFrameBudgetMs = 2f;
		this.maxPooledZonesPerPrefab = 24;
		this.maxAutoZonePrewarmTarget = 96;
		this.enableIncrementalNormalZoneActivation = true;
		this.normalZoneStagingTransformThreshold = 128;
		this.normalZoneStagedMaxOperationsPerFrame = 64;
		this.normalZoneStagedFrameBudgetMs = 0.75f;
		this.runtimeDespawnRadius = 6;
		this.maxChunkDespawnsPerFrame = 1;
		this.maxChunkLifecycleOpsPerTick = 1;
		this.maxZoneSpawnsPerFrame = 4;
		this.zoneSpawnFrameBudgetMs = 1f;
		this.maxPostSpawnInitOpsPerFrame = 2;
		this.postSpawnInitFrameBudgetMs = 0.75f;
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x00022680 File Offset: 0x00020880
	private void ApplyUltra()
	{
		this.maxVisibleRadius = 4;
		this.minVisibleRadius = 2;
		this.spawnRadius = this.maxVisibleRadius + 2;
		this.viewAngle = 130f;
		this.viewAngleBuffer = 12f;
		this.deactivationDelay = 12;
		this.visibilityCheckInterval = 0.08f;
		this.maxVisibilityChangesPerFrame = 5;
		this.maxPooledChunksPerPrefab = 10;
		this.maxAutoChunkPrewarmTarget = 48;
		this.prewarmInstancesPerFrame = 2;
		this.prewarmFrameBudgetMs = 2f;
		this.maxPooledZonesPerPrefab = 16;
		this.maxAutoZonePrewarmTarget = 128;
		this.enableIncrementalNormalZoneActivation = true;
		this.normalZoneStagingTransformThreshold = 128;
		this.normalZoneStagedMaxOperationsPerFrame = 96;
		this.normalZoneStagedFrameBudgetMs = 1f;
		this.runtimeDespawnRadius = 7;
		this.maxChunkDespawnsPerFrame = 2;
		this.maxChunkLifecycleOpsPerTick = 2;
		this.maxZoneSpawnsPerFrame = 5;
		this.zoneSpawnFrameBudgetMs = 1.5f;
		this.maxPostSpawnInitOpsPerFrame = 3;
		this.postSpawnInitFrameBudgetMs = 1f;
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0002276C File Offset: 0x0002096C
	public void ApplyWorldDensityPreset()
	{
		switch (this.worldDensity)
		{
		case ChunkManagerConfig.WorldDensityPreset.Custom:
			return;
		case ChunkManagerConfig.WorldDensityPreset.Low:
			this.minVisibleRadius = 1;
			this.maxVisibleRadius = 1;
			this.spawnRadius = this.maxVisibleRadius + 2;
			this.runtimeDespawnRadius = this.spawnRadius + 2;
			this.chunkPoolCoverageMultiplier = 1f;
			this.maxChunkDespawnsPerFrame = 4;
			this.maxChunkLifecycleOpsPerTick = 2;
			return;
		case ChunkManagerConfig.WorldDensityPreset.Medium:
			this.minVisibleRadius = 1;
			this.maxVisibleRadius = 2;
			this.spawnRadius = this.maxVisibleRadius + 2;
			this.runtimeDespawnRadius = this.spawnRadius + 2;
			this.chunkPoolCoverageMultiplier = 1.05f;
			this.maxChunkDespawnsPerFrame = 3;
			this.maxChunkLifecycleOpsPerTick = 2;
			return;
		case ChunkManagerConfig.WorldDensityPreset.High:
			this.minVisibleRadius = 2;
			this.maxVisibleRadius = 3;
			this.spawnRadius = this.maxVisibleRadius + 2;
			this.runtimeDespawnRadius = this.spawnRadius + 2;
			this.chunkPoolCoverageMultiplier = 1.15f;
			this.maxChunkDespawnsPerFrame = 2;
			this.maxChunkLifecycleOpsPerTick = 2;
			return;
		case ChunkManagerConfig.WorldDensityPreset.Ultra:
			this.minVisibleRadius = 2;
			this.maxVisibleRadius = 4;
			this.spawnRadius = this.maxVisibleRadius + 2;
			this.runtimeDespawnRadius = this.spawnRadius + 2;
			this.chunkPoolCoverageMultiplier = 1.25f;
			this.maxChunkDespawnsPerFrame = 2;
			this.maxChunkLifecycleOpsPerTick = 3;
			return;
		default:
			return;
		}
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x000228AC File Offset: 0x00020AAC
	private void OnValidate()
	{
		if (this.worldDensity == ChunkManagerConfig.WorldDensityPreset.Custom)
		{
			return;
		}
		this.minVisibleRadius = Mathf.Min(this.minVisibleRadius, this.maxVisibleRadius - 1);
		this.spawnRadius = this.maxVisibleRadius + 2;
		this.runtimeDespawnRadius = Mathf.Max(this.runtimeDespawnRadius, this.spawnRadius + 2);
	}

	// Token: 0x0400031B RID: 795
	[Header("Preset")]
	[Tooltip("Seleziona un preset e premi 'Apply Preset' nel context menu per sovrascrivere i valori sotto.")]
	public ChunkManagerConfig.QualityPreset activePreset = ChunkManagerConfig.QualityPreset.Medium;

	// Token: 0x0400031C RID: 796
	[Header("Spawn & Visibility")]
	[Tooltip("Raggio in celle entro cui vengono spawnati nuovi chunk attorno al player.")]
	[Range(1f, 10f)]
	public int spawnRadius = 3;

	// Token: 0x0400031D RID: 797
	[Tooltip("Raggio minimo: chunk sempre attivi indipendentemente dalla direzione camera.")]
	[Range(1f, 6f)]
	public int minVisibleRadius = 2;

	// Token: 0x0400031E RID: 798
	[Tooltip("Raggio massimo oltre cui i chunk vengono sempre nascosti.")]
	[Range(1f, 12f)]
	public int maxVisibleRadius = 6;

	// Token: 0x0400031F RID: 799
	[Tooltip("Angolo FOV per frustum culling (gradi). 180 = tutto davanti.")]
	[Range(45f, 180f)]
	public float viewAngle = 120f;

	// Token: 0x04000320 RID: 800
	[Tooltip("Buffer angolare extra per evitare flickering durante rotazione rapida.")]
	[Range(0f, 45f)]
	public float viewAngleBuffer = 15f;

	// Token: 0x04000321 RID: 801
	[Tooltip("Frame che un chunk deve restare fuori FOV prima di essere nascosto.")]
	[Range(1f, 30f)]
	public int deactivationDelay = 10;

	// Token: 0x04000322 RID: 802
	[Tooltip("Intervallo tra i check di visibilità (secondi).")]
	[Range(0.05f, 1f)]
	public float visibilityCheckInterval = 0.1f;

	// Token: 0x04000323 RID: 803
	[Tooltip("Chunk attivati/disattivati per frame massimo.")]
	[Range(1f, 20f)]
	public int maxVisibilityChangesPerFrame = 5;

	// Token: 0x04000324 RID: 804
	[Tooltip("Raggio (in celle) entro cui i Collider dei chunk restano attivi. Oltre, vengono disabilitati per ridurre la physics broadphase. 0 = sempre attivi (legacy).")]
	[Range(0f, 10f)]
	public int colliderActiveRadius = 2;

	// Token: 0x04000325 RID: 805
	[Tooltip("Intervallo (secondi) tra i check di far-collider toggle. Usa valori bassi solo se il player si muove molto veloce.")]
	[Range(0.1f, 2f)]
	public float colliderToggleInterval = 0.5f;

	// Token: 0x04000326 RID: 806
	[Header("Chunk Pool")]
	[Tooltip("Istanze inattive mantenute in pool per prefab chunk.")]
	[Range(1f, 64f)]
	public int maxPooledChunksPerPrefab = 12;

	// Token: 0x04000327 RID: 807
	[Tooltip("Cap massimo del prewarm auto-calcolato per i chunk.")]
	[Range(4f, 256f)]
	public int maxAutoChunkPrewarmTarget = 96;

	// Token: 0x04000328 RID: 808
	[Tooltip("Istanze create per frame durante il prewarm.")]
	[Range(1f, 16f)]
	public int prewarmInstancesPerFrame = 2;

	// Token: 0x04000329 RID: 809
	[Tooltip("Budget ms per frame durante il prewarm.")]
	[Range(0.25f, 8f)]
	public float prewarmFrameBudgetMs = 1.5f;

	// Token: 0x0400032A RID: 810
	[Header("Zone Pool")]
	[Tooltip("Istanze inattive mantenute in pool per prefab zona.")]
	[Range(1f, 128f)]
	public int maxPooledZonesPerPrefab = 16;

	// Token: 0x0400032B RID: 811
	[Tooltip("Cap massimo del prewarm auto-calcolato per le zone.")]
	[Range(16f, 1024f)]
	public int maxAutoZonePrewarmTarget = 192;

	// Token: 0x0400032C RID: 812
	[Tooltip("Ritira le gerarchie zona fuori mappa e le spegne a piccoli gruppi su più frame. Evita i freeze da SetActive(false) ricorsivo durante il traversal.")]
	public bool enableIncrementalZoneRetirement = true;

	// Token: 0x0400032D RID: 813
	[Tooltip("GameObject della gerarchia zona processati al massimo per frame durante il retirement.")]
	[Range(1f, 256f)]
	public int maxZoneRetirementGroupsPerFrame = 128;

	// Token: 0x0400032E RID: 814
	[Tooltip("Budget main-thread per frame del retirement zone. Ogni gruppo già iniziato viene completato.")]
	[Range(0.1f, 4f)]
	public float zoneRetirementFrameBudgetMs = 0.75f;

	// Token: 0x0400032F RID: 815
	[Tooltip("Cap separato per GameObject distrutti per frame, isolati leaf-to-root.")]
	[Range(1f, 64f)]
	public int maxZoneDestructionGroupsPerFrame = 16;

	// Token: 0x04000330 RID: 816
	[Tooltip("High-water dei gruppi di lavoro retirement schedulati. I despawn restano in coda finché il backlog scende sotto metà soglia.")]
	[Range(256f, 32768f)]
	public int maxPendingZoneRetirementWorkGroups = 4096;

	// Token: 0x04000331 RID: 817
	[Tooltip("Le zone normali pooled sopra la soglia mantengono i discendenti dormienti e li riattivano a piccoli batch, evitando SetActive ricorsivi durante il gameplay.")]
	public bool enableIncrementalNormalZoneActivation = true;

	// Token: 0x04000332 RID: 818
	[Tooltip("Numero minimo di Transform discendenti per applicare la staged activation a una zona normale.")]
	[Range(32f, 4096f)]
	public int normalZoneStagingTransformThreshold = 128;

	// Token: 0x04000333 RID: 819
	[Tooltip("Transform riattivati al massimo per frame per ciascuna zona normale staged.")]
	[Range(1f, 256f)]
	public int normalZoneStagedMaxOperationsPerFrame = 32;

	// Token: 0x04000334 RID: 820
	[Tooltip("Budget main-thread per frame per ciascuna zona normale staged.")]
	[Range(0.1f, 4f)]
	public float normalZoneStagedFrameBudgetMs = 0.5f;

	// Token: 0x04000335 RID: 821
	[Header("Runtime Despawn")]
	[Tooltip("Raggio in celle oltre cui i chunk vengono rilasciati nel pool.")]
	[Range(2f, 32f)]
	public int runtimeDespawnRadius = 10;

	// Token: 0x04000336 RID: 822
	[Tooltip("Chunk despawnati per frame massimo.")]
	[Range(1f, 8f)]
	public int maxChunkDespawnsPerFrame = 1;

	// Token: 0x04000337 RID: 823
	[Tooltip("Operazioni lifecycle (spawn+despawn) per tick.")]
	[Range(1f, 4f)]
	public int maxChunkLifecycleOpsPerTick = 1;

	// Token: 0x04000338 RID: 824
	[Header("Deferred Zone Spawn")]
	[Tooltip("Zone spawnate per frame nella coda differita.")]
	[Range(1f, 16f)]
	public int maxZoneSpawnsPerFrame = 3;

	// Token: 0x04000339 RID: 825
	[Tooltip("Budget ms per frame per lo spawn zone differito.")]
	[Range(0.25f, 8f)]
	public float zoneSpawnFrameBudgetMs = 1f;

	// Token: 0x0400033A RID: 826
	[Header("Post Spawn Init")]
	[Tooltip("Chunk inizializzati per frame nella coda post-spawn.")]
	[Range(1f, 16f)]
	public int maxPostSpawnInitOpsPerFrame = 2;

	// Token: 0x0400033B RID: 827
	[Tooltip("Budget ms per frame per la coda post-spawn init.")]
	[Range(0.1f, 8f)]
	public float postSpawnInitFrameBudgetMs = 0.75f;

	// Token: 0x0400033C RID: 828
	[Header("Feature Flags")]
	[Tooltip("Usa Job System per calcoli di visibilità (richiede Burst/Collections)")]
	public bool useJobs = true;

	// Token: 0x0400033D RID: 829
	[Tooltip("Abilita il pooling dei chunk runtime per ridurre instantiate/destroy")]
	public bool enableChunkPooling = true;

	// Token: 0x0400033E RID: 830
	[Tooltip("Abilita il pooling delle zone (parte più pesante del runtime spawn)")]
	public bool enableZonePooling = true;

	// Token: 0x0400033F RID: 831
	[Tooltip("Esegue prewarm del pool all'avvio in modo progressivo")]
	public bool prewarmPoolOnStart = true;

	// Token: 0x04000340 RID: 832
	[Tooltip("Prewarm iniziale delle zone nel pool. OFF di default: le zone sono gerarchie molto grandi e il prewarm di ogni tipo puo saturare RAM/VRAM nelle build.")]
	public bool prewarmZonePoolOnStart;

	// Token: 0x04000341 RID: 833
	[Tooltip("Esegue Awake/OnEnable/Start su ogni clone del pool zone durante il loading. OFF di default: il pool viene costruito sotto un contenitore inattivo e le gerarchie grandi vengono risvegliate incrementalmente al primo uso.")]
	public bool warmZonePoolLifecycleOnStart;

	// Token: 0x04000342 RID: 834
	[Tooltip("Materializza all'avvio i payload Resources delle zone pesanti. OFF di default: un payload come Park puo trattenere diversi GB di texture/mesh e migliaia di GameObject per tutta la sessione.")]
	public bool preloadHeavyZonePayloadsOnStart;

	// Token: 0x04000343 RID: 835
	[Header("Heavy POI Predictive Prefetch")]
	[Tooltip("Avvia soltanto Resources.LoadAsync del payload heavy lungo la traiettoria prevista. Non istanzia gerarchie e non modifica i raggi di spawn/visibilita.")]
	public bool enableAdaptiveHeavyPoiPrefetch = true;

	// Token: 0x04000344 RID: 836
	[Tooltip("Secondi di anticipo I/O desiderati alla velocita corrente.")]
	[Range(0.5f, 8f)]
	public float heavyPoiPrefetchTargetLeadSeconds = 3f;

	// Token: 0x04000345 RID: 837
	[Tooltip("Fallback conservativo quando la velocita non e ancora attendibile.")]
	[Range(1f, 8f)]
	public int heavyPoiPrefetchMinLeadCells = 3;

	// Token: 0x04000346 RID: 838
	[Tooltip("Limite superiore del lookahead per contenere cache resolver, I/O e memoria Resources.")]
	[Range(1f, 16f)]
	public int heavyPoiPrefetchMaxLeadCells = 8;

	// Token: 0x04000347 RID: 839
	[Tooltip("Celle laterali controllate su ciascun lato. Due coprono anche l'ingresso laterale di una footprint 4x4.")]
	[Range(0f, 4f)]
	public int heavyPoiPrefetchLateralCells = 2;

	// Token: 0x04000348 RID: 840
	[Tooltip("Margine in avanti per intercettare l'origine di POI multi-cella senza anticiparne l'Instantiate.")]
	[Range(0f, 4f)]
	public int heavyPoiPrefetchFootprintPaddingCells = 2;

	// Token: 0x04000349 RID: 841
	[Tooltip("Dopo il completamento del loading impedisce Instantiate sincroni delle zone. Una richiesta senza istanza pooled viene rimessa in coda finche' un'istanza torna disponibile.")]
	public bool forbidRuntimeZoneInstantiate = true;

	// Token: 0x0400034A RID: 842
	[Tooltip("Dopo il prewarm, renderizza offscreen un'istanza di ogni prefab pooled. OFF di default: forza l'upload GPU dell'intero catalogo e puo causare memory thrashing.")]
	public bool warmRenderPoolOnStart;

	// Token: 0x0400034B RID: 843
	[Tooltip("Se true, processa spawn/despawn in coda sequenziale nello stesso tick")]
	public bool useSequentialChunkLifecycleQueue = true;

	// Token: 0x0400034C RID: 844
	[Tooltip("Spawna le zone di ogni chunk in modo differito su più frame")]
	public bool enableDeferredZoneSpawn = true;

	// Token: 0x0400034D RID: 845
	[Tooltip("Esegue inizializzazioni non critiche dei chunk in coda con budget per frame")]
	public bool enableDeferredPostSpawnInit = true;

	// Token: 0x0400034E RID: 846
	[Tooltip("Prepara la cache visuale dei chunk in modo differito per ridurre spike al primo hide/show")]
	public bool preCacheVisualsAfterSpawn = true;

	// Token: 0x0400034F RID: 847
	[Tooltip("Abilita cache persistente su disco dei target prewarm map pool")]
	public bool enablePersistentPrewarmTargetCache = true;

	// Token: 0x04000350 RID: 848
	[Tooltip("Auto-calcola il target di prewarm chunk in base ai chunk potenzialmente mantenuti vivi")]
	public bool autoSizeChunkPrewarmFromCoverage = true;

	// Token: 0x04000351 RID: 849
	[Tooltip("Se true usa runtimeDespawnRadius come copertura pool quando il runtime despawn è attivo")]
	public bool useRuntimeDespawnRadiusForPoolCoverage = true;

	// Token: 0x04000352 RID: 850
	[Tooltip("Se attivo, rilascia nel pool i chunk runtime troppo lontani dal player")]
	public bool enableRuntimeChunkDespawn = true;

	// Token: 0x04000353 RID: 851
	[Tooltip("Se true, calcola il target minimo del pool zone dal raggio di copertura effettiva")]
	public bool useDynamicZonePoolTarget = true;

	// Token: 0x04000354 RID: 852
	[Header("Pool Coverage")]
	[Tooltip("Moltiplicatore sulla copertura pool chunk (1.0 = copertura completa stimata)")]
	[Range(0.75f, 1.5f)]
	public float chunkPoolCoverageMultiplier = 1.05f;

	// Token: 0x04000355 RID: 853
	[Tooltip("Moltiplicatore di sicurezza per coprire picchi zone (es. 1.25 = 25% buffer)")]
	[Range(1f, 3f)]
	public float zonePoolCoverageMultiplier = 1.3f;

	// Token: 0x04000356 RID: 854
	[Header("Prewarm Manual Counts")]
	[Tooltip("Target manuale fallback chunk prewarm quando autoSizeChunkPrewarmFromCoverage è disattivato")]
	[Range(0f, 256f)]
	public int initialPrewarmCount = 24;

	// Token: 0x04000357 RID: 855
	[Tooltip("Target manuale fallback zone prewarm quando useDynamicZonePoolTarget è disattivato")]
	[Range(0f, 1024f)]
	public int initialZonePrewarmCount = 96;

	// Token: 0x04000358 RID: 856
	[Tooltip("Numero minimo di istanze prewarm per prefab chunk valido")]
	[Range(0f, 8f)]
	public int minPrewarmPerPrefab = 1;

	// Token: 0x04000359 RID: 857
	[Tooltip("Numero minimo di istanze zone per prefab")]
	[Range(0f, 32f)]
	public int minZonePrewarmPerPrefab = 1;

	// Token: 0x0400035A RID: 858
	[Header("Loading Boost (budget usati SOLO durante il loading iniziale)")]
	[Tooltip("Se attivo, finché la mappa non è pronta (IsMapReady=false, loading screen su) il ChunkManager usa i budget 'loading*' qui sotto al posto di quelli conservativi. Tanto lo schermo è coperto: budget alti = loading reale più corto.")]
	public bool useLoadingBudgets = true;

	// Token: 0x0400035B RID: 859
	[Tooltip("Istanze create per frame durante il prewarm SOTTO loading (al posto di prewarmInstancesPerFrame).")]
	[Range(1f, 64f)]
	public int loadingPrewarmInstancesPerFrame = 8;

	// Token: 0x0400035C RID: 860
	[Tooltip("Budget ms per frame del prewarm SOTTO loading (al posto di prewarmFrameBudgetMs).")]
	[Range(1f, 33f)]
	public float loadingPrewarmFrameBudgetMs = 12f;

	// Token: 0x0400035D RID: 861
	[Tooltip("Zone spawnate per frame SOTTO loading (al posto di maxZoneSpawnsPerFrame).")]
	[Range(1f, 64f)]
	public int loadingMaxZoneSpawnsPerFrame = 12;

	// Token: 0x0400035E RID: 862
	[Tooltip("Budget ms per frame dello zone spawn differito SOTTO loading (al posto di zoneSpawnFrameBudgetMs).")]
	[Range(1f, 33f)]
	public float loadingZoneSpawnFrameBudgetMs = 10f;

	// Token: 0x0400035F RID: 863
	[Tooltip("Operazioni post-spawn init per frame SOTTO loading (al posto di maxPostSpawnInitOpsPerFrame).")]
	[Range(1f, 32f)]
	public int loadingMaxPostSpawnInitOpsPerFrame = 8;

	// Token: 0x04000360 RID: 864
	[Tooltip("Budget ms per frame del post-spawn init SOTTO loading (al posto di postSpawnInitFrameBudgetMs).")]
	[Range(0.5f, 33f)]
	public float loadingPostSpawnInitFrameBudgetMs = 6f;

	// Token: 0x04000361 RID: 865
	[Tooltip("Operazioni lifecycle chunk (spawn) per tick SOTTO loading (al posto di maxChunkLifecycleOpsPerTick). Sotto loading il tick gira OGNI frame, senza throttle da intervallo.")]
	[Range(1f, 8f)]
	public int loadingMaxChunkLifecycleOpsPerTick = 4;

	// Token: 0x04000362 RID: 866
	[Header("Cache")]
	[Tooltip("Forza rebuild della cache prewarm al prossimo avvio")]
	public bool forceRebuildPrewarmTargetCache;

	// Token: 0x04000363 RID: 867
	[SerializeField]
	[HideInInspector]
	public int prewarmTargetCacheSchemaVersion = 6;

	// Token: 0x04000364 RID: 868
	[Header("World Density (gameplay quality preset)")]
	[Tooltip("Quanta 'larga' è la copertura del mondo intorno al player. Influisce su spawnRadius e sul pool prewarm. Usa Custom per editare i valori manualmente; gli altri preset li sovrascrivono in Awake/OnEnable.")]
	public ChunkManagerConfig.WorldDensityPreset worldDensity = ChunkManagerConfig.WorldDensityPreset.Medium;

	// Token: 0x02000056 RID: 86
	public enum QualityPreset
	{
		// Token: 0x04000366 RID: 870
		Custom,
		// Token: 0x04000367 RID: 871
		Low,
		// Token: 0x04000368 RID: 872
		Medium,
		// Token: 0x04000369 RID: 873
		High,
		// Token: 0x0400036A RID: 874
		Ultra
	}

	// Token: 0x02000057 RID: 87
	public enum WorldDensityPreset
	{
		// Token: 0x0400036C RID: 876
		Custom,
		// Token: 0x0400036D RID: 877
		Low,
		// Token: 0x0400036E RID: 878
		Medium,
		// Token: 0x0400036F RID: 879
		High,
		// Token: 0x04000370 RID: 880
		Ultra
	}
}
