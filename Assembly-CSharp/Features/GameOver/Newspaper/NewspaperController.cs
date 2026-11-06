using System;
using System.Collections;
using Features.CensorBars;
using Features.Localization;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;
using UnityEngine.UI;

namespace Features.GameOver.Newspaper
{
	// Token: 0x020011D6 RID: 4566
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-725)]
	public class NewspaperController : MonoBehaviour
	{
		// Token: 0x17001669 RID: 5737
		// (get) Token: 0x06008C4C RID: 35916 RVA: 0x00394CD1 File Offset: 0x00392ED1
		// (set) Token: 0x06008C4D RID: 35917 RVA: 0x00394CD8 File Offset: 0x00392ED8
		public static NewspaperController Instance { get; private set; }

		// Token: 0x1700166A RID: 5738
		// (get) Token: 0x06008C4E RID: 35918 RVA: 0x00394CE0 File Offset: 0x00392EE0
		// (set) Token: 0x06008C4F RID: 35919 RVA: 0x00394CE8 File Offset: 0x00392EE8
		public bool IsShowing { get; private set; }

		// Token: 0x06008C50 RID: 35920 RVA: 0x00394CF4 File Offset: 0x00392EF4
		private void Awake()
		{
			if (NewspaperController.Instance != null && NewspaperController.Instance != this)
			{
				Object.Destroy(this);
				return;
			}
			NewspaperController.Instance = this;
			if (this.rootObject == null)
			{
				this.rootObject = base.gameObject;
			}
			StreamerArtworkImage.BindKnownImages(this.rootObject.transform);
			if (this.rootGroup == null)
			{
				this.rootGroup = base.GetComponent<CanvasGroup>();
			}
			this.EnsureAudioSource();
			if (this.animatedRect != null)
			{
				this._basePosition = this.animatedRect.anchoredPosition;
				this._baseScale = this.animatedRect.localScale;
			}
			this.ResetVisualStateImmediate();
		}

		// Token: 0x06008C51 RID: 35921 RVA: 0x00394DA8 File Offset: 0x00392FA8
		private void OnDestroy()
		{
			this.RestoreCursor();
			if (NewspaperController.Instance == this)
			{
				NewspaperController.Instance = null;
			}
		}

		// Token: 0x06008C52 RID: 35922 RVA: 0x00394DC3 File Offset: 0x00392FC3
		public void Toggle()
		{
			if (this.IsShowing)
			{
				this.Hide();
				return;
			}
			this.Show();
		}

		// Token: 0x06008C53 RID: 35923 RVA: 0x00394DDC File Offset: 0x00392FDC
		public void Show()
		{
			if (this.IsShowing)
			{
				return;
			}
			this.EnsureGameObjectActive();
			this.IsShowing = true;
			this.BindPhoto();
			this._targetZoom = (this._currentZoom = this.minZoom);
			this._panOffset = Vector2.zero;
			this._dragging = false;
			this._interactReady = false;
			if (this.enableZoomPan)
			{
				this.UnlockCursorForUI();
			}
			if (this._animRoutine != null)
			{
				base.StopCoroutine(this._animRoutine);
			}
			this._animRoutine = base.StartCoroutine(this.PlayOpenAnimation());
			if (this.debugLogs)
			{
				Debug.Log("[Newspaper] Show");
			}
		}

		// Token: 0x06008C54 RID: 35924 RVA: 0x00394E7C File Offset: 0x0039307C
		public void Hide()
		{
			if (!this.IsShowing)
			{
				return;
			}
			this.IsShowing = false;
			this._interactReady = false;
			this._dragging = false;
			this.RestoreCursor();
			if (this._animRoutine != null)
			{
				base.StopCoroutine(this._animRoutine);
			}
			this._animRoutine = base.StartCoroutine(this.PlayCloseAnimation());
			if (this.debugLogs)
			{
				Debug.Log("[Newspaper] Hide");
			}
		}

		// Token: 0x06008C55 RID: 35925 RVA: 0x00394EE8 File Offset: 0x003930E8
		private void BindPhoto()
		{
			NewspaperPhotoService instance = NewspaperPhotoService.Instance;
			bool flag = instance != null && instance.HasPhoto && instance.Photo != null;
			if (this.photoSlot != null)
			{
				if (flag)
				{
					this.photoSlot.texture = instance.Photo;
				}
				StreamerCensorImage.Bind(this.photoSlot, false);
				this.photoSlot.gameObject.SetActive(flag);
			}
			if (this.photoPlaceholder != null)
			{
				this.photoPlaceholder.SetActive(!flag);
			}
			if (this.debugLogs && !flag)
			{
				Debug.LogWarning("[Newspaper] No photo available — showing placeholder.");
			}
		}

		// Token: 0x06008C56 RID: 35926 RVA: 0x00394F8D File Offset: 0x0039318D
		private IEnumerator PlayOpenAnimation()
		{
			this.SetInteractable(true);
			if (this.audioSource != null && this.openClip != null)
			{
				this.audioSource.PlayOneShot(this.openClip, this.openVolume);
			}
			float t = 0f;
			float startAngle = this.spinTurns * 360f;
			if (this.animatedRect != null)
			{
				this.animatedRect.anchoredPosition = this._basePosition;
				this.animatedRect.localScale = Vector3.zero;
				this.animatedRect.localEulerAngles = new Vector3(0f, 0f, startAngle);
			}
			if (this.rootGroup != null)
			{
				this.rootGroup.alpha = 1f;
			}
			while (t < this.openDuration)
			{
				t += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(t / this.openDuration);
				float num2 = this.openEasing.Evaluate(num);
				if (this.animatedRect != null)
				{
					this.animatedRect.localScale = this._baseScale * (this.endScale * num2);
					float num3 = Mathf.Lerp(startAngle, 0f, num);
					this.animatedRect.localEulerAngles = new Vector3(0f, 0f, num3);
				}
				yield return null;
			}
			if (this.animatedRect != null)
			{
				this.animatedRect.localScale = this._baseScale * this.endScale;
				this.animatedRect.localEulerAngles = Vector3.zero;
				this.animatedRect.anchoredPosition = this._basePosition;
			}
			this._interactReady = this.enableZoomPan;
			this._animRoutine = null;
			yield break;
		}

		// Token: 0x06008C57 RID: 35927 RVA: 0x00394F9C File Offset: 0x0039319C
		private IEnumerator PlayCloseAnimation()
		{
			this.SetInteractable(false);
			float startAlpha = ((this.rootGroup != null) ? this.rootGroup.alpha : 1f);
			float t = 0f;
			while (t < this.closeDuration)
			{
				t += Time.unscaledDeltaTime;
				float num = ((this.closeDuration > 0f) ? Mathf.Clamp01(t / this.closeDuration) : 1f);
				if (this.rootGroup != null)
				{
					this.rootGroup.alpha = Mathf.Lerp(startAlpha, 0f, num);
				}
				yield return null;
			}
			this.ResetVisualStateImmediate();
			this._animRoutine = null;
			yield break;
		}

		// Token: 0x06008C58 RID: 35928 RVA: 0x00394FAC File Offset: 0x003931AC
		private void Update()
		{
			if (this.IsShowing && !ControllerWorldPointer.BlocksContextInput && (Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.Escape) || (ControllerInput.IsActive && ControllerInput.GetKeyDown(KeyCode.N))))
			{
				EscapeInputCoordinator.Consume(this);
				ControllerInput.ConsumeUIInput();
				this.Hide();
				return;
			}
			if (this.IsShowing && ControllerInput.IsActive && !ControllerWorldPointer.BlocksContextInput)
			{
				if (ControllerInput.GetButtonDown(ControllerButton.East))
				{
					EscapeInputCoordinator.Consume(this);
					ControllerInput.ConsumeUIInput();
					this.Hide();
					return;
				}
				if (this._interactReady && this.enableZoomPan && this.animatedRect != null)
				{
					this._dragging = false;
					if (ControllerInput.GetButtonDown(ControllerButton.North))
					{
						this._targetZoom = this.minZoom;
						this._panOffset = Vector2.zero;
					}
					if (ControllerInput.Modifier)
					{
						this._targetZoom = NewspaperController.ControllerZoomTarget(this._targetZoom, ControllerInput.Look.y, Time.unscaledDeltaTime, this.minZoom, this.maxZoom, this.zoomStep);
					}
					else
					{
						this._panOffset += ControllerInput.Look * (Mathf.Min(this.ViewportSize().x, this.ViewportSize().y) * 0.65f * this.panSpeed * Time.unscaledDeltaTime);
					}
					this._currentZoom = Mathf.Lerp(this._currentZoom, this._targetZoom, 1f - Mathf.Exp(-this.zoomLerp * Time.unscaledDeltaTime));
					this.ApplyTransform();
				}
				ControllerInput.ConsumeUIInput();
				return;
			}
			else
			{
				if (!this.IsShowing || !this._interactReady || !this.enableZoomPan || this.animatedRect == null)
				{
					return;
				}
				this.HandleZoom();
				this.HandlePan();
				this.ApplyTransform();
				return;
			}
		}

		// Token: 0x06008C59 RID: 35929 RVA: 0x0039517A File Offset: 0x0039337A
		internal static float ControllerZoomTarget(float current, float stick, float elapsed, float minimum, float maximum, float step)
		{
			return Mathf.Clamp(current + stick * step * 3f * Mathf.Max(0f, elapsed), minimum, maximum);
		}

		// Token: 0x06008C5A RID: 35930 RVA: 0x0039519C File Offset: 0x0039339C
		private void OnGUI()
		{
			if (!this.IsShowing || !ControllerInput.IsActive || GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return;
			}
			GUI.Box(new Rect((float)Screen.width * 0.2f, (float)Screen.height - 48f, (float)Screen.width * 0.6f, 28f), GameLocalization.GetSource("input", "RS: pan    LT + RS: zoom    Y: reset    B: back"));
		}

		// Token: 0x06008C5B RID: 35931 RVA: 0x00395204 File Offset: 0x00393404
		private void HandleZoom()
		{
			float axis = GameplayInput.GetAxis("Mouse ScrollWheel");
			if (Mathf.Abs(axis) > 0.0001f)
			{
				this._targetZoom = Mathf.Clamp(this._targetZoom + Mathf.Sign(axis) * this.zoomStep, this.minZoom, this.maxZoom);
			}
			this._currentZoom = Mathf.Lerp(this._currentZoom, this._targetZoom, 1f - Mathf.Exp(-this.zoomLerp * Time.unscaledDeltaTime));
		}

		// Token: 0x06008C5C RID: 35932 RVA: 0x00395284 File Offset: 0x00393484
		private void HandlePan()
		{
			if (GameplayInput.GetMouseButtonDown(0))
			{
				this._dragging = true;
				this._lastMousePos = GameplayInput.mousePosition;
			}
			else if (GameplayInput.GetMouseButtonUp(0))
			{
				this._dragging = false;
			}
			if (this._dragging)
			{
				Vector3 vector = GameplayInput.mousePosition - this._lastMousePos;
				this._lastMousePos = GameplayInput.mousePosition;
				this._panOffset += new Vector2(vector.x, vector.y) * this.panSpeed;
			}
		}

		// Token: 0x06008C5D RID: 35933 RVA: 0x00395310 File Offset: 0x00393510
		private void ApplyTransform()
		{
			this.animatedRect.localScale = this._baseScale * (this.endScale * this._currentZoom);
			float num = this.endScale * this._currentZoom;
			Vector2 vector = this.animatedRect.rect.size * (this._baseScale.x * num);
			Vector2 vector2 = this.ViewportSize();
			float num2 = Mathf.Max(0f, (vector.x - vector2.x) * 0.5f);
			float num3 = Mathf.Max(0f, (vector.y - vector2.y) * 0.5f);
			this._panOffset.x = Mathf.Clamp(this._panOffset.x, -num2, num2);
			this._panOffset.y = Mathf.Clamp(this._panOffset.y, -num3, num3);
			this.animatedRect.anchoredPosition = this._basePosition + this._panOffset;
		}

		// Token: 0x06008C5E RID: 35934 RVA: 0x00395414 File Offset: 0x00393614
		private Vector2 ViewportSize()
		{
			RectTransform rectTransform = this.animatedRect.parent as RectTransform;
			if (rectTransform != null)
			{
				return rectTransform.rect.size;
			}
			return new Vector2((float)Screen.width, (float)Screen.height);
		}

		// Token: 0x06008C5F RID: 35935 RVA: 0x0039545B File Offset: 0x0039365B
		private void UnlockCursorForUI()
		{
			if (this._cursorStateSaved)
			{
				return;
			}
			this._savedCursorLock = Cursor.lockState;
			this._savedCursorVisible = Cursor.visible;
			this._cursorStateSaved = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		// Token: 0x06008C60 RID: 35936 RVA: 0x0039548F File Offset: 0x0039368F
		private void RestoreCursor()
		{
			if (!this._cursorStateSaved)
			{
				return;
			}
			Cursor.lockState = this._savedCursorLock;
			Cursor.visible = this._savedCursorVisible;
			this._cursorStateSaved = false;
		}

		// Token: 0x06008C61 RID: 35937 RVA: 0x003954B8 File Offset: 0x003936B8
		private void ResetVisualStateImmediate()
		{
			if (this.rootGroup != null)
			{
				this.rootGroup.alpha = 0f;
				this.rootGroup.interactable = false;
				this.rootGroup.blocksRaycasts = false;
			}
			if (this.animatedRect != null)
			{
				this.animatedRect.localScale = Vector3.zero;
				this.animatedRect.localEulerAngles = Vector3.zero;
				this.animatedRect.anchoredPosition = this._basePosition;
			}
			this._panOffset = Vector2.zero;
		}

		// Token: 0x06008C62 RID: 35938 RVA: 0x00395545 File Offset: 0x00393745
		private void SetInteractable(bool value)
		{
			if (this.rootGroup == null)
			{
				return;
			}
			this.rootGroup.interactable = value;
			this.rootGroup.blocksRaycasts = value;
		}

		// Token: 0x06008C63 RID: 35939 RVA: 0x0039556E File Offset: 0x0039376E
		private void EnsureGameObjectActive()
		{
			if (this.rootObject != null && !this.rootObject.activeSelf)
			{
				this.rootObject.SetActive(true);
			}
		}

		// Token: 0x06008C64 RID: 35940 RVA: 0x00395598 File Offset: 0x00393798
		private void EnsureAudioSource()
		{
			if (this.audioSource != null)
			{
				return;
			}
			this.audioSource = base.GetComponent<AudioSource>();
			if (this.audioSource == null)
			{
				this.audioSource = base.gameObject.AddComponent<AudioSource>();
				this.audioSource.playOnAwake = false;
				this.audioSource.spatialBlend = 0f;
			}
		}

		// Token: 0x04008F42 RID: 36674
		[Header("Root")]
		[Tooltip("CanvasGroup root del giornale (alpha + interactable + blocksRaycasts).")]
		[SerializeField]
		private CanvasGroup rootGroup;

		// Token: 0x04008F43 RID: 36675
		[Tooltip("GameObject root da riattivare in Show se inactive. Tipicamente il GO stesso.")]
		[SerializeField]
		private GameObject rootObject;

		// Token: 0x04008F44 RID: 36676
		[Tooltip("RectTransform animato in entrata (scale + spin). Tipicamente il root del giornale.")]
		[SerializeField]
		private RectTransform animatedRect;

		// Token: 0x04008F45 RID: 36677
		[Header("Photo")]
		[Tooltip("RawImage del riquadro centrale dove appare la foto catturata.")]
		[SerializeField]
		private RawImage photoSlot;

		// Token: 0x04008F46 RID: 36678
		[Tooltip("Mostrato quando non c'e' nessuna foto catturata (caso difensivo: la cattura Polizia garantisce sempre la foto). Opzionale.")]
		[SerializeField]
		private GameObject photoPlaceholder;

		// Token: 0x04008F47 RID: 36679
		[Header("Open Animation (cartoon)")]
		[Tooltip("Durata dell'animazione di entrata (s), su tempo non scalato.")]
		[Min(0.05f)]
		[SerializeField]
		private float openDuration = 0.5f;

		// Token: 0x04008F48 RID: 36680
		[Tooltip("Numero di giri completi durante l'entrata (spin cartoon).")]
		[Min(0f)]
		[SerializeField]
		private float spinTurns = 1.5f;

		// Token: 0x04008F49 RID: 36681
		[Tooltip("Scala finale del giornale.")]
		[SerializeField]
		private float endScale = 1f;

		// Token: 0x04008F4A RID: 36682
		[Tooltip("Easing dell'entrata: overshoot 'back' (supera 1 e rientra) per il feel cartoon.")]
		[SerializeField]
		private AnimationCurve openEasing = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0f, 0f),
			new Keyframe(0.7f, 1.1f, 0.5f, 0.5f),
			new Keyframe(1f, 1f, 0f, 0f)
		});

		// Token: 0x04008F4B RID: 36683
		[Tooltip("Durata fade-out alla chiusura (s).")]
		[Min(0f)]
		[SerializeField]
		private float closeDuration = 0.2f;

		// Token: 0x04008F4C RID: 36684
		[Header("Audio")]
		[Tooltip("Clip suonata all'apertura del giornale. Opzionale.")]
		[SerializeField]
		private AudioClip openClip;

		// Token: 0x04008F4D RID: 36685
		[Tooltip("AudioSource. Se null, creato runtime su questo GO.")]
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04008F4E RID: 36686
		[Range(0f, 1f)]
		[SerializeField]
		private float openVolume = 0.8f;

		// Token: 0x04008F4F RID: 36687
		[Header("Zoom & Pan (giornale aperto)")]
		[Tooltip("Se true, con giornale aperto la rotella zooma e il drag (tasto sx) sposta il foglio entro i bordi. Il cursore viene sbloccato in Show e ripristinato in Hide.")]
		[SerializeField]
		private bool enableZoomPan = true;

		// Token: 0x04008F50 RID: 36688
		[Tooltip("Zoom minimo (1 = giornale intero, niente pan possibile).")]
		[Min(0.1f)]
		[SerializeField]
		private float minZoom = 1f;

		// Token: 0x04008F51 RID: 36689
		[Tooltip("Zoom massimo. Con la foto a 1920px c'e' ampio margine prima di sgranare.")]
		[Min(1f)]
		[SerializeField]
		private float maxZoom = 4f;

		// Token: 0x04008F52 RID: 36690
		[Tooltip("Velocita' di risposta della rotella (incremento zoom per tacca).")]
		[SerializeField]
		private float zoomStep = 0.35f;

		// Token: 0x04008F53 RID: 36691
		[Tooltip("Morbidezza dello zoom (lerp). Piu' alto = piu' reattivo.")]
		[SerializeField]
		private float zoomLerp = 12f;

		// Token: 0x04008F54 RID: 36692
		[Tooltip("Velocita' del pan in drag (moltiplicatore dello spostamento del mouse).")]
		[SerializeField]
		private float panSpeed = 1f;

		// Token: 0x04008F55 RID: 36693
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008F57 RID: 36695
		private Coroutine _animRoutine;

		// Token: 0x04008F58 RID: 36696
		private float _targetZoom = 1f;

		// Token: 0x04008F59 RID: 36697
		private float _currentZoom = 1f;

		// Token: 0x04008F5A RID: 36698
		private Vector2 _panOffset;

		// Token: 0x04008F5B RID: 36699
		private Vector2 _basePosition;

		// Token: 0x04008F5C RID: 36700
		private Vector3 _baseScale = Vector3.one;

		// Token: 0x04008F5D RID: 36701
		private bool _dragging;

		// Token: 0x04008F5E RID: 36702
		private Vector3 _lastMousePos;

		// Token: 0x04008F5F RID: 36703
		private bool _interactReady;

		// Token: 0x04008F60 RID: 36704
		private bool _cursorStateSaved;

		// Token: 0x04008F61 RID: 36705
		private CursorLockMode _savedCursorLock;

		// Token: 0x04008F62 RID: 36706
		private bool _savedCursorVisible;
	}
}
