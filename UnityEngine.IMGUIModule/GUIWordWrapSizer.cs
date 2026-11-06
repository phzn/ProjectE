using System;

namespace UnityEngine
{
	// Token: 0x02000035 RID: 53
	internal sealed class GUIWordWrapSizer : GUILayoutEntry
	{
		// Token: 0x060003FE RID: 1022 RVA: 0x0000D95C File Offset: 0x0000BB5C
		public GUIWordWrapSizer(GUIStyle style, GUIContent content, GUILayoutOption[] options)
			: base(0f, 0f, 0f, 0f, style)
		{
			this.m_Content = new GUIContent(content);
			this.ApplyOptions(options);
			this.m_ForcedMinHeight = this.minHeight;
			this.m_ForcedMaxHeight = this.maxHeight;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000D9B4 File Offset: 0x0000BBB4
		public override void CalcWidth()
		{
			bool flag = this.minWidth == 0f || this.maxWidth == 0f;
			if (flag)
			{
				float num;
				float num2;
				base.style.CalcMinMaxWidth(this.m_Content, out num, out num2);
				num = Mathf.Ceil(num);
				num2 = Mathf.Ceil(num2);
				bool flag2 = this.minWidth == 0f;
				if (flag2)
				{
					this.minWidth = num;
				}
				bool flag3 = this.maxWidth == 0f;
				if (flag3)
				{
					this.maxWidth = num2;
				}
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000DA3C File Offset: 0x0000BC3C
		public override void CalcHeight()
		{
			bool flag = this.m_ForcedMinHeight == 0f || this.m_ForcedMaxHeight == 0f;
			if (flag)
			{
				float num = base.style.CalcHeight(this.m_Content, this.rect.width);
				bool flag2 = this.m_ForcedMinHeight == 0f;
				if (flag2)
				{
					this.minHeight = num;
				}
				else
				{
					this.minHeight = this.m_ForcedMinHeight;
				}
				bool flag3 = this.m_ForcedMaxHeight == 0f;
				if (flag3)
				{
					this.maxHeight = num;
				}
				else
				{
					this.maxHeight = this.m_ForcedMaxHeight;
				}
			}
		}

		// Token: 0x04000104 RID: 260
		private readonly GUIContent m_Content;

		// Token: 0x04000105 RID: 261
		private readonly float m_ForcedMinHeight;

		// Token: 0x04000106 RID: 262
		private readonly float m_ForcedMaxHeight;
	}
}
