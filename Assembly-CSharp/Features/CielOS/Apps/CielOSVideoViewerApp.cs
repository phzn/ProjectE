using System;
using System.IO;
using Features.CensorBars;
using Features.CielOS.VFS;
using Features.Localization;
using Features.MainMenu;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Features.CielOS.Apps
{
	// Token: 0x02001418 RID: 5144
	public sealed class CielOSVideoViewerApp : MonoBehaviour
	{
		// Token: 0x06009EB3 RID: 40627 RVA: 0x0041491C File Offset: 0x00412B1C
		public static CielOSVideoViewerApp Mount(RectTransform content, string virtualPath)
		{
			if (content == null)
			{
				return null;
			}
			GameObject gameObject = new GameObject("VideoViewer", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(CielOSVideoViewerApp)
			});
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(content, false);
			CielOSVideoViewerApp.Stretch(rectTransform);
			CielOSVideoViewerApp component = gameObject.GetComponent<CielOSVideoViewerApp>();
			component.Build();
			component.Load(virtualPath);
			return component;
		}

		// Token: 0x06009EB4 RID: 40628 RVA: 0x00414998 File Offset: 0x00412B98
		private void Build()
		{
			CielOSTheme active = CielOSTheme.Active;
			base.GetComponent<Image>().color = ((active != null) ? active.windowBackground : new Color(0.07f, 0.055f, 0.085f, 1f));
			GameObject gameObject = new GameObject("Viewport", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(RectMask2D)
			});
			this._viewport = (RectTransform)gameObject.transform;
			this._viewport.SetParent(base.transform, false);
			this._viewport.anchorMin = Vector2.zero;
			this._viewport.anchorMax = Vector2.one;
			this._viewport.offsetMin = new Vector2(0f, 66f);
			this._viewport.offsetMax = Vector2.zero;
			Image component = gameObject.GetComponent<Image>();
			component.color = ((active != null) ? new Color(active.windowBackground.r, active.windowBackground.g, active.windowBackground.b, 1f) : new Color(0.07f, 0.055f, 0.085f, 1f));
			component.raycastTarget = false;
			this._backdrop = CielOSVideoViewerApp.MakeRawImage("MediaBackdrop", this._viewport);
			this._backdrop.color = new Color(0.42f, 0.36f, 0.46f, 0.58f);
			this._video = CielOSVideoViewerApp.MakeRawImage("Video", this._viewport);
			this._video.color = Color.white;
			this.BuildControls(active);
			this._audioSource = base.gameObject.AddComponent<AudioSource>();
			this._audioSource.playOnAwake = false;
			this._audioSource.loop = false;
			this._audioSource.spatialBlend = 0f;
			this._audioSource.volume = 1f;
			GameAudioRouter.Route(this._audioSource, GameAudioRouter.Channel.Sfx);
			this._player = base.gameObject.AddComponent<VideoPlayer>();
			this._player.playOnAwake = false;
			this._player.source = VideoSource.Url;
			this._player.renderMode = VideoRenderMode.APIOnly;
			this._player.audioOutputMode = VideoAudioOutputMode.AudioSource;
			this._player.controlledAudioTrackCount = 1;
			this._player.SetTargetAudioSource(0, this._audioSource);
			this._player.waitForFirstFrame = true;
			this._player.skipOnDrop = true;
			this._player.prepareCompleted += this.HandlePrepared;
			this._player.errorReceived += this.HandleError;
			this._player.loopPointReached += this.HandleFinished;
		}

		// Token: 0x06009EB5 RID: 40629 RVA: 0x00414C60 File Offset: 0x00412E60
		private void BuildControls(CielOSTheme theme)
		{
			Image image = CielOSVideoViewerApp.NewImage("Controls", base.transform);
			this._controls = (RectTransform)image.transform;
			this._controls.anchorMin = new Vector2(0f, 0f);
			this._controls.anchorMax = new Vector2(1f, 0f);
			this._controls.pivot = new Vector2(0.5f, 0f);
			this._controls.sizeDelta = new Vector2(0f, 66f);
			this._controls.anchoredPosition = Vector2.zero;
			image.color = ((theme != null) ? theme.controlBackground : new Color(0.16f, 0.12f, 0.19f, 1f));
			Button button = CielOSVideoViewerApp.MakeButton(this._controls, "Play", "PLAY", 66f, true, out this._playLabel);
			button.onClick.AddListener(new UnityAction(this.TogglePlay));
			CielOSSfx.Attach(button);
			Button button2 = CielOSVideoViewerApp.MakeButton(this._controls, "Mute", "AUDIO", 72f, false, out this._muteLabel);
			button2.onClick.AddListener(new UnityAction(this.ToggleMute));
			CielOSSfx.Attach(button2);
			this._timeLabel = CielOSVideoViewerApp.MakeText(this._controls, "Time", "00:00  /  00:00", 12f, TextAlignmentOptions.Left, FontStyles.Normal);
			RectTransform rectTransform = (RectTransform)this._timeLabel.transform;
			Vector2 vector = new Vector2(0f, 0f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rectTransform.pivot = new Vector2(0f, 0.5f);
			rectTransform.anchoredPosition = new Vector2(84f, 19f);
			rectTransform.sizeDelta = new Vector2(126f, 32f);
			this._timeLabel.enableWordWrapping = false;
			this._timeLabel.overflowMode = TextOverflowModes.Ellipsis;
			this._timeLabel.extraPadding = true;
			if (theme != null && theme.monospaceFont != null)
			{
				this._timeLabel.font = theme.monospaceFont;
			}
			this._progress = CielOSVideoViewerApp.MakeSlider(this._controls);
			this._progress.onValueChanged.AddListener(new UnityAction<float>(this.SeekNormalized));
			this.LayoutControls();
			this._statusLabel = CielOSVideoViewerApp.MakeText(this._viewport, "Status", "Loading video...", 16f, TextAlignmentOptions.Center, FontStyles.Bold);
			CielOSVideoViewerApp.Stretch((RectTransform)this._statusLabel.transform);
			this._statusLabel.color = new Color(1f, 1f, 1f, 0.76f);
		}

		// Token: 0x06009EB6 RID: 40630 RVA: 0x00414F24 File Offset: 0x00413124
		private void Load(string virtualPath)
		{
			string text = CielOSFileSystem.Instance.ToReal(virtualPath);
			if (!File.Exists(text))
			{
				this.ShowStatus("Video not found", Array.Empty<object>());
				return;
			}
			this._player.url = text;
			this.ShowStatus("Loading video...", Array.Empty<object>());
			this._player.Prepare();
		}

		// Token: 0x06009EB7 RID: 40631 RVA: 0x00414F80 File Offset: 0x00413180
		private void HandlePrepared(VideoPlayer source)
		{
			if (source != this._player)
			{
				return;
			}
			this._aspect = ((source.height > 0U) ? (source.width / source.height) : 1.7777778f);
			this._video.texture = source.texture;
			StreamerCensorImage.Bind(this._video, false);
			this._backdrop.texture = source.texture;
			StreamerCensorImage.Bind(this._backdrop, false);
			this.HideStatus();
			this.LayoutMedia();
			source.Play();
			CielOSContentText.Bind(this._playLabel, "PAUSE");
		}

		// Token: 0x06009EB8 RID: 40632 RVA: 0x0041501F File Offset: 0x0041321F
		private void HandleError(VideoPlayer source, string message)
		{
			if (source != this._player)
			{
				return;
			}
			this.ShowStatus("Can't play this video\n{0}", new object[] { message });
			CielOSContentText.Bind(this._playLabel, "PLAY");
		}

		// Token: 0x06009EB9 RID: 40633 RVA: 0x00415055 File Offset: 0x00413255
		private void HandleFinished(VideoPlayer source)
		{
			if (source != this._player)
			{
				return;
			}
			CielOSContentText.Bind(this._playLabel, "PLAY");
			this.SetProgress(1f);
		}

		// Token: 0x06009EBA RID: 40634 RVA: 0x00415084 File Offset: 0x00413284
		private void Update()
		{
			if (this._player == null || !this._player.isPrepared)
			{
				return;
			}
			if (this._player.texture != null && this._video.texture != this._player.texture)
			{
				this._video.texture = this._player.texture;
				this._backdrop.texture = this._player.texture;
			}
			if (this._player.length > 0.001)
			{
				this.SetProgress(Mathf.Clamp01((float)(this._player.time / this._player.length)));
				this._timeLabel.text = CielOSVideoViewerApp.FormatTime(this._player.time) + "  /  " + CielOSVideoViewerApp.FormatTime(this._player.length);
			}
		}

		// Token: 0x06009EBB RID: 40635 RVA: 0x00415178 File Offset: 0x00413378
		private void LateUpdate()
		{
			if (this._viewport == null)
			{
				return;
			}
			Rect rect = this._viewport.rect;
			if (Mathf.Abs(rect.width - this._lastWidth) > 0.5f || Mathf.Abs(rect.height - this._lastHeight) > 0.5f)
			{
				this.LayoutMedia();
			}
		}

		// Token: 0x06009EBC RID: 40636 RVA: 0x004151DA File Offset: 0x004133DA
		private void OnRectTransformDimensionsChange()
		{
			this.LayoutControls();
		}

		// Token: 0x06009EBD RID: 40637 RVA: 0x004151E4 File Offset: 0x004133E4
		private void LayoutControls()
		{
			if (this._controls == null || this._progress == null || this._timeLabel == null)
			{
				return;
			}
			float width = this._controls.rect.width;
			if (width < 1f)
			{
				return;
			}
			CielOSVideoViewerApp.ControlLayoutMetrics controlLayoutMetrics = CielOSVideoViewerApp.CalculateControlLayout(width);
			RectTransform rectTransform = (RectTransform)this._progress.transform;
			float num = width - controlLayoutMetrics.progressRight;
			rectTransform.anchoredPosition = new Vector2((controlLayoutMetrics.progressLeft - num) * 0.5f, -4f);
			rectTransform.sizeDelta = new Vector2(-(controlLayoutMetrics.progressLeft + num), 20f);
			RectTransform rectTransform2 = (RectTransform)this._timeLabel.transform;
			rectTransform2.anchoredPosition = new Vector2(controlLayoutMetrics.timeLeft, 19f);
			rectTransform2.sizeDelta = new Vector2(controlLayoutMetrics.timeRight - controlLayoutMetrics.timeLeft, 32f);
		}

		// Token: 0x06009EBE RID: 40638 RVA: 0x004152D4 File Offset: 0x004134D4
		public static CielOSVideoViewerApp.ControlLayoutMetrics CalculateControlLayout(float availableWidth)
		{
			float num = Mathf.Max(320f, availableWidth);
			float num2 = num - 8f - 72f;
			float num3 = 84f;
			float num4 = Mathf.Max(num3, Mathf.Min(num3 + 126f, num2 - 10f));
			float num5 = 10f;
			float num6 = num - 10f;
			return new CielOSVideoViewerApp.ControlLayoutMetrics
			{
				playRight = 74f,
				progressLeft = num5,
				progressRight = num6,
				timeLeft = num3,
				timeRight = num4,
				audioLeft = num2
			};
		}

		// Token: 0x06009EBF RID: 40639 RVA: 0x00415368 File Offset: 0x00413568
		private void LayoutMedia()
		{
			if (this._viewport == null || this._video == null || this._backdrop == null)
			{
				return;
			}
			Rect rect = this._viewport.rect;
			if (rect.width < 1f || rect.height < 1f)
			{
				return;
			}
			this._lastWidth = rect.width;
			this._lastHeight = rect.height;
			CielOSVideoViewerApp.SizeForAspect(this._video.rectTransform, rect.width, rect.height, this._aspect, false);
			CielOSVideoViewerApp.SizeForAspect(this._backdrop.rectTransform, rect.width, rect.height, this._aspect, true);
		}

		// Token: 0x06009EC0 RID: 40640 RVA: 0x00415430 File Offset: 0x00413630
		private void TogglePlay()
		{
			if (this._player == null || !this._player.isPrepared)
			{
				return;
			}
			if (this._player.isPlaying)
			{
				this._player.Pause();
				CielOSContentText.Bind(this._playLabel, "PLAY");
				return;
			}
			if (this._player.length > 0.0 && this._player.time >= this._player.length - 0.05)
			{
				this._player.time = 0.0;
			}
			this._player.Play();
			CielOSContentText.Bind(this._playLabel, "PAUSE");
		}

		// Token: 0x06009EC1 RID: 40641 RVA: 0x004154EC File Offset: 0x004136EC
		private void ToggleMute()
		{
			if (this._audioSource == null)
			{
				return;
			}
			this._audioSource.mute = !this._audioSource.mute;
			CielOSContentText.Bind(this._muteLabel, this._audioSource.mute ? "MUTED" : "AUDIO");
		}

		// Token: 0x06009EC2 RID: 40642 RVA: 0x00415548 File Offset: 0x00413748
		private void SeekNormalized(float value)
		{
			if (this._internalSlider || this._player == null || !this._player.isPrepared || this._player.length <= 0.0)
			{
				return;
			}
			this._player.time = (double)Mathf.Clamp01(value) * this._player.length;
		}

		// Token: 0x06009EC3 RID: 40643 RVA: 0x004155AD File Offset: 0x004137AD
		private void SetProgress(float value)
		{
			if (this._progress == null)
			{
				return;
			}
			this._internalSlider = true;
			this._progress.SetValueWithoutNotify(value);
			this._internalSlider = false;
		}

		// Token: 0x06009EC4 RID: 40644 RVA: 0x004155D8 File Offset: 0x004137D8
		private void ShowStatus(string text, params object[] arguments)
		{
			if (this._statusLabel == null)
			{
				return;
			}
			if (arguments.Length == 0)
			{
				CielOSContentText.Bind(this._statusLabel, text);
			}
			else
			{
				CielOSContentText.BindFormat(this._statusLabel, text, arguments);
			}
			this._statusLabel.gameObject.SetActive(true);
		}

		// Token: 0x06009EC5 RID: 40645 RVA: 0x00415624 File Offset: 0x00413824
		private void HideStatus()
		{
			if (this._statusLabel != null)
			{
				this._statusLabel.gameObject.SetActive(false);
			}
		}

		// Token: 0x06009EC6 RID: 40646 RVA: 0x00415648 File Offset: 0x00413848
		private void OnDisable()
		{
			if (this._player == null || !this._player.isPrepared)
			{
				return;
			}
			this._resumeAfterEnable = this._player.isPlaying;
			if (this._resumeAfterEnable)
			{
				this._player.Pause();
			}
		}

		// Token: 0x06009EC7 RID: 40647 RVA: 0x00415695 File Offset: 0x00413895
		private void OnEnable()
		{
			if (this._resumeAfterEnable && this._player != null && this._player.isPrepared)
			{
				this._player.Play();
				this._resumeAfterEnable = false;
			}
		}

		// Token: 0x06009EC8 RID: 40648 RVA: 0x004156CC File Offset: 0x004138CC
		private void OnDestroy()
		{
			if (this._player == null)
			{
				return;
			}
			this._player.prepareCompleted -= this.HandlePrepared;
			this._player.errorReceived -= this.HandleError;
			this._player.loopPointReached -= this.HandleFinished;
			this._player.Stop();
		}

		// Token: 0x06009EC9 RID: 40649 RVA: 0x00415738 File Offset: 0x00413938
		private static Button MakeButton(RectTransform parent, string name, string text, float width, bool left, out TMP_Text label)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button)
			});
			gameObject.transform.SetParent(parent, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			Vector2 vector = new Vector2(left ? 0f : 1f, 0f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform.pivot = new Vector2(left ? 0f : 1f, 0.5f);
			rectTransform.anchoredPosition = new Vector2(left ? 8f : (-8f), 19f);
			rectTransform.sizeDelta = new Vector2(width, 32f);
			Image component = gameObject.GetComponent<Image>();
			CielOSTheme active = CielOSTheme.Active;
			component.color = ((active != null) ? active.accent : new Color(0.66f, 0.38f, 0.79f, 1f));
			CielOSTheme.Round(component);
			label = CielOSVideoViewerApp.MakeText(rectTransform, "Label", text, 11f, TextAlignmentOptions.Center, FontStyles.Bold);
			CielOSVideoViewerApp.Stretch((RectTransform)label.transform);
			return gameObject.GetComponent<Button>();
		}

		// Token: 0x06009ECA RID: 40650 RVA: 0x00415880 File Offset: 0x00413A80
		private static Slider MakeSlider(RectTransform parent)
		{
			GameObject gameObject = new GameObject("Progress", new Type[]
			{
				typeof(RectTransform),
				typeof(Slider)
			});
			gameObject.transform.SetParent(parent, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.anchoredPosition = new Vector2(0f, -4f);
			rectTransform.sizeDelta = new Vector2(-20f, 20f);
			Image image = CielOSVideoViewerApp.NewImage("Background", rectTransform);
			RectTransform rectTransform2 = (RectTransform)image.transform;
			rectTransform2.anchorMin = new Vector2(0f, 0.5f);
			rectTransform2.anchorMax = new Vector2(1f, 0.5f);
			rectTransform2.pivot = new Vector2(0.5f, 0.5f);
			rectTransform2.anchoredPosition = Vector2.zero;
			rectTransform2.sizeDelta = new Vector2(0f, 4f);
			CielOSTheme active = CielOSTheme.Active;
			image.color = ((active != null) ? active.barTrack : new Color(0.2f, 0.15f, 0.26f, 1f));
			CielOSTheme.Round(image);
			GameObject gameObject2 = new GameObject("FillArea", new Type[] { typeof(RectTransform) });
			gameObject2.transform.SetParent(rectTransform, false);
			RectTransform rectTransform3 = (RectTransform)gameObject2.transform;
			rectTransform3.anchorMin = new Vector2(0f, 0.5f);
			rectTransform3.anchorMax = new Vector2(1f, 0.5f);
			rectTransform3.pivot = new Vector2(0.5f, 0.5f);
			rectTransform3.anchoredPosition = Vector2.zero;
			rectTransform3.sizeDelta = new Vector2(0f, 4f);
			Image image2 = CielOSVideoViewerApp.NewImage("Fill", gameObject2.transform);
			CielOSVideoViewerApp.Stretch((RectTransform)image2.transform);
			image2.color = ((active != null) ? active.barFill : new Color(0.66f, 0.38f, 0.79f, 1f));
			CielOSTheme.Round(image2);
			Slider component = gameObject.GetComponent<Slider>();
			component.direction = Slider.Direction.LeftToRight;
			component.minValue = 0f;
			component.maxValue = 1f;
			component.value = 0f;
			component.fillRect = (RectTransform)image2.transform;
			component.handleRect = null;
			component.targetGraphic = image;
			return component;
		}

		// Token: 0x06009ECB RID: 40651 RVA: 0x00415B2C File Offset: 0x00413D2C
		private static RawImage MakeRawImage(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(RawImage)
			});
			gameObject.transform.SetParent(parent, false);
			RawImage component = gameObject.GetComponent<RawImage>();
			component.raycastTarget = false;
			return component;
		}

		// Token: 0x06009ECC RID: 40652 RVA: 0x00411EE2 File Offset: 0x004100E2
		private static Image NewImage(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject.transform.SetParent(parent, false);
			return gameObject.GetComponent<Image>();
		}

		// Token: 0x06009ECD RID: 40653 RVA: 0x00415B78 File Offset: 0x00413D78
		private static TMP_Text MakeText(Transform parent, string name, string content, float size, TextAlignmentOptions alignment, FontStyles style)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject.transform.SetParent(parent, false);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			component.text = content;
			component.fontSize = size;
			component.alignment = alignment;
			component.fontStyle = style;
			component.color = Color.white;
			component.raycastTarget = false;
			CielOSTheme active = CielOSTheme.Active;
			if (active != null && active.uiFont != null)
			{
				component.font = active.uiFont;
			}
			if (name == "Time")
			{
				LocalizedGameText.BindFontOnly(component);
			}
			else
			{
				CielOSContentText.Bind(component, content);
			}
			return component;
		}

		// Token: 0x06009ECE RID: 40654 RVA: 0x00415C38 File Offset: 0x00413E38
		private static void SizeForAspect(RectTransform rect, float areaWidth, float areaHeight, float mediaAspect, bool cover)
		{
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rect.pivot = vector;
			rect.anchorMin = (rect.anchorMax = vector);
			rect.anchoredPosition = Vector2.zero;
			rect.sizeDelta = CielOSImageViewerApp.CalculateAspectSize(areaWidth, areaHeight, mediaAspect, cover);
		}

		// Token: 0x06009ECF RID: 40655 RVA: 0x00415C88 File Offset: 0x00413E88
		private static string FormatTime(double seconds)
		{
			int num = Mathf.Max(0, Mathf.FloorToInt((float)seconds));
			int num2 = num / 3600;
			int num3 = num / 60 % 60;
			int num4 = num % 60;
			if (num2 <= 0)
			{
				return string.Format("{0:00}:{1:00}", num3, num4);
			}
			return string.Format("{0:00}:{1:00}:{2:00}", num2, num3, num4);
		}

		// Token: 0x06009ED0 RID: 40656 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void Stretch(RectTransform rect)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
		}

		// Token: 0x0400A2DD RID: 41693
		public const float ControlStripHeight = 66f;

		// Token: 0x0400A2DE RID: 41694
		public const float ProgressTrackHeight = 4f;

		// Token: 0x0400A2DF RID: 41695
		public const float ProgressHitAreaHeight = 20f;

		// Token: 0x0400A2E0 RID: 41696
		public const bool ProgressUsesVisibleHandle = false;

		// Token: 0x0400A2E1 RID: 41697
		private RectTransform _viewport;

		// Token: 0x0400A2E2 RID: 41698
		private RectTransform _controls;

		// Token: 0x0400A2E3 RID: 41699
		private RawImage _backdrop;

		// Token: 0x0400A2E4 RID: 41700
		private RawImage _video;

		// Token: 0x0400A2E5 RID: 41701
		private TMP_Text _playLabel;

		// Token: 0x0400A2E6 RID: 41702
		private TMP_Text _muteLabel;

		// Token: 0x0400A2E7 RID: 41703
		private TMP_Text _timeLabel;

		// Token: 0x0400A2E8 RID: 41704
		private TMP_Text _statusLabel;

		// Token: 0x0400A2E9 RID: 41705
		private Slider _progress;

		// Token: 0x0400A2EA RID: 41706
		private VideoPlayer _player;

		// Token: 0x0400A2EB RID: 41707
		private AudioSource _audioSource;

		// Token: 0x0400A2EC RID: 41708
		private bool _internalSlider;

		// Token: 0x0400A2ED RID: 41709
		private bool _resumeAfterEnable;

		// Token: 0x0400A2EE RID: 41710
		private float _aspect = 1.7777778f;

		// Token: 0x0400A2EF RID: 41711
		private float _lastWidth = -1f;

		// Token: 0x0400A2F0 RID: 41712
		private float _lastHeight = -1f;

		// Token: 0x02001419 RID: 5145
		public struct ControlLayoutMetrics
		{
			// Token: 0x0400A2F1 RID: 41713
			public float playRight;

			// Token: 0x0400A2F2 RID: 41714
			public float progressLeft;

			// Token: 0x0400A2F3 RID: 41715
			public float progressRight;

			// Token: 0x0400A2F4 RID: 41716
			public float timeLeft;

			// Token: 0x0400A2F5 RID: 41717
			public float timeRight;

			// Token: 0x0400A2F6 RID: 41718
			public float audioLeft;
		}
	}
}
