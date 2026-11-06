using System;

namespace UnityEngine.UI
{
	// Token: 0x02000020 RID: 32
	public interface ILayoutElement
	{
		// Token: 0x0600026F RID: 623
		void CalculateLayoutInputHorizontal();

		// Token: 0x06000270 RID: 624
		void CalculateLayoutInputVertical();

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000271 RID: 625
		float minWidth { get; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000272 RID: 626
		float preferredWidth { get; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000273 RID: 627
		float flexibleWidth { get; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000274 RID: 628
		float minHeight { get; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000275 RID: 629
		float preferredHeight { get; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000276 RID: 630
		float flexibleHeight { get; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000277 RID: 631
		int layoutPriority { get; }
	}
}
