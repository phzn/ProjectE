using System;

namespace Unity.Properties
{
	// Token: 0x02000018 RID: 24
	[Serializable]
	public class InvalidContainerTypeException : Exception
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002ED3 File Offset: 0x000010D3
		public Type Type { get; }

		// Token: 0x0600005A RID: 90 RVA: 0x00002EDB File Offset: 0x000010DB
		public InvalidContainerTypeException(Type type)
			: base(InvalidContainerTypeException.GetMessageForType(type))
		{
			this.Type = type;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002EF2 File Offset: 0x000010F2
		public InvalidContainerTypeException(Type type, Exception inner)
			: base(InvalidContainerTypeException.GetMessageForType(type), inner)
		{
			this.Type = type;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002F0C File Offset: 0x0000110C
		private static string GetMessageForType(Type type)
		{
			return string.Concat(new string[] { "Invalid container Type=[", type.Name, ".", type.Name, "]" });
		}
	}
}
