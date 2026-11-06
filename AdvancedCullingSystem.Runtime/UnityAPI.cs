using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace NGS.AdvancedCullingSystem
{
	// Token: 0x02000009 RID: 9
	public static class UnityAPI
	{
		// Token: 0x06000027 RID: 39 RVA: 0x00002AB4 File Offset: 0x00000CB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] FindObjectsOfType<T>() where T : Object
		{
			return Object.FindObjectsByType<T>(FindObjectsSortMode.None);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002ABC File Offset: 0x00000CBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T FindObjectOfType<T>() where T : Object
		{
			return Object.FindAnyObjectByType<T>();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002AC3 File Offset: 0x00000CC3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RaycastCommand NewRaycastCommand(Vector3 origin, Vector3 direction, float distance = 5000f, int layerMask = -1)
		{
			return new RaycastCommand(origin, direction, new QueryParameters(layerMask, false, QueryTriggerInteraction.UseGlobal, false), distance);
		}
	}
}
