using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DA_Assets.Tools;
using Newtonsoft.Json;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000054 RID: 84
	public struct FObject : IHaveId, IVisible
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00006F83 File Offset: 0x00005183
		// (set) Token: 0x06000202 RID: 514 RVA: 0x00006F8B File Offset: 0x0000518B
		[IgnoreDataMember]
		public SyncData Data { readonly get; set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00006F94 File Offset: 0x00005194
		// (set) Token: 0x06000204 RID: 516 RVA: 0x00006F9C File Offset: 0x0000519C
		[JsonProperty("id")]
		public string Id { readonly get; set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00006FA5 File Offset: 0x000051A5
		// (set) Token: 0x06000206 RID: 518 RVA: 0x00006FAD File Offset: 0x000051AD
		[JsonProperty("name")]
		public string Name { readonly get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00006FB6 File Offset: 0x000051B6
		// (set) Token: 0x06000208 RID: 520 RVA: 0x00006FBE File Offset: 0x000051BE
		[JsonProperty("type")]
		public NodeType Type { readonly get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00006FC7 File Offset: 0x000051C7
		// (set) Token: 0x0600020A RID: 522 RVA: 0x00006FCF File Offset: 0x000051CF
		[JsonProperty("rotation")]
		public float? Rotation { readonly get; set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00006FD8 File Offset: 0x000051D8
		// (set) Token: 0x0600020C RID: 524 RVA: 0x00006FE0 File Offset: 0x000051E0
		[JsonProperty("blendMode")]
		public string BlendMode { readonly get; set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00006FE9 File Offset: 0x000051E9
		// (set) Token: 0x0600020E RID: 526 RVA: 0x00006FF1 File Offset: 0x000051F1
		[JsonProperty("children")]
		public List<FObject> Children { readonly get; set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00006FFA File Offset: 0x000051FA
		// (set) Token: 0x06000210 RID: 528 RVA: 0x00007002 File Offset: 0x00005202
		[JsonProperty("absoluteBoundingBox")]
		public BoundingBox AbsoluteBoundingBox { readonly get; set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000700B File Offset: 0x0000520B
		// (set) Token: 0x06000212 RID: 530 RVA: 0x00007013 File Offset: 0x00005213
		[JsonProperty("absoluteRenderBounds")]
		public BoundingBox AbsoluteRenderBounds { readonly get; set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000701C File Offset: 0x0000521C
		// (set) Token: 0x06000214 RID: 532 RVA: 0x00007024 File Offset: 0x00005224
		[JsonProperty("constraints")]
		public Constraints Constraints { readonly get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000702D File Offset: 0x0000522D
		// (set) Token: 0x06000216 RID: 534 RVA: 0x00007035 File Offset: 0x00005235
		[JsonProperty("relativeTransform")]
		public List<List<float?>> RelativeTransform { readonly get; set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000703E File Offset: 0x0000523E
		// (set) Token: 0x06000218 RID: 536 RVA: 0x00007046 File Offset: 0x00005246
		[JsonProperty("size")]
		public Vector2 Size { readonly get; set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000704F File Offset: 0x0000524F
		// (set) Token: 0x0600021A RID: 538 RVA: 0x00007057 File Offset: 0x00005257
		[JsonProperty("clipsContent")]
		public bool? ClipsContent { readonly get; set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00007060 File Offset: 0x00005260
		// (set) Token: 0x0600021C RID: 540 RVA: 0x00007068 File Offset: 0x00005268
		[JsonProperty("fills")]
		public List<Paint> Fills { readonly get; set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00007071 File Offset: 0x00005271
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00007079 File Offset: 0x00005279
		[JsonProperty("strokes")]
		public List<Paint> Strokes { readonly get; set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00007082 File Offset: 0x00005282
		// (set) Token: 0x06000220 RID: 544 RVA: 0x0000708A File Offset: 0x0000528A
		[JsonProperty("cornerRadius")]
		public float? CornerRadius { readonly get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00007093 File Offset: 0x00005293
		// (set) Token: 0x06000222 RID: 546 RVA: 0x0000709B File Offset: 0x0000529B
		[JsonProperty("strokeWeight")]
		public float StrokeWeight { readonly get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000223 RID: 547 RVA: 0x000070A4 File Offset: 0x000052A4
		// (set) Token: 0x06000224 RID: 548 RVA: 0x000070AC File Offset: 0x000052AC
		[JsonProperty("individualStrokeWeights")]
		public IndividualStrokeWeights IndividualStrokeWeights { readonly get; set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000225 RID: 549 RVA: 0x000070B5 File Offset: 0x000052B5
		// (set) Token: 0x06000226 RID: 550 RVA: 0x000070BD File Offset: 0x000052BD
		[JsonProperty("strokeAlign")]
		public StrokeAlign StrokeAlign { readonly get; set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000227 RID: 551 RVA: 0x000070C6 File Offset: 0x000052C6
		// (set) Token: 0x06000228 RID: 552 RVA: 0x000070CE File Offset: 0x000052CE
		[JsonProperty("effects")]
		public List<Effect> Effects { readonly get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000070D7 File Offset: 0x000052D7
		// (set) Token: 0x0600022A RID: 554 RVA: 0x000070DF File Offset: 0x000052DF
		[JsonProperty("overflowDirection")]
		public OverflowDirection OverflowDirection { readonly get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600022B RID: 555 RVA: 0x000070E8 File Offset: 0x000052E8
		// (set) Token: 0x0600022C RID: 556 RVA: 0x000070F0 File Offset: 0x000052F0
		[JsonProperty("fillGeometry")]
		public List<FillGeometry> FillGeometry { readonly get; set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600022D RID: 557 RVA: 0x000070F9 File Offset: 0x000052F9
		// (set) Token: 0x0600022E RID: 558 RVA: 0x00007101 File Offset: 0x00005301
		[JsonProperty("strokeGeometry")]
		public List<FillGeometry> StrokeGeometry { readonly get; set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000710A File Offset: 0x0000530A
		// (set) Token: 0x06000230 RID: 560 RVA: 0x00007112 File Offset: 0x00005312
		[JsonProperty("strokeCap")]
		public StrokeCap StrokeCap { readonly get; set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000711B File Offset: 0x0000531B
		// (set) Token: 0x06000232 RID: 562 RVA: 0x00007123 File Offset: 0x00005323
		[JsonProperty("strokeJoin")]
		public string StrokeJoin { readonly get; set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0000712C File Offset: 0x0000532C
		// (set) Token: 0x06000234 RID: 564 RVA: 0x00007134 File Offset: 0x00005334
		[JsonProperty("strokeMiterAngle")]
		public float? StrokeMiterAngle { readonly get; set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000713D File Offset: 0x0000533D
		// (set) Token: 0x06000236 RID: 566 RVA: 0x00007145 File Offset: 0x00005345
		[JsonProperty("opacity")]
		public float? Opacity { readonly get; set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000714E File Offset: 0x0000534E
		// (set) Token: 0x06000238 RID: 568 RVA: 0x00007156 File Offset: 0x00005356
		[JsonProperty("preserveRatio")]
		public bool? PreserveRatio { readonly get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000715F File Offset: 0x0000535F
		// (set) Token: 0x0600023A RID: 570 RVA: 0x00007167 File Offset: 0x00005367
		[JsonProperty("layoutAlign")]
		public LayoutAlign LayoutAlign { readonly get; set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00007170 File Offset: 0x00005370
		// (set) Token: 0x0600023C RID: 572 RVA: 0x00007178 File Offset: 0x00005378
		[JsonProperty("layoutGrow")]
		public float? LayoutGrow { readonly get; set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00007181 File Offset: 0x00005381
		// (set) Token: 0x0600023E RID: 574 RVA: 0x00007189 File Offset: 0x00005389
		[JsonProperty("characters")]
		public string Characters { readonly get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00007192 File Offset: 0x00005392
		// (set) Token: 0x06000240 RID: 576 RVA: 0x0000719A File Offset: 0x0000539A
		[JsonProperty("style")]
		public Style Style { readonly get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000241 RID: 577 RVA: 0x000071A3 File Offset: 0x000053A3
		// (set) Token: 0x06000242 RID: 578 RVA: 0x000071AB File Offset: 0x000053AB
		[JsonProperty("styleOverrideTable")]
		public Dictionary<string, Style> StyleOverrideTable { readonly get; set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000243 RID: 579 RVA: 0x000071B4 File Offset: 0x000053B4
		// (set) Token: 0x06000244 RID: 580 RVA: 0x000071BC File Offset: 0x000053BC
		[JsonProperty("layoutMode")]
		public LayoutMode LayoutMode { readonly get; set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000245 RID: 581 RVA: 0x000071C5 File Offset: 0x000053C5
		// (set) Token: 0x06000246 RID: 582 RVA: 0x000071CD File Offset: 0x000053CD
		[JsonProperty("itemSpacing")]
		public float? ItemSpacing { readonly get; set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000247 RID: 583 RVA: 0x000071D6 File Offset: 0x000053D6
		// (set) Token: 0x06000248 RID: 584 RVA: 0x000071DE File Offset: 0x000053DE
		[JsonProperty("counterAxisSpacing")]
		public float? CounterAxisSpacing { readonly get; set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000249 RID: 585 RVA: 0x000071E7 File Offset: 0x000053E7
		// (set) Token: 0x0600024A RID: 586 RVA: 0x000071EF File Offset: 0x000053EF
		[JsonProperty("visible")]
		public bool? Visible { readonly get; set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600024B RID: 587 RVA: 0x000071F8 File Offset: 0x000053F8
		// (set) Token: 0x0600024C RID: 588 RVA: 0x00007200 File Offset: 0x00005400
		[JsonProperty("primaryAxisSizingMode")]
		public PrimaryAxisSizingMode PrimaryAxisSizingMode { readonly get; set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00007209 File Offset: 0x00005409
		// (set) Token: 0x0600024E RID: 590 RVA: 0x00007211 File Offset: 0x00005411
		[JsonProperty("counterAxisSizingMode")]
		public CounterAxisSizingMode CounterAxisSizingMode { readonly get; set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000721A File Offset: 0x0000541A
		// (set) Token: 0x06000250 RID: 592 RVA: 0x00007222 File Offset: 0x00005422
		[JsonProperty("counterAxisAlignContent")]
		public string CounterAxisAlignContent { readonly get; set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000722B File Offset: 0x0000542B
		// (set) Token: 0x06000252 RID: 594 RVA: 0x00007233 File Offset: 0x00005433
		[JsonProperty("primaryAxisAlignItems")]
		public PrimaryAxisAlignItem PrimaryAxisAlignItems { readonly get; set; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000723C File Offset: 0x0000543C
		// (set) Token: 0x06000254 RID: 596 RVA: 0x00007244 File Offset: 0x00005444
		[JsonProperty("counterAxisAlignItems")]
		public CounterAxisAlignItem CounterAxisAlignItems { readonly get; set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000724D File Offset: 0x0000544D
		// (set) Token: 0x06000256 RID: 598 RVA: 0x00007255 File Offset: 0x00005455
		[JsonProperty("layoutWrap")]
		public LayoutWrap LayoutWrap { readonly get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000725E File Offset: 0x0000545E
		// (set) Token: 0x06000258 RID: 600 RVA: 0x00007266 File Offset: 0x00005466
		[JsonProperty("isMask")]
		public bool? IsMask { readonly get; set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000726F File Offset: 0x0000546F
		// (set) Token: 0x0600025A RID: 602 RVA: 0x00007277 File Offset: 0x00005477
		[JsonProperty("paddingLeft")]
		public float? PaddingLeft { readonly get; set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600025B RID: 603 RVA: 0x00007280 File Offset: 0x00005480
		// (set) Token: 0x0600025C RID: 604 RVA: 0x00007288 File Offset: 0x00005488
		[JsonProperty("paddingRight")]
		public float? PaddingRight { readonly get; set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600025D RID: 605 RVA: 0x00007291 File Offset: 0x00005491
		// (set) Token: 0x0600025E RID: 606 RVA: 0x00007299 File Offset: 0x00005499
		[JsonProperty("paddingTop")]
		public float? PaddingTop { readonly get; set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600025F RID: 607 RVA: 0x000072A2 File Offset: 0x000054A2
		// (set) Token: 0x06000260 RID: 608 RVA: 0x000072AA File Offset: 0x000054AA
		[JsonProperty("paddingBottom")]
		public float? PaddingBottom { readonly get; set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000261 RID: 609 RVA: 0x000072B3 File Offset: 0x000054B3
		// (set) Token: 0x06000262 RID: 610 RVA: 0x000072BB File Offset: 0x000054BB
		[JsonProperty("horizontalPadding")]
		public float? HorizontalPadding { readonly get; set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000263 RID: 611 RVA: 0x000072C4 File Offset: 0x000054C4
		// (set) Token: 0x06000264 RID: 612 RVA: 0x000072CC File Offset: 0x000054CC
		[JsonProperty("verticalPadding")]
		public float? VerticalPadding { readonly get; set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000265 RID: 613 RVA: 0x000072D5 File Offset: 0x000054D5
		// (set) Token: 0x06000266 RID: 614 RVA: 0x000072DD File Offset: 0x000054DD
		[JsonProperty("rectangleCornerRadii")]
		public List<float> CornerRadiuses { readonly get; set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000267 RID: 615 RVA: 0x000072E6 File Offset: 0x000054E6
		// (set) Token: 0x06000268 RID: 616 RVA: 0x000072EE File Offset: 0x000054EE
		[JsonProperty("arcData")]
		public ArcData ArcData { readonly get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000269 RID: 617 RVA: 0x000072F7 File Offset: 0x000054F7
		// (set) Token: 0x0600026A RID: 618 RVA: 0x000072FF File Offset: 0x000054FF
		[JsonProperty("strokeDashes")]
		public List<float> StrokeDashes { readonly get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00007308 File Offset: 0x00005508
		// (set) Token: 0x0600026C RID: 620 RVA: 0x00007310 File Offset: 0x00005510
		[JsonProperty("layoutPositioning")]
		public LayoutPositioning LayoutPositioning { readonly get; set; }
	}
}
