using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000C8 RID: 200
	[Serializable]
	public class TransformRecordSerializeData : ITransform
	{
		// Token: 0x06000382 RID: 898 RVA: 0x00025264 File Offset: 0x00023464
		public void Serialize(TransformRecord tr)
		{
			this.transform = tr.transform;
			this.localPosition = tr.localPosition;
			this.localRotation = tr.localRotation;
			this.position = tr.position;
			this.rotation = tr.rotation;
			this.scale = tr.scale;
			this.localToWorldMatrix = tr.localToWorldMatrix;
			this.worldToLocalMatrix = tr.worldToLocalMatrix;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x000252D4 File Offset: 0x000234D4
		public void Deserialize(TransformRecord tr)
		{
			tr.localPosition = this.localPosition;
			tr.localRotation = this.localRotation;
			tr.position = this.position;
			tr.rotation = this.rotation;
			tr.scale = this.scale;
			tr.localToWorldMatrix = this.localToWorldMatrix;
			tr.worldToLocalMatrix = this.worldToLocalMatrix;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00025338 File Offset: 0x00023538
		public int GetLocalHash()
		{
			int num = 0;
			if (this.transform)
			{
				num += 123 + this.transform.childCount * 345;
			}
			return num;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0002536C File Offset: 0x0002356C
		public int GetGlobalHash()
		{
			return 0 + this.localPosition.GetHashCode() + this.localRotation.GetHashCode();
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00025393 File Offset: 0x00023593
		public void GetUsedTransform(HashSet<Transform> transformSet)
		{
			if (this.transform)
			{
				transformSet.Add(this.transform);
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x000253B0 File Offset: 0x000235B0
		public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
		{
			int num = ((this.transform != null) ? this.transform.GetInstanceID() : 0);
			if (num != 0 && replaceDict.ContainsKey(num))
			{
				this.transform = replaceDict[num];
			}
		}

		// Token: 0x04000774 RID: 1908
		public Transform transform;

		// Token: 0x04000775 RID: 1909
		public Vector3 localPosition;

		// Token: 0x04000776 RID: 1910
		public Quaternion localRotation;

		// Token: 0x04000777 RID: 1911
		public Vector3 position;

		// Token: 0x04000778 RID: 1912
		public Quaternion rotation;

		// Token: 0x04000779 RID: 1913
		public Vector3 scale;

		// Token: 0x0400077A RID: 1914
		public Matrix4x4 localToWorldMatrix;

		// Token: 0x0400077B RID: 1915
		public Matrix4x4 worldToLocalMatrix;
	}
}
