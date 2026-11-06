using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x02000073 RID: 115
	public class TMP_UpdateRegistry
	{
		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x00038110 File Offset: 0x00036310
		public static TMP_UpdateRegistry instance
		{
			get
			{
				if (TMP_UpdateRegistry.s_Instance == null)
				{
					TMP_UpdateRegistry.s_Instance = new TMP_UpdateRegistry();
				}
				return TMP_UpdateRegistry.s_Instance;
			}
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00038128 File Offset: 0x00036328
		protected TMP_UpdateRegistry()
		{
			Canvas.willRenderCanvases += this.PerformUpdateForCanvasRendererObjects;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00038178 File Offset: 0x00036378
		public static void RegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			TMP_UpdateRegistry.instance.InternalRegisterCanvasElementForLayoutRebuild(element);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00038188 File Offset: 0x00036388
		private bool InternalRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			int instanceID = (element as Object).GetInstanceID();
			if (this.m_LayoutQueueLookup.Contains(instanceID))
			{
				return false;
			}
			this.m_LayoutQueueLookup.Add(instanceID);
			this.m_LayoutRebuildQueue.Add(element);
			return true;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000381CB File Offset: 0x000363CB
		public static void RegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			TMP_UpdateRegistry.instance.InternalRegisterCanvasElementForGraphicRebuild(element);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000381DC File Offset: 0x000363DC
		private bool InternalRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			int instanceID = (element as Object).GetInstanceID();
			if (this.m_GraphicQueueLookup.Contains(instanceID))
			{
				return false;
			}
			this.m_GraphicQueueLookup.Add(instanceID);
			this.m_GraphicRebuildQueue.Add(element);
			return true;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00038220 File Offset: 0x00036420
		private void PerformUpdateForCanvasRendererObjects()
		{
			for (int i = 0; i < this.m_LayoutRebuildQueue.Count; i++)
			{
				TMP_UpdateRegistry.instance.m_LayoutRebuildQueue[i].Rebuild(CanvasUpdate.Prelayout);
			}
			if (this.m_LayoutRebuildQueue.Count > 0)
			{
				this.m_LayoutRebuildQueue.Clear();
				this.m_LayoutQueueLookup.Clear();
			}
			for (int j = 0; j < this.m_GraphicRebuildQueue.Count; j++)
			{
				TMP_UpdateRegistry.instance.m_GraphicRebuildQueue[j].Rebuild(CanvasUpdate.PreRender);
			}
			if (this.m_GraphicRebuildQueue.Count > 0)
			{
				this.m_GraphicRebuildQueue.Clear();
				this.m_GraphicQueueLookup.Clear();
			}
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x000382CD File Offset: 0x000364CD
		private void PerformUpdateForMeshRendererObjects()
		{
			Debug.Log("Perform update of MeshRenderer objects.");
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x000382D9 File Offset: 0x000364D9
		public static void UnRegisterCanvasElementForRebuild(ICanvasElement element)
		{
			TMP_UpdateRegistry.instance.InternalUnRegisterCanvasElementForLayoutRebuild(element);
			TMP_UpdateRegistry.instance.InternalUnRegisterCanvasElementForGraphicRebuild(element);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x000382F4 File Offset: 0x000364F4
		private void InternalUnRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			int instanceID = (element as Object).GetInstanceID();
			TMP_UpdateRegistry.instance.m_LayoutRebuildQueue.Remove(element);
			this.m_GraphicQueueLookup.Remove(instanceID);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0003832C File Offset: 0x0003652C
		private void InternalUnRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			int instanceID = (element as Object).GetInstanceID();
			TMP_UpdateRegistry.instance.m_GraphicRebuildQueue.Remove(element);
			this.m_LayoutQueueLookup.Remove(instanceID);
		}

		// Token: 0x04000568 RID: 1384
		private static TMP_UpdateRegistry s_Instance;

		// Token: 0x04000569 RID: 1385
		private readonly List<ICanvasElement> m_LayoutRebuildQueue = new List<ICanvasElement>();

		// Token: 0x0400056A RID: 1386
		private HashSet<int> m_LayoutQueueLookup = new HashSet<int>();

		// Token: 0x0400056B RID: 1387
		private readonly List<ICanvasElement> m_GraphicRebuildQueue = new List<ICanvasElement>();

		// Token: 0x0400056C RID: 1388
		private HashSet<int> m_GraphicQueueLookup = new HashSet<int>();
	}
}
