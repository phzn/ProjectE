using System;

namespace UnityEngine.AddressableAssets.Utility
{
	// Token: 0x0200001B RID: 27
	internal class AssetReferenceUtilities
	{
		// Token: 0x06000181 RID: 385 RVA: 0x00006C69 File Offset: 0x00004E69
		internal static string FormatName(string name)
		{
			if (name.EndsWith("(Clone)", StringComparison.Ordinal))
			{
				name = name.Replace("(Clone)", "");
			}
			return name;
		}
	}
}
