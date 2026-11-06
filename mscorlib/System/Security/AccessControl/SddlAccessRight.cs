using System;
using System.Collections.Generic;

namespace System.Security.AccessControl
{
	// Token: 0x02000549 RID: 1353
	internal class SddlAccessRight
	{
		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x0600358A RID: 13706 RVA: 0x000C1875 File Offset: 0x000BFA75
		// (set) Token: 0x0600358B RID: 13707 RVA: 0x000C187D File Offset: 0x000BFA7D
		public string Name { get; set; }

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x0600358C RID: 13708 RVA: 0x000C1886 File Offset: 0x000BFA86
		// (set) Token: 0x0600358D RID: 13709 RVA: 0x000C188E File Offset: 0x000BFA8E
		public int Value { get; set; }

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x0600358E RID: 13710 RVA: 0x000C1897 File Offset: 0x000BFA97
		// (set) Token: 0x0600358F RID: 13711 RVA: 0x000C189F File Offset: 0x000BFA9F
		public int ObjectType { get; set; }

		// Token: 0x06003590 RID: 13712 RVA: 0x000C18A8 File Offset: 0x000BFAA8
		public static SddlAccessRight LookupByName(string s)
		{
			foreach (SddlAccessRight sddlAccessRight in SddlAccessRight.rights)
			{
				if (sddlAccessRight.Name == s)
				{
					return sddlAccessRight;
				}
			}
			return null;
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x000C18E0 File Offset: 0x000BFAE0
		public static SddlAccessRight[] Decompose(int mask)
		{
			foreach (SddlAccessRight sddlAccessRight in SddlAccessRight.rights)
			{
				if (mask == sddlAccessRight.Value)
				{
					return new SddlAccessRight[] { sddlAccessRight };
				}
			}
			int num = 0;
			List<SddlAccessRight> list = new List<SddlAccessRight>();
			int num2 = 0;
			foreach (SddlAccessRight sddlAccessRight2 in SddlAccessRight.rights)
			{
				if ((mask & sddlAccessRight2.Value) == sddlAccessRight2.Value && (num2 | sddlAccessRight2.Value) != num2)
				{
					if (num == 0)
					{
						num = sddlAccessRight2.ObjectType;
					}
					if (sddlAccessRight2.ObjectType != 0 && num != sddlAccessRight2.ObjectType)
					{
						return null;
					}
					list.Add(sddlAccessRight2);
					num2 |= sddlAccessRight2.Value;
				}
				if (num2 == mask)
				{
					return list.ToArray();
				}
			}
			return null;
		}

		// Token: 0x040024FD RID: 9469
		private static readonly SddlAccessRight[] rights = new SddlAccessRight[]
		{
			new SddlAccessRight
			{
				Name = "CC",
				Value = 1,
				ObjectType = 1
			},
			new SddlAccessRight
			{
				Name = "DC",
				Value = 2,
				ObjectType = 1
			},
			new SddlAccessRight
			{
				Name = "LC",
				Value = 4,
				ObjectType = 1
			},
			new SddlAccessRight
			{
				Name = "SW",
				Value = 8,
				ObjectType = 1
			},
			new SddlAccessRight
			{
				Name = "RP",
				Value = 16,
				ObjectType = 1
			},
			new SddlAccessRight
			{
				Name = "WP",
				Value = 32,
				ObjectType = 1
			},
			new SddlAccessRight
			{
				Name = "DT",
				Value = 64,
				ObjectType = 1
			},
			new SddlAccessRight
			{
				Name = "LO",
				Value = 128,
				ObjectType = 1
			},
			new SddlAccessRight
			{
				Name = "CR",
				Value = 256,
				ObjectType = 1
			},
			new SddlAccessRight
			{
				Name = "SD",
				Value = 65536
			},
			new SddlAccessRight
			{
				Name = "RC",
				Value = 131072
			},
			new SddlAccessRight
			{
				Name = "WD",
				Value = 262144
			},
			new SddlAccessRight
			{
				Name = "WO",
				Value = 524288
			},
			new SddlAccessRight
			{
				Name = "GA",
				Value = 268435456
			},
			new SddlAccessRight
			{
				Name = "GX",
				Value = 536870912
			},
			new SddlAccessRight
			{
				Name = "GW",
				Value = 1073741824
			},
			new SddlAccessRight
			{
				Name = "GR",
				Value = int.MinValue
			},
			new SddlAccessRight
			{
				Name = "FA",
				Value = 2032127,
				ObjectType = 2
			},
			new SddlAccessRight
			{
				Name = "FR",
				Value = 1179785,
				ObjectType = 2
			},
			new SddlAccessRight
			{
				Name = "FW",
				Value = 1179926,
				ObjectType = 2
			},
			new SddlAccessRight
			{
				Name = "FX",
				Value = 1179808,
				ObjectType = 2
			},
			new SddlAccessRight
			{
				Name = "KA",
				Value = 983103,
				ObjectType = 3
			},
			new SddlAccessRight
			{
				Name = "KR",
				Value = 131097,
				ObjectType = 3
			},
			new SddlAccessRight
			{
				Name = "KW",
				Value = 131078,
				ObjectType = 3
			},
			new SddlAccessRight
			{
				Name = "KX",
				Value = 131097,
				ObjectType = 3
			},
			new SddlAccessRight
			{
				Name = "NW",
				Value = 1
			},
			new SddlAccessRight
			{
				Name = "NR",
				Value = 2
			},
			new SddlAccessRight
			{
				Name = "NX",
				Value = 4
			}
		};
	}
}
