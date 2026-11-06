using System;
using System.Collections.Generic;
using System.IO;
using Features.CielOS;
using Features.Localization;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Headphones;
using ProjectExhibitionist.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjektSumperk
{
	// Token: 0x020003FB RID: 1019
	public class MusicPlayerController : MonoBehaviour
	{
		// Token: 0x060019C8 RID: 6600 RVA: 0x000A3804 File Offset: 0x000A1A04
		private void Start()
		{
			if (this.musicAudioSource != null)
			{
				this.musicAudioSource.Stop();
				this.musicAudioSource.enabled = false;
			}
			this._service = CielOSAudioService.Ensure(null);
			if (HeadphonesEquipment.IsWorn && PlayerRegistry.Player != null)
			{
				HeadphonesMusicController.GetOrAdd(PlayerRegistry.Player.gameObject);
			}
			if (this._service != null)
			{
				this._service.TrackChanged += this.HandleTrackChanged;
				this._service.StateChanged += this.SyncPlayerUi;
				this._service.Rescan();
			}
			this.SyncPlayerUi();
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x000A38B4 File Offset: 0x000A1AB4
		private void Update()
		{
			if (this._service == null)
			{
				return;
			}
			float durationSeconds = this._service.DurationSeconds;
			float playbackSeconds = this._service.PlaybackSeconds;
			if (this.timerSlider != null)
			{
				this.timerSlider.minValue = 0f;
				this.timerSlider.maxValue = Mathf.Max(1f, durationSeconds);
				this.timerSlider.SetValueWithoutNotify(Mathf.Clamp(playbackSeconds, 0f, Mathf.Max(1f, durationSeconds)));
			}
			if (this.timerText != null)
			{
				this.timerText.text = MusicPlayerController.FormatTime(Mathf.Max(0f, durationSeconds - playbackSeconds));
			}
			if (this.timerText2 != null)
			{
				this.timerText2.text = MusicPlayerController.FormatTime(playbackSeconds);
			}
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x000A3988 File Offset: 0x000A1B88
		public void GetAudioFile(GameObject clip)
		{
			if (!this.RequireHeadphones() || this._service == null || clip == null)
			{
				return;
			}
			string name = clip.name;
			for (int i = 0; i < this._service.Tracks.Count; i++)
			{
				if (string.Equals(Path.GetFileNameWithoutExtension(this._service.Tracks[i]), name, StringComparison.OrdinalIgnoreCase))
				{
					this._service.PlayIndex(i);
					if (this.musicPanel != null)
					{
						this.musicPanel.SetActive(true);
					}
					if (this.listPanel != null)
					{
						this.listPanel.SetActive(false);
					}
					return;
				}
			}
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x000A3A37 File Offset: 0x000A1C37
		public void PreviousSong()
		{
			if (!this.RequirePlayableTrack())
			{
				return;
			}
			this._service.SkipPrevious();
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x000A3A4D File Offset: 0x000A1C4D
		public void NextSong()
		{
			if (!this.RequirePlayableTrack())
			{
				return;
			}
			this._service.Next();
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x000A3A63 File Offset: 0x000A1C63
		public void PauseAudio()
		{
			if (this._service != null && this._service.IsPlaying)
			{
				this._service.TogglePlayPause();
			}
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x000A3A8B File Offset: 0x000A1C8B
		public void UnPauseAudio()
		{
			if (!this.RequirePlayableTrack())
			{
				return;
			}
			if (!this._service.IsPlaying)
			{
				this._service.TogglePlayPause();
			}
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x000A3AAE File Offset: 0x000A1CAE
		public void ResetPlayer()
		{
			this.SyncPlayerUi();
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x00004F31 File Offset: 0x00003131
		public void StartTimer()
		{
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x000A3AAE File Offset: 0x000A1CAE
		public void ExitPlayer()
		{
			this.SyncPlayerUi();
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x000A3AB6 File Offset: 0x000A1CB6
		private bool RequireHeadphones()
		{
			if (HeadphonesEquipment.IsWorn)
			{
				return true;
			}
			ToastService.Show(GameLocalization.GetSource("phone_story", "Put on your headphones to listen to music."), GameLocalization.GetSource("phone_story", "Headphones required"), null, -1f, null);
			return false;
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x000A3AEC File Offset: 0x000A1CEC
		private bool RequirePlayableTrack()
		{
			if (!this.RequireHeadphones() || this._service == null)
			{
				return false;
			}
			if (this._service.Tracks.Count > 0)
			{
				return true;
			}
			ToastService.Show(GameLocalization.GetSource("phone_story", "Import a track from the CielOS Music app first."), GameLocalization.GetSource("phone_story", "No music found"), null, -1f, null);
			return false;
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x000A3B51 File Offset: 0x000A1D51
		private void HandleTrackChanged()
		{
			this.RebuildTrackList();
			this.SyncPlayerUi();
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x000A3B60 File Offset: 0x000A1D60
		private void RebuildTrackList()
		{
			if (this.parent == null || this.musicPrefab == null || this._service == null)
			{
				return;
			}
			for (int i = 0; i < this.songClips.Count; i++)
			{
				if (this.songClips[i] != null)
				{
					Object.Destroy(this.songClips[i]);
				}
			}
			this.songClips.Clear();
			for (int j = 0; j < this._service.Tracks.Count; j++)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this._service.Tracks[j]);
				GameObject gameObject = Object.Instantiate<GameObject>(this.musicPrefab, this.parent, false);
				gameObject.name = fileNameWithoutExtension;
				TMP_Text componentInChildren = gameObject.GetComponentInChildren<TMP_Text>(true);
				if (componentInChildren != null)
				{
					componentInChildren.text = fileNameWithoutExtension;
					LocalizedGameText.BindFontOnly(componentInChildren);
				}
				AudioSource component = gameObject.GetComponent<AudioSource>();
				if (component != null)
				{
					component.enabled = false;
				}
				this.songClips.Add(gameObject);
			}
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x000A3C78 File Offset: 0x000A1E78
		private void SyncPlayerUi()
		{
			if (this._service == null)
			{
				return;
			}
			string text = this._service.CurrentName;
			text = (string.IsNullOrEmpty(text) ? "No track" : Path.GetFileNameWithoutExtension(text));
			bool flag = string.IsNullOrEmpty(this._service.CurrentName);
			MusicPlayerController.BindTrackName(this.songNameText, text, flag);
			MusicPlayerController.BindTrackName(this.songNameDI, text, flag);
			this.isMusicPlaying = this._service.IsPlaying || this._service.IsPaused || this._service.IsLoading;
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x000A3D0F File Offset: 0x000A1F0F
		private static void BindTrackName(TMP_Text text, string name, bool empty)
		{
			if (text == null)
			{
				return;
			}
			if (empty)
			{
				LocalizedGameText.BindSource(text, "phone_story", "No track", null);
				return;
			}
			LocalizedGameText.BindFontOnly(text);
			text.text = name;
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x000A3D40 File Offset: 0x000A1F40
		private static string FormatTime(float seconds)
		{
			int num = Mathf.Max(0, Mathf.FloorToInt(seconds));
			return string.Format("{0:00}:{1:00}", num / 60, num % 60);
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x000A3D76 File Offset: 0x000A1F76
		private void OnDestroy()
		{
			if (this._service == null)
			{
				return;
			}
			this._service.TrackChanged -= this.HandleTrackChanged;
			this._service.StateChanged -= this.SyncPlayerUi;
		}

		// Token: 0x04001AFE RID: 6910
		public GameObject musicPrefab;

		// Token: 0x04001AFF RID: 6911
		public Transform parent;

		// Token: 0x04001B00 RID: 6912
		public AudioSource musicAudioSource;

		// Token: 0x04001B01 RID: 6913
		public TMP_Text songNameText;

		// Token: 0x04001B02 RID: 6914
		public TMP_Text songNameDI;

		// Token: 0x04001B03 RID: 6915
		public TMP_Text timerText;

		// Token: 0x04001B04 RID: 6916
		public TMP_Text timerText2;

		// Token: 0x04001B05 RID: 6917
		public Slider timerSlider;

		// Token: 0x04001B06 RID: 6918
		public GameObject musicPanel;

		// Token: 0x04001B07 RID: 6919
		public GameObject listPanel;

		// Token: 0x04001B08 RID: 6920
		public List<GameObject> songClips = new List<GameObject>();

		// Token: 0x04001B09 RID: 6921
		public bool isMusicPlaying;

		// Token: 0x04001B0A RID: 6922
		public GameObject TimeText;

		// Token: 0x04001B0B RID: 6923
		public GameObject SigBatt;

		// Token: 0x04001B0C RID: 6924
		private CielOSAudioService _service;
	}
}
