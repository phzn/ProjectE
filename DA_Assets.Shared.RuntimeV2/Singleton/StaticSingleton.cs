using System;

namespace DA_Assets.Singleton
{
	// Token: 0x02000009 RID: 9
	public class StaticSingleton<T>
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000025D4 File Offset: 0x000007D4
		public static T Instance
		{
			get
			{
				if (StaticSingleton<T>.instance == null)
				{
					StaticSingleton<T>.instance = Activator.CreateInstance<T>();
				}
				return StaticSingleton<T>.instance;
			}
		}

		// Token: 0x0400000A RID: 10
		private static T instance;
	}
}
