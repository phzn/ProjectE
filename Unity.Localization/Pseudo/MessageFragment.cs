using System;
using System.Text;

namespace UnityEngine.Localization.Pseudo
{
	// Token: 0x020000A9 RID: 169
	public abstract class MessageFragment
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x000107B2 File Offset: 0x0000E9B2
		public int Length
		{
			get
			{
				if (this.m_StartIndex != -1)
				{
					return this.m_EndIndex - this.m_StartIndex;
				}
				return this.m_OriginalString.Length;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x000107D6 File Offset: 0x0000E9D6
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x000107DE File Offset: 0x0000E9DE
		public Message Message { get; private set; }

		// Token: 0x060004A8 RID: 1192 RVA: 0x000107E7 File Offset: 0x0000E9E7
		internal void Initialize(Message parent, string original, int start, int end)
		{
			this.Message = parent;
			this.m_OriginalString = original;
			this.m_StartIndex = start;
			this.m_EndIndex = end;
			this.m_CachedToString = null;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0001080D File Offset: 0x0000EA0D
		internal void Initialize(Message parent, string text)
		{
			this.Message = parent;
			this.m_OriginalString = text;
			this.m_StartIndex = -1;
			this.m_EndIndex = -1;
			this.m_CachedToString = null;
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00010834 File Offset: 0x0000EA34
		public WritableMessageFragment CreateTextFragment(int start, int end)
		{
			WritableMessageFragment writableMessageFragment = WritableMessageFragment.Pool.Get();
			int num = ((this.m_StartIndex == -1) ? start : (this.m_StartIndex + start));
			int num2 = ((this.m_StartIndex == -1) ? end : (this.m_StartIndex + end));
			writableMessageFragment.Initialize(this.Message, this.m_OriginalString, num, num2);
			return writableMessageFragment;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001088C File Offset: 0x0000EA8C
		public ReadOnlyMessageFragment CreateReadonlyTextFragment(int start, int end)
		{
			ReadOnlyMessageFragment readOnlyMessageFragment = ReadOnlyMessageFragment.Pool.Get();
			int num = ((this.m_StartIndex == -1) ? start : (this.m_StartIndex + start));
			int num2 = ((this.m_StartIndex == -1) ? end : (this.m_StartIndex + end));
			readOnlyMessageFragment.Initialize(this.Message, this.m_OriginalString, num, num2);
			return readOnlyMessageFragment;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x000108E4 File Offset: 0x0000EAE4
		public override string ToString()
		{
			if (this.m_CachedToString == null)
			{
				this.m_CachedToString = ((this.m_StartIndex == -1) ? this.m_OriginalString : this.m_OriginalString.Substring(this.m_StartIndex, this.m_EndIndex - this.m_StartIndex));
			}
			return this.m_CachedToString;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00010934 File Offset: 0x0000EB34
		internal void BuildString(StringBuilder builder)
		{
			if (this.m_StartIndex == -1)
			{
				builder.Append(this.m_OriginalString);
				return;
			}
			builder.Append(this.m_OriginalString, this.m_StartIndex, this.m_EndIndex - this.m_StartIndex);
		}

		// Token: 0x17000113 RID: 275
		public char this[int index]
		{
			get
			{
				int num = ((this.m_StartIndex == -1) ? 0 : this.m_StartIndex);
				return this.m_OriginalString[num + index];
			}
		}

		// Token: 0x0400015A RID: 346
		protected string m_OriginalString;

		// Token: 0x0400015B RID: 347
		protected int m_StartIndex;

		// Token: 0x0400015C RID: 348
		protected int m_EndIndex;

		// Token: 0x0400015D RID: 349
		private string m_CachedToString;
	}
}
