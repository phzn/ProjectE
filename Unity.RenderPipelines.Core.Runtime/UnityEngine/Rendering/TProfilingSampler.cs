using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x02000077 RID: 119
	internal class TProfilingSampler<TEnum> : ProfilingSampler where TEnum : Enum
	{
		// Token: 0x060003B2 RID: 946 RVA: 0x000104B0 File Offset: 0x0000E6B0
		static TProfilingSampler()
		{
			string[] names = Enum.GetNames(typeof(TEnum));
			Array values = Enum.GetValues(typeof(TEnum));
			for (int i = 0; i < names.Length; i++)
			{
				TProfilingSampler<TEnum> tprofilingSampler = new TProfilingSampler<TEnum>(names[i]);
				TProfilingSampler<TEnum>.samples.Add((TEnum)((object)values.GetValue(i)), tprofilingSampler);
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00010515 File Offset: 0x0000E715
		public TProfilingSampler(string name)
			: base(name)
		{
		}

		// Token: 0x0400020F RID: 527
		internal static Dictionary<TEnum, TProfilingSampler<TEnum>> samples = new Dictionary<TEnum, TProfilingSampler<TEnum>>();
	}
}
