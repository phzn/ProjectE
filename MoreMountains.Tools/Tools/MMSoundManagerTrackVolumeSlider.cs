using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x020000D9 RID: 217
	public class MMSoundManagerTrackVolumeSlider : MonoBehaviour, MMEventListener<MMSoundManagerEvent>, MMEventListenerBase, MMEventListener<MMSoundManagerTrackEvent>, MMEventListener<MMSoundManagerTrackFadeEvent>
	{
		// Token: 0x060004AF RID: 1199 RVA: 0x00016298 File Offset: 0x00014498
		protected virtual void Awake()
		{
			this._slider = base.gameObject.GetComponent<Slider>();
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x000162AB File Offset: 0x000144AB
		protected virtual void Start()
		{
			if (MMPersistentSingleton<MMSoundManager>.HasInstance)
			{
				this.UpdateSliderValueWithTrackVolume();
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000162BC File Offset: 0x000144BC
		protected virtual void LateUpdate()
		{
			if (this.Mode == MMSoundManagerTrackVolumeSlider.Modes.Read)
			{
				float trackVolume = MMPersistentSingleton<MMSoundManager>.Instance.GetTrackVolume(this.Track, false);
				this._slider.value = trackVolume;
			}
			if (this._resetNeeded && Time.unscaledTime >= this._resetTimestamp)
			{
				this.Mode = this._resetToMode;
				this._resetNeeded = false;
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00016317 File Offset: 0x00014517
		public virtual void ChangeModeToRead(float duration)
		{
			this._resetToMode = MMSoundManagerTrackVolumeSlider.Modes.Write;
			this.Mode = MMSoundManagerTrackVolumeSlider.Modes.Read;
			this._resetTimestamp = Time.unscaledTime + duration;
			this._resetNeeded = true;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001633C File Offset: 0x0001453C
		public virtual void UpdateVolume(float newValue)
		{
			if (this.Mode == MMSoundManagerTrackVolumeSlider.Modes.Read)
			{
				return;
			}
			float num = MMMaths.Remap(newValue, 0f, 1f, this.MinVolume, this.MaxVolume);
			MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, this.Track, num);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001637C File Offset: 0x0001457C
		public void OnMMEvent(MMSoundManagerEvent soundManagerEvent)
		{
			if (soundManagerEvent.EventType == MMSoundManagerEventTypes.SettingsLoaded)
			{
				this.UpdateSliderValueWithTrackVolume();
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001638D File Offset: 0x0001458D
		public virtual void UpdateSliderValueWithTrackVolume()
		{
			this._slider.value = MMMaths.Remap(MMPersistentSingleton<MMSoundManager>.Instance.GetTrackVolume(this.Track, false), 0f, 1f, this.MinVolume, this.MaxVolume);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x000163C8 File Offset: 0x000145C8
		public void OnMMEvent(MMSoundManagerTrackEvent trackEvent)
		{
			switch (trackEvent.TrackEventType)
			{
			case MMSoundManagerTrackEventTypes.MuteTrack:
				if (this.ChangeModeOnMute)
				{
					this.ChangeModeToRead(this.ModeSwitchBufferTime);
					return;
				}
				break;
			case MMSoundManagerTrackEventTypes.UnmuteTrack:
				if (this.ChangeModeOnUnmute)
				{
					this.ChangeModeToRead(this.ModeSwitchBufferTime);
					return;
				}
				break;
			case MMSoundManagerTrackEventTypes.SetVolumeTrack:
				if (this.ChangeModeOnTrackVolumeChange)
				{
					this.ChangeModeToRead(this.ModeSwitchBufferTime);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001642D File Offset: 0x0001462D
		public void OnMMEvent(MMSoundManagerTrackFadeEvent fadeEvent)
		{
			if (this.ChangeModeOnTrackFade)
			{
				this.ChangeModeToRead(fadeEvent.FadeDuration + this.ModeSwitchBufferTime);
			}
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001644A File Offset: 0x0001464A
		protected virtual void OnEnable()
		{
			this.MMEventStartListening<MMSoundManagerEvent>();
			this.MMEventStartListening<MMSoundManagerTrackEvent>();
			this.MMEventStartListening<MMSoundManagerTrackFadeEvent>();
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001645E File Offset: 0x0001465E
		protected virtual void OnDisable()
		{
			this.MMEventStopListening<MMSoundManagerEvent>();
			this.MMEventStopListening<MMSoundManagerTrackEvent>();
			this.MMEventStopListening<MMSoundManagerTrackFadeEvent>();
		}

		// Token: 0x04000621 RID: 1569
		[Header("Track Volume Settings")]
		[Tooltip("The track to change volume on")]
		public MMSoundManager.MMSoundManagerTracks Track;

		// Token: 0x04000622 RID: 1570
		[Tooltip("The volume to apply to the track when the slider is at its minimum")]
		public float MinVolume;

		// Token: 0x04000623 RID: 1571
		[Tooltip("The volume to apply to the track when the slider is at its maximum")]
		public float MaxVolume = 1f;

		// Token: 0x04000624 RID: 1572
		[Header("Read/Write Mode")]
		[Tooltip("in read mode, the value of the slider will be applied to the volume of the track. in read mode, the slider will move to reflect the volume of the track")]
		public MMSoundManagerTrackVolumeSlider.Modes Mode = MMSoundManagerTrackVolumeSlider.Modes.Write;

		// Token: 0x04000625 RID: 1573
		[Tooltip("if this is true, the slider will automatically switch to read mode for the required duration when a track fade event is caught")]
		public bool ChangeModeOnTrackFade = true;

		// Token: 0x04000626 RID: 1574
		[Tooltip("if this is true, the slider will automatically switch to read mode for the required duration when a track mute event is caught")]
		public bool ChangeModeOnMute = true;

		// Token: 0x04000627 RID: 1575
		[Tooltip("if this is true, the slider will automatically switch to read mode for the required duration when a track unmute event is caught")]
		public bool ChangeModeOnUnmute = true;

		// Token: 0x04000628 RID: 1576
		[Tooltip("if this is true, the slider will automatically switch to read mode for the required duration when a track volume change event is caught")]
		public bool ChangeModeOnTrackVolumeChange;

		// Token: 0x04000629 RID: 1577
		[Tooltip("when switching automatically (and temporarily) to Read Mode, the minimum duration the slider will remain in that mode")]
		public float ModeSwitchBufferTime = 0.1f;

		// Token: 0x0400062A RID: 1578
		protected Slider _slider;

		// Token: 0x0400062B RID: 1579
		protected MMSoundManagerTrackVolumeSlider.Modes _resetToMode;

		// Token: 0x0400062C RID: 1580
		protected bool _resetNeeded;

		// Token: 0x0400062D RID: 1581
		protected float _resetTimestamp;

		// Token: 0x0200041A RID: 1050
		public enum Modes
		{
			// Token: 0x04001F18 RID: 7960
			Read,
			// Token: 0x04001F19 RID: 7961
			Write
		}
	}
}
