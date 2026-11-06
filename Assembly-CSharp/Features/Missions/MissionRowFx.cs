using System;
using System.Collections;
using Features.MainMenu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Missions
{
	// Token: 0x02000FA2 RID: 4002
	[DisallowMultipleComponent]
	public sealed class MissionRowFx : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x0600787C RID: 30844 RVA: 0x00312DE4 File Offset: 0x00310FE4
		public void Configure(Image background, Image accent, Color baseColor, Color hoverColor)
		{
			this._background = background;
			this._accent = accent;
			this._baseColor = baseColor;
			this._hoverColor = hoverColor;
			this._baseScale = base.transform.localScale;
			this._group = base.GetComponent<CanvasGroup>();
			if (this._group == null)
			{
				this._group = base.gameObject.AddComponent<CanvasGroup>();
			}
			this.ApplyImmediate(false);
		}

		// Token: 0x0600787D RID: 30845 RVA: 0x00312E54 File Offset: 0x00311054
		public void PlayEntrance(float delay)
		{
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this._entrance != null)
			{
				base.StopCoroutine(this._entrance);
			}
			this._entrance = base.StartCoroutine(this.EntranceRoutine(Mathf.Max(0f, delay)));
		}

		// Token: 0x0600787E RID: 30846 RVA: 0x00312EA8 File Offset: 0x003110A8
		public void OnPointerEnter(PointerEventData eventData)
		{
			this._hovered = true;
			MainMenuUiFeedbackAudio.PlayHover();
			this.TransitionTo(true);
		}

		// Token: 0x0600787F RID: 30847 RVA: 0x00312EBD File Offset: 0x003110BD
		public void OnPointerExit(PointerEventData eventData)
		{
			this._hovered = false;
			this.TransitionTo(false);
		}

		// Token: 0x06007880 RID: 30848 RVA: 0x00312ECD File Offset: 0x003110CD
		public void PulseProgress()
		{
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this._pulse != null)
			{
				base.StopCoroutine(this._pulse);
			}
			this._pulse = base.StartCoroutine(this.PulseRoutine());
		}

		// Token: 0x06007881 RID: 30849 RVA: 0x00312F0B File Offset: 0x0031110B
		private void TransitionTo(bool hovered)
		{
			if (this._transition != null)
			{
				base.StopCoroutine(this._transition);
			}
			this._transition = base.StartCoroutine(this.TransitionRoutine(hovered));
		}

		// Token: 0x06007882 RID: 30850 RVA: 0x00312F34 File Offset: 0x00311134
		private IEnumerator TransitionRoutine(bool hovered)
		{
			Vector3 startScale = base.transform.localScale;
			Color startBackground = ((this._background != null) ? this._background.color : this._baseColor);
			Color startAccent = ((this._accent != null) ? this._accent.color : Color.clear);
			Vector3 targetScale = this._baseScale * (hovered ? 1.018f : 1f);
			Color targetBackground = (hovered ? this._hoverColor : this._baseColor);
			Color targetAccent = new Color(1f, 0.31f, 0.61f, hovered ? 1f : 0f);
			float elapsed = 0f;
			while (elapsed < 0.16f)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = 1f - Mathf.Pow(1f - Mathf.Clamp01(elapsed / 0.16f), 3f);
				base.transform.localScale = Vector3.LerpUnclamped(startScale, targetScale, num);
				if (this._background != null)
				{
					this._background.color = Color.Lerp(startBackground, targetBackground, num);
				}
				if (this._accent != null)
				{
					this._accent.color = Color.Lerp(startAccent, targetAccent, num);
				}
				yield return null;
			}
			base.transform.localScale = targetScale;
			if (this._background != null)
			{
				this._background.color = targetBackground;
			}
			if (this._accent != null)
			{
				this._accent.color = targetAccent;
			}
			this._transition = null;
			yield break;
		}

		// Token: 0x06007883 RID: 30851 RVA: 0x00312F4A File Offset: 0x0031114A
		private IEnumerator PulseRoutine()
		{
			Vector3 restingScale = this._baseScale * (this._hovered ? 1.018f : 1f);
			Color restingAccent = new Color(1f, 0.31f, 0.61f, this._hovered ? 1f : 0f);
			float elapsed = 0f;
			while (elapsed < 0.34f)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Sin(Mathf.Clamp01(elapsed / 0.34f) * 3.1415927f);
				base.transform.localScale = restingScale * (1f + num * 0.016f);
				if (this._accent != null)
				{
					this._accent.color = new Color(1f, 0.31f, 0.61f, Mathf.Lerp(restingAccent.a, 1f, num));
				}
				yield return null;
			}
			base.transform.localScale = restingScale;
			if (this._accent != null)
			{
				this._accent.color = restingAccent;
			}
			this._pulse = null;
			yield break;
		}

		// Token: 0x06007884 RID: 30852 RVA: 0x00312F59 File Offset: 0x00311159
		private IEnumerator EntranceRoutine(float delay)
		{
			this._group.alpha = 0f;
			base.transform.localScale = this._baseScale * 0.965f;
			if (delay > 0f)
			{
				yield return new WaitForSecondsRealtime(delay);
			}
			float elapsed = 0f;
			while (elapsed < 0.22f)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / 0.22f);
				float num2 = 1f - Mathf.Pow(1f - num, 3f) + Mathf.Sin(num * 3.1415927f) * 0.035f;
				this._group.alpha = num;
				base.transform.localScale = Vector3.LerpUnclamped(this._baseScale * 0.965f, this._baseScale, num2);
				yield return null;
			}
			this._group.alpha = 1f;
			base.transform.localScale = this._baseScale;
			this._entrance = null;
			yield break;
		}

		// Token: 0x06007885 RID: 30853 RVA: 0x00312F70 File Offset: 0x00311170
		private void OnDisable()
		{
			this._hovered = false;
			this._transition = null;
			this._pulse = null;
			this._entrance = null;
			if (this._group != null)
			{
				this._group.alpha = 1f;
			}
			this.ApplyImmediate(false);
		}

		// Token: 0x06007886 RID: 30854 RVA: 0x00312FC0 File Offset: 0x003111C0
		private void ApplyImmediate(bool hovered)
		{
			base.transform.localScale = ((this._baseScale == Vector3.zero) ? Vector3.one : this._baseScale);
			if (this._background != null)
			{
				this._background.color = (hovered ? this._hoverColor : this._baseColor);
			}
			if (this._accent != null)
			{
				this._accent.color = new Color(1f, 0.31f, 0.61f, hovered ? 1f : 0f);
			}
		}

		// Token: 0x04007BD9 RID: 31705
		private Image _background;

		// Token: 0x04007BDA RID: 31706
		private Image _accent;

		// Token: 0x04007BDB RID: 31707
		private Color _baseColor;

		// Token: 0x04007BDC RID: 31708
		private Color _hoverColor;

		// Token: 0x04007BDD RID: 31709
		private Vector3 _baseScale;

		// Token: 0x04007BDE RID: 31710
		private CanvasGroup _group;

		// Token: 0x04007BDF RID: 31711
		private Coroutine _transition;

		// Token: 0x04007BE0 RID: 31712
		private Coroutine _pulse;

		// Token: 0x04007BE1 RID: 31713
		private Coroutine _entrance;

		// Token: 0x04007BE2 RID: 31714
		private bool _hovered;
	}
}
