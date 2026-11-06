using System;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Social
{
	// Token: 0x0200145E RID: 5214
	public class CielXWheelScroll : ScrollRect, IControllerScrollSync
	{
		// Token: 0x0600A0E7 RID: 41191 RVA: 0x00004F31 File Offset: 0x00003131
		public override void OnBeginDrag(PointerEventData eventData)
		{
		}

		// Token: 0x0600A0E8 RID: 41192 RVA: 0x00004F31 File Offset: 0x00003131
		public override void OnDrag(PointerEventData eventData)
		{
		}

		// Token: 0x0600A0E9 RID: 41193 RVA: 0x00004F31 File Offset: 0x00003131
		public override void OnEndDrag(PointerEventData eventData)
		{
		}

		// Token: 0x0600A0EA RID: 41194 RVA: 0x00429EE8 File Offset: 0x004280E8
		public override void OnScroll(PointerEventData data)
		{
			if (base.content == null || base.viewport == null)
			{
				return;
			}
			this.EnsureTarget();
			float num = Mathf.Max(0f, base.content.rect.height - base.viewport.rect.height);
			float num2 = base.viewport.rect.height * this.notchFraction;
			float num3 = Mathf.Clamp(data.scrollDelta.y, -1f, 1f);
			this._targetY = Mathf.Clamp(this._targetY - num3 * num2, 0f, num);
		}

		// Token: 0x0600A0EB RID: 41195 RVA: 0x00429F9C File Offset: 0x0042819C
		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (base.content == null || base.viewport == null)
			{
				return;
			}
			float num = Mathf.Max(0f, base.content.rect.height - base.viewport.rect.height);
			if (this._targetY < 0f)
			{
				this._targetY = Mathf.Clamp(base.content.anchoredPosition.y, 0f, num);
				return;
			}
			this._targetY = Mathf.Clamp(this._targetY, 0f, num);
			Vector2 anchoredPosition = base.content.anchoredPosition;
			float num2 = Mathf.Lerp(anchoredPosition.y, this._targetY, 1f - Mathf.Exp(-this.smoothing * Time.unscaledDeltaTime));
			if (Mathf.Abs(num2 - anchoredPosition.y) < 0.05f)
			{
				num2 = this._targetY;
			}
			if (!Mathf.Approximately(num2, anchoredPosition.y))
			{
				anchoredPosition.y = num2;
				base.content.anchoredPosition = anchoredPosition;
			}
		}

		// Token: 0x0600A0EC RID: 41196 RVA: 0x0042A0B7 File Offset: 0x004282B7
		private void EnsureTarget()
		{
			if (this._targetY < 0f && base.content != null)
			{
				this._targetY = Mathf.Max(0f, base.content.anchoredPosition.y);
			}
		}

		// Token: 0x0600A0ED RID: 41197 RVA: 0x0042A0F4 File Offset: 0x004282F4
		public void SyncControllerScroll()
		{
			this._targetY = ((base.content != null) ? Mathf.Max(0f, base.content.anchoredPosition.y) : (-1f));
		}

		// Token: 0x0600A0EE RID: 41198 RVA: 0x0042A12C File Offset: 0x0042832C
		public void SnapTop()
		{
			this._targetY = 0f;
			if (base.content != null)
			{
				Vector2 anchoredPosition = base.content.anchoredPosition;
				anchoredPosition.y = 0f;
				base.content.anchoredPosition = anchoredPosition;
			}
		}

		// Token: 0x0400A49A RID: 42138
		[Tooltip("Pixels scrolled per wheel notch. Kept for API compat but NOT used for the step size (see notchFraction) — absolute pixel steps don't translate across nested-canvas scales.")]
		public float pixelsPerNotch = 90f;

		// Token: 0x0400A49B RID: 42139
		[Tooltip("Fraction of the VIEWPORT height scrolled per wheel notch. Scale-independent: 0.08 = 8% of a screenful per notch. Keep small.")]
		public float notchFraction = 0.05f;

		// Token: 0x0400A49C RID: 42140
		[Tooltip("Higher = snappier easing; lower = smoother/slower catch-up.")]
		public float smoothing = 12f;

		// Token: 0x0400A49D RID: 42141
		private float _targetY = -1f;
	}
}
