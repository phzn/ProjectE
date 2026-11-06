using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MasturbationSystem
{
	// Token: 0x02000FFF RID: 4095
	[DisallowMultipleComponent]
	public class OrgasmMeterHUD : MonoBehaviour
	{
		// Token: 0x06007C39 RID: 31801 RVA: 0x0032BCE0 File Offset: 0x00329EE0
		private void Awake()
		{
			if (this.forceRestingBaselineOnAwake)
			{
				this.ApplyRestingBaseline();
			}
			if (this.rootCanvasGroup != null)
			{
				this.rootCanvasGroup.alpha = 0f;
			}
			this.GetFillMaterial(this.fillImage);
			this.ApplyFillAmountToMaterial(0f);
		}

		// Token: 0x06007C3A RID: 31802 RVA: 0x0032BD34 File Offset: 0x00329F34
		internal Material GetFillMaterial(Image image)
		{
			if (image == null || image != this.fillImage)
			{
				return null;
			}
			if (this._fillMaterialInstance == null && image.material != null)
			{
				this._fillMaterialInstance = new Material(image.material)
				{
					name = image.material.name + " (Instance)"
				};
			}
			if (this._fillMaterialInstance != null && image.material != this._fillMaterialInstance)
			{
				image.material = this._fillMaterialInstance;
			}
			return this._fillMaterialInstance;
		}

		// Token: 0x06007C3B RID: 31803 RVA: 0x0032BDD8 File Offset: 0x00329FD8
		private void ApplyFillAmountToMaterial(float value)
		{
			if (this._fillMaterialInstance == null || !this._fillMaterialInstance.HasProperty(OrgasmMeterHUD.FillAmountId))
			{
				return;
			}
			if (this._fillMaterialInstance.GetFloat(OrgasmMeterHUD.FillAmountId) != value)
			{
				this._fillMaterialInstance.SetFloat(OrgasmMeterHUD.FillAmountId, value);
			}
			this.SynchronizeRenderedMaterial();
		}

		// Token: 0x06007C3C RID: 31804 RVA: 0x0032BE30 File Offset: 0x0032A030
		private void SynchronizeRenderedMaterial()
		{
			if (this.fillImage == null || this._fillMaterialInstance == null)
			{
				return;
			}
			Material materialForRendering = this.fillImage.materialForRendering;
			if (materialForRendering == null || materialForRendering == this._fillMaterialInstance || !materialForRendering.HasProperty(OrgasmMeterHUD.FillAmountId))
			{
				return;
			}
			float @float = this._fillMaterialInstance.GetFloat(OrgasmMeterHUD.FillAmountId);
			if (materialForRendering.GetFloat(OrgasmMeterHUD.FillAmountId) != @float)
			{
				materialForRendering.SetFloat(OrgasmMeterHUD.FillAmountId, @float);
			}
			this.CopyRenderedVector(materialForRendering, OrgasmMeterHUD.HalfSizeId);
			this.CopyRenderedVector(materialForRendering, OrgasmMeterHUD.RadiiId);
			this.CopyRenderedVector(materialForRendering, OrgasmMeterHUD.RectPropsId);
		}

		// Token: 0x06007C3D RID: 31805 RVA: 0x0032BEDC File Offset: 0x0032A0DC
		private void CopyRenderedVector(Material rendered, int property)
		{
			if (!rendered.HasProperty(property) || !this._fillMaterialInstance.HasProperty(property))
			{
				return;
			}
			Vector4 vector = this._fillMaterialInstance.GetVector(property);
			if (rendered.GetVector(property) != vector)
			{
				rendered.SetVector(property, vector);
			}
		}

		// Token: 0x06007C3E RID: 31806 RVA: 0x0032BF28 File Offset: 0x0032A128
		private void Start()
		{
			if (this.meter == null)
			{
				this.meter = OrgasmMeterController.Instance;
			}
			this.HookMeter();
			if (this.playHideOnStart && !this.ShouldShowBar)
			{
				this.HandleEmptied();
			}
			this._startCompleted = true;
			this.StartBootReveal(!this._disabledBeforeStart);
		}

		// Token: 0x06007C3F RID: 31807 RVA: 0x0032BF80 File Offset: 0x0032A180
		private void OnEnable()
		{
			if (!this._startCompleted)
			{
				return;
			}
			if (this.meter == null)
			{
				this.meter = OrgasmMeterController.Instance;
			}
			this.HookMeter();
			if (this.rootCanvasGroup == null)
			{
				return;
			}
			if (this._bootRevealCompleted)
			{
				this.rootCanvasGroup.alpha = 1f;
				return;
			}
			this.StartBootReveal(false);
		}

		// Token: 0x06007C40 RID: 31808 RVA: 0x0032BFE4 File Offset: 0x0032A1E4
		private void OnDisable()
		{
			if (!this._startCompleted)
			{
				this._disabledBeforeStart = true;
			}
			if (this._bootRevealCoroutine != null)
			{
				base.StopCoroutine(this._bootRevealCoroutine);
			}
			this._bootRevealCoroutine = null;
			this.UnhookMeter();
			this.StopBarFeedbacks();
			this._hideTransitionPending = false;
		}

		// Token: 0x06007C41 RID: 31809 RVA: 0x0032C023 File Offset: 0x0032A223
		private void StartBootReveal(bool includeDelay)
		{
			if (this.rootCanvasGroup == null)
			{
				return;
			}
			if (this._bootRevealCoroutine != null)
			{
				base.StopCoroutine(this._bootRevealCoroutine);
			}
			this._bootRevealCoroutine = base.StartCoroutine(this.BootRevealRoutine(includeDelay));
		}

		// Token: 0x06007C42 RID: 31810 RVA: 0x0032C05B File Offset: 0x0032A25B
		private IEnumerator BootRevealRoutine(bool includeDelay)
		{
			if (includeDelay && this.bootDelay > 0f)
			{
				yield return new WaitForSecondsRealtime(this.bootDelay);
			}
			if (this.bootFadeInDuration <= 0f)
			{
				this.rootCanvasGroup.alpha = 1f;
				this._bootRevealCompleted = true;
				this._bootRevealCoroutine = null;
				yield break;
			}
			float initialAlpha = this.rootCanvasGroup.alpha;
			float t = 0f;
			while (t < this.bootFadeInDuration)
			{
				t += Time.unscaledDeltaTime;
				this.rootCanvasGroup.alpha = Mathf.Lerp(initialAlpha, 1f, Mathf.Clamp01(t / this.bootFadeInDuration));
				yield return null;
			}
			this.rootCanvasGroup.alpha = 1f;
			this._bootRevealCompleted = true;
			this._bootRevealCoroutine = null;
			yield break;
		}

		// Token: 0x06007C43 RID: 31811 RVA: 0x0032C071 File Offset: 0x0032A271
		private void OnDestroy()
		{
			this.UnhookMeter();
			if (this._fillMaterialInstance != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._fillMaterialInstance);
				}
				else
				{
					Object.DestroyImmediate(this._fillMaterialInstance);
				}
				this._fillMaterialInstance = null;
			}
		}

		// Token: 0x06007C44 RID: 31812 RVA: 0x0032C0B0 File Offset: 0x0032A2B0
		private void HookMeter()
		{
			if (this.meter == null || this._hookedToMeter)
			{
				return;
			}
			this.meter.OnFillingStart += this.HandleFillingStart;
			this.meter.OnEmptied += this.HandleEmptied;
			this.meter.OnValueChanged += this.HandleValueChanged;
			this.meter.OnMaxReached += this.HandleMaxReached;
			this._displayedValue = this.meter.Value;
			this._targetValue = this.meter.Value;
			this.ApplyFillAmountToMaterial(this.meter.Value);
			this._hookedToMeter = true;
			if (this.ShouldShowBar)
			{
				this.HandleFillingStart();
				return;
			}
			this.ApplyRestingBaseline();
		}

		// Token: 0x06007C45 RID: 31813 RVA: 0x0032C180 File Offset: 0x0032A380
		private void UnhookMeter()
		{
			if (this.meter == null || !this._hookedToMeter)
			{
				return;
			}
			this.meter.OnFillingStart -= this.HandleFillingStart;
			this.meter.OnEmptied -= this.HandleEmptied;
			this.meter.OnValueChanged -= this.HandleValueChanged;
			this.meter.OnMaxReached -= this.HandleMaxReached;
			this._hookedToMeter = false;
		}

		// Token: 0x06007C46 RID: 31814 RVA: 0x0032C207 File Offset: 0x0032A407
		private void HandleMaxReached()
		{
			if (this.debugMode)
			{
				Debug.Log("[OrgasmMeterHUD] Climax feedback");
			}
			if (this.climaxFeedback != null)
			{
				this.climaxFeedback.PlayFeedbacks();
			}
		}

		// Token: 0x06007C47 RID: 31815 RVA: 0x0032C234 File Offset: 0x0032A434
		private void Update()
		{
			if (this.meter == null)
			{
				this._hookedToMeter = false;
			}
			if (!this._hookedToMeter)
			{
				if (this.meter == null)
				{
					this.meter = OrgasmMeterController.Instance;
				}
				if (this.meter != null)
				{
					this.HookMeter();
				}
				if (!this._hookedToMeter)
				{
					return;
				}
			}
			if (this.fillImage == null)
			{
				return;
			}
			if (Mathf.Abs(this._targetValue - this._displayedValue) < this.fillSnapThreshold)
			{
				if (!Mathf.Approximately(this._displayedValue, this._targetValue))
				{
					this._displayedValue = this._targetValue;
					this.ApplyFillAmountToMaterial(this._displayedValue);
				}
				return;
			}
			this._displayedValue = Mathf.MoveTowards(this._displayedValue, this._targetValue, this.fillCatchUpSpeed * Time.deltaTime);
			this.ApplyFillAmountToMaterial(this._displayedValue);
		}

		// Token: 0x17001414 RID: 5140
		// (get) Token: 0x06007C48 RID: 31816 RVA: 0x0032C318 File Offset: 0x0032A518
		private bool ShouldShowBar
		{
			get
			{
				return this.meter != null && (this.meter.State != OrgasmMeterController.MeterState.Resting || this.meter.Value > 0f);
			}
		}

		// Token: 0x06007C49 RID: 31817 RVA: 0x0032C34C File Offset: 0x0032A54C
		private void LateUpdate()
		{
			this.SynchronizeRenderedMaterial();
			if (this.ShouldShowBar)
			{
				return;
			}
			if (this._hideTransitionPending && this.hideFeedback != null && this.hideFeedback.IsPlaying)
			{
				return;
			}
			this._hideTransitionPending = false;
			this.ApplyRestingBaseline();
		}

		// Token: 0x06007C4A RID: 31818 RVA: 0x0032C39C File Offset: 0x0032A59C
		private void ApplyRestingBaseline()
		{
			if (this.heartTransform != null && this.heartTransform.anchoredPosition != this.heartRestingAnchoredPos)
			{
				this.heartTransform.anchoredPosition = this.heartRestingAnchoredPos;
			}
			if (this.barTransform != null && this.barTransform.localScale.x != 0f)
			{
				Vector3 localScale = this.barTransform.localScale;
				localScale.x = 0f;
				this.barTransform.localScale = localScale;
			}
			if (this.barCanvasGroup != null && this.barCanvasGroup.alpha != 0f)
			{
				this.barCanvasGroup.alpha = 0f;
			}
			this._displayedValue = (this._targetValue = 0f);
			this.ApplyFillAmountToMaterial(0f);
			this.SetBarActive(false);
		}

		// Token: 0x06007C4B RID: 31819 RVA: 0x0032C480 File Offset: 0x0032A680
		private void SetBarActive(bool active)
		{
			Transform transform = ((this.barTransform != null) ? this.barTransform : ((this.barCanvasGroup != null) ? this.barCanvasGroup.transform : null));
			if (transform != null && transform != base.transform && transform.gameObject.activeSelf != active)
			{
				transform.gameObject.SetActive(active);
			}
		}

		// Token: 0x06007C4C RID: 31820 RVA: 0x0032C4F4 File Offset: 0x0032A6F4
		private void StopBarFeedbacks()
		{
			if (this.showFeedback != null)
			{
				this.showFeedback.StopFeedbacks();
			}
			if (this.hideFeedback != null)
			{
				this.hideFeedback.StopFeedbacks();
			}
			if (this.climaxFeedback != null)
			{
				this.climaxFeedback.StopFeedbacks();
			}
		}

		// Token: 0x06007C4D RID: 31821 RVA: 0x0032C54C File Offset: 0x0032A74C
		private void HandleFillingStart()
		{
			if (this.debugMode)
			{
				Debug.Log("[OrgasmMeterHUD] Show feedback");
			}
			this._hideTransitionPending = false;
			if (this.hideFeedback != null)
			{
				this.hideFeedback.StopFeedbacks();
			}
			this.SetBarActive(true);
			if (this.showFeedback != null)
			{
				this.showFeedback.PlayFeedbacks();
				return;
			}
			if (this.barCanvasGroup != null)
			{
				this.barCanvasGroup.alpha = 1f;
			}
			if (this.barTransform != null)
			{
				Vector3 localScale = this.barTransform.localScale;
				localScale.x = 1f;
				this.barTransform.localScale = localScale;
			}
		}

		// Token: 0x06007C4E RID: 31822 RVA: 0x0032C5FC File Offset: 0x0032A7FC
		private void HandleEmptied()
		{
			if (this.debugMode)
			{
				Debug.Log("[OrgasmMeterHUD] Hide feedback");
			}
			if (this.showFeedback != null)
			{
				this.showFeedback.StopFeedbacks();
			}
			if (this.climaxFeedback != null)
			{
				this.climaxFeedback.StopFeedbacks();
			}
			this._hideTransitionPending = true;
			if (this.hideFeedback != null)
			{
				this.hideFeedback.PlayFeedbacks();
			}
			else
			{
				this.ApplyRestingBaseline();
			}
			this._targetValue = 0f;
		}

		// Token: 0x06007C4F RID: 31823 RVA: 0x0032C680 File Offset: 0x0032A880
		private void HandleValueChanged(float v)
		{
			this._targetValue = v;
		}

		// Token: 0x04007FF7 RID: 32759
		[Header("References")]
		[Tooltip("Controller logico da osservare. Se null, viene preso dal singleton OrgasmMeterController.Instance allo Start.")]
		[SerializeField]
		private OrgasmMeterController meter;

		// Token: 0x04007FF8 RID: 32760
		[Tooltip("Image (filled) della barra. fillAmount viene lerpato verso meter.Value.")]
		[SerializeField]
		private Image fillImage;

		// Token: 0x04007FF9 RID: 32761
		[Header("Feedbacks (MMF)")]
		[Tooltip("Suonato sulla transizione Resting → Active: cuore center → left + bar scale-in.")]
		[SerializeField]
		private MMF_Player showFeedback;

		// Token: 0x04007FFA RID: 32762
		[Tooltip("Suonato quando il meter torna a Resting: cuore left → center + bar scale-out.")]
		[SerializeField]
		private MMF_Player hideFeedback;

		// Token: 0x04007FFB RID: 32763
		[Tooltip("Suonato quando il meter raggiunge il massimo: shake/bump della barra e (opzionale) camera shake. Lo aggiungi a inspector come MMF_Player con i feedback che vuoi.")]
		[SerializeField]
		private MMF_Player climaxFeedback;

		// Token: 0x04007FFC RID: 32764
		[Header("Fill Animation")]
		[Tooltip("Quanto la barra puo' avanzare al secondo verso il valore target. Valore alto = piu' reattiva, valore basso = effetto 'tick incrementale' piu' pronunciato.")]
		[Min(0.01f)]
		[SerializeField]
		private float fillCatchUpSpeed = 0.6f;

		// Token: 0x04007FFD RID: 32765
		[Tooltip("Snap immediato se la differenza scende sotto questa soglia (evita lerp eterno).")]
		[Min(0f)]
		[SerializeField]
		private float fillSnapThreshold = 0.002f;

		// Token: 0x04007FFE RID: 32766
		[Header("Initial State")]
		[Tooltip("Se true, all'avvio nasconde la barra (richiede che hideFeedback porti effettivamente la barra in stato 'closed'). Lascia false se gestisci la setup nel prefab.")]
		[SerializeField]
		private bool playHideOnStart;

		// Token: 0x04007FFF RID: 32767
		[Tooltip("Se true, in Awake forza lo stato Resting baseline: cuore a heartRestingAnchoredPos, barra scale.x=0 e barCanvasGroup.alpha=0. Risolve i flash visivi del primo frame dovuti a overrides di scena o a riordinamento del Canvas update.")]
		[SerializeField]
		private bool forceRestingBaselineOnAwake = true;

		// Token: 0x04008000 RID: 32768
		[Tooltip("RectTransform del cuore. Se forceRestingBaselineOnAwake = true, viene riportato a heartRestingAnchoredPos in Awake.")]
		[SerializeField]
		private RectTransform heartTransform;

		// Token: 0x04008001 RID: 32769
		[Tooltip("RectTransform della barra. Se forceRestingBaselineOnAwake = true, localScale.x viene azzerato in Awake.")]
		[SerializeField]
		private RectTransform barTransform;

		// Token: 0x04008002 RID: 32770
		[Tooltip("CanvasGroup della barra. Se forceRestingBaselineOnAwake = true, alpha viene azzerato in Awake.")]
		[SerializeField]
		private CanvasGroup barCanvasGroup;

		// Token: 0x04008003 RID: 32771
		[Tooltip("AnchoredPosition baseline del cuore in stato Resting.")]
		[SerializeField]
		private Vector2 heartRestingAnchoredPos = Vector2.zero;

		// Token: 0x04008004 RID: 32772
		[Header("Boot Reveal")]
		[Tooltip("CanvasGroup del root del prefab. Tenuto a alpha=0 per bootDelay secondi all'avvio, poi fade-in in bootFadeInDuration. Maschera eventuali assestamenti del primo frame del Canvas.")]
		[SerializeField]
		private CanvasGroup rootCanvasGroup;

		// Token: 0x04008005 RID: 32773
		[Tooltip("Secondi di delay dall'avvio prima che il widget cominci ad apparire.")]
		[Min(0f)]
		[SerializeField]
		private float bootDelay = 2f;

		// Token: 0x04008006 RID: 32774
		[Tooltip("Durata del fade-in di rivelazione del widget.")]
		[Min(0f)]
		[SerializeField]
		private float bootFadeInDuration = 0.4f;

		// Token: 0x04008007 RID: 32775
		[Header("Debug")]
		[SerializeField]
		private bool debugMode;

		// Token: 0x04008008 RID: 32776
		private float _displayedValue;

		// Token: 0x04008009 RID: 32777
		private float _targetValue;

		// Token: 0x0400800A RID: 32778
		private bool _hookedToMeter;

		// Token: 0x0400800B RID: 32779
		private Material _fillMaterialInstance;

		// Token: 0x0400800C RID: 32780
		private Coroutine _bootRevealCoroutine;

		// Token: 0x0400800D RID: 32781
		private bool _startCompleted;

		// Token: 0x0400800E RID: 32782
		private bool _bootRevealCompleted;

		// Token: 0x0400800F RID: 32783
		private bool _disabledBeforeStart;

		// Token: 0x04008010 RID: 32784
		private bool _hideTransitionPending;

		// Token: 0x04008011 RID: 32785
		private static readonly int FillAmountId = Shader.PropertyToID("_FillAmount");

		// Token: 0x04008012 RID: 32786
		private static readonly int HalfSizeId = Shader.PropertyToID("_halfSize");

		// Token: 0x04008013 RID: 32787
		private static readonly int RadiiId = Shader.PropertyToID("_r");

		// Token: 0x04008014 RID: 32788
		private static readonly int RectPropsId = Shader.PropertyToID("_rect2props");
	}
}
