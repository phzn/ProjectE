using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x0200013F RID: 319
	[ExecuteAlways]
	public class UIFoldout : Toggle
	{
		// Token: 0x0600099F RID: 2463 RVA: 0x0002BCD5 File Offset: 0x00029ED5
		protected override void Start()
		{
			base.Start();
			this.onValueChanged.AddListener(new UnityAction<bool>(this.SetState));
			this.SetState(base.isOn);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0002BD00 File Offset: 0x00029F00
		private void OnValidate()
		{
			this.SetState(base.isOn, false);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0002BD0F File Offset: 0x00029F0F
		public void SetState(bool state)
		{
			this.SetState(state, true);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0002BD1C File Offset: 0x00029F1C
		public void SetState(bool state, bool rebuildLayout)
		{
			if (this.arrowOpened == null || this.arrowClosed == null || this.content == null)
			{
				return;
			}
			if (this.arrowOpened.activeSelf != state)
			{
				this.arrowOpened.SetActive(state);
			}
			if (this.arrowClosed.activeSelf == state)
			{
				this.arrowClosed.SetActive(!state);
			}
			if (this.content.activeSelf != state)
			{
				this.content.SetActive(state);
			}
			if (rebuildLayout)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(base.transform.parent as RectTransform);
			}
		}

		// Token: 0x0400058E RID: 1422
		public GameObject content;

		// Token: 0x0400058F RID: 1423
		public GameObject arrowOpened;

		// Token: 0x04000590 RID: 1424
		public GameObject arrowClosed;
	}
}
