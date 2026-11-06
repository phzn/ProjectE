using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI
{
	// Token: 0x0200000C RID: 12
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.ai.navigation@1.1/manual/NavMeshObstacle.html")]
	[NativeHeader("Modules/AI/Components/NavMeshObstacle.bindings.h")]
	[MovedFrom("UnityEngine")]
	public sealed class NavMeshObstacle : Behaviour
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B8 RID: 184
		// (set) Token: 0x060000B9 RID: 185
		public extern float height
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000BA RID: 186
		// (set) Token: 0x060000BB RID: 187
		public extern float radius
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00002A90 File Offset: 0x00000C90
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00002AA6 File Offset: 0x00000CA6
		public Vector3 velocity
		{
			get
			{
				Vector3 vector;
				this.get_velocity_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_velocity_Injected(ref value);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000BE RID: 190
		// (set) Token: 0x060000BF RID: 191
		public extern bool carving
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000C0 RID: 192
		// (set) Token: 0x060000C1 RID: 193
		public extern bool carveOnlyStationary
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000C2 RID: 194
		// (set) Token: 0x060000C3 RID: 195
		[NativeProperty("MoveThreshold")]
		public extern float carvingMoveThreshold
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000C4 RID: 196
		// (set) Token: 0x060000C5 RID: 197
		[NativeProperty("TimeToStationary")]
		public extern float carvingTimeToStationary
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000C6 RID: 198
		// (set) Token: 0x060000C7 RID: 199
		public extern NavMeshObstacleShape shape
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00002AB0 File Offset: 0x00000CB0
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00002AC6 File Offset: 0x00000CC6
		public Vector3 center
		{
			get
			{
				Vector3 vector;
				this.get_center_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_center_Injected(ref value);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00002AD0 File Offset: 0x00000CD0
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00002AE6 File Offset: 0x00000CE6
		public Vector3 size
		{
			[FreeFunction("NavMeshObstacleScriptBindings::GetSize", HasExplicitThis = true)]
			get
			{
				Vector3 vector;
				this.get_size_Injected(out vector);
				return vector;
			}
			[FreeFunction("NavMeshObstacleScriptBindings::SetSize", HasExplicitThis = true)]
			set
			{
				this.set_size_Injected(ref value);
			}
		}

		// Token: 0x060000CC RID: 204
		[FreeFunction("NavMeshObstacleScriptBindings::FitExtents", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void FitExtents();

		// Token: 0x060000CE RID: 206
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_velocity_Injected(out Vector3 ret);

		// Token: 0x060000CF RID: 207
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_velocity_Injected(ref Vector3 value);

		// Token: 0x060000D0 RID: 208
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_center_Injected(out Vector3 ret);

		// Token: 0x060000D1 RID: 209
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_center_Injected(ref Vector3 value);

		// Token: 0x060000D2 RID: 210
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_size_Injected(out Vector3 ret);

		// Token: 0x060000D3 RID: 211
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_size_Injected(ref Vector3 value);
	}
}
