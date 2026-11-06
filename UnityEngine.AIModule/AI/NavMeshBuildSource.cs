using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.AI
{
	// Token: 0x0200001E RID: 30
	[UsedByNativeCode]
	[NativeHeader("Modules/AI/Public/NavMeshBindingTypes.h")]
	public struct NavMeshBuildSource
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00003380 File Offset: 0x00001580
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00003398 File Offset: 0x00001598
		public Matrix4x4 transform
		{
			get
			{
				return this.m_Transform;
			}
			set
			{
				this.m_Transform = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600017E RID: 382 RVA: 0x000033A4 File Offset: 0x000015A4
		// (set) Token: 0x0600017F RID: 383 RVA: 0x000033BC File Offset: 0x000015BC
		public Vector3 size
		{
			get
			{
				return this.m_Size;
			}
			set
			{
				this.m_Size = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000180 RID: 384 RVA: 0x000033C8 File Offset: 0x000015C8
		// (set) Token: 0x06000181 RID: 385 RVA: 0x000033E0 File Offset: 0x000015E0
		public NavMeshBuildSourceShape shape
		{
			get
			{
				return this.m_Shape;
			}
			set
			{
				this.m_Shape = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000182 RID: 386 RVA: 0x000033EC File Offset: 0x000015EC
		// (set) Token: 0x06000183 RID: 387 RVA: 0x00003404 File Offset: 0x00001604
		public int area
		{
			get
			{
				return this.m_Area;
			}
			set
			{
				this.m_Area = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00003410 File Offset: 0x00001610
		// (set) Token: 0x06000185 RID: 389 RVA: 0x0000342B File Offset: 0x0000162B
		public bool generateLinks
		{
			get
			{
				return this.m_GenerateLinks != 0;
			}
			set
			{
				this.m_GenerateLinks = (value ? 1 : 0);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000186 RID: 390 RVA: 0x0000343C File Offset: 0x0000163C
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00003459 File Offset: 0x00001659
		public Object sourceObject
		{
			get
			{
				return NavMeshBuildSource.InternalGetObject(this.m_InstanceID);
			}
			set
			{
				this.m_InstanceID = ((value != null) ? value.GetInstanceID() : 0);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00003474 File Offset: 0x00001674
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00003491 File Offset: 0x00001691
		public Component component
		{
			get
			{
				return NavMeshBuildSource.InternalGetComponent(this.m_ComponentID);
			}
			set
			{
				this.m_ComponentID = ((value != null) ? value.GetInstanceID() : 0);
			}
		}

		// Token: 0x0600018A RID: 394
		[StaticAccessor("NavMeshBuildSource", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Component InternalGetComponent(int instanceID);

		// Token: 0x0600018B RID: 395
		[StaticAccessor("NavMeshBuildSource", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object InternalGetObject(int instanceID);

		// Token: 0x04000059 RID: 89
		private Matrix4x4 m_Transform;

		// Token: 0x0400005A RID: 90
		private Vector3 m_Size;

		// Token: 0x0400005B RID: 91
		private NavMeshBuildSourceShape m_Shape;

		// Token: 0x0400005C RID: 92
		private int m_Area;

		// Token: 0x0400005D RID: 93
		private int m_InstanceID;

		// Token: 0x0400005E RID: 94
		private int m_ComponentID;

		// Token: 0x0400005F RID: 95
		private int m_GenerateLinks;
	}
}
