using System;
using UnityEngine.UIElements.UIR;

namespace UnityEngine.UIElements
{
	// Token: 0x02000250 RID: 592
	[HelpURL("UIE-Runtime-Panel-Settings")]
	public class PanelSettings : ScriptableObject
	{
		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060010D7 RID: 4311 RVA: 0x0003D1DC File Offset: 0x0003B3DC
		// (set) Token: 0x060010D8 RID: 4312 RVA: 0x0003D1F4 File Offset: 0x0003B3F4
		public ThemeStyleSheet themeStyleSheet
		{
			get
			{
				return this.themeUss;
			}
			set
			{
				this.themeUss = value;
				this.ApplyThemeStyleSheet(null);
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x060010D9 RID: 4313 RVA: 0x0003D206 File Offset: 0x0003B406
		// (set) Token: 0x060010DA RID: 4314 RVA: 0x0003D20E File Offset: 0x0003B40E
		public RenderTexture targetTexture
		{
			get
			{
				return this.m_TargetTexture;
			}
			set
			{
				this.m_TargetTexture = value;
				this.m_PanelAccess.SetTargetTexture();
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060010DB RID: 4315 RVA: 0x0003D224 File Offset: 0x0003B424
		// (set) Token: 0x060010DC RID: 4316 RVA: 0x0003D22C File Offset: 0x0003B42C
		public PanelScaleMode scaleMode
		{
			get
			{
				return this.m_ScaleMode;
			}
			set
			{
				this.m_ScaleMode = value;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060010DD RID: 4317 RVA: 0x0003D238 File Offset: 0x0003B438
		// (set) Token: 0x060010DE RID: 4318 RVA: 0x0003D250 File Offset: 0x0003B450
		public float referenceSpritePixelsPerUnit
		{
			get
			{
				return this.m_ReferenceSpritePixelsPerUnit;
			}
			set
			{
				this.m_ReferenceSpritePixelsPerUnit = value;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x0003D25A File Offset: 0x0003B45A
		// (set) Token: 0x060010E0 RID: 4320 RVA: 0x0003D262 File Offset: 0x0003B462
		public float scale
		{
			get
			{
				return this.m_Scale;
			}
			set
			{
				this.m_Scale = value;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060010E1 RID: 4321 RVA: 0x0003D26B File Offset: 0x0003B46B
		// (set) Token: 0x060010E2 RID: 4322 RVA: 0x0003D273 File Offset: 0x0003B473
		public float referenceDpi
		{
			get
			{
				return this.m_ReferenceDpi;
			}
			set
			{
				this.m_ReferenceDpi = ((value >= 1f) ? value : 96f);
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x0003D28B File Offset: 0x0003B48B
		// (set) Token: 0x060010E4 RID: 4324 RVA: 0x0003D293 File Offset: 0x0003B493
		public float fallbackDpi
		{
			get
			{
				return this.m_FallbackDpi;
			}
			set
			{
				this.m_FallbackDpi = ((value >= 1f) ? value : 96f);
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060010E5 RID: 4325 RVA: 0x0003D2AB File Offset: 0x0003B4AB
		// (set) Token: 0x060010E6 RID: 4326 RVA: 0x0003D2B3 File Offset: 0x0003B4B3
		public Vector2Int referenceResolution
		{
			get
			{
				return this.m_ReferenceResolution;
			}
			set
			{
				this.m_ReferenceResolution = value;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060010E7 RID: 4327 RVA: 0x0003D2BC File Offset: 0x0003B4BC
		// (set) Token: 0x060010E8 RID: 4328 RVA: 0x0003D2C4 File Offset: 0x0003B4C4
		public PanelScreenMatchMode screenMatchMode
		{
			get
			{
				return this.m_ScreenMatchMode;
			}
			set
			{
				this.m_ScreenMatchMode = value;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060010E9 RID: 4329 RVA: 0x0003D2CD File Offset: 0x0003B4CD
		// (set) Token: 0x060010EA RID: 4330 RVA: 0x0003D2D5 File Offset: 0x0003B4D5
		public float match
		{
			get
			{
				return this.m_Match;
			}
			set
			{
				this.m_Match = value;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x0003D2DE File Offset: 0x0003B4DE
		// (set) Token: 0x060010EC RID: 4332 RVA: 0x0003D2E6 File Offset: 0x0003B4E6
		public float sortingOrder
		{
			get
			{
				return this.m_SortingOrder;
			}
			set
			{
				this.m_SortingOrder = value;
				this.ApplySortingOrder();
			}
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x0003D2F7 File Offset: 0x0003B4F7
		internal void ApplySortingOrder()
		{
			this.m_PanelAccess.SetSortingPriority();
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x0003D306 File Offset: 0x0003B506
		// (set) Token: 0x060010EF RID: 4335 RVA: 0x0003D30E File Offset: 0x0003B50E
		public int targetDisplay
		{
			get
			{
				return this.m_TargetDisplay;
			}
			set
			{
				this.m_TargetDisplay = value;
				this.m_PanelAccess.SetTargetDisplay();
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x0003D324 File Offset: 0x0003B524
		// (set) Token: 0x060010F1 RID: 4337 RVA: 0x0003D32C File Offset: 0x0003B52C
		public bool clearDepthStencil
		{
			get
			{
				return this.m_ClearDepthStencil;
			}
			set
			{
				this.m_ClearDepthStencil = value;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x0003D335 File Offset: 0x0003B535
		public float depthClearValue
		{
			get
			{
				return 0.99f;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x0003D33C File Offset: 0x0003B53C
		// (set) Token: 0x060010F4 RID: 4340 RVA: 0x0003D344 File Offset: 0x0003B544
		public bool clearColor
		{
			get
			{
				return this.m_ClearColor;
			}
			set
			{
				this.m_ClearColor = value;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x0003D34D File Offset: 0x0003B54D
		// (set) Token: 0x060010F6 RID: 4342 RVA: 0x0003D355 File Offset: 0x0003B555
		public Color colorClearValue
		{
			get
			{
				return this.m_ColorClearValue;
			}
			set
			{
				this.m_ColorClearValue = value;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x0003D35E File Offset: 0x0003B55E
		internal BaseRuntimePanel panel
		{
			get
			{
				return this.m_PanelAccess.panel;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x060010F8 RID: 4344 RVA: 0x0003D36B File Offset: 0x0003B56B
		internal bool isInitialized
		{
			get
			{
				PanelSettings.RuntimePanelAccess panelAccess = this.m_PanelAccess;
				return panelAccess != null && panelAccess.isInitialized;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x060010F9 RID: 4345 RVA: 0x0003D37F File Offset: 0x0003B57F
		internal VisualElement visualTree
		{
			get
			{
				return this.m_PanelAccess.panel.visualTree;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060010FA RID: 4346 RVA: 0x0003D391 File Offset: 0x0003B591
		// (set) Token: 0x060010FB RID: 4347 RVA: 0x0003D399 File Offset: 0x0003B599
		public DynamicAtlasSettings dynamicAtlasSettings
		{
			get
			{
				return this.m_DynamicAtlasSettings;
			}
			set
			{
				this.m_DynamicAtlasSettings = value;
			}
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0003D3A4 File Offset: 0x0003B5A4
		private PanelSettings()
		{
			this.m_PanelAccess = new PanelSettings.RuntimePanelAccess(this);
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00003CD2 File Offset: 0x00001ED2
		private void Reset()
		{
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0003D450 File Offset: 0x0003B650
		private void OnEnable()
		{
			bool flag = this.themeUss == null;
			if (flag)
			{
				Debug.LogWarning("No Theme Style Sheet set to PanelSettings " + base.name + ", UI will not render properly", this);
			}
			this.UpdateScreenDPI();
			this.InitializeShaders();
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0003D49C File Offset: 0x0003B69C
		private void OnDisable()
		{
			this.m_PanelAccess.DisposePanel();
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0003D49C File Offset: 0x0003B69C
		internal void DisposePanel()
		{
			this.m_PanelAccess.DisposePanel();
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001101 RID: 4353 RVA: 0x0003D4AB File Offset: 0x0003B6AB
		// (set) Token: 0x06001102 RID: 4354 RVA: 0x0003D4B3 File Offset: 0x0003B6B3
		private float ScreenDPI { get; set; }

		// Token: 0x06001103 RID: 4355 RVA: 0x0003D4BC File Offset: 0x0003B6BC
		internal void UpdateScreenDPI()
		{
			this.ScreenDPI = Screen.dpi;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0003D4CC File Offset: 0x0003B6CC
		private void ApplyThemeStyleSheet(VisualElement root = null)
		{
			bool flag = !this.m_PanelAccess.isInitialized;
			if (!flag)
			{
				bool flag2 = root == null;
				if (flag2)
				{
					root = this.visualTree;
				}
				bool flag3 = this.m_OldThemeUss != this.themeUss && this.m_OldThemeUss != null;
				if (flag3)
				{
					if (root != null)
					{
						root.styleSheets.Remove(this.m_OldThemeUss);
					}
				}
				bool flag4 = this.themeUss != null;
				if (flag4)
				{
					this.themeUss.isDefaultStyleSheet = true;
					if (root != null)
					{
						root.styleSheets.Add(this.themeUss);
					}
				}
				this.m_OldThemeUss = this.themeUss;
			}
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x0003D590 File Offset: 0x0003B790
		private void InitializeShaders()
		{
			bool flag = this.m_AtlasBlitShader == null;
			if (flag)
			{
				this.m_AtlasBlitShader = Shader.Find(Shaders.k_AtlasBlit);
			}
			bool flag2 = this.m_RuntimeShader == null;
			if (flag2)
			{
				this.m_RuntimeShader = Shader.Find(Shaders.k_Runtime);
			}
			bool flag3 = this.m_RuntimeWorldShader == null;
			if (flag3)
			{
				this.m_RuntimeWorldShader = Shader.Find(Shaders.k_RuntimeWorld);
			}
			this.m_PanelAccess.SetTargetTexture();
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x0003D610 File Offset: 0x0003B810
		internal void ApplyPanelSettings()
		{
			Rect targetRect = this.m_TargetRect;
			float resolvedScale = this.m_ResolvedScale;
			this.UpdateScreenDPI();
			this.m_TargetRect = this.GetDisplayRect();
			this.m_ResolvedScale = this.ResolveScale(this.m_TargetRect, this.ScreenDPI);
			bool flag = this.visualTree.style.width.value == 0f || this.m_ResolvedScale != resolvedScale || this.m_TargetRect.width != targetRect.width || this.m_TargetRect.height != targetRect.height;
			if (flag)
			{
				this.panel.scale = ((this.m_ResolvedScale == 0f) ? 0f : (1f / this.m_ResolvedScale));
				this.visualTree.style.left = 0f;
				this.visualTree.style.top = 0f;
				this.visualTree.style.width = this.m_TargetRect.width * this.m_ResolvedScale;
				this.visualTree.style.height = this.m_TargetRect.height * this.m_ResolvedScale;
			}
			this.panel.targetTexture = this.targetTexture;
			this.panel.targetDisplay = this.targetDisplay;
			this.panel.drawToCameras = false;
			this.panel.clearSettings = new PanelClearSettings
			{
				clearColor = this.m_ClearColor,
				clearDepthStencil = this.m_ClearDepthStencil,
				color = this.m_ColorClearValue
			};
			this.panel.referenceSpritePixelsPerUnit = this.referenceSpritePixelsPerUnit;
			DynamicAtlas dynamicAtlas = this.panel.atlas as DynamicAtlas;
			bool flag2 = dynamicAtlas != null;
			if (flag2)
			{
				dynamicAtlas.minAtlasSize = this.dynamicAtlasSettings.minAtlasSize;
				dynamicAtlas.maxAtlasSize = this.dynamicAtlasSettings.maxAtlasSize;
				dynamicAtlas.maxSubTextureSize = this.dynamicAtlasSettings.maxSubTextureSize;
				dynamicAtlas.activeFilters = this.dynamicAtlasSettings.activeFilters;
				dynamicAtlas.customFilter = this.dynamicAtlasSettings.customFilter;
			}
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0003D869 File Offset: 0x0003BA69
		public void SetScreenToPanelSpaceFunction(Func<Vector2, Vector2> screentoPanelSpaceFunction)
		{
			this.m_AssignedScreenToPanel = screentoPanelSpaceFunction;
			this.panel.screenToPanelSpace = this.m_AssignedScreenToPanel;
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x0003D888 File Offset: 0x0003BA88
		internal float ResolveScale(Rect targetRect, float screenDpi)
		{
			float num = 1f;
			switch (this.scaleMode)
			{
			case PanelScaleMode.ConstantPhysicalSize:
			{
				float num2 = ((screenDpi == 0f) ? this.fallbackDpi : screenDpi);
				bool flag = num2 != 0f;
				if (flag)
				{
					num = this.referenceDpi / num2;
				}
				break;
			}
			case PanelScaleMode.ScaleWithScreenSize:
			{
				bool flag2 = this.referenceResolution.x * this.referenceResolution.y != 0;
				if (flag2)
				{
					Vector2 vector = this.referenceResolution;
					Vector2 vector2 = new Vector2(targetRect.width / vector.x, targetRect.height / vector.y);
					PanelScreenMatchMode screenMatchMode = this.screenMatchMode;
					PanelScreenMatchMode panelScreenMatchMode = screenMatchMode;
					float num4;
					if (panelScreenMatchMode != PanelScreenMatchMode.Shrink)
					{
						if (panelScreenMatchMode != PanelScreenMatchMode.Expand)
						{
							float num3 = Mathf.Clamp01(this.match);
							num4 = Mathf.Lerp(vector2.x, vector2.y, num3);
						}
						else
						{
							num4 = Mathf.Min(vector2.x, vector2.y);
						}
					}
					else
					{
						num4 = Mathf.Max(vector2.x, vector2.y);
					}
					bool flag3 = num4 != 0f;
					if (flag3)
					{
						num = 1f / num4;
					}
				}
				break;
			}
			}
			bool flag4 = this.scale > 0f;
			if (flag4)
			{
				num /= this.scale;
			}
			else
			{
				num = 0f;
			}
			return num;
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0003DA14 File Offset: 0x0003BC14
		internal Rect GetDisplayRect()
		{
			bool flag = this.m_TargetTexture != null;
			Rect rect;
			if (flag)
			{
				rect = new Rect(0f, 0f, (float)this.m_TargetTexture.width, (float)this.m_TargetTexture.height);
			}
			else
			{
				rect = new Rect(0f, 0f, (float)BaseRuntimePanel.getScreenRenderingWidth(this.targetDisplay), (float)BaseRuntimePanel.getScreenRenderingHeight(this.targetDisplay));
			}
			return rect;
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0003DA88 File Offset: 0x0003BC88
		internal void AttachAndInsertUIDocumentToVisualTree(UIDocument uiDocument)
		{
			bool flag = this.m_AttachedUIDocumentsList == null;
			if (flag)
			{
				this.m_AttachedUIDocumentsList = new UIDocumentList();
			}
			else
			{
				this.m_AttachedUIDocumentsList.RemoveFromListAndFromVisualTree(uiDocument);
			}
			this.m_AttachedUIDocumentsList.AddToListAndToVisualTree(uiDocument, this.visualTree, 0);
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0003DAD8 File Offset: 0x0003BCD8
		internal void DetachUIDocument(UIDocument uiDocument)
		{
			bool flag = this.m_AttachedUIDocumentsList == null;
			if (!flag)
			{
				this.m_AttachedUIDocumentsList.RemoveFromListAndFromVisualTree(uiDocument);
				bool flag2 = this.m_AttachedUIDocumentsList.m_AttachedUIDocuments.Count == 0;
				if (flag2)
				{
					this.m_PanelAccess.MarkPotentiallyEmpty();
				}
			}
		}

		// Token: 0x0400076E RID: 1902
		private const int k_DefaultSortingOrder = 0;

		// Token: 0x0400076F RID: 1903
		private const float k_DefaultScaleValue = 1f;

		// Token: 0x04000770 RID: 1904
		internal const string k_DefaultStyleSheetPath = "Packages/com.unity.ui/PackageResources/StyleSheets/Generated/Default.tss.asset";

		// Token: 0x04000771 RID: 1905
		[SerializeField]
		private ThemeStyleSheet themeUss;

		// Token: 0x04000772 RID: 1906
		[SerializeField]
		private RenderTexture m_TargetTexture;

		// Token: 0x04000773 RID: 1907
		[SerializeField]
		private PanelScaleMode m_ScaleMode = PanelScaleMode.ConstantPhysicalSize;

		// Token: 0x04000774 RID: 1908
		[SerializeField]
		private float m_ReferenceSpritePixelsPerUnit = 100f;

		// Token: 0x04000775 RID: 1909
		[SerializeField]
		private float m_Scale = 1f;

		// Token: 0x04000776 RID: 1910
		private const float DefaultDpi = 96f;

		// Token: 0x04000777 RID: 1911
		[SerializeField]
		private float m_ReferenceDpi = 96f;

		// Token: 0x04000778 RID: 1912
		[SerializeField]
		private float m_FallbackDpi = 96f;

		// Token: 0x04000779 RID: 1913
		[SerializeField]
		private Vector2Int m_ReferenceResolution = new Vector2Int(1200, 800);

		// Token: 0x0400077A RID: 1914
		[SerializeField]
		private PanelScreenMatchMode m_ScreenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;

		// Token: 0x0400077B RID: 1915
		[Range(0f, 1f)]
		[SerializeField]
		private float m_Match = 0f;

		// Token: 0x0400077C RID: 1916
		[SerializeField]
		private float m_SortingOrder = 0f;

		// Token: 0x0400077D RID: 1917
		[SerializeField]
		private int m_TargetDisplay = 0;

		// Token: 0x0400077E RID: 1918
		[SerializeField]
		private bool m_ClearDepthStencil = true;

		// Token: 0x0400077F RID: 1919
		[SerializeField]
		private bool m_ClearColor;

		// Token: 0x04000780 RID: 1920
		[SerializeField]
		private Color m_ColorClearValue = Color.clear;

		// Token: 0x04000781 RID: 1921
		private PanelSettings.RuntimePanelAccess m_PanelAccess;

		// Token: 0x04000782 RID: 1922
		internal UIDocumentList m_AttachedUIDocumentsList;

		// Token: 0x04000783 RID: 1923
		[SerializeField]
		[HideInInspector]
		private DynamicAtlasSettings m_DynamicAtlasSettings = DynamicAtlasSettings.defaults;

		// Token: 0x04000784 RID: 1924
		[SerializeField]
		[HideInInspector]
		private Shader m_AtlasBlitShader;

		// Token: 0x04000785 RID: 1925
		[SerializeField]
		[HideInInspector]
		private Shader m_RuntimeShader;

		// Token: 0x04000786 RID: 1926
		[HideInInspector]
		[SerializeField]
		private Shader m_RuntimeWorldShader;

		// Token: 0x04000787 RID: 1927
		[SerializeField]
		public PanelTextSettings textSettings;

		// Token: 0x04000788 RID: 1928
		private Rect m_TargetRect;

		// Token: 0x04000789 RID: 1929
		private float m_ResolvedScale;

		// Token: 0x0400078A RID: 1930
		private StyleSheet m_OldThemeUss;

		// Token: 0x0400078C RID: 1932
		private Func<Vector2, Vector2> m_AssignedScreenToPanel;

		// Token: 0x02000251 RID: 593
		private class RuntimePanelAccess
		{
			// Token: 0x0600110C RID: 4364 RVA: 0x0003DB26 File Offset: 0x0003BD26
			internal RuntimePanelAccess(PanelSettings settings)
			{
				this.m_Settings = settings;
			}

			// Token: 0x170003A1 RID: 929
			// (get) Token: 0x0600110D RID: 4365 RVA: 0x0003DB37 File Offset: 0x0003BD37
			internal bool isInitialized
			{
				get
				{
					return this.m_RuntimePanel != null;
				}
			}

			// Token: 0x170003A2 RID: 930
			// (get) Token: 0x0600110E RID: 4366 RVA: 0x0003DB44 File Offset: 0x0003BD44
			internal BaseRuntimePanel panel
			{
				get
				{
					bool flag = this.m_RuntimePanel == null;
					if (flag)
					{
						this.m_RuntimePanel = this.CreateRelatedRuntimePanel();
						this.m_RuntimePanel.sortingPriority = this.m_Settings.m_SortingOrder;
						this.m_RuntimePanel.targetDisplay = this.m_Settings.m_TargetDisplay;
						VisualElement visualTree = this.m_RuntimePanel.visualTree;
						visualTree.name = this.m_Settings.name;
						this.m_Settings.ApplyThemeStyleSheet(visualTree);
						bool flag2 = this.m_Settings.m_TargetTexture != null;
						if (flag2)
						{
							this.m_RuntimePanel.targetTexture = this.m_Settings.m_TargetTexture;
						}
						bool flag3 = this.m_Settings.m_AssignedScreenToPanel != null;
						if (flag3)
						{
							this.m_Settings.SetScreenToPanelSpaceFunction(this.m_Settings.m_AssignedScreenToPanel);
						}
					}
					return this.m_RuntimePanel;
				}
			}

			// Token: 0x0600110F RID: 4367 RVA: 0x0003DC30 File Offset: 0x0003BE30
			internal void DisposePanel()
			{
				bool flag = this.m_RuntimePanel != null;
				if (flag)
				{
					this.DisposeRelatedPanel();
					this.m_RuntimePanel = null;
				}
			}

			// Token: 0x06001110 RID: 4368 RVA: 0x0003DC5C File Offset: 0x0003BE5C
			internal void SetTargetTexture()
			{
				bool flag = this.m_RuntimePanel != null;
				if (flag)
				{
					this.m_RuntimePanel.targetTexture = this.m_Settings.targetTexture;
				}
			}

			// Token: 0x06001111 RID: 4369 RVA: 0x0003DC90 File Offset: 0x0003BE90
			internal void SetSortingPriority()
			{
				bool flag = this.m_RuntimePanel != null;
				if (flag)
				{
					this.m_RuntimePanel.sortingPriority = this.m_Settings.m_SortingOrder;
				}
			}

			// Token: 0x06001112 RID: 4370 RVA: 0x0003DCC4 File Offset: 0x0003BEC4
			internal void SetTargetDisplay()
			{
				bool flag = this.m_RuntimePanel != null;
				if (flag)
				{
					this.m_RuntimePanel.targetDisplay = this.m_Settings.m_TargetDisplay;
				}
			}

			// Token: 0x06001113 RID: 4371 RVA: 0x0003DCF8 File Offset: 0x0003BEF8
			private BaseRuntimePanel CreateRelatedRuntimePanel()
			{
				return (RuntimePanel)UIElementsRuntimeUtility.FindOrCreateRuntimePanel(this.m_Settings, new UIElementsRuntimeUtility.CreateRuntimePanelDelegate(RuntimePanel.Create));
			}

			// Token: 0x06001114 RID: 4372 RVA: 0x0003DD28 File Offset: 0x0003BF28
			private void DisposeRelatedPanel()
			{
				UIElementsRuntimeUtility.DisposeRuntimePanel(this.m_Settings);
			}

			// Token: 0x06001115 RID: 4373 RVA: 0x0003DD37 File Offset: 0x0003BF37
			internal void MarkPotentiallyEmpty()
			{
				UIElementsRuntimeUtility.MarkPotentiallyEmpty(this.m_Settings);
			}

			// Token: 0x0400078D RID: 1933
			private readonly PanelSettings m_Settings;

			// Token: 0x0400078E RID: 1934
			private BaseRuntimePanel m_RuntimePanel;
		}
	}
}
