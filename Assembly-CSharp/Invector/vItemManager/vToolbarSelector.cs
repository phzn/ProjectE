using System;
using Invector.vCharacterController;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x020005D8 RID: 1496
	public class vToolbarSelector : MonoBehaviour
	{
		// Token: 0x1400003D RID: 61
		// (add) Token: 0x060022D1 RID: 8913 RVA: 0x000C7A70 File Offset: 0x000C5C70
		// (remove) Token: 0x060022D2 RID: 8914 RVA: 0x000C7AA8 File Offset: 0x000C5CA8
		public event vToolbarSelector.OnSelectToolbar onSelect;

		// Token: 0x060022D3 RID: 8915 RVA: 0x000C7ADD File Offset: 0x000C5CDD
		protected virtual void GetMasterWindow()
		{
			if (!this.masterWindow)
			{
				this.masterWindow = base.transform.parent.GetComponentInParent<vMasterWindow>();
			}
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x000C7B04 File Offset: 0x000C5D04
		protected virtual void Awake()
		{
			for (int i = 0; i < this.selectables.Length; i++)
			{
				this.onSelect += this.selectables[i].OnSelectTool;
				int val = i;
				this.selectables[i].onSelect.AddListener(delegate
				{
					this.SelectToolbar(val);
				});
			}
			this.GetMasterWindow();
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x000C7B76 File Offset: 0x000C5D76
		public virtual void Update()
		{
			if (this.selectRightInput.GetButtonDown())
			{
				this.SelectRight();
				return;
			}
			if (this.selectLeftInput.GetButtonDown())
			{
				this.SelectLeft();
			}
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x000C7B9F File Offset: 0x000C5D9F
		public virtual void SelectRight()
		{
			this.index = (this.index + 1) % this.selectables.Length;
			this.onSelect(this.selectables[this.index]);
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x000C7BD0 File Offset: 0x000C5DD0
		public virtual void SelectLeft()
		{
			this.index--;
			if (this.index < 0)
			{
				this.index = this.selectables.Length - 1;
			}
			this.onSelect(this.selectables[this.index]);
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x000C7C1C File Offset: 0x000C5E1C
		public virtual void SelectToolbar(int index)
		{
			this.index = index;
			this.onSelect(this.selectables[this.index]);
			this.masterWindow.SetCurrentWindow(this.selectables[this.index].targetWindow);
		}

		// Token: 0x04002315 RID: 8981
		public GenericInput selectRightInput;

		// Token: 0x04002316 RID: 8982
		public GenericInput selectLeftInput;

		// Token: 0x04002317 RID: 8983
		public vMasterWindow masterWindow;

		// Token: 0x04002318 RID: 8984
		public vToolbarButton[] selectables;

		// Token: 0x04002319 RID: 8985
		public int index;

		// Token: 0x020005D9 RID: 1497
		// (Invoke) Token: 0x060022DB RID: 8923
		public delegate void OnSelectToolbar(vToolbarButton toolbarButton);
	}
}
