using System;
using DA_Assets.DAI;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x020002E7 RID: 743
	[Serializable]
	public class SettingsLinker : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x00085E66 File Offset: 0x00084066
		[SerializeProperty("mainSettings")]
		public MainSettings MainSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.mainSettings);
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001498 RID: 5272 RVA: 0x00085E79 File Offset: 0x00084079
		[SerializeProperty("uitkSettings")]
		public UITK_Settings UITK_Settings
		{
			get
			{
				return this.monoBeh.Link(ref this.uitkSettings);
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001499 RID: 5273 RVA: 0x00085E8C File Offset: 0x0008408C
		[SerializeProperty("imageSpritesSettings")]
		public ImageSpritesSettings ImageSpritesSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.imageSpritesSettings);
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x0600149A RID: 5274 RVA: 0x00085E9F File Offset: 0x0008409F
		public TextureImporterSettings TextureImporterSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.textureImporterSettings);
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x0600149B RID: 5275 RVA: 0x00085EB2 File Offset: 0x000840B2
		[SerializeProperty("shadowSettings")]
		public ShadowSettings ShadowSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.shadowSettings);
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x0600149C RID: 5276 RVA: 0x00085EC5 File Offset: 0x000840C5
		[SerializeProperty("buttonSettings")]
		public ButtonSettings ButtonSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.buttonSettings);
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x00085ED8 File Offset: 0x000840D8
		[SerializeProperty("locSettings")]
		public LocalizationSettings LocalizationSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.locSettings);
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x00085EEB File Offset: 0x000840EB
		[SerializeProperty("scriptGenSettings")]
		public ScriptGeneratorSettings ScriptGeneratorSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.scriptGenSettings);
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x0600149F RID: 5279 RVA: 0x00085EFE File Offset: 0x000840FE
		[SerializeProperty("novaSettings")]
		public NovaSettings NovaSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.novaSettings);
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060014A0 RID: 5280 RVA: 0x00085F11 File Offset: 0x00084111
		[SerializeProperty("joshPuiSettings")]
		public JoshPuiSettings JoshPuiSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.joshPuiSettings);
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x060014A1 RID: 5281 RVA: 0x00085F24 File Offset: 0x00084124
		[SerializeProperty("mpuikitSettings")]
		public MPUIKitSettings MPUIKitSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.mpuikitSettings);
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x060014A2 RID: 5282 RVA: 0x00085F37 File Offset: 0x00084137
		[SerializeProperty("dttPuiSettings")]
		public DttPuiSettings DttPuiSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.dttPuiSettings);
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x060014A3 RID: 5283 RVA: 0x00085F4A File Offset: 0x0008414A
		[SerializeProperty("svgImageSettings")]
		public SvgImageSettings SvgImageSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.svgImageSettings);
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x060014A4 RID: 5284 RVA: 0x00085F5D File Offset: 0x0008415D
		[SerializeProperty("svgImporterSettings")]
		public SVGImporterSettings SVGImporterSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.svgImporterSettings);
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x00085F70 File Offset: 0x00084170
		[SerializeProperty("unityImageSettings")]
		public UnityImageSettings UnityImageSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.unityImageSettings);
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x060014A6 RID: 5286 RVA: 0x00085F83 File Offset: 0x00084183
		[SerializeProperty("rawImageSettings")]
		public RawImageSettings RawImageSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.rawImageSettings);
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x060014A7 RID: 5287 RVA: 0x00085F96 File Offset: 0x00084196
		[SerializeProperty("shapes2D_Settings")]
		public Shapes2DSettings Shapes2DSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.shapes2D_Settings);
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x060014A8 RID: 5288 RVA: 0x00085FA9 File Offset: 0x000841A9
		[SerializeProperty("srSettings")]
		public SpriteRendererSettings SpriteRendererSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.srSettings);
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x060014A9 RID: 5289 RVA: 0x00085FBC File Offset: 0x000841BC
		[SerializeProperty("textMeshSettings")]
		public TextMeshSettings TextMeshSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.textMeshSettings);
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x060014AA RID: 5290 RVA: 0x00085FCF File Offset: 0x000841CF
		[SerializeProperty("textFontsSettings")]
		public TextFontsSettings TextFontsSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.textFontsSettings);
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x060014AB RID: 5291 RVA: 0x00085FE2 File Offset: 0x000841E2
		[SerializeProperty("unityTextSettings")]
		public UnityTextSettings UnityTextSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.unityTextSettings);
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x060014AC RID: 5292 RVA: 0x00085FF5 File Offset: 0x000841F5
		[SerializeProperty("prefabSettings")]
		public PrefabSettings PrefabSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.prefabSettings);
			}
		}

		// Token: 0x04001638 RID: 5688
		[SerializeField]
		private MainSettings mainSettings;

		// Token: 0x04001639 RID: 5689
		[SerializeField]
		private UITK_Settings uitkSettings;

		// Token: 0x0400163A RID: 5690
		[SerializeField]
		private ImageSpritesSettings imageSpritesSettings;

		// Token: 0x0400163B RID: 5691
		[SerializeField]
		private TextureImporterSettings textureImporterSettings;

		// Token: 0x0400163C RID: 5692
		[SerializeField]
		private ShadowSettings shadowSettings;

		// Token: 0x0400163D RID: 5693
		[SerializeField]
		private ButtonSettings buttonSettings;

		// Token: 0x0400163E RID: 5694
		[SerializeField]
		private LocalizationSettings locSettings;

		// Token: 0x0400163F RID: 5695
		[SerializeField]
		private ScriptGeneratorSettings scriptGenSettings;

		// Token: 0x04001640 RID: 5696
		[SerializeField]
		private NovaSettings novaSettings;

		// Token: 0x04001641 RID: 5697
		[SerializeField]
		private JoshPuiSettings joshPuiSettings;

		// Token: 0x04001642 RID: 5698
		[SerializeField]
		private MPUIKitSettings mpuikitSettings;

		// Token: 0x04001643 RID: 5699
		[SerializeField]
		private DttPuiSettings dttPuiSettings;

		// Token: 0x04001644 RID: 5700
		[SerializeField]
		private SvgImageSettings svgImageSettings;

		// Token: 0x04001645 RID: 5701
		[SerializeField]
		private SVGImporterSettings svgImporterSettings;

		// Token: 0x04001646 RID: 5702
		[SerializeField]
		private UnityImageSettings unityImageSettings;

		// Token: 0x04001647 RID: 5703
		[SerializeField]
		private RawImageSettings rawImageSettings;

		// Token: 0x04001648 RID: 5704
		[SerializeField]
		private Shapes2DSettings shapes2D_Settings;

		// Token: 0x04001649 RID: 5705
		[SerializeField]
		private SpriteRendererSettings srSettings;

		// Token: 0x0400164A RID: 5706
		[SerializeField]
		private TextMeshSettings textMeshSettings;

		// Token: 0x0400164B RID: 5707
		[SerializeField]
		private TextFontsSettings textFontsSettings;

		// Token: 0x0400164C RID: 5708
		[SerializeField]
		private UnityTextSettings unityTextSettings;

		// Token: 0x0400164D RID: 5709
		[SerializeField]
		private PrefabSettings prefabSettings;
	}
}
