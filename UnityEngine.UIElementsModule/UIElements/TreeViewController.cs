using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000044 RID: 68
	public abstract class TreeViewController : BaseTreeViewController
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x00009317 File Offset: 0x00007517
		protected TreeView treeView
		{
			get
			{
				return base.view as TreeView;
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00009324 File Offset: 0x00007524
		protected override VisualElement MakeItem()
		{
			bool flag = this.treeView.makeItem == null;
			VisualElement visualElement;
			if (flag)
			{
				bool flag2 = this.treeView.bindItem != null;
				if (flag2)
				{
					throw new NotImplementedException("You must specify makeItem if bindItem is specified.");
				}
				visualElement = new Label();
			}
			else
			{
				visualElement = this.treeView.makeItem();
			}
			return visualElement;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00009380 File Offset: 0x00007580
		protected override void BindItem(VisualElement element, int index)
		{
			bool flag = this.treeView.bindItem == null;
			if (flag)
			{
				bool flag2 = this.treeView.makeItem != null;
				if (flag2)
				{
					throw new NotImplementedException("You must specify bindItem if makeItem is specified.");
				}
				Label label = (Label)element;
				object itemForIndex = this.GetItemForIndex(index);
				label.text = ((itemForIndex != null) ? itemForIndex.ToString() : null) ?? "null";
			}
			else
			{
				this.treeView.bindItem(element, index);
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x000093FE File Offset: 0x000075FE
		protected override void UnbindItem(VisualElement element, int index)
		{
			Action<VisualElement, int> unbindItem = this.treeView.unbindItem;
			if (unbindItem != null)
			{
				unbindItem(element, index);
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000941A File Offset: 0x0000761A
		protected override void DestroyItem(VisualElement element)
		{
			Action<VisualElement> destroyItem = this.treeView.destroyItem;
			if (destroyItem != null)
			{
				destroyItem(element);
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00009438 File Offset: 0x00007638
		internal override object GetItemForId(int id)
		{
			IDefaultTreeViewController defaultTreeViewController = this as IDefaultTreeViewController;
			bool flag = defaultTreeViewController != null;
			object obj;
			if (flag)
			{
				obj = defaultTreeViewController.GetItemDataForId(id);
			}
			else
			{
				obj = base.GetItemForId(id);
			}
			return obj;
		}
	}
}
