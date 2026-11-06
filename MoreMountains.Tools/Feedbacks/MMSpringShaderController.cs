using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000285 RID: 645
	[AddComponentMenu("More Mountains/Springs/MMSpringShaderController")]
	public class MMSpringShaderController : MMSpringFloatComponent<ShaderController>
	{
		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06001255 RID: 4693 RVA: 0x0004F844 File Offset: 0x0004DA44
		// (set) Token: 0x06001256 RID: 4694 RVA: 0x0004F851 File Offset: 0x0004DA51
		public override float TargetFloat
		{
			get
			{
				return this.Target.DrivenLevel;
			}
			set
			{
				this.Target.DrivenLevel = value;
			}
		}
	}
}
