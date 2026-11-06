using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.AI
{
	// Token: 0x0200001F RID: 31
	[NativeHeader("Modules/AI/Public/NavMeshBindingTypes.h")]
	public struct NavMeshBuildMarkup
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600018C RID: 396 RVA: 0x000034AC File Offset: 0x000016AC
		// (set) Token: 0x0600018D RID: 397 RVA: 0x000034C7 File Offset: 0x000016C7
		public bool overrideArea
		{
			get
			{
				return this.m_OverrideArea != 0;
			}
			set
			{
				this.m_OverrideArea = (value ? 1 : 0);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600018E RID: 398 RVA: 0x000034D8 File Offset: 0x000016D8
		// (set) Token: 0x0600018F RID: 399 RVA: 0x000034F0 File Offset: 0x000016F0
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

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000190 RID: 400 RVA: 0x000034FC File Offset: 0x000016FC
		// (set) Token: 0x06000191 RID: 401 RVA: 0x00003517 File Offset: 0x00001717
		public bool overrideIgnore
		{
			get
			{
				return this.m_InheritIgnoreFromBuild == 0;
			}
			set
			{
				this.m_InheritIgnoreFromBuild = (value ? 0 : 1);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00003528 File Offset: 0x00001728
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00003543 File Offset: 0x00001743
		public bool ignoreFromBuild
		{
			get
			{
				return this.m_IgnoreFromBuild != 0;
			}
			set
			{
				this.m_IgnoreFromBuild = (value ? 1 : 0);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00003554 File Offset: 0x00001754
		// (set) Token: 0x06000195 RID: 405 RVA: 0x0000356F File Offset: 0x0000176F
		public bool overrideGenerateLinks
		{
			get
			{
				return this.m_OverrideGenerateLinks != 0;
			}
			set
			{
				this.m_OverrideGenerateLinks = (value ? 1 : 0);
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00003580 File Offset: 0x00001780
		// (set) Token: 0x06000197 RID: 407 RVA: 0x0000359B File Offset: 0x0000179B
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

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000198 RID: 408 RVA: 0x000035AC File Offset: 0x000017AC
		// (set) Token: 0x06000199 RID: 409 RVA: 0x000035C7 File Offset: 0x000017C7
		public bool applyToChildren
		{
			get
			{
				return this.m_IgnoreChildren == 0;
			}
			set
			{
				this.m_IgnoreChildren = (value ? 0 : 1);
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600019A RID: 410 RVA: 0x000035D8 File Offset: 0x000017D8
		// (set) Token: 0x0600019B RID: 411 RVA: 0x000035F5 File Offset: 0x000017F5
		public Transform root
		{
			get
			{
				return NavMeshBuildMarkup.InternalGetRootGO(this.m_InstanceID);
			}
			set
			{
				this.m_InstanceID = ((value != null) ? value.GetInstanceID() : 0);
			}
		}

		// Token: 0x0600019C RID: 412
		[StaticAccessor("NavMeshBuildMarkup", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Transform InternalGetRootGO(int instanceID);

		// Token: 0x04000060 RID: 96
		private int m_OverrideArea;

		// Token: 0x04000061 RID: 97
		private int m_Area;

		// Token: 0x04000062 RID: 98
		private int m_InheritIgnoreFromBuild;

		// Token: 0x04000063 RID: 99
		private int m_IgnoreFromBuild;

		// Token: 0x04000064 RID: 100
		private int m_OverrideGenerateLinks;

		// Token: 0x04000065 RID: 101
		private int m_GenerateLinks;

		// Token: 0x04000066 RID: 102
		private int m_InstanceID;

		// Token: 0x04000067 RID: 103
		private int m_IgnoreChildren;
	}
}
