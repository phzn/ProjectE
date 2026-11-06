using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Core;

namespace Unity.Entities
{
	// Token: 0x02000053 RID: 83
	internal sealed class SystemState_
	{
		// Token: 0x0600023E RID: 574 RVA: 0x0000B44C File Offset: 0x0000964C
		public SystemState_(ref SystemState systemState)
		{
			this._SystemHandle = systemState.SystemHandle;
			this._World = systemState.World;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000B46C File Offset: 0x0000966C
		public unsafe static SystemState_ FromPointer(SystemState* systemState)
		{
			if (systemState != null)
			{
				return new SystemState_(ref *systemState);
			}
			return null;
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000B47B File Offset: 0x0000967B
		public EntityManager EntityManager
		{
			get
			{
				return this._World.EntityManager;
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000B488 File Offset: 0x00009688
		private unsafe static string GetName(SystemHandle handle, World world)
		{
			if (world == null || !world.IsCreated)
			{
				return null;
			}
			SystemState* ptr = world.Unmanaged.ResolveSystemState(handle);
			if (ptr == null)
			{
				return null;
			}
			return ptr->DebugName.ToString();
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000B4CD File Offset: 0x000096CD
		public SystemDebugView Data
		{
			get
			{
				return new SystemDebugView(this._SystemHandle);
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000B4DA File Offset: 0x000096DA
		public unsafe TimeData Time
		{
			get
			{
				return *this.Resolve()->World.Time;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000B4F1 File Offset: 0x000096F1
		public unsafe uint LastSystemVersion
		{
			get
			{
				return this.Resolve()->LastSystemVersion;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000B4FE File Offset: 0x000096FE
		public unsafe uint GlobalSystemVersion
		{
			get
			{
				return this.Resolve()->GlobalSystemVersion;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000B50B File Offset: 0x0000970B
		public unsafe bool Enabled
		{
			get
			{
				return this.Resolve()->Enabled;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000B518 File Offset: 0x00009718
		private unsafe bool RequireMatchingQueriesForUpdate
		{
			get
			{
				return this.Resolve()->RequireMatchingQueriesForUpdate;
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000B528 File Offset: 0x00009728
		private unsafe SystemState* Resolve()
		{
			if (this._World == null || !this._World.IsCreated)
			{
				throw new NullReferenceException("World is null");
			}
			SystemState* ptr = this._World.Unmanaged.ResolveSystemState(this._SystemHandle);
			if (ptr == null)
			{
				throw new NullReferenceException("SystemState is null");
			}
			return ptr;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000B580 File Offset: 0x00009780
		public unsafe List<EntityQuery> EntityQueries
		{
			get
			{
				UnsafeList<EntityQuery> unsafeList = *this.Resolve()->EntityQueries;
				List<EntityQuery> list = new List<EntityQuery>();
				foreach (EntityQuery entityQuery in unsafeList)
				{
					list.Add(entityQuery);
				}
				return list;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000B5E8 File Offset: 0x000097E8
		public unsafe List<ComponentType> ReadOnlyTypes
		{
			get
			{
				List<ComponentType> list = new List<ComponentType>();
				foreach (TypeIndex typeIndex in this.Resolve()->m_JobDependencyForReadingSystems)
				{
					list.Add(ComponentType.ReadOnly(typeIndex));
				}
				return list;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000B64C File Offset: 0x0000984C
		public unsafe List<ComponentType> ReadWriteTypes
		{
			get
			{
				List<ComponentType> list = new List<ComponentType>();
				foreach (TypeIndex typeIndex in this.Resolve()->m_JobDependencyForWritingSystems)
				{
					list.Add(ComponentType.ReadWrite(typeIndex));
				}
				return list;
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000B6B0 File Offset: 0x000098B0
		public override string ToString()
		{
			return SystemState_.GetName(this._SystemHandle, this._World);
		}

		// Token: 0x0400009B RID: 155
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private SystemHandle _SystemHandle;

		// Token: 0x0400009C RID: 156
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private World _World;
	}
}
