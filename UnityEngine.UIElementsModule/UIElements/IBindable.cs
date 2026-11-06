using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000073 RID: 115
	public interface IBindable
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000542 RID: 1346
		// (set) Token: 0x06000543 RID: 1347
		IBinding binding { get; set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000544 RID: 1348
		// (set) Token: 0x06000545 RID: 1349
		string bindingPath { get; set; }
	}
}
