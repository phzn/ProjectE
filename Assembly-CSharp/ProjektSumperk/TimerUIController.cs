using System;
using System.Collections;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjektSumperk
{
	// Token: 0x02000406 RID: 1030
	public class TimerUIController : MonoBehaviour
	{
		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001A21 RID: 6689 RVA: 0x000A5BA3 File Offset: 0x000A3DA3
		public float CurrentTime
		{
			get
			{
				return this._currentTime;
			}
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x000A5BAB File Offset: 0x000A3DAB
		private void Start()
		{
			PhoneTimerRuntime.Ensure();
			this.startTime = PhoneTimerRuntime.Duration;
			this._currentTime = PhoneTimerRuntime.Remaining;
			this.isTimerRunning = PhoneTimerRuntime.IsRunning;
			this.InstallEditableDigits();
			this.UpdateTimerDisplay();
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x000A5BDF File Offset: 0x000A3DDF
		private void OnEnable()
		{
			PhoneTimerRuntime.Ensure();
			this.startTime = PhoneTimerRuntime.Duration;
			this._currentTime = PhoneTimerRuntime.Remaining;
			this.isTimerRunning = PhoneTimerRuntime.IsRunning;
			if (PhoneTimerRuntime.DidFinish)
			{
				this.PresentFinishedState();
			}
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x000A5C14 File Offset: 0x000A3E14
		private void Update()
		{
			bool flag = this.isTimerRunning;
			this.isTimerRunning = PhoneTimerRuntime.IsRunning;
			this._currentTime = PhoneTimerRuntime.Remaining;
			if (this._editableDigits != null)
			{
				this._editableDigits.SetEditable(!this.isTimerRunning);
			}
			if (this._editableDigits != null && this._editableDigits.IsEditing && !this.isTimerRunning)
			{
				return;
			}
			if (!this.isTimerRunning && !flag)
			{
				if (PhoneTimerRuntime.DidFinish && !this._finishPresented)
				{
					this.PresentFinishedState();
				}
				return;
			}
			this.UpdateTimerDisplay();
			float num = ((this.startTime > 0f) ? (this._currentTime / this.startTime) : 0f);
			if (this.timerCircle != null)
			{
				this.timerCircle.fillAmount = num;
			}
			if (this.DITimerCircle != null)
			{
				this.DITimerCircle.fillAmount = num;
			}
			if (this.isTimerRunning || !PhoneTimerRuntime.DidFinish || this._finishPresented)
			{
				return;
			}
			this.PresentFinishedState();
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x000A5D20 File Offset: 0x000A3F20
		public void AdjustSeconds(float seconds)
		{
			if (this.isTimerRunning)
			{
				return;
			}
			this.startTime = Mathf.Clamp(this.startTime + seconds, 1f, 359999f);
			PhoneTimerRuntime.SetDuration(this.startTime);
			this._currentTime = this.startTime;
			this._finishPresented = false;
			this.UpdateTimerDisplay();
			if (this.timerCircle != null)
			{
				this.timerCircle.fillAmount = 1f;
			}
			if (this.DITimerCircle != null)
			{
				this.DITimerCircle.fillAmount = 1f;
			}
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x000A5DB4 File Offset: 0x000A3FB4
		public void SetDurationSeconds(float seconds)
		{
			if (this.isTimerRunning)
			{
				return;
			}
			this.startTime = Mathf.Clamp(seconds, 1f, 359999f);
			PhoneTimerRuntime.SetDuration(this.startTime);
			this._currentTime = this.startTime;
			this._finishPresented = false;
			this.UpdateTimerDisplay();
			if (this.timerCircle != null)
			{
				this.timerCircle.fillAmount = 1f;
			}
			if (this.DITimerCircle != null)
			{
				this.DITimerCircle.fillAmount = 1f;
			}
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void ResetTimer()
		{
			if (this.DITimer != null)
			{
				this.DITimer.gameObject.SetActive(false);
			}
			this._currentTime = this.startTime;
			PhoneTimerRuntime.Reset(this.startTime);
			this._finishPresented = false;
			this.UpdateTimerDisplay();
			if (this.timerCircle != null)
			{
				this.timerCircle.fillAmount = 1f;
			}
			if (this.DITimerCircle != null)
			{
				this.DITimerCircle.fillAmount = 1f;
			}
			this.isTimerRunning = false;
			if (this.TimeText != null)
			{
				this.TimeText.SetActive(true);
			}
			if (this.SigBatt != null)
			{
				this.SigBatt.SetActive(true);
			}
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x000A5F08 File Offset: 0x000A4108
		public void StartTimer()
		{
			if (this._editableDigits != null)
			{
				this._editableDigits.Commit();
			}
			if (this._currentTime <= 0f)
			{
				this._currentTime = this.startTime;
			}
			PhoneTimerRuntime.Start(this._currentTime);
			this.isTimerRunning = true;
			if (this._editableDigits != null)
			{
				this._editableDigits.SetEditable(false);
			}
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x000A5F73 File Offset: 0x000A4173
		public void OnTimerFinish()
		{
			this.PresentFinishedState();
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x000A5F7C File Offset: 0x000A417C
		private void PresentFinishedState()
		{
			if (this._finishPresented)
			{
				return;
			}
			this._finishPresented = true;
			this.isTimerRunning = false;
			this._currentTime = 0f;
			this.UpdateTimerDisplay();
			if (this.timerCircle != null)
			{
				this.timerCircle.fillAmount = 0f;
			}
			if (this.DITimerCircle != null)
			{
				this.DITimerCircle.fillAmount = 0f;
			}
			if (this.DITimerResponseText != null)
			{
				LocalizedGameText.BindSource(this.DITimerResponseText, "phone_story", "Time's up!", null).FitTranslationToRectangle(false);
			}
			if (base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.CloseDynamicIslandAfterDelay());
			}
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x000A602C File Offset: 0x000A422C
		private IEnumerator CloseDynamicIslandAfterDelay()
		{
			yield return new WaitForSecondsRealtime(2f);
			if (this.TimeText != null)
			{
				this.TimeText.SetActive(true);
			}
			if (this.SigBatt != null)
			{
				this.SigBatt.SetActive(true);
			}
			if (this.DITimer != null)
			{
				this.DITimer.Play("TimerDIAnimationRev", -1, 0f);
			}
			yield break;
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x000A603C File Offset: 0x000A423C
		private void UpdateTimerDisplay()
		{
			int num = Mathf.CeilToInt(this._currentTime);
			int num2 = num / 3600;
			int num3 = num % 3600 / 60;
			int num4 = num % 60;
			string text = string.Format("{0:00}:{1:00}:{2:00}", num2, num3, num4);
			if (this.timerText != null)
			{
				this.timerText.text = text;
			}
			if (this.DITimerText != null)
			{
				this.DITimerText.text = text;
			}
			if (this._editableDigits != null)
			{
				this._editableDigits.SetTextWithoutNotify(text);
			}
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x000A60D8 File Offset: 0x000A42D8
		internal bool TrySetDurationFromText(string value)
		{
			float num;
			if (this.isTimerRunning || !TimerUIController.TryParseDuration(value, out num))
			{
				return false;
			}
			this.SetDurationSeconds(num);
			return true;
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x000A6104 File Offset: 0x000A4304
		internal static bool TryParseDuration(string value, out float duration)
		{
			duration = 0f;
			if (string.IsNullOrWhiteSpace(value))
			{
				return false;
			}
			string[] array = value.Trim().Split(':', StringSplitOptions.None);
			if (array.Length < 1 || array.Length > 3)
			{
				return false;
			}
			int num = 0;
			int num2 = 0;
			int num3;
			if (array.Length == 3)
			{
				if (!int.TryParse(array[0], out num) || !int.TryParse(array[1], out num2) || !int.TryParse(array[2], out num3))
				{
					return false;
				}
			}
			else if (array.Length == 2)
			{
				if (!int.TryParse(array[0], out num2) || !int.TryParse(array[1], out num3))
				{
					return false;
				}
			}
			else if (!int.TryParse(array[0], out num3))
			{
				return false;
			}
			if (num < 0 || num2 < 0 || num3 < 0 || (array.Length == 3 && num2 > 59) || (array.Length > 1 && num3 > 59))
			{
				return false;
			}
			duration = (float)num * 3600f + (float)num2 * 60f + (float)num3;
			return duration >= 1f && duration <= 359999f;
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x000A61EC File Offset: 0x000A43EC
		private void InstallEditableDigits()
		{
			if (this.timerText == null)
			{
				return;
			}
			this._editableDigits = this.timerText.GetComponent<TimerEditableDigits>();
			if (this._editableDigits == null)
			{
				this._editableDigits = this.timerText.gameObject.AddComponent<TimerEditableDigits>();
			}
			this._editableDigits.Configure(this, this.timerText);
			this._editableDigits.SetEditable(!this.isTimerRunning);
		}

		// Token: 0x04001B46 RID: 6982
		public Image timerCircle;

		// Token: 0x04001B47 RID: 6983
		public Image DITimerCircle;

		// Token: 0x04001B48 RID: 6984
		public TMP_Text timerText;

		// Token: 0x04001B49 RID: 6985
		public TMP_Text DITimerText;

		// Token: 0x04001B4A RID: 6986
		public TMP_Text DITimerResponseText;

		// Token: 0x04001B4B RID: 6987
		[Min(1f)]
		public float startTime = 300f;

		// Token: 0x04001B4C RID: 6988
		public bool isTimerRunning;

		// Token: 0x04001B4D RID: 6989
		public Animator DITimer;

		// Token: 0x04001B4E RID: 6990
		public GameObject TimeText;

		// Token: 0x04001B4F RID: 6991
		public GameObject SigBatt;

		// Token: 0x04001B50 RID: 6992
		private float _currentTime;

		// Token: 0x04001B51 RID: 6993
		private bool _finishPresented;

		// Token: 0x04001B52 RID: 6994
		private TimerEditableDigits _editableDigits;
	}
}
