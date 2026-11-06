using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000051 RID: 81
	internal class SystemDebugView
	{
		// Token: 0x06000233 RID: 563 RVA: 0x0000B164 File Offset: 0x00009364
		public SystemDebugView(SystemHandle mUnmanagedSystem)
		{
			this.m_unmanagedSystem = mUnmanagedSystem;
			this.m_World = null;
			foreach (World world in World.s_AllWorlds)
			{
				if (world.SequenceNumber == (ulong)this.m_unmanagedSystem.m_WorldSeqNo)
				{
					this.m_World = world;
				}
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000B1E0 File Offset: 0x000093E0
		public unsafe SystemDebugView(ComponentSystemBase mManagedSystem)
		{
			if (mManagedSystem != null && mManagedSystem.World != null && mManagedSystem.World.IsCreated)
			{
				this.m_World = mManagedSystem.World;
				this.m_unmanagedSystem = mManagedSystem.CheckedState()->SystemHandle;
				return;
			}
			this.m_unmanagedSystem = default(SystemHandle);
			this.m_World = null;
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000B23C File Offset: 0x0000943C
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public unsafe object UserData
		{
			get
			{
				SystemState* ptr = this.m_World.Unmanaged.ResolveSystemState(this.m_unmanagedSystem);
				if (ptr->m_SystemPtr == null)
				{
					return ptr->ManagedSystem;
				}
				Type typeOfSystem = this.m_World.Unmanaged.GetTypeOfSystem(this.m_unmanagedSystem);
				if (typeOfSystem == null)
				{
					return null;
				}
				object obj = Activator.CreateInstance(typeOfSystem);
				if (obj == null)
				{
					return null;
				}
				GCHandle gchandle = GCHandle.Alloc(obj, GCHandleType.Pinned);
				UnsafeUtility.MemCpy((void*)gchandle.AddrOfPinnedObject(), ptr->m_SystemPtr, (long)Marshal.SizeOf(typeOfSystem));
				gchandle.Free();
				return obj;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000B2D4 File Offset: 0x000094D4
		public SystemState_ SystemState
		{
			get
			{
				if (this.m_World == null)
				{
					return null;
				}
				return SystemState_.FromPointer(this.m_World.Unmanaged.ResolveSystemState(this.m_unmanagedSystem));
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000B30C File Offset: 0x0000950C
		public Type Type
		{
			get
			{
				if (this.m_World == null)
				{
					return null;
				}
				return this.m_World.Unmanaged.GetTypeOfSystem(this.m_unmanagedSystem);
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000B33C File Offset: 0x0000953C
		public override string ToString()
		{
			if (this.m_World == null)
			{
				return "World has been disposed";
			}
			Type typeOfSystem = this.m_World.Unmanaged.GetTypeOfSystem(this.m_unmanagedSystem);
			if (typeOfSystem == null)
			{
				return "System has been disposed";
			}
			return typeOfSystem.Name;
		}

		// Token: 0x04000098 RID: 152
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly World m_World;

		// Token: 0x04000099 RID: 153
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly SystemHandle m_unmanagedSystem;
	}
}
