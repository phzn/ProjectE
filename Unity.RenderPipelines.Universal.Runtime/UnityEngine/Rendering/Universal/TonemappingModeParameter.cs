using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000A7 RID: 167
	[Serializable]
	public sealed class TonemappingModeParameter : VolumeParameter<TonemappingMode>
	{
		// Token: 0x0600058F RID: 1423 RVA: 0x00020248 File Offset: 0x0001E448
		public TonemappingModeParameter(TonemappingMode value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}
}
