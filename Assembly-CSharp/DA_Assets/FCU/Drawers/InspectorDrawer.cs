using System;
using System.Collections.Generic;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU.Drawers
{
	// Token: 0x0200039D RID: 925
	[Serializable]
	public class InspectorDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600181D RID: 6173 RVA: 0x000996CC File Offset: 0x000978CC
		public SelectableFObject FillSelectableFramesArray(FObject document, int maxDepth = 2)
		{
			this._selectableHambItems.Clear();
			SelectableFObject selectableFObject = new SelectableFObject();
			this.FillNewSelectableItemRecursively(selectableFObject, document, 0, maxDepth);
			if (!this.CompareIdsRecursively(this._document, selectableFObject))
			{
				this._document = selectableFObject;
				this._document.SetAllSelected(true);
			}
			this.monoBeh.DelegateHolder.UpdateScrollContent();
			return this._document;
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x00099734 File Offset: 0x00097934
		public void FillNewSelectableItemRecursively(SelectableFObject parentItem, FObject parent, int currentDepth, int maxDepth)
		{
			parentItem.Id = parent.Id;
			parentItem.Type = parent.Type;
			parentItem.Name = parent.Name;
			if (currentDepth > maxDepth)
			{
				return;
			}
			if (parent.Children.IsEmpty<FObject>())
			{
				return;
			}
			foreach (FObject fobject in parent.Children)
			{
				if (this.IsAllowed(fobject, parent))
				{
					SelectableFObject selectableFObject = new SelectableFObject();
					this.FillNewSelectableItemRecursively(selectableFObject, fobject, currentDepth + 1, maxDepth);
					parentItem.Childs.Add(selectableFObject);
				}
			}
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x000997E8 File Offset: 0x000979E8
		private bool IsAllowed(FObject fobject, FObject parent)
		{
			List<FcuTag> list;
			this.monoBeh.TagSetter.TryGetManualTags(fobject, out list);
			return !list.Contains(FcuTag.Ignore) && fobject.IsVisible() && (fobject.Type == NodeType.CANVAS || parent.Type == NodeType.CANVAS);
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x0009983C File Offset: 0x00097A3C
		private bool CompareIdsRecursively(SelectableFObject item1, SelectableFObject item2)
		{
			if (item1.Id != item2.Id)
			{
				return false;
			}
			if (item1.Childs.Count != item2.Childs.Count)
			{
				return false;
			}
			for (int i = 0; i < item1.Childs.Count; i++)
			{
				if (!this.CompareIdsRecursively(item1.Childs[i], item2.Childs[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001821 RID: 6177 RVA: 0x000998B1 File Offset: 0x00097AB1
		public List<HamburgerItem> SelectableHamburgerItems
		{
			get
			{
				return this._selectableHambItems;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001822 RID: 6178 RVA: 0x000998B9 File Offset: 0x00097AB9
		public SelectableFObject SelectableDocument
		{
			get
			{
				return this._document;
			}
		}

		// Token: 0x0400197D RID: 6525
		[SerializeField]
		private List<HamburgerItem> _selectableHambItems = new List<HamburgerItem>();

		// Token: 0x0400197E RID: 6526
		[SerializeField]
		private SelectableFObject _document = new SelectableFObject();
	}
}
