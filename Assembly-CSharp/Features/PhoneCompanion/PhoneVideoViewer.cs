using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Features.CensorBars;
using Features.CielVoice;
using Features.Localization;
using ProjectExhibitionist.Core;
using ProjektSumperk;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E58 RID: 3672
	[DefaultExecutionOrder(-500)]
	public sealed class PhoneVideoViewer : MonoBehaviour
	{
		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x06006DA0 RID: 28064 RVA: 0x002C077E File Offset: 0x002BE97E
		public static bool IsOpen
		{
			get
			{
				return PhoneVideoViewer._instance != null && PhoneVideoViewer._instance._open;
			}
		}

		// Token: 0x06006DA1 RID: 28065 RVA: 0x002C0799 File Offset: 0x002BE999
		public static bool CloseIfOpen()
		{
			if (!PhoneVideoViewer.IsOpen)
			{
				return false;
			}
			PhoneVideoViewer._instance.Close();
			return true;
		}

		// Token: 0x06006DA2 RID: 28066 RVA: 0x002C07AF File Offset: 0x002BE9AF
		public static void Show(string videoPath)
		{
			if (string.IsNullOrEmpty(videoPath) || !File.Exists(videoPath))
			{
				return;
			}
			if (PhoneVideoViewer._instance == null)
			{
				PhoneVideoViewer._instance = new GameObject("PhoneVideoViewer").AddComponent<PhoneVideoViewer>();
			}
			PhoneVideoViewer._instance.Open(videoPath);
		}

		// Token: 0x06006DA3 RID: 28067 RVA: 0x002C07F0 File Offset: 0x002BE9F0
		private void Awake()
		{
			if (PhoneVideoViewer._instance != null && PhoneVideoViewer._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			PhoneVideoViewer._instance = this;
			this.BuildUi();
			this.BuildPlayer();
			this._canvas.gameObject.SetActive(false);
		}

		// Token: 0x06006DA4 RID: 28068 RVA: 0x002C0848 File Offset: 0x002BEA48
		private void BuildPlayer()
		{
			this._player = base.gameObject.AddComponent<VideoPlayer>();
			this._player.playOnAwake = false;
			this._player.source = VideoSource.Url;
			this._player.renderMode = VideoRenderMode.APIOnly;
			this._player.audioOutputMode = VideoAudioOutputMode.Direct;
			this._player.waitForFirstFrame = true;
			this._player.skipOnDrop = true;
			this._player.prepareCompleted += this.OnPrepared;
			this._player.errorReceived += this.OnError;
			this._player.loopPointReached += this.OnFinished;
		}

		// Token: 0x06006DA5 RID: 28069 RVA: 0x002C08F4 File Offset: 0x002BEAF4
		private void Open(string videoPath)
		{
			if (!this._open)
			{
				this.SuppressCielAudio();
			}
			this._videoPath = videoPath;
			this._open = true;
			this._deleteArmed = false;
			this._deleteArmedUntil = 0f;
			this._canvas.gameObject.SetActive(true);
			LocalizedLegacyGameText.BindComputed(this._titleLabel, () => PhoneVideoViewer.BuildDisplayTitle(videoPath));
			LocalizedLegacyGameText.BindSource(this._statusLabel, "phone_story", "Loading video...");
			this._statusLabel.gameObject.SetActive(true);
			if (this._centerPlayButton != null)
			{
				this._centerPlayButton.SetActive(false);
			}
			this._videoImage.texture = null;
			this._playLabel.text = "▶";
			LocalizedLegacyGameText.BindSource(this._muteLabel, "phone_story", "AUDIO");
			LocalizedLegacyGameText.BindSource(this._deleteLabel, "phone_story", "DELETE");
			LocalizedLegacyGameText.BindSource(this._saveLabel, "phone_story", "SAVE");
			this._progress.SetValueWithoutNotify(0f);
			this._timeLabel.text = "00:00  /  00:00";
			this._player.Stop();
			this._player.url = videoPath;
			this._player.Prepare();
		}

		// Token: 0x06006DA6 RID: 28070 RVA: 0x002C0A50 File Offset: 0x002BEC50
		private void OnPrepared(VideoPlayer source)
		{
			if (!this._open || source != this._player)
			{
				return;
			}
			this._statusLabel.gameObject.SetActive(false);
			this._videoImage.texture = this._player.texture;
			float num = ((source.height > 0U) ? (source.width / source.height) : 1.7777778f);
			if (this._videoAspect != null)
			{
				this._videoAspect.aspectRatio = Mathf.Max(0.1f, num);
			}
			LocalizedLegacyGameText.BindSource(this._muteLabel, "phone_story", (this._player.audioTrackCount > 0) ? "AUDIO" : "NO AUDIO");
			this._muteLabel.color = ((this._player.audioTrackCount > 0) ? PhoneVideoViewer.White : PhoneVideoViewer.SoftWhite);
			this._player.Play();
			this._playLabel.text = "II";
			if (this._centerPlayButton != null)
			{
				this._centerPlayButton.SetActive(false);
			}
		}

		// Token: 0x06006DA7 RID: 28071 RVA: 0x002C0B68 File Offset: 0x002BED68
		private void OnError(VideoPlayer source, string message)
		{
			if (!this._open || source != this._player)
			{
				return;
			}
			LocalizedLegacyGameText.BindComputed(this._statusLabel, () => GameLocalization.FormatSource("phone_story", "Unable to play video\n{0}", new object[] { message }));
			this._statusLabel.gameObject.SetActive(true);
			this._playLabel.text = "▶";
			if (this._centerPlayButton != null)
			{
				this._centerPlayButton.SetActive(false);
			}
		}

		// Token: 0x06006DA8 RID: 28072 RVA: 0x002C0BEC File Offset: 0x002BEDEC
		private void OnFinished(VideoPlayer source)
		{
			if (source != this._player)
			{
				return;
			}
			this._playLabel.text = "▶";
			if (this._centerPlayButton != null)
			{
				this._centerPlayButton.SetActive(true);
			}
			this._sliderInternal = true;
			this._progress.SetValueWithoutNotify(1f);
			this._sliderInternal = false;
		}

		// Token: 0x06006DA9 RID: 28073 RVA: 0x002C0C50 File Offset: 0x002BEE50
		private void Update()
		{
			if (!this._open)
			{
				return;
			}
			this.MaintainCielAudioSilence();
			if (this._player.texture != null && this._videoImage.texture != this._player.texture)
			{
				this._videoImage.texture = this._player.texture;
			}
			if (this._player.isPrepared && this._player.length > 0.001)
			{
				double num = Math.Max(0.0, this._player.time);
				double num2 = Math.Max(0.0, this._player.length);
				this._sliderInternal = true;
				this._progress.SetValueWithoutNotify(Mathf.Clamp01((float)(num / num2)));
				this._sliderInternal = false;
				this._timeLabel.text = PhoneVideoViewer.FormatTime(num) + "  /  " + PhoneVideoViewer.FormatTime(num2);
				this._playLabel.text = (this._player.isPlaying ? "II" : "▶");
				if (this._centerPlayButton != null && !this._statusLabel.gameObject.activeSelf && this._centerPlayButton.activeSelf == this._player.isPlaying)
				{
					this._centerPlayButton.SetActive(!this._player.isPlaying);
				}
			}
			if (this._deleteArmed && Time.unscaledTime > this._deleteArmedUntil)
			{
				this._deleteArmed = false;
				LocalizedLegacyGameText.BindSource(this._deleteLabel, "phone_story", "DELETE");
			}
			if (EscapeInputCoordinator.TryConsume(this))
			{
				this.Close();
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.Space))
			{
				this.TogglePlay();
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.M))
			{
				this.ToggleMute();
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.LeftArrow))
			{
				this.SeekBy(-5.0);
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.RightArrow))
			{
				this.SeekBy(5.0);
			}
		}

		// Token: 0x06006DAA RID: 28074 RVA: 0x002C0E5F File Offset: 0x002BF05F
		public void Close()
		{
			if (!this._open)
			{
				return;
			}
			this._open = false;
			this._player.Stop();
			this._videoImage.texture = null;
			this._canvas.gameObject.SetActive(false);
			this.RestoreCielAudio();
		}

		// Token: 0x06006DAB RID: 28075 RVA: 0x002C0EA0 File Offset: 0x002BF0A0
		private void SuppressCielAudio()
		{
			this.RestoreCielAudio();
			CielVoiceController cielVoiceController = CielVoiceController.Instance;
			if (cielVoiceController == null)
			{
				cielVoiceController = Object.FindObjectOfType<CielVoiceController>(true);
			}
			if (cielVoiceController == null)
			{
				return;
			}
			this._cielAudioRoot = cielVoiceController.transform.root;
			this.CaptureAndMuteNewCielSources();
			this._nextCielAudioScan = Time.unscaledTime + 0.5f;
		}

		// Token: 0x06006DAC RID: 28076 RVA: 0x002C0EFC File Offset: 0x002BF0FC
		private void MaintainCielAudioSilence()
		{
			for (int i = 0; i < this._cielAudioStates.Count; i++)
			{
				AudioSource source = this._cielAudioStates[i].source;
				if (source != null && !source.mute)
				{
					source.mute = true;
				}
			}
			if (this._cielAudioRoot != null && Time.unscaledTime >= this._nextCielAudioScan)
			{
				this.CaptureAndMuteNewCielSources();
				this._nextCielAudioScan = Time.unscaledTime + 0.5f;
			}
		}

		// Token: 0x06006DAD RID: 28077 RVA: 0x002C0F7C File Offset: 0x002BF17C
		private void CaptureAndMuteNewCielSources()
		{
			if (this._cielAudioRoot == null)
			{
				return;
			}
			foreach (AudioSource audioSource in this._cielAudioRoot.GetComponentsInChildren<AudioSource>(true))
			{
				if (!(audioSource == null) && !this.HasCapturedCielSource(audioSource))
				{
					this._cielAudioStates.Add(new PhoneVideoViewer.CielAudioState
					{
						source = audioSource,
						wasMuted = audioSource.mute
					});
					audioSource.mute = true;
				}
			}
		}

		// Token: 0x06006DAE RID: 28078 RVA: 0x002C0FFC File Offset: 0x002BF1FC
		private bool HasCapturedCielSource(AudioSource source)
		{
			for (int i = 0; i < this._cielAudioStates.Count; i++)
			{
				if (this._cielAudioStates[i].source == source)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006DAF RID: 28079 RVA: 0x002C103C File Offset: 0x002BF23C
		private void RestoreCielAudio()
		{
			for (int i = 0; i < this._cielAudioStates.Count; i++)
			{
				PhoneVideoViewer.CielAudioState cielAudioState = this._cielAudioStates[i];
				if (cielAudioState.source != null)
				{
					cielAudioState.source.mute = cielAudioState.wasMuted;
				}
			}
			this._cielAudioStates.Clear();
			this._cielAudioRoot = null;
			this._nextCielAudioScan = 0f;
		}

		// Token: 0x06006DB0 RID: 28080 RVA: 0x002C10A8 File Offset: 0x002BF2A8
		private void TogglePlay()
		{
			if (!this._player.isPrepared)
			{
				return;
			}
			if (this._player.isPlaying)
			{
				this._player.Pause();
				this._playLabel.text = "▶";
				if (this._centerPlayButton != null)
				{
					this._centerPlayButton.SetActive(true);
					return;
				}
			}
			else
			{
				if (this._player.length > 0.0 && this._player.time >= this._player.length - 0.05)
				{
					this._player.time = 0.0;
				}
				this._player.Play();
				this._playLabel.text = "II";
				if (this._centerPlayButton != null)
				{
					this._centerPlayButton.SetActive(false);
				}
			}
		}

		// Token: 0x06006DB1 RID: 28081 RVA: 0x002C118C File Offset: 0x002BF38C
		private void ToggleMute()
		{
			if (!this._player.isPrepared || this._player.audioTrackCount == 0)
			{
				return;
			}
			bool directAudioMute = this._player.GetDirectAudioMute(0);
			this._player.SetDirectAudioMute(0, !directAudioMute);
			LocalizedLegacyGameText.BindSource(this._muteLabel, "phone_story", directAudioMute ? "AUDIO" : "MUTED");
			this._muteLabel.color = (directAudioMute ? PhoneVideoViewer.White : PhoneVideoViewer.Red);
		}

		// Token: 0x06006DB2 RID: 28082 RVA: 0x002C120C File Offset: 0x002BF40C
		private void SeekBy(double delta)
		{
			if (!this._player.isPrepared || !this._player.canSetTime)
			{
				return;
			}
			this._player.time = Math.Max(0.0, Math.Min(this._player.length, this._player.time + delta));
		}

		// Token: 0x06006DB3 RID: 28083 RVA: 0x002C126C File Offset: 0x002BF46C
		private void OnProgressChanged(float value)
		{
			if (this._sliderInternal || !this._player.isPrepared || !this._player.canSetTime)
			{
				return;
			}
			this._player.time = Math.Max(0.0, this._player.length * (double)value);
		}

		// Token: 0x06006DB4 RID: 28084 RVA: 0x002C12C4 File Offset: 0x002BF4C4
		private void SaveToPc()
		{
			if (string.IsNullOrEmpty(this._videoPath) || !File.Exists(this._videoPath))
			{
				return;
			}
			try
			{
				string text = PlayerMediaExport.CopyVideo(this._videoPath);
				Debug.Log("[PhoneVideoViewer] Video saved: " + text);
				base.StartCoroutine(this.TemporaryButtonFeedback(this._saveLabel, "SAVED", "SAVE"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[PhoneVideoViewer] Save to PC failed: " + ex.Message);
				base.StartCoroutine(this.TemporaryButtonFeedback(this._saveLabel, "ERROR", "SAVE"));
			}
		}

		// Token: 0x06006DB5 RID: 28085 RVA: 0x002C1370 File Offset: 0x002BF570
		private void DeleteVideo()
		{
			if (!this._deleteArmed)
			{
				this._deleteArmed = true;
				this._deleteArmedUntil = Time.unscaledTime + 3f;
				LocalizedLegacyGameText.BindSource(this._deleteLabel, "phone_story", "CONFIRM");
				return;
			}
			string videoPath = this._videoPath;
			this.Close();
			PhoneVideoStorage.DeleteVideo(videoPath);
			PhotoGallery photoGallery = Object.FindObjectOfType<PhotoGallery>(true);
			if (photoGallery != null)
			{
				photoGallery.RefreshVideos();
			}
		}

		// Token: 0x06006DB6 RID: 28086 RVA: 0x002C13DB File Offset: 0x002BF5DB
		private IEnumerator TemporaryButtonFeedback(Text label, string temporary, string normal)
		{
			LocalizedLegacyGameText.BindSource(label, "phone_story", temporary);
			yield return new WaitForSecondsRealtime(1.4f);
			if (label != null)
			{
				LocalizedLegacyGameText.BindSource(label, "phone_story", normal);
			}
			yield break;
		}

		// Token: 0x06006DB7 RID: 28087 RVA: 0x002C13F8 File Offset: 0x002BF5F8
		private void BuildUi()
		{
			GameObject gameObject = new GameObject("PhoneVideoViewerCanvas", new Type[]
			{
				typeof(Canvas),
				typeof(CanvasScaler),
				typeof(GraphicRaycaster)
			});
			gameObject.transform.SetParent(base.transform, false);
			this._canvas = gameObject.GetComponent<Canvas>();
			this._canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this._canvas.sortingOrder = 1110;
			GameplayHudVisibility.RegisterCanvas(this._canvas);
			CanvasScaler component = gameObject.GetComponent<CanvasScaler>();
			component.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component.referenceResolution = new Vector2(1920f, 1080f);
			component.matchWidthOrHeight = 0.5f;
			Image image = PhoneVideoViewer.NewImage(gameObject.transform, "Backdrop", Color.black);
			PhoneVideoViewer.Stretch(image.rectTransform);
			Button button = image.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.transition = Selectable.Transition.None;
			button.onClick.AddListener(new UnityAction(this.Close));
			Image image2 = PhoneVideoViewer.NewImage(gameObject.transform, "VideoFrame", new Color(0f, 0f, 0f, 1f));
			PhoneVideoViewer.Stretch(image2.rectTransform);
			GameObject gameObject2 = new GameObject("Video", new Type[]
			{
				typeof(RectTransform),
				typeof(RawImage),
				typeof(AspectRatioFitter)
			});
			RectTransform rectTransform = (RectTransform)gameObject2.transform;
			rectTransform.SetParent(image2.transform, false);
			PhoneVideoViewer.SetRect(rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(1920f, 1080f), new Vector2(0.5f, 0.5f));
			this._videoImage = gameObject2.GetComponent<RawImage>();
			StreamerCensorImage.Bind(this._videoImage, false);
			this._videoImage.color = Color.white;
			this._videoImage.raycastTarget = true;
			this._videoAspect = gameObject2.GetComponent<AspectRatioFitter>();
			this._videoAspect.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
			this._videoAspect.aspectRatio = 1.7777778f;
			Button button2 = gameObject2.AddComponent<Button>();
			button2.targetGraphic = this._videoImage;
			button2.transition = Selectable.Transition.None;
			button2.onClick.AddListener(new UnityAction(this.TogglePlay));
			this._statusLabel = PhoneVideoViewer.NewText(image2.transform, "Status", "Loading video...", 27, PhoneVideoViewer.SoftWhite, TextAnchor.MiddleCenter);
			PhoneVideoViewer.Stretch(this._statusLabel.rectTransform);
			Image image3 = PhoneVideoViewer.NewImage(image2.transform, "CenterPlay", new Color(0f, 0f, 0f, 0.58f));
			PhoneVideoViewer.SetRect(image3.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(88f, 88f), new Vector2(0.5f, 0.5f));
			image3.sprite = PhoneUiSpriteFactory.Circle;
			image3.preserveAspect = true;
			Button button3 = image3.gameObject.AddComponent<Button>();
			button3.targetGraphic = image3;
			button3.onClick.AddListener(new UnityAction(this.TogglePlay));
			Text text = PhoneVideoViewer.NewText(image3.transform, "Glyph", "▶", 38, PhoneVideoViewer.White, TextAnchor.MiddleCenter);
			PhoneVideoViewer.Stretch(text.rectTransform);
			text.rectTransform.anchoredPosition = new Vector2(3f, 0f);
			this._centerPlayButton = image3.gameObject;
			this._centerPlayButton.SetActive(false);
			Image image4 = PhoneVideoViewer.NewImage(gameObject.transform, "Header", PhoneVideoViewer.Panel);
			PhoneVideoViewer.SetRect(image4.rectTransform, new Vector2(0f, 1f), new Vector2(1f, 1f), Vector2.zero, new Vector2(0f, 88f), new Vector2(0.5f, 1f));
			PhoneVideoViewer.MakeButton(image4.transform, "Close", "×", new Vector2(48f, 48f), new Color(1f, 1f, 1f, 0.1f), new UnityAction(this.Close), new Vector2(0f, 1f), new Vector2(48f, -44f), 27);
			this._titleLabel = PhoneVideoViewer.NewText(image4.transform, "Title", "", 26, PhoneVideoViewer.White, TextAnchor.MiddleCenter);
			PhoneVideoViewer.Stretch(this._titleLabel.rectTransform);
			this._titleLabel.rectTransform.offsetMin = new Vector2(126f, 12f);
			this._titleLabel.rectTransform.offsetMax = new Vector2(-126f, -12f);
			this._titleLabel.resizeTextForBestFit = true;
			this._titleLabel.resizeTextMinSize = 16;
			this._titleLabel.resizeTextMaxSize = 26;
			this._titleLabel.horizontalOverflow = HorizontalWrapMode.Wrap;
			this._titleLabel.verticalOverflow = VerticalWrapMode.Truncate;
			Image image5 = PhoneVideoViewer.NewImage(gameObject.transform, "Controls", PhoneVideoViewer.Panel);
			PhoneVideoViewer.SetRect(image5.rectTransform, new Vector2(0f, 0f), new Vector2(1f, 0f), Vector2.zero, new Vector2(0f, 132f), new Vector2(0.5f, 0f));
			this._playLabel = PhoneVideoViewer.MakeButton(image5.transform, "PlayPause", "▶", new Vector2(48f, 48f), new Color(1f, 1f, 1f, 0.12f), new UnityAction(this.TogglePlay), new Vector2(0f, 0f), new Vector2(52f, 40f), 25);
			this._progress = this.BuildSlider(image5.transform);
			this._progress.onValueChanged.AddListener(new UnityAction<float>(this.OnProgressChanged));
			this._timeLabel = PhoneVideoViewer.NewText(image5.transform, "Time", "00:00  /  00:00", 18, PhoneVideoViewer.SoftWhite, TextAnchor.MiddleLeft);
			PhoneVideoViewer.SetRect(this._timeLabel.rectTransform, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(94f, 40f), new Vector2(250f, 42f), new Vector2(0f, 0.5f));
			this._deleteLabel = PhoneVideoViewer.MakeButton(image5.transform, "Delete", "DELETE", new Vector2(108f, 44f), new Color(0.38f, 0.055f, 0.075f, 0.92f), new UnityAction(this.DeleteVideo), new Vector2(1f, 0f), new Vector2(-66f, 40f), 16);
			this._deleteLabel.color = new Color(1f, 0.55f, 0.58f, 1f);
			this._saveLabel = PhoneVideoViewer.MakeButton(image5.transform, "Save", "SAVE", new Vector2(104f, 44f), new Color(1f, 1f, 1f, 0.12f), new UnityAction(this.SaveToPc), new Vector2(1f, 0f), new Vector2(-180f, 40f), 16);
			this._muteLabel = PhoneVideoViewer.MakeButton(image5.transform, "Mute", "AUDIO", new Vector2(92f, 44f), new Color(1f, 1f, 1f, 0.08f), new UnityAction(this.ToggleMute), new Vector2(1f, 0f), new Vector2(-442f, 40f), 15);
			PhoneVideoViewer.MakeButton(image5.transform, "OpenExportFolder", "OPEN FOLDER", new Vector2(144f, 44f), new Color(1f, 1f, 1f, 0.08f), new UnityAction(PlayerMediaExport.OpenVideosFolder), new Vector2(1f, 0f), new Vector2(-316f, 40f), 15);
			Text text2 = PhoneVideoViewer.NewText(image5.transform, "ExportLocation", PlayerMediaExport.LocationLabel + " / Videos", 14, PhoneVideoViewer.SoftWhite, TextAnchor.MiddleCenter);
			LocalizedLegacyGameText.BindComputed(text2, () => GameLocalization.FormatSource("phone_story", "{0} / Videos", new object[] { PlayerMediaExport.LocationLabel })).FitTranslationToRectangle();
			PhoneVideoViewer.SetRect(text2.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(-60f, 40f), new Vector2(720f, 42f), new Vector2(0.5f, 0.5f));
		}

		// Token: 0x06006DB8 RID: 28088 RVA: 0x002C1D20 File Offset: 0x002BFF20
		private Slider BuildSlider(Transform parent)
		{
			GameObject gameObject = new GameObject("Progress", new Type[]
			{
				typeof(RectTransform),
				typeof(Slider)
			});
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(parent, false);
			PhoneVideoViewer.SetRect(rectTransform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -10f), new Vector2(-64f, 28f), new Vector2(0.5f, 1f));
			Image image = PhoneVideoViewer.NewImage(gameObject.transform, "Background", new Color(1f, 1f, 1f, 0.2f));
			PhoneVideoViewer.SetRect(image.rectTransform, new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), Vector2.zero, new Vector2(0f, 4f), new Vector2(0.5f, 0.5f));
			RectTransform rectTransform2 = PhoneVideoViewer.NewRect(gameObject.transform, "Fill Area");
			PhoneVideoViewer.SetRect(rectTransform2, new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), Vector2.zero, new Vector2(0f, 4f), new Vector2(0.5f, 0.5f));
			Image image2 = PhoneVideoViewer.NewImage(rectTransform2, "Fill", PhoneVideoViewer.Red);
			PhoneVideoViewer.Stretch(image2.rectTransform);
			Slider component = gameObject.GetComponent<Slider>();
			component.fillRect = image2.rectTransform;
			component.handleRect = null;
			component.targetGraphic = image;
			component.minValue = 0f;
			component.maxValue = 1f;
			component.value = 0f;
			component.direction = Slider.Direction.LeftToRight;
			return component;
		}

		// Token: 0x06006DB9 RID: 28089 RVA: 0x002C1EEC File Offset: 0x002C00EC
		private static Text MakeButton(Transform parent, string name, string label, Vector2 size, Color color, UnityAction action, Vector2 anchor, Vector2 position, int fontSize)
		{
			Image image = PhoneVideoViewer.NewImage(parent, name, color);
			PhoneVideoViewer.SetRect(image.rectTransform, anchor, anchor, position, size, new Vector2(0.5f, 0.5f));
			image.sprite = PhoneUiSpriteFactory.Rounded;
			image.type = Image.Type.Sliced;
			Button button = image.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.onClick.AddListener(action);
			Text text = PhoneVideoViewer.NewText(image.transform, "Label", label, fontSize, PhoneVideoViewer.White, TextAnchor.MiddleCenter);
			PhoneVideoViewer.Stretch(text.rectTransform);
			LocalizedLegacyGameText.BindSource(text, "phone_story", label).FitTranslationToRectangle();
			return text;
		}

		// Token: 0x06006DBA RID: 28090 RVA: 0x002C1F8C File Offset: 0x002C018C
		private static Image NewImage(Transform parent, string name, Color color)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			gameObject.transform.SetParent(parent, false);
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			return component;
		}

		// Token: 0x06006DBB RID: 28091 RVA: 0x002C1FE8 File Offset: 0x002C01E8
		private static Text NewText(Transform parent, string name, string value, int size, Color color, TextAnchor alignment)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Text)
			});
			gameObject.transform.SetParent(parent, false);
			Text component = gameObject.GetComponent<Text>();
			component.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component.text = value;
			component.fontSize = size;
			component.color = color;
			component.alignment = alignment;
			component.raycastTarget = false;
			LocalizedLegacyGameText.BindFontOnly(component);
			return component;
		}

		// Token: 0x06006DBC RID: 28092 RVA: 0x002B9704 File Offset: 0x002B7904
		private static RectTransform NewRect(Transform parent, string name)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(parent, false);
			return (RectTransform)gameObject.transform;
		}

		// Token: 0x06006DBD RID: 28093 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void Stretch(RectTransform rt)
		{
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
		}

		// Token: 0x06006DBE RID: 28094 RVA: 0x002C2076 File Offset: 0x002C0276
		private static void SetRect(RectTransform rt, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 size, Vector2 pivot)
		{
			rt.anchorMin = anchorMin;
			rt.anchorMax = anchorMax;
			rt.pivot = pivot;
			rt.anchoredPosition = anchoredPosition;
			rt.sizeDelta = size;
		}

		// Token: 0x06006DBF RID: 28095 RVA: 0x002C20A0 File Offset: 0x002C02A0
		private static string FormatTime(double seconds)
		{
			int num = Math.Max(0, (int)Math.Floor(seconds));
			int num2 = num / 3600;
			int num3 = num / 60 % 60;
			int num4 = num % 60;
			if (num2 <= 0)
			{
				return string.Format("{0:00}:{1:00}", num3, num4);
			}
			return string.Format("{0:00}:{1:00}:{2:00}", num2, num3, num4);
		}

		// Token: 0x06006DC0 RID: 28096 RVA: 0x002C2108 File Offset: 0x002C0308
		private static string BuildDisplayTitle(string videoPath)
		{
			PhoneVideoMetadata phoneVideoMetadata = PhoneVideoStorage.LoadMetadata(videoPath);
			DateTime dateTime;
			if (phoneVideoMetadata != null && !string.IsNullOrEmpty(phoneVideoMetadata.createdUtc) && DateTime.TryParse(phoneVideoMetadata.createdUtc, null, DateTimeStyles.RoundtripKind, out dateTime))
			{
				dateTime = dateTime.ToLocalTime();
			}
			else
			{
				dateTime = File.GetLastWriteTime(videoPath);
			}
			return GameLocalization.FormatSource("phone_story", "Video • {0:dd/MM/yyyy  HH:mm}", new object[] { dateTime });
		}

		// Token: 0x06006DC1 RID: 28097 RVA: 0x002C2170 File Offset: 0x002C0370
		private void OnDestroy()
		{
			this.RestoreCielAudio();
			if (this._player != null)
			{
				this._player.prepareCompleted -= this.OnPrepared;
				this._player.errorReceived -= this.OnError;
				this._player.loopPointReached -= this.OnFinished;
			}
			if (PhoneVideoViewer._instance == this)
			{
				PhoneVideoViewer._instance = null;
			}
		}

		// Token: 0x0400703F RID: 28735
		private static PhoneVideoViewer _instance;

		// Token: 0x04007040 RID: 28736
		private Canvas _canvas;

		// Token: 0x04007041 RID: 28737
		private VideoPlayer _player;

		// Token: 0x04007042 RID: 28738
		private RawImage _videoImage;

		// Token: 0x04007043 RID: 28739
		private Text _playLabel;

		// Token: 0x04007044 RID: 28740
		private Text _timeLabel;

		// Token: 0x04007045 RID: 28741
		private Text _titleLabel;

		// Token: 0x04007046 RID: 28742
		private Text _statusLabel;

		// Token: 0x04007047 RID: 28743
		private Text _muteLabel;

		// Token: 0x04007048 RID: 28744
		private Text _deleteLabel;

		// Token: 0x04007049 RID: 28745
		private Text _saveLabel;

		// Token: 0x0400704A RID: 28746
		private GameObject _centerPlayButton;

		// Token: 0x0400704B RID: 28747
		private AspectRatioFitter _videoAspect;

		// Token: 0x0400704C RID: 28748
		private Slider _progress;

		// Token: 0x0400704D RID: 28749
		private bool _open;

		// Token: 0x0400704E RID: 28750
		private bool _sliderInternal;

		// Token: 0x0400704F RID: 28751
		private bool _deleteArmed;

		// Token: 0x04007050 RID: 28752
		private float _deleteArmedUntil;

		// Token: 0x04007051 RID: 28753
		private string _videoPath;

		// Token: 0x04007052 RID: 28754
		private readonly List<PhoneVideoViewer.CielAudioState> _cielAudioStates = new List<PhoneVideoViewer.CielAudioState>();

		// Token: 0x04007053 RID: 28755
		private Transform _cielAudioRoot;

		// Token: 0x04007054 RID: 28756
		private float _nextCielAudioScan;

		// Token: 0x04007055 RID: 28757
		private static readonly Color Panel = new Color(0.025f, 0.025f, 0.032f, 0.84f);

		// Token: 0x04007056 RID: 28758
		private static readonly Color White = new Color(1f, 1f, 1f, 0.96f);

		// Token: 0x04007057 RID: 28759
		private static readonly Color SoftWhite = new Color(1f, 1f, 1f, 0.72f);

		// Token: 0x04007058 RID: 28760
		private static readonly Color Red = new Color(1f, 0.16f, 0.18f, 1f);

		// Token: 0x04007059 RID: 28761
		private const string PlayGlyph = "▶";

		// Token: 0x0400705A RID: 28762
		private const string PauseGlyph = "II";

		// Token: 0x02000E59 RID: 3673
		private struct CielAudioState
		{
			// Token: 0x0400705B RID: 28763
			public AudioSource source;

			// Token: 0x0400705C RID: 28764
			public bool wasMuted;
		}
	}
}
