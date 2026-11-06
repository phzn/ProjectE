using System;
using System.Collections.Generic;

namespace UnityEngine.Localization.Pseudo
{
	// Token: 0x020000B0 RID: 176
	[Serializable]
	public class Expander : IPseudoLocalizationMethod
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00011776 File Offset: 0x0000F976
		public List<Expander.ExpansionRule> ExpansionRules
		{
			get
			{
				return this.m_ExpansionRules;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x0001177E File Offset: 0x0000F97E
		// (set) Token: 0x060004DF RID: 1247 RVA: 0x00011786 File Offset: 0x0000F986
		public Expander.InsertLocation Location
		{
			get
			{
				return this.m_Location;
			}
			set
			{
				this.m_Location = value;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x0001178F File Offset: 0x0000F98F
		public List<char> PaddingCharacters
		{
			get
			{
				return this.m_PaddingCharacters;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00011797 File Offset: 0x0000F997
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x0001179F File Offset: 0x0000F99F
		public int MinimumStringLength
		{
			get
			{
				return this.m_MinimumStringLength;
			}
			set
			{
				this.m_MinimumStringLength = Mathf.Max(0, value);
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x000117B0 File Offset: 0x0000F9B0
		public Expander()
		{
			this.AddCharacterRange('!', '~');
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001186C File Offset: 0x0000FA6C
		public Expander(char paddingCharacter)
		{
			this.PaddingCharacters.Add(paddingCharacter);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001192C File Offset: 0x0000FB2C
		public Expander(char start, char end)
		{
			this.AddCharacterRange(start, end);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000119E8 File Offset: 0x0000FBE8
		public void AddCharacterRange(char start, char end)
		{
			for (char c = start; c < end; c += '\u0001')
			{
				this.PaddingCharacters.Add(c);
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00011A0E File Offset: 0x0000FC0E
		public void SetConstantExpansion(float expansion)
		{
			if (this.m_ExpansionRules != null)
			{
				this.m_ExpansionRules.Clear();
			}
			this.AddExpansionRule(0, int.MaxValue, expansion);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00011A30 File Offset: 0x0000FC30
		public void AddExpansionRule(int minCharacters, int maxCharacters, float expansion)
		{
			if (this.m_ExpansionRules == null)
			{
				this.m_ExpansionRules = new List<Expander.ExpansionRule>();
			}
			this.m_ExpansionRules.Add(new Expander.ExpansionRule(minCharacters, maxCharacters, expansion));
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00011A58 File Offset: 0x0000FC58
		internal float GetExpansionForLength(int length)
		{
			foreach (Expander.ExpansionRule expansionRule in this.ExpansionRules)
			{
				if (expansionRule.InRange(length))
				{
					return expansionRule.ExpansionAmount;
				}
			}
			return 0f;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00011AC0 File Offset: 0x0000FCC0
		public void Transform(Message message)
		{
			int length = message.Length;
			int num = Mathf.Max(length, this.MinimumStringLength);
			int num2 = Mathf.CeilToInt(this.GetExpansionForLength(num) * (float)num);
			if (num2 > 0)
			{
				num2 += num - length;
				char[] array = new char[num2];
				Random.InitState(this.GetRandomSeed(message.Original));
				for (int i = 0; i < num2; i++)
				{
					array[i] = this.PaddingCharacters[Random.Range(0, this.PaddingCharacters.Count)];
				}
				this.AddPaddingToMessage(message, array);
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00011B4C File Offset: 0x0000FD4C
		private void AddPaddingToMessage(Message message, char[] padding)
		{
			MessageFragment messageFragment = null;
			MessageFragment messageFragment2 = null;
			string text = new string(padding);
			if (this.Location == Expander.InsertLocation.Start)
			{
				messageFragment = message.CreateTextFragment(text);
			}
			else if (this.Location == Expander.InsertLocation.End)
			{
				messageFragment2 = message.CreateTextFragment(text);
			}
			else
			{
				int num = Mathf.FloorToInt((float)padding.Length * 0.5f);
				messageFragment = message.CreateTextFragment(text, 0, num);
				messageFragment2 = message.CreateTextFragment(text, num, padding.Length - 1);
			}
			if (messageFragment != null)
			{
				message.Fragments.Insert(0, messageFragment);
			}
			if (messageFragment2 != null)
			{
				message.Fragments.Add(messageFragment2);
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00011BD0 File Offset: 0x0000FDD0
		private int GetRandomSeed(string input)
		{
			return input.GetHashCode();
		}

		// Token: 0x0400016C RID: 364
		[SerializeField]
		private List<Expander.ExpansionRule> m_ExpansionRules = new List<Expander.ExpansionRule>
		{
			new Expander.ExpansionRule(0, 10, 2f),
			new Expander.ExpansionRule(10, 20, 1f),
			new Expander.ExpansionRule(20, 30, 0.8f),
			new Expander.ExpansionRule(30, 50, 0.6f),
			new Expander.ExpansionRule(50, 70, 0.7f),
			new Expander.ExpansionRule(70, int.MaxValue, 0.3f)
		};

		// Token: 0x0400016D RID: 365
		[SerializeField]
		private Expander.InsertLocation m_Location = Expander.InsertLocation.End;

		// Token: 0x0400016E RID: 366
		[SerializeField]
		private int m_MinimumStringLength = 1;

		// Token: 0x0400016F RID: 367
		[SerializeField]
		private List<char> m_PaddingCharacters = new List<char>();

		// Token: 0x02000150 RID: 336
		public enum InsertLocation
		{
			// Token: 0x040002FF RID: 767
			Start,
			// Token: 0x04000300 RID: 768
			End,
			// Token: 0x04000301 RID: 769
			Both
		}

		// Token: 0x02000151 RID: 337
		[Serializable]
		public struct ExpansionRule : IComparable<Expander.ExpansionRule>
		{
			// Token: 0x17000191 RID: 401
			// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00019415 File Offset: 0x00017615
			// (set) Token: 0x060007E4 RID: 2020 RVA: 0x0001941D File Offset: 0x0001761D
			public int MinCharacters
			{
				get
				{
					return this.m_MinCharacters;
				}
				set
				{
					this.m_MinCharacters = Mathf.Max(0, value);
				}
			}

			// Token: 0x17000192 RID: 402
			// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0001942C File Offset: 0x0001762C
			// (set) Token: 0x060007E6 RID: 2022 RVA: 0x00019434 File Offset: 0x00017634
			public int MaxCharacters
			{
				get
				{
					return this.m_MaxCharacters;
				}
				set
				{
					this.m_MaxCharacters = Mathf.Max(0, value);
				}
			}

			// Token: 0x17000193 RID: 403
			// (get) Token: 0x060007E7 RID: 2023 RVA: 0x00019443 File Offset: 0x00017643
			// (set) Token: 0x060007E8 RID: 2024 RVA: 0x0001944B File Offset: 0x0001764B
			public float ExpansionAmount
			{
				get
				{
					return this.m_ExpansionAmount;
				}
				set
				{
					this.m_ExpansionAmount = Mathf.Max(0f, value);
				}
			}

			// Token: 0x060007E9 RID: 2025 RVA: 0x0001945E File Offset: 0x0001765E
			public ExpansionRule(int minCharacters, int maxCharacters, float expansion)
			{
				this.m_MinCharacters = Mathf.Max(0, minCharacters);
				this.m_MaxCharacters = Mathf.Max(0, maxCharacters);
				this.m_ExpansionAmount = Mathf.Max(0f, expansion);
			}

			// Token: 0x060007EA RID: 2026 RVA: 0x0001948B File Offset: 0x0001768B
			internal bool InRange(int length)
			{
				return length >= this.MinCharacters && length < this.MaxCharacters;
			}

			// Token: 0x060007EB RID: 2027 RVA: 0x000194A4 File Offset: 0x000176A4
			public int CompareTo(Expander.ExpansionRule other)
			{
				return this.MinCharacters.CompareTo(other.MinCharacters);
			}

			// Token: 0x04000302 RID: 770
			[SerializeField]
			private int m_MinCharacters;

			// Token: 0x04000303 RID: 771
			[SerializeField]
			private int m_MaxCharacters;

			// Token: 0x04000304 RID: 772
			[SerializeField]
			private float m_ExpansionAmount;
		}
	}
}
