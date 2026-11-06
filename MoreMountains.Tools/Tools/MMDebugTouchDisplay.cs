using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000042 RID: 66
	public class MMDebugTouchDisplay : MonoBehaviour
	{
		// Token: 0x0600014B RID: 331 RVA: 0x0000709C File Offset: 0x0000529C
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000070A4 File Offset: 0x000052A4
		protected virtual void Initialization()
		{
			this._touchDisplays = new List<RectTransform>();
			for (int i = 0; i < this.TouchProvision; i++)
			{
				RectTransform rectTransform = Object.Instantiate<RectTransform>(this.TouchPrefab);
				rectTransform.transform.SetParent(this.TargetCanvas.transform);
				rectTransform.name = "MMDebugTouchDisplay_" + i.ToString();
				rectTransform.gameObject.SetActive(false);
				this._touchDisplays.Add(rectTransform);
			}
			base.enabled = false;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00007125 File Offset: 0x00005325
		protected virtual void Update()
		{
			this.DisableAllDisplays();
			this.DetectTouches();
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007134 File Offset: 0x00005334
		protected virtual void DetectTouches()
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				this._touchDisplays[i].gameObject.SetActive(true);
				this._touchDisplays[i].position = Input.GetTouch(i).position;
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000718C File Offset: 0x0000538C
		protected virtual void DisableAllDisplays()
		{
			foreach (RectTransform rectTransform in this._touchDisplays)
			{
				rectTransform.gameObject.SetActive(false);
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000071E4 File Offset: 0x000053E4
		protected virtual void OnDisable()
		{
			this.DisableAllDisplays();
		}

		// Token: 0x040001BE RID: 446
		[Header("Bindings")]
		public Canvas TargetCanvas;

		// Token: 0x040001BF RID: 447
		[Header("Touches")]
		public RectTransform TouchPrefab;

		// Token: 0x040001C0 RID: 448
		public int TouchProvision = 6;

		// Token: 0x040001C1 RID: 449
		protected List<RectTransform> _touchDisplays;
	}
}
