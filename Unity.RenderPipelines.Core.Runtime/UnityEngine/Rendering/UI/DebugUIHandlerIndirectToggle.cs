using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x0200012B RID: 299
	public class DebugUIHandlerIndirectToggle : DebugUIHandlerWidget
	{
		// Token: 0x060008EE RID: 2286 RVA: 0x00029BAA File Offset: 0x00027DAA
		public void Init()
		{
			this.UpdateValueLabel();
			this.valueToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleValueChanged));
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00029BCE File Offset: 0x00027DCE
		private void OnToggleValueChanged(bool value)
		{
			this.setter(this.index, value);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00029BE2 File Offset: 0x00027DE2
		public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			this.nameLabel.color = this.colorSelected;
			this.checkmarkImage.color = this.colorSelected;
			return true;
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x00029C07 File Offset: 0x00027E07
		public override void OnDeselection()
		{
			this.nameLabel.color = this.colorDefault;
			this.checkmarkImage.color = this.colorDefault;
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00029C2C File Offset: 0x00027E2C
		public override void OnAction()
		{
			bool flag = !this.getter(this.index);
			this.setter(this.index, flag);
			this.UpdateValueLabel();
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00029C66 File Offset: 0x00027E66
		internal void UpdateValueLabel()
		{
			if (this.valueToggle != null)
			{
				this.valueToggle.isOn = this.getter(this.index);
			}
		}

		// Token: 0x04000537 RID: 1335
		public Text nameLabel;

		// Token: 0x04000538 RID: 1336
		public Toggle valueToggle;

		// Token: 0x04000539 RID: 1337
		public Image checkmarkImage;

		// Token: 0x0400053A RID: 1338
		public Func<int, bool> getter;

		// Token: 0x0400053B RID: 1339
		public Action<int, bool> setter;

		// Token: 0x0400053C RID: 1340
		internal int index;
	}
}
