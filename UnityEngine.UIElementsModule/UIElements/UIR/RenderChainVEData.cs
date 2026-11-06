using System;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements.UIR.Implementation;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200045D RID: 1117
	internal struct RenderChainVEData
	{
		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x060022F3 RID: 8947 RVA: 0x00087AC8 File Offset: 0x00085CC8
		internal RenderChainCommand lastClosingOrLastCommand
		{
			get
			{
				return this.lastClosingCommand ?? this.lastCommand;
			}
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x00087AEC File Offset: 0x00085CEC
		internal static bool AllocatesID(BMPAlloc alloc)
		{
			return alloc.ownedState == OwnedState.Owned && alloc.IsValid();
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x00087B14 File Offset: 0x00085D14
		internal static bool InheritsID(BMPAlloc alloc)
		{
			return alloc.ownedState == OwnedState.Inherited && alloc.IsValid();
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x060022F6 RID: 8950 RVA: 0x00087B38 File Offset: 0x00085D38
		public bool isIgnoringDynamicColorHint
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.flags & RenderDataFlags.IsIgnoringDynamicColorHint) == RenderDataFlags.IsIgnoringDynamicColorHint;
			}
		}

		// Token: 0x04000FEE RID: 4078
		internal VisualElement prev;

		// Token: 0x04000FEF RID: 4079
		internal VisualElement next;

		// Token: 0x04000FF0 RID: 4080
		internal VisualElement groupTransformAncestor;

		// Token: 0x04000FF1 RID: 4081
		internal VisualElement boneTransformAncestor;

		// Token: 0x04000FF2 RID: 4082
		internal VisualElement prevDirty;

		// Token: 0x04000FF3 RID: 4083
		internal VisualElement nextDirty;

		// Token: 0x04000FF4 RID: 4084
		internal RenderDataFlags flags;

		// Token: 0x04000FF5 RID: 4085
		internal int hierarchyDepth;

		// Token: 0x04000FF6 RID: 4086
		internal RenderDataDirtyTypes dirtiedValues;

		// Token: 0x04000FF7 RID: 4087
		internal uint dirtyID;

		// Token: 0x04000FF8 RID: 4088
		internal RenderChainCommand firstCommand;

		// Token: 0x04000FF9 RID: 4089
		internal RenderChainCommand lastCommand;

		// Token: 0x04000FFA RID: 4090
		internal RenderChainCommand firstClosingCommand;

		// Token: 0x04000FFB RID: 4091
		internal RenderChainCommand lastClosingCommand;

		// Token: 0x04000FFC RID: 4092
		internal bool isInChain;

		// Token: 0x04000FFD RID: 4093
		internal bool isHierarchyHidden;

		// Token: 0x04000FFE RID: 4094
		internal bool localFlipsWinding;

		// Token: 0x04000FFF RID: 4095
		internal bool localTransformScaleZero;

		// Token: 0x04001000 RID: 4096
		internal bool worldFlipsWinding;

		// Token: 0x04001001 RID: 4097
		public bool worldTransformScaleZero;

		// Token: 0x04001002 RID: 4098
		internal ClipMethod clipMethod;

		// Token: 0x04001003 RID: 4099
		internal int childrenStencilRef;

		// Token: 0x04001004 RID: 4100
		internal int childrenMaskDepth;

		// Token: 0x04001005 RID: 4101
		internal bool disableNudging;

		// Token: 0x04001006 RID: 4102
		internal MeshHandle data;

		// Token: 0x04001007 RID: 4103
		internal MeshHandle closingData;

		// Token: 0x04001008 RID: 4104
		internal Matrix4x4 verticesSpace;

		// Token: 0x04001009 RID: 4105
		internal int displacementUVStart;

		// Token: 0x0400100A RID: 4106
		internal int displacementUVEnd;

		// Token: 0x0400100B RID: 4107
		internal BMPAlloc transformID;

		// Token: 0x0400100C RID: 4108
		internal BMPAlloc clipRectID;

		// Token: 0x0400100D RID: 4109
		internal BMPAlloc opacityID;

		// Token: 0x0400100E RID: 4110
		internal BMPAlloc textCoreSettingsID;

		// Token: 0x0400100F RID: 4111
		internal BMPAlloc colorID;

		// Token: 0x04001010 RID: 4112
		internal BMPAlloc backgroundColorID;

		// Token: 0x04001011 RID: 4113
		internal BMPAlloc borderLeftColorID;

		// Token: 0x04001012 RID: 4114
		internal BMPAlloc borderTopColorID;

		// Token: 0x04001013 RID: 4115
		internal BMPAlloc borderRightColorID;

		// Token: 0x04001014 RID: 4116
		internal BMPAlloc borderBottomColorID;

		// Token: 0x04001015 RID: 4117
		internal BMPAlloc tintColorID;

		// Token: 0x04001016 RID: 4118
		internal float compositeOpacity;

		// Token: 0x04001017 RID: 4119
		internal Color backgroundColor;

		// Token: 0x04001018 RID: 4120
		internal BasicNode<TextureEntry> textures;
	}
}
