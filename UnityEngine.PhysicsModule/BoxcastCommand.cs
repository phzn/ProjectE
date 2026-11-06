using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000046 RID: 70
	[NativeHeader("Modules/Physics/BatchCommands/BoxcastCommand.h")]
	[NativeHeader("Runtime/Jobs/ScriptBindings/JobsBindingsTypes.h")]
	public struct BoxcastCommand
	{
		// Token: 0x06000505 RID: 1285 RVA: 0x00007368 File Offset: 0x00005568
		public BoxcastCommand(Vector3 center, Vector3 halfExtents, Quaternion orientation, Vector3 direction, QueryParameters queryParameters, float distance = 3.4028235E+38f)
		{
			this.center = center;
			this.halfExtents = halfExtents;
			this.orientation = orientation;
			this.direction = direction;
			this.distance = distance;
			this.physicsScene = Physics.defaultPhysicsScene;
			this.queryParameters = queryParameters;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000073B4 File Offset: 0x000055B4
		public BoxcastCommand(PhysicsScene physicsScene, Vector3 center, Vector3 halfExtents, Quaternion orientation, Vector3 direction, QueryParameters queryParameters, float distance = 3.4028235E+38f)
		{
			this.center = center;
			this.halfExtents = halfExtents;
			this.orientation = orientation;
			this.direction = direction;
			this.distance = distance;
			this.physicsScene = physicsScene;
			this.queryParameters = queryParameters;
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x000073F2 File Offset: 0x000055F2
		// (set) Token: 0x06000508 RID: 1288 RVA: 0x000073FA File Offset: 0x000055FA
		public Vector3 center { readonly get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x00007403 File Offset: 0x00005603
		// (set) Token: 0x0600050A RID: 1290 RVA: 0x0000740B File Offset: 0x0000560B
		public Vector3 halfExtents { readonly get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x00007414 File Offset: 0x00005614
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x0000741C File Offset: 0x0000561C
		public Quaternion orientation { readonly get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x00007425 File Offset: 0x00005625
		// (set) Token: 0x0600050E RID: 1294 RVA: 0x0000742D File Offset: 0x0000562D
		public Vector3 direction { readonly get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x00007436 File Offset: 0x00005636
		// (set) Token: 0x06000510 RID: 1296 RVA: 0x0000743E File Offset: 0x0000563E
		public float distance { readonly get; set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x00007447 File Offset: 0x00005647
		// (set) Token: 0x06000512 RID: 1298 RVA: 0x0000744F File Offset: 0x0000564F
		public PhysicsScene physicsScene { readonly get; set; }

		// Token: 0x06000513 RID: 1299 RVA: 0x00007458 File Offset: 0x00005658
		public static JobHandle ScheduleBatch(NativeArray<BoxcastCommand> commands, NativeArray<RaycastHit> results, int minCommandsPerJob, int maxHits, JobHandle dependsOn = default(JobHandle))
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
					BatchQueryJob<BoxcastCommand, RaycastHit> batchQueryJob = new BatchQueryJob<BoxcastCommand, RaycastHit>(commands, results);
					JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<BatchQueryJob<BoxcastCommand, RaycastHit>>(ref batchQueryJob), BatchQueryJobStruct<BatchQueryJob<BoxcastCommand, RaycastHit>>.Initialize(), dependsOn, ScheduleMode.Batched);
					jobHandle = BoxcastCommand.ScheduleBoxcastBatch(ref jobScheduleParameters, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<BoxcastCommand>(commands), commands.Length, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<RaycastHit>(results), results.Length, minCommandsPerJob, maxHits);
				}
			}
			return jobHandle;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00007500 File Offset: 0x00005700
		public static JobHandle ScheduleBatch(NativeArray<BoxcastCommand> commands, NativeArray<RaycastHit> results, int minCommandsPerJob, JobHandle dependsOn = default(JobHandle))
		{
			return BoxcastCommand.ScheduleBatch(commands, results, minCommandsPerJob, 1, dependsOn);
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0000751C File Offset: 0x0000571C
		[FreeFunction("ScheduleBoxcastCommandBatch", ThrowsException = true)]
		private unsafe static JobHandle ScheduleBoxcastBatch(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob, int maxHits)
		{
			JobHandle jobHandle;
			BoxcastCommand.ScheduleBoxcastBatch_Injected(ref parameters, commands, commandLen, result, resultLen, minCommandsPerJob, maxHits, out jobHandle);
			return jobHandle;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0000753C File Offset: 0x0000573C
		[Obsolete("This struct signature is no longer supported. Use struct with a QueryParameters instead", false)]
		public BoxcastCommand(Vector3 center, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance = 3.4028235E+38f, int layerMask = -5)
		{
			this.center = center;
			this.halfExtents = halfExtents;
			this.orientation = orientation;
			this.direction = direction;
			this.distance = distance;
			this.physicsScene = Physics.defaultPhysicsScene;
			this.queryParameters = QueryParameters.Default;
			this.layerMask = layerMask;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00007594 File Offset: 0x00005794
		[Obsolete("This struct signature is no longer supported. Use struct with a QueryParameters instead", false)]
		public BoxcastCommand(PhysicsScene physicsScene, Vector3 center, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance = 3.4028235E+38f, int layerMask = -5)
		{
			this.center = center;
			this.halfExtents = halfExtents;
			this.orientation = orientation;
			this.direction = direction;
			this.distance = distance;
			this.physicsScene = physicsScene;
			this.queryParameters = QueryParameters.Default;
			this.layerMask = layerMask;
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x000075EC File Offset: 0x000057EC
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x00007609 File Offset: 0x00005809
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

		// Token: 0x0600051A RID: 1306
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void ScheduleBoxcastBatch_Injected(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob, int maxHits, out JobHandle ret);

		// Token: 0x0400011F RID: 287
		public QueryParameters queryParameters;
	}
}
