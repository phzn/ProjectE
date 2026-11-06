using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Jobs
{
	// Token: 0x02000173 RID: 371
	public struct JobRaycastAll
	{
		// Token: 0x06000BA5 RID: 2981 RVA: 0x00040AE0 File Offset: 0x0003ECE0
		public JobRaycastAll(NativeArray<RaycastCommand> commands, NativeArray<RaycastHit> results, PhysicsScene physicsScene, int maxHits, Allocator allocator, JobDependencyTracker dependencyTracker, float minStep = 0.0001f)
		{
			if (maxHits <= 0)
			{
				throw new ArgumentException("maxHits should be greater than zero");
			}
			if (results.Length < commands.Length * maxHits)
			{
				throw new ArgumentException("Results array length does not match maxHits count");
			}
			if (minStep < 0f)
			{
				throw new ArgumentException("minStep should be more or equal to zero");
			}
			this.results = results;
			this.maxHits = maxHits;
			this.minStep = minStep;
			this.commands = commands;
			this.physicsScene = physicsScene;
			this.semiResults = dependencyTracker.NewNativeArray<RaycastHit>(maxHits * commands.Length, allocator, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00040B70 File Offset: 0x0003ED70
		public JobHandle Schedule(JobHandle dependency)
		{
			for (int i = 0; i < this.maxHits; i++)
			{
				NativeArray<RaycastHit> subArray = this.semiResults.GetSubArray(i * this.commands.Length, this.commands.Length);
				dependency = RaycastCommand.ScheduleBatch(this.commands, subArray, 128, dependency);
				if (i < this.maxHits - 1)
				{
					dependency = new JobRaycastAll.JobCreateCommands
					{
						commands = this.commands,
						raycastHits = subArray,
						minStep = this.minStep,
						physicsScene = this.physicsScene
					}.Schedule(this.commands.Length, 256, dependency);
				}
			}
			return new JobRaycastAll.JobCombineResults
			{
				semiResults = this.semiResults,
				maxHits = this.maxHits,
				results = this.results
			}.Schedule(dependency);
		}

		// Token: 0x04000793 RID: 1939
		private int maxHits;

		// Token: 0x04000794 RID: 1940
		public readonly float minStep;

		// Token: 0x04000795 RID: 1941
		private NativeArray<RaycastHit> results;

		// Token: 0x04000796 RID: 1942
		private NativeArray<RaycastHit> semiResults;

		// Token: 0x04000797 RID: 1943
		private NativeArray<RaycastCommand> commands;

		// Token: 0x04000798 RID: 1944
		public PhysicsScene physicsScene;

		// Token: 0x02000174 RID: 372
		[BurstCompile]
		private struct JobCreateCommands : IJobParallelFor
		{
			// Token: 0x06000BA7 RID: 2983 RVA: 0x00040C60 File Offset: 0x0003EE60
			public void Execute(int index)
			{
				RaycastHit raycastHit = this.raycastHits[index];
				if (raycastHit.normal != default(Vector3))
				{
					RaycastCommand raycastCommand = this.commands[index];
					Vector3 vector = raycastHit.point + raycastCommand.direction.normalized * this.minStep;
					float num = raycastCommand.distance - (vector - raycastCommand.from).magnitude;
					QueryParameters queryParameters = new QueryParameters(raycastCommand.queryParameters.layerMask, false, QueryTriggerInteraction.Ignore, false);
					this.commands[index] = new RaycastCommand(this.physicsScene, vector, raycastCommand.direction, queryParameters, num);
					return;
				}
				this.commands[index] = new RaycastCommand(this.physicsScene, Vector3.zero, Vector3.up, new QueryParameters(0, false, QueryTriggerInteraction.Ignore, false), 1f);
			}

			// Token: 0x04000799 RID: 1945
			public NativeArray<RaycastCommand> commands;

			// Token: 0x0400079A RID: 1946
			[ReadOnly]
			public NativeArray<RaycastHit> raycastHits;

			// Token: 0x0400079B RID: 1947
			public float minStep;

			// Token: 0x0400079C RID: 1948
			public PhysicsScene physicsScene;
		}

		// Token: 0x02000175 RID: 373
		[BurstCompile]
		private struct JobCombineResults : IJob
		{
			// Token: 0x06000BA8 RID: 2984 RVA: 0x00040D50 File Offset: 0x0003EF50
			public void Execute()
			{
				int num = this.semiResults.Length / this.maxHits;
				for (int i = 0; i < num; i++)
				{
					int num2 = 0;
					for (int j = this.maxHits - 1; j >= 0; j--)
					{
						if (math.any(this.semiResults[i + j * num].normal))
						{
							this.results[i + num2] = this.semiResults[i + j * num];
							num2 += num;
						}
					}
				}
			}

			// Token: 0x0400079D RID: 1949
			public int maxHits;

			// Token: 0x0400079E RID: 1950
			[ReadOnly]
			public NativeArray<RaycastHit> semiResults;

			// Token: 0x0400079F RID: 1951
			public NativeArray<RaycastHit> results;
		}
	}
}
