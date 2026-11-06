using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000169 RID: 361
	internal struct DragAndDropArgs : IListDragAndDropArgs
	{
		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x0002E647 File Offset: 0x0002C847
		// (set) Token: 0x06000BC1 RID: 3009 RVA: 0x0002E64F File Offset: 0x0002C84F
		public object target { readonly get; set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x0002E658 File Offset: 0x0002C858
		// (set) Token: 0x06000BC3 RID: 3011 RVA: 0x0002E660 File Offset: 0x0002C860
		public int insertAtIndex { readonly get; set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x0002E669 File Offset: 0x0002C869
		// (set) Token: 0x06000BC5 RID: 3013 RVA: 0x0002E671 File Offset: 0x0002C871
		public int parentId { readonly get; set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x0002E67A File Offset: 0x0002C87A
		// (set) Token: 0x06000BC7 RID: 3015 RVA: 0x0002E682 File Offset: 0x0002C882
		public int childIndex { readonly get; set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x0002E68B File Offset: 0x0002C88B
		// (set) Token: 0x06000BC9 RID: 3017 RVA: 0x0002E693 File Offset: 0x0002C893
		public DragAndDropPosition dragAndDropPosition { readonly get; set; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0002E69C File Offset: 0x0002C89C
		// (set) Token: 0x06000BCB RID: 3019 RVA: 0x0002E6A4 File Offset: 0x0002C8A4
		public DragAndDropData dragAndDropData { readonly get; set; }
	}
}
