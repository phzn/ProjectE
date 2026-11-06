using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Serialization;
using UnityEngine.Sprites;
using UnityEngine.U2D;

namespace UnityEngine.UI
{
	// Token: 0x02000016 RID: 22
	[RequireComponent(typeof(CanvasRenderer))]
	[AddComponentMenu("UI/Image", 11)]
	public class Image : MaskableGraphic, ISerializationCallbackReceiver, ILayoutElement, ICanvasRaycastFilter
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000110 RID: 272 RVA: 0x0000699C File Offset: 0x00004B9C
		// (set) Token: 0x06000111 RID: 273 RVA: 0x000069A4 File Offset: 0x00004BA4
		public Sprite sprite
		{
			get
			{
				return this.m_Sprite;
			}
			set
			{
				if (this.m_Sprite != null)
				{
					if (this.m_Sprite != value)
					{
						this.m_SkipLayoutUpdate = this.m_Sprite.rect.size.Equals(value ? value.rect.size : Vector2.zero);
						this.m_SkipMaterialUpdate = this.m_Sprite.texture == (value ? value.texture : null);
						this.m_Sprite = value;
						this.<set_sprite>g__ResetAlphaHitThresholdIfNeeded|11_0();
						this.SetAllDirty();
						this.TrackSprite();
						return;
					}
				}
				else if (value != null)
				{
					this.m_SkipLayoutUpdate = value.rect.size == Vector2.zero;
					this.m_SkipMaterialUpdate = value.texture == null;
					this.m_Sprite = value;
					this.<set_sprite>g__ResetAlphaHitThresholdIfNeeded|11_0();
					this.SetAllDirty();
					this.TrackSprite();
				}
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00006AA4 File Offset: 0x00004CA4
		public void DisableSpriteOptimizations()
		{
			this.m_SkipLayoutUpdate = false;
			this.m_SkipMaterialUpdate = false;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00006AB4 File Offset: 0x00004CB4
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00006ABC File Offset: 0x00004CBC
		public Sprite overrideSprite
		{
			get
			{
				return this.activeSprite;
			}
			set
			{
				if (SetPropertyUtility.SetClass<Sprite>(ref this.m_OverrideSprite, value))
				{
					this.SetAllDirty();
					this.TrackSprite();
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00006AD8 File Offset: 0x00004CD8
		private Sprite activeSprite
		{
			get
			{
				if (!(this.m_OverrideSprite != null))
				{
					return this.sprite;
				}
				return this.m_OverrideSprite;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00006AF5 File Offset: 0x00004CF5
		// (set) Token: 0x06000117 RID: 279 RVA: 0x00006AFD File Offset: 0x00004CFD
		public Image.Type type
		{
			get
			{
				return this.m_Type;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<Image.Type>(ref this.m_Type, value))
				{
					this.SetVerticesDirty();
				}
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00006B13 File Offset: 0x00004D13
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00006B1B File Offset: 0x00004D1B
		public bool preserveAspect
		{
			get
			{
				return this.m_PreserveAspect;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<bool>(ref this.m_PreserveAspect, value))
				{
					this.SetVerticesDirty();
				}
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00006B31 File Offset: 0x00004D31
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00006B39 File Offset: 0x00004D39
		public bool fillCenter
		{
			get
			{
				return this.m_FillCenter;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<bool>(ref this.m_FillCenter, value))
				{
					this.SetVerticesDirty();
				}
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00006B4F File Offset: 0x00004D4F
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00006B57 File Offset: 0x00004D57
		public Image.FillMethod fillMethod
		{
			get
			{
				return this.m_FillMethod;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<Image.FillMethod>(ref this.m_FillMethod, value))
				{
					this.SetVerticesDirty();
					this.m_FillOrigin = 0;
				}
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00006B74 File Offset: 0x00004D74
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00006B7C File Offset: 0x00004D7C
		public float fillAmount
		{
			get
			{
				return this.m_FillAmount;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_FillAmount, Mathf.Clamp01(value)))
				{
					this.SetVerticesDirty();
				}
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00006B97 File Offset: 0x00004D97
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00006B9F File Offset: 0x00004D9F
		public bool fillClockwise
		{
			get
			{
				return this.m_FillClockwise;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<bool>(ref this.m_FillClockwise, value))
				{
					this.SetVerticesDirty();
				}
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00006BB5 File Offset: 0x00004DB5
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00006BBD File Offset: 0x00004DBD
		public int fillOrigin
		{
			get
			{
				return this.m_FillOrigin;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<int>(ref this.m_FillOrigin, value))
				{
					this.SetVerticesDirty();
				}
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00006BD3 File Offset: 0x00004DD3
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00006BE1 File Offset: 0x00004DE1
		[Obsolete("eventAlphaThreshold has been deprecated. Use eventMinimumAlphaThreshold instead (UnityUpgradable) -> alphaHitTestMinimumThreshold")]
		public float eventAlphaThreshold
		{
			get
			{
				return 1f - this.alphaHitTestMinimumThreshold;
			}
			set
			{
				this.alphaHitTestMinimumThreshold = 1f - value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00006BF0 File Offset: 0x00004DF0
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00006BF8 File Offset: 0x00004DF8
		public float alphaHitTestMinimumThreshold
		{
			get
			{
				return this.m_AlphaHitTestMinimumThreshold;
			}
			set
			{
				if (this.sprite != null && (GraphicsFormatUtility.IsCrunchFormat(this.sprite.texture.format) || !this.sprite.texture.isReadable))
				{
					throw new InvalidOperationException("alphaHitTestMinimumThreshold should not be modified on a texture not readeable or not using Crunch Compression.");
				}
				this.m_AlphaHitTestMinimumThreshold = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00006C4E File Offset: 0x00004E4E
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00006C56 File Offset: 0x00004E56
		public bool useSpriteMesh
		{
			get
			{
				return this.m_UseSpriteMesh;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<bool>(ref this.m_UseSpriteMesh, value))
				{
					this.SetVerticesDirty();
				}
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006C6C File Offset: 0x00004E6C
		protected Image()
		{
			base.useLegacyMeshGeneration = false;
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00006CBC File Offset: 0x00004EBC
		public static Material defaultETC1GraphicMaterial
		{
			get
			{
				if (Image.s_ETC1DefaultUI == null)
				{
					Image.s_ETC1DefaultUI = Canvas.GetETC1SupportedCanvasMaterial();
				}
				return Image.s_ETC1DefaultUI;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00006CDC File Offset: 0x00004EDC
		public override Texture mainTexture
		{
			get
			{
				if (!(this.activeSprite == null))
				{
					return this.activeSprite.texture;
				}
				if (this.material != null && this.material.mainTexture != null)
				{
					return this.material.mainTexture;
				}
				return Graphic.s_WhiteTexture;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00006D38 File Offset: 0x00004F38
		public bool hasBorder
		{
			get
			{
				return this.activeSprite != null && this.activeSprite.border.sqrMagnitude > 0f;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00006D6F File Offset: 0x00004F6F
		// (set) Token: 0x0600012F RID: 303 RVA: 0x00006D77 File Offset: 0x00004F77
		public float pixelsPerUnitMultiplier
		{
			get
			{
				return this.m_PixelsPerUnitMultiplier;
			}
			set
			{
				this.m_PixelsPerUnitMultiplier = Mathf.Max(0.01f, value);
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00006D90 File Offset: 0x00004F90
		public float pixelsPerUnit
		{
			get
			{
				float num = 100f;
				if (this.activeSprite)
				{
					num = this.activeSprite.pixelsPerUnit;
				}
				if (base.canvas)
				{
					this.m_CachedReferencePixelsPerUnit = base.canvas.referencePixelsPerUnit;
				}
				return num / this.m_CachedReferencePixelsPerUnit;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00006DE2 File Offset: 0x00004FE2
		protected float multipliedPixelsPerUnit
		{
			get
			{
				return this.pixelsPerUnit * this.m_PixelsPerUnitMultiplier;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00006DF4 File Offset: 0x00004FF4
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00006E42 File Offset: 0x00005042
		public override Material material
		{
			get
			{
				if (this.m_Material != null)
				{
					return this.m_Material;
				}
				if (this.activeSprite && this.activeSprite.associatedAlphaSplitTexture != null)
				{
					return Image.defaultETC1GraphicMaterial;
				}
				return this.defaultMaterial;
			}
			set
			{
				base.material = value;
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006E4B File Offset: 0x0000504B
		public virtual void OnBeforeSerialize()
		{
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006E50 File Offset: 0x00005050
		public virtual void OnAfterDeserialize()
		{
			if (this.m_FillOrigin < 0)
			{
				this.m_FillOrigin = 0;
			}
			else if (this.m_FillMethod == Image.FillMethod.Horizontal && this.m_FillOrigin > 1)
			{
				this.m_FillOrigin = 0;
			}
			else if (this.m_FillMethod == Image.FillMethod.Vertical && this.m_FillOrigin > 1)
			{
				this.m_FillOrigin = 0;
			}
			else if (this.m_FillOrigin > 3)
			{
				this.m_FillOrigin = 0;
			}
			this.m_FillAmount = Mathf.Clamp(this.m_FillAmount, 0f, 1f);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006ED0 File Offset: 0x000050D0
		private void PreserveSpriteAspectRatio(ref Rect rect, Vector2 spriteSize)
		{
			float num = spriteSize.x / spriteSize.y;
			float num2 = rect.width / rect.height;
			if (num > num2)
			{
				float height = rect.height;
				rect.height = rect.width * (1f / num);
				rect.y += (height - rect.height) * base.rectTransform.pivot.y;
				return;
			}
			float width = rect.width;
			rect.width = rect.height * num;
			rect.x += (width - rect.width) * base.rectTransform.pivot.x;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006F7C File Offset: 0x0000517C
		private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
		{
			Vector4 vector = ((this.activeSprite == null) ? Vector4.zero : DataUtility.GetPadding(this.activeSprite));
			Vector2 vector2 = ((this.activeSprite == null) ? Vector2.zero : new Vector2(this.activeSprite.rect.width, this.activeSprite.rect.height));
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			int num = Mathf.RoundToInt(vector2.x);
			int num2 = Mathf.RoundToInt(vector2.y);
			Vector4 vector3 = new Vector4(vector.x / (float)num, vector.y / (float)num2, ((float)num - vector.z) / (float)num, ((float)num2 - vector.w) / (float)num2);
			if (shouldPreserveAspect && vector2.sqrMagnitude > 0f)
			{
				this.PreserveSpriteAspectRatio(ref pixelAdjustedRect, vector2);
			}
			vector3 = new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * vector3.x, pixelAdjustedRect.y + pixelAdjustedRect.height * vector3.y, pixelAdjustedRect.x + pixelAdjustedRect.width * vector3.z, pixelAdjustedRect.y + pixelAdjustedRect.height * vector3.w);
			return vector3;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000070C0 File Offset: 0x000052C0
		public override void SetNativeSize()
		{
			if (this.activeSprite != null)
			{
				float num = this.activeSprite.rect.width / this.pixelsPerUnit;
				float num2 = this.activeSprite.rect.height / this.pixelsPerUnit;
				base.rectTransform.anchorMax = base.rectTransform.anchorMin;
				base.rectTransform.sizeDelta = new Vector2(num, num2);
				this.SetAllDirty();
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00007140 File Offset: 0x00005340
		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			if (this.activeSprite == null)
			{
				base.OnPopulateMesh(toFill);
				return;
			}
			switch (this.type)
			{
			case Image.Type.Simple:
				if (!this.useSpriteMesh)
				{
					this.GenerateSimpleSprite(toFill, this.m_PreserveAspect);
					return;
				}
				this.GenerateSprite(toFill, this.m_PreserveAspect);
				return;
			case Image.Type.Sliced:
				this.GenerateSlicedSprite(toFill);
				return;
			case Image.Type.Tiled:
				this.GenerateTiledSprite(toFill);
				return;
			case Image.Type.Filled:
				this.GenerateFilledSprite(toFill, this.m_PreserveAspect);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000071C2 File Offset: 0x000053C2
		private void TrackSprite()
		{
			if (this.activeSprite != null && this.activeSprite.texture == null)
			{
				Image.TrackImage(this);
				this.m_Tracked = true;
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000071F2 File Offset: 0x000053F2
		protected override void OnEnable()
		{
			base.OnEnable();
			this.TrackSprite();
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00007200 File Offset: 0x00005400
		protected override void OnDisable()
		{
			base.OnDisable();
			if (this.m_Tracked)
			{
				Image.UnTrackImage(this);
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00007218 File Offset: 0x00005418
		protected override void UpdateMaterial()
		{
			base.UpdateMaterial();
			if (this.activeSprite == null)
			{
				base.canvasRenderer.SetAlphaTexture(null);
				return;
			}
			Texture2D associatedAlphaSplitTexture = this.activeSprite.associatedAlphaSplitTexture;
			if (associatedAlphaSplitTexture != null)
			{
				base.canvasRenderer.SetAlphaTexture(associatedAlphaSplitTexture);
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00007268 File Offset: 0x00005468
		protected override void OnCanvasHierarchyChanged()
		{
			base.OnCanvasHierarchyChanged();
			if (base.canvas == null)
			{
				this.m_CachedReferencePixelsPerUnit = 100f;
				return;
			}
			if (base.canvas.referencePixelsPerUnit != this.m_CachedReferencePixelsPerUnit)
			{
				this.m_CachedReferencePixelsPerUnit = base.canvas.referencePixelsPerUnit;
				if (this.type == Image.Type.Sliced || this.type == Image.Type.Tiled)
				{
					this.SetVerticesDirty();
					this.SetLayoutDirty();
				}
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000072D8 File Offset: 0x000054D8
		private void GenerateSimpleSprite(VertexHelper vh, bool lPreserveAspect)
		{
			Vector4 drawingDimensions = this.GetDrawingDimensions(lPreserveAspect);
			Vector4 vector = ((this.activeSprite != null) ? DataUtility.GetOuterUV(this.activeSprite) : Vector4.zero);
			Color color = this.color;
			vh.Clear();
			vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.y), color, new Vector2(vector.x, vector.y));
			vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.w), color, new Vector2(vector.x, vector.w));
			vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.w), color, new Vector2(vector.z, vector.w));
			vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.y), color, new Vector2(vector.z, vector.y));
			vh.AddTriangle(0, 1, 2);
			vh.AddTriangle(2, 3, 0);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000073FC File Offset: 0x000055FC
		private void GenerateSprite(VertexHelper vh, bool lPreserveAspect)
		{
			Vector2 vector = new Vector2(this.activeSprite.rect.width, this.activeSprite.rect.height);
			Vector2 vector2 = this.activeSprite.pivot / vector;
			Vector2 pivot = base.rectTransform.pivot;
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			if (lPreserveAspect & (vector.sqrMagnitude > 0f))
			{
				this.PreserveSpriteAspectRatio(ref pixelAdjustedRect, vector);
			}
			Vector2 vector3 = new Vector2(pixelAdjustedRect.width, pixelAdjustedRect.height);
			Vector3 size = this.activeSprite.bounds.size;
			Vector2 vector4 = (pivot - vector2) * vector3;
			Color color = this.color;
			vh.Clear();
			Vector2[] vertices = this.activeSprite.vertices;
			Vector2[] uv = this.activeSprite.uv;
			for (int i = 0; i < vertices.Length; i++)
			{
				vh.AddVert(new Vector3(vertices[i].x / size.x * vector3.x - vector4.x, vertices[i].y / size.y * vector3.y - vector4.y), color, new Vector2(uv[i].x, uv[i].y));
			}
			ushort[] triangles = this.activeSprite.triangles;
			for (int j = 0; j < triangles.Length; j += 3)
			{
				vh.AddTriangle((int)triangles[j], (int)triangles[j + 1], (int)triangles[j + 2]);
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000075AC File Offset: 0x000057AC
		private void GenerateSlicedSprite(VertexHelper toFill)
		{
			if (!this.hasBorder)
			{
				this.GenerateSimpleSprite(toFill, false);
				return;
			}
			Vector4 vector;
			Vector4 vector2;
			Vector4 vector3;
			Vector4 vector4;
			if (this.activeSprite != null)
			{
				vector = DataUtility.GetOuterUV(this.activeSprite);
				vector2 = DataUtility.GetInnerUV(this.activeSprite);
				vector3 = DataUtility.GetPadding(this.activeSprite);
				vector4 = this.activeSprite.border;
			}
			else
			{
				vector = Vector4.zero;
				vector2 = Vector4.zero;
				vector3 = Vector4.zero;
				vector4 = Vector4.zero;
			}
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			Vector4 adjustedBorders = this.GetAdjustedBorders(vector4 / this.multipliedPixelsPerUnit, pixelAdjustedRect);
			vector3 /= this.multipliedPixelsPerUnit;
			Image.s_VertScratch[0] = new Vector2(vector3.x, vector3.y);
			Image.s_VertScratch[3] = new Vector2(pixelAdjustedRect.width - vector3.z, pixelAdjustedRect.height - vector3.w);
			Image.s_VertScratch[1].x = adjustedBorders.x;
			Image.s_VertScratch[1].y = adjustedBorders.y;
			Image.s_VertScratch[2].x = pixelAdjustedRect.width - adjustedBorders.z;
			Image.s_VertScratch[2].y = pixelAdjustedRect.height - adjustedBorders.w;
			for (int i = 0; i < 4; i++)
			{
				Vector2[] array = Image.s_VertScratch;
				int num = i;
				array[num].x = array[num].x + pixelAdjustedRect.x;
				Vector2[] array2 = Image.s_VertScratch;
				int num2 = i;
				array2[num2].y = array2[num2].y + pixelAdjustedRect.y;
			}
			Image.s_UVScratch[0] = new Vector2(vector.x, vector.y);
			Image.s_UVScratch[1] = new Vector2(vector2.x, vector2.y);
			Image.s_UVScratch[2] = new Vector2(vector2.z, vector2.w);
			Image.s_UVScratch[3] = new Vector2(vector.z, vector.w);
			toFill.Clear();
			for (int j = 0; j < 3; j++)
			{
				int num3 = j + 1;
				for (int k = 0; k < 3; k++)
				{
					if (this.m_FillCenter || j != 1 || k != 1)
					{
						int num4 = k + 1;
						if (Image.s_VertScratch[num3].x - Image.s_VertScratch[j].x > 0f && Image.s_VertScratch[num4].y - Image.s_VertScratch[k].y > 0f)
						{
							Image.AddQuad(toFill, new Vector2(Image.s_VertScratch[j].x, Image.s_VertScratch[k].y), new Vector2(Image.s_VertScratch[num3].x, Image.s_VertScratch[num4].y), this.color, new Vector2(Image.s_UVScratch[j].x, Image.s_UVScratch[k].y), new Vector2(Image.s_UVScratch[num3].x, Image.s_UVScratch[num4].y));
						}
					}
				}
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00007914 File Offset: 0x00005B14
		private void GenerateTiledSprite(VertexHelper toFill)
		{
			Vector4 vector;
			Vector4 vector2;
			Vector4 vector3;
			Vector2 vector4;
			if (this.activeSprite != null)
			{
				vector = DataUtility.GetOuterUV(this.activeSprite);
				vector2 = DataUtility.GetInnerUV(this.activeSprite);
				vector3 = this.activeSprite.border;
				vector4 = this.activeSprite.rect.size;
			}
			else
			{
				vector = Vector4.zero;
				vector2 = Vector4.zero;
				vector3 = Vector4.zero;
				vector4 = Vector2.one * 100f;
			}
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			float num = (vector4.x - vector3.x - vector3.z) / this.multipliedPixelsPerUnit;
			float num2 = (vector4.y - vector3.y - vector3.w) / this.multipliedPixelsPerUnit;
			vector3 = this.GetAdjustedBorders(vector3 / this.multipliedPixelsPerUnit, pixelAdjustedRect);
			Vector2 vector5 = new Vector2(vector2.x, vector2.y);
			Vector2 vector6 = new Vector2(vector2.z, vector2.w);
			float x = vector3.x;
			float num3 = pixelAdjustedRect.width - vector3.z;
			float y = vector3.y;
			float num4 = pixelAdjustedRect.height - vector3.w;
			toFill.Clear();
			Vector2 vector7 = vector6;
			if (num <= 0f)
			{
				num = num3 - x;
			}
			if (num2 <= 0f)
			{
				num2 = num4 - y;
			}
			if (this.activeSprite != null && (this.hasBorder || this.activeSprite.packed || (this.activeSprite.texture != null && this.activeSprite.texture.wrapMode != TextureWrapMode.Repeat)))
			{
				long num5;
				long num6;
				if (this.m_FillCenter)
				{
					num5 = (long)Math.Ceiling((double)((num3 - x) / num));
					num6 = (long)Math.Ceiling((double)((num4 - y) / num2));
					double num7;
					if (this.hasBorder)
					{
						num7 = ((double)num5 + 2.0) * ((double)num6 + 2.0) * 4.0;
					}
					else
					{
						num7 = (double)(num5 * num6) * 4.0;
					}
					if (num7 > 65000.0)
					{
						Debug.LogError("Too many sprite tiles on Image \"" + base.name + "\". The tile size will be increased. To remove the limit on the number of tiles, set the Wrap mode to Repeat in the Image Import Settings", this);
						double num8 = 16250.0;
						double num9;
						if (this.hasBorder)
						{
							num9 = ((double)num5 + 2.0) / ((double)num6 + 2.0);
						}
						else
						{
							num9 = (double)num5 / (double)num6;
						}
						double num10 = Math.Sqrt(num8 / num9);
						double num11 = num10 * num9;
						if (this.hasBorder)
						{
							num10 -= 2.0;
							num11 -= 2.0;
						}
						num5 = (long)Math.Floor(num10);
						num6 = (long)Math.Floor(num11);
						num = (num3 - x) / (float)num5;
						num2 = (num4 - y) / (float)num6;
					}
				}
				else if (this.hasBorder)
				{
					num5 = (long)Math.Ceiling((double)((num3 - x) / num));
					num6 = (long)Math.Ceiling((double)((num4 - y) / num2));
					if (((double)(num6 + num5) + 2.0) * 2.0 * 4.0 > 65000.0)
					{
						Debug.LogError("Too many sprite tiles on Image \"" + base.name + "\". The tile size will be increased. To remove the limit on the number of tiles, set the Wrap mode to Repeat in the Image Import Settings", this);
						double num12 = 16250.0;
						double num13 = (double)num5 / (double)num6;
						double num14 = (num12 - 4.0) / (2.0 * (1.0 + num13));
						double num15 = num14 * num13;
						num5 = (long)Math.Floor(num14);
						num6 = (long)Math.Floor(num15);
						num = (num3 - x) / (float)num5;
						num2 = (num4 - y) / (float)num6;
					}
				}
				else
				{
					num5 = (num6 = 0L);
				}
				if (this.m_FillCenter)
				{
					for (long num16 = 0L; num16 < num6; num16 += 1L)
					{
						float num17 = y + (float)num16 * num2;
						float num18 = y + (float)(num16 + 1L) * num2;
						if (num18 > num4)
						{
							vector7.y = vector5.y + (vector6.y - vector5.y) * (num4 - num17) / (num18 - num17);
							num18 = num4;
						}
						vector7.x = vector6.x;
						for (long num19 = 0L; num19 < num5; num19 += 1L)
						{
							float num20 = x + (float)num19 * num;
							float num21 = x + (float)(num19 + 1L) * num;
							if (num21 > num3)
							{
								vector7.x = vector5.x + (vector6.x - vector5.x) * (num3 - num20) / (num21 - num20);
								num21 = num3;
							}
							Image.AddQuad(toFill, new Vector2(num20, num17) + pixelAdjustedRect.position, new Vector2(num21, num18) + pixelAdjustedRect.position, this.color, vector5, vector7);
						}
					}
				}
				if (this.hasBorder)
				{
					vector7 = vector6;
					for (long num22 = 0L; num22 < num6; num22 += 1L)
					{
						float num23 = y + (float)num22 * num2;
						float num24 = y + (float)(num22 + 1L) * num2;
						if (num24 > num4)
						{
							vector7.y = vector5.y + (vector6.y - vector5.y) * (num4 - num23) / (num24 - num23);
							num24 = num4;
						}
						Image.AddQuad(toFill, new Vector2(0f, num23) + pixelAdjustedRect.position, new Vector2(x, num24) + pixelAdjustedRect.position, this.color, new Vector2(vector.x, vector5.y), new Vector2(vector5.x, vector7.y));
						Image.AddQuad(toFill, new Vector2(num3, num23) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, num24) + pixelAdjustedRect.position, this.color, new Vector2(vector6.x, vector5.y), new Vector2(vector.z, vector7.y));
					}
					vector7 = vector6;
					for (long num25 = 0L; num25 < num5; num25 += 1L)
					{
						float num26 = x + (float)num25 * num;
						float num27 = x + (float)(num25 + 1L) * num;
						if (num27 > num3)
						{
							vector7.x = vector5.x + (vector6.x - vector5.x) * (num3 - num26) / (num27 - num26);
							num27 = num3;
						}
						Image.AddQuad(toFill, new Vector2(num26, 0f) + pixelAdjustedRect.position, new Vector2(num27, y) + pixelAdjustedRect.position, this.color, new Vector2(vector5.x, vector.y), new Vector2(vector7.x, vector5.y));
						Image.AddQuad(toFill, new Vector2(num26, num4) + pixelAdjustedRect.position, new Vector2(num27, pixelAdjustedRect.height) + pixelAdjustedRect.position, this.color, new Vector2(vector5.x, vector6.y), new Vector2(vector7.x, vector.w));
					}
					Image.AddQuad(toFill, new Vector2(0f, 0f) + pixelAdjustedRect.position, new Vector2(x, y) + pixelAdjustedRect.position, this.color, new Vector2(vector.x, vector.y), new Vector2(vector5.x, vector5.y));
					Image.AddQuad(toFill, new Vector2(num3, 0f) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, y) + pixelAdjustedRect.position, this.color, new Vector2(vector6.x, vector.y), new Vector2(vector.z, vector5.y));
					Image.AddQuad(toFill, new Vector2(0f, num4) + pixelAdjustedRect.position, new Vector2(x, pixelAdjustedRect.height) + pixelAdjustedRect.position, this.color, new Vector2(vector.x, vector6.y), new Vector2(vector5.x, vector.w));
					Image.AddQuad(toFill, new Vector2(num3, num4) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, pixelAdjustedRect.height) + pixelAdjustedRect.position, this.color, new Vector2(vector6.x, vector6.y), new Vector2(vector.z, vector.w));
					return;
				}
			}
			else
			{
				Vector2 vector8 = new Vector2((num3 - x) / num, (num4 - y) / num2);
				if (this.m_FillCenter)
				{
					Image.AddQuad(toFill, new Vector2(x, y) + pixelAdjustedRect.position, new Vector2(num3, num4) + pixelAdjustedRect.position, this.color, Vector2.Scale(vector5, vector8), Vector2.Scale(vector6, vector8));
				}
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00008280 File Offset: 0x00006480
		private static void AddQuad(VertexHelper vertexHelper, Vector3[] quadPositions, Color32 color, Vector3[] quadUVs)
		{
			int currentVertCount = vertexHelper.currentVertCount;
			for (int i = 0; i < 4; i++)
			{
				vertexHelper.AddVert(quadPositions[i], color, quadUVs[i]);
			}
			vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000082D4 File Offset: 0x000064D4
		private static void AddQuad(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color, Vector2 uvMin, Vector2 uvMax)
		{
			int currentVertCount = vertexHelper.currentVertCount;
			vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0f), color, new Vector2(uvMin.x, uvMin.y));
			vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0f), color, new Vector2(uvMin.x, uvMax.y));
			vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0f), color, new Vector2(uvMax.x, uvMax.y));
			vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0f), color, new Vector2(uvMax.x, uvMin.y));
			vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000083D8 File Offset: 0x000065D8
		private Vector4 GetAdjustedBorders(Vector4 border, Rect adjustedRect)
		{
			Rect rect = base.rectTransform.rect;
			for (int i = 0; i <= 1; i++)
			{
				if (rect.size[i] != 0f)
				{
					float num = adjustedRect.size[i] / rect.size[i];
					ref Vector4 ptr = ref border;
					int num2 = i;
					ptr[num2] *= num;
					ptr = ref border;
					num2 = i + 2;
					ptr[num2] *= num;
				}
				float num3 = border[i] + border[i + 2];
				if (adjustedRect.size[i] < num3 && num3 != 0f)
				{
					float num = adjustedRect.size[i] / num3;
					ref Vector4 ptr = ref border;
					int num2 = i;
					ptr[num2] *= num;
					ptr = ref border;
					num2 = i + 2;
					ptr[num2] *= num;
				}
			}
			return border;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000084F4 File Offset: 0x000066F4
		private void GenerateFilledSprite(VertexHelper toFill, bool preserveAspect)
		{
			toFill.Clear();
			if (this.m_FillAmount < 0.001f)
			{
				return;
			}
			Vector4 drawingDimensions = this.GetDrawingDimensions(preserveAspect);
			object obj = ((this.activeSprite != null) ? DataUtility.GetOuterUV(this.activeSprite) : Vector4.zero);
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.color = this.color;
			object obj2 = obj;
			float num = obj2.x;
			float num2 = obj2.y;
			float num3 = obj2.z;
			float num4 = obj2.w;
			if (this.m_FillMethod == Image.FillMethod.Horizontal || this.m_FillMethod == Image.FillMethod.Vertical)
			{
				if (this.fillMethod == Image.FillMethod.Horizontal)
				{
					float num5 = (num3 - num) * this.m_FillAmount;
					if (this.m_FillOrigin == 1)
					{
						drawingDimensions.x = drawingDimensions.z - (drawingDimensions.z - drawingDimensions.x) * this.m_FillAmount;
						num = num3 - num5;
					}
					else
					{
						drawingDimensions.z = drawingDimensions.x + (drawingDimensions.z - drawingDimensions.x) * this.m_FillAmount;
						num3 = num + num5;
					}
				}
				else if (this.fillMethod == Image.FillMethod.Vertical)
				{
					float num6 = (num4 - num2) * this.m_FillAmount;
					if (this.m_FillOrigin == 1)
					{
						drawingDimensions.y = drawingDimensions.w - (drawingDimensions.w - drawingDimensions.y) * this.m_FillAmount;
						num2 = num4 - num6;
					}
					else
					{
						drawingDimensions.w = drawingDimensions.y + (drawingDimensions.w - drawingDimensions.y) * this.m_FillAmount;
						num4 = num2 + num6;
					}
				}
			}
			Image.s_Xy[0] = new Vector2(drawingDimensions.x, drawingDimensions.y);
			Image.s_Xy[1] = new Vector2(drawingDimensions.x, drawingDimensions.w);
			Image.s_Xy[2] = new Vector2(drawingDimensions.z, drawingDimensions.w);
			Image.s_Xy[3] = new Vector2(drawingDimensions.z, drawingDimensions.y);
			Image.s_Uv[0] = new Vector2(num, num2);
			Image.s_Uv[1] = new Vector2(num, num4);
			Image.s_Uv[2] = new Vector2(num3, num4);
			Image.s_Uv[3] = new Vector2(num3, num2);
			if (this.m_FillAmount < 1f && this.m_FillMethod != Image.FillMethod.Horizontal && this.m_FillMethod != Image.FillMethod.Vertical)
			{
				if (this.fillMethod == Image.FillMethod.Radial90)
				{
					if (Image.RadialCut(Image.s_Xy, Image.s_Uv, this.m_FillAmount, this.m_FillClockwise, this.m_FillOrigin))
					{
						Image.AddQuad(toFill, Image.s_Xy, this.color, Image.s_Uv);
						return;
					}
				}
				else
				{
					if (this.fillMethod == Image.FillMethod.Radial180)
					{
						for (int i = 0; i < 2; i++)
						{
							int num7 = ((this.m_FillOrigin > 1) ? 1 : 0);
							float num8;
							float num9;
							float num10;
							float num11;
							if (this.m_FillOrigin == 0 || this.m_FillOrigin == 2)
							{
								num8 = 0f;
								num9 = 1f;
								if (i == num7)
								{
									num10 = 0f;
									num11 = 0.5f;
								}
								else
								{
									num10 = 0.5f;
									num11 = 1f;
								}
							}
							else
							{
								num10 = 0f;
								num11 = 1f;
								if (i == num7)
								{
									num8 = 0.5f;
									num9 = 1f;
								}
								else
								{
									num8 = 0f;
									num9 = 0.5f;
								}
							}
							Image.s_Xy[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num10);
							Image.s_Xy[1].x = Image.s_Xy[0].x;
							Image.s_Xy[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num11);
							Image.s_Xy[3].x = Image.s_Xy[2].x;
							Image.s_Xy[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num8);
							Image.s_Xy[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num9);
							Image.s_Xy[2].y = Image.s_Xy[1].y;
							Image.s_Xy[3].y = Image.s_Xy[0].y;
							Image.s_Uv[0].x = Mathf.Lerp(num, num3, num10);
							Image.s_Uv[1].x = Image.s_Uv[0].x;
							Image.s_Uv[2].x = Mathf.Lerp(num, num3, num11);
							Image.s_Uv[3].x = Image.s_Uv[2].x;
							Image.s_Uv[0].y = Mathf.Lerp(num2, num4, num8);
							Image.s_Uv[1].y = Mathf.Lerp(num2, num4, num9);
							Image.s_Uv[2].y = Image.s_Uv[1].y;
							Image.s_Uv[3].y = Image.s_Uv[0].y;
							float num12 = (this.m_FillClockwise ? (this.fillAmount * 2f - (float)i) : (this.m_FillAmount * 2f - (float)(1 - i)));
							if (Image.RadialCut(Image.s_Xy, Image.s_Uv, Mathf.Clamp01(num12), this.m_FillClockwise, (i + this.m_FillOrigin + 3) % 4))
							{
								Image.AddQuad(toFill, Image.s_Xy, this.color, Image.s_Uv);
							}
						}
						return;
					}
					if (this.fillMethod == Image.FillMethod.Radial360)
					{
						for (int j = 0; j < 4; j++)
						{
							float num13;
							float num14;
							if (j < 2)
							{
								num13 = 0f;
								num14 = 0.5f;
							}
							else
							{
								num13 = 0.5f;
								num14 = 1f;
							}
							float num15;
							float num16;
							if (j == 0 || j == 3)
							{
								num15 = 0f;
								num16 = 0.5f;
							}
							else
							{
								num15 = 0.5f;
								num16 = 1f;
							}
							Image.s_Xy[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num13);
							Image.s_Xy[1].x = Image.s_Xy[0].x;
							Image.s_Xy[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num14);
							Image.s_Xy[3].x = Image.s_Xy[2].x;
							Image.s_Xy[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num15);
							Image.s_Xy[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num16);
							Image.s_Xy[2].y = Image.s_Xy[1].y;
							Image.s_Xy[3].y = Image.s_Xy[0].y;
							Image.s_Uv[0].x = Mathf.Lerp(num, num3, num13);
							Image.s_Uv[1].x = Image.s_Uv[0].x;
							Image.s_Uv[2].x = Mathf.Lerp(num, num3, num14);
							Image.s_Uv[3].x = Image.s_Uv[2].x;
							Image.s_Uv[0].y = Mathf.Lerp(num2, num4, num15);
							Image.s_Uv[1].y = Mathf.Lerp(num2, num4, num16);
							Image.s_Uv[2].y = Image.s_Uv[1].y;
							Image.s_Uv[3].y = Image.s_Uv[0].y;
							float num17 = (this.m_FillClockwise ? (this.m_FillAmount * 4f - (float)((j + this.m_FillOrigin) % 4)) : (this.m_FillAmount * 4f - (float)(3 - (j + this.m_FillOrigin) % 4)));
							if (Image.RadialCut(Image.s_Xy, Image.s_Uv, Mathf.Clamp01(num17), this.m_FillClockwise, (j + 2) % 4))
							{
								Image.AddQuad(toFill, Image.s_Xy, this.color, Image.s_Uv);
							}
						}
						return;
					}
				}
			}
			else
			{
				Image.AddQuad(toFill, Image.s_Xy, this.color, Image.s_Uv);
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00008DD0 File Offset: 0x00006FD0
		private static bool RadialCut(Vector3[] xy, Vector3[] uv, float fill, bool invert, int corner)
		{
			if (fill < 0.001f)
			{
				return false;
			}
			if ((corner & 1) == 1)
			{
				invert = !invert;
			}
			if (!invert && fill > 0.999f)
			{
				return true;
			}
			float num = Mathf.Clamp01(fill);
			if (invert)
			{
				num = 1f - num;
			}
			num *= 1.5707964f;
			float num2 = Mathf.Cos(num);
			float num3 = Mathf.Sin(num);
			Image.RadialCut(xy, num2, num3, invert, corner);
			Image.RadialCut(uv, num2, num3, invert, corner);
			return true;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00008E40 File Offset: 0x00007040
		private static void RadialCut(Vector3[] xy, float cos, float sin, bool invert, int corner)
		{
			int num = (corner + 1) % 4;
			int num2 = (corner + 2) % 4;
			int num3 = (corner + 3) % 4;
			if ((corner & 1) == 1)
			{
				if (sin > cos)
				{
					cos /= sin;
					sin = 1f;
					if (invert)
					{
						xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
						xy[num2].x = xy[num].x;
					}
				}
				else if (cos > sin)
				{
					sin /= cos;
					cos = 1f;
					if (!invert)
					{
						xy[num2].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
						xy[num3].y = xy[num2].y;
					}
				}
				else
				{
					cos = 1f;
					sin = 1f;
				}
				if (!invert)
				{
					xy[num3].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
					return;
				}
				xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
				return;
			}
			else
			{
				if (cos > sin)
				{
					sin /= cos;
					cos = 1f;
					if (!invert)
					{
						xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
						xy[num2].y = xy[num].y;
					}
				}
				else if (sin > cos)
				{
					cos /= sin;
					sin = 1f;
					if (invert)
					{
						xy[num2].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
						xy[num3].x = xy[num2].x;
					}
				}
				else
				{
					cos = 1f;
					sin = 1f;
				}
				if (invert)
				{
					xy[num3].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
					return;
				}
				xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
				return;
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000909E File Offset: 0x0000729E
		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000090A0 File Offset: 0x000072A0
		public virtual void CalculateLayoutInputVertical()
		{
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600014B RID: 331 RVA: 0x000090A2 File Offset: 0x000072A2
		public virtual float minWidth
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600014C RID: 332 RVA: 0x000090AC File Offset: 0x000072AC
		public virtual float preferredWidth
		{
			get
			{
				if (this.activeSprite == null)
				{
					return 0f;
				}
				if (this.type == Image.Type.Sliced || this.type == Image.Type.Tiled)
				{
					return DataUtility.GetMinSize(this.activeSprite).x / this.pixelsPerUnit;
				}
				return this.activeSprite.rect.size.x / this.pixelsPerUnit;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00009116 File Offset: 0x00007316
		public virtual float flexibleWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600014E RID: 334 RVA: 0x0000911D File Offset: 0x0000731D
		public virtual float minHeight
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00009124 File Offset: 0x00007324
		public virtual float preferredHeight
		{
			get
			{
				if (this.activeSprite == null)
				{
					return 0f;
				}
				if (this.type == Image.Type.Sliced || this.type == Image.Type.Tiled)
				{
					return DataUtility.GetMinSize(this.activeSprite).y / this.pixelsPerUnit;
				}
				return this.activeSprite.rect.size.y / this.pixelsPerUnit;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000150 RID: 336 RVA: 0x0000918E File Offset: 0x0000738E
		public virtual float flexibleHeight
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00009195 File Offset: 0x00007395
		public virtual int layoutPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00009198 File Offset: 0x00007398
		public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			if (this.alphaHitTestMinimumThreshold <= 0f)
			{
				return true;
			}
			if (this.alphaHitTestMinimumThreshold > 1f)
			{
				return false;
			}
			if (this.activeSprite == null)
			{
				return true;
			}
			Vector2 vector;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, screenPoint, eventCamera, out vector))
			{
				return false;
			}
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			if (this.m_PreserveAspect)
			{
				this.PreserveSpriteAspectRatio(ref pixelAdjustedRect, new Vector2((float)this.activeSprite.texture.width, (float)this.activeSprite.texture.height));
			}
			vector.x += base.rectTransform.pivot.x * pixelAdjustedRect.width;
			vector.y += base.rectTransform.pivot.y * pixelAdjustedRect.height;
			vector = this.MapCoordinate(vector, pixelAdjustedRect);
			float num = vector.x / (float)this.activeSprite.texture.width;
			float num2 = vector.y / (float)this.activeSprite.texture.height;
			bool flag;
			try
			{
				flag = this.activeSprite.texture.GetPixelBilinear(num, num2).a >= this.alphaHitTestMinimumThreshold;
			}
			catch (UnityException ex)
			{
				Debug.LogError("Using alphaHitTestMinimumThreshold greater than 0 on Image whose sprite texture cannot be read. " + ex.Message + " Also make sure to disable sprite packing for this sprite.", this);
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00009300 File Offset: 0x00007500
		private Vector2 MapCoordinate(Vector2 local, Rect rect)
		{
			Rect rect2 = this.activeSprite.rect;
			if (this.type == Image.Type.Simple || this.type == Image.Type.Filled)
			{
				return new Vector2(rect2.position.x + local.x * rect2.width / rect.width, rect2.position.y + local.y * rect2.height / rect.height);
			}
			Vector4 border = this.activeSprite.border;
			Vector4 adjustedBorders = this.GetAdjustedBorders(border / this.pixelsPerUnit, rect);
			for (int i = 0; i < 2; i++)
			{
				if (local[i] > adjustedBorders[i])
				{
					if (rect.size[i] - local[i] <= adjustedBorders[i + 2])
					{
						ref Vector2 ptr = ref local;
						int num = i;
						ptr[num] -= rect.size[i] - rect2.size[i];
					}
					else if (this.type == Image.Type.Sliced)
					{
						float num2 = Mathf.InverseLerp(adjustedBorders[i], rect.size[i] - adjustedBorders[i + 2], local[i]);
						local[i] = Mathf.Lerp(border[i], rect2.size[i] - border[i + 2], num2);
					}
					else
					{
						ref Vector2 ptr = ref local;
						int num = i;
						ptr[num] -= adjustedBorders[i];
						local[i] = Mathf.Repeat(local[i], rect2.size[i] - border[i] - border[i + 2]);
						ptr = ref local;
						num = i;
						ptr[num] += border[i];
					}
				}
			}
			return local + rect2.position;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00009524 File Offset: 0x00007724
		private static void RebuildImage(SpriteAtlas spriteAtlas)
		{
			for (int i = Image.m_TrackedTexturelessImages.Count - 1; i >= 0; i--)
			{
				Image image = Image.m_TrackedTexturelessImages[i];
				if (null != image.activeSprite && spriteAtlas.CanBindTo(image.activeSprite))
				{
					image.SetAllDirty();
					Image.m_TrackedTexturelessImages.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00009581 File Offset: 0x00007781
		private static void TrackImage(Image g)
		{
			if (!Image.s_Initialized)
			{
				SpriteAtlasManager.atlasRegistered += Image.RebuildImage;
				Image.s_Initialized = true;
			}
			Image.m_TrackedTexturelessImages.Add(g);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000095AC File Offset: 0x000077AC
		private static void UnTrackImage(Image g)
		{
			Image.m_TrackedTexturelessImages.Remove(g);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000095BA File Offset: 0x000077BA
		protected override void OnDidApplyAnimationProperties()
		{
			this.SetMaterialDirty();
			this.SetVerticesDirty();
			base.SetRaycastDirty();
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000960C File Offset: 0x0000780C
		[CompilerGenerated]
		private void <set_sprite>g__ResetAlphaHitThresholdIfNeeded|11_0()
		{
			if (!this.<set_sprite>g__SpriteSupportsAlphaHitTest|11_1() && this.m_AlphaHitTestMinimumThreshold > 0f)
			{
				Debug.LogWarning("Sprite was changed for one not readable or with Crunch Compression. Resetting the AlphaHitThreshold to 0.", this);
				this.m_AlphaHitTestMinimumThreshold = 0f;
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000963C File Offset: 0x0000783C
		[CompilerGenerated]
		private bool <set_sprite>g__SpriteSupportsAlphaHitTest|11_1()
		{
			return this.m_Sprite != null && this.m_Sprite.texture != null && !GraphicsFormatUtility.IsCrunchFormat(this.m_Sprite.texture.format) && this.m_Sprite.texture.isReadable;
		}

		// Token: 0x04000070 RID: 112
		protected static Material s_ETC1DefaultUI = null;

		// Token: 0x04000071 RID: 113
		[FormerlySerializedAs("m_Frame")]
		[SerializeField]
		private Sprite m_Sprite;

		// Token: 0x04000072 RID: 114
		[NonSerialized]
		private Sprite m_OverrideSprite;

		// Token: 0x04000073 RID: 115
		[SerializeField]
		private Image.Type m_Type;

		// Token: 0x04000074 RID: 116
		[SerializeField]
		private bool m_PreserveAspect;

		// Token: 0x04000075 RID: 117
		[SerializeField]
		private bool m_FillCenter = true;

		// Token: 0x04000076 RID: 118
		[SerializeField]
		private Image.FillMethod m_FillMethod = Image.FillMethod.Radial360;

		// Token: 0x04000077 RID: 119
		[Range(0f, 1f)]
		[SerializeField]
		private float m_FillAmount = 1f;

		// Token: 0x04000078 RID: 120
		[SerializeField]
		private bool m_FillClockwise = true;

		// Token: 0x04000079 RID: 121
		[SerializeField]
		private int m_FillOrigin;

		// Token: 0x0400007A RID: 122
		private float m_AlphaHitTestMinimumThreshold;

		// Token: 0x0400007B RID: 123
		private bool m_Tracked;

		// Token: 0x0400007C RID: 124
		[SerializeField]
		private bool m_UseSpriteMesh;

		// Token: 0x0400007D RID: 125
		[SerializeField]
		private float m_PixelsPerUnitMultiplier = 1f;

		// Token: 0x0400007E RID: 126
		private float m_CachedReferencePixelsPerUnit = 100f;

		// Token: 0x0400007F RID: 127
		private static readonly Vector2[] s_VertScratch = new Vector2[4];

		// Token: 0x04000080 RID: 128
		private static readonly Vector2[] s_UVScratch = new Vector2[4];

		// Token: 0x04000081 RID: 129
		private static readonly Vector3[] s_Xy = new Vector3[4];

		// Token: 0x04000082 RID: 130
		private static readonly Vector3[] s_Uv = new Vector3[4];

		// Token: 0x04000083 RID: 131
		private static List<Image> m_TrackedTexturelessImages = new List<Image>();

		// Token: 0x04000084 RID: 132
		private static bool s_Initialized;

		// Token: 0x02000086 RID: 134
		public enum Type
		{
			// Token: 0x0400026F RID: 623
			Simple,
			// Token: 0x04000270 RID: 624
			Sliced,
			// Token: 0x04000271 RID: 625
			Tiled,
			// Token: 0x04000272 RID: 626
			Filled
		}

		// Token: 0x02000087 RID: 135
		public enum FillMethod
		{
			// Token: 0x04000274 RID: 628
			Horizontal,
			// Token: 0x04000275 RID: 629
			Vertical,
			// Token: 0x04000276 RID: 630
			Radial90,
			// Token: 0x04000277 RID: 631
			Radial180,
			// Token: 0x04000278 RID: 632
			Radial360
		}

		// Token: 0x02000088 RID: 136
		public enum OriginHorizontal
		{
			// Token: 0x0400027A RID: 634
			Left,
			// Token: 0x0400027B RID: 635
			Right
		}

		// Token: 0x02000089 RID: 137
		public enum OriginVertical
		{
			// Token: 0x0400027D RID: 637
			Bottom,
			// Token: 0x0400027E RID: 638
			Top
		}

		// Token: 0x0200008A RID: 138
		public enum Origin90
		{
			// Token: 0x04000280 RID: 640
			BottomLeft,
			// Token: 0x04000281 RID: 641
			TopLeft,
			// Token: 0x04000282 RID: 642
			TopRight,
			// Token: 0x04000283 RID: 643
			BottomRight
		}

		// Token: 0x0200008B RID: 139
		public enum Origin180
		{
			// Token: 0x04000285 RID: 645
			Bottom,
			// Token: 0x04000286 RID: 646
			Left,
			// Token: 0x04000287 RID: 647
			Top,
			// Token: 0x04000288 RID: 648
			Right
		}

		// Token: 0x0200008C RID: 140
		public enum Origin360
		{
			// Token: 0x0400028A RID: 650
			Bottom,
			// Token: 0x0400028B RID: 651
			Right,
			// Token: 0x0400028C RID: 652
			Top,
			// Token: 0x0400028D RID: 653
			Left
		}
	}
}
