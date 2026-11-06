using System;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x02000295 RID: 661
	public static class UnityCompatibility
	{
		// Token: 0x0600107F RID: 4223 RVA: 0x0006452A File Offset: 0x0006272A
		public static T[] FindObjectsByTypeSorted<T>() where T : Object
		{
			return Object.FindObjectsByType<T>(FindObjectsSortMode.InstanceID);
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00064532 File Offset: 0x00062732
		public static T[] FindObjectsByTypeUnsorted<T>() where T : Object
		{
			return Object.FindObjectsByType<T>(FindObjectsSortMode.None);
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0006453A File Offset: 0x0006273A
		public static T[] FindObjectsByTypeUnsortedWithInactive<T>() where T : Object
		{
			return Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00064543 File Offset: 0x00062743
		public static T FindAnyObjectByType<T>() where T : Object
		{
			return Object.FindAnyObjectByType<T>();
		}
	}
}
