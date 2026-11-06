using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjektSumperk
{
	// Token: 0x02000404 RID: 1028
	public class RadioController : MonoBehaviour
	{
		// Token: 0x06001A16 RID: 6678 RVA: 0x000A591D File Offset: 0x000A3B1D
		public void StartRadio()
		{
			if (this.isRadioPlaying)
			{
				return;
			}
			this.RadioSource.clip = this.oldRadio;
			this.RadioSource.Play();
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x000A5944 File Offset: 0x000A3B44
		private void Update()
		{
			this.fmRangeText.text = this.fmSlider.value.ToString("0.0");
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x000A5974 File Offset: 0x000A3B74
		public void OnPress()
		{
			this.RadioSource.clip = this.oldRadio;
			this.RadioSource.Play();
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x000A5992 File Offset: 0x000A3B92
		public void OnRelease()
		{
			this.RadioSource.clip = this.oldRadio;
			this.RadioSource.Pause();
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x000A59B0 File Offset: 0x000A3BB0
		public void Channels(int index)
		{
			this.RadioSource.Stop();
			this.RadioSource.clip = this.RadioChannelAudio[index];
			this.RadioSource.Play();
			this.isRadioPlaying = true;
			if (index == 0)
			{
				this.radioTitle.text = (this.notchTitle.text = "StarRock FM");
				this.fmSlider.value = 92.7f;
				return;
			}
			if (index == 1)
			{
				this.radioTitle.text = (this.notchTitle.text = "RetroWave Radio");
				this.fmSlider.value = 93.5f;
				return;
			}
			if (index == 2)
			{
				this.radioTitle.text = (this.notchTitle.text = "FutureGroove 101");
				this.fmSlider.value = 94.3f;
				return;
			}
			if (index == 3)
			{
				this.radioTitle.text = (this.notchTitle.text = "WildWest FM");
				this.fmSlider.value = 95.01f;
				return;
			}
			if (index == 4)
			{
				this.radioTitle.text = (this.notchTitle.text = "Cosmic Radio");
				this.fmSlider.value = 106.8f;
			}
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x000A5AEA File Offset: 0x000A3CEA
		public void play()
		{
			this.RadioSource.volume = 1f;
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x000A5AFC File Offset: 0x000A3CFC
		public void pause()
		{
			this.RadioSource.volume = 0f;
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x000A5B0E File Offset: 0x000A3D0E
		public void ExitRadio()
		{
			this.RadioSource.Stop();
			this.isRadioPlaying = false;
			this.TimeText.SetActive(true);
			this.SigBatt.SetActive(true);
		}

		// Token: 0x04001B3A RID: 6970
		public Text fmRangeText;

		// Token: 0x04001B3B RID: 6971
		public Slider fmSlider;

		// Token: 0x04001B3C RID: 6972
		public AudioClip oldRadio;

		// Token: 0x04001B3D RID: 6973
		public AudioClip[] RadioChannelAudio;

		// Token: 0x04001B3E RID: 6974
		public AudioSource RadioSource;

		// Token: 0x04001B3F RID: 6975
		public bool isRadioPlaying;

		// Token: 0x04001B40 RID: 6976
		public TMP_Text radioTitle;

		// Token: 0x04001B41 RID: 6977
		public TMP_Text notchTitle;

		// Token: 0x04001B42 RID: 6978
		public GameObject TimeText;

		// Token: 0x04001B43 RID: 6979
		public GameObject SigBatt;
	}
}
