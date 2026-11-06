using System;
using UnityEngine;

namespace DA_Assets.DAI
{
	// Token: 0x0200003B RID: 59
	public static class MonoBehaviourLinkerExtensions
	{
		// Token: 0x06000127 RID: 295 RVA: 0x00006108 File Offset: 0x00004308
		public static T2 Link<T2, T3>(this T3 monoBeh, ref T2 linker) where T2 : MonoBehaviourLinkerRuntime<T3> where T3 : Object
		{
			bool flag = false;
			bool flag2 = false;
			if (linker == null)
			{
				flag = true;
				flag2 = true;
				AttributeValidator.Validate<T2>(new Type[] { typeof(SerializableAttribute) });
				linker = (T2)((object)Activator.CreateInstance(typeof(T2)));
			}
			if (linker.MonoBeh == null)
			{
				flag = true;
				linker.MonoBeh = monoBeh;
			}
			if (flag2)
			{
				linker.OnLink();
			}
			if (flag)
			{
				linker.SetDirty();
			}
			return linker;
		}
	}
}
