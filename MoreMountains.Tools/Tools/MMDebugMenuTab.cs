using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000044 RID: 68
	public class MMDebugMenuTab : MonoBehaviour
	{
		// Token: 0x0600015B RID: 347 RVA: 0x0000734D File Offset: 0x0000554D
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00007355 File Offset: 0x00005555
		protected virtual void Initialization()
		{
			if (this.ForceScaleOne)
			{
				base.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00007374 File Offset: 0x00005574
		public virtual void Select()
		{
			this.Manager.Select(this.Index);
			this.TabText.color = this.SelectedTextColor;
			this.TabBackground.color = this.SelectedBackgroundColor;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000073A9 File Offset: 0x000055A9
		public virtual void Deselect()
		{
			this.TabText.color = this.DeselectedTextColor;
			this.TabBackground.color = this.DeselectedBackgroundColor;
		}

		// Token: 0x040001CB RID: 459
		public Text TabText;

		// Token: 0x040001CC RID: 460
		public Image TabBackground;

		// Token: 0x040001CD RID: 461
		public Color SelectedBackgroundColor;

		// Token: 0x040001CE RID: 462
		public Color DeselectedBackgroundColor;

		// Token: 0x040001CF RID: 463
		public Color SelectedTextColor;

		// Token: 0x040001D0 RID: 464
		public Color DeselectedTextColor;

		// Token: 0x040001D1 RID: 465
		public int Index;

		// Token: 0x040001D2 RID: 466
		public MMDebugMenuTabManager Manager;

		// Token: 0x040001D3 RID: 467
		public bool ForceScaleOne = true;
	}
}
