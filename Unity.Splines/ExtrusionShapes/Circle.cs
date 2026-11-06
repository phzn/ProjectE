using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.ExtrusionShapes
{
	// Token: 0x0200004D RID: 77
	[Serializable]
	public sealed class Circle : IExtrudeShape
	{
		// Token: 0x0600031F RID: 799 RVA: 0x000101E9 File Offset: 0x0000E3E9
		public void Setup(ISpline path, int segmentCount)
		{
			this.m_Rads = math.radians(360f / (float)this.SideCount);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00010203 File Offset: 0x0000E403
		public float2 GetPosition(float t, int index)
		{
			return new float2(math.cos((float)index * this.m_Rads), math.sin((float)index * this.m_Rads));
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000321 RID: 801 RVA: 0x00010226 File Offset: 0x0000E426
		// (set) Token: 0x06000322 RID: 802 RVA: 0x0001022E File Offset: 0x0000E42E
		public int SideCount
		{
			get
			{
				return this.m_Sides;
			}
			set
			{
				this.m_Sides = value;
			}
		}

		// Token: 0x040000FD RID: 253
		[SerializeField]
		[Min(2f)]
		private int m_Sides = 8;

		// Token: 0x040000FE RID: 254
		private float m_Rads;
	}
}
