using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000DA RID: 218
	public struct MMGameEvent
	{
		// Token: 0x060004BB RID: 1211 RVA: 0x000164AC File Offset: 0x000146AC
		public static void Trigger(string eventName, int intParameter = 0, Vector2 vector2Parameter = default(Vector2), Vector3 vector3Parameter = default(Vector3), bool boolParameter = false, string stringParameter = "")
		{
			MMGameEvent.e.EventName = eventName;
			MMGameEvent.e.IntParameter = intParameter;
			MMGameEvent.e.Vector2Parameter = vector2Parameter;
			MMGameEvent.e.Vector3Parameter = vector3Parameter;
			MMGameEvent.e.BoolParameter = boolParameter;
			MMGameEvent.e.StringParameter = stringParameter;
			MMEventManager.TriggerEvent<MMGameEvent>(MMGameEvent.e);
		}

		// Token: 0x0400062E RID: 1582
		private static MMGameEvent e;

		// Token: 0x0400062F RID: 1583
		public string EventName;

		// Token: 0x04000630 RID: 1584
		public int IntParameter;

		// Token: 0x04000631 RID: 1585
		public Vector2 Vector2Parameter;

		// Token: 0x04000632 RID: 1586
		public Vector3 Vector3Parameter;

		// Token: 0x04000633 RID: 1587
		public bool BoolParameter;

		// Token: 0x04000634 RID: 1588
		public string StringParameter;
	}
}
