using System;
using UnityEngine.UIElements.UIR;

namespace UnityEngine.UIElements
{
	// Token: 0x020002AC RID: 684
	internal struct ColorPage
	{
		// Token: 0x060013B2 RID: 5042 RVA: 0x00045204 File Offset: 0x00043404
		public static ColorPage Init(RenderChain renderChain, BMPAlloc alloc)
		{
			bool flag = alloc.IsValid();
			return new ColorPage
			{
				isValid = flag,
				pageAndID = (flag ? renderChain.shaderInfoAllocator.ColorAllocToVertexData(alloc) : default(Color32))
			};
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00045250 File Offset: 0x00043450
		public MeshBuilderNative.NativeColorPage ToNativeColorPage()
		{
			return new MeshBuilderNative.NativeColorPage
			{
				isValid = (this.isValid ? 1 : 0),
				pageAndID = this.pageAndID
			};
		}

		// Token: 0x04000908 RID: 2312
		public bool isValid;

		// Token: 0x04000909 RID: 2313
		public Color32 pageAndID;
	}
}
