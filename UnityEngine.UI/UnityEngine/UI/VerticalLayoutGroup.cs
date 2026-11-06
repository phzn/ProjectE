using System;

namespace UnityEngine.UI
{
	// Token: 0x02000029 RID: 41
	[AddComponentMenu("Layout/Vertical Layout Group", 151)]
	public class VerticalLayoutGroup : HorizontalOrVerticalLayoutGroup
	{
		// Token: 0x060002D8 RID: 728 RVA: 0x0000F570 File Offset: 0x0000D770
		protected VerticalLayoutGroup()
		{
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000F578 File Offset: 0x0000D778
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			base.CalcAlongAxis(0, true);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000F588 File Offset: 0x0000D788
		public override void CalculateLayoutInputVertical()
		{
			base.CalcAlongAxis(1, true);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000F592 File Offset: 0x0000D792
		public override void SetLayoutHorizontal()
		{
			base.SetChildrenAlongAxis(0, true);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000F59C File Offset: 0x0000D79C
		public override void SetLayoutVertical()
		{
			base.SetChildrenAlongAxis(1, true);
		}
	}
}
