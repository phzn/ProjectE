using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x0200004E RID: 78
	[Serializable]
	public class TextStyle
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0002469C File Offset: 0x0002289C
		public static TextStyle NormalStyle
		{
			get
			{
				bool flag = TextStyle.k_NormalStyle == null;
				if (flag)
				{
					TextStyle.k_NormalStyle = new TextStyle("Normal", string.Empty, string.Empty);
				}
				return TextStyle.k_NormalStyle;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000249 RID: 585 RVA: 0x000246D8 File Offset: 0x000228D8
		// (set) Token: 0x0600024A RID: 586 RVA: 0x000246F0 File Offset: 0x000228F0
		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				bool flag = value != this.m_Name;
				if (flag)
				{
					this.m_Name = value;
				}
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600024B RID: 587 RVA: 0x00024718 File Offset: 0x00022918
		// (set) Token: 0x0600024C RID: 588 RVA: 0x00024730 File Offset: 0x00022930
		public int hashCode
		{
			get
			{
				return this.m_HashCode;
			}
			set
			{
				bool flag = value != this.m_HashCode;
				if (flag)
				{
					this.m_HashCode = value;
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00024758 File Offset: 0x00022958
		public string styleOpeningDefinition
		{
			get
			{
				return this.m_OpeningDefinition;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00024770 File Offset: 0x00022970
		public string styleClosingDefinition
		{
			get
			{
				return this.m_ClosingDefinition;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00024788 File Offset: 0x00022988
		public uint[] styleOpeningTagArray
		{
			get
			{
				return this.m_OpeningTagArray;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000250 RID: 592 RVA: 0x000247A0 File Offset: 0x000229A0
		public uint[] styleClosingTagArray
		{
			get
			{
				return this.m_ClosingTagArray;
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x000247B8 File Offset: 0x000229B8
		internal TextStyle(string styleName, string styleOpeningDefinition, string styleClosingDefinition)
		{
			this.m_Name = styleName;
			this.m_HashCode = TextUtilities.GetHashCodeCaseInSensitive(styleName);
			this.m_OpeningDefinition = styleOpeningDefinition;
			this.m_ClosingDefinition = styleClosingDefinition;
			this.RefreshStyle();
		}

		// Token: 0x06000252 RID: 594 RVA: 0x000247EC File Offset: 0x000229EC
		public void RefreshStyle()
		{
			this.m_HashCode = TextUtilities.GetHashCodeCaseInSensitive(this.m_Name);
			int length = this.m_OpeningDefinition.Length;
			this.m_OpeningTagArray = new uint[length];
			this.m_OpeningTagUnicodeArray = new uint[length];
			for (int i = 0; i < length; i++)
			{
				this.m_OpeningTagArray[i] = (uint)this.m_OpeningDefinition[i];
				this.m_OpeningTagUnicodeArray[i] = (uint)this.m_OpeningDefinition[i];
			}
			int length2 = this.m_ClosingDefinition.Length;
			this.m_ClosingTagArray = new uint[length2];
			this.m_ClosingTagUnicodeArray = new uint[length2];
			for (int j = 0; j < length2; j++)
			{
				this.m_ClosingTagArray[j] = (uint)this.m_ClosingDefinition[j];
				this.m_ClosingTagUnicodeArray[j] = (uint)this.m_ClosingDefinition[j];
			}
		}

		// Token: 0x04000404 RID: 1028
		internal static TextStyle k_NormalStyle;

		// Token: 0x04000405 RID: 1029
		[SerializeField]
		private string m_Name;

		// Token: 0x04000406 RID: 1030
		[SerializeField]
		private int m_HashCode;

		// Token: 0x04000407 RID: 1031
		[SerializeField]
		private string m_OpeningDefinition;

		// Token: 0x04000408 RID: 1032
		[SerializeField]
		private string m_ClosingDefinition;

		// Token: 0x04000409 RID: 1033
		[SerializeField]
		private uint[] m_OpeningTagArray;

		// Token: 0x0400040A RID: 1034
		[SerializeField]
		private uint[] m_ClosingTagArray;

		// Token: 0x0400040B RID: 1035
		[SerializeField]
		internal uint[] m_OpeningTagUnicodeArray;

		// Token: 0x0400040C RID: 1036
		[SerializeField]
		internal uint[] m_ClosingTagUnicodeArray;
	}
}
