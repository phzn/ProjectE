using System;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x02000187 RID: 391
	internal sealed class EntityNameStorageDebugView
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0003111C File Offset: 0x0002F31C
		public FixedString64Bytes[] Table
		{
			get
			{
				FixedString64Bytes[] array = new FixedString64Bytes[EntityNameStorage.Entries];
				for (int i = 0; i < EntityNameStorage.Entries; i++)
				{
					EntityNameStorage.GetFixedString(i, ref array[i]);
				}
				return array;
			}
		}
	}
}
