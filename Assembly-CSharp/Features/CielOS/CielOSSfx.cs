using System;
using Features.MainMenu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS
{
	// Token: 0x02001392 RID: 5010
	public static class CielOSSfx
	{
		// Token: 0x06009B10 RID: 39696 RVA: 0x003F6AC1 File Offset: 0x003F4CC1
		public static void Configure(AudioClip clickClip, float volume = 1f)
		{
			CielOSSfx._clickClip = clickClip;
			CielOSSfx._volume = Mathf.Clamp01(volume);
			CielOSSfx.EnsureSource();
		}

		// Token: 0x06009B11 RID: 39697 RVA: 0x003F6ADC File Offset: 0x003F4CDC
		private static void EnsureSource()
		{
			if (CielOSSfx._source != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("CielOSSfx");
			Object.DontDestroyOnLoad(gameObject);
			CielOSSfx._source = gameObject.AddComponent<AudioSource>();
			CielOSSfx._source.playOnAwake = false;
			CielOSSfx._source.spatialBlend = 0f;
		}

		// Token: 0x06009B12 RID: 39698 RVA: 0x003F6B2C File Offset: 0x003F4D2C
		public static void PlayClick()
		{
			if (CielOSSfx._clickClip == null)
			{
				return;
			}
			CielOSSfx.EnsureSource();
			if (CielOSSfx._source == null)
			{
				return;
			}
			CielOSSfx._source.pitch = 1f;
			CielOSSfx._source.PlayOneShot(CielOSSfx._clickClip, CielOSSfx._volume * GameSettingsService.SfxVolume);
		}

		// Token: 0x06009B13 RID: 39699 RVA: 0x003F6B84 File Offset: 0x003F4D84
		public static void PlayHover()
		{
			if (CielOSSfx._clickClip == null)
			{
				return;
			}
			CielOSSfx.EnsureSource();
			if (CielOSSfx._source == null)
			{
				return;
			}
			CielOSSfx._source.pitch = 1.14f;
			CielOSSfx._source.PlayOneShot(CielOSSfx._clickClip, CielOSSfx._volume * 0.22f * GameSettingsService.SfxVolume);
		}

		// Token: 0x06009B14 RID: 39700 RVA: 0x003F6BE4 File Offset: 0x003F4DE4
		public static void PlayPickup()
		{
			CielOSSfx.EnsureSource();
			if (CielOSSfx._source == null)
			{
				return;
			}
			if (CielOSSfx._pickupClip == null)
			{
				float[] array = new float[3308];
				for (int i = 0; i < 3308; i++)
				{
					float num = (float)i / 22050f;
					float num2 = Mathf.Sin(3.1415927f * (float)i / 3308f);
					float num3 = 6.2831855f * (510f * num + 900f * num * num);
					array[i] = Mathf.Sin(num3) * num2 * 0.32f;
				}
				CielOSSfx._pickupClip = AudioClip.Create("CielOS Snake Pickup", 3308, 1, 22050, false);
				CielOSSfx._pickupClip.SetData(array, 0);
			}
			CielOSSfx._source.pitch = 1f;
			CielOSSfx._source.PlayOneShot(CielOSSfx._pickupClip, CielOSSfx._volume * GameSettingsService.SfxVolume);
		}

		// Token: 0x06009B15 RID: 39701 RVA: 0x003F6CC8 File Offset: 0x003F4EC8
		public static void Attach(Button button)
		{
			if (button == null)
			{
				return;
			}
			button.onClick.RemoveListener(new UnityAction(CielOSSfx.PlayClick));
			button.onClick.AddListener(new UnityAction(CielOSSfx.PlayClick));
		}

		// Token: 0x04009FFB RID: 40955
		private static AudioClip _clickClip;

		// Token: 0x04009FFC RID: 40956
		private static AudioClip _pickupClip;

		// Token: 0x04009FFD RID: 40957
		private static AudioSource _source;

		// Token: 0x04009FFE RID: 40958
		private static float _volume = 1f;
	}
}
