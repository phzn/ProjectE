using System;

namespace TMPro
{
	// Token: 0x02000027 RID: 39
	public static class TMP_Compatibility
	{
		// Token: 0x0600013B RID: 315 RVA: 0x0001778C File Offset: 0x0001598C
		public static TextAlignmentOptions ConvertTextAlignmentEnumValues(TextAlignmentOptions oldValue)
		{
			switch (oldValue)
			{
			case (TextAlignmentOptions)0:
				return TextAlignmentOptions.TopLeft;
			case (TextAlignmentOptions)1:
				return TextAlignmentOptions.Top;
			case (TextAlignmentOptions)2:
				return TextAlignmentOptions.TopRight;
			case (TextAlignmentOptions)3:
				return TextAlignmentOptions.TopJustified;
			case (TextAlignmentOptions)4:
				return TextAlignmentOptions.Left;
			case (TextAlignmentOptions)5:
				return TextAlignmentOptions.Center;
			case (TextAlignmentOptions)6:
				return TextAlignmentOptions.Right;
			case (TextAlignmentOptions)7:
				return TextAlignmentOptions.Justified;
			case (TextAlignmentOptions)8:
				return TextAlignmentOptions.BottomLeft;
			case (TextAlignmentOptions)9:
				return TextAlignmentOptions.Bottom;
			case (TextAlignmentOptions)10:
				return TextAlignmentOptions.BottomRight;
			case (TextAlignmentOptions)11:
				return TextAlignmentOptions.BottomJustified;
			case (TextAlignmentOptions)12:
				return TextAlignmentOptions.BaselineLeft;
			case (TextAlignmentOptions)13:
				return TextAlignmentOptions.Baseline;
			case (TextAlignmentOptions)14:
				return TextAlignmentOptions.BaselineRight;
			case (TextAlignmentOptions)15:
				return TextAlignmentOptions.BaselineJustified;
			case (TextAlignmentOptions)16:
				return TextAlignmentOptions.MidlineLeft;
			case (TextAlignmentOptions)17:
				return TextAlignmentOptions.Midline;
			case (TextAlignmentOptions)18:
				return TextAlignmentOptions.MidlineRight;
			case (TextAlignmentOptions)19:
				return TextAlignmentOptions.MidlineJustified;
			case (TextAlignmentOptions)20:
				return TextAlignmentOptions.CaplineLeft;
			case (TextAlignmentOptions)21:
				return TextAlignmentOptions.Capline;
			case (TextAlignmentOptions)22:
				return TextAlignmentOptions.CaplineRight;
			case (TextAlignmentOptions)23:
				return TextAlignmentOptions.CaplineJustified;
			default:
				return TextAlignmentOptions.TopLeft;
			}
		}

		// Token: 0x0200007A RID: 122
		public enum AnchorPositions
		{
			// Token: 0x0400057F RID: 1407
			TopLeft,
			// Token: 0x04000580 RID: 1408
			Top,
			// Token: 0x04000581 RID: 1409
			TopRight,
			// Token: 0x04000582 RID: 1410
			Left,
			// Token: 0x04000583 RID: 1411
			Center,
			// Token: 0x04000584 RID: 1412
			Right,
			// Token: 0x04000585 RID: 1413
			BottomLeft,
			// Token: 0x04000586 RID: 1414
			Bottom,
			// Token: 0x04000587 RID: 1415
			BottomRight,
			// Token: 0x04000588 RID: 1416
			BaseLine,
			// Token: 0x04000589 RID: 1417
			None
		}
	}
}
