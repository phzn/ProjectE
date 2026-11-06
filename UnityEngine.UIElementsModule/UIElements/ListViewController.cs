using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200003E RID: 62
	public class ListViewController : BaseListViewController
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600029F RID: 671 RVA: 0x00008A07 File Offset: 0x00006C07
		protected ListView listView
		{
			get
			{
				return base.view as ListView;
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00008A14 File Offset: 0x00006C14
		protected override VisualElement MakeItem()
		{
			bool flag = this.listView.makeItem == null;
			VisualElement visualElement;
			if (flag)
			{
				bool flag2 = this.listView.bindItem != null;
				if (flag2)
				{
					throw new NotImplementedException("You must specify makeItem if bindItem is specified.");
				}
				visualElement = new Label();
			}
			else
			{
				visualElement = this.listView.makeItem();
			}
			return visualElement;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00008A70 File Offset: 0x00006C70
		protected override void BindItem(VisualElement element, int index)
		{
			bool flag = this.listView.bindItem == null;
			if (flag)
			{
				bool flag2 = this.listView.makeItem != null;
				if (flag2)
				{
					throw new NotImplementedException("You must specify bindItem if makeItem is specified.");
				}
				Label label = (Label)element;
				object obj = this.listView.itemsSource[index];
				label.text = ((obj != null) ? obj.ToString() : null) ?? "null";
			}
			else
			{
				this.listView.bindItem(element, index);
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00008AF8 File Offset: 0x00006CF8
		protected override void UnbindItem(VisualElement element, int index)
		{
			Action<VisualElement, int> unbindItem = this.listView.unbindItem;
			if (unbindItem != null)
			{
				unbindItem(element, index);
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00008B14 File Offset: 0x00006D14
		protected override void DestroyItem(VisualElement element)
		{
			Action<VisualElement> destroyItem = this.listView.destroyItem;
			if (destroyItem != null)
			{
				destroyItem(element);
			}
		}
	}
}
