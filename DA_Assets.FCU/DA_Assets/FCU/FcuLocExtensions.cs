using System;
using DA_Assets.Singleton;

namespace DA_Assets.FCU
{
	// Token: 0x02000022 RID: 34
	public static class FcuLocExtensions
	{
		// Token: 0x06000086 RID: 134 RVA: 0x000031F0 File Offset: 0x000013F0
		public static string Localize(this FcuLocKey key, params object[] args)
		{
			return SingletonScriptableObject<FcuConfig>.Instance.Localizator.GetLocalizedText(key, args);
		}
	}
}
