using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x0200005E RID: 94
	[Serializable]
	public class CullingSettings : IDataValidate
	{
		// Token: 0x0600014F RID: 335 RVA: 0x0000ECD1 File Offset: 0x0000CED1
		public CullingSettings()
		{
			this.distanceCullingLength = new CheckSliderSerializeData(false, 30f);
			this.distanceCullingFadeRatio = 0.2f;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000ED08 File Offset: 0x0000CF08
		public void DataValidate()
		{
			this.distanceCullingLength.DataValidate(0f, 100f);
			this.distanceCullingFadeRatio = Mathf.Clamp01(this.distanceCullingFadeRatio);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000ED30 File Offset: 0x0000CF30
		public CullingSettings Clone()
		{
			return new CullingSettings
			{
				cameraCullingMode = this.cameraCullingMode,
				cameraCullingMethod = this.cameraCullingMethod,
				cameraCullingRenderers = new List<Renderer>(this.cameraCullingRenderers),
				distanceCullingLength = this.distanceCullingLength.Clone(),
				distanceCullingFadeRatio = this.distanceCullingFadeRatio,
				distanceCullingReferenceObject = this.distanceCullingReferenceObject
			};
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00002137 File Offset: 0x00000337
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x0400022B RID: 555
		public CullingSettings.CameraCullingMode cameraCullingMode = CullingSettings.CameraCullingMode.AnimatorLinkage;

		// Token: 0x0400022C RID: 556
		public CullingSettings.CameraCullingMethod cameraCullingMethod;

		// Token: 0x0400022D RID: 557
		public List<Renderer> cameraCullingRenderers = new List<Renderer>();

		// Token: 0x0400022E RID: 558
		public CheckSliderSerializeData distanceCullingLength;

		// Token: 0x0400022F RID: 559
		[Range(0f, 1f)]
		public float distanceCullingFadeRatio;

		// Token: 0x04000230 RID: 560
		public GameObject distanceCullingReferenceObject;

		// Token: 0x0200005F RID: 95
		public enum CameraCullingMode
		{
			// Token: 0x04000232 RID: 562
			Off,
			// Token: 0x04000233 RID: 563
			Reset = 10,
			// Token: 0x04000234 RID: 564
			Keep = 20,
			// Token: 0x04000235 RID: 565
			AnimatorLinkage = 30
		}

		// Token: 0x02000060 RID: 96
		public enum CameraCullingMethod
		{
			// Token: 0x04000237 RID: 567
			AutomaticRenderer,
			// Token: 0x04000238 RID: 568
			ManualRenderer = 10
		}

		// Token: 0x02000061 RID: 97
		public struct CullingParams
		{
			// Token: 0x06000153 RID: 339 RVA: 0x0000ED94 File Offset: 0x0000CF94
			public void Convert(CullingSettings cullingSettings)
			{
				this.useDistanceCulling = cullingSettings.distanceCullingLength.use;
				this.distanceCullingLength = cullingSettings.distanceCullingLength.value;
				this.distanceCullingFadeRatio = cullingSettings.distanceCullingFadeRatio;
			}

			// Token: 0x04000239 RID: 569
			public bool useDistanceCulling;

			// Token: 0x0400023A RID: 570
			public float distanceCullingLength;

			// Token: 0x0400023B RID: 571
			public float distanceCullingFadeRatio;
		}
	}
}
