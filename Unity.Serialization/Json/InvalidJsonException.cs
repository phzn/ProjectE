using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Unity.Serialization.Json
{
	// Token: 0x02000072 RID: 114
	[Serializable]
	public class InvalidJsonException : Exception
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000BA65 File Offset: 0x00009C65
		// (set) Token: 0x060002A1 RID: 673 RVA: 0x0000BA6D File Offset: 0x00009C6D
		public int Line { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000BA76 File Offset: 0x00009C76
		// (set) Token: 0x060002A3 RID: 675 RVA: 0x0000BA7E File Offset: 0x00009C7E
		public int Character { get; set; }

		// Token: 0x060002A4 RID: 676 RVA: 0x0000BA87 File Offset: 0x00009C87
		internal InvalidJsonException(JsonValidationResult result)
			: base(result.ToString())
		{
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00002174 File Offset: 0x00000374
		public InvalidJsonException(string message)
			: base(message)
		{
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000217D File Offset: 0x0000037D
		public InvalidJsonException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000BA9C File Offset: 0x00009C9C
		protected InvalidJsonException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.Line = info.GetInt32("Line");
			this.Character = info.GetInt32("Character");
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000BAC8 File Offset: 0x00009CC8
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("Line", this.Line);
			info.AddValue("Character", this.Character);
			base.GetObjectData(info, context);
		}
	}
}
