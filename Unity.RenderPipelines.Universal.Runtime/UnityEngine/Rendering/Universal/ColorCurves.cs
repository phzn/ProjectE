using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000091 RID: 145
	[VolumeComponentMenuForRenderPipeline("Post-processing/Color Curves", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class ColorCurves : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06000569 RID: 1385 RVA: 0x0001F771 File Offset: 0x0001D971
		public bool IsActive()
		{
			return true;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001F774 File Offset: 0x0001D974
		public bool IsTileCompatible()
		{
			return true;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001F778 File Offset: 0x0001D978
		public ColorCurves()
		{
			Keyframe[] array = new Keyframe[]
			{
				new Keyframe(0f, 0f, 1f, 1f),
				new Keyframe(1f, 1f, 1f, 1f)
			};
			float num = 0f;
			bool flag = false;
			Vector2 vector = new Vector2(0f, 1f);
			this.master = new TextureCurveParameter(new TextureCurve(array, num, flag, in vector), false);
			Keyframe[] array2 = new Keyframe[]
			{
				new Keyframe(0f, 0f, 1f, 1f),
				new Keyframe(1f, 1f, 1f, 1f)
			};
			float num2 = 0f;
			bool flag2 = false;
			vector = new Vector2(0f, 1f);
			this.red = new TextureCurveParameter(new TextureCurve(array2, num2, flag2, in vector), false);
			Keyframe[] array3 = new Keyframe[]
			{
				new Keyframe(0f, 0f, 1f, 1f),
				new Keyframe(1f, 1f, 1f, 1f)
			};
			float num3 = 0f;
			bool flag3 = false;
			vector = new Vector2(0f, 1f);
			this.green = new TextureCurveParameter(new TextureCurve(array3, num3, flag3, in vector), false);
			Keyframe[] array4 = new Keyframe[]
			{
				new Keyframe(0f, 0f, 1f, 1f),
				new Keyframe(1f, 1f, 1f, 1f)
			};
			float num4 = 0f;
			bool flag4 = false;
			vector = new Vector2(0f, 1f);
			this.blue = new TextureCurveParameter(new TextureCurve(array4, num4, flag4, in vector), false);
			Keyframe[] array5 = new Keyframe[0];
			float num5 = 0.5f;
			bool flag5 = true;
			vector = new Vector2(0f, 1f);
			this.hueVsHue = new TextureCurveParameter(new TextureCurve(array5, num5, flag5, in vector), false);
			Keyframe[] array6 = new Keyframe[0];
			float num6 = 0.5f;
			bool flag6 = true;
			vector = new Vector2(0f, 1f);
			this.hueVsSat = new TextureCurveParameter(new TextureCurve(array6, num6, flag6, in vector), false);
			Keyframe[] array7 = new Keyframe[0];
			float num7 = 0.5f;
			bool flag7 = false;
			vector = new Vector2(0f, 1f);
			this.satVsSat = new TextureCurveParameter(new TextureCurve(array7, num7, flag7, in vector), false);
			Keyframe[] array8 = new Keyframe[0];
			float num8 = 0.5f;
			bool flag8 = false;
			vector = new Vector2(0f, 1f);
			this.lumVsSat = new TextureCurveParameter(new TextureCurve(array8, num8, flag8, in vector), false);
			base..ctor();
		}

		// Token: 0x040003E4 RID: 996
		[Tooltip("Affects the luminance across the whole image.")]
		public TextureCurveParameter master;

		// Token: 0x040003E5 RID: 997
		[Tooltip("Affects the red channel intensity across the whole image.")]
		public TextureCurveParameter red;

		// Token: 0x040003E6 RID: 998
		[Tooltip("Affects the green channel intensity across the whole image.")]
		public TextureCurveParameter green;

		// Token: 0x040003E7 RID: 999
		[Tooltip("Affects the blue channel intensity across the whole image.")]
		public TextureCurveParameter blue;

		// Token: 0x040003E8 RID: 1000
		[Tooltip("Shifts the input hue (x-axis) according to the output hue (y-axis).")]
		public TextureCurveParameter hueVsHue;

		// Token: 0x040003E9 RID: 1001
		[Tooltip("Adjusts saturation (y-axis) according to the input hue (x-axis).")]
		public TextureCurveParameter hueVsSat;

		// Token: 0x040003EA RID: 1002
		[Tooltip("Adjusts saturation (y-axis) according to the input saturation (x-axis).")]
		public TextureCurveParameter satVsSat;

		// Token: 0x040003EB RID: 1003
		[Tooltip("Adjusts saturation (y-axis) according to the input luminance (x-axis).")]
		public TextureCurveParameter lumVsSat;
	}
}
