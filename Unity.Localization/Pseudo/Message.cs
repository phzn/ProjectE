using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Pool;

namespace UnityEngine.Localization.Pseudo
{
	// Token: 0x020000AC RID: 172
	public class Message
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00010A23 File Offset: 0x0000EC23
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x00010A2B File Offset: 0x0000EC2B
		public string Original { get; private set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00010A34 File Offset: 0x0000EC34
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x00010A3C File Offset: 0x0000EC3C
		public List<MessageFragment> Fragments { get; private set; } = new List<MessageFragment>();

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00010A48 File Offset: 0x0000EC48
		public int Length
		{
			get
			{
				int num = 0;
				foreach (MessageFragment messageFragment in this.Fragments)
				{
					num += messageFragment.Length;
				}
				return num;
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00010AA0 File Offset: 0x0000ECA0
		public WritableMessageFragment CreateTextFragment(string original, int start, int end)
		{
			WritableMessageFragment writableMessageFragment = WritableMessageFragment.Pool.Get();
			writableMessageFragment.Initialize(this, original, start, end);
			return writableMessageFragment;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00010AB6 File Offset: 0x0000ECB6
		public WritableMessageFragment CreateTextFragment(string original)
		{
			WritableMessageFragment writableMessageFragment = WritableMessageFragment.Pool.Get();
			writableMessageFragment.Initialize(this, original);
			return writableMessageFragment;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00010ACA File Offset: 0x0000ECCA
		public ReadOnlyMessageFragment CreateReadonlyTextFragment(string original, int start, int end)
		{
			ReadOnlyMessageFragment readOnlyMessageFragment = ReadOnlyMessageFragment.Pool.Get();
			readOnlyMessageFragment.Initialize(this, original, start, end);
			return readOnlyMessageFragment;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00010AE0 File Offset: 0x0000ECE0
		public ReadOnlyMessageFragment CreateReadonlyTextFragment(string original)
		{
			ReadOnlyMessageFragment readOnlyMessageFragment = ReadOnlyMessageFragment.Pool.Get();
			readOnlyMessageFragment.Initialize(this, original);
			return readOnlyMessageFragment;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00010AF4 File Offset: 0x0000ECF4
		public void ReplaceFragment(MessageFragment original, MessageFragment replacement)
		{
			int num = this.Fragments.IndexOf(original);
			if (num == -1)
			{
				throw new Exception("Can not replace Fragment " + original.ToString() + " that is not part of the message.");
			}
			this.Fragments[num] = replacement;
			this.ReleaseFragment(original);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00010B44 File Offset: 0x0000ED44
		public void ReleaseFragment(MessageFragment fragment)
		{
			WritableMessageFragment writableMessageFragment = fragment as WritableMessageFragment;
			if (writableMessageFragment != null)
			{
				WritableMessageFragment.Pool.Release(writableMessageFragment);
				return;
			}
			ReadOnlyMessageFragment readOnlyMessageFragment = fragment as ReadOnlyMessageFragment;
			if (readOnlyMessageFragment != null)
			{
				ReadOnlyMessageFragment.Pool.Release(readOnlyMessageFragment);
			}
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00010B7C File Offset: 0x0000ED7C
		internal static Message CreateMessage(string text)
		{
			Message message = Message.Pool.Get();
			message.Fragments.Add(message.CreateTextFragment(text));
			message.Original = text;
			return message;
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00010BB0 File Offset: 0x0000EDB0
		internal void Release()
		{
			foreach (MessageFragment messageFragment in this.Fragments)
			{
				this.ReleaseFragment(messageFragment);
			}
			this.Fragments.Clear();
			Message.Pool.Release(this);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00010C1C File Offset: 0x0000EE1C
		public override string ToString()
		{
			StringBuilder stringBuilder;
			string text;
			using (StringBuilderPool.Get(out stringBuilder))
			{
				foreach (MessageFragment messageFragment in this.Fragments)
				{
					messageFragment.BuildString(stringBuilder);
				}
				text = stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x04000161 RID: 353
		internal static readonly ObjectPool<Message> Pool = new ObjectPool<Message>(() => new Message(), null, null, null, false, 10, 10000);
	}
}
