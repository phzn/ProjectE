using System;
using System.Collections.Generic;
using Pathfinding.Jobs;
using Pathfinding.Serialization;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Rules
{
	// Token: 0x02000211 RID: 529
	[JsonOptIn]
	public class GridGraphRules
	{
		// Token: 0x06000DF3 RID: 3571 RVA: 0x000557DF File Offset: 0x000539DF
		public void AddRule(GridGraphRule rule)
		{
			this.rules.Add(rule);
			this.lastHash = -1L;
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x000557F5 File Offset: 0x000539F5
		public void RemoveRule(GridGraphRule rule)
		{
			this.rules.Remove(rule);
			this.lastHash = -1L;
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0005580C File Offset: 0x00053A0C
		public IReadOnlyList<GridGraphRule> GetRules()
		{
			if (this.rules == null)
			{
				this.rules = new List<GridGraphRule>();
			}
			return this.rules.AsReadOnly();
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0005582C File Offset: 0x00053A2C
		private long Hash()
		{
			long num = 196613L;
			for (int i = 0; i < this.rules.Count; i++)
			{
				if (this.rules[i] != null && this.rules[i].enabled)
				{
					num = (num * 1572869L) ^ (long)this.rules[i].Hash;
				}
			}
			return num;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00055894 File Offset: 0x00053A94
		public void RebuildIfNecessary()
		{
			long num = this.Hash();
			if (num == this.lastHash && this.jobSystemCallbacks != null && this.mainThreadCallbacks != null)
			{
				return;
			}
			this.lastHash = num;
			this.Rebuild();
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x000558D0 File Offset: 0x00053AD0
		public void Rebuild()
		{
			this.rules = this.rules ?? new List<GridGraphRule>();
			this.jobSystemCallbacks = this.jobSystemCallbacks ?? new List<Action<GridGraphRules.Context>>[6];
			for (int i = 0; i < this.jobSystemCallbacks.Length; i++)
			{
				if (this.jobSystemCallbacks[i] != null)
				{
					this.jobSystemCallbacks[i].Clear();
				}
			}
			this.mainThreadCallbacks = this.mainThreadCallbacks ?? new List<Action<GridGraphRules.Context>>[6];
			for (int j = 0; j < this.mainThreadCallbacks.Length; j++)
			{
				if (this.mainThreadCallbacks[j] != null)
				{
					this.mainThreadCallbacks[j].Clear();
				}
			}
			for (int k = 0; k < this.rules.Count; k++)
			{
				if (this.rules[k].enabled)
				{
					this.rules[k].Register(this);
				}
			}
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x000559B0 File Offset: 0x00053BB0
		public void DisposeUnmanagedData()
		{
			if (this.rules != null)
			{
				for (int i = 0; i < this.rules.Count; i++)
				{
					if (this.rules[i] != null)
					{
						this.rules[i].DisposeUnmanagedData();
						this.rules[i].SetDirty();
					}
				}
			}
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00055A0C File Offset: 0x00053C0C
		private static void CallActions(List<Action<GridGraphRules.Context>> actions, GridGraphRules.Context context)
		{
			if (actions != null)
			{
				try
				{
					for (int i = 0; i < actions.Count; i++)
					{
						actions[i](context);
					}
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
			}
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00055A54 File Offset: 0x00053C54
		public IEnumerator<JobHandle> ExecuteRule(GridGraphRule.Pass rule, GridGraphRules.Context context)
		{
			if (this.jobSystemCallbacks == null)
			{
				this.Rebuild();
			}
			GridGraphRules.CallActions(this.jobSystemCallbacks[(int)rule], context);
			if (this.mainThreadCallbacks[(int)rule] != null && this.mainThreadCallbacks[(int)rule].Count > 0)
			{
				if (!context.tracker.forceLinearDependencies)
				{
					yield return context.tracker.AllWritesDependency;
				}
				GridGraphRules.CallActions(this.mainThreadCallbacks[(int)rule], context);
			}
			yield break;
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x00055A74 File Offset: 0x00053C74
		public void ExecuteRuleMainThread(GridGraphRule.Pass rule, GridGraphRules.Context context)
		{
			if (this.jobSystemCallbacks == null)
			{
				this.Rebuild();
			}
			if (this.jobSystemCallbacks[(int)rule] != null && this.jobSystemCallbacks[(int)rule].Count > 0)
			{
				throw new Exception(string.Concat(new string[]
				{
					"A job system pass has been added for the ",
					rule.ToString(),
					" pass. ",
					rule.ToString(),
					" only supports main thread callbacks."
				}));
			}
			if (context.tracker != null)
			{
				context.tracker.AllWritesDependency.Complete();
			}
			GridGraphRules.CallActions(this.mainThreadCallbacks[(int)rule], context);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x00055B1C File Offset: 0x00053D1C
		public void AddJobSystemPass(GridGraphRule.Pass pass, Action<GridGraphRules.Context> action)
		{
			if (this.jobSystemCallbacks[(int)pass] == null)
			{
				this.jobSystemCallbacks[(int)pass] = new List<Action<GridGraphRules.Context>>();
			}
			this.jobSystemCallbacks[(int)pass].Add(action);
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00055B50 File Offset: 0x00053D50
		public void AddMainThreadPass(GridGraphRule.Pass pass, Action<GridGraphRules.Context> action)
		{
			if (this.mainThreadCallbacks[(int)pass] == null)
			{
				this.mainThreadCallbacks[(int)pass] = new List<Action<GridGraphRules.Context>>();
			}
			this.mainThreadCallbacks[(int)pass].Add(action);
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00055B84 File Offset: 0x00053D84
		[Obsolete("Use AddJobSystemPass or AddMainThreadPass instead")]
		public void Add(GridGraphRule.Pass pass, Action<GridGraphRules.Context> action)
		{
			this.AddJobSystemPass(pass, action);
		}

		// Token: 0x040009F3 RID: 2547
		private List<Action<GridGraphRules.Context>>[] jobSystemCallbacks;

		// Token: 0x040009F4 RID: 2548
		private List<Action<GridGraphRules.Context>>[] mainThreadCallbacks;

		// Token: 0x040009F5 RID: 2549
		[JsonMember]
		private List<GridGraphRule> rules = new List<GridGraphRule>();

		// Token: 0x040009F6 RID: 2550
		private long lastHash;

		// Token: 0x02000212 RID: 530
		public class Context
		{
			// Token: 0x1700021A RID: 538
			// (get) Token: 0x06000E01 RID: 3585 RVA: 0x00055BA1 File Offset: 0x00053DA1
			public JobDependencyTracker tracker
			{
				get
				{
					return this.data.dependencyTracker;
				}
			}

			// Token: 0x040009F7 RID: 2551
			public GridGraph graph;

			// Token: 0x040009F8 RID: 2552
			public GridGraphScanData data;
		}
	}
}
