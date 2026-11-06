using System;
using UnityEngine;
using UnityEngine.UI;

namespace Invector
{
	// Token: 0x020004D5 RID: 1237
	public class vGridLayoutExpand : MonoBehaviour
	{
		// Token: 0x06001DDA RID: 7642 RVA: 0x000B6A6E File Offset: 0x000B4C6E
		private void Start()
		{
			this.grid = base.GetComponent<GridLayoutGroup>();
			this.rect = base.GetComponent<RectTransform>();
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x000B6A88 File Offset: 0x000B4C88
		private void OnDrawGizmos()
		{
			if (Application.isPlaying)
			{
				return;
			}
			this.grid = base.GetComponent<GridLayoutGroup>();
			this.rect = base.GetComponent<RectTransform>();
			this.UpdateBottomSize();
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x000B6AB0 File Offset: 0x000B4CB0
		private void UpdateBottomSize()
		{
			double num = (double)this.rect.childCount / (double)this.count;
			this.multiple = (float)Math.Round(num, MidpointRounding.AwayFromZero) + 1f;
			if (this.multiple != this.oldMultiple)
			{
				Vector2 offsetMin = this.rect.offsetMin;
				float num2 = (this.grid.cellSize.y + this.grid.spacing.y) * -this.multiple;
				offsetMin.y = num2;
				this.rect.offsetMin = offsetMin;
				this.oldMultiple = this.multiple;
			}
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x000B6B4A File Offset: 0x000B4D4A
		private void Update()
		{
			this.UpdateBottomSize();
		}

		// Token: 0x04001EE9 RID: 7913
		private GridLayoutGroup grid;

		// Token: 0x04001EEA RID: 7914
		public int count;

		// Token: 0x04001EEB RID: 7915
		private RectTransform rect;

		// Token: 0x04001EEC RID: 7916
		private float multiple;

		// Token: 0x04001EED RID: 7917
		private float oldMultiple;
	}
}
