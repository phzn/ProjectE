using System;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000B03 RID: 2819
	public static class CustomizationSessionStore
	{
		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06004FC6 RID: 20422 RVA: 0x001EE746 File Offset: 0x001EC946
		public static bool HasPendingContext
		{
			get
			{
				return CustomizationSessionStore._pendingContext != null;
			}
		}

		// Token: 0x06004FC7 RID: 20423 RVA: 0x001EE750 File Offset: 0x001EC950
		public static void SetPendingContext(CustomizationSessionContext context)
		{
			CustomizationSessionStore._pendingContext = context;
		}

		// Token: 0x06004FC8 RID: 20424 RVA: 0x001EE758 File Offset: 0x001EC958
		public static CustomizationSessionContext PeekPendingContext()
		{
			return CustomizationSessionStore._pendingContext;
		}

		// Token: 0x06004FC9 RID: 20425 RVA: 0x001EE75F File Offset: 0x001EC95F
		public static CustomizationSessionContext ConsumePendingContext()
		{
			CustomizationSessionContext pendingContext = CustomizationSessionStore._pendingContext;
			CustomizationSessionStore._pendingContext = null;
			return pendingContext;
		}

		// Token: 0x06004FCA RID: 20426 RVA: 0x001EE76C File Offset: 0x001EC96C
		public static void Clear()
		{
			CustomizationSessionStore._pendingContext = null;
		}

		// Token: 0x040050C3 RID: 20675
		private static CustomizationSessionContext _pendingContext;
	}
}
