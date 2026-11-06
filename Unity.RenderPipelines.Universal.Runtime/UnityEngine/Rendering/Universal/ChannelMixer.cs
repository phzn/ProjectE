using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200008E RID: 142
	[VolumeComponentMenuForRenderPipeline("Post-processing/Channel Mixer", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class ChannelMixer : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06000560 RID: 1376 RVA: 0x0001F47C File Offset: 0x0001D67C
		public bool IsActive()
		{
			return this.redOutRedIn.value != 100f || this.redOutGreenIn.value != 0f || this.redOutBlueIn.value != 0f || this.greenOutRedIn.value != 0f || this.greenOutGreenIn.value != 100f || this.greenOutBlueIn.value != 0f || this.blueOutRedIn.value != 0f || this.blueOutGreenIn.value != 0f || this.blueOutBlueIn.value != 100f;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001F536 File Offset: 0x0001D736
		public bool IsTileCompatible()
		{
			return true;
		}

		// Token: 0x040003D5 RID: 981
		[Tooltip("Modify influence of the red channel in the overall mix.")]
		public ClampedFloatParameter redOutRedIn = new ClampedFloatParameter(100f, -200f, 200f, false);

		// Token: 0x040003D6 RID: 982
		[Tooltip("Modify influence of the green channel in the overall mix.")]
		public ClampedFloatParameter redOutGreenIn = new ClampedFloatParameter(0f, -200f, 200f, false);

		// Token: 0x040003D7 RID: 983
		[Tooltip("Modify influence of the blue channel in the overall mix.")]
		public ClampedFloatParameter redOutBlueIn = new ClampedFloatParameter(0f, -200f, 200f, false);

		// Token: 0x040003D8 RID: 984
		[Tooltip("Modify influence of the red channel in the overall mix.")]
		public ClampedFloatParameter greenOutRedIn = new ClampedFloatParameter(0f, -200f, 200f, false);

		// Token: 0x040003D9 RID: 985
		[Tooltip("Modify influence of the green channel in the overall mix.")]
		public ClampedFloatParameter greenOutGreenIn = new ClampedFloatParameter(100f, -200f, 200f, false);

		// Token: 0x040003DA RID: 986
		[Tooltip("Modify influence of the blue channel in the overall mix.")]
		public ClampedFloatParameter greenOutBlueIn = new ClampedFloatParameter(0f, -200f, 200f, false);

		// Token: 0x040003DB RID: 987
		[Tooltip("Modify influence of the red channel in the overall mix.")]
		public ClampedFloatParameter blueOutRedIn = new ClampedFloatParameter(0f, -200f, 200f, false);

		// Token: 0x040003DC RID: 988
		[Tooltip("Modify influence of the green channel in the overall mix.")]
		public ClampedFloatParameter blueOutGreenIn = new ClampedFloatParameter(0f, -200f, 200f, false);

		// Token: 0x040003DD RID: 989
		[Tooltip("Modify influence of the blue channel in the overall mix.")]
		public ClampedFloatParameter blueOutBlueIn = new ClampedFloatParameter(100f, -200f, 200f, false);
	}
}
