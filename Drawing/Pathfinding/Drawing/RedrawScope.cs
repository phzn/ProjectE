using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Pathfinding.Drawing
{
	// Token: 0x0200002B RID: 43
	public struct RedrawScope : IDisposable
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x000084B5 File Offset: 0x000066B5
		public bool isValid
		{
			get
			{
				return this.id != 0;
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000084C0 File Offset: 0x000066C0
		internal RedrawScope(DrawingData gizmos, int id)
		{
			this.gizmos = gizmos.gizmosHandle;
			this.id = id;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000084D5 File Offset: 0x000066D5
		internal RedrawScope(DrawingData gizmos)
		{
			this.gizmos = gizmos.gizmosHandle;
			this.id = RedrawScope.idCounter++;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000084F8 File Offset: 0x000066F8
		internal void Draw()
		{
			if (this.gizmos.IsAllocated)
			{
				DrawingData drawingData = this.gizmos.Target as DrawingData;
				if (drawingData != null)
				{
					drawingData.Draw(this);
				}
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00008534 File Offset: 0x00006734
		public void Rewind()
		{
			GameObject gameObject = null;
			if (this.gizmos.IsAllocated)
			{
				DrawingData drawingData = this.gizmos.Target as DrawingData;
				if (drawingData != null)
				{
					gameObject = drawingData.GetAssociatedGameObject(this);
				}
			}
			this.Dispose();
			this = DrawingManager.GetRedrawScope(gameObject);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00008584 File Offset: 0x00006784
		internal void DrawUntilDispose(GameObject associatedGameObject)
		{
			DrawingData drawingData = this.gizmos.Target as DrawingData;
			if (drawingData != null)
			{
				drawingData.DrawUntilDisposed(this, associatedGameObject);
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000085B4 File Offset: 0x000067B4
		public void Dispose()
		{
			if (this.gizmos.IsAllocated)
			{
				DrawingData drawingData = this.gizmos.Target as DrawingData;
				if (drawingData != null)
				{
					drawingData.DisposeRedrawScope(this);
				}
			}
			this.gizmos = default(GCHandle);
			this.id = 0;
		}

		// Token: 0x04000084 RID: 132
		internal GCHandle gizmos;

		// Token: 0x04000085 RID: 133
		internal int id;

		// Token: 0x04000086 RID: 134
		private static int idCounter = 1;
	}
}
