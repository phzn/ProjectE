using System;
using ProjectExhibitionist.Core.SceneManagement;
using Unity.Profiling;
using UnityEngine;

namespace ProjectExhibitionist.PoiTravel
{
	// Token: 0x02000964 RID: 2404
	[DisallowMultipleComponent]
	public sealed class PoiStaticDetailBudget : MonoBehaviour
	{
		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x060041C2 RID: 16834 RVA: 0x00186FAA File Offset: 0x001851AA
		public int CellCount
		{
			get
			{
				return this.cells.Length;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x060041C3 RID: 16835 RVA: 0x00186FB4 File Offset: 0x001851B4
		// (set) Token: 0x060041C4 RID: 16836 RVA: 0x00186FBC File Offset: 0x001851BC
		public int RendererCount { get; private set; }

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x060041C5 RID: 16837 RVA: 0x00186FC5 File Offset: 0x001851C5
		// (set) Token: 0x060041C6 RID: 16838 RVA: 0x00186FCD File Offset: 0x001851CD
		public int SuppressedRenderers { get; private set; }

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x060041C7 RID: 16839 RVA: 0x00186FD6 File Offset: 0x001851D6
		// (set) Token: 0x060041C8 RID: 16840 RVA: 0x00186FDE File Offset: 0x001851DE
		public int ChangesLastFrame { get; private set; }

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x060041C9 RID: 16841 RVA: 0x00186FE7 File Offset: 0x001851E7
		// (set) Token: 0x060041CA RID: 16842 RVA: 0x00186FEF File Offset: 0x001851EF
		public float LastWorkMilliseconds { get; private set; }

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x060041CB RID: 16843 RVA: 0x00186FF8 File Offset: 0x001851F8
		// (set) Token: 0x060041CC RID: 16844 RVA: 0x00187000 File Offset: 0x00185200
		public float MaximumWorkMilliseconds { get; private set; }

		// Token: 0x060041CD RID: 16845 RVA: 0x0018700C File Offset: 0x0018520C
		private void OnEnable()
		{
			this.RendererCount = (this.SuppressedRenderers = (this.ChangesLastFrame = 0));
			this.MaximumWorkMilliseconds = (this.LastWorkMilliseconds = 0f);
			this.hideCursor = (this.checkCursor = 0);
			this.viewCamera = null;
			foreach (PoiStaticDetailBudget.Cell cell in this.cells)
			{
				cell.originalOff = new bool[cell.renderers.Length];
				cell.suppressed = new bool[cell.renderers.Length];
				cell.wantsHidden = false;
				cell.cursor = cell.renderers.Length;
				for (int j = 0; j < cell.renderers.Length; j++)
				{
					if (cell.renderers[j] != null)
					{
						cell.originalOff[j] = cell.renderers[j].forceRenderingOff;
						int rendererCount = this.RendererCount;
						this.RendererCount = rendererCount + 1;
					}
				}
			}
		}

		// Token: 0x060041CE RID: 16846 RVA: 0x00187114 File Offset: 0x00185314
		private void LateUpdate()
		{
			if (this.viewCamera == null || !this.viewCamera.isActiveAndEnabled)
			{
				this.viewCamera = Camera.main;
			}
			if (this.viewCamera == null)
			{
				return;
			}
			Transform player = PlayerRegistry.Player;
			this.Tick((player != null) ? player.position : this.viewCamera.transform.position, this.viewCamera.transform.position);
		}

		// Token: 0x060041CF RID: 16847 RVA: 0x00187194 File Offset: 0x00185394
		public void Tick(Vector3 playerPosition, Vector3 cameraPosition)
		{
			using (PoiStaticDetailBudget.TickMarker.Auto())
			{
				double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
				this.ChangesLastFrame = 0;
				Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
				int num = Mathf.Min(this.maxCellChecksPerFrame, this.cells.Length);
				for (int i = 0; i < num; i++)
				{
					PoiStaticDetailBudget.Cell cell = this.cells[this.checkCursor];
					this.checkCursor = (this.checkCursor + 1) % this.cells.Length;
					bool flag = PoiStaticDetailBudget.ShouldHide(PerformanceMapDetailController.TransformBounds(cell.localBounds, localToWorldMatrix), playerPosition, cameraPosition, cell.distance, this.distanceHysteresis, cell.wantsHidden);
					if (flag != cell.wantsHidden)
					{
						cell.wantsHidden = flag;
						cell.cursor = 0;
					}
				}
				int num2 = 0;
				while (num2 < this.cells.Length && this.HasBudget(realtimeSinceStartupAsDouble))
				{
					if (!this.cells[num2].wantsHidden)
					{
						this.ApplyCell(this.cells[num2], realtimeSinceStartupAsDouble);
					}
					num2++;
				}
				int num3 = 0;
				while (num3 < this.cells.Length && this.HasBudget(realtimeSinceStartupAsDouble))
				{
					PoiStaticDetailBudget.Cell cell2 = this.cells[this.hideCursor];
					this.hideCursor = (this.hideCursor + 1) % this.cells.Length;
					if (cell2.wantsHidden)
					{
						this.ApplyCell(cell2, realtimeSinceStartupAsDouble);
					}
					num3++;
				}
				this.LastWorkMilliseconds = (float)((Time.realtimeSinceStartupAsDouble - realtimeSinceStartupAsDouble) * 1000.0);
				this.MaximumWorkMilliseconds = Mathf.Max(this.MaximumWorkMilliseconds, this.LastWorkMilliseconds);
			}
		}

		// Token: 0x060041D0 RID: 16848 RVA: 0x00187350 File Offset: 0x00185550
		public static bool ShouldHide(Bounds bounds, Vector3 player, Vector3 camera, float distance, float hysteresis, bool hidden)
		{
			float num = distance + (hidden ? 0f : hysteresis);
			return Mathf.Min(bounds.SqrDistance(player), bounds.SqrDistance(camera)) > num * num;
		}

		// Token: 0x060041D1 RID: 16849 RVA: 0x00187387 File Offset: 0x00185587
		private bool HasBudget(double start)
		{
			return this.ChangesLastFrame < this.maxRendererChangesPerFrame && (Time.realtimeSinceStartupAsDouble - start) * 1000.0 < (double)this.workBudgetMilliseconds;
		}

		// Token: 0x060041D2 RID: 16850 RVA: 0x001873B4 File Offset: 0x001855B4
		private void ApplyCell(PoiStaticDetailBudget.Cell cell, double start)
		{
			while (cell.cursor < cell.renderers.Length && this.HasBudget(start))
			{
				int num = cell.cursor;
				cell.cursor = num + 1;
				int num2 = num;
				MeshRenderer meshRenderer = cell.renderers[num2];
				if (!(meshRenderer == null) && cell.suppressed[num2] != cell.wantsHidden)
				{
					cell.suppressed[num2] = cell.wantsHidden;
					meshRenderer.forceRenderingOff = cell.originalOff[num2] || cell.wantsHidden;
					this.SuppressedRenderers += (cell.wantsHidden ? 1 : (-1));
					num = this.ChangesLastFrame;
					this.ChangesLastFrame = num + 1;
				}
			}
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x00187468 File Offset: 0x00185668
		private void OnDisable()
		{
			foreach (PoiStaticDetailBudget.Cell cell in this.cells)
			{
				if (cell.originalOff != null)
				{
					for (int j = 0; j < cell.renderers.Length; j++)
					{
						if (cell.renderers[j] != null && cell.suppressed[j])
						{
							cell.renderers[j].forceRenderingOff = cell.originalOff[j];
						}
					}
				}
			}
			this.SuppressedRenderers = 0;
		}

		// Token: 0x040041AA RID: 16810
		[SerializeField]
		private PoiStaticDetailBudget.Cell[] cells = Array.Empty<PoiStaticDetailBudget.Cell>();

		// Token: 0x040041AB RID: 16811
		[SerializeField]
		[Min(1f)]
		private int maxRendererChangesPerFrame = 128;

		// Token: 0x040041AC RID: 16812
		[SerializeField]
		[Min(1f)]
		private int maxCellChecksPerFrame = 128;

		// Token: 0x040041AD RID: 16813
		[SerializeField]
		[Min(0.05f)]
		private float workBudgetMilliseconds = 0.75f;

		// Token: 0x040041AE RID: 16814
		[SerializeField]
		[Min(0f)]
		private float distanceHysteresis = 6f;

		// Token: 0x040041AF RID: 16815
		private static readonly ProfilerMarker TickMarker = new ProfilerMarker("POI.StaticDetail.Tick");

		// Token: 0x040041B0 RID: 16816
		private Camera viewCamera;

		// Token: 0x040041B1 RID: 16817
		private int hideCursor;

		// Token: 0x040041B2 RID: 16818
		private int checkCursor;

		// Token: 0x02000965 RID: 2405
		[Serializable]
		public sealed class Cell
		{
			// Token: 0x040041B8 RID: 16824
			public Bounds localBounds;

			// Token: 0x040041B9 RID: 16825
			public float distance = 36f;

			// Token: 0x040041BA RID: 16826
			public MeshRenderer[] renderers = Array.Empty<MeshRenderer>();

			// Token: 0x040041BB RID: 16827
			[NonSerialized]
			internal bool[] originalOff;

			// Token: 0x040041BC RID: 16828
			[NonSerialized]
			internal bool[] suppressed;

			// Token: 0x040041BD RID: 16829
			[NonSerialized]
			internal bool wantsHidden;

			// Token: 0x040041BE RID: 16830
			[NonSerialized]
			internal int cursor;
		}
	}
}
