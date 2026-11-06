using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000C7 RID: 199
	public class TransformRecord : IValid, ITransform
	{
		// Token: 0x0600037D RID: 893 RVA: 0x00025128 File Offset: 0x00023328
		public TransformRecord(Transform t, bool read)
		{
			if (t)
			{
				this.transform = t;
				this.id = t.GetInstanceID();
				if (read)
				{
					this.localPosition = t.localPosition;
					this.localRotation = t.localRotation;
					this.position = t.position;
					this.rotation = t.rotation;
					this.scale = t.lossyScale;
					this.localToWorldMatrix = t.localToWorldMatrix;
					this.worldToLocalMatrix = t.worldToLocalMatrix;
				}
				if (t.parent)
				{
					this.pid = t.parent.GetInstanceID();
				}
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x000251CE File Offset: 0x000233CE
		public Vector3 InverseTransformDirection(Vector3 dir)
		{
			return Quaternion.Inverse(this.rotation) * dir;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000251E1 File Offset: 0x000233E1
		public bool IsValid()
		{
			return this.id != 0;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000251EC File Offset: 0x000233EC
		public void GetUsedTransform(HashSet<Transform> transformSet)
		{
			transformSet.Add(this.transform);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000251FC File Offset: 0x000233FC
		public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
		{
			if (replaceDict.ContainsKey(this.id))
			{
				Transform transform = replaceDict[this.id];
				this.transform = transform;
				this.id = this.transform.GetInstanceID();
			}
			if (replaceDict.ContainsKey(this.pid))
			{
				Transform transform2 = replaceDict[this.pid];
				this.pid = transform2.GetInstanceID();
			}
		}

		// Token: 0x0400076A RID: 1898
		public Transform transform;

		// Token: 0x0400076B RID: 1899
		public int id;

		// Token: 0x0400076C RID: 1900
		public Vector3 localPosition;

		// Token: 0x0400076D RID: 1901
		public Quaternion localRotation;

		// Token: 0x0400076E RID: 1902
		public Vector3 position;

		// Token: 0x0400076F RID: 1903
		public Quaternion rotation;

		// Token: 0x04000770 RID: 1904
		public Vector3 scale;

		// Token: 0x04000771 RID: 1905
		public Matrix4x4 localToWorldMatrix;

		// Token: 0x04000772 RID: 1906
		public Matrix4x4 worldToLocalMatrix;

		// Token: 0x04000773 RID: 1907
		public int pid;
	}
}
