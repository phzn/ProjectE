using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200016F RID: 367
	internal readonly struct HandleDragAndDropArgs
	{
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x0002E6E4 File Offset: 0x0002C8E4
		public Vector2 position { get; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x0002E6EC File Offset: 0x0002C8EC
		public object target
		{
			get
			{
				return this.m_DragAndDropArgs.target;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x0002E6F9 File Offset: 0x0002C8F9
		public int insertAtIndex
		{
			get
			{
				return this.m_DragAndDropArgs.insertAtIndex;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x0002E706 File Offset: 0x0002C906
		public int parentId
		{
			get
			{
				return this.m_DragAndDropArgs.parentId;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x0002E713 File Offset: 0x0002C913
		public int childIndex
		{
			get
			{
				return this.m_DragAndDropArgs.childIndex;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x0002E720 File Offset: 0x0002C920
		public DragAndDropPosition dropPosition
		{
			get
			{
				return this.m_DragAndDropArgs.dragAndDropPosition;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x0002E72D File Offset: 0x0002C92D
		public DragAndDropData dragAndDropData
		{
			get
			{
				return this.m_DragAndDropArgs.dragAndDropData;
			}
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x0002E73A File Offset: 0x0002C93A
		internal HandleDragAndDropArgs(Vector2 position, DragAndDropArgs dragAndDropArgs)
		{
			this.position = position;
			this.m_DragAndDropArgs = dragAndDropArgs;
		}

		// Token: 0x04000595 RID: 1429
		private readonly DragAndDropArgs m_DragAndDropArgs;
	}
}
