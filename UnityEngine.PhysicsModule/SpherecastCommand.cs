using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000044 RID: 68
	[NativeHeader("Runtime/Jobs/ScriptBindings/JobsBindingsTypes.h")]
	[NativeHeader("Modules/Physics/BatchCommands/SpherecastCommand.h")]
	public struct SpherecastCommand
	{
		// Token: 0x060004DB RID: 1243 RVA: 0x00006E48 File Offset: 0x00005048
		public SpherecastCommand(Vector3 origin, float radius, Vector3 direction, QueryParameters queryParameters, float distance = 3.4028235E+38f)
		{
			this.origin = origin;
			this.direction = direction;
			this.radius = radius;
			this.distance = distance;
			this.physicsScene = Physics.defaultPhysicsScene;
			this.queryParameters = queryParameters;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00006E80 File Offset: 0x00005080
		public SpherecastCommand(PhysicsScene physicsScene, Vector3 origin, float radius, Vector3 direction, QueryParameters queryParameters, float distance = 3.4028235E+38f)
		{
			this.origin = origin;
			this.direction = direction;
			this.radius = radius;
			this.distance = distance;
			this.physicsScene = physicsScene;
			this.queryParameters = queryParameters;
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00006EB5 File Offset: 0x000050B5
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x00006EBD File Offset: 0x000050BD
		public Vector3 origin { readonly get; set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00006EC6 File Offset: 0x000050C6
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x00006ECE File Offset: 0x000050CE
		public float radius { readonly get; set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00006ED7 File Offset: 0x000050D7
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x00006EDF File Offset: 0x000050DF
		public Vector3 direction { readonly get; set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00006EE8 File Offset: 0x000050E8
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x00006EF0 File Offset: 0x000050F0
		public float distance { readonly get; set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x00006EF9 File Offset: 0x000050F9
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x00006F01 File Offset: 0x00005101
		public PhysicsScene physicsScene { readonly get; set; }

		// Token: 0x060004E7 RID: 1255 RVA: 0x00006F0C File Offset: 0x0000510C
		public static JobHandle ScheduleBatch(NativeArray<SpherecastCommand> commands, NativeArray<RaycastHit> results, int minCommandsPerJob, int maxHits, JobHandle dependsOn = default(JobHandle))
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
					BatchQueryJob<SpherecastCommand, RaycastHit> batchQueryJob = new BatchQueryJob<SpherecastCommand, RaycastHit>(commands, results);
					JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<BatchQueryJob<SpherecastCommand, RaycastHit>>(ref batchQueryJob), BatchQueryJobStruct<BatchQueryJob<SpherecastCommand, RaycastHit>>.Initialize(), dependsOn, ScheduleMode.Batched);
					jobHandle = SpherecastCommand.ScheduleSpherecastBatch(ref jobScheduleParameters, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<SpherecastCommand>(commands), commands.Length, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<RaycastHit>(results), results.Length, minCommandsPerJob, maxHits);
				}
			}
			return jobHandle;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00006FB4 File Offset: 0x000051B4
		public static JobHandle ScheduleBatch(NativeArray<SpherecastCommand> commands, NativeArray<RaycastHit> results, int minCommandsPerJob, JobHandle dependsOn = default(JobHandle))
		{
			return SpherecastCommand.ScheduleBatch(commands, results, minCommandsPerJob, 1, dependsOn);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00006FD0 File Offset: 0x000051D0
		[FreeFunction("ScheduleSpherecastCommandBatch", ThrowsException = true)]
		private unsafe static JobHandle ScheduleSpherecastBatch(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob, int maxHits)
		{
			JobHandle jobHandle;
			SpherecastCommand.ScheduleSpherecastBatch_Injected(ref parameters, commands, commandLen, result, resultLen, minCommandsPerJob, maxHits, out jobHandle);
			return jobHandle;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00006FF0 File Offset: 0x000051F0
		[Obsolete("This struct signature is no longer supported. Use struct with a QueryParameters instead", false)]
		public SpherecastCommand(Vector3 origin, float radius, Vector3 direction, float distance = 3.4028235E+38f, int layerMask = -5)
		{
			this.origin = origin;
			this.direction = direction;
			this.radius = radius;
			this.distance = distance;
			this.physicsScene = Physics.defaultPhysicsScene;
			this.queryParameters = QueryParameters.Default;
			this.layerMask = layerMask;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00007040 File Offset: 0x00005240
		[Obsolete("This struct signature is no longer supported. Use struct with a QueryParameters instead", false)]
		public SpherecastCommand(PhysicsScene physicsScene, Vector3 origin, float radius, Vector3 direction, float distance = 3.4028235E+38f, int layerMask = -5)
		{
			this.origin = origin;
			this.direction = direction;
			this.radius = radius;
			this.distance = distance;
			this.physicsScene = physicsScene;
			this.queryParameters = QueryParameters.Default;
			this.layerMask = layerMask;
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x0000708C File Offset: 0x0000528C
		// (set) Token: 0x060004ED RID: 1261 RVA: 0x000070A9 File Offset: 0x000052A9
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

		// Token: 0x060004EE RID: 1262
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void ScheduleSpherecastBatch_Injected(ref JobsUtility.JobScheduleParameters parameters, void* commands, int commandLen, void* result, int resultLen, int minCommandsPerJob, int maxHits, out JobHandle ret);

		// Token: 0x04000111 RID: 273
		public QueryParameters queryParameters;
	}
}
