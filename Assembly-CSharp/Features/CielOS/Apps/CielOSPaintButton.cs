using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Apps
{
	// Token: 0x020013FD RID: 5117
	[RequireComponent(typeof(Button))]
	public class CielOSPaintButton : MonoBehaviour
	{
		// Token: 0x06009DFD RID: 40445 RVA: 0x0040F82C File Offset: 0x0040DA2C
		private void Awake()
		{
			if (this.button == null)
			{
				this.button = base.GetComponent<Button>();
			}
			if (this.paint == null)
			{
				this.paint = base.GetComponentInParent<CielOSPaintApp>();
			}
			if (this.button != null)
			{
				this.button.onClick.AddListener(new UnityAction(this.Invoke));
			}
		}

		// Token: 0x06009DFE RID: 40446 RVA: 0x0040F898 File Offset: 0x0040DA98
		private void Invoke()
		{
			if (this.paint == null)
			{
				return;
			}
			switch (this.action)
			{
			case CielOSPaintButton.Action.BrushSmall:
				this.paint.SetBrushSmall();
				return;
			case CielOSPaintButton.Action.BrushMedium:
				this.paint.SetBrushMedium();
				return;
			case CielOSPaintButton.Action.BrushLarge:
				this.paint.SetBrushLarge();
				return;
			case CielOSPaintButton.Action.Clear:
				this.paint.ClearCanvas();
				return;
			case CielOSPaintButton.Action.Save:
				this.paint.Save();
				return;
			case CielOSPaintButton.Action.EditColor:
				this.paint.EditActiveColor();
				return;
			default:
				return;
			}
		}

		// Token: 0x0400A244 RID: 41540
		[SerializeField]
		private CielOSPaintButton.Action action;

		// Token: 0x0400A245 RID: 41541
		[SerializeField]
		private CielOSPaintApp paint;

		// Token: 0x0400A246 RID: 41542
		[SerializeField]
		private Button button;

		// Token: 0x020013FE RID: 5118
		public enum Action
		{
			// Token: 0x0400A248 RID: 41544
			BrushSmall,
			// Token: 0x0400A249 RID: 41545
			BrushMedium,
			// Token: 0x0400A24A RID: 41546
			BrushLarge,
			// Token: 0x0400A24B RID: 41547
			Clear,
			// Token: 0x0400A24C RID: 41548
			Save,
			// Token: 0x0400A24D RID: 41549
			EditColor
		}
	}
}
