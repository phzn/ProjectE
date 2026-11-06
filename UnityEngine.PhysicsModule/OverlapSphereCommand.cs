using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000048 RID: 72
	[NativeHeader("Modules/Physics/BatchCommands/OverlapSphereCommand.h")]
	public struct OverlapSphereCommand
	{
		// Token: 0x0600052A RID: 1322 RVA: 0x0000773D File Offset: 0x0000593D
		public OverlapSphereCommand(Vector3 point, float radius, QueryParameters queryParameters)
		{
			this.point = point;
			this.radius = radius;
			this.queryParameters = queryParameters;
			this.physicsScene = Physics.defaultPhysicsScene;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00007763 File Offset: 0x00005963
		public OverlapSphereCommand(PhysicsScene physicsScene, Vector3 point, float radius, QueryParameters queryParameters)
		{
			this.physicsScene = physicsScene;
			this.point = point;
			this.radius = radius;
			this.queryParameters = queryParameters;
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00007786 File Offset: 0x00005986
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x0000778E File Offset: 0x0000598E
		public Vector3 point { readonly get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x00007797 File Offset: 0x00005997
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x0000779F File Offset: 0x0000599F
		public float radius { readonly get; set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x000077A8 File Offset: 0x000059A8
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x000077B0 File Offset: 0x000059B0
		public PhysicsScene physicsScene { readonly get; set; }

		// Token: 0x06000532 RID: 1330 RVA: 0x000077BC File Offset: 0x000059BC
		public static JobHandle ScheduleBatch(NativeArray<OverlapSphereCommand> commands, NativeArray<ColliderHit> results, int minCommandsPerJob, int maxHits, JobHandle dependsOn = default(JobHandle))
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
					BatchQueryJob<OverlapSphereCommand, ColliderHit> batchQueryJob = new BatchQueryJob<OverlapSphereCommand, ColliderHit>(commands, results);
					JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<BatchQueryJob<OverlapSphereCommand, ColliderHit>>(ref batchQueryJob), BatchQueryJobStruct<BatchQueryJob<OverlapSphereCommand, ColliderHit>>.Initialize(), dependsOn, ScheduleMode.Batched);
					jobHandle = OverlapSphereCommand.ScheduleOverlapSphereBatch(ref jobScheduleParameters, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<OverlapSphereCommand>(commands), commands.Length, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<ColliderHit>(results), results.Length, minCommandsPerJob, maxHits);
				}
			}
			return jobHandle;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00007864 File Offset: 0x00005A64
		[FreeFunction("ScheduleOverlapSphereCommandBatch", ThrowsException = true)]
		private unsafe static JobHandle ScheduleOverlapSphereBatch(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob, int maxHits)
		{
			JobHandle jobHandle;
			OverlapSphereCommand.ScheduleOverlapSphereBatch_Injected(ref parameters, commands, commandLen, result, resultLen, minCommandsPerJob, maxHits, out jobHandle);
			return jobHandle;
		}

		// Token: 0x06000534 RID: 1332
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void ScheduleOverlapSphereBatch_Injected(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob, int maxHits, out JobHandle ret);

		// Token: 0x04000128 RID: 296
		public QueryParameters queryParameters;
	}
}
