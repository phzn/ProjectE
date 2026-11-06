using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DA_Assets.Extensions;
using DA_Assets.Tools;
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200007E RID: 126
	[Serializable]
	public class SyncData : IHaveId
	{
		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000338 RID: 824 RVA: 0x000079D6 File Offset: 0x00005BD6
		// (set) Token: 0x06000339 RID: 825 RVA: 0x000079DE File Offset: 0x00005BDE
		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600033A RID: 826 RVA: 0x000079E7 File Offset: 0x00005BE7
		// (set) Token: 0x0600033B RID: 827 RVA: 0x000079EF File Offset: 0x00005BEF
		public string ProjectId
		{
			get
			{
				return this.projectId;
			}
			set
			{
				this.projectId = value;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600033C RID: 828 RVA: 0x000079F8 File Offset: 0x00005BF8
		// (set) Token: 0x0600033D RID: 829 RVA: 0x00007A00 File Offset: 0x00005C00
		public FNames Names
		{
			get
			{
				return this.names;
			}
			set
			{
				this.names = value;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00007A09 File Offset: 0x00005C09
		// (set) Token: 0x0600033F RID: 831 RVA: 0x00007A11 File Offset: 0x00005C11
		public List<FcuTag> Tags
		{
			get
			{
				return this.tags;
			}
			set
			{
				this.tags = value;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00007A1A File Offset: 0x00005C1A
		// (set) Token: 0x06000341 RID: 833 RVA: 0x00007A22 File Offset: 0x00005C22
		public FcuImageType FcuImageType
		{
			get
			{
				return this.fcuImageType;
			}
			set
			{
				this.fcuImageType = value;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000342 RID: 834 RVA: 0x00007A2B File Offset: 0x00005C2B
		// (set) Token: 0x06000343 RID: 835 RVA: 0x00007A33 File Offset: 0x00005C33
		public ButtonComponent ButtonComponent
		{
			get
			{
				return this.buttonComponent;
			}
			set
			{
				this.buttonComponent = value;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00007A3C File Offset: 0x00005C3C
		// (set) Token: 0x06000345 RID: 837 RVA: 0x00007A44 File Offset: 0x00005C44
		public List<int> ChildIndexes
		{
			get
			{
				return this.childIndexes;
			}
			set
			{
				this.childIndexes = value;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000346 RID: 838 RVA: 0x00007A4D File Offset: 0x00005C4D
		// (set) Token: 0x06000347 RID: 839 RVA: 0x00007A55 File Offset: 0x00005C55
		public MonoBehaviour FigmaConverterUnity
		{
			get
			{
				return this.fcu;
			}
			set
			{
				this.fcu = value;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00007A5E File Offset: 0x00005C5E
		// (set) Token: 0x06000349 RID: 841 RVA: 0x00007A66 File Offset: 0x00005C66
		public GameObject GameObject
		{
			get
			{
				return this.gameObject;
			}
			set
			{
				this.gameObject = value;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00007A6F File Offset: 0x00005C6F
		// (set) Token: 0x0600034B RID: 843 RVA: 0x00007A77 File Offset: 0x00005C77
		public int Hash
		{
			get
			{
				return this.hash;
			}
			set
			{
				this.hash = value;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600034C RID: 844 RVA: 0x00007A80 File Offset: 0x00005C80
		// (set) Token: 0x0600034D RID: 845 RVA: 0x00007A88 File Offset: 0x00005C88
		public List<FcuHierarchy> Hierarchy
		{
			get
			{
				return this.hierarchy;
			}
			set
			{
				this.hierarchy = value;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00007A94 File Offset: 0x00005C94
		public string NameHierarchy
		{
			get
			{
				if (this.hierarchy.IsEmpty<FcuHierarchy>())
				{
					return null;
				}
				return string.Join(FcuConfig.HierarchyDelimiter.ToString(), this.hierarchy.Select<FcuHierarchy, string>((FcuHierarchy x) => x.Name));
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600034F RID: 847 RVA: 0x00007AEC File Offset: 0x00005CEC
		// (set) Token: 0x06000350 RID: 848 RVA: 0x00007B38 File Offset: 0x00005D38
		public SyncData RootFrame
		{
			get
			{
				if (this.rootFrameGO == null)
				{
					return this.rootFrameSD;
				}
				SyncHelper component = this.rootFrameGO.GetComponent<SyncHelper>();
				if (component == null || component.Data == null)
				{
					return this.rootFrameSD;
				}
				return component.Data;
			}
			set
			{
				if (((value != null) ? value.GameObject : null) != null)
				{
					this.rootFrameGO = value.GameObject;
				}
				if (this.rootFrameGO == null)
				{
					this.rootFrameSD = value;
					return;
				}
				SyncHelper component = this.rootFrameGO.GetComponent<SyncHelper>();
				if (component != null)
				{
					component.Data = value;
					return;
				}
				this.rootFrameSD = value;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000351 RID: 849 RVA: 0x00007B9F File Offset: 0x00005D9F
		// (set) Token: 0x06000352 RID: 850 RVA: 0x00007BA7 File Offset: 0x00005DA7
		[Clear]
		public string UitkType { get; set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000353 RID: 851 RVA: 0x00007BB0 File Offset: 0x00005DB0
		// (set) Token: 0x06000354 RID: 852 RVA: 0x00007BB8 File Offset: 0x00005DB8
		[Clear]
		public string HashData { get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000355 RID: 853 RVA: 0x00007BC1 File Offset: 0x00005DC1
		// (set) Token: 0x06000356 RID: 854 RVA: 0x00007BC9 File Offset: 0x00005DC9
		[Clear]
		public XmlElement XmlElement { get; set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000357 RID: 855 RVA: 0x00007BD2 File Offset: 0x00005DD2
		// (set) Token: 0x06000358 RID: 856 RVA: 0x00007BDA File Offset: 0x00005DDA
		[Clear]
		public FObject Parent { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000359 RID: 857 RVA: 0x00007BE3 File Offset: 0x00005DE3
		// (set) Token: 0x0600035A RID: 858 RVA: 0x00007BEB File Offset: 0x00005DEB
		public string HashDataTree
		{
			get
			{
				return this.hashDataTree;
			}
			set
			{
				this.hashDataTree = value;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600035B RID: 859 RVA: 0x00007BF4 File Offset: 0x00005DF4
		// (set) Token: 0x0600035C RID: 860 RVA: 0x00007BFC File Offset: 0x00005DFC
		public int ParentIndex
		{
			get
			{
				return this.parentIndex;
			}
			set
			{
				this.parentIndex = value;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00007C05 File Offset: 0x00005E05
		// (set) Token: 0x0600035E RID: 862 RVA: 0x00007C0D File Offset: 0x00005E0D
		public UguiTransformData UguiTransformData
		{
			get
			{
				return this.uguiTransformData;
			}
			set
			{
				this.uguiTransformData = value;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600035F RID: 863 RVA: 0x00007C16 File Offset: 0x00005E16
		// (set) Token: 0x06000360 RID: 864 RVA: 0x00007C1E File Offset: 0x00005E1E
		public FGraphic Graphic
		{
			get
			{
				return this.graphic;
			}
			set
			{
				this.graphic = value;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00007C27 File Offset: 0x00005E27
		// (set) Token: 0x06000362 RID: 866 RVA: 0x00007C2F File Offset: 0x00005E2F
		public UIDocument UIDocument { get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00007C38 File Offset: 0x00005E38
		// (set) Token: 0x06000364 RID: 868 RVA: 0x00007C40 File Offset: 0x00005E40
		public GameObject RectGameObject
		{
			get
			{
				return this.rectGameObject;
			}
			set
			{
				this.rectGameObject = value;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000365 RID: 869 RVA: 0x00007C49 File Offset: 0x00005E49
		// (set) Token: 0x06000366 RID: 870 RVA: 0x00007C51 File Offset: 0x00005E51
		public Vector2Int SpriteSize
		{
			get
			{
				return this.spriteSize;
			}
			set
			{
				this.spriteSize = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000367 RID: 871 RVA: 0x00007C5A File Offset: 0x00005E5A
		// (set) Token: 0x06000368 RID: 872 RVA: 0x00007C62 File Offset: 0x00005E62
		public FRect FRect
		{
			get
			{
				return this.rect;
			}
			set
			{
				this.rect = value;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00007C6B File Offset: 0x00005E6B
		// (set) Token: 0x0600036A RID: 874 RVA: 0x00007C73 File Offset: 0x00005E73
		public string SpritePath
		{
			get
			{
				return this.spritePath;
			}
			set
			{
				this.spritePath = value;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00007C7C File Offset: 0x00005E7C
		// (set) Token: 0x0600036C RID: 876 RVA: 0x00007C84 File Offset: 0x00005E84
		public string Link
		{
			get
			{
				return this.link;
			}
			set
			{
				this.link = value;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00007C8D File Offset: 0x00005E8D
		// (set) Token: 0x0600036E RID: 878 RVA: 0x00007C95 File Offset: 0x00005E95
		public List<string> TagReasons
		{
			get
			{
				return this.tagReasons;
			}
			set
			{
				this.tagReasons = value;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00007C9E File Offset: 0x00005E9E
		// (set) Token: 0x06000370 RID: 880 RVA: 0x00007CA6 File Offset: 0x00005EA6
		public string DownloadableReason
		{
			get
			{
				return this.downloadableReason;
			}
			set
			{
				this.downloadableReason = value;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00007CAF File Offset: 0x00005EAF
		// (set) Token: 0x06000372 RID: 882 RVA: 0x00007CB7 File Offset: 0x00005EB7
		public string GenerativeReason
		{
			get
			{
				return this.generativeReason;
			}
			set
			{
				this.generativeReason = value;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00007CC0 File Offset: 0x00005EC0
		// (set) Token: 0x06000374 RID: 884 RVA: 0x00007CC8 File Offset: 0x00005EC8
		public int DownloadAttempsCount
		{
			get
			{
				return this.downloadAttempsCount;
			}
			set
			{
				this.downloadAttempsCount = value;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00007CD1 File Offset: 0x00005ED1
		// (set) Token: 0x06000376 RID: 886 RVA: 0x00007CD9 File Offset: 0x00005ED9
		public bool IsDuplicate
		{
			get
			{
				return this.isDuplicate;
			}
			set
			{
				this.isDuplicate = value;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000377 RID: 887 RVA: 0x00007CE2 File Offset: 0x00005EE2
		// (set) Token: 0x06000378 RID: 888 RVA: 0x00007CEA File Offset: 0x00005EEA
		public bool IsMutual
		{
			get
			{
				return this.isMutual;
			}
			set
			{
				this.isMutual = value;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000379 RID: 889 RVA: 0x00007CF3 File Offset: 0x00005EF3
		// (set) Token: 0x0600037A RID: 890 RVA: 0x00007CFB File Offset: 0x00005EFB
		public bool IsEmpty
		{
			get
			{
				return this.isEmpty;
			}
			set
			{
				this.isEmpty = value;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600037B RID: 891 RVA: 0x00007D04 File Offset: 0x00005F04
		// (set) Token: 0x0600037C RID: 892 RVA: 0x00007D0C File Offset: 0x00005F0C
		public bool NeedDownload
		{
			get
			{
				return this.needDownload;
			}
			set
			{
				this.needDownload = value;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00007D15 File Offset: 0x00005F15
		// (set) Token: 0x0600037E RID: 894 RVA: 0x00007D1D File Offset: 0x00005F1D
		public bool NeedGenerate
		{
			get
			{
				return this.needGenerate;
			}
			set
			{
				this.needGenerate = value;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00007D26 File Offset: 0x00005F26
		// (set) Token: 0x06000380 RID: 896 RVA: 0x00007D2E File Offset: 0x00005F2E
		public bool ForceImage
		{
			get
			{
				return this.forceImage;
			}
			set
			{
				this.forceImage = value;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00007D37 File Offset: 0x00005F37
		// (set) Token: 0x06000382 RID: 898 RVA: 0x00007D3F File Offset: 0x00005F3F
		public bool IsOverlappedByStroke
		{
			get
			{
				return this.isOverlappedByStroke;
			}
			set
			{
				this.isOverlappedByStroke = value;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000383 RID: 899 RVA: 0x00007D48 File Offset: 0x00005F48
		// (set) Token: 0x06000384 RID: 900 RVA: 0x00007D50 File Offset: 0x00005F50
		public bool ForceContainer
		{
			get
			{
				return this.forceContainer;
			}
			set
			{
				this.forceContainer = value;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00007D59 File Offset: 0x00005F59
		// (set) Token: 0x06000386 RID: 902 RVA: 0x00007D61 File Offset: 0x00005F61
		public bool InsideDownloadable
		{
			get
			{
				return this.isInsideDownloadable;
			}
			set
			{
				this.isInsideDownloadable = value;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000387 RID: 903 RVA: 0x00007D6A File Offset: 0x00005F6A
		// (set) Token: 0x06000388 RID: 904 RVA: 0x00007D72 File Offset: 0x00005F72
		public bool HasFontAsset
		{
			get
			{
				return this.hasFontAsset;
			}
			set
			{
				this.hasFontAsset = value;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00007D7B File Offset: 0x00005F7B
		// (set) Token: 0x0600038A RID: 906 RVA: 0x00007D83 File Offset: 0x00005F83
		public ImageFormat ImageFormat
		{
			get
			{
				return this.imageFormat;
			}
			set
			{
				this.imageFormat = value;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00007D8C File Offset: 0x00005F8C
		// (set) Token: 0x0600038C RID: 908 RVA: 0x00007D94 File Offset: 0x00005F94
		public float Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00007D9D File Offset: 0x00005F9D
		// (set) Token: 0x0600038E RID: 910 RVA: 0x00007DA5 File Offset: 0x00005FA5
		public Vector2 MaxSpriteSize
		{
			get
			{
				return this.maxSpriteSize;
			}
			set
			{
				this.maxSpriteSize = value;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00007DAE File Offset: 0x00005FAE
		// (set) Token: 0x06000390 RID: 912 RVA: 0x00007DB6 File Offset: 0x00005FB6
		public Transform ParentTransformRect
		{
			get
			{
				return this.parentTransformRect;
			}
			set
			{
				this.parentTransformRect = value;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00007DBF File Offset: 0x00005FBF
		// (set) Token: 0x06000392 RID: 914 RVA: 0x00007DC7 File Offset: 0x00005FC7
		public Transform ParentTransform
		{
			get
			{
				return this.parentTransform;
			}
			set
			{
				this.parentTransform = value;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00007DD0 File Offset: 0x00005FD0
		// (set) Token: 0x06000394 RID: 916 RVA: 0x00007DD8 File Offset: 0x00005FD8
		public int HierarchyLevel { get; set; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00007DE1 File Offset: 0x00005FE1
		// (set) Token: 0x06000396 RID: 918 RVA: 0x00007DE9 File Offset: 0x00005FE9
		public int SiblingIndex { get; set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00007DF2 File Offset: 0x00005FF2
		// (set) Token: 0x06000398 RID: 920 RVA: 0x00007DFA File Offset: 0x00005FFA
		public bool PartOfPrefab
		{
			get
			{
				return this.partOfPrefab;
			}
			set
			{
				this.partOfPrefab = value;
			}
		}

		// Token: 0x04000483 RID: 1155
		[SerializeField]
		private string id;

		// Token: 0x04000484 RID: 1156
		[SerializeField]
		private string projectId;

		// Token: 0x04000485 RID: 1157
		[SerializeField]
		private FNames names;

		// Token: 0x04000486 RID: 1158
		[Space]
		[SerializeField]
		private List<FcuTag> tags = new List<FcuTag>();

		// Token: 0x04000487 RID: 1159
		[SerializeField]
		private FcuImageType fcuImageType;

		// Token: 0x04000488 RID: 1160
		[SerializeField]
		private ButtonComponent buttonComponent;

		// Token: 0x04000489 RID: 1161
		[SerializeField]
		private List<int> childIndexes = new List<int>();

		// Token: 0x0400048A RID: 1162
		[Space]
		[SerializeField]
		private MonoBehaviour fcu;

		// Token: 0x0400048B RID: 1163
		[SerializeField]
		private GameObject gameObject;

		// Token: 0x0400048C RID: 1164
		[Space]
		[SerializeField]
		private int hash;

		// Token: 0x0400048D RID: 1165
		[SerializeField]
		[HideInInspector]
		private List<FcuHierarchy> hierarchy = new List<FcuHierarchy>();

		// Token: 0x0400048E RID: 1166
		[SerializeField]
		private GameObject rootFrameGO;

		// Token: 0x0400048F RID: 1167
		[SerializeField]
		private SyncData rootFrameSD;

		// Token: 0x04000494 RID: 1172
		[SerializeField]
		[Clear]
		private string hashDataTree;

		// Token: 0x04000495 RID: 1173
		[SerializeField]
		[Clear]
		private int parentIndex;

		// Token: 0x04000496 RID: 1174
		[SerializeField]
		[Clear]
		private UguiTransformData uguiTransformData;

		// Token: 0x04000497 RID: 1175
		[SerializeField]
		[Clear]
		private FGraphic graphic;

		// Token: 0x04000499 RID: 1177
		[SerializeField]
		[Clear]
		private GameObject rectGameObject;

		// Token: 0x0400049A RID: 1178
		[SerializeField]
		[Clear]
		private Vector2Int spriteSize;

		// Token: 0x0400049B RID: 1179
		[SerializeField]
		[Clear]
		private FRect rect;

		// Token: 0x0400049C RID: 1180
		[SerializeField]
		[Clear]
		private string spritePath;

		// Token: 0x0400049D RID: 1181
		[SerializeField]
		[Clear]
		private string link;

		// Token: 0x0400049E RID: 1182
		[SerializeField]
		private List<string> tagReasons = new List<string>();

		// Token: 0x0400049F RID: 1183
		[SerializeField]
		[Clear]
		private string downloadableReason;

		// Token: 0x040004A0 RID: 1184
		[SerializeField]
		[Clear]
		private string generativeReason;

		// Token: 0x040004A1 RID: 1185
		[SerializeField]
		[Clear]
		private int downloadAttempsCount;

		// Token: 0x040004A2 RID: 1186
		[SerializeField]
		private bool isDuplicate;

		// Token: 0x040004A3 RID: 1187
		[SerializeField]
		private bool isMutual;

		// Token: 0x040004A4 RID: 1188
		[SerializeField]
		private bool isEmpty;

		// Token: 0x040004A5 RID: 1189
		[SerializeField]
		private bool needDownload;

		// Token: 0x040004A6 RID: 1190
		[SerializeField]
		[Clear]
		private bool needGenerate;

		// Token: 0x040004A7 RID: 1191
		[SerializeField]
		private bool forceImage;

		// Token: 0x040004A8 RID: 1192
		[SerializeField]
		private bool isOverlappedByStroke;

		// Token: 0x040004A9 RID: 1193
		[SerializeField]
		private bool forceContainer;

		// Token: 0x040004AA RID: 1194
		[SerializeField]
		private bool isInsideDownloadable;

		// Token: 0x040004AB RID: 1195
		[SerializeField]
		[Clear]
		private bool hasFontAsset;

		// Token: 0x040004AC RID: 1196
		[SerializeField]
		[Clear]
		private ImageFormat imageFormat;

		// Token: 0x040004AD RID: 1197
		[SerializeField]
		[Clear]
		private float scale;

		// Token: 0x040004AE RID: 1198
		[SerializeField]
		[Clear]
		private Vector2 maxSpriteSize;

		// Token: 0x040004AF RID: 1199
		[SerializeField]
		[Clear]
		private Transform parentTransformRect;

		// Token: 0x040004B0 RID: 1200
		[SerializeField]
		[Clear]
		private Transform parentTransform;

		// Token: 0x040004B3 RID: 1203
		[SerializeField]
		private bool partOfPrefab;
	}
}
