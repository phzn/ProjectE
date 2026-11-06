using System;
using System.Collections.Generic;
using System.IO;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Singleton;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x0200000D RID: 13
	[CreateAssetMenu(menuName = "D.A. Assets/FCU Config")]
	public class FcuConfig : AssetConfig<FcuConfig>
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002A94 File Offset: 0x00000C94
		public static List<TagConfig> TagConfigs
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.tags;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002AA0 File Offset: 0x00000CA0
		public static string WebLogFileName
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.webLogFileName;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002AAC File Offset: 0x00000CAC
		public static string DateTimeFormat1
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.dateTimeFormat1;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002AB8 File Offset: 0x00000CB8
		public static string CanvasGameObjectName
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.canvasGameObjectName;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002AC4 File Offset: 0x00000CC4
		public static string I2LocGameObjectName
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.i2LocGameObjectName;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002AD0 File Offset: 0x00000CD0
		public static RoundingConfig Rounding
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.rounding;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002ADC File Offset: 0x00000CDC
		public static int RecentProjectsLimit
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.recentProjectsLimit;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002AE8 File Offset: 0x00000CE8
		public static int FigmaSessionsLimit
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.figmaSessionsLimit;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002AF4 File Offset: 0x00000CF4
		public static int LogFilesLimit
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.logFilesLimit;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002B00 File Offset: 0x00000D00
		public static int MaxRenderSize
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.maxRenderSize;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002B0C File Offset: 0x00000D0C
		public static int RenderUpscaleFactor
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.renderUpscaleFactor;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002B18 File Offset: 0x00000D18
		public static string BlurredObjectTag
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.blurredObjectTag;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002B24 File Offset: 0x00000D24
		public static string BlurCameraTag
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.blurCameraTag;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002B30 File Offset: 0x00000D30
		public static char RealTagSeparator
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.realTagSeparator;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002B3C File Offset: 0x00000D3C
		public static int ChildParsingLimit
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.childParsingLimit;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002B48 File Offset: 0x00000D48
		public static int ApiRequestsCountLimit
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.apiRequestsCountLimit;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002B54 File Offset: 0x00000D54
		public static int ApiTimeoutSec
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.apiTimeoutSec;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002B60 File Offset: 0x00000D60
		public static int ChunkSizeGetNodes
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.chunkSizeGetNodes;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002B6C File Offset: 0x00000D6C
		public static int FrameListDepth
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.frameListDepth;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002B78 File Offset: 0x00000D78
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00002B84 File Offset: 0x00000D84
		public static string GoogleFontsApiKey
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.gFontsApiKey;
			}
			set
			{
				SingletonScriptableObject<FcuConfig>.Instance.gFontsApiKey = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002B91 File Offset: 0x00000D91
		public static Sprite WhiteSprite32px
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.whiteSprite32px;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002B9D File Offset: 0x00000D9D
		public static Sprite MissingImageTexture128px
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.missingImageTexture128px;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002BA9 File Offset: 0x00000DA9
		public static TextAsset BaseClass
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.baseClass;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002BB5 File Offset: 0x00000DB5
		public static Material ImageLinearMaterial
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.imageLinearMaterial;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002BC1 File Offset: 0x00000DC1
		public static VectorMaterials VectorMaterials
		{
			get
			{
				return SingletonScriptableObject<FcuConfig>.Instance.vectorMaterials;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002BCD File Offset: 0x00000DCD
		public static char HierarchyDelimiter { get; } = 47;

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002BD4 File Offset: 0x00000DD4
		public static string PARENT_ID { get; } = "603951929:602259738";

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002BDB File Offset: 0x00000DDB
		public static char AsterisksChar { get; } = 8226;

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002BE2 File Offset: 0x00000DE2
		public static string DefaultLocalizationCulture { get; } = "en-US";

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002BE9 File Offset: 0x00000DE9
		public static string RATEME_PREFS_KEY { get; } = "DONT_SHOW_RATEME";

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002BF0 File Offset: 0x00000DF0
		public static string RECENT_PROJECTS_PREFS_KEY { get; } = "recentProjectsPrefsKey";

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002BF7 File Offset: 0x00000DF7
		public static string FIGMA_SESSIONS_PREFS_KEY { get; } = "FigmaSessions";

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002BFE File Offset: 0x00000DFE
		public static string ClientId
		{
			get
			{
				return "LaB1ONuPoY7QCdfshDbQbT";
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002C05 File Offset: 0x00000E05
		public static string ClientSecret
		{
			get
			{
				return "E9PblceydtAyE7Onhg5FHLmnvingDp";
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00002C0C File Offset: 0x00000E0C
		public static string RedirectUri
		{
			get
			{
				return "http://localhost:1923/";
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002C13 File Offset: 0x00000E13
		public static string OAuthUrl
		{
			get
			{
				return "https://www.figma.com/oauth?client_id={0}&redirect_uri={1}&scope=file_read&state={2}&response_type=code";
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00002C1A File Offset: 0x00000E1A
		public static string LogPath
		{
			get
			{
				if (FcuConfig.logPath.IsEmpty())
				{
					FcuConfig.logPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Logs");
				}
				FcuConfig.logPath.CreateFolderIfNotExists();
				return FcuConfig.logPath;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002C55 File Offset: 0x00000E55
		public static string CachePath
		{
			get
			{
				if (FcuConfig.cachePath.IsEmpty())
				{
					FcuConfig.cachePath = Path.Combine(Path.GetTempPath(), "FcuCache");
				}
				FcuConfig.cachePath.CreateFolderIfNotExists();
				return FcuConfig.cachePath;
			}
		}

		// Token: 0x0400001F RID: 31
		[SerializeField]
		private List<TagConfig> tags;

		// Token: 0x04000020 RID: 32
		[Header("File names")]
		[SerializeField]
		private string webLogFileName;

		// Token: 0x04000021 RID: 33
		[Header("Formats")]
		[SerializeField]
		private string dateTimeFormat1;

		// Token: 0x04000022 RID: 34
		[Header("GameObject names")]
		[SerializeField]
		private string canvasGameObjectName;

		// Token: 0x04000023 RID: 35
		[SerializeField]
		private string i2LocGameObjectName;

		// Token: 0x04000024 RID: 36
		[Header("Values")]
		[SerializeField]
		private RoundingConfig rounding;

		// Token: 0x04000025 RID: 37
		[SerializeField]
		private int recentProjectsLimit = 20;

		// Token: 0x04000026 RID: 38
		[SerializeField]
		private int figmaSessionsLimit = 10;

		// Token: 0x04000027 RID: 39
		[SerializeField]
		private int logFilesLimit = 50;

		// Token: 0x04000028 RID: 40
		[SerializeField]
		private int maxRenderSize = 4096;

		// Token: 0x04000029 RID: 41
		[SerializeField]
		private int renderUpscaleFactor = 2;

		// Token: 0x0400002A RID: 42
		[SerializeField]
		private string blurredObjectTag = "UIBlur";

		// Token: 0x0400002B RID: 43
		[SerializeField]
		private string blurCameraTag = "BackgroundBlur";

		// Token: 0x0400002C RID: 44
		[SerializeField]
		private char realTagSeparator = '-';

		// Token: 0x0400002D RID: 45
		[Tooltip("If an object has more than **N** children, **SmartTags** and **Hashes** will not be assigned to them.")]
		[SerializeField]
		private int childParsingLimit = 512;

		// Token: 0x0400002E RID: 46
		[Header("Api")]
		[SerializeField]
		private int apiRequestsCountLimit = 2;

		// Token: 0x0400002F RID: 47
		[SerializeField]
		private int apiTimeoutSec = 5;

		// Token: 0x04000030 RID: 48
		[SerializeField]
		private int chunkSizeGetNodes;

		// Token: 0x04000031 RID: 49
		[SerializeField]
		private int frameListDepth = 2;

		// Token: 0x04000032 RID: 50
		[SerializeField]
		private string gFontsApiKey;

		// Token: 0x04000033 RID: 51
		[Header("Other")]
		[SerializeField]
		private Sprite whiteSprite32px;

		// Token: 0x04000034 RID: 52
		[SerializeField]
		private Sprite missingImageTexture128px;

		// Token: 0x04000035 RID: 53
		[SerializeField]
		private TextAsset baseClass;

		// Token: 0x04000036 RID: 54
		[SerializeField]
		private Material imageLinearMaterial;

		// Token: 0x04000037 RID: 55
		[SerializeField]
		private VectorMaterials vectorMaterials;

		// Token: 0x04000038 RID: 56
		public const string ProductName = "Figma Converter for Unity";

		// Token: 0x04000039 RID: 57
		public const string ProductNameShort = "FCU";

		// Token: 0x0400003A RID: 58
		public const string DestroyChilds = "Destroy childs";

		// Token: 0x0400003B RID: 59
		public const string SetFcuToSyncHelpers = "Set current FCU to SyncHelpers";

		// Token: 0x0400003C RID: 60
		public const string CompareTwoObjects = "Compare two selected objects";

		// Token: 0x0400003D RID: 61
		public const string DestroyLastImported = "Destroy last imported frames";

		// Token: 0x0400003E RID: 62
		public const string DestroySyncHelpers = "Destroy SyncHelpers";

		// Token: 0x0400003F RID: 63
		public const string CreatePrefabs = "Create Prefabs";

		// Token: 0x04000040 RID: 64
		public const string UpdatePrefabs = "Update Prefabs";

		// Token: 0x04000041 RID: 65
		public const string Create = "Create";

		// Token: 0x04000042 RID: 66
		public const string OptimizeSyncHelpers = "Optimize SyncHelpers";

		// Token: 0x04000043 RID: 67
		public const string GenerateScripts = "Generate scripts";

		// Token: 0x04000044 RID: 68
		public const float IMAGE_SCALE_MIN = 0.25f;

		// Token: 0x04000045 RID: 69
		public const float IMAGE_SCALE_MAX = 4f;

		// Token: 0x0400004D RID: 77
		private static string logPath;

		// Token: 0x0400004E RID: 78
		private static string cachePath;
	}
}
