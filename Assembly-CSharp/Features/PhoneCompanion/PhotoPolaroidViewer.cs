using System;
using System.Collections;
using System.IO;
using Features.CensorBars;
using Features.Localization;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Interactions;
using ProjektSumperk;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E5E RID: 3678
	[DisallowMultipleComponent]
	public class PhotoPolaroidViewer : MonoBehaviour
	{
		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x06006DD1 RID: 28113 RVA: 0x002C2371 File Offset: 0x002C0571
		// (set) Token: 0x06006DD2 RID: 28114 RVA: 0x002C2378 File Offset: 0x002C0578
		public static bool IsOpen { get; private set; }

		// Token: 0x06006DD3 RID: 28115 RVA: 0x002C2380 File Offset: 0x002C0580
		public static bool CloseIfOpen()
		{
			if (!PhotoPolaroidViewer.IsOpen || PhotoPolaroidViewer._activeViewer == null)
			{
				return false;
			}
			PhotoPolaroidViewer._activeViewer.Close();
			return true;
		}

		// Token: 0x06006DD4 RID: 28116 RVA: 0x002C23A4 File Offset: 0x002C05A4
		public static void Show(Sprite sprite)
		{
			if (sprite == null)
			{
				return;
			}
			if (PhotoPolaroidViewer._standaloneInstance == null)
			{
				GameObject gameObject = new GameObject("PhotoPolaroidViewer (standalone)");
				gameObject.SetActive(false);
				Object.DontDestroyOnLoad(gameObject);
				PhotoPolaroidViewer._standaloneInstance = gameObject.AddComponent<PhotoPolaroidViewer>();
				PhotoPolaroidViewer._standaloneInstance._standalone = true;
				gameObject.SetActive(true);
			}
			PhotoPolaroidViewer._standaloneInstance.ShowPolaroid(sprite);
		}

		// Token: 0x06006DD5 RID: 28117 RVA: 0x002C2406 File Offset: 0x002C0606
		private void Awake()
		{
			this._sourceImage = base.GetComponent<Image>();
			StreamerCensorImage.Bind(this._sourceImage, false);
		}

		// Token: 0x06006DD6 RID: 28118 RVA: 0x002C2420 File Offset: 0x002C0620
		private void OnEnable()
		{
			if (this._standalone)
			{
				return;
			}
			base.StartCoroutine(this.GrabSpriteEndOfFrame());
		}

		// Token: 0x06006DD7 RID: 28119 RVA: 0x002C2438 File Offset: 0x002C0638
		private IEnumerator GrabSpriteEndOfFrame()
		{
			yield return new WaitForEndOfFrame();
			if (this._sourceImage == null)
			{
				this._sourceImage = base.GetComponent<Image>();
			}
			StreamerCensorImage.Bind(this._sourceImage, false);
			Sprite sprite = ((this._sourceImage != null) ? this._sourceImage.sprite : null);
			this.ShowPolaroid(sprite);
			if (this._sourceImage != null)
			{
				this._sourceImage.enabled = false;
			}
			yield break;
		}

		// Token: 0x06006DD8 RID: 28120 RVA: 0x002C2448 File Offset: 0x002C0648
		private void ShowPolaroid(Sprite sprite)
		{
			if (sprite == null)
			{
				return;
			}
			if (PhotoPolaroidViewer._activeViewer != null && PhotoPolaroidViewer._activeViewer != this)
			{
				PhotoPolaroidViewer._activeViewer.Close();
			}
			this.EnsureViewer();
			this._currentSprite = sprite;
			float num = ((sprite.rect.height > 0f) ? (sprite.rect.width / sprite.rect.height) : 1.7777778f);
			float num2 = 1080f * this.heightFraction;
			float num3 = num2 * num;
			RectTransform rectTransform = (RectTransform)this._photoImage.transform.parent;
			rectTransform.sizeDelta = new Vector2(num3 + this.frameBorder * 2f, num2 + this.frameBorder * 2f);
			((RectTransform)this._photoImage.transform).sizeDelta = new Vector2(num3, num2);
			this._photoImage.sprite = sprite;
			StreamerCensorImage.Bind(this._photoImage, false);
			this._photoImage.preserveAspect = true;
			this._zoom = 1f;
			this._zoomTarget = 1f;
			this._pan = Vector2.zero;
			this._panTarget = Vector2.zero;
			rectTransform.localScale = Vector3.one;
			rectTransform.anchoredPosition = Vector2.zero;
			this._panRoot = rectTransform;
			this._viewerCanvas.gameObject.SetActive(true);
			PhotoPolaroidViewer.IsOpen = true;
			PhotoPolaroidViewer._activeViewer = this;
			this.LiftInteractionHud(true);
			PhoneHudHints.ShowPhotoViewer();
			this.ApplyLiftedHudSorting();
		}

		// Token: 0x06006DD9 RID: 28121 RVA: 0x002C25D4 File Offset: 0x002C07D4
		private void LiftInteractionHud(bool lift)
		{
			if (!lift)
			{
				if (!this._hudSortingCaptured)
				{
					return;
				}
				if (this._hudCanvas != null)
				{
					this._hudCanvas.sortingOrder = this._hudSavedOrder;
					this._hudCanvas.overrideSorting = this._hudSavedOverride;
				}
				this._hudSortingCaptured = false;
				this._hudCanvas = null;
				return;
			}
			else
			{
				if (this._hudSortingCaptured && this._hudCanvas != null)
				{
					return;
				}
				this._hudSortingCaptured = false;
				InteractionHUDController instance = InteractionHUDController.Instance;
				if (instance == null)
				{
					return;
				}
				this._hudCanvas = instance.GetComponent<Canvas>();
				if (this._hudCanvas == null)
				{
					Canvas componentInParent = instance.GetComponentInParent<Canvas>(true);
					this._hudCanvas = instance.gameObject.AddComponent<Canvas>();
					if (componentInParent != null)
					{
						this._hudCanvas.additionalShaderChannels = componentInParent.additionalShaderChannels;
					}
				}
				GameplayHudVisibility.RegisterCanvas(this._hudCanvas);
				this._hudSavedOverride = this._hudCanvas.overrideSorting;
				this._hudSavedOrder = this._hudCanvas.sortingOrder;
				this._hudSortingCaptured = true;
				this.ApplyLiftedHudSorting();
				return;
			}
		}

		// Token: 0x06006DDA RID: 28122 RVA: 0x002C26E4 File Offset: 0x002C08E4
		private void ApplyLiftedHudSorting()
		{
			if (!this._hudSortingCaptured || this._hudCanvas == null || !this._hudCanvas.isActiveAndEnabled)
			{
				return;
			}
			this._hudCanvas.overrideSorting = true;
			this._hudCanvas.sortingOrder = 1200;
		}

		// Token: 0x06006DDB RID: 28123 RVA: 0x002C2734 File Offset: 0x002C0934
		private void EnsureViewer()
		{
			if (this._viewerCanvas != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("PhotoPolaroidCanvas", new Type[]
			{
				typeof(Canvas),
				typeof(CanvasScaler),
				typeof(GraphicRaycaster)
			});
			this._viewerCanvas = gameObject.GetComponent<Canvas>();
			this._viewerCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this._viewerCanvas.sortingOrder = 1100;
			GameplayHudVisibility.RegisterCanvas(this._viewerCanvas);
			CanvasScaler component = gameObject.GetComponent<CanvasScaler>();
			component.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component.referenceResolution = new Vector2(1920f, 1080f);
			component.matchWidthOrHeight = 1f;
			Object.DontDestroyOnLoad(gameObject);
			this._viewerRoot = gameObject;
			GameObject gameObject2 = new GameObject("Backdrop", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(PhotoPolaroidViewer.CloseOnClick)
			});
			RectTransform rectTransform = (RectTransform)gameObject2.transform;
			rectTransform.SetParent(gameObject.transform, false);
			PhotoPolaroidViewer.StretchFull(rectTransform);
			gameObject2.GetComponent<Image>().color = new Color(0f, 0f, 0f, this.backdropAlpha);
			gameObject2.GetComponent<PhotoPolaroidViewer.CloseOnClick>().Init(this);
			GameObject gameObject3 = new GameObject("Shadow", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			RectTransform rectTransform2 = (RectTransform)gameObject3.transform;
			rectTransform2.SetParent(gameObject.transform, false);
			RectTransform rectTransform3 = rectTransform2;
			RectTransform rectTransform4 = rectTransform2;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform4.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			rectTransform2.pivot = new Vector2(0.5f, 0.5f);
			rectTransform2.anchoredPosition = new Vector2(0f, -10f);
			Image component2 = gameObject3.GetComponent<Image>();
			component2.color = new Color(0f, 0f, 0f, 0.35f);
			component2.raycastTarget = false;
			GameObject gameObject4 = new GameObject("Frame", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			RectTransform rectTransform5 = (RectTransform)gameObject4.transform;
			rectTransform5.SetParent(gameObject.transform, false);
			RectTransform rectTransform6 = rectTransform5;
			RectTransform rectTransform7 = rectTransform5;
			vector = new Vector2(0.5f, 0.5f);
			rectTransform7.anchorMax = vector;
			rectTransform6.anchorMin = vector;
			rectTransform5.pivot = new Vector2(0.5f, 0.5f);
			rectTransform5.anchoredPosition = Vector2.zero;
			Image component3 = gameObject4.GetComponent<Image>();
			component3.color = Color.white;
			component3.raycastTarget = false;
			GameObject gameObject5 = new GameObject("Photo", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			RectTransform rectTransform8 = (RectTransform)gameObject5.transform;
			rectTransform8.SetParent(rectTransform5, false);
			vector = new Vector2(0.5f, 0.5f);
			rectTransform8.anchorMax = vector;
			rectTransform8.anchorMin = vector;
			rectTransform8.pivot = new Vector2(0.5f, 0.5f);
			rectTransform8.anchoredPosition = Vector2.zero;
			this._photoImage = gameObject5.GetComponent<Image>();
			this._photoImage.raycastTarget = false;
			rectTransform2.sizeDelta = new Vector2(20f, 20f);
			this._shadowRt = rectTransform2;
			this._frameRtRef = rectTransform5;
			if (!this._standalone)
			{
				this.BuildActionButtons(gameObject.transform);
			}
		}

		// Token: 0x06006DDC RID: 28124 RVA: 0x002C2A98 File Offset: 0x002C0C98
		private void BuildActionButtons(Transform canvasParent)
		{
			GameObject gameObject = new GameObject("ActionBar", new Type[]
			{
				typeof(RectTransform),
				typeof(HorizontalLayoutGroup),
				typeof(ContentSizeFitter)
			});
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(canvasParent, false);
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			Vector2 vector = new Vector2(1f, 0f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform.pivot = new Vector2(1f, 0f);
			rectTransform.anchoredPosition = new Vector2(-32f, 32f);
			HorizontalLayoutGroup component = gameObject.GetComponent<HorizontalLayoutGroup>();
			component.spacing = 12f;
			component.childAlignment = TextAnchor.MiddleRight;
			component.childControlWidth = true;
			component.childControlHeight = true;
			component.childForceExpandWidth = false;
			component.childForceExpandHeight = false;
			ContentSizeFitter component2 = gameObject.GetComponent<ContentSizeFitter>();
			component2.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			component2.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			this._saveBtnLabel = this.MakeActionButton(rectTransform, "Save to PC", new Color(0.16f, 0.45f, 0.85f, 0.95f), 150f, new UnityAction(this.SaveCurrentPhotoToPC));
			this.MakeActionButton(rectTransform, "Open folder", new Color(0.16f, 0.25f, 0.36f, 0.95f), 160f, new UnityAction(PlayerMediaExport.OpenPhotosFolder));
			this.MakeActionButton(rectTransform, "Delete", new Color(0.82f, 0.16f, 0.16f, 0.95f), 120f, new UnityAction(this.DeleteCurrentPhoto));
			GameObject gameObject2 = new GameObject("ExportLocation", new Type[]
			{
				typeof(RectTransform),
				typeof(Text)
			});
			gameObject2.transform.SetParent(canvasParent, false);
			Text component3 = gameObject2.GetComponent<Text>();
			component3.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component3.fontSize = 17;
			component3.color = new Color(1f, 1f, 1f, 0.8f);
			component3.alignment = TextAnchor.LowerLeft;
			component3.raycastTarget = false;
			LocalizedLegacyGameText.BindComputed(component3, () => GameLocalization.FormatSource("phone_story", "{0} / Photos", new object[] { PlayerMediaExport.LocationLabel }));
			RectTransform rectTransform4 = component3.rectTransform;
			vector = (component3.rectTransform.anchorMax = Vector2.zero);
			rectTransform4.anchorMin = vector;
			component3.rectTransform.pivot = Vector2.zero;
			component3.rectTransform.anchoredPosition = new Vector2(32f, 34f);
			component3.rectTransform.sizeDelta = new Vector2(850f, 50f);
		}

		// Token: 0x06006DDD RID: 28125 RVA: 0x002C2D34 File Offset: 0x002C0F34
		private Text MakeActionButton(Transform parent, string label, Color color, float width, UnityAction onClick)
		{
			GameObject gameObject = new GameObject(label, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button),
				typeof(LayoutElement)
			});
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(parent, false);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.preferredWidth = width;
			component.preferredHeight = 52f;
			Image component2 = gameObject.GetComponent<Image>();
			component2.color = color;
			component2.sprite = null;
			component2.type = Image.Type.Simple;
			gameObject.GetComponent<Button>().onClick.AddListener(onClick);
			GameObject gameObject2 = new GameObject("Label", new Type[]
			{
				typeof(RectTransform),
				typeof(Text)
			});
			RectTransform rectTransform2 = (RectTransform)gameObject2.transform;
			rectTransform2.SetParent(rectTransform, false);
			PhotoPolaroidViewer.StretchFull(rectTransform2);
			Text component3 = gameObject2.GetComponent<Text>();
			component3.text = label;
			component3.alignment = TextAnchor.MiddleCenter;
			component3.color = Color.white;
			component3.fontSize = 22;
			component3.fontStyle = FontStyle.Bold;
			component3.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component3.raycastTarget = false;
			LocalizedLegacyGameText.BindSource(component3, "phone_story", label);
			return component3;
		}

		// Token: 0x06006DDE RID: 28126 RVA: 0x002C2E6C File Offset: 0x002C106C
		private void DeleteCurrentPhoto()
		{
			string text = ((this._currentSprite != null) ? this._currentSprite.name : null);
			if (!string.IsNullOrEmpty(text) && File.Exists(text))
			{
				try
				{
					File.Delete(text);
				}
				catch (Exception ex)
				{
					Debug.LogWarning("[PhotoPolaroidViewer] Delete file fallito: " + ex.Message);
				}
			}
			if (this._currentSprite != null)
			{
				PhotoGallery photoGallery = Object.FindObjectOfType<PhotoGallery>(true);
				if (photoGallery != null && photoGallery.parent != null)
				{
					foreach (Image image in photoGallery.parent.GetComponentsInChildren<Image>(true))
					{
						if (image.sprite == this._currentSprite)
						{
							Transform transform = image.transform;
							while (transform.parent != null && transform.parent != photoGallery.parent)
							{
								transform = transform.parent;
							}
							Object.Destroy(transform.gameObject);
							break;
						}
					}
				}
			}
			this.Close();
		}

		// Token: 0x06006DDF RID: 28127 RVA: 0x002C2F8C File Offset: 0x002C118C
		private void SaveCurrentPhotoToPC()
		{
			string text = ((this._currentSprite != null) ? this._currentSprite.name : null);
			if (string.IsNullOrEmpty(text) || !File.Exists(text))
			{
				Debug.LogWarning("[PhotoPolaroidViewer] Save to PC: file sorgente non trovato.");
				return;
			}
			try
			{
				string text2 = PlayerMediaExport.CopyPhoto(text);
				this.ShowSaveFeedback("Saved!");
				Debug.Log("[PhotoPolaroidViewer] Foto salvata sul PC: " + text2);
			}
			catch (Exception ex)
			{
				Debug.LogError("[PhotoPolaroidViewer] Save to PC fallito: " + ex.Message);
				this.ShowSaveFeedback("Failed");
			}
		}

		// Token: 0x06006DE0 RID: 28128 RVA: 0x002C302C File Offset: 0x002C122C
		private void ShowSaveFeedback(string msg)
		{
			if (this._saveBtnLabel == null)
			{
				return;
			}
			base.StopCoroutine("SaveFeedbackRoutine");
			base.StartCoroutine(this.SaveFeedbackRoutine(msg));
		}

		// Token: 0x06006DE1 RID: 28129 RVA: 0x002C3056 File Offset: 0x002C1256
		private IEnumerator SaveFeedbackRoutine(string msg)
		{
			string original = "Save to PC";
			LocalizedLegacyGameText.BindSource(this._saveBtnLabel, "phone_story", msg);
			yield return new WaitForSecondsRealtime(1.2f);
			if (this._saveBtnLabel != null)
			{
				LocalizedLegacyGameText.BindSource(this._saveBtnLabel, "phone_story", original);
			}
			yield break;
		}

		// Token: 0x06006DE2 RID: 28130 RVA: 0x002C306C File Offset: 0x002C126C
		private void Update()
		{
			if (!PhotoPolaroidViewer.IsOpen)
			{
				return;
			}
			if (PhotoPolaroidViewer._activeViewer == this && EscapeInputCoordinator.TryConsume(this))
			{
				this.Close();
				return;
			}
			float axis = GameplayInput.GetAxis("Mouse ScrollWheel");
			if (Mathf.Abs(axis) > 0.0001f)
			{
				this._zoomTarget = Mathf.Clamp(this._zoomTarget + Mathf.Sign(axis) * this.zoomStep, this.minZoom, this.maxZoom);
			}
			if (ControllerInput.IsControllerActive && !GraphicsSettingsRuntimeUI.AnyOpen)
			{
				if (ControllerInput.GetButton(ControllerButton.LeftTrigger))
				{
					this._zoomTarget = Mathf.Clamp(this._zoomTarget + ControllerInput.Look.y * Time.unscaledDeltaTime * 2f, this.minZoom, this.maxZoom);
				}
				else
				{
					this._panTarget += ControllerInput.Look * (450f * Time.unscaledDeltaTime);
				}
			}
			this._zoom = Mathf.Lerp(this._zoom, this._zoomTarget, 1f - Mathf.Exp(-this.zoomSmooth * Time.unscaledDeltaTime));
			if (this._frameRtRef != null)
			{
				this._frameRtRef.localScale = Vector3.one * this._zoom;
			}
			if (this._panRoot != null)
			{
				this._panTarget = this.ClampPan(this._panTarget);
				this._pan = Vector2.Lerp(this._pan, this._panTarget, 1f - Mathf.Exp(-this.panSmooth * Time.unscaledDeltaTime));
				this._panRoot.anchoredPosition = this._pan;
			}
		}

		// Token: 0x06006DE3 RID: 28131 RVA: 0x002C320A File Offset: 0x002C140A
		public void AddPan(Vector2 delta)
		{
			this._panTarget = this.ClampPan(this._panTarget + delta);
		}

		// Token: 0x06006DE4 RID: 28132 RVA: 0x002C3224 File Offset: 0x002C1424
		private Vector2 ClampPan(Vector2 p)
		{
			if (this._frameRtRef == null)
			{
				return Vector2.zero;
			}
			Vector2 vector = this._frameRtRef.sizeDelta * this._zoom;
			Vector2 vector2 = new Vector2(1920f, 1080f);
			float num = Mathf.Max(0f, (vector.x - vector2.x) * 0.5f);
			float num2 = Mathf.Max(0f, (vector.y - vector2.y) * 0.5f);
			return new Vector2(Mathf.Clamp(p.x, -num, num), Mathf.Clamp(p.y, -num2, num2));
		}

		// Token: 0x06006DE5 RID: 28133 RVA: 0x002C32CC File Offset: 0x002C14CC
		private void LateUpdate()
		{
			if (PhotoPolaroidViewer._activeViewer == this && PhotoPolaroidViewer.IsOpen)
			{
				PhoneHudHints.ShowPhotoViewer();
				this.ApplyLiftedHudSorting();
			}
			if (this._shadowRt != null && this._frameRtRef != null)
			{
				this._shadowRt.sizeDelta = this._frameRtRef.sizeDelta + new Vector2(36f, 36f);
				this._shadowRt.localScale = this._frameRtRef.localScale;
			}
		}

		// Token: 0x06006DE6 RID: 28134 RVA: 0x002C3354 File Offset: 0x002C1554
		public void Close()
		{
			this.LiftInteractionHud(false);
			if (this._viewerCanvas != null)
			{
				this._viewerCanvas.gameObject.SetActive(false);
			}
			if (PhotoPolaroidViewer._activeViewer == this)
			{
				PhoneHudHints.HidePhotoViewer();
				PhotoPolaroidViewer.IsOpen = false;
				PhotoPolaroidViewer._activeViewer = null;
			}
			if (this._standalone)
			{
				return;
			}
			if (this._sourceImage != null)
			{
				this._sourceImage.enabled = true;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x06006DE7 RID: 28135 RVA: 0x002C33D4 File Offset: 0x002C15D4
		private void OnDisable()
		{
			if (PhotoPolaroidViewer._activeViewer != this)
			{
				return;
			}
			this.LiftInteractionHud(false);
			if (this._viewerCanvas != null)
			{
				this._viewerCanvas.gameObject.SetActive(false);
			}
			PhoneHudHints.HidePhotoViewer();
			PhotoPolaroidViewer.IsOpen = false;
			PhotoPolaroidViewer._activeViewer = null;
			if (this._sourceImage != null)
			{
				this._sourceImage.enabled = true;
			}
		}

		// Token: 0x06006DE8 RID: 28136 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void StretchFull(RectTransform rt)
		{
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
		}

		// Token: 0x06006DE9 RID: 28137 RVA: 0x002C3440 File Offset: 0x002C1640
		private void OnDestroy()
		{
			this.LiftInteractionHud(false);
			if (this._viewerRoot != null)
			{
				Object.Destroy(this._viewerRoot);
			}
			if (PhotoPolaroidViewer._activeViewer == this)
			{
				PhoneHudHints.HidePhotoViewer();
				PhotoPolaroidViewer.IsOpen = false;
				PhotoPolaroidViewer._activeViewer = null;
			}
			if (PhotoPolaroidViewer._standaloneInstance == this)
			{
				PhotoPolaroidViewer._standaloneInstance = null;
			}
		}

		// Token: 0x04007066 RID: 28774
		[Tooltip("Frazione dell'altezza schermo occupata dalla foto (0..1).")]
		[Range(0.3f, 0.95f)]
		public float heightFraction = 0.72f;

		// Token: 0x04007067 RID: 28775
		[Tooltip("Spessore cornice bianca (px, su schermo 1080p di riferimento).")]
		public float frameBorder = 28f;

		// Token: 0x04007068 RID: 28776
		[Tooltip("Opacita' dello sfondo scurito dietro la polaroid.")]
		[Range(0f, 1f)]
		public float backdropAlpha = 0.78f;

		// Token: 0x04007069 RID: 28777
		[Header("Zoom foto (rotellina)")]
		[Tooltip("Zoom massimo della foto (1 = dimensione base).")]
		public float maxZoom = 5f;

		// Token: 0x0400706A RID: 28778
		[Tooltip("Zoom minimo (puo' essere < 1 per rimpicciolire leggermente).")]
		public float minZoom = 1f;

		// Token: 0x0400706B RID: 28779
		[Tooltip("Velocita' dello zoom per tacca di rotellina.")]
		public float zoomStep = 0.35f;

		// Token: 0x0400706C RID: 28780
		[Tooltip("Velocita' di smoothing dello zoom.")]
		public float zoomSmooth = 12f;

		// Token: 0x0400706E RID: 28782
		private static PhotoPolaroidViewer _activeViewer;

		// Token: 0x0400706F RID: 28783
		private static PhotoPolaroidViewer _standaloneInstance;

		// Token: 0x04007070 RID: 28784
		private bool _standalone;

		// Token: 0x04007071 RID: 28785
		private Image _sourceImage;

		// Token: 0x04007072 RID: 28786
		private Canvas _viewerCanvas;

		// Token: 0x04007073 RID: 28787
		private GameObject _viewerRoot;

		// Token: 0x04007074 RID: 28788
		private Image _photoImage;

		// Token: 0x04007075 RID: 28789
		private RectTransform _frameRoot;

		// Token: 0x04007076 RID: 28790
		private float _zoom = 1f;

		// Token: 0x04007077 RID: 28791
		private float _zoomTarget = 1f;

		// Token: 0x04007078 RID: 28792
		[Header("Pan (click sinistro + drag quando zoomata)")]
		[Tooltip("Distanza (px) oltre cui un click col tasto sinistro e' considerato un DRAG (pan) e NON chiude il viewer. Sotto questa soglia e' un click 'pulito' che chiude.")]
		public float dragThreshold = 8f;

		// Token: 0x04007079 RID: 28793
		[Tooltip("Smoothing del pan/zoom della foto: piu' BASSO = piu' fluido/morbido, piu' alto = piu' reattivo.")]
		public float panSmooth = 14f;

		// Token: 0x0400707A RID: 28794
		private Vector2 _pan;

		// Token: 0x0400707B RID: 28795
		private Vector2 _panTarget;

		// Token: 0x0400707C RID: 28796
		private RectTransform _panRoot;

		// Token: 0x0400707D RID: 28797
		private Sprite _currentSprite;

		// Token: 0x0400707E RID: 28798
		private Canvas _hudCanvas;

		// Token: 0x0400707F RID: 28799
		private int _hudSavedOrder;

		// Token: 0x04007080 RID: 28800
		private bool _hudSavedOverride;

		// Token: 0x04007081 RID: 28801
		private bool _hudSortingCaptured;

		// Token: 0x04007082 RID: 28802
		private Text _saveBtnLabel;

		// Token: 0x04007083 RID: 28803
		private RectTransform _shadowRt;

		// Token: 0x04007084 RID: 28804
		private RectTransform _frameRtRef;

		// Token: 0x02000E5F RID: 3679
		private class CloseOnClick : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler, IPointerUpHandler
		{
			// Token: 0x06006DEB RID: 28139 RVA: 0x002C352C File Offset: 0x002C172C
			public void Init(PhotoPolaroidViewer owner)
			{
				this._owner = owner;
			}

			// Token: 0x06006DEC RID: 28140 RVA: 0x002C3535 File Offset: 0x002C1735
			public void OnPointerDown(PointerEventData e)
			{
				this._dragging = true;
				this._moved = 0f;
			}

			// Token: 0x06006DED RID: 28141 RVA: 0x002C354C File Offset: 0x002C174C
			public void OnDrag(PointerEventData e)
			{
				if (!this._dragging || this._owner == null)
				{
					return;
				}
				this._moved += e.delta.magnitude;
				float num = ((Screen.height > 0) ? (1080f / (float)Screen.height) : 1f);
				this._owner.AddPan(e.delta * num);
			}

			// Token: 0x06006DEE RID: 28142 RVA: 0x002C35BE File Offset: 0x002C17BE
			public void OnPointerUp(PointerEventData e)
			{
				if (!this._dragging)
				{
					return;
				}
				this._dragging = false;
				if (this._owner != null && this._moved <= this._owner.dragThreshold)
				{
					this._owner.Close();
				}
			}

			// Token: 0x04007085 RID: 28805
			private PhotoPolaroidViewer _owner;

			// Token: 0x04007086 RID: 28806
			private bool _dragging;

			// Token: 0x04007087 RID: 28807
			private float _moved;
		}
	}
}
