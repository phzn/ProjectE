using System;
using System.Globalization;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.AzureSky;

namespace ProjektSumperk
{
	// Token: 0x020003D7 RID: 983
	public class DateTimeManager : MonoBehaviour
	{
		// Token: 0x0600191F RID: 6431 RVA: 0x0009F93C File Offset: 0x0009DB3C
		private void OnEnable()
		{
			this._nextAzureLookup = 0f;
			this._displayedMinute = (this._displayedDate = long.MinValue);
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x0009F96C File Offset: 0x0009DB6C
		private void Update()
		{
			if (this._azure == null && Time.unscaledTime >= this._nextAzureLookup)
			{
				this._azure = Object.FindObjectOfType<AzureCoreSystem>();
				this._nextAzureLookup = Time.unscaledTime + 1f;
			}
			DateTime dateTime = this.ResolveAzureDateTime();
			this.RefreshClock(dateTime);
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0009F9C0 File Offset: 0x0009DBC0
		private void RefreshClock(DateTime current)
		{
			TimeSpan timeOfDay = current.TimeOfDay;
			if (this.analog)
			{
				if (this.hours != null)
				{
					this.hours.localRotation = Quaternion.Euler(0f, 0f, (float)timeOfDay.TotalHours * -30f);
				}
				if (this.minutes != null)
				{
					this.minutes.localRotation = Quaternion.Euler(0f, 0f, (float)timeOfDay.TotalMinutes * -6f);
				}
				if (this.seconds != null)
				{
					this.seconds.localRotation = Quaternion.Euler(0f, 0f, (float)timeOfDay.TotalSeconds * -6f);
				}
			}
			else
			{
				if (this.hours != null)
				{
					this.hours.localRotation = Quaternion.Euler(0f, 0f, (float)current.Hour * -30f);
				}
				if (this.minutes != null)
				{
					this.minutes.localRotation = Quaternion.Euler(0f, 0f, (float)current.Minute * -6f);
				}
				if (this.seconds != null)
				{
					this.seconds.localRotation = Quaternion.Euler(0f, 0f, (float)current.Second * -6f);
				}
			}
			long num = current.Ticks / 600000000L;
			if (this._displayedMinute != num)
			{
				this._displayedMinute = num;
				string text = current.ToString("HH:mm");
				if (this.digitalTime != null)
				{
					this.digitalTime.text = text;
				}
				if (this.notchTime != null)
				{
					this.notchTime.text = text;
				}
			}
			if (this.DateText == null)
			{
				return;
			}
			if (this._localizedDateText != this.DateText)
			{
				this._localizedDateText = this.DateText;
				LocalizedGameText.BindFontOnly(this.DateText);
				this._displayedDate = long.MinValue;
			}
			string currentCode = GameLocalization.CurrentCode;
			long ticks = current.Date.Ticks;
			if (this._displayedDate != ticks || this._displayedLocale != currentCode)
			{
				this._displayedDate = ticks;
				this._displayedLocale = currentCode;
				this.DateText.text = ((currentCode == "en") ? current.ToString("dddd dd MMMM") : current.ToString("dddd dd MMMM", CultureInfo.GetCultureInfo((currentCode == "zh-Hans") ? "zh-CN" : currentCode)));
			}
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x0009FC60 File Offset: 0x0009DE60
		private DateTime ResolveAzureDateTime()
		{
			if (this._azure == null || this._azure.timeSystem == null)
			{
				return DateTime.Now;
			}
			AzureTimeSystem timeSystem = this._azure.timeSystem;
			int num = Mathf.FloorToInt(Mathf.Repeat(timeSystem.timeOfDay, 24f) * 3600f);
			DateTime dateTime;
			try
			{
				dateTime = new DateTime(timeSystem.year, timeSystem.month, timeSystem.day);
				dateTime = dateTime.AddSeconds((double)num);
			}
			catch (ArgumentOutOfRangeException)
			{
				dateTime = DateTime.Today.AddSeconds((double)num);
			}
			return dateTime;
		}

		// Token: 0x04001A38 RID: 6712
		public TMP_Text digitalTime;

		// Token: 0x04001A39 RID: 6713
		public TMP_Text notchTime;

		// Token: 0x04001A3A RID: 6714
		public TMP_Text DateText;

		// Token: 0x04001A3B RID: 6715
		private const float HoursToDegrees = 30f;

		// Token: 0x04001A3C RID: 6716
		private const float MinutesToDegrees = 6f;

		// Token: 0x04001A3D RID: 6717
		private const float SecondsToDegrees = 6f;

		// Token: 0x04001A3E RID: 6718
		public Transform hours;

		// Token: 0x04001A3F RID: 6719
		public Transform minutes;

		// Token: 0x04001A40 RID: 6720
		public Transform seconds;

		// Token: 0x04001A41 RID: 6721
		public bool analog = true;

		// Token: 0x04001A42 RID: 6722
		private AzureCoreSystem _azure;

		// Token: 0x04001A43 RID: 6723
		private float _nextAzureLookup;

		// Token: 0x04001A44 RID: 6724
		private long _displayedMinute = long.MinValue;

		// Token: 0x04001A45 RID: 6725
		private long _displayedDate = long.MinValue;

		// Token: 0x04001A46 RID: 6726
		private string _displayedLocale;

		// Token: 0x04001A47 RID: 6727
		private TMP_Text _localizedDateText;
	}
}
