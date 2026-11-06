using System;

namespace UnityEngine.UIElements.Internal
{
	// Token: 0x020004D0 RID: 1232
	internal class MultiColumnHeaderColumnSortIndicator : VisualElement
	{
		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x0600269E RID: 9886 RVA: 0x000A25D3 File Offset: 0x000A07D3
		// (set) Token: 0x0600269F RID: 9887 RVA: 0x000A25E0 File Offset: 0x000A07E0
		public string sortOrderLabel
		{
			get
			{
				return this.m_IndexLabel.text;
			}
			set
			{
				this.m_IndexLabel.text = value;
			}
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x000A25F0 File Offset: 0x000A07F0
		public MultiColumnHeaderColumnSortIndicator()
		{
			base.AddToClassList(MultiColumnHeaderColumnSortIndicator.ussClassName);
			base.pickingMode = PickingMode.Ignore;
			VisualElement visualElement = new VisualElement
			{
				pickingMode = PickingMode.Ignore
			};
			visualElement.AddToClassList(MultiColumnHeaderColumnSortIndicator.arrowUssClassName);
			base.Add(visualElement);
			this.m_IndexLabel = new Label
			{
				pickingMode = PickingMode.Ignore
			};
			this.m_IndexLabel.AddToClassList(MultiColumnHeaderColumnSortIndicator.indexLabelUssClassName);
			base.Add(this.m_IndexLabel);
		}

		// Token: 0x04001295 RID: 4757
		public static readonly string ussClassName = MultiColumnHeaderColumn.ussClassName + "__sort-indicator";

		// Token: 0x04001296 RID: 4758
		public static readonly string arrowUssClassName = MultiColumnHeaderColumnSortIndicator.ussClassName + "__arrow";

		// Token: 0x04001297 RID: 4759
		public static readonly string indexLabelUssClassName = MultiColumnHeaderColumnSortIndicator.ussClassName + "__index-label";

		// Token: 0x04001298 RID: 4760
		private Label m_IndexLabel;
	}
}
