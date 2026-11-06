using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Drawing
{
	// Token: 0x02000045 RID: 69
	public static class GizmoContext
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000ADA1 File Offset: 0x00008FA1
		// (set) Token: 0x0600024F RID: 591 RVA: 0x0000ADAD File Offset: 0x00008FAD
		public static int selectionSize
		{
			get
			{
				GizmoContext.Refresh();
				return GizmoContext.selectionSizeInternal;
			}
			private set
			{
				GizmoContext.selectionSizeInternal = value;
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000ADB5 File Offset: 0x00008FB5
		internal static void SetDirty()
		{
			GizmoContext.dirty = true;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00002104 File Offset: 0x00000304
		private static void Refresh()
		{
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000ADBD File Offset: 0x00008FBD
		public static bool InSelection(Component c)
		{
			return GizmoContext.InSelection(c.transform);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000ADCC File Offset: 0x00008FCC
		public static bool InSelection(Transform tr)
		{
			GizmoContext.Refresh();
			Transform transform = tr;
			while (tr != null)
			{
				if (GizmoContext.selectedTransforms.Contains(tr))
				{
					GizmoContext.selectedTransforms.Add(transform);
					return true;
				}
				tr = tr.parent;
			}
			return false;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000AE0F File Offset: 0x0000900F
		public static bool InActiveSelection(Component c)
		{
			return GizmoContext.InActiveSelection(c.transform);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000AE1C File Offset: 0x0000901C
		public static bool InActiveSelection(Transform tr)
		{
			return false;
		}

		// Token: 0x04000100 RID: 256
		private static HashSet<Transform> selectedTransforms = new HashSet<Transform>();

		// Token: 0x04000101 RID: 257
		internal static bool drawingGizmos;

		// Token: 0x04000102 RID: 258
		internal static bool dirty;

		// Token: 0x04000103 RID: 259
		private static int selectionSizeInternal;
	}
}
