using System;

namespace UnityEngine.Localization.SmartFormat.Core.Extensions
{
	// Token: 0x0200008F RID: 143
	[Serializable]
	public abstract class FormatterBase : IFormatter, ISerializationCallbackReceiver
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x0000E759 File Offset: 0x0000C959
		// (set) Token: 0x060003E5 RID: 997 RVA: 0x0000E761 File Offset: 0x0000C961
		public string[] Names
		{
			get
			{
				return this.m_Names;
			}
			set
			{
				this.m_Names = value;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060003E6 RID: 998
		public abstract string[] DefaultNames { get; }

		// Token: 0x060003E7 RID: 999
		public abstract bool TryEvaluateFormat(IFormattingInfo formattingInfo);

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000E76A File Offset: 0x0000C96A
		public virtual void OnAfterDeserialize()
		{
			if (this.Names == null || this.Names.Length == 0)
			{
				this.Names = this.DefaultNames;
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000E789 File Offset: 0x0000C989
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x0400011F RID: 287
		[SerializeField]
		private string[] m_Names;
	}
}
