using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003EF RID: 1007
	[Flags]
	internal enum VisualElementFlags
	{
		// Token: 0x04000DAD RID: 3501
		WorldTransformDirty = 1,
		// Token: 0x04000DAE RID: 3502
		WorldTransformInverseDirty = 2,
		// Token: 0x04000DAF RID: 3503
		WorldClipDirty = 4,
		// Token: 0x04000DB0 RID: 3504
		BoundingBoxDirty = 8,
		// Token: 0x04000DB1 RID: 3505
		WorldBoundingBoxDirty = 16,
		// Token: 0x04000DB2 RID: 3506
		EventCallbackParentCategoriesDirty = 32,
		// Token: 0x04000DB3 RID: 3507
		LayoutManual = 64,
		// Token: 0x04000DB4 RID: 3508
		CompositeRoot = 128,
		// Token: 0x04000DB5 RID: 3509
		RequireMeasureFunction = 256,
		// Token: 0x04000DB6 RID: 3510
		EnableViewDataPersistence = 512,
		// Token: 0x04000DB7 RID: 3511
		DisableClipping = 1024,
		// Token: 0x04000DB8 RID: 3512
		NeedsAttachToPanelEvent = 2048,
		// Token: 0x04000DB9 RID: 3513
		HierarchyDisplayed = 4096,
		// Token: 0x04000DBA RID: 3514
		StyleInitialized = 8192,
		// Token: 0x04000DBB RID: 3515
		Init = 4159
	}
}
