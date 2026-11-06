using System;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x0200015F RID: 351
	public class VirtualMeshContainer : IDisposable
	{
		// Token: 0x06000691 RID: 1681 RVA: 0x000020C3 File Offset: 0x000002C3
		public VirtualMeshContainer()
		{
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00039AE2 File Offset: 0x00037CE2
		public VirtualMeshContainer(VirtualMesh vmesh)
		{
			this.shareVirtualMesh = vmesh;
			this.uniqueData = null;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00039AF8 File Offset: 0x00037CF8
		public void Dispose()
		{
			VirtualMesh virtualMesh = this.shareVirtualMesh;
			if (virtualMesh == null)
			{
				return;
			}
			virtualMesh.Dispose();
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x00039B0A File Offset: 0x00037D0A
		public bool hasUniqueData
		{
			get
			{
				return this.uniqueData != null;
			}
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00039B15 File Offset: 0x00037D15
		public int GetTransformCount()
		{
			if (this.hasUniqueData)
			{
				return this.uniqueData.transformData.transformArray.Length;
			}
			return this.shareVirtualMesh.TransformCount;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00039B3D File Offset: 0x00037D3D
		public Transform GetTransformFromIndex(int index)
		{
			if (this.hasUniqueData)
			{
				return this.uniqueData.transformData.transformArray[index];
			}
			return this.shareVirtualMesh.transformData.GetTransformFromIndex(index);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00039B6B File Offset: 0x00037D6B
		public Transform GetCenterTransform()
		{
			if (this.hasUniqueData)
			{
				return this.uniqueData.transformData.transformArray[this.shareVirtualMesh.centerTransformIndex];
			}
			return this.shareVirtualMesh.GetCenterTransform();
		}

		// Token: 0x04000A90 RID: 2704
		public VirtualMesh shareVirtualMesh;

		// Token: 0x04000A91 RID: 2705
		public VirtualMesh.UniqueSerializationData uniqueData;
	}
}
