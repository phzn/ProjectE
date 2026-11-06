using System;

namespace Unity.Entities
{
	// Token: 0x0200028C RID: 652
	internal struct SortingUtilities
	{
		// Token: 0x0600115B RID: 4443 RVA: 0x00049474 File Offset: 0x00047674
		public unsafe static void InsertSorted(ComponentType* data, int length, ComponentType newValue)
		{
			while (length > 0 && newValue < data[length - 1])
			{
				data[length] = data[length - 1];
				length--;
			}
			data[length] = newValue;
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x000494DC File Offset: 0x000476DC
		public unsafe static void InsertSorted(ComponentTypeInArchetype* data, int length, ComponentType newValue)
		{
			ComponentTypeInArchetype componentTypeInArchetype = new ComponentTypeInArchetype(newValue);
			while (length > 0 && componentTypeInArchetype < data[length - 1])
			{
				data[length] = data[length - 1];
				length--;
			}
			data[length] = componentTypeInArchetype;
		}
	}
}
