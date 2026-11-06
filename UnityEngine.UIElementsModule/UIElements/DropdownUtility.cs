using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000181 RID: 385
	internal static class DropdownUtility
	{
		// Token: 0x06000C43 RID: 3139 RVA: 0x000312A0 File Offset: 0x0002F4A0
		internal static IGenericMenu CreateDropdown()
		{
			IGenericMenu genericMenu2;
			if (DropdownUtility.MakeDropdownFunc == null)
			{
				IGenericMenu genericMenu = new GenericDropdownMenu();
				genericMenu2 = genericMenu;
			}
			else
			{
				genericMenu2 = DropdownUtility.MakeDropdownFunc();
			}
			return genericMenu2;
		}

		// Token: 0x040005D7 RID: 1495
		internal static Func<IGenericMenu> MakeDropdownFunc;
	}
}
