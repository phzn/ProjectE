using System;

namespace Unity.Entities
{
	// Token: 0x02000167 RID: 359
	internal readonly struct ManagedEntityDataAccess
	{
		// Token: 0x060007CA RID: 1994 RVA: 0x0002A6B8 File Offset: 0x000288B8
		public static int AllocHandle(ManagedEntityDataAccess instance)
		{
			int num = ManagedEntityDataAccess.s_Instances.Length;
			for (int i = 0; i < num; i++)
			{
				if (ManagedEntityDataAccess.s_Instances[i].World == null)
				{
					ManagedEntityDataAccess.s_Instances[i] = instance;
					return i;
				}
			}
			ManagedEntityDataAccess[] array = new ManagedEntityDataAccess[num * 2];
			for (int j = 0; j < num; j++)
			{
				array[j] = ManagedEntityDataAccess.s_Instances[j];
			}
			array[num] = instance;
			ManagedEntityDataAccess.s_Instances = array;
			return num;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0002A739 File Offset: 0x00028939
		public static void FreeHandle(int i)
		{
			ManagedEntityDataAccess.s_Instances[i] = default(ManagedEntityDataAccess);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0002A74E File Offset: 0x0002894E
		public static ref ManagedEntityDataAccess GetInstance(int handle)
		{
			return ref ManagedEntityDataAccess.s_Instances[handle];
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0002A760 File Offset: 0x00028960
		public static ManagedEntityDataAccess Debugger_GetInstance(int handle)
		{
			if (handle >= 0 && handle < ManagedEntityDataAccess.s_Instances.Length)
			{
				return ManagedEntityDataAccess.s_Instances[handle];
			}
			return default(ManagedEntityDataAccess);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0002A794 File Offset: 0x00028994
		public ManagedEntityDataAccess(World world, ManagedComponentStore store)
		{
			this.World = world;
			this.ManagedComponentStore = store;
		}

		// Token: 0x0400044D RID: 1101
		private static volatile ManagedEntityDataAccess[] s_Instances = new ManagedEntityDataAccess[4];

		// Token: 0x0400044E RID: 1102
		public readonly World World;

		// Token: 0x0400044F RID: 1103
		public readonly ManagedComponentStore ManagedComponentStore;
	}
}
