using System;
using UnityEngine.UI.Collections;

namespace UnityEngine.UI
{
	// Token: 0x02000007 RID: 7
	public class CanvasUpdateRegistry
	{
		// Token: 0x0600001A RID: 26 RVA: 0x000021F4 File Offset: 0x000003F4
		protected CanvasUpdateRegistry()
		{
			Canvas.willRenderCanvases += this.PerformUpdate;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002262 File Offset: 0x00000462
		public static CanvasUpdateRegistry instance
		{
			get
			{
				if (CanvasUpdateRegistry.s_Instance == null)
				{
					CanvasUpdateRegistry.s_Instance = new CanvasUpdateRegistry();
				}
				return CanvasUpdateRegistry.s_Instance;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000227C File Offset: 0x0000047C
		private bool ObjectValidForUpdate(ICanvasElement element)
		{
			bool flag = element != null;
			if (element is Object)
			{
				flag = element as Object != null;
			}
			return flag;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000022A8 File Offset: 0x000004A8
		private void CleanInvalidItems()
		{
			for (int i = this.m_LayoutRebuildQueue.Count - 1; i >= 0; i--)
			{
				ICanvasElement canvasElement = this.m_LayoutRebuildQueue[i];
				if (canvasElement == null)
				{
					this.m_LayoutRebuildQueue.RemoveAt(i);
				}
				else if (canvasElement.IsDestroyed())
				{
					this.m_LayoutRebuildQueue.RemoveAt(i);
					canvasElement.LayoutComplete();
				}
			}
			for (int j = this.m_GraphicRebuildQueue.Count - 1; j >= 0; j--)
			{
				ICanvasElement canvasElement2 = this.m_GraphicRebuildQueue[j];
				if (canvasElement2 == null)
				{
					this.m_GraphicRebuildQueue.RemoveAt(j);
				}
				else if (canvasElement2.IsDestroyed())
				{
					this.m_GraphicRebuildQueue.RemoveAt(j);
					canvasElement2.GraphicUpdateComplete();
				}
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002358 File Offset: 0x00000558
		private void PerformUpdate()
		{
			UISystemProfilerApi.BeginSample(UISystemProfilerApi.SampleType.Layout);
			this.CleanInvalidItems();
			this.m_PerformingLayoutUpdate = true;
			this.m_LayoutRebuildQueue.Sort(CanvasUpdateRegistry.s_SortLayoutFunction);
			for (int i = 0; i <= 2; i++)
			{
				for (int j = 0; j < this.m_LayoutRebuildQueue.Count; j++)
				{
					ICanvasElement canvasElement = this.m_LayoutRebuildQueue[j];
					try
					{
						if (this.ObjectValidForUpdate(canvasElement))
						{
							canvasElement.Rebuild((CanvasUpdate)i);
						}
					}
					catch (Exception ex)
					{
						Debug.LogException(ex, canvasElement.transform);
					}
				}
			}
			for (int k = 0; k < this.m_LayoutRebuildQueue.Count; k++)
			{
				this.m_LayoutRebuildQueue[k].LayoutComplete();
			}
			this.m_LayoutRebuildQueue.Clear();
			this.m_PerformingLayoutUpdate = false;
			UISystemProfilerApi.EndSample(UISystemProfilerApi.SampleType.Layout);
			UISystemProfilerApi.BeginSample(UISystemProfilerApi.SampleType.Render);
			ClipperRegistry.instance.Cull();
			this.m_PerformingGraphicUpdate = true;
			for (int l = 3; l < 5; l++)
			{
				for (int m = 0; m < this.m_GraphicRebuildQueue.Count; m++)
				{
					try
					{
						ICanvasElement canvasElement2 = this.m_GraphicRebuildQueue[m];
						if (this.ObjectValidForUpdate(canvasElement2))
						{
							canvasElement2.Rebuild((CanvasUpdate)l);
						}
					}
					catch (Exception ex2)
					{
						Debug.LogException(ex2, this.m_GraphicRebuildQueue[m].transform);
					}
				}
			}
			for (int n = 0; n < this.m_GraphicRebuildQueue.Count; n++)
			{
				this.m_GraphicRebuildQueue[n].GraphicUpdateComplete();
			}
			this.m_GraphicRebuildQueue.Clear();
			this.m_PerformingGraphicUpdate = false;
			UISystemProfilerApi.EndSample(UISystemProfilerApi.SampleType.Render);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000024F4 File Offset: 0x000006F4
		private static int ParentCount(Transform child)
		{
			if (child == null)
			{
				return 0;
			}
			Transform transform = child.parent;
			int num = 0;
			while (transform != null)
			{
				num++;
				transform = transform.parent;
			}
			return num;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000252C File Offset: 0x0000072C
		private static int SortLayoutList(ICanvasElement x, ICanvasElement y)
		{
			Transform transform = x.transform;
			Transform transform2 = y.transform;
			return CanvasUpdateRegistry.ParentCount(transform) - CanvasUpdateRegistry.ParentCount(transform2);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002552 File Offset: 0x00000752
		public static void RegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			CanvasUpdateRegistry.instance.InternalRegisterCanvasElementForLayoutRebuild(element);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002560 File Offset: 0x00000760
		public static bool TryRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			return CanvasUpdateRegistry.instance.InternalRegisterCanvasElementForLayoutRebuild(element);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000256D File Offset: 0x0000076D
		private bool InternalRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			return !this.m_LayoutRebuildQueue.Contains(element) && this.m_LayoutRebuildQueue.AddUnique(element, true);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000258C File Offset: 0x0000078C
		public static void RegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			CanvasUpdateRegistry.instance.InternalRegisterCanvasElementForGraphicRebuild(element);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000259A File Offset: 0x0000079A
		public static bool TryRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			return CanvasUpdateRegistry.instance.InternalRegisterCanvasElementForGraphicRebuild(element);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000025A7 File Offset: 0x000007A7
		private bool InternalRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			if (this.m_PerformingGraphicUpdate)
			{
				Debug.LogError(string.Format("Trying to add {0} for graphic rebuild while we are already inside a graphic rebuild loop. This is not supported.", element));
				return false;
			}
			return this.m_GraphicRebuildQueue.AddUnique(element, true);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000025D0 File Offset: 0x000007D0
		public static void UnRegisterCanvasElementForRebuild(ICanvasElement element)
		{
			CanvasUpdateRegistry.instance.InternalUnRegisterCanvasElementForLayoutRebuild(element);
			CanvasUpdateRegistry.instance.InternalUnRegisterCanvasElementForGraphicRebuild(element);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000025E8 File Offset: 0x000007E8
		public static void DisableCanvasElementForRebuild(ICanvasElement element)
		{
			CanvasUpdateRegistry.instance.InternalDisableCanvasElementForLayoutRebuild(element);
			CanvasUpdateRegistry.instance.InternalDisableCanvasElementForGraphicRebuild(element);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002600 File Offset: 0x00000800
		private void InternalUnRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			if (this.m_PerformingLayoutUpdate)
			{
				Debug.LogError(string.Format("Trying to remove {0} from rebuild list while we are already inside a rebuild loop. This is not supported.", element));
				return;
			}
			element.LayoutComplete();
			CanvasUpdateRegistry.instance.m_LayoutRebuildQueue.Remove(element);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002632 File Offset: 0x00000832
		private void InternalUnRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			if (this.m_PerformingGraphicUpdate)
			{
				Debug.LogError(string.Format("Trying to remove {0} from rebuild list while we are already inside a rebuild loop. This is not supported.", element));
				return;
			}
			element.GraphicUpdateComplete();
			CanvasUpdateRegistry.instance.m_GraphicRebuildQueue.Remove(element);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002664 File Offset: 0x00000864
		private void InternalDisableCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			if (this.m_PerformingLayoutUpdate)
			{
				Debug.LogError(string.Format("Trying to remove {0} from rebuild list while we are already inside a rebuild loop. This is not supported.", element));
				return;
			}
			element.LayoutComplete();
			CanvasUpdateRegistry.instance.m_LayoutRebuildQueue.DisableItem(element);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002696 File Offset: 0x00000896
		private void InternalDisableCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			if (this.m_PerformingGraphicUpdate)
			{
				Debug.LogError(string.Format("Trying to remove {0} from rebuild list while we are already inside a rebuild loop. This is not supported.", element));
				return;
			}
			element.GraphicUpdateComplete();
			CanvasUpdateRegistry.instance.m_GraphicRebuildQueue.DisableItem(element);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000026C8 File Offset: 0x000008C8
		public static bool IsRebuildingLayout()
		{
			return CanvasUpdateRegistry.instance.m_PerformingLayoutUpdate;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000026D4 File Offset: 0x000008D4
		public static bool IsRebuildingGraphics()
		{
			return CanvasUpdateRegistry.instance.m_PerformingGraphicUpdate;
		}

		// Token: 0x04000013 RID: 19
		private static CanvasUpdateRegistry s_Instance;

		// Token: 0x04000014 RID: 20
		private bool m_PerformingLayoutUpdate;

		// Token: 0x04000015 RID: 21
		private bool m_PerformingGraphicUpdate;

		// Token: 0x04000016 RID: 22
		private string[] m_CanvasUpdateProfilerStrings = new string[] { "CanvasUpdate.Prelayout", "CanvasUpdate.Layout", "CanvasUpdate.PostLayout", "CanvasUpdate.PreRender", "CanvasUpdate.LatePreRender" };

		// Token: 0x04000017 RID: 23
		private const string m_CullingUpdateProfilerString = "ClipperRegistry.Cull";

		// Token: 0x04000018 RID: 24
		private readonly IndexedSet<ICanvasElement> m_LayoutRebuildQueue = new IndexedSet<ICanvasElement>();

		// Token: 0x04000019 RID: 25
		private readonly IndexedSet<ICanvasElement> m_GraphicRebuildQueue = new IndexedSet<ICanvasElement>();

		// Token: 0x0400001A RID: 26
		private static readonly Comparison<ICanvasElement> s_SortLayoutFunction = new Comparison<ICanvasElement>(CanvasUpdateRegistry.SortLayoutList);
	}
}
