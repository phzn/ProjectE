using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI
{
	// Token: 0x0200001A RID: 26
	[MovedFrom("UnityEngine")]
	[NativeHeader("Modules/AI/NavMeshPath.bindings.h")]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class NavMeshPath
	{
		// Token: 0x06000171 RID: 369 RVA: 0x000032CE File Offset: 0x000014CE
		public NavMeshPath()
		{
			this.m_Ptr = NavMeshPath.InitializeNavMeshPath();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000032E4 File Offset: 0x000014E4
		~NavMeshPath()
		{
			NavMeshPath.DestroyNavMeshPath(this.m_Ptr);
			this.m_Ptr = IntPtr.Zero;
		}

		// Token: 0x06000173 RID: 371
		[FreeFunction("NavMeshPathScriptBindings::InitializeNavMeshPath")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr InitializeNavMeshPath();

		// Token: 0x06000174 RID: 372
		[FreeFunction("NavMeshPathScriptBindings::DestroyNavMeshPath", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyNavMeshPath(IntPtr ptr);

		// Token: 0x06000175 RID: 373
		[FreeFunction("NavMeshPathScriptBindings::GetCornersNonAlloc", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetCornersNonAlloc([Out] Vector3[] results);

		// Token: 0x06000176 RID: 374
		[FreeFunction("NavMeshPathScriptBindings::CalculateCornersInternal", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Vector3[] CalculateCornersInternal();

		// Token: 0x06000177 RID: 375
		[FreeFunction("NavMeshPathScriptBindings::ClearCornersInternal", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ClearCornersInternal();

		// Token: 0x06000178 RID: 376 RVA: 0x00003324 File Offset: 0x00001524
		public void ClearCorners()
		{
			this.ClearCornersInternal();
			this.m_Corners = null;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00003338 File Offset: 0x00001538
		private void CalculateCorners()
		{
			bool flag = this.m_Corners == null;
			if (flag)
			{
				this.m_Corners = this.CalculateCornersInternal();
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00003360 File Offset: 0x00001560
		public Vector3[] corners
		{
			get
			{
				this.CalculateCorners();
				return this.m_Corners;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600017B RID: 379
		public extern NavMeshPathStatus status
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x04000043 RID: 67
		internal IntPtr m_Ptr;

		// Token: 0x04000044 RID: 68
		internal Vector3[] m_Corners;
	}
}
