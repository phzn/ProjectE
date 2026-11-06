using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000E9 RID: 233
	public class VolumeComponentMenuForRenderPipeline : VolumeComponentMenu
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00025A83 File Offset: 0x00023C83
		public Type[] pipelineTypes { get; }

		// Token: 0x060007AA RID: 1962 RVA: 0x00025A8C File Offset: 0x00023C8C
		public VolumeComponentMenuForRenderPipeline(string menu, params Type[] pipelineTypes)
			: base(menu)
		{
			if (pipelineTypes == null)
			{
				throw new Exception("Specify a list of supported pipeline");
			}
			foreach (Type type in pipelineTypes)
			{
				if (!typeof(RenderPipeline).IsAssignableFrom(type))
				{
					throw new Exception(string.Format("You can only specify types that inherit from {0}, please check {1}", typeof(RenderPipeline), type));
				}
			}
			this.pipelineTypes = pipelineTypes;
		}
	}
}
