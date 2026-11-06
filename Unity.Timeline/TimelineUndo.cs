using System;
using System.Diagnostics;

namespace UnityEngine.Timeline
{
	// Token: 0x02000052 RID: 82
	internal static class TimelineUndo
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000A951 File Offset: 0x00008B51
		internal static bool undoEnabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000A954 File Offset: 0x00008B54
		public static void PushDestroyUndo(TimelineAsset timeline, Object thingToDirty, Object objectToDestroy)
		{
			if (objectToDestroy != null)
			{
				Object.Destroy(objectToDestroy);
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000A965 File Offset: 0x00008B65
		[Conditional("UNITY_EDITOR")]
		public static void PushUndo(Object[] thingsToDirty, string operation)
		{
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000A967 File Offset: 0x00008B67
		[Conditional("UNITY_EDITOR")]
		public static void PushUndo(Object thingToDirty, string operation)
		{
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000A969 File Offset: 0x00008B69
		[Conditional("UNITY_EDITOR")]
		public static void RegisterCreatedObjectUndo(Object thingCreated, string operation)
		{
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000A96B File Offset: 0x00008B6B
		internal static string UndoName(string name)
		{
			return "Timeline " + name;
		}
	}
}
