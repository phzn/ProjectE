using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000063 RID: 99
	[Serializable]
	public class CustomSkinningSettings : IValid, IDataValidate, ITransform
	{
		// Token: 0x0600015F RID: 351 RVA: 0x0000706D File Offset: 0x0000526D
		public void DataValidate()
		{
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000F00C File Offset: 0x0000D20C
		public bool IsValid()
		{
			if (!this.enable)
			{
				return false;
			}
			if (this.skinningBones.Count == 0)
			{
				return false;
			}
			return this.skinningBones.Any<Transform>((Transform n) => n != null);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000F061 File Offset: 0x0000D261
		public CustomSkinningSettings Clone()
		{
			return new CustomSkinningSettings
			{
				enable = this.enable,
				skinningBones = new List<Transform>(this.skinningBones)
			};
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00002137 File Offset: 0x00000337
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000F088 File Offset: 0x0000D288
		public void GetUsedTransform(HashSet<Transform> transformSet)
		{
			foreach (Transform transform in this.skinningBones)
			{
				if (transform)
				{
					transformSet.Add(transform);
				}
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000F0E4 File Offset: 0x0000D2E4
		public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
		{
			for (int i = 0; i < this.skinningBones.Count; i++)
			{
				Transform transform = this.skinningBones[i];
				if (transform && replaceDict.ContainsKey(transform.GetInstanceID()))
				{
					this.skinningBones[i] = replaceDict[transform.GetInstanceID()];
				}
			}
		}

		// Token: 0x0400023F RID: 575
		public bool enable;

		// Token: 0x04000240 RID: 576
		public List<Transform> skinningBones = new List<Transform>();
	}
}
