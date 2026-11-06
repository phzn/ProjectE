using System;
using System.Collections;
using ProjectExhibitionist;
using UnityEngine;

namespace Features.Gym
{
	// Token: 0x020011B1 RID: 4529
	[DisallowMultipleComponent]
	public sealed class GymExerciseSweatController : MonoBehaviour
	{
		// Token: 0x1700162C RID: 5676
		// (get) Token: 0x06008AEE RID: 35566 RVA: 0x0038CAF9 File Offset: 0x0038ACF9
		public bool IsExerciseActive
		{
			get
			{
				return this._exerciseActive;
			}
		}

		// Token: 0x1700162D RID: 5677
		// (get) Token: 0x06008AEF RID: 35567 RVA: 0x0038CB01 File Offset: 0x0038AD01
		public float CurrentOpacity
		{
			get
			{
				return this._currentOpacity;
			}
		}

		// Token: 0x06008AF0 RID: 35568 RVA: 0x0038CB0C File Offset: 0x0038AD0C
		public void Configure(SkinDecalController skinDecalController, SkinDecal sweatDecal, float minimumDelay, float maximumDelay, float fadeInDuration, float peakOpacity, float fadeOutDuration)
		{
			if (this._ownsDecal && this._sweatDecal != null && this._sweatDecal != sweatDecal)
			{
				this.RemoveOwnedDecal();
			}
			this._skinDecalController = skinDecalController;
			this._sweatDecal = sweatDecal;
			this._minimumDelay = Mathf.Max(0f, minimumDelay);
			this._maximumDelay = Mathf.Max(this._minimumDelay, maximumDelay);
			this._fadeInDuration = Mathf.Max(0f, fadeInDuration);
			this._peakOpacity = Mathf.Clamp01(peakOpacity);
			this._fadeOutDuration = Mathf.Max(0f, fadeOutDuration);
		}

		// Token: 0x06008AF1 RID: 35569 RVA: 0x0038CBA6 File Offset: 0x0038ADA6
		public void BeginExercise()
		{
			if (this._skinDecalController == null || this._sweatDecal == null)
			{
				return;
			}
			this.StopRunningRoutine();
			this._exerciseActive = true;
			this._routine = base.StartCoroutine(this.WaitThenFadeIn());
		}

		// Token: 0x06008AF2 RID: 35570 RVA: 0x0038CBE4 File Offset: 0x0038ADE4
		public void EndExercise()
		{
			if (!this._exerciseActive)
			{
				return;
			}
			this._exerciseActive = false;
			this.StopRunningRoutine();
			if (this._ownsDecal && this._skinDecalController != null && this._sweatDecal != null && this._skinDecalController.HasDecal(this._sweatDecal))
			{
				this._routine = base.StartCoroutine(this.FadeOutAndRemove());
				return;
			}
			this._ownsDecal = false;
			this._currentOpacity = 0f;
		}

		// Token: 0x06008AF3 RID: 35571 RVA: 0x0038CC63 File Offset: 0x0038AE63
		private IEnumerator WaitThenFadeIn()
		{
			float delay = Random.Range(this._minimumDelay, this._maximumDelay);
			float elapsed = 0f;
			while (this._exerciseActive && elapsed < delay)
			{
				elapsed += Time.deltaTime;
				yield return null;
			}
			if (!this._exerciseActive || this._skinDecalController == null || this._sweatDecal == null)
			{
				this._routine = null;
				yield break;
			}
			if (!this._ownsDecal)
			{
				this._decalWasPresentBeforeGym = this._skinDecalController.TryGetDecalState(this._sweatDecal, out this._previousDecalActive, out this._previousDecalOpacity, out this._previousDecalTint);
				if (this._decalWasPresentBeforeGym && this._previousDecalActive)
				{
					this._routine = null;
					yield break;
				}
				if (this._decalWasPresentBeforeGym)
				{
					this._skinDecalController.SetDecalOpacity(this._sweatDecal, 0f);
					this._skinDecalController.SetDecalActive(this._sweatDecal, true);
				}
				else
				{
					this._skinDecalController.AddDecal(this._sweatDecal, null, new float?(0f));
					this._skinDecalController.SetDecalActive(this._sweatDecal, true);
				}
				this._ownsDecal = true;
				this._currentOpacity = 0f;
			}
			float startOpacity = this._currentOpacity;
			float elapsedFade = 0f;
			while (this._exerciseActive && elapsedFade < this._fadeInDuration)
			{
				elapsedFade += Time.deltaTime;
				float num = ((this._fadeInDuration <= 0f) ? 1f : Mathf.Clamp01(elapsedFade / this._fadeInDuration));
				this.SetOwnedOpacity(Mathf.Lerp(startOpacity, this._peakOpacity, GymExerciseSweatController.SmoothFade(num)));
				yield return null;
			}
			if (this._exerciseActive)
			{
				this.SetOwnedOpacity(this._peakOpacity);
			}
			this._routine = null;
			yield break;
		}

		// Token: 0x06008AF4 RID: 35572 RVA: 0x0038CC72 File Offset: 0x0038AE72
		private IEnumerator FadeOutAndRemove()
		{
			float startOpacity = this._currentOpacity;
			float elapsed = 0f;
			while (!this._exerciseActive && elapsed < this._fadeOutDuration)
			{
				elapsed += Time.deltaTime;
				float num = ((this._fadeOutDuration <= 0f) ? 1f : Mathf.Clamp01(elapsed / this._fadeOutDuration));
				this.SetOwnedOpacity(Mathf.Lerp(startOpacity, 0f, GymExerciseSweatController.SmoothFade(num)));
				yield return null;
			}
			if (!this._exerciseActive)
			{
				this.RemoveOwnedDecal();
			}
			this._routine = null;
			yield break;
		}

		// Token: 0x06008AF5 RID: 35573 RVA: 0x0038CC84 File Offset: 0x0038AE84
		private void SetOwnedOpacity(float opacity)
		{
			if (!this._ownsDecal || this._skinDecalController == null || this._sweatDecal == null)
			{
				return;
			}
			this._currentOpacity = Mathf.Clamp01(opacity);
			this._skinDecalController.SetDecalOpacity(this._sweatDecal, this._currentOpacity);
		}

		// Token: 0x06008AF6 RID: 35574 RVA: 0x001811C8 File Offset: 0x0017F3C8
		private static float SmoothFade(float value)
		{
			value = Mathf.Clamp01(value);
			return value * value * (3f - 2f * value);
		}

		// Token: 0x06008AF7 RID: 35575 RVA: 0x0038CCD9 File Offset: 0x0038AED9
		private void StopRunningRoutine()
		{
			if (this._routine == null)
			{
				return;
			}
			base.StopCoroutine(this._routine);
			this._routine = null;
		}

		// Token: 0x06008AF8 RID: 35576 RVA: 0x0038CCF8 File Offset: 0x0038AEF8
		private void RemoveOwnedDecal()
		{
			if (this._ownsDecal && this._skinDecalController != null && this._sweatDecal != null && this._skinDecalController.HasDecal(this._sweatDecal))
			{
				if (this._decalWasPresentBeforeGym)
				{
					this._skinDecalController.SetDecalTint(this._sweatDecal, this._previousDecalTint);
					this._skinDecalController.SetDecalOpacity(this._sweatDecal, this._previousDecalOpacity);
					this._skinDecalController.SetDecalActive(this._sweatDecal, this._previousDecalActive);
				}
				else
				{
					this._skinDecalController.RemoveDecal(this._sweatDecal);
				}
			}
			this._ownsDecal = false;
			this._currentOpacity = 0f;
			this._decalWasPresentBeforeGym = false;
			this._previousDecalActive = false;
			this._previousDecalOpacity = 0f;
			this._previousDecalTint = Color.white;
		}

		// Token: 0x06008AF9 RID: 35577 RVA: 0x0038CDD8 File Offset: 0x0038AFD8
		private void OnDisable()
		{
			this.StopRunningRoutine();
			this._exerciseActive = false;
			this.RemoveOwnedDecal();
		}

		// Token: 0x04008DC8 RID: 36296
		private SkinDecalController _skinDecalController;

		// Token: 0x04008DC9 RID: 36297
		private SkinDecal _sweatDecal;

		// Token: 0x04008DCA RID: 36298
		private float _minimumDelay = 10f;

		// Token: 0x04008DCB RID: 36299
		private float _maximumDelay = 15f;

		// Token: 0x04008DCC RID: 36300
		private float _fadeInDuration = 12f;

		// Token: 0x04008DCD RID: 36301
		private float _peakOpacity = 1f;

		// Token: 0x04008DCE RID: 36302
		private float _fadeOutDuration = 6f;

		// Token: 0x04008DCF RID: 36303
		private Coroutine _routine;

		// Token: 0x04008DD0 RID: 36304
		private bool _exerciseActive;

		// Token: 0x04008DD1 RID: 36305
		private bool _ownsDecal;

		// Token: 0x04008DD2 RID: 36306
		private float _currentOpacity;

		// Token: 0x04008DD3 RID: 36307
		private bool _decalWasPresentBeforeGym;

		// Token: 0x04008DD4 RID: 36308
		private bool _previousDecalActive;

		// Token: 0x04008DD5 RID: 36309
		private float _previousDecalOpacity;

		// Token: 0x04008DD6 RID: 36310
		private Color _previousDecalTint = Color.white;
	}
}
