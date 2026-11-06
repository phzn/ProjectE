using System;
using System.Collections.Generic;

namespace UnityEngine.Localization.Pseudo
{
	// Token: 0x020000AE RID: 174
	[Serializable]
	public class CharacterSubstitutor : IPseudoLocalizationMethod, ISerializationCallbackReceiver
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x0001135F File Offset: 0x0000F55F
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x00011367 File Offset: 0x0000F567
		public CharacterSubstitutor.SubstitutionMethod Method
		{
			get
			{
				return this.m_SubstitutionMethod;
			}
			set
			{
				this.m_SubstitutionMethod = value;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x00011370 File Offset: 0x0000F570
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x00011378 File Offset: 0x0000F578
		public Dictionary<char, char> ReplacementMap { get; private set; } = new Dictionary<char, char>();

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x00011381 File Offset: 0x0000F581
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x00011389 File Offset: 0x0000F589
		public CharacterSubstitutor.ListSelectionMethod ListMode
		{
			get
			{
				return this.m_ListMode;
			}
			set
			{
				this.m_ListMode = value;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00011392 File Offset: 0x0000F592
		public List<char> ReplacementList
		{
			get
			{
				return this.m_ReplacementList;
			}
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001139A File Offset: 0x0000F59A
		private int GetRandomSeed(string input)
		{
			return input.GetHashCode();
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x000113A4 File Offset: 0x0000F5A4
		internal char ReplaceCharFromMap(char value)
		{
			char c;
			if (this.ReplacementMap != null && this.ReplacementMap.TryGetValue(value, out c))
			{
				return c;
			}
			return value;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x000113CC File Offset: 0x0000F5CC
		public void OnBeforeSerialize()
		{
			if (this.m_ReplacementsMap == null)
			{
				this.m_ReplacementsMap = new List<CharacterSubstitutor.CharReplacement>();
			}
			this.m_ReplacementsMap.Clear();
			foreach (KeyValuePair<char, char> keyValuePair in this.ReplacementMap)
			{
				this.m_ReplacementsMap.Add(new CharacterSubstitutor.CharReplacement
				{
					original = keyValuePair.Key,
					replacement = keyValuePair.Value
				});
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00011468 File Offset: 0x0000F668
		public void OnAfterDeserialize()
		{
			if (this.ReplacementMap == null)
			{
				this.ReplacementMap = new Dictionary<char, char>();
			}
			this.ReplacementMap.Clear();
			foreach (CharacterSubstitutor.CharReplacement charReplacement in this.m_ReplacementsMap)
			{
				this.ReplacementMap[charReplacement.original] = charReplacement.replacement;
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x000114EC File Offset: 0x0000F6EC
		private void TransformFragment(WritableMessageFragment writableFragment)
		{
			switch (this.Method)
			{
			case CharacterSubstitutor.SubstitutionMethod.ToUpper:
				writableFragment.Text = writableFragment.Text.ToUpper();
				return;
			case CharacterSubstitutor.SubstitutionMethod.ToLower:
				writableFragment.Text = writableFragment.Text.ToLower();
				return;
			case CharacterSubstitutor.SubstitutionMethod.List:
				if (this.m_ReplacementList != null && this.m_ReplacementList.Count != 0)
				{
					if (this.m_ReplacementList.Count == 1)
					{
						writableFragment.Text = new string(this.m_ReplacementList[0], writableFragment.Length);
						return;
					}
					char[] array = new char[writableFragment.Length];
					if (this.ListMode == CharacterSubstitutor.ListSelectionMethod.Random)
					{
						Random.InitState(this.GetRandomSeed(writableFragment.Message.Original));
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = this.m_ReplacementList[Random.Range(0, this.m_ReplacementList.Count)];
						}
					}
					else
					{
						if (this.ListMode == CharacterSubstitutor.ListSelectionMethod.LoopFromStart)
						{
							this.m_ReplacementsPosition = 0;
						}
						int j = 0;
						while (j < array.Length)
						{
							array[j] = this.m_ReplacementList[this.m_ReplacementsPosition % this.m_ReplacementList.Count];
							j++;
							this.m_ReplacementsPosition++;
						}
					}
					writableFragment.Text = new string(array);
				}
				return;
			case CharacterSubstitutor.SubstitutionMethod.Map:
			{
				char[] array2 = new char[writableFragment.Length];
				for (int k = 0; k < array2.Length; k++)
				{
					array2[k] = this.ReplaceCharFromMap(writableFragment[k]);
				}
				writableFragment.Text = new string(array2);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00011674 File Offset: 0x0000F874
		public void Transform(Message message)
		{
			foreach (MessageFragment messageFragment in message.Fragments)
			{
				WritableMessageFragment writableMessageFragment = messageFragment as WritableMessageFragment;
				if (writableMessageFragment != null)
				{
					this.TransformFragment(writableMessageFragment);
				}
			}
		}

		// Token: 0x04000164 RID: 356
		[SerializeField]
		private CharacterSubstitutor.SubstitutionMethod m_SubstitutionMethod;

		// Token: 0x04000165 RID: 357
		[SerializeField]
		private CharacterSubstitutor.ListSelectionMethod m_ListMode;

		// Token: 0x04000166 RID: 358
		[SerializeField]
		private List<CharacterSubstitutor.CharReplacement> m_ReplacementsMap;

		// Token: 0x04000167 RID: 359
		[SerializeField]
		private List<char> m_ReplacementList = new List<char> { '_' };

		// Token: 0x04000168 RID: 360
		internal int m_ReplacementsPosition;

		// Token: 0x0200014D RID: 333
		public enum SubstitutionMethod
		{
			// Token: 0x040002F4 RID: 756
			ToUpper,
			// Token: 0x040002F5 RID: 757
			ToLower,
			// Token: 0x040002F6 RID: 758
			List,
			// Token: 0x040002F7 RID: 759
			Map
		}

		// Token: 0x0200014E RID: 334
		[Serializable]
		private struct CharReplacement
		{
			// Token: 0x040002F8 RID: 760
			public char original;

			// Token: 0x040002F9 RID: 761
			public char replacement;
		}

		// Token: 0x0200014F RID: 335
		public enum ListSelectionMethod
		{
			// Token: 0x040002FB RID: 763
			Random,
			// Token: 0x040002FC RID: 764
			LoopFromPrevious,
			// Token: 0x040002FD RID: 765
			LoopFromStart
		}
	}
}
