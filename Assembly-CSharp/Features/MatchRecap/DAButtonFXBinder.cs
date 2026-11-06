using System;
using System.Collections;
using DA_Assets.DAB;
using UnityEngine;
using UnityEngine.Events;

namespace Features.MatchRecap
{
	// Token: 0x02000FC0 RID: 4032
	[RequireComponent(typeof(DAButton))]
	public class DAButtonFXBinder : MonoBehaviour
	{
		// Token: 0x060079B7 RID: 31159 RVA: 0x0031BD00 File Offset: 0x00319F00
		private void Awake()
		{
			this._btn = base.GetComponent<DAButton>();
			this._baseScale = base.transform.localScale;
			if (this._btn == null)
			{
				return;
			}
			this._btn.OnPointerEnterEvent.AddListener(new UnityAction(this.OnHoverIn));
			this._btn.OnPointerExitEvent.AddListener(new UnityAction(this.OnHoverOut));
		}

		// Token: 0x060079B8 RID: 31160 RVA: 0x0031BD74 File Offset: 0x00319F74
		private void OnDestroy()
		{
			if (this._btn == null)
			{
				return;
			}
			this._btn.OnPointerEnterEvent.RemoveListener(new UnityAction(this.OnHoverIn));
			this._btn.OnPointerExitEvent.RemoveListener(new UnityAction(this.OnHoverOut));
		}

		// Token: 0x060079B9 RID: 31161 RVA: 0x0031BDC8 File Offset: 0x00319FC8
		private void OnHoverIn()
		{
			this.TweenScaleTo(this._baseScale * this.hoverScale);
		}

		// Token: 0x060079BA RID: 31162 RVA: 0x0031BDE1 File Offset: 0x00319FE1
		private void OnHoverOut()
		{
			this.TweenScaleTo(this._baseScale);
		}

		// Token: 0x060079BB RID: 31163 RVA: 0x0031BDEF File Offset: 0x00319FEF
		private void TweenScaleTo(Vector3 target)
		{
			if (this._scaleRoutine != null)
			{
				base.StopCoroutine(this._scaleRoutine);
			}
			this._scaleRoutine = base.StartCoroutine(this.ScaleRoutine(target));
		}

		// Token: 0x060079BC RID: 31164 RVA: 0x0031BE18 File Offset: 0x0031A018
		private IEnumerator ScaleRoutine(Vector3 target)
		{
			Vector3 from = base.transform.localScale;
			float elapsed = 0f;
			while (elapsed < this.hoverDuration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / this.hoverDuration);
				float num2 = num * num * (3f - 2f * num);
				base.transform.localScale = Vector3.LerpUnclamped(from, target, num2);
				yield return null;
			}
			base.transform.localScale = target;
			this._scaleRoutine = null;
			yield break;
		}

		// Token: 0x04007CFE RID: 31998
		[Header("Minimal Hover")]
		[Tooltip("Target scale quando il puntatore e' sopra. 1 = niente ingrandimento; tipico 1.03–1.06.")]
		[Range(1f, 1.2f)]
		[SerializeField]
		private float hoverScale = 1.04f;

		// Token: 0x04007CFF RID: 31999
		[Tooltip("Durata del tween di entrata/uscita hover (s).")]
		[Min(0.01f)]
		[SerializeField]
		private float hoverDuration = 0.15f;

		// Token: 0x04007D00 RID: 32000
		private DAButton _btn;

		// Token: 0x04007D01 RID: 32001
		private Vector3 _baseScale;

		// Token: 0x04007D02 RID: 32002
		private Coroutine _scaleRoutine;
	}
}
