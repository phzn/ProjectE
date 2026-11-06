using System;
using TMPro;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003D0 RID: 976
	public class CallManager : MonoBehaviour
	{
		// Token: 0x060018FA RID: 6394 RVA: 0x00004F31 File Offset: 0x00003131
		private void Start()
		{
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x0009EC14 File Offset: 0x0009CE14
		private void IncreaseSeconds()
		{
			this.timerS += 1f;
			if (this.timerS >= 60f)
			{
				this.timerS = 0f;
			}
			this.timerTextS.text = ":" + this.timerS.ToString("00");
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x0009EC70 File Offset: 0x0009CE70
		private void IncreaseMinutes()
		{
			this.timerM += 1f;
			this.timerTextM.text = this.timerM.ToString("00");
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x0009EC9F File Offset: 0x0009CE9F
		public void ReceiveDICall()
		{
			this.notchRev.Play("DIGetCallAnimationRev", -1, 0f);
			this.numberText.text = this.callName.text;
			base.Invoke("ReceiveCall", 1f);
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x0009ECDD File Offset: 0x0009CEDD
		public void RejectDICall()
		{
			this.notchRev.Play("DIGetCallAnimationRev", -1, 0f);
			base.Invoke("HideDINotch", 1.5f);
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x0009ED05 File Offset: 0x0009CF05
		private void HideDINotch()
		{
			this.notchRev.gameObject.SetActive(false);
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x0009ED18 File Offset: 0x0009CF18
		public void ReceiveCall()
		{
			this.dialPanel.SetActive(true);
			this.callingPanel.SetActive(true);
			this.callName.text = this.numberText.text;
			this.StartTimer();
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x0009ED4E File Offset: 0x0009CF4E
		public void Clear()
		{
			this.numberText.text = "";
			this.clear.SetActive(false);
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x0009ED6C File Offset: 0x0009CF6C
		public void StartCall()
		{
			if (this.numberText.text.Length < 3)
			{
				return;
			}
			this.dialPanel.SetActive(true);
			this.callingPanel.SetActive(true);
			this.callName.text = this.numberText.text;
			base.Invoke("StartTimer", 10f);
			this.beepSource.clip = this.ringTone;
			this.beepSource.Play();
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x0009EDE8 File Offset: 0x0009CFE8
		public void StartCallFromContacts(string name)
		{
			this.callingPanel.SetActive(true);
			this.callName.text = name;
			base.Invoke("StartTimer", 10f);
			this.beepSource.clip = this.ringTone;
			this.beepSource.Play();
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x0009EE3C File Offset: 0x0009D03C
		public void StartTimer()
		{
			this.timerTextM.gameObject.SetActive(true);
			base.InvokeRepeating("IncreaseSeconds", 1f, 1f);
			base.InvokeRepeating("IncreaseMinutes", 60f, 60f);
			this.beepSource.clip = this.conversation;
			this.beepSource.Play();
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x0009EEA0 File Offset: 0x0009D0A0
		public void EndCall()
		{
			this.Clear();
			this.callingPanel.SetActive(false);
			this.dialPanel.SetActive(true);
			this.timerS = 0f;
			this.timerM = 0f;
			this.timerTextS.text = ":00";
			this.timerTextM.text = "00";
			this.timerTextM.gameObject.SetActive(false);
			this.beepSource.Stop();
			base.CancelInvoke();
			if (this.isCallFromContacts)
			{
				this.isCallFromContacts = false;
			}
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x0009EF34 File Offset: 0x0009D134
		public void GetSetNumberText(string num)
		{
			if (this.numberText.text.Length >= 12)
			{
				return;
			}
			TMP_Text tmp_Text = this.numberText;
			tmp_Text.text += num;
			this.clear.SetActive(this.numberText.text.Length > 0);
			this.Beep(num);
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x0009EF94 File Offset: 0x0009D194
		public void Beep(string i)
		{
			if (i != "*" && i != "#")
			{
				this.beepSource.clip = this.beep[int.Parse(i)];
				this.beepSource.Play();
				return;
			}
			this.beepSource.clip = this.beep[1];
			this.beepSource.Play();
		}

		// Token: 0x040019F9 RID: 6649
		public TMP_Text numberText;

		// Token: 0x040019FA RID: 6650
		public GameObject callingPanel;

		// Token: 0x040019FB RID: 6651
		public GameObject dialPanel;

		// Token: 0x040019FC RID: 6652
		public TMP_Text callName;

		// Token: 0x040019FD RID: 6653
		public GameObject clear;

		// Token: 0x040019FE RID: 6654
		public AudioClip[] beep;

		// Token: 0x040019FF RID: 6655
		public AudioSource beepSource;

		// Token: 0x04001A00 RID: 6656
		public AudioClip ringTone;

		// Token: 0x04001A01 RID: 6657
		public AudioClip conversation;

		// Token: 0x04001A02 RID: 6658
		public TMP_Text timerTextM;

		// Token: 0x04001A03 RID: 6659
		public TMP_Text timerTextS;

		// Token: 0x04001A04 RID: 6660
		private float timerM;

		// Token: 0x04001A05 RID: 6661
		private float timerS;

		// Token: 0x04001A06 RID: 6662
		public bool isCallFromContacts;

		// Token: 0x04001A07 RID: 6663
		public Animator notchRev;
	}
}
