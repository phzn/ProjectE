using System;
using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
	// Token: 0x02000416 RID: 1046
	internal class StyleMatchingContext
	{
		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x0600215B RID: 8539 RVA: 0x0007E323 File Offset: 0x0007C523
		public int styleSheetCount
		{
			get
			{
				return this.m_StyleSheetStack.Count;
			}
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x0007E330 File Offset: 0x0007C530
		public StyleMatchingContext(Action<VisualElement, MatchResultInfo> processResult)
		{
			this.m_StyleSheetStack = new List<StyleSheet>();
			this.variableContext = StyleVariableContext.none;
			this.currentElement = null;
			this.processResult = processResult;
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x0007E36C File Offset: 0x0007C56C
		public void AddStyleSheet(StyleSheet sheet)
		{
			bool flag = sheet == null;
			if (!flag)
			{
				this.m_StyleSheetStack.Add(sheet);
			}
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x0007E394 File Offset: 0x0007C594
		public void RemoveStyleSheetRange(int index, int count)
		{
			this.m_StyleSheetStack.RemoveRange(index, count);
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x0007E3A8 File Offset: 0x0007C5A8
		public StyleSheet GetStyleSheetAt(int index)
		{
			return this.m_StyleSheetStack[index];
		}

		// Token: 0x04000E2A RID: 3626
		private List<StyleSheet> m_StyleSheetStack;

		// Token: 0x04000E2B RID: 3627
		public StyleVariableContext variableContext;

		// Token: 0x04000E2C RID: 3628
		public VisualElement currentElement;

		// Token: 0x04000E2D RID: 3629
		public Action<VisualElement, MatchResultInfo> processResult;

		// Token: 0x04000E2E RID: 3630
		public AncestorFilter ancestorFilter = new AncestorFilter();
	}
}
