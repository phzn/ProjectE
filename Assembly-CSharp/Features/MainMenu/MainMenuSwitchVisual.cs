using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x0200109E RID: 4254
	[RequireComponent(typeof(Toggle))]
	public sealed class MainMenuSwitchVisual : MonoBehaviour
	{
		// Token: 0x060080D0 RID: 32976 RVA: 0x00348C44 File Offset: 0x00346E44
		public void Configure(RectTransform knobRect, Image trackImage)
		{
			this.knob = knobRect;
			this.track = trackImage;
			if (this._toggle == null)
			{
				this._toggle = base.GetComponent<Toggle>();
			}
			this._knobImage = ((this.knob != null) ? this.knob.GetComponent<Image>() : null);
			this.Snap(this._toggle != null && this._toggle.isOn);
		}

		// Token: 0x060080D1 RID: 32977 RVA: 0x00348CC0 File Offset: 0x00346EC0
		private void Awake()
		{
			this._toggle = base.GetComponent<Toggle>();
			this._knobImage = ((this.knob != null) ? this.knob.GetComponent<Image>() : null);
			this._toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleChanged));
		}

		// Token: 0x060080D2 RID: 32978 RVA: 0x00348D17 File Offset: 0x00346F17
		private void OnEnable()
		{
			this.Snap(this._toggle != null && this._toggle.isOn);
		}

		// Token: 0x060080D3 RID: 32979 RVA: 0x00348D3B File Offset: 0x00346F3B
		private void OnToggleChanged(bool isOn)
		{
			if (!base.isActiveAndEnabled)
			{
				this.Snap(isOn);
				return;
			}
			if (this._routine != null)
			{
				base.StopCoroutine(this._routine);
			}
			this._routine = base.StartCoroutine(this.SlideRoutine(isOn));
		}

		// Token: 0x060080D4 RID: 32980 RVA: 0x00348D74 File Offset: 0x00346F74
		public void Snap(bool isOn)
		{
			this.ApplyPose(isOn ? 1f : 0f, isOn);
		}

		// Token: 0x060080D5 RID: 32981 RVA: 0x00348D8C File Offset: 0x00346F8C
		private IEnumerator SlideRoutine(bool isOn)
		{
			float start = this.CurrentPose();
			float target = (isOn ? 1f : 0f);
			float elapsed = 0f;
			while (elapsed < this.slideDuration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / this.slideDuration));
				this.ApplyPose(Mathf.Lerp(start, target, num), isOn);
				yield return null;
			}
			this.ApplyPose(target, isOn);
			this._routine = null;
			yield break;
		}

		// Token: 0x060080D6 RID: 32982 RVA: 0x00348DA2 File Offset: 0x00346FA2
		private float CurrentPose()
		{
			if (this.knob == null)
			{
				return 0f;
			}
			return Mathf.InverseLerp(0f, 1f, this.knob.anchorMin.x);
		}

		// Token: 0x060080D7 RID: 32983 RVA: 0x00348DD8 File Offset: 0x00346FD8
		private void ApplyPose(float pose, bool isOn)
		{
			if (this.knob != null)
			{
				float num = Mathf.Lerp(0f, 1f, pose);
				this.knob.anchorMin = new Vector2(num, 0.5f);
				this.knob.anchorMax = new Vector2(num, 0.5f);
				this.knob.pivot = new Vector2(num, 0.5f);
				this.knob.anchoredPosition = new Vector2(Mathf.Lerp(this.knobInset, -this.knobInset, pose), 0f);
			}
			if (this.track != null)
			{
				this.track.color = Color.Lerp(this.trackOffColor, this.trackOnColor, pose);
			}
			if (this._knobImage != null)
			{
				this._knobImage.color = Color.Lerp(this.knobOffColor, this.knobOnColor, pose);
			}
		}

		// Token: 0x04008412 RID: 33810
		[SerializeField]
		private RectTransform knob;

		// Token: 0x04008413 RID: 33811
		[SerializeField]
		private Image track;

		// Token: 0x04008414 RID: 33812
		[SerializeField]
		private Color trackOffColor = new Color(0.035f, 0.015f, 0.035f, 1f);

		// Token: 0x04008415 RID: 33813
		[SerializeField]
		private Color trackOnColor = new Color(1f, 0.31f, 0.61f, 1f);

		// Token: 0x04008416 RID: 33814
		[SerializeField]
		private Color knobOffColor = new Color(0.78f, 0.67f, 0.76f, 1f);

		// Token: 0x04008417 RID: 33815
		[SerializeField]
		private Color knobOnColor = new Color(1f, 0.97f, 1f, 1f);

		// Token: 0x04008418 RID: 33816
		[SerializeField]
		[Min(0.01f)]
		private float slideDuration = 0.14f;

		// Token: 0x04008419 RID: 33817
		[SerializeField]
		[Min(0f)]
		private float knobInset = 4f;

		// Token: 0x0400841A RID: 33818
		private Toggle _toggle;

		// Token: 0x0400841B RID: 33819
		private Image _knobImage;

		// Token: 0x0400841C RID: 33820
		private Coroutine _routine;
	}
}
