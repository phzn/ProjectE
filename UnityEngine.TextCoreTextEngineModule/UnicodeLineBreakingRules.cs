using System;
using System.Collections.Generic;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000051 RID: 81
	[Serializable]
	public class UnicodeLineBreakingRules
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600026A RID: 618 RVA: 0x00024F56 File Offset: 0x00023156
		public TextAsset lineBreakingRules
		{
			get
			{
				return this.m_UnicodeLineBreakingRules;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00024F5E File Offset: 0x0002315E
		public TextAsset leadingCharacters
		{
			get
			{
				return this.m_LeadingCharacters;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00024F66 File Offset: 0x00023166
		public TextAsset followingCharacters
		{
			get
			{
				return this.m_FollowingCharacters;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00024F70 File Offset: 0x00023170
		// (set) Token: 0x0600026E RID: 622 RVA: 0x00024FA8 File Offset: 0x000231A8
		internal HashSet<uint> leadingCharactersLookup
		{
			get
			{
				bool flag = this.m_LeadingCharactersLookup == null;
				if (flag)
				{
					this.LoadLineBreakingRules(this.leadingCharacters, this.followingCharacters);
				}
				return this.m_LeadingCharactersLookup;
			}
			set
			{
				this.m_LeadingCharactersLookup = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600026F RID: 623 RVA: 0x00024FB4 File Offset: 0x000231B4
		// (set) Token: 0x06000270 RID: 624 RVA: 0x00024FEC File Offset: 0x000231EC
		internal HashSet<uint> followingCharactersLookup
		{
			get
			{
				bool flag = this.m_LeadingCharactersLookup == null;
				if (flag)
				{
					this.LoadLineBreakingRules(this.leadingCharacters, this.followingCharacters);
				}
				return this.m_FollowingCharactersLookup;
			}
			set
			{
				this.m_FollowingCharactersLookup = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000271 RID: 625 RVA: 0x00024FF5 File Offset: 0x000231F5
		// (set) Token: 0x06000272 RID: 626 RVA: 0x00024FFD File Offset: 0x000231FD
		public bool useModernHangulLineBreakingRules
		{
			get
			{
				return this.m_UseModernHangulLineBreakingRules;
			}
			set
			{
				this.m_UseModernHangulLineBreakingRules = value;
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00025008 File Offset: 0x00023208
		internal void LoadLineBreakingRules()
		{
			bool flag = this.m_LeadingCharactersLookup == null;
			if (flag)
			{
				bool flag2 = this.m_LeadingCharacters == null;
				if (flag2)
				{
					this.m_LeadingCharacters = Resources.Load<TextAsset>("LineBreaking Leading Characters");
				}
				this.m_LeadingCharactersLookup = ((this.m_LeadingCharacters != null) ? UnicodeLineBreakingRules.GetCharacters(this.m_LeadingCharacters) : new HashSet<uint>());
				bool flag3 = this.m_FollowingCharacters == null;
				if (flag3)
				{
					this.m_FollowingCharacters = Resources.Load<TextAsset>("LineBreaking Following Characters");
				}
				this.m_FollowingCharactersLookup = ((this.m_FollowingCharacters != null) ? UnicodeLineBreakingRules.GetCharacters(this.m_FollowingCharacters) : new HashSet<uint>());
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x000250B4 File Offset: 0x000232B4
		internal void LoadLineBreakingRules(TextAsset leadingRules, TextAsset followingRules)
		{
			bool flag = this.m_LeadingCharactersLookup == null;
			if (flag)
			{
				bool flag2 = leadingRules == null;
				if (flag2)
				{
					leadingRules = Resources.Load<TextAsset>("LineBreaking Leading Characters");
				}
				this.m_LeadingCharactersLookup = ((leadingRules != null) ? UnicodeLineBreakingRules.GetCharacters(leadingRules) : new HashSet<uint>());
				bool flag3 = followingRules == null;
				if (flag3)
				{
					followingRules = Resources.Load<TextAsset>("LineBreaking Following Characters");
				}
				this.m_FollowingCharactersLookup = ((followingRules != null) ? UnicodeLineBreakingRules.GetCharacters(followingRules) : new HashSet<uint>());
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00025138 File Offset: 0x00023338
		private static HashSet<uint> GetCharacters(TextAsset file)
		{
			HashSet<uint> hashSet = new HashSet<uint>();
			string text = file.text;
			for (int i = 0; i < text.Length; i++)
			{
				hashSet.Add((uint)text[i]);
			}
			return hashSet;
		}

		// Token: 0x04000411 RID: 1041
		[SerializeField]
		private TextAsset m_UnicodeLineBreakingRules;

		// Token: 0x04000412 RID: 1042
		[SerializeField]
		private TextAsset m_LeadingCharacters;

		// Token: 0x04000413 RID: 1043
		[SerializeField]
		private TextAsset m_FollowingCharacters;

		// Token: 0x04000414 RID: 1044
		[SerializeField]
		private bool m_UseModernHangulLineBreakingRules;

		// Token: 0x04000415 RID: 1045
		private HashSet<uint> m_LeadingCharactersLookup;

		// Token: 0x04000416 RID: 1046
		private HashSet<uint> m_FollowingCharactersLookup;
	}
}
