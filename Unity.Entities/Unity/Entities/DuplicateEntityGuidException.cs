using System;

namespace Unity.Entities
{
	// Token: 0x02000086 RID: 134
	[Serializable]
	internal class DuplicateEntityGuidException : Exception
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000DB24 File Offset: 0x0000BD24
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x0000DB2C File Offset: 0x0000BD2C
		public DuplicateEntityGuid[] DuplicateEntityGuids { get; private set; }

		// Token: 0x060002E1 RID: 737 RVA: 0x0000DB35 File Offset: 0x0000BD35
		public DuplicateEntityGuidException(DuplicateEntityGuid[] duplicates)
			: base(DuplicateEntityGuidException.CreateMessage(duplicates))
		{
			this.DuplicateEntityGuids = duplicates;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000DB4A File Offset: 0x0000BD4A
		private static string CreateMessage(DuplicateEntityGuid[] duplicates)
		{
			return string.Format("Found {0} {1} components that are shared by more than one Entity", duplicates.Length, "EntityGuid") + "; see $exception.DuplicateEntityGuids for more information.";
		}
	}
}
