using System;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000230 RID: 560
	public class Ripple : MonoBehaviour
	{
		// Token: 0x06000F55 RID: 3925 RVA: 0x00065FE0 File Offset: 0x000641E0
		private void Start()
		{
			base.transform.localScale = new Vector3(0f, 0f, 0f);
			this.colorImg = base.GetComponent<Image>();
			this.colorImg.raycastTarget = false;
			this.colorImg.color = new Color(this.startColor.r, this.startColor.g, this.startColor.b, this.startColor.a);
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00066060 File Offset: 0x00064260
		private void Update()
		{
			if (!this.unscaledTime)
			{
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(this.maxSize, this.maxSize, this.maxSize), Time.deltaTime * this.speed);
				this.colorImg.color = Color.Lerp(this.colorImg.color, new Color(this.transitionColor.r, this.transitionColor.g, this.transitionColor.b, this.transitionColor.a), Time.deltaTime * this.speed);
				if ((double)base.transform.localScale.x >= (double)this.maxSize * 0.998)
				{
					if (base.transform.parent.childCount == 1)
					{
						base.transform.parent.gameObject.SetActive(false);
					}
					Object.Destroy(base.gameObject);
					return;
				}
			}
			else
			{
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(this.maxSize, this.maxSize, this.maxSize), Time.unscaledDeltaTime * this.speed);
				this.colorImg.color = Color.Lerp(this.colorImg.color, new Color(this.transitionColor.r, this.transitionColor.g, this.transitionColor.b, this.transitionColor.a), Time.unscaledDeltaTime * this.speed);
				if ((double)base.transform.localScale.x >= (double)this.maxSize * 0.998)
				{
					if (base.transform.parent.childCount == 1)
					{
						base.transform.parent.gameObject.SetActive(false);
					}
					Object.Destroy(base.gameObject);
				}
			}
		}

		// Token: 0x04001041 RID: 4161
		public bool unscaledTime;

		// Token: 0x04001042 RID: 4162
		public float speed;

		// Token: 0x04001043 RID: 4163
		public float maxSize;

		// Token: 0x04001044 RID: 4164
		public Color startColor;

		// Token: 0x04001045 RID: 4165
		public Color transitionColor;

		// Token: 0x04001046 RID: 4166
		private Image colorImg;
	}
}
