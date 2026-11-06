using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using UnityEngine.UI.CoroutineTween;

namespace UnityEngine.UI
{
	// Token: 0x02000012 RID: 18
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	[ExecuteAlways]
	public abstract class Graphic : UIBehaviour, ICanvasElement
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000053BD File Offset: 0x000035BD
		public static Material defaultGraphicMaterial
		{
			get
			{
				if (Graphic.s_DefaultUI == null)
				{
					Graphic.s_DefaultUI = Canvas.GetDefaultCanvasMaterial();
				}
				return Graphic.s_DefaultUI;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000053DB File Offset: 0x000035DB
		// (set) Token: 0x060000BB RID: 187 RVA: 0x000053E3 File Offset: 0x000035E3
		public virtual Color color
		{
			get
			{
				return this.m_Color;
			}
			set
			{
				if (SetPropertyUtility.SetColor(ref this.m_Color, value))
				{
					this.SetVerticesDirty();
				}
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000053F9 File Offset: 0x000035F9
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00005404 File Offset: 0x00003604
		public virtual bool raycastTarget
		{
			get
			{
				return this.m_RaycastTarget;
			}
			set
			{
				if (value != this.m_RaycastTarget)
				{
					if (this.m_RaycastTarget)
					{
						GraphicRegistry.UnregisterRaycastGraphicForCanvas(this.canvas, this);
					}
					this.m_RaycastTarget = value;
					if (this.m_RaycastTarget && base.isActiveAndEnabled)
					{
						GraphicRegistry.RegisterRaycastGraphicForCanvas(this.canvas, this);
					}
				}
				this.m_RaycastTargetCache = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00005458 File Offset: 0x00003658
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00005460 File Offset: 0x00003660
		public Vector4 raycastPadding
		{
			get
			{
				return this.m_RaycastPadding;
			}
			set
			{
				this.m_RaycastPadding = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00005469 File Offset: 0x00003669
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00005471 File Offset: 0x00003671
		protected bool useLegacyMeshGeneration { get; set; }

		// Token: 0x060000C2 RID: 194 RVA: 0x0000547C File Offset: 0x0000367C
		protected Graphic()
		{
			if (this.m_ColorTweenRunner == null)
			{
				this.m_ColorTweenRunner = new TweenRunner<ColorTween>();
			}
			this.m_ColorTweenRunner.Init(this);
			this.useLegacyMeshGeneration = true;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000054CE File Offset: 0x000036CE
		public virtual void SetAllDirty()
		{
			if (this.m_SkipLayoutUpdate)
			{
				this.m_SkipLayoutUpdate = false;
			}
			else
			{
				this.SetLayoutDirty();
			}
			if (this.m_SkipMaterialUpdate)
			{
				this.m_SkipMaterialUpdate = false;
			}
			else
			{
				this.SetMaterialDirty();
			}
			this.SetVerticesDirty();
			this.SetRaycastDirty();
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000550A File Offset: 0x0000370A
		public virtual void SetLayoutDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			if (this.m_OnDirtyLayoutCallback != null)
			{
				this.m_OnDirtyLayoutCallback();
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005533 File Offset: 0x00003733
		public virtual void SetVerticesDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.m_VertsDirty = true;
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
			if (this.m_OnDirtyVertsCallback != null)
			{
				this.m_OnDirtyVertsCallback();
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000555E File Offset: 0x0000375E
		public virtual void SetMaterialDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.m_MaterialDirty = true;
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
			if (this.m_OnDirtyMaterialCallback != null)
			{
				this.m_OnDirtyMaterialCallback();
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000558C File Offset: 0x0000378C
		public void SetRaycastDirty()
		{
			if (this.m_RaycastTargetCache != this.m_RaycastTarget)
			{
				if (this.m_RaycastTarget && base.isActiveAndEnabled)
				{
					GraphicRegistry.RegisterRaycastGraphicForCanvas(this.canvas, this);
				}
				else if (!this.m_RaycastTarget)
				{
					GraphicRegistry.UnregisterRaycastGraphicForCanvas(this.canvas, this);
				}
			}
			this.m_RaycastTargetCache = this.m_RaycastTarget;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000055E5 File Offset: 0x000037E5
		protected override void OnRectTransformDimensionsChange()
		{
			if (base.gameObject.activeInHierarchy)
			{
				if (CanvasUpdateRegistry.IsRebuildingLayout())
				{
					this.SetVerticesDirty();
					return;
				}
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000560E File Offset: 0x0000380E
		protected override void OnBeforeTransformParentChanged()
		{
			GraphicRegistry.UnregisterGraphicForCanvas(this.canvas, this);
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005627 File Offset: 0x00003827
		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.m_Canvas = null;
			if (!this.IsActive())
			{
				return;
			}
			this.CacheCanvas();
			GraphicRegistry.RegisterGraphicForCanvas(this.canvas, this);
			this.SetAllDirty();
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00005657 File Offset: 0x00003857
		public int depth
		{
			get
			{
				return this.canvasRenderer.absoluteDepth;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00005664 File Offset: 0x00003864
		public RectTransform rectTransform
		{
			get
			{
				if (this.m_RectTransform == null)
				{
					this.m_RectTransform = base.GetComponent<RectTransform>();
				}
				return this.m_RectTransform;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00005680 File Offset: 0x00003880
		public Canvas canvas
		{
			get
			{
				if (this.m_Canvas == null)
				{
					this.CacheCanvas();
				}
				return this.m_Canvas;
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000569C File Offset: 0x0000389C
		private void CacheCanvas()
		{
			List<Canvas> list = CollectionPool<List<Canvas>, Canvas>.Get();
			base.gameObject.GetComponentsInParent<Canvas>(false, list);
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].isActiveAndEnabled)
					{
						this.m_Canvas = list[i];
						break;
					}
					if (i == list.Count - 1)
					{
						this.m_Canvas = null;
					}
				}
			}
			else
			{
				this.m_Canvas = null;
			}
			CollectionPool<List<Canvas>, Canvas>.Release(list);
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00005714 File Offset: 0x00003914
		public CanvasRenderer canvasRenderer
		{
			get
			{
				if (this.m_CanvasRenderer == null)
				{
					this.m_CanvasRenderer = base.GetComponent<CanvasRenderer>();
					if (this.m_CanvasRenderer == null)
					{
						this.m_CanvasRenderer = base.gameObject.AddComponent<CanvasRenderer>();
					}
				}
				return this.m_CanvasRenderer;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00005749 File Offset: 0x00003949
		public virtual Material defaultMaterial
		{
			get
			{
				return Graphic.defaultGraphicMaterial;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00005750 File Offset: 0x00003950
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x0000576D File Offset: 0x0000396D
		public virtual Material material
		{
			get
			{
				if (!(this.m_Material != null))
				{
					return this.defaultMaterial;
				}
				return this.m_Material;
			}
			set
			{
				if (this.m_Material == value)
				{
					return;
				}
				this.m_Material = value;
				this.SetMaterialDirty();
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x0000578C File Offset: 0x0000398C
		public virtual Material materialForRendering
		{
			get
			{
				List<IMaterialModifier> list = CollectionPool<List<IMaterialModifier>, IMaterialModifier>.Get();
				base.GetComponents<IMaterialModifier>(list);
				Material material = this.material;
				for (int i = 0; i < list.Count; i++)
				{
					material = list[i].GetModifiedMaterial(material);
				}
				CollectionPool<List<IMaterialModifier>, IMaterialModifier>.Release(list);
				return material;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x000057D3 File Offset: 0x000039D3
		public virtual Texture mainTexture
		{
			get
			{
				return Graphic.s_WhiteTexture;
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000057DA File Offset: 0x000039DA
		protected override void OnEnable()
		{
			base.OnEnable();
			this.CacheCanvas();
			GraphicRegistry.RegisterGraphicForCanvas(this.canvas, this);
			if (Graphic.s_WhiteTexture == null)
			{
				Graphic.s_WhiteTexture = Texture2D.whiteTexture;
			}
			this.SetAllDirty();
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005811 File Offset: 0x00003A11
		protected override void OnDisable()
		{
			GraphicRegistry.DisableGraphicForCanvas(this.canvas, this);
			CanvasUpdateRegistry.DisableCanvasElementForRebuild(this);
			if (this.canvasRenderer != null)
			{
				this.canvasRenderer.Clear();
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			base.OnDisable();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000584F File Offset: 0x00003A4F
		protected override void OnDestroy()
		{
			GraphicRegistry.UnregisterGraphicForCanvas(this.canvas, this);
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (this.m_CachedMesh)
			{
				Object.Destroy(this.m_CachedMesh);
			}
			this.m_CachedMesh = null;
			base.OnDestroy();
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005888 File Offset: 0x00003A88
		protected override void OnCanvasHierarchyChanged()
		{
			Canvas canvas = this.m_Canvas;
			this.m_Canvas = null;
			if (!this.IsActive())
			{
				GraphicRegistry.UnregisterGraphicForCanvas(canvas, this);
				return;
			}
			this.CacheCanvas();
			if (canvas != this.m_Canvas)
			{
				GraphicRegistry.UnregisterGraphicForCanvas(canvas, this);
				if (this.IsActive())
				{
					GraphicRegistry.RegisterGraphicForCanvas(this.canvas, this);
				}
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000058E2 File Offset: 0x00003AE2
		public virtual void OnCullingChanged()
		{
			if (!this.canvasRenderer.cull && (this.m_VertsDirty || this.m_MaterialDirty))
			{
				CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005908 File Offset: 0x00003B08
		public virtual void Rebuild(CanvasUpdate update)
		{
			if (this.canvasRenderer == null || this.canvasRenderer.cull)
			{
				return;
			}
			if (update == CanvasUpdate.PreRender)
			{
				if (this.m_VertsDirty)
				{
					this.UpdateGeometry();
					this.m_VertsDirty = false;
				}
				if (this.m_MaterialDirty)
				{
					this.UpdateMaterial();
					this.m_MaterialDirty = false;
				}
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000595F File Offset: 0x00003B5F
		public virtual void LayoutComplete()
		{
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005961 File Offset: 0x00003B61
		public virtual void GraphicUpdateComplete()
		{
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005963 File Offset: 0x00003B63
		protected virtual void UpdateMaterial()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.canvasRenderer.materialCount = 1;
			this.canvasRenderer.SetMaterial(this.materialForRendering, 0);
			this.canvasRenderer.SetTexture(this.mainTexture);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000599D File Offset: 0x00003B9D
		protected virtual void UpdateGeometry()
		{
			if (this.useLegacyMeshGeneration)
			{
				this.DoLegacyMeshGeneration();
				return;
			}
			this.DoMeshGeneration();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000059B4 File Offset: 0x00003BB4
		private void DoMeshGeneration()
		{
			if (this.rectTransform != null && this.rectTransform.rect.width >= 0f && this.rectTransform.rect.height >= 0f)
			{
				this.OnPopulateMesh(Graphic.s_VertexHelper);
			}
			else
			{
				Graphic.s_VertexHelper.Clear();
			}
			List<Component> list = CollectionPool<List<Component>, Component>.Get();
			base.GetComponents(typeof(IMeshModifier), list);
			for (int i = 0; i < list.Count; i++)
			{
				((IMeshModifier)list[i]).ModifyMesh(Graphic.s_VertexHelper);
			}
			CollectionPool<List<Component>, Component>.Release(list);
			Graphic.s_VertexHelper.FillMesh(Graphic.workerMesh);
			this.canvasRenderer.SetMesh(Graphic.workerMesh);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005A80 File Offset: 0x00003C80
		private void DoLegacyMeshGeneration()
		{
			if (this.rectTransform != null && this.rectTransform.rect.width >= 0f && this.rectTransform.rect.height >= 0f)
			{
				this.OnPopulateMesh(Graphic.workerMesh);
			}
			else
			{
				Graphic.workerMesh.Clear();
			}
			List<Component> list = CollectionPool<List<Component>, Component>.Get();
			base.GetComponents(typeof(IMeshModifier), list);
			for (int i = 0; i < list.Count; i++)
			{
				((IMeshModifier)list[i]).ModifyMesh(Graphic.workerMesh);
			}
			CollectionPool<List<Component>, Component>.Release(list);
			this.canvasRenderer.SetMesh(Graphic.workerMesh);
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00005B3A File Offset: 0x00003D3A
		protected static Mesh workerMesh
		{
			get
			{
				if (Graphic.s_Mesh == null)
				{
					Graphic.s_Mesh = new Mesh();
					Graphic.s_Mesh.name = "Shared UI Mesh";
				}
				return Graphic.s_Mesh;
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005B67 File Offset: 0x00003D67
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use OnPopulateMesh instead.", true)]
		protected virtual void OnFillVBO(List<UIVertex> vbo)
		{
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005B69 File Offset: 0x00003D69
		[Obsolete("Use OnPopulateMesh(VertexHelper vh) instead.", false)]
		protected virtual void OnPopulateMesh(Mesh m)
		{
			this.OnPopulateMesh(Graphic.s_VertexHelper);
			Graphic.s_VertexHelper.FillMesh(m);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005B84 File Offset: 0x00003D84
		protected virtual void OnPopulateMesh(VertexHelper vh)
		{
			Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
			Vector4 vector = new Vector4(pixelAdjustedRect.x, pixelAdjustedRect.y, pixelAdjustedRect.x + pixelAdjustedRect.width, pixelAdjustedRect.y + pixelAdjustedRect.height);
			Color32 color = this.color;
			vh.Clear();
			vh.AddVert(new Vector3(vector.x, vector.y), color, new Vector2(0f, 0f));
			vh.AddVert(new Vector3(vector.x, vector.w), color, new Vector2(0f, 1f));
			vh.AddVert(new Vector3(vector.z, vector.w), color, new Vector2(1f, 1f));
			vh.AddVert(new Vector3(vector.z, vector.y), color, new Vector2(1f, 0f));
			vh.AddTriangle(0, 1, 2);
			vh.AddTriangle(2, 3, 0);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005C9F File Offset: 0x00003E9F
		protected override void OnDidApplyAnimationProperties()
		{
			this.SetAllDirty();
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005CA7 File Offset: 0x00003EA7
		public virtual void SetNativeSize()
		{
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005CAC File Offset: 0x00003EAC
		public virtual bool Raycast(Vector2 sp, Camera eventCamera)
		{
			if (!base.isActiveAndEnabled)
			{
				return false;
			}
			Transform transform = base.transform;
			List<Component> list = CollectionPool<List<Component>, Component>.Get();
			bool flag = false;
			bool flag2 = true;
			while (transform != null)
			{
				transform.GetComponents<Component>(list);
				for (int i = 0; i < list.Count; i++)
				{
					Canvas canvas = list[i] as Canvas;
					if (canvas != null && canvas.overrideSorting)
					{
						flag2 = false;
					}
					ICanvasRaycastFilter canvasRaycastFilter = list[i] as ICanvasRaycastFilter;
					if (canvasRaycastFilter != null)
					{
						bool flag3 = true;
						CanvasGroup canvasGroup = list[i] as CanvasGroup;
						if (canvasGroup != null)
						{
							if (!canvasGroup.enabled)
							{
								goto IL_00CD;
							}
							if (!flag && canvasGroup.ignoreParentGroups)
							{
								flag = true;
								flag3 = canvasRaycastFilter.IsRaycastLocationValid(sp, eventCamera);
							}
							else if (!flag)
							{
								flag3 = canvasRaycastFilter.IsRaycastLocationValid(sp, eventCamera);
							}
						}
						else
						{
							flag3 = canvasRaycastFilter.IsRaycastLocationValid(sp, eventCamera);
						}
						if (!flag3)
						{
							CollectionPool<List<Component>, Component>.Release(list);
							return false;
						}
					}
					IL_00CD:;
				}
				transform = (flag2 ? transform.parent : null);
			}
			CollectionPool<List<Component>, Component>.Release(list);
			return true;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005DBC File Offset: 0x00003FBC
		public Vector2 PixelAdjustPoint(Vector2 point)
		{
			if (!this.canvas || this.canvas.renderMode == RenderMode.WorldSpace || this.canvas.scaleFactor == 0f || !this.canvas.pixelPerfect)
			{
				return point;
			}
			return RectTransformUtility.PixelAdjustPoint(point, base.transform, this.canvas);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005E18 File Offset: 0x00004018
		public Rect GetPixelAdjustedRect()
		{
			if (!this.canvas || this.canvas.renderMode == RenderMode.WorldSpace || this.canvas.scaleFactor == 0f || !this.canvas.pixelPerfect)
			{
				return this.rectTransform.rect;
			}
			return RectTransformUtility.PixelAdjustRect(this.rectTransform, this.canvas);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00005E7C File Offset: 0x0000407C
		public virtual void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
			this.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha, true);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005E8C File Offset: 0x0000408C
		public virtual void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha, bool useRGB)
		{
			if (this.canvasRenderer == null || (!useRGB && !useAlpha))
			{
				return;
			}
			if (this.canvasRenderer.GetColor().Equals(targetColor))
			{
				this.m_ColorTweenRunner.StopTween();
				return;
			}
			ColorTween.ColorTweenMode colorTweenMode = ((useRGB && useAlpha) ? ColorTween.ColorTweenMode.All : (useRGB ? ColorTween.ColorTweenMode.RGB : ColorTween.ColorTweenMode.Alpha));
			ColorTween colorTween = new ColorTween
			{
				duration = duration,
				startColor = this.canvasRenderer.GetColor(),
				targetColor = targetColor
			};
			colorTween.AddOnChangedCallback(new UnityAction<Color>(this.canvasRenderer.SetColor));
			colorTween.ignoreTimeScale = ignoreTimeScale;
			colorTween.tweenMode = colorTweenMode;
			this.m_ColorTweenRunner.StartTween(colorTween);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005F48 File Offset: 0x00004148
		private static Color CreateColorFromAlpha(float alpha)
		{
			Color black = Color.black;
			black.a = alpha;
			return black;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005F64 File Offset: 0x00004164
		public virtual void CrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
			this.CrossFadeColor(Graphic.CreateColorFromAlpha(alpha), duration, ignoreTimeScale, true, false);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005F76 File Offset: 0x00004176
		public void RegisterDirtyLayoutCallback(UnityAction action)
		{
			this.m_OnDirtyLayoutCallback = (UnityAction)Delegate.Combine(this.m_OnDirtyLayoutCallback, action);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005F8F File Offset: 0x0000418F
		public void UnregisterDirtyLayoutCallback(UnityAction action)
		{
			this.m_OnDirtyLayoutCallback = (UnityAction)Delegate.Remove(this.m_OnDirtyLayoutCallback, action);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005FA8 File Offset: 0x000041A8
		public void RegisterDirtyVerticesCallback(UnityAction action)
		{
			this.m_OnDirtyVertsCallback = (UnityAction)Delegate.Combine(this.m_OnDirtyVertsCallback, action);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005FC1 File Offset: 0x000041C1
		public void UnregisterDirtyVerticesCallback(UnityAction action)
		{
			this.m_OnDirtyVertsCallback = (UnityAction)Delegate.Remove(this.m_OnDirtyVertsCallback, action);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005FDA File Offset: 0x000041DA
		public void RegisterDirtyMaterialCallback(UnityAction action)
		{
			this.m_OnDirtyMaterialCallback = (UnityAction)Delegate.Combine(this.m_OnDirtyMaterialCallback, action);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005FF3 File Offset: 0x000041F3
		public void UnregisterDirtyMaterialCallback(UnityAction action)
		{
			this.m_OnDirtyMaterialCallback = (UnityAction)Delegate.Remove(this.m_OnDirtyMaterialCallback, action);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006024 File Offset: 0x00004224
		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}

		// Token: 0x0400004E RID: 78
		protected static Material s_DefaultUI = null;

		// Token: 0x0400004F RID: 79
		protected static Texture2D s_WhiteTexture = null;

		// Token: 0x04000050 RID: 80
		[FormerlySerializedAs("m_Mat")]
		[SerializeField]
		protected Material m_Material;

		// Token: 0x04000051 RID: 81
		[SerializeField]
		private Color m_Color = Color.white;

		// Token: 0x04000052 RID: 82
		[NonSerialized]
		protected bool m_SkipLayoutUpdate;

		// Token: 0x04000053 RID: 83
		[NonSerialized]
		protected bool m_SkipMaterialUpdate;

		// Token: 0x04000054 RID: 84
		[SerializeField]
		private bool m_RaycastTarget = true;

		// Token: 0x04000055 RID: 85
		private bool m_RaycastTargetCache = true;

		// Token: 0x04000056 RID: 86
		[SerializeField]
		private Vector4 m_RaycastPadding;

		// Token: 0x04000057 RID: 87
		[NonSerialized]
		private RectTransform m_RectTransform;

		// Token: 0x04000058 RID: 88
		[NonSerialized]
		private CanvasRenderer m_CanvasRenderer;

		// Token: 0x04000059 RID: 89
		[NonSerialized]
		private Canvas m_Canvas;

		// Token: 0x0400005A RID: 90
		[NonSerialized]
		private bool m_VertsDirty;

		// Token: 0x0400005B RID: 91
		[NonSerialized]
		private bool m_MaterialDirty;

		// Token: 0x0400005C RID: 92
		[NonSerialized]
		protected UnityAction m_OnDirtyLayoutCallback;

		// Token: 0x0400005D RID: 93
		[NonSerialized]
		protected UnityAction m_OnDirtyVertsCallback;

		// Token: 0x0400005E RID: 94
		[NonSerialized]
		protected UnityAction m_OnDirtyMaterialCallback;

		// Token: 0x0400005F RID: 95
		[NonSerialized]
		protected static Mesh s_Mesh;

		// Token: 0x04000060 RID: 96
		[NonSerialized]
		private static readonly VertexHelper s_VertexHelper = new VertexHelper();

		// Token: 0x04000061 RID: 97
		[NonSerialized]
		protected Mesh m_CachedMesh;

		// Token: 0x04000062 RID: 98
		[NonSerialized]
		protected Vector2[] m_CachedUvs;

		// Token: 0x04000063 RID: 99
		[NonSerialized]
		private readonly TweenRunner<ColorTween> m_ColorTweenRunner;
	}
}
