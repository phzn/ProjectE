using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000067 RID: 103
	[Serializable]
	public class NormalAlignmentSettings : IValid, IDataValidate, ITransform
	{
		// Token: 0x0600018C RID: 396 RVA: 0x0000706D File Offset: 0x0000526D
		public void DataValidate()
		{
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000F9BC File Offset: 0x0000DBBC
		public bool IsValid()
		{
			NormalAlignmentSettings.AlignmentMode alignmentMode = this.alignmentMode;
			return alignmentMode <= NormalAlignmentSettings.AlignmentMode.Transform;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000F9D7 File Offset: 0x0000DBD7
		public NormalAlignmentSettings Clone()
		{
			return new NormalAlignmentSettings
			{
				alignmentMode = this.alignmentMode,
				adjustmentTransform = this.adjustmentTransform
			};
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000F9F8 File Offset: 0x0000DBF8
		public override int GetHashCode()
		{
			int num = 0;
			num = (int)(num + this.alignmentMode * (NormalAlignmentSettings.AlignmentMode)105);
			if (this.adjustmentTransform)
			{
				num += this.adjustmentTransform.GetInstanceID();
				num += this.adjustmentTransform.position.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000FA4B File Offset: 0x0000DC4B
		public void GetUsedTransform(HashSet<Transform> transformSet)
		{
			if (this.adjustmentTransform)
			{
				transformSet.Add(this.adjustmentTransform);
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000FA67 File Offset: 0x0000DC67
		public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
		{
			if (this.adjustmentTransform && replaceDict.ContainsKey(this.adjustmentTransform.GetInstanceID()))
			{
				this.adjustmentTransform = replaceDict[this.adjustmentTransform.GetInstanceID()];
			}
		}

		// Token: 0x04000258 RID: 600
		public NormalAlignmentSettings.AlignmentMode alignmentMode;

		// Token: 0x04000259 RID: 601
		public Transform adjustmentTransform;

		// Token: 0x02000068 RID: 104
		public enum AlignmentMode
		{
			// Token: 0x0400025B RID: 603
			None,
			// Token: 0x0400025C RID: 604
			BoundingBoxCenter,
			// Token: 0x0400025D RID: 605
			Transform
		}
	}
}
