using System;

namespace UnityEngine.UIElements.Internal
{
	// Token: 0x020004D1 RID: 1233
	internal class MultiColumnHeaderColumnIcon : Image
	{
		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x060026A2 RID: 9890 RVA: 0x000A26AA File Offset: 0x000A08AA
		// (set) Token: 0x060026A3 RID: 9891 RVA: 0x000A26B2 File Offset: 0x000A08B2
		public bool isImageInline { get; set; }

		// Token: 0x060026A4 RID: 9892 RVA: 0x000A26BB File Offset: 0x000A08BB
		public MultiColumnHeaderColumnIcon()
		{
			base.AddToClassList(MultiColumnHeaderColumnIcon.ussClassName);
			base.RegisterCallback<CustomStyleResolvedEvent>(delegate(CustomStyleResolvedEvent evt)
			{
				this.UpdateClassList();
			}, TrickleDown.NoTrickleDown);
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x000A26E8 File Offset: 0x000A08E8
		public void UpdateClassList()
		{
			base.parent.RemoveFromClassList(MultiColumnHeaderColumn.hasIconUssClassName);
			bool flag = base.image != null || base.sprite != null || base.vectorImage != null;
			if (flag)
			{
				base.parent.AddToClassList(MultiColumnHeaderColumn.hasIconUssClassName);
			}
		}

		// Token: 0x04001299 RID: 4761
		public new static readonly string ussClassName = MultiColumnHeaderColumn.ussClassName + "__icon";
	}
}
