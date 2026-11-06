using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000402 RID: 1026
	public class VisualElementFocusChangeDirection : FocusChangeDirection
	{
		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x060020E4 RID: 8420 RVA: 0x0007C79C File Offset: 0x0007A99C
		public static FocusChangeDirection left
		{
			get
			{
				return VisualElementFocusChangeDirection.s_Left;
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x060020E5 RID: 8421 RVA: 0x0007C7A3 File Offset: 0x0007A9A3
		public static FocusChangeDirection right
		{
			get
			{
				return VisualElementFocusChangeDirection.s_Right;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x060020E6 RID: 8422 RVA: 0x0007C7AC File Offset: 0x0007A9AC
		protected new static VisualElementFocusChangeDirection lastValue
		{
			get
			{
				return VisualElementFocusChangeDirection.s_Right;
			}
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x0003CF11 File Offset: 0x0003B111
		protected VisualElementFocusChangeDirection(int value)
			: base(value)
		{
		}

		// Token: 0x04000DEB RID: 3563
		private static readonly VisualElementFocusChangeDirection s_Left = new VisualElementFocusChangeDirection(FocusChangeDirection.lastValue + 1);

		// Token: 0x04000DEC RID: 3564
		private static readonly VisualElementFocusChangeDirection s_Right = new VisualElementFocusChangeDirection(FocusChangeDirection.lastValue + 2);
	}
}
