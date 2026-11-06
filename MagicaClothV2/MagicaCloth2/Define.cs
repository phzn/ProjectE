using System;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x02000071 RID: 113
	public static class Define
	{
		// Token: 0x02000072 RID: 114
		public enum Result
		{
			// Token: 0x04000291 RID: 657
			None,
			// Token: 0x04000292 RID: 658
			Empty,
			// Token: 0x04000293 RID: 659
			Success,
			// Token: 0x04000294 RID: 660
			Cancel,
			// Token: 0x04000295 RID: 661
			Process,
			// Token: 0x04000296 RID: 662
			Warning = 10000,
			// Token: 0x04000297 RID: 663
			RenderMesh_UnknownWarning = 10100,
			// Token: 0x04000298 RID: 664
			RenderMesh_VertexWeightIs5BonesOrMore,
			// Token: 0x04000299 RID: 665
			Init_NonUniformScale = 10200,
			// Token: 0x0400029A RID: 666
			Error = 20000,
			// Token: 0x0400029B RID: 667
			SerializeData_InvalidData = 20050,
			// Token: 0x0400029C RID: 668
			SerializeData_Over31Renderers,
			// Token: 0x0400029D RID: 669
			SerializeData_DuplicateRootBone,
			// Token: 0x0400029E RID: 670
			SerializeData_DuplicateRenderer,
			// Token: 0x0400029F RID: 671
			Init_InvalidData = 20100,
			// Token: 0x040002A0 RID: 672
			Init_InvalidPaintMap,
			// Token: 0x040002A1 RID: 673
			Init_PaintMapNotReadable,
			// Token: 0x040002A2 RID: 674
			Init_ScaleIsZero,
			// Token: 0x040002A3 RID: 675
			Init_NegativeScale,
			// Token: 0x040002A4 RID: 676
			RenderSetup_Exception = 20200,
			// Token: 0x040002A5 RID: 677
			RenderSetup_UnknownError,
			// Token: 0x040002A6 RID: 678
			RenderSetup_InvalidSource,
			// Token: 0x040002A7 RID: 679
			RenderSetup_NoMeshOnRenderer,
			// Token: 0x040002A8 RID: 680
			RenderSetup_InvalidType,
			// Token: 0x040002A9 RID: 681
			RenderSetup_Unreadable,
			// Token: 0x040002AA RID: 682
			RenderSetup_Over65535vertices,
			// Token: 0x040002AB RID: 683
			VirtualMesh_UnknownError = 20300,
			// Token: 0x040002AC RID: 684
			VirtualMesh_InvalidSetup,
			// Token: 0x040002AD RID: 685
			VirtualMesh_InvalidRenderData,
			// Token: 0x040002AE RID: 686
			VirtualMesh_ImportError,
			// Token: 0x040002AF RID: 687
			VirtualMesh_SelectionException,
			// Token: 0x040002B0 RID: 688
			VirtualMesh_SelectionUnknownError,
			// Token: 0x040002B1 RID: 689
			VirtualMesh_InvalidSelection,
			// Token: 0x040002B2 RID: 690
			CreateCloth_Exception = 20400,
			// Token: 0x040002B3 RID: 691
			CreateCloth_UnknownError,
			// Token: 0x040002B4 RID: 692
			CreateCloth_InvalidCloth,
			// Token: 0x040002B5 RID: 693
			CreateCloth_InvalidSerializeData,
			// Token: 0x040002B6 RID: 694
			CreateCloth_InvalidSetupList,
			// Token: 0x040002B7 RID: 695
			CreateCloth_NoRenderer,
			// Token: 0x040002B8 RID: 696
			CreateCloth_InvalidPaintMap,
			// Token: 0x040002B9 RID: 697
			CreateCloth_PaintMapNotReadable,
			// Token: 0x040002BA RID: 698
			CreateCloth_PaintMapCountMismatch,
			// Token: 0x040002BB RID: 699
			CreateCloth_CanNotStart,
			// Token: 0x040002BC RID: 700
			CreateCloth_VertexAttributeListCountMismatch,
			// Token: 0x040002BD RID: 701
			CreateCloth_VertexAttributeListIsNull,
			// Token: 0x040002BE RID: 702
			CreateCloth_VertexAttributeListDataMismatch,
			// Token: 0x040002BF RID: 703
			CreateCloth_InvalidVertexAttributeData,
			// Token: 0x040002C0 RID: 704
			Reduction_Exception = 20500,
			// Token: 0x040002C1 RID: 705
			Reduction_UnknownError,
			// Token: 0x040002C2 RID: 706
			Reduction_InitError,
			// Token: 0x040002C3 RID: 707
			Reduction_SameDistanceException,
			// Token: 0x040002C4 RID: 708
			Reduction_SimpleDistanceException,
			// Token: 0x040002C5 RID: 709
			Reduction_ShapeDistanceException,
			// Token: 0x040002C6 RID: 710
			Reduction_MaxSideLengthZero,
			// Token: 0x040002C7 RID: 711
			Reduction_OrganizationError,
			// Token: 0x040002C8 RID: 712
			Reduction_StoreVirtualMeshError,
			// Token: 0x040002C9 RID: 713
			Reduction_CalcAverageException,
			// Token: 0x040002CA RID: 714
			Optimize_Exception = 20600,
			// Token: 0x040002CB RID: 715
			ProxyMesh_Exception = 20700,
			// Token: 0x040002CC RID: 716
			ProxyMesh_UnknownError,
			// Token: 0x040002CD RID: 717
			ProxyMesh_ApplySelectionError,
			// Token: 0x040002CE RID: 718
			ProxyMesh_ConvertError,
			// Token: 0x040002CF RID: 719
			ProxyMesh_Over32767Vertices,
			// Token: 0x040002D0 RID: 720
			ProxyMesh_Over32767Edges,
			// Token: 0x040002D1 RID: 721
			ProxyMesh_Over32767Triangles,
			// Token: 0x040002D2 RID: 722
			MappingMesh_Exception = 20800,
			// Token: 0x040002D3 RID: 723
			MappingMesh_UnknownError,
			// Token: 0x040002D4 RID: 724
			MappingMesh_ProxyError,
			// Token: 0x040002D5 RID: 725
			ClothInit_Exception = 22000,
			// Token: 0x040002D6 RID: 726
			ClothInit_FailedAddRenderer,
			// Token: 0x040002D7 RID: 727
			ClothProcess_Exception = 22100,
			// Token: 0x040002D8 RID: 728
			ClothProcess_UnknownError,
			// Token: 0x040002D9 RID: 729
			ClothProcess_Invalid,
			// Token: 0x040002DA RID: 730
			ClothProcess_InvalidRenderHandleList,
			// Token: 0x040002DB RID: 731
			ClothProcess_GenerateSelectionError,
			// Token: 0x040002DC RID: 732
			ClothProcess_OverflowTeamCount4096,
			// Token: 0x040002DD RID: 733
			Constraint_Exception = 22200,
			// Token: 0x040002DE RID: 734
			Constraint_UnknownError,
			// Token: 0x040002DF RID: 735
			Constraint_CreateDistanceException,
			// Token: 0x040002E0 RID: 736
			Constraint_CreateTriangleBendingException,
			// Token: 0x040002E1 RID: 737
			Constraint_CreateInertiaException,
			// Token: 0x040002E2 RID: 738
			Constraint_CreateSelfCollisionException,
			// Token: 0x040002E3 RID: 739
			MagicaMesh_UnknownError = 22500,
			// Token: 0x040002E4 RID: 740
			MagicaMesh_Invalid,
			// Token: 0x040002E5 RID: 741
			MagicaMesh_InvalidRenderer,
			// Token: 0x040002E6 RID: 742
			MagicaMesh_InvalidMeshFilter,
			// Token: 0x040002E7 RID: 743
			PreBuildData_UnknownError = 22600,
			// Token: 0x040002E8 RID: 744
			PreBuildData_MagicaClothException,
			// Token: 0x040002E9 RID: 745
			PreBuildData_VirtualMeshDeserializationException,
			// Token: 0x040002EA RID: 746
			PreBuildData_VerificationResult,
			// Token: 0x040002EB RID: 747
			PreBuildData_VersionMismatch,
			// Token: 0x040002EC RID: 748
			PreBuildData_InvalidClothData,
			// Token: 0x040002ED RID: 749
			PreBuildData_Empty,
			// Token: 0x040002EE RID: 750
			PreBuildData_InvalidScale,
			// Token: 0x040002EF RID: 751
			PreBuild_UnknownError = 22700,
			// Token: 0x040002F0 RID: 752
			PreBuild_Exception,
			// Token: 0x040002F1 RID: 753
			PreBuild_InvalidPreBuildData,
			// Token: 0x040002F2 RID: 754
			PreBuild_InvalidRenderSetupData,
			// Token: 0x040002F3 RID: 755
			PreBuild_SetupDeserializationError,
			// Token: 0x040002F4 RID: 756
			Deserialization_UnknownError = 22800,
			// Token: 0x040002F5 RID: 757
			Deserialization_Exception,
			// Token: 0x040002F6 RID: 758
			InitSerializeData_UnknownError = 22900,
			// Token: 0x040002F7 RID: 759
			InitSerializeData_InvalidHash,
			// Token: 0x040002F8 RID: 760
			InitSerializeData_InvalidVersion,
			// Token: 0x040002F9 RID: 761
			InitSerializeData_InvalidSetupData,
			// Token: 0x040002FA RID: 762
			InitSerializeData_ClothTypeMismatch,
			// Token: 0x040002FB RID: 763
			InitSerializeData_SetupCountMismatch,
			// Token: 0x040002FC RID: 764
			InitSerializeData_CustomSkinningBoneCountMismatch,
			// Token: 0x040002FD RID: 765
			InitSerializeData_MeshClothSetupValidationError,
			// Token: 0x040002FE RID: 766
			InitSerializeData_BoneClothSetupValidationError,
			// Token: 0x040002FF RID: 767
			InitSerializeData_BoneSpringSetupValidationError,
			// Token: 0x04000300 RID: 768
			InitSerializeData_DeserializationError,
			// Token: 0x04000301 RID: 769
			InitSerializeData_InvalidCloneMesh
		}

		// Token: 0x02000073 RID: 115
		public static class System
		{
			// Token: 0x04000302 RID: 770
			public const string DefineSymbol = "MAGICACLOTH2";

			// Token: 0x04000303 RID: 771
			public const int LatestPreBuildVersion = 2;

			// Token: 0x04000304 RID: 772
			public const float Epsilon = 1E-08f;

			// Token: 0x04000305 RID: 773
			public const int MaxRendererCount = 31;

			// Token: 0x04000306 RID: 774
			public const float MinimumGridSize = 1E-05f;

			// Token: 0x04000307 RID: 775
			public const int MaximumTeamCount = 4096;

			// Token: 0x04000308 RID: 776
			public const int DefaultSimulationFrequency = 90;

			// Token: 0x04000309 RID: 777
			public const int SimulationFrequency_Low = 30;

			// Token: 0x0400030A RID: 778
			public const int SimulationFrequency_Hi = 150;

			// Token: 0x0400030B RID: 779
			public const int DefaultMaxSimulationCountPerFrame = 3;

			// Token: 0x0400030C RID: 780
			public const int MaxSimulationCountPerFrame_Low = 1;

			// Token: 0x0400030D RID: 781
			public const int MaxSimulationCountPerFrame_Hi = 5;

			// Token: 0x0400030E RID: 782
			public const float SameSurfaceAngle = 80f;

			// Token: 0x0400030F RID: 783
			public const float MaxDistanceRatioFutuerPrediction = 1.3f;

			// Token: 0x04000310 RID: 784
			public const int SplitProxyMeshVertexCount = 300;

			// Token: 0x04000311 RID: 785
			public const bool ReductionEnable = true;

			// Token: 0x04000312 RID: 786
			public const float ReductionSameDistance = 0.001f;

			// Token: 0x04000313 RID: 787
			public const bool ReductionDontMakeLine = true;

			// Token: 0x04000314 RID: 788
			public const float ReductionJoinPositionAdjustment = 1f;

			// Token: 0x04000315 RID: 789
			public const int ReductionMaxStep = 100;

			// Token: 0x04000316 RID: 790
			public const int MaxProxyMeshVertexCount = 32767;

			// Token: 0x04000317 RID: 791
			public const int MaxProxyMeshEdgeCount = 32767;

			// Token: 0x04000318 RID: 792
			public const int MaxProxyMeshTriangleCount = 32767;

			// Token: 0x04000319 RID: 793
			public const float ProxyMeshTrianglePairAngle = 20f;

			// Token: 0x0400031A RID: 794
			public const float ProxyMeshBoneClothTriangleAngle = 120f;

			// Token: 0x0400031B RID: 795
			public const float FrictionMass = 3f;

			// Token: 0x0400031C RID: 796
			public const float DepthMass = 5f;

			// Token: 0x0400031D RID: 797
			public const float FrictionDampingRate = 0.6f;

			// Token: 0x0400031E RID: 798
			public const float PositionAverageExponent = 0.5f;

			// Token: 0x0400031F RID: 799
			public const float MaxRealVelocity = 0.5f;

			// Token: 0x04000320 RID: 800
			public const float TetherCompressionStiffness = 1f;

			// Token: 0x04000321 RID: 801
			public const float TetherStretchStiffness = 1f;

			// Token: 0x04000322 RID: 802
			public const float TetherStretchLimit = 0.03f;

			// Token: 0x04000323 RID: 803
			public const float TetherStiffnessWidth = 0.3f;

			// Token: 0x04000324 RID: 804
			public const float TetherCompressionVelocityAttenuation = 0.7f;

			// Token: 0x04000325 RID: 805
			public const float TetherStretchVelocityAttenuation = 0.7f;

			// Token: 0x04000326 RID: 806
			public const float DistanceVelocityAttenuation = 0.3f;

			// Token: 0x04000327 RID: 807
			public const float DistanceVerticalStiffness = 1f;

			// Token: 0x04000328 RID: 808
			public const float DistanceHorizontalStiffness = 0.5f;

			// Token: 0x04000329 RID: 809
			public const float TriangleBendingMaxAngle = 120f;

			// Token: 0x0400032A RID: 810
			public const float VolumeMinAngle = 90f;

			// Token: 0x0400032B RID: 811
			public const float MaxAngleLimit = 179f;

			// Token: 0x0400032C RID: 812
			public const int AngleLimitIteration = 3;

			// Token: 0x0400032D RID: 813
			public const float AngleLimitAttenuation = 0.9f;

			// Token: 0x0400032E RID: 814
			public const float MaxMovementSpeedLimit = 10f;

			// Token: 0x0400032F RID: 815
			public const float MaxRotationSpeedLimit = 1440f;

			// Token: 0x04000330 RID: 816
			public const float MaxParticleSpeedLimit = 10f;

			// Token: 0x04000331 RID: 817
			public const int ExpandedColliderCount = 8;

			// Token: 0x04000332 RID: 818
			public const float ColliderCollisionDynamicFrictionRatio = 1f;

			// Token: 0x04000333 RID: 819
			public const float ColliderCollisionStaticFrictionRatio = 1f;

			// Token: 0x04000334 RID: 820
			public const float CustomSkinningAngularAttenuation = 1f;

			// Token: 0x04000335 RID: 821
			public const float CustomSkinningDistanceReduction = 0.6f;

			// Token: 0x04000336 RID: 822
			public const float CustomSkinningDistancePow = 2f;

			// Token: 0x04000337 RID: 823
			public const int SelfCollisionSolverIteration = 4;

			// Token: 0x04000338 RID: 824
			public const int SelfCollisionIgnoreGrid = 1000000;

			// Token: 0x04000339 RID: 825
			public const int SelfCollisionIntersectDiv = 2;

			// Token: 0x0400033A RID: 826
			public const float SelfCollisionFixedMass = 100f;

			// Token: 0x0400033B RID: 827
			public const float SelfCollisionFrictionMass = 10f;

			// Token: 0x0400033C RID: 828
			public const float SelfCollisionClothMass = 50f;

			// Token: 0x0400033D RID: 829
			public const float SelfCollisionSCR = 2f;

			// Token: 0x0400033E RID: 830
			public static readonly float SelfCollisionPointTriangleAngleCos = math.cos(math.radians(60f));

			// Token: 0x0400033F RID: 831
			public const float SelfCollisionThicknessMin = 0.001f;

			// Token: 0x04000340 RID: 832
			public const float SelfCollisionThicknessMax = 0.05f;

			// Token: 0x04000341 RID: 833
			public const float WindMaxTime = 10000f;

			// Token: 0x04000342 RID: 834
			public const float WindBaseSpeed = 7.5f;

			// Token: 0x04000343 RID: 835
			public const float BoneSpringDistanceStiffness = 0.5f;

			// Token: 0x04000344 RID: 836
			public const float BoneSpringTetherCompressionLimit = 0.8f;

			// Token: 0x04000345 RID: 837
			public const float BoneSpringCollisionFriction = 0.5f;

			// Token: 0x04000346 RID: 838
			public const float DistanceCullingMaxLength = 100f;
		}
	}
}
