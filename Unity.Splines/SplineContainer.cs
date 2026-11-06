using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200001F RID: 31
	[AddComponentMenu("Splines/Spline Container")]
	[ExecuteAlways]
	public sealed class SplineContainer : MonoBehaviour, ISplineContainer, ISerializationCallbackReceiver
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000155 RID: 341 RVA: 0x00006800 File Offset: 0x00004A00
		// (remove) Token: 0x06000156 RID: 342 RVA: 0x00006834 File Offset: 0x00004A34
		public static event Action<SplineContainer, int> SplineAdded;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000157 RID: 343 RVA: 0x00006868 File Offset: 0x00004A68
		// (remove) Token: 0x06000158 RID: 344 RVA: 0x0000689C File Offset: 0x00004A9C
		public static event Action<SplineContainer, int> SplineRemoved;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000159 RID: 345 RVA: 0x000068D0 File Offset: 0x00004AD0
		// (remove) Token: 0x0600015A RID: 346 RVA: 0x00006904 File Offset: 0x00004B04
		public static event Action<SplineContainer, int, int> SplineReordered;

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00006938 File Offset: 0x00004B38
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00006964 File Offset: 0x00004B64
		public IReadOnlyList<Spline> Splines
		{
			get
			{
				ReadOnlyCollection<Spline> readOnlyCollection;
				if ((readOnlyCollection = this.m_ReadOnlySplines) == null)
				{
					readOnlyCollection = (this.m_ReadOnlySplines = new ReadOnlyCollection<Spline>(this.m_Splines));
				}
				return readOnlyCollection;
			}
			set
			{
				if (value == null)
				{
					this.m_Splines = Array.Empty<Spline>();
					return;
				}
				this.ClearCaches();
				this.DisposeNativeSplinesCache();
				for (int k = 0; k < this.m_Splines.Length; k++)
				{
					int num = SplineContainer.IndexOf(value, this.m_Splines[k]);
					if (num == -1)
					{
						this.m_RemovedSplinesIndices.Add(k);
					}
					else if (num != k)
					{
						this.m_ReorderedSplinesIndices.Add(new ValueTuple<int, int>(k, num));
					}
				}
				int i;
				Predicate<Spline> <>9__0;
				int i2;
				for (i = 0; i < value.Count; i = i2 + 1)
				{
					Spline[] splines = this.m_Splines;
					Predicate<Spline> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = (Spline spline) => spline == value[i]);
					}
					if (Array.FindIndex<Spline>(splines, predicate) == -1)
					{
						this.m_AddedSplinesIndices.Add(i);
					}
					i2 = i;
				}
				this.m_Splines = new Spline[value.Count];
				for (int j = 0; j < this.m_Splines.Length; j++)
				{
					this.m_Splines[j] = value[j];
					if (this.IsNonUniformlyScaled)
					{
						this.GetOrBakeNativeSpline<Spline>(this.m_Splines[j]);
					}
				}
				this.m_ReadOnlySplines = new ReadOnlyCollection<Spline>(this.m_Splines);
				foreach (int num2 in this.m_RemovedSplinesIndices)
				{
					Action<SplineContainer, int> splineRemoved = SplineContainer.SplineRemoved;
					if (splineRemoved != null)
					{
						splineRemoved(this, num2);
					}
				}
				foreach (int num3 in this.m_AddedSplinesIndices)
				{
					Action<SplineContainer, int> splineAdded = SplineContainer.SplineAdded;
					if (splineAdded != null)
					{
						splineAdded(this, num3);
					}
				}
				foreach (ValueTuple<int, int> valueTuple in this.m_ReorderedSplinesIndices)
				{
					Action<SplineContainer, int, int> splineReordered = SplineContainer.SplineReordered;
					if (splineReordered != null)
					{
						splineReordered(this, valueTuple.Item1, valueTuple.Item2);
					}
				}
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00006BC8 File Offset: 0x00004DC8
		private static int IndexOf(IReadOnlyList<Spline> self, Spline elementToFind)
		{
			for (int i = 0; i < self.Count; i++)
			{
				if (self[i] == elementToFind)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00006BF3 File Offset: 0x00004DF3
		public KnotLinkCollection KnotLinkCollection
		{
			get
			{
				return this.m_Knots;
			}
		}

		// Token: 0x17000048 RID: 72
		public Spline this[int index]
		{
			get
			{
				return this.m_Splines[index];
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006C08 File Offset: 0x00004E08
		~SplineContainer()
		{
			this.DisposeNativeSplinesCache();
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00006C34 File Offset: 0x00004E34
		private void OnEnable()
		{
			Spline.Changed += this.OnSplineChanged;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006C47 File Offset: 0x00004E47
		private void OnDisable()
		{
			Spline.Changed -= this.OnSplineChanged;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00006C5A File Offset: 0x00004E5A
		private void OnDestroy()
		{
			this.DisposeNativeSplinesCache();
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00006C64 File Offset: 0x00004E64
		public void Warmup()
		{
			for (int i = 0; i < this.Splines.Count; i++)
			{
				Spline spline = this.Splines[i];
				spline.Warmup();
				this.GetOrBakeNativeSpline<Spline>(spline);
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00006CA2 File Offset: 0x00004EA2
		internal void ClearCaches()
		{
			this.m_ReorderedSplinesIndices.Clear();
			this.m_RemovedSplinesIndices.Clear();
			this.m_AddedSplinesIndices.Clear();
			this.m_ReadOnlySplines = null;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00006CCC File Offset: 0x00004ECC
		private void DisposeNativeSplinesCache()
		{
			foreach (KeyValuePair<ISpline, NativeSpline> keyValuePair in this.m_NativeSplinesCache)
			{
				keyValuePair.Value.Dispose();
			}
			this.m_NativeSplinesCache.Clear();
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00006D34 File Offset: 0x00004F34
		private void OnSplineChanged(Spline spline, int index, SplineModification modificationType)
		{
			int num = Array.IndexOf<Spline>(this.m_Splines, spline);
			if (num < 0)
			{
				return;
			}
			switch (modificationType)
			{
			case SplineModification.KnotModified:
				this.SetLinkedKnotPosition(new SplineKnotIndex(num, index));
				break;
			case SplineModification.KnotInserted:
			case SplineModification.KnotReordered:
				this.m_Knots.KnotInserted(num, index);
				break;
			case SplineModification.KnotRemoved:
				this.m_Knots.KnotRemoved(num, index);
				break;
			}
			NativeSpline nativeSpline;
			if (this.m_NativeSplinesCache.TryGetValue(spline, out nativeSpline))
			{
				nativeSpline.Dispose();
			}
			this.m_NativeSplinesCache.Remove(spline);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00006DBC File Offset: 0x00004FBC
		private void OnKnotModified(Spline spline, int index)
		{
			int num = Array.IndexOf<Spline>(this.m_Splines, spline);
			if (num >= 0)
			{
				this.SetLinkedKnotPosition(new SplineKnotIndex(num, index));
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00006DE7 File Offset: 0x00004FE7
		private bool IsNonUniformlyScaled
		{
			get
			{
				float3 @float = base.transform.lossyScale;
				return !math.all(@float == @float.x);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00006E0C File Offset: 0x0000500C
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00006E21 File Offset: 0x00005021
		public Spline Spline
		{
			get
			{
				if (this.m_Splines.Length == 0)
				{
					return null;
				}
				return this.m_Splines[0];
			}
			set
			{
				if (this.m_Splines.Length != 0)
				{
					this.m_Splines[0] = value;
				}
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00006E35 File Offset: 0x00005035
		public bool Evaluate(float t, out float3 position, out float3 tangent, out float3 upVector)
		{
			return this.Evaluate(0, t, out position, out tangent, out upVector);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00006E43 File Offset: 0x00005043
		public bool Evaluate(int splineIndex, float t, out float3 position, out float3 tangent, out float3 upVector)
		{
			return this.Evaluate<Spline>(this.m_Splines[splineIndex], t, out position, out tangent, out upVector);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00006E5C File Offset: 0x0000505C
		public bool Evaluate<T>(T spline, float t, out float3 position, out float3 tangent, out float3 upVector) where T : ISpline
		{
			if (spline == null)
			{
				position = float3.zero;
				tangent = new float3(0f, 0f, 1f);
				upVector = new float3(0f, 1f, 0f);
				return false;
			}
			if (this.IsNonUniformlyScaled)
			{
				return this.GetOrBakeNativeSpline<T>(spline).Evaluate(t, out position, out tangent, out upVector);
			}
			bool flag = spline.Evaluate(t, out position, out tangent, out upVector);
			if (flag)
			{
				position = base.transform.TransformPoint(position);
				tangent = base.transform.TransformVector(tangent);
				upVector = base.transform.TransformDirection(upVector);
			}
			return flag;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00006F45 File Offset: 0x00005145
		public float3 EvaluatePosition(float t)
		{
			return this.EvaluatePosition(0, t);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006F4F File Offset: 0x0000514F
		public float3 EvaluatePosition(int splineIndex, float t)
		{
			return this.EvaluatePosition<Spline>(this.m_Splines[splineIndex], t);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006F60 File Offset: 0x00005160
		public float3 EvaluatePosition<T>(T spline, float t) where T : ISpline
		{
			if (spline == null)
			{
				return float.PositiveInfinity;
			}
			if (this.IsNonUniformlyScaled)
			{
				return this.GetOrBakeNativeSpline<T>(spline).EvaluatePosition(t);
			}
			return base.transform.TransformPoint(spline.EvaluatePosition(t));
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00006FB2 File Offset: 0x000051B2
		public float3 EvaluateTangent(float t)
		{
			return this.EvaluateTangent(0, t);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00006FBC File Offset: 0x000051BC
		public float3 EvaluateTangent(int splineIndex, float t)
		{
			return this.EvaluateTangent<Spline>(this.m_Splines[splineIndex], t);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00006FD0 File Offset: 0x000051D0
		public float3 EvaluateTangent<T>(T spline, float t) where T : ISpline
		{
			if (spline == null)
			{
				return float.PositiveInfinity;
			}
			if (this.IsNonUniformlyScaled)
			{
				return this.GetOrBakeNativeSpline<T>(spline).EvaluateTangent(t);
			}
			return base.transform.TransformVector(spline.EvaluateTangent(t));
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007022 File Offset: 0x00005222
		public float3 EvaluateUpVector(float t)
		{
			return this.EvaluateUpVector(0, t);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000702C File Offset: 0x0000522C
		public float3 EvaluateUpVector(int splineIndex, float t)
		{
			return this.EvaluateUpVector<Spline>(this.m_Splines[splineIndex], t);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007040 File Offset: 0x00005240
		public float3 EvaluateUpVector<T>(T spline, float t) where T : ISpline
		{
			if (spline == null)
			{
				return float3.zero;
			}
			if (this.IsNonUniformlyScaled)
			{
				return this.GetOrBakeNativeSpline<T>(spline).EvaluateUpVector(t);
			}
			return base.transform.TransformDirection(spline.EvaluateUpVector(t));
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000708D File Offset: 0x0000528D
		public float3 EvaluateAcceleration(float t)
		{
			return this.EvaluateAcceleration(0, t);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007097 File Offset: 0x00005297
		public float3 EvaluateAcceleration(int splineIndex, float t)
		{
			return this.EvaluateAcceleration<Spline>(this.m_Splines[splineIndex], t);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000070A8 File Offset: 0x000052A8
		public float3 EvaluateAcceleration<T>(T spline, float t) where T : ISpline
		{
			if (spline == null)
			{
				return float3.zero;
			}
			if (this.IsNonUniformlyScaled)
			{
				return this.GetOrBakeNativeSpline<T>(spline).EvaluateAcceleration(t);
			}
			return base.transform.TransformVector(spline.EvaluateAcceleration(t));
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000070F5 File Offset: 0x000052F5
		public float CalculateLength()
		{
			return this.CalculateLength(0);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000070FE File Offset: 0x000052FE
		public float CalculateLength(int splineIndex)
		{
			return this.m_Splines[splineIndex].CalculateLength(base.transform.localToWorldMatrix);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000711D File Offset: 0x0000531D
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007120 File Offset: 0x00005320
		public void OnAfterDeserialize()
		{
			if (this.m_Spline != null && this.m_Spline.Count > 0)
			{
				if (this.m_Splines == null || this.m_Splines.Length == 0 || (this.m_Splines.Length == 1 && this.m_Splines[0].Count == 0))
				{
					this.m_Splines = new Spline[] { this.m_Spline };
				}
				this.m_Spline = new Spline();
			}
			bool flag = this.m_ReadOnlySplines == null || this.m_ReadOnlySplines.Count != this.m_Splines.Length;
			if (!flag)
			{
				for (int i = 0; i < this.m_Splines.Length; i++)
				{
					if (this.m_ReadOnlySplines[i] != this.m_Splines[i])
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				this.m_ReadOnlySplines = new ReadOnlyCollection<Spline>(this.m_Splines);
			}
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000071F8 File Offset: 0x000053F8
		private NativeSpline GetOrBakeNativeSpline<T>(T spline) where T : ISpline
		{
			NativeSpline nativeSpline;
			if (!this.m_NativeSplinesCache.TryGetValue(spline, out nativeSpline))
			{
				this.m_NativeSplinesCacheTransform = base.transform.localToWorldMatrix;
				nativeSpline = new NativeSpline(spline, this.m_NativeSplinesCacheTransform, true, Allocator.Persistent);
				this.m_NativeSplinesCache.Add(spline, nativeSpline);
			}
			else if (!MathUtility.All(this.m_NativeSplinesCacheTransform, base.transform.localToWorldMatrix))
			{
				this.m_NativeSplinesCacheTransform = base.transform.localToWorldMatrix;
				SplineContainer.s_AllocPreventionHelperBuffer.Clear();
				foreach (ISpline spline2 in this.m_NativeSplinesCache.Keys)
				{
					NativeSpline nativeSpline2 = this.m_NativeSplinesCache[spline2];
					NativeSpline nativeSpline3 = new NativeSpline(spline, this.m_NativeSplinesCacheTransform, true, Allocator.Persistent);
					if (spline2 == spline)
					{
						nativeSpline = nativeSpline3;
					}
					nativeSpline2.Dispose();
					SplineContainer.s_AllocPreventionHelperBuffer.Add(new SplineContainer.SplineToNative
					{
						spline = spline2,
						nativeSpline = nativeSpline3
					});
				}
				for (int i = 0; i < SplineContainer.s_AllocPreventionHelperBuffer.Count; i++)
				{
					SplineContainer.SplineToNative splineToNative = SplineContainer.s_AllocPreventionHelperBuffer[i];
					this.m_NativeSplinesCache[splineToNative.spline] = splineToNative.nativeSpline;
				}
			}
			return nativeSpline;
		}

		// Token: 0x0400006B RID: 107
		private const string k_IconPath = "Packages/com.unity.splines/Editor/Editor Resources/Icons/SplineComponent.png";

		// Token: 0x0400006C RID: 108
		[SerializeField]
		[Obsolete]
		[HideInInspector]
		private Spline m_Spline;

		// Token: 0x0400006D RID: 109
		[SerializeField]
		private Spline[] m_Splines = new Spline[]
		{
			new Spline()
		};

		// Token: 0x0400006E RID: 110
		[SerializeField]
		private KnotLinkCollection m_Knots = new KnotLinkCollection();

		// Token: 0x0400006F RID: 111
		[TupleElementNames(new string[] { "previousIndex", "newIndex" })]
		private List<ValueTuple<int, int>> m_ReorderedSplinesIndices = new List<ValueTuple<int, int>>();

		// Token: 0x04000070 RID: 112
		private List<int> m_RemovedSplinesIndices = new List<int>();

		// Token: 0x04000071 RID: 113
		private List<int> m_AddedSplinesIndices = new List<int>();

		// Token: 0x04000075 RID: 117
		private ReadOnlyCollection<Spline> m_ReadOnlySplines;

		// Token: 0x04000076 RID: 118
		private Dictionary<ISpline, NativeSpline> m_NativeSplinesCache = new Dictionary<ISpline, NativeSpline>();

		// Token: 0x04000077 RID: 119
		private float4x4 m_NativeSplinesCacheTransform = float4x4.identity;

		// Token: 0x04000078 RID: 120
		private static List<SplineContainer.SplineToNative> s_AllocPreventionHelperBuffer = new List<SplineContainer.SplineToNative>(32);

		// Token: 0x0200005E RID: 94
		private struct SplineToNative
		{
			// Token: 0x04000139 RID: 313
			public ISpline spline;

			// Token: 0x0400013A RID: 314
			public NativeSpline nativeSpline;
		}
	}
}
