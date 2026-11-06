using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000034 RID: 52
	[Serializable]
	public class GPUIShaderInstance
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000A074 File Offset: 0x00008274
		public Shader originalShader
		{
			get
			{
				if (string.IsNullOrEmpty(this.shaderName))
				{
					return null;
				}
				return GPUIUtility.FindShader(this.shaderName);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000A090 File Offset: 0x00008290
		public Shader replacementShader
		{
			get
			{
				if (string.IsNullOrEmpty(this.replacementShaderName))
				{
					return null;
				}
				return GPUIUtility.FindShader(this.replacementShaderName);
			}
		}

		// Token: 0x04000144 RID: 324
		public string shaderName;

		// Token: 0x04000145 RID: 325
		public string replacementShaderName;

		// Token: 0x04000146 RID: 326
		public string extensionCode;

		// Token: 0x04000147 RID: 327
		public bool isUseOriginal;

		// Token: 0x04000148 RID: 328
		public string modifiedDate;
	}
}
