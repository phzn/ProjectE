using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002D4 RID: 724
	public interface IStyle
	{
		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x060016A9 RID: 5801
		// (set) Token: 0x060016AA RID: 5802
		StyleEnum<Align> alignContent { get; set; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x060016AB RID: 5803
		// (set) Token: 0x060016AC RID: 5804
		StyleEnum<Align> alignItems { get; set; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x060016AD RID: 5805
		// (set) Token: 0x060016AE RID: 5806
		StyleEnum<Align> alignSelf { get; set; }

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x060016AF RID: 5807
		// (set) Token: 0x060016B0 RID: 5808
		StyleColor backgroundColor { get; set; }

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x060016B1 RID: 5809
		// (set) Token: 0x060016B2 RID: 5810
		StyleBackground backgroundImage { get; set; }

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x060016B3 RID: 5811
		// (set) Token: 0x060016B4 RID: 5812
		StyleBackgroundPosition backgroundPositionX { get; set; }

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x060016B5 RID: 5813
		// (set) Token: 0x060016B6 RID: 5814
		StyleBackgroundPosition backgroundPositionY { get; set; }

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x060016B7 RID: 5815
		// (set) Token: 0x060016B8 RID: 5816
		StyleBackgroundRepeat backgroundRepeat { get; set; }

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x060016B9 RID: 5817
		// (set) Token: 0x060016BA RID: 5818
		StyleBackgroundSize backgroundSize { get; set; }

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x060016BB RID: 5819
		// (set) Token: 0x060016BC RID: 5820
		StyleColor borderBottomColor { get; set; }

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x060016BD RID: 5821
		// (set) Token: 0x060016BE RID: 5822
		StyleLength borderBottomLeftRadius { get; set; }

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060016BF RID: 5823
		// (set) Token: 0x060016C0 RID: 5824
		StyleLength borderBottomRightRadius { get; set; }

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x060016C1 RID: 5825
		// (set) Token: 0x060016C2 RID: 5826
		StyleFloat borderBottomWidth { get; set; }

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x060016C3 RID: 5827
		// (set) Token: 0x060016C4 RID: 5828
		StyleColor borderLeftColor { get; set; }

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x060016C5 RID: 5829
		// (set) Token: 0x060016C6 RID: 5830
		StyleFloat borderLeftWidth { get; set; }

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x060016C7 RID: 5831
		// (set) Token: 0x060016C8 RID: 5832
		StyleColor borderRightColor { get; set; }

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x060016C9 RID: 5833
		// (set) Token: 0x060016CA RID: 5834
		StyleFloat borderRightWidth { get; set; }

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x060016CB RID: 5835
		// (set) Token: 0x060016CC RID: 5836
		StyleColor borderTopColor { get; set; }

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x060016CD RID: 5837
		// (set) Token: 0x060016CE RID: 5838
		StyleLength borderTopLeftRadius { get; set; }

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x060016CF RID: 5839
		// (set) Token: 0x060016D0 RID: 5840
		StyleLength borderTopRightRadius { get; set; }

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x060016D1 RID: 5841
		// (set) Token: 0x060016D2 RID: 5842
		StyleFloat borderTopWidth { get; set; }

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x060016D3 RID: 5843
		// (set) Token: 0x060016D4 RID: 5844
		StyleLength bottom { get; set; }

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x060016D5 RID: 5845
		// (set) Token: 0x060016D6 RID: 5846
		StyleColor color { get; set; }

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x060016D7 RID: 5847
		// (set) Token: 0x060016D8 RID: 5848
		StyleCursor cursor { get; set; }

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x060016D9 RID: 5849
		// (set) Token: 0x060016DA RID: 5850
		StyleEnum<DisplayStyle> display { get; set; }

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x060016DB RID: 5851
		// (set) Token: 0x060016DC RID: 5852
		StyleLength flexBasis { get; set; }

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x060016DD RID: 5853
		// (set) Token: 0x060016DE RID: 5854
		StyleEnum<FlexDirection> flexDirection { get; set; }

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x060016DF RID: 5855
		// (set) Token: 0x060016E0 RID: 5856
		StyleFloat flexGrow { get; set; }

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x060016E1 RID: 5857
		// (set) Token: 0x060016E2 RID: 5858
		StyleFloat flexShrink { get; set; }

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x060016E3 RID: 5859
		// (set) Token: 0x060016E4 RID: 5860
		StyleEnum<Wrap> flexWrap { get; set; }

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x060016E5 RID: 5861
		// (set) Token: 0x060016E6 RID: 5862
		StyleLength fontSize { get; set; }

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x060016E7 RID: 5863
		// (set) Token: 0x060016E8 RID: 5864
		StyleLength height { get; set; }

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x060016E9 RID: 5865
		// (set) Token: 0x060016EA RID: 5866
		StyleEnum<Justify> justifyContent { get; set; }

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x060016EB RID: 5867
		// (set) Token: 0x060016EC RID: 5868
		StyleLength left { get; set; }

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x060016ED RID: 5869
		// (set) Token: 0x060016EE RID: 5870
		StyleLength letterSpacing { get; set; }

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x060016EF RID: 5871
		// (set) Token: 0x060016F0 RID: 5872
		StyleLength marginBottom { get; set; }

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x060016F1 RID: 5873
		// (set) Token: 0x060016F2 RID: 5874
		StyleLength marginLeft { get; set; }

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x060016F3 RID: 5875
		// (set) Token: 0x060016F4 RID: 5876
		StyleLength marginRight { get; set; }

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x060016F5 RID: 5877
		// (set) Token: 0x060016F6 RID: 5878
		StyleLength marginTop { get; set; }

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x060016F7 RID: 5879
		// (set) Token: 0x060016F8 RID: 5880
		StyleLength maxHeight { get; set; }

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x060016F9 RID: 5881
		// (set) Token: 0x060016FA RID: 5882
		StyleLength maxWidth { get; set; }

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x060016FB RID: 5883
		// (set) Token: 0x060016FC RID: 5884
		StyleLength minHeight { get; set; }

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x060016FD RID: 5885
		// (set) Token: 0x060016FE RID: 5886
		StyleLength minWidth { get; set; }

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x060016FF RID: 5887
		// (set) Token: 0x06001700 RID: 5888
		StyleFloat opacity { get; set; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001701 RID: 5889
		// (set) Token: 0x06001702 RID: 5890
		StyleEnum<Overflow> overflow { get; set; }

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001703 RID: 5891
		// (set) Token: 0x06001704 RID: 5892
		StyleLength paddingBottom { get; set; }

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001705 RID: 5893
		// (set) Token: 0x06001706 RID: 5894
		StyleLength paddingLeft { get; set; }

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001707 RID: 5895
		// (set) Token: 0x06001708 RID: 5896
		StyleLength paddingRight { get; set; }

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001709 RID: 5897
		// (set) Token: 0x0600170A RID: 5898
		StyleLength paddingTop { get; set; }

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x0600170B RID: 5899
		// (set) Token: 0x0600170C RID: 5900
		StyleEnum<Position> position { get; set; }

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x0600170D RID: 5901
		// (set) Token: 0x0600170E RID: 5902
		StyleLength right { get; set; }

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x0600170F RID: 5903
		// (set) Token: 0x06001710 RID: 5904
		StyleRotate rotate { get; set; }

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001711 RID: 5905
		// (set) Token: 0x06001712 RID: 5906
		StyleScale scale { get; set; }

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001713 RID: 5907
		// (set) Token: 0x06001714 RID: 5908
		StyleEnum<TextOverflow> textOverflow { get; set; }

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001715 RID: 5909
		// (set) Token: 0x06001716 RID: 5910
		StyleTextShadow textShadow { get; set; }

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001717 RID: 5911
		// (set) Token: 0x06001718 RID: 5912
		StyleLength top { get; set; }

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001719 RID: 5913
		// (set) Token: 0x0600171A RID: 5914
		StyleTransformOrigin transformOrigin { get; set; }

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x0600171B RID: 5915
		// (set) Token: 0x0600171C RID: 5916
		StyleList<TimeValue> transitionDelay { get; set; }

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x0600171D RID: 5917
		// (set) Token: 0x0600171E RID: 5918
		StyleList<TimeValue> transitionDuration { get; set; }

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x0600171F RID: 5919
		// (set) Token: 0x06001720 RID: 5920
		StyleList<StylePropertyName> transitionProperty { get; set; }

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001721 RID: 5921
		// (set) Token: 0x06001722 RID: 5922
		StyleList<EasingFunction> transitionTimingFunction { get; set; }

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001723 RID: 5923
		// (set) Token: 0x06001724 RID: 5924
		StyleTranslate translate { get; set; }

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001725 RID: 5925
		// (set) Token: 0x06001726 RID: 5926
		StyleColor unityBackgroundImageTintColor { get; set; }

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001727 RID: 5927
		// (set) Token: 0x06001728 RID: 5928
		StyleFont unityFont { get; set; }

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001729 RID: 5929
		// (set) Token: 0x0600172A RID: 5930
		StyleFontDefinition unityFontDefinition { get; set; }

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x0600172B RID: 5931
		// (set) Token: 0x0600172C RID: 5932
		StyleEnum<FontStyle> unityFontStyleAndWeight { get; set; }

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x0600172D RID: 5933
		// (set) Token: 0x0600172E RID: 5934
		StyleEnum<OverflowClipBox> unityOverflowClipBox { get; set; }

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x0600172F RID: 5935
		// (set) Token: 0x06001730 RID: 5936
		StyleLength unityParagraphSpacing { get; set; }

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001731 RID: 5937
		// (set) Token: 0x06001732 RID: 5938
		StyleInt unitySliceBottom { get; set; }

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001733 RID: 5939
		// (set) Token: 0x06001734 RID: 5940
		StyleInt unitySliceLeft { get; set; }

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001735 RID: 5941
		// (set) Token: 0x06001736 RID: 5942
		StyleInt unitySliceRight { get; set; }

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001737 RID: 5943
		// (set) Token: 0x06001738 RID: 5944
		StyleFloat unitySliceScale { get; set; }

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001739 RID: 5945
		// (set) Token: 0x0600173A RID: 5946
		StyleInt unitySliceTop { get; set; }

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x0600173B RID: 5947
		// (set) Token: 0x0600173C RID: 5948
		StyleEnum<TextAnchor> unityTextAlign { get; set; }

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x0600173D RID: 5949
		// (set) Token: 0x0600173E RID: 5950
		StyleColor unityTextOutlineColor { get; set; }

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x0600173F RID: 5951
		// (set) Token: 0x06001740 RID: 5952
		StyleFloat unityTextOutlineWidth { get; set; }

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001741 RID: 5953
		// (set) Token: 0x06001742 RID: 5954
		StyleEnum<TextOverflowPosition> unityTextOverflowPosition { get; set; }

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001743 RID: 5955
		// (set) Token: 0x06001744 RID: 5956
		StyleEnum<Visibility> visibility { get; set; }

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001745 RID: 5957
		// (set) Token: 0x06001746 RID: 5958
		StyleEnum<WhiteSpace> whiteSpace { get; set; }

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001747 RID: 5959
		// (set) Token: 0x06001748 RID: 5960
		StyleLength width { get; set; }

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001749 RID: 5961
		// (set) Token: 0x0600174A RID: 5962
		StyleLength wordSpacing { get; set; }

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x0600174B RID: 5963
		// (set) Token: 0x0600174C RID: 5964
		[Obsolete("unityBackgroundScaleMode is deprecated. Use background-* properties instead.")]
		StyleEnum<ScaleMode> unityBackgroundScaleMode { get; set; }
	}
}
