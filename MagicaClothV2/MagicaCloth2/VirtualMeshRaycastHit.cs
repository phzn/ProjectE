using System;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x02000161 RID: 353
	public struct VirtualMeshRaycastHit : IComparable<VirtualMeshRaycastHit>, IValid
	{
		// Token: 0x06000698 RID: 1688 RVA: 0x00039B9D File Offset: 0x00037D9D
		public int CompareTo(VirtualMeshRaycastHit other)
		{
			if (this.distance == other.distance)
			{
				return 0;
			}
			if (this.distance >= other.distance)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00039BC0 File Offset: 0x00037DC0
		public bool IsValid()
		{
			return this.type > VirtualMeshPrimitive.None;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00039BCC File Offset: 0x00037DCC
		public override string ToString()
		{
			return string.Format("{0} [{1}] pos:{2}, dist:{3}, nor:{4}", new object[] { this.type, this.index, this.position, this.distance, this.normal });
		}

		// Token: 0x04000A97 RID: 2711
		public VirtualMeshPrimitive type;

		// Token: 0x04000A98 RID: 2712
		public int index;

		// Token: 0x04000A99 RID: 2713
		public float3 position;

		// Token: 0x04000A9A RID: 2714
		public float3 normal;

		// Token: 0x04000A9B RID: 2715
		public float distance;
	}
}
