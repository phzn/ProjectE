using System;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x02000492 RID: 1170
	internal struct SelectorMatchRecord
	{
		// Token: 0x060024A6 RID: 9382 RVA: 0x00099912 File Offset: 0x00097B12
		public SelectorMatchRecord(StyleSheet sheet, int styleSheetIndexInStack)
		{
			this = default(SelectorMatchRecord);
			this.sheet = sheet;
			this.styleSheetIndexInStack = styleSheetIndexInStack;
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x0009992C File Offset: 0x00097B2C
		public static int Compare(SelectorMatchRecord a, SelectorMatchRecord b)
		{
			bool flag = a.sheet.isDefaultStyleSheet != b.sheet.isDefaultStyleSheet;
			int num;
			if (flag)
			{
				num = (a.sheet.isDefaultStyleSheet ? (-1) : 1);
			}
			else
			{
				int num2 = a.complexSelector.specificity.CompareTo(b.complexSelector.specificity);
				bool flag2 = num2 == 0;
				if (flag2)
				{
					num2 = a.styleSheetIndexInStack.CompareTo(b.styleSheetIndexInStack);
				}
				bool flag3 = num2 == 0;
				if (flag3)
				{
					num2 = a.complexSelector.orderInStyleSheet.CompareTo(b.complexSelector.orderInStyleSheet);
				}
				num = num2;
			}
			return num;
		}

		// Token: 0x04001195 RID: 4501
		public StyleSheet sheet;

		// Token: 0x04001196 RID: 4502
		public int styleSheetIndexInStack;

		// Token: 0x04001197 RID: 4503
		public StyleComplexSelector complexSelector;
	}
}
