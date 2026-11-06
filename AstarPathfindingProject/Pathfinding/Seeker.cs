using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000024 RID: 36
	[AddComponentMenu("Pathfinding/Seeker")]
	[DisallowMultipleComponent]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/seeker.html")]
	public class Seeker : VersionedMonoBehaviour
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000A659 File Offset: 0x00008859
		// (set) Token: 0x06000227 RID: 551 RVA: 0x0000A661 File Offset: 0x00008861
		[Obsolete("Use tagEntryCosts or tagCostMultipliers instead", false)]
		public uint[] tagPenalties
		{
			get
			{
				return this.tagEntryCosts;
			}
			set
			{
				this.tagEntryCosts = value;
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000A66C File Offset: 0x0000886C
		public Seeker()
		{
			this.onPathDelegate = new OnPathDelegate(this.OnPathComplete);
			this.onPartialPathDelegate = new OnPathDelegate(this.OnPartialPathComplete);
			this.tagCostMultipliers = new float[32];
			for (int i = 0; i < this.tagCostMultipliers.Length; i++)
			{
				this.tagCostMultipliers[i] = 1f;
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000A713 File Offset: 0x00008913
		protected override void Awake()
		{
			base.Awake();
			this.startEndModifier.Awake(this);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000A727 File Offset: 0x00008927
		public Path GetCurrentPath()
		{
			return this.path;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000A72F File Offset: 0x0000892F
		public void CancelCurrentPathRequest(bool pool = true)
		{
			if (!this.IsDone())
			{
				this.path.FailWithError("Canceled by script (Seeker.CancelCurrentPathRequest)");
				if (pool)
				{
					this.path.Claim(this.path);
					this.path.Release(this.path, false);
				}
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000A76F File Offset: 0x0000896F
		private void OnDestroy()
		{
			this.ReleaseClaimedPath();
			this.startEndModifier.OnDestroy(this);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000A783 File Offset: 0x00008983
		private void ReleaseClaimedPath()
		{
			if (this.prevPath != null)
			{
				this.prevPath.Release(this, true);
				this.prevPath = null;
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000A7A4 File Offset: 0x000089A4
		public void RegisterModifier(IPathModifier modifier)
		{
			if (!this.modifiers.Contains(modifier))
			{
				this.modifiers.Add(modifier);
				this.modifiers.Sort((IPathModifier a, IPathModifier b) => a.Order.CompareTo(b.Order));
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000A7F5 File Offset: 0x000089F5
		public void DeregisterModifier(IPathModifier modifier)
		{
			this.modifiers.Remove(modifier);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000A804 File Offset: 0x00008A04
		private void ForceRegisterModifiers()
		{
			base.GetComponents<IPathModifier>(this.modifiers);
			this.modifiers.Sort((IPathModifier a, IPathModifier b) => a.Order.CompareTo(b.Order));
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000A83C File Offset: 0x00008A3C
		public void PostProcess(Path path)
		{
			this.RunModifiers(Seeker.ModifierPass.PostProcess, path);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000A848 File Offset: 0x00008A48
		public void RunModifiers(Seeker.ModifierPass pass, Path path)
		{
			if (!Application.isPlaying)
			{
				this.ForceRegisterModifiers();
			}
			if (pass == Seeker.ModifierPass.PreProcess)
			{
				if (this.preProcessPath != null)
				{
					this.preProcessPath(path);
				}
				for (int i = 0; i < this.modifiers.Count; i++)
				{
					this.modifiers[i].PreProcess(path);
				}
				return;
			}
			if (pass == Seeker.ModifierPass.PostProcess)
			{
				if (this.postProcessPath != null)
				{
					this.postProcessPath(path);
				}
				for (int j = 0; j < this.modifiers.Count; j++)
				{
					this.modifiers[j].Apply(path);
				}
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000A8E2 File Offset: 0x00008AE2
		public bool IsDone()
		{
			return this.path == null || this.path.PipelineState >= PathState.Returning;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000A8FF File Offset: 0x00008AFF
		private void OnPathComplete(Path path)
		{
			this.OnPathComplete(path, true, true);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000A90C File Offset: 0x00008B0C
		private void OnPathComplete(Path p, bool runModifiers, bool sendCallbacks)
		{
			if (p != null && p != this.path && sendCallbacks)
			{
				return;
			}
			if (this == null || p == null || p != this.path)
			{
				return;
			}
			if (!this.path.error && runModifiers)
			{
				this.RunModifiers(Seeker.ModifierPass.PostProcess, this.path);
			}
			if (sendCallbacks)
			{
				p.Claim(this);
				if (this.tmpPathCallback != null || this.pathCallback != null)
				{
					if (this.tmpPathCallback != null)
					{
						this.tmpPathCallback(p);
					}
					if (this.pathCallback != null)
					{
						this.pathCallback(p);
					}
				}
				if (this.prevPath != null)
				{
					this.prevPath.Release(this, true);
				}
				this.prevPath = p;
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000A9C3 File Offset: 0x00008BC3
		private void OnPartialPathComplete(Path p)
		{
			this.OnPathComplete(p, true, false);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000A9CE File Offset: 0x00008BCE
		private void OnMultiPathComplete(Path p)
		{
			this.OnPathComplete(p, false, true);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000A9D9 File Offset: 0x00008BD9
		[Obsolete("Use the overload that takes a callback instead")]
		public Path StartPath(Vector3 start, Vector3 end)
		{
			return this.StartPath(start, end, null);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000A9E4 File Offset: 0x00008BE4
		public Path StartPath(Vector3 start, Vector3 end, OnPathDelegate callback)
		{
			return this.StartPath(ABPath.Construct(start, end, null), callback);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000A9F5 File Offset: 0x00008BF5
		public Path StartPath(Vector3 start, Vector3 end, OnPathDelegate callback, GraphMask graphMask)
		{
			return this.StartPath(ABPath.Construct(start, end, null), callback, graphMask);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000AA08 File Offset: 0x00008C08
		public Path StartPath(Path p)
		{
			return this.StartPath(p, null);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000AA12 File Offset: 0x00008C12
		public Path StartPath(Path p, OnPathDelegate callback)
		{
			if (p.traversalConstraint.graphMask.containsAllGraphs)
			{
				p.traversalConstraint.graphMask = this.graphMask;
			}
			this.StartPathInternal(p, callback);
			return p;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000AA40 File Offset: 0x00008C40
		public Path StartPath(Path p, OnPathDelegate callback, GraphMask graphMask)
		{
			p.traversalConstraint.graphMask = graphMask;
			this.StartPathInternal(p, callback);
			return p;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000AA58 File Offset: 0x00008C58
		private void StartPathInternal(Path p, OnPathDelegate callback)
		{
			MultiTargetPath multiTargetPath = p as MultiTargetPath;
			if (multiTargetPath != null)
			{
				OnPathDelegate[] array = new OnPathDelegate[multiTargetPath.targetPoints.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.onPartialPathDelegate;
				}
				multiTargetPath.callbacks = array;
				p.callback = (OnPathDelegate)Delegate.Combine(p.callback, new OnPathDelegate(this.OnMultiPathComplete));
			}
			else
			{
				p.callback = (OnPathDelegate)Delegate.Combine(p.callback, this.onPathDelegate);
			}
			p.traversalConstraint.tags = this.traversableTags;
			if (p.traversalCosts.tagCostMultipliers == null)
			{
				p.traversalCosts.tagCostMultipliers = this.tagCostMultipliers;
			}
			if (p.traversalCosts.tagEntryCosts == null)
			{
				p.traversalCosts.tagEntryCosts = this.tagEntryCosts;
			}
			if (p.traversalConstraint.traversalProvider == null)
			{
				p.traversalConstraint.traversalProvider = this.traversalProvider;
			}
			if (p.traversalCosts.traversalProvider == null)
			{
				p.traversalCosts.traversalProvider = this.traversalProvider;
			}
			if (this.path != null && this.path.PipelineState <= PathState.Processing && this.path.CompleteState != PathCompleteState.Error && this.lastPathID == (uint)this.path.pathID)
			{
				this.path.FailWithError("Canceled path because a new one was requested.\nThis happens when a new path is requested from the seeker when one was already being calculated.\nFor example if a unit got a new order, you might request a new path directly instead of waiting for the now invalid path to be calculated. Which is probably what you want.\nIf you are getting this a lot, you might want to consider how you are scheduling path requests.");
			}
			this.path = p;
			this.tmpPathCallback = callback;
			this.lastPathID = (uint)this.path.pathID;
			this.RunModifiers(Seeker.ModifierPass.PreProcess, this.path);
			AstarPath.StartPath(this.path, false, false);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000ABE4 File Offset: 0x00008DE4
		public MultiTargetPath StartMultiTargetPath(Vector3 start, Vector3[] endPoints, bool pathsForAll, OnPathDelegate callback, GraphMask graphMask)
		{
			MultiTargetPath multiTargetPath = MultiTargetPath.Construct(start, endPoints, null, null);
			multiTargetPath.pathsForAll = pathsForAll;
			this.StartPath(multiTargetPath, callback, graphMask);
			return multiTargetPath;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000AC0F File Offset: 0x00008E0F
		public MultiTargetPath StartMultiTargetPath(Vector3 start, Vector3[] endPoints, bool pathsForAll, OnPathDelegate callback)
		{
			return this.StartMultiTargetPath(start, endPoints, pathsForAll, callback, this.graphMask);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000AC24 File Offset: 0x00008E24
		public MultiTargetPath StartMultiTargetPath(Vector3[] startPoints, Vector3 end, bool pathsForAll, OnPathDelegate callback, GraphMask graphMask)
		{
			MultiTargetPath multiTargetPath = MultiTargetPath.Construct(startPoints, end, null, null);
			multiTargetPath.pathsForAll = pathsForAll;
			this.StartPath(multiTargetPath, callback, graphMask);
			return multiTargetPath;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000AC4F File Offset: 0x00008E4F
		public MultiTargetPath StartMultiTargetPath(Vector3[] startPoints, Vector3 end, bool pathsForAll, OnPathDelegate callback)
		{
			return this.StartMultiTargetPath(startPoints, end, pathsForAll, callback, this.graphMask);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000AC62 File Offset: 0x00008E62
		protected override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			if (this.graphMaskCompatibility != -1)
			{
				this.graphMask = new GraphMask((uint)this.graphMaskCompatibility);
				this.graphMaskCompatibility = -1;
			}
			base.OnUpgradeSerializedData(ref migrations, unityThread);
		}

		// Token: 0x04000124 RID: 292
		public bool drawGizmos = true;

		// Token: 0x04000125 RID: 293
		public bool detailedGizmos;

		// Token: 0x04000126 RID: 294
		public StartEndModifier startEndModifier = new StartEndModifier();

		// Token: 0x04000127 RID: 295
		public int traversableTags = -1;

		// Token: 0x04000128 RID: 296
		public float[] tagCostMultipliers;

		// Token: 0x04000129 RID: 297
		[FormerlySerializedAs("tagPenalties")]
		public uint[] tagEntryCosts = new uint[32];

		// Token: 0x0400012A RID: 298
		public GraphMask graphMask = GraphMask.everything;

		// Token: 0x0400012B RID: 299
		public ITraversalProvider traversalProvider;

		// Token: 0x0400012C RID: 300
		[FormerlySerializedAs("graphMask")]
		private int graphMaskCompatibility = -1;

		// Token: 0x0400012D RID: 301
		[Obsolete("Pass a callback every time to the StartPath method instead, or use ai.SetPath+ai.pathPending on the movement script. You can cache it in your own script if you want to avoid the GC allocation of creating a new delegate.")]
		public OnPathDelegate pathCallback;

		// Token: 0x0400012E RID: 302
		public OnPathDelegate preProcessPath;

		// Token: 0x0400012F RID: 303
		public OnPathDelegate postProcessPath;

		// Token: 0x04000130 RID: 304
		[NonSerialized]
		protected Path path;

		// Token: 0x04000131 RID: 305
		[NonSerialized]
		private Path prevPath;

		// Token: 0x04000132 RID: 306
		private readonly OnPathDelegate onPathDelegate;

		// Token: 0x04000133 RID: 307
		private readonly OnPathDelegate onPartialPathDelegate;

		// Token: 0x04000134 RID: 308
		private OnPathDelegate tmpPathCallback;

		// Token: 0x04000135 RID: 309
		protected uint lastPathID;

		// Token: 0x04000136 RID: 310
		private readonly List<IPathModifier> modifiers = new List<IPathModifier>();

		// Token: 0x02000025 RID: 37
		public enum ModifierPass
		{
			// Token: 0x04000138 RID: 312
			PreProcess,
			// Token: 0x04000139 RID: 313
			PostProcess = 2
		}
	}
}
