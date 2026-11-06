using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x0200022E RID: 558
	public class ProgressBar : MonoBehaviour
	{
		// Token: 0x06000F4B RID: 3915 RVA: 0x00065C89 File Offset: 0x00063E89
		private void Start()
		{
			this.UpdateUI();
			this.InitializeEvents();
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x00065C98 File Offset: 0x00063E98
		private void Update()
		{
			if (!this.isOn)
			{
				return;
			}
			if (this.currentPercent <= this.maxValue && !this.invert)
			{
				this.currentPercent += (float)this.speed * Time.unscaledDeltaTime;
			}
			else if (this.currentPercent >= this.minValue && this.invert)
			{
				this.currentPercent -= (float)this.speed * Time.unscaledDeltaTime;
			}
			if (this.currentPercent >= this.maxValue && this.speed != 0 && this.restart && !this.invert)
			{
				this.currentPercent = 0f;
			}
			else if (this.currentPercent <= this.minValue && this.speed != 0 && this.restart && this.invert)
			{
				this.currentPercent = this.maxValue;
			}
			else if (this.currentPercent >= this.maxValue && this.speed != 0 && !this.restart && !this.invert)
			{
				this.currentPercent = this.maxValue;
			}
			else if (this.currentPercent <= this.minValue && this.speed != 0 && !this.restart && this.invert)
			{
				this.currentPercent = this.minValue;
			}
			this.UpdateUI();
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00065DE8 File Offset: 0x00063FE8
		public void UpdateUI()
		{
			this.loadingBar.fillAmount = this.currentPercent / this.maxValue;
			if (this.addSuffix)
			{
				this.textPercent.text = this.currentPercent.ToString("F" + this.decimals.ToString()) + this.suffix;
			}
			else
			{
				this.textPercent.text = this.currentPercent.ToString("F" + this.decimals.ToString());
			}
			if (this.addPrefix)
			{
				this.textPercent.text = this.prefix + this.textPercent.text;
			}
			if (this.eventSource != null)
			{
				this.eventSource.value = this.currentPercent;
			}
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00065EC0 File Offset: 0x000640C0
		public void InitializeEvents()
		{
			if (Application.isPlaying && this.onValueChanged.GetPersistentEventCount() != 0)
			{
				if (this.eventSource == null)
				{
					this.eventSource = base.gameObject.AddComponent(typeof(Slider)) as Slider;
				}
				this.eventSource.transition = Selectable.Transition.None;
				this.eventSource.minValue = this.minValue;
				this.eventSource.maxValue = this.maxValue;
				this.eventSource.onValueChanged.AddListener(new UnityAction<float>(this.onValueChanged.Invoke));
			}
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x00065F61 File Offset: 0x00064161
		public void ClearEvents()
		{
			this.eventSource.onValueChanged.RemoveAllListeners();
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x00065F73 File Offset: 0x00064173
		public void ChangeValue(float newValue)
		{
			this.currentPercent = newValue;
			this.UpdateUI();
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00065F73 File Offset: 0x00064173
		public void SetValue(float newValue)
		{
			this.currentPercent = newValue;
			this.UpdateUI();
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00065F82 File Offset: 0x00064182
		public void SetValue(float newValue, string newPrefix = null, string newSuffix = null, bool updateUI = true)
		{
			this.currentPercent = newValue;
			this.prefix = newPrefix;
			this.suffix = newSuffix;
			if (updateUI)
			{
				this.UpdateUI();
			}
		}

		// Token: 0x0400102F RID: 4143
		public float currentPercent;

		// Token: 0x04001030 RID: 4144
		[Range(0f, 100f)]
		public int speed;

		// Token: 0x04001031 RID: 4145
		public float minValue;

		// Token: 0x04001032 RID: 4146
		public float maxValue = 100f;

		// Token: 0x04001033 RID: 4147
		public float valueLimit = 100f;

		// Token: 0x04001034 RID: 4148
		public Image loadingBar;

		// Token: 0x04001035 RID: 4149
		public TextMeshProUGUI textPercent;

		// Token: 0x04001036 RID: 4150
		public bool isOn;

		// Token: 0x04001037 RID: 4151
		public bool restart;

		// Token: 0x04001038 RID: 4152
		public bool invert;

		// Token: 0x04001039 RID: 4153
		public bool addPrefix;

		// Token: 0x0400103A RID: 4154
		public bool addSuffix = true;

		// Token: 0x0400103B RID: 4155
		public string prefix = "";

		// Token: 0x0400103C RID: 4156
		public string suffix = "%";

		// Token: 0x0400103D RID: 4157
		public bool isLooped;

		// Token: 0x0400103E RID: 4158
		[Range(0f, 5f)]
		public int decimals;

		// Token: 0x0400103F RID: 4159
		public ProgressBar.ProgressBarEvent onValueChanged;

		// Token: 0x04001040 RID: 4160
		[HideInInspector]
		public Slider eventSource;

		// Token: 0x0200022F RID: 559
		[Serializable]
		public class ProgressBarEvent : UnityEvent<float>
		{
		}
	}
}
