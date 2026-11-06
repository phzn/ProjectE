using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000059 RID: 89
	public struct Paint : IVisible
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600027D RID: 637 RVA: 0x000073A1 File Offset: 0x000055A1
		// (set) Token: 0x0600027E RID: 638 RVA: 0x000073A9 File Offset: 0x000055A9
		[JsonProperty("blendMode")]
		public string BlendMode { readonly get; set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600027F RID: 639 RVA: 0x000073B2 File Offset: 0x000055B2
		// (set) Token: 0x06000280 RID: 640 RVA: 0x000073BA File Offset: 0x000055BA
		[JsonProperty("type")]
		public PaintType Type { readonly get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000281 RID: 641 RVA: 0x000073C3 File Offset: 0x000055C3
		// (set) Token: 0x06000282 RID: 642 RVA: 0x000073CB File Offset: 0x000055CB
		[JsonProperty("color")]
		public Color Color { readonly get; set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000283 RID: 643 RVA: 0x000073D4 File Offset: 0x000055D4
		// (set) Token: 0x06000284 RID: 644 RVA: 0x000073DC File Offset: 0x000055DC
		[JsonProperty("visible")]
		public bool? Visible { readonly get; set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000285 RID: 645 RVA: 0x000073E5 File Offset: 0x000055E5
		// (set) Token: 0x06000286 RID: 646 RVA: 0x000073ED File Offset: 0x000055ED
		[JsonProperty("scaleMode")]
		public string ScaleMode { readonly get; set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000287 RID: 647 RVA: 0x000073F6 File Offset: 0x000055F6
		// (set) Token: 0x06000288 RID: 648 RVA: 0x000073FE File Offset: 0x000055FE
		[JsonProperty("scalingFactor")]
		public string ScalingFactor { readonly get; set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000289 RID: 649 RVA: 0x00007407 File Offset: 0x00005607
		// (set) Token: 0x0600028A RID: 650 RVA: 0x0000740F File Offset: 0x0000560F
		[JsonProperty("imageRef")]
		public string ImageRef { readonly get; set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600028B RID: 651 RVA: 0x00007418 File Offset: 0x00005618
		// (set) Token: 0x0600028C RID: 652 RVA: 0x00007420 File Offset: 0x00005620
		[JsonProperty("gifRef")]
		public string GifRef { readonly get; set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00007429 File Offset: 0x00005629
		// (set) Token: 0x0600028E RID: 654 RVA: 0x00007431 File Offset: 0x00005631
		[JsonProperty("imageTransform")]
		public List<List<float>> ImageTransform { readonly get; set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0000743A File Offset: 0x0000563A
		// (set) Token: 0x06000290 RID: 656 RVA: 0x00007442 File Offset: 0x00005642
		[JsonProperty("gradientHandlePositions")]
		public List<Vector2> GradientHandlePositions { readonly get; set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000744B File Offset: 0x0000564B
		// (set) Token: 0x06000292 RID: 658 RVA: 0x00007453 File Offset: 0x00005653
		[JsonProperty("gradientStops")]
		public List<GradientStop> GradientStops { readonly get; set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000745C File Offset: 0x0000565C
		// (set) Token: 0x06000294 RID: 660 RVA: 0x00007464 File Offset: 0x00005664
		[JsonProperty("opacity")]
		public float? Opacity { readonly get; set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000746D File Offset: 0x0000566D
		// (set) Token: 0x06000296 RID: 662 RVA: 0x00007475 File Offset: 0x00005675
		[JsonProperty("filters")]
		public Filters Filters { readonly get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000747E File Offset: 0x0000567E
		// (set) Token: 0x06000298 RID: 664 RVA: 0x00007486 File Offset: 0x00005686
		[JsonProperty("rotation")]
		public float? Rotation { readonly get; set; }
	}
}
