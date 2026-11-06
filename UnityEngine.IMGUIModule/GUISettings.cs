using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000025 RID: 37
	[NativeHeader("Modules/IMGUI/GUISkin.bindings.h")]
	[Serializable]
	public sealed class GUISettings
	{
		// Token: 0x06000290 RID: 656
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float Internal_GetCursorFlashSpeed();

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000AA20 File Offset: 0x00008C20
		// (set) Token: 0x06000292 RID: 658 RVA: 0x0000AA38 File Offset: 0x00008C38
		public bool doubleClickSelectsWord
		{
			get
			{
				return this.m_DoubleClickSelectsWord;
			}
			set
			{
				this.m_DoubleClickSelectsWord = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000AA44 File Offset: 0x00008C44
		// (set) Token: 0x06000294 RID: 660 RVA: 0x0000AA5C File Offset: 0x00008C5C
		public bool tripleClickSelectsLine
		{
			get
			{
				return this.m_TripleClickSelectsLine;
			}
			set
			{
				this.m_TripleClickSelectsLine = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000AA68 File Offset: 0x00008C68
		// (set) Token: 0x06000296 RID: 662 RVA: 0x0000AA80 File Offset: 0x00008C80
		public Color cursorColor
		{
			get
			{
				return this.m_CursorColor;
			}
			set
			{
				this.m_CursorColor = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000AA8C File Offset: 0x00008C8C
		// (set) Token: 0x06000298 RID: 664 RVA: 0x0000AAC1 File Offset: 0x00008CC1
		public float cursorFlashSpeed
		{
			get
			{
				bool flag = this.m_CursorFlashSpeed >= 0f;
				float num;
				if (flag)
				{
					num = this.m_CursorFlashSpeed;
				}
				else
				{
					num = GUISettings.Internal_GetCursorFlashSpeed();
				}
				return num;
			}
			set
			{
				this.m_CursorFlashSpeed = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000AACC File Offset: 0x00008CCC
		// (set) Token: 0x0600029A RID: 666 RVA: 0x0000AAE4 File Offset: 0x00008CE4
		public Color selectionColor
		{
			get
			{
				return this.m_SelectionColor;
			}
			set
			{
				this.m_SelectionColor = value;
			}
		}

		// Token: 0x040000A2 RID: 162
		[SerializeField]
		private bool m_DoubleClickSelectsWord = true;

		// Token: 0x040000A3 RID: 163
		[SerializeField]
		private bool m_TripleClickSelectsLine = true;

		// Token: 0x040000A4 RID: 164
		[SerializeField]
		private Color m_CursorColor = Color.white;

		// Token: 0x040000A5 RID: 165
		[SerializeField]
		private float m_CursorFlashSpeed = -1f;

		// Token: 0x040000A6 RID: 166
		[SerializeField]
		private Color m_SelectionColor = new Color(0.5f, 0.5f, 1f);
	}
}
