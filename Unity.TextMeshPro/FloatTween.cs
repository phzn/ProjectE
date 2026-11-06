using System;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro
{
	// Token: 0x0200002A RID: 42
	internal struct FloatTween : ITweenValue
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600014F RID: 335 RVA: 0x000179BD File Offset: 0x00015BBD
		// (set) Token: 0x06000150 RID: 336 RVA: 0x000179C5 File Offset: 0x00015BC5
		public float startValue
		{
			get
			{
				return this.m_StartValue;
			}
			set
			{
				this.m_StartValue = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000151 RID: 337 RVA: 0x000179CE File Offset: 0x00015BCE
		// (set) Token: 0x06000152 RID: 338 RVA: 0x000179D6 File Offset: 0x00015BD6
		public float targetValue
		{
			get
			{
				return this.m_TargetValue;
			}
			set
			{
				this.m_TargetValue = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000179DF File Offset: 0x00015BDF
		// (set) Token: 0x06000154 RID: 340 RVA: 0x000179E7 File Offset: 0x00015BE7
		public float duration
		{
			get
			{
				return this.m_Duration;
			}
			set
			{
				this.m_Duration = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000179F0 File Offset: 0x00015BF0
		// (set) Token: 0x06000156 RID: 342 RVA: 0x000179F8 File Offset: 0x00015BF8
		public bool ignoreTimeScale
		{
			get
			{
				return this.m_IgnoreTimeScale;
			}
			set
			{
				this.m_IgnoreTimeScale = value;
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00017A04 File Offset: 0x00015C04
		public void TweenValue(float floatPercentage)
		{
			if (!this.ValidTarget())
			{
				return;
			}
			float num = Mathf.Lerp(this.m_StartValue, this.m_TargetValue, floatPercentage);
			this.m_Target.Invoke(num);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00017A39 File Offset: 0x00015C39
		public void AddOnChangedCallback(UnityAction<float> callback)
		{
			if (this.m_Target == null)
			{
				this.m_Target = new FloatTween.FloatTweenCallback();
			}
			this.m_Target.AddListener(callback);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00017A5A File Offset: 0x00015C5A
		public bool GetIgnoreTimescale()
		{
			return this.m_IgnoreTimeScale;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00017A62 File Offset: 0x00015C62
		public float GetDuration()
		{
			return this.m_Duration;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00017A6A File Offset: 0x00015C6A
		public bool ValidTarget()
		{
			return this.m_Target != null;
		}

		// Token: 0x04000153 RID: 339
		private FloatTween.FloatTweenCallback m_Target;

		// Token: 0x04000154 RID: 340
		private float m_StartValue;

		// Token: 0x04000155 RID: 341
		private float m_TargetValue;

		// Token: 0x04000156 RID: 342
		private float m_Duration;

		// Token: 0x04000157 RID: 343
		private bool m_IgnoreTimeScale;

		// Token: 0x0200007D RID: 125
		public class FloatTweenCallback : UnityEvent<float>
		{
		}
	}
}
