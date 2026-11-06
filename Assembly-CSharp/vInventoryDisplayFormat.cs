using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Invector.vItemManager;

// Token: 0x0200012E RID: 302
public static class vInventoryDisplayFormat
{
	// Token: 0x06000A11 RID: 2577 RVA: 0x00045C84 File Offset: 0x00043E84
	public static string DisplayFormat(this vItemType value)
	{
		if (vInventoryDisplayFormat.ItemTypeFormats.Count == 0)
		{
			foreach (vItemType vItemType in Enum.GetValues(typeof(vItemType)).OfType<vItemType>().ToArray<vItemType>())
			{
				vInventoryDisplayFormat.ItemTypeFormats.Add(vItemType.GetDisplayFormat<vItemType>());
			}
		}
		return vInventoryDisplayFormat.ItemTypeFormats[(int)value];
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x00045CE4 File Offset: 0x00043EE4
	public static string DisplayFormat(this vItemAttributes value)
	{
		if (vInventoryDisplayFormat.ItemAttributeFormats.Count == 0)
		{
			foreach (vItemAttributes vItemAttributes in Enum.GetValues(typeof(vItemAttributes)).OfType<vItemAttributes>().ToArray<vItemAttributes>())
			{
				vInventoryDisplayFormat.ItemAttributeFormats.Add(vItemAttributes.GetDisplayFormat<vItemAttributes>());
			}
		}
		return vInventoryDisplayFormat.ItemAttributeFormats[(int)value];
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x00045D44 File Offset: 0x00043F44
	private static string GetDisplayFormat<T>(this T value) where T : Enum
	{
		MemberInfo memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault<MemberInfo>();
		string text;
		if (memberInfo == null)
		{
			text = null;
		}
		else
		{
			DescriptionAttribute customAttribute = memberInfo.GetCustomAttribute<DescriptionAttribute>();
			text = ((customAttribute != null) ? customAttribute.Description : null);
		}
		return text ?? value.ToString();
	}

	// Token: 0x0400094A RID: 2378
	private static readonly List<string> ItemTypeFormats = new List<string>();

	// Token: 0x0400094B RID: 2379
	private static readonly List<string> ItemAttributeFormats = new List<string>();
}
