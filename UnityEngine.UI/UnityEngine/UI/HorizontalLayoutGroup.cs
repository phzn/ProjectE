using System;

namespace UnityEngine.UI
{
	// Token: 0x0200001E RID: 30
	[AddComponentMenu("Layout/Horizontal Layout Group", 150)]
	public class HorizontalLayoutGroup : HorizontalOrVerticalLayoutGroup
	{
		// Token: 0x06000256 RID: 598 RVA: 0x0000E079 File Offset: 0x0000C279
		protected HorizontalLayoutGroup()
		{
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000E081 File Offset: 0x0000C281
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			base.CalcAlongAxis(0, false);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000E091 File Offset: 0x0000C291
		public override void CalculateLayoutInputVertical()
		{
			base.CalcAlongAxis(1, false);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000E09B File Offset: 0x0000C29B
		public override void SetLayoutHorizontal()
		{
			base.SetChildrenAlongAxis(0, false);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000E0A5 File Offset: 0x0000C2A5
		public override void SetLayoutVertical()
		{
			base.SetChildrenAlongAxis(1, false);
		}
	}
}
