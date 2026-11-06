using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public struct ColorBlock : IEquatable<ColorBlock>
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000026F3 File Offset: 0x000008F3
		// (set) Token: 0x06000031 RID: 49 RVA: 0x000026FB File Offset: 0x000008FB
		public Color normalColor
		{
			get
			{
				return this.m_NormalColor;
			}
			set
			{
				this.m_NormalColor = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002704 File Offset: 0x00000904
		// (set) Token: 0x06000033 RID: 51 RVA: 0x0000270C File Offset: 0x0000090C
		public Color highlightedColor
		{
			get
			{
				return this.m_HighlightedColor;
			}
			set
			{
				this.m_HighlightedColor = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002715 File Offset: 0x00000915
		// (set) Token: 0x06000035 RID: 53 RVA: 0x0000271D File Offset: 0x0000091D
		public Color pressedColor
		{
			get
			{
				return this.m_PressedColor;
			}
			set
			{
				this.m_PressedColor = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002726 File Offset: 0x00000926
		// (set) Token: 0x06000037 RID: 55 RVA: 0x0000272E File Offset: 0x0000092E
		public Color selectedColor
		{
			get
			{
				return this.m_SelectedColor;
			}
			set
			{
				this.m_SelectedColor = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002737 File Offset: 0x00000937
		// (set) Token: 0x06000039 RID: 57 RVA: 0x0000273F File Offset: 0x0000093F
		public Color disabledColor
		{
			get
			{
				return this.m_DisabledColor;
			}
			set
			{
				this.m_DisabledColor = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002748 File Offset: 0x00000948
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002750 File Offset: 0x00000950
		public float colorMultiplier
		{
			get
			{
				return this.m_ColorMultiplier;
			}
			set
			{
				this.m_ColorMultiplier = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002759 File Offset: 0x00000959
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002761 File Offset: 0x00000961
		public float fadeDuration
		{
			get
			{
				return this.m_FadeDuration;
			}
			set
			{
				this.m_FadeDuration = value;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002858 File Offset: 0x00000A58
		public override bool Equals(object obj)
		{
			return obj is ColorBlock && this.Equals((ColorBlock)obj);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002870 File Offset: 0x00000A70
		public bool Equals(ColorBlock other)
		{
			return this.normalColor == other.normalColor && this.highlightedColor == other.highlightedColor && this.pressedColor == other.pressedColor && this.selectedColor == other.selectedColor && this.disabledColor == other.disabledColor && this.colorMultiplier == other.colorMultiplier && this.fadeDuration == other.fadeDuration;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002901 File Offset: 0x00000B01
		public static bool operator ==(ColorBlock point1, ColorBlock point2)
		{
			return point1.Equals(point2);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000290B File Offset: 0x00000B0B
		public static bool operator !=(ColorBlock point1, ColorBlock point2)
		{
			return !point1.Equals(point2);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002918 File Offset: 0x00000B18
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0400001B RID: 27
		[FormerlySerializedAs("normalColor")]
		[SerializeField]
		private Color m_NormalColor;

		// Token: 0x0400001C RID: 28
		[FormerlySerializedAs("highlightedColor")]
		[SerializeField]
		private Color m_HighlightedColor;

		// Token: 0x0400001D RID: 29
		[FormerlySerializedAs("pressedColor")]
		[SerializeField]
		private Color m_PressedColor;

		// Token: 0x0400001E RID: 30
		[FormerlySerializedAs("m_HighlightedColor")]
		[SerializeField]
		private Color m_SelectedColor;

		// Token: 0x0400001F RID: 31
		[FormerlySerializedAs("disabledColor")]
		[SerializeField]
		private Color m_DisabledColor;

		// Token: 0x04000020 RID: 32
		[Range(1f, 5f)]
		[SerializeField]
		private float m_ColorMultiplier;

		// Token: 0x04000021 RID: 33
		[FormerlySerializedAs("fadeDuration")]
		[SerializeField]
		private float m_FadeDuration;

		// Token: 0x04000022 RID: 34
		public static ColorBlock defaultColorBlock = new ColorBlock
		{
			m_NormalColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue),
			m_HighlightedColor = new Color32(245, 245, 245, byte.MaxValue),
			m_PressedColor = new Color32(200, 200, 200, byte.MaxValue),
			m_SelectedColor = new Color32(245, 245, 245, byte.MaxValue),
			m_DisabledColor = new Color32(200, 200, 200, 128),
			colorMultiplier = 1f,
			fadeDuration = 0.1f
		};
	}
}
