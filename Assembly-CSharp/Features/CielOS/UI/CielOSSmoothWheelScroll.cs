using System;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.CielOS.UI
{
	// Token: 0x020013CC RID: 5068
	public sealed class CielOSSmoothWheelScroll : MonoBehaviour, IScrollHandler, IEventSystemHandler, IBeginDragHandler, IControllerScrollSync
	{
		// Token: 0x06009C90 RID: 40080 RVA: 0x00402C3D File Offset: 0x00400E3D
		private void Awake()
		{
			this.Bind(base.GetComponent<ScrollRect>());
		}

		// Token: 0x06009C91 RID: 40081 RVA: 0x00402C4B File Offset: 0x00400E4B
		private void OnDisable()
		{
			this.ResetTarget();
		}

		// Token: 0x06009C92 RID: 40082 RVA: 0x00402C53 File Offset: 0x00400E53
		public void Bind(ScrollRect scroll)
		{
			this._scroll = scroll;
			if (this._scroll != null)
			{
				this._scroll.scrollSensitivity = 0f;
			}
			this.ResetTarget();
		}

		// Token: 0x06009C93 RID: 40083 RVA: 0x00402C80 File Offset: 0x00400E80
		public void SetWheelStep(float viewportFraction)
		{
			this.viewportFractionPerNotch = Mathf.Clamp(viewportFraction, 0.02f, 0.5f);
		}

		// Token: 0x06009C94 RID: 40084 RVA: 0x00402C98 File Offset: 0x00400E98
		public void ResetTarget()
		{
			this._hasTarget = false;
			if (this._scroll != null)
			{
				this._scroll.velocity = Vector2.zero;
			}
		}

		// Token: 0x06009C95 RID: 40085 RVA: 0x00402CC0 File Offset: 0x00400EC0
		public void OnScroll(PointerEventData eventData)
		{
			if (this._scroll == null || this._scroll.content == null || this._scroll.viewport == null || eventData == null)
			{
				return;
			}
			if (Mathf.Approximately(eventData.scrollDelta.y, 0f))
			{
				return;
			}
			float num = Mathf.Max(0f, this._scroll.content.rect.height - this._scroll.viewport.rect.height);
			if (!this._hasTarget)
			{
				this._targetY = Mathf.Clamp(this._scroll.content.anchoredPosition.y, 0f, num);
			}
			float num2 = Mathf.Max(24f, this._scroll.viewport.rect.height * this.viewportFractionPerNotch);
			this._targetY = Mathf.Clamp(this._targetY - Mathf.Sign(eventData.scrollDelta.y) * num2, 0f, num);
			this._hasTarget = true;
		}

		// Token: 0x06009C96 RID: 40086 RVA: 0x00402C4B File Offset: 0x00400E4B
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.ResetTarget();
		}

		// Token: 0x06009C97 RID: 40087 RVA: 0x00402C4B File Offset: 0x00400E4B
		public void SyncControllerScroll()
		{
			this.ResetTarget();
		}

		// Token: 0x06009C98 RID: 40088 RVA: 0x00402DE4 File Offset: 0x00400FE4
		private void LateUpdate()
		{
			if (!this._hasTarget || this._scroll == null || this._scroll.content == null || this._scroll.viewport == null)
			{
				return;
			}
			float num = Mathf.Max(0f, this._scroll.content.rect.height - this._scroll.viewport.rect.height);
			this._targetY = Mathf.Clamp(this._targetY, 0f, num);
			Vector2 anchoredPosition = this._scroll.content.anchoredPosition;
			float num2 = Mathf.Lerp(anchoredPosition.y, this._targetY, 1f - Mathf.Exp(-this.smoothing * Time.unscaledDeltaTime));
			if (Mathf.Abs(num2 - this._targetY) < 0.1f)
			{
				num2 = this._targetY;
				this._hasTarget = false;
			}
			this._scroll.velocity = Vector2.zero;
			this._scroll.content.anchoredPosition = new Vector2(anchoredPosition.x, num2);
		}

		// Token: 0x0400A130 RID: 41264
		[SerializeField]
		[Range(0.02f, 0.5f)]
		private float viewportFractionPerNotch = 0.12f;

		// Token: 0x0400A131 RID: 41265
		[SerializeField]
		[Min(1f)]
		private float smoothing = 14f;

		// Token: 0x0400A132 RID: 41266
		private ScrollRect _scroll;

		// Token: 0x0400A133 RID: 41267
		private float _targetY;

		// Token: 0x0400A134 RID: 41268
		private bool _hasTarget;
	}
}
