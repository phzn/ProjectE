using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Tutorials
{
	// Token: 0x02000BCC RID: 3020
	[DisallowMultipleComponent]
	public sealed class SmoothTutorialScrollRect : ScrollRect
	{
		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x060056E8 RID: 22248 RVA: 0x0021D2B2 File Offset: 0x0021B4B2
		public float ScrollStepPixels
		{
			get
			{
				return this.scrollStepPixels;
			}
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x060056E9 RID: 22249 RVA: 0x0021D2BA File Offset: 0x0021B4BA
		public float SmoothingTime
		{
			get
			{
				return this.smoothingTime;
			}
		}

		// Token: 0x060056EA RID: 22250 RVA: 0x0021D2C2 File Offset: 0x0021B4C2
		public void Configure(float stepPixels, float smoothTime)
		{
			this.scrollStepPixels = Mathf.Max(1f, stepPixels);
			this.smoothingTime = Mathf.Max(0.01f, smoothTime);
		}

		// Token: 0x060056EB RID: 22251 RVA: 0x0021D2E6 File Offset: 0x0021B4E6
		public void JumpTo(float normalizedPosition)
		{
			this.CancelWheelScroll();
			base.verticalNormalizedPosition = Mathf.Clamp01(normalizedPosition);
		}

		// Token: 0x060056EC RID: 22252 RVA: 0x0021D2FC File Offset: 0x0021B4FC
		public override void OnScroll(PointerEventData eventData)
		{
			if (!this.IsActive() || !base.vertical || base.content == null || base.viewport == null)
			{
				base.OnScroll(eventData);
				return;
			}
			float y = eventData.scrollDelta.y;
			if (Mathf.Approximately(y, 0f))
			{
				return;
			}
			Canvas.ForceUpdateCanvases();
			float num = Mathf.Max(1f, base.content.rect.height - base.viewport.rect.height);
			if (!this._isWheelScrolling)
			{
				this._targetNormalizedPosition = base.verticalNormalizedPosition;
				this._smoothVelocity = 0f;
				this.StopMovement();
			}
			float num2 = Mathf.Sign(y);
			this._targetNormalizedPosition = Mathf.Clamp01(this._targetNormalizedPosition + num2 * this.scrollStepPixels / num);
			this._isWheelScrolling = true;
			eventData.Use();
		}

		// Token: 0x060056ED RID: 22253 RVA: 0x0021D3E3 File Offset: 0x0021B5E3
		public override void OnBeginDrag(PointerEventData eventData)
		{
			this.CancelWheelScroll();
			base.OnBeginDrag(eventData);
		}

		// Token: 0x060056EE RID: 22254 RVA: 0x0021D3F4 File Offset: 0x0021B5F4
		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (!this._isWheelScrolling)
			{
				return;
			}
			float num = Mathf.SmoothDamp(base.verticalNormalizedPosition, this._targetNormalizedPosition, ref this._smoothVelocity, this.smoothingTime, float.PositiveInfinity, Time.unscaledDeltaTime);
			base.verticalNormalizedPosition = num;
			if (Mathf.Abs(num - this._targetNormalizedPosition) < 0.0001f && Mathf.Abs(this._smoothVelocity) < 0.0001f)
			{
				base.verticalNormalizedPosition = this._targetNormalizedPosition;
				this.CancelWheelScroll();
			}
		}

		// Token: 0x060056EF RID: 22255 RVA: 0x0021D477 File Offset: 0x0021B677
		private void CancelWheelScroll()
		{
			this._isWheelScrolling = false;
			this._smoothVelocity = 0f;
			this.StopMovement();
		}

		// Token: 0x04005743 RID: 22339
		[SerializeField]
		[Min(1f)]
		private float scrollStepPixels = 52f;

		// Token: 0x04005744 RID: 22340
		[SerializeField]
		[Min(0.01f)]
		private float smoothingTime = 0.16f;

		// Token: 0x04005745 RID: 22341
		private float _targetNormalizedPosition;

		// Token: 0x04005746 RID: 22342
		private float _smoothVelocity;

		// Token: 0x04005747 RID: 22343
		private bool _isWheelScrolling;
	}
}
