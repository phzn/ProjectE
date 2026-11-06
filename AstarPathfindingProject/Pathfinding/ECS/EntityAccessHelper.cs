using System;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

namespace Pathfinding.ECS
{
	// Token: 0x0200030E RID: 782
	internal static class EntityAccessHelper
	{
		// Token: 0x04000E06 RID: 3590
		public static readonly int GlobalSystemVersionOffset = UnsafeUtility.GetFieldOffset(typeof(ComponentTypeHandle<int>).GetField("m_GlobalSystemVersion", BindingFlags.Instance | BindingFlags.NonPublic));
	}
}
