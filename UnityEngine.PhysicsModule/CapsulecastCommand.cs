using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000045 RID: 69
	[NativeHeader("Modules/Physics/BatchCommands/CapsulecastCommand.h")]
	[NativeHeader("Runtime/Jobs/ScriptBindings/JobsBindingsTypes.h")]
	public struct CapsulecastCommand
	{
		// Token: 0x060004EF RID: 1263 RVA: 0x000070B8 File Offset: 0x000052B8
		public CapsulecastCommand(Vector3 p1, Vector3 p2, float radius, Vector3 direction, QueryParameters queryParameters, float distance = 3.4028235E+38f)
		{
			this.point1 = p1;
			this.point2 = p2;
			this.direction = direction;
			this.radius = radius;
			this.distance = distance;
			this.physicsScene = Physics.defaultPhysicsScene;
			this.queryParameters = queryParameters;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00007104 File Offset: 0x00005304
		public CapsulecastCommand(PhysicsScene physicsScene, Vector3 p1, Vector3 p2, float radius, Vector3 direction, QueryParameters queryParameters, float distance = 3.4028235E+38f)
		{
			this.point1 = p1;
			this.point2 = p2;
			this.direction = direction;
			this.radius = radius;
			this.distance = distance;
			this.physicsScene = physicsScene;
			this.queryParameters = queryParameters;
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x00007142 File Offset: 0x00005342
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x0000714A File Offset: 0x0000534A
		public Vector3 point1 { readonly get; set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x00007153 File Offset: 0x00005353
		// (set) Token: 0x060004F4 RID: 1268 RVA: 0x0000715B File Offset: 0x0000535B
		public Vector3 point2 { readonly get; set; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x00007164 File Offset: 0x00005364
		// (set) Token: 0x060004F6 RID: 1270 RVA: 0x0000716C File Offset: 0x0000536C
		public float radius { readonly get; set; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x00007175 File Offset: 0x00005375
		// (set) Token: 0x060004F8 RID: 1272 RVA: 0x0000717D File Offset: 0x0000537D
		public Vector3 direction { readonly get; set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00007186 File Offset: 0x00005386
		// (set) Token: 0x060004FA RID: 1274 RVA: 0x0000718E File Offset: 0x0000538E
		public float distance { readonly get; set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x00007197 File Offset: 0x00005397
		// (set) Token: 0x060004FC RID: 1276 RVA: 0x0000719F File Offset: 0x0000539F
		public PhysicsScene physicsScene { readonly get; set; }

		// Token: 0x060004FD RID: 1277 RVA: 0x000071A8 File Offset: 0x000053A8
		public static JobHandle ScheduleBatch(NativeArray<CapsulecastCommand> commands, NativeArray<RaycastHit> results, int minCommandsPerJob, int maxHits, JobHandle dependsOn = default(JobHandle))
		{
			bool flag = maxHits < 1;
			JobHandle jobHandle;
			if (flag)
			{
				Debug.LogWarning("maxHits should be greater than 0.");
				jobHandle = default(JobHandle);
			}
			else
			{
				bool flag2 = results.Length < maxHits * commands.Length;
				if (flag2)
				{
					Debug.LogWarning("The supplied results buffer is too small, there should be at least maxHits space per each command in the batch.");
					jobHandle = default(JobHandle);
				}
				else
				{
					BatchQueryJob<CapsulecastCommand, RaycastHit> batchQueryJob = new BatchQueryJob<CapsulecastCommand, RaycastHit>(commands, results);
					JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<BatchQueryJob<CapsulecastCommand, RaycastHit>>(ref batchQueryJob), BatchQueryJobStruct<BatchQueryJob<CapsulecastCommand, RaycastHit>>.Initialize(), dependsOn, ScheduleMode.Batched);
					jobHandle = CapsulecastCommand.ScheduleCapsulecastBatch(ref jobScheduleParameters, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<CapsulecastCommand>(commands), commands.Length, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<RaycastHit>(results), results.Length, minCommandsPerJob, maxHits);
				}
			}
			return jobHandle;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00007250 File Offset: 0x00005450
		public static JobHandle ScheduleBatch(NativeArray<CapsulecastCommand> commands, NativeArray<RaycastHit> results, int minCommandsPerJob, JobHandle dependsOn = default(JobHandle))
		{
			return CapsulecastCommand.ScheduleBatch(commands, results, minCommandsPerJob, 1, dependsOn);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0000726C File Offset: 0x0000546C
		[FreeFunction("ScheduleCapsulecastCommandBatch", ThrowsException = true)]
		private unsafe static JobHandle ScheduleCapsulecastBatch(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob, int maxHits)
		{
			JobHandle jobHandle;
			CapsulecastCommand.ScheduleCapsulecastBatch_Injected(ref parameters, commands, commandLen, result, resultLen, minCommandsPerJob, maxHits, out jobHandle);
			return jobHandle;
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0000728C File Offset: 0x0000548C
		[Obsolete("This struct signature is no longer supported. Use struct with a QueryParameters instead", false)]
		public CapsulecastCommand(Vector3 p1, Vector3 p2, float radius, Vector3 direction, float distance = 3.4028235E+38f, int layerMask = -5)
		{
			this.point1 = p1;
			this.point2 = p2;
			this.direction = direction;
			this.radius = radius;
			this.distance = distance;
			this.physicsScene = Physics.defaultPhysicsScene;
			this.queryParameters = QueryParameters.Default;
			this.layerMask = layerMask;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x000072E4 File Offset: 0x000054E4
		[Obsolete("This struct signature is no longer supported. Use struct with a QueryParameters instead", false)]
		public CapsulecastCommand(PhysicsScene physicsScene, Vector3 p1, Vector3 p2, float radius, Vector3 direction, float distance = 3.4028235E+38f, int layerMask = -5)
		{
			this.point1 = p1;
			this.point2 = p2;
			this.direction = direction;
			this.radius = radius;
			this.distance = distance;
			this.physicsScene = physicsScene;
			this.queryParameters = QueryParameters.Default;
			this.layerMask = layerMask;
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x0000733C File Offset: 0x0000553C
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x00007359 File Offset: 0x00005559
		[Obsolete("Layer Mask is now a part of QueryParameters struct", false)]
		public int layerMask
		{
			get
			{
				return this.queryParameters.layerMask;
			}
			set
			{
				this.queryParameters.layerMask = value;
			}
		}

		// Token: 0x06000504 RID: 1284
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void ScheduleCapsulecastBatch_Injected(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob, int maxHits, out JobHandle ret);

		// Token: 0x04000118 RID: 280
		public QueryParameters queryParameters;
	}
}
