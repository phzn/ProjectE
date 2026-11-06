using System;
using System.Diagnostics;
using UnityEngine.UIElements.Experimental;

namespace UnityEngine.UIElements
{
	// Token: 0x0200004B RID: 75
	internal class ReusableCollectionItem
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0000BC4E File Offset: 0x00009E4E
		public virtual VisualElement rootElement
		{
			get
			{
				return this.bindableElement;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600032B RID: 811 RVA: 0x0000BC56 File Offset: 0x00009E56
		// (set) Token: 0x0600032C RID: 812 RVA: 0x0000BC5E File Offset: 0x00009E5E
		public VisualElement bindableElement { get; protected set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000BC67 File Offset: 0x00009E67
		// (set) Token: 0x0600032E RID: 814 RVA: 0x0000BC6F File Offset: 0x00009E6F
		public ValueAnimation<StyleValues> animator { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000BC78 File Offset: 0x00009E78
		// (set) Token: 0x06000330 RID: 816 RVA: 0x0000BC80 File Offset: 0x00009E80
		public int index { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000BC89 File Offset: 0x00009E89
		// (set) Token: 0x06000332 RID: 818 RVA: 0x0000BC91 File Offset: 0x00009E91
		public int id { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000BC9A File Offset: 0x00009E9A
		// (set) Token: 0x06000334 RID: 820 RVA: 0x0000BCA2 File Offset: 0x00009EA2
		internal bool isDragGhost { get; private set; }

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000335 RID: 821 RVA: 0x0000BCAC File Offset: 0x00009EAC
		// (remove) Token: 0x06000336 RID: 822 RVA: 0x0000BCE4 File Offset: 0x00009EE4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<ReusableCollectionItem> onGeometryChanged;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000337 RID: 823 RVA: 0x0000BD1C File Offset: 0x00009F1C
		// (remove) Token: 0x06000338 RID: 824 RVA: 0x0000BD54 File Offset: 0x00009F54
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<ReusableCollectionItem> onDestroy;

		// Token: 0x06000339 RID: 825 RVA: 0x0000BD8C File Offset: 0x00009F8C
		public ReusableCollectionItem()
		{
			this.index = (this.id = -1);
			this.m_GeometryChangedEventCallback = new EventCallback<GeometryChangedEvent>(this.OnGeometryChanged);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000BDC5 File Offset: 0x00009FC5
		public virtual void Init(VisualElement item)
		{
			this.bindableElement = item;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000BDD0 File Offset: 0x00009FD0
		public virtual void PreAttachElement()
		{
			this.rootElement.AddToClassList(BaseVerticalCollectionView.itemUssClassName);
			this.rootElement.RegisterCallback<GeometryChangedEvent>(this.m_GeometryChangedEventCallback, TrickleDown.NoTrickleDown);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000BDF8 File Offset: 0x00009FF8
		public virtual void DetachElement()
		{
			this.rootElement.RemoveFromClassList(BaseVerticalCollectionView.itemUssClassName);
			this.rootElement.UnregisterCallback<GeometryChangedEvent>(this.m_GeometryChangedEventCallback, TrickleDown.NoTrickleDown);
			VisualElement rootElement = this.rootElement;
			if (rootElement != null)
			{
				rootElement.RemoveFromHierarchy();
			}
			this.SetSelected(false);
			this.SetDragGhost(false);
			this.index = (this.id = -1);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000BE5E File Offset: 0x0000A05E
		public virtual void DestroyElement()
		{
			Action<ReusableCollectionItem> action = this.onDestroy;
			if (action != null)
			{
				action(this);
			}
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000BE74 File Offset: 0x0000A074
		public virtual void SetSelected(bool selected)
		{
			if (selected)
			{
				this.rootElement.AddToClassList(BaseVerticalCollectionView.itemSelectedVariantUssClassName);
				this.rootElement.pseudoStates |= PseudoStates.Checked;
			}
			else
			{
				this.rootElement.RemoveFromClassList(BaseVerticalCollectionView.itemSelectedVariantUssClassName);
				this.rootElement.pseudoStates &= ~PseudoStates.Checked;
			}
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000BED8 File Offset: 0x0000A0D8
		public virtual void SetDragGhost(bool dragGhost)
		{
			this.isDragGhost = dragGhost;
			this.rootElement.style.maxHeight = (this.isDragGhost ? StyleKeyword.Undefined : StyleKeyword.Initial);
			this.bindableElement.style.display = (this.isDragGhost ? DisplayStyle.None : DisplayStyle.Flex);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000BF32 File Offset: 0x0000A132
		protected void OnGeometryChanged(GeometryChangedEvent evt)
		{
			Action<ReusableCollectionItem> action = this.onGeometryChanged;
			if (action != null)
			{
				action(this);
			}
		}

		// Token: 0x040000EE RID: 238
		public const int UndefinedIndex = -1;

		// Token: 0x040000F5 RID: 245
		protected EventCallback<GeometryChangedEvent> m_GeometryChangedEventCallback;
	}
}
