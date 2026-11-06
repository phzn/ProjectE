using System;

namespace UnityEngine.Rendering.Universal.LibTessDotNet
{
	// Token: 0x02000114 RID: 276
	internal struct ContourVertex
	{
		// Token: 0x060008C5 RID: 2245 RVA: 0x00038718 File Offset: 0x00036918
		public override string ToString()
		{
			return string.Format("{0}, {1}", this.Position, this.Data);
		}

		// Token: 0x040007F3 RID: 2035
		public Vec3 Position;

		// Token: 0x040007F4 RID: 2036
		public object Data;
	}
}
