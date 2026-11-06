using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000034 RID: 52
	internal interface IPixelPerfectCamera
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001D2 RID: 466
		// (set) Token: 0x060001D3 RID: 467
		int assetsPPU { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001D4 RID: 468
		// (set) Token: 0x060001D5 RID: 469
		int refResolutionX { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001D6 RID: 470
		// (set) Token: 0x060001D7 RID: 471
		int refResolutionY { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001D8 RID: 472
		// (set) Token: 0x060001D9 RID: 473
		bool upscaleRT { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001DA RID: 474
		// (set) Token: 0x060001DB RID: 475
		bool pixelSnapping { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001DC RID: 476
		// (set) Token: 0x060001DD RID: 477
		bool cropFrameX { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001DE RID: 478
		// (set) Token: 0x060001DF RID: 479
		bool cropFrameY { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001E0 RID: 480
		// (set) Token: 0x060001E1 RID: 481
		bool stretchFill { get; set; }
	}
}
