using System;
using System.Collections.Generic;

namespace UnityEngine.Localization.SmartFormat.Core.Settings
{
	// Token: 0x0200007E RID: 126
	[Serializable]
	public class SmartSettings
	{
		// Token: 0x0600033E RID: 830 RVA: 0x0000C961 File Offset: 0x0000AB61
		internal SmartSettings()
		{
			this.CaseSensitivity = CaseSensitivityType.CaseSensitive;
			this.ConvertCharacterStringLiterals = true;
			this.FormatErrorAction = ErrorAction.ThrowError;
			this.ParseErrorAction = ErrorAction.ThrowError;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600033F RID: 831 RVA: 0x0000C98C File Offset: 0x0000AB8C
		// (set) Token: 0x06000340 RID: 832 RVA: 0x0000C994 File Offset: 0x0000AB94
		public ErrorAction FormatErrorAction
		{
			get
			{
				return this.m_FormatErrorAction;
			}
			set
			{
				this.m_FormatErrorAction = value;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000C99D File Offset: 0x0000AB9D
		// (set) Token: 0x06000342 RID: 834 RVA: 0x0000C9A5 File Offset: 0x0000ABA5
		public ErrorAction ParseErrorAction
		{
			get
			{
				return this.m_ParseErrorAction;
			}
			set
			{
				this.m_ParseErrorAction = value;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000C9AE File Offset: 0x0000ABAE
		// (set) Token: 0x06000344 RID: 836 RVA: 0x0000C9B6 File Offset: 0x0000ABB6
		public CaseSensitivityType CaseSensitivity
		{
			get
			{
				return this.m_CaseSensitivity;
			}
			set
			{
				this.m_CaseSensitivity = value;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000C9BF File Offset: 0x0000ABBF
		// (set) Token: 0x06000346 RID: 838 RVA: 0x0000C9C7 File Offset: 0x0000ABC7
		public bool ConvertCharacterStringLiterals
		{
			get
			{
				return this.m_ConvertCharacterStringLiterals;
			}
			set
			{
				this.m_ConvertCharacterStringLiterals = value;
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000C9D0 File Offset: 0x0000ABD0
		internal IEqualityComparer<string> GetCaseSensitivityComparer()
		{
			CaseSensitivityType caseSensitivity = this.CaseSensitivity;
			if (caseSensitivity == CaseSensitivityType.CaseSensitive)
			{
				return StringComparer.Ordinal;
			}
			if (caseSensitivity != CaseSensitivityType.CaseInsensitive)
			{
				throw new InvalidOperationException(string.Format("The case sensitivity type [{0}] is unknown.", this.CaseSensitivity));
			}
			return StringComparer.OrdinalIgnoreCase;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000CA14 File Offset: 0x0000AC14
		internal StringComparison GetCaseSensitivityComparison()
		{
			CaseSensitivityType caseSensitivity = this.CaseSensitivity;
			if (caseSensitivity == CaseSensitivityType.CaseSensitive)
			{
				return StringComparison.Ordinal;
			}
			if (caseSensitivity != CaseSensitivityType.CaseInsensitive)
			{
				throw new InvalidOperationException(string.Format("The case sensitivity type [{0}] is unknown.", this.CaseSensitivity));
			}
			return StringComparison.OrdinalIgnoreCase;
		}

		// Token: 0x040000DC RID: 220
		[SerializeField]
		private ErrorAction m_FormatErrorAction;

		// Token: 0x040000DD RID: 221
		[SerializeField]
		private ErrorAction m_ParseErrorAction;

		// Token: 0x040000DE RID: 222
		[Tooltip("Determines whether placeholders are case-sensitive or not.")]
		[SerializeField]
		private CaseSensitivityType m_CaseSensitivity;

		// Token: 0x040000DF RID: 223
		[Tooltip("This setting is relevant for the 'Parsing.LiteralText', If true (the default), character string literals are treated like in normal string.Format: string.Format(\"\t\") will return a \"TAB\" character If false, character string literals are not converted, just like with this string.Format: string.Format(@\"\t\") will return the 2 characters \"\" and \"t\"")]
		[SerializeField]
		private bool m_ConvertCharacterStringLiterals = true;
	}
}
