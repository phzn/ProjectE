using System;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Drawers;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000301 RID: 769
	[DisallowMultipleComponent]
	[Serializable]
	public sealed class FigmaConverterUnity : MonoBehaviour
	{
		// Token: 0x06001521 RID: 5409 RVA: 0x00004F31 File Offset: 0x00003131
		private void Start()
		{
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x00089CA2 File Offset: 0x00087EA2
		public void Reset()
		{
			this.Settings.ButtonSettings.DAB_Settings.Reset();
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06001523 RID: 5411 RVA: 0x00089CB9 File Offset: 0x00087EB9
		[SerializeProperty("importController")]
		public ProjectImporter ProjectImporter
		{
			get
			{
				return this.Link(ref this.importController);
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001524 RID: 5412 RVA: 0x00089CC7 File Offset: 0x00087EC7
		[SerializeProperty("canvasDrawer")]
		public CanvasDrawer CanvasDrawer
		{
			get
			{
				return this.Link(ref this.canvasDrawer);
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001525 RID: 5413 RVA: 0x00089CD5 File Offset: 0x00087ED5
		[SerializeProperty("projectCacher")]
		public ProjectCacher ProjectCacher
		{
			get
			{
				return this.Link(ref this.projectCacher);
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001526 RID: 5414 RVA: 0x00089CE3 File Offset: 0x00087EE3
		[SerializeProperty("hashCacher")]
		public HashCacher HashCacher
		{
			get
			{
				return this.Link(ref this.hashCacher);
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x00089CF1 File Offset: 0x00087EF1
		[SerializeProperty("projectDownloader")]
		public ProjectDownloader ProjectDownloader
		{
			get
			{
				return this.Link(ref this.projectDownloader);
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001528 RID: 5416 RVA: 0x00089CFF File Offset: 0x00087EFF
		[SerializeProperty("imageTypeSetter")]
		public ImageTypeSetter ImageTypeSetter
		{
			get
			{
				return this.Link(ref this.imageTypeSetter);
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x00089D0D File Offset: 0x00087F0D
		[SerializeProperty("duplicateFinder")]
		public DuplicateFinder DuplicateFinder
		{
			get
			{
				return this.Link(ref this.duplicateFinder);
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x0600152A RID: 5418 RVA: 0x00089D1B File Offset: 0x00087F1B
		[SerializeProperty("scriptGenerator")]
		public ScriptGenerator ScriptGenerator
		{
			get
			{
				return this.Link(ref this.scriptGenerator);
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x0600152B RID: 5419 RVA: 0x00089D29 File Offset: 0x00087F29
		[SerializeProperty("delegateHolder")]
		public DelegateHolder DelegateHolder
		{
			get
			{
				return this.Link(ref this.delegateHolder);
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x00089D37 File Offset: 0x00087F37
		[SerializeProperty("settings")]
		public SettingsLinker Settings
		{
			get
			{
				return this.Link(ref this.settings);
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x0600152D RID: 5421 RVA: 0x00089D45 File Offset: 0x00087F45
		[SerializeProperty("eventHandlers")]
		public FcuEventHandlers EventHandlers
		{
			get
			{
				return this.Link(ref this.eventHandlers);
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x0600152E RID: 5422 RVA: 0x00089D53 File Offset: 0x00087F53
		[SerializeProperty("events")]
		public FcuEvents Events
		{
			get
			{
				return this.Link(ref this.events);
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x0600152F RID: 5423 RVA: 0x00089D61 File Offset: 0x00087F61
		[SerializeProperty("ctsController")]
		public CancellationTokenController CancellationTokenController
		{
			get
			{
				return this.Link(ref this.ctsController);
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001530 RID: 5424 RVA: 0x00089D6F File Offset: 0x00087F6F
		[SerializeProperty("prefabCreator")]
		public PrefabCreator PrefabCreator
		{
			get
			{
				return this.Link(ref this.prefabCreator);
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06001531 RID: 5425 RVA: 0x00089D7D File Offset: 0x00087F7D
		[SerializeProperty("prefabUpdater")]
		public PrefabUpdater PrefabUpdater
		{
			get
			{
				return this.Link(ref this.prefabUpdater);
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001532 RID: 5426 RVA: 0x00089D8B File Offset: 0x00087F8B
		[SerializeProperty("folderCreator")]
		public FolderCreator FolderCreator
		{
			get
			{
				return this.Link(ref this.folderCreator);
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001533 RID: 5427 RVA: 0x00089D99 File Offset: 0x00087F99
		[SerializeProperty("inspectorDrawer")]
		public InspectorDrawer InspectorDrawer
		{
			get
			{
				return this.Link(ref this.inspectorDrawer);
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06001534 RID: 5428 RVA: 0x00089DA7 File Offset: 0x00087FA7
		[SerializeProperty("authorizer")]
		public Authorizer Authorizer
		{
			get
			{
				return this.Link(ref this.authorizer);
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06001535 RID: 5429 RVA: 0x00089DB5 File Offset: 0x00087FB5
		[SerializeProperty("requestSender")]
		public RequestSender RequestSender
		{
			get
			{
				return this.Link(ref this.requestSender);
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06001536 RID: 5430 RVA: 0x00089DC3 File Offset: 0x00087FC3
		[SerializeProperty("hashGenerator")]
		public HashGenerator HashGenerator
		{
			get
			{
				return this.Link(ref this.hashGenerator);
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001537 RID: 5431 RVA: 0x00089DD1 File Offset: 0x00087FD1
		[SerializeProperty("nameHumanizer")]
		public NameHumanizer NameHumanizer
		{
			get
			{
				return this.Link(ref this.nameHumanizer);
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001538 RID: 5432 RVA: 0x00089DDF File Offset: 0x00087FDF
		[SerializeProperty("fontDownloader")]
		public FontDownloader FontDownloader
		{
			get
			{
				return this.Link(ref this.fontDownloader);
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06001539 RID: 5433 RVA: 0x00089DED File Offset: 0x00087FED
		[SerializeProperty("fontLoader")]
		public FontLoader FontLoader
		{
			get
			{
				return this.Link(ref this.fontLoader);
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x0600153A RID: 5434 RVA: 0x00089DFB File Offset: 0x00087FFB
		[SerializeProperty("graphicHelpers")]
		public GraphicHelpers GraphicHelpers
		{
			get
			{
				return this.Link(ref this.graphicHelpers);
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x0600153B RID: 5435 RVA: 0x00089E09 File Offset: 0x00088009
		[SerializeProperty("tagSetter")]
		public TagSetter TagSetter
		{
			get
			{
				return this.Link(ref this.tagSetter);
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x0600153C RID: 5436 RVA: 0x00089E17 File Offset: 0x00088017
		[SerializeProperty("spriteProcessor")]
		public SpriteProcessor SpriteProcessor
		{
			get
			{
				return this.Link(ref this.spriteProcessor);
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x0600153D RID: 5437 RVA: 0x00089E25 File Offset: 0x00088025
		[SerializeProperty("tools")]
		public AssetTools AssetTools
		{
			get
			{
				return this.Link(ref this.tools);
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x0600153E RID: 5438 RVA: 0x00089E33 File Offset: 0x00088033
		[SerializeProperty("nameSetter")]
		public NameSetter NameSetter
		{
			get
			{
				return this.Link(ref this.nameSetter);
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x0600153F RID: 5439 RVA: 0x00089E41 File Offset: 0x00088041
		[SerializeProperty("syncHelper")]
		public SyncHelpers SyncHelpers
		{
			get
			{
				return this.Link(ref this.syncHelper);
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x00089E4F File Offset: 0x0008804F
		[SerializeProperty("transformSetter")]
		public TransformSetter TransformSetter
		{
			get
			{
				return this.Link(ref this.transformSetter);
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001541 RID: 5441 RVA: 0x00089E5D File Offset: 0x0008805D
		[SerializeProperty("currentProject")]
		public CurrentProject CurrentProject
		{
			get
			{
				return this.Link(ref this.currentProject);
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001542 RID: 5442 RVA: 0x00089E6B File Offset: 0x0008806B
		[SerializeProperty("spriteGenerator")]
		public SpriteGenerator SpriteGenerator
		{
			get
			{
				return this.Link(ref this.spriteGenerator);
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001543 RID: 5443 RVA: 0x00089E79 File Offset: 0x00088079
		[SerializeProperty("spriteColorizer")]
		public SpriteColorizer SpriteColorizer
		{
			get
			{
				return this.Link(ref this.spriteColorizer);
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001544 RID: 5444 RVA: 0x00089E87 File Offset: 0x00088087
		[SerializeProperty("spritePathSetter")]
		public SpritePathSetter SpritePathSetter
		{
			get
			{
				return this.Link(ref this.spritePathSetter);
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001545 RID: 5445 RVA: 0x00089E95 File Offset: 0x00088095
		[SerializeProperty("spriteDownloader")]
		public SpriteDownloader SpriteDownloader
		{
			get
			{
				return this.Link(ref this.spriteDownloader);
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001546 RID: 5446 RVA: 0x00089EA3 File Offset: 0x000880A3
		[SerializeProperty("spriteSlicer")]
		public SpriteSlicer SpriteSlicer
		{
			get
			{
				return this.Link(ref this.spriteSlicer);
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001547 RID: 5447 RVA: 0x00089EB1 File Offset: 0x000880B1
		[SerializeProperty("preImportDataCreator")]
		public PreImportDataCreator PreImportDataCreator
		{
			get
			{
				return this.Link(ref this.preImportDataCreator);
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001548 RID: 5448 RVA: 0x00089EBF File Offset: 0x000880BF
		public string Guid
		{
			get
			{
				return this.guid.CreateShortGuid(out this.guid);
			}
		}

		// Token: 0x040016C4 RID: 5828
		[SerializeField]
		private ProjectImporter importController;

		// Token: 0x040016C5 RID: 5829
		[SerializeField]
		private CanvasDrawer canvasDrawer;

		// Token: 0x040016C6 RID: 5830
		[SerializeField]
		private ProjectCacher projectCacher;

		// Token: 0x040016C7 RID: 5831
		[SerializeField]
		private HashCacher hashCacher;

		// Token: 0x040016C8 RID: 5832
		[SerializeField]
		private ProjectDownloader projectDownloader;

		// Token: 0x040016C9 RID: 5833
		[SerializeField]
		private ImageTypeSetter imageTypeSetter;

		// Token: 0x040016CA RID: 5834
		[SerializeField]
		private DuplicateFinder duplicateFinder;

		// Token: 0x040016CB RID: 5835
		[SerializeField]
		private ScriptGenerator scriptGenerator;

		// Token: 0x040016CC RID: 5836
		[SerializeField]
		private DelegateHolder delegateHolder;

		// Token: 0x040016CD RID: 5837
		[SerializeField]
		private SettingsLinker settings;

		// Token: 0x040016CE RID: 5838
		[SerializeField]
		private FcuEventHandlers eventHandlers;

		// Token: 0x040016CF RID: 5839
		[SerializeField]
		private FcuEvents events;

		// Token: 0x040016D0 RID: 5840
		[SerializeField]
		private CancellationTokenController ctsController;

		// Token: 0x040016D1 RID: 5841
		[SerializeField]
		private PrefabCreator prefabCreator;

		// Token: 0x040016D2 RID: 5842
		[SerializeField]
		private PrefabUpdater prefabUpdater;

		// Token: 0x040016D3 RID: 5843
		[SerializeField]
		private FolderCreator folderCreator;

		// Token: 0x040016D4 RID: 5844
		[SerializeField]
		private InspectorDrawer inspectorDrawer;

		// Token: 0x040016D5 RID: 5845
		[SerializeField]
		private Authorizer authorizer;

		// Token: 0x040016D6 RID: 5846
		[SerializeField]
		private RequestSender requestSender;

		// Token: 0x040016D7 RID: 5847
		[SerializeField]
		private HashGenerator hashGenerator;

		// Token: 0x040016D8 RID: 5848
		[SerializeField]
		private NameHumanizer nameHumanizer;

		// Token: 0x040016D9 RID: 5849
		[SerializeField]
		private FontDownloader fontDownloader;

		// Token: 0x040016DA RID: 5850
		[SerializeField]
		private FontLoader fontLoader;

		// Token: 0x040016DB RID: 5851
		[SerializeField]
		private GraphicHelpers graphicHelpers;

		// Token: 0x040016DC RID: 5852
		[SerializeField]
		private TagSetter tagSetter;

		// Token: 0x040016DD RID: 5853
		[SerializeField]
		private SpriteProcessor spriteProcessor;

		// Token: 0x040016DE RID: 5854
		[SerializeField]
		private AssetTools tools;

		// Token: 0x040016DF RID: 5855
		[SerializeField]
		private NameSetter nameSetter;

		// Token: 0x040016E0 RID: 5856
		[SerializeField]
		private SyncHelpers syncHelper;

		// Token: 0x040016E1 RID: 5857
		[SerializeField]
		private TransformSetter transformSetter;

		// Token: 0x040016E2 RID: 5858
		[SerializeField]
		private CurrentProject currentProject;

		// Token: 0x040016E3 RID: 5859
		[SerializeField]
		private SpriteGenerator spriteGenerator;

		// Token: 0x040016E4 RID: 5860
		[SerializeField]
		private SpriteColorizer spriteColorizer;

		// Token: 0x040016E5 RID: 5861
		[SerializeField]
		private SpritePathSetter spritePathSetter;

		// Token: 0x040016E6 RID: 5862
		[SerializeField]
		private SpriteDownloader spriteDownloader;

		// Token: 0x040016E7 RID: 5863
		[SerializeField]
		private SpriteSlicer spriteSlicer;

		// Token: 0x040016E8 RID: 5864
		[SerializeField]
		private PreImportDataCreator preImportDataCreator;

		// Token: 0x040016E9 RID: 5865
		[SerializeField]
		private string guid;
	}
}
