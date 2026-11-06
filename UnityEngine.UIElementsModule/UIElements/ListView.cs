using System;
using System.Collections;

namespace UnityEngine.UIElements
{
	// Token: 0x020000E1 RID: 225
	public class ListView : BaseListView
	{
		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x0001D7F5 File Offset: 0x0001B9F5
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x0001D800 File Offset: 0x0001BA00
		public new Func<VisualElement> makeItem
		{
			get
			{
				return this.m_MakeItem;
			}
			set
			{
				bool flag = value != this.m_MakeItem;
				if (flag)
				{
					this.m_MakeItem = value;
					base.Rebuild();
				}
			}
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0001D82E File Offset: 0x0001BA2E
		internal void SetMakeItemWithoutNotify(Func<VisualElement> func)
		{
			this.m_MakeItem = func;
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x0001D838 File Offset: 0x0001BA38
		// (set) Token: 0x060007B4 RID: 1972 RVA: 0x0001D840 File Offset: 0x0001BA40
		public new Action<VisualElement, int> bindItem
		{
			get
			{
				return this.m_BindItem;
			}
			set
			{
				bool flag = value != this.m_BindItem;
				if (flag)
				{
					this.m_BindItem = value;
					base.RefreshItems();
				}
			}
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0001D86E File Offset: 0x0001BA6E
		internal void SetBindItemWithoutNotify(Action<VisualElement, int> callback)
		{
			this.m_BindItem = callback;
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0001D878 File Offset: 0x0001BA78
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x0001D880 File Offset: 0x0001BA80
		public new Action<VisualElement, int> unbindItem { get; set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x0001D889 File Offset: 0x0001BA89
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x0001D891 File Offset: 0x0001BA91
		public new Action<VisualElement> destroyItem { get; set; }

		// Token: 0x060007BA RID: 1978 RVA: 0x0001D89C File Offset: 0x0001BA9C
		internal override bool HasValidDataAndBindings()
		{
			return base.HasValidDataAndBindings() && this.makeItem != null == (this.bindItem != null);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0001D8CD File Offset: 0x0001BACD
		protected override CollectionViewController CreateViewController()
		{
			return new ListViewController();
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0001D8D4 File Offset: 0x0001BAD4
		public ListView()
		{
			base.AddToClassList(BaseListView.ussClassName);
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0001D8EA File Offset: 0x0001BAEA
		public ListView(IList itemsSource, float itemHeight = -1f, Func<VisualElement> makeItem = null, Action<VisualElement, int> bindItem = null)
			: base(itemsSource, itemHeight)
		{
			base.AddToClassList(BaseListView.ussClassName);
			this.makeItem = makeItem;
			this.bindItem = bindItem;
		}

		// Token: 0x04000353 RID: 851
		private Func<VisualElement> m_MakeItem;

		// Token: 0x04000354 RID: 852
		private Action<VisualElement, int> m_BindItem;

		// Token: 0x020000E2 RID: 226
		public new class UxmlFactory : UxmlFactory<ListView, ListView.UxmlTraits>
		{
		}

		// Token: 0x020000E3 RID: 227
		public new class UxmlTraits : BaseListView.UxmlTraits
		{
		}
	}
}
