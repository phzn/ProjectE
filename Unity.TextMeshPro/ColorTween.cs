using System;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro
{
	// Token: 0x02000029 RID: 41
	internal struct ColorTween : ITweenValue
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000140 RID: 320 RVA: 0x0001789B File Offset: 0x00015A9B
		// (set) Token: 0x06000141 RID: 321 RVA: 0x000178A3 File Offset: 0x00015AA3
		public Color startColor
		{
			get
			{
				return this.m_StartColor;
			}
			set
			{
				this.m_StartColor = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000142 RID: 322 RVA: 0x000178AC File Offset: 0x00015AAC
		// (set) Token: 0x06000143 RID: 323 RVA: 0x000178B4 File Offset: 0x00015AB4
		public Color targetColor
		{
			get
			{
				return this.m_TargetColor;
			}
			set
			{
				this.m_TargetColor = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000144 RID: 324 RVA: 0x000178BD File Offset: 0x00015ABD
		// (set) Token: 0x06000145 RID: 325 RVA: 0x000178C5 File Offset: 0x00015AC5
		public ColorTween.ColorTweenMode tweenMode
		{
			get
			{
				return this.m_TweenMode;
			}
			set
			{
				this.m_TweenMode = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000146 RID: 326 RVA: 0x000178CE File Offset: 0x00015ACE
		// (set) Token: 0x06000147 RID: 327 RVA: 0x000178D6 File Offset: 0x00015AD6
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

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000148 RID: 328 RVA: 0x000178DF File Offset: 0x00015ADF
		// (set) Token: 0x06000149 RID: 329 RVA: 0x000178E7 File Offset: 0x00015AE7
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

		// Token: 0x0600014A RID: 330 RVA: 0x000178F0 File Offset: 0x00015AF0
		public void TweenValue(float floatPercentage)
		{
			if (!this.ValidTarget())
			{
				return;
			}
			Color color = Color.Lerp(this.m_StartColor, this.m_TargetColor, floatPercentage);
			if (this.m_TweenMode == ColorTween.ColorTweenMode.Alpha)
			{
				color.r = this.m_StartColor.r;
				color.g = this.m_StartColor.g;
				color.b = this.m_StartColor.b;
			}
			else if (this.m_TweenMode == ColorTween.ColorTweenMode.RGB)
			{
				color.a = this.m_StartColor.a;
			}
			this.m_Target.Invoke(color);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00017981 File Offset: 0x00015B81
		public void AddOnChangedCallback(UnityAction<Color> callback)
		{
			if (this.m_Target == null)
			{
				this.m_Target = new ColorTween.ColorTweenCallback();
			}
			this.m_Target.AddListener(callback);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000179A2 File Offset: 0x00015BA2
		public bool GetIgnoreTimescale()
		{
			return this.m_IgnoreTimeScale;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000179AA File Offset: 0x00015BAA
		public float GetDuration()
		{
			return this.m_Duration;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000179B2 File Offset: 0x00015BB2
		public bool ValidTarget()
		{
			return this.m_Target != null;
		}

		// Token: 0x0400014D RID: 333
		private ColorTween.ColorTweenCallback m_Target;

		// Token: 0x0400014E RID: 334
		private Color m_StartColor;

		// Token: 0x0400014F RID: 335
		private Color m_TargetColor;

		// Token: 0x04000150 RID: 336
		private ColorTween.ColorTweenMode m_TweenMode;

		// Token: 0x04000151 RID: 337
		private float m_Duration;

		// Token: 0x04000152 RID: 338
		private bool m_IgnoreTimeScale;

		// Token: 0x0200007B RID: 123
		public enum ColorTweenMode
		{
			// Token: 0x0400058B RID: 1419
			All,
			// Token: 0x0400058C RID: 1420
			RGB,
			// Token: 0x0400058D RID: 1421
			Alpha
		}

		// Token: 0x0200007C RID: 124
		public class ColorTweenCallback : UnityEvent<Color>
		{
		}
	}
}
