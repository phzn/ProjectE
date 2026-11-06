using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200046A RID: 1130
	internal struct TextCoreSettings : IEquatable<TextCoreSettings>
	{
		// Token: 0x0600231D RID: 8989 RVA: 0x0008851C File Offset: 0x0008671C
		public override bool Equals(object obj)
		{
			return obj is TextCoreSettings && this.Equals((TextCoreSettings)obj);
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x00088548 File Offset: 0x00086748
		public bool Equals(TextCoreSettings other)
		{
			return other.faceColor == this.faceColor && other.outlineColor == this.outlineColor && other.outlineWidth == this.outlineWidth && other.underlayColor == this.underlayColor && other.underlayOffset == this.underlayOffset && other.underlaySoftness == this.underlaySoftness;
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x000885C8 File Offset: 0x000867C8
		public override int GetHashCode()
		{
			int num = 75905159;
			num = num * -1521134295 + this.faceColor.GetHashCode();
			num = num * -1521134295 + this.outlineColor.GetHashCode();
			num = num * -1521134295 + this.outlineWidth.GetHashCode();
			num = num * -1521134295 + this.underlayColor.GetHashCode();
			num = num * -1521134295 + this.underlayOffset.x.GetHashCode();
			num = num * -1521134295 + this.underlayOffset.y.GetHashCode();
			return num * -1521134295 + this.underlaySoftness.GetHashCode();
		}

		// Token: 0x0400103E RID: 4158
		public Color faceColor;

		// Token: 0x0400103F RID: 4159
		public Color outlineColor;

		// Token: 0x04001040 RID: 4160
		public float outlineWidth;

		// Token: 0x04001041 RID: 4161
		public Color underlayColor;

		// Token: 0x04001042 RID: 4162
		public Vector2 underlayOffset;

		// Token: 0x04001043 RID: 4163
		public float underlaySoftness;
	}
}
