using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x02000131 RID: 305
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public interface IValueField<T>
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000A17 RID: 2583
		// (set) Token: 0x06000A18 RID: 2584
		T value { get; set; }

		// Token: 0x06000A19 RID: 2585
		void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, T startValue);

		// Token: 0x06000A1A RID: 2586
		void StartDragging();

		// Token: 0x06000A1B RID: 2587
		void StopDragging();
	}
}
