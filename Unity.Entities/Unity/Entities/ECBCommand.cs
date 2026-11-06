using System;

namespace Unity.Entities
{
	// Token: 0x0200010B RID: 267
	internal enum ECBCommand
	{
		// Token: 0x040002D2 RID: 722
		InstantiateEntity,
		// Token: 0x040002D3 RID: 723
		CreateEntity,
		// Token: 0x040002D4 RID: 724
		DestroyEntity,
		// Token: 0x040002D5 RID: 725
		AddComponent,
		// Token: 0x040002D6 RID: 726
		AddMultipleComponents,
		// Token: 0x040002D7 RID: 727
		RemoveComponent,
		// Token: 0x040002D8 RID: 728
		RemoveMultipleComponents,
		// Token: 0x040002D9 RID: 729
		SetComponent,
		// Token: 0x040002DA RID: 730
		SetEntityEnabled,
		// Token: 0x040002DB RID: 731
		SetComponentEnabled,
		// Token: 0x040002DC RID: 732
		SetName,
		// Token: 0x040002DD RID: 733
		AddBuffer,
		// Token: 0x040002DE RID: 734
		SetBuffer,
		// Token: 0x040002DF RID: 735
		AppendToBuffer,
		// Token: 0x040002E0 RID: 736
		AddManagedComponentData,
		// Token: 0x040002E1 RID: 737
		SetManagedComponentData,
		// Token: 0x040002E2 RID: 738
		MoveManagedComponentData,
		// Token: 0x040002E3 RID: 739
		AddComponentLinkedEntityGroup,
		// Token: 0x040002E4 RID: 740
		SetComponentLinkedEntityGroup,
		// Token: 0x040002E5 RID: 741
		ReplaceComponentLinkedEntityGroup,
		// Token: 0x040002E6 RID: 742
		AddSharedComponentData,
		// Token: 0x040002E7 RID: 743
		SetSharedComponentData,
		// Token: 0x040002E8 RID: 744
		AddUnmanagedSharedComponentData,
		// Token: 0x040002E9 RID: 745
		SetUnmanagedSharedComponentData,
		// Token: 0x040002EA RID: 746
		AddUnmanagedSharedComponentValueForMultipleEntities,
		// Token: 0x040002EB RID: 747
		SetUnmanagedSharedComponentValueForMultipleEntities,
		// Token: 0x040002EC RID: 748
		AddUnmanagedSharedComponentValueForEntityQuery,
		// Token: 0x040002ED RID: 749
		SetUnmanagedSharedComponentValueForEntityQuery,
		// Token: 0x040002EE RID: 750
		AddComponentForEntityQuery,
		// Token: 0x040002EF RID: 751
		AddMultipleComponentsForMultipleEntities,
		// Token: 0x040002F0 RID: 752
		AddMultipleComponentsForEntityQuery,
		// Token: 0x040002F1 RID: 753
		RemoveComponentForEntityQuery,
		// Token: 0x040002F2 RID: 754
		RemoveMultipleComponentsForMultipleEntities,
		// Token: 0x040002F3 RID: 755
		RemoveMultipleComponentsForEntityQuery,
		// Token: 0x040002F4 RID: 756
		AddSharedComponentWithValueForMultipleEntities,
		// Token: 0x040002F5 RID: 757
		AddSharedComponentWithValueForEntityQuery,
		// Token: 0x040002F6 RID: 758
		SetSharedComponentValueForMultipleEntities,
		// Token: 0x040002F7 RID: 759
		SetSharedComponentValueForEntityQuery,
		// Token: 0x040002F8 RID: 760
		AddComponentForMultipleEntities,
		// Token: 0x040002F9 RID: 761
		AddComponentObjectForMultipleEntities,
		// Token: 0x040002FA RID: 762
		AddComponentObjectForEntityQuery,
		// Token: 0x040002FB RID: 763
		SetComponentObjectForMultipleEntities,
		// Token: 0x040002FC RID: 764
		RemoveComponentForMultipleEntities,
		// Token: 0x040002FD RID: 765
		DestroyMultipleEntities,
		// Token: 0x040002FE RID: 766
		DestroyForEntityQuery
	}
}
