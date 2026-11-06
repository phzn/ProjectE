using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200005D RID: 93
	public struct Style
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060002AD RID: 685 RVA: 0x00007528 File Offset: 0x00005728
		// (set) Token: 0x060002AE RID: 686 RVA: 0x00007530 File Offset: 0x00005730
		[JsonProperty("fontFamily")]
		public string FontFamily { readonly get; set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060002AF RID: 687 RVA: 0x00007539 File Offset: 0x00005739
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x00007541 File Offset: 0x00005741
		[JsonProperty("fontPostScriptName")]
		public string FontPostScriptName { readonly get; set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000754A File Offset: 0x0000574A
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x00007552 File Offset: 0x00005752
		[JsonProperty("fontWeight")]
		public int FontWeight { readonly get; set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000755B File Offset: 0x0000575B
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x00007563 File Offset: 0x00005763
		[JsonProperty("paragraphSpacing")]
		public float ParagraphSpacing { readonly get; set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000756C File Offset: 0x0000576C
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x00007574 File Offset: 0x00005774
		[JsonProperty("textAutoResize")]
		public TextAutoResize TextAutoResize { readonly get; set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000757D File Offset: 0x0000577D
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x00007585 File Offset: 0x00005785
		[JsonProperty("fontSize")]
		public float FontSize { readonly get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000758E File Offset: 0x0000578E
		// (set) Token: 0x060002BA RID: 698 RVA: 0x00007596 File Offset: 0x00005796
		[JsonProperty("textAlignHorizontal")]
		public string TextAlignHorizontal { readonly get; set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000759F File Offset: 0x0000579F
		// (set) Token: 0x060002BC RID: 700 RVA: 0x000075A7 File Offset: 0x000057A7
		[JsonProperty("textAlignVertical")]
		public string TextAlignVertical { readonly get; set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060002BD RID: 701 RVA: 0x000075B0 File Offset: 0x000057B0
		// (set) Token: 0x060002BE RID: 702 RVA: 0x000075B8 File Offset: 0x000057B8
		[JsonProperty("letterSpacing")]
		public float LetterSpacing { readonly get; set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060002BF RID: 703 RVA: 0x000075C1 File Offset: 0x000057C1
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x000075C9 File Offset: 0x000057C9
		[JsonProperty("lineHeightPx")]
		public float LineHeightPx { readonly get; set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x000075D2 File Offset: 0x000057D2
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x000075DA File Offset: 0x000057DA
		[JsonProperty("lineHeightPercentFontSize")]
		public float? LineHeightPercentFontSize { readonly get; set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x000075E3 File Offset: 0x000057E3
		// (set) Token: 0x060002C4 RID: 708 RVA: 0x000075EB File Offset: 0x000057EB
		[JsonProperty("textCase")]
		public string TextCase { readonly get; set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x000075F4 File Offset: 0x000057F4
		// (set) Token: 0x060002C6 RID: 710 RVA: 0x000075FC File Offset: 0x000057FC
		[JsonProperty("textDecoration")]
		public string TextDecoration { readonly get; set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x00007605 File Offset: 0x00005805
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x0000760D File Offset: 0x0000580D
		[JsonProperty("italic")]
		public bool? Italic { readonly get; set; }
	}
}
