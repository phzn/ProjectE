using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Invector
{
	// Token: 0x020004CF RID: 1231
	public class vAutoScrollVertical : MonoBehaviour
	{
		// Token: 0x06001DAD RID: 7597 RVA: 0x000B5C7A File Offset: 0x000B3E7A
		public void Awake()
		{
			this.sr = base.gameObject.GetComponent<ScrollRect>();
			if (this.sr)
			{
				this.contentRect = this.sr.content;
			}
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x000B5CAB File Offset: 0x000B3EAB
		private void Update()
		{
			this.OnUpdateSelected();
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x000B5CB4 File Offset: 0x000B3EB4
		public void OnUpdateSelected()
		{
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject == null || currentSelectedGameObject.transform.parent != this.contentRect.transform)
			{
				return;
			}
			float height = this.sr.content.rect.height;
			float height2 = this.sr.viewport.rect.height;
			float y = currentSelectedGameObject.transform.localPosition.y;
			float num = y + currentSelectedGameObject.GetComponent<RectTransform>().rect.height / 2f;
			float num2 = y - currentSelectedGameObject.GetComponent<RectTransform>().rect.height / 2f;
			float num3 = (height - height2) * this.sr.normalizedPosition.y - height * 0.5f;
			float num4 = num3 + height2;
			float num5;
			if (num > num4)
			{
				num5 = num - height2 + currentSelectedGameObject.GetComponent<RectTransform>().rect.height;
			}
			else
			{
				if (num2 >= num3)
				{
					return;
				}
				num5 = num2 - currentSelectedGameObject.GetComponent<RectTransform>().rect.height;
			}
			float num6 = (num5 + height) / (height - height2);
			Vector2 vector = new Vector2(0f, Mathf.Clamp01(num6));
			this.sr.normalizedPosition = Vector2.Lerp(this.sr.normalizedPosition, vector, 10f * Time.fixedDeltaTime);
		}

		// Token: 0x04001ECD RID: 7885
		private ScrollRect sr;

		// Token: 0x04001ECE RID: 7886
		private RectTransform contentRect;
	}
}
