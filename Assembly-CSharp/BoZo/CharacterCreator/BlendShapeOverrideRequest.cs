using System;

namespace BoZo.CharacterCreator
{
	// Token: 0x0200079A RID: 1946
	public class BlendShapeOverrideRequest
	{
		// Token: 0x06003360 RID: 13152 RVA: 0x00123F9B File Offset: 0x0012219B
		public BlendShapeOverrideRequest(string sourceId, BlendShapePriority priority, float value)
		{
			this.SourceId = sourceId;
			this.Priority = priority;
			this.Value = value;
			this.IsActive = true;
		}

		// Token: 0x0400340C RID: 13324
		public string SourceId;

		// Token: 0x0400340D RID: 13325
		public BlendShapePriority Priority;

		// Token: 0x0400340E RID: 13326
		public float Value;

		// Token: 0x0400340F RID: 13327
		public bool IsActive;
	}
}
