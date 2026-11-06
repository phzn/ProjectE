using System;
using System.Collections;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist;
using UnityEngine;

namespace Features.PoolDive
{
	// Token: 0x02000DD9 RID: 3545
	[DisallowMultipleComponent]
	public class PoolWetOnExit : MonoBehaviour
	{
		// Token: 0x060068F0 RID: 26864 RVA: 0x002A0044 File Offset: 0x0029E244
		private void Awake()
		{
			if (this.swimming == null)
			{
				this.swimming = base.GetComponentInParent<vSwimming>();
			}
			if (this.swimming == null)
			{
				this.swimming = base.GetComponentInChildren<vSwimming>();
			}
			if (this.controller == null)
			{
				this.controller = base.GetComponentInParent<vThirdPersonController>();
			}
			if (this.controller == null && this.swimming != null)
			{
				this.controller = this.swimming.GetComponentInParent<vThirdPersonController>();
			}
			Transform transform = ((this.swimming != null) ? this.swimming.transform.root : base.transform.root);
			if (this.wetController == null)
			{
				this.wetController = transform.GetComponentInChildren<CielWetController>(true);
			}
			if (this.skinDecalController == null)
			{
				this.skinDecalController = transform.GetComponentInChildren<SkinDecalController>(true);
			}
			if (this.swimming == null)
			{
				Debug.LogWarning("[PoolWetOnExit] vSwimming not found on " + base.name + " - disabling.");
				base.enabled = false;
				return;
			}
			if (this.wetController == null)
			{
				Debug.LogWarning("[PoolWetOnExit] CielWetController not found under the player - disabling.");
				base.enabled = false;
			}
		}

		// Token: 0x060068F1 RID: 26865 RVA: 0x002A0180 File Offset: 0x0029E380
		private void Update()
		{
			if (this.swimming == null)
			{
				return;
			}
			bool inTheWater = this.swimming.inTheWater;
			if (inTheWater && !this._wasInWater)
			{
				this._exitPending = false;
				this.OnEnterWater();
			}
			else if (!inTheWater && this._wasInWater)
			{
				if (this.controller != null && this.controller.customAction)
				{
					if (!this._exitPending)
					{
						this._exitPending = true;
						base.StartCoroutine(this.OnExitAfterCustomAction());
					}
				}
				else
				{
					this.OnExitWater("exit edge (no custom action)");
				}
			}
			this._wasInWater = inTheWater;
		}

		// Token: 0x060068F2 RID: 26866 RVA: 0x002A0218 File Offset: 0x0029E418
		private IEnumerator OnExitAfterCustomAction()
		{
			float timeout = 4f;
			while (this.controller != null && this.controller.customAction && timeout > 0f)
			{
				timeout -= Time.deltaTime;
				yield return null;
			}
			if (!this._exitPending)
			{
				yield break;
			}
			this._exitPending = false;
			this.OnExitWater("after climb custom action");
			yield break;
		}

		// Token: 0x060068F3 RID: 26867 RVA: 0x002A0227 File Offset: 0x0029E427
		private void OnEnterWater()
		{
			if (this.debugLogs)
			{
				Debug.Log("[PoolWetOnExit] Enter water → reflection wet ON, decal off.", this);
			}
			if (this._decayRoutine != null)
			{
				base.StopCoroutine(this._decayRoutine);
				this._decayRoutine = null;
			}
			this.SetReflectionWet(true);
			this.SetDecal(false);
		}

		// Token: 0x060068F4 RID: 26868 RVA: 0x002A0268 File Offset: 0x0029E468
		private void OnExitWater(string reason)
		{
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PoolWetOnExit] Exit water ({0}) → decal ON, decay starts (hold {1:0.#}s).", reason, this.wetDuration), this);
			}
			this.SetDecal(true);
			if (this._decayRoutine != null)
			{
				base.StopCoroutine(this._decayRoutine);
			}
			this._decayRoutine = base.StartCoroutine(this.HoldThenDry());
		}

		// Token: 0x060068F5 RID: 26869 RVA: 0x002A02C6 File Offset: 0x0029E4C6
		private IEnumerator HoldThenDry()
		{
			float num = Mathf.Max(0f, this.wetDuration);
			if (num > 0f)
			{
				yield return new WaitForSeconds(num);
			}
			this.SetReflectionWet(false);
			this.SetDecal(false);
			this._decayRoutine = null;
			if (this.debugLogs)
			{
				Debug.Log("[PoolWetOnExit] Decay done → dry.", this);
			}
			yield break;
		}

		// Token: 0x060068F6 RID: 26870 RVA: 0x002A02D5 File Offset: 0x0029E4D5
		private void SetReflectionWet(bool on)
		{
			if (this.wetController == null || this._reflectionWet == on)
			{
				return;
			}
			this._reflectionWet = on;
			this.wetController.SetWet(on);
		}

		// Token: 0x060068F7 RID: 26871 RVA: 0x002A0304 File Offset: 0x0029E504
		private void SetDecal(bool on)
		{
			if (this.skinDecalController == null || this.sweatDecal == null)
			{
				return;
			}
			if (this._decalActive == on)
			{
				return;
			}
			this._decalActive = on;
			if (this._decalFadeRoutine != null)
			{
				base.StopCoroutine(this._decalFadeRoutine);
			}
			if (on)
			{
				this.skinDecalController.AddDecal(this.sweatDecal, null, new float?(0f));
				this.skinDecalController.SetDecalActive(this.sweatDecal, true);
				this._decalFadeRoutine = base.StartCoroutine(this.FadeDecal(0f, this.sweatDecalPeakOpacity, false));
				return;
			}
			this._decalFadeRoutine = base.StartCoroutine(this.FadeDecal(this.sweatDecalPeakOpacity, 0f, true));
		}

		// Token: 0x060068F8 RID: 26872 RVA: 0x002A03C9 File Offset: 0x0029E5C9
		private IEnumerator FadeDecal(float from, float to, bool removeAtEnd)
		{
			float fadeTime = Mathf.Max(0f, this.sweatDecalFadeDuration);
			if (fadeTime > 0f && this.skinDecalController != null && this.sweatDecal != null)
			{
				float elapsed = 0f;
				while (elapsed < fadeTime)
				{
					elapsed += Time.deltaTime;
					float num = Mathf.Clamp01(elapsed / fadeTime);
					this.skinDecalController.SetDecalOpacity(this.sweatDecal, Mathf.Lerp(from, to, num));
					yield return null;
				}
			}
			if (this.skinDecalController != null && this.sweatDecal != null)
			{
				this.skinDecalController.SetDecalOpacity(this.sweatDecal, to);
			}
			if (removeAtEnd && this.skinDecalController != null && this.sweatDecal != null)
			{
				this.skinDecalController.RemoveDecal(this.sweatDecal);
			}
			this._decalFadeRoutine = null;
			yield break;
		}

		// Token: 0x060068F9 RID: 26873 RVA: 0x002A03ED File Offset: 0x0029E5ED
		public void ResetForHomeReturn()
		{
			this.OnDisable();
			this._wasInWater = false;
		}

		// Token: 0x060068FA RID: 26874 RVA: 0x002A03FC File Offset: 0x0029E5FC
		private void OnDisable()
		{
			if (this._decayRoutine != null)
			{
				base.StopCoroutine(this._decayRoutine);
				this._decayRoutine = null;
			}
			if (this._decalFadeRoutine != null)
			{
				base.StopCoroutine(this._decalFadeRoutine);
				this._decalFadeRoutine = null;
			}
			this._exitPending = false;
			if (this.wetController != null)
			{
				this.wetController.SetWet(false);
			}
			this._reflectionWet = false;
			if (this.skinDecalController != null && this.sweatDecal != null)
			{
				this.skinDecalController.RemoveDecal(this.sweatDecal);
			}
			this._decalActive = false;
		}

		// Token: 0x04006B90 RID: 27536
		[Header("Refs (auto-resolved)")]
		[SerializeField]
		private vSwimming swimming;

		// Token: 0x04006B91 RID: 27537
		[SerializeField]
		private vThirdPersonController controller;

		// Token: 0x04006B92 RID: 27538
		[SerializeField]
		private CielWetController wetController;

		// Token: 0x04006B93 RID: 27539
		[SerializeField]
		private SkinDecalController skinDecalController;

		// Token: 0x04006B94 RID: 27540
		[Header("Wet effect")]
		[Tooltip("Seconds the wet look stays at full AFTER she leaves the water, before it fades back to dry. The decay only ever starts on exit; while she is in the water the reflection look stays on indefinitely. The reflection fade-in/out durations themselves live on CielWetController (default 5s each).")]
		[SerializeField]
		private float wetDuration = 60f;

		// Token: 0x04006B95 RID: 27541
		[Header("Sweat decal (shown only OUT of the water)")]
		[Tooltip("Sweat skin decal activated when Ciel climbs OUT of the pool (assign the 'Sweat Light' SkinDecal asset). It is NOT shown while she is in the water; it fades in on exit and fades back out alongside the reflection look.")]
		[SerializeField]
		private SkinDecal sweatDecal;

		// Token: 0x04006B96 RID: 27542
		[Tooltip("Opacity the sweat decal fades up to when wet (its peak).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float sweatDecalPeakOpacity = 1f;

		// Token: 0x04006B97 RID: 27543
		[Tooltip("Seconds for the sweat decal to fade IN (0 → peak) and OUT (peak → 0), same gradual approach as the shower.")]
		[SerializeField]
		private float sweatDecalFadeDuration = 5f;

		// Token: 0x04006B98 RID: 27544
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04006B99 RID: 27545
		private bool _wasInWater;

		// Token: 0x04006B9A RID: 27546
		private bool _exitPending;

		// Token: 0x04006B9B RID: 27547
		private bool _reflectionWet;

		// Token: 0x04006B9C RID: 27548
		private bool _decalActive;

		// Token: 0x04006B9D RID: 27549
		private Coroutine _decayRoutine;

		// Token: 0x04006B9E RID: 27550
		private Coroutine _decalFadeRoutine;
	}
}
