using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020002D3 RID: 723
	public interface IResolvedStyle
	{
		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x0600165B RID: 5723
		Align alignContent { get; }

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x0600165C RID: 5724
		Align alignItems { get; }

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x0600165D RID: 5725
		Align alignSelf { get; }

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x0600165E RID: 5726
		Color backgroundColor { get; }

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x0600165F RID: 5727
		Background backgroundImage { get; }

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001660 RID: 5728
		BackgroundPosition backgroundPositionX { get; }

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001661 RID: 5729
		BackgroundPosition backgroundPositionY { get; }

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001662 RID: 5730
		BackgroundRepeat backgroundRepeat { get; }

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001663 RID: 5731
		BackgroundSize backgroundSize { get; }

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001664 RID: 5732
		Color borderBottomColor { get; }

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001665 RID: 5733
		float borderBottomLeftRadius { get; }

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001666 RID: 5734
		float borderBottomRightRadius { get; }

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001667 RID: 5735
		float borderBottomWidth { get; }

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001668 RID: 5736
		Color borderLeftColor { get; }

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001669 RID: 5737
		float borderLeftWidth { get; }

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x0600166A RID: 5738
		Color borderRightColor { get; }

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x0600166B RID: 5739
		float borderRightWidth { get; }

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x0600166C RID: 5740
		Color borderTopColor { get; }

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x0600166D RID: 5741
		float borderTopLeftRadius { get; }

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x0600166E RID: 5742
		float borderTopRightRadius { get; }

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x0600166F RID: 5743
		float borderTopWidth { get; }

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001670 RID: 5744
		float bottom { get; }

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001671 RID: 5745
		Color color { get; }

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001672 RID: 5746
		DisplayStyle display { get; }

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001673 RID: 5747
		StyleFloat flexBasis { get; }

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001674 RID: 5748
		FlexDirection flexDirection { get; }

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001675 RID: 5749
		float flexGrow { get; }

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001676 RID: 5750
		float flexShrink { get; }

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001677 RID: 5751
		Wrap flexWrap { get; }

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06001678 RID: 5752
		float fontSize { get; }

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001679 RID: 5753
		float height { get; }

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x0600167A RID: 5754
		Justify justifyContent { get; }

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x0600167B RID: 5755
		float left { get; }

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x0600167C RID: 5756
		float letterSpacing { get; }

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x0600167D RID: 5757
		float marginBottom { get; }

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x0600167E RID: 5758
		float marginLeft { get; }

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x0600167F RID: 5759
		float marginRight { get; }

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001680 RID: 5760
		float marginTop { get; }

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001681 RID: 5761
		StyleFloat maxHeight { get; }

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001682 RID: 5762
		StyleFloat maxWidth { get; }

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001683 RID: 5763
		StyleFloat minHeight { get; }

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001684 RID: 5764
		StyleFloat minWidth { get; }

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001685 RID: 5765
		float opacity { get; }

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001686 RID: 5766
		float paddingBottom { get; }

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001687 RID: 5767
		float paddingLeft { get; }

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001688 RID: 5768
		float paddingRight { get; }

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001689 RID: 5769
		float paddingTop { get; }

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x0600168A RID: 5770
		Position position { get; }

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x0600168B RID: 5771
		float right { get; }

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x0600168C RID: 5772
		Rotate rotate { get; }

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x0600168D RID: 5773
		Scale scale { get; }

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x0600168E RID: 5774
		TextOverflow textOverflow { get; }

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x0600168F RID: 5775
		float top { get; }

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001690 RID: 5776
		Vector3 transformOrigin { get; }

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001691 RID: 5777
		IEnumerable<TimeValue> transitionDelay { get; }

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001692 RID: 5778
		IEnumerable<TimeValue> transitionDuration { get; }

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001693 RID: 5779
		IEnumerable<StylePropertyName> transitionProperty { get; }

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001694 RID: 5780
		IEnumerable<EasingFunction> transitionTimingFunction { get; }

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001695 RID: 5781
		Vector3 translate { get; }

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001696 RID: 5782
		Color unityBackgroundImageTintColor { get; }

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001697 RID: 5783
		Font unityFont { get; }

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001698 RID: 5784
		FontDefinition unityFontDefinition { get; }

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001699 RID: 5785
		FontStyle unityFontStyleAndWeight { get; }

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x0600169A RID: 5786
		float unityParagraphSpacing { get; }

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x0600169B RID: 5787
		int unitySliceBottom { get; }

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x0600169C RID: 5788
		int unitySliceLeft { get; }

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x0600169D RID: 5789
		int unitySliceRight { get; }

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x0600169E RID: 5790
		float unitySliceScale { get; }

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x0600169F RID: 5791
		int unitySliceTop { get; }

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x060016A0 RID: 5792
		TextAnchor unityTextAlign { get; }

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x060016A1 RID: 5793
		Color unityTextOutlineColor { get; }

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x060016A2 RID: 5794
		float unityTextOutlineWidth { get; }

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x060016A3 RID: 5795
		TextOverflowPosition unityTextOverflowPosition { get; }

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060016A4 RID: 5796
		Visibility visibility { get; }

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x060016A5 RID: 5797
		WhiteSpace whiteSpace { get; }

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060016A6 RID: 5798
		float width { get; }

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060016A7 RID: 5799
		float wordSpacing { get; }

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x060016A8 RID: 5800
		[Obsolete("unityBackgroundScaleMode is deprecated. Use background-* properties instead.")]
		StyleEnum<ScaleMode> unityBackgroundScaleMode { get; }
	}
}
