using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.VFX
{
	// Token: 0x02000018 RID: 24
	[NativeType(Header = "Modules/VFX/Public/VFXSpawnerState.h")]
	[RequiredByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class VFXSpawnerState : IDisposable
	{
		// Token: 0x0600008A RID: 138 RVA: 0x00002840 File Offset: 0x00000A40
		public VFXSpawnerState()
			: this(VFXSpawnerState.Internal_Create(), true)
		{
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00002850 File Offset: 0x00000A50
		internal VFXSpawnerState(IntPtr ptr, bool owner)
		{
			this.m_Ptr = ptr;
			this.m_Owner = owner;
		}

		// Token: 0x0600008C RID: 140
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr Internal_Create();

		// Token: 0x0600008D RID: 141 RVA: 0x00002868 File Offset: 0x00000A68
		[RequiredByNativeCode]
		internal static VFXSpawnerState CreateSpawnerStateWrapper()
		{
			VFXSpawnerState vfxspawnerState = new VFXSpawnerState(IntPtr.Zero, false);
			vfxspawnerState.PrepareWrapper();
			return vfxspawnerState;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00002890 File Offset: 0x00000A90
		private void PrepareWrapper()
		{
			bool owner = this.m_Owner;
			if (owner)
			{
				throw new Exception("VFXSpawnerState : SetWrapValue is reserved to CreateWrapper object");
			}
			bool flag = this.m_WrapEventAttribute != null;
			if (flag)
			{
				throw new Exception("VFXSpawnerState : Unexpected calling twice prepare wrapper");
			}
			this.m_WrapEventAttribute = VFXEventAttribute.CreateEventAttributeWrapper();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000028D8 File Offset: 0x00000AD8
		[RequiredByNativeCode]
		internal void SetWrapValue(IntPtr ptrToSpawnerState, IntPtr ptrToEventAttribute)
		{
			bool owner = this.m_Owner;
			if (owner)
			{
				throw new Exception("VFXSpawnerState : SetWrapValue is reserved to CreateWrapper object");
			}
			bool flag = this.m_WrapEventAttribute == null;
			if (flag)
			{
				throw new Exception("VFXSpawnerState : Missing PrepareWrapper");
			}
			this.m_Ptr = ptrToSpawnerState;
			this.m_WrapEventAttribute.SetWrapValue(ptrToEventAttribute);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00002928 File Offset: 0x00000B28
		internal IntPtr GetPtr()
		{
			return this.m_Ptr;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00002940 File Offset: 0x00000B40
		private void Release()
		{
			bool flag = this.m_Ptr != IntPtr.Zero && this.m_Owner;
			if (flag)
			{
				VFXSpawnerState.Internal_Destroy(this.m_Ptr);
			}
			this.m_Ptr = IntPtr.Zero;
			this.m_WrapEventAttribute = null;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00002990 File Offset: 0x00000B90
		~VFXSpawnerState()
		{
			this.Release();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000029C0 File Offset: 0x00000BC0
		public void Dispose()
		{
			this.Release();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000094 RID: 148
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Destroy(IntPtr ptr);

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000029D4 File Offset: 0x00000BD4
		// (set) Token: 0x06000096 RID: 150 RVA: 0x000029EF File Offset: 0x00000BEF
		public bool playing
		{
			get
			{
				return this.loopState == VFXSpawnerLoopState.Looping;
			}
			set
			{
				this.loopState = (value ? VFXSpawnerLoopState.Looping : VFXSpawnerLoopState.Finished);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000097 RID: 151
		public extern bool newLoop
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000098 RID: 152
		// (set) Token: 0x06000099 RID: 153
		public extern VFXSpawnerLoopState loopState
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600009A RID: 154
		// (set) Token: 0x0600009B RID: 155
		public extern float spawnCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600009C RID: 156
		// (set) Token: 0x0600009D RID: 157
		public extern float deltaTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600009E RID: 158
		// (set) Token: 0x0600009F RID: 159
		public extern float totalTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000A0 RID: 160
		// (set) Token: 0x060000A1 RID: 161
		public extern float delayBeforeLoop
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000A2 RID: 162
		// (set) Token: 0x060000A3 RID: 163
		public extern float loopDuration
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000A4 RID: 164
		// (set) Token: 0x060000A5 RID: 165
		public extern float delayAfterLoop
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000A6 RID: 166
		// (set) Token: 0x060000A7 RID: 167
		public extern int loopIndex
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000A8 RID: 168
		// (set) Token: 0x060000A9 RID: 169
		public extern int loopCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060000AA RID: 170
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern VFXEventAttribute Internal_GetVFXEventAttribute();

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00002A00 File Offset: 0x00000C00
		public VFXEventAttribute vfxEventAttribute
		{
			get
			{
				bool flag = !this.m_Owner && this.m_WrapEventAttribute != null;
				VFXEventAttribute vfxeventAttribute;
				if (flag)
				{
					vfxeventAttribute = this.m_WrapEventAttribute;
				}
				else
				{
					vfxeventAttribute = this.Internal_GetVFXEventAttribute();
				}
				return vfxeventAttribute;
			}
		}

		// Token: 0x0400011B RID: 283
		private IntPtr m_Ptr;

		// Token: 0x0400011C RID: 284
		private bool m_Owner;

		// Token: 0x0400011D RID: 285
		private VFXEventAttribute m_WrapEventAttribute;
	}
}
