using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

namespace UnityEngine.UI
{
	// Token: 0x02000032 RID: 50
	[AddComponentMenu("UI/Rect Mask 2D", 14)]
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public class RectMask2D : UIBehaviour, IClipper, ICanvasRaycastFilter
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000323 RID: 803 RVA: 0x0001080B File Offset: 0x0000EA0B
		// (set) Token: 0x06000324 RID: 804 RVA: 0x00010813 File Offset: 0x0000EA13
		public Vector4 padding
		{
			get
			{
				return this.m_Padding;
			}
			set
			{
				this.m_Padding = value;
				MaskUtilities.Notify2DMaskStateChanged(this);
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000325 RID: 805 RVA: 0x00010822 File Offset: 0x0000EA22
		// (set) Token: 0x06000326 RID: 806 RVA: 0x0001082A File Offset: 0x0000EA2A
		public Vector2Int softness
		{
			get
			{
				return this.m_Softness;
			}
			set
			{
				this.m_Softness.x = Mathf.Max(0, value.x);
				this.m_Softness.y = Mathf.Max(0, value.y);
				MaskUtilities.Notify2DMaskStateChanged(this);
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00010864 File Offset: 0x0000EA64
		internal Canvas Canvas
		{
			get
			{
				if (this.m_Canvas == null)
				{
					List<Canvas> list = CollectionPool<List<Canvas>, Canvas>.Get();
					base.gameObject.GetComponentsInParent<Canvas>(false, list);
					if (list.Count > 0)
					{
						this.m_Canvas = list[list.Count - 1];
					}
					else
					{
						this.m_Canvas = null;
					}
					CollectionPool<List<Canvas>, Canvas>.Release(list);
				}
				return this.m_Canvas;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000328 RID: 808 RVA: 0x000108C4 File Offset: 0x0000EAC4
		public Rect canvasRect
		{
			get
			{
				return this.m_VertexClipper.GetCanvasRect(this.rectTransform, this.Canvas);
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000329 RID: 809 RVA: 0x000108E0 File Offset: 0x0000EAE0
		public RectTransform rectTransform
		{
			get
			{
				RectTransform rectTransform;
				if ((rectTransform = this.m_RectTransform) == null)
				{
					rectTransform = (this.m_RectTransform = base.GetComponent<RectTransform>());
				}
				return rectTransform;
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00010906 File Offset: 0x0000EB06
		protected RectMask2D()
		{
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00010946 File Offset: 0x0000EB46
		protected override void OnEnable()
		{
			base.OnEnable();
			this.m_ShouldRecalculateClipRects = true;
			ClipperRegistry.Register(this);
			MaskUtilities.Notify2DMaskStateChanged(this);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00010961 File Offset: 0x0000EB61
		protected override void OnDisable()
		{
			base.OnDisable();
			this.m_ClipTargets.Clear();
			this.m_MaskableTargets.Clear();
			this.m_Clippers.Clear();
			ClipperRegistry.Disable(this);
			MaskUtilities.Notify2DMaskStateChanged(this);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00010996 File Offset: 0x0000EB96
		protected override void OnDestroy()
		{
			ClipperRegistry.Unregister(this);
			base.OnDestroy();
		}

		// Token: 0x0600032E RID: 814 RVA: 0x000109A4 File Offset: 0x0000EBA4
		public virtual bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
		{
			return !base.isActiveAndEnabled || RectTransformUtility.RectangleContainsScreenPoint(this.rectTransform, sp, eventCamera, this.m_Padding);
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600032F RID: 815 RVA: 0x000109C4 File Offset: 0x0000EBC4
		private Rect rootCanvasRect
		{
			get
			{
				this.rectTransform.GetWorldCorners(this.m_Corners);
				if (this.Canvas != null)
				{
					Canvas rootCanvas = this.Canvas.rootCanvas;
					for (int i = 0; i < 4; i++)
					{
						this.m_Corners[i] = rootCanvas.transform.InverseTransformPoint(this.m_Corners[i]);
					}
				}
				return new Rect(this.m_Corners[0].x, this.m_Corners[0].y, this.m_Corners[2].x - this.m_Corners[0].x, this.m_Corners[2].y - this.m_Corners[0].y);
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00010A94 File Offset: 0x0000EC94
		public virtual void PerformClipping()
		{
			if (this.Canvas == null)
			{
				return;
			}
			if (this.m_ShouldRecalculateClipRects)
			{
				MaskUtilities.GetRectMasksForClip(this, this.m_Clippers);
				this.m_ShouldRecalculateClipRects = false;
			}
			bool flag = true;
			Rect rect = Clipping.FindCullAndClipWorldRect(this.m_Clippers, out flag);
			RenderMode renderMode = this.Canvas.rootCanvas.renderMode;
			if ((renderMode == RenderMode.ScreenSpaceCamera || renderMode == RenderMode.ScreenSpaceOverlay) && !rect.Overlaps(this.rootCanvasRect, true))
			{
				rect = Rect.zero;
				flag = false;
			}
			if (rect != this.m_LastClipRectCanvasSpace)
			{
				foreach (IClippable clippable in this.m_ClipTargets)
				{
					clippable.SetClipRect(rect, flag);
				}
				using (HashSet<MaskableGraphic>.Enumerator enumerator2 = this.m_MaskableTargets.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						MaskableGraphic maskableGraphic = enumerator2.Current;
						maskableGraphic.SetClipRect(rect, flag);
						maskableGraphic.Cull(rect, flag);
					}
					goto IL_01B5;
				}
			}
			if (this.m_ForceClip)
			{
				foreach (IClippable clippable2 in this.m_ClipTargets)
				{
					clippable2.SetClipRect(rect, flag);
				}
				using (HashSet<MaskableGraphic>.Enumerator enumerator2 = this.m_MaskableTargets.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						MaskableGraphic maskableGraphic2 = enumerator2.Current;
						maskableGraphic2.SetClipRect(rect, flag);
						if (maskableGraphic2.canvasRenderer.hasMoved)
						{
							maskableGraphic2.Cull(rect, flag);
						}
					}
					goto IL_01B5;
				}
			}
			foreach (MaskableGraphic maskableGraphic3 in this.m_MaskableTargets)
			{
				maskableGraphic3.Cull(rect, flag);
			}
			IL_01B5:
			this.m_LastClipRectCanvasSpace = rect;
			this.m_ForceClip = false;
			this.UpdateClipSoftness();
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00010CAC File Offset: 0x0000EEAC
		public virtual void UpdateClipSoftness()
		{
			if (this.Canvas == null)
			{
				return;
			}
			foreach (IClippable clippable in this.m_ClipTargets)
			{
				clippable.SetClipSoftness(this.m_Softness);
			}
			foreach (MaskableGraphic maskableGraphic in this.m_MaskableTargets)
			{
				maskableGraphic.SetClipSoftness(this.m_Softness);
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00010D5C File Offset: 0x0000EF5C
		public void AddClippable(IClippable clippable)
		{
			if (clippable == null)
			{
				return;
			}
			this.m_ShouldRecalculateClipRects = true;
			MaskableGraphic maskableGraphic = clippable as MaskableGraphic;
			if (maskableGraphic == null)
			{
				this.m_ClipTargets.Add(clippable);
			}
			else
			{
				this.m_MaskableTargets.Add(maskableGraphic);
			}
			this.m_ForceClip = true;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00010DA8 File Offset: 0x0000EFA8
		public void RemoveClippable(IClippable clippable)
		{
			if (clippable == null)
			{
				return;
			}
			this.m_ShouldRecalculateClipRects = true;
			clippable.SetClipRect(default(Rect), false);
			MaskableGraphic maskableGraphic = clippable as MaskableGraphic;
			if (maskableGraphic == null)
			{
				this.m_ClipTargets.Remove(clippable);
			}
			else
			{
				this.m_MaskableTargets.Remove(maskableGraphic);
			}
			this.m_ForceClip = true;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00010E03 File Offset: 0x0000F003
		protected override void OnTransformParentChanged()
		{
			this.m_Canvas = null;
			base.OnTransformParentChanged();
			this.m_ShouldRecalculateClipRects = true;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00010E19 File Offset: 0x0000F019
		protected override void OnCanvasHierarchyChanged()
		{
			this.m_Canvas = null;
			base.OnCanvasHierarchyChanged();
			this.m_ShouldRecalculateClipRects = true;
		}

		// Token: 0x0400010D RID: 269
		[NonSerialized]
		private readonly RectangularVertexClipper m_VertexClipper = new RectangularVertexClipper();

		// Token: 0x0400010E RID: 270
		[NonSerialized]
		private RectTransform m_RectTransform;

		// Token: 0x0400010F RID: 271
		[NonSerialized]
		private HashSet<MaskableGraphic> m_MaskableTargets = new HashSet<MaskableGraphic>();

		// Token: 0x04000110 RID: 272
		[NonSerialized]
		private HashSet<IClippable> m_ClipTargets = new HashSet<IClippable>();

		// Token: 0x04000111 RID: 273
		[NonSerialized]
		private bool m_ShouldRecalculateClipRects;

		// Token: 0x04000112 RID: 274
		[NonSerialized]
		private List<RectMask2D> m_Clippers = new List<RectMask2D>();

		// Token: 0x04000113 RID: 275
		[NonSerialized]
		private Rect m_LastClipRectCanvasSpace;

		// Token: 0x04000114 RID: 276
		[NonSerialized]
		private bool m_ForceClip;

		// Token: 0x04000115 RID: 277
		[SerializeField]
		private Vector4 m_Padding;

		// Token: 0x04000116 RID: 278
		[SerializeField]
		private Vector2Int m_Softness;

		// Token: 0x04000117 RID: 279
		[NonSerialized]
		private Canvas m_Canvas;

		// Token: 0x04000118 RID: 280
		private Vector3[] m_Corners = new Vector3[4];
	}
}
