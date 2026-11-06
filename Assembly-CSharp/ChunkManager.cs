using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Features.Detection;
using Features.MapGeneration.Scripts;
using Features.NPC;
using Features.Rank;
using Features.RenderingPrecision;
using Features.RunModes;
using Features.WantedPoster;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Diagnostics;
using ProjectExhibitionist.Subway;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class ChunkManager : MonoBehaviour
{
	// Token: 0x1700004C RID: 76
	// (get) Token: 0x060001CC RID: 460 RVA: 0x0000CB7A File Offset: 0x0000AD7A
	// (set) Token: 0x060001CD RID: 461 RVA: 0x0000CB81 File Offset: 0x0000AD81
	public static ChunkManager Instance { get; private set; }

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060001CE RID: 462 RVA: 0x0000CB89 File Offset: 0x0000AD89
	// (set) Token: 0x060001CF RID: 463 RVA: 0x0000CB91 File Offset: 0x0000AD91
	public string ActiveMapCatalogResource { get; private set; } = "MapCatalog";

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000CB9A File Offset: 0x0000AD9A
	public bool IsCatalogLoaded
	{
		get
		{
			return this.availableChunks != null && this.availableChunks.Length != 0;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000CBB0 File Offset: 0x0000ADB0
	public bool CatalogLoadFailed
	{
		get
		{
			return this._catalogLoadFailed;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000CBB8 File Offset: 0x0000ADB8
	public bool IsAwaitingCatalog
	{
		get
		{
			return !this.IsCatalogLoaded && !this._catalogLoadFailed && !this.IsTearingDown && base.isActiveAndEnabled;
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000CBDA File Offset: 0x0000ADDA
	public bool IsReadingCatalog
	{
		get
		{
			return this._catalogRequest != null;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000CBE5 File Offset: 0x0000ADE5
	public float CatalogLoadProgress
	{
		get
		{
			if (this.IsCatalogLoaded)
			{
				return 1f;
			}
			if (this._catalogRequest == null)
			{
				return 0f;
			}
			return Mathf.Clamp01(this._catalogRequest.progress);
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000CC13 File Offset: 0x0000AE13
	private int spawnRadius
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 3;
			}
			return chunkManagerConfig.spawnRadius;
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000CC26 File Offset: 0x0000AE26
	private int minVisibleRadius
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 2;
			}
			return chunkManagerConfig.minVisibleRadius;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000CC39 File Offset: 0x0000AE39
	private int maxVisibleRadius
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 6;
			}
			return chunkManagerConfig.maxVisibleRadius;
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000CC4C File Offset: 0x0000AE4C
	private float viewAngle
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 120f;
			}
			return chunkManagerConfig.viewAngle;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000CC63 File Offset: 0x0000AE63
	private float viewAngleBuffer
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 15f;
			}
			return chunkManagerConfig.viewAngleBuffer;
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x060001DA RID: 474 RVA: 0x0000CC7A File Offset: 0x0000AE7A
	private int deactivationDelay
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 10;
			}
			return chunkManagerConfig.deactivationDelay;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x060001DB RID: 475 RVA: 0x0000CC8E File Offset: 0x0000AE8E
	private float visibilityCheckInterval
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 0.1f;
			}
			return chunkManagerConfig.visibilityCheckInterval;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x060001DC RID: 476 RVA: 0x0000CCA5 File Offset: 0x0000AEA5
	private int maxVisibilityChangesPerFrame
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 5;
			}
			return chunkManagerConfig.maxVisibilityChangesPerFrame;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x060001DD RID: 477 RVA: 0x0000CCB8 File Offset: 0x0000AEB8
	private int colliderActiveRadius
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 2;
			}
			return chunkManagerConfig.colliderActiveRadius;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x060001DE RID: 478 RVA: 0x0000CCCB File Offset: 0x0000AECB
	private float colliderToggleInterval
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 0.5f;
			}
			return chunkManagerConfig.colliderToggleInterval;
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x060001DF RID: 479 RVA: 0x0000CCE2 File Offset: 0x0000AEE2
	private bool enableChunkPooling
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.enableChunkPooling;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000CCF5 File Offset: 0x0000AEF5
	private bool prewarmPoolOnStart
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.prewarmPoolOnStart;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000CD08 File Offset: 0x0000AF08
	private bool warmRenderPoolOnStart
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.warmRenderPoolOnStart;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000CD1B File Offset: 0x0000AF1B
	private int initialPrewarmCount
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 24;
			}
			return chunkManagerConfig.initialPrewarmCount;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000CD2F File Offset: 0x0000AF2F
	private int minPrewarmPerPrefab
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 1;
			}
			return chunkManagerConfig.minPrewarmPerPrefab;
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000CD42 File Offset: 0x0000AF42
	private int prewarmInstancesPerFrame
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 2;
			}
			return chunkManagerConfig.prewarmInstancesPerFrame;
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000CD55 File Offset: 0x0000AF55
	private float prewarmFrameBudgetMs
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 1.5f;
			}
			return chunkManagerConfig.prewarmFrameBudgetMs;
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000CD6C File Offset: 0x0000AF6C
	private int maxPooledInstancesPerPrefab
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 12;
			}
			return chunkManagerConfig.maxPooledChunksPerPrefab;
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000CD80 File Offset: 0x0000AF80
	private int effectiveMaxPooledInstancesPerPrefab
	{
		get
		{
			return Mathf.Max(this.maxPooledInstancesPerPrefab, 2 * Mathf.Max(1, this.spawnRadius) + 1);
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000CD9D File Offset: 0x0000AF9D
	private bool autoSizeChunkPrewarmFromCoverage
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.autoSizeChunkPrewarmFromCoverage;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000CDB0 File Offset: 0x0000AFB0
	private bool useRuntimeDespawnRadiusForPoolCoverage
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.useRuntimeDespawnRadiusForPoolCoverage;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x060001EA RID: 490 RVA: 0x0000CDC3 File Offset: 0x0000AFC3
	private float chunkPoolCoverageMultiplier
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 1.05f;
			}
			return chunkManagerConfig.chunkPoolCoverageMultiplier;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x060001EB RID: 491 RVA: 0x0000CDDA File Offset: 0x0000AFDA
	private int maxAutoChunkPrewarmTarget
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 96;
			}
			return chunkManagerConfig.maxAutoChunkPrewarmTarget;
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x060001EC RID: 492 RVA: 0x0000CDEE File Offset: 0x0000AFEE
	private bool enableZonePooling
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.enableZonePooling;
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x060001ED RID: 493 RVA: 0x0000CE01 File Offset: 0x0000B001
	private bool prewarmZonePoolOnStart
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.prewarmZonePoolOnStart;
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x060001EE RID: 494 RVA: 0x0000CE14 File Offset: 0x0000B014
	private bool warmZonePoolLifecycleOnStart
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig != null && chunkManagerConfig.warmZonePoolLifecycleOnStart;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060001EF RID: 495 RVA: 0x0000CE27 File Offset: 0x0000B027
	private bool preloadHeavyZonePayloadsOnStart
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig != null && chunkManagerConfig.preloadHeavyZonePayloadsOnStart;
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000CE3A File Offset: 0x0000B03A
	private bool useDynamicZonePoolTarget
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.useDynamicZonePoolTarget;
		}
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000CE4D File Offset: 0x0000B04D
	private int initialZonePrewarmCount
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 96;
			}
			return chunkManagerConfig.initialZonePrewarmCount;
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000CE61 File Offset: 0x0000B061
	private float zonePoolCoverageMultiplier
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 1.3f;
			}
			return chunkManagerConfig.zonePoolCoverageMultiplier;
		}
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000CE78 File Offset: 0x0000B078
	private int maxAutoZonePrewarmTarget
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 192;
			}
			return chunkManagerConfig.maxAutoZonePrewarmTarget;
		}
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000CE8F File Offset: 0x0000B08F
	private int minZonePrewarmPerPrefab
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 1;
			}
			return chunkManagerConfig.minZonePrewarmPerPrefab;
		}
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000CEA2 File Offset: 0x0000B0A2
	private int maxPooledZonesPerPrefab
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 32;
			}
			return chunkManagerConfig.maxPooledZonesPerPrefab;
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000CEB6 File Offset: 0x0000B0B6
	private bool forbidRuntimeZoneInstantiate
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.forbidRuntimeZoneInstantiate;
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000CEC9 File Offset: 0x0000B0C9
	private bool enableIncrementalZoneRetirement
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.enableIncrementalZoneRetirement;
		}
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000CEDC File Offset: 0x0000B0DC
	private int maxZoneRetirementGroupsPerFrame
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 128;
			}
			return chunkManagerConfig.maxZoneRetirementGroupsPerFrame;
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000CEF3 File Offset: 0x0000B0F3
	private float zoneRetirementFrameBudgetMs
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 0.75f;
			}
			return chunkManagerConfig.zoneRetirementFrameBudgetMs;
		}
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x060001FA RID: 506 RVA: 0x0000CF0A File Offset: 0x0000B10A
	private int maxZoneDestructionGroupsPerFrame
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 16;
			}
			return chunkManagerConfig.maxZoneDestructionGroupsPerFrame;
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x060001FB RID: 507 RVA: 0x0000CF1E File Offset: 0x0000B11E
	private int maxPendingZoneRetirementWorkGroups
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 4096;
			}
			return chunkManagerConfig.maxPendingZoneRetirementWorkGroups;
		}
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x060001FC RID: 508 RVA: 0x0000CF35 File Offset: 0x0000B135
	private bool enableIncrementalNormalZoneActivation
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.enableIncrementalNormalZoneActivation;
		}
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x060001FD RID: 509 RVA: 0x0000CF48 File Offset: 0x0000B148
	private int normalZoneStagingTransformThreshold
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 128;
			}
			return chunkManagerConfig.normalZoneStagingTransformThreshold;
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x060001FE RID: 510 RVA: 0x0000CF5F File Offset: 0x0000B15F
	private int normalZoneStagedMaxOperationsPerFrame
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 32;
			}
			return chunkManagerConfig.normalZoneStagedMaxOperationsPerFrame;
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x060001FF RID: 511 RVA: 0x0000CF73 File Offset: 0x0000B173
	private float normalZoneStagedFrameBudgetMs
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 0.5f;
			}
			return chunkManagerConfig.normalZoneStagedFrameBudgetMs;
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x06000200 RID: 512 RVA: 0x0000CF8A File Offset: 0x0000B18A
	private bool enableAdaptiveHeavyPoiPrefetch
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.enableAdaptiveHeavyPoiPrefetch;
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000201 RID: 513 RVA: 0x0000CF9D File Offset: 0x0000B19D
	private float heavyPoiPrefetchTargetLeadSeconds
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 3f;
			}
			return chunkManagerConfig.heavyPoiPrefetchTargetLeadSeconds;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000202 RID: 514 RVA: 0x0000CFB4 File Offset: 0x0000B1B4
	private int heavyPoiPrefetchMinLeadCells
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 3;
			}
			return chunkManagerConfig.heavyPoiPrefetchMinLeadCells;
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06000203 RID: 515 RVA: 0x0000CFC7 File Offset: 0x0000B1C7
	private int heavyPoiPrefetchMaxLeadCells
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 8;
			}
			return chunkManagerConfig.heavyPoiPrefetchMaxLeadCells;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000204 RID: 516 RVA: 0x0000CFDA File Offset: 0x0000B1DA
	private int heavyPoiPrefetchLateralCells
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 2;
			}
			return chunkManagerConfig.heavyPoiPrefetchLateralCells;
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000205 RID: 517 RVA: 0x0000CFED File Offset: 0x0000B1ED
	private int heavyPoiPrefetchFootprintPaddingCells
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 2;
			}
			return chunkManagerConfig.heavyPoiPrefetchFootprintPaddingCells;
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000206 RID: 518 RVA: 0x0000D000 File Offset: 0x0000B200
	// (set) Token: 0x06000207 RID: 519 RVA: 0x0000D008 File Offset: 0x0000B208
	public int RuntimeChunkPoolMisses { get; private set; }

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x06000208 RID: 520 RVA: 0x0000D011 File Offset: 0x0000B211
	// (set) Token: 0x06000209 RID: 521 RVA: 0x0000D019 File Offset: 0x0000B219
	public int RuntimeZonePoolMisses { get; private set; }

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x0600020A RID: 522 RVA: 0x0000D022 File Offset: 0x0000B222
	// (set) Token: 0x0600020B RID: 523 RVA: 0x0000D02A File Offset: 0x0000B22A
	public int RuntimeZonePoolDeferrals { get; private set; }

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x0600020C RID: 524 RVA: 0x0000D033 File Offset: 0x0000B233
	// (set) Token: 0x0600020D RID: 525 RVA: 0x0000D03B File Offset: 0x0000B23B
	public int DeferredStreamingBudgetFrame { get; private set; } = -1;

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x0600020E RID: 526 RVA: 0x0000D044 File Offset: 0x0000B244
	// (set) Token: 0x0600020F RID: 527 RVA: 0x0000D04C File Offset: 0x0000B24C
	public double DeferredStreamingBudgetMsLastFrame { get; private set; }

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06000210 RID: 528 RVA: 0x0000D055 File Offset: 0x0000B255
	// (set) Token: 0x06000211 RID: 529 RVA: 0x0000D05D File Offset: 0x0000B25D
	public double DeferredStreamingElapsedMsLastFrame { get; private set; }

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06000212 RID: 530 RVA: 0x0000D066 File Offset: 0x0000B266
	// (set) Token: 0x06000213 RID: 531 RVA: 0x0000D06E File Offset: 0x0000B26E
	public int DeferredStreamingBudgetDeferralsLastFrame { get; private set; }

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x06000214 RID: 532 RVA: 0x0000D077 File Offset: 0x0000B277
	// (set) Token: 0x06000215 RID: 533 RVA: 0x0000D07F File Offset: 0x0000B27F
	public bool DeferredStreamingBudgetActiveLastFrame { get; private set; }

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x06000216 RID: 534 RVA: 0x0000D088 File Offset: 0x0000B288
	// (set) Token: 0x06000217 RID: 535 RVA: 0x0000D090 File Offset: 0x0000B290
	public int PrebakedMeshColliderVariants { get; private set; }

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000218 RID: 536 RVA: 0x0000D099 File Offset: 0x0000B299
	// (set) Token: 0x06000219 RID: 537 RVA: 0x0000D0A1 File Offset: 0x0000B2A1
	public int SkippedUnreadableMeshColliderVariants { get; private set; }

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x0600021A RID: 538 RVA: 0x0000D0AA File Offset: 0x0000B2AA
	// (set) Token: 0x0600021B RID: 539 RVA: 0x0000D0B2 File Offset: 0x0000B2B2
	public int RuntimeZonePoolFallbacks { get; private set; }

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x0600021C RID: 540 RVA: 0x0000D0BB File Offset: 0x0000B2BB
	// (set) Token: 0x0600021D RID: 541 RVA: 0x0000D0C3 File Offset: 0x0000B2C3
	public int RuntimeZoneColdInstantiates { get; private set; }

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x0600021E RID: 542 RVA: 0x0000D0CC File Offset: 0x0000B2CC
	// (set) Token: 0x0600021F RID: 543 RVA: 0x0000D0D4 File Offset: 0x0000B2D4
	public int TotalZoneInstantiates { get; private set; }

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x06000220 RID: 544 RVA: 0x0000D0DD File Offset: 0x0000B2DD
	public int AvailableZonePoolCount
	{
		get
		{
			return ChunkManager.GetTotalPoolCount(this._zonePoolByPrefab);
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x06000221 RID: 545 RVA: 0x0000D0EA File Offset: 0x0000B2EA
	public int AvailableChunkPoolCount
	{
		get
		{
			return ChunkManager.GetTotalPoolCount(this._chunkPoolByPrefab);
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x06000222 RID: 546 RVA: 0x0000D0F7 File Offset: 0x0000B2F7
	public int PendingZoneSpawnCount
	{
		get
		{
			return this._pendingZoneSpawns.Count;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x06000223 RID: 547 RVA: 0x0000D104 File Offset: 0x0000B304
	public int PendingZoneRetirementCount
	{
		get
		{
			return this._pendingZoneRetirements.Count;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x06000224 RID: 548 RVA: 0x0000D111 File Offset: 0x0000B311
	// (set) Token: 0x06000225 RID: 549 RVA: 0x0000D119 File Offset: 0x0000B319
	public int ZoneRetirementGroupsProcessedLastFrame { get; private set; }

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x06000226 RID: 550 RVA: 0x0000D122 File Offset: 0x0000B322
	// (set) Token: 0x06000227 RID: 551 RVA: 0x0000D12A File Offset: 0x0000B32A
	public int ZoneRetirementDestroyGroupsProcessedLastFrame { get; private set; }

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x06000228 RID: 552 RVA: 0x0000D133 File Offset: 0x0000B333
	public int PendingZoneRetirementWorkGroups
	{
		get
		{
			return this._pendingZoneRetirementWorkGroups;
		}
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x06000229 RID: 553 RVA: 0x0000D13B File Offset: 0x0000B33B
	// (set) Token: 0x0600022A RID: 554 RVA: 0x0000D143 File Offset: 0x0000B343
	public int PeakPendingZoneRetirementWorkGroups { get; private set; }

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x0600022B RID: 555 RVA: 0x0000D14C File Offset: 0x0000B34C
	// (set) Token: 0x0600022C RID: 556 RVA: 0x0000D154 File Offset: 0x0000B354
	public bool IsZoneRetirementBackpressured { get; private set; }

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x0600022D RID: 557 RVA: 0x0000D15D File Offset: 0x0000B35D
	// (set) Token: 0x0600022E RID: 558 RVA: 0x0000D165 File Offset: 0x0000B365
	public int ZoneRetirementDespawnDeferrals { get; private set; }

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x0600022F RID: 559 RVA: 0x0000D16E File Offset: 0x0000B36E
	public float OldestZoneRetirementAgeMs
	{
		get
		{
			return this.GetOldestZoneRetirementAgeMs();
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x06000230 RID: 560 RVA: 0x0000D176 File Offset: 0x0000B376
	public int PendingChunkDespawnCount
	{
		get
		{
			return this._pendingChunkDespawns.Count;
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x06000231 RID: 561 RVA: 0x0000D183 File Offset: 0x0000B383
	public int PendingNormalZoneWakeCount
	{
		get
		{
			return this._pendingNormalZoneWakeSet.Count;
		}
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x06000232 RID: 562 RVA: 0x0000D190 File Offset: 0x0000B390
	// (set) Token: 0x06000233 RID: 563 RVA: 0x0000D198 File Offset: 0x0000B398
	public int NormalZoneWakeOperationsLastFrame { get; private set; }

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06000234 RID: 564 RVA: 0x0000D1A1 File Offset: 0x0000B3A1
	// (set) Token: 0x06000235 RID: 565 RVA: 0x0000D1A9 File Offset: 0x0000B3A9
	public int NormalZoneWakePrioritySelectionsLastFrame { get; private set; }

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x06000236 RID: 566 RVA: 0x0000D1B2 File Offset: 0x0000B3B2
	// (set) Token: 0x06000237 RID: 567 RVA: 0x0000D1BA File Offset: 0x0000B3BA
	public double NormalZoneWakeElapsedMsLastFrame { get; private set; }

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06000238 RID: 568 RVA: 0x0000D1C3 File Offset: 0x0000B3C3
	public int PendingVisibilityActivationCount
	{
		get
		{
			return this._pendingActivationSet.Count;
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x06000239 RID: 569 RVA: 0x0000D1D0 File Offset: 0x0000B3D0
	public int PendingVisibilityDeactivationCount
	{
		get
		{
			return this._pendingDeactivationSet.Count;
		}
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x0600023A RID: 570 RVA: 0x0000D1DD File Offset: 0x0000B3DD
	public int SpawnedChunkCount
	{
		get
		{
			return this._spawnedChunks.Count;
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x0600023B RID: 571 RVA: 0x0000D1EA File Offset: 0x0000B3EA
	public int PresentationVisibleChunkCount
	{
		get
		{
			return this.CountPresentationVisibleChunks();
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x0600023C RID: 572 RVA: 0x0000D1F2 File Offset: 0x0000B3F2
	public int VisuallyHiddenChunkCount
	{
		get
		{
			return this._visuallyHiddenChunks.Count;
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x0600023D RID: 573 RVA: 0x0000D1FF File Offset: 0x0000B3FF
	public int ResolverCachedCellCount
	{
		get
		{
			ChunkSpawnResolver spawnResolver = this._spawnResolver;
			if (spawnResolver == null)
			{
				return 0;
			}
			return spawnResolver.CachedCellCount;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x0600023E RID: 574 RVA: 0x0000D212 File Offset: 0x0000B412
	public int CommittedSpawnPlanCount
	{
		get
		{
			return this._committedSpawnPlan.Count;
		}
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x0600023F RID: 575 RVA: 0x0000D21F File Offset: 0x0000B41F
	public int CommittedSpawnOwnerCount
	{
		get
		{
			return this._committedSpawnCellOwners.Count;
		}
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06000240 RID: 576 RVA: 0x0000D22C File Offset: 0x0000B42C
	// (set) Token: 0x06000241 RID: 577 RVA: 0x0000D234 File Offset: 0x0000B434
	public int MiniMapLayoutRevision { get; private set; }

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x06000242 RID: 578 RVA: 0x0000D23D File Offset: 0x0000B43D
	public int ResidentHeavyPayloadCount
	{
		get
		{
			return this.CountResidentHeavyPayloads();
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06000243 RID: 579 RVA: 0x0000D245 File Offset: 0x0000B445
	public int PresentationVisibleHeavyPayloadCount
	{
		get
		{
			return this.CountPresentationVisibleHeavyPayloads();
		}
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000244 RID: 580 RVA: 0x0000D24D File Offset: 0x0000B44D
	public int TrackedHeavyPayloadCount
	{
		get
		{
			return this._heavyZoneOwners.Count;
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000245 RID: 581 RVA: 0x0000D25A File Offset: 0x0000B45A
	public int PendingHeavyPayloadCount
	{
		get
		{
			return this.HeavyPayloadLoadPendingCount + this.HeavyPayloadWakePendingCount;
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x06000246 RID: 582 RVA: 0x0000D269 File Offset: 0x0000B469
	public int HeavyPayloadLoadPendingCount
	{
		get
		{
			return this.CountPendingHeavyPayloads(false);
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x06000247 RID: 583 RVA: 0x0000D272 File Offset: 0x0000B472
	public int HeavyPayloadWakePendingCount
	{
		get
		{
			return this.CountPendingHeavyPayloads(true);
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06000248 RID: 584 RVA: 0x0000D27B File Offset: 0x0000B47B
	public int ActiveHeavyPayloadAssetLoadCount
	{
		get
		{
			return HeavyZoneStreamingProxy.ActivePayloadAssetLoadCount;
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06000249 RID: 585 RVA: 0x0000D282 File Offset: 0x0000B482
	public float HeavyPoiPrefetchEstimatedSpeed
	{
		get
		{
			return this._heavyPoiPrefetchEstimatedSpeed;
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x0600024A RID: 586 RVA: 0x0000D28A File Offset: 0x0000B48A
	public int HeavyPoiPrefetchLeadCells
	{
		get
		{
			return this._heavyPoiPrefetchLeadCells;
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x0600024B RID: 587 RVA: 0x0000D292 File Offset: 0x0000B492
	// (set) Token: 0x0600024C RID: 588 RVA: 0x0000D29A File Offset: 0x0000B49A
	public int HeavyPoiPrefetchSweepCount { get; private set; }

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x0600024D RID: 589 RVA: 0x0000D2A3 File Offset: 0x0000B4A3
	// (set) Token: 0x0600024E RID: 590 RVA: 0x0000D2AB File Offset: 0x0000B4AB
	public int HeavyPoiPrefetchPreviewCellCount { get; private set; }

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x0600024F RID: 591 RVA: 0x0000D2B4 File Offset: 0x0000B4B4
	// (set) Token: 0x06000250 RID: 592 RVA: 0x0000D2BC File Offset: 0x0000B4BC
	public int HeavyPoiPrefetchAssetLoadStartCount { get; private set; }

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x06000251 RID: 593 RVA: 0x0000D2C8 File Offset: 0x0000B4C8
	public bool HasChunkUnderPlayer
	{
		get
		{
			if (PlayerProximityProvider.Instance == null)
			{
				return false;
			}
			Vector2Int currentCell = PlayerProximityProvider.Instance.CurrentCell;
			Vector2Int vector2Int;
			ChunkInstance chunkInstance;
			return this._cellToChunkMap.TryGetValue(currentCell, out vector2Int) && this._spawnedChunks.TryGetValue(vector2Int, out chunkInstance) && !(((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null) && this.IsChunkCurrentlyVisible(vector2Int, chunkInstance) && (this.colliderActiveRadius <= 0 || (chunkInstance.collidersCurrentlyEnabled && !chunkInstance.collidersCacheDirty));
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06000252 RID: 594 RVA: 0x0000D34D File Offset: 0x0000B54D
	public int CurrentSpawnCoverageHoleCount
	{
		get
		{
			if (!(PlayerProximityProvider.Instance != null))
			{
				return -1;
			}
			return this.CountUnprocessedCellsInRadius(PlayerProximityProvider.Instance.CurrentCell, Mathf.Max(0, this.spawnRadius));
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x06000253 RID: 595 RVA: 0x0000D37A File Offset: 0x0000B57A
	public int CurrentOperationalCoreHoleCount
	{
		get
		{
			if (!(PlayerProximityProvider.Instance != null))
			{
				return -1;
			}
			return this.CountOperationalCoreHoles(PlayerProximityProvider.Instance.CurrentCell, Mathf.Max(0, this.minVisibleRadius));
		}
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000D3A8 File Offset: 0x0000B5A8
	private int CountOperationalCoreHoles(Vector2Int center, int radius)
	{
		int num = 0;
		for (int i = -radius; i <= radius; i++)
		{
			for (int j = -radius; j <= radius; j++)
			{
				Vector2Int vector2Int = center + new Vector2Int(i, j);
				Vector2Int vector2Int2;
				ChunkInstance chunkInstance;
				if (!this._cellToChunkMap.TryGetValue(vector2Int, out vector2Int2) || !this._spawnedChunks.TryGetValue(vector2Int2, out chunkInstance) || ((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null || !this.IsChunkCurrentlyVisible(vector2Int2, chunkInstance) || this._pendingZoneCountByChunk.ContainsKey(chunkInstance) || this._pendingHeavyZoneCountByChunk.ContainsKey(chunkInstance))
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0000D44C File Offset: 0x0000B64C
	private int CountPresentationVisibleChunks()
	{
		int num = 0;
		foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._spawnedChunks)
		{
			ChunkInstance value = keyValuePair.Value;
			if (((value != null) ? value.chunkRoot : null) != null && this.IsChunkCurrentlyVisible(keyValuePair.Key, keyValuePair.Value))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0000D4D0 File Offset: 0x0000B6D0
	private int CountResidentHeavyPayloads()
	{
		int num = 0;
		foreach (KeyValuePair<GameObject, GameObject> keyValuePair in this._zoneInstanceToPrefabMap)
		{
			GameObject key = keyValuePair.Key;
			HeavyZoneStreamingProxy heavyZoneStreamingProxy = ((key != null) ? key.GetComponent<HeavyZoneStreamingProxy>() : null);
			if (heavyZoneStreamingProxy != null && heavyZoneStreamingProxy.PayloadInstance != null)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000D558 File Offset: 0x0000B758
	private int CountPresentationVisibleHeavyPayloads()
	{
		int num = 0;
		foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._spawnedChunks)
		{
			ChunkInstance value = keyValuePair.Value;
			if (!(((value != null) ? value.chunkRoot : null) == null) && this.IsChunkCurrentlyVisible(keyValuePair.Key, value) && value.spawnedZones != null)
			{
				for (int i = 0; i < value.spawnedZones.Count; i++)
				{
					GameObject gameObject = value.spawnedZones[i];
					HeavyZoneStreamingProxy heavyZoneStreamingProxy = ((gameObject != null) ? gameObject.GetComponent<HeavyZoneStreamingProxy>() : null);
					if (heavyZoneStreamingProxy != null && heavyZoneStreamingProxy.IsOperationalReady && heavyZoneStreamingProxy.PayloadInstance != null && heavyZoneStreamingProxy.gameObject.activeInHierarchy)
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0000D658 File Offset: 0x0000B858
	private int CountPendingHeavyPayloads(bool wakePending)
	{
		int num = 0;
		foreach (HeavyZoneStreamingProxy heavyZoneStreamingProxy in this._heavyZoneOwners.Keys)
		{
			if (heavyZoneStreamingProxy != null && (wakePending ? heavyZoneStreamingProxy.IsWakePending : heavyZoneStreamingProxy.IsLoadPending))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x06000259 RID: 601 RVA: 0x0000D6CC File Offset: 0x0000B8CC
	private bool enableRuntimeChunkDespawn
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.enableRuntimeChunkDespawn;
		}
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x0600025A RID: 602 RVA: 0x0000D6DF File Offset: 0x0000B8DF
	private int runtimeDespawnRadius
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 10;
			}
			return chunkManagerConfig.runtimeDespawnRadius;
		}
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x0600025B RID: 603 RVA: 0x0000D6F3 File Offset: 0x0000B8F3
	private int maxChunkDespawnsPerFrame
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 1;
			}
			return chunkManagerConfig.maxChunkDespawnsPerFrame;
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x0600025C RID: 604 RVA: 0x0000D706 File Offset: 0x0000B906
	private bool useSequentialChunkLifecycleQueue
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.useSequentialChunkLifecycleQueue;
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x0600025D RID: 605 RVA: 0x0000D719 File Offset: 0x0000B919
	private int maxChunkLifecycleOpsPerTick
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 1;
			}
			return chunkManagerConfig.maxChunkLifecycleOpsPerTick;
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x0600025E RID: 606 RVA: 0x0000D72C File Offset: 0x0000B92C
	private bool enableDeferredZoneSpawn
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.enableDeferredZoneSpawn;
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x0600025F RID: 607 RVA: 0x0000D73F File Offset: 0x0000B93F
	private int maxZoneSpawnsPerFrame
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 3;
			}
			return chunkManagerConfig.maxZoneSpawnsPerFrame;
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x06000260 RID: 608 RVA: 0x0000D752 File Offset: 0x0000B952
	private float zoneSpawnFrameBudgetMs
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 1f;
			}
			return chunkManagerConfig.zoneSpawnFrameBudgetMs;
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06000261 RID: 609 RVA: 0x0000D769 File Offset: 0x0000B969
	private bool enableDeferredPostSpawnInit
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.enableDeferredPostSpawnInit;
		}
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x06000262 RID: 610 RVA: 0x0000D77C File Offset: 0x0000B97C
	private int maxPostSpawnInitOpsPerFrame
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 2;
			}
			return chunkManagerConfig.maxPostSpawnInitOpsPerFrame;
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x06000263 RID: 611 RVA: 0x0000D78F File Offset: 0x0000B98F
	private float postSpawnInitFrameBudgetMs
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 0.75f;
			}
			return chunkManagerConfig.postSpawnInitFrameBudgetMs;
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x06000264 RID: 612 RVA: 0x0000D7A6 File Offset: 0x0000B9A6
	private bool preCacheVisualsAfterSpawn
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.preCacheVisualsAfterSpawn;
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000265 RID: 613 RVA: 0x0000D7B9 File Offset: 0x0000B9B9
	private bool enablePersistentPrewarmTargetCache
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.enablePersistentPrewarmTargetCache;
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06000266 RID: 614 RVA: 0x0000D7CC File Offset: 0x0000B9CC
	private bool forceRebuildPrewarmTargetCache
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig != null && chunkManagerConfig.forceRebuildPrewarmTargetCache;
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06000267 RID: 615 RVA: 0x0000D7DF File Offset: 0x0000B9DF
	private int prewarmTargetCacheSchemaVersion
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 6;
			}
			return chunkManagerConfig.prewarmTargetCacheSchemaVersion;
		}
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x06000268 RID: 616 RVA: 0x0000D7F2 File Offset: 0x0000B9F2
	private bool IsLoadingBoostActive
	{
		get
		{
			if (!this._fullGameplayReady)
			{
				ChunkManagerConfig chunkManagerConfig = this.config;
				return chunkManagerConfig == null || chunkManagerConfig.useLoadingBudgets;
			}
			return false;
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06000269 RID: 617 RVA: 0x0000D80F File Offset: 0x0000BA0F
	private int effPrewarmInstancesPerFrame
	{
		get
		{
			if (!this.IsLoadingBoostActive)
			{
				return this.prewarmInstancesPerFrame;
			}
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 8;
			}
			return chunkManagerConfig.loadingPrewarmInstancesPerFrame;
		}
	}

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x0600026A RID: 618 RVA: 0x0000D831 File Offset: 0x0000BA31
	private float effPrewarmFrameBudgetMs
	{
		get
		{
			if (!this.IsLoadingBoostActive)
			{
				return this.prewarmFrameBudgetMs;
			}
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 12f;
			}
			return chunkManagerConfig.loadingPrewarmFrameBudgetMs;
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x0600026B RID: 619 RVA: 0x0000D857 File Offset: 0x0000BA57
	private int effMaxZoneSpawnsPerFrame
	{
		get
		{
			if (!this.IsLoadingBoostActive)
			{
				return this.maxZoneSpawnsPerFrame;
			}
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 12;
			}
			return chunkManagerConfig.loadingMaxZoneSpawnsPerFrame;
		}
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x0600026C RID: 620 RVA: 0x0000D87A File Offset: 0x0000BA7A
	private float effZoneSpawnFrameBudgetMs
	{
		get
		{
			if (!this.IsLoadingBoostActive)
			{
				return this.zoneSpawnFrameBudgetMs;
			}
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 10f;
			}
			return chunkManagerConfig.loadingZoneSpawnFrameBudgetMs;
		}
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x0600026D RID: 621 RVA: 0x0000D8A0 File Offset: 0x0000BAA0
	private int effMaxPostSpawnInitOpsPerFrame
	{
		get
		{
			if (!this.IsLoadingBoostActive)
			{
				return this.maxPostSpawnInitOpsPerFrame;
			}
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 8;
			}
			return chunkManagerConfig.loadingMaxPostSpawnInitOpsPerFrame;
		}
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x0600026E RID: 622 RVA: 0x0000D8C2 File Offset: 0x0000BAC2
	private float effPostSpawnInitFrameBudgetMs
	{
		get
		{
			if (!this.IsLoadingBoostActive)
			{
				return this.postSpawnInitFrameBudgetMs;
			}
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 6f;
			}
			return chunkManagerConfig.loadingPostSpawnInitFrameBudgetMs;
		}
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x0600026F RID: 623 RVA: 0x0000D8E8 File Offset: 0x0000BAE8
	private int effMaxChunkLifecycleOpsPerTick
	{
		get
		{
			if (!this.IsLoadingBoostActive)
			{
				return this.maxChunkLifecycleOpsPerTick;
			}
			ChunkManagerConfig chunkManagerConfig = this.config;
			if (chunkManagerConfig == null)
			{
				return 4;
			}
			return chunkManagerConfig.loadingMaxChunkLifecycleOpsPerTick;
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x06000270 RID: 624 RVA: 0x0000D90A File Offset: 0x0000BB0A
	private bool useJobs
	{
		get
		{
			ChunkManagerConfig chunkManagerConfig = this.config;
			return chunkManagerConfig == null || chunkManagerConfig.useJobs;
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x06000271 RID: 625 RVA: 0x0000D91D File Offset: 0x0000BB1D
	private bool useSetActiveForVisibility
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x06000272 RID: 626 RVA: 0x0000D920 File Offset: 0x0000BB20
	private PoiResidencyPrefabResolver ZonePrefabResolver
	{
		get
		{
			PoiResidencyPrefabResolver poiResidencyPrefabResolver;
			if ((poiResidencyPrefabResolver = this._poiPrefabResolver) == null)
			{
				poiResidencyPrefabResolver = (this._poiPrefabResolver = new PoiResidencyPrefabResolver(MapResidencyPilotSelection.Mode, new Func<string, GameObject>(Resources.Load<GameObject>)));
			}
			return poiResidencyPrefabResolver;
		}
	}

	// Token: 0x06000273 RID: 627 RVA: 0x0000D956 File Offset: 0x0000BB56
	private GameObject ResolveZonePrefab(ZoneData zone)
	{
		return this.ZonePrefabResolver.Resolve((zone != null) ? zone.zonePrefab : null);
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0000D96F File Offset: 0x0000BB6F
	private void ClearZonePrefabResolver()
	{
		PoiResidencyPrefabResolver poiPrefabResolver = this._poiPrefabResolver;
		if (poiPrefabResolver != null)
		{
			poiPrefabResolver.Clear();
		}
		this._poiPrefabResolver = null;
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x06000275 RID: 629 RVA: 0x0000D989 File Offset: 0x0000BB89
	public bool CanShiftWorldOrigin
	{
		get
		{
			return this._prewarmPoolCoroutine == null;
		}
	}

	// Token: 0x06000276 RID: 630 RVA: 0x0000D994 File Offset: 0x0000BB94
	public static bool IsWorldOriginGroupingTransform(Transform candidate)
	{
		return !(candidate == null) && (candidate.GetComponent<ChunkManager>() != null || (ChunkManager.Instance != null && candidate == ChunkManager.Instance._runtimeWorldRoot));
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x06000277 RID: 631 RVA: 0x0000D9D0 File Offset: 0x0000BBD0
	// (set) Token: 0x06000278 RID: 632 RVA: 0x0000D9D8 File Offset: 0x0000BBD8
	public bool WarmRenderStarted { get; private set; }

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x06000279 RID: 633 RVA: 0x0000D9E1 File Offset: 0x0000BBE1
	public int ActiveWarmRenderResourceCount
	{
		get
		{
			return this._warmRenderResources.Count;
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x0600027A RID: 634 RVA: 0x0000D9EE File Offset: 0x0000BBEE
	public int PinnedMeshColliderBakeMeshCount
	{
		get
		{
			return this._meshColliderBakeMeshes.Count;
		}
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x0600027B RID: 635 RVA: 0x0000D9FB File Offset: 0x0000BBFB
	public int PendingMeshColliderBakeVariantCount
	{
		get
		{
			return this._meshColliderBakePending.Count;
		}
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x0600027C RID: 636 RVA: 0x0000DA08 File Offset: 0x0000BC08
	public int ActiveMeshColliderBakeVariantCount
	{
		get
		{
			if (!this._meshColliderBakeJobScheduled)
			{
				return 0;
			}
			return this._meshColliderBakeWave.Count;
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x0600027D RID: 637 RVA: 0x0000DA1F File Offset: 0x0000BC1F
	// (set) Token: 0x0600027E RID: 638 RVA: 0x0000DA27 File Offset: 0x0000BC27
	public int CompletedMeshColliderBakeVariantCount { get; private set; }

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x0600027F RID: 639 RVA: 0x0000DA30 File Offset: 0x0000BC30
	// (set) Token: 0x06000280 RID: 640 RVA: 0x0000DA38 File Offset: 0x0000BC38
	public int MeshColliderBakeWaveCount { get; private set; }

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06000281 RID: 641 RVA: 0x0000DA41 File Offset: 0x0000BC41
	// (set) Token: 0x06000282 RID: 642 RVA: 0x0000DA49 File Offset: 0x0000BC49
	public int PeakMeshColliderBakeWaveVariants { get; private set; }

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06000283 RID: 643 RVA: 0x0000DA52 File Offset: 0x0000BC52
	// (set) Token: 0x06000284 RID: 644 RVA: 0x0000DA5A File Offset: 0x0000BC5A
	public long MeshColliderBakeVertexCount { get; private set; }

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000285 RID: 645 RVA: 0x0000DA63 File Offset: 0x0000BC63
	// (set) Token: 0x06000286 RID: 646 RVA: 0x0000DA6B File Offset: 0x0000BC6B
	public long MeshColliderBakeIndexCount { get; private set; }

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06000287 RID: 647 RVA: 0x0000DA74 File Offset: 0x0000BC74
	// (set) Token: 0x06000288 RID: 648 RVA: 0x0000DA7C File Offset: 0x0000BC7C
	public double MeshColliderBakeJobSeconds { get; private set; }

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x06000289 RID: 649 RVA: 0x0000DA85 File Offset: 0x0000BC85
	// (set) Token: 0x0600028A RID: 650 RVA: 0x0000DA8D File Offset: 0x0000BC8D
	public double LongestMeshColliderBakeWaveSeconds { get; private set; }

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600028B RID: 651 RVA: 0x0000DA98 File Offset: 0x0000BC98
	// (remove) Token: 0x0600028C RID: 652 RVA: 0x0000DAD0 File Offset: 0x0000BCD0
	public event ChunkManager.ChunkEvent OnChunkSpawned;

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x0600028D RID: 653 RVA: 0x0000DB08 File Offset: 0x0000BD08
	// (remove) Token: 0x0600028E RID: 654 RVA: 0x0000DB40 File Offset: 0x0000BD40
	public event ChunkManager.ChunkEvent OnChunkRemoved;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x0600028F RID: 655 RVA: 0x0000DB78 File Offset: 0x0000BD78
	// (remove) Token: 0x06000290 RID: 656 RVA: 0x0000DBB0 File Offset: 0x0000BDB0
	public event ChunkManager.ChunkEvent OnChunkActivated;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06000291 RID: 657 RVA: 0x0000DBE8 File Offset: 0x0000BDE8
	// (remove) Token: 0x06000292 RID: 658 RVA: 0x0000DC20 File Offset: 0x0000BE20
	public event ChunkManager.ChunkEvent OnChunkDeactivated;

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06000293 RID: 659 RVA: 0x0000DC58 File Offset: 0x0000BE58
	// (remove) Token: 0x06000294 RID: 660 RVA: 0x0000DC90 File Offset: 0x0000BE90
	public event ChunkManager.ChunkEvent OnChunkZonesReady;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000295 RID: 661 RVA: 0x0000DCC8 File Offset: 0x0000BEC8
	// (remove) Token: 0x06000296 RID: 662 RVA: 0x0000DD00 File Offset: 0x0000BF00
	public event Action<GameObject> OnContentRootReady;

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06000297 RID: 663 RVA: 0x0000DD38 File Offset: 0x0000BF38
	// (remove) Token: 0x06000298 RID: 664 RVA: 0x0000DD70 File Offset: 0x0000BF70
	public event Action OnInitialGenerationComplete;

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000299 RID: 665 RVA: 0x0000DDA5 File Offset: 0x0000BFA5
	public bool IsInitialGenerationComplete
	{
		get
		{
			return this._initialGenerationComplete;
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x0600029A RID: 666 RVA: 0x0000DDAD File Offset: 0x0000BFAD
	public bool IsMapReady
	{
		get
		{
			return this._fullGameplayReady;
		}
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x0600029B RID: 667 RVA: 0x0000DDB5 File Offset: 0x0000BFB5
	public int InitializationWorkVersion
	{
		get
		{
			return this._initializationWorkVersion;
		}
	}

	// Token: 0x0600029C RID: 668 RVA: 0x0000DDBD File Offset: 0x0000BFBD
	private void RecordCompletedInitializationWork()
	{
		if (!this._fullGameplayReady && !this.IsTearingDown && this._initializationWorkVersion < 2147483647)
		{
			this._initializationWorkVersion++;
		}
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x0600029D RID: 669 RVA: 0x0000DDEA File Offset: 0x0000BFEA
	// (set) Token: 0x0600029E RID: 670 RVA: 0x0000DDF2 File Offset: 0x0000BFF2
	public bool IsPoiTravelSuspended { get; private set; }

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x0600029F RID: 671 RVA: 0x0000DDFC File Offset: 0x0000BFFC
	public float InitialLoadingProgress
	{
		get
		{
			if (this._fullGameplayReady)
			{
				return 1f;
			}
			if (PlayerProximityProvider.Instance == null)
			{
				return 0f;
			}
			Vector2Int currentCell = PlayerProximityProvider.Instance.CurrentCell;
			float num = this.CalculateAreaReadinessProgress(currentCell, Mathf.Clamp(this.maxVisibleRadius, 1, this.spawnRadius));
			float num2 = this.CalculateAreaReadinessProgress(currentCell, this.spawnRadius);
			return Mathf.Clamp01(num * 0.45f + num2 * 0.25f + this._poolPrewarmProgress * 0.15f + this._warmRenderProgress * 0.15f);
		}
	}

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x060002A0 RID: 672 RVA: 0x0000DE89 File Offset: 0x0000C089
	// (remove) Token: 0x060002A1 RID: 673 RVA: 0x0000DEB3 File Offset: 0x0000C0B3
	public event Action OnMapReady
	{
		add
		{
			this._onMapReady = (Action)Delegate.Combine(this._onMapReady, value);
			if (this._fullGameplayReady && value != null)
			{
				value();
			}
		}
		remove
		{
			this._onMapReady = (Action)Delegate.Remove(this._onMapReady, value);
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000DECC File Offset: 0x0000C0CC
	// (set) Token: 0x060002A3 RID: 675 RVA: 0x0000DED4 File Offset: 0x0000C0D4
	public bool IsTearingDown { get; private set; }

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000DEDD File Offset: 0x0000C0DD
	public float CellSize
	{
		get
		{
			return this.cellSize;
		}
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0000DEE8 File Offset: 0x0000C0E8
	public bool TryReserveLongWayHomeStreetCells(Object owner, IReadOnlyList<Vector2Int> cells, Func<GameObject, int, bool> shellIsWalkable, out string error)
	{
		error = null;
		if (!RunModeService.IsLongWayHome || owner == null || cells == null || shellIsWalkable == null || this._spawnResolver == null || (this._longWayHomeStreetOwner != null && this._longWayHomeStreetOwner != owner))
		{
			error = "The Long Way Home street reservation is unavailable.";
			return false;
		}
		Dictionary<Vector2Int, ChunkSpawnResolver.SpawnEntry> dictionary = new Dictionary<Vector2Int, ChunkSpawnResolver.SpawnEntry>();
		foreach (Vector2Int vector2Int in cells)
		{
			if (!dictionary.ContainsKey(vector2Int))
			{
				Vector2Int vector2Int2;
				ChunkInstance chunkInstance;
				Vector2Int vector2Int3;
				ChunkSpawnResolver.SpawnEntry spawnEntry;
				if (this._cellToChunkMap.TryGetValue(vector2Int, out vector2Int2) && this._spawnedChunks.TryGetValue(vector2Int2, out chunkInstance) && ((chunkInstance != null) ? chunkInstance.chunkRoot : null) != null && chunkInstance.chunkData == null)
				{
					int num = Mathf.RoundToInt(chunkInstance.chunkRoot.transform.eulerAngles.y / 90f);
					if (!shellIsWalkable(chunkInstance.chunkRoot, num))
					{
						error = string.Format("The authored home street shell at {0} is not walkable.", vector2Int);
						return false;
					}
				}
				else if (this._committedSpawnCellOwners.TryGetValue(vector2Int, out vector2Int3) && this._committedSpawnPlan.TryGetValue(vector2Int3, out spawnEntry))
				{
					if (vector2Int3 != vector2Int || !ChunkManager.IsCertifiedLongWayHomeStreetEntry(spawnEntry, shellIsWalkable))
					{
						error = string.Format("An existing district crosses the requested street at {0}.", vector2Int);
						return false;
					}
				}
				else
				{
					ChunkSpawnResolver.SpawnEntry spawnEntry2 = this._spawnResolver.ResolvePreview(vector2Int);
					if (ChunkSpawnResolver.IsSubwayChunk(spawnEntry2.chunkData))
					{
						spawnEntry2 = this._spawnResolver.ResolveWithoutSubway(vector2Int);
					}
					if (!ChunkManager.IsCertifiedLongWayHomeStreetEntry(spawnEntry2, shellIsWalkable))
					{
						spawnEntry2 = this._spawnResolver.ResolveSmallFallback(vector2Int, spawnEntry2.chunkData, true);
					}
					if (!ChunkManager.IsCertifiedLongWayHomeStreetEntry(spawnEntry2, shellIsWalkable))
					{
						spawnEntry2 = default(ChunkSpawnResolver.SpawnEntry);
						foreach (ChunkData chunkData in this.availableChunks)
						{
							if (!(chunkData == null) && !(chunkData.gridSize != Vector2Int.one) && !ChunkSpawnResolver.IsSubwayChunk(chunkData) && RankPoiPolicy.IsChunkAllowed(chunkData))
							{
								ChunkSpawnResolver.SpawnEntry spawnEntry3 = this._spawnResolver.ResolveWithoutSpecialZones(vector2Int, chunkData);
								if (ChunkManager.IsCertifiedLongWayHomeStreetEntry(spawnEntry3, shellIsWalkable))
								{
									spawnEntry2 = spawnEntry3;
									break;
								}
							}
						}
					}
					if (!spawnEntry2.IsValid)
					{
						error = string.Format("No walkable authored road shell is available at {0}.", vector2Int);
						return false;
					}
					dictionary.Add(vector2Int, spawnEntry2);
				}
			}
		}
		this._longWayHomeStreetOwner = owner;
		foreach (KeyValuePair<Vector2Int, ChunkSpawnResolver.SpawnEntry> keyValuePair in dictionary)
		{
			this.CommitSpawnPlanEntry(keyValuePair.Key, keyValuePair.Value, keyValuePair.Value.chunkData);
			this._unvisitedLongWayHomeStreetOrigins.Add(keyValuePair.Key);
		}
		return true;
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x0000E200 File Offset: 0x0000C400
	private static bool IsCertifiedLongWayHomeStreetEntry(ChunkSpawnResolver.SpawnEntry entry, Func<GameObject, int, bool> shellIsWalkable)
	{
		return entry.IsValid && entry.chunkData.gridSize == Vector2Int.one && entry.chunkData.template != null && shellIsWalkable(entry.chunkData.template.chunkPrefab, entry.rotation);
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x0000E260 File Offset: 0x0000C460
	public void ReleaseLongWayHomeStreetReservations(Object owner)
	{
		if (this._longWayHomeStreetOwner != owner)
		{
			return;
		}
		foreach (Vector2Int vector2Int in this._unvisitedLongWayHomeStreetOrigins)
		{
			ChunkSpawnResolver.SpawnEntry spawnEntry;
			if (this._committedSpawnPlan.TryGetValue(vector2Int, out spawnEntry))
			{
				this.ForgetRankRestrictedSpawnPlan(vector2Int, spawnEntry);
			}
		}
		this._unvisitedLongWayHomeStreetOrigins.Clear();
		this._longWayHomeStreetOwner = null;
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x060002A8 RID: 680 RVA: 0x0000E2E8 File Offset: 0x0000C4E8
	public bool IsPreparationFocusSelectionPending
	{
		get
		{
			return this._preparationFocusSelectionPending && this._preparationFocusOwner != null;
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000E300 File Offset: 0x0000C500
	private bool HasPreparationFocus
	{
		get
		{
			return this._preparationFocusOwner != null && !this._preparationFocusSelectionPending;
		}
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x060002AA RID: 682 RVA: 0x0000E31B File Offset: 0x0000C51B
	private Vector2Int StreamingPlayerCell
	{
		get
		{
			if (this.HasPreparationFocus)
			{
				return this.WorldToCell(this._preparationFocusPosition);
			}
			if (!(PlayerProximityProvider.Instance != null))
			{
				return Vector2Int.zero;
			}
			return PlayerProximityProvider.Instance.CurrentCell;
		}
	}

	// Token: 0x060002AB RID: 683 RVA: 0x0000E34F File Offset: 0x0000C54F
	public bool BeginPreparationFocusSelection(Object owner)
	{
		if (owner == null || (this._preparationFocusOwner != null && this._preparationFocusOwner != owner))
		{
			return false;
		}
		this._preparationFocusOwner = owner;
		this._preparationFocusSelectionPending = true;
		return true;
	}

	// Token: 0x060002AC RID: 684 RVA: 0x0000E387 File Offset: 0x0000C587
	public bool TryGetPreparationFocus(out Vector3 position)
	{
		position = this._preparationFocusPosition;
		return this.HasPreparationFocus;
	}

	// Token: 0x060002AD RID: 685 RVA: 0x0000E39B File Offset: 0x0000C59B
	public bool SetPreparationFocus(Object owner, Vector3 position)
	{
		if (owner == null || (this._preparationFocusOwner != null && this._preparationFocusOwner != owner))
		{
			return false;
		}
		this._preparationFocusOwner = owner;
		this._preparationFocusPosition = position;
		this._preparationFocusSelectionPending = false;
		return true;
	}

	// Token: 0x060002AE RID: 686 RVA: 0x0000E3DC File Offset: 0x0000C5DC
	public bool TryWakeRecoveryArea(Object owner, Vector3 position)
	{
		if (owner == null || this._preparationFocusOwner != owner || !this.HasPreparationFocus || !base.isActiveAndEnabled || !this._mapInitialized || this.IsPoiTravelSuspended || this.IsTearingDown)
		{
			return false;
		}
		Vector2Int vector2Int = this.WorldToCell(position);
		if (vector2Int != this.WorldToCell(this._preparationFocusPosition))
		{
			return false;
		}
		this.EnsureNearChunkVisibility(vector2Int);
		this.EnsureNearColliders(vector2Int);
		Physics.SyncTransforms();
		return this.IsPreparationAreaReady(position, 0);
	}

	// Token: 0x060002AF RID: 687 RVA: 0x0000E464 File Offset: 0x0000C664
	public void ReleasePreparationFocus(Object owner)
	{
		if (this._preparationFocusOwner != owner)
		{
			return;
		}
		this.ReleaseDebugPoiReservation(owner);
		if (this.HasPreparationFocus)
		{
			this._poiTravelDroughtScheduler.ObservePosition(new Vector2(this._preparationFocusPosition.x, this._preparationFocusPosition.z), 0f);
		}
		this._preparationFocusOwner = null;
		this._preparationFocusSelectionPending = false;
		this._hasLastStreamingPlayerCell = false;
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x0000E4D0 File Offset: 0x0000C6D0
	public bool TryGetPreparationPoiPreview(Object owner, Vector2Int cell, ProceduralPoiKind kind, out ChunkSpawnResolver.SpawnEntry entry)
	{
		entry = default(ChunkSpawnResolver.SpawnEntry);
		if (owner == null || this._preparationFocusOwner != owner || this._spawnResolver == null || kind == ProceduralPoiKind.None || !RankPoiPolicy.IsPoiAllowed(kind) || this.IsPoiTravelSuspended || this.IsTearingDown)
		{
			return false;
		}
		Vector2Int vector2Int;
		if (this._committedSpawnCellOwners.TryGetValue(cell, out vector2Int) && vector2Int != cell)
		{
			return false;
		}
		if (this._committedSpawnPlan.TryGetValue(cell, out entry))
		{
			return ChunkManager.GetSelectedPoiKind(entry, entry.chunkData) == kind && RankPoiPolicy.IsSpawnAllowed(entry);
		}
		entry = this._spawnResolver.ResolvePreview(cell);
		if (!entry.IsValid || ChunkManager.GetSelectedPoiKind(entry, entry.chunkData) != kind || !RankPoiPolicy.IsSpawnAllowed(entry) || !this.AreCellsFree(cell, entry.chunkData.gridSize) || !this.AreCommittedSpawnPlanCellsFree(cell, entry.chunkData.gridSize) || this.HasCommittedSpecialSpacingConflict(cell, entry.chunkData.gridSize) || (ChunkSpawnResolver.IsSubwayChunk(entry.chunkData) && this.HasAdjacentSubwayChunk(cell)))
		{
			return false;
		}
		for (int i = 0; i < entry.chunkData.gridSize.x; i++)
		{
			for (int j = 0; j < entry.chunkData.gridSize.y; j++)
			{
				if (this._cellToChunkMap.ContainsKey(cell + new Vector2Int(i, j)) || this._committedSpawnCellOwners.ContainsKey(cell + new Vector2Int(i, j)))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x0000E678 File Offset: 0x0000C878
	public bool TryReserveLongWayHomeMallExit(LongWayHomeWorldRoute owner, Vector2Int cell, out ChunkSpawnResolver.SpawnEntry entry)
	{
		return this.TryReserveLongWayHomePoiExit(owner, cell, ProceduralPoiKind.Mall, out entry);
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x0000E684 File Offset: 0x0000C884
	public bool TryReserveLongWayHomePoiExit(LongWayHomeWorldRoute owner, Vector2Int cell, ProceduralPoiKind kind, out ChunkSpawnResolver.SpawnEntry entry)
	{
		entry = default(ChunkSpawnResolver.SpawnEntry);
		return kind != ProceduralPoiKind.None && RunModeService.IsLongWayHomePoiPrologue && LongWayHomeRuntimeController.ProloguePoiKind(RunModeService.CurrentLongWayHomePrologue) == kind && this.TryReservePreparationPoiCore(owner, cell, kind, out entry);
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x0000E6B4 File Offset: 0x0000C8B4
	private bool TryReservePreparationPoiCore(Object owner, Vector2Int cell, ProceduralPoiKind kind, out ChunkSpawnResolver.SpawnEntry entry)
	{
		if (!this.TryGetPreparationPoiPreview(owner, cell, kind, out entry))
		{
			return false;
		}
		if (this._debugPoiReservationOwner == owner && this._debugPoiReservationCell == cell)
		{
			return true;
		}
		this.ReleaseDebugPoiReservation(owner);
		if (this._committedSpawnPlan.ContainsKey(cell))
		{
			return true;
		}
		this._debugPoiReservationOwner = owner;
		this._debugPoiReservationCell = cell;
		this._debugPoiReservationMaterialized = false;
		this.CommitSpawnPlanEntry(cell, entry, entry.chunkData);
		return true;
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x0000E730 File Offset: 0x0000C930
	private void ReleaseDebugPoiReservation(Object owner)
	{
		if (owner == null || this._debugPoiReservationOwner != owner)
		{
			return;
		}
		ChunkSpawnResolver.SpawnEntry spawnEntry;
		if (!this._debugPoiReservationMaterialized && this._committedSpawnPlan.TryGetValue(this._debugPoiReservationCell, out spawnEntry))
		{
			this.ForgetRankRestrictedSpawnPlan(this._debugPoiReservationCell, spawnEntry);
		}
		this._debugPoiReservationOwner = null;
		this._debugPoiReservationCell = default(Vector2Int);
		this._debugPoiReservationMaterialized = false;
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x0000E790 File Offset: 0x0000C990
	public bool IsPreparationAreaReady(Vector3 position, int radius = 0)
	{
		if (!this._mapInitialized || this.IsPoiTravelSuspended)
		{
			return false;
		}
		Vector2Int vector2Int = this.WorldToCell(position);
		radius = Mathf.Max(0, radius);
		for (int i = -radius; i <= radius; i++)
		{
			for (int j = -radius; j <= radius; j++)
			{
				Vector2Int vector2Int2;
				ChunkInstance chunkInstance;
				if (!this._cellToChunkMap.TryGetValue(vector2Int + new Vector2Int(i, j), out vector2Int2) || !this._spawnedChunks.TryGetValue(vector2Int2, out chunkInstance) || ((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null || !chunkInstance.chunkRoot.activeInHierarchy || !this.IsChunkCurrentlyVisible(vector2Int2, chunkInstance))
				{
					return false;
				}
				Collider[] array2;
				Collider[] array = (this._chunkBaseColliderCache.TryGetValue(chunkInstance.chunkRoot, out array2) ? array2 : chunkInstance.cachedColliders);
				bool flag = false;
				if (array != null)
				{
					for (int k = 0; k < array.Length; k++)
					{
						if (array[k] != null && array[k].enabled && !array[k].isTrigger && array[k].gameObject.activeInHierarchy)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000E8C2 File Offset: 0x0000CAC2
	public IReadOnlyDictionary<Vector2Int, ChunkInstance> SpawnedChunks
	{
		get
		{
			return this._spawnedChunks;
		}
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x0000E8CC File Offset: 0x0000CACC
	public void CopyCommittedSpawnPlan(List<KeyValuePair<Vector2Int, ChunkSpawnResolver.SpawnEntry>> destination)
	{
		if (destination == null)
		{
			throw new ArgumentNullException("destination");
		}
		destination.Clear();
		foreach (KeyValuePair<Vector2Int, ChunkSpawnResolver.SpawnEntry> keyValuePair in this._committedSpawnPlan)
		{
			ChunkSpawnResolver.SpawnEntry value = keyValuePair.Value;
			destination.Add(new KeyValuePair<Vector2Int, ChunkSpawnResolver.SpawnEntry>(keyValuePair.Key, new ChunkSpawnResolver.SpawnEntry(value.chunkData, value.rotation, (value.zoneIndices != null) ? ((int[])value.zoneIndices.Clone()) : null, value.wasPresented)));
		}
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x0000E978 File Offset: 0x0000CB78
	public bool IsChunkPresentationVisible(ChunkInstance chunk)
	{
		return chunk != null && chunk.chunkRoot != null && this.IsChunkCurrentlyVisible(chunk.originCell, chunk);
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000E99A File Offset: 0x0000CB9A
	public bool HasSpecialZoneReservation
	{
		get
		{
			return this.HasAnySpecialZoneReservation();
		}
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x060002BA RID: 698 RVA: 0x0000E9A2 File Offset: 0x0000CBA2
	// (set) Token: 0x060002BB RID: 699 RVA: 0x0000E9A9 File Offset: 0x0000CBA9
	public static int? DiagnosticSessionSeedOverride { get; set; }

	// Token: 0x060002BC RID: 700 RVA: 0x0000E9B1 File Offset: 0x0000CBB1
	public bool TryGetSessionSeed(out int sessionSeed)
	{
		if (this._spawnResolver == null)
		{
			sessionSeed = 0;
			return false;
		}
		sessionSeed = this._spawnResolver.SessionSeed;
		return true;
	}

	// Token: 0x060002BD RID: 701 RVA: 0x0000E9D0 File Offset: 0x0000CBD0
	public bool TryGetCommittedMiniMapEntry(Vector2Int cell, out Vector2Int origin, out ChunkSpawnResolver.SpawnEntry entry)
	{
		origin = cell;
		entry = default(ChunkSpawnResolver.SpawnEntry);
		return this._committedSpawnCellOwners.TryGetValue(cell, out origin) && this._committedSpawnPlan.TryGetValue(origin, out entry) && entry.IsValid && RankPoiPolicy.IsSpawnAllowed(entry);
	}

	// Token: 0x060002BE RID: 702 RVA: 0x0000EA23 File Offset: 0x0000CC23
	public bool TryGetObservedMiniMapEntry(Vector2Int cell, out Vector2Int origin, out ChunkSpawnResolver.SpawnEntry entry)
	{
		return this.TryGetCommittedMiniMapEntry(cell, out origin, out entry) && entry.wasPresented;
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0000EA38 File Offset: 0x0000CC38
	public bool MarkMiniMapObserved(Vector2Int origin)
	{
		ChunkSpawnResolver.SpawnEntry spawnEntry;
		ChunkInstance chunkInstance;
		if (!this._fullGameplayReady || this.HasPreparationFocus || !this._committedSpawnPlan.TryGetValue(origin, out spawnEntry) || !spawnEntry.IsValid || spawnEntry.wasPresented || !this._spawnedChunks.TryGetValue(origin, out chunkInstance) || ((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null || !chunkInstance.chunkRoot.activeInHierarchy || !this.IsChunkCurrentlyVisible(origin, chunkInstance) || this.HasPendingZoneContent(chunkInstance))
		{
			return false;
		}
		ChunkTemplate template = spawnEntry.chunkData.template;
		int num = ((template != null) ? template.zoneSlots.Count : 0);
		if (num > 0 && (spawnEntry.zoneIndices == null || spawnEntry.zoneIndices.Length < num))
		{
			return false;
		}
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			int num3 = spawnEntry.zoneIndices[i];
			if (num3 >= 0 && num3 < spawnEntry.chunkData.availableZones.Count)
			{
				ZoneData zoneData = spawnEntry.chunkData.availableZones[num3];
				if (((zoneData != null) ? zoneData.zonePrefab : null) != null)
				{
					num2++;
				}
			}
		}
		int num4 = 0;
		for (int j = 0; j < chunkInstance.spawnedZones.Count; j++)
		{
			if (chunkInstance.spawnedZones[j] != null && chunkInstance.spawnedZones[j].activeInHierarchy)
			{
				num4++;
			}
		}
		if (num4 < num2 || (ChunkManager.SelectionContainsSpecialZone(spawnEntry, spawnEntry.chunkData) && !ChunkManager.IsSelectedSpecialZonePresentationReady(spawnEntry, chunkInstance)))
		{
			return false;
		}
		this._committedSpawnPlan[origin] = new ChunkSpawnResolver.SpawnEntry(spawnEntry.chunkData, spawnEntry.rotation, spawnEntry.zoneIndices, true);
		int miniMapLayoutRevision = this.MiniMapLayoutRevision;
		this.MiniMapLayoutRevision = miniMapLayoutRevision + 1;
		return true;
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x0000EBF3 File Offset: 0x0000CDF3
	public bool TryResolveSeededMiniMapEntry(Vector2Int cell, out Vector2Int origin, out ChunkSpawnResolver.SpawnEntry entry)
	{
		origin = cell;
		entry = default(ChunkSpawnResolver.SpawnEntry);
		return RunModeService.HasSeededFreeWorld && this._spawnResolver != null && this.TryResolvePackedSeedPreview(cell, out origin, out entry);
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x0000EC20 File Offset: 0x0000CE20
	public bool TryResolveDiagnosticPoiPreview(Vector2Int cell, ProceduralPoiKind kind, out Vector2Int origin, out ChunkSpawnResolver.SpawnEntry entry)
	{
		origin = cell;
		entry = default(ChunkSpawnResolver.SpawnEntry);
		if (!this._mapInitialized || this._spawnResolver == null || this.IsPoiTravelSuspended || this.IsTearingDown || kind == ProceduralPoiKind.None || !RankPoiPolicy.IsPoiAllowed(kind))
		{
			return false;
		}
		if (!this.TryResolvePackedSeedPreview(cell, out origin, out entry) || ChunkManager.GetSelectedPoiKind(entry, entry.chunkData) != kind)
		{
			origin = cell;
			entry = default(ChunkSpawnResolver.SpawnEntry);
			return false;
		}
		return true;
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000EC9D File Offset: 0x0000CE9D
	public int DiagnosticRuntimeDespawnRadius
	{
		get
		{
			return this.runtimeDespawnRadius;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000ECA5 File Offset: 0x0000CEA5
	public bool DiagnosticRuntimeChunkDespawnEnabled
	{
		get
		{
			return this.enableRuntimeChunkDespawn;
		}
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x0000ECB0 File Offset: 0x0000CEB0
	private bool TryResolvePackedSeedPreview(Vector2Int cell, out Vector2Int origin, out ChunkSpawnResolver.SpawnEntry entry)
	{
		origin = cell;
		entry = default(ChunkSpawnResolver.SpawnEntry);
		if (this._spawnResolver == null)
		{
			return false;
		}
		if (this.TryGetCommittedMiniMapEntry(cell, out origin, out entry))
		{
			return true;
		}
		origin = cell;
		entry = default(ChunkSpawnResolver.SpawnEntry);
		if (this._cellToChunkMap.ContainsKey(cell))
		{
			return false;
		}
		Vector2Int vector2Int;
		if (this._spawnResolver.TryResolveContainingSpecialChunkOrigin(cell, out vector2Int))
		{
			ChunkSpawnResolver.SpawnEntry spawnEntry = this._spawnResolver.ResolvePreview(vector2Int);
			if (spawnEntry.IsValid && this.AreCellsFree(vector2Int, spawnEntry.chunkData.gridSize) && this.AreCommittedSpawnPlanCellsFree(vector2Int, spawnEntry.chunkData.gridSize) && !this.HasCommittedSpecialSpacingConflict(vector2Int, spawnEntry.chunkData.gridSize))
			{
				origin = vector2Int;
				entry = spawnEntry;
			}
		}
		if (!entry.IsValid)
		{
			entry = this._spawnResolver.ResolvePreview(origin);
		}
		ChunkData chunkData = entry.chunkData;
		if (chunkData == null)
		{
			return false;
		}
		if (!RankPoiPolicy.IsSpawnAllowed(entry) || !this.AreCellsFree(origin, chunkData.gridSize) || !this.AreCommittedSpawnPlanCellsFree(origin, chunkData.gridSize))
		{
			entry = this._spawnResolver.ResolveSmallFallback(origin, chunkData, true);
			chunkData = entry.chunkData;
		}
		if (ChunkSpawnResolver.IsSubwayChunk(chunkData) && this.HasAdjacentSubwayChunk(origin))
		{
			entry = this._spawnResolver.ResolvePreviewWithoutSubway(origin);
			chunkData = entry.chunkData;
		}
		if (chunkData != null && ChunkManager.SelectionContainsSpecialZone(entry, chunkData) && this.HasCommittedSpecialSpacingConflict(origin, chunkData.gridSize))
		{
			this.RebuildEntryWithoutSpecialZones(origin, ref entry, ref chunkData);
		}
		if (chunkData == null)
		{
			return false;
		}
		if (ChunkManager.RequiresStableTerrainOrientation(chunkData) && entry.rotation != 0)
		{
			entry = new ChunkSpawnResolver.SpawnEntry(chunkData, 0, entry.zoneIndices, false);
		}
		return entry.IsValid;
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x0000EE9C File Offset: 0x0000D09C
	public bool IsMiniMapSeededPreviewEntryValid(Vector2Int origin, ChunkSpawnResolver.SpawnEntry entry)
	{
		return RunModeService.HasSeededFreeWorld && entry.IsValid && RankPoiPolicy.IsSpawnAllowed(entry) && !this._committedSpawnCellOwners.ContainsKey(origin) && this.AreCellsFree(origin, entry.chunkData.gridSize) && this.AreCommittedSpawnPlanCellsFree(origin, entry.chunkData.gridSize) && (!ChunkManager.SelectionContainsSpecialZone(entry, entry.chunkData) || !this.HasCommittedSpecialSpacingConflict(origin, entry.chunkData.gridSize)) && (!ChunkSpawnResolver.IsSubwayChunk(entry.chunkData) || !this.HasAdjacentSubwayChunk(origin));
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x0000EF38 File Offset: 0x0000D138
	public bool TryResolveMiniMapPreview(Vector2Int cell, out ChunkSpawnResolver.SpawnEntry entry)
	{
		if (this._spawnResolver == null)
		{
			entry = default(ChunkSpawnResolver.SpawnEntry);
			return false;
		}
		Vector2Int vector2Int;
		if (this._committedSpawnCellOwners.TryGetValue(cell, out vector2Int) && this._committedSpawnPlan.TryGetValue(vector2Int, out entry) && RankPoiPolicy.IsSpawnAllowed(entry))
		{
			return entry.IsValid;
		}
		entry = this._spawnResolver.ResolvePreview(cell);
		return entry.IsValid;
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x0000EFA4 File Offset: 0x0000D1A4
	public bool TryResolveMiniMapMetadataPreview(Vector2Int cell, out ChunkSpawnResolver.SpawnEntry entry)
	{
		if (this._spawnResolver == null)
		{
			entry = default(ChunkSpawnResolver.SpawnEntry);
			return false;
		}
		Vector2Int vector2Int;
		if (this._committedSpawnCellOwners.TryGetValue(cell, out vector2Int) && this._committedSpawnPlan.TryGetValue(vector2Int, out entry) && RankPoiPolicy.IsSpawnAllowed(entry))
		{
			return entry.IsValid;
		}
		entry = this._spawnResolver.ResolveMetadataPreview(cell);
		return entry.IsValid;
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x0000F00D File Offset: 0x0000D20D
	public bool TryResolveMiniMapFallback(Vector2Int cell, ChunkData rejectedChunk, bool excludeSpecialZones, out ChunkSpawnResolver.SpawnEntry entry)
	{
		if (this._spawnResolver == null)
		{
			entry = default(ChunkSpawnResolver.SpawnEntry);
			return false;
		}
		entry = this._spawnResolver.ResolveSmallFallback(cell, rejectedChunk, excludeSpecialZones);
		return entry.IsValid;
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x0000F03D File Offset: 0x0000D23D
	public bool TryResolveMiniMapWithoutSpecialZones(Vector2Int cell, ChunkData chunkData, out ChunkSpawnResolver.SpawnEntry entry)
	{
		if (this._spawnResolver == null)
		{
			entry = default(ChunkSpawnResolver.SpawnEntry);
			return false;
		}
		entry = this._spawnResolver.ResolveWithoutSpecialZones(cell, chunkData);
		return entry.IsValid;
	}

	// Token: 0x060002CA RID: 714 RVA: 0x0000F06C File Offset: 0x0000D26C
	private void Awake()
	{
		if (ChunkManager.Instance != null && ChunkManager.Instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		ChunkManager.Instance = this;
		WorldOrigin.Shifted += this.OnWorldOriginShifted;
		if (this.config != null)
		{
			this.config.ApplyWorldDensityPreset();
		}
	}

	// Token: 0x060002CB RID: 715 RVA: 0x0000F0D0 File Offset: 0x0000D2D0
	private void OnDestroy()
	{
		WorldOrigin.Shifted -= this.OnWorldOriginShifted;
		if (ChunkManager.Instance == this)
		{
			ChunkManager.Instance = null;
		}
		if (this._prewarmPoolCoroutine != null)
		{
			base.StopCoroutine(this._prewarmPoolCoroutine);
		}
		this.ReleaseWarmRenderResources();
		this.ReleaseMeshColliderBakeRequests();
		this._chunkVisualCaches.Clear();
		this._chunkLightingAvailability.Clear();
		this._chunkBaseTransformsCache.Clear();
		this._visuallyHiddenChunks.Clear();
		this._chunkPoolByPrefab.Clear();
		this._poolCountByPrefab.Clear();
		this._instanceToPrefabMap.Clear();
		this._zonePoolByPrefab.Clear();
		this._zonePoolCountByPrefab.Clear();
		this._zoneInstanceToPrefabMap.Clear();
		this.ClearZonePrefabResolver();
		this._chunkBaseColliderCache.Clear();
		this._stagedHeavyColliderOwners.Clear();
		this._zonePlaceholderStates.Clear();
		this._heavyZonePlaceholders.Clear();
		this._incrementalZonePlaceholders.Clear();
		this.CancelAllIncrementalNormalZoneWakes();
		this._pendingZoneRetirements.Clear();
		this._retiringZoneRoots.Clear();
		this._retiringZoneCountByPrefab.Clear();
		this._pendingZoneRetirementWorkGroups = 0;
		this.IsZoneRetirementBackpressured = false;
		this._retiringSpecialZoneReservationCount = 0;
		this._pendingChunkDespawns.Clear();
		this._pendingChunkDespawnSet.Clear();
		this._pendingActivationSet.Clear();
		this._pendingDeactivationSet.Clear();
		this._pendingPostSpawnInit.Clear();
		this._pendingPostSpawnInitSet.Clear();
		this._pendingChunkZonesReadyNotifications.Clear();
		this._pendingChunkZonesReadyByChunk.Clear();
		this._zonesReadyNotifiedChunks.Clear();
		this._specialZoneReservationCells.Clear();
		this.ReleaseDebugPoiReservation(this._debugPoiReservationOwner);
		this._committedSpawnPlan.Clear();
		int miniMapLayoutRevision = this.MiniMapLayoutRevision;
		this.MiniMapLayoutRevision = miniMapLayoutRevision + 1;
		this._unvisitedLongWayHomeStreetOrigins.Clear();
		this._longWayHomeStreetOwner = null;
		this._committedSpawnCellOwners.Clear();
		this._committedSpecialExclusionOwners.Clear();
		this._encounteredPoiOrigins.Clear();
		this._poiOriginsReachedNearbyCore.Clear();
		this._travelOffsetPinnedSpecialOrigins.Clear();
		this._hasPresentedPoiOrigin = false;
		this._hasCountedPresentedPoiEncounter = false;
		this._poiTravelDroughtScheduler.Reset();
		this._poiTravelDroughtScheduler.ClearPoiRecency();
		this.DisposeNativeCollections();
	}

	// Token: 0x060002CC RID: 716 RVA: 0x0000F310 File Offset: 0x0000D510
	public void ApplyConfig(ChunkManagerConfig cfg)
	{
		if (cfg == null)
		{
			return;
		}
		this.config = cfg;
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[ChunkManager] Config applicato: preset '{0}'", cfg.activePreset));
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x060002CD RID: 717 RVA: 0x0000F345 File Offset: 0x0000D545
	public ChunkManagerConfig Config
	{
		get
		{
			return this.config;
		}
	}

	// Token: 0x060002CE RID: 718 RVA: 0x0000F34D File Offset: 0x0000D54D
	[ContextMenu("Test/Force Rebuild Cache")]
	public void TestForceRebuildCache()
	{
		if (this.config != null)
		{
			this.config.forceRebuildPrewarmTargetCache = true;
			Debug.Log("[ChunkManager] Force rebuild cache abilitato. Riavvia la scena per applicare.");
		}
	}

	// Token: 0x060002CF RID: 719 RVA: 0x0000F374 File Offset: 0x0000D574
	public void ApplyTravelOffset(Vector2Int deltaCells, Transform keepAlive = null)
	{
		if (this._spawnResolver == null)
		{
			Debug.LogWarning("[ChunkManager] ApplyTravelOffset ignorato: il resolver non è ancora inizializzato (mappa procedurale non avviata).");
			return;
		}
		if (deltaCells == Vector2Int.zero)
		{
			return;
		}
		ChunkInstance chunkContaining = this.GetChunkContaining(keepAlive);
		Vector2Int vector2Int = ((chunkContaining != null) ? chunkContaining.originCell : default(Vector2Int));
		bool flag = chunkContaining != null && this._specialZoneReservationCells.Contains(vector2Int);
		Vector2Int travelOffset = this._spawnResolver.TravelOffset;
		this._spawnResolver.SetTravelOffset(travelOffset + deltaCells);
		this.ReleaseDebugPoiReservation(this._debugPoiReservationOwner);
		this._committedSpawnPlan.Clear();
		int miniMapLayoutRevision = this.MiniMapLayoutRevision;
		this.MiniMapLayoutRevision = miniMapLayoutRevision + 1;
		this._unvisitedLongWayHomeStreetOrigins.Clear();
		this._longWayHomeStreetOwner = null;
		this._committedSpawnCellOwners.Clear();
		this._committedSpecialExclusionOwners.Clear();
		this._encounteredPoiOrigins.Clear();
		this._poiOriginsReachedNearbyCore.Clear();
		this._travelOffsetPinnedSpecialOrigins.Clear();
		if (flag)
		{
			this._poiOriginsReachedNearbyCore.Add(vector2Int);
			this._travelOffsetPinnedSpecialOrigins.Add(vector2Int);
			Vector2Int vector2Int2 = vector2Int;
			ChunkData chunkData = chunkContaining.chunkData;
			this.RegisterCommittedSpecialExclusion(vector2Int2, (chunkData != null) ? chunkData.gridSize : Vector2Int.one);
		}
		this._hasPresentedPoiOrigin = false;
		this._hasCountedPresentedPoiEncounter = false;
		this._poiTravelDroughtScheduler.Reset();
		this._pendingChunkDespawns.Clear();
		this._pendingChunkDespawnSet.Clear();
		this._hasLastStreamingPlayerCell = false;
		this._streamingTravelDirection = Vector2Int.zero;
		this.ResetHeavyPoiPrefetchPrediction();
		int num = 0;
		int num2 = 0;
		List<Vector2Int> list = new List<Vector2Int>(this._spawnedChunks.Count);
		foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._spawnedChunks)
		{
			if (this.IsRuntimeChunk(keyValuePair.Value))
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (Vector2Int vector2Int3 in list)
		{
			ChunkInstance chunkInstance;
			if (this._spawnedChunks.TryGetValue(vector2Int3, out chunkInstance) && !(((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null))
			{
				if (chunkContaining != null && chunkInstance == chunkContaining)
				{
					num2++;
				}
				else
				{
					this.DespawnRuntimeChunk(vector2Int3, chunkInstance);
					num++;
				}
			}
		}
		Debug.Log(string.Format("[ChunkManager] Travel offset {0} -> {1} ", travelOffset, this._spawnResolver.TravelOffset) + string.Format("({0} celle): {1} chunk runtime rilasciati", deltaCells, num) + ((num2 > 0) ? string.Format(" ({0} risparmiato: ospita la corsa)", num2) : "") + ", lo streaming li rigenera.");
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000F638 File Offset: 0x0000D838
	public Vector2Int TravelOffset
	{
		get
		{
			if (this._spawnResolver == null)
			{
				return Vector2Int.zero;
			}
			return this._spawnResolver.TravelOffset;
		}
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x0000F653 File Offset: 0x0000D853
	[ContextMenu("Test/Set Preset: Low")]
	public void TestSetLow()
	{
		this.SetQualityPreset(ChunkManagerConfig.QualityPreset.Low);
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x0000F65C File Offset: 0x0000D85C
	[ContextMenu("Test/Set Preset: Medium")]
	public void TestSetMedium()
	{
		this.SetQualityPreset(ChunkManagerConfig.QualityPreset.Medium);
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x0000F665 File Offset: 0x0000D865
	[ContextMenu("Test/Set Preset: High")]
	public void TestSetHigh()
	{
		this.SetQualityPreset(ChunkManagerConfig.QualityPreset.High);
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x0000F66E File Offset: 0x0000D86E
	[ContextMenu("Test/Set Preset: Ultra")]
	public void TestSetUltra()
	{
		this.SetQualityPreset(ChunkManagerConfig.QualityPreset.Ultra);
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x0000F677 File Offset: 0x0000D877
	public void SetQualityPreset(ChunkManagerConfig.QualityPreset preset)
	{
		if (this.config == null)
		{
			Debug.LogWarning("[ChunkManager] Nessun ChunkManagerConfig assegnato — impossibile cambiare preset.");
			return;
		}
		this.config.ApplyPreset(preset);
		this.ApplyConfig(this.config);
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x0000F6AC File Offset: 0x0000D8AC
	private void DisposeNativeCollections()
	{
		if (this._visibilityJobScheduled)
		{
			this._currentVisibilityJob.Complete();
			this._visibilityJobScheduled = false;
		}
		if (this._chunkJobData.IsCreated)
		{
			this._chunkJobData.Dispose();
		}
		if (this._visibilityResults.IsCreated)
		{
			this._visibilityResults.Dispose();
		}
		if (this._occupiedCellsNative.IsCreated)
		{
			this._occupiedCellsNative.Dispose();
		}
		if (this._chunksToActivate.IsCreated)
		{
			this._chunksToActivate.Dispose();
		}
		if (this._chunksToDeactivate.IsCreated)
		{
			this._chunksToDeactivate.Dispose();
		}
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x0000F74C File Offset: 0x0000D94C
	public void BeginFastTeardown(List<GameObject> rootsOut)
	{
		if (this.IsTearingDown)
		{
			return;
		}
		this.IsTearingDown = true;
		this.ReleaseDebugPoiReservation(this._debugPoiReservationOwner);
		if (this._prewarmPoolCoroutine != null)
		{
			base.StopCoroutine(this._prewarmPoolCoroutine);
			this._prewarmPoolCoroutine = null;
		}
		base.StopAllCoroutines();
		this.ReleaseWarmRenderResources();
		this.ReleaseMeshColliderBakeRequests();
		if (this._visibilityJobScheduled)
		{
			this._currentVisibilityJob.Complete();
			this._visibilityJobScheduled = false;
		}
		base.enabled = false;
		this._pendingZoneSpawns.Clear();
		this._pendingZoneCountByChunk.Clear();
		this._pendingHeavyZoneCountByChunk.Clear();
		this._heavyZoneOwners.Clear();
		this._stagedHeavyColliderOwners.Clear();
		this.CancelAllIncrementalNormalZoneWakes();
		this._pendingChunkZonesReadyNotifications.Clear();
		this._pendingChunkZonesReadyByChunk.Clear();
		this._zonesReadyNotifiedChunks.Clear();
		this._pendingChunkDespawns.Clear();
		this._pendingChunkDespawnSet.Clear();
		this._pendingPostSpawnInit.Clear();
		this._pendingPostSpawnInitSet.Clear();
		this._pendingActivations.Clear();
		this._pendingDeactivations.Clear();
		this._pendingActivationSet.Clear();
		this._pendingDeactivationSet.Clear();
		this._pendingZoneRetirements.Clear();
		this._retiringZoneRoots.Clear();
		this._retiringZoneCountByPrefab.Clear();
		this._pendingZoneRetirementWorkGroups = 0;
		this.IsZoneRetirementBackpressured = false;
		this._retiringSpecialZoneReservationCount = 0;
		if (rootsOut != null)
		{
			ChunkManager.CollectChildRoots(this._chunkPoolContainer, rootsOut);
			ChunkManager.CollectChildRoots(this._zonePoolContainer, rootsOut);
			if (this._chunkPoolContainer != null)
			{
				rootsOut.Add(this._chunkPoolContainer.gameObject);
			}
			if (this._zonePoolContainer != null)
			{
				rootsOut.Add(this._zonePoolContainer.gameObject);
			}
			foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._spawnedChunks)
			{
				ChunkInstance value = keyValuePair.Value;
				if (((value != null) ? value.chunkRoot : null) != null && this._instanceToPrefabMap.ContainsKey(value.chunkRoot))
				{
					rootsOut.Add(value.chunkRoot);
				}
			}
		}
		this._chunkPoolContainer = null;
		this._zonePoolContainer = null;
		this._zonePoolCreationGate = null;
		this.MarkSpawnedChunkMembershipChanged();
		this._spawnedChunks.Clear();
		this._processedCells.Clear();
		this._cellToChunkMap.Clear();
		this._chunkPoolByPrefab.Clear();
		this._poolCountByPrefab.Clear();
		this._instanceToPrefabMap.Clear();
		this._zonePoolByPrefab.Clear();
		this._zonePoolCountByPrefab.Clear();
		this._zoneInstanceToPrefabMap.Clear();
		this.ClearZonePrefabResolver();
		this._chunkBaseColliderCache.Clear();
		this._stagedHeavyColliderOwners.Clear();
		this._zonePlaceholderStates.Clear();
		this._heavyZonePlaceholders.Clear();
		this._incrementalZonePlaceholders.Clear();
		this._incrementalNormalZoneOwners.Clear();
		this._pendingNormalZoneWakes.Clear();
		this._pendingNormalZoneWakeSet.Clear();
		this._specialZoneReservationCells.Clear();
		this._committedSpecialExclusionOwners.Clear();
		this._encounteredPoiOrigins.Clear();
		this._poiOriginsReachedNearbyCore.Clear();
		this._travelOffsetPinnedSpecialOrigins.Clear();
		this._hasPresentedPoiOrigin = false;
		this._hasCountedPresentedPoiEncounter = false;
		this._poiTravelDroughtScheduler.Reset();
		this._poiTravelDroughtScheduler.ClearPoiRecency();
		this._chunkVisualCaches.Clear();
		this._chunkLightingAvailability.Clear();
		this._chunkBaseTransformsCache.Clear();
		this._visuallyHiddenChunks.Clear();
		this._chunkDeactivationCounters.Clear();
		this._chunksList.Clear();
		this._chunkIdMap.Clear();
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[ChunkManager] Fast teardown avviato: {0} root raccolti per la distruzione in blocco.", (rootsOut != null) ? rootsOut.Count : 0));
		}
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0000FB34 File Offset: 0x0000DD34
	private static void CollectChildRoots(Transform container, List<GameObject> rootsOut)
	{
		if (container == null)
		{
			return;
		}
		for (int i = 0; i < container.childCount; i++)
		{
			Transform child = container.GetChild(i);
			if (child != null)
			{
				rootsOut.Add(child.gameObject);
			}
		}
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x0000FB7C File Offset: 0x0000DD7C
	private void Start()
	{
		if (this.cellSize <= 0f && GameSystemsBootstrapper.Instance != null)
		{
			this.cellSize = GameSystemsBootstrapper.Instance.CellSize;
		}
		if (this.cellSize <= 0f)
		{
			this.cellSize = 60f;
		}
		if (this.staticChunksContainer == null && GameSystemsBootstrapper.Instance != null)
		{
			this.staticChunksContainer = GameSystemsBootstrapper.Instance.StaticChunksContainer;
		}
		if (PlayerProximityProvider.Instance != null)
		{
			PlayerProximityProvider.Instance.SetCellSize(this.cellSize);
		}
		if (this.config != null)
		{
			this.ApplyConfig(this.config);
		}
		base.StartCoroutine(this.InitializeMapRoutine());
	}

	// Token: 0x060002DA RID: 730 RVA: 0x0000FC3A File Offset: 0x0000DE3A
	private IEnumerator InitializeMapRoutine()
	{
		this._initializationWorkVersion = 0;
		this._initializationPhase = "loading-catalog";
		if (this.availableChunks == null || this.availableChunks.Length == 0)
		{
			Stopwatch catalogStopwatch = Stopwatch.StartNew();
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance != null)
			{
				instance.Initialize();
			}
			this.ActiveMapCatalogResource = MapGraphicsSelection.Catalog((instance != null) ? instance.Model.mapLighting : GraphicsSettingsModel.MapLightingQuality.Original);
			Debug.Log(string.Format("[ChunkManager] Catalog {0} load started, t={1:F1}s", this.ActiveMapCatalogResource, Time.realtimeSinceStartup));
			MemoryReport.Log("catalog-start");
			ResourceRequest request = Resources.LoadAsync<MapCatalogSO>(this.ActiveMapCatalogResource);
			yield return this.WaitForCatalogRequest(request);
			MapCatalogSO mapCatalogSO = request.asset as MapCatalogSO;
			if (mapCatalogSO == null && this.ActiveMapCatalogResource != "MapCatalog")
			{
				Debug.LogWarning("[ChunkManager] Selected map catalog '" + this.ActiveMapCatalogResource + "' missing; using the standard map.");
				this.ActiveMapCatalogResource = "MapCatalog";
				request = Resources.LoadAsync<MapCatalogSO>(this.ActiveMapCatalogResource);
				yield return this.WaitForCatalogRequest(request);
				mapCatalogSO = request.asset as MapCatalogSO;
			}
			catalogStopwatch.Stop();
			this._catalogRequest = null;
			MemoryReport.Log("catalog-end");
			if (mapCatalogSO == null || mapCatalogSO.availableChunks == null || mapCatalogSO.availableChunks.Length == 0)
			{
				this._catalogLoadFailed = true;
				Debug.LogError("[ChunkManager] Catalogo mappa 'MapCatalog' non trovato in Resources (o vuoto): generazione mappa annullata. Attesa: Assets/Features/MapGeneration/Resources/MapCatalog.asset");
				yield break;
			}
			this.availableChunks = mapCatalogSO.availableChunks;
			if (this.biomeManager == null)
			{
				this.biomeManager = mapCatalogSO.biomeManager;
			}
			this.RecordCompletedInitializationWork();
			Debug.Log(string.Format("[ChunkManager] Catalogo {0} caricato in {1} ms ({2} chunk data), t={3:F1}s.", new object[]
			{
				this.ActiveMapCatalogResource,
				catalogStopwatch.ElapsedMilliseconds,
				this.availableChunks.Length,
				Time.realtimeSinceStartup
			}));
			catalogStopwatch = null;
			request = null;
		}
		this._initializationPhase = "preparing-map";
		this.CacheChunkWeights();
		int num = global::UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		num = RunModeService.ResolveWorldSeedForCurrentRun(num, ChunkManager.DiagnosticSessionSeedOverride);
		this._spawnResolver = new ChunkSpawnResolver(num, this.availableChunks, this.biomeManager, RunModeService.HasSeededFreeWorld, 0.8f);
		this.BeginPoiResidencySession();
		this._encounteredPoiOrigins.Clear();
		this._poiOriginsReachedNearbyCore.Clear();
		this._travelOffsetPinnedSpecialOrigins.Clear();
		this._hasPresentedPoiOrigin = false;
		this._hasCountedPresentedPoiEncounter = false;
		this._poiTravelDroughtScheduler.Reset();
		this._poiTravelDroughtScheduler.ClearPoiRecency();
		if (this.enableDebugLogs)
		{
			string text = string.Format("[ChunkManager] SpawnResolver inizializzato con seed: {0}", num);
			string text2;
			if (!(this.biomeManager != null))
			{
				text2 = " - biomi disattivi";
			}
			else
			{
				string text3 = " - biomi attivi ({0})";
				List<BiomeConfig> biomes = this.biomeManager.biomes;
				text2 = string.Format(text3, (biomes != null) ? biomes.Count : 0);
			}
			Debug.Log(text + text2);
		}
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[ChunkManager] Inizializzato - CellSize: {0}, UseJobs: {1}", this.cellSize, this.useJobs));
		}
		this.InitializeChunkPool();
		this.RegisterPreSpawnedChunks();
		this.InitializeJobsData();
		if ((this.enableChunkPooling && this.prewarmPoolOnStart) || (this.enableZonePooling && this.prewarmZonePoolOnStart))
		{
			this._poolPrewarmProgress = 0f;
			this._warmRenderProgress = (this.warmRenderPoolOnStart ? 0f : 1f);
			this._prewarmPoolCoroutine = base.StartCoroutine(this.PausePrewarmWhileWorldSuspended(this.PrewarmChunkPoolRoutine()));
		}
		else
		{
			this._initializationPhase = "waiting-spawn-area";
		}
		this._mapInitialized = true;
		this.RecordCompletedInitializationWork();
		yield break;
	}

	// Token: 0x060002DB RID: 731 RVA: 0x0000FC49 File Offset: 0x0000DE49
	private IEnumerator WaitForCatalogRequest(ResourceRequest request)
	{
		this._catalogRequest = request;
		float recordedProgress = 0f;
		double nextHeartbeatAt = Time.realtimeSinceStartupAsDouble + 1.0;
		while (!request.isDone)
		{
			float progress = request.progress;
			double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
			if (progress >= recordedProgress + 0.01f || realtimeSinceStartupAsDouble >= nextHeartbeatAt)
			{
				recordedProgress = Mathf.Max(recordedProgress, progress);
				nextHeartbeatAt = realtimeSinceStartupAsDouble + 1.0;
				this.RecordCompletedInitializationWork();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002DC RID: 732 RVA: 0x0000FC60 File Offset: 0x0000DE60
	private void InitializeJobsData()
	{
		if (!this.useJobs)
		{
			return;
		}
		this._chunksToActivate = new NativeList<int>(64, Allocator.Persistent);
		this._chunksToDeactivate = new NativeList<int>(64, Allocator.Persistent);
		this._occupiedCellsNative = new NativeHashSet<int2>(256, Allocator.Persistent);
		foreach (Vector2Int vector2Int in this._processedCells)
		{
			this._occupiedCellsNative.Add(new int2(vector2Int.x, vector2Int.y));
		}
	}

	// Token: 0x060002DD RID: 733 RVA: 0x0000FD10 File Offset: 0x0000DF10
	private void InitializeChunkPool()
	{
		if (!this.enableChunkPooling && !this.enableZonePooling)
		{
			return;
		}
		Transform transform = this.EnsureRuntimeWorldRoot();
		if (this.enableChunkPooling && this._chunkPoolContainer == null)
		{
			GameObject gameObject = new GameObject("ChunkPool");
			gameObject.transform.SetParent(transform, false);
			this._chunkPoolContainer = gameObject.transform;
		}
		if (this.enableZonePooling && this._zonePoolContainer == null)
		{
			GameObject gameObject2 = new GameObject("ZonePool");
			gameObject2.transform.SetParent(transform, false);
			this._zonePoolContainer = gameObject2.transform;
		}
	}

	// Token: 0x060002DE RID: 734 RVA: 0x0000FDAC File Offset: 0x0000DFAC
	private Transform EnsureRuntimeWorldRoot()
	{
		if (this._runtimeWorldRoot != null)
		{
			return this._runtimeWorldRoot;
		}
		Transform transform = base.transform.Find("__PROCEDURAL_RUNTIME_WORLD");
		if (transform != null)
		{
			this._runtimeWorldRoot = transform;
			return this._runtimeWorldRoot;
		}
		GameObject gameObject = new GameObject("__PROCEDURAL_RUNTIME_WORLD");
		gameObject.transform.SetParent(base.transform, false);
		this._runtimeWorldRoot = gameObject.transform;
		return this._runtimeWorldRoot;
	}

	// Token: 0x060002DF RID: 735 RVA: 0x0000FE25 File Offset: 0x0000E025
	private IEnumerator PausePrewarmWhileWorldSuspended(IEnumerator work)
	{
		try
		{
			for (;;)
			{
				if (!this.IsPoiTravelSuspended || this.IsTearingDown)
				{
					if (this.IsTearingDown || !work.MoveNext())
					{
						break;
					}
					IEnumerator enumerator = work.Current as IEnumerator;
					if (enumerator != null)
					{
						yield return this.PausePrewarmWhileWorldSuspended(enumerator);
					}
					else
					{
						yield return work.Current;
					}
				}
				else
				{
					yield return null;
				}
			}
			yield break;
		}
		finally
		{
			IDisposable disposable = work as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		yield break;
		yield break;
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x0000FE3B File Offset: 0x0000E03B
	private IEnumerator PrewarmChunkPoolRoutine()
	{
		this._initializationPhase = "waiting-visible-area";
		while (!this._initialGenerationComplete && !this.IsTearingDown)
		{
			yield return null;
		}
		if (this.IsTearingDown)
		{
			this._prewarmPoolCoroutine = null;
			yield break;
		}
		double prewarmStartedAt = Time.realtimeSinceStartupAsDouble;
		Debug.Log(string.Format("[ChunkManager] Prewarm start: initial area ready ({0} chunk), ", this._spawnedChunks.Count) + string.Format("t={0:F1}s", Time.realtimeSinceStartup));
		int createdChunkInstances = 0;
		int createdZoneInstances = 0;
		int lifecycleWarmedZoneInstances = 0;
		int createdThisFrame = 0;
		float num = Time.realtimeSinceStartup;
		this._initializationPhase = "building-prewarm-targets";
		Dictionary<GameObject, int> dictionary;
		Dictionary<GameObject, int> zoneTargets;
		this.GetOrBuildPrewarmTargets(out dictionary, out zoneTargets);
		int totalToCreate = 0;
		int completedCreates = 0;
		if (this.enableChunkPooling && this.prewarmPoolOnStart && this.initialPrewarmCount > 0)
		{
			foreach (KeyValuePair<GameObject, int> keyValuePair in dictionary)
			{
				totalToCreate += Mathf.Max(0, keyValuePair.Value - this.GetPoolCount(keyValuePair.Key));
			}
		}
		if (this.enableZonePooling && this.prewarmZonePoolOnStart)
		{
			foreach (KeyValuePair<GameObject, int> keyValuePair2 in zoneTargets)
			{
				if (!ChunkManager.IsNeverPooledHeavyZone(keyValuePair2.Key))
				{
					totalToCreate += Mathf.Max(0, keyValuePair2.Value - this.GetZonePoolCount(keyValuePair2.Key));
				}
			}
		}
		if (totalToCreate == 0)
		{
			this._poolPrewarmProgress = 1f;
		}
		if (this.enableChunkPooling && this.prewarmPoolOnStart && this.initialPrewarmCount > 0)
		{
			this._initializationPhase = "creating-chunk-pool";
			foreach (KeyValuePair<GameObject, int> keyValuePair3 in dictionary)
			{
				GameObject prefab = keyValuePair3.Key;
				int targetCount = keyValuePair3.Value;
				int poolCount = this.GetPoolCount(prefab);
				int num2;
				for (int i = poolCount; i < targetCount; i = num2 + 1)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(prefab, this._chunkPoolContainer);
					this.RegisterPooledInstance(gameObject, prefab);
					this.CacheBaseChunkColliders(gameObject);
					this.CollectMeshColliderBakeRequests(gameObject);
					gameObject.SetActive(false);
					this.ReturnInstanceToPool(gameObject, prefab);
					this.RecordCompletedInitializationWork();
					num2 = createdChunkInstances;
					createdChunkInstances = num2 + 1;
					num2 = createdThisFrame;
					createdThisFrame = num2 + 1;
					num2 = completedCreates;
					completedCreates = num2 + 1;
					this._poolPrewarmProgress = Mathf.Clamp01((float)completedCreates / (float)totalToCreate);
					float num3 = (Time.realtimeSinceStartup - num) * 1000f;
					if (createdThisFrame >= this.effPrewarmInstancesPerFrame || num3 >= this.effPrewarmFrameBudgetMs)
					{
						createdThisFrame = 0;
						yield return null;
						num = Time.realtimeSinceStartup;
					}
					num2 = i;
				}
				prefab = null;
			}
			Dictionary<GameObject, int>.Enumerator enumerator2 = default(Dictionary<GameObject, int>.Enumerator);
		}
		if (this.enableZonePooling && this.prewarmZonePoolOnStart)
		{
			this._initializationPhase = "creating-zone-pool";
			bool zonePoolWasActive = this._zonePoolContainer != null && this._zonePoolContainer.gameObject.activeSelf;
			bool buildDormant = !this.warmZonePoolLifecycleOnStart && this._zonePoolContainer != null;
			if (buildDormant && zonePoolWasActive)
			{
				this._zonePoolContainer.gameObject.SetActive(false);
			}
			try
			{
				foreach (KeyValuePair<GameObject, int> keyValuePair4 in zoneTargets)
				{
					GameObject prefab = keyValuePair4.Key;
					if (!ChunkManager.IsNeverPooledHeavyZone(prefab))
					{
						int targetCount = keyValuePair4.Value;
						int zonePoolCount = this.GetZonePoolCount(prefab);
						bool usesHeavyStreamingProxy = prefab != null && prefab.GetComponent<HeavyZoneStreamingProxy>() != null;
						int num2;
						for (int i = zonePoolCount; i < targetCount; i = num2 + 1)
						{
							GameObject instance = Object.Instantiate<GameObject>(prefab, new Vector3(0f, -5000f, 0f), Quaternion.identity, this._zonePoolContainer);
							this.RegisterPooledZoneInstance(instance, prefab);
							if (this.warmZonePoolLifecycleOnStart && !usesHeavyStreamingProxy)
							{
								yield return null;
								yield return null;
								num2 = lifecycleWarmedZoneInstances;
								lifecycleWarmedZoneInstances = num2 + 1;
								createdThisFrame = 0;
								num = Time.realtimeSinceStartup;
							}
							this.CollectMeshColliderBakeRequests(instance);
							IncrementalPooledZoneActivation incrementalPooledZoneActivation = this.PrepareIncrementalNormalZoneActivation(instance);
							instance.SetActive(false);
							if (incrementalPooledZoneActivation != null)
							{
								incrementalPooledZoneActivation.PrepareDormancyWhileRootInactive();
							}
							this.ReturnZoneInstanceToPool(instance, prefab);
							this.RecordCompletedInitializationWork();
							num2 = createdZoneInstances;
							createdZoneInstances = num2 + 1;
							num2 = createdThisFrame;
							createdThisFrame = num2 + 1;
							num2 = completedCreates;
							completedCreates = num2 + 1;
							this._poolPrewarmProgress = Mathf.Clamp01((float)completedCreates / (float)totalToCreate);
							float num4 = (Time.realtimeSinceStartup - num) * 1000f;
							if (createdThisFrame >= this.effPrewarmInstancesPerFrame || num4 >= this.effPrewarmFrameBudgetMs)
							{
								createdThisFrame = 0;
								yield return null;
								num = Time.realtimeSinceStartup;
							}
							instance = null;
							num2 = i;
						}
						prefab = null;
					}
				}
				Dictionary<GameObject, int>.Enumerator enumerator2 = default(Dictionary<GameObject, int>.Enumerator);
			}
			finally
			{
				if (buildDormant && this._zonePoolContainer != null && zonePoolWasActive)
				{
					this._zonePoolContainer.gameObject.SetActive(true);
				}
			}
		}
		if (this.preloadHeavyZonePayloadsOnStart)
		{
			yield return this.PreloadHeavyZonePayloadsRoutine(zoneTargets);
		}
		yield return this.BakeCollectedMeshCollidersRoutine();
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[ChunkManager] Prewarm completato. Chunk creati: {0}, ", createdChunkInstances) + string.Format("Zone create: {0}, lifecycle zone warmed: {1}", createdZoneInstances, lifecycleWarmedZoneInstances));
		}
		this._poolPrewarmProgress = 1f;
		if (this.warmRenderPoolOnStart)
		{
			yield return this.WarmRenderPooledPrefabsRoutine();
		}
		this._warmRenderProgress = 1f;
		this._initializationPhase = "waiting-spawn-area";
		this._prewarmPoolCoroutine = null;
		Debug.Log(string.Format("[ChunkManager] Prewarm end: {0} chunk, {1} zone instances, ", createdChunkInstances, createdZoneInstances) + string.Format("{0:F1}s, t={1:F1}s", Time.realtimeSinceStartupAsDouble - prewarmStartedAt, Time.realtimeSinceStartup));
		yield break;
		yield break;
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x0000FE4A File Offset: 0x0000E04A
	private IEnumerator PreloadHeavyZonePayloadsRoutine(Dictionary<GameObject, int> zoneTargets)
	{
		this._initializationPhase = "preloading-heavy-payloads";
		if (zoneTargets == null || zoneTargets.Count == 0)
		{
			yield break;
		}
		Vector3 preloadPosition = new Vector3(0f, -5000f, 0f);
		foreach (KeyValuePair<GameObject, int> keyValuePair in zoneTargets)
		{
			GameObject prefab = keyValuePair.Key;
			HeavyZoneStreamingProxy heavyZoneStreamingProxy = ((prefab != null) ? prefab.GetComponent<HeavyZoneStreamingProxy>() : null);
			if (!(heavyZoneStreamingProxy == null) && !heavyZoneStreamingProxy.NeverPool)
			{
				GameObject instance = null;
				Queue<GameObject> queue;
				if (this._zonePoolByPrefab.TryGetValue(prefab, out queue) && queue.Count > 0)
				{
					instance = queue.Dequeue();
					this._zonePoolCountByPrefab[prefab] = queue.Count;
				}
				if (instance == null)
				{
					instance = Object.Instantiate<GameObject>(prefab, this._zonePoolContainer);
					this.RegisterPooledZoneInstance(instance, prefab);
					this.RecordCompletedInitializationWork();
				}
				instance.transform.SetParent(this._zonePoolContainer);
				instance.transform.SetPositionAndRotation(preloadPosition, Quaternion.identity);
				instance.SetActive(true);
				HeavyZoneStreamingProxy proxy = instance.GetComponent<HeavyZoneStreamingProxy>();
				float started = Time.realtimeSinceStartup;
				while (proxy != null && !proxy.IsReady && !proxy.LoadFailed)
				{
					yield return null;
				}
				if (proxy != null && proxy.LoadFailed)
				{
					Debug.LogError("[ChunkManager] Heavy zone startup preload failed for '" + prefab.name + "'.");
				}
				else if (this.enableDebugLogs)
				{
					Debug.Log("[ChunkManager] Heavy zone startup preload ready for '" + prefab.name + "' in " + string.Format("{0:F0} ms.", (Time.realtimeSinceStartup - started) * 1000f));
				}
				this.CollectMeshColliderBakeRequests(instance);
				if (proxy != null && proxy.UsesStagedPooledActivation && proxy.IsReady && !proxy.LoadFailed)
				{
					yield return proxy.PrepareStagedDormancyAfterWarmup();
				}
				instance.SetActive(false);
				this.ReturnZoneInstanceToPool(instance, prefab);
				if (proxy != null && proxy.IsReady && !proxy.LoadFailed)
				{
					this.RecordCompletedInitializationWork();
				}
				prefab = null;
				instance = null;
				proxy = null;
			}
		}
		Dictionary<GameObject, int>.Enumerator enumerator = default(Dictionary<GameObject, int>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x0000FE60 File Offset: 0x0000E060
	private void CollectMeshColliderBakeRequests(GameObject root)
	{
		if (root == null)
		{
			return;
		}
		this._meshColliderPrewarmScratch.Clear();
		root.GetComponentsInChildren<MeshCollider>(true, this._meshColliderPrewarmScratch);
		for (int i = 0; i < this._meshColliderPrewarmScratch.Count; i++)
		{
			MeshCollider meshCollider = this._meshColliderPrewarmScratch[i];
			Mesh mesh = ((meshCollider != null) ? meshCollider.sharedMesh : null);
			if (!(mesh == null))
			{
				ChunkManager.MeshColliderBakeKey meshColliderBakeKey = new ChunkManager.MeshColliderBakeKey(mesh.GetInstanceID(), meshCollider.convex, meshCollider.cookingOptions);
				if (this._meshColliderBakeKeys.Add(meshColliderBakeKey) && (!meshCollider.enabled || !meshCollider.gameObject.activeInHierarchy))
				{
					if (!mesh.isReadable)
					{
						int skippedUnreadableMeshColliderVariants = this.SkippedUnreadableMeshColliderVariants;
						this.SkippedUnreadableMeshColliderVariants = skippedUnreadableMeshColliderVariants + 1;
					}
					else
					{
						this._meshColliderBakeRequests.Add(new ChunkManager.MeshColliderBakeRequest
						{
							meshId = meshColliderBakeKey.meshId,
							convex = meshColliderBakeKey.convex,
							cookingOptions = meshColliderBakeKey.cookingOptions
						});
						this._meshColliderBakeMeshes[meshColliderBakeKey.meshId] = mesh;
					}
				}
			}
		}
		this._meshColliderPrewarmScratch.Clear();
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x0000FF88 File Offset: 0x0000E188
	private IEnumerator BakeCollectedMeshCollidersRoutine()
	{
		this._initializationPhase = "baking-colliders";
		int requestCount = this._meshColliderBakeRequests.Count;
		this.PrebakedMeshColliderVariants = requestCount;
		if (requestCount == 0 || this.IsTearingDown)
		{
			this.ReleaseMeshColliderBakeRequests();
			yield break;
		}
		this.CompleteMeshColliderBakeJob();
		this.CompletedMeshColliderBakeVariantCount = 0;
		this.MeshColliderBakeWaveCount = 0;
		this.PeakMeshColliderBakeWaveVariants = 0;
		this.MeshColliderBakeVertexCount = 0L;
		this.MeshColliderBakeIndexCount = 0L;
		this.MeshColliderBakeJobSeconds = 0.0;
		this.LongestMeshColliderBakeWaveSeconds = 0.0;
		foreach (Mesh mesh in this._meshColliderBakeMeshes.Values)
		{
			if (!(mesh == null))
			{
				this.MeshColliderBakeVertexCount += (long)mesh.vertexCount;
				for (int i = 0; i < mesh.subMeshCount; i++)
				{
					this.MeshColliderBakeIndexCount += (long)((ulong)mesh.GetIndexCount(i));
				}
			}
		}
		Debug.Log(string.Format("[ColliderBake] start variants={0}, meshes={1}, ", requestCount, this._meshColliderBakeMeshes.Count) + string.Format("vertices={0}, indices={1}, ", this.MeshColliderBakeVertexCount, this.MeshColliderBakeIndexCount) + string.Format("waveLimit={0}, workerBatch=8", 64));
		MemoryReport.Log("collider-bake-start");
		this._meshColliderBakePending.Clear();
		this._meshColliderBakePending.AddRange(this._meshColliderBakeRequests);
		while (!this.IsTearingDown && this.BuildNextMeshColliderBakeWave() > 0)
		{
			int count = this._meshColliderBakeWave.Count;
			this._meshColliderBakeNativeRequests = new NativeArray<ChunkManager.MeshColliderBakeRequest>(count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int j = 0; j < count; j++)
			{
				this._meshColliderBakeNativeRequests[j] = this._meshColliderBakeWave[j];
			}
			this._meshColliderBakeWaveStartedAt = Time.realtimeSinceStartupAsDouble;
			this._meshColliderBakeJobHandle = new ChunkManager.BakeMeshCollidersJob
			{
				requests = this._meshColliderBakeNativeRequests
			}.Schedule(count, 8, default(JobHandle));
			this._meshColliderBakeJobScheduled = true;
			int meshColliderBakeWaveCount = this.MeshColliderBakeWaveCount;
			this.MeshColliderBakeWaveCount = meshColliderBakeWaveCount + 1;
			this.PeakMeshColliderBakeWaveVariants = Mathf.Max(this.PeakMeshColliderBakeWaveVariants, count);
			while (!this._meshColliderBakeJobHandle.IsCompleted && !this.IsTearingDown)
			{
				yield return null;
			}
			this.CompleteMeshColliderBakeJob();
			this.RecordCompletedInitializationWork();
			if (!this.IsTearingDown)
			{
				yield return null;
			}
		}
		this.ReleaseMeshColliderBakeRequests();
		Debug.Log(string.Format("[ColliderBake] complete variants={0}/{1}, ", this.CompletedMeshColliderBakeVariantCount, requestCount) + string.Format("waves={0}, peakWaveVariants={1}, ", this.MeshColliderBakeWaveCount, this.PeakMeshColliderBakeWaveVariants) + string.Format("jobWallSeconds={0:F3}, longestWaveWallSeconds={1:F3}", this.MeshColliderBakeJobSeconds, this.LongestMeshColliderBakeWaveSeconds));
		MemoryReport.Log("collider-bake-end");
		if (this.enableDebugLogs)
		{
			Debug.Log("[ChunkManager] PhysX pre-bake completato per " + string.Format("{0} varianti MeshCollider ", this.PrebakedMeshColliderVariants) + string.Format("({0} non-readable affidate al build).", this.SkippedUnreadableMeshColliderVariants));
		}
		yield break;
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x0000FF98 File Offset: 0x0000E198
	private int BuildNextMeshColliderBakeWave()
	{
		this._meshColliderBakeWave.Clear();
		this._meshColliderBakeWaveMeshIds.Clear();
		int num = this._meshColliderBakePending.Count - 1;
		while (num >= 0 && this._meshColliderBakeWave.Count < 64)
		{
			ChunkManager.MeshColliderBakeRequest meshColliderBakeRequest = this._meshColliderBakePending[num];
			if (this._meshColliderBakeWaveMeshIds.Add(meshColliderBakeRequest.meshId))
			{
				this._meshColliderBakeWave.Add(meshColliderBakeRequest);
				this._meshColliderBakePending.RemoveAt(num);
			}
			num--;
		}
		return this._meshColliderBakeWave.Count;
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00010028 File Offset: 0x0000E228
	private void CompleteMeshColliderBakeJob()
	{
		if (this._meshColliderBakeJobScheduled)
		{
			this._meshColliderBakeJobHandle.Complete();
			this._meshColliderBakeJobScheduled = false;
			double num = Math.Max(0.0, Time.realtimeSinceStartupAsDouble - this._meshColliderBakeWaveStartedAt);
			this.MeshColliderBakeJobSeconds += num;
			this.LongestMeshColliderBakeWaveSeconds = Math.Max(this.LongestMeshColliderBakeWaveSeconds, num);
			this.CompletedMeshColliderBakeVariantCount += this._meshColliderBakeWave.Count;
		}
		if (this._meshColliderBakeNativeRequests.IsCreated)
		{
			this._meshColliderBakeNativeRequests.Dispose();
		}
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x000100BC File Offset: 0x0000E2BC
	private void ReleaseMeshColliderBakeRequests()
	{
		this.CompleteMeshColliderBakeJob();
		this._meshColliderBakePending.Clear();
		this._meshColliderBakeWave.Clear();
		this._meshColliderBakeWaveMeshIds.Clear();
		this._meshColliderBakeRequests.Clear();
		this._meshColliderBakeKeys.Clear();
		this._meshColliderBakeMeshes.Clear();
		this._meshColliderPrewarmScratch.Clear();
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x0001011C File Offset: 0x0000E31C
	private ChunkManager.WarmRenderResources AcquireWarmRenderResources(string name)
	{
		ChunkManager.WarmRenderResources warmRenderResources = new ChunkManager.WarmRenderResources(this, name, this.EnsureRuntimeWorldRoot());
		this._warmRenderResources.Add(warmRenderResources);
		return warmRenderResources;
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x00010148 File Offset: 0x0000E348
	private void ReleaseWarmRenderResources()
	{
		foreach (ChunkManager.WarmRenderResources warmRenderResources in new List<ChunkManager.WarmRenderResources>(this._warmRenderResources))
		{
			warmRenderResources.Dispose();
		}
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x000101A0 File Offset: 0x0000E3A0
	private IEnumerator WarmRenderPooledPrefabsRoutine()
	{
		if (this.IsTearingDown)
		{
			yield break;
		}
		ChunkManager.WarmRenderResources resources = this.AcquireWarmRenderResources("MapWarmupCamera");
		try
		{
			yield return this.WarmRenderPooledPrefabsOwnedRoutine(resources);
		}
		finally
		{
			resources.Dispose();
		}
		yield break;
		yield break;
	}

	// Token: 0x060002EA RID: 746 RVA: 0x000101AF File Offset: 0x0000E3AF
	private IEnumerator WarmRenderPooledPrefabsOwnedRoutine(ChunkManager.WarmRenderResources resources)
	{
		this._initializationPhase = "warming-render";
		this.WarmRenderStarted = true;
		float warmStart = Time.realtimeSinceStartup;
		Vector3 warmPos = new Vector3(0f, -5000f, 0f);
		Camera warmCamera = resources.Camera;
		int warmedPrefabs = 0;
		List<ValueTuple<GameObject, bool>> warmTargets = new List<ValueTuple<GameObject, bool>>();
		if (this.enableChunkPooling)
		{
			foreach (GameObject gameObject in this._chunkPoolByPrefab.Keys)
			{
				warmTargets.Add(new ValueTuple<GameObject, bool>(gameObject, false));
			}
		}
		if (this.enableZonePooling)
		{
			foreach (GameObject gameObject2 in this._zonePoolByPrefab.Keys)
			{
				warmTargets.Add(new ValueTuple<GameObject, bool>(gameObject2, true));
			}
		}
		int totalWarmTargets = warmTargets.Count + this._registeredWarmRenderPrefabs.Count;
		int completedWarmTargets = 0;
		if (totalWarmTargets == 0)
		{
			this._warmRenderProgress = 1f;
		}
		int warmBatchSize = Mathf.Max(1, this.warmRenderBatchSize);
		List<ValueTuple<GameObject, GameObject, bool>> warmBatch = new List<ValueTuple<GameObject, GameObject, bool>>(warmBatchSize);
		HashSet<HeavyZoneStreamingProxy> completedHeavyPayloads = new HashSet<HeavyZoneStreamingProxy>();
		for (int batchStart = 0; batchStart < warmTargets.Count; batchStart += warmBatchSize)
		{
			warmBatch.Clear();
			completedHeavyPayloads.Clear();
			this._initializationPhase = "warming-render";
			int num = Mathf.Min(batchStart + warmBatchSize, warmTargets.Count);
			for (int j = batchStart; j < num; j++)
			{
				GameObject item = warmTargets[j].Item1;
				bool item2 = warmTargets[j].Item2;
				Queue<GameObject> queue;
				if (!(item2 ? this._zonePoolByPrefab : this._chunkPoolByPrefab).TryGetValue(item, out queue) || queue.Count == 0)
				{
					int num2 = completedWarmTargets;
					completedWarmTargets = num2 + 1;
					this._warmRenderProgress = Mathf.Clamp01((float)completedWarmTargets / (float)totalWarmTargets);
				}
				else
				{
					GameObject gameObject3 = queue.Dequeue();
					(item2 ? this._zonePoolCountByPrefab : this._poolCountByPrefab)[item] = queue.Count;
					if (gameObject3 == null)
					{
						int num2 = completedWarmTargets;
						completedWarmTargets = num2 + 1;
						this._warmRenderProgress = Mathf.Clamp01((float)completedWarmTargets / (float)totalWarmTargets);
					}
					else
					{
						IncrementalPooledZoneActivation component = gameObject3.GetComponent<IncrementalPooledZoneActivation>();
						if (component != null)
						{
							component.RestoreSnapshotWhileRootInactive();
						}
						gameObject3.transform.SetPositionAndRotation(warmPos, Quaternion.identity);
						HeavyZoneStreamingProxy heavyZoneStreamingProxy = (item2 ? gameObject3.GetComponent<HeavyZoneStreamingProxy>() : null);
						if (heavyZoneStreamingProxy != null && heavyZoneStreamingProxy.UsesStagedPooledActivation)
						{
							heavyZoneStreamingProxy.ArmStagedWake();
						}
						gameObject3.SetActive(true);
						warmBatch.Add(new ValueTuple<GameObject, GameObject, bool>(gameObject3, item, item2));
					}
				}
			}
			if (warmBatch.Count != 0)
			{
				float heavyWaitStarted = Time.realtimeSinceStartup;
				this._initializationPhase = "waiting-render-payloads";
				bool waitingForHeavyPayload;
				do
				{
					waitingForHeavyPayload = false;
					for (int k = 0; k < warmBatch.Count; k++)
					{
						if (warmBatch[k].Item3 && !(warmBatch[k].Item1 == null))
						{
							HeavyZoneStreamingProxy component2 = warmBatch[k].Item1.GetComponent<HeavyZoneStreamingProxy>();
							if (!(component2 == null) && !component2.LoadFailed)
							{
								if (component2.IsOperationalReady)
								{
									if (completedHeavyPayloads.Add(component2))
									{
										this.RecordCompletedInitializationWork();
									}
								}
								else
								{
									waitingForHeavyPayload = true;
								}
							}
						}
					}
					if (waitingForHeavyPayload)
					{
						yield return null;
					}
				}
				while (waitingForHeavyPayload);
				this._initializationPhase = "warming-render";
				float num3 = (Time.realtimeSinceStartup - heavyWaitStarted) * 1000f;
				if (num3 > 100f && this.enableDebugLogs)
				{
					Debug.Log(string.Format("[ChunkManager] Heavy payload startup warmup completed in {0:F0} ms.", num3));
				}
				Bounds bounds = ChunkManager.ComputeInstanceRendererBounds(warmBatch[0].Item1, warmPos);
				for (int l = 1; l < warmBatch.Count; l++)
				{
					bounds.Encapsulate(ChunkManager.ComputeInstanceRendererBounds(warmBatch[l].Item1, warmPos));
				}
				warmCamera.transform.position = bounds.center + Vector3.up * (bounds.extents.y + 50f);
				warmCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
				warmCamera.orthographicSize = Mathf.Max(bounds.extents.x, bounds.extents.z) + 10f;
				warmCamera.nearClipPlane = 0.3f;
				warmCamera.farClipPlane = bounds.size.y + 200f;
				yield return null;
				int num2;
				for (int i = 0; i < warmBatch.Count; i = num2 + 1)
				{
					ValueTuple<GameObject, GameObject, bool> entry = warmBatch[i];
					num2 = completedWarmTargets;
					completedWarmTargets = num2 + 1;
					this._warmRenderProgress = Mathf.Clamp01((float)completedWarmTargets / (float)totalWarmTargets);
					if (!(entry.Item1 == null))
					{
						if (entry.Item3)
						{
							HeavyZoneStreamingProxy component3 = entry.Item1.GetComponent<HeavyZoneStreamingProxy>();
							if (component3 != null && component3.UsesStagedPooledActivation && component3.IsOperationalReady && !component3.LoadFailed)
							{
								this._initializationPhase = "preparing-render-dormancy";
								yield return component3.PrepareStagedDormancyAfterWarmup();
								if (entry.Item1 == null)
								{
									goto IL_0750;
								}
							}
						}
						entry.Item1.SetActive(false);
						IncrementalPooledZoneActivation component4 = entry.Item1.GetComponent<IncrementalPooledZoneActivation>();
						if (component4 != null)
						{
							component4.PrepareDormancyWhileRootInactive();
						}
						if (entry.Item3)
						{
							this.ReturnZoneInstanceToPool(entry.Item1, entry.Item2);
						}
						else
						{
							this.ReturnInstanceToPool(entry.Item1, entry.Item2);
						}
						this.RecordCompletedInitializationWork();
						this._initializationPhase = "warming-render";
						num2 = warmedPrefabs;
						warmedPrefabs = num2 + 1;
						entry = default(ValueTuple<GameObject, GameObject, bool>);
					}
					IL_0750:
					num2 = i;
				}
			}
		}
		foreach (GameObject gameObject4 in this._registeredWarmRenderPrefabs)
		{
			int num2;
			if (gameObject4 != null)
			{
				this._initializationPhase = "warming-external-prefabs";
				yield return ChunkManager.WarmRenderPrefabOnce(gameObject4, resources, warmPos);
				this.RecordCompletedInitializationWork();
				num2 = warmedPrefabs;
				warmedPrefabs = num2 + 1;
			}
			num2 = completedWarmTargets;
			completedWarmTargets = num2 + 1;
			this._warmRenderProgress = Mathf.Clamp01((float)completedWarmTargets / (float)totalWarmTargets);
		}
		List<GameObject>.Enumerator enumerator2 = default(List<GameObject>.Enumerator);
		this._warmRenderProgress = 1f;
		if (this.enableDebugLogs)
		{
			float num4 = (Time.realtimeSinceStartup - warmStart) * 1000f;
			Debug.Log(string.Format("[ChunkManager] Warm render completato: {0} prefab in {1:F0} ms (batch={2})", warmedPrefabs, num4, warmBatchSize));
		}
		yield break;
		yield break;
	}

	// Token: 0x060002EB RID: 747 RVA: 0x000101C8 File Offset: 0x0000E3C8
	public bool RegisterWarmRenderPrefabs(IEnumerable<GameObject> prefabs)
	{
		if (this.WarmRenderStarted || prefabs == null)
		{
			return false;
		}
		bool flag = false;
		foreach (GameObject gameObject in prefabs)
		{
			if (!(gameObject == null) && !this._registeredWarmRenderPrefabs.Contains(gameObject))
			{
				this._registeredWarmRenderPrefabs.Add(gameObject);
				flag = true;
			}
		}
		return flag;
	}

	// Token: 0x060002EC RID: 748 RVA: 0x00010240 File Offset: 0x0000E440
	public IEnumerator WarmRenderExtraPrefabsNow(IEnumerable<GameObject> prefabs)
	{
		if (prefabs == null || this.IsTearingDown)
		{
			yield break;
		}
		Vector3 warmPos = new Vector3(0f, -5000f, 0f);
		ChunkManager.WarmRenderResources resources = this.AcquireWarmRenderResources("MapWarmupCamera_Extra");
		try
		{
			foreach (GameObject gameObject in prefabs)
			{
				if (resources.IsDisposed || this.IsTearingDown)
				{
					yield break;
				}
				if (gameObject != null)
				{
					yield return this.PausePrewarmWhileWorldSuspended(ChunkManager.WarmRenderPrefabOnce(gameObject, resources, warmPos));
				}
			}
			IEnumerator<GameObject> enumerator = null;
		}
		finally
		{
			resources.Dispose();
		}
		yield break;
		yield break;
	}

	// Token: 0x060002ED RID: 749 RVA: 0x00010256 File Offset: 0x0000E456
	private static IEnumerator WarmRenderPrefabOnce(GameObject prefab, ChunkManager.WarmRenderResources resources, Vector3 warmPos)
	{
		if (resources.IsDisposed)
		{
			yield break;
		}
		GameObject instance = resources.InstantiateOwned(prefab, warmPos);
		try
		{
			Camera camera = resources.Camera;
			Bounds bounds = ChunkManager.ComputeInstanceRendererBounds(instance, warmPos);
			camera.transform.position = bounds.center + Vector3.up * (bounds.extents.y + 50f);
			camera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
			camera.orthographicSize = Mathf.Max(bounds.extents.x, bounds.extents.z) + 10f;
			camera.nearClipPlane = 0.3f;
			camera.farClipPlane = bounds.size.y + 200f;
			yield return null;
		}
		finally
		{
			resources.ReleaseInstance(instance);
		}
		yield break;
		yield break;
	}

	// Token: 0x060002EE RID: 750 RVA: 0x00010274 File Offset: 0x0000E474
	private static Bounds ComputeInstanceRendererBounds(GameObject instance, Vector3 fallbackCenter)
	{
		Renderer[] componentsInChildren = instance.GetComponentsInChildren<Renderer>(false);
		if (componentsInChildren.Length == 0)
		{
			return new Bounds(fallbackCenter, new Vector3(200f, 200f, 200f));
		}
		Bounds bounds = componentsInChildren[0].bounds;
		for (int i = 1; i < componentsInChildren.Length; i++)
		{
			bounds.Encapsulate(componentsInChildren[i].bounds);
		}
		return bounds;
	}

	// Token: 0x060002EF RID: 751 RVA: 0x000102D0 File Offset: 0x0000E4D0
	private IncrementalPooledZoneActivation PrepareIncrementalNormalZoneActivation(GameObject instance)
	{
		if (!this.enableIncrementalNormalZoneActivation || instance == null || instance.GetComponent<HeavyZoneStreamingProxy>() != null)
		{
			return null;
		}
		IncrementalPooledZoneActivation component = instance.GetComponent<IncrementalPooledZoneActivation>();
		if (component != null)
		{
			return component;
		}
		this._normalZoneTransformScratch.Clear();
		instance.GetComponentsInChildren<Transform>(true, this._normalZoneTransformScratch);
		int num = Mathf.Max(0, this._normalZoneTransformScratch.Count - 1);
		this._normalZoneTransformScratch.Clear();
		if (num < Mathf.Max(1, this.normalZoneStagingTransformThreshold))
		{
			return null;
		}
		IncrementalPooledZoneActivation incrementalPooledZoneActivation = instance.AddComponent<IncrementalPooledZoneActivation>();
		incrementalPooledZoneActivation.CaptureAuthoredHierarchy();
		return incrementalPooledZoneActivation;
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x00010364 File Offset: 0x0000E564
	private void ProcessDeferredStreamingQueues()
	{
		bool flag = this._fullGameplayReady && !this.IsLoadingBoostActive;
		this._deferredStreamingBudget.BeginFrame((double)this.effPostSpawnInitFrameBudgetMs + (double)this.effZoneSpawnFrameBudgetMs, flag);
		this.DeferredStreamingBudgetFrame = Time.frameCount;
		this.DeferredStreamingBudgetActiveLastFrame = flag;
		this.DeferredStreamingBudgetMsLastFrame = this._deferredStreamingBudget.LimitMilliseconds;
		bool flag2 = flag && this.enableDeferredPostSpawnInit && this._pendingPostSpawnInit.Count > 0 && this._pendingZoneSpawns.Count > 0 && this.effMaxPostSpawnInitOpsPerFrame > 0 && this.effMaxZoneSpawnsPerFrame > 0 && this.effPostSpawnInitFrameBudgetMs > 0f && this.effZoneSpawnFrameBudgetMs > 0f;
		bool flag3 = flag2 && this._preferDeferredZoneSpawnWhenContended;
		if (flag2)
		{
			this._preferDeferredZoneSpawnWhenContended = !this._preferDeferredZoneSpawnWhenContended;
		}
		try
		{
			if (!flag3)
			{
				this.ProcessPostSpawnInitializationQueueWithinBudget(this._deferredStreamingBudget);
			}
			this.ProcessSeededPoiPoolReclamation();
			this.ProcessSeededNormalZonePoolReplenishment();
			this.ProcessDeferredZoneSpawnQueueWithinBudget(this._deferredStreamingBudget);
			if (flag3)
			{
				this.ProcessPostSpawnInitializationQueueWithinBudget(this._deferredStreamingBudget);
			}
		}
		finally
		{
			this.DeferredStreamingElapsedMsLastFrame = this._deferredStreamingBudget.ElapsedMilliseconds;
			this.DeferredStreamingBudgetDeferralsLastFrame = this._deferredStreamingBudget.DeferredQueueCount;
		}
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x000104A4 File Offset: 0x0000E6A4
	private void ProcessDeferredZoneSpawnQueue()
	{
		this.ProcessDeferredZoneSpawnQueueWithinBudget(null);
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x000104B0 File Offset: 0x0000E6B0
	private void ProcessDeferredZoneSpawnQueueWithinBudget(DeferredStreamingFrameBudget budget)
	{
		if (this._pendingZoneSpawns.Count == 0)
		{
			return;
		}
		double num = ((budget != null) ? budget.BeginQueue() : DeferredStreamingFrameBudget.CurrentTimeMilliseconds());
		int num2 = 0;
		int count = this._pendingZoneSpawns.Count;
		try
		{
			while (this._pendingZoneSpawns.Count > 0 && num2 < this.effMaxZoneSpawnsPerFrame && count-- > 0)
			{
				if ((budget != null) ? (!budget.CanStartOperation(num, (double)this.effZoneSpawnFrameBudgetMs)) : (DeferredStreamingFrameBudget.CurrentTimeMilliseconds() - num >= (double)this.effZoneSpawnFrameBudgetMs))
				{
					break;
				}
				ChunkManager.PendingZoneSpawn pendingZoneSpawn;
				if (!this.TryTakeHighestPriorityPendingZoneSpawn(out pendingZoneSpawn))
				{
					break;
				}
				bool flag = this.SpawnSingleZone(pendingZoneSpawn.chunk, pendingZoneSpawn.slotIndex, pendingZoneSpawn.entry);
				num2++;
				int num3;
				if (!flag)
				{
					if (!pendingZoneSpawn.hasDeferred)
					{
						pendingZoneSpawn.hasDeferred = true;
						int runtimeZonePoolDeferrals = this.RuntimeZonePoolDeferrals;
						this.RuntimeZonePoolDeferrals = runtimeZonePoolDeferrals + 1;
					}
					pendingZoneSpawn.lastPoolRevisionTried = this._zonePoolRevision;
					this._pendingZoneSpawns.Enqueue(pendingZoneSpawn);
				}
				else if (pendingZoneSpawn.chunk != null && this._pendingZoneCountByChunk.TryGetValue(pendingZoneSpawn.chunk, out num3))
				{
					num3--;
					if (num3 <= 0)
					{
						this._pendingZoneCountByChunk.Remove(pendingZoneSpawn.chunk);
						this.TryNotifyChunkZonesReady(pendingZoneSpawn.chunk);
					}
					else
					{
						this._pendingZoneCountByChunk[pendingZoneSpawn.chunk] = num3;
					}
				}
			}
		}
		finally
		{
			if (budget != null)
			{
				budget.EndQueue(num);
			}
		}
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x00010630 File Offset: 0x0000E830
	private void ProcessSeededPoiPoolReclamation()
	{
		Vector2Int vector2Int;
		if (!this._fullGameplayReady || !this.enableZonePooling || !this.TryGetCurrentStreamingPlayerCell(out vector2Int))
		{
			return;
		}
		foreach (ChunkManager.PendingZoneSpawn pendingZoneSpawn in this._pendingZoneSpawns)
		{
			if (pendingZoneSpawn.hasDeferred && this.IsActiveChunkInstance(pendingZoneSpawn.chunk) && this.GetChunkStreamingDistance(pendingZoneSpawn.chunk, vector2Int) <= 1 && ChunkManager.PendingSpawnSelectsPoiSlot(pendingZoneSpawn))
			{
				int[] zoneIndices = pendingZoneSpawn.entry.zoneIndices;
				int num = ((zoneIndices != null) ? zoneIndices[pendingZoneSpawn.slotIndex] : (-1));
				if (num >= 0 && num < pendingZoneSpawn.chunk.chunkData.availableZones.Count)
				{
					GameObject gameObject = this.ResolveZonePrefab(pendingZoneSpawn.chunk.chunkData.availableZones[num]);
					if (!(gameObject == null) && this.GetLiveZonePoolCount(gameObject) <= 0 && this.GetRetiringZoneCount(gameObject) <= 0 && !ChunkManager.IsNeverPooledHeavyZone(gameObject))
					{
						foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._spawnedChunks)
						{
							ChunkInstance value = keyValuePair.Value;
							ChunkSpawnResolver.SpawnEntry spawnEntry;
							if (value != pendingZoneSpawn.chunk && this.IsActiveChunkInstance(value) && this.GetChunkStreamingDistance(value, vector2Int) > 1 && !this._travelOffsetPinnedSpecialOrigins.Contains(keyValuePair.Key) && this._committedSpawnPlan.TryGetValue(keyValuePair.Key, out spawnEntry) && spawnEntry.zoneIndices != null)
							{
								for (int i = 0; i < spawnEntry.zoneIndices.Length; i++)
								{
									int num2 = spawnEntry.zoneIndices[i];
									if (num2 >= 0 && num2 < value.chunkData.availableZones.Count)
									{
										ZoneData zoneData = value.chunkData.availableZones[num2];
										if (!(this.ResolveZonePrefab(zoneData) != gameObject) && ProceduralZonePolicy.IsSpecialZone(zoneData))
										{
											for (int j = 0; j < value.spawnedZones.Count; j++)
											{
												GameObject gameObject2 = value.spawnedZones[j];
												GameObject gameObject3;
												if (!(gameObject2 == null) && !gameObject2.activeInHierarchy && !this._retiringZoneRoots.Contains(gameObject2) && !(gameObject2.name != "Zone_" + zoneData.name) && this._zoneInstanceToPrefabMap.TryGetValue(gameObject2, out gameObject3) && !(gameObject3 != gameObject))
												{
													this.DetachSeededPoiPayload(value, gameObject2, i, spawnEntry);
													return;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x0001094C File Offset: 0x0000EB4C
	private void DetachSeededPoiPayload(ChunkInstance chunk, GameObject root, int slot, ChunkSpawnResolver.SpawnEntry entry)
	{
		this.RestoreZoneAuthoredVisualDefaults(chunk.originCell, root.transform);
		int num = 0;
		HeavyZoneStreamingProxy component = root.GetComponent<HeavyZoneStreamingProxy>();
		if (component != null)
		{
			component.SetStreamingUrgency(false);
			component.OperationalReady -= this.HandleHeavyZoneOperationalReady;
			component.PayloadFailed -= this.HandleHeavyZoneFailed;
			if (this._heavyZoneOwners.Remove(component))
			{
				num++;
			}
			this._heavyZonePlaceholders.Remove(component);
			List<HeavyZoneStreamingProxy> list;
			if (this._stagedHeavyColliderOwners.TryGetValue(chunk, out list))
			{
				list.Remove(component);
				if (list.Count == 0)
				{
					this._stagedHeavyColliderOwners.Remove(chunk);
				}
			}
		}
		IncrementalPooledZoneActivation component2 = root.GetComponent<IncrementalPooledZoneActivation>();
		if (component2 != null)
		{
			component2.OperationalReady -= this.HandleIncrementalNormalZoneOperationalReady;
			if (this._incrementalNormalZoneOwners.Remove(component2))
			{
				num++;
			}
			this._incrementalZonePlaceholders.Remove(component2);
			this.CancelIncrementalNormalZoneWake(component2);
		}
		int num2;
		if (num > 0 && this._pendingHeavyZoneCountByChunk.TryGetValue(chunk, out num2))
		{
			if (num2 <= num)
			{
				this._pendingHeavyZoneCountByChunk.Remove(chunk);
			}
			else
			{
				this._pendingHeavyZoneCountByChunk[chunk] = num2 - num;
			}
		}
		this.InitializeChunkPool();
		chunk.spawnedZones.Remove(root);
		this.ReleaseZoneRootInternal(root, false, true);
		chunk.cachedColliders = null;
		chunk.collidersCacheDirty = true;
		List<Transform> orCacheBaseTransforms = this.GetOrCacheBaseTransforms(chunk.chunkRoot);
		Transform transform = ((slot < orCacheBaseTransforms.Count) ? orCacheBaseTransforms[slot] : chunk.chunkRoot.transform);
		ChunkManager.RestoreZonePlaceholder(this.GetOrCaptureZonePlaceholderState(transform, chunk.chunkRoot.transform));
		this.InvalidateChunkVisualCache(chunk.originCell);
		this.CancelPendingChunkZonesReady(chunk);
		this._zonesReadyNotifiedChunks.Remove(chunk);
		int num3;
		this._pendingZoneCountByChunk.TryGetValue(chunk, out num3);
		this._pendingZoneCountByChunk[chunk] = num3 + 1;
		this._pendingZoneSpawns.Enqueue(new ChunkManager.PendingZoneSpawn
		{
			chunk = chunk,
			slotIndex = slot,
			entry = entry,
			lastPoolRevisionTried = -1,
			hasDeferred = false
		});
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x00010B64 File Offset: 0x0000ED64
	private void ProcessSeededNormalZonePoolReplenishment()
	{
		if (!this._fullGameplayReady || !this.enableZonePooling || !this.forbidRuntimeZoneInstantiate || this._pendingZoneSpawns.Count == 0)
		{
			return;
		}
		using (ChunkManager.SeededZonePoolReplenishmentMarker.Auto())
		{
			Vector2Int vector2Int;
			bool flag = this.TryGetCurrentStreamingPlayerCell(out vector2Int);
			foreach (ChunkManager.PendingZoneSpawn pendingZoneSpawn in this._pendingZoneSpawns)
			{
				int num;
				int num2;
				int num3;
				if (pendingZoneSpawn.hasDeferred && this.IsActiveChunkInstance(pendingZoneSpawn.chunk) && (!flag || this.GetPendingZoneSpawnPriority(pendingZoneSpawn, vector2Int, out num, out num2, out num3)))
				{
					int[] zoneIndices = pendingZoneSpawn.entry.zoneIndices;
					ChunkData chunkData = pendingZoneSpawn.chunk.chunkData;
					List<ZoneData> list = ((chunkData != null) ? chunkData.availableZones : null);
					if (zoneIndices != null && list != null && pendingZoneSpawn.slotIndex >= 0 && pendingZoneSpawn.slotIndex < zoneIndices.Length)
					{
						int num4 = zoneIndices[pendingZoneSpawn.slotIndex];
						if (num4 >= 0 && num4 < list.Count)
						{
							ZoneData zoneData = list[num4];
							GameObject gameObject = ((zoneData != null) ? zoneData.zonePrefab : null);
							if (!(gameObject == null) && !ProceduralZonePolicy.IsSpecialZone(zoneData) && !(gameObject.GetComponent<HeavyZoneStreamingProxy>() != null) && this.GetLiveZonePoolCount(gameObject) <= 0 && this.GetRetiringZoneCount(gameObject) <= 0)
							{
								ChunkTemplate template = pendingZoneSpawn.chunk.chunkData.template;
								List<ZoneSlot> list2 = ((template != null) ? template.zoneSlots : null);
								ZoneData zoneData2;
								if (RunModeService.HasSeededFreeWorld || list2 == null || pendingZoneSpawn.slotIndex >= list2.Count || !this.TrySelectPooledZoneFallback(pendingZoneSpawn.chunk, list2[pendingZoneSpawn.slotIndex], zoneData, out zoneData2))
								{
									this.InitializeChunkPool();
									if (this._zonePoolCreationGate == null)
									{
										GameObject gameObject2 = new GameObject("ZonePoolCreationGate");
										gameObject2.SetActive(false);
										gameObject2.transform.SetParent(this._zonePoolContainer, false);
										this._zonePoolCreationGate = gameObject2.transform;
									}
									GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject, new Vector3(0f, -5000f, 0f), Quaternion.identity, this._zonePoolCreationGate);
									this.RegisterPooledZoneInstance(gameObject3, gameObject);
									IncrementalPooledZoneActivation incrementalPooledZoneActivation = this.PrepareIncrementalNormalZoneActivation(gameObject3);
									gameObject3.SetActive(false);
									if (incrementalPooledZoneActivation != null)
									{
										incrementalPooledZoneActivation.PrepareDormancyWhileRootInactive();
									}
									gameObject3.transform.SetParent(this._zonePoolContainer, true);
									this.ReturnZoneInstanceToPool(gameObject3, gameObject);
									num3 = this.TotalZoneInstantiates;
									this.TotalZoneInstantiates = num3 + 1;
									num3 = this.RuntimeZoneColdInstantiates;
									this.RuntimeZoneColdInstantiates = num3 + 1;
									break;
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x00010E6C File Offset: 0x0000F06C
	private bool TryTakeHighestPriorityPendingZoneSpawn(out ChunkManager.PendingZoneSpawn selected)
	{
		selected = default(ChunkManager.PendingZoneSpawn);
		int count = this._pendingZoneSpawns.Count;
		if (count == 0)
		{
			return false;
		}
		Vector2Int vector2Int;
		bool flag = this.TryGetCurrentStreamingPlayerCell(out vector2Int);
		bool flag2 = false;
		int num = int.MaxValue;
		int num2 = int.MaxValue;
		int num3 = int.MaxValue;
		for (int i = 0; i < count; i++)
		{
			ChunkManager.PendingZoneSpawn pendingZoneSpawn = this._pendingZoneSpawns.Dequeue();
			if (!this.IsActiveChunkInstance(pendingZoneSpawn.chunk))
			{
				this.DiscardPendingZoneSpawn(pendingZoneSpawn.chunk);
			}
			else if (pendingZoneSpawn.lastPoolRevisionTried == this._zonePoolRevision)
			{
				this._pendingZoneSpawns.Enqueue(pendingZoneSpawn);
			}
			else
			{
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				if (flag && !this.GetPendingZoneSpawnPriority(pendingZoneSpawn, vector2Int, out num4, out num5, out num6))
				{
					this._pendingZoneSpawns.Enqueue(pendingZoneSpawn);
				}
				else if (flag2 && num4 >= num && (num4 != num || num5 >= num2) && (num4 != num || num5 != num2 || num6 >= num3) && (num4 != num || num5 != num2 || num6 != num3 || (pendingZoneSpawn.chunk.originCell.x >= selected.chunk.originCell.x && (pendingZoneSpawn.chunk.originCell.x != selected.chunk.originCell.x || (pendingZoneSpawn.chunk.originCell.y >= selected.chunk.originCell.y && (pendingZoneSpawn.chunk.originCell.y != selected.chunk.originCell.y || pendingZoneSpawn.slotIndex >= selected.slotIndex))))))
				{
					this._pendingZoneSpawns.Enqueue(pendingZoneSpawn);
				}
				else
				{
					if (flag2)
					{
						this._pendingZoneSpawns.Enqueue(selected);
					}
					selected = pendingZoneSpawn;
					flag2 = true;
					num = num4;
					num2 = num5;
					num3 = num6;
				}
			}
		}
		return flag2;
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x00011080 File Offset: 0x0000F280
	private bool GetPendingZoneSpawnPriority(ChunkManager.PendingZoneSpawn pending, Vector2Int playerCell, out int distance, out int directionTier, out int distanceSqr)
	{
		ChunkInstance chunk = pending.chunk;
		ChunkData chunkData = chunk.chunkData;
		Vector2Int vector2Int = ((chunkData != null) ? chunkData.gridSize : Vector2Int.one);
		distance = ChunkManager.ChebyshevStepsToChunk(chunk.originCell, vector2Int, playerCell);
		bool flag = ChunkManager.PendingSpawnSelectsPoiSlot(pending);
		bool flag2 = ChunkManager.SelectionContainsSpecialZone(pending.entry, chunk.chunkData) || vector2Int.x > 1 || vector2Int.y > 1;
		int num = Mathf.Max(1, this.minVisibleRadius);
		if (this._fullGameplayReady && flag && distance > 1)
		{
			directionTier = int.MaxValue;
			distanceSqr = int.MaxValue;
			return false;
		}
		int num2 = ((!this._fullGameplayReady) ? Mathf.Max(num, this.spawnRadius) : (num + (flag2 ? 1 : 0)));
		if (distance > num2)
		{
			directionTier = int.MaxValue;
			distanceSqr = int.MaxValue;
			return false;
		}
		int num3 = chunk.originCell.x - playerCell.x;
		int num4 = chunk.originCell.y - playerCell.y;
		int num5 = num3 * this._streamingTravelDirection.x + num4 * this._streamingTravelDirection.y;
		directionTier = ((num3 == 0 && num4 == 0) ? 0 : ((this._streamingTravelDirection == Vector2Int.zero || num5 > 0) ? 1 : ((num5 == 0) ? 2 : 3)));
		distanceSqr = num3 * num3 + num4 * num4;
		return true;
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x000111E0 File Offset: 0x0000F3E0
	private static bool PendingSpawnSelectsPoiSlot(ChunkManager.PendingZoneSpawn pending)
	{
		ChunkInstance chunk = pending.chunk;
		ChunkData chunkData = ((chunk != null) ? chunk.chunkData : null);
		if (((chunkData != null) ? chunkData.availableZones : null) == null)
		{
			return false;
		}
		int[] zoneIndices = pending.entry.zoneIndices;
		if (zoneIndices != null && pending.slotIndex >= 0 && pending.slotIndex < zoneIndices.Length)
		{
			int num = zoneIndices[pending.slotIndex];
			return num >= 0 && num < chunkData.availableZones.Count && ProceduralZonePolicy.IsSpecialZone(chunkData.availableZones[num]);
		}
		return zoneIndices == null && ProceduralZonePolicy.IsAlwaysSpecialChunk(chunkData);
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x00011270 File Offset: 0x0000F470
	private void GetOrBuildPrewarmTargets(out Dictionary<GameObject, int> chunkTargets, out Dictionary<GameObject, int> zoneTargets)
	{
		if (this.enablePersistentPrewarmTargetCache && this.TryLoadPrewarmTargetsFromCache(out chunkTargets, out zoneTargets))
		{
			this.EnsureConnectingSubwayZoneTargets(zoneTargets);
			return;
		}
		chunkTargets = this.BuildPrewarmTargets();
		zoneTargets = this.BuildZonePrewarmTargets();
		this.EnsureConnectingSubwayZoneTargets(zoneTargets);
		if (this.enablePersistentPrewarmTargetCache)
		{
			this.SavePrewarmTargetsToCache(chunkTargets, zoneTargets);
		}
	}

	// Token: 0x060002FA RID: 762 RVA: 0x000112C4 File Offset: 0x0000F4C4
	private bool TryLoadPrewarmTargetsFromCache(out Dictionary<GameObject, int> chunkTargets, out Dictionary<GameObject, int> zoneTargets)
	{
		chunkTargets = new Dictionary<GameObject, int>();
		zoneTargets = new Dictionary<GameObject, int>();
		if (this.forceRebuildPrewarmTargetCache)
		{
			return false;
		}
		string prewarmCacheFilePath = this.GetPrewarmCacheFilePath();
		if (!File.Exists(prewarmCacheFilePath))
		{
			return false;
		}
		bool flag;
		try
		{
			string text = File.ReadAllText(prewarmCacheFilePath);
			if (string.IsNullOrWhiteSpace(text))
			{
				flag = false;
			}
			else
			{
				ChunkManager.MapPrewarmTargetCacheData mapPrewarmTargetCacheData = JsonUtility.FromJson<ChunkManager.MapPrewarmTargetCacheData>(text);
				if (mapPrewarmTargetCacheData == null)
				{
					flag = false;
				}
				else if (mapPrewarmTargetCacheData.schemaVersion != this.prewarmTargetCacheSchemaVersion)
				{
					flag = false;
				}
				else if (!string.Equals(mapPrewarmTargetCacheData.buildVersion, Application.version, StringComparison.Ordinal))
				{
					flag = false;
				}
				else
				{
					string text2 = this.ComputePrewarmInputHash();
					if (!string.Equals(mapPrewarmTargetCacheData.inputHash, text2, StringComparison.Ordinal))
					{
						flag = false;
					}
					else
					{
						Dictionary<string, GameObject> dictionary = this.BuildChunkPrewarmKeyMap();
						for (int i = 0; i < mapPrewarmTargetCacheData.chunkTargets.Count; i++)
						{
							ChunkManager.PrewarmTargetEntry prewarmTargetEntry = mapPrewarmTargetCacheData.chunkTargets[i];
							GameObject gameObject;
							if (prewarmTargetEntry != null && !string.IsNullOrEmpty(prewarmTargetEntry.key) && prewarmTargetEntry.count > 0 && dictionary.TryGetValue(prewarmTargetEntry.key, out gameObject) && gameObject != null)
							{
								int num;
								chunkTargets[gameObject] = Mathf.Max(chunkTargets.TryGetValue(gameObject, out num) ? num : 0, prewarmTargetEntry.count);
							}
						}
						Dictionary<string, GameObject> dictionary2 = this.BuildZonePrewarmKeyMap();
						for (int j = 0; j < mapPrewarmTargetCacheData.zoneTargets.Count; j++)
						{
							ChunkManager.PrewarmTargetEntry prewarmTargetEntry2 = mapPrewarmTargetCacheData.zoneTargets[j];
							GameObject gameObject2;
							if (prewarmTargetEntry2 != null && !string.IsNullOrEmpty(prewarmTargetEntry2.key) && prewarmTargetEntry2.count > 0 && dictionary2.TryGetValue(prewarmTargetEntry2.key, out gameObject2) && gameObject2 != null)
							{
								int num2;
								zoneTargets[gameObject2] = Mathf.Max(zoneTargets.TryGetValue(gameObject2, out num2) ? num2 : 0, prewarmTargetEntry2.count);
							}
						}
						bool flag2 = !this.enableChunkPooling || !this.prewarmPoolOnStart || this.initialPrewarmCount <= 0 || chunkTargets.Count > 0;
						bool flag3 = !this.enableZonePooling || !this.prewarmZonePoolOnStart || zoneTargets.Count > 0;
						if (!flag2 || !flag3)
						{
							flag = false;
						}
						else
						{
							if (this.enableDebugLogs)
							{
								Debug.Log(string.Format("[ChunkManager] Prewarm target cache HIT ({0}) - ChunkTargets: {1}, ZoneTargets: {2}", prewarmCacheFilePath, chunkTargets.Count, zoneTargets.Count));
							}
							flag = true;
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			if (this.enableDebugLogs)
			{
				Debug.LogWarning("[ChunkManager] Errore lettura prewarm cache: " + ex.Message + ". Fallback a rebuild.");
			}
			flag = false;
		}
		return flag;
	}

	// Token: 0x060002FB RID: 763 RVA: 0x00011568 File Offset: 0x0000F768
	private void SavePrewarmTargetsToCache(Dictionary<GameObject, int> chunkTargets, Dictionary<GameObject, int> zoneTargets)
	{
		try
		{
			ChunkManager.MapPrewarmTargetCacheData mapPrewarmTargetCacheData = new ChunkManager.MapPrewarmTargetCacheData
			{
				schemaVersion = this.prewarmTargetCacheSchemaVersion,
				buildVersion = Application.version,
				inputHash = this.ComputePrewarmInputHash()
			};
			Dictionary<GameObject, string> dictionary = this.BuildChunkPrefabToPrewarmKeyMap();
			foreach (KeyValuePair<GameObject, int> keyValuePair in chunkTargets)
			{
				string text;
				if (!(keyValuePair.Key == null) && keyValuePair.Value > 0 && dictionary.TryGetValue(keyValuePair.Key, out text) && !string.IsNullOrEmpty(text))
				{
					mapPrewarmTargetCacheData.chunkTargets.Add(new ChunkManager.PrewarmTargetEntry
					{
						key = text,
						count = keyValuePair.Value
					});
				}
			}
			Dictionary<GameObject, string> dictionary2 = this.BuildZonePrefabToPrewarmKeyMap();
			foreach (KeyValuePair<GameObject, int> keyValuePair2 in zoneTargets)
			{
				string text2;
				if (!(keyValuePair2.Key == null) && keyValuePair2.Value > 0 && dictionary2.TryGetValue(keyValuePair2.Key, out text2) && !string.IsNullOrEmpty(text2))
				{
					mapPrewarmTargetCacheData.zoneTargets.Add(new ChunkManager.PrewarmTargetEntry
					{
						key = text2,
						count = keyValuePair2.Value
					});
				}
			}
			string text3 = Path.Combine(Application.persistentDataPath, "cache/map");
			if (!Directory.Exists(text3))
			{
				Directory.CreateDirectory(text3);
			}
			string prewarmCacheFilePath = this.GetPrewarmCacheFilePath();
			string text4 = prewarmCacheFilePath + ".tmp";
			string text5 = JsonUtility.ToJson(mapPrewarmTargetCacheData, false);
			File.WriteAllText(text4, text5);
			if (File.Exists(prewarmCacheFilePath))
			{
				File.Delete(prewarmCacheFilePath);
			}
			File.Move(text4, prewarmCacheFilePath);
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[ChunkManager] Prewarm target cache SAVED ({0}) - ChunkTargets: {1}, ZoneTargets: {2}", prewarmCacheFilePath, mapPrewarmTargetCacheData.chunkTargets.Count, mapPrewarmTargetCacheData.zoneTargets.Count));
			}
		}
		catch (Exception ex)
		{
			if (this.enableDebugLogs)
			{
				Debug.LogWarning("[ChunkManager] Errore salvataggio prewarm cache: " + ex.Message);
			}
		}
	}

	// Token: 0x060002FC RID: 764 RVA: 0x000117B8 File Offset: 0x0000F9B8
	private string GetPrewarmCacheFilePath()
	{
		return Path.Combine(Application.persistentDataPath, "cache/map", "prewarm_targets_v1.json");
	}

	// Token: 0x060002FD RID: 765 RVA: 0x000117D0 File Offset: 0x0000F9D0
	private string BuildChunkPrewarmKey(ChunkData chunk)
	{
		string text = ((chunk != null) ? chunk.name : "null");
		string text2 = ((((chunk != null) ? chunk.template : null) != null) ? chunk.template.name : "null");
		Object @object;
		if (chunk == null)
		{
			@object = null;
		}
		else
		{
			ChunkTemplate template = chunk.template;
			@object = ((template != null) ? template.chunkPrefab : null);
		}
		string text3 = ((@object != null) ? chunk.template.chunkPrefab.name : "null");
		return string.Concat(new string[] { text, "|", text2, "|", text3 });
	}

	// Token: 0x060002FE RID: 766 RVA: 0x0001187C File Offset: 0x0000FA7C
	private string BuildZonePrewarmKey(ZoneData zone)
	{
		string text = ((zone != null) ? zone.name : "null");
		GameObject gameObject = this.ResolveZonePrefab(zone);
		string text2 = ((gameObject != null) ? gameObject.name : "null");
		return text + "|" + text2;
	}

	// Token: 0x060002FF RID: 767 RVA: 0x000118CC File Offset: 0x0000FACC
	private Dictionary<GameObject, string> BuildChunkPrefabToPrewarmKeyMap()
	{
		Dictionary<GameObject, string> dictionary = new Dictionary<GameObject, string>();
		if (this.availableChunks == null)
		{
			return dictionary;
		}
		for (int i = 0; i < this.availableChunks.Length; i++)
		{
			ChunkData chunkData = this.availableChunks[i];
			GameObject gameObject;
			if (chunkData == null)
			{
				gameObject = null;
			}
			else
			{
				ChunkTemplate template = chunkData.template;
				gameObject = ((template != null) ? template.chunkPrefab : null);
			}
			GameObject gameObject2 = gameObject;
			if (!(gameObject2 == null))
			{
				dictionary[gameObject2] = this.BuildChunkPrewarmKey(chunkData);
			}
		}
		return dictionary;
	}

	// Token: 0x06000300 RID: 768 RVA: 0x00011938 File Offset: 0x0000FB38
	private Dictionary<string, GameObject> BuildChunkPrewarmKeyMap()
	{
		Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
		if (this.availableChunks == null)
		{
			return dictionary;
		}
		for (int i = 0; i < this.availableChunks.Length; i++)
		{
			ChunkData chunkData = this.availableChunks[i];
			GameObject gameObject;
			if (chunkData == null)
			{
				gameObject = null;
			}
			else
			{
				ChunkTemplate template = chunkData.template;
				gameObject = ((template != null) ? template.chunkPrefab : null);
			}
			GameObject gameObject2 = gameObject;
			if (!(gameObject2 == null))
			{
				dictionary[this.BuildChunkPrewarmKey(chunkData)] = gameObject2;
			}
		}
		return dictionary;
	}

	// Token: 0x06000301 RID: 769 RVA: 0x000119A4 File Offset: 0x0000FBA4
	private Dictionary<GameObject, string> BuildZonePrefabToPrewarmKeyMap()
	{
		Dictionary<GameObject, string> dictionary = new Dictionary<GameObject, string>();
		if (this.availableChunks == null)
		{
			return dictionary;
		}
		for (int i = 0; i < this.availableChunks.Length; i++)
		{
			ChunkData chunkData = this.availableChunks[i];
			if (((chunkData != null) ? chunkData.availableZones : null) != null)
			{
				for (int j = 0; j < chunkData.availableZones.Count; j++)
				{
					ZoneData zoneData = chunkData.availableZones[j];
					GameObject gameObject = this.ResolveZonePrefab(zoneData);
					if (!(gameObject == null))
					{
						dictionary[gameObject] = this.BuildZonePrewarmKey(zoneData);
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06000302 RID: 770 RVA: 0x00011A34 File Offset: 0x0000FC34
	private Dictionary<string, GameObject> BuildZonePrewarmKeyMap()
	{
		Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
		if (this.availableChunks == null)
		{
			return dictionary;
		}
		for (int i = 0; i < this.availableChunks.Length; i++)
		{
			ChunkData chunkData = this.availableChunks[i];
			if (((chunkData != null) ? chunkData.availableZones : null) != null)
			{
				for (int j = 0; j < chunkData.availableZones.Count; j++)
				{
					ZoneData zoneData = chunkData.availableZones[j];
					GameObject gameObject = this.ResolveZonePrefab(zoneData);
					if (!(gameObject == null))
					{
						dictionary[this.BuildZonePrewarmKey(zoneData)] = gameObject;
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06000303 RID: 771 RVA: 0x00011AC4 File Offset: 0x0000FCC4
	private string ComputePrewarmInputHash()
	{
		StringBuilder stringBuilder = new StringBuilder(2048);
		stringBuilder.Append("schema=").Append(this.prewarmTargetCacheSchemaVersion).Append('|');
		stringBuilder.Append("zonePrewarmPlanner=4|");
		stringBuilder.Append("poiPrefabRouting=1:").Append(this.ZonePrefabResolver.Mode).Append('|');
		stringBuilder.Append("cellSize=").Append(this.cellSize).Append('|');
		stringBuilder.Append("spawnRadius=").Append(this.spawnRadius).Append('|');
		stringBuilder.Append("mapCatalog=").Append(this.ActiveMapCatalogResource).Append('|');
		stringBuilder.Append("maxVisibleRadius=").Append(this.maxVisibleRadius).Append('|');
		stringBuilder.Append("runtimeDespawnRadius=").Append(this.runtimeDespawnRadius).Append('|');
		stringBuilder.Append("enableRuntimeChunkDespawn=").Append(this.enableRuntimeChunkDespawn ? 1 : 0).Append('|');
		stringBuilder.Append("initialPrewarmCount=").Append(this.initialPrewarmCount).Append('|');
		stringBuilder.Append("minPrewarmPerPrefab=").Append(this.minPrewarmPerPrefab).Append('|');
		stringBuilder.Append("autoSizeChunkPrewarmFromCoverage=").Append(this.autoSizeChunkPrewarmFromCoverage ? 1 : 0).Append('|');
		stringBuilder.Append("useRuntimeDespawnRadiusForPoolCoverage=").Append(this.useRuntimeDespawnRadiusForPoolCoverage ? 1 : 0).Append('|');
		stringBuilder.Append("chunkPoolCoverageMultiplier=").Append(this.chunkPoolCoverageMultiplier).Append('|');
		stringBuilder.Append("maxAutoChunkPrewarmTarget=").Append(this.maxAutoChunkPrewarmTarget).Append('|');
		stringBuilder.Append("initialZonePrewarmCount=").Append(this.initialZonePrewarmCount).Append('|');
		stringBuilder.Append("useDynamicZonePoolTarget=").Append(this.useDynamicZonePoolTarget ? 1 : 0).Append('|');
		stringBuilder.Append("zonePoolCoverageMultiplier=").Append(this.zonePoolCoverageMultiplier).Append('|');
		stringBuilder.Append("maxAutoZonePrewarmTarget=").Append(this.maxAutoZonePrewarmTarget).Append('|');
		stringBuilder.Append("maxPooledZonesPerPrefab=").Append(this.maxPooledZonesPerPrefab).Append('|');
		stringBuilder.Append("minZonePrewarmPerPrefab=").Append(this.minZonePrewarmPerPrefab).Append('|');
		if (this.availableChunks != null)
		{
			for (int i = 0; i < this.availableChunks.Length; i++)
			{
				ChunkData chunkData = this.availableChunks[i];
				if (chunkData == null)
				{
					stringBuilder.Append("chunk:null|");
				}
				else
				{
					stringBuilder.Append("chunk=").Append(chunkData.name).Append('|');
					stringBuilder.Append("chunkWeight=").Append(chunkData.spawnWeight).Append('|');
					stringBuilder.Append("containsSubway=").Append(chunkData.containsSubway ? 1 : 0).Append('|');
					stringBuilder.Append("grid=").Append(chunkData.gridSize.x).Append('x')
						.Append(chunkData.gridSize.y)
						.Append('|');
					stringBuilder.Append("template=").Append((chunkData.template != null) ? chunkData.template.name : "null").Append('|');
					StringBuilder stringBuilder2 = stringBuilder.Append("prefab=");
					ChunkTemplate template = chunkData.template;
					stringBuilder2.Append((((template != null) ? template.chunkPrefab : null) != null) ? chunkData.template.chunkPrefab.name : "null").Append('|');
					ChunkTemplate template2 = chunkData.template;
					int num = ((((template2 != null) ? template2.zoneSlots : null) != null) ? chunkData.template.zoneSlots.Count : 0);
					stringBuilder.Append("slotCount=").Append(num).Append('|');
					ChunkTemplate template3 = chunkData.template;
					if (((template3 != null) ? template3.zoneSlots : null) != null)
					{
						for (int j = 0; j < chunkData.template.zoneSlots.Count; j++)
						{
							ZoneSlot zoneSlot = chunkData.template.zoneSlots[j];
							if (zoneSlot == null)
							{
								stringBuilder.Append("slot:null|");
							}
							else
							{
								stringBuilder.Append("slotKey=").Append((int)zoneSlot.parentChunkSize).Append(':')
									.Append((int)zoneSlot.occupancy)
									.Append('|');
							}
						}
					}
					if (chunkData.availableZones != null)
					{
						for (int k = 0; k < chunkData.availableZones.Count; k++)
						{
							ZoneData zoneData = chunkData.availableZones[k];
							stringBuilder.Append("zone=").Append((zoneData != null) ? zoneData.name : "null").Append('|');
							stringBuilder.Append("zoneWeight=").Append((zoneData != null) ? zoneData.spawnWeight : 0f).Append('|');
							GameObject gameObject = this.ResolveZonePrefab(zoneData);
							stringBuilder.Append("zonePrefab=").Append((gameObject != null) ? gameObject.name : "null").Append('|');
							stringBuilder.Append("zoneLabel=").Append((zoneData != null) ? zoneData.zoneName : "null").Append('|');
							stringBuilder.Append("zoneKey=").Append((int)((zoneData != null) ? zoneData.targetChunkSize : ((ChunkSize)(-1)))).Append(':')
								.Append((int)((zoneData != null) ? zoneData.occupancy : ((ZoneOccupancy)(-1))))
								.Append('|');
							stringBuilder.Append("zoneExcluded=").Append(ProceduralZonePolicy.IsExcludedZone(zoneData) ? 1 : 0).Append('|');
							stringBuilder.Append("zoneSpecial=").Append(ProceduralZonePolicy.IsSpecialZone(zoneData) ? 1 : 0).Append('|');
							HeavyZoneStreamingProxy heavyZoneStreamingProxy = ((gameObject != null) ? gameObject.GetComponent<HeavyZoneStreamingProxy>() : null);
							stringBuilder.Append("zoneHeavy=").Append((heavyZoneStreamingProxy != null) ? 1 : 0).Append('|');
							stringBuilder.Append("zoneNeverPool=").Append((heavyZoneStreamingProxy != null && heavyZoneStreamingProxy.NeverPool) ? 1 : 0).Append('|');
						}
					}
				}
			}
		}
		if (this.biomeManager != null && this.biomeManager.biomes != null)
		{
			foreach (BiomeConfig biomeConfig in this.biomeManager.biomes)
			{
				if (biomeConfig == null)
				{
					stringBuilder.Append("biome:null|");
				}
				else
				{
					stringBuilder.Append("biome=").Append(biomeConfig.biomeName).Append('|');
					stringBuilder.Append("biomeWeight=").Append(biomeConfig.baseSpawnWeight).Append('|');
					stringBuilder.Append("biomeFallback=").Append((biomeConfig.fallbackBiome != null) ? biomeConfig.fallbackBiome.biomeName : "null").Append('|');
					if (biomeConfig.availableZones != null)
					{
						foreach (ZoneData zoneData2 in biomeConfig.availableZones)
						{
							stringBuilder.Append("bz=").Append(this.BuildZonePrewarmKey(zoneData2)).Append(':')
								.Append((int)((zoneData2 != null) ? zoneData2.targetChunkSize : ((ChunkSize)(-1))))
								.Append(':')
								.Append((int)((zoneData2 != null) ? zoneData2.occupancy : ((ZoneOccupancy)(-1))))
								.Append(':')
								.Append((zoneData2 != null) ? zoneData2.spawnWeight : 0f)
								.Append('|');
						}
					}
					if (biomeConfig.chunkWeightOverrides != null)
					{
						foreach (BiomeConfig.ChunkWeightOverride chunkWeightOverride in biomeConfig.chunkWeightOverrides)
						{
							stringBuilder.Append("bo=").Append((((chunkWeightOverride != null) ? chunkWeightOverride.chunk : null) != null) ? chunkWeightOverride.chunk.name : "null").Append(':')
								.Append((chunkWeightOverride != null) ? chunkWeightOverride.multiplier : 0f)
								.Append('|');
						}
					}
				}
			}
		}
		uint num2 = 2166136261U;
		string text = stringBuilder.ToString();
		for (int l = 0; l < text.Length; l++)
		{
			num2 ^= (uint)text[l];
			num2 *= 16777619U;
		}
		return num2.ToString("X8");
	}

	// Token: 0x06000304 RID: 772 RVA: 0x0001247C File Offset: 0x0001067C
	private Dictionary<GameObject, int> BuildZonePrewarmTargets()
	{
		Dictionary<GameObject, int> dictionary = new Dictionary<GameObject, int>();
		Dictionary<GameObject, float> dictionary2 = new Dictionary<GameObject, float>();
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		if (this.availableChunks == null || this.availableChunks.Length == 0)
		{
			return dictionary;
		}
		foreach (ChunkData chunkData in this.availableChunks)
		{
			if (!(((chunkData != null) ? chunkData.template : null) == null))
			{
				float num4 = Mathf.Max(0.0001f, chunkData.spawnWeight);
				int num5 = ((chunkData.template.zoneSlots != null) ? chunkData.template.zoneSlots.Count : 0);
				num2 += (float)num5 * num4;
				num3 += num4;
				if (num5 != 0)
				{
					for (int j = 0; j < chunkData.template.zoneSlots.Count; j++)
					{
						ZoneSlot zoneSlot = chunkData.template.zoneSlots[j];
						List<ZoneData> compatibleZones = chunkData.GetCompatibleZones(zoneSlot);
						if (compatibleZones != null && compatibleZones.Count != 0)
						{
							float num6 = 0f;
							for (int k = 0; k < compatibleZones.Count; k++)
							{
								ZoneData zoneData = compatibleZones[k];
								if (this.IsZoneEligibleForPrewarm(zoneData))
								{
									num6 += Mathf.Max(0.0001f, zoneData.spawnWeight);
								}
							}
							if (num6 > 0f)
							{
								for (int l = 0; l < compatibleZones.Count; l++)
								{
									ZoneData zoneData2 = compatibleZones[l];
									if (this.IsZoneEligibleForPrewarm(zoneData2))
									{
										GameObject gameObject = this.ResolveZonePrefab(zoneData2);
										float num7 = Mathf.Max(0.0001f, zoneData2.spawnWeight) / num6;
										float num8 = num4 * num7;
										if (!dictionary2.ContainsKey(gameObject))
										{
											dictionary2[gameObject] = 0f;
										}
										Dictionary<GameObject, float> dictionary3 = dictionary2;
										GameObject gameObject2 = gameObject;
										dictionary3[gameObject2] += num8;
										num += num8;
									}
								}
							}
						}
					}
				}
			}
		}
		if (dictionary2.Count == 0 || num <= 0f)
		{
			return dictionary;
		}
		float num9 = ((num3 > 0f) ? (num2 / num3) : 1f);
		int num10 = Mathf.CeilToInt((float)this.EstimatePoolCoverageChunkCount() * num9 * this.zonePoolCoverageMultiplier);
		num10 = Mathf.Clamp(num10, this.minZonePrewarmPerPrefab, Mathf.Max(this.minZonePrewarmPerPrefab, this.maxAutoZonePrewarmTarget));
		int num11 = (this.useDynamicZonePoolTarget ? num10 : Mathf.Max(0, this.initialZonePrewarmCount));
		foreach (KeyValuePair<GameObject, float> keyValuePair in dictionary2)
		{
			float num12 = keyValuePair.Value / num;
			int num13 = Mathf.RoundToInt((float)num11 * num12);
			int num14 = Mathf.Max(this.minZonePrewarmPerPrefab, this.maxPooledZonesPerPrefab);
			int num15 = Mathf.Clamp(Mathf.Max(this.minZonePrewarmPerPrefab, num13), this.minZonePrewarmPerPrefab, num14);
			dictionary[keyValuePair.Key] = num15;
		}
		this.BoostZoneTargetsForBiomes(dictionary);
		this.CapPoolableSpecialHeavyZoneTargets(dictionary);
		this.EnsureSpawnWindowZoneCoverageTargets(dictionary);
		this.EnsureExactRuntimeIdentityZoneTargets(dictionary);
		return dictionary;
	}

	// Token: 0x06000305 RID: 773 RVA: 0x000127B0 File Offset: 0x000109B0
	private void EnsureExactRuntimeIdentityZoneTargets(Dictionary<GameObject, int> targets)
	{
		if (targets == null || this.availableChunks == null || this.availableChunks.Length == 0)
		{
			return;
		}
		List<GameObject> list = new List<GameObject>(4);
		for (int i = 0; i < this.availableChunks.Length; i++)
		{
			ChunkData chunkData = this.availableChunks[i];
			List<ZoneData> list2 = ((chunkData != null) ? chunkData.availableZones : null);
			if (list2 != null)
			{
				for (int j = 0; j < list2.Count; j++)
				{
					ZoneData zoneData = list2[j];
					GameObject gameObject = this.ResolveZonePrefab(zoneData);
					if (this.IsZoneEligibleForPrewarm(zoneData) && !ProceduralZonePolicy.IsSpecialZone(zoneData) && ProceduralZonePolicy.RequiresExactRuntimeIdentity(zoneData) && !list.Contains(gameObject))
					{
						list.Add(gameObject);
					}
				}
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		list.Sort(new Comparison<GameObject>(ChunkManager.CompareZonePrefabNames));
		int num = Mathf.Max(1, this.minZonePrewarmPerPrefab);
		int num2 = Mathf.Min(Mathf.Max(num, this.maxPooledZonesPerPrefab), Mathf.Max(3, num));
		for (int k = 0; k < list.Count; k++)
		{
			GameObject gameObject2 = list[k];
			if (!targets.ContainsKey(gameObject2))
			{
				targets[gameObject2] = num;
			}
		}
		int num3 = 0;
		foreach (int num4 in targets.Values)
		{
			num3 += num4;
		}
		int num5 = Mathf.Max(targets.Count * num, this.maxAutoZonePrewarmTarget);
		for (int l = 0; l < list.Count; l++)
		{
			GameObject gameObject3 = list[l];
			int m = targets[gameObject3];
			while (m < num2)
			{
				if (num3 >= num5)
				{
					GameObject gameObject4 = null;
					int num6 = num;
					foreach (KeyValuePair<GameObject, int> keyValuePair in targets)
					{
						if (!(keyValuePair.Key == gameObject3) && !list.Contains(keyValuePair.Key) && keyValuePair.Value > num6)
						{
							gameObject4 = keyValuePair.Key;
							num6 = keyValuePair.Value;
						}
					}
					if (gameObject4 == null)
					{
						Debug.LogWarning("[ChunkManager] Exact zone reserve limit: prefab='" + gameObject3.name + "', " + string.Format("reserved={0}/{1}, globalCap={2}.", m, num2, num5));
						break;
					}
					targets[gameObject4] = num6 - 1;
					num3--;
				}
				m++;
				targets[gameObject3] = m;
				num3++;
			}
		}
	}

	// Token: 0x06000306 RID: 774 RVA: 0x00012A60 File Offset: 0x00010C60
	private bool IsActiveChunkInstance(ChunkInstance chunk)
	{
		ChunkInstance chunkInstance;
		return chunk != null && chunk.chunkRoot != null && this._spawnedChunks.TryGetValue(chunk.originCell, out chunkInstance) && chunkInstance == chunk;
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00012A9C File Offset: 0x00010C9C
	private void DiscardPendingZoneSpawn(ChunkInstance chunk)
	{
		int num;
		if (chunk == null || !this._pendingZoneCountByChunk.TryGetValue(chunk, out num))
		{
			return;
		}
		if (num <= 1)
		{
			this._pendingZoneCountByChunk.Remove(chunk);
			return;
		}
		this._pendingZoneCountByChunk[chunk] = num - 1;
	}

	// Token: 0x06000308 RID: 776 RVA: 0x00012AE0 File Offset: 0x00010CE0
	private void CapPoolableSpecialHeavyZoneTargets(Dictionary<GameObject, int> targets)
	{
		if (targets == null || targets.Count == 0 || this.availableChunks == null)
		{
			return;
		}
		HashSet<GameObject> hashSet = new HashSet<GameObject>();
		for (int i = 0; i < this.availableChunks.Length; i++)
		{
			ChunkData chunkData = this.availableChunks[i];
			List<ZoneData> list = ((chunkData != null) ? chunkData.availableZones : null);
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					ZoneData zoneData = list[j];
					GameObject gameObject = this.ResolveZonePrefab(zoneData);
					HeavyZoneStreamingProxy heavyZoneStreamingProxy = ((gameObject != null) ? gameObject.GetComponent<HeavyZoneStreamingProxy>() : null);
					if (heavyZoneStreamingProxy != null && !heavyZoneStreamingProxy.NeverPool && ProceduralZonePolicy.IsSpecialZone(zoneData))
					{
						hashSet.Add(gameObject);
					}
				}
			}
		}
		foreach (GameObject gameObject2 in hashSet)
		{
			int num;
			if (targets.TryGetValue(gameObject2, out num) && num > 1)
			{
				targets[gameObject2] = 1;
			}
		}
	}

	// Token: 0x06000309 RID: 777 RVA: 0x00012BEC File Offset: 0x00010DEC
	private int EstimateBiomeChurnWindowChunks()
	{
		int num = this.EstimateVisibleChunkCount(Mathf.Max(1, this.spawnRadius));
		int num2 = 2 * this.GetPoolCoverageRadius() + 1;
		return num + num2;
	}

	// Token: 0x0600030A RID: 778 RVA: 0x00012C18 File Offset: 0x00010E18
	private void BoostZoneTargetsForBiomes(Dictionary<GameObject, int> targets)
	{
		if (this.biomeManager == null || this.biomeManager.biomes == null || this.biomeManager.biomes.Count == 0)
		{
			return;
		}
		if (this.availableChunks == null || this.availableChunks.Length == 0)
		{
			return;
		}
		int num = this.EstimateBiomeChurnWindowChunks();
		List<ZoneData> list = new List<ZoneData>(16);
		foreach (BiomeConfig biomeConfig in this.biomeManager.biomes)
		{
			if (!(biomeConfig == null))
			{
				HashSet<ZoneData> hashSet = ChunkManager.BuildZoneSet(biomeConfig);
				HashSet<ZoneData> hashSet2 = ((biomeConfig.fallbackBiome != null) ? ChunkManager.BuildZoneSet(biomeConfig.fallbackBiome) : null);
				float num2 = 0f;
				foreach (ChunkData chunkData in this.availableChunks)
				{
					Object @object;
					if (chunkData == null)
					{
						@object = null;
					}
					else
					{
						ChunkTemplate template = chunkData.template;
						@object = ((template != null) ? template.chunkPrefab : null);
					}
					if (!(@object == null))
					{
						num2 += ChunkManager.BiomeAdjustedChunkWeight(chunkData, biomeConfig);
					}
				}
				if (num2 > 0f)
				{
					Dictionary<GameObject, float> dictionary = new Dictionary<GameObject, float>();
					foreach (ChunkData chunkData2 in this.availableChunks)
					{
						Object object2;
						if (chunkData2 == null)
						{
							object2 = null;
						}
						else
						{
							ChunkTemplate template2 = chunkData2.template;
							object2 = ((template2 != null) ? template2.chunkPrefab : null);
						}
						if (!(object2 == null) && chunkData2.template.zoneSlots != null)
						{
							float num3 = ChunkManager.BiomeAdjustedChunkWeight(chunkData2, biomeConfig) / num2;
							if (num3 > 0f)
							{
								foreach (ZoneSlot zoneSlot in chunkData2.template.zoneSlots)
								{
									this.FillCompatible(chunkData2.availableZones, zoneSlot, hashSet, list);
									if (list.Count == 0)
									{
										this.FillCompatible(chunkData2.availableZones, zoneSlot, hashSet2, list);
									}
									if (list.Count == 0)
									{
										this.FillCompatible(chunkData2.availableZones, zoneSlot, null, list);
									}
									if (list.Count != 0)
									{
										float num4 = 0f;
										foreach (ZoneData zoneData in list)
										{
											num4 += Mathf.Max(0.0001f, zoneData.spawnWeight);
										}
										foreach (ZoneData zoneData2 in list)
										{
											GameObject gameObject = this.ResolveZonePrefab(zoneData2);
											if (!(gameObject == null))
											{
												float num5 = Mathf.Max(0.0001f, zoneData2.spawnWeight) / num4;
												float num6;
												dictionary.TryGetValue(gameObject, out num6);
												dictionary[gameObject] = num6 + num3 * num5;
											}
										}
									}
								}
							}
						}
					}
					foreach (KeyValuePair<GameObject, float> keyValuePair in dictionary)
					{
						int num7 = Mathf.CeilToInt((float)num * keyValuePair.Value);
						num7 = Mathf.Clamp(num7, this.minZonePrewarmPerPrefab, Mathf.Max(this.minZonePrewarmPerPrefab, this.maxPooledZonesPerPrefab));
						int num8;
						if (!targets.TryGetValue(keyValuePair.Key, out num8) || num7 > num8)
						{
							targets[keyValuePair.Key] = num7;
						}
					}
				}
			}
		}
		if (this.useDynamicZonePoolTarget)
		{
			int num9 = 0;
			foreach (int num10 in targets.Values)
			{
				num9 += num10;
			}
			int num11 = Mathf.Max(targets.Count * Mathf.Max(1, this.minZonePrewarmPerPrefab), this.maxAutoZonePrewarmTarget);
			if (num9 > num11)
			{
				float num12 = (float)num11 / (float)num9;
				List<GameObject> list2 = new List<GameObject>(targets.Keys);
				int num13 = Mathf.Max(1, this.minZonePrewarmPerPrefab);
				int num14 = 0;
				using (List<GameObject>.Enumerator enumerator6 = list2.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						GameObject gameObject2 = enumerator6.Current;
						int num15 = Mathf.Max(num13, Mathf.FloorToInt((float)targets[gameObject2] * num12));
						targets[gameObject2] = num15;
						num14 += num15;
					}
					goto IL_04EB;
				}
				IL_045B:
				GameObject gameObject3 = null;
				int num16 = num13;
				foreach (GameObject gameObject4 in list2)
				{
					int num17 = targets[gameObject4];
					if (num17 > num13 && num17 >= num16 && (gameObject3 == null || num17 > num16 || (num17 == num16 && ChunkManager.CompareZonePrefabNames(gameObject4, gameObject3) < 0)))
					{
						gameObject3 = gameObject4;
						num16 = num17;
					}
				}
				if (gameObject3 == null)
				{
					goto IL_04F4;
				}
				targets[gameObject3] = num16 - 1;
				num14--;
				IL_04EB:
				if (num14 > num11)
				{
					goto IL_045B;
				}
				IL_04F4:
				if (this.enableDebugLogs)
				{
					Debug.Log(string.Format("[ChunkManager] Zone prewarm targets scalati: {0} → cap {1}", num9, num11));
				}
			}
		}
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00013204 File Offset: 0x00011404
	private void EnsureSpawnWindowZoneCoverageTargets(Dictionary<GameObject, int> targets)
	{
		if (targets == null || this.availableChunks == null || this.availableChunks.Length == 0)
		{
			return;
		}
		int num = 2 * Mathf.Max(1, this.spawnRadius) + 1;
		int num2 = num * num;
		List<ChunkManager.ZoneCandidateCoverageRequirement> list = new List<ChunkManager.ZoneCandidateCoverageRequirement>(8);
		for (int i = 0; i < this.availableChunks.Length; i++)
		{
			ChunkData chunkData = this.availableChunks[i];
			bool flag;
			if (chunkData == null)
			{
				flag = null != null;
			}
			else
			{
				ChunkTemplate template = chunkData.template;
				flag = ((template != null) ? template.zoneSlots : null) != null;
			}
			if (flag && chunkData.template.zoneSlots.Count != 0 && chunkData.availableZones != null)
			{
				int num3 = Mathf.Max(1, chunkData.gridSize.x * chunkData.gridSize.y);
				int num4 = Mathf.Max(1, Mathf.CeilToInt((float)num2 / (float)num3));
				if (ProceduralZonePolicy.IsAlwaysSpecialChunk(chunkData))
				{
					num4 = 1;
				}
				HashSet<ZonePoolKey> hashSet = new HashSet<ZonePoolKey>();
				for (int j = 0; j < chunkData.template.zoneSlots.Count; j++)
				{
					ZoneSlot zoneSlot = chunkData.template.zoneSlots[j];
					ZonePoolKey zonePoolKey = new ZonePoolKey(zoneSlot.parentChunkSize, zoneSlot.occupancy);
					if (hashSet.Add(zonePoolKey))
					{
						int num5 = 0;
						for (int k = 0; k < chunkData.template.zoneSlots.Count; k++)
						{
							ZoneSlot zoneSlot2 = chunkData.template.zoneSlots[k];
							if (zoneSlot2.parentChunkSize == zonePoolKey.chunkSize && zoneSlot2.occupancy == zonePoolKey.occupancy)
							{
								num5++;
							}
						}
						List<GameObject> list2 = new List<GameObject>(16);
						for (int l = 0; l < chunkData.availableZones.Count; l++)
						{
							ZoneData zoneData = chunkData.availableZones[l];
							if (this.IsZoneEligibleForPrewarm(zoneData) && !ProceduralZonePolicy.IsSpecialZone(zoneData) && zoneData.IsCompatibleWith(zoneSlot) && !list2.Contains(zoneData.zonePrefab))
							{
								list2.Add(zoneData.zonePrefab);
							}
						}
						if (list2.Count != 0)
						{
							list2.Sort(new Comparison<GameObject>(ChunkManager.CompareZonePrefabNames));
							ChunkManager.ZoneCandidateCoverageRequirement zoneCandidateCoverageRequirement = ChunkManager.FindCoverageRequirement(list, list2);
							if (zoneCandidateCoverageRequirement == null)
							{
								zoneCandidateCoverageRequirement = new ChunkManager.ZoneCandidateCoverageRequirement();
								zoneCandidateCoverageRequirement.prefabs.AddRange(list2);
								list.Add(zoneCandidateCoverageRequirement);
							}
							zoneCandidateCoverageRequirement.requiredInstances = Mathf.Max(zoneCandidateCoverageRequirement.requiredInstances, num4 * num5);
						}
					}
				}
			}
		}
		list.Sort((ChunkManager.ZoneCandidateCoverageRequirement a, ChunkManager.ZoneCandidateCoverageRequirement b) => b.requiredInstances.CompareTo(a.requiredInstances));
		for (int m = 0; m < list.Count; m++)
		{
			this.EnsureAggregateZoneTarget(targets, list[m]);
		}
	}

	// Token: 0x0600030C RID: 780 RVA: 0x000134B4 File Offset: 0x000116B4
	private void EnsureAggregateZoneTarget(Dictionary<GameObject, int> targets, ChunkManager.ZoneCandidateCoverageRequirement requirement)
	{
		int num = 0;
		int num2 = 0;
		foreach (KeyValuePair<GameObject, int> keyValuePair in targets)
		{
			num2 += keyValuePair.Value;
		}
		for (int i = 0; i < requirement.prefabs.Count; i++)
		{
			GameObject gameObject = requirement.prefabs[i];
			int num3;
			if (targets.TryGetValue(gameObject, out num3))
			{
				num += num3;
			}
		}
		int num4 = Mathf.Max(this.minZonePrewarmPerPrefab, this.maxPooledZonesPerPrefab);
		int num5 = Mathf.Max(targets.Count * Mathf.Max(1, this.minZonePrewarmPerPrefab), this.maxAutoZonePrewarmTarget);
		while (num < requirement.requiredInstances && num2 < num5)
		{
			GameObject gameObject2 = null;
			int num6 = int.MaxValue;
			for (int j = 0; j < requirement.prefabs.Count; j++)
			{
				GameObject gameObject3 = requirement.prefabs[j];
				int num7;
				targets.TryGetValue(gameObject3, out num7);
				if (num7 < num4 && (gameObject2 == null || num7 < num6 || (num7 == num6 && ChunkManager.CompareZonePrefabNames(gameObject3, gameObject2) < 0)))
				{
					gameObject2 = gameObject3;
					num6 = num7;
				}
			}
			if (gameObject2 == null)
			{
				break;
			}
			targets[gameObject2] = num6 + 1;
			num++;
			num2++;
		}
		if (num < requirement.requiredInstances)
		{
			Debug.LogWarning(string.Format("[ChunkManager] Zone prewarm coverage limit: candidateSet={0}, ", requirement.prefabs.Count) + string.Format("required={0}, reserved={1}, ", requirement.requiredInstances, num) + string.Format("perPrefabCap={0}, globalCap={1}. Runtime spawn remains deferred safely.", num4, num5));
			return;
		}
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[ChunkManager] Zone prewarm candidate coverage: variants={0}, ", requirement.prefabs.Count) + string.Format("reserved={0}/{1}.", num, requirement.requiredInstances));
		}
	}

	// Token: 0x0600030D RID: 781 RVA: 0x000136BC File Offset: 0x000118BC
	private static ChunkManager.ZoneCandidateCoverageRequirement FindCoverageRequirement(List<ChunkManager.ZoneCandidateCoverageRequirement> requirements, List<GameObject> prefabs)
	{
		for (int i = 0; i < requirements.Count; i++)
		{
			List<GameObject> prefabs2 = requirements[i].prefabs;
			if (prefabs2.Count == prefabs.Count)
			{
				bool flag = true;
				for (int j = 0; j < prefabs.Count; j++)
				{
					if (!(prefabs2[j] == prefabs[j]))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return requirements[i];
				}
			}
		}
		return null;
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00013730 File Offset: 0x00011930
	private static int CompareZonePrefabNames(GameObject a, GameObject b)
	{
		if (a == b)
		{
			return 0;
		}
		if (a == null)
		{
			return 1;
		}
		if (b == null)
		{
			return -1;
		}
		int num = string.CompareOrdinal(a.name, b.name);
		if (num == 0)
		{
			return a.GetInstanceID().CompareTo(b.GetInstanceID());
		}
		return num;
	}

	// Token: 0x0600030F RID: 783 RVA: 0x00013784 File Offset: 0x00011984
	private bool IsZoneEligibleForPrewarm(ZoneData zone)
	{
		if (zone == null || ProceduralZonePolicy.IsExcludedZone(zone))
		{
			return false;
		}
		GameObject gameObject = this.ResolveZonePrefab(zone);
		return gameObject != null && !ChunkManager.IsNeverPooledHeavyZone(gameObject);
	}

	// Token: 0x06000310 RID: 784 RVA: 0x000137C0 File Offset: 0x000119C0
	private static HashSet<ZoneData> BuildZoneSet(BiomeConfig biome)
	{
		HashSet<ZoneData> hashSet = new HashSet<ZoneData>();
		if (((biome != null) ? biome.availableZones : null) != null)
		{
			foreach (ZoneData zoneData in biome.availableZones)
			{
				if (zoneData != null)
				{
					hashSet.Add(zoneData);
				}
			}
		}
		return hashSet;
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00013834 File Offset: 0x00011A34
	private static float BiomeAdjustedChunkWeight(ChunkData chunk, BiomeConfig biome)
	{
		float num = Mathf.Max(0f, chunk.spawnWeight);
		if (biome.chunkWeightOverrides != null)
		{
			foreach (BiomeConfig.ChunkWeightOverride chunkWeightOverride in biome.chunkWeightOverrides)
			{
				if (chunkWeightOverride != null && chunkWeightOverride.chunk == chunk)
				{
					num *= Mathf.Max(0f, chunkWeightOverride.multiplier);
					break;
				}
			}
		}
		return num;
	}

	// Token: 0x06000312 RID: 786 RVA: 0x000138C0 File Offset: 0x00011AC0
	private void FillCompatible(List<ZoneData> zones, ZoneSlot slot, HashSet<ZoneData> filter, List<ZoneData> result)
	{
		result.Clear();
		if (zones == null)
		{
			return;
		}
		foreach (ZoneData zoneData in zones)
		{
			if (this.IsZoneEligibleForPrewarm(zoneData) && zoneData.IsCompatibleWith(slot) && (filter == null || filter.Contains(zoneData)))
			{
				result.Add(zoneData);
			}
		}
	}

	// Token: 0x06000313 RID: 787 RVA: 0x00013938 File Offset: 0x00011B38
	private int EstimateVisibleChunkCount(int radius)
	{
		return 1 + 2 * radius * (radius + 1);
	}

	// Token: 0x06000314 RID: 788 RVA: 0x00013944 File Offset: 0x00011B44
	private int GetPoolCoverageRadius()
	{
		int num = Mathf.Max(1, this.spawnRadius);
		if (this.enableRuntimeChunkDespawn && this.useRuntimeDespawnRadiusForPoolCoverage)
		{
			num = Mathf.Max(num, this.runtimeDespawnRadius);
		}
		return num;
	}

	// Token: 0x06000315 RID: 789 RVA: 0x0001397C File Offset: 0x00011B7C
	private float EstimateWeightedAverageChunkAreaCells()
	{
		if (this.availableChunks == null || this.availableChunks.Length == 0)
		{
			return 1f;
		}
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < this.availableChunks.Length; i++)
		{
			ChunkData chunkData = this.availableChunks[i];
			Object @object;
			if (chunkData == null)
			{
				@object = null;
			}
			else
			{
				ChunkTemplate template = chunkData.template;
				@object = ((template != null) ? template.chunkPrefab : null);
			}
			if (!(@object == null))
			{
				float num3 = Mathf.Max(0.0001f, chunkData.spawnWeight);
				int num4 = Mathf.Max(1, chunkData.gridSize.x) * Mathf.Max(1, chunkData.gridSize.y);
				num += (float)num4 * num3;
				num2 += num3;
			}
		}
		if (num2 <= 0f)
		{
			return 1f;
		}
		return Mathf.Max(1f, num / num2);
	}

	// Token: 0x06000316 RID: 790 RVA: 0x00013A48 File Offset: 0x00011C48
	private int EstimatePoolCoverageChunkCount()
	{
		int num = this.EstimateVisibleChunkCount(this.GetPoolCoverageRadius());
		float num2 = this.EstimateWeightedAverageChunkAreaCells();
		return Mathf.Max(1, Mathf.CeilToInt((float)num / num2));
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00013A78 File Offset: 0x00011C78
	private Dictionary<GameObject, int> BuildPrewarmTargets()
	{
		Dictionary<GameObject, int> dictionary = new Dictionary<GameObject, int>();
		if (this.availableChunks == null || this.availableChunks.Length == 0)
		{
			return dictionary;
		}
		float num = 0f;
		int num2 = 0;
		Dictionary<GameObject, float> dictionary2 = new Dictionary<GameObject, float>();
		foreach (ChunkData chunkData in this.availableChunks)
		{
			GameObject gameObject;
			if (chunkData == null)
			{
				gameObject = null;
			}
			else
			{
				ChunkTemplate template = chunkData.template;
				gameObject = ((template != null) ? template.chunkPrefab : null);
			}
			GameObject gameObject2 = gameObject;
			if (!(gameObject2 == null))
			{
				float num3 = Mathf.Max(0.0001f, chunkData.spawnWeight);
				num += num3;
				if (!dictionary2.ContainsKey(gameObject2))
				{
					dictionary2[gameObject2] = 0f;
					num2++;
				}
				Dictionary<GameObject, float> dictionary3 = dictionary2;
				GameObject gameObject3 = gameObject2;
				dictionary3[gameObject3] += num3;
			}
		}
		if (num2 == 0)
		{
			return dictionary;
		}
		int num4 = this.EstimatePoolCoverageChunkCount();
		int num5 = Mathf.Max(0, this.minPrewarmPerPrefab * num2);
		int num6 = Mathf.CeilToInt((float)num4 * this.chunkPoolCoverageMultiplier);
		num6 = Mathf.Clamp(num6, num5, Mathf.Max(num5, this.maxAutoChunkPrewarmTarget));
		int num7 = (this.autoSizeChunkPrewarmFromCoverage ? num6 : this.initialPrewarmCount);
		if (this.enableDebugLogs && this.autoSizeChunkPrewarmFromCoverage)
		{
			int poolCoverageRadius = this.GetPoolCoverageRadius();
			float num8 = this.EstimateWeightedAverageChunkAreaCells();
			Debug.Log(string.Format("[ChunkManager] Auto chunk prewarm target: {0} (coverageRadius: {1}, estimatedChunks: {2}, avgChunkAreaCells: {3:0.00}, multiplier: {4:0.00})", new object[] { num7, poolCoverageRadius, num4, num8, this.chunkPoolCoverageMultiplier }));
		}
		foreach (KeyValuePair<GameObject, float> keyValuePair in dictionary2)
		{
			GameObject key = keyValuePair.Key;
			float num9 = keyValuePair.Value / num;
			int num10 = Mathf.RoundToInt((float)num7 * num9);
			int num11 = Mathf.Clamp(Mathf.Max(this.minPrewarmPerPrefab, num10), this.minPrewarmPerPrefab, this.effectiveMaxPooledInstancesPerPrefab);
			dictionary[key] = num11;
		}
		this.BoostChunkTargetsForBiomes(dictionary);
		return dictionary;
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00013CA0 File Offset: 0x00011EA0
	private void BoostChunkTargetsForBiomes(Dictionary<GameObject, int> targets)
	{
		if (this.biomeManager == null || this.biomeManager.biomes == null || this.biomeManager.biomes.Count == 0)
		{
			return;
		}
		if (this.availableChunks == null || this.availableChunks.Length == 0)
		{
			return;
		}
		int num = this.EstimateBiomeChurnWindowChunks();
		foreach (BiomeConfig biomeConfig in this.biomeManager.biomes)
		{
			if (!(biomeConfig == null))
			{
				float num2 = 0f;
				foreach (ChunkData chunkData in this.availableChunks)
				{
					Object @object;
					if (chunkData == null)
					{
						@object = null;
					}
					else
					{
						ChunkTemplate template = chunkData.template;
						@object = ((template != null) ? template.chunkPrefab : null);
					}
					if (!(@object == null))
					{
						num2 += ChunkManager.BiomeAdjustedChunkWeight(chunkData, biomeConfig);
					}
				}
				if (num2 > 0f)
				{
					Dictionary<GameObject, float> dictionary = new Dictionary<GameObject, float>();
					foreach (ChunkData chunkData2 in this.availableChunks)
					{
						GameObject gameObject;
						if (chunkData2 == null)
						{
							gameObject = null;
						}
						else
						{
							ChunkTemplate template2 = chunkData2.template;
							gameObject = ((template2 != null) ? template2.chunkPrefab : null);
						}
						GameObject gameObject2 = gameObject;
						if (!(gameObject2 == null))
						{
							float num3 = ChunkManager.BiomeAdjustedChunkWeight(chunkData2, biomeConfig) / num2;
							float num4;
							dictionary.TryGetValue(gameObject2, out num4);
							dictionary[gameObject2] = num4 + num3;
						}
					}
					foreach (KeyValuePair<GameObject, float> keyValuePair in dictionary)
					{
						int num5 = Mathf.CeilToInt((float)num * keyValuePair.Value);
						num5 = Mathf.Clamp(num5, this.minPrewarmPerPrefab, Mathf.Max(this.minPrewarmPerPrefab, this.effectiveMaxPooledInstancesPerPrefab));
						int num6;
						if (!targets.TryGetValue(keyValuePair.Key, out num6) || num5 > num6)
						{
							targets[keyValuePair.Key] = num5;
						}
					}
				}
			}
		}
		if (this.autoSizeChunkPrewarmFromCoverage)
		{
			int num7 = 0;
			foreach (int num8 in targets.Values)
			{
				num7 += num8;
			}
			int num9 = Mathf.Max(targets.Count * Mathf.Max(1, this.minPrewarmPerPrefab), this.maxAutoChunkPrewarmTarget);
			if (num7 > num9)
			{
				float num10 = (float)num9 / (float)num7;
				foreach (GameObject gameObject3 in new List<GameObject>(targets.Keys))
				{
					targets[gameObject3] = Mathf.Max(Mathf.Max(1, this.minPrewarmPerPrefab), Mathf.FloorToInt((float)targets[gameObject3] * num10));
				}
				if (this.enableDebugLogs)
				{
					Debug.Log(string.Format("[ChunkManager] Chunk prewarm targets scalati: {0} → cap {1}", num7, num9));
				}
			}
		}
	}

	// Token: 0x06000319 RID: 793 RVA: 0x00013FF4 File Offset: 0x000121F4
	private GameObject AcquireChunkRoot(ChunkData chunkData, Vector3 worldPos, Quaternion rot)
	{
		GameObject gameObject;
		if (chunkData == null)
		{
			gameObject = null;
		}
		else
		{
			ChunkTemplate template = chunkData.template;
			gameObject = ((template != null) ? template.chunkPrefab : null);
		}
		GameObject gameObject2 = gameObject;
		if (gameObject2 == null)
		{
			return null;
		}
		Transform transform = this.EnsureRuntimeWorldRoot();
		if (!this.enableChunkPooling)
		{
			GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject2, worldPos, rot, transform);
			this.RegisterPooledInstance(gameObject3, gameObject2);
			return gameObject3;
		}
		Queue<GameObject> queue;
		if (this._chunkPoolByPrefab.TryGetValue(gameObject2, out queue) && queue.Count > 0)
		{
			GameObject gameObject4 = queue.Dequeue();
			this._poolCountByPrefab[gameObject2] = queue.Count;
			gameObject4.transform.SetParent(transform);
			gameObject4.transform.SetPositionAndRotation(worldPos, rot);
			this.RestoreChunkZonePlaceholders(gameObject4);
			gameObject4.SetActive(true);
			return gameObject4;
		}
		if (this._fullGameplayReady)
		{
			int runtimeChunkPoolMisses = this.RuntimeChunkPoolMisses;
			this.RuntimeChunkPoolMisses = runtimeChunkPoolMisses + 1;
		}
		GameObject gameObject5;
		using (ChunkManager.ChunkPoolMissInstantiateMarker.Auto())
		{
			gameObject5 = Object.Instantiate<GameObject>(gameObject2, worldPos, rot, transform);
		}
		this.RegisterPooledInstance(gameObject5, gameObject2);
		return gameObject5;
	}

	// Token: 0x0600031A RID: 794 RVA: 0x0001410C File Offset: 0x0001230C
	private void ReleaseChunkRoot(GameObject chunkRoot)
	{
		if (chunkRoot == null)
		{
			return;
		}
		GameObject gameObject;
		Queue<GameObject> queue;
		if (this._instanceToPrefabMap.TryGetValue(chunkRoot, out gameObject) && gameObject != null && this._chunkPoolByPrefab.TryGetValue(gameObject, out queue) && queue.Contains(chunkRoot))
		{
			return;
		}
		this.RestoreChunkZonePlaceholders(chunkRoot);
		if (!this.enableChunkPooling)
		{
			this._instanceToPrefabMap.Remove(chunkRoot);
			this.ForgetChunkZonePlaceholders(chunkRoot);
			this._chunkBaseColliderCache.Remove(chunkRoot);
			this._chunkBaseTransformsCache.Remove(chunkRoot);
			Object.Destroy(chunkRoot);
			return;
		}
		GameObject gameObject2;
		if (!this._instanceToPrefabMap.TryGetValue(chunkRoot, out gameObject2) || gameObject2 == null)
		{
			this._instanceToPrefabMap.Remove(chunkRoot);
			this.ForgetChunkZonePlaceholders(chunkRoot);
			this._chunkBaseColliderCache.Remove(chunkRoot);
			this._chunkBaseTransformsCache.Remove(chunkRoot);
			Object.Destroy(chunkRoot);
			return;
		}
		Queue<GameObject> queue2;
		if ((this._chunkPoolByPrefab.TryGetValue(gameObject2, out queue2) ? queue2.Count : 0) >= this.effectiveMaxPooledInstancesPerPrefab || ChunkManager.GetTotalPoolCount(this._chunkPoolByPrefab) >= Mathf.Max(1, this.maxAutoChunkPrewarmTarget))
		{
			this._instanceToPrefabMap.Remove(chunkRoot);
			this.ForgetChunkZonePlaceholders(chunkRoot);
			this._chunkBaseColliderCache.Remove(chunkRoot);
			this._chunkBaseTransformsCache.Remove(chunkRoot);
			Object.Destroy(chunkRoot);
			return;
		}
		chunkRoot.transform.SetParent(this._chunkPoolContainer);
		chunkRoot.SetActive(false);
		this.ReturnInstanceToPool(chunkRoot, gameObject2);
	}

	// Token: 0x0600031B RID: 795 RVA: 0x00014277 File Offset: 0x00012477
	private void EnqueuePostSpawnInitialization(Vector2Int cell)
	{
		if (!this.enableDeferredPostSpawnInit)
		{
			return;
		}
		if (this._pendingPostSpawnInitSet.Add(cell))
		{
			this._pendingPostSpawnInit.Enqueue(cell);
		}
	}

	// Token: 0x0600031C RID: 796 RVA: 0x0001429C File Offset: 0x0001249C
	private void ProcessPostSpawnInitializationQueue()
	{
		this.ProcessPostSpawnInitializationQueueWithinBudget(null);
	}

	// Token: 0x0600031D RID: 797 RVA: 0x000142A8 File Offset: 0x000124A8
	private void ProcessPostSpawnInitializationQueueWithinBudget(DeferredStreamingFrameBudget budget)
	{
		if (!this.enableDeferredPostSpawnInit || this._pendingPostSpawnInit.Count == 0)
		{
			return;
		}
		int num = 0;
		double num2 = ((budget != null) ? budget.BeginQueue() : DeferredStreamingFrameBudget.CurrentTimeMilliseconds());
		try
		{
			while (this._pendingPostSpawnInit.Count > 0 && num < this.effMaxPostSpawnInitOpsPerFrame)
			{
				if ((budget != null) ? (!budget.CanStartOperation(num2, (double)this.effPostSpawnInitFrameBudgetMs)) : (DeferredStreamingFrameBudget.CurrentTimeMilliseconds() - num2 >= (double)this.effPostSpawnInitFrameBudgetMs))
				{
					break;
				}
				Vector2Int vector2Int = this._pendingPostSpawnInit.Dequeue();
				this._pendingPostSpawnInitSet.Remove(vector2Int);
				ChunkInstance chunkInstance;
				if (this._spawnedChunks.TryGetValue(vector2Int, out chunkInstance) && !(((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null))
				{
					if (this.preCacheVisualsAfterSpawn)
					{
						this.CacheChunkVisualComponents(vector2Int, chunkInstance, null);
					}
					this.RecordCompletedInitializationWork();
					num++;
				}
			}
		}
		finally
		{
			if (budget != null)
			{
				budget.EndQueue(num2);
			}
		}
	}

	// Token: 0x0600031E RID: 798 RVA: 0x000143A0 File Offset: 0x000125A0
	private GameObject AcquireZoneRoot(GameObject zonePrefab, Transform parent, Vector3 position, Quaternion rotation)
	{
		return this.AcquireZoneRootWithResidency(zonePrefab, parent, position, rotation, null);
	}

	// Token: 0x0600031F RID: 799 RVA: 0x000143C4 File Offset: 0x000125C4
	private GameObject AcquireZoneRootWithResidency(GameObject zonePrefab, Transform parent, Vector3 position, Quaternion rotation, PoiResidencyContext? context)
	{
		if (zonePrefab == null)
		{
			return null;
		}
		bool flag = ChunkManager.IsRuntimeStreamableNeverPoolZone(zonePrefab);
		if (!this.enableZonePooling)
		{
			int num;
			if (this._fullGameplayReady && this.forbidRuntimeZoneInstantiate && !flag)
			{
				num = this.RuntimeZonePoolMisses;
				this.RuntimeZonePoolMisses = num + 1;
				return null;
			}
			if (this._fullGameplayReady)
			{
				num = this.RuntimeZoneColdInstantiates;
				this.RuntimeZoneColdInstantiates = num + 1;
			}
			GameObject gameObject = this.InstantiateZoneWithResidency(zonePrefab, parent, position, rotation, context);
			num = this.TotalZoneInstantiates;
			this.TotalZoneInstantiates = num + 1;
			this.RegisterPooledZoneInstance(gameObject, zonePrefab);
			return gameObject;
		}
		else
		{
			GameObject gameObject2 = null;
			using (ChunkManager.ZonePoolAcquireMarker.Auto())
			{
				Queue<GameObject> queue;
				if (this._zonePoolByPrefab.TryGetValue(zonePrefab, out queue))
				{
					while (queue.Count > 0 && gameObject2 == null)
					{
						gameObject2 = queue.Dequeue();
					}
					this._zonePoolCountByPrefab[zonePrefab] = queue.Count;
				}
			}
			if (gameObject2 != null)
			{
				using (ChunkManager.ZonePoolActivateMarker.Auto())
				{
					if (context != null)
					{
						gameObject2.SetActive(false);
						PoiResidencyPlacement poiResidencyPlacement = PoiResidencyPlacement.Configure(gameObject2, context.Value);
						HeavyZoneStreamingProxy component = gameObject2.GetComponent<HeavyZoneStreamingProxy>();
						GameObject gameObject3 = ((component != null) ? component.PayloadInstance : null);
						if (gameObject3 != null)
						{
							poiResidencyPlacement.BindPayload(gameObject3);
						}
					}
					using (ChunkManager.ZonePoolActivateParentMarker.Auto())
					{
						gameObject2.transform.SetParent(parent);
					}
					using (ChunkManager.ZonePoolActivatePoseMarker.Auto())
					{
						gameObject2.transform.SetPositionAndRotation(position, rotation);
					}
					HeavyZoneStreamingProxy component2 = gameObject2.GetComponent<HeavyZoneStreamingProxy>();
					if (component2 != null && component2.UsesStagedPooledActivation)
					{
						component2.ArmStagedWake();
					}
					IncrementalPooledZoneActivation component3 = gameObject2.GetComponent<IncrementalPooledZoneActivation>();
					if (component3 != null && component3.HasSnapshot)
					{
						component3.ArmStagedWake();
					}
					using (ChunkManager.ZonePoolActivateRootEnableMarker.Auto())
					{
						gameObject2.SetActive(true);
					}
				}
				return gameObject2;
			}
			int num;
			if (this._fullGameplayReady)
			{
				num = this.RuntimeZonePoolMisses;
				this.RuntimeZonePoolMisses = num + 1;
				if (this.forbidRuntimeZoneInstantiate && !flag)
				{
					return null;
				}
				num = this.RuntimeZoneColdInstantiates;
				this.RuntimeZoneColdInstantiates = num + 1;
			}
			GameObject gameObject4;
			using (ChunkManager.ZonePoolMissInstantiateMarker.Auto())
			{
				gameObject4 = this.InstantiateZoneWithResidency(zonePrefab, parent, position, rotation, context);
			}
			num = this.TotalZoneInstantiates;
			this.TotalZoneInstantiates = num + 1;
			this.RegisterPooledZoneInstance(gameObject4, zonePrefab);
			return gameObject4;
		}
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00014708 File Offset: 0x00012908
	private GameObject InstantiateZoneWithResidency(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, PoiResidencyContext? context)
	{
		if (context == null)
		{
			return Object.Instantiate<GameObject>(prefab, position, rotation, parent);
		}
		if (this._poiResidencyCreationGate == null)
		{
			this.EnsureRuntimeWorldRoot();
			GameObject gameObject = new GameObject("PoiResidencyCreationGate");
			gameObject.SetActive(false);
			gameObject.transform.SetParent(base.transform, false);
			this._poiResidencyCreationGate = gameObject.transform;
		}
		GameObject gameObject2 = Object.Instantiate<GameObject>(prefab, position, rotation, this._poiResidencyCreationGate);
		GameObject gameObject3;
		try
		{
			bool activeSelf = gameObject2.activeSelf;
			gameObject2.SetActive(false);
			gameObject2.transform.SetParent(parent, false);
			gameObject2.transform.SetPositionAndRotation(position, rotation);
			gameObject2.transform.localScale = prefab.transform.localScale;
			PoiResidencyPlacement.Configure(gameObject2, context.Value);
			gameObject2.SetActive(activeSelf);
			gameObject3 = gameObject2;
		}
		catch
		{
			if (Application.isPlaying)
			{
				Object.Destroy(gameObject2);
			}
			else
			{
				Object.DestroyImmediate(gameObject2);
			}
			throw;
		}
		return gameObject3;
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00014800 File Offset: 0x00012A00
	private void BeginPoiResidencySession()
	{
		ChunkManager._nextPoiResidencySessionGeneration = ((ChunkManager._nextPoiResidencySessionGeneration == int.MaxValue) ? 1 : (ChunkManager._nextPoiResidencySessionGeneration + 1));
		this._poiResidencySessionGeneration = ChunkManager._nextPoiResidencySessionGeneration;
		NPCRosterSnapshot currentNpcRoster = RunModeService.CurrentNpcRoster;
		this._poiResidencyRunId = ((currentNpcRoster != null) ? currentNpcRoster.runId : null) ?? string.Empty;
		PoiResidencyLedger.BeginSession(this._poiResidencySessionGeneration);
	}

	// Token: 0x06000322 RID: 802 RVA: 0x00014860 File Offset: 0x00012A60
	private PoiResidencyContext? CapturePoiResidencyContext(ChunkInstance chunk, int slotIndex, HeavyZoneStreamingProxy proxy)
	{
		PoiResidencyCatalog.Entry entry;
		int num;
		if (proxy == null || !PoiResidencyCatalog.TryGetByResourcePath(proxy.ResourcesPrefabPath, out entry) || !MapResidencyPilotSelection.IsFamilyEnabled(entry.family) || !this.TryGetSessionSeed(out num))
		{
			return null;
		}
		if (this._poiResidencySessionGeneration == 0)
		{
			this.BeginPoiResidencySession();
		}
		return new PoiResidencyContext?(new PoiResidencyContext(this._poiResidencyRunId, this._poiResidencySessionGeneration, num, chunk.originCell, this.TravelOffset, slotIndex, entry.family));
	}

	// Token: 0x06000323 RID: 803 RVA: 0x000148E0 File Offset: 0x00012AE0
	private int GetLiveZonePoolCount(GameObject prefab)
	{
		Queue<GameObject> queue;
		if (prefab == null || !this._zonePoolByPrefab.TryGetValue(prefab, out queue) || queue == null)
		{
			return 0;
		}
		int count = queue.Count;
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = queue.Dequeue();
			if (gameObject != null)
			{
				queue.Enqueue(gameObject);
			}
		}
		this._zonePoolCountByPrefab[prefab] = queue.Count;
		return queue.Count;
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00014950 File Offset: 0x00012B50
	private bool TrySelectPooledZoneFallback(ChunkInstance chunk, ZoneSlot slot, ZoneData desired, out ZoneData actual)
	{
		actual = null;
		bool flag;
		if (chunk == null)
		{
			flag = null != null;
		}
		else
		{
			ChunkData chunkData = chunk.chunkData;
			flag = ((chunkData != null) ? chunkData.availableZones : null) != null;
		}
		if (!flag || desired == null)
		{
			return false;
		}
		ChunkSpawnResolver.SpawnEntry spawnEntry;
		if (chunk.preservesCommittedZoneIdentity || (this._committedSpawnPlan.TryGetValue(chunk.originCell, out spawnEntry) && spawnEntry.chunkData == chunk.chunkData && spawnEntry.wasPresented))
		{
			return false;
		}
		if (ProceduralZonePolicy.IsSpecialZone(desired) && ProceduralZonePolicy.IsAlwaysSpecialChunk(chunk.chunkData))
		{
			return false;
		}
		BiomeConfig biomeConfig = ((this._spawnResolver != null) ? this._spawnResolver.GetBiomeForCell(chunk.originCell + this._spawnResolver.TravelOffset) : null);
		BiomeConfig biomeConfig2 = ((biomeConfig != null) ? biomeConfig.fallbackBiome : null);
		List<ZoneData> availableZones = chunk.chunkData.availableZones;
		for (int i = 0; i < 3; i++)
		{
			ZoneData zoneData = null;
			int num = 0;
			for (int j = 0; j < availableZones.Count; j++)
			{
				ZoneData zoneData2 = availableZones[j];
				if (!(zoneData2 == null) && !(zoneData2 == desired) && !(zoneData2.zonePrefab == null) && !ProceduralZonePolicy.IsExcludedZone(zoneData2) && !ProceduralZonePolicy.IsSpecialZone(zoneData2) && zoneData2.IsCompatibleWith(slot) && !ChunkManager.IsNeverPooledHeavyZone(zoneData2.zonePrefab))
				{
					bool flag2 = ((biomeConfig != null) ? biomeConfig.availableZones : null) != null && biomeConfig.availableZones.Contains(zoneData2);
					bool flag3 = ((biomeConfig2 != null) ? biomeConfig2.availableZones : null) != null && biomeConfig2.availableZones.Contains(zoneData2);
					if ((i != 0 || flag2) && (i != 1 || (!flag2 && flag3)) && (i != 2 || (!flag2 && !flag3)))
					{
						int liveZonePoolCount = this.GetLiveZonePoolCount(zoneData2.zonePrefab);
						if (liveZonePoolCount > num)
						{
							zoneData = zoneData2;
							num = liveZonePoolCount;
						}
					}
				}
			}
			if (zoneData != null)
			{
				actual = zoneData;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000325 RID: 805 RVA: 0x00014B54 File Offset: 0x00012D54
	private static bool IsNeverPooledHeavyZone(GameObject prefab)
	{
		HeavyZoneStreamingProxy heavyZoneStreamingProxy = ((prefab != null) ? prefab.GetComponent<HeavyZoneStreamingProxy>() : null);
		return heavyZoneStreamingProxy != null && heavyZoneStreamingProxy.NeverPool;
	}

	// Token: 0x06000326 RID: 806 RVA: 0x00014B88 File Offset: 0x00012D88
	private static bool IsRuntimeStreamableNeverPoolZone(GameObject prefab)
	{
		HeavyZoneStreamingProxy heavyZoneStreamingProxy = ((prefab != null) ? prefab.GetComponent<HeavyZoneStreamingProxy>() : null);
		return heavyZoneStreamingProxy != null && heavyZoneStreamingProxy.NeverPool && heavyZoneStreamingProxy.UsesStagedPooledActivation;
	}

	// Token: 0x06000327 RID: 807 RVA: 0x00014BC1 File Offset: 0x00012DC1
	private void ReleaseZoneRoot(GameObject zoneRoot, bool holdSpecialReservation)
	{
		this.ReleaseZoneRootInternal(zoneRoot, holdSpecialReservation, false);
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00014BCC File Offset: 0x00012DCC
	private void ReleaseZoneRootInternal(GameObject zoneRoot, bool holdSpecialReservation, bool retireOffscreenImmediately)
	{
		if (zoneRoot == null)
		{
			return;
		}
		if (this._retiringZoneRoots.Contains(zoneRoot))
		{
			return;
		}
		GameObject gameObject;
		this._zoneInstanceToPrefabMap.TryGetValue(zoneRoot, out gameObject);
		Queue<GameObject> queue = null;
		if (gameObject != null && this._zonePoolByPrefab.TryGetValue(gameObject, out queue) && queue.Contains(zoneRoot))
		{
			return;
		}
		HeavyZoneStreamingProxy component = zoneRoot.GetComponent<HeavyZoneStreamingProxy>();
		bool flag = component != null && component.NeverPool;
		bool flag2 = component != null && !flag && component.UsesStagedPooledActivation;
		IncrementalPooledZoneActivation component2 = zoneRoot.GetComponent<IncrementalPooledZoneActivation>();
		bool flag3 = component2 != null && component2.HasSnapshot;
		if (this.enableIncrementalZoneRetirement && this._zonePoolContainer != null && !this._zonePoolContainer.gameObject.activeSelf && base.gameObject.activeInHierarchy)
		{
			this._zonePoolContainer.gameObject.SetActive(true);
		}
		bool flag4 = this.enableIncrementalZoneRetirement && this._zonePoolContainer != null && this._zonePoolContainer.gameObject.activeInHierarchy;
		bool flag5 = this._retiringCompletedRun && this.IsPoiTravelSuspended && !zoneRoot.activeInHierarchy && (flag || !this.enableZonePooling || gameObject == null);
		bool flag6 = flag || !this.enableZonePooling || gameObject == null || ((queue != null) ? queue.Count : 0) + this.GetRetiringZoneCount(gameObject) >= this.maxPooledZonesPerPrefab || ChunkManager.GetTotalPoolCount(this._zonePoolByPrefab) + this.GetTotalRetiringZonePoolCount() >= Mathf.Max(1, this.maxAutoZonePrewarmTarget);
		bool flag7 = component != null && component.enabled;
		if (flag4 && zoneRoot.activeInHierarchy && flag7)
		{
			using (ChunkManager.ZoneRetirementPrepareMarker.Auto())
			{
				component.enabled = false;
			}
		}
		if (!zoneRoot.activeInHierarchy)
		{
			zoneRoot.SetActive(false);
			if (flag || !this.enableZonePooling || gameObject == null)
			{
				if (flag4 || flag5)
				{
					this.QueueZoneRetirement(zoneRoot, gameObject, true, holdSpecialReservation, component, flag7, retireOffscreenImmediately);
					return;
				}
				this._zoneInstanceToPrefabMap.Remove(zoneRoot);
				Object.Destroy(zoneRoot);
				return;
			}
			else
			{
				if (flag4 && (flag2 || flag3))
				{
					this.QueueZoneRetirement(zoneRoot, gameObject, false, holdSpecialReservation, component, flag7, retireOffscreenImmediately);
					return;
				}
				zoneRoot.transform.SetParent(this._zonePoolContainer, true);
				zoneRoot.transform.position = ChunkManager.ZoneRetirementOffscreenPosition;
				this.ReturnZoneInstanceToPool(zoneRoot, gameObject);
				return;
			}
		}
		else if (flag)
		{
			if (flag4)
			{
				this.QueueZoneRetirement(zoneRoot, gameObject, true, holdSpecialReservation, component, flag7, retireOffscreenImmediately);
				return;
			}
			this._zoneInstanceToPrefabMap.Remove(zoneRoot);
			Object.Destroy(zoneRoot);
			return;
		}
		else if (!this.enableZonePooling)
		{
			if (flag4)
			{
				this.QueueZoneRetirement(zoneRoot, gameObject, true, holdSpecialReservation, component, flag7, retireOffscreenImmediately);
				return;
			}
			this._zoneInstanceToPrefabMap.Remove(zoneRoot);
			Object.Destroy(zoneRoot);
			return;
		}
		else if (gameObject == null)
		{
			if (flag4)
			{
				this.QueueZoneRetirement(zoneRoot, null, true, holdSpecialReservation, component, flag7, retireOffscreenImmediately);
				return;
			}
			this._zoneInstanceToPrefabMap.Remove(zoneRoot);
			Object.Destroy(zoneRoot);
			return;
		}
		else
		{
			if (flag4)
			{
				this.QueueZoneRetirement(zoneRoot, gameObject, flag6, holdSpecialReservation, component, flag7, retireOffscreenImmediately);
				return;
			}
			if (flag6)
			{
				this._zoneInstanceToPrefabMap.Remove(zoneRoot);
				Object.Destroy(zoneRoot);
				return;
			}
			using (ChunkManager.ZonePoolReturnMarker.Auto())
			{
				using (ChunkManager.ZonePoolReparentMarker.Auto())
				{
					zoneRoot.transform.SetParent(this._zonePoolContainer);
				}
				using (ChunkManager.ZonePoolDeactivateMarker.Auto())
				{
					zoneRoot.SetActive(false);
				}
				this.ReturnZoneInstanceToPool(zoneRoot, gameObject);
			}
			return;
		}
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00014FC0 File Offset: 0x000131C0
	private void QueueZoneRetirement(GameObject zoneRoot, GameObject prefab, bool destroyWhenComplete, bool holdSpecialReservation, HeavyZoneStreamingProxy heavyProxy, bool heavyProxyAuthoredEnabled, bool retireOffscreenImmediately = false)
	{
		using (ChunkManager.ZonePoolReturnMarker.Auto())
		{
			using (ChunkManager.ZonePoolReparentMarker.Auto())
			{
				zoneRoot.transform.SetParent(this._zonePoolContainer, true);
			}
			IncrementalPooledZoneActivation component = zoneRoot.GetComponent<IncrementalPooledZoneActivation>();
			bool flag = (heavyProxy == null || heavyProxy.HasCompleteColliderSnapshot) && (component == null || component.IsOperationalReady);
			if (retireOffscreenImmediately && flag)
			{
				zoneRoot.transform.position = ChunkManager.ZoneRetirementOffscreenPosition;
			}
			bool flag2 = !destroyWhenComplete && heavyProxy != null && heavyProxy.UsesStagedPooledActivation;
			bool flag3 = !destroyWhenComplete && component != null && component.HasSnapshot;
			int num = 3;
			if (flag2)
			{
				heavyProxy.BeginStagedSleep();
				num = Mathf.Max(1, heavyProxy.StagedSleepWorkRemaining);
			}
			else if (flag3)
			{
				this.CancelIncrementalNormalZoneWake(component);
				component.BeginStagedSleep();
				num = Mathf.Max(1, component.StagedSleepWorkRemaining);
			}
			ChunkManager.PendingZoneRetirement pendingZoneRetirement = new ChunkManager.PendingZoneRetirement
			{
				zoneRoot = zoneRoot,
				prefab = prefab,
				destroyWhenComplete = destroyWhenComplete,
				holdsSpecialReservation = holdSpecialReservation,
				heavyProxy = heavyProxy,
				heavyProxyAuthoredEnabled = heavyProxyAuthoredEnabled,
				usesStagedHeavyRetirement = flag2,
				stagedNormalProxy = component,
				usesStagedNormalRetirement = flag3,
				remainingWorkGroups = num,
				enqueuedAtRealtime = Time.realtimeSinceStartupAsDouble
			};
			if (!flag2 && !flag3)
			{
				pendingZoneRetirement.hierarchy.Add(zoneRoot.transform);
			}
			this._pendingZoneRetirements.Enqueue(pendingZoneRetirement);
			this.AddZoneRetirementWork(pendingZoneRetirement.remainingWorkGroups);
			this._retiringZoneRoots.Add(zoneRoot);
			if (holdSpecialReservation)
			{
				this._retiringSpecialZoneReservationCount++;
			}
			if (destroyWhenComplete)
			{
				this._zoneInstanceToPrefabMap.Remove(zoneRoot);
			}
			else if (prefab != null)
			{
				int num2;
				this._retiringZoneCountByPrefab.TryGetValue(prefab, out num2);
				this._retiringZoneCountByPrefab[prefab] = num2 + 1;
			}
		}
	}

	// Token: 0x0600032A RID: 810 RVA: 0x000151F0 File Offset: 0x000133F0
	private void ProcessZoneRetirementQueue()
	{
		this.ZoneRetirementGroupsProcessedLastFrame = 0;
		this.ZoneRetirementDestroyGroupsProcessedLastFrame = 0;
		if (this._pendingZoneRetirements.Count == 0)
		{
			return;
		}
		int num = (this._retiringCompletedRun ? 256 : Mathf.Max(1, this.maxZoneRetirementGroupsPerFrame));
		int num2 = (this._retiringCompletedRun ? 128 : Mathf.Max(1, this.maxZoneDestructionGroupsPerFrame));
		double num3 = (this._retiringCompletedRun ? 3.0 : Math.Max(0.1, (double)this.zoneRetirementFrameBudgetMs));
		double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
		using (ChunkManager.ZoneRetirementProcessMarker.Auto())
		{
			while (this._pendingZoneRetirements.Count > 0 && this.ZoneRetirementGroupsProcessedLastFrame < num && (this.ZoneRetirementGroupsProcessedLastFrame == 0 || (Time.realtimeSinceStartupAsDouble - realtimeSinceStartupAsDouble) * 1000.0 < num3))
			{
				bool flag = this._reusableRetirementPriorityTokens > 0;
				ChunkManager.PendingZoneRetirement pendingZoneRetirement;
				if (!this.TryDequeueProcessableZoneRetirement(flag, num2, out pendingZoneRetirement) && !this.TryDequeueProcessableZoneRetirement(!flag, num2, out pendingZoneRetirement))
				{
					break;
				}
				bool flag3;
				bool flag2 = this.ProcessOneZoneRetirementGroup(pendingZoneRetirement, out flag3);
				this.ConsumeZoneRetirementWork(pendingZoneRetirement, 1);
				int num4 = this.ZoneRetirementGroupsProcessedLastFrame;
				this.ZoneRetirementGroupsProcessedLastFrame = num4 + 1;
				if (flag3)
				{
					num4 = this.ZoneRetirementDestroyGroupsProcessedLastFrame;
					this.ZoneRetirementDestroyGroupsProcessedLastFrame = num4 + 1;
				}
				if (pendingZoneRetirement.destroyWhenComplete)
				{
					this._reusableRetirementPriorityTokens = 3;
				}
				else if (this._reusableRetirementPriorityTokens > 0)
				{
					this._reusableRetirementPriorityTokens--;
				}
				if (flag2)
				{
					if (pendingZoneRetirement.remainingWorkGroups > 0)
					{
						this.ConsumeZoneRetirementWork(pendingZoneRetirement, pendingZoneRetirement.remainingWorkGroups);
					}
					this.CompleteZoneRetirement(pendingZoneRetirement);
				}
				else
				{
					this._pendingZoneRetirements.Enqueue(pendingZoneRetirement);
				}
			}
		}
	}

	// Token: 0x0600032B RID: 811 RVA: 0x000153C0 File Offset: 0x000135C0
	private bool TryDequeueProcessableZoneRetirement(bool reusable, int destroyGroupLimit, out ChunkManager.PendingZoneRetirement selected)
	{
		int count = this._pendingZoneRetirements.Count;
		for (int i = 0; i < count; i++)
		{
			ChunkManager.PendingZoneRetirement pendingZoneRetirement = this._pendingZoneRetirements.Dequeue();
			bool flag = reusable != pendingZoneRetirement.destroyWhenComplete;
			bool flag2 = pendingZoneRetirement.destroyRootScheduled && pendingZoneRetirement.zoneRoot != null;
			bool flag3 = ChunkManager.WillProcessZoneDestructionGroup(pendingZoneRetirement) && this.ZoneRetirementDestroyGroupsProcessedLastFrame >= destroyGroupLimit;
			if (flag && !flag2 && !flag3)
			{
				selected = pendingZoneRetirement;
				return true;
			}
			this._pendingZoneRetirements.Enqueue(pendingZoneRetirement);
		}
		selected = null;
		return false;
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0001544C File Offset: 0x0001364C
	private static bool WillProcessZoneDestructionGroup(ChunkManager.PendingZoneRetirement retirement)
	{
		return retirement.destroyWhenComplete && retirement.preparationComplete && retirement.hierarchyIndex >= 0 && !retirement.destroyRootScheduled;
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00015474 File Offset: 0x00013674
	private bool ProcessOneZoneRetirementGroup(ChunkManager.PendingZoneRetirement retirement, out bool processedDestroyGroup)
	{
		processedDestroyGroup = false;
		GameObject zoneRoot = retirement.zoneRoot;
		if (zoneRoot == null)
		{
			return true;
		}
		if (retirement.usesStagedHeavyRetirement)
		{
			HeavyZoneStreamingProxy heavyProxy = retirement.heavyProxy;
			if (heavyProxy != null)
			{
				heavyProxy.ProcessOneStagedSleepStep();
			}
			if (heavyProxy != null && heavyProxy.StagedSleepWorkRemaining > 0)
			{
				return false;
			}
			if (zoneRoot.activeSelf)
			{
				using (ChunkManager.ZonePoolDeactivateMarker.Auto())
				{
					zoneRoot.SetActive(false);
				}
			}
			retirement.rootDeactivated = true;
			return true;
		}
		else if (retirement.usesStagedNormalRetirement)
		{
			IncrementalPooledZoneActivation stagedNormalProxy = retirement.stagedNormalProxy;
			if (stagedNormalProxy != null)
			{
				stagedNormalProxy.ProcessOneStagedSleepStep();
			}
			if (stagedNormalProxy != null && stagedNormalProxy.StagedSleepWorkRemaining > 0)
			{
				return false;
			}
			if (zoneRoot.activeSelf)
			{
				using (ChunkManager.ZonePoolDeactivateMarker.Auto())
				{
					zoneRoot.SetActive(false);
				}
			}
			retirement.rootDeactivated = true;
			return true;
		}
		else
		{
			if (!retirement.preparationComplete)
			{
				using (ChunkManager.ZoneRetirementPrepareMarker.Auto())
				{
					if (retirement.discoveryIndex < retirement.hierarchy.Count)
					{
						List<Transform> hierarchy = retirement.hierarchy;
						int num = retirement.discoveryIndex;
						retirement.discoveryIndex = num + 1;
						Transform transform = hierarchy[num];
						if (ChunkManager.IsOwnedZoneRetirementTransform(transform, zoneRoot.transform))
						{
							for (int i = 0; i < transform.childCount; i++)
							{
								retirement.hierarchy.Add(transform.GetChild(i));
								retirement.remainingWorkGroups += 2;
								this.AddZoneRetirementWork(2);
							}
						}
					}
					if (retirement.discoveryIndex >= retirement.hierarchy.Count)
					{
						retirement.preparationComplete = true;
						retirement.hierarchyIndex = retirement.hierarchy.Count - 1;
					}
				}
				return false;
			}
			if (retirement.hierarchyIndex >= 0)
			{
				List<Transform> hierarchy2 = retirement.hierarchy;
				int num = retirement.hierarchyIndex;
				retirement.hierarchyIndex = num - 1;
				Transform transform2 = hierarchy2[num];
				if (transform2 == null)
				{
					return false;
				}
				if (!ChunkManager.IsOwnedZoneRetirementTransform(transform2, zoneRoot.transform))
				{
					return false;
				}
				GameObject gameObject = transform2.gameObject;
				if (retirement.destroyWhenComplete)
				{
					bool flag = gameObject == zoneRoot;
					if (gameObject.activeSelf)
					{
						if (flag)
						{
							using (ChunkManager.ZonePoolDeactivateMarker.Auto())
							{
								gameObject.SetActive(false);
								goto IL_0263;
							}
						}
						gameObject.SetActive(false);
					}
					IL_0263:
					retirement.rootDeactivated = retirement.rootDeactivated || flag;
					retirement.destroyRootScheduled = retirement.destroyRootScheduled || flag;
					processedDestroyGroup = true;
					ChunkManager.DestroyRetiredZoneGroup(gameObject);
					return false;
				}
				if (!gameObject.activeInHierarchy)
				{
					if (gameObject == zoneRoot)
					{
						retirement.rootDeactivated = true;
						retirement.restoreObjectIndex = retirement.deactivatedObjects.Count - 1;
					}
					return false;
				}
				if (gameObject == zoneRoot)
				{
					using (ChunkManager.ZonePoolDeactivateMarker.Auto())
					{
						zoneRoot.SetActive(false);
					}
					retirement.rootDeactivated = true;
					retirement.restoreObjectIndex = retirement.deactivatedObjects.Count - 1;
				}
				else
				{
					gameObject.SetActive(false);
					retirement.deactivatedObjects.Add(gameObject);
					retirement.remainingWorkGroups++;
					this.AddZoneRetirementWork(1);
				}
				return false;
			}
			else
			{
				if (!retirement.rootDeactivated)
				{
					using (ChunkManager.ZonePoolDeactivateMarker.Auto())
					{
						zoneRoot.SetActive(false);
					}
					retirement.rootDeactivated = true;
					retirement.restoreObjectIndex = retirement.deactivatedObjects.Count - 1;
					return false;
				}
				if (retirement.restoreObjectIndex >= 0)
				{
					List<GameObject> deactivatedObjects = retirement.deactivatedObjects;
					int num = retirement.restoreObjectIndex;
					retirement.restoreObjectIndex = num - 1;
					GameObject gameObject2 = deactivatedObjects[num];
					if (gameObject2 != null)
					{
						gameObject2.SetActive(true);
					}
					return false;
				}
				return true;
			}
		}
	}

	// Token: 0x0600032E RID: 814 RVA: 0x00015880 File Offset: 0x00013A80
	private void AddZoneRetirementWork(int groups)
	{
		if (groups <= 0)
		{
			return;
		}
		this._pendingZoneRetirementWorkGroups += groups;
		this.PeakPendingZoneRetirementWorkGroups = Mathf.Max(this.PeakPendingZoneRetirementWorkGroups, this._pendingZoneRetirementWorkGroups);
		if (this._pendingZoneRetirementWorkGroups >= Mathf.Max(1, this.maxPendingZoneRetirementWorkGroups))
		{
			this.IsZoneRetirementBackpressured = true;
		}
	}

	// Token: 0x0600032F RID: 815 RVA: 0x000158D4 File Offset: 0x00013AD4
	private void ConsumeZoneRetirementWork(ChunkManager.PendingZoneRetirement retirement, int groups)
	{
		int num = Mathf.Min(Mathf.Max(0, groups), retirement.remainingWorkGroups);
		retirement.remainingWorkGroups -= num;
		this._pendingZoneRetirementWorkGroups = Mathf.Max(0, this._pendingZoneRetirementWorkGroups - num);
		if (this.IsZoneRetirementBackpressured && this._pendingZoneRetirementWorkGroups <= Mathf.Max(1, this.maxPendingZoneRetirementWorkGroups / 2))
		{
			this.IsZoneRetirementBackpressured = false;
		}
	}

	// Token: 0x06000330 RID: 816 RVA: 0x0001593C File Offset: 0x00013B3C
	private float GetOldestZoneRetirementAgeMs()
	{
		if (this._pendingZoneRetirements.Count == 0)
		{
			return 0f;
		}
		double num = double.MaxValue;
		foreach (ChunkManager.PendingZoneRetirement pendingZoneRetirement in this._pendingZoneRetirements)
		{
			num = Math.Min(num, pendingZoneRetirement.enqueuedAtRealtime);
		}
		return (float)Math.Max(0.0, (Time.realtimeSinceStartupAsDouble - num) * 1000.0);
	}

	// Token: 0x06000331 RID: 817 RVA: 0x000159D4 File Offset: 0x00013BD4
	private static bool IsOwnedZoneRetirementTransform(Transform candidate, Transform zoneRoot)
	{
		return candidate != null && zoneRoot != null && (candidate == zoneRoot || candidate.IsChildOf(zoneRoot));
	}

	// Token: 0x06000332 RID: 818 RVA: 0x000159F7 File Offset: 0x00013BF7
	private static void DestroyRetiredZoneGroup(GameObject group)
	{
		if (group == null)
		{
			return;
		}
		group.transform.SetParent(null, true);
		Object.Destroy(group);
	}

	// Token: 0x06000333 RID: 819 RVA: 0x00015A18 File Offset: 0x00013C18
	private void CompleteZoneRetirement(ChunkManager.PendingZoneRetirement retirement)
	{
		GameObject zoneRoot = retirement.zoneRoot;
		GameObject prefab = retirement.prefab;
		this._retiringZoneRoots.Remove(zoneRoot);
		int num;
		if (!retirement.destroyWhenComplete && prefab != null && this._retiringZoneCountByPrefab.TryGetValue(prefab, out num))
		{
			if (num <= 1)
			{
				this._retiringZoneCountByPrefab.Remove(prefab);
			}
			else
			{
				this._retiringZoneCountByPrefab[prefab] = num - 1;
			}
		}
		if (retirement.holdsSpecialReservation)
		{
			this._retiringSpecialZoneReservationCount = Mathf.Max(0, this._retiringSpecialZoneReservationCount - 1);
		}
		if (retirement.destroyWhenComplete || zoneRoot == null)
		{
			this._zoneInstanceToPrefabMap.Remove(zoneRoot);
			return;
		}
		using (ChunkManager.ZoneRetirementFinalizeMarker.Auto())
		{
			if (prefab == null || this._zonePoolContainer == null)
			{
				this._zoneInstanceToPrefabMap.Remove(zoneRoot);
				Object.Destroy(zoneRoot);
			}
			else
			{
				if (retirement.heavyProxy != null)
				{
					retirement.heavyProxy.enabled = retirement.heavyProxyAuthoredEnabled;
				}
				zoneRoot.transform.position = ChunkManager.ZoneRetirementOffscreenPosition;
				this.ReturnZoneInstanceToPool(zoneRoot, prefab);
			}
		}
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00015B50 File Offset: 0x00013D50
	private int GetRetiringZoneCount(GameObject prefab)
	{
		int num;
		if (!(prefab != null) || !this._retiringZoneCountByPrefab.TryGetValue(prefab, out num))
		{
			return 0;
		}
		return num;
	}

	// Token: 0x06000335 RID: 821 RVA: 0x00015B7C File Offset: 0x00013D7C
	private int GetTotalRetiringZonePoolCount()
	{
		int num = 0;
		foreach (int num2 in this._retiringZoneCountByPrefab.Values)
		{
			num += num2;
		}
		return num;
	}

	// Token: 0x06000336 RID: 822 RVA: 0x00015BD4 File Offset: 0x00013DD4
	private void RegisterPooledZoneInstance(GameObject instance, GameObject prefab)
	{
		if (instance == null || prefab == null)
		{
			return;
		}
		this._zoneInstanceToPrefabMap[instance] = prefab;
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00015BF8 File Offset: 0x00013DF8
	private void ReturnZoneInstanceToPool(GameObject instance, GameObject prefab)
	{
		Queue<GameObject> queue;
		if (!this._zonePoolByPrefab.TryGetValue(prefab, out queue))
		{
			queue = new Queue<GameObject>();
			this._zonePoolByPrefab[prefab] = queue;
		}
		queue.Enqueue(instance);
		this._zonePoolCountByPrefab[prefab] = queue.Count;
		this._zonePoolRevision++;
	}

	// Token: 0x06000338 RID: 824 RVA: 0x00015C50 File Offset: 0x00013E50
	private int GetZonePoolCount(GameObject prefab)
	{
		if (prefab == null)
		{
			return 0;
		}
		int num;
		if (this._zonePoolCountByPrefab.TryGetValue(prefab, out num))
		{
			return num;
		}
		return 0;
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00015C7C File Offset: 0x00013E7C
	private static int GetTotalPoolCount(Dictionary<GameObject, Queue<GameObject>> pools)
	{
		int num = 0;
		foreach (Queue<GameObject> queue in pools.Values)
		{
			if (queue != null)
			{
				num += queue.Count;
			}
		}
		return num;
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00015CD8 File Offset: 0x00013ED8
	private void RegisterPooledInstance(GameObject instance, GameObject prefab)
	{
		if (instance == null || prefab == null)
		{
			return;
		}
		this._instanceToPrefabMap[instance] = prefab;
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00015CFC File Offset: 0x00013EFC
	private void ReturnInstanceToPool(GameObject instance, GameObject prefab)
	{
		Queue<GameObject> queue;
		if (!this._chunkPoolByPrefab.TryGetValue(prefab, out queue))
		{
			queue = new Queue<GameObject>();
			this._chunkPoolByPrefab[prefab] = queue;
		}
		queue.Enqueue(instance);
		this._poolCountByPrefab[prefab] = queue.Count;
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00015D48 File Offset: 0x00013F48
	private int GetPoolCount(GameObject prefab)
	{
		if (prefab == null)
		{
			return 0;
		}
		int num;
		if (this._poolCountByPrefab.TryGetValue(prefab, out num))
		{
			return num;
		}
		return 0;
	}

	// Token: 0x0600033D RID: 829 RVA: 0x00015D73 File Offset: 0x00013F73
	private bool IsRuntimeChunk(ChunkInstance chunk)
	{
		return ((chunk != null) ? chunk.chunkRoot : null) != null && this._instanceToPrefabMap.ContainsKey(chunk.chunkRoot);
	}

	// Token: 0x0600033E RID: 830 RVA: 0x00015D9C File Offset: 0x00013F9C
	private void DestroySpawnedZones(ChunkInstance chunk, bool holdSpecialReservation, bool retireOffscreenImmediately)
	{
		if (((chunk != null) ? chunk.spawnedZones : null) == null)
		{
			return;
		}
		for (int i = 0; i < chunk.spawnedZones.Count; i++)
		{
			GameObject gameObject = chunk.spawnedZones[i];
			if (gameObject != null)
			{
				this.ReleaseZoneRootInternal(gameObject, holdSpecialReservation, retireOffscreenImmediately);
			}
		}
		chunk.spawnedZones.Clear();
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00015DF8 File Offset: 0x00013FF8
	private void ClearChunkCellState(Vector2Int cell)
	{
		this._visuallyHiddenChunks.Remove(cell);
		this._chunkVisualCaches.Remove(cell);
		this._chunkLightingAvailability.Remove(cell);
		this._chunkDeactivationCounters.Remove(cell);
		this._pendingPostSpawnInitSet.Remove(cell);
		this._pendingActivationSet.Remove(cell);
		this._pendingDeactivationSet.Remove(cell);
	}

	// Token: 0x06000340 RID: 832 RVA: 0x00015E60 File Offset: 0x00014060
	private void Update()
	{
		using (ChunkManager.UpdateMarker.Auto())
		{
			if (!this.IsPoiTravelSuspended)
			{
				if (this._mapInitialized)
				{
					if (!this.IsPreparationFocusSelectionPending)
					{
						this.ProcessPendingChunkZonesReadyNotifications();
						if (!(PlayerProximityProvider.Instance == null))
						{
							Stopwatch stopwatch = null;
							if (this.enableVerboseDiagnostics)
							{
								stopwatch = Stopwatch.StartNew();
							}
							PlayerProximityProvider instance = PlayerProximityProvider.Instance;
							Vector2Int streamingPlayerCell = this.StreamingPlayerCell;
							bool flag = this.UpdateStreamingTravelDirection(streamingPlayerCell);
							Vector3 vector = (this.HasPreparationFocus ? this._preparationFocusPosition : instance.Position);
							this.UpdatePoiTravelDroughtState(vector, streamingPlayerCell);
							this.UpdateHeavyPoiMotionEstimate(vector);
							this.MaybePrefetchHeavyPoiAssets(streamingPlayerCell, flag, instance.Forward);
							bool flag2 = !this._cellToChunkMap.ContainsKey(streamingPlayerCell);
							int num = Mathf.Min(Mathf.Max(0, this.minVisibleRadius), Mathf.Max(0, this.spawnRadius));
							bool flag3 = this.HasUnprocessedCellsInRadius(streamingPlayerCell, num);
							bool flag4 = flag2 || flag3;
							if (flag4)
							{
								this.ProcessChunkLifecycleQueue(streamingPlayerCell);
							}
							this.EnsureNearChunkVisibility(streamingPlayerCell);
							this.EnsureNearColliders(streamingPlayerCell);
							this.UpdateHeavyZoneStreamingPriorities(streamingPlayerCell);
							this.ProcessZoneStreamingLifecycleQueues();
							this.ProcessDeferredStreamingQueues();
							if (this._visibilityJobScheduled)
							{
								this.CompleteVisibilityJob();
							}
							if (Time.time - this._lastVisibilityCheck >= this.visibilityCheckInterval)
							{
								this._lastVisibilityCheck = Time.time;
								if (this.useJobs && this._spawnedChunks.Count > 0)
								{
									this.ScheduleVisibilityJob(streamingPlayerCell);
								}
								else
								{
									this.UpdateChunkVisibilityFallback(streamingPlayerCell);
								}
							}
							this.ProcessVisibilityQueues();
							float num2 = (this.IsLoadingBoostActive ? 0f : 0.1f);
							bool flag5 = this.HasUnprocessedCellsInRadius(streamingPlayerCell, this.spawnRadius);
							bool flag6 = this.HasUnprocessedCellsInDirectionalCoreFrontier(streamingPlayerCell, num);
							if (!flag4 && (flag2 || flag5 || flag6 || flag || Time.time - this._lastSpawnCheck >= num2))
							{
								this._lastSpawnCheck = Time.time;
								this.ProcessChunkLifecycleQueue(streamingPlayerCell);
							}
							this.UpdateChunkLightingAvailability(streamingPlayerCell);
							if (this.colliderActiveRadius > 0 && Time.time - this._lastColliderToggleCheck >= this.colliderToggleInterval)
							{
								this._lastColliderToggleCheck = Time.time;
								this.UpdateFarColliderToggle(streamingPlayerCell);
							}
							if (stopwatch != null)
							{
								stopwatch.Stop();
								this._selfUpdateAccumMs += stopwatch.Elapsed.TotalMilliseconds;
								this._selfUpdateAccumCount++;
								this._selfUpdateLogTimer += Time.unscaledDeltaTime;
								if (this._selfUpdateLogTimer >= 1f && this._selfUpdateAccumCount > 0)
								{
									double num3 = this._selfUpdateAccumMs / (double)this._selfUpdateAccumCount;
									Debug.Log(string.Format("[ChunkMgr.Update] avg over {0} frames = {1:F3}ms", this._selfUpdateAccumCount, num3));
									this._selfUpdateAccumMs = 0.0;
									this._selfUpdateAccumCount = 0;
									this._selfUpdateLogTimer = 0f;
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000341 RID: 833 RVA: 0x00016160 File Offset: 0x00014360
	private void ProcessZoneStreamingLifecycleQueues()
	{
		this.NormalZoneWakeOperationsLastFrame = 0;
		this.NormalZoneWakePrioritySelectionsLastFrame = 0;
		this.NormalZoneWakeElapsedMsLastFrame = 0.0;
		bool flag = this._pendingZoneRetirements.Count > 0;
		bool flag2 = this.HasEligibleIncrementalNormalZoneWake();
		if (flag && flag2)
		{
			if (this.IsZoneRetirementBackpressured)
			{
				this._urgentZoneRetirementContentionTurns = 0;
				this.ProcessIncrementalNormalZoneWakeQueue();
				this.ProcessZoneRetirementQueue();
				return;
			}
			int num = ((this._pendingChunkDespawns.Count > Mathf.Max(2, this.maxChunkDespawnsPerFrame * 2)) ? 1 : 2);
			if (this._urgentZoneRetirementContentionTurns < num)
			{
				this._urgentZoneRetirementContentionTurns++;
				this.ZoneRetirementGroupsProcessedLastFrame = 0;
				this.ZoneRetirementDestroyGroupsProcessedLastFrame = 0;
				this.ProcessIncrementalNormalZoneWakeQueue();
				return;
			}
			this._urgentZoneRetirementContentionTurns = 0;
			this.ProcessZoneRetirementQueue();
			return;
		}
		else
		{
			if (flag)
			{
				this._urgentZoneRetirementContentionTurns = 0;
				this.ProcessZoneRetirementQueue();
				return;
			}
			this._urgentZoneRetirementContentionTurns = 0;
			this.ZoneRetirementGroupsProcessedLastFrame = 0;
			this.ZoneRetirementDestroyGroupsProcessedLastFrame = 0;
			if (flag2)
			{
				this.ProcessIncrementalNormalZoneWakeQueue();
			}
			return;
		}
	}

	// Token: 0x06000342 RID: 834 RVA: 0x0001624C File Offset: 0x0001444C
	private void EnsureNearColliders(Vector2Int playerCell)
	{
		if (this.colliderActiveRadius <= 0)
		{
			return;
		}
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				Vector2Int vector2Int;
				ChunkInstance chunkInstance;
				if (this._cellToChunkMap.TryGetValue(new Vector2Int(playerCell.x + i, playerCell.y + j), out vector2Int) && this._spawnedChunks.TryGetValue(vector2Int, out chunkInstance) && !(((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null) && chunkInstance.chunkRoot.activeInHierarchy && (!chunkInstance.collidersCurrentlyEnabled || chunkInstance.collidersCacheDirty || !ChunkManager.HasEnabledActiveCollider(chunkInstance.cachedColliders)))
				{
					if (chunkInstance.collidersCacheDirty || chunkInstance.cachedColliders == null)
					{
						using (ChunkManager.EnsureNearCollidersCacheRebuildMarker.Auto())
						{
							ChunkManager.RebuildChunkColliderCache(chunkInstance);
						}
					}
					int num = 0;
					foreach (Collider collider in chunkInstance.cachedColliders)
					{
						if (collider != null && !collider.enabled)
						{
							collider.enabled = true;
							num++;
						}
					}
					this.SetStagedHeavyColliderState(chunkInstance, true);
					chunkInstance.collidersCurrentlyEnabled = true;
					if (this.enableVerboseDiagnostics && num > 0)
					{
						Debug.Log(string.Format("[CollDiag] near-pass: riaccesi {0} collider su {1} (dist {2})", num, chunkInstance.chunkRoot.name, Mathf.Max(Mathf.Abs(i), Mathf.Abs(j))));
					}
				}
			}
		}
	}

	// Token: 0x06000343 RID: 835 RVA: 0x000163EC File Offset: 0x000145EC
	private static bool HasEnabledActiveCollider(Collider[] colliders)
	{
		if (colliders == null)
		{
			return false;
		}
		foreach (Collider collider in colliders)
		{
			if (collider != null && collider.enabled && collider.gameObject.activeInHierarchy)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000344 RID: 836 RVA: 0x00016434 File Offset: 0x00014634
	private void UpdateFarColliderToggle(Vector2Int playerCell)
	{
		int colliderActiveRadius = this.colliderActiveRadius;
		int count = this._spawnedChunks.Count;
		if (count == 0)
		{
			return;
		}
		if (this.enableVerboseDiagnostics)
		{
			Vector2Int vector2Int;
			ChunkInstance chunkInstance;
			if (!this._cellToChunkMap.TryGetValue(playerCell, out vector2Int))
			{
				Debug.LogWarning(string.Format("[CollDiag] NESSUN chunk sotto il player (cell={0})", playerCell));
			}
			else if (this._spawnedChunks.TryGetValue(vector2Int, out chunkInstance) && ((chunkInstance != null) ? chunkInstance.chunkRoot : null) != null && this.IsRuntimeChunk(chunkInstance) && !chunkInstance.collidersCurrentlyEnabled)
			{
				Debug.LogWarning(string.Format("[CollDiag] chunk sotto il player con COLLIDER SPENTI (cell={0}, chunk={1})", playerCell, chunkInstance.chunkRoot.name));
			}
		}
		if (this._collToggleMembershipVersion != this._spawnedChunkMembershipVersion)
		{
			this._collToggleKeys.Clear();
			foreach (Vector2Int vector2Int2 in this._spawnedChunks.Keys)
			{
				this._collToggleKeys.Add(vector2Int2);
			}
			if (this._collToggleCursor >= this._collToggleKeys.Count)
			{
				this._collToggleCursor = 0;
			}
			this._collToggleMembershipVersion = this._spawnedChunkMembershipVersion;
		}
		Stopwatch stopwatch = Stopwatch.StartNew();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		while (num3 < count && num < 6 && num2 < 1500 && stopwatch.Elapsed.TotalMilliseconds < 1.0)
		{
			num3++;
			Vector2Int vector2Int3 = this._collToggleKeys[this._collToggleCursor];
			this._collToggleCursor = (this._collToggleCursor + 1) % this._collToggleKeys.Count;
			ChunkInstance chunkInstance2;
			if (this._spawnedChunks.TryGetValue(vector2Int3, out chunkInstance2) && chunkInstance2 != null && !(chunkInstance2.chunkRoot == null) && chunkInstance2.chunkRoot.activeInHierarchy)
			{
				Vector2Int originCell = chunkInstance2.originCell;
				ChunkData chunkData = chunkInstance2.chunkData;
				Vector2Int vector2Int4 = ((chunkData != null) ? chunkData.gridSize : Vector2Int.one);
				int num4 = ChunkManager.ChebyshevStepsToChunk(originCell, vector2Int4, playerCell);
				bool flag = num4 <= colliderActiveRadius || ChunkManager.HasActivePoiResidencyUse(chunkInstance2);
				if (chunkInstance2.collidersCurrentlyEnabled != flag || chunkInstance2.collidersCacheDirty)
				{
					if (chunkInstance2.collidersCacheDirty || chunkInstance2.cachedColliders == null)
					{
						ChunkManager.RebuildChunkColliderCache(chunkInstance2);
					}
					Collider[] cachedColliders = chunkInstance2.cachedColliders;
					int num5 = 0;
					foreach (Collider collider in cachedColliders)
					{
						if (collider != null && collider.enabled != flag)
						{
							collider.enabled = flag;
							num2++;
							num5++;
						}
					}
					this.SetStagedHeavyColliderState(chunkInstance2, flag);
					if (this.enableVerboseDiagnostics && flag && num5 > 0 && num4 <= 1)
					{
						Debug.LogWarning(string.Format("[CollDiag] collider riaccesi in RITARDO su {0} (dist {1} celle, {2} collider)", chunkInstance2.chunkRoot.name, num4, num5));
					}
					chunkInstance2.collidersCurrentlyEnabled = flag;
					num++;
				}
			}
		}
	}

	// Token: 0x06000345 RID: 837 RVA: 0x0001673C File Offset: 0x0001493C
	private void ProcessChunkLifecycleQueue(Vector2Int playerCell)
	{
		using (ChunkManager.ChunkLifecycleMarker.Auto())
		{
			if (this.enableRuntimeChunkDespawn)
			{
				this.QueueFarRuntimeChunksForDespawn(playerCell);
			}
			if (!this.useSequentialChunkLifecycleQueue)
			{
				this.TrySpawnChunksAroundPlayer(playerCell);
				int num = Mathf.Min(Mathf.Max(0, this.minVisibleRadius), Mathf.Max(0, this.spawnRadius));
				bool flag = this.HasUnprocessedCellsInRadius(playerCell, num);
				if (this.enableRuntimeChunkDespawn && !flag)
				{
					this.ProcessChunkDespawnQueue(playerCell, this.maxChunkDespawnsPerFrame);
				}
				this.MaybeFireInitialGenerationComplete(playerCell);
				this.MaybeFireFullGameplayReady(playerCell);
			}
			else
			{
				int i = Mathf.Max(1, this.effMaxChunkLifecycleOpsPerTick);
				while (i > 0)
				{
					bool flag2 = !this._cellToChunkMap.ContainsKey(playerCell);
					bool flag3 = this.HasChunkSpawnCandidate(playerCell);
					bool flag4 = this.enableRuntimeChunkDespawn && this._pendingChunkDespawns.Count > 0;
					int num2 = Mathf.Min(Mathf.Max(0, this.minVisibleRadius), Mathf.Max(0, this.spawnRadius));
					if (this.HasUnprocessedCellsInRadius(playerCell, num2))
					{
						if (!flag3 || !this.TrySpawnChunksAroundPlayer(playerCell))
						{
							break;
						}
						i--;
					}
					else if (flag3 && (flag2 || !flag4 || this._preferChunkSpawnWhenContended) && this.TrySpawnChunksAroundPlayer(playerCell))
					{
						i--;
						if (flag4 && !flag2)
						{
							this._preferChunkSpawnWhenContended = false;
						}
					}
					else
					{
						if (flag4)
						{
							int num3 = Mathf.Min(Mathf.Max(1, this.maxChunkDespawnsPerFrame), i);
							int num4 = this.ProcessChunkDespawnQueue(playerCell, num3);
							if (num4 > 0)
							{
								i -= num4;
								this._preferChunkSpawnWhenContended = true;
								continue;
							}
						}
						if (!flag3 || !this.TrySpawnChunksAroundPlayer(playerCell))
						{
							break;
						}
						i--;
						if (flag4 && !flag2)
						{
							this._preferChunkSpawnWhenContended = false;
						}
					}
				}
				this.MaybeFireInitialGenerationComplete(playerCell);
				this.MaybeFireFullGameplayReady(playerCell);
			}
		}
	}

	// Token: 0x06000346 RID: 838 RVA: 0x00016928 File Offset: 0x00014B28
	private void MaybeFireInitialGenerationComplete(Vector2Int playerCell)
	{
		if (this._initialGenerationComplete)
		{
			return;
		}
		if (!this.IsInitialPresentationAreaReady(playerCell))
		{
			return;
		}
		this._initialGenerationComplete = true;
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[ChunkManager] Area iniziale visibile pronta: {0} chunk presenti.", this._spawnedChunks.Count));
		}
		try
		{
			Action onInitialGenerationComplete = this.OnInitialGenerationComplete;
			if (onInitialGenerationComplete != null)
			{
				onInitialGenerationComplete();
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("[ChunkManager] OnInitialGenerationComplete handler threw: " + ex.Message);
		}
	}

	// Token: 0x06000347 RID: 839 RVA: 0x000169B4 File Offset: 0x00014BB4
	private void MaybeFireFullGameplayReady(Vector2Int playerCell)
	{
		if (this._fullGameplayReady || !this._initialGenerationComplete)
		{
			return;
		}
		if (this._prewarmPoolCoroutine != null)
		{
			return;
		}
		if (!this.IsAreaReady(playerCell, this.spawnRadius))
		{
			return;
		}
		this._fullGameplayReady = true;
		this._initializationPhase = "ready";
		this._poolPrewarmProgress = 1f;
		this._warmRenderProgress = 1f;
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[ChunkManager] MapReady completo: {0} chunk, pool e warm render pronti.", this._spawnedChunks.Count));
		}
		try
		{
			Action onMapReady = this._onMapReady;
			if (onMapReady != null)
			{
				onMapReady();
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("[ChunkManager] OnMapReady handler threw: " + ex.Message);
		}
	}

	// Token: 0x06000348 RID: 840 RVA: 0x00016A78 File Offset: 0x00014C78
	private bool IsInitialPresentationAreaReady(Vector2Int playerCell)
	{
		int num = Mathf.Clamp(this.maxVisibleRadius, 1, this.spawnRadius);
		return this.IsAreaReady(playerCell, num);
	}

	// Token: 0x06000349 RID: 841 RVA: 0x00016AA0 File Offset: 0x00014CA0
	private bool IsAreaReady(Vector2Int playerCell, int radius)
	{
		for (int i = -radius; i <= radius; i++)
		{
			for (int j = -radius; j <= radius; j++)
			{
				Vector2Int vector2Int = new Vector2Int(playerCell.x + i, playerCell.y + j);
				Vector2Int vector2Int2;
				if (!this._cellToChunkMap.TryGetValue(vector2Int, out vector2Int2))
				{
					if (!this._processedCells.Contains(vector2Int))
					{
						return false;
					}
				}
				else
				{
					ChunkInstance chunkInstance;
					if (!this._spawnedChunks.TryGetValue(vector2Int2, out chunkInstance) || ((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null)
					{
						return false;
					}
					if (this._pendingZoneCountByChunk.ContainsKey(chunkInstance))
					{
						return false;
					}
					if (this._pendingHeavyZoneCountByChunk.ContainsKey(chunkInstance))
					{
						return false;
					}
					if (this._pendingPostSpawnInitSet.Contains(vector2Int2))
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x0600034A RID: 842 RVA: 0x00016B68 File Offset: 0x00014D68
	private float CalculateAreaReadinessProgress(Vector2Int playerCell, int radius)
	{
		int num = (radius * 2 + 1) * (radius * 2 + 1);
		int num2 = 0;
		for (int i = -radius; i <= radius; i++)
		{
			for (int j = -radius; j <= radius; j++)
			{
				Vector2Int vector2Int = new Vector2Int(playerCell.x + i, playerCell.y + j);
				Vector2Int vector2Int2;
				ChunkInstance chunkInstance;
				if (!this._cellToChunkMap.TryGetValue(vector2Int, out vector2Int2))
				{
					if (this._processedCells.Contains(vector2Int))
					{
						num2++;
					}
				}
				else if (this._spawnedChunks.TryGetValue(vector2Int2, out chunkInstance) && ((chunkInstance != null) ? chunkInstance.chunkRoot : null) != null && !this._pendingZoneCountByChunk.ContainsKey(chunkInstance) && !this._pendingHeavyZoneCountByChunk.ContainsKey(chunkInstance) && !this._pendingPostSpawnInitSet.Contains(vector2Int2))
				{
					num2++;
				}
			}
		}
		if (num <= 0)
		{
			return 1f;
		}
		return (float)num2 / (float)num;
	}

	// Token: 0x0600034B RID: 843 RVA: 0x00016C50 File Offset: 0x00014E50
	public string GetInitialReadinessDiagnostic()
	{
		if (PlayerProximityProvider.Instance == null)
		{
			return "player-provider=null";
		}
		Vector2Int currentCell = PlayerProximityProvider.Instance.CurrentCell;
		int num = Mathf.Clamp(this.maxVisibleRadius, 1, this.spawnRadius);
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		HashSet<ChunkInstance> hashSet = new HashSet<ChunkInstance>();
		for (int i = -num; i <= num; i++)
		{
			for (int j = -num; j <= num; j++)
			{
				Vector2Int vector2Int = new Vector2Int(currentCell.x + i, currentCell.y + j);
				Vector2Int vector2Int2;
				ChunkInstance chunkInstance;
				if (!this._cellToChunkMap.TryGetValue(vector2Int, out vector2Int2))
				{
					if (!this._processedCells.Contains(vector2Int))
					{
						num2++;
					}
				}
				else if (!this._spawnedChunks.TryGetValue(vector2Int2, out chunkInstance) || ((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null)
				{
					num3++;
				}
				else
				{
					if (hashSet.Add(chunkInstance) && (this._pendingZoneCountByChunk.ContainsKey(chunkInstance) || this._pendingHeavyZoneCountByChunk.ContainsKey(chunkInstance)))
					{
						num4++;
					}
					if (this._pendingPostSpawnInitSet.Contains(vector2Int2))
					{
						num5++;
					}
				}
			}
		}
		return string.Concat(new string[]
		{
			string.Format("initialReady={0}, fullReady={1}, ", this._initialGenerationComplete, this._fullGameplayReady),
			string.Format("prewarmActive={0}, spawnAreaReady={1}, ", this._prewarmPoolCoroutine != null, this.IsAreaReady(currentCell, this.spawnRadius)),
			string.Format("initializationPhase={0}, workVersion={1}, ", this._initializationPhase, this._initializationWorkVersion),
			string.Format("bakeRequests={0}, bakePending={1}, ", this._meshColliderBakeRequests.Count, this._meshColliderBakePending.Count),
			string.Format("bakeWave={0}, bakeJobScheduled={1}, ", this._meshColliderBakeWave.Count, this._meshColliderBakeJobScheduled),
			string.Format("spawned={0}, unresolvedVisibleCells={1}, ", this._spawnedChunks.Count, num2),
			string.Format("missingVisibleChunks={0}, visibleChunksWithZonesPending={1}, ", num3, num4),
			string.Format("visiblePostInitPending={0}, totalZoneQueue={1}, ", num5, this._pendingZoneSpawns.Count),
			string.Format("totalPostInitQueue={0}", this._pendingPostSpawnInit.Count)
		});
	}

	// Token: 0x0600034C RID: 844 RVA: 0x00016ECC File Offset: 0x000150CC
	private void LateUpdate()
	{
		if (this._visibilityJobScheduled)
		{
			this.CompleteVisibilityJob();
		}
	}

	// Token: 0x0600034D RID: 845 RVA: 0x00016EDC File Offset: 0x000150DC
	public void ResetForNewRun()
	{
		if (!this._mapInitialized || this._spawnResolver == null || this.IsTearingDown)
		{
			return;
		}
		this._completedRunRetired = false;
		this.SuspendForPoiTravel();
		List<Vector2Int> list = new List<Vector2Int>(this._spawnedChunks.Keys);
		this._specialZoneReservationCells.Clear();
		foreach (Vector2Int vector2Int in list)
		{
			ChunkInstance chunkInstance;
			if (this._spawnedChunks.TryGetValue(vector2Int, out chunkInstance) && !(((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null))
			{
				chunkInstance.collidersCurrentlyEnabled = true;
				this.DespawnRuntimeChunk(vector2Int, chunkInstance);
			}
		}
		this.ClearCompletedRunState();
		int num = global::UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		num = RunModeService.ResolveWorldSeedForCurrentRun(num, ChunkManager.DiagnosticSessionSeedOverride);
		this._spawnResolver = new ChunkSpawnResolver(num, this.availableChunks, this.biomeManager, RunModeService.HasSeededFreeWorld, 0.8f);
		this.BeginPoiResidencySession();
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[ChunkManager] New run city seed={0}; resident pools retained.", num));
		}
	}

	// Token: 0x0600034E RID: 846 RVA: 0x00017000 File Offset: 0x00015200
	public IEnumerator RetireCompletedRun()
	{
		while (this._retiringCompletedRun && !this.IsTearingDown)
		{
			yield return null;
		}
		if (this._completedRunRetired || !this._mapInitialized || this._spawnResolver == null || this.IsTearingDown)
		{
			yield break;
		}
		this._retiringCompletedRun = true;
		try
		{
			this.SuspendForPoiTravel();
			while (this._meshColliderBakeJobScheduled && !this._meshColliderBakeJobHandle.IsCompleted && !this.IsTearingDown)
			{
				yield return null;
			}
			if (this.IsTearingDown)
			{
				yield break;
			}
			this.CompleteMeshColliderBakeJob();
			List<Vector2Int> runtimeCells = new List<Vector2Int>(this._spawnedChunks.Keys);
			this._specialZoneReservationCells.Clear();
			int index = 0;
			while (index < runtimeCells.Count)
			{
				if (this.IsTearingDown)
				{
					yield break;
				}
				double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
				int num = 0;
				while (index < runtimeCells.Count && num < 2 && (num == 0 || (Time.realtimeSinceStartupAsDouble - realtimeSinceStartupAsDouble) * 1000.0 < 2.0))
				{
					List<Vector2Int> list = runtimeCells;
					int num2 = index;
					index = num2 + 1;
					Vector2Int vector2Int = list[num2];
					ChunkInstance chunkInstance;
					if (this._spawnedChunks.TryGetValue(vector2Int, out chunkInstance) && !(((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null))
					{
						chunkInstance.collidersCurrentlyEnabled = true;
						this.DespawnRuntimeChunk(vector2Int, chunkInstance);
						num++;
					}
				}
				this.ProcessZoneRetirementQueue();
				yield return null;
			}
			this.ClearCompletedRunState();
			while (this._pendingZoneRetirements.Count > 0)
			{
				if (this.IsTearingDown)
				{
					yield break;
				}
				this.ProcessZoneRetirementQueue();
				yield return null;
			}
			yield return null;
			this.ClearZonePrefabResolver();
			this._completedRunRetired = true;
			runtimeCells = null;
		}
		finally
		{
			this._retiringCompletedRun = false;
		}
		yield break;
		yield break;
	}

	// Token: 0x0600034F RID: 847 RVA: 0x00017010 File Offset: 0x00015210
	private void ClearCompletedRunState()
	{
		this._subwayConnections.Clear();
		this._spawnedChunks.Clear();
		this.MarkSpawnedChunkMembershipChanged();
		this._pendingZoneSpawns.Clear();
		this._pendingZoneCountByChunk.Clear();
		this._pendingHeavyZoneCountByChunk.Clear();
		this._pendingChunkZonesReadyNotifications.Clear();
		this._pendingChunkZonesReadyByChunk.Clear();
		this._zonesReadyNotifiedChunks.Clear();
		this._pendingPostSpawnInit.Clear();
		this._pendingPostSpawnInitSet.Clear();
		this._processedCells.Clear();
		this._cellToChunkMap.Clear();
		if (this._occupiedCellsNative.IsCreated)
		{
			this._occupiedCellsNative.Clear();
		}
		this.ReleaseDebugPoiReservation(this._debugPoiReservationOwner);
		this._committedSpawnPlan.Clear();
		int miniMapLayoutRevision = this.MiniMapLayoutRevision;
		this.MiniMapLayoutRevision = miniMapLayoutRevision + 1;
		this._committedSpawnCellOwners.Clear();
		this._committedSpecialExclusionOwners.Clear();
		this._unvisitedLongWayHomeStreetOrigins.Clear();
		this._longWayHomeStreetOwner = null;
		this._preparationFocusOwner = null;
		this._preparationFocusSelectionPending = false;
		this._encounteredPoiOrigins.Clear();
		this._poiOriginsReachedNearbyCore.Clear();
		this._travelOffsetPinnedSpecialOrigins.Clear();
		this._hasPresentedPoiOrigin = false;
		this._hasCountedPresentedPoiEncounter = false;
		this._poiTravelDroughtScheduler.Reset();
		this._poiTravelDroughtScheduler.ClearPoiRecency();
	}

	// Token: 0x06000350 RID: 848 RVA: 0x00017164 File Offset: 0x00015364
	public void SuspendForPoiTravel()
	{
		if (this.IsPoiTravelSuspended || this.IsTearingDown)
		{
			return;
		}
		this.IsPoiTravelSuspended = true;
		this.CompleteMeshColliderBakeJob();
		this.ReleaseDebugPoiReservation(this._debugPoiReservationOwner);
		this._preparationFocusOwner = null;
		this._preparationFocusSelectionPending = false;
		if (this._visibilityJobScheduled)
		{
			this.CompleteVisibilityJob();
		}
		this.RemoveStaticSceneChunkRegistrations();
		if (this.staticChunksContainer != null)
		{
			this.staticChunksContainer.gameObject.SetActive(false);
		}
		this.staticChunksContainer = null;
		this._pendingActivations.Clear();
		this._pendingDeactivations.Clear();
		this._pendingActivationSet.Clear();
		this._pendingDeactivationSet.Clear();
		this._pendingChunkDespawns.Clear();
		this._pendingChunkDespawnSet.Clear();
		this._hasLastStreamingPlayerCell = false;
		this._streamingTravelDirection = Vector2Int.zero;
		this._poiTravelDroughtScheduler.Reset();
		this.ResetHeavyPoiPrefetchPrediction();
		this._initialGenerationComplete = false;
		this._fullGameplayReady = false;
		this._lastVisibilityCheck = 0f;
		this._lastSpawnCheck = 0f;
		this._lastColliderToggleCheck = 0f;
		this.EnsureRuntimeWorldRoot().gameObject.SetActive(false);
		if (this.enableDebugLogs)
		{
			Debug.Log("[ChunkManager] Procedural world suspended for POI travel; " + string.Format("{0} runtime chunks retained.", this._spawnedChunks.Count));
		}
	}

	// Token: 0x06000351 RID: 849 RVA: 0x000172BC File Offset: 0x000154BC
	public void ResumeAfterPoiTravel(Transform newStaticChunksContainer)
	{
		this.staticChunksContainer = newStaticChunksContainer;
		if (!this.IsPoiTravelSuspended)
		{
			return;
		}
		this.IsPoiTravelSuspended = false;
		this.EnsureRuntimeWorldRoot().gameObject.SetActive(true);
		this.RegisterPreSpawnedChunks();
		this._initialGenerationComplete = false;
		this._fullGameplayReady = false;
		this._hasLastStreamingPlayerCell = false;
		this._streamingTravelDirection = Vector2Int.zero;
		this._poiTravelDroughtScheduler.Reset();
		this.ResetHeavyPoiPrefetchPrediction();
		this._lastVisibilityCheck = 0f;
		this._lastSpawnCheck = 0f;
		this._lastColliderToggleCheck = 0f;
		if (PlayerProximityProvider.Instance != null)
		{
			Vector2Int currentCell = PlayerProximityProvider.Instance.CurrentCell;
			this.EnsureNearChunkVisibility(currentCell);
			this.EnsureNearColliders(currentCell);
			this._lightingMembershipVersion = -1;
			this.UpdateChunkLightingAvailability(currentCell);
		}
		Physics.SyncTransforms();
		if (this.enableDebugLogs)
		{
			Debug.Log("[ChunkManager] Procedural world resumed after POI travel; " + string.Format("{0} chunks registered.", this._spawnedChunks.Count));
		}
	}

	// Token: 0x06000352 RID: 850 RVA: 0x000173B4 File Offset: 0x000155B4
	private void RemoveStaticSceneChunkRegistrations()
	{
		List<Vector2Int> list = new List<Vector2Int>();
		foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._spawnedChunks)
		{
			if (!this.IsRuntimeChunk(keyValuePair.Value))
			{
				list.Add(keyValuePair.Key);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		List<Vector2Int> list2 = new List<Vector2Int>();
		for (int i = 0; i < list.Count; i++)
		{
			Vector2Int vector2Int = list[i];
			ChunkInstance chunkInstance;
			if (this._spawnedChunks.TryGetValue(vector2Int, out chunkInstance))
			{
				this.ClearChunkCellState(vector2Int);
				this._spawnedChunks.Remove(vector2Int);
				list2.Clear();
				foreach (KeyValuePair<Vector2Int, Vector2Int> keyValuePair2 in this._cellToChunkMap)
				{
					if (keyValuePair2.Value == vector2Int)
					{
						list2.Add(keyValuePair2.Key);
					}
				}
				for (int j = 0; j < list2.Count; j++)
				{
					Vector2Int vector2Int2 = list2[j];
					this._processedCells.Remove(vector2Int2);
					this._cellToChunkMap.Remove(vector2Int2);
					if (this._occupiedCellsNative.IsCreated)
					{
						this._occupiedCellsNative.Remove(new int2(vector2Int2.x, vector2Int2.y));
					}
				}
				this._specialZoneReservationCells.Remove(vector2Int);
				this._pendingZoneCountByChunk.Remove(chunkInstance);
				this.UnregisterHeavyZones(chunkInstance);
				this.UnregisterIncrementalNormalZones(chunkInstance);
				this.CancelPendingChunkZonesReady(chunkInstance);
				this._zonesReadyNotifiedChunks.Remove(chunkInstance);
				this._pendingChunkDespawnSet.Remove(vector2Int);
				if (((chunkInstance != null) ? chunkInstance.chunkRoot : null) != null)
				{
					this._chunkBaseColliderCache.Remove(chunkInstance.chunkRoot);
					this._chunkBaseTransformsCache.Remove(chunkInstance.chunkRoot);
				}
				ChunkManager.ChunkEvent onChunkRemoved = this.OnChunkRemoved;
				if (onChunkRemoved != null)
				{
					onChunkRemoved(chunkInstance);
				}
			}
		}
		this.MarkSpawnedChunkMembershipChanged();
	}

	// Token: 0x06000353 RID: 851 RVA: 0x000175EC File Offset: 0x000157EC
	private void RegisterPreSpawnedChunks()
	{
		int num = 0;
		if (this.staticChunksContainer != null)
		{
			foreach (object obj in this.staticChunksContainer)
			{
				Transform transform = (Transform)obj;
				if (transform.GetComponentInChildren<SnapPoint>(true) == null && transform.GetComponent<PreSpawnedChunk>() == null)
				{
					Debug.LogWarning(string.Concat(new string[]
					{
						"[ChunkManager] '",
						transform.name,
						"' in '",
						this.staticChunksContainer.name,
						"' non ha SnapPoint né PreSpawnedChunk: NON registrato come chunk statico."
					}));
				}
				else
				{
					Vector2Int vector2Int = this.CalculateChunkSizeFromSnapPoints(transform);
					if (this.RegisterChunk(transform.gameObject, null, vector2Int))
					{
						num++;
					}
				}
			}
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[ChunkManager] Registrati {0} chunk da '{1}'", num, this.staticChunksContainer.name));
			}
			return;
		}
		foreach (PreSpawnedChunk preSpawnedChunk in Object.FindObjectsOfType<PreSpawnedChunk>())
		{
			if (this.RegisterChunk(preSpawnedChunk.gameObject, preSpawnedChunk.chunkData, preSpawnedChunk.gridSize))
			{
				num++;
			}
		}
		if (num == 0)
		{
			num = this.RegisterLegacyChunks();
		}
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[ChunkManager] Registrati {0} chunk pre-spawnati", num));
		}
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00017768 File Offset: 0x00015968
	private int RegisterLegacyChunks()
	{
		int num = 0;
		SnapPoint[] array = Object.FindObjectsOfType<SnapPoint>();
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[ChunkManager] Trovati {0} SnapPoint in scena", array.Length));
		}
		HashSet<Transform> hashSet = new HashSet<Transform>();
		foreach (SnapPoint snapPoint in array)
		{
			if (!(snapPoint == null))
			{
				Transform parent = snapPoint.transform.parent;
				if (parent == null)
				{
					if (this.enableDebugLogs)
					{
						Debug.LogWarning("[ChunkManager] SnapPoint '" + snapPoint.name + "' non ha parent");
					}
				}
				else
				{
					Transform parent2 = parent.parent;
					if (parent2 == null)
					{
						if (this.enableDebugLogs)
						{
							Debug.LogWarning("[ChunkManager] Container '" + parent.name + "' non ha parent");
						}
					}
					else if (!hashSet.Contains(parent2))
					{
						hashSet.Add(parent2);
						Vector2Int vector2Int = this.CalculateChunkSizeFromSnapPoints(parent2);
						if (this.enableDebugLogs)
						{
							Debug.Log(string.Format("[ChunkManager] Trovato chunk '{0}' size {1}x{2}", parent2.name, vector2Int.x, vector2Int.y));
						}
						if (this.RegisterChunk(parent2.gameObject, null, vector2Int))
						{
							num++;
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06000355 RID: 853 RVA: 0x000178B8 File Offset: 0x00015AB8
	private Vector2Int CalculateChunkSizeFromSnapPoints(Transform chunkRoot)
	{
		Transform transform = chunkRoot.Find("SnapPoints");
		if (transform == null)
		{
			foreach (object obj in chunkRoot)
			{
				Transform transform2 = (Transform)obj;
				if (transform2.name.Contains("SnapPoint"))
				{
					transform = transform2.parent;
					break;
				}
			}
		}
		if (transform == null)
		{
			return Vector2Int.one;
		}
		Vector3 vector = Vector3.positiveInfinity;
		Vector3 vector2 = Vector3.negativeInfinity;
		int num = 0;
		foreach (object obj2 in transform)
		{
			Transform transform3 = (Transform)obj2;
			if (transform3.GetComponent<SnapPoint>() != null)
			{
				Vector3 position = transform3.position;
				vector = Vector3.Min(vector, position);
				vector2 = Vector3.Max(vector2, position);
				num++;
			}
		}
		if (num < 2)
		{
			return Vector2Int.one;
		}
		float num2 = vector2.x - vector.x;
		float num3 = vector2.z - vector.z;
		int num4 = Mathf.Max(1, Mathf.RoundToInt(num2 / this.cellSize));
		int num5 = Mathf.Max(1, Mathf.RoundToInt(num3 / this.cellSize));
		return new Vector2Int(num4, num5);
	}

	// Token: 0x06000356 RID: 854 RVA: 0x00017A28 File Offset: 0x00015C28
	private bool RegisterChunk(GameObject chunkObj, ChunkData data, Vector2Int size)
	{
		Vector2Int vector2Int = this.WorldToCell(chunkObj.transform.position);
		if (this._spawnedChunks.ContainsKey(vector2Int))
		{
			if (this.enableDebugLogs)
			{
				Debug.LogWarning(string.Format("[ChunkManager] Cella {0} già occupata", vector2Int));
			}
			return false;
		}
		ChunkInstance chunkInstance = new ChunkInstance(data, chunkObj.transform.position, chunkObj.transform.rotation);
		chunkInstance.chunkRoot = chunkObj;
		chunkInstance.originCell = vector2Int;
		this._visuallyHiddenChunks.Remove(vector2Int);
		this.EnqueuePostSpawnInitialization(vector2Int);
		this.MarkSpawnedChunkMembershipChanged();
		this._spawnedChunks[vector2Int] = chunkInstance;
		this.MarkCellsOccupied(vector2Int, size);
		if (ChunkManager.ChunkContainsSpecialZone(chunkInstance))
		{
			if (this._specialZoneReservationCells.Count > 0 && !this._specialZoneReservationCells.Contains(vector2Int))
			{
				Debug.LogWarning("[ChunkManager] More than one pre-spawned special zone is active; procedural specials will remain blocked until only one remains.");
			}
			this._specialZoneReservationCells.Add(vector2Int);
		}
		this.NotifyGraphManager(chunkInstance, size);
		Action<GameObject> onContentRootReady = this.OnContentRootReady;
		if (onContentRootReady != null)
		{
			onContentRootReady(chunkObj);
		}
		ChunkManager.ChunkEvent onChunkSpawned = this.OnChunkSpawned;
		if (onChunkSpawned != null)
		{
			onChunkSpawned(chunkInstance);
		}
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[ChunkManager] Registrato chunk '{0}' at {1}", chunkObj.name, vector2Int));
		}
		this.RecordCompletedInitializationWork();
		return true;
	}

	// Token: 0x06000357 RID: 855 RVA: 0x00017B60 File Offset: 0x00015D60
	private void QueueFarRuntimeChunksForDespawn(Vector2Int playerCell)
	{
		foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._spawnedChunks)
		{
			Vector2Int key = keyValuePair.Key;
			ChunkInstance value = keyValuePair.Value;
			if (ChunkManager.HasActivePoiResidencyUse(value))
			{
				this._hasLastStreamingPlayerCell = false;
			}
			else
			{
				ChunkData chunkData = value.chunkData;
				Vector2Int vector2Int = ((chunkData != null) ? chunkData.gridSize : Vector2Int.one);
				int num = ChunkManager.ChebyshevStepsToChunk(key, vector2Int, playerCell);
				if (num > this.maxVisibleRadius && this.IsChunkCurrentlyVisible(key, value) && this._pendingDeactivationSet.Add(key))
				{
					this._pendingDeactivations.Enqueue(key);
				}
				if (this.IsRuntimeChunk(value) && !this._pendingChunkDespawnSet.Contains(key) && num > this.runtimeDespawnRadius)
				{
					this._pendingChunkDespawns.Enqueue(key);
					this._pendingChunkDespawnSet.Add(key);
				}
			}
		}
	}

	// Token: 0x06000358 RID: 856 RVA: 0x00017C64 File Offset: 0x00015E64
	private int ProcessChunkDespawnQueue(Vector2Int playerCell, int maxOps)
	{
		if (this.IsZoneRetirementBackpressured)
		{
			int zoneRetirementDespawnDeferrals = this.ZoneRetirementDespawnDeferrals;
			this.ZoneRetirementDespawnDeferrals = zoneRetirementDespawnDeferrals + 1;
			return 0;
		}
		if (maxOps <= 0)
		{
			return 0;
		}
		int num = 0;
		while (this._pendingChunkDespawns.Count > 0 && num < maxOps)
		{
			Vector2Int vector2Int = this._pendingChunkDespawns.Dequeue();
			this._pendingChunkDespawnSet.Remove(vector2Int);
			ChunkInstance chunkInstance;
			if (this._spawnedChunks.TryGetValue(vector2Int, out chunkInstance) && !(((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null) && this.IsRuntimeChunk(chunkInstance))
			{
				ChunkData chunkData = chunkInstance.chunkData;
				Vector2Int vector2Int2 = ((chunkData != null) ? chunkData.gridSize : Vector2Int.one);
				if (ChunkManager.ChebyshevStepsToChunk(vector2Int, vector2Int2, playerCell) > this.runtimeDespawnRadius)
				{
					if (ChunkManager.HasActivePoiResidencyUse(chunkInstance))
					{
						this._hasLastStreamingPlayerCell = false;
					}
					else
					{
						this.DespawnRuntimeChunk(vector2Int, chunkInstance);
						num++;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06000359 RID: 857 RVA: 0x00017D3C File Offset: 0x00015F3C
	private static bool HasActivePoiResidencyUse(ChunkInstance chunk)
	{
		if (SubwayTravelService.IsPreparingDepartureChunk(chunk))
		{
			return true;
		}
		if (((chunk != null) ? chunk.spawnedZones : null) == null)
		{
			return false;
		}
		foreach (GameObject gameObject in chunk.spawnedZones)
		{
			PoiResidencyPlacement poiResidencyPlacement;
			if (gameObject != null && gameObject.TryGetComponent<PoiResidencyPlacement>(out poiResidencyPlacement) && poiResidencyPlacement.HasActiveGameplayUse)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600035A RID: 858 RVA: 0x00017DC4 File Offset: 0x00015FC4
	private void DespawnRuntimeChunk(Vector2Int cell, ChunkInstance chunk)
	{
		ChunkData chunkData = chunk.chunkData;
		Vector2Int vector2Int = ((chunkData != null) ? chunkData.gridSize : Vector2Int.one);
		bool flag = this._specialZoneReservationCells.Contains(cell) && !this._poiOriginsReachedNearbyCore.Contains(cell);
		bool flag2 = this.colliderActiveRadius > 0 && !chunk.collidersCurrentlyEnabled;
		this.ApplyChunkVisibility(cell, chunk, true);
		this.RestoreChunkAuthoredLightDefaults(cell);
		this.ClearChunkCellState(cell);
		this.MarkSpawnedChunkMembershipChanged();
		this._spawnedChunks.Remove(cell);
		this.UnmarkCellsOccupied(cell, vector2Int);
		this.DestroySpawnedZones(chunk, flag, flag2);
		this._specialZoneReservationCells.Remove(cell);
		if (this._travelOffsetPinnedSpecialOrigins.Remove(cell))
		{
			this.UnregisterCommittedSpecialExclusion(cell, vector2Int);
			this._poiOriginsReachedNearbyCore.Remove(cell);
		}
		this._pendingZoneCountByChunk.Remove(chunk);
		this.UnregisterHeavyZones(chunk);
		this.UnregisterIncrementalNormalZones(chunk);
		this.CancelPendingChunkZonesReady(chunk);
		this._zonesReadyNotifiedChunks.Remove(chunk);
		ChunkManager.ChunkEvent onChunkRemoved = this.OnChunkRemoved;
		if (onChunkRemoved != null)
		{
			onChunkRemoved(chunk);
		}
		this.ReleaseChunkRoot(chunk.chunkRoot);
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00017ED8 File Offset: 0x000160D8
	private void UnmarkCellsOccupied(Vector2Int origin, Vector2Int size)
	{
		for (int i = 0; i < size.x; i++)
		{
			for (int j = 0; j < size.y; j++)
			{
				Vector2Int vector2Int = origin + new Vector2Int(i, j);
				this._processedCells.Remove(vector2Int);
				this._cellToChunkMap.Remove(vector2Int);
				if (this._occupiedCellsNative.IsCreated)
				{
					this._occupiedCellsNative.Remove(new int2(vector2Int.x, vector2Int.y));
				}
			}
		}
	}

	// Token: 0x0600035C RID: 860 RVA: 0x00017F60 File Offset: 0x00016160
	private bool TrySpawnChunksAroundPlayer(Vector2Int playerCell)
	{
		int num = Mathf.Min(Mathf.Max(0, this.minVisibleRadius), Mathf.Max(0, this.spawnRadius));
		List<Vector2Int> list = this.GetFreeCellsInRadius(playerCell, num);
		if (list.Count == 0 && num < this.spawnRadius)
		{
			list = this.GetFreeCellsInRadius(playerCell, this.spawnRadius);
		}
		if (list.Count == 0)
		{
			list = this.GetFreeCellsInDirectionalCoreFrontier(playerCell, num);
		}
		if (list.Count == 0)
		{
			return false;
		}
		int num2 = Mathf.Min(16, list.Count);
		for (int i = 0; i < num2; i++)
		{
			Vector2Int vector2Int = this.SelectHighestPrioritySpawnCell(list, playerCell);
			if (this.TrySpawnChunkAt(vector2Int, playerCell))
			{
				return true;
			}
			list.Remove(vector2Int);
		}
		return false;
	}

	// Token: 0x0600035D RID: 861 RVA: 0x0001800C File Offset: 0x0001620C
	private Vector2Int SelectHighestPrioritySpawnCell(List<Vector2Int> candidates, Vector2Int playerCell)
	{
		Vector2Int vector2Int = candidates[0];
		int num = int.MaxValue;
		int num2 = int.MaxValue;
		int num3 = int.MaxValue;
		int num4 = int.MinValue;
		int num5 = int.MaxValue;
		for (int i = 0; i < candidates.Count; i++)
		{
			Vector2Int vector2Int2 = candidates[i];
			int num6 = vector2Int2.x - playerCell.x;
			int num7 = vector2Int2.y - playerCell.y;
			int num8 = num6 * this._streamingTravelDirection.x + num7 * this._streamingTravelDirection.y;
			int num9;
			if (num6 == 0 && num7 == 0)
			{
				num9 = 0;
			}
			else if (this._streamingTravelDirection == Vector2Int.zero || num8 > 0)
			{
				num9 = 1;
			}
			else if (num8 == 0)
			{
				num9 = 2;
			}
			else
			{
				num9 = 3;
			}
			int num10 = Mathf.Max(Mathf.Abs(num6), Mathf.Abs(num7));
			int num11 = num6 * num6 + num7 * num7;
			int num12 = Mathf.Abs(num6 * this._streamingTravelDirection.y - num7 * this._streamingTravelDirection.x);
			if (num9 < num || (num9 == num && num10 < num2) || (num9 == num && num10 == num2 && num11 < num3) || (num9 == num && num10 == num2 && num11 == num3 && num8 > num4) || (num9 == num && num10 == num2 && num11 == num3 && num8 == num4 && num12 < num5) || (num9 == num && num10 == num2 && num11 == num3 && num8 == num4 && num12 == num5 && (vector2Int2.x < vector2Int.x || (vector2Int2.x == vector2Int.x && vector2Int2.y < vector2Int.y))))
			{
				vector2Int = vector2Int2;
				num = num9;
				num2 = num10;
				num3 = num11;
				num4 = num8;
				num5 = num12;
			}
		}
		return vector2Int;
	}

	// Token: 0x0600035E RID: 862 RVA: 0x000181E0 File Offset: 0x000163E0
	private bool HasChunkSpawnCandidate(Vector2Int playerCell)
	{
		if (this.HasUnprocessedCellsInRadius(playerCell, this.spawnRadius))
		{
			return true;
		}
		int num = Mathf.Min(Mathf.Max(0, this.minVisibleRadius), Mathf.Max(0, this.spawnRadius));
		return this.HasUnprocessedCellsInDirectionalCoreFrontier(playerCell, num);
	}

	// Token: 0x0600035F RID: 863 RVA: 0x00018224 File Offset: 0x00016424
	private bool HasUnprocessedCellsInDirectionalCoreFrontier(Vector2Int playerCell, int coreRadius)
	{
		if (this._streamingTravelDirection == Vector2Int.zero)
		{
			return false;
		}
		Vector2Int vector2Int = playerCell + this._streamingTravelDirection;
		for (int i = -coreRadius; i <= coreRadius; i++)
		{
			for (int j = -coreRadius; j <= coreRadius; j++)
			{
				Vector2Int vector2Int2 = vector2Int + new Vector2Int(i, j);
				if (!ChunkManager.IsInsideCore(vector2Int2, playerCell, coreRadius) && !this._cellToChunkMap.ContainsKey(vector2Int2))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000360 RID: 864 RVA: 0x00018298 File Offset: 0x00016498
	private List<Vector2Int> GetFreeCellsInDirectionalCoreFrontier(Vector2Int playerCell, int coreRadius)
	{
		this._freeCellsCache.Clear();
		if (this._streamingTravelDirection == Vector2Int.zero)
		{
			return this._freeCellsCache;
		}
		Vector2Int vector2Int = playerCell + this._streamingTravelDirection;
		for (int i = -coreRadius; i <= coreRadius; i++)
		{
			for (int j = -coreRadius; j <= coreRadius; j++)
			{
				Vector2Int vector2Int2 = vector2Int + new Vector2Int(i, j);
				if (!ChunkManager.IsInsideCore(vector2Int2, playerCell, coreRadius) && !this._cellToChunkMap.ContainsKey(vector2Int2))
				{
					this._freeCellsCache.Add(vector2Int2);
				}
			}
		}
		return this._freeCellsCache;
	}

	// Token: 0x06000361 RID: 865 RVA: 0x00018328 File Offset: 0x00016528
	private static bool IsInsideCore(Vector2Int cell, Vector2Int center, int radius)
	{
		return Mathf.Abs(cell.x - center.x) <= radius && Mathf.Abs(cell.y - center.y) <= radius;
	}

	// Token: 0x06000362 RID: 866 RVA: 0x00018360 File Offset: 0x00016560
	private void MaybePrefetchHeavyPoiAssets(Vector2Int playerCell, bool playerCellChanged, Vector3 playerForward)
	{
		if (!this.enableAdaptiveHeavyPoiPrefetch || this._spawnResolver == null)
		{
			return;
		}
		Vector2 heavyPoiPrefetchMotionDirection = this._heavyPoiPrefetchMotionDirection;
		if (heavyPoiPrefetchMotionDirection.sqrMagnitude < 0.01f)
		{
			if (this._streamingTravelDirection != Vector2Int.zero)
			{
				heavyPoiPrefetchMotionDirection = new Vector2((float)this._streamingTravelDirection.x, (float)this._streamingTravelDirection.y);
			}
			else
			{
				heavyPoiPrefetchMotionDirection = new Vector2(playerForward.x, playerForward.z);
			}
		}
		Vector2Int vector2Int = ChunkManager.QuantizeHeavyPoiPrefetchDirection(heavyPoiPrefetchMotionDirection);
		if (vector2Int == Vector2Int.zero)
		{
			return;
		}
		int num = ChunkManager.CalculateHeavyPoiPrefetchLeadCells(this._heavyPoiPrefetchEstimatedSpeed, this.cellSize, this.heavyPoiPrefetchTargetLeadSeconds, this.heavyPoiPrefetchMinLeadCells, this.heavyPoiPrefetchMaxLeadCells);
		this._heavyPoiPrefetchLeadCells = num;
		bool flag = !this._hasHeavyPoiPrefetchSignature || playerCell != this._lastHeavyPoiPrefetchCell || vector2Int != this._lastHeavyPoiPrefetchDirection || num != this._lastHeavyPoiPrefetchLeadCells;
		if (!playerCellChanged)
		{
			if (!flag)
			{
				return;
			}
			if (Time.unscaledTime < this._nextHeavyPoiPrefetchSignatureSweepTime)
			{
				return;
			}
		}
		this._hasHeavyPoiPrefetchSignature = true;
		this._lastHeavyPoiPrefetchCell = playerCell;
		this._lastHeavyPoiPrefetchDirection = vector2Int;
		this._lastHeavyPoiPrefetchLeadCells = num;
		this._nextHeavyPoiPrefetchSignatureSweepTime = Time.unscaledTime + 0.35f;
		this.PrefetchHeavyPoiAssetsAhead(playerCell, vector2Int, num);
	}

	// Token: 0x06000363 RID: 867 RVA: 0x00018498 File Offset: 0x00016698
	private void PrefetchHeavyPoiAssetsAhead(Vector2Int playerCell, Vector2Int direction, int leadCells)
	{
		if (this._spawnResolver == null || direction == Vector2Int.zero)
		{
			return;
		}
		int heavyPoiPrefetchSweepCount = this.HeavyPoiPrefetchSweepCount;
		this.HeavyPoiPrefetchSweepCount = heavyPoiPrefetchSweepCount + 1;
		this._heavyPoiPrefetchCellScratch.Clear();
		this._heavyPoiPrefetchPathScratch.Clear();
		Vector2Int vector2Int = new Vector2Int(-direction.y, direction.x);
		int num = Mathf.Max(1, leadCells) + Mathf.Max(0, this.heavyPoiPrefetchFootprintPaddingCells);
		int num2 = Mathf.Max(0, this.heavyPoiPrefetchLateralCells) * 2 + 1;
		int num3 = ((direction.x != 0 && direction.y != 0) ? 2 : 1);
		bool flag = false;
		int num4 = 1;
		while (num4 <= num && !flag)
		{
			Vector2Int vector2Int2 = playerCell + direction * num4;
			int num5 = 0;
			while (num5 < num2 && !flag)
			{
				int centerOutLateralOffset = ChunkManager.GetCenterOutLateralOffset(num5);
				Vector2Int vector2Int3 = vector2Int2 + vector2Int * centerOutLateralOffset;
				int num6 = 0;
				while (num6 < num3 && !flag)
				{
					Vector2Int vector2Int4 = vector2Int3 + ChunkManager.GetHeavyPoiPrefetchDiagonalLaneOffset(direction, num6);
					flag = this.TryPrefetchHeavyPoiAssetAtCell(vector2Int4);
					num6++;
				}
				num5++;
			}
			num4++;
		}
		this._heavyPoiPrefetchCellScratch.Clear();
		this._heavyPoiPrefetchPathScratch.Clear();
	}

	// Token: 0x06000364 RID: 868 RVA: 0x000185D0 File Offset: 0x000167D0
	private bool TryPrefetchHeavyPoiAssetAtCell(Vector2Int previewCell)
	{
		if (!this._heavyPoiPrefetchCellScratch.Add(previewCell))
		{
			return false;
		}
		int num = this.HeavyPoiPrefetchPreviewCellCount;
		this.HeavyPoiPrefetchPreviewCellCount = num + 1;
		ChunkSpawnResolver.SpawnEntry spawnEntry = this._spawnResolver.ResolvePreview(previewCell);
		if (spawnEntry.IsValid && spawnEntry.zoneIndices != null)
		{
			ChunkData chunkData = spawnEntry.chunkData;
			if (((chunkData != null) ? chunkData.availableZones : null) != null)
			{
				for (int i = 0; i < spawnEntry.zoneIndices.Length; i++)
				{
					int num2 = spawnEntry.zoneIndices[i];
					if (num2 >= 0 && num2 < spawnEntry.chunkData.availableZones.Count)
					{
						ZoneData zoneData = spawnEntry.chunkData.availableZones[num2];
						GameObject gameObject = this.ResolveZonePrefab(zoneData);
						HeavyZoneStreamingProxy heavyZoneStreamingProxy = ((gameObject != null) ? gameObject.GetComponent<HeavyZoneStreamingProxy>() : null);
						string text = ((heavyZoneStreamingProxy != null) ? heavyZoneStreamingProxy.ResourcesPrefabPath : null);
						if (!string.IsNullOrWhiteSpace(text) && this._heavyPoiPrefetchPathScratch.Add(text) && HeavyZoneStreamingProxy.PrefetchPayloadAsset(text))
						{
							num = this.HeavyPoiPrefetchAssetLoadStartCount;
							this.HeavyPoiPrefetchAssetLoadStartCount = num + 1;
							return true;
						}
					}
				}
				return false;
			}
		}
		return false;
	}

	// Token: 0x06000365 RID: 869 RVA: 0x000186D4 File Offset: 0x000168D4
	private void UpdatePoiTravelDroughtState(Vector3 playerPosition, Vector2Int playerCell)
	{
		float num = Mathf.Max(1f, this.cellSize) * 4f;
		if (!this.HasPreparationFocus)
		{
			this._poiTravelDroughtScheduler.ObservePosition(new Vector2(playerPosition.x, playerPosition.z), num);
		}
		if (!this._fullGameplayReady)
		{
			return;
		}
		bool flag = false;
		Vector2Int vector2Int = default(Vector2Int);
		int num2 = int.MaxValue;
		foreach (Vector2Int vector2Int2 in this._specialZoneReservationCells)
		{
			ChunkSpawnResolver.SpawnEntry spawnEntry;
			ChunkInstance chunkInstance;
			if (this._committedSpawnPlan.TryGetValue(vector2Int2, out spawnEntry) && spawnEntry.IsValid && ChunkManager.SelectionContainsSpecialZone(spawnEntry, spawnEntry.chunkData) && this._spawnedChunks.TryGetValue(vector2Int2, out chunkInstance) && ChunkManager.HasMaterializedSelectedSpecialZone(spawnEntry, chunkInstance))
			{
				ChunkData chunkData = spawnEntry.chunkData;
				Vector2Int vector2Int3 = ((chunkData != null) ? chunkData.gridSize : Vector2Int.one);
				int num3 = ChunkManager.ChebyshevStepsToChunk(vector2Int2, vector2Int3, playerCell);
				if (num3 <= 1 && (!flag || (num3 <= num2 && (num3 != num2 || ChunkManager.CompareCells(vector2Int2, vector2Int) < 0))))
				{
					flag = true;
					vector2Int = vector2Int2;
					num2 = num3;
				}
			}
		}
		bool flag2 = this._hasPresentedPoiOrigin != flag || (flag && this._presentedPoiOrigin != vector2Int);
		this._hasPresentedPoiOrigin = flag;
		this._presentedPoiOrigin = vector2Int;
		if (flag2)
		{
			this._hasCountedPresentedPoiEncounter = false;
		}
		foreach (Vector2Int vector2Int4 in this._specialZoneReservationCells)
		{
			ChunkSpawnResolver.SpawnEntry spawnEntry2;
			ChunkInstance chunkInstance2;
			if (this._committedSpawnPlan.TryGetValue(vector2Int4, out spawnEntry2) && spawnEntry2.IsValid && this._spawnedChunks.TryGetValue(vector2Int4, out chunkInstance2))
			{
				ChunkManager.SetSelectedSpecialZonePresentation(spawnEntry2, chunkInstance2, flag && vector2Int4 == vector2Int);
			}
		}
		ChunkSpawnResolver.SpawnEntry spawnEntry3;
		ChunkInstance chunkInstance3;
		if (!flag || !this._committedSpawnPlan.TryGetValue(vector2Int, out spawnEntry3) || !this._spawnedChunks.TryGetValue(vector2Int, out chunkInstance3) || !this.IsChunkCurrentlyVisible(vector2Int, chunkInstance3) || !ChunkManager.IsSelectedSpecialZonePresentationReady(spawnEntry3, chunkInstance3))
		{
			return;
		}
		this._poiOriginsReachedNearbyCore.Add(vector2Int);
		if (this.HasPreparationFocus)
		{
			return;
		}
		if (!this._hasCountedPresentedPoiEncounter)
		{
			this._poiTravelDroughtScheduler.MarkPoiEncountered();
			this._hasCountedPresentedPoiEncounter = true;
		}
		if (this._encounteredPoiOrigins.Add(vector2Int))
		{
			ProceduralPoiKind selectedPoiKind = ChunkManager.GetSelectedPoiKind(spawnEntry3, spawnEntry3.chunkData);
			if (selectedPoiKind != ProceduralPoiKind.None)
			{
				this._poiTravelDroughtScheduler.MarkPoiTypeAccepted(selectedPoiKind);
			}
		}
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0001895C File Offset: 0x00016B5C
	private static int CompareCells(Vector2Int first, Vector2Int second)
	{
		int num = first.x.CompareTo(second.x);
		if (num == 0)
		{
			return first.y.CompareTo(second.y);
		}
		return num;
	}

	// Token: 0x06000367 RID: 871 RVA: 0x0001899C File Offset: 0x00016B9C
	private void UpdateHeavyPoiMotionEstimate(Vector3 playerPosition)
	{
		float unscaledTime = Time.unscaledTime;
		if (!this._hasHeavyPoiMotionSample)
		{
			this._hasHeavyPoiMotionSample = true;
			this._lastHeavyPoiMotionPosition = playerPosition;
			this._lastHeavyPoiMotionTime = unscaledTime;
			this._heavyPoiPrefetchEstimatedSpeed = 0f;
			this._heavyPoiPrefetchMotionDirection = Vector2.zero;
			return;
		}
		float num = unscaledTime - this._lastHeavyPoiMotionTime;
		if (num <= 0.0001f)
		{
			return;
		}
		Vector3 vector = playerPosition - this._lastHeavyPoiMotionPosition;
		this._lastHeavyPoiMotionPosition = playerPosition;
		this._lastHeavyPoiMotionTime = unscaledTime;
		Vector2 vector2 = new Vector2(vector.x, vector.z);
		float magnitude = vector2.magnitude;
		float num2 = Mathf.Max(1f, this.cellSize) * 8f;
		if (!float.IsFinite(magnitude) || magnitude > num2)
		{
			this._heavyPoiPrefetchEstimatedSpeed = 0f;
			this._heavyPoiPrefetchMotionDirection = Vector2.zero;
			this._hasHeavyPoiPrefetchSignature = false;
			return;
		}
		float num3 = magnitude / num;
		if (!float.IsFinite(num3))
		{
			num3 = 0f;
		}
		float num4 = 1f - Mathf.Exp(-4f * num);
		this._heavyPoiPrefetchEstimatedSpeed = Mathf.Lerp(this._heavyPoiPrefetchEstimatedSpeed, num3, num4);
		if (magnitude > 0.01f)
		{
			Vector2 vector3 = vector2 / magnitude;
			if (this._heavyPoiPrefetchMotionDirection.sqrMagnitude < 0.0001f)
			{
				this._heavyPoiPrefetchMotionDirection = vector3;
				return;
			}
			float num5 = 1f - Mathf.Exp(-6f * num);
			Vector2 vector4 = Vector2.Lerp(this._heavyPoiPrefetchMotionDirection, vector3, num5);
			if (vector4.sqrMagnitude > 0.0001f)
			{
				this._heavyPoiPrefetchMotionDirection = vector4.normalized;
			}
		}
	}

	// Token: 0x06000368 RID: 872 RVA: 0x00018B20 File Offset: 0x00016D20
	private static int CalculateHeavyPoiPrefetchLeadCells(float estimatedSpeed, float worldCellSize, float targetLeadSeconds, int minimumLeadCells, int maximumLeadCells)
	{
		int num = Mathf.Max(1, minimumLeadCells);
		int num2 = Mathf.Max(num, maximumLeadCells);
		if (!float.IsFinite(estimatedSpeed) || !float.IsFinite(worldCellSize) || !float.IsFinite(targetLeadSeconds) || worldCellSize <= 0.001f || targetLeadSeconds <= 0f)
		{
			return num;
		}
		int num3 = Mathf.CeilToInt(Mathf.Max(0f, estimatedSpeed) * targetLeadSeconds / worldCellSize);
		return Mathf.Clamp(Mathf.Max(num, num3), num, num2);
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00018B8C File Offset: 0x00016D8C
	private static Vector2Int QuantizeHeavyPoiPrefetchDirection(Vector2 direction)
	{
		if (!float.IsFinite(direction.x) || !float.IsFinite(direction.y) || direction.sqrMagnitude < 0.0001f)
		{
			return Vector2Int.zero;
		}
		direction.Normalize();
		int num = ((Mathf.Abs(direction.x) >= 0.35f) ? ((direction.x > 0f) ? 1 : (-1)) : 0);
		int num2 = ((Mathf.Abs(direction.y) >= 0.35f) ? ((direction.y > 0f) ? 1 : (-1)) : 0);
		return new Vector2Int(num, num2);
	}

	// Token: 0x0600036A RID: 874 RVA: 0x00018C24 File Offset: 0x00016E24
	private static int GetCenterOutLateralOffset(int sampleIndex)
	{
		if (sampleIndex <= 0)
		{
			return 0;
		}
		int num = (sampleIndex + 1) / 2;
		if ((sampleIndex & 1) != 1)
		{
			return num;
		}
		return -num;
	}

	// Token: 0x0600036B RID: 875 RVA: 0x00018C47 File Offset: 0x00016E47
	private static Vector2Int GetHeavyPoiPrefetchDiagonalLaneOffset(Vector2Int direction, int laneIndex)
	{
		if (laneIndex <= 0 || direction.x == 0 || direction.y == 0)
		{
			return Vector2Int.zero;
		}
		return new Vector2Int(direction.x, 0);
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00018C74 File Offset: 0x00016E74
	private void ResetHeavyPoiPrefetchPrediction()
	{
		this._hasHeavyPoiMotionSample = false;
		this._lastHeavyPoiMotionPosition = Vector3.zero;
		this._lastHeavyPoiMotionTime = 0f;
		this._heavyPoiPrefetchEstimatedSpeed = 0f;
		this._heavyPoiPrefetchMotionDirection = Vector2.zero;
		this._heavyPoiPrefetchLeadCells = 0;
		this._hasHeavyPoiPrefetchSignature = false;
		this._lastHeavyPoiPrefetchCell = Vector2Int.zero;
		this._lastHeavyPoiPrefetchDirection = Vector2Int.zero;
		this._lastHeavyPoiPrefetchLeadCells = -1;
		this._nextHeavyPoiPrefetchSignatureSweepTime = 0f;
		this._heavyPoiPrefetchCellScratch.Clear();
		this._heavyPoiPrefetchPathScratch.Clear();
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00018D00 File Offset: 0x00016F00
	private bool HasUnprocessedCellsInRadius(Vector2Int center, int radius)
	{
		return this.CountUnprocessedCellsInRadius(center, radius) > 0;
	}

	// Token: 0x0600036E RID: 878 RVA: 0x00018D10 File Offset: 0x00016F10
	private int CountUnprocessedCellsInRadius(Vector2Int center, int radius)
	{
		int num = 0;
		for (int i = -radius; i <= radius; i++)
		{
			for (int j = -radius; j <= radius; j++)
			{
				if (!this._cellToChunkMap.ContainsKey(center + new Vector2Int(i, j)))
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x0600036F RID: 879 RVA: 0x00018D58 File Offset: 0x00016F58
	private bool TrySpawnChunkAt(Vector2Int cell, Vector2Int playerCell)
	{
		if (this._cellToChunkMap.ContainsKey(cell))
		{
			return false;
		}
		Vector2Int vector2Int;
		bool flag = this._committedSpawnCellOwners.TryGetValue(cell, out vector2Int);
		if (flag && vector2Int != cell)
		{
			return this.TrySpawnChunkAt(vector2Int, playerCell);
		}
		Vector2Int vector2Int2;
		if (!flag && this._spawnResolver != null && this._spawnResolver.TryResolveContainingSpecialChunkOrigin(cell, out vector2Int2) && vector2Int2 != cell && !this._cellToChunkMap.ContainsKey(vector2Int2))
		{
			return this.TrySpawnChunkAt(vector2Int2, playerCell);
		}
		if (this._processedCells.Remove(cell) && this._occupiedCellsNative.IsCreated)
		{
			this._occupiedCellsNative.Remove(new int2(cell.x, cell.y));
		}
		if (!this.AreCellsFree(cell, Vector2Int.one))
		{
			return false;
		}
		ChunkSpawnResolver.SpawnEntry spawnEntry;
		bool flag2 = this._committedSpawnPlan.TryGetValue(cell, out spawnEntry);
		if (!flag2)
		{
			spawnEntry = this._spawnResolver.Resolve(cell);
		}
		ChunkData chunkData = (spawnEntry.IsValid ? spawnEntry.chunkData : this.SelectChunkForCell(cell, null, false, false));
		if (!RankPoiPolicy.IsChunkAllowed(chunkData) || !RankPoiPolicy.IsSpawnAllowed(spawnEntry))
		{
			if (flag2)
			{
				this.ForgetRankRestrictedSpawnPlan(cell, spawnEntry);
			}
			spawnEntry = this._spawnResolver.ResolveSmallFallback(cell, chunkData, true);
			chunkData = (spawnEntry.IsValid ? spawnEntry.chunkData : null);
			flag2 = false;
		}
		if (chunkData != null && (!this.AreCellsFree(cell, chunkData.gridSize) || (!flag2 && !this.AreCommittedSpawnPlanCellsFree(cell, chunkData.gridSize))))
		{
			if (flag2)
			{
				return false;
			}
			ChunkData chunkData2 = chunkData;
			spawnEntry = this._spawnResolver.ResolveSmallFallback(cell, chunkData2, true);
			chunkData = (spawnEntry.IsValid ? spawnEntry.chunkData : null);
			if (this.enableVerboseDiagnostics)
			{
				Debug.Log(string.Format("[ChunkManager] '{0}' non entra in {1}; uso fallback '{2}'.", chunkData2.name, cell, ((chunkData != null) ? chunkData.name : null) ?? "nessuno"));
			}
		}
		if (ChunkSpawnResolver.IsSubwayChunk(chunkData) && this.HasAdjacentSubwayChunk(cell))
		{
			spawnEntry = this._spawnResolver.ResolveWithoutSubway(cell);
			chunkData = (spawnEntry.IsValid ? spawnEntry.chunkData : null);
		}
		this.ApplyPoiTravelHybridPolicy(cell, playerCell, flag2, ref spawnEntry, ref chunkData);
		if (!flag2 && chunkData != null && ChunkManager.SelectionContainsSpecialZone(spawnEntry, chunkData) && this.HasCommittedSpecialSpacingConflict(cell, chunkData.gridSize))
		{
			this.RebuildEntryWithoutSpecialZones(cell, ref spawnEntry, ref chunkData);
		}
		if (!flag2)
		{
			this.ApplyActivePoiLimit(cell, ref spawnEntry, ref chunkData);
		}
		if (chunkData == null)
		{
			if (this.enableDebugLogs)
			{
				Debug.LogWarning(string.Format("[ChunkManager] Nessun chunk valido per cella {0} (small chunks: {1})", cell, this._smallChunks.Count));
			}
			return false;
		}
		Vector3 vector = this.CellToWorld(cell, chunkData.gridSize);
		int num = (spawnEntry.IsValid ? spawnEntry.rotation : global::UnityEngine.Random.Range(0, 4));
		if (ChunkManager.RequiresStableTerrainOrientation(chunkData))
		{
			num = 0;
		}
		if (spawnEntry.IsValid && spawnEntry.rotation != num)
		{
			spawnEntry = new ChunkSpawnResolver.SpawnEntry(chunkData, num, spawnEntry.zoneIndices, false);
		}
		Quaternion quaternion = Quaternion.Euler(0f, (float)(num * 90), 0f);
		GameObject gameObject = this.AcquireChunkRoot(chunkData, vector, quaternion);
		if (gameObject == null)
		{
			if (this.enableDebugLogs)
			{
				Debug.LogWarning(string.Format("[ChunkManager] Acquire dal pool fallito per chunk '{0}' in cella {1}", chunkData.name, cell));
			}
			return false;
		}
		gameObject.name = string.Format("Chunk_{0}_{1}_{2}", chunkData.name, cell.x, cell.y);
		ChunkInstance chunkInstance = new ChunkInstance(chunkData, vector, quaternion);
		chunkInstance.chunkRoot = gameObject;
		chunkInstance.originCell = cell;
		chunkInstance.preservesCommittedZoneIdentity = flag2 && !this._unvisitedLongWayHomeStreetOrigins.Contains(cell);
		chunkInstance.collidersCacheDirty = true;
		this.EnqueuePostSpawnInitialization(cell);
		if (ChunkManager.SelectionContainsSpecialZone(spawnEntry, chunkData))
		{
			this._specialZoneReservationCells.Add(cell);
		}
		this.MarkSpawnedChunkMembershipChanged();
		this._spawnedChunks[cell] = chunkInstance;
		bool flag3 = this._debugPoiReservationOwner != null && this._debugPoiReservationCell == cell && !this._debugPoiReservationMaterialized;
		if (flag3)
		{
			this._debugPoiReservationMaterialized = true;
		}
		this.MarkCellsOccupied(cell, chunkData.gridSize);
		this.CommitSpawnPlanEntry(cell, spawnEntry, chunkData);
		this._unvisitedLongWayHomeStreetOrigins.Remove(cell);
		this.RecordAcceptedPoiForTravelPolicy(spawnEntry, chunkData, flag2 && !flag3);
		this.EnsureSpawnedChunkCollidersReadyBeforePhysics(chunkInstance, playerCell);
		Action<GameObject> onContentRootReady = this.OnContentRootReady;
		if (onContentRootReady != null)
		{
			onContentRootReady(gameObject);
		}
		if (this.enableZoneSpawning)
		{
			this.SpawnZonesForChunk(chunkInstance, num, spawnEntry);
		}
		bool flag4 = this.ShouldChunkBeActive(cell, chunkInstance, playerCell);
		if (flag4)
		{
			this._visuallyHiddenChunks.Remove(cell);
		}
		else
		{
			this.ApplyChunkVisibility(cell, chunkInstance, false);
		}
		this.TryNotifyChunkZonesReady(chunkInstance);
		ChunkManager.ChunkEvent onChunkSpawned = this.OnChunkSpawned;
		if (onChunkSpawned != null)
		{
			onChunkSpawned(chunkInstance);
		}
		if (!flag4)
		{
			ChunkManager.ChunkEvent onChunkDeactivated = this.OnChunkDeactivated;
			if (onChunkDeactivated != null)
			{
				onChunkDeactivated(chunkInstance);
			}
		}
		if (this.enableVerboseDiagnostics)
		{
			Debug.Log(string.Format("[ChunkManager] Spawned '{0}' at {1}", chunkData.name, cell));
		}
		this.RecordCompletedInitializationWork();
		return true;
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00019274 File Offset: 0x00017474
	private bool AreCommittedSpawnPlanCellsFree(Vector2Int origin, Vector2Int size)
	{
		for (int i = 0; i < Mathf.Max(1, size.x); i++)
		{
			for (int j = 0; j < Mathf.Max(1, size.y); j++)
			{
				Vector2Int vector2Int = origin + new Vector2Int(i, j);
				Vector2Int vector2Int2;
				if (this._committedSpawnCellOwners.TryGetValue(vector2Int, out vector2Int2) && vector2Int2 != origin)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06000371 RID: 881 RVA: 0x000192DC File Offset: 0x000174DC
	private void CommitSpawnPlanEntry(Vector2Int origin, ChunkSpawnResolver.SpawnEntry entry, ChunkData chunkData)
	{
		if (!entry.IsValid || chunkData == null)
		{
			return;
		}
		ChunkSpawnResolver.SpawnEntry spawnEntry;
		if (this._committedSpawnPlan.TryGetValue(origin, out spawnEntry) && spawnEntry.wasPresented && !entry.wasPresented)
		{
			entry = new ChunkSpawnResolver.SpawnEntry(entry.chunkData, entry.rotation, entry.zoneIndices, true);
		}
		this._committedSpawnPlan[origin] = entry;
		int miniMapLayoutRevision = this.MiniMapLayoutRevision;
		this.MiniMapLayoutRevision = miniMapLayoutRevision + 1;
		if (ChunkManager.SelectionContainsSpecialZone(entry, chunkData))
		{
			this.RegisterCommittedSpecialExclusion(origin, chunkData.gridSize);
		}
		ChunkSpawnResolver spawnResolver = this._spawnResolver;
		if (spawnResolver != null)
		{
			spawnResolver.Invalidate(origin);
		}
		Vector2Int gridSize = chunkData.gridSize;
		for (int i = 0; i < Mathf.Max(1, gridSize.x); i++)
		{
			for (int j = 0; j < Mathf.Max(1, gridSize.y); j++)
			{
				this._committedSpawnCellOwners[origin + new Vector2Int(i, j)] = origin;
			}
		}
	}

	// Token: 0x06000372 RID: 882 RVA: 0x000193D0 File Offset: 0x000175D0
	private void ForgetRankRestrictedSpawnPlan(Vector2Int origin, ChunkSpawnResolver.SpawnEntry entry)
	{
		this._committedSpawnPlan.Remove(origin);
		int miniMapLayoutRevision = this.MiniMapLayoutRevision;
		this.MiniMapLayoutRevision = miniMapLayoutRevision + 1;
		if (entry.chunkData == null)
		{
			return;
		}
		Vector2Int gridSize = entry.chunkData.gridSize;
		this.UnregisterCommittedSpecialExclusion(origin, gridSize);
		for (int i = 0; i < Mathf.Max(1, gridSize.x); i++)
		{
			for (int j = 0; j < Mathf.Max(1, gridSize.y); j++)
			{
				Vector2Int vector2Int = origin + new Vector2Int(i, j);
				Vector2Int vector2Int2;
				if (this._committedSpawnCellOwners.TryGetValue(vector2Int, out vector2Int2) && vector2Int2 == origin)
				{
					this._committedSpawnCellOwners.Remove(vector2Int);
				}
			}
		}
		this._encounteredPoiOrigins.Remove(origin);
		this._poiOriginsReachedNearbyCore.Remove(origin);
		this._travelOffsetPinnedSpecialOrigins.Remove(origin);
	}

	// Token: 0x06000373 RID: 883 RVA: 0x000194AC File Offset: 0x000176AC
	private bool HasAdjacentSubwayChunk(Vector2Int cell)
	{
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if ((i != 0 || j != 0) && ChunkManager.ChunkContainsSubway(this.GetChunkAt(cell + new Vector2Int(i, j))))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000374 RID: 884 RVA: 0x000194F4 File Offset: 0x000176F4
	private static bool ChunkContainsSubway(ChunkInstance chunk)
	{
		if (chunk == null)
		{
			return false;
		}
		if (ChunkSpawnResolver.IsSubwayChunk(chunk.chunkData))
		{
			return true;
		}
		if (chunk.chunkRoot != null && chunk.chunkRoot.GetComponentInChildren<SubwayRideDirector>(true) != null)
		{
			return true;
		}
		if (chunk.spawnedZones != null)
		{
			for (int i = 0; i < chunk.spawnedZones.Count; i++)
			{
				GameObject gameObject = chunk.spawnedZones[i];
				if (gameObject != null && gameObject.GetComponentInChildren<SubwayRideDirector>(true) != null)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000375 RID: 885 RVA: 0x00019580 File Offset: 0x00017780
	private void NotifyGraphManager(ChunkInstance chunk, Vector2Int size)
	{
		if (AStarGraphManager.Instance == null)
		{
			return;
		}
		Vector3 vector = new Vector3((float)size.x * this.cellSize, 50f, (float)size.y * this.cellSize);
		AStarGraphManager.Instance.OnChunkSpawned(chunk.worldPosition, vector);
	}

	// Token: 0x06000376 RID: 886 RVA: 0x000195D8 File Offset: 0x000177D8
	private void NotifyGraphManagerZonesReady(ChunkInstance chunk)
	{
		if (chunk == null || chunk.chunkRoot == null)
		{
			return;
		}
		if (AStarGraphManager.Instance == null)
		{
			return;
		}
		ChunkData chunkData = chunk.chunkData;
		Vector2Int vector2Int = ((chunkData != null) ? chunkData.gridSize : Vector2Int.one);
		Vector3 vector = new Vector3((float)vector2Int.x * this.cellSize, 50f, (float)vector2Int.y * this.cellSize);
		AStarGraphManager.Instance.OnChunkZonesReady(chunk.worldPosition, vector);
	}

	// Token: 0x06000377 RID: 887 RVA: 0x00019658 File Offset: 0x00017858
	private void ScheduleVisibilityJob(Vector2Int playerCell)
	{
		if (this._spawnedChunks.Count == 0)
		{
			return;
		}
		this.SyncChunkJobData();
		if (this._chunksList.Count == 0)
		{
			return;
		}
		float2 @float = new float2(0f, 1f);
		Camera main = Camera.main;
		if (main != null)
		{
			Vector3 forward = main.transform.forward;
			Vector2 vector = new Vector2(forward.x, forward.z);
			if (vector.sqrMagnitude > 0.001f)
			{
				vector.Normalize();
				@float = new float2(vector.x, vector.y);
			}
		}
		float num = Mathf.Cos((this.viewAngle + this.viewAngleBuffer) * 0.5f * 0.017453292f);
		ChunkVisibilityJob chunkVisibilityJob = new ChunkVisibilityJob
		{
			chunks = this._chunkJobData,
			playerCell = new float2((float)playerCell.x, (float)playerCell.y),
			minVisibleRadius = (float)this.minVisibleRadius,
			maxVisibleRadius = (float)this.maxVisibleRadius,
			cameraDirection = @float,
			halfViewAngleCos = num,
			results = this._visibilityResults
		};
		this._currentVisibilityJob = chunkVisibilityJob.Schedule(this._chunksList.Count, 32, default(JobHandle));
		this._visibilityJobScheduled = true;
	}

	// Token: 0x06000378 RID: 888 RVA: 0x000197AC File Offset: 0x000179AC
	private void CompleteVisibilityJob()
	{
		if (!this._visibilityJobScheduled)
		{
			return;
		}
		this._currentVisibilityJob.Complete();
		this._visibilityJobScheduled = false;
		for (int i = 0; i < this._chunksList.Count; i++)
		{
			ChunkVisibilityResult chunkVisibilityResult = this._visibilityResults[i];
			ChunkInstance chunkInstance = this._chunksList[i];
			if (!(((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null))
			{
				Vector2Int originCell = chunkInstance.originCell;
				bool flag = this.IsChunkCurrentlyVisible(originCell, chunkInstance);
				if (chunkVisibilityResult.shouldBeActive && !flag)
				{
					if (this._pendingActivationSet.Add(originCell))
					{
						this._pendingActivations.Enqueue(originCell);
					}
					this._chunkDeactivationCounters.Remove(originCell);
				}
				else if (!chunkVisibilityResult.shouldBeActive && flag)
				{
					if (!this._chunkDeactivationCounters.ContainsKey(originCell))
					{
						this._chunkDeactivationCounters[originCell] = 0;
					}
					Dictionary<Vector2Int, int> chunkDeactivationCounters = this._chunkDeactivationCounters;
					Vector2Int vector2Int = originCell;
					int num = chunkDeactivationCounters[vector2Int];
					chunkDeactivationCounters[vector2Int] = num + 1;
					if (this._chunkDeactivationCounters[originCell] >= this.deactivationDelay && this._pendingDeactivationSet.Add(originCell))
					{
						this._pendingDeactivations.Enqueue(originCell);
					}
				}
				else if (chunkVisibilityResult.shouldBeActive && flag)
				{
					this._chunkDeactivationCounters.Remove(originCell);
				}
			}
		}
	}

	// Token: 0x06000379 RID: 889 RVA: 0x000198FC File Offset: 0x00017AFC
	private void SyncChunkJobData()
	{
		if (this._chunksListMembershipVersion != this._spawnedChunkMembershipVersion)
		{
			this._chunksList.Clear();
			this._chunkIdMap.Clear();
			foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._spawnedChunks)
			{
				this._chunksList.Add(keyValuePair.Value);
				this._chunkIdMap[this._chunksList.Count - 1] = keyValuePair.Value;
			}
			if (!this._chunkJobData.IsCreated || this._chunkJobData.Length != this._chunksList.Count)
			{
				if (this._chunkJobData.IsCreated)
				{
					this._chunkJobData.Dispose();
				}
				if (this._visibilityResults.IsCreated)
				{
					this._visibilityResults.Dispose();
				}
				this._chunkJobData = new NativeArray<ChunkJobData>(this._chunksList.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this._visibilityResults = new NativeArray<ChunkVisibilityResult>(this._chunksList.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
			this._chunksListMembershipVersion = this._spawnedChunkMembershipVersion;
		}
		for (int i = 0; i < this._chunksList.Count; i++)
		{
			ChunkInstance chunkInstance = this._chunksList[i];
			if (!(((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null))
			{
				ChunkData chunkData = chunkInstance.chunkData;
				Vector2Int vector2Int = ((chunkData != null) ? chunkData.gridSize : Vector2Int.one);
				this._chunkJobData[i] = new ChunkJobData
				{
					id = i,
					cellPosition = new float2((float)chunkInstance.originCell.x, (float)chunkInstance.originCell.y),
					cellSize = new float2((float)vector2Int.x, (float)vector2Int.y),
					isCurrentlyActive = this.IsChunkCurrentlyVisible(chunkInstance.originCell, chunkInstance)
				};
			}
		}
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00019AFC File Offset: 0x00017CFC
	private void UpdateChunkVisibilityFallback(Vector2Int playerCell)
	{
		foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._spawnedChunks)
		{
			Vector2Int key = keyValuePair.Key;
			ChunkInstance value = keyValuePair.Value;
			if (!(value.chunkRoot == null))
			{
				bool flag = this.ShouldChunkBeActive(key, value, playerCell);
				bool flag2 = this.IsChunkCurrentlyVisible(key, value);
				if (flag && !flag2)
				{
					if (this._pendingActivationSet.Add(key))
					{
						this._pendingActivations.Enqueue(key);
					}
					this._chunkDeactivationCounters.Remove(key);
				}
				else if (!flag && flag2)
				{
					if (!this._chunkDeactivationCounters.ContainsKey(key))
					{
						this._chunkDeactivationCounters[key] = 0;
					}
					Dictionary<Vector2Int, int> chunkDeactivationCounters = this._chunkDeactivationCounters;
					Vector2Int vector2Int = key;
					int num = chunkDeactivationCounters[vector2Int];
					chunkDeactivationCounters[vector2Int] = num + 1;
					if (this._chunkDeactivationCounters[key] >= this.deactivationDelay && this._pendingDeactivationSet.Add(key))
					{
						this._pendingDeactivations.Enqueue(key);
					}
				}
				else if (flag && flag2)
				{
					this._chunkDeactivationCounters.Remove(key);
				}
			}
		}
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00019C4C File Offset: 0x00017E4C
	private void ProcessVisibilityQueues()
	{
		int num = 0;
		while (this._pendingActivations.Count > 0)
		{
			if (num >= this.maxVisibilityChangesPerFrame)
			{
				break;
			}
			Vector2Int vector2Int = this._pendingActivations.Dequeue();
			this._pendingActivationSet.Remove(vector2Int);
			Vector2Int streamingPlayerCell = this.StreamingPlayerCell;
			ChunkInstance chunkInstance;
			if (this._spawnedChunks.TryGetValue(vector2Int, out chunkInstance) && chunkInstance.chunkRoot != null && !this.IsChunkCurrentlyVisible(vector2Int, chunkInstance) && this.ShouldChunkBeActive(vector2Int, chunkInstance, streamingPlayerCell))
			{
				this.ApplyChunkVisibility(vector2Int, chunkInstance, true);
				num++;
				ChunkManager.ChunkEvent onChunkActivated = this.OnChunkActivated;
				if (onChunkActivated != null)
				{
					onChunkActivated(chunkInstance);
				}
			}
		}
		while (this._pendingDeactivations.Count > 0 && num < this.maxVisibilityChangesPerFrame)
		{
			Vector2Int vector2Int2 = this._pendingDeactivations.Dequeue();
			this._pendingDeactivationSet.Remove(vector2Int2);
			this._chunkDeactivationCounters.Remove(vector2Int2);
			ChunkInstance chunkInstance2;
			if (this._spawnedChunks.TryGetValue(vector2Int2, out chunkInstance2) && chunkInstance2.chunkRoot != null && this.IsChunkCurrentlyVisible(vector2Int2, chunkInstance2))
			{
				Vector2Int streamingPlayerCell2 = this.StreamingPlayerCell;
				if (!this.ShouldChunkBeActive(vector2Int2, chunkInstance2, streamingPlayerCell2))
				{
					this.ApplyChunkVisibility(vector2Int2, chunkInstance2, false);
					num++;
					ChunkManager.ChunkEvent onChunkDeactivated = this.OnChunkDeactivated;
					if (onChunkDeactivated != null)
					{
						onChunkDeactivated(chunkInstance2);
					}
				}
			}
		}
	}

	// Token: 0x0600037C RID: 892 RVA: 0x00019D98 File Offset: 0x00017F98
	private bool IsChunkCurrentlyVisible(Vector2Int cell, ChunkInstance chunk)
	{
		if (((chunk != null) ? chunk.chunkRoot : null) == null)
		{
			return false;
		}
		if (this.useSetActiveForVisibility)
		{
			return chunk.chunkRoot.activeSelf;
		}
		return chunk.chunkRoot.activeSelf && !this._visuallyHiddenChunks.Contains(cell);
	}

	// Token: 0x0600037D RID: 893 RVA: 0x00019DF0 File Offset: 0x00017FF0
	private void ApplyChunkVisibility(Vector2Int cell, ChunkInstance chunk, bool visible)
	{
		if (((chunk != null) ? chunk.chunkRoot : null) == null)
		{
			return;
		}
		if (!visible && ChunkManager.HasActivePoiResidencyUse(chunk))
		{
			return;
		}
		if (this.useSetActiveForVisibility)
		{
			if (chunk.chunkRoot.activeSelf != visible)
			{
				chunk.chunkRoot.SetActive(visible);
			}
			return;
		}
		bool flag = this._visuallyHiddenChunks.Contains(cell);
		if (visible && !flag)
		{
			return;
		}
		this.CacheChunkVisualComponents(cell, chunk, null);
		this.ApplyVisualOnlyState(cell, visible);
	}

	// Token: 0x0600037E RID: 894 RVA: 0x00019E68 File Offset: 0x00018068
	private void CacheChunkVisualComponents(Vector2Int cell, ChunkInstance chunk, ChunkManager.ChunkVisualCache previousHiddenCache = null)
	{
		using (ChunkManager.CacheChunkVisualComponentsMarker.Auto())
		{
			if (!(((chunk != null) ? chunk.chunkRoot : null) == null))
			{
				if (!this._chunkVisualCaches.ContainsKey(cell))
				{
					Renderer[] chunkOwnedComponents = ChunkManager.GetChunkOwnedComponents<Renderer>(chunk.chunkRoot);
					LODGroup[] chunkOwnedComponents2 = ChunkManager.GetChunkOwnedComponents<LODGroup>(chunk.chunkRoot);
					Light[] chunkOwnedComponents3 = ChunkManager.GetChunkOwnedComponents<Light>(chunk.chunkRoot);
					bool[] array = ChunkManager.FindWantedOwnedRenderers(chunk.chunkRoot, chunkOwnedComponents);
					Terrain[] chunkOwnedComponents4 = ChunkManager.GetChunkOwnedComponents<Terrain>(chunk.chunkRoot);
					ChunkManager.ChunkVisualCache chunkVisualCache = new ChunkManager.ChunkVisualCache();
					chunkVisualCache.renderers = chunkOwnedComponents;
					chunkVisualCache.rendererDefaults = ChunkManager.PreserveHiddenVisualDefaults<Renderer>(chunkOwnedComponents, (previousHiddenCache != null) ? previousHiddenCache.renderers : null, (previousHiddenCache != null) ? previousHiddenCache.rendererDefaults : null, (Renderer component) => component.enabled);
					chunkVisualCache.rendererUsesIndependentEnabledState = array;
					chunkVisualCache.rendererForceRenderingOffDefaults = ChunkManager.PreserveHiddenVisualDefaults<Renderer>(chunkOwnedComponents, (previousHiddenCache != null) ? previousHiddenCache.renderers : null, (previousHiddenCache != null) ? previousHiddenCache.rendererForceRenderingOffDefaults : null, (Renderer component) => component.forceRenderingOff);
					chunkVisualCache.lodGroups = chunkOwnedComponents2;
					chunkVisualCache.lodGroupDefaults = ChunkManager.PreserveHiddenVisualDefaults<LODGroup>(chunkOwnedComponents2, (previousHiddenCache != null) ? previousHiddenCache.lodGroups : null, (previousHiddenCache != null) ? previousHiddenCache.lodGroupDefaults : null, (LODGroup component) => component.enabled);
					chunkVisualCache.lights = chunkOwnedComponents3;
					chunkVisualCache.lightDefaults = ChunkManager.PreserveHiddenVisualDefaults<Light>(chunkOwnedComponents3, (previousHiddenCache != null) ? previousHiddenCache.lights : null, (previousHiddenCache != null) ? previousHiddenCache.lightDefaults : null, (Light component) => component.enabled);
					chunkVisualCache.managedStreetLights = ChunkManager.FindManagedStreetLights(chunkOwnedComponents3);
					chunkVisualCache.terrains = chunkOwnedComponents4;
					chunkVisualCache.terrainDefaults = ChunkManager.PreserveHiddenVisualDefaults<Terrain>(chunkOwnedComponents4, (previousHiddenCache != null) ? previousHiddenCache.terrains : null, (previousHiddenCache != null) ? previousHiddenCache.terrainDefaults : null, (Terrain component) => component.enabled);
					ChunkManager.ChunkVisualCache chunkVisualCache2 = chunkVisualCache;
					this._chunkVisualCaches[cell] = chunkVisualCache2;
					Vector2Int vector2Int;
					if (!this._chunkLightingAvailability.ContainsKey(cell) && this.TryGetCurrentStreamingPlayerCell(out vector2Int))
					{
						ChunkData chunkData = chunk.chunkData;
						bool flag = ChunkManager.ChebyshevStepsToChunk(cell, (chunkData != null) ? chunkData.gridSize : Vector2Int.one, vector2Int) <= Mathf.Max(Mathf.Max(0, this.spawnRadius), this.minVisibleRadius);
						this._chunkLightingAvailability[cell] = flag;
					}
					this.ApplyCachedChunkLightingAvailability(cell, chunkVisualCache2);
				}
			}
		}
	}

	// Token: 0x0600037F RID: 895 RVA: 0x0001A120 File Offset: 0x00018320
	private void UpdateChunkLightingAvailability(Vector2Int playerCell)
	{
		int num = Mathf.Max(Mathf.Max(0, this.spawnRadius), this.minVisibleRadius);
		int num2 = Mathf.Max(num, this.runtimeDespawnRadius);
		if (this._lightingMembershipVersion == this._spawnedChunkMembershipVersion && this._lightingPlayerCell == playerCell && this._lightingSpawnRadius == num && this._lightingDespawnRadius == num2)
		{
			return;
		}
		this._lightingMembershipVersion = this._spawnedChunkMembershipVersion;
		this._lightingPlayerCell = playerCell;
		this._lightingSpawnRadius = num;
		this._lightingDespawnRadius = num2;
		foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._spawnedChunks)
		{
			ChunkInstance value = keyValuePair.Value;
			if (!(((value != null) ? value.chunkRoot : null) == null))
			{
				Vector2Int key = keyValuePair.Key;
				ChunkData chunkData = value.chunkData;
				int num3 = ChunkManager.ChebyshevStepsToChunk(key, (chunkData != null) ? chunkData.gridSize : Vector2Int.one, playerCell);
				bool flag2;
				bool flag = this._chunkLightingAvailability.TryGetValue(keyValuePair.Key, out flag2);
				bool flag3 = num3 <= num || (flag2 && num3 <= num2);
				bool flag4 = !flag || flag3 != flag2;
				this._chunkLightingAvailability[keyValuePair.Key] = flag3;
				ChunkManager.ChunkVisualCache chunkVisualCache;
				if (!this._chunkVisualCaches.TryGetValue(keyValuePair.Key, out chunkVisualCache))
				{
					this.CacheChunkVisualComponents(keyValuePair.Key, value, null);
				}
				else if (flag4)
				{
					this.ApplyCachedChunkLightingAvailability(keyValuePair.Key, chunkVisualCache);
				}
			}
		}
	}

	// Token: 0x06000380 RID: 896 RVA: 0x0001A2BC File Offset: 0x000184BC
	private void ApplyCachedChunkLightingAvailability(Vector2Int cell, ChunkManager.ChunkVisualCache cache)
	{
		bool flag2;
		bool flag = !this._chunkLightingAvailability.TryGetValue(cell, out flag2) || flag2;
		for (int i = 0; i < cache.lights.Length; i++)
		{
			Light light = cache.lights[i];
			if (!(light == null))
			{
				StreetLightController streetLightController = ((cache.managedStreetLights != null && i < cache.managedStreetLights.Length) ? cache.managedStreetLights[i] : null);
				if (streetLightController != null)
				{
					streetLightController.SetChunkLightingAvailable(flag);
					if (streetLightController.IsRuntimeEligible)
					{
						goto IL_0079;
					}
				}
				light.enabled = flag && cache.lightDefaults[i];
			}
			IL_0079:;
		}
	}

	// Token: 0x06000381 RID: 897 RVA: 0x0001A354 File Offset: 0x00018554
	private void RestoreZoneAuthoredVisualDefaults(Vector2Int cell, Transform zoneRoot)
	{
		ChunkManager.ChunkVisualCache chunkVisualCache;
		if (zoneRoot == null || !this._chunkVisualCaches.TryGetValue(cell, out chunkVisualCache))
		{
			return;
		}
		for (int i = 0; i < chunkVisualCache.renderers.Length; i++)
		{
			Renderer renderer = chunkVisualCache.renderers[i];
			if (!(renderer == null) && renderer.transform.IsChildOf(zoneRoot))
			{
				if (chunkVisualCache.rendererUsesIndependentEnabledState != null && i < chunkVisualCache.rendererUsesIndependentEnabledState.Length && chunkVisualCache.rendererUsesIndependentEnabledState[i])
				{
					renderer.forceRenderingOff = chunkVisualCache.rendererForceRenderingOffDefaults != null && i < chunkVisualCache.rendererForceRenderingOffDefaults.Length && chunkVisualCache.rendererForceRenderingOffDefaults[i];
				}
				else
				{
					renderer.enabled = chunkVisualCache.rendererDefaults[i];
				}
			}
		}
		for (int j = 0; j < chunkVisualCache.lodGroups.Length; j++)
		{
			LODGroup lodgroup = chunkVisualCache.lodGroups[j];
			if (lodgroup != null && lodgroup.transform.IsChildOf(zoneRoot))
			{
				lodgroup.enabled = chunkVisualCache.lodGroupDefaults[j];
			}
		}
		for (int k = 0; k < chunkVisualCache.terrains.Length; k++)
		{
			Terrain terrain = chunkVisualCache.terrains[k];
			if (terrain != null && terrain.transform.IsChildOf(zoneRoot))
			{
				terrain.enabled = chunkVisualCache.terrainDefaults[k];
			}
		}
		for (int l = 0; l < chunkVisualCache.lights.Length; l++)
		{
			Light light = chunkVisualCache.lights[l];
			if (!(light == null) && light.transform.IsChildOf(zoneRoot))
			{
				StreetLightController streetLightController = ((chunkVisualCache.managedStreetLights != null && l < chunkVisualCache.managedStreetLights.Length) ? chunkVisualCache.managedStreetLights[l] : null);
				if (streetLightController != null)
				{
					streetLightController.SetChunkLightingAvailable(true);
					if (streetLightController.IsRuntimeEligible)
					{
						goto IL_01AA;
					}
				}
				light.enabled = chunkVisualCache.lightDefaults[l];
			}
			IL_01AA:;
		}
	}

	// Token: 0x06000382 RID: 898 RVA: 0x0001A520 File Offset: 0x00018720
	private void RestoreChunkAuthoredLightDefaults(Vector2Int cell)
	{
		ChunkManager.ChunkVisualCache chunkVisualCache;
		if (!this._chunkVisualCaches.TryGetValue(cell, out chunkVisualCache))
		{
			return;
		}
		for (int i = 0; i < chunkVisualCache.lights.Length; i++)
		{
			Light light = chunkVisualCache.lights[i];
			if (!(light == null))
			{
				StreetLightController streetLightController = ((chunkVisualCache.managedStreetLights != null && i < chunkVisualCache.managedStreetLights.Length) ? chunkVisualCache.managedStreetLights[i] : null);
				if (streetLightController == null || !streetLightController.IsRuntimeEligible)
				{
					light.enabled = chunkVisualCache.lightDefaults[i];
				}
			}
		}
	}

	// Token: 0x06000383 RID: 899 RVA: 0x0001A5A4 File Offset: 0x000187A4
	private static bool[] FindWantedOwnedRenderers(GameObject chunkRoot, Renderer[] renderers)
	{
		bool[] array = new bool[renderers.Length];
		WantedPoster[] componentsInChildren = chunkRoot.GetComponentsInChildren<WantedPoster>(true);
		if (componentsInChildren.Length == 0)
		{
			return array;
		}
		HashSet<Renderer> hashSet = new HashSet<Renderer>();
		foreach (WantedPoster wantedPoster in componentsInChildren)
		{
			if (!(wantedPoster == null))
			{
				Renderer[] componentsInChildren2 = wantedPoster.GetComponentsInChildren<Renderer>(true);
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					if (componentsInChildren2[j] != null)
					{
						hashSet.Add(componentsInChildren2[j]);
					}
				}
			}
		}
		for (int k = 0; k < renderers.Length; k++)
		{
			array[k] = renderers[k] != null && hashSet.Contains(renderers[k]);
		}
		return array;
	}

	// Token: 0x06000384 RID: 900 RVA: 0x0001A654 File Offset: 0x00018854
	private static StreetLightController[] FindManagedStreetLights(Light[] lights)
	{
		StreetLightController[] array = new StreetLightController[lights.Length];
		for (int i = 0; i < lights.Length; i++)
		{
			Light light = lights[i];
			if (!(light == null))
			{
				StreetLightController component = light.GetComponent<StreetLightController>();
				array[i] = component;
			}
		}
		return array;
	}

	// Token: 0x06000385 RID: 901 RVA: 0x0001A694 File Offset: 0x00018894
	private void EnsureNearChunkVisibility(Vector2Int playerCell)
	{
		int num = Mathf.Max(0, this.minVisibleRadius);
		for (int i = -num; i <= num; i++)
		{
			for (int j = -num; j <= num; j++)
			{
				Vector2Int vector2Int = playerCell + new Vector2Int(i, j);
				Vector2Int vector2Int2;
				ChunkInstance chunkInstance;
				if (this._cellToChunkMap.TryGetValue(vector2Int, out vector2Int2) && this._spawnedChunks.TryGetValue(vector2Int2, out chunkInstance) && !(((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null) && !this.IsChunkCurrentlyVisible(vector2Int2, chunkInstance))
				{
					this.ApplyChunkVisibility(vector2Int2, chunkInstance, true);
					this._pendingActivationSet.Remove(vector2Int2);
					ChunkManager.ChunkEvent onChunkActivated = this.OnChunkActivated;
					if (onChunkActivated != null)
					{
						onChunkActivated(chunkInstance);
					}
				}
			}
		}
	}

	// Token: 0x06000386 RID: 902 RVA: 0x0001A754 File Offset: 0x00018954
	private bool UpdateStreamingTravelDirection(Vector2Int playerCell)
	{
		if (!this._hasLastStreamingPlayerCell)
		{
			this._hasLastStreamingPlayerCell = true;
			this._lastStreamingPlayerCell = playerCell;
			this._streamingTravelDirection = Vector2Int.zero;
			return true;
		}
		if (playerCell == this._lastStreamingPlayerCell)
		{
			return false;
		}
		Vector2Int vector2Int = playerCell - this._lastStreamingPlayerCell;
		this._lastStreamingPlayerCell = playerCell;
		this._streamingTravelDirection = new Vector2Int((vector2Int.x == 0) ? 0 : ((vector2Int.x > 0) ? 1 : (-1)), (vector2Int.y == 0) ? 0 : ((vector2Int.y > 0) ? 1 : (-1)));
		return true;
	}

	// Token: 0x06000387 RID: 903 RVA: 0x0001A7E8 File Offset: 0x000189E8
	private void MarkSpawnedChunkMembershipChanged()
	{
		if (this._visibilityJobScheduled)
		{
			this.CompleteVisibilityJob();
		}
		this._spawnedChunkMembershipVersion++;
		int miniMapLayoutRevision = this.MiniMapLayoutRevision;
		this.MiniMapLayoutRevision = miniMapLayoutRevision + 1;
	}

	// Token: 0x06000388 RID: 904 RVA: 0x0001A824 File Offset: 0x00018A24
	private static bool[] PreserveHiddenVisualDefaults<T>(T[] components, T[] previousComponents, bool[] previousDefaults, Func<T, bool> isEnabled) where T : Component
	{
		bool[] array = new bool[components.Length];
		Dictionary<T, bool> dictionary = null;
		if (previousComponents != null && previousDefaults != null)
		{
			int num = Mathf.Min(previousComponents.Length, previousDefaults.Length);
			dictionary = new Dictionary<T, bool>(num);
			for (int i = 0; i < num; i++)
			{
				T t = previousComponents[i];
				if (t != null)
				{
					dictionary[t] = previousDefaults[i];
				}
			}
		}
		for (int j = 0; j < components.Length; j++)
		{
			T t2 = components[j];
			bool flag;
			array[j] = ((!(t2 != null)) ? false : ((dictionary == null || !dictionary.TryGetValue(t2, out flag)) ? isEnabled(t2) : flag));
		}
		return array;
	}

	// Token: 0x06000389 RID: 905 RVA: 0x0001A8D4 File Offset: 0x00018AD4
	private static T[] GetChunkOwnedComponents<T>(GameObject chunkRoot) where T : Component
	{
		T[] componentsInChildren = chunkRoot.GetComponentsInChildren<T>(true);
		List<T> list = new List<T>(componentsInChildren.Length);
		foreach (T t in componentsInChildren)
		{
			if (!(t == null) && !(t.GetComponentInParent<NPCEquipmentManager>(true) != null) && !(t.GetComponentInParent<NPCSenses>(true) != null))
			{
				list.Add(t);
			}
		}
		return list.ToArray();
	}

	// Token: 0x0600038A RID: 906 RVA: 0x0001A94E File Offset: 0x00018B4E
	private static void RebuildChunkColliderCache(ChunkInstance chunk)
	{
		if (((chunk != null) ? chunk.chunkRoot : null) == null)
		{
			return;
		}
		chunk.cachedColliders = ChunkManager.GetChunkOwnedComponents<Collider>(chunk.chunkRoot);
		chunk.collidersCacheDirty = false;
	}

	// Token: 0x0600038B RID: 907 RVA: 0x0001A980 File Offset: 0x00018B80
	private Collider[] CacheBaseChunkColliders(GameObject chunkRoot)
	{
		if (chunkRoot == null)
		{
			return Array.Empty<Collider>();
		}
		Collider[] chunkOwnedComponents;
		if (this._chunkBaseColliderCache.TryGetValue(chunkRoot, out chunkOwnedComponents))
		{
			return chunkOwnedComponents;
		}
		chunkOwnedComponents = ChunkManager.GetChunkOwnedComponents<Collider>(chunkRoot);
		this._chunkBaseColliderCache[chunkRoot] = chunkOwnedComponents;
		return chunkOwnedComponents;
	}

	// Token: 0x0600038C RID: 908 RVA: 0x0001A9C4 File Offset: 0x00018BC4
	private void EnsureSpawnedChunkCollidersReadyBeforePhysics(ChunkInstance chunk, Vector2Int playerCell)
	{
		if (((chunk != null) ? chunk.chunkRoot : null) == null)
		{
			return;
		}
		ChunkData chunkData = chunk.chunkData;
		Vector2Int vector2Int = ((chunkData != null) ? chunkData.gridSize : Vector2Int.one);
		if (ChunkManager.ChebyshevStepsToChunk(chunk.originCell, vector2Int, playerCell) > 1)
		{
			return;
		}
		Collider[] array = this.CacheBaseChunkColliders(chunk.chunkRoot);
		chunk.cachedColliders = array;
		if (array != null)
		{
			foreach (Collider collider in array)
			{
				if (collider != null && !collider.enabled)
				{
					collider.enabled = true;
				}
			}
		}
		chunk.collidersCurrentlyEnabled = true;
		chunk.collidersCacheDirty = false;
	}

	// Token: 0x0600038D RID: 909 RVA: 0x0001AA5D File Offset: 0x00018C5D
	private void PrepareChunkColliderCacheForStagedHeavyAttach(ChunkInstance chunk)
	{
		if (chunk == null || (!chunk.collidersCacheDirty && chunk.cachedColliders != null))
		{
			return;
		}
		ChunkManager.RebuildChunkColliderCache(chunk);
	}

	// Token: 0x0600038E RID: 910 RVA: 0x0001AA7C File Offset: 0x00018C7C
	private void MergeChunkColliderSubtree(ChunkInstance chunk, GameObject subtreeRoot)
	{
		using (ChunkManager.MergeChunkColliderSubtreeMarker.Auto())
		{
			this.MergeChunkColliderSubtreeCore(chunk, subtreeRoot);
		}
	}

	// Token: 0x0600038F RID: 911 RVA: 0x0001AAC0 File Offset: 0x00018CC0
	private void MergeChunkColliderSubtreeCore(ChunkInstance chunk, GameObject subtreeRoot)
	{
		if (chunk == null || subtreeRoot == null)
		{
			return;
		}
		this._colliderSubtreeScratch.Clear();
		subtreeRoot.GetComponentsInChildren<Collider>(true, this._colliderSubtreeScratch);
		int num = 0;
		bool flag = this.colliderActiveRadius > 0;
		for (int i = 0; i < this._colliderSubtreeScratch.Count; i++)
		{
			Collider collider = this._colliderSubtreeScratch[i];
			if (!(collider == null))
			{
				if (collider.GetComponentInParent<NPCEquipmentManager>(true) != null || collider.GetComponentInParent<NPCSenses>(true) != null)
				{
					this._colliderSubtreeScratch[i] = null;
				}
				else
				{
					if (flag && collider.enabled != chunk.collidersCurrentlyEnabled)
					{
						collider.enabled = chunk.collidersCurrentlyEnabled;
					}
					num++;
				}
			}
		}
		Collider[] cachedColliders = chunk.cachedColliders;
		if (chunk.collidersCacheDirty || cachedColliders == null || num == 0)
		{
			this._colliderSubtreeScratch.Clear();
			return;
		}
		Collider[] array = new Collider[cachedColliders.Length + num];
		Array.Copy(cachedColliders, array, cachedColliders.Length);
		int num2 = cachedColliders.Length;
		for (int j = 0; j < this._colliderSubtreeScratch.Count; j++)
		{
			Collider collider2 = this._colliderSubtreeScratch[j];
			if (collider2 != null)
			{
				array[num2++] = collider2;
			}
		}
		chunk.cachedColliders = array;
		this._colliderSubtreeScratch.Clear();
	}

	// Token: 0x06000390 RID: 912 RVA: 0x0001AC18 File Offset: 0x00018E18
	private void ApplyVisualOnlyState(Vector2Int cell, bool visible)
	{
		using (ChunkManager.ApplyVisualOnlyStateMarker.Auto())
		{
			ChunkInstance chunkInstance;
			if (!visible && this._spawnedChunks.TryGetValue(cell, out chunkInstance) && ChunkManager.HasActivePoiResidencyUse(chunkInstance))
			{
				visible = true;
			}
			ChunkManager.ChunkVisualCache chunkVisualCache;
			if (this._chunkVisualCaches.TryGetValue(cell, out chunkVisualCache))
			{
				for (int i = 0; i < chunkVisualCache.renderers.Length; i++)
				{
					Renderer renderer = chunkVisualCache.renderers[i];
					if (!(renderer == null))
					{
						if (chunkVisualCache.rendererUsesIndependentEnabledState != null && i < chunkVisualCache.rendererUsesIndependentEnabledState.Length && chunkVisualCache.rendererUsesIndependentEnabledState[i])
						{
							bool flag = chunkVisualCache.rendererForceRenderingOffDefaults != null && i < chunkVisualCache.rendererForceRenderingOffDefaults.Length && chunkVisualCache.rendererForceRenderingOffDefaults[i];
							renderer.forceRenderingOff = !visible || flag;
						}
						else
						{
							renderer.enabled = visible && chunkVisualCache.rendererDefaults[i];
						}
					}
				}
				for (int j = 0; j < chunkVisualCache.lodGroups.Length; j++)
				{
					LODGroup lodgroup = chunkVisualCache.lodGroups[j];
					if (!(lodgroup == null))
					{
						lodgroup.enabled = visible && chunkVisualCache.lodGroupDefaults[j];
					}
				}
				for (int k = 0; k < chunkVisualCache.terrains.Length; k++)
				{
					Terrain terrain = chunkVisualCache.terrains[k];
					if (!(terrain == null))
					{
						terrain.enabled = visible && chunkVisualCache.terrainDefaults[k];
					}
				}
				if (visible)
				{
					this._visuallyHiddenChunks.Remove(cell);
				}
				else
				{
					this._visuallyHiddenChunks.Add(cell);
				}
			}
		}
	}

	// Token: 0x06000391 RID: 913 RVA: 0x0001ADD0 File Offset: 0x00018FD0
	private bool ShouldChunkBeActive(Vector2Int cell, ChunkInstance chunk, Vector2Int playerCell)
	{
		if (ChunkManager.HasActivePoiResidencyUse(chunk))
		{
			return true;
		}
		ChunkData chunkData = chunk.chunkData;
		Vector2Int vector2Int = ((chunkData != null) ? chunkData.gridSize : Vector2Int.one);
		int num = ChunkManager.ChebyshevStepsToChunk(cell, vector2Int, playerCell);
		if (num <= this.minVisibleRadius)
		{
			return true;
		}
		if (num <= this.maxVisibleRadius)
		{
			Vector2 vector = new Vector2((float)cell.x, (float)cell.y);
			Vector2 vector2 = new Vector2((float)(cell.x + vector2Int.x), (float)(cell.y + vector2Int.y));
			Vector2 vector3 = new Vector2((float)playerCell.x + 0.5f, (float)playerCell.y + 0.5f);
			return this.IsChunkInFOV(vector, vector2, vector3);
		}
		return false;
	}

	// Token: 0x06000392 RID: 914 RVA: 0x0001AE88 File Offset: 0x00019088
	private static int ChebyshevStepsToChunk(Vector2Int chunkOrigin, Vector2Int chunkSize, Vector2Int playerCell)
	{
		int num = ChunkManager.AxisStep(chunkOrigin.x, chunkSize.x, playerCell.x);
		int num2 = ChunkManager.AxisStep(chunkOrigin.y, chunkSize.y, playerCell.y);
		return Mathf.Max(num, num2);
	}

	// Token: 0x06000393 RID: 915 RVA: 0x0001AED0 File Offset: 0x000190D0
	private static int AxisStep(int chunkOrigin, int chunkSize, int player)
	{
		int num = chunkOrigin + Mathf.Max(1, chunkSize) - 1;
		if (player < chunkOrigin)
		{
			return chunkOrigin - player;
		}
		if (player > num)
		{
			return player - num;
		}
		return 0;
	}

	// Token: 0x06000394 RID: 916 RVA: 0x0001AEFC File Offset: 0x000190FC
	private bool IsChunkInFOV(Vector2 chunkMin, Vector2 chunkMax, Vector2 playerPos)
	{
		Camera main = Camera.main;
		if (main == null)
		{
			return true;
		}
		Vector3 forward = main.transform.forward;
		Vector2 vector = new Vector2(forward.x, forward.z);
		if (vector.sqrMagnitude < 0.001f)
		{
			return true;
		}
		vector.Normalize();
		float num = Mathf.Cos((this.viewAngle + this.viewAngleBuffer) * 0.5f * 0.017453292f);
		foreach (Vector2 vector2 in new Vector2[]
		{
			new Vector2(chunkMin.x, chunkMin.y),
			new Vector2(chunkMax.x, chunkMin.y),
			new Vector2(chunkMin.x, chunkMax.y),
			new Vector2(chunkMax.x, chunkMax.y),
			(chunkMin + chunkMax) * 0.5f,
			new Vector2((chunkMin.x + chunkMax.x) * 0.5f, chunkMax.y),
			new Vector2((chunkMin.x + chunkMax.x) * 0.5f, chunkMin.y),
			new Vector2(chunkMin.x, (chunkMin.y + chunkMax.y) * 0.5f),
			new Vector2(chunkMax.x, (chunkMin.y + chunkMax.y) * 0.5f)
		})
		{
			if (this.IsPointInFOV(vector2, playerPos, vector, num))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000395 RID: 917 RVA: 0x0001B0B4 File Offset: 0x000192B4
	private bool IsPointInFOV(Vector2 point, Vector2 playerPos, Vector2 cameraDir, float halfAngleCos)
	{
		Vector2 vector = point - playerPos;
		if (vector.sqrMagnitude < 0.001f)
		{
			return true;
		}
		vector.Normalize();
		return Vector2.Dot(cameraDir, vector) >= halfAngleCos;
	}

	// Token: 0x06000396 RID: 918 RVA: 0x0001B0F0 File Offset: 0x000192F0
	private void SpawnZonesForChunk(ChunkInstance chunk, int rotation, ChunkSpawnResolver.SpawnEntry entry = default(ChunkSpawnResolver.SpawnEntry))
	{
		ChunkData chunkData = chunk.chunkData;
		if (((chunkData != null) ? chunkData.template : null) == null)
		{
			return;
		}
		ChunkTemplate template = chunk.chunkData.template;
		if (template.zoneSlots == null || template.zoneSlots.Count == 0)
		{
			return;
		}
		if (this.enableDeferredZoneSpawn)
		{
			int count = template.zoneSlots.Count;
			int num;
			this._pendingZoneCountByChunk.TryGetValue(chunk, out num);
			this._pendingZoneCountByChunk[chunk] = num + count;
			for (int i = 0; i < count; i++)
			{
				this._pendingZoneSpawns.Enqueue(new ChunkManager.PendingZoneSpawn
				{
					chunk = chunk,
					slotIndex = i,
					entry = entry,
					lastPoolRevisionTried = -1,
					hasDeferred = false
				});
			}
			if (this.enableVerboseDiagnostics)
			{
				Debug.Log(string.Format("[ZoneDiag] enqueue cell={0} chunk={1} slots={2} entryValida={3} coda={4}", new object[]
				{
					chunk.originCell,
					chunk.chunkData.name,
					count,
					entry.IsValid,
					this._pendingZoneSpawns.Count
				}));
				return;
			}
		}
		else
		{
			for (int j = 0; j < template.zoneSlots.Count; j++)
			{
				this.SpawnSingleZone(chunk, j, entry);
			}
			this.TryNotifyChunkZonesReady(chunk);
		}
	}

	// Token: 0x06000397 RID: 919 RVA: 0x0001B24C File Offset: 0x0001944C
	private bool SpawnSingleZone(ChunkInstance chunk, int slotIndex, ChunkSpawnResolver.SpawnEntry entry)
	{
		Object @object;
		if (chunk == null)
		{
			@object = null;
		}
		else
		{
			ChunkData chunkData = chunk.chunkData;
			@object = ((chunkData != null) ? chunkData.template : null);
		}
		if (@object == null || chunk.chunkRoot == null)
		{
			Debug.LogWarning(string.Format("[ZoneDiag] skip slot {0}: chunk/template/root null (cell={1})", slotIndex, (chunk != null) ? new Vector2Int?(chunk.originCell) : null));
			return true;
		}
		if (!this.IsActiveChunkInstance(chunk))
		{
			if (this.enableVerboseDiagnostics)
			{
				Debug.LogWarning(string.Format("[ZoneDiag] scarto entry stantia cell={0} slot={1} chunk={2}", chunk.originCell, slotIndex, chunk.chunkData.name));
			}
			return true;
		}
		ChunkTemplate template = chunk.chunkData.template;
		if (slotIndex < 0 || slotIndex >= template.zoneSlots.Count)
		{
			Debug.LogWarning(string.Format("[ZoneDiag] skip cell={0}: slotIndex {1} fuori range ({2} slot)", chunk.originCell, slotIndex, template.zoneSlots.Count));
			return true;
		}
		ZoneSlot zoneSlot = template.zoneSlots[slotIndex];
		ZoneData zoneData = null;
		if (entry.IsValid && entry.zoneIndices != null && slotIndex < entry.zoneIndices.Length)
		{
			int num = entry.zoneIndices[slotIndex];
			if (num >= 0 && num < chunk.chunkData.availableZones.Count)
			{
				zoneData = chunk.chunkData.availableZones[num];
			}
		}
		if (zoneData == null)
		{
			zoneData = chunk.chunkData.GetRandomZoneForSlot(zoneSlot, !entry.IsValid);
		}
		if (!RankPoiPolicy.IsZoneAllowed(zoneData))
		{
			zoneData = chunk.chunkData.GetRandomZoneForSlot(zoneSlot, true);
		}
		bool flag = ProceduralZonePolicy.IsSpecialZone(zoneData);
		if (((zoneData != null) ? zoneData.zonePrefab : null) == null)
		{
			Debug.LogWarning(string.Format("[ZoneDiag] skip cell={0} slot={1}: zoneData={2} zonePrefab null", chunk.originCell, slotIndex, (zoneData == null) ? "null" : zoneData.name));
			return true;
		}
		List<Transform> orCacheBaseTransforms = this.GetOrCacheBaseTransforms(chunk.chunkRoot);
		Transform transform = ((slotIndex < orCacheBaseTransforms.Count) ? orCacheBaseTransforms[slotIndex] : chunk.chunkRoot.transform);
		ChunkManager.ZonePlaceholderVisualState orCaptureZonePlaceholderState = this.GetOrCaptureZonePlaceholderState(transform, chunk.chunkRoot.transform);
		ChunkManager.RestoreZonePlaceholder(orCaptureZonePlaceholderState);
		ZoneData zoneData2 = zoneData;
		ZoneData zoneData3;
		if (this._fullGameplayReady && this.forbidRuntimeZoneInstantiate && !RunModeService.HasSeededFreeWorld && !ProceduralZonePolicy.RequiresExactRuntimeIdentity(zoneData) && this.GetLiveZonePoolCount(zoneData.zonePrefab) == 0 && this.TrySelectPooledZoneFallback(chunk, zoneSlot, zoneData, out zoneData3))
		{
			zoneData2 = zoneData3;
			int runtimeZonePoolFallbacks = this.RuntimeZonePoolFallbacks;
			this.RuntimeZonePoolFallbacks = runtimeZonePoolFallbacks + 1;
		}
		GameObject gameObject = this.ResolveZonePrefab(zoneData2);
		HeavyZoneStreamingProxy component = gameObject.GetComponent<HeavyZoneStreamingProxy>();
		if (component != null && component.UsesStagedPooledActivation && (chunk.collidersCacheDirty || chunk.cachedColliders == null))
		{
			this.PrepareChunkColliderCacheForStagedHeavyAttach(chunk);
		}
		GameObject gameObject2 = this.AcquireZoneRootWithResidency(gameObject, chunk.chunkRoot.transform, transform.position, transform.rotation, this.CapturePoiResidencyContext(chunk, slotIndex, component));
		if (gameObject2 == null)
		{
			if (this.enableVerboseDiagnostics)
			{
				Debug.Log(string.Format("[ZoneDiag] defer cell={0} slot={1}: pool vuoto per '{2}'", chunk.originCell, slotIndex, zoneData.name));
			}
			return false;
		}
		zoneData = zoneData2;
		this.RememberMaterializedZone(chunk, slotIndex, zoneData);
		if (ProceduralZonePolicy.IsSpecialZone(zoneData))
		{
			this._specialZoneReservationCells.Add(chunk.originCell);
		}
		else if (flag && !ChunkManager.SelectionContainsSpecialZoneExceptSlot(entry, chunk.chunkData, slotIndex))
		{
			this._specialZoneReservationCells.Remove(chunk.originCell);
		}
		if (this.enableVerboseDiagnostics)
		{
			Debug.Log(string.Format("[ZoneDiag] spawn OK cell={0} slot={1} zona={2}", chunk.originCell, slotIndex, zoneData.name));
		}
		gameObject2.name = "Zone_" + zoneData.name;
		chunk.spawnedZones.Add(gameObject2);
		HeavyZoneStreamingProxy component2 = gameObject2.GetComponent<HeavyZoneStreamingProxy>();
		if (component2 != null)
		{
			component2.SetStreamingUrgency(this.IsChunkStreamingUrgent(chunk));
		}
		if (component2 != null && component2.UsesStagedPooledActivation)
		{
			component2.SetChunkCollidersEnabled(chunk.collidersCurrentlyEnabled);
		}
		IncrementalPooledZoneActivation component3 = gameObject2.GetComponent<IncrementalPooledZoneActivation>();
		if (component3 != null && component3.HasSnapshot)
		{
			if (orCaptureZonePlaceholderState != null)
			{
				this._incrementalZonePlaceholders[component3] = orCaptureZonePlaceholderState;
			}
			if (component3.IsOperationalReady)
			{
				this.PublishIncrementalNormalZoneOperationalContent(chunk, component3);
			}
			else
			{
				this.RegisterIncrementalNormalZone(chunk, component3);
			}
		}
		else if (component2 == null)
		{
			ChunkManager.HideZonePlaceholder(orCaptureZonePlaceholderState);
			this.MergeChunkColliderSubtree(chunk, gameObject2);
			Action<GameObject> onContentRootReady = this.OnContentRootReady;
			if (onContentRootReady != null)
			{
				onContentRootReady(gameObject2);
			}
			this.InvalidateChunkVisualCache(chunk.originCell);
		}
		else if (!component2.IsOperationalReady && !component2.LoadFailed)
		{
			if (orCaptureZonePlaceholderState != null)
			{
				this._heavyZonePlaceholders[component2] = orCaptureZonePlaceholderState;
			}
			this.RegisterHeavyZone(chunk, component2);
		}
		else if (component2.IsOperationalReady)
		{
			if (orCaptureZonePlaceholderState != null)
			{
				this._heavyZonePlaceholders[component2] = orCaptureZonePlaceholderState;
			}
			this.PublishHeavyZoneOperationalContent(chunk, component2);
		}
		else
		{
			component2.SetStreamingUrgency(false);
		}
		if (this._fullGameplayReady && ProceduralZonePolicy.IsSpecialZone(zoneData))
		{
			Vector2Int vector2Int;
			bool flag2;
			if (this.TryGetCurrentStreamingPlayerCell(out vector2Int))
			{
				Vector2Int originCell = chunk.originCell;
				ChunkData chunkData2 = chunk.chunkData;
				flag2 = ChunkManager.ChebyshevStepsToChunk(originCell, (chunkData2 != null) ? chunkData2.gridSize : Vector2Int.one, vector2Int) <= 1;
			}
			else
			{
				flag2 = false;
			}
			if (!flag2 && gameObject2.activeSelf)
			{
				gameObject2.SetActive(false);
			}
		}
		this.RecordCompletedInitializationWork();
		return true;
	}

	// Token: 0x06000398 RID: 920 RVA: 0x0001B7A8 File Offset: 0x000199A8
	private void RememberMaterializedZone(ChunkInstance chunk, int slotIndex, ZoneData zone)
	{
		bool flag;
		if (chunk == null)
		{
			flag = null != null;
		}
		else
		{
			ChunkData chunkData = chunk.chunkData;
			flag = ((chunkData != null) ? chunkData.availableZones : null) != null;
		}
		ChunkSpawnResolver.SpawnEntry spawnEntry;
		if (!flag || zone == null || slotIndex < 0 || !this._committedSpawnPlan.TryGetValue(chunk.originCell, out spawnEntry) || spawnEntry.chunkData != chunk.chunkData)
		{
			return;
		}
		int num = chunk.chunkData.availableZones.IndexOf(zone);
		if (num < 0 || (spawnEntry.zoneIndices != null && slotIndex < spawnEntry.zoneIndices.Length && spawnEntry.zoneIndices[slotIndex] == num))
		{
			return;
		}
		int num2 = slotIndex + 1;
		int[] zoneIndices = spawnEntry.zoneIndices;
		int num3 = ((zoneIndices != null) ? zoneIndices.Length : 0);
		ChunkTemplate template = chunk.chunkData.template;
		int num4 = Mathf.Max(num2, Mathf.Max(num3, (template != null) ? template.zoneSlots.Count : 0));
		int[] array = new int[num4];
		for (int i = 0; i < num4; i++)
		{
			array[i] = -1;
		}
		if (spawnEntry.zoneIndices != null)
		{
			Array.Copy(spawnEntry.zoneIndices, array, spawnEntry.zoneIndices.Length);
		}
		array[slotIndex] = num;
		ChunkSpawnResolver.SpawnEntry spawnEntry2 = new ChunkSpawnResolver.SpawnEntry(chunk.chunkData, spawnEntry.rotation, array, false);
		if (ChunkManager.SelectionContainsSpecialZone(spawnEntry, chunk.chunkData) && !ChunkManager.SelectionContainsSpecialZone(spawnEntry2, chunk.chunkData))
		{
			this.UnregisterCommittedSpecialExclusion(chunk.originCell, chunk.chunkData.gridSize);
		}
		this.CommitSpawnPlanEntry(chunk.originCell, spawnEntry2, chunk.chunkData);
	}

	// Token: 0x06000399 RID: 921 RVA: 0x0001B90C File Offset: 0x00019B0C
	private void RegisterIncrementalNormalZone(ChunkInstance chunk, IncrementalPooledZoneActivation staged)
	{
		if (chunk == null || staged == null || this._incrementalNormalZoneOwners.ContainsKey(staged))
		{
			return;
		}
		this._incrementalNormalZoneOwners.Add(staged, chunk);
		int num;
		this._pendingHeavyZoneCountByChunk.TryGetValue(chunk, out num);
		this._pendingHeavyZoneCountByChunk[chunk] = num + 1;
		staged.OperationalReady += this.HandleIncrementalNormalZoneOperationalReady;
		if (staged.IsOperationalReady)
		{
			this.CompleteIncrementalNormalZone(staged);
			return;
		}
		if (this._pendingNormalZoneWakeSet.Add(staged))
		{
			this._pendingNormalZoneWakes.Enqueue(staged);
		}
	}

	// Token: 0x0600039A RID: 922 RVA: 0x0001B99C File Offset: 0x00019B9C
	private void ProcessIncrementalNormalZoneWakeQueue()
	{
		this.NormalZoneWakeOperationsLastFrame = 0;
		this.NormalZoneWakePrioritySelectionsLastFrame = 0;
		this.NormalZoneWakeElapsedMsLastFrame = 0.0;
		if (this._pendingNormalZoneWakes.Count == 0)
		{
			return;
		}
		int num = Mathf.Max(1, this.normalZoneStagedMaxOperationsPerFrame);
		double num2 = Math.Max(0.1, (double)this.normalZoneStagedFrameBudgetMs);
		long timestamp = Stopwatch.GetTimestamp();
		long num3 = Math.Max(1L, (long)Math.Ceiling((double)Stopwatch.Frequency * num2 / 1000.0));
		int num4 = 0;
		while (this._pendingNormalZoneWakes.Count > 0 && num4 < num && (num4 == 0 || Stopwatch.GetTimestamp() - timestamp < num3))
		{
			int normalZoneWakePrioritySelectionsLastFrame = this.NormalZoneWakePrioritySelectionsLastFrame;
			this.NormalZoneWakePrioritySelectionsLastFrame = normalZoneWakePrioritySelectionsLastFrame + 1;
			IncrementalPooledZoneActivation incrementalPooledZoneActivation;
			if (!this.TryTakeHighestPriorityIncrementalNormalZoneWake(out incrementalPooledZoneActivation))
			{
				break;
			}
			bool flag = incrementalPooledZoneActivation.ProcessOneStagedWakeStep();
			num4++;
			if (flag && this._pendingNormalZoneWakeSet.Contains(incrementalPooledZoneActivation))
			{
				this._pendingNormalZoneWakes.Enqueue(incrementalPooledZoneActivation);
			}
			else
			{
				this._pendingNormalZoneWakeSet.Remove(incrementalPooledZoneActivation);
			}
		}
		this.NormalZoneWakeOperationsLastFrame = num4;
		this.NormalZoneWakeElapsedMsLastFrame = (double)(Stopwatch.GetTimestamp() - timestamp) * 1000.0 / (double)Stopwatch.Frequency;
	}

	// Token: 0x0600039B RID: 923 RVA: 0x0001BAC4 File Offset: 0x00019CC4
	private bool HasEligibleIncrementalNormalZoneWake()
	{
		if (this._pendingNormalZoneWakeSet.Count == 0)
		{
			return false;
		}
		Vector2Int vector2Int;
		bool flag = this.TryGetCurrentStreamingPlayerCell(out vector2Int);
		int num = ((!this._fullGameplayReady) ? Mathf.Max(1, this.spawnRadius) : Mathf.Max(1, this.minVisibleRadius));
		foreach (IncrementalPooledZoneActivation incrementalPooledZoneActivation in this._pendingNormalZoneWakeSet)
		{
			ChunkInstance chunkInstance;
			if (incrementalPooledZoneActivation != null && incrementalPooledZoneActivation.gameObject.activeInHierarchy && this._incrementalNormalZoneOwners.TryGetValue(incrementalPooledZoneActivation, out chunkInstance) && this.IsActiveChunkInstance(chunkInstance) && (!flag || this.GetChunkStreamingDistance(chunkInstance, vector2Int) <= num))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600039C RID: 924 RVA: 0x0001BB98 File Offset: 0x00019D98
	private bool TryTakeHighestPriorityIncrementalNormalZoneWake(out IncrementalPooledZoneActivation selected)
	{
		selected = null;
		int count = this._pendingNormalZoneWakes.Count;
		if (count == 0)
		{
			return false;
		}
		Vector2Int vector2Int;
		bool flag = this.TryGetCurrentStreamingPlayerCell(out vector2Int);
		int num = ((!this._fullGameplayReady) ? Mathf.Max(1, this.spawnRadius) : Mathf.Max(1, this.minVisibleRadius));
		int num2 = int.MaxValue;
		int num3 = int.MaxValue;
		for (int i = 0; i < count; i++)
		{
			IncrementalPooledZoneActivation incrementalPooledZoneActivation = this._pendingNormalZoneWakes.Dequeue();
			ChunkInstance chunkInstance;
			if (incrementalPooledZoneActivation == null || !this._pendingNormalZoneWakeSet.Contains(incrementalPooledZoneActivation) || !this._incrementalNormalZoneOwners.TryGetValue(incrementalPooledZoneActivation, out chunkInstance) || !this.IsActiveChunkInstance(chunkInstance))
			{
				this._pendingNormalZoneWakeSet.Remove(incrementalPooledZoneActivation);
			}
			else
			{
				int num4 = (flag ? this.GetChunkStreamingDistance(chunkInstance, vector2Int) : 0);
				if (!incrementalPooledZoneActivation.gameObject.activeInHierarchy || (flag && num4 > num))
				{
					this._pendingNormalZoneWakes.Enqueue(incrementalPooledZoneActivation);
				}
				else
				{
					int num5 = (flag ? (chunkInstance.originCell.x - vector2Int.x) : 0);
					int num6 = (flag ? (chunkInstance.originCell.y - vector2Int.y) : 0);
					int num7 = num5 * this._streamingTravelDirection.x + num6 * this._streamingTravelDirection.y;
					int num8 = ((num5 == 0 && num6 == 0) ? 0 : ((this._streamingTravelDirection == Vector2Int.zero || num7 > 0) ? 1 : ((num7 == 0) ? 2 : 3)));
					if (!(selected == null) && num4 >= num2 && (num4 != num2 || num8 >= num3))
					{
						this._pendingNormalZoneWakes.Enqueue(incrementalPooledZoneActivation);
					}
					else
					{
						if (selected != null)
						{
							this._pendingNormalZoneWakes.Enqueue(selected);
						}
						selected = incrementalPooledZoneActivation;
						num2 = num4;
						num3 = num8;
					}
				}
			}
		}
		return selected != null;
	}

	// Token: 0x0600039D RID: 925 RVA: 0x0001BD7C File Offset: 0x00019F7C
	private bool TryGetCurrentStreamingPlayerCell(out Vector2Int playerCell)
	{
		if (this.HasPreparationFocus)
		{
			playerCell = this.WorldToCell(this._preparationFocusPosition);
			return true;
		}
		if (PlayerProximityProvider.Instance != null)
		{
			playerCell = PlayerProximityProvider.Instance.CurrentCell;
			return true;
		}
		if (this._hasLastStreamingPlayerCell)
		{
			playerCell = this._lastStreamingPlayerCell;
			return true;
		}
		playerCell = default(Vector2Int);
		return false;
	}

	// Token: 0x0600039E RID: 926 RVA: 0x0001BDE4 File Offset: 0x00019FE4
	private int GetChunkStreamingDistance(ChunkInstance chunk, Vector2Int playerCell)
	{
		if (chunk == null)
		{
			return int.MaxValue;
		}
		ChunkData chunkData = chunk.chunkData;
		Vector2Int vector2Int = ((chunkData != null) ? chunkData.gridSize : Vector2Int.one);
		return ChunkManager.ChebyshevStepsToChunk(chunk.originCell, vector2Int, playerCell);
	}

	// Token: 0x0600039F RID: 927 RVA: 0x0001BE20 File Offset: 0x0001A020
	private bool IsChunkStreamingUrgent(ChunkInstance chunk)
	{
		Vector2Int vector2Int;
		return this.TryGetCurrentStreamingPlayerCell(out vector2Int) && this.GetChunkStreamingDistance(chunk, vector2Int) <= Mathf.Max(1, this.minVisibleRadius);
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x0001BE54 File Offset: 0x0001A054
	private void UpdateHeavyZoneStreamingPriorities(Vector2Int playerCell)
	{
		int num = Mathf.Max(1, this.minVisibleRadius);
		foreach (KeyValuePair<HeavyZoneStreamingProxy, ChunkInstance> keyValuePair in this._heavyZoneOwners)
		{
			HeavyZoneStreamingProxy key = keyValuePair.Key;
			ChunkInstance value = keyValuePair.Value;
			if (key != null)
			{
				bool flag = this.IsActiveChunkInstance(value) && this.GetChunkStreamingDistance(value, playerCell) <= num;
				if (key.IsStreamingUrgent != flag)
				{
					key.SetStreamingUrgency(flag);
				}
			}
		}
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x0001BEF8 File Offset: 0x0001A0F8
	private void HandleIncrementalNormalZoneOperationalReady(IncrementalPooledZoneActivation staged)
	{
		this.CompleteIncrementalNormalZone(staged);
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x0001BF04 File Offset: 0x0001A104
	private void CompleteIncrementalNormalZone(IncrementalPooledZoneActivation staged)
	{
		ChunkInstance chunkInstance;
		if (staged == null || !this._incrementalNormalZoneOwners.TryGetValue(staged, out chunkInstance))
		{
			return;
		}
		staged.OperationalReady -= this.HandleIncrementalNormalZoneOperationalReady;
		this._incrementalNormalZoneOwners.Remove(staged);
		this._pendingNormalZoneWakeSet.Remove(staged);
		int num;
		if (this._pendingHeavyZoneCountByChunk.TryGetValue(chunkInstance, out num))
		{
			if (num <= 1)
			{
				this._pendingHeavyZoneCountByChunk.Remove(chunkInstance);
			}
			else
			{
				this._pendingHeavyZoneCountByChunk[chunkInstance] = num - 1;
			}
		}
		ChunkInstance chunkInstance2;
		if (!this._spawnedChunks.TryGetValue(chunkInstance.originCell, out chunkInstance2) || chunkInstance2 != chunkInstance || !(chunkInstance.chunkRoot != null))
		{
			this._incrementalZonePlaceholders.Remove(staged);
			return;
		}
		this.PublishIncrementalNormalZoneOperationalContent(chunkInstance, staged);
		this.TryNotifyChunkZonesReady(chunkInstance);
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x0001BFD0 File Offset: 0x0001A1D0
	private void PublishIncrementalNormalZoneOperationalContent(ChunkInstance chunk, IncrementalPooledZoneActivation staged)
	{
		if (chunk == null || staged == null || !staged.IsOperationalReady)
		{
			return;
		}
		ChunkManager.ZonePlaceholderVisualState zonePlaceholderVisualState;
		if (this._incrementalZonePlaceholders.TryGetValue(staged, out zonePlaceholderVisualState))
		{
			ChunkManager.HideZonePlaceholder(zonePlaceholderVisualState);
			this._incrementalZonePlaceholders.Remove(staged);
		}
		this.MergeChunkColliderSubtree(chunk, staged.gameObject);
		Action<GameObject> onContentRootReady = this.OnContentRootReady;
		if (onContentRootReady != null)
		{
			onContentRootReady(staged.gameObject);
		}
		this.InvalidateChunkVisualCache(chunk.originCell);
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0001C048 File Offset: 0x0001A248
	private void RegisterHeavyZone(ChunkInstance chunk, HeavyZoneStreamingProxy proxy)
	{
		if (chunk == null || proxy == null || this._heavyZoneOwners.ContainsKey(proxy))
		{
			return;
		}
		this._heavyZoneOwners.Add(proxy, chunk);
		int num;
		this._pendingHeavyZoneCountByChunk.TryGetValue(chunk, out num);
		this._pendingHeavyZoneCountByChunk[chunk] = num + 1;
		proxy.OperationalReady += this.HandleHeavyZoneOperationalReady;
		proxy.PayloadFailed += this.HandleHeavyZoneFailed;
		if (proxy.IsOperationalReady)
		{
			this.CompleteHeavyZone(proxy, null);
			return;
		}
		if (proxy.LoadFailed)
		{
			this.CompleteHeavyZone(proxy, "Payload failed before the operational-ready subscription.");
		}
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x0001C0E4 File Offset: 0x0001A2E4
	private void HandleHeavyZoneOperationalReady(HeavyZoneStreamingProxy proxy)
	{
		this.CompleteHeavyZone(proxy, null);
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x0001C0EE File Offset: 0x0001A2EE
	private void HandleHeavyZoneFailed(HeavyZoneStreamingProxy proxy, string reason)
	{
		this.CompleteHeavyZone(proxy, reason);
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0001C0F8 File Offset: 0x0001A2F8
	private void CompleteHeavyZone(HeavyZoneStreamingProxy proxy, string failureReason)
	{
		ChunkInstance chunkInstance;
		if (proxy == null || !this._heavyZoneOwners.TryGetValue(proxy, out chunkInstance))
		{
			return;
		}
		proxy.OperationalReady -= this.HandleHeavyZoneOperationalReady;
		proxy.PayloadFailed -= this.HandleHeavyZoneFailed;
		this._heavyZoneOwners.Remove(proxy);
		int num;
		if (this._pendingHeavyZoneCountByChunk.TryGetValue(chunkInstance, out num))
		{
			if (num <= 1)
			{
				this._pendingHeavyZoneCountByChunk.Remove(chunkInstance);
			}
			else
			{
				this._pendingHeavyZoneCountByChunk[chunkInstance] = num - 1;
			}
		}
		ChunkInstance chunkInstance2;
		if (!this._spawnedChunks.TryGetValue(chunkInstance.originCell, out chunkInstance2) || chunkInstance2 != chunkInstance || !(chunkInstance.chunkRoot != null))
		{
			this._heavyZonePlaceholders.Remove(proxy);
			proxy.SetStreamingUrgency(false);
			return;
		}
		if (!string.IsNullOrEmpty(failureReason))
		{
			Debug.LogError(string.Format("[ChunkManager] Heavy zone failed in chunk {0}: {1}", chunkInstance.originCell, failureReason), proxy);
		}
		if (string.IsNullOrEmpty(failureReason))
		{
			this.PublishHeavyZoneOperationalContent(chunkInstance, proxy);
		}
		else
		{
			this._heavyZonePlaceholders.Remove(proxy);
		}
		proxy.SetStreamingUrgency(false);
		this.TryNotifyChunkZonesReady(chunkInstance);
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x0001C214 File Offset: 0x0001A414
	private void PublishHeavyZoneOperationalContent(ChunkInstance chunk, HeavyZoneStreamingProxy proxy)
	{
		if (chunk == null || proxy == null || !proxy.IsOperationalReady)
		{
			return;
		}
		GameObject payloadInstance = proxy.PayloadInstance;
		if (payloadInstance == null)
		{
			return;
		}
		ChunkManager.ZonePlaceholderVisualState zonePlaceholderVisualState;
		if (this._heavyZonePlaceholders.TryGetValue(proxy, out zonePlaceholderVisualState))
		{
			ChunkManager.HideZonePlaceholder(zonePlaceholderVisualState);
			this._heavyZonePlaceholders.Remove(proxy);
		}
		if (proxy.UsesStagedPooledActivation)
		{
			if (!proxy.HasCompleteColliderSnapshot)
			{
				return;
			}
			this.RegisterStagedHeavyColliderOwner(chunk, proxy);
			proxy.SetChunkCollidersEnabled(chunk.collidersCurrentlyEnabled);
		}
		else
		{
			this.MergeChunkColliderSubtree(chunk, payloadInstance);
		}
		this.InvalidateChunkVisualCache(chunk.originCell);
		Action<GameObject> onContentRootReady = this.OnContentRootReady;
		if (onContentRootReady == null)
		{
			return;
		}
		onContentRootReady(payloadInstance);
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x0001C2B8 File Offset: 0x0001A4B8
	private void RegisterStagedHeavyColliderOwner(ChunkInstance chunk, HeavyZoneStreamingProxy proxy)
	{
		List<HeavyZoneStreamingProxy> list;
		if (!this._stagedHeavyColliderOwners.TryGetValue(chunk, out list))
		{
			list = new List<HeavyZoneStreamingProxy>(1);
			this._stagedHeavyColliderOwners.Add(chunk, list);
		}
		if (!list.Contains(proxy))
		{
			list.Add(proxy);
		}
	}

	// Token: 0x060003AA RID: 938 RVA: 0x0001C2FC File Offset: 0x0001A4FC
	private void SetStagedHeavyColliderState(ChunkInstance chunk, bool enabled)
	{
		List<HeavyZoneStreamingProxy> list;
		if (chunk == null || !this._stagedHeavyColliderOwners.TryGetValue(chunk, out list))
		{
			return;
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			HeavyZoneStreamingProxy heavyZoneStreamingProxy = list[i];
			if (heavyZoneStreamingProxy == null)
			{
				list.RemoveAt(i);
			}
			else
			{
				heavyZoneStreamingProxy.SetChunkCollidersEnabled(enabled);
			}
		}
		if (list.Count == 0)
		{
			this._stagedHeavyColliderOwners.Remove(chunk);
		}
	}

	// Token: 0x060003AB RID: 939 RVA: 0x0001C366 File Offset: 0x0001A566
	public bool HasPendingZoneContent(ChunkInstance chunk)
	{
		return chunk != null && (this._pendingZoneCountByChunk.ContainsKey(chunk) || this._pendingHeavyZoneCountByChunk.ContainsKey(chunk));
	}

	// Token: 0x060003AC RID: 940 RVA: 0x0001C38C File Offset: 0x0001A58C
	private bool IsChunkEligibleForZonesReady(ChunkInstance chunk)
	{
		ChunkInstance chunkInstance;
		return chunk != null && !(chunk.chunkRoot == null) && !this._pendingZoneCountByChunk.ContainsKey(chunk) && !this._pendingHeavyZoneCountByChunk.ContainsKey(chunk) && this._spawnedChunks.TryGetValue(chunk.originCell, out chunkInstance) && chunkInstance == chunk;
	}

	// Token: 0x060003AD RID: 941 RVA: 0x0001C3E4 File Offset: 0x0001A5E4
	private void TryNotifyChunkZonesReady(ChunkInstance chunk)
	{
		if (!this.IsChunkEligibleForZonesReady(chunk) || this._zonesReadyNotifiedChunks.Contains(chunk))
		{
			return;
		}
		if (!this._fullGameplayReady)
		{
			this.PublishChunkZonesReady(chunk);
			return;
		}
		if (this._pendingChunkZonesReadyByChunk.ContainsKey(chunk))
		{
			return;
		}
		ChunkManager.PendingChunkZonesReadyNotification pendingChunkZonesReadyNotification = new ChunkManager.PendingChunkZonesReadyNotification
		{
			chunk = chunk,
			originCell = chunk.originCell,
			chunkRoot = chunk.chunkRoot,
			enqueuedFrame = Time.frameCount
		};
		LinkedListNode<ChunkManager.PendingChunkZonesReadyNotification> linkedListNode = this._pendingChunkZonesReadyNotifications.AddLast(pendingChunkZonesReadyNotification);
		this._pendingChunkZonesReadyByChunk.Add(chunk, linkedListNode);
	}

	// Token: 0x060003AE RID: 942 RVA: 0x0001C47A File Offset: 0x0001A67A
	private void ProcessPendingChunkZonesReadyNotifications()
	{
		this.ProcessPendingChunkZonesReadyNotificationsForFrame(Time.frameCount);
	}

	// Token: 0x060003AF RID: 943 RVA: 0x0001C488 File Offset: 0x0001A688
	private void ProcessPendingChunkZonesReadyNotificationsForFrame(int currentFrame)
	{
		if (this._lastChunkZonesReadyFanoutFrame == currentFrame)
		{
			return;
		}
		while (this._pendingChunkZonesReadyNotifications.First != null)
		{
			LinkedListNode<ChunkManager.PendingChunkZonesReadyNotification> first = this._pendingChunkZonesReadyNotifications.First;
			ChunkManager.PendingChunkZonesReadyNotification value = first.Value;
			if (value.enqueuedFrame >= currentFrame)
			{
				return;
			}
			this._pendingChunkZonesReadyNotifications.RemoveFirst();
			LinkedListNode<ChunkManager.PendingChunkZonesReadyNotification> linkedListNode;
			if (value.chunk != null && this._pendingChunkZonesReadyByChunk.TryGetValue(value.chunk, out linkedListNode) && linkedListNode == first)
			{
				this._pendingChunkZonesReadyByChunk.Remove(value.chunk);
			}
			if (value.chunk != null && value.chunk.chunkRoot == value.chunkRoot && !(value.chunk.originCell != value.originCell) && this.IsChunkEligibleForZonesReady(value.chunk) && !this._zonesReadyNotifiedChunks.Contains(value.chunk))
			{
				this._lastChunkZonesReadyFanoutFrame = currentFrame;
				this.PublishChunkZonesReady(value.chunk);
				return;
			}
		}
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x0001C578 File Offset: 0x0001A778
	private void CancelPendingChunkZonesReady(ChunkInstance chunk)
	{
		LinkedListNode<ChunkManager.PendingChunkZonesReadyNotification> linkedListNode;
		if (chunk == null || !this._pendingChunkZonesReadyByChunk.TryGetValue(chunk, out linkedListNode))
		{
			return;
		}
		this._pendingChunkZonesReadyByChunk.Remove(chunk);
		this._pendingChunkZonesReadyNotifications.Remove(linkedListNode);
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x0001C5B4 File Offset: 0x0001A7B4
	private void PublishChunkZonesReady(ChunkInstance chunk)
	{
		if (!this.IsChunkEligibleForZonesReady(chunk) || !this._zonesReadyNotifiedChunks.Add(chunk))
		{
			return;
		}
		using (ChunkManager.ZonesReadyFanoutMarker.Auto())
		{
			try
			{
				ChunkManager.ChunkEvent onChunkZonesReady = this.OnChunkZonesReady;
				if (onChunkZonesReady != null)
				{
					onChunkZonesReady(chunk);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			finally
			{
				this.NotifyGraphManagerZonesReady(chunk);
			}
		}
		this.RecordCompletedInitializationWork();
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x0001C648 File Offset: 0x0001A848
	private void UnregisterHeavyZones(ChunkInstance chunk)
	{
		if (chunk == null)
		{
			return;
		}
		this._stagedHeavyColliderOwners.Remove(chunk);
		List<HeavyZoneStreamingProxy> list = null;
		foreach (KeyValuePair<HeavyZoneStreamingProxy, ChunkInstance> keyValuePair in this._heavyZoneOwners)
		{
			if (keyValuePair.Value == chunk)
			{
				if (list == null)
				{
					list = new List<HeavyZoneStreamingProxy>(1);
				}
				list.Add(keyValuePair.Key);
			}
		}
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				HeavyZoneStreamingProxy heavyZoneStreamingProxy = list[i];
				if (heavyZoneStreamingProxy != null)
				{
					heavyZoneStreamingProxy.SetStreamingUrgency(false);
					heavyZoneStreamingProxy.OperationalReady -= this.HandleHeavyZoneOperationalReady;
					heavyZoneStreamingProxy.PayloadFailed -= this.HandleHeavyZoneFailed;
				}
				this._heavyZonePlaceholders.Remove(heavyZoneStreamingProxy);
				this._heavyZoneOwners.Remove(heavyZoneStreamingProxy);
			}
		}
		this._pendingHeavyZoneCountByChunk.Remove(chunk);
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x0001C748 File Offset: 0x0001A948
	private void UnregisterIncrementalNormalZones(ChunkInstance chunk)
	{
		if (chunk == null)
		{
			return;
		}
		List<IncrementalPooledZoneActivation> list = null;
		foreach (KeyValuePair<IncrementalPooledZoneActivation, ChunkInstance> keyValuePair in this._incrementalNormalZoneOwners)
		{
			if (keyValuePair.Value == chunk)
			{
				if (list == null)
				{
					list = new List<IncrementalPooledZoneActivation>(1);
				}
				list.Add(keyValuePair.Key);
			}
		}
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				IncrementalPooledZoneActivation incrementalPooledZoneActivation = list[i];
				if (incrementalPooledZoneActivation != null)
				{
					incrementalPooledZoneActivation.OperationalReady -= this.HandleIncrementalNormalZoneOperationalReady;
					incrementalPooledZoneActivation.BeginStagedSleep();
				}
				this._incrementalZonePlaceholders.Remove(incrementalPooledZoneActivation);
				this.CancelIncrementalNormalZoneWake(incrementalPooledZoneActivation);
				this._incrementalNormalZoneOwners.Remove(incrementalPooledZoneActivation);
			}
		}
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x0001C820 File Offset: 0x0001AA20
	private void CancelIncrementalNormalZoneWake(IncrementalPooledZoneActivation staged)
	{
		if (staged != null)
		{
			this._pendingNormalZoneWakeSet.Remove(staged);
		}
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0001C834 File Offset: 0x0001AA34
	private void CancelAllIncrementalNormalZoneWakes()
	{
		foreach (KeyValuePair<IncrementalPooledZoneActivation, ChunkInstance> keyValuePair in this._incrementalNormalZoneOwners)
		{
			IncrementalPooledZoneActivation key = keyValuePair.Key;
			if (!(key == null))
			{
				key.OperationalReady -= this.HandleIncrementalNormalZoneOperationalReady;
				key.CancelStagedWorkForTeardown();
			}
		}
		this._incrementalNormalZoneOwners.Clear();
		this._pendingNormalZoneWakes.Clear();
		this._pendingNormalZoneWakeSet.Clear();
		this._incrementalZonePlaceholders.Clear();
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x0001C8D8 File Offset: 0x0001AAD8
	private ChunkManager.ZonePlaceholderVisualState GetOrCaptureZonePlaceholderState(Transform spawnTransform, Transform chunkRoot)
	{
		if (spawnTransform == null || spawnTransform == chunkRoot)
		{
			return null;
		}
		ChunkManager.ZonePlaceholderVisualState zonePlaceholderVisualState;
		if (this._zonePlaceholderStates.TryGetValue(spawnTransform, out zonePlaceholderVisualState))
		{
			return zonePlaceholderVisualState;
		}
		Renderer[] componentsInChildren = spawnTransform.GetComponentsInChildren<Renderer>(true);
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return null;
		}
		ChunkManager.ZonePlaceholderVisualState zonePlaceholderVisualState2 = new ChunkManager.ZonePlaceholderVisualState
		{
			renderers = componentsInChildren,
			authoredEnabled = new bool[componentsInChildren.Length]
		};
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			zonePlaceholderVisualState2.authoredEnabled[i] = componentsInChildren[i] != null && componentsInChildren[i].enabled;
		}
		this._zonePlaceholderStates.Add(spawnTransform, zonePlaceholderVisualState2);
		return zonePlaceholderVisualState2;
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x0001C974 File Offset: 0x0001AB74
	private static void RestoreZonePlaceholder(ChunkManager.ZonePlaceholderVisualState state)
	{
		if (((state != null) ? state.renderers : null) == null)
		{
			return;
		}
		for (int i = 0; i < state.renderers.Length; i++)
		{
			Renderer renderer = state.renderers[i];
			if (renderer != null)
			{
				renderer.enabled = state.authoredEnabled[i];
			}
		}
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x0001C9C4 File Offset: 0x0001ABC4
	private static void HideZonePlaceholder(ChunkManager.ZonePlaceholderVisualState state)
	{
		if (((state != null) ? state.renderers : null) == null)
		{
			return;
		}
		for (int i = 0; i < state.renderers.Length; i++)
		{
			Renderer renderer = state.renderers[i];
			if (renderer != null)
			{
				renderer.enabled = false;
			}
		}
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x0001CA0C File Offset: 0x0001AC0C
	private void RestoreChunkZonePlaceholders(GameObject chunkRoot)
	{
		List<Transform> list;
		if (chunkRoot == null || !this._chunkBaseTransformsCache.TryGetValue(chunkRoot, out list))
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			Transform transform = list[i];
			ChunkManager.ZonePlaceholderVisualState zonePlaceholderVisualState;
			if (transform != null && this._zonePlaceholderStates.TryGetValue(transform, out zonePlaceholderVisualState))
			{
				ChunkManager.RestoreZonePlaceholder(zonePlaceholderVisualState);
			}
		}
	}

	// Token: 0x060003BA RID: 954 RVA: 0x0001CA6C File Offset: 0x0001AC6C
	private void ForgetChunkZonePlaceholders(GameObject chunkRoot)
	{
		List<Transform> list;
		if (chunkRoot == null || !this._chunkBaseTransformsCache.TryGetValue(chunkRoot, out list))
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			Transform transform = list[i];
			if (transform != null)
			{
				this._zonePlaceholderStates.Remove(transform);
			}
		}
	}

	// Token: 0x060003BB RID: 955 RVA: 0x0001CAC4 File Offset: 0x0001ACC4
	private void InvalidateChunkVisualCache(Vector2Int cell)
	{
		ChunkManager.ChunkVisualCache chunkVisualCache;
		this._chunkVisualCaches.TryGetValue(cell, out chunkVisualCache);
		this._chunkVisualCaches.Remove(cell);
		ChunkInstance chunkInstance;
		if (this._spawnedChunks.TryGetValue(cell, out chunkInstance))
		{
			this.CacheChunkVisualComponents(cell, chunkInstance, chunkVisualCache);
			if (this._visuallyHiddenChunks.Contains(cell))
			{
				this.ApplyVisualOnlyState(cell, false);
			}
		}
	}

	// Token: 0x060003BC RID: 956 RVA: 0x0001CB1C File Offset: 0x0001AD1C
	private List<Transform> FindChildrenWithName(Transform parent, string name)
	{
		List<Transform> list = new List<Transform>();
		this.FindChildrenRecursive(parent, name, list);
		return list;
	}

	// Token: 0x060003BD RID: 957 RVA: 0x0001CB3C File Offset: 0x0001AD3C
	private List<Transform> GetOrCacheBaseTransforms(GameObject chunkRoot)
	{
		if (chunkRoot == null)
		{
			return new List<Transform>();
		}
		List<Transform> list;
		if (this._chunkBaseTransformsCache.TryGetValue(chunkRoot, out list))
		{
			return list;
		}
		List<Transform> list2 = this.FindChildrenWithName(chunkRoot.transform, "Base");
		this._chunkBaseTransformsCache[chunkRoot] = list2;
		return list2;
	}

	// Token: 0x060003BE RID: 958 RVA: 0x0001CB8C File Offset: 0x0001AD8C
	private void FindChildrenRecursive(Transform parent, string name, List<Transform> result)
	{
		foreach (object obj in parent)
		{
			Transform transform = (Transform)obj;
			if (transform.name.Contains(name))
			{
				result.Add(transform);
			}
			this.FindChildrenRecursive(transform, name, result);
		}
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0001CBF8 File Offset: 0x0001ADF8
	private void OnWorldOriginShifted(Vector3 delta)
	{
		foreach (ChunkInstance chunkInstance in this._spawnedChunks.Values)
		{
			if (chunkInstance != null)
			{
				chunkInstance.worldPosition += delta;
			}
		}
		this._preparationFocusPosition += delta;
		this._lastHeavyPoiMotionPosition += delta;
		this._poiTravelDroughtScheduler.ApplyWorldOriginShift(new Vector2(delta.x, delta.z));
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x0001CCA0 File Offset: 0x0001AEA0
	public Vector2Int WorldToCell(Vector3 worldPos)
	{
		worldPos = WorldOrigin.ToLogical(worldPos);
		return new Vector2Int(Mathf.RoundToInt(worldPos.x / this.cellSize), Mathf.RoundToInt(worldPos.z / this.cellSize));
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x0001CCD4 File Offset: 0x0001AED4
	public Vector3 CellToWorld(Vector2Int cell, Vector2Int size)
	{
		Vector3 vector = new Vector3((float)cell.x * this.cellSize, 0f, (float)cell.y * this.cellSize);
		if (size.x > 1 || size.y > 1)
		{
			vector += new Vector3((float)(size.x - 1) * this.cellSize * 0.5f, 0f, (float)(size.y - 1) * this.cellSize * 0.5f);
		}
		return WorldOrigin.ToUnity(vector);
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x0001CD64 File Offset: 0x0001AF64
	private void MarkCellsOccupied(Vector2Int origin, Vector2Int size)
	{
		for (int i = 0; i < size.x; i++)
		{
			for (int j = 0; j < size.y; j++)
			{
				Vector2Int vector2Int = origin + new Vector2Int(i, j);
				this._processedCells.Add(vector2Int);
				this._cellToChunkMap[vector2Int] = origin;
				if (this._occupiedCellsNative.IsCreated)
				{
					this._occupiedCellsNative.Add(new int2(vector2Int.x, vector2Int.y));
				}
			}
		}
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x0001CDEC File Offset: 0x0001AFEC
	private bool AreCellsFree(Vector2Int origin, Vector2Int size)
	{
		for (int i = 0; i < size.x; i++)
		{
			for (int j = 0; j < size.y; j++)
			{
				if (this._processedCells.Contains(origin + new Vector2Int(i, j)))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x0001CE3C File Offset: 0x0001B03C
	private List<Vector2Int> GetFreeCellsInRadius(Vector2Int center, int radius)
	{
		this._freeCellsCache.Clear();
		for (int i = -radius; i <= radius; i++)
		{
			for (int j = -radius; j <= radius; j++)
			{
				Vector2Int vector2Int = center + new Vector2Int(i, j);
				if (!this._cellToChunkMap.ContainsKey(vector2Int))
				{
					this._freeCellsCache.Add(vector2Int);
				}
			}
		}
		return this._freeCellsCache;
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x0001CE9C File Offset: 0x0001B09C
	private void CacheChunkWeights()
	{
		this._smallChunks.Clear();
		this._largeChunks.Clear();
		this._smallChunksWeight = 0f;
		this._largeChunksWeight = 0f;
		this._totalSpawnWeight = 0f;
		if (this.availableChunks == null)
		{
			return;
		}
		foreach (ChunkData chunkData in this.availableChunks)
		{
			if (!(chunkData == null) && RankPoiPolicy.IsChunkAllowed(chunkData))
			{
				this._totalSpawnWeight += chunkData.spawnWeight;
				if (chunkData.gridSize.x <= 1 && chunkData.gridSize.y <= 1)
				{
					this._smallChunks.Add(chunkData);
					this._smallChunksWeight += chunkData.spawnWeight;
				}
				else
				{
					this._largeChunks.Add(chunkData);
					this._largeChunksWeight += chunkData.spawnWeight;
				}
			}
		}
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[ChunkManager] Chunk weights cached: {0} small (weight: {1:F1}), {2} large (weight: {3:F1}), total: {4:F1}", new object[]
			{
				this._smallChunks.Count,
				this._smallChunksWeight,
				this._largeChunks.Count,
				this._largeChunksWeight,
				this._totalSpawnWeight
			}));
		}
		if (this._smallChunks.Count == 0)
		{
			Debug.LogWarning("[ChunkManager] ATTENZIONE: Nessun chunk 1x1 disponibile! Potrebbero rimanere buchi nella mappa.");
		}
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x0001D00C File Offset: 0x0001B20C
	private ChunkData SelectChunkForCell(Vector2Int cell, ChunkData excludedChunk = null, bool excludeHeavyMultiCell = false, bool excludeSpecialChunks = false)
	{
		if (this.availableChunks == null || this.availableChunks.Length == 0)
		{
			return null;
		}
		this._validChunksCache.Clear();
		float num = 0f;
		foreach (ChunkData chunkData in this.availableChunks)
		{
			if (!(chunkData == null) && !(chunkData == excludedChunk) && (!excludeHeavyMultiCell || !ChunkManager.IsHeavyMultiCellChunk(chunkData)) && (!excludeSpecialChunks || !ProceduralZonePolicy.IsAlwaysSpecialChunk(chunkData)) && this.AreCellsFree(cell, chunkData.gridSize))
			{
				this._validChunksCache.Add(new ValueTuple<ChunkData, float>(chunkData, chunkData.spawnWeight));
				num += chunkData.spawnWeight;
			}
		}
		if (this._validChunksCache.Count == 0)
		{
			return null;
		}
		float num2 = global::UnityEngine.Random.Range(0f, num);
		float num3 = 0f;
		foreach (ValueTuple<ChunkData, float> valueTuple in this._validChunksCache)
		{
			ChunkData item = valueTuple.Item1;
			float item2 = valueTuple.Item2;
			num3 += item2;
			if (num2 <= num3)
			{
				return item;
			}
		}
		return this._validChunksCache[this._validChunksCache.Count - 1].Item1;
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x0001D15C File Offset: 0x0001B35C
	private static bool IsHeavyMultiCellChunk(ChunkData chunk)
	{
		return chunk != null && chunk.gridSize.x * chunk.gridSize.y >= 9;
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x0001D188 File Offset: 0x0001B388
	private static bool RequiresStableTerrainOrientation(ChunkData chunk)
	{
		if (((chunk != null) ? chunk.availableZones : null) == null)
		{
			return false;
		}
		for (int i = 0; i < chunk.availableZones.Count; i++)
		{
			ZoneData zoneData = chunk.availableZones[i];
			HeavyZoneStreamingProxy heavyZoneStreamingProxy = ((((zoneData != null) ? zoneData.zonePrefab : null) != null) ? zoneData.zonePrefab.GetComponent<HeavyZoneStreamingProxy>() : null);
			if (heavyZoneStreamingProxy != null && heavyZoneStreamingProxy.ResourcesPrefabPath.IndexOf("PARK - 3x3", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x0001D20C File Offset: 0x0001B40C
	private bool HasSpawnedHeavyMultiCellChunk()
	{
		foreach (ChunkInstance chunkInstance in this._spawnedChunks.Values)
		{
			if (chunkInstance != null && ChunkManager.IsHeavyMultiCellChunk(chunkInstance.chunkData))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060003CA RID: 970 RVA: 0x0001D274 File Offset: 0x0001B474
	private bool HasAnySpecialZoneReservation()
	{
		ProceduralPoiKind proceduralPoiKind;
		return this.HasBlockingSpecialZoneReservation(false, out proceduralPoiKind);
	}

	// Token: 0x060003CB RID: 971 RVA: 0x0001D28C File Offset: 0x0001B48C
	private bool HasBlockingSpecialZoneReservation(bool allowCompactLookahead, out ProceduralPoiKind pendingCompactKind)
	{
		pendingCompactKind = ProceduralPoiKind.None;
		if (this._retiringSpecialZoneReservationCount > 0)
		{
			return true;
		}
		if (this._specialZoneReservationCells.Count == 0)
		{
			return false;
		}
		Vector2Int vector2Int;
		if (!this.TryGetCurrentStreamingPlayerCell(out vector2Int))
		{
			return true;
		}
		float num = Mathf.Max(1f, this.cellSize);
		float num2 = (allowCompactLookahead ? Mathf.Min(num, Mathf.Max(0f, PoiTravelDroughtScheduler.CalculateStreamingApproachLeadMetres(this.spawnRadius, this.minVisibleRadius, num) - 300f)) : 0f);
		int num3 = 0;
		ProceduralPoiKind proceduralPoiKind = ProceduralPoiKind.None;
		foreach (Vector2Int vector2Int2 in this._specialZoneReservationCells)
		{
			ChunkSpawnResolver.SpawnEntry spawnEntry;
			if (!this._committedSpawnPlan.TryGetValue(vector2Int2, out spawnEntry) || !spawnEntry.IsValid || spawnEntry.chunkData == null)
			{
				if (!this._poiOriginsReachedNearbyCore.Contains(vector2Int2))
				{
					return true;
				}
			}
			else
			{
				Vector2Int gridSize = spawnEntry.chunkData.gridSize;
				if (!this._poiOriginsReachedNearbyCore.Contains(vector2Int2))
				{
					if (ChunkManager.ChebyshevStepsToChunk(vector2Int2, gridSize, vector2Int) <= 1)
					{
						return true;
					}
					if (this._streamingTravelDirection == Vector2Int.zero)
					{
						return true;
					}
					if (ChunkManager.DoesChunkFootprintIntersectForwardCorridor(vector2Int2, gridSize, vector2Int, this._streamingTravelDirection))
					{
						num3++;
						float num4 = (float)(ChunkManager.ChebyshevStepsToChunk(vector2Int2, gridSize, vector2Int) - 1) * num;
						if (num2 <= 0f || num3 > 1 || !ChunkManager.SelectionContainsDroughtReliefZone(spawnEntry, spawnEntry.chunkData) || ChunkManager.SelectionContainsLargePoi(spawnEntry, spawnEntry.chunkData) || num4 > num2)
						{
							return true;
						}
						proceduralPoiKind = ChunkManager.GetSelectedPoiKind(spawnEntry, spawnEntry.chunkData);
					}
				}
			}
		}
		pendingCompactKind = proceduralPoiKind;
		return false;
	}

	// Token: 0x060003CC RID: 972 RVA: 0x0001D468 File Offset: 0x0001B668
	private static bool DoesChunkFootprintIntersectForwardCorridor(Vector2Int origin, Vector2Int size, Vector2Int playerCell, Vector2Int direction)
	{
		int num = Mathf.Max(1, size.x);
		int num2 = Mathf.Max(1, size.y);
		int num3 = Mathf.Abs(direction.x) + Mathf.Abs(direction.y);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				int num4 = origin.x + i - playerCell.x;
				int num5 = origin.y + j - playerCell.y;
				int num6 = num4 * direction.x + num5 * direction.y;
				int num7 = Mathf.Abs(num4 * direction.y - num5 * direction.x);
				if (num6 > 0 && num7 <= num3)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060003CD RID: 973 RVA: 0x0001D52C File Offset: 0x0001B72C
	private void ApplyPoiTravelHybridPolicy(Vector2Int cell, Vector2Int playerCell, bool hasCommittedEntry, ref ChunkSpawnResolver.SpawnEntry entry, ref ChunkData chunkData)
	{
		if (RunModeService.HasSeededFreeWorld)
		{
			return;
		}
		if (hasCommittedEntry || chunkData == null || this._spawnResolver == null)
		{
			return;
		}
		ProceduralPoiKind proceduralPoiKind;
		bool flag = this.HasBlockingSpecialZoneReservation(true, out proceduralPoiKind);
		PoiTypeWeightMultipliers poiTypeWeightsAfterPending = this._poiTravelDroughtScheduler.GetPoiTypeWeightsAfterPending(proceduralPoiKind);
		float num = PoiTravelDroughtScheduler.CalculateStreamingApproachLeadMetres(this.spawnRadius, this.minVisibleRadius, this.cellSize);
		bool flag2 = this._poiTravelDroughtScheduler.IsPoiPending(num);
		bool flag3 = ChunkManager.SelectionContainsLargePoi(entry, chunkData) && !this._poiTravelDroughtScheduler.ShouldBlockLargePoi;
		if (flag2 && !flag3 && !flag && this.IsEligibleDroughtReliefSpawnCell(cell, playerCell) && !this.HasCommittedSpecialSpacingConflict(cell, Vector2Int.one))
		{
			ChunkSpawnResolver.SpawnEntry spawnEntry = this._spawnResolver.ResolveDroughtRelief(cell, poiTypeWeightsAfterPending);
			if (spawnEntry.IsValid && spawnEntry.chunkData != null && spawnEntry.chunkData.gridSize.x <= 1 && spawnEntry.chunkData.gridSize.y <= 1 && ChunkManager.SelectionContainsDroughtReliefZone(spawnEntry, spawnEntry.chunkData))
			{
				entry = spawnEntry;
				chunkData = spawnEntry.chunkData;
				return;
			}
			if (this._poiTravelDroughtScheduler.HardLimitReached)
			{
				string text = "[ChunkManager] Relief POI hard fallback unavailable at ";
				Vector2Int vector2Int = cell;
				Debug.LogError(text + vector2Int.ToString() + ". The catalog must expose a global Kombini, Pool or Art Gallery candidate.");
			}
		}
		if (ChunkManager.SelectionContainsSpecialZone(entry, chunkData))
		{
			entry = this._spawnResolver.ResolveSelectedSpecialWithRecency(cell, entry, poiTypeWeightsAfterPending);
			chunkData = (entry.IsValid ? entry.chunkData : null);
			if (ChunkManager.SelectionContainsSpecialZone(entry, chunkData) && !this._spawnResolver.PassesUniqueSpecialRecencyAdmission(cell, entry, poiTypeWeightsAfterPending))
			{
				this.RebuildEntryWithoutSpecialZones(cell, ref entry, ref chunkData);
			}
		}
		if (ChunkManager.SelectionContainsSpecialZone(entry, chunkData) && !ChunkManager.DoesChunkFootprintIntersectForwardCorridor(cell, chunkData.gridSize, playerCell, this._streamingTravelDirection))
		{
			this.RebuildEntryWithoutSpecialZones(cell, ref entry, ref chunkData);
		}
		if (ChunkManager.SelectionContainsSpecialZone(entry, chunkData) && !ChunkManager.SelectionContainsLargePoi(entry, chunkData) && !flag2)
		{
			this.RebuildEntryWithoutSpecialZones(cell, ref entry, ref chunkData);
		}
		if (ChunkManager.SelectionContainsLargePoi(entry, chunkData) && this._poiTravelDroughtScheduler.ShouldBlockLargePoi)
		{
			this.RebuildEntryWithoutSpecialZones(cell, ref entry, ref chunkData);
		}
	}

	// Token: 0x060003CE RID: 974 RVA: 0x0001D78C File Offset: 0x0001B98C
	private bool IsEligibleDroughtReliefSpawnCell(Vector2Int cell, Vector2Int playerCell)
	{
		int num = cell.x - playerCell.x;
		int num2 = cell.y - playerCell.y;
		if (Mathf.Max(Mathf.Abs(num), Mathf.Abs(num2)) > Mathf.Max(1, this.spawnRadius + 1))
		{
			return false;
		}
		if (this._streamingTravelDirection == Vector2Int.zero)
		{
			return false;
		}
		int num3 = num * this._streamingTravelDirection.x + num2 * this._streamingTravelDirection.y;
		int num4 = Mathf.Abs(num * this._streamingTravelDirection.y - num2 * this._streamingTravelDirection.x);
		return num3 > 0 && num4 <= 1;
	}

	// Token: 0x060003CF RID: 975 RVA: 0x0001D83C File Offset: 0x0001BA3C
	private void RebuildEntryWithoutSpecialZones(Vector2Int cell, ref ChunkSpawnResolver.SpawnEntry entry, ref ChunkData chunkData)
	{
		if (chunkData == null)
		{
			return;
		}
		entry = (ProceduralZonePolicy.IsAlwaysSpecialChunk(chunkData) ? this._spawnResolver.ResolveSmallFallback(cell, chunkData, true) : this._spawnResolver.ResolveWithoutSpecialZones(cell, chunkData));
		chunkData = (entry.IsValid ? entry.chunkData : null);
		if (chunkData != null && ChunkManager.SelectionContainsSpecialZone(entry, chunkData))
		{
			Debug.LogError("[ChunkManager] Il fallback hybrid anti-POI contiene ancora un POI; " + string.Format("spawn annullato in {0}.", cell));
			chunkData = null;
		}
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x0001D8D0 File Offset: 0x0001BAD0
	private bool HasCommittedSpecialSpacingConflict(Vector2Int origin, Vector2Int size)
	{
		int num = Mathf.Max(1, size.x);
		int num2 = Mathf.Max(1, size.y);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				Vector2Int vector2Int = origin + new Vector2Int(i, j);
				Vector2Int vector2Int2;
				if (this._committedSpecialExclusionOwners.TryGetValue(vector2Int, out vector2Int2) && vector2Int2 != origin)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x0001D940 File Offset: 0x0001BB40
	private void RegisterCommittedSpecialExclusion(Vector2Int origin, Vector2Int size)
	{
		int num = Mathf.Max(1, size.x);
		int num2 = Mathf.Max(1, size.y);
		for (int i = -2; i < num + 2; i++)
		{
			for (int j = -2; j < num2 + 2; j++)
			{
				Vector2Int vector2Int = origin + new Vector2Int(i, j);
				Vector2Int vector2Int2;
				if (!this._committedSpecialExclusionOwners.TryGetValue(vector2Int, out vector2Int2) || !(vector2Int2 != origin))
				{
					this._committedSpecialExclusionOwners[vector2Int] = origin;
				}
			}
		}
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x0001D9C0 File Offset: 0x0001BBC0
	private void UnregisterCommittedSpecialExclusion(Vector2Int origin, Vector2Int size)
	{
		int num = Mathf.Max(1, size.x);
		int num2 = Mathf.Max(1, size.y);
		for (int i = -2; i < num + 2; i++)
		{
			for (int j = -2; j < num2 + 2; j++)
			{
				Vector2Int vector2Int = origin + new Vector2Int(i, j);
				Vector2Int vector2Int2;
				if (this._committedSpecialExclusionOwners.TryGetValue(vector2Int, out vector2Int2) && vector2Int2 == origin)
				{
					this._committedSpecialExclusionOwners.Remove(vector2Int);
				}
			}
		}
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x0001DA3F File Offset: 0x0001BC3F
	private void RecordAcceptedPoiForTravelPolicy(ChunkSpawnResolver.SpawnEntry entry, ChunkData chunkData, bool wasAlreadyCommitted)
	{
		if (!ChunkManager.SelectionContainsSpecialZone(entry, chunkData))
		{
			return;
		}
		if (!wasAlreadyCommitted && ChunkManager.SelectionContainsLargePoi(entry, chunkData))
		{
			this._poiTravelDroughtScheduler.MarkLargePoiAccepted();
		}
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x0001DA64 File Offset: 0x0001BC64
	private void ApplyActivePoiLimit(Vector2Int cell, ref ChunkSpawnResolver.SpawnEntry entry, ref ChunkData chunkData)
	{
		if (RunModeService.HasSeededFreeWorld)
		{
			return;
		}
		bool flag = ChunkManager.SelectionContainsDroughtReliefZone(entry, chunkData) && !ChunkManager.SelectionContainsLargePoi(entry, chunkData);
		ProceduralPoiKind proceduralPoiKind;
		if (chunkData == null || !ChunkManager.SelectionContainsSpecialZone(entry, chunkData) || !this.HasBlockingSpecialZoneReservation(flag, out proceduralPoiKind))
		{
			return;
		}
		ChunkData chunkData2 = chunkData;
		this.RebuildEntryWithoutSpecialZones(cell, ref entry, ref chunkData);
		if (this.enableVerboseDiagnostics)
		{
			string text = string.Format("[ChunkManager] POI '{0}' bloccato in {1}: ", chunkData2.name, cell);
			string text2 = "un altro POI è già attivo/in retirement; fallback '";
			ChunkData chunkData3 = chunkData;
			Debug.Log(text + text2 + (((chunkData3 != null) ? chunkData3.name : null) ?? "nessuno") + "'.");
		}
		if (chunkData != null && ChunkManager.SelectionContainsSpecialZone(entry, chunkData))
		{
			Debug.LogError("[ChunkManager] Il fallback anti-POI contiene ancora un POI; " + string.Format("spawn annullato in {0}.", cell));
			chunkData = null;
		}
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x0001DB54 File Offset: 0x0001BD54
	private static bool SelectionContainsSpecialZone(ChunkSpawnResolver.SpawnEntry entry, ChunkData chunkData)
	{
		if (ProceduralZonePolicy.IsAlwaysSpecialChunk(chunkData))
		{
			return true;
		}
		if (!entry.IsValid || entry.zoneIndices == null || ((chunkData != null) ? chunkData.availableZones : null) == null)
		{
			return false;
		}
		for (int i = 0; i < entry.zoneIndices.Length; i++)
		{
			int num = entry.zoneIndices[i];
			if (num >= 0 && num < chunkData.availableZones.Count && ProceduralZonePolicy.IsSpecialZone(chunkData.availableZones[num]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x0001DBD0 File Offset: 0x0001BDD0
	private static bool SelectionContainsDroughtReliefZone(ChunkSpawnResolver.SpawnEntry entry, ChunkData chunkData)
	{
		if (!entry.IsValid || entry.zoneIndices == null || ((chunkData != null) ? chunkData.availableZones : null) == null)
		{
			return false;
		}
		for (int i = 0; i < entry.zoneIndices.Length; i++)
		{
			int num = entry.zoneIndices[i];
			if (num >= 0 && num < chunkData.availableZones.Count && ProceduralZonePolicy.IsDroughtReliefZone(chunkData.availableZones[num]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0001DC44 File Offset: 0x0001BE44
	private static ProceduralPoiKind GetSelectedPoiKind(ChunkSpawnResolver.SpawnEntry entry, ChunkData chunkData)
	{
		if (entry.IsValid && entry.zoneIndices != null && ((chunkData != null) ? chunkData.availableZones : null) != null)
		{
			for (int i = 0; i < entry.zoneIndices.Length; i++)
			{
				int num = entry.zoneIndices[i];
				if (num >= 0 && num < chunkData.availableZones.Count)
				{
					ProceduralPoiKind poiKind = ProceduralZonePolicy.GetPoiKind(chunkData.availableZones[num]);
					if (poiKind != ProceduralPoiKind.None)
					{
						return poiKind;
					}
				}
			}
		}
		if (chunkData != null && chunkData.containsSubway)
		{
			return ProceduralPoiKind.Subway;
		}
		return ProceduralPoiKind.None;
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0001DCCC File Offset: 0x0001BECC
	private static bool IsSelectedSpecialZoneRoot(ChunkSpawnResolver.SpawnEntry entry, ChunkData chunkData, GameObject zoneRoot)
	{
		if (zoneRoot == null || ((chunkData != null) ? chunkData.availableZones : null) == null)
		{
			return false;
		}
		bool flag = false;
		int[] zoneIndices = entry.zoneIndices;
		int num = ((zoneIndices != null) ? zoneIndices.Length : 0);
		for (int i = 0; i < num; i++)
		{
			int num2 = entry.zoneIndices[i];
			if (num2 >= 0 && num2 < chunkData.availableZones.Count)
			{
				ZoneData zoneData = chunkData.availableZones[num2];
				if (ProceduralZonePolicy.IsSpecialZone(zoneData))
				{
					flag = true;
					if (zoneRoot.name == "Zone_" + zoneData.name)
					{
						return true;
					}
				}
			}
		}
		return chunkData.containsSubway && !flag;
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x0001DD74 File Offset: 0x0001BF74
	private static bool SetSelectedSpecialZonePresentation(ChunkSpawnResolver.SpawnEntry entry, ChunkInstance chunk, bool presented)
	{
		if (!entry.IsValid || ((chunk != null) ? chunk.spawnedZones : null) == null)
		{
			return false;
		}
		if (!presented && ChunkManager.HasActivePoiResidencyUse(chunk))
		{
			return true;
		}
		bool flag = false;
		for (int i = 0; i < chunk.spawnedZones.Count; i++)
		{
			GameObject gameObject = chunk.spawnedZones[i];
			if (ChunkManager.IsSelectedSpecialZoneRoot(entry, chunk.chunkData, gameObject))
			{
				flag = true;
				if (gameObject.activeSelf != presented)
				{
					gameObject.SetActive(presented);
				}
			}
		}
		return flag;
	}

	// Token: 0x060003DA RID: 986 RVA: 0x0001DDF0 File Offset: 0x0001BFF0
	private static bool IsSelectedSpecialZonePresentationReady(ChunkSpawnResolver.SpawnEntry entry, ChunkInstance chunk)
	{
		if (!entry.IsValid || ((chunk != null) ? chunk.spawnedZones : null) == null)
		{
			return false;
		}
		for (int i = 0; i < chunk.spawnedZones.Count; i++)
		{
			GameObject gameObject = chunk.spawnedZones[i];
			if (ChunkManager.IsSelectedSpecialZoneRoot(entry, chunk.chunkData, gameObject) && gameObject.activeInHierarchy)
			{
				HeavyZoneStreamingProxy component = gameObject.GetComponent<HeavyZoneStreamingProxy>();
				if (component != null)
				{
					if (component.IsOperationalReady && component.PayloadInstance != null && component.PayloadInstance.activeInHierarchy)
					{
						return true;
					}
				}
				else
				{
					IncrementalPooledZoneActivation component2 = gameObject.GetComponent<IncrementalPooledZoneActivation>();
					if (component2 == null || !component2.HasSnapshot || component2.IsOperationalReady)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x060003DB RID: 987 RVA: 0x0001DEAC File Offset: 0x0001C0AC
	private static bool HasMaterializedSelectedSpecialZone(ChunkSpawnResolver.SpawnEntry entry, ChunkInstance chunk)
	{
		if (entry.IsValid)
		{
			bool flag;
			if (chunk == null)
			{
				flag = null != null;
			}
			else
			{
				ChunkData chunkData = chunk.chunkData;
				flag = ((chunkData != null) ? chunkData.availableZones : null) != null;
			}
			if (flag && chunk.spawnedZones != null && chunk.spawnedZones.Count != 0)
			{
				int[] zoneIndices = entry.zoneIndices;
				int num = ((zoneIndices != null) ? zoneIndices.Length : 0);
				for (int i = 0; i < num; i++)
				{
					int num2 = entry.zoneIndices[i];
					if (num2 >= 0 && num2 < chunk.chunkData.availableZones.Count)
					{
						ZoneData zoneData = chunk.chunkData.availableZones[num2];
						if (ProceduralZonePolicy.IsSpecialZone(zoneData))
						{
							string text = "Zone_" + zoneData.name;
							for (int j = 0; j < chunk.spawnedZones.Count; j++)
							{
								GameObject gameObject = chunk.spawnedZones[j];
								if (gameObject != null && gameObject.name == text)
								{
									return true;
								}
							}
						}
					}
				}
				return chunk.chunkData.containsSubway && chunk.spawnedZones.Count > 0;
			}
		}
		return false;
	}

	// Token: 0x060003DC RID: 988 RVA: 0x0001DFC8 File Offset: 0x0001C1C8
	private static bool SelectionContainsLargePoi(ChunkSpawnResolver.SpawnEntry entry, ChunkData chunkData)
	{
		if (chunkData == null)
		{
			return false;
		}
		if (chunkData.containsSubway)
		{
			return true;
		}
		if (entry.IsValid && entry.zoneIndices != null && chunkData.availableZones != null)
		{
			for (int i = 0; i < entry.zoneIndices.Length; i++)
			{
				int num = entry.zoneIndices[i];
				if (num >= 0 && num < chunkData.availableZones.Count && ProceduralZonePolicy.IsLargePoiZone(chunkData.availableZones[num]))
				{
					return true;
				}
			}
			return false;
		}
		return ProceduralZonePolicy.IsAlwaysSpecialChunk(chunkData) && ProceduralZonePolicy.IsLargePoiChunk(chunkData);
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0001E058 File Offset: 0x0001C258
	private static bool SelectionContainsSpecialZoneExceptSlot(ChunkSpawnResolver.SpawnEntry entry, ChunkData chunkData, int excludedSlotIndex)
	{
		if (!entry.IsValid || entry.zoneIndices == null || ((chunkData != null) ? chunkData.availableZones : null) == null)
		{
			return false;
		}
		for (int i = 0; i < entry.zoneIndices.Length; i++)
		{
			if (i != excludedSlotIndex)
			{
				int num = entry.zoneIndices[i];
				if (num >= 0 && num < chunkData.availableZones.Count && ProceduralZonePolicy.IsSpecialZone(chunkData.availableZones[num]))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060003DE RID: 990 RVA: 0x0001E0D0 File Offset: 0x0001C2D0
	private static bool ChunkContainsSpecialZone(ChunkInstance chunk)
	{
		if (chunk == null)
		{
			return false;
		}
		if (ProceduralZonePolicy.IsAlwaysSpecialChunk(chunk.chunkData))
		{
			return true;
		}
		GameObject chunkRoot = chunk.chunkRoot;
		if (chunkRoot == null)
		{
			return false;
		}
		if (chunkRoot.GetComponentInChildren<SubwayRideDirector>(true) != null)
		{
			return true;
		}
		if (chunkRoot.GetComponentInChildren<KombiniSlidingDoor>(true) != null)
		{
			return true;
		}
		HeavyZoneStreamingProxy[] componentsInChildren = chunkRoot.GetComponentsInChildren<HeavyZoneStreamingProxy>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			string text = ((componentsInChildren[i] != null) ? componentsInChildren[i].ResourcesPrefabPath : null);
			if (!string.IsNullOrEmpty(text) && (text.IndexOf("ParkRuntime/", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("ShellRuntime/", StringComparison.OrdinalIgnoreCase) >= 0))
			{
				return true;
			}
		}
		string name = chunkRoot.name;
		return !string.IsNullOrEmpty(name) && (name.IndexOf("PARK - 3x3", StringComparison.OrdinalIgnoreCase) >= 0 || name.IndexOf(" Shell", StringComparison.OrdinalIgnoreCase) >= 0);
	}

	// Token: 0x060003DF RID: 991 RVA: 0x0001E1B4 File Offset: 0x0001C3B4
	public ChunkInstance GetChunkContaining(Transform t)
	{
		if (t == null)
		{
			return null;
		}
		foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._spawnedChunks)
		{
			ChunkInstance value = keyValuePair.Value;
			if (((value != null) ? value.chunkRoot : null) != null && t.IsChildOf(value.chunkRoot.transform))
			{
				return value;
			}
		}
		return null;
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x0001E244 File Offset: 0x0001C444
	public bool TryObserveMaterializedPoi(Vector2Int originCell, int slotIndex, string expectedAddress, out GameObject root, out bool operational)
	{
		root = null;
		operational = false;
		PoiResidencyCatalog.Entry entry;
		ChunkInstance chunkInstance;
		if (!this.IsTearingDown && PoiResidencyCatalog.TryGetByAddress(expectedAddress, out entry) && PoiResidencyPrefabResolver.IsDirectBaselineFamily(entry.family) && !MapResidencyPilotSelection.IsFamilyEnabled(entry.family) && this._spawnedChunks.TryGetValue(originCell, out chunkInstance) && !(((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null))
		{
			ChunkData chunkData = chunkInstance.chunkData;
			ChunkSpawnResolver.SpawnEntry spawnEntry;
			if (((chunkData != null) ? chunkData.availableZones : null) != null && chunkInstance.spawnedZones != null && this._committedSpawnPlan.TryGetValue(originCell, out spawnEntry) && !(spawnEntry.chunkData != chunkInstance.chunkData) && spawnEntry.zoneIndices != null && slotIndex >= 0 && slotIndex < spawnEntry.zoneIndices.Length)
			{
				int num = spawnEntry.zoneIndices[slotIndex];
				if (num < 0 || num >= chunkInstance.chunkData.availableZones.Count)
				{
					return false;
				}
				ZoneData zoneData = chunkInstance.chunkData.availableZones[num];
				HeavyZoneStreamingProxy heavyZoneStreamingProxy = ((((zoneData != null) ? zoneData.zonePrefab : null) != null) ? zoneData.zonePrefab.GetComponent<HeavyZoneStreamingProxy>() : null);
				GameObject gameObject;
				List<Transform> list;
				if (heavyZoneStreamingProxy == null || heavyZoneStreamingProxy.ResourcesPrefabPath != entry.resourceKey || this._poiPrefabResolver == null || !this._poiPrefabResolver.TryGetResolved(zoneData.zonePrefab, out gameObject) || !this._chunkBaseTransformsCache.TryGetValue(chunkInstance.chunkRoot, out list))
				{
					return false;
				}
				Transform transform = ((slotIndex < list.Count) ? list[slotIndex] : chunkInstance.chunkRoot.transform);
				if (transform == null)
				{
					return false;
				}
				foreach (GameObject gameObject2 in chunkInstance.spawnedZones)
				{
					GameObject gameObject3;
					if (!(gameObject2 == null) && !(gameObject2.name != "Zone_" + zoneData.name) && !(gameObject2.transform.parent != chunkInstance.chunkRoot.transform) && (gameObject2.transform.position - transform.position).sqrMagnitude <= 0.0001f && this._zoneInstanceToPrefabMap.TryGetValue(gameObject2, out gameObject3) && !(gameObject3 != gameObject))
					{
						if (root != null)
						{
							root = null;
							return false;
						}
						root = gameObject2;
					}
				}
				if (root == null)
				{
					return false;
				}
				if (this._retiringZoneRoots.Contains(root) || root.GetComponent<HeavyZoneStreamingProxy>() != null || root.GetComponent<PoiResidencyPlacement>() != null)
				{
					root = null;
					return false;
				}
				IncrementalPooledZoneActivation component = root.GetComponent<IncrementalPooledZoneActivation>();
				operational = root.activeInHierarchy && (component == null || (component.HasSnapshot && component.IsOperationalReady && !component.IsDormant && component.StagedWakeWorkRemaining == 0));
				return true;
			}
		}
		return false;
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x0001E570 File Offset: 0x0001C770
	public bool IsRootPooledAndDormant(GameObject root)
	{
		GameObject gameObject;
		Queue<GameObject> queue;
		if (root == null || root.activeSelf || root.activeInHierarchy || this._retiringZoneRoots.Contains(root) || this._zonePoolContainer == null || root.transform.parent != this._zonePoolContainer || root.GetComponent<HeavyZoneStreamingProxy>() != null || root.GetComponent<PoiResidencyPlacement>() != null || !this._zoneInstanceToPrefabMap.TryGetValue(root, out gameObject) || gameObject == null || !this._zonePoolByPrefab.TryGetValue(gameObject, out queue) || !queue.Contains(root) || this.GetChunkContaining(root.transform) != null)
		{
			return false;
		}
		foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._spawnedChunks)
		{
			ChunkInstance value = keyValuePair.Value;
			if (((value != null) ? value.spawnedZones : null) != null && keyValuePair.Value.spawnedZones.Contains(root))
			{
				return false;
			}
		}
		using (Queue<ChunkManager.PendingZoneRetirement>.Enumerator enumerator2 = this._pendingZoneRetirements.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				if (enumerator2.Current.zoneRoot == root)
				{
					return false;
				}
			}
		}
		IncrementalPooledZoneActivation component = root.GetComponent<IncrementalPooledZoneActivation>();
		return component == null || (component.HasSnapshot && component.IsDormant && !component.IsOperationalReady && component.StagedSleepWorkRemaining == 0 && !this._incrementalNormalZoneOwners.ContainsKey(component) && !this._pendingNormalZoneWakeSet.Contains(component));
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x0001E748 File Offset: 0x0001C948
	public ChunkInstance GetChunkAt(Vector2Int cell)
	{
		ChunkInstance chunkInstance;
		if (this._spawnedChunks.TryGetValue(cell, out chunkInstance))
		{
			return chunkInstance;
		}
		Vector2Int vector2Int;
		if (this._cellToChunkMap.TryGetValue(cell, out vector2Int))
		{
			return this._spawnedChunks.GetValueOrDefault(vector2Int);
		}
		return null;
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x0001E785 File Offset: 0x0001C985
	public List<ChunkInstance> GetAllChunks()
	{
		return new List<ChunkInstance>(this._spawnedChunks.Values);
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x0001E798 File Offset: 0x0001C998
	private void OnDrawGizmos()
	{
		if (!this.drawGizmos)
		{
			return;
		}
		if (!Application.isPlaying)
		{
			return;
		}
		if (PlayerProximityProvider.Instance == null)
		{
			return;
		}
		Vector3 position = PlayerProximityProvider.Instance.Position;
		Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
		this.DrawCircleGizmo(position, (float)this.spawnRadius * this.cellSize);
		Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
		this.DrawCircleGizmo(position, (float)this.minVisibleRadius * this.cellSize);
		Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
		this.DrawCircleGizmo(position, (float)this.maxVisibleRadius * this.cellSize);
		Camera main = Camera.main;
		if (main != null)
		{
			Gizmos.color = new Color(0f, 0.5f, 1f, 0.4f);
			Vector3 forward = main.transform.forward;
			forward.y = 0f;
			forward.Normalize();
			float viewAngle = this.viewAngle;
			float num = (float)this.maxVisibleRadius * this.cellSize;
			Vector3 vector = Quaternion.Euler(0f, -this.viewAngle * 0.5f, 0f) * forward;
			Vector3 vector2 = Quaternion.Euler(0f, this.viewAngle * 0.5f, 0f) * forward;
			Gizmos.DrawLine(position, position + vector * num);
			Gizmos.DrawLine(position, position + vector2 * num);
			this.DrawArcGizmo(position, forward, num, this.viewAngle);
		}
		Vector2Int vector2Int = this.WorldToCell(position);
		Vector3 vector3 = this.CellToWorld(vector2Int, Vector2Int.one);
		Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
		Gizmos.DrawWireCube(vector3, new Vector3(this.cellSize, 2f, this.cellSize));
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x0001E99C File Offset: 0x0001CB9C
	private void DrawArcGizmo(Vector3 center, Vector3 forward, float radius, float angle)
	{
		int num = 16;
		float num2 = angle * 0.5f;
		float num3 = angle / (float)num;
		Vector3 vector = center + Quaternion.Euler(0f, -num2, 0f) * forward * radius;
		for (int i = 1; i <= num; i++)
		{
			float num4 = -num2 + (float)i * num3;
			Vector3 vector2 = center + Quaternion.Euler(0f, num4, 0f) * forward * radius;
			Gizmos.DrawLine(vector, vector2);
			vector = vector2;
		}
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x0001EA2C File Offset: 0x0001CC2C
	private void DrawCircleGizmo(Vector3 center, float radius)
	{
		int num = 32;
		float num2 = 360f / (float)num;
		Vector3 vector = center + new Vector3(radius, 0f, 0f);
		for (int i = 1; i <= num; i++)
		{
			float num3 = (float)i * num2 * 0.017453292f;
			Vector3 vector2 = center + new Vector3(Mathf.Cos(num3) * radius, 0f, Mathf.Sin(num3) * radius);
			Gizmos.DrawLine(vector, vector2);
			vector = vector2;
		}
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x0001EAA4 File Offset: 0x0001CCA4
	private static Vector2Int SubwayDirectionKey(Vector3 direction)
	{
		if (Mathf.Abs(direction.x) < Mathf.Abs(direction.z))
		{
			return new Vector2Int(0, (direction.z >= 0f) ? 1 : (-1));
		}
		return new Vector2Int((direction.x >= 0f) ? 1 : (-1), 0);
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x0001EAF8 File Offset: 0x0001CCF8
	public bool TryGetSubwayConnection(Vector2Int source, Vector3 direction, out ChunkManager.SubwayConnection connection)
	{
		return this._subwayConnections.TryGetValue(new ValueTuple<Vector2Int, Vector2Int>(source, ChunkManager.SubwayDirectionKey(direction)), out connection);
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x0001EB12 File Offset: 0x0001CD12
	public bool CanCreateSubwayConnection(Vector2Int source, Vector3 direction, Vector2Int destination, Vector3 arrivalDirection)
	{
		return !this._subwayConnections.ContainsKey(new ValueTuple<Vector2Int, Vector2Int>(source, ChunkManager.SubwayDirectionKey(direction))) && !this._subwayConnections.ContainsKey(new ValueTuple<Vector2Int, Vector2Int>(destination, -ChunkManager.SubwayDirectionKey(arrivalDirection)));
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x0001EB50 File Offset: 0x0001CD50
	public bool RememberSubwayConnection(Vector2Int source, Vector3 direction, ChunkManager.SubwayConnection outward, ChunkManager.SubwayConnection reverse)
	{
		if (!this.CanCreateSubwayConnection(source, direction, outward.destination, outward.arrivalDirection))
		{
			return false;
		}
		Vector2Int vector2Int = ChunkManager.SubwayDirectionKey(direction);
		this._subwayConnections[new ValueTuple<Vector2Int, Vector2Int>(source, vector2Int)] = outward;
		this._subwayConnections[new ValueTuple<Vector2Int, Vector2Int>(outward.destination, -ChunkManager.SubwayDirectionKey(outward.arrivalDirection))] = reverse;
		return true;
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x0001EBB8 File Offset: 0x0001CDB8
	private void EnsureConnectingSubwayZoneTargets(Dictionary<GameObject, int> targets)
	{
		if (targets == null || this.availableChunks == null || RunModeService.IsLongWayHome)
		{
			return;
		}
		foreach (ChunkData chunkData in this.availableChunks)
		{
			if (((chunkData != null) ? chunkData.availableZones : null) != null)
			{
				foreach (ZoneData zoneData in chunkData.availableZones)
				{
					if (ProceduralZonePolicy.GetPoiKind(zoneData) == ProceduralPoiKind.Subway && this.IsZoneEligibleForPrewarm(zoneData))
					{
						GameObject gameObject = this.ResolveZonePrefab(zoneData);
						if (!(gameObject == null))
						{
							int num;
							targets.TryGetValue(gameObject, out num);
							targets[gameObject] = Mathf.Max(2, num);
						}
					}
				}
			}
		}
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0001EC88 File Offset: 0x0001CE88
	public bool TryGetSubwayTravelLayout(Transform departure, out ChunkInstance station, out ChunkSpawnResolver.SpawnEntry entry)
	{
		station = this.GetChunkContaining(departure);
		entry = default(ChunkSpawnResolver.SpawnEntry);
		ChunkInstance chunkInstance = station;
		if (((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null || this._spawnResolver == null)
		{
			return false;
		}
		if (this._committedSpawnPlan.TryGetValue(station.originCell, out entry) && ChunkManager.GetSelectedPoiKind(entry, entry.chunkData) == ProceduralPoiKind.Subway)
		{
			return true;
		}
		ChunkData chunkData = station.chunkData;
		if (!ChunkSpawnResolver.IsSubwayChunk(chunkData) && this.availableChunks != null)
		{
			foreach (ChunkData chunkData2 in this.availableChunks)
			{
				if (ChunkSpawnResolver.IsSubwayChunk(chunkData2))
				{
					chunkData = chunkData2;
					break;
				}
			}
		}
		if (!ChunkSpawnResolver.IsSubwayChunk(chunkData))
		{
			return false;
		}
		int num = (Mathf.RoundToInt(station.rotation.eulerAngles.y / 90f) % 4 + 4) % 4;
		entry = this._spawnResolver.ResolveSubwayConnection(station.originCell, chunkData, num);
		return entry.IsValid;
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x0001ED80 File Offset: 0x0001CF80
	public bool CanReserveSubwayDestination(Vector2Int cell, ChunkSpawnResolver.SpawnEntry layout)
	{
		if (!layout.IsValid || this.IsPoiTravelSuspended || this.IsTearingDown || !RankPoiPolicy.IsSpawnAllowed(layout))
		{
			return false;
		}
		ChunkSpawnResolver.SpawnEntry spawnEntry;
		if (this._committedSpawnPlan.TryGetValue(cell, out spawnEntry))
		{
			if (!(spawnEntry.chunkData != layout.chunkData) && spawnEntry.rotation == layout.rotation)
			{
				int[] zoneIndices = spawnEntry.zoneIndices;
				int? num = ((zoneIndices != null) ? new int?(zoneIndices.Length) : null);
				int[] zoneIndices2 = layout.zoneIndices;
				int? num2 = ((zoneIndices2 != null) ? new int?(zoneIndices2.Length) : null);
				if ((num.GetValueOrDefault() == num2.GetValueOrDefault()) & (num != null == (num2 != null)))
				{
					int num3 = 0;
					for (;;)
					{
						int num4 = num3;
						int[] zoneIndices3 = spawnEntry.zoneIndices;
						if (num4 >= ((zoneIndices3 != null) ? zoneIndices3.Length : 0))
						{
							return true;
						}
						if (spawnEntry.zoneIndices[num3] != layout.zoneIndices[num3])
						{
							break;
						}
						num3++;
					}
					return false;
				}
			}
			return false;
		}
		for (int i = 0; i < layout.chunkData.gridSize.x; i++)
		{
			for (int j = 0; j < layout.chunkData.gridSize.y; j++)
			{
				if (this._cellToChunkMap.ContainsKey(cell + new Vector2Int(i, j)))
				{
					return false;
				}
			}
		}
		return this.AreCellsFree(cell, layout.chunkData.gridSize) && this.AreCommittedSpawnPlanCellsFree(cell, layout.chunkData.gridSize) && !this.HasCommittedSpecialSpacingConflict(cell, layout.chunkData.gridSize) && !this.HasAdjacentSubwayChunk(cell);
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x0001EF20 File Offset: 0x0001D120
	public bool TryReserveSubwayDestination(SubwayRideDirector owner, Vector2Int cell, ChunkSpawnResolver.SpawnEntry layout)
	{
		if (owner == null || RunModeService.IsLongWayHome || !this.CanReserveSubwayDestination(cell, layout) || !this.SetPreparationFocus(owner, this.CellToWorld(cell, layout.chunkData.gridSize)))
		{
			return false;
		}
		this.ReleaseDebugPoiReservation(owner);
		if (this._committedSpawnPlan.ContainsKey(cell))
		{
			return true;
		}
		this._debugPoiReservationOwner = owner;
		this._debugPoiReservationCell = cell;
		this._debugPoiReservationMaterialized = false;
		this.CommitSpawnPlanEntry(cell, layout, layout.chunkData);
		return true;
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0001EFA0 File Offset: 0x0001D1A0
	public bool TryGetReadySubwayDestination(Vector2Int cell, Vector3 direction, out SubwayRideDirector arrival)
	{
		arrival = null;
		ChunkInstance chunkInstance;
		if (!this._spawnedChunks.TryGetValue(cell, out chunkInstance) || ((chunkInstance != null) ? chunkInstance.chunkRoot : null) == null || this.HasPendingZoneContent(chunkInstance) || !this.IsPreparationAreaReady(chunkInstance.worldPosition, 0))
		{
			return false;
		}
		foreach (SubwayRideDirector subwayRideDirector in chunkInstance.chunkRoot.GetComponentsInChildren<SubwayRideDirector>(true))
		{
			if (subwayRideDirector.isActiveAndEnabled && Vector3.Dot(subwayRideDirector.TravelDirection, direction) > 0.99f && subwayRideDirector.InteriorZone != null)
			{
				arrival = subwayRideDirector;
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000182 RID: 386
	private static readonly ProfilerMarker ChunkPoolMissInstantiateMarker = new ProfilerMarker("ChunkManager.Instantiate.ChunkPoolMiss");

	// Token: 0x04000183 RID: 387
	private static readonly ProfilerMarker ZonePoolMissInstantiateMarker = new ProfilerMarker("ChunkManager.Instantiate.ZonePoolMiss");

	// Token: 0x04000184 RID: 388
	private static readonly ProfilerMarker SeededZonePoolReplenishmentMarker = new ProfilerMarker("ChunkManager.SeededZonePoolReplenishment");

	// Token: 0x04000185 RID: 389
	private static readonly ProfilerMarker ZonePoolAcquireMarker = new ProfilerMarker("ChunkManager.ZonePoolAcquire");

	// Token: 0x04000186 RID: 390
	private static readonly ProfilerMarker ZonePoolActivateMarker = new ProfilerMarker("ChunkManager.ZonePoolActivate");

	// Token: 0x04000187 RID: 391
	private static readonly ProfilerMarker ZonePoolActivateParentMarker = new ProfilerMarker("ChunkManager.ZonePoolActivate.Parent");

	// Token: 0x04000188 RID: 392
	private static readonly ProfilerMarker ZonePoolActivatePoseMarker = new ProfilerMarker("ChunkManager.ZonePoolActivate.Pose");

	// Token: 0x04000189 RID: 393
	private static readonly ProfilerMarker ZonePoolActivateRootEnableMarker = new ProfilerMarker("ChunkManager.ZonePoolActivate.RootEnable");

	// Token: 0x0400018A RID: 394
	private static readonly ProfilerMarker ZonePoolReturnMarker = new ProfilerMarker("ChunkManager.ZonePoolReturn");

	// Token: 0x0400018B RID: 395
	private static readonly ProfilerMarker ZonePoolReparentMarker = new ProfilerMarker("ChunkManager.ZonePoolReturn.Reparent");

	// Token: 0x0400018C RID: 396
	private static readonly ProfilerMarker ZonePoolDeactivateMarker = new ProfilerMarker("ChunkManager.ZonePoolReturn.Deactivate");

	// Token: 0x0400018D RID: 397
	private static readonly ProfilerMarker ZoneRetirementPrepareMarker = new ProfilerMarker("ChunkManager.ZoneRetirement.Prepare");

	// Token: 0x0400018E RID: 398
	private static readonly ProfilerMarker ZoneRetirementProcessMarker = new ProfilerMarker("ChunkManager.ZoneRetirement.Process");

	// Token: 0x0400018F RID: 399
	private static readonly ProfilerMarker ZoneRetirementFinalizeMarker = new ProfilerMarker("ChunkManager.ZoneRetirement.Finalize");

	// Token: 0x04000190 RID: 400
	private static readonly Vector3 ZoneRetirementOffscreenPosition = new Vector3(0f, -5000f, 0f);

	// Token: 0x04000191 RID: 401
	private static readonly ProfilerMarker ChunkLifecycleMarker = new ProfilerMarker("ChunkManager.ProcessChunkLifecycleQueue");

	// Token: 0x04000192 RID: 402
	private static readonly ProfilerMarker UpdateMarker = new ProfilerMarker("ChunkManager.Update");

	// Token: 0x04000193 RID: 403
	private static readonly ProfilerMarker CacheChunkVisualComponentsMarker = new ProfilerMarker("ChunkManager.CacheChunkVisualComponents");

	// Token: 0x04000194 RID: 404
	private static readonly ProfilerMarker EnsureNearCollidersCacheRebuildMarker = new ProfilerMarker("ChunkManager.EnsureNearColliders.CacheRebuild");

	// Token: 0x04000195 RID: 405
	private static readonly ProfilerMarker MergeChunkColliderSubtreeMarker = new ProfilerMarker("ChunkManager.MergeChunkColliderSubtree");

	// Token: 0x04000196 RID: 406
	private static readonly ProfilerMarker ApplyVisualOnlyStateMarker = new ProfilerMarker("ChunkManager.ApplyVisualOnlyState");

	// Token: 0x04000197 RID: 407
	private static readonly ProfilerMarker ZonesReadyFanoutMarker = new ProfilerMarker("ChunkManager.ZonesReadyFanout");

	// Token: 0x04000199 RID: 409
	[Header("Configuration (Required)")]
	[Tooltip("ScriptableObject con TUTTI i parametri configurabili (performance, qualità, pooling).")]
	[SerializeField]
	private ChunkManagerConfig config;

	// Token: 0x0400019A RID: 410
	[Header("Grid Setup (Required)")]
	[Tooltip("Dimensione cella griglia. Se 0, usa il valore dal GameSystemsBootstrapper.")]
	[SerializeField]
	private float cellSize;

	// Token: 0x0400019B RID: 411
	[Header("Chunk Data (Required)")]
	[Tooltip("Lasciare VUOTO sul prefab ---CORE: il catalogo viene caricato da Resources/MapCatalog solo quando la mappa procedurale parte davvero (vedi MapCatalogSO). Se popolato a mano qui, ha la precedenza e nessun caricamento viene fatto.")]
	[SerializeField]
	private ChunkData[] availableChunks;

	// Token: 0x0400019C RID: 412
	[Header("Biomes (Optional)")]
	[Tooltip("Se assegnato, le zone vengono scelte per quartieri coerenti (Residential/Suburban/City) tramite BiomeField. Se vuoto, selezione classica dall'intera lista del chunk.")]
	[SerializeField]
	private BiomeManager biomeManager;

	// Token: 0x0400019D RID: 413
	private const string MAP_CATALOG_RESOURCE_NAME = "MapCatalog";

	// Token: 0x0400019E RID: 414
	public const string PERFORMANCE_MAP_CATALOG_RESOURCE_NAME = "MapCatalog_Performance";

	// Token: 0x040001A0 RID: 416
	private ResourceRequest _catalogRequest;

	// Token: 0x040001A1 RID: 417
	private bool _catalogLoadFailed;

	// Token: 0x040001A2 RID: 418
	[Header("Static Container (Optional)")]
	[Tooltip("Se assegnato, registra i figli come chunk pre-spawnati. Altrimenti usa GameSystemsBootstrapper.")]
	[SerializeField]
	private Transform staticChunksContainer;

	// Token: 0x040001A3 RID: 419
	[Header("Zone Spawning")]
	[SerializeField]
	private bool enableZoneSpawning = true;

	// Token: 0x040001A4 RID: 420
	[Header("Warm Render")]
	[Tooltip("Quanti prefab attivare e renderizzare per frame nel warm render iniziale (il gate resta warmRenderPoolOnStart). 1 = comportamento storico, un prefab per frame. Valori piu' alti accorciano il loading senza ridurre la copertura: ogni prefab viene comunque attivato e renderizzato almeno una volta.")]
	[SerializeField]
	private int warmRenderBatchSize = 6;

	// Token: 0x040001A5 RID: 421
	[Header("Debug")]
	[SerializeField]
	private bool enableDebugLogs = true;

	// Token: 0x040001A6 RID: 422
	[Tooltip("Diagnostica ad alta frequenza per profiling/collaudo chunk. Lasciare OFF nel gameplay normale.")]
	[SerializeField]
	private bool enableVerboseDiagnostics;

	// Token: 0x040001A7 RID: 423
	[SerializeField]
	private bool drawGizmos = true;

	// Token: 0x040001C1 RID: 449
	private Dictionary<Vector2Int, ChunkInstance> _spawnedChunks = new Dictionary<Vector2Int, ChunkInstance>();

	// Token: 0x040001C2 RID: 450
	private readonly Dictionary<Vector2Int, ChunkSpawnResolver.SpawnEntry> _committedSpawnPlan = new Dictionary<Vector2Int, ChunkSpawnResolver.SpawnEntry>(4096);

	// Token: 0x040001C3 RID: 451
	private readonly Dictionary<Vector2Int, Vector2Int> _committedSpawnCellOwners = new Dictionary<Vector2Int, Vector2Int>(8192);

	// Token: 0x040001C4 RID: 452
	private readonly Dictionary<Vector2Int, Vector2Int> _committedSpecialExclusionOwners = new Dictionary<Vector2Int, Vector2Int>(4096);

	// Token: 0x040001C5 RID: 453
	private readonly HashSet<Vector2Int> _encounteredPoiOrigins = new HashSet<Vector2Int>();

	// Token: 0x040001C6 RID: 454
	private readonly HashSet<Vector2Int> _poiOriginsReachedNearbyCore = new HashSet<Vector2Int>();

	// Token: 0x040001C7 RID: 455
	private readonly HashSet<Vector2Int> _travelOffsetPinnedSpecialOrigins = new HashSet<Vector2Int>();

	// Token: 0x040001C8 RID: 456
	private bool _hasPresentedPoiOrigin;

	// Token: 0x040001C9 RID: 457
	private Vector2Int _presentedPoiOrigin;

	// Token: 0x040001CA RID: 458
	private bool _hasCountedPresentedPoiEncounter;

	// Token: 0x040001CB RID: 459
	private readonly PoiTravelDroughtScheduler _poiTravelDroughtScheduler = new PoiTravelDroughtScheduler();

	// Token: 0x040001CC RID: 460
	private const int CommittedSpecialSpacingRadius = 2;

	// Token: 0x040001CD RID: 461
	private const int PoiNearbyCoreRadiusCells = 1;

	// Token: 0x040001CE RID: 462
	private int _spawnedChunkMembershipVersion;

	// Token: 0x040001CF RID: 463
	private int _chunksListMembershipVersion = -1;

	// Token: 0x040001D0 RID: 464
	private int _collToggleMembershipVersion = -1;

	// Token: 0x040001D1 RID: 465
	private HashSet<Vector2Int> _processedCells = new HashSet<Vector2Int>();

	// Token: 0x040001D2 RID: 466
	private Dictionary<Vector2Int, Vector2Int> _cellToChunkMap = new Dictionary<Vector2Int, Vector2Int>();

	// Token: 0x040001D3 RID: 467
	private HashSet<Vector2Int> _visuallyHiddenChunks = new HashSet<Vector2Int>();

	// Token: 0x040001D4 RID: 468
	private Dictionary<Vector2Int, ChunkManager.ChunkVisualCache> _chunkVisualCaches = new Dictionary<Vector2Int, ChunkManager.ChunkVisualCache>();

	// Token: 0x040001D5 RID: 469
	private readonly Dictionary<Vector2Int, bool> _chunkLightingAvailability = new Dictionary<Vector2Int, bool>();

	// Token: 0x040001D6 RID: 470
	private int _lightingMembershipVersion = -1;

	// Token: 0x040001D7 RID: 471
	private Vector2Int _lightingPlayerCell;

	// Token: 0x040001D8 RID: 472
	private int _lightingSpawnRadius = -1;

	// Token: 0x040001D9 RID: 473
	private int _lightingDespawnRadius = -1;

	// Token: 0x040001DA RID: 474
	private Dictionary<GameObject, List<Transform>> _chunkBaseTransformsCache = new Dictionary<GameObject, List<Transform>>();

	// Token: 0x040001DB RID: 475
	private readonly Dictionary<GameObject, Collider[]> _chunkBaseColliderCache = new Dictionary<GameObject, Collider[]>(96);

	// Token: 0x040001DC RID: 476
	private Dictionary<GameObject, Queue<GameObject>> _chunkPoolByPrefab = new Dictionary<GameObject, Queue<GameObject>>();

	// Token: 0x040001DD RID: 477
	private Dictionary<GameObject, int> _poolCountByPrefab = new Dictionary<GameObject, int>();

	// Token: 0x040001DE RID: 478
	private Dictionary<GameObject, GameObject> _instanceToPrefabMap = new Dictionary<GameObject, GameObject>();

	// Token: 0x040001DF RID: 479
	private Dictionary<GameObject, Queue<GameObject>> _zonePoolByPrefab = new Dictionary<GameObject, Queue<GameObject>>();

	// Token: 0x040001E0 RID: 480
	private Dictionary<GameObject, int> _zonePoolCountByPrefab = new Dictionary<GameObject, int>();

	// Token: 0x040001E1 RID: 481
	private Dictionary<GameObject, GameObject> _zoneInstanceToPrefabMap = new Dictionary<GameObject, GameObject>();

	// Token: 0x040001E2 RID: 482
	private PoiResidencyPrefabResolver _poiPrefabResolver;

	// Token: 0x040001E3 RID: 483
	private readonly Queue<ChunkManager.PendingZoneRetirement> _pendingZoneRetirements = new Queue<ChunkManager.PendingZoneRetirement>(16);

	// Token: 0x040001E4 RID: 484
	private readonly HashSet<GameObject> _retiringZoneRoots = new HashSet<GameObject>();

	// Token: 0x040001E5 RID: 485
	private readonly Dictionary<GameObject, int> _retiringZoneCountByPrefab = new Dictionary<GameObject, int>();

	// Token: 0x040001E6 RID: 486
	private int _pendingZoneRetirementWorkGroups;

	// Token: 0x040001E7 RID: 487
	private int _reusableRetirementPriorityTokens = 3;

	// Token: 0x040001E8 RID: 488
	private int _zonePoolRevision;

	// Token: 0x040001E9 RID: 489
	private Queue<Vector2Int> _pendingChunkDespawns = new Queue<Vector2Int>();

	// Token: 0x040001EA RID: 490
	private HashSet<Vector2Int> _pendingChunkDespawnSet = new HashSet<Vector2Int>();

	// Token: 0x040001EB RID: 491
	private bool _preferChunkSpawnWhenContended = true;

	// Token: 0x040001EC RID: 492
	private bool _hasLastStreamingPlayerCell;

	// Token: 0x040001ED RID: 493
	private Vector2Int _lastStreamingPlayerCell;

	// Token: 0x040001EE RID: 494
	private Vector2Int _streamingTravelDirection;

	// Token: 0x040001EF RID: 495
	private bool _hasHeavyPoiMotionSample;

	// Token: 0x040001F0 RID: 496
	private Vector3 _lastHeavyPoiMotionPosition;

	// Token: 0x040001F1 RID: 497
	private float _lastHeavyPoiMotionTime;

	// Token: 0x040001F2 RID: 498
	private float _heavyPoiPrefetchEstimatedSpeed;

	// Token: 0x040001F3 RID: 499
	private Vector2 _heavyPoiPrefetchMotionDirection;

	// Token: 0x040001F4 RID: 500
	private int _heavyPoiPrefetchLeadCells;

	// Token: 0x040001F5 RID: 501
	private bool _hasHeavyPoiPrefetchSignature;

	// Token: 0x040001F6 RID: 502
	private Vector2Int _lastHeavyPoiPrefetchCell;

	// Token: 0x040001F7 RID: 503
	private Vector2Int _lastHeavyPoiPrefetchDirection;

	// Token: 0x040001F8 RID: 504
	private int _lastHeavyPoiPrefetchLeadCells = -1;

	// Token: 0x040001F9 RID: 505
	private float _nextHeavyPoiPrefetchSignatureSweepTime;

	// Token: 0x040001FA RID: 506
	private readonly HashSet<Vector2Int> _heavyPoiPrefetchCellScratch = new HashSet<Vector2Int>();

	// Token: 0x040001FB RID: 507
	private readonly HashSet<string> _heavyPoiPrefetchPathScratch = new HashSet<string>();

	// Token: 0x040001FC RID: 508
	private Queue<Vector2Int> _pendingPostSpawnInit = new Queue<Vector2Int>();

	// Token: 0x040001FD RID: 509
	private HashSet<Vector2Int> _pendingPostSpawnInitSet = new HashSet<Vector2Int>();

	// Token: 0x040001FE RID: 510
	private readonly Queue<ChunkManager.PendingZoneSpawn> _pendingZoneSpawns = new Queue<ChunkManager.PendingZoneSpawn>(64);

	// Token: 0x040001FF RID: 511
	private readonly Dictionary<ChunkInstance, int> _pendingZoneCountByChunk = new Dictionary<ChunkInstance, int>(32);

	// Token: 0x04000200 RID: 512
	private readonly Dictionary<ChunkInstance, int> _pendingHeavyZoneCountByChunk = new Dictionary<ChunkInstance, int>(8);

	// Token: 0x04000201 RID: 513
	private readonly Dictionary<HeavyZoneStreamingProxy, ChunkInstance> _heavyZoneOwners = new Dictionary<HeavyZoneStreamingProxy, ChunkInstance>(8);

	// Token: 0x04000202 RID: 514
	private readonly Dictionary<IncrementalPooledZoneActivation, ChunkInstance> _incrementalNormalZoneOwners = new Dictionary<IncrementalPooledZoneActivation, ChunkInstance>(16);

	// Token: 0x04000203 RID: 515
	private readonly Queue<IncrementalPooledZoneActivation> _pendingNormalZoneWakes = new Queue<IncrementalPooledZoneActivation>(16);

	// Token: 0x04000204 RID: 516
	private readonly HashSet<IncrementalPooledZoneActivation> _pendingNormalZoneWakeSet = new HashSet<IncrementalPooledZoneActivation>();

	// Token: 0x04000205 RID: 517
	private readonly Dictionary<Transform, ChunkManager.ZonePlaceholderVisualState> _zonePlaceholderStates = new Dictionary<Transform, ChunkManager.ZonePlaceholderVisualState>(64);

	// Token: 0x04000206 RID: 518
	private readonly Dictionary<HeavyZoneStreamingProxy, ChunkManager.ZonePlaceholderVisualState> _heavyZonePlaceholders = new Dictionary<HeavyZoneStreamingProxy, ChunkManager.ZonePlaceholderVisualState>(8);

	// Token: 0x04000207 RID: 519
	private readonly Dictionary<IncrementalPooledZoneActivation, ChunkManager.ZonePlaceholderVisualState> _incrementalZonePlaceholders = new Dictionary<IncrementalPooledZoneActivation, ChunkManager.ZonePlaceholderVisualState>(16);

	// Token: 0x04000208 RID: 520
	private readonly Dictionary<ChunkInstance, List<HeavyZoneStreamingProxy>> _stagedHeavyColliderOwners = new Dictionary<ChunkInstance, List<HeavyZoneStreamingProxy>>(4);

	// Token: 0x04000209 RID: 521
	private readonly HashSet<Vector2Int> _specialZoneReservationCells = new HashSet<Vector2Int>();

	// Token: 0x0400020A RID: 522
	private int _retiringSpecialZoneReservationCount;

	// Token: 0x0400020B RID: 523
	private readonly HashSet<ChunkInstance> _zonesReadyNotifiedChunks = new HashSet<ChunkInstance>();

	// Token: 0x0400020C RID: 524
	private readonly LinkedList<ChunkManager.PendingChunkZonesReadyNotification> _pendingChunkZonesReadyNotifications = new LinkedList<ChunkManager.PendingChunkZonesReadyNotification>();

	// Token: 0x0400020D RID: 525
	private readonly Dictionary<ChunkInstance, LinkedListNode<ChunkManager.PendingChunkZonesReadyNotification>> _pendingChunkZonesReadyByChunk = new Dictionary<ChunkInstance, LinkedListNode<ChunkManager.PendingChunkZonesReadyNotification>>(32);

	// Token: 0x0400020E RID: 526
	private int _lastChunkZonesReadyFanoutFrame = -1;

	// Token: 0x0400020F RID: 527
	private const string RuntimeWorldRootName = "__PROCEDURAL_RUNTIME_WORLD";

	// Token: 0x04000210 RID: 528
	private Transform _runtimeWorldRoot;

	// Token: 0x04000211 RID: 529
	private Transform _chunkPoolContainer;

	// Token: 0x04000212 RID: 530
	private Transform _zonePoolContainer;

	// Token: 0x04000213 RID: 531
	private Transform _zonePoolCreationGate;

	// Token: 0x04000214 RID: 532
	private Transform _poiResidencyCreationGate;

	// Token: 0x04000215 RID: 533
	private static int _nextPoiResidencySessionGeneration;

	// Token: 0x04000216 RID: 534
	private int _poiResidencySessionGeneration;

	// Token: 0x04000217 RID: 535
	private string _poiResidencyRunId;

	// Token: 0x04000218 RID: 536
	private Coroutine _prewarmPoolCoroutine;

	// Token: 0x04000219 RID: 537
	private bool _retiringCompletedRun;

	// Token: 0x0400021A RID: 538
	private bool _completedRunRetired;

	// Token: 0x0400021B RID: 539
	private bool _mapInitialized;

	// Token: 0x0400021C RID: 540
	private bool _fullGameplayReady;

	// Token: 0x0400021D RID: 541
	private float _poolPrewarmProgress = 1f;

	// Token: 0x0400021E RID: 542
	private float _warmRenderProgress = 1f;

	// Token: 0x0400021F RID: 543
	private int _initializationWorkVersion;

	// Token: 0x04000220 RID: 544
	private string _initializationPhase = "not-started";

	// Token: 0x04000221 RID: 545
	private readonly List<GameObject> _registeredWarmRenderPrefabs = new List<GameObject>();

	// Token: 0x04000223 RID: 547
	private NativeArray<ChunkJobData> _chunkJobData;

	// Token: 0x04000224 RID: 548
	private NativeArray<ChunkVisibilityResult> _visibilityResults;

	// Token: 0x04000225 RID: 549
	private NativeHashSet<int2> _occupiedCellsNative;

	// Token: 0x04000226 RID: 550
	private NativeList<int> _chunksToActivate;

	// Token: 0x04000227 RID: 551
	private NativeList<int> _chunksToDeactivate;

	// Token: 0x04000228 RID: 552
	private List<ChunkInstance> _chunksList = new List<ChunkInstance>();

	// Token: 0x04000229 RID: 553
	private Dictionary<int, ChunkInstance> _chunkIdMap = new Dictionary<int, ChunkInstance>();

	// Token: 0x0400022A RID: 554
	private int _nextChunkId;

	// Token: 0x0400022B RID: 555
	private const string PREWARM_CACHE_DIRECTORY = "cache/map";

	// Token: 0x0400022C RID: 556
	private const string PREWARM_CACHE_FILE_NAME = "prewarm_targets_v1.json";

	// Token: 0x0400022D RID: 557
	private float _lastVisibilityCheck;

	// Token: 0x0400022E RID: 558
	private float _lastSpawnCheck;

	// Token: 0x0400022F RID: 559
	private float _lastColliderToggleCheck;

	// Token: 0x04000230 RID: 560
	private const float SPAWN_CHECK_INTERVAL = 0.1f;

	// Token: 0x04000231 RID: 561
	private JobHandle _currentVisibilityJob;

	// Token: 0x04000232 RID: 562
	private bool _visibilityJobScheduled;

	// Token: 0x04000233 RID: 563
	private List<Vector2Int> _freeCellsCache = new List<Vector2Int>(128);

	// Token: 0x04000234 RID: 564
	[TupleElementNames(new string[] { "chunk", "weight" })]
	private List<ValueTuple<ChunkData, float>> _validChunksCache = new List<ValueTuple<ChunkData, float>>(16);

	// Token: 0x04000235 RID: 565
	private readonly List<Collider> _colliderSubtreeScratch = new List<Collider>(256);

	// Token: 0x04000236 RID: 566
	private readonly List<Transform> _normalZoneTransformScratch = new List<Transform>(512);

	// Token: 0x04000237 RID: 567
	private readonly List<MeshCollider> _meshColliderPrewarmScratch = new List<MeshCollider>(256);

	// Token: 0x04000238 RID: 568
	private readonly Dictionary<int, Mesh> _meshColliderBakeMeshes = new Dictionary<int, Mesh>();

	// Token: 0x04000239 RID: 569
	private readonly HashSet<ChunkManager.WarmRenderResources> _warmRenderResources = new HashSet<ChunkManager.WarmRenderResources>();

	// Token: 0x04000241 RID: 577
	private double _meshColliderBakeWaveStartedAt;

	// Token: 0x04000242 RID: 578
	private readonly HashSet<ChunkManager.MeshColliderBakeKey> _meshColliderBakeKeys = new HashSet<ChunkManager.MeshColliderBakeKey>();

	// Token: 0x04000243 RID: 579
	private readonly List<ChunkManager.MeshColliderBakeRequest> _meshColliderBakeRequests = new List<ChunkManager.MeshColliderBakeRequest>(256);

	// Token: 0x04000244 RID: 580
	private readonly List<ChunkManager.MeshColliderBakeRequest> _meshColliderBakePending = new List<ChunkManager.MeshColliderBakeRequest>(256);

	// Token: 0x04000245 RID: 581
	private readonly List<ChunkManager.MeshColliderBakeRequest> _meshColliderBakeWave = new List<ChunkManager.MeshColliderBakeRequest>(256);

	// Token: 0x04000246 RID: 582
	private readonly HashSet<int> _meshColliderBakeWaveMeshIds = new HashSet<int>();

	// Token: 0x04000247 RID: 583
	private const int MESH_COLLIDER_BAKE_BATCH_SIZE = 64;

	// Token: 0x04000248 RID: 584
	private NativeArray<ChunkManager.MeshColliderBakeRequest> _meshColliderBakeNativeRequests;

	// Token: 0x04000249 RID: 585
	private JobHandle _meshColliderBakeJobHandle;

	// Token: 0x0400024A RID: 586
	private bool _meshColliderBakeJobScheduled;

	// Token: 0x0400024B RID: 587
	private bool _preferNormalZoneWakeWhenContended = true;

	// Token: 0x0400024C RID: 588
	private int _urgentZoneRetirementContentionTurns;

	// Token: 0x0400024D RID: 589
	private bool _preferDeferredZoneSpawnWhenContended;

	// Token: 0x0400024E RID: 590
	private DeferredStreamingFrameBudget _deferredStreamingBudget = new DeferredStreamingFrameBudget();

	// Token: 0x0400024F RID: 591
	private Dictionary<Vector2Int, int> _chunkDeactivationCounters = new Dictionary<Vector2Int, int>();

	// Token: 0x04000250 RID: 592
	private Queue<Vector2Int> _pendingActivations = new Queue<Vector2Int>();

	// Token: 0x04000251 RID: 593
	private Queue<Vector2Int> _pendingDeactivations = new Queue<Vector2Int>();

	// Token: 0x04000252 RID: 594
	private readonly HashSet<Vector2Int> _pendingActivationSet = new HashSet<Vector2Int>();

	// Token: 0x04000253 RID: 595
	private readonly HashSet<Vector2Int> _pendingDeactivationSet = new HashSet<Vector2Int>();

	// Token: 0x04000254 RID: 596
	private float _totalSpawnWeight;

	// Token: 0x04000255 RID: 597
	private List<ChunkData> _smallChunks = new List<ChunkData>();

	// Token: 0x04000256 RID: 598
	private List<ChunkData> _largeChunks = new List<ChunkData>();

	// Token: 0x04000257 RID: 599
	private float _smallChunksWeight;

	// Token: 0x04000258 RID: 600
	private float _largeChunksWeight;

	// Token: 0x04000259 RID: 601
	private ChunkSpawnResolver _spawnResolver;

	// Token: 0x04000261 RID: 609
	private bool _initialGenerationComplete;

	// Token: 0x04000263 RID: 611
	private Action _onMapReady;

	// Token: 0x04000265 RID: 613
	private Object _longWayHomeStreetOwner;

	// Token: 0x04000266 RID: 614
	private readonly HashSet<Vector2Int> _unvisitedLongWayHomeStreetOrigins = new HashSet<Vector2Int>();

	// Token: 0x04000267 RID: 615
	private Object _preparationFocusOwner;

	// Token: 0x04000268 RID: 616
	private Vector3 _preparationFocusPosition;

	// Token: 0x04000269 RID: 617
	private bool _preparationFocusSelectionPending;

	// Token: 0x0400026A RID: 618
	private Object _debugPoiReservationOwner;

	// Token: 0x0400026B RID: 619
	private Vector2Int _debugPoiReservationCell;

	// Token: 0x0400026C RID: 620
	private bool _debugPoiReservationMaterialized;

	// Token: 0x0400026E RID: 622
	private double _selfUpdateAccumMs;

	// Token: 0x0400026F RID: 623
	private int _selfUpdateAccumCount;

	// Token: 0x04000270 RID: 624
	private float _selfUpdateLogTimer;

	// Token: 0x04000271 RID: 625
	private readonly List<Vector2Int> _collToggleKeys = new List<Vector2Int>(128);

	// Token: 0x04000272 RID: 626
	private int _collToggleCursor;

	// Token: 0x04000273 RID: 627
	private const int COLL_TOGGLE_MAX_CHUNKS_PER_TICK = 6;

	// Token: 0x04000274 RID: 628
	private const int COLL_TOGGLE_MAX_COLLIDERS_PER_TICK = 1500;

	// Token: 0x04000275 RID: 629
	private const double COLL_TOGGLE_BUDGET_MS = 1.0;

	// Token: 0x04000276 RID: 630
	[TupleElementNames(new string[] { "cell", "direction" })]
	private readonly Dictionary<ValueTuple<Vector2Int, Vector2Int>, ChunkManager.SubwayConnection> _subwayConnections = new Dictionary<ValueTuple<Vector2Int, Vector2Int>, ChunkManager.SubwayConnection>();

	// Token: 0x0200003B RID: 59
	private readonly struct MeshColliderBakeKey : IEquatable<ChunkManager.MeshColliderBakeKey>
	{
		// Token: 0x060003F2 RID: 1010 RVA: 0x0001F5A1 File Offset: 0x0001D7A1
		public MeshColliderBakeKey(int meshId, bool convex, MeshColliderCookingOptions cookingOptions)
		{
			this.meshId = meshId;
			this.convex = (convex ? 1 : 0);
			this.cookingOptions = cookingOptions;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0001F5BE File Offset: 0x0001D7BE
		public bool Equals(ChunkManager.MeshColliderBakeKey other)
		{
			return this.meshId == other.meshId && this.convex == other.convex && this.cookingOptions == other.cookingOptions;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0001F5EC File Offset: 0x0001D7EC
		public override bool Equals(object obj)
		{
			if (obj is ChunkManager.MeshColliderBakeKey)
			{
				ChunkManager.MeshColliderBakeKey meshColliderBakeKey = (ChunkManager.MeshColliderBakeKey)obj;
				return this.Equals(meshColliderBakeKey);
			}
			return false;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0001F611 File Offset: 0x0001D811
		public override int GetHashCode()
		{
			return (((this.meshId * 397) ^ this.convex) * 397) ^ (int)this.cookingOptions;
		}

		// Token: 0x04000277 RID: 631
		public readonly int meshId;

		// Token: 0x04000278 RID: 632
		public readonly int convex;

		// Token: 0x04000279 RID: 633
		public readonly MeshColliderCookingOptions cookingOptions;
	}

	// Token: 0x0200003C RID: 60
	private struct MeshColliderBakeRequest
	{
		// Token: 0x0400027A RID: 634
		public int meshId;

		// Token: 0x0400027B RID: 635
		public int convex;

		// Token: 0x0400027C RID: 636
		public MeshColliderCookingOptions cookingOptions;
	}

	// Token: 0x0200003D RID: 61
	private struct BakeMeshCollidersJob : IJobParallelFor
	{
		// Token: 0x060003F6 RID: 1014 RVA: 0x0001F634 File Offset: 0x0001D834
		public void Execute(int index)
		{
			ChunkManager.MeshColliderBakeRequest meshColliderBakeRequest = this.requests[index];
			Physics.BakeMesh(meshColliderBakeRequest.meshId, meshColliderBakeRequest.convex != 0, meshColliderBakeRequest.cookingOptions);
		}

		// Token: 0x0400027D RID: 637
		[Unity.Collections.ReadOnly]
		public NativeArray<ChunkManager.MeshColliderBakeRequest> requests;
	}

	// Token: 0x0200003E RID: 62
	private sealed class WarmRenderResources : IDisposable
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0001F668 File Offset: 0x0001D868
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x0001F670 File Offset: 0x0001D870
		public Camera Camera { get; private set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x0001F679 File Offset: 0x0001D879
		// (set) Token: 0x060003FA RID: 1018 RVA: 0x0001F681 File Offset: 0x0001D881
		public bool IsDisposed { get; private set; }

		// Token: 0x060003FB RID: 1019 RVA: 0x0001F68C File Offset: 0x0001D88C
		public WarmRenderResources(ChunkManager owner, string name, Transform parent)
		{
			this._owner = owner;
			this._cameraObject = new GameObject(name);
			try
			{
				this._cameraObject.transform.SetParent(parent, false);
				this.Camera = this._cameraObject.AddComponent<Camera>();
				this._target = RenderTexture.GetTemporary(256, 256, 24);
				this.Camera.targetTexture = this._target;
				this.Camera.orthographic = true;
				this.Camera.clearFlags = CameraClearFlags.Color;
				this.Camera.backgroundColor = Color.black;
				this.Camera.depth = -100f;
			}
			catch
			{
				this.Dispose();
				throw;
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001F75C File Offset: 0x0001D95C
		public GameObject InstantiateOwned(GameObject prefab, Vector3 position)
		{
			if (this.IsDisposed)
			{
				return null;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(prefab, position, Quaternion.identity, this.Camera.transform.parent);
			this._instances.Add(gameObject);
			return gameObject;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0001F79D File Offset: 0x0001D99D
		public void ReleaseInstance(GameObject instance)
		{
			this._instances.Remove(instance);
			ChunkManager.WarmRenderResources.DestroyOwned(instance);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0001F7B4 File Offset: 0x0001D9B4
		public void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			this.IsDisposed = true;
			if (this.Camera != null)
			{
				this.Camera.enabled = false;
				this.Camera.targetTexture = null;
			}
			if (this._target != null)
			{
				RenderTexture.ReleaseTemporary(this._target);
				this._target = null;
			}
			foreach (GameObject gameObject in this._instances)
			{
				ChunkManager.WarmRenderResources.DestroyOwned(gameObject);
			}
			this._instances.Clear();
			ChunkManager.WarmRenderResources.DestroyOwned(this._cameraObject);
			this._cameraObject = null;
			this.Camera = null;
			this._owner._warmRenderResources.Remove(this);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0001F890 File Offset: 0x0001DA90
		private static void DestroyOwned(GameObject instance)
		{
			if (instance == null)
			{
				return;
			}
			instance.SetActive(false);
			if (Application.isPlaying)
			{
				Object.Destroy(instance);
				return;
			}
			Object.DestroyImmediate(instance);
		}

		// Token: 0x0400027E RID: 638
		private readonly ChunkManager _owner;

		// Token: 0x0400027F RID: 639
		private GameObject _cameraObject;

		// Token: 0x04000280 RID: 640
		private RenderTexture _target;

		// Token: 0x04000281 RID: 641
		private readonly List<GameObject> _instances = new List<GameObject>();
	}

	// Token: 0x0200003F RID: 63
	[Serializable]
	private sealed class PrewarmTargetEntry
	{
		// Token: 0x04000284 RID: 644
		public string key;

		// Token: 0x04000285 RID: 645
		public int count;
	}

	// Token: 0x02000040 RID: 64
	[Serializable]
	private sealed class MapPrewarmTargetCacheData
	{
		// Token: 0x04000286 RID: 646
		public int schemaVersion;

		// Token: 0x04000287 RID: 647
		public string buildVersion;

		// Token: 0x04000288 RID: 648
		public string inputHash;

		// Token: 0x04000289 RID: 649
		public List<ChunkManager.PrewarmTargetEntry> chunkTargets = new List<ChunkManager.PrewarmTargetEntry>();

		// Token: 0x0400028A RID: 650
		public List<ChunkManager.PrewarmTargetEntry> zoneTargets = new List<ChunkManager.PrewarmTargetEntry>();
	}

	// Token: 0x02000041 RID: 65
	private sealed class ZoneCandidateCoverageRequirement
	{
		// Token: 0x0400028B RID: 651
		public readonly List<GameObject> prefabs = new List<GameObject>();

		// Token: 0x0400028C RID: 652
		public int requiredInstances;
	}

	// Token: 0x02000042 RID: 66
	private sealed class ChunkVisualCache
	{
		// Token: 0x0400028D RID: 653
		public Renderer[] renderers;

		// Token: 0x0400028E RID: 654
		public bool[] rendererDefaults;

		// Token: 0x0400028F RID: 655
		public bool[] rendererUsesIndependentEnabledState;

		// Token: 0x04000290 RID: 656
		public bool[] rendererForceRenderingOffDefaults;

		// Token: 0x04000291 RID: 657
		public LODGroup[] lodGroups;

		// Token: 0x04000292 RID: 658
		public bool[] lodGroupDefaults;

		// Token: 0x04000293 RID: 659
		public Light[] lights;

		// Token: 0x04000294 RID: 660
		public bool[] lightDefaults;

		// Token: 0x04000295 RID: 661
		public StreetLightController[] managedStreetLights;

		// Token: 0x04000296 RID: 662
		public Terrain[] terrains;

		// Token: 0x04000297 RID: 663
		public bool[] terrainDefaults;
	}

	// Token: 0x02000043 RID: 67
	private sealed class PendingZoneRetirement
	{
		// Token: 0x04000298 RID: 664
		public GameObject zoneRoot;

		// Token: 0x04000299 RID: 665
		public GameObject prefab;

		// Token: 0x0400029A RID: 666
		public readonly List<Transform> hierarchy = new List<Transform>(512);

		// Token: 0x0400029B RID: 667
		public int discoveryIndex;

		// Token: 0x0400029C RID: 668
		public int hierarchyIndex = -1;

		// Token: 0x0400029D RID: 669
		public bool preparationComplete;

		// Token: 0x0400029E RID: 670
		public readonly List<GameObject> deactivatedObjects = new List<GameObject>(256);

		// Token: 0x0400029F RID: 671
		public int restoreObjectIndex = -1;

		// Token: 0x040002A0 RID: 672
		public bool rootDeactivated;

		// Token: 0x040002A1 RID: 673
		public bool destroyRootScheduled;

		// Token: 0x040002A2 RID: 674
		public bool destroyWhenComplete;

		// Token: 0x040002A3 RID: 675
		public bool holdsSpecialReservation;

		// Token: 0x040002A4 RID: 676
		public HeavyZoneStreamingProxy heavyProxy;

		// Token: 0x040002A5 RID: 677
		public bool heavyProxyAuthoredEnabled;

		// Token: 0x040002A6 RID: 678
		public bool usesStagedHeavyRetirement;

		// Token: 0x040002A7 RID: 679
		public IncrementalPooledZoneActivation stagedNormalProxy;

		// Token: 0x040002A8 RID: 680
		public bool usesStagedNormalRetirement;

		// Token: 0x040002A9 RID: 681
		public int remainingWorkGroups = 3;

		// Token: 0x040002AA RID: 682
		public double enqueuedAtRealtime;
	}

	// Token: 0x02000044 RID: 68
	private struct PendingZoneSpawn
	{
		// Token: 0x040002AB RID: 683
		public ChunkInstance chunk;

		// Token: 0x040002AC RID: 684
		public int slotIndex;

		// Token: 0x040002AD RID: 685
		public ChunkSpawnResolver.SpawnEntry entry;

		// Token: 0x040002AE RID: 686
		public int lastPoolRevisionTried;

		// Token: 0x040002AF RID: 687
		public bool hasDeferred;
	}

	// Token: 0x02000045 RID: 69
	private sealed class ZonePlaceholderVisualState
	{
		// Token: 0x040002B0 RID: 688
		public Renderer[] renderers;

		// Token: 0x040002B1 RID: 689
		public bool[] authoredEnabled;
	}

	// Token: 0x02000046 RID: 70
	private struct PendingChunkZonesReadyNotification
	{
		// Token: 0x040002B2 RID: 690
		public ChunkInstance chunk;

		// Token: 0x040002B3 RID: 691
		public Vector2Int originCell;

		// Token: 0x040002B4 RID: 692
		public GameObject chunkRoot;

		// Token: 0x040002B5 RID: 693
		public int enqueuedFrame;
	}

	// Token: 0x02000047 RID: 71
	// (Invoke) Token: 0x06000407 RID: 1031
	public delegate void ChunkEvent(ChunkInstance chunk);

	// Token: 0x02000048 RID: 72
	public readonly struct SubwayConnection
	{
		// Token: 0x0600040A RID: 1034 RVA: 0x0001F925 File Offset: 0x0001DB25
		public SubwayConnection(Vector2Int destination, Vector3 boardingLogical, Vector3 arrivalLogical, Vector3 arrivalDirection)
		{
			this.destination = destination;
			this.boardingLogical = boardingLogical;
			this.arrivalLogical = arrivalLogical;
			this.arrivalDirection = arrivalDirection;
		}

		// Token: 0x040002B6 RID: 694
		public readonly Vector2Int destination;

		// Token: 0x040002B7 RID: 695
		public readonly Vector3 boardingLogical;

		// Token: 0x040002B8 RID: 696
		public readonly Vector3 arrivalLogical;

		// Token: 0x040002B9 RID: 697
		public readonly Vector3 arrivalDirection;
	}
}
