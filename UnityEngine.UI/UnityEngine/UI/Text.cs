using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	// Token: 0x0200003A RID: 58
	[RequireComponent(typeof(CanvasRenderer))]
	[AddComponentMenu("UI/Legacy/Text", 100)]
	public class Text : MaskableGraphic, ILayoutElement
	{
		// Token: 0x0600043E RID: 1086 RVA: 0x0001500E File Offset: 0x0001320E
		protected Text()
		{
			base.useLegacyMeshGeneration = false;
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x00015040 File Offset: 0x00013240
		public TextGenerator cachedTextGenerator
		{
			get
			{
				TextGenerator textGenerator;
				if ((textGenerator = this.m_TextCache) == null)
				{
					textGenerator = (this.m_TextCache = ((this.m_Text.Length != 0) ? new TextGenerator(this.m_Text.Length) : new TextGenerator()));
				}
				return textGenerator;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00015084 File Offset: 0x00013284
		public TextGenerator cachedTextGeneratorForLayout
		{
			get
			{
				TextGenerator textGenerator;
				if ((textGenerator = this.m_TextCacheForLayout) == null)
				{
					textGenerator = (this.m_TextCacheForLayout = new TextGenerator());
				}
				return textGenerator;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x000150AC File Offset: 0x000132AC
		public override Texture mainTexture
		{
			get
			{
				if (this.font != null && this.font.material != null && this.font.material.mainTexture != null)
				{
					return this.font.material.mainTexture;
				}
				if (this.m_Material != null)
				{
					return this.m_Material.mainTexture;
				}
				return base.mainTexture;
			}
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00015124 File Offset: 0x00013324
		public void FontTextureChanged()
		{
			if (!this)
			{
				return;
			}
			if (this.m_DisableFontTextureRebuiltCallback)
			{
				return;
			}
			this.cachedTextGenerator.Invalidate();
			if (!this.IsActive())
			{
				return;
			}
			if (CanvasUpdateRegistry.IsRebuildingGraphics() || CanvasUpdateRegistry.IsRebuildingLayout())
			{
				this.UpdateGeometry();
				return;
			}
			this.SetAllDirty();
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x00015172 File Offset: 0x00013372
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x00015180 File Offset: 0x00013380
		public Font font
		{
			get
			{
				return this.m_FontData.font;
			}
			set
			{
				if (this.m_FontData.font == value)
				{
					return;
				}
				if (base.isActiveAndEnabled)
				{
					FontUpdateTracker.UntrackText(this);
				}
				this.m_FontData.font = value;
				if (base.isActiveAndEnabled)
				{
					FontUpdateTracker.TrackText(this);
				}
				this.SetAllDirty();
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x000151CF File Offset: 0x000133CF
		// (set) Token: 0x06000446 RID: 1094 RVA: 0x000151D8 File Offset: 0x000133D8
		public virtual string text
		{
			get
			{
				return this.m_Text;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					if (this.m_Text != value)
					{
						this.m_Text = value;
						this.SetVerticesDirty();
						this.SetLayoutDirty();
					}
					return;
				}
				if (string.IsNullOrEmpty(this.m_Text))
				{
					return;
				}
				this.m_Text = "";
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x0001522E File Offset: 0x0001342E
		// (set) Token: 0x06000448 RID: 1096 RVA: 0x0001523B File Offset: 0x0001343B
		public bool supportRichText
		{
			get
			{
				return this.m_FontData.richText;
			}
			set
			{
				if (this.m_FontData.richText == value)
				{
					return;
				}
				this.m_FontData.richText = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x00015264 File Offset: 0x00013464
		// (set) Token: 0x0600044A RID: 1098 RVA: 0x00015271 File Offset: 0x00013471
		public bool resizeTextForBestFit
		{
			get
			{
				return this.m_FontData.bestFit;
			}
			set
			{
				if (this.m_FontData.bestFit == value)
				{
					return;
				}
				this.m_FontData.bestFit = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x0001529A File Offset: 0x0001349A
		// (set) Token: 0x0600044C RID: 1100 RVA: 0x000152A7 File Offset: 0x000134A7
		public int resizeTextMinSize
		{
			get
			{
				return this.m_FontData.minSize;
			}
			set
			{
				if (this.m_FontData.minSize == value)
				{
					return;
				}
				this.m_FontData.minSize = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x000152D0 File Offset: 0x000134D0
		// (set) Token: 0x0600044E RID: 1102 RVA: 0x000152DD File Offset: 0x000134DD
		public int resizeTextMaxSize
		{
			get
			{
				return this.m_FontData.maxSize;
			}
			set
			{
				if (this.m_FontData.maxSize == value)
				{
					return;
				}
				this.m_FontData.maxSize = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x00015306 File Offset: 0x00013506
		// (set) Token: 0x06000450 RID: 1104 RVA: 0x00015313 File Offset: 0x00013513
		public TextAnchor alignment
		{
			get
			{
				return this.m_FontData.alignment;
			}
			set
			{
				if (this.m_FontData.alignment == value)
				{
					return;
				}
				this.m_FontData.alignment = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x0001533C File Offset: 0x0001353C
		// (set) Token: 0x06000452 RID: 1106 RVA: 0x00015349 File Offset: 0x00013549
		public bool alignByGeometry
		{
			get
			{
				return this.m_FontData.alignByGeometry;
			}
			set
			{
				if (this.m_FontData.alignByGeometry == value)
				{
					return;
				}
				this.m_FontData.alignByGeometry = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x0001536C File Offset: 0x0001356C
		// (set) Token: 0x06000454 RID: 1108 RVA: 0x00015379 File Offset: 0x00013579
		public int fontSize
		{
			get
			{
				return this.m_FontData.fontSize;
			}
			set
			{
				if (this.m_FontData.fontSize == value)
				{
					return;
				}
				this.m_FontData.fontSize = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x000153A2 File Offset: 0x000135A2
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x000153AF File Offset: 0x000135AF
		public HorizontalWrapMode horizontalOverflow
		{
			get
			{
				return this.m_FontData.horizontalOverflow;
			}
			set
			{
				if (this.m_FontData.horizontalOverflow == value)
				{
					return;
				}
				this.m_FontData.horizontalOverflow = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x000153D8 File Offset: 0x000135D8
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x000153E5 File Offset: 0x000135E5
		public VerticalWrapMode verticalOverflow
		{
			get
			{
				return this.m_FontData.verticalOverflow;
			}
			set
			{
				if (this.m_FontData.verticalOverflow == value)
				{
					return;
				}
				this.m_FontData.verticalOverflow = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x0001540E File Offset: 0x0001360E
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x0001541B File Offset: 0x0001361B
		public float lineSpacing
		{
			get
			{
				return this.m_FontData.lineSpacing;
			}
			set
			{
				if (this.m_FontData.lineSpacing == value)
				{
					return;
				}
				this.m_FontData.lineSpacing = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x00015444 File Offset: 0x00013644
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x00015451 File Offset: 0x00013651
		public FontStyle fontStyle
		{
			get
			{
				return this.m_FontData.fontStyle;
			}
			set
			{
				if (this.m_FontData.fontStyle == value)
				{
					return;
				}
				this.m_FontData.fontStyle = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0001547C File Offset: 0x0001367C
		public float pixelsPerUnit
		{
			get
			{
				Canvas canvas = base.canvas;
				if (!canvas)
				{
					return 1f;
				}
				if (!this.font || this.font.dynamic)
				{
					return canvas.scaleFactor;
				}
				if (this.m_FontData.fontSize <= 0 || this.font.fontSize <= 0)
				{
					return 1f;
				}
				return (float)this.font.fontSize / (float)this.m_FontData.fontSize;
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000154FA File Offset: 0x000136FA
		protected override void OnEnable()
		{
			base.OnEnable();
			this.cachedTextGenerator.Invalidate();
			FontUpdateTracker.TrackText(this);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00015513 File Offset: 0x00013713
		protected override void OnDisable()
		{
			FontUpdateTracker.UntrackText(this);
			base.OnDisable();
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00015521 File Offset: 0x00013721
		protected override void UpdateGeometry()
		{
			if (this.font != null)
			{
				base.UpdateGeometry();
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00015537 File Offset: 0x00013737
		internal void AssignDefaultFont()
		{
			this.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00015549 File Offset: 0x00013749
		internal void AssignDefaultFontIfNecessary()
		{
			if (this.font == null)
			{
				this.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0001556C File Offset: 0x0001376C
		public TextGenerationSettings GetGenerationSettings(Vector2 extents)
		{
			TextGenerationSettings textGenerationSettings = default(TextGenerationSettings);
			textGenerationSettings.generationExtents = extents;
			if (this.font != null && this.font.dynamic)
			{
				textGenerationSettings.fontSize = this.m_FontData.fontSize;
				textGenerationSettings.resizeTextMinSize = this.m_FontData.minSize;
				textGenerationSettings.resizeTextMaxSize = this.m_FontData.maxSize;
			}
			textGenerationSettings.textAnchor = this.m_FontData.alignment;
			textGenerationSettings.alignByGeometry = this.m_FontData.alignByGeometry;
			textGenerationSettings.scaleFactor = this.pixelsPerUnit;
			textGenerationSettings.color = this.color;
			textGenerationSettings.font = this.font;
			textGenerationSettings.pivot = base.rectTransform.pivot;
			textGenerationSettings.richText = this.m_FontData.richText;
			textGenerationSettings.lineSpacing = this.m_FontData.lineSpacing;
			textGenerationSettings.fontStyle = this.m_FontData.fontStyle;
			textGenerationSettings.resizeTextForBestFit = this.m_FontData.bestFit;
			textGenerationSettings.updateBounds = false;
			textGenerationSettings.horizontalOverflow = this.m_FontData.horizontalOverflow;
			textGenerationSettings.verticalOverflow = this.m_FontData.verticalOverflow;
			return textGenerationSettings;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000156AC File Offset: 0x000138AC
		public static Vector2 GetTextAnchorPivot(TextAnchor anchor)
		{
			switch (anchor)
			{
			case TextAnchor.UpperLeft:
				return new Vector2(0f, 1f);
			case TextAnchor.UpperCenter:
				return new Vector2(0.5f, 1f);
			case TextAnchor.UpperRight:
				return new Vector2(1f, 1f);
			case TextAnchor.MiddleLeft:
				return new Vector2(0f, 0.5f);
			case TextAnchor.MiddleCenter:
				return new Vector2(0.5f, 0.5f);
			case TextAnchor.MiddleRight:
				return new Vector2(1f, 0.5f);
			case TextAnchor.LowerLeft:
				return new Vector2(0f, 0f);
			case TextAnchor.LowerCenter:
				return new Vector2(0.5f, 0f);
			case TextAnchor.LowerRight:
				return new Vector2(1f, 0f);
			default:
				return Vector2.zero;
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00015780 File Offset: 0x00013980
		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			if (this.font == null)
			{
				return;
			}
			this.m_DisableFontTextureRebuiltCallback = true;
			Vector2 size = base.rectTransform.rect.size;
			TextGenerationSettings generationSettings = this.GetGenerationSettings(size);
			this.cachedTextGenerator.PopulateWithErrors(this.text, generationSettings, base.gameObject);
			IList<UIVertex> verts = this.cachedTextGenerator.verts;
			float num = 1f / this.pixelsPerUnit;
			int count = verts.Count;
			if (count <= 0)
			{
				toFill.Clear();
				return;
			}
			Vector2 vector = new Vector2(verts[0].position.x, verts[0].position.y) * num;
			vector = base.PixelAdjustPoint(vector) - vector;
			toFill.Clear();
			if (vector != Vector2.zero)
			{
				for (int i = 0; i < count; i++)
				{
					int num2 = i & 3;
					this.m_TempVerts[num2] = verts[i];
					UIVertex[] tempVerts = this.m_TempVerts;
					int num3 = num2;
					tempVerts[num3].position = tempVerts[num3].position * num;
					UIVertex[] tempVerts2 = this.m_TempVerts;
					int num4 = num2;
					tempVerts2[num4].position.x = tempVerts2[num4].position.x + vector.x;
					UIVertex[] tempVerts3 = this.m_TempVerts;
					int num5 = num2;
					tempVerts3[num5].position.y = tempVerts3[num5].position.y + vector.y;
					if (num2 == 3)
					{
						toFill.AddUIVertexQuad(this.m_TempVerts);
					}
				}
			}
			else
			{
				for (int j = 0; j < count; j++)
				{
					int num6 = j & 3;
					this.m_TempVerts[num6] = verts[j];
					UIVertex[] tempVerts4 = this.m_TempVerts;
					int num7 = num6;
					tempVerts4[num7].position = tempVerts4[num7].position * num;
					if (num6 == 3)
					{
						toFill.AddUIVertexQuad(this.m_TempVerts);
					}
				}
			}
			this.m_DisableFontTextureRebuiltCallback = false;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00015970 File Offset: 0x00013B70
		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00015972 File Offset: 0x00013B72
		public virtual void CalculateLayoutInputVertical()
		{
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x00015974 File Offset: 0x00013B74
		public virtual float minWidth
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x0001597C File Offset: 0x00013B7C
		public virtual float preferredWidth
		{
			get
			{
				TextGenerationSettings generationSettings = this.GetGenerationSettings(Vector2.zero);
				return this.cachedTextGeneratorForLayout.GetPreferredWidth(this.m_Text, generationSettings) / this.pixelsPerUnit;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x000159AE File Offset: 0x00013BAE
		public virtual float flexibleWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x000159B5 File Offset: 0x00013BB5
		public virtual float minHeight
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x000159BC File Offset: 0x00013BBC
		public virtual float preferredHeight
		{
			get
			{
				TextGenerationSettings generationSettings = this.GetGenerationSettings(new Vector2(base.GetPixelAdjustedRect().size.x, 0f));
				return this.cachedTextGeneratorForLayout.GetPreferredHeight(this.m_Text, generationSettings) / this.pixelsPerUnit;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x00015A06 File Offset: 0x00013C06
		public virtual float flexibleHeight
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x00015A0D File Offset: 0x00013C0D
		public virtual int layoutPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0400016F RID: 367
		[SerializeField]
		private FontData m_FontData = FontData.defaultFontData;

		// Token: 0x04000170 RID: 368
		[TextArea(3, 10)]
		[SerializeField]
		protected string m_Text = string.Empty;

		// Token: 0x04000171 RID: 369
		private TextGenerator m_TextCache;

		// Token: 0x04000172 RID: 370
		private TextGenerator m_TextCacheForLayout;

		// Token: 0x04000173 RID: 371
		protected static Material s_DefaultText;

		// Token: 0x04000174 RID: 372
		[NonSerialized]
		protected bool m_DisableFontTextureRebuiltCallback;

		// Token: 0x04000175 RID: 373
		private readonly UIVertex[] m_TempVerts = new UIVertex[4];
	}
}
