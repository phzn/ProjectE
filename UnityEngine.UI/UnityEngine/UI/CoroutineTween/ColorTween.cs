using System;
using UnityEngine.Events;

namespace UnityEngine.UI.CoroutineTween
{
	// Token: 0x02000048 RID: 72
	internal struct ColorTween : ITweenValue
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x000172D3 File Offset: 0x000154D3
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x000172DB File Offset: 0x000154DB
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

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x000172E4 File Offset: 0x000154E4
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x000172EC File Offset: 0x000154EC
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

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x000172F5 File Offset: 0x000154F5
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x000172FD File Offset: 0x000154FD
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

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00017306 File Offset: 0x00015506
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x0001730E File Offset: 0x0001550E
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

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x00017317 File Offset: 0x00015517
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x0001731F File Offset: 0x0001551F
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

		// Token: 0x060004E7 RID: 1255 RVA: 0x00017328 File Offset: 0x00015528
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

		// Token: 0x060004E8 RID: 1256 RVA: 0x000173B9 File Offset: 0x000155B9
		public void AddOnChangedCallback(UnityAction<Color> callback)
		{
			if (this.m_Target == null)
			{
				this.m_Target = new ColorTween.ColorTweenCallback();
			}
			this.m_Target.AddListener(callback);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000173DA File Offset: 0x000155DA
		public bool GetIgnoreTimescale()
		{
			return this.m_IgnoreTimeScale;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x000173E2 File Offset: 0x000155E2
		public float GetDuration()
		{
			return this.m_Duration;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x000173EA File Offset: 0x000155EA
		public bool ValidTarget()
		{
			return this.m_Target != null;
		}

		// Token: 0x04000198 RID: 408
		private ColorTween.ColorTweenCallback m_Target;

		// Token: 0x04000199 RID: 409
		private Color m_StartColor;

		// Token: 0x0400019A RID: 410
		private Color m_TargetColor;

		// Token: 0x0400019B RID: 411
		private ColorTween.ColorTweenMode m_TweenMode;

		// Token: 0x0400019C RID: 412
		private float m_Duration;

		// Token: 0x0400019D RID: 413
		private bool m_IgnoreTimeScale;

		// Token: 0x020000BB RID: 187
		public enum ColorTweenMode
		{
			// Token: 0x04000327 RID: 807
			All,
			// Token: 0x04000328 RID: 808
			RGB,
			// Token: 0x04000329 RID: 809
			Alpha
		}

		// Token: 0x020000BC RID: 188
		public class ColorTweenCallback : UnityEvent<Color>
		{
		}
	}
}
