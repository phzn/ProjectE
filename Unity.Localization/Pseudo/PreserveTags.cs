using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace UnityEngine.Localization.Pseudo
{
	// Token: 0x020000B2 RID: 178
	[Serializable]
	public class PreserveTags : IPseudoLocalizationMethod
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00011CC7 File Offset: 0x0000FEC7
		// (set) Token: 0x060004F1 RID: 1265 RVA: 0x00011CCF File Offset: 0x0000FECF
		public char Opening
		{
			get
			{
				return this.m_Opening;
			}
			set
			{
				this.m_Opening = value;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00011CD8 File Offset: 0x0000FED8
		// (set) Token: 0x060004F3 RID: 1267 RVA: 0x00011CE0 File Offset: 0x0000FEE0
		public char Closing
		{
			get
			{
				return this.m_Closing;
			}
			set
			{
				this.m_Closing = value;
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00011CEC File Offset: 0x0000FEEC
		public void Transform(Message message)
		{
			List<MessageFragment> list;
			using (CollectionPool<List<MessageFragment>, MessageFragment>.Get(out list))
			{
				for (int i = 0; i < message.Fragments.Count; i++)
				{
					int num = 0;
					int num2 = -1;
					MessageFragment messageFragment = message.Fragments[i];
					WritableMessageFragment writableMessageFragment = messageFragment as WritableMessageFragment;
					if (writableMessageFragment != null)
					{
						for (int j = 0; j < messageFragment.Length; j++)
						{
							if (messageFragment[j] == this.m_Opening)
							{
								num2 = j;
							}
							else if (messageFragment[j] == this.m_Closing && num2 != -1)
							{
								int num3 = j + 1;
								if (num != num2)
								{
									list.Add(writableMessageFragment.CreateTextFragment(num, num2));
								}
								list.Add(writableMessageFragment.CreateReadonlyTextFragment(num2, num3));
								num2 = -1;
								num = j + 1;
							}
						}
						message.ReleaseFragment(messageFragment);
						if (num != messageFragment.Length)
						{
							list.Add(writableMessageFragment.CreateTextFragment(num, messageFragment.Length));
						}
					}
					else
					{
						list.Add(messageFragment);
					}
				}
				message.Fragments.Clear();
				message.Fragments.AddRange(list);
			}
		}

		// Token: 0x04000170 RID: 368
		[SerializeField]
		private char m_Opening = '<';

		// Token: 0x04000171 RID: 369
		[SerializeField]
		private char m_Closing = '>';
	}
}
