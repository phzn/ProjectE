using System;
using System.Runtime.Serialization;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000857 RID: 2135
	[Serializable]
	public sealed class SwitchExpressionException : InvalidOperationException
	{
		// Token: 0x0600472A RID: 18218 RVA: 0x000E7F85 File Offset: 0x000E6185
		public SwitchExpressionException()
			: base("Non-exhaustive switch expression failed to match its input.")
		{
		}

		// Token: 0x0600472B RID: 18219 RVA: 0x000E7F92 File Offset: 0x000E6192
		public SwitchExpressionException(Exception innerException)
			: base("Non-exhaustive switch expression failed to match its input.", innerException)
		{
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x000E7FA0 File Offset: 0x000E61A0
		public SwitchExpressionException(object unmatchedValue)
			: this()
		{
			this.UnmatchedValue = unmatchedValue;
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x000E7FAF File Offset: 0x000E61AF
		private SwitchExpressionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.UnmatchedValue = info.GetValue("UnmatchedValue", typeof(object));
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x000E7FD4 File Offset: 0x000E61D4
		public SwitchExpressionException(string message)
			: base(message)
		{
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x000E7FDD File Offset: 0x000E61DD
		public SwitchExpressionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06004730 RID: 18224 RVA: 0x000E7FE7 File Offset: 0x000E61E7
		public object UnmatchedValue { get; }

		// Token: 0x06004731 RID: 18225 RVA: 0x000E7FEF File Offset: 0x000E61EF
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("UnmatchedValue", this.UnmatchedValue, typeof(object));
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06004732 RID: 18226 RVA: 0x000E8014 File Offset: 0x000E6214
		public override string Message
		{
			get
			{
				if (this.UnmatchedValue == null)
				{
					return base.Message;
				}
				string text = SR.Format("Unmatched value was {0}.", this.UnmatchedValue.ToString());
				return base.Message + Environment.NewLine + text;
			}
		}
	}
}
