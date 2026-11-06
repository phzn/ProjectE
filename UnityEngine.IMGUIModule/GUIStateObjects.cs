using System;
using System.Collections.Generic;
using System.Security;

namespace UnityEngine
{
	// Token: 0x02000029 RID: 41
	internal class GUIStateObjects
	{
		// Token: 0x060002DD RID: 733 RVA: 0x0000B880 File Offset: 0x00009A80
		[SecuritySafeCritical]
		internal static object GetStateObject(Type t, int controlID)
		{
			object obj;
			bool flag = !GUIStateObjects.s_StateCache.TryGetValue(controlID, out obj) || obj.GetType() != t;
			if (flag)
			{
				obj = Activator.CreateInstance(t);
				GUIStateObjects.s_StateCache[controlID] = obj;
			}
			return obj;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000B8CC File Offset: 0x00009ACC
		internal static object QueryStateObject(Type t, int controlID)
		{
			object obj = GUIStateObjects.s_StateCache[controlID];
			bool flag = t.IsInstanceOfType(obj);
			object obj2;
			if (flag)
			{
				obj2 = obj;
			}
			else
			{
				obj2 = null;
			}
			return obj2;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000B8FB File Offset: 0x00009AFB
		internal static void Tests_ClearObjects()
		{
			GUIStateObjects.s_StateCache.Clear();
		}

		// Token: 0x040000C9 RID: 201
		private static Dictionary<int, object> s_StateCache = new Dictionary<int, object>();
	}
}
