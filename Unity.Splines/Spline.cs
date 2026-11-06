using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200001C RID: 28
	[Serializable]
	public class Spline : ISpline, IReadOnlyList<BezierKnot>, IEnumerable<BezierKnot>, IEnumerable, IReadOnlyCollection<BezierKnot>, IList<BezierKnot>, ICollection<BezierKnot>
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00004660 File Offset: 0x00002860
		private IEnumerable<ISplineModificationHandler> embeddedSplineData
		{
			get
			{
				foreach (SplineDataKeyValuePair<int> splineDataKeyValuePair in this.m_IntData)
				{
					yield return splineDataKeyValuePair.Value;
				}
				IEnumerator<SplineDataKeyValuePair<int>> enumerator = null;
				foreach (SplineDataKeyValuePair<float> splineDataKeyValuePair2 in this.m_FloatData)
				{
					yield return splineDataKeyValuePair2.Value;
				}
				IEnumerator<SplineDataKeyValuePair<float>> enumerator2 = null;
				foreach (SplineDataKeyValuePair<float4> splineDataKeyValuePair3 in this.m_Float4Data)
				{
					yield return splineDataKeyValuePair3.Value;
				}
				IEnumerator<SplineDataKeyValuePair<float4>> enumerator3 = null;
				foreach (SplineDataKeyValuePair<Object> splineDataKeyValuePair4 in this.m_ObjectData)
				{
					yield return splineDataKeyValuePair4.Value;
				}
				IEnumerator<SplineDataKeyValuePair<Object>> enumerator4 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004670 File Offset: 0x00002870
		public bool TryGetFloatData(string key, out SplineData<float> data)
		{
			return this.m_FloatData.TryGetValue(key, out data);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000467F File Offset: 0x0000287F
		public bool TryGetFloat4Data(string key, out SplineData<float4> data)
		{
			return this.m_Float4Data.TryGetValue(key, out data);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000468E File Offset: 0x0000288E
		public bool TryGetIntData(string key, out SplineData<int> data)
		{
			return this.m_IntData.TryGetValue(key, out data);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000469D File Offset: 0x0000289D
		public bool TryGetObjectData(string key, out SplineData<Object> data)
		{
			return this.m_ObjectData.TryGetValue(key, out data);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000046AC File Offset: 0x000028AC
		public SplineData<float> GetOrCreateFloatData(string key)
		{
			return this.m_FloatData.GetOrCreate(key);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000046BA File Offset: 0x000028BA
		public SplineData<float4> GetOrCreateFloat4Data(string key)
		{
			return this.m_Float4Data.GetOrCreate(key);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000046C8 File Offset: 0x000028C8
		public SplineData<int> GetOrCreateIntData(string key)
		{
			return this.m_IntData.GetOrCreate(key);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000046D6 File Offset: 0x000028D6
		public SplineData<Object> GetOrCreateObjectData(string key)
		{
			return this.m_ObjectData.GetOrCreate(key);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000046E4 File Offset: 0x000028E4
		public bool RemoveFloatData(string key)
		{
			return this.m_FloatData.Remove(key);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000046F2 File Offset: 0x000028F2
		public bool RemoveFloat4Data(string key)
		{
			return this.m_Float4Data.Remove(key);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004700 File Offset: 0x00002900
		public bool RemoveIntData(string key)
		{
			return this.m_IntData.Remove(key);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000470E File Offset: 0x0000290E
		public bool RemoveObjectData(string key)
		{
			return this.m_ObjectData.Remove(key);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000471C File Offset: 0x0000291C
		public IEnumerable<string> GetFloatDataKeys()
		{
			return this.m_FloatData.Keys;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004729 File Offset: 0x00002929
		public IEnumerable<string> GetFloat4DataKeys()
		{
			return this.m_Float4Data.Keys;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004736 File Offset: 0x00002936
		public IEnumerable<string> GetIntDataKeys()
		{
			return this.m_IntData.Keys;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004743 File Offset: 0x00002943
		public IEnumerable<string> GetObjectDataKeys()
		{
			return this.m_ObjectData.Keys;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004750 File Offset: 0x00002950
		public IEnumerable<string> GetSplineDataKeys(EmbeddedSplineDataType type)
		{
			switch (type)
			{
			case EmbeddedSplineDataType.Int:
				return this.m_IntData.Keys;
			case EmbeddedSplineDataType.Float:
				return this.m_FloatData.Keys;
			case EmbeddedSplineDataType.Float4:
				return this.m_Float4Data.Keys;
			case EmbeddedSplineDataType.Object:
				return this.m_ObjectData.Keys;
			default:
				throw new InvalidEnumArgumentException();
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000047AA File Offset: 0x000029AA
		public IEnumerable<SplineData<float>> GetFloatDataValues()
		{
			return this.m_FloatData.Values;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000047B7 File Offset: 0x000029B7
		public IEnumerable<SplineData<float4>> GetFloat4DataValues()
		{
			return this.m_Float4Data.Values;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000047C4 File Offset: 0x000029C4
		public IEnumerable<SplineData<int>> GetIntDataValues()
		{
			return this.m_IntData.Values;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000047D1 File Offset: 0x000029D1
		public IEnumerable<SplineData<Object>> GetObjectDataValues()
		{
			return this.m_ObjectData.Values;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000047DE File Offset: 0x000029DE
		public void SetFloatData(string key, SplineData<float> value)
		{
			this.m_FloatData[key] = value;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000047ED File Offset: 0x000029ED
		public void SetFloat4Data(string key, SplineData<float4> value)
		{
			this.m_Float4Data[key] = value;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000047FC File Offset: 0x000029FC
		public void SetIntData(string key, SplineData<int> value)
		{
			this.m_IntData[key] = value;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000480B File Offset: 0x00002A0B
		public void SetObjectData(string key, SplineData<Object> value)
		{
			this.m_ObjectData[key] = value;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000BF RID: 191 RVA: 0x0000481A File Offset: 0x00002A1A
		public int Count
		{
			get
			{
				return this.m_Knots.Count;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00004827 File Offset: 0x00002A27
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000C1 RID: 193 RVA: 0x0000482C File Offset: 0x00002A2C
		// (remove) Token: 0x060000C2 RID: 194 RVA: 0x00004864 File Offset: 0x00002A64
		[Obsolete("Deprecated, use Changed instead.")]
		public event Action changed;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000C3 RID: 195 RVA: 0x0000489C File Offset: 0x00002A9C
		// (remove) Token: 0x060000C4 RID: 196 RVA: 0x000048D0 File Offset: 0x00002AD0
		public static event Action<Spline, int, SplineModification> Changed;

		// Token: 0x060000C5 RID: 197 RVA: 0x00004904 File Offset: 0x00002B04
		internal void SetDirtyNoNotify()
		{
			this.EnsureMetaDataValid();
			this.m_Length = -1f;
			int i = 0;
			int count = this.m_MetaData.Count;
			while (i < count)
			{
				this.m_MetaData[i].InvalidateCache();
				i++;
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000494C File Offset: 0x00002B4C
		internal void SetDirty(SplineModification modificationEvent, int knotIndex = -1)
		{
			this.SetDirtyNoNotify();
			Action action = this.changed;
			if (action != null)
			{
				action();
			}
			this.OnSplineChanged();
			foreach (ISplineModificationHandler splineModificationHandler in this.embeddedSplineData)
			{
				splineModificationHandler.OnSplineModified(new SplineModificationData(this, modificationEvent, knotIndex, this.m_LastKnotChangeCurveLengths.Item1, this.m_LastKnotChangeCurveLengths.Item2));
			}
			Action<Spline, int, SplineModification> action2 = Spline.Changed;
			if (action2 == null)
			{
				return;
			}
			action2(this, knotIndex, modificationEvent);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000049E4 File Offset: 0x00002BE4
		protected virtual void OnSplineChanged()
		{
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000049E6 File Offset: 0x00002BE6
		private void EnsureMetaDataValid()
		{
			while (this.m_MetaData.Count < this.m_Knots.Count)
			{
				this.m_MetaData.Add(new Spline.MetaData());
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004A12 File Offset: 0x00002C12
		public void EnforceTangentModeNoNotify(int index)
		{
			this.EnforceTangentModeNoNotify(new SplineRange(index, 1));
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004A24 File Offset: 0x00002C24
		public void EnforceTangentModeNoNotify(SplineRange range)
		{
			for (int i = range.Start; i <= range.End; i++)
			{
				this.ApplyTangentModeNoNotify(i, BezierTangent.Out);
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004A51 File Offset: 0x00002C51
		public TangentMode GetTangentMode(int index)
		{
			this.EnsureMetaDataValid();
			if (this.m_MetaData.Count <= 0)
			{
				return TangentMode.Broken;
			}
			return this.m_MetaData[index].Mode;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004A7A File Offset: 0x00002C7A
		public void SetTangentMode(TangentMode mode)
		{
			this.SetTangentMode(new SplineRange(0, this.Count), mode, BezierTangent.Out);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004A90 File Offset: 0x00002C90
		public void SetTangentMode(int index, TangentMode mode, BezierTangent main = BezierTangent.Out)
		{
			if (this.GetTangentMode(index) == mode)
			{
				return;
			}
			if (index == this.Count - 1 && !this.Closed)
			{
				main = BezierTangent.In;
			}
			this.SetTangentMode(new SplineRange(index, 1), mode, main);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004AC4 File Offset: 0x00002CC4
		public void SetTangentMode(SplineRange range, TangentMode mode, BezierTangent main = BezierTangent.Out)
		{
			foreach (int num in range)
			{
				this.CacheKnotOperationCurves(num);
				this.SetTangentModeNoNotify(num, mode, main);
				this.SetDirty(SplineModification.KnotModified, num);
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004B20 File Offset: 0x00002D20
		public void SetTangentModeNoNotify(int index, TangentMode mode, BezierTangent main = BezierTangent.Out)
		{
			this.EnsureMetaDataValid();
			BezierKnot bezierKnot = this.m_Knots[index];
			if (this.m_MetaData[index].Mode == TangentMode.Linear && mode >= TangentMode.Mirrored)
			{
				bezierKnot.TangentIn = SplineUtility.GetExplicitLinearTangent(bezierKnot, this.Previous(index));
				bezierKnot.TangentOut = SplineUtility.GetExplicitLinearTangent(bezierKnot, this.Next(index));
			}
			this.m_MetaData[index].Mode = mode;
			this.m_Knots[index] = bezierKnot;
			this.ApplyTangentModeNoNotify(index, main);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004BA8 File Offset: 0x00002DA8
		private void ApplyTangentModeNoNotify(int index, BezierTangent main = BezierTangent.Out)
		{
			BezierKnot bezierKnot = this.m_Knots[index];
			switch (this.GetTangentMode(index))
			{
			case TangentMode.AutoSmooth:
				bezierKnot = SplineUtility.GetAutoSmoothKnot(bezierKnot.Position, this.Previous(index).Position, this.Next(index).Position, math.mul(bezierKnot.Rotation, math.up()), this.m_MetaData[index].Tension);
				break;
			case TangentMode.Linear:
				bezierKnot.TangentIn = float3.zero;
				bezierKnot.TangentOut = float3.zero;
				break;
			case TangentMode.Mirrored:
				bezierKnot = bezierKnot.BakeTangentDirectionToRotation(true, main);
				break;
			case TangentMode.Continuous:
				bezierKnot = bezierKnot.BakeTangentDirectionToRotation(false, main);
				break;
			}
			this.m_Knots[index] = bezierKnot;
			this.SetDirtyNoNotify();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004C6C File Offset: 0x00002E6C
		public float GetAutoSmoothTension(int index)
		{
			return this.m_MetaData[index].Tension;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004C7F File Offset: 0x00002E7F
		public void SetAutoSmoothTension(int index, float tension)
		{
			this.SetAutoSmoothTension(new SplineRange(index, 1), tension);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004C8F File Offset: 0x00002E8F
		public void SetAutoSmoothTension(SplineRange range, float tension)
		{
			this.SetAutoSmoothTensionInternal(range, tension, true);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004C9A File Offset: 0x00002E9A
		public void SetAutoSmoothTensionNoNotify(int index, float tension)
		{
			this.SetAutoSmoothTensionInternal(new SplineRange(index, 1), tension, false);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00004CAB File Offset: 0x00002EAB
		public void SetAutoSmoothTensionNoNotify(SplineRange range, float tension)
		{
			this.SetAutoSmoothTensionInternal(range, tension, false);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004CB8 File Offset: 0x00002EB8
		private void SetAutoSmoothTensionInternal(SplineRange range, float tension, bool setDirty)
		{
			int i = 0;
			int count = range.Count;
			while (i < count)
			{
				int num = range[i];
				this.CacheKnotOperationCurves(num);
				this.m_MetaData[num].Tension = tension;
				if (this.m_MetaData[num].Mode == TangentMode.AutoSmooth)
				{
					this.ApplyTangentModeNoNotify(num, BezierTangent.Out);
				}
				if (setDirty)
				{
					this.SetDirty(SplineModification.KnotModified, num);
				}
				i++;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00004D21 File Offset: 0x00002F21
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x00004D2C File Offset: 0x00002F2C
		[Obsolete("Use GetTangentMode and SetTangentMode.")]
		public SplineType EditType
		{
			get
			{
				return this.m_EditModeType;
			}
			set
			{
				if (this.m_EditModeType == value)
				{
					return;
				}
				this.m_EditModeType = value;
				TangentMode tangentMode = value.GetTangentMode();
				for (int i = 0; i < this.Count; i++)
				{
					this.SetTangentModeNoNotify(i, tangentMode, BezierTangent.Out);
				}
				this.SetDirty(SplineModification.Default, -1);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00004D73 File Offset: 0x00002F73
		// (set) Token: 0x060000DA RID: 218 RVA: 0x00004D7B File Offset: 0x00002F7B
		public IEnumerable<BezierKnot> Knots
		{
			get
			{
				return this.m_Knots;
			}
			set
			{
				this.m_Knots = new List<BezierKnot>(value);
				this.m_MetaData = new List<Spline.MetaData>(this.m_Knots.Count);
				this.SetDirty(SplineModification.Default, -1);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00004DA7 File Offset: 0x00002FA7
		// (set) Token: 0x060000DC RID: 220 RVA: 0x00004DAF File Offset: 0x00002FAF
		public bool Closed
		{
			get
			{
				return this.m_Closed;
			}
			set
			{
				if (this.m_Closed == value)
				{
					return;
				}
				this.m_Closed = value;
				this.CheckAutoSmoothExtremityKnots();
				this.SetDirty(SplineModification.ClosedModified, -1);
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004DD0 File Offset: 0x00002FD0
		internal void CheckAutoSmoothExtremityKnots()
		{
			if (this.GetTangentMode(0) == TangentMode.AutoSmooth)
			{
				this.ApplyTangentModeNoNotify(0, BezierTangent.Out);
			}
			if (this.Count > 2 && this.GetTangentMode(this.Count - 1) == TangentMode.AutoSmooth)
			{
				this.ApplyTangentModeNoNotify(this.Count - 1, BezierTangent.Out);
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004E0B File Offset: 0x0000300B
		public int IndexOf(BezierKnot item)
		{
			return this.m_Knots.IndexOf(item);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004E19 File Offset: 0x00003019
		public void Insert(int index, BezierKnot knot)
		{
			this.Insert(index, knot, TangentMode.Broken, 0.5f);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004E29 File Offset: 0x00003029
		public void Insert(int index, BezierKnot knot, TangentMode mode)
		{
			this.Insert(index, knot, mode, 0.5f);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004E39 File Offset: 0x00003039
		public void Insert(int index, BezierKnot knot, TangentMode mode, float tension)
		{
			this.CacheKnotOperationCurves(index);
			this.InsertNoNotify(index, knot, mode, tension);
			this.SetDirty(SplineModification.KnotInserted, index);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004E58 File Offset: 0x00003058
		private void InsertNoNotify(int index, BezierKnot knot, TangentMode mode, float tension)
		{
			this.EnsureMetaDataValid();
			this.m_Knots.Insert(index, knot);
			this.m_MetaData.Insert(index, new Spline.MetaData
			{
				Mode = mode,
				Tension = tension
			});
			int num = this.PreviousIndex(index);
			if (num != index)
			{
				this.ApplyTangentModeNoNotify(num, BezierTangent.Out);
			}
			this.ApplyTangentModeNoNotify(index, BezierTangent.Out);
			int num2 = this.NextIndex(index);
			if (num2 != index)
			{
				this.ApplyTangentModeNoNotify(num2, BezierTangent.Out);
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004EC8 File Offset: 0x000030C8
		internal void InsertOnCurve(int index, float curveT)
		{
			int num = SplineUtility.PreviousIndex(index, this.Count, this.Closed);
			BezierKnot bezierKnot = this.m_Knots[num];
			BezierKnot bezierKnot2 = this.m_Knots[index];
			BezierCurve bezierCurve = new BezierCurve(bezierKnot, this.m_Knots[index]);
			BezierCurve bezierCurve2;
			BezierCurve bezierCurve3;
			CurveUtility.Split(bezierCurve, curveT, out bezierCurve2, out bezierCurve3);
			if (this.GetTangentMode(num) == TangentMode.Mirrored)
			{
				this.SetTangentMode(num, TangentMode.Continuous, BezierTangent.Out);
			}
			if (this.GetTangentMode(index) == TangentMode.Mirrored)
			{
				this.SetTangentMode(index, TangentMode.Continuous, BezierTangent.Out);
			}
			if (SplineUtility.AreTangentsModifiable(this.GetTangentMode(num)))
			{
				bezierKnot.TangentOut = math.mul(math.inverse(bezierKnot.Rotation), bezierCurve2.Tangent0);
			}
			if (SplineUtility.AreTangentsModifiable(this.GetTangentMode(index)))
			{
				bezierKnot2.TangentIn = math.mul(math.inverse(bezierKnot2.Rotation), bezierCurve3.Tangent1);
			}
			float3 @float = CurveUtility.EvaluateUpVector(bezierCurve, curveT, math.rotate(bezierKnot.Rotation, math.up()), math.rotate(bezierKnot2.Rotation, math.up()), true);
			quaternion quaternion = quaternion.LookRotationSafe(math.normalizesafe(bezierCurve3.Tangent0, default(float3)), @float);
			quaternion quaternion2 = math.inverse(quaternion);
			this.SetKnotNoNotify(num, bezierKnot, BezierTangent.Out);
			this.SetKnotNoNotify(index, bezierKnot2, BezierTangent.Out);
			BezierKnot bezierKnot3 = new BezierKnot(bezierCurve2.P3, math.mul(quaternion2, bezierCurve2.Tangent1), math.mul(quaternion2, bezierCurve3.Tangent0), quaternion);
			this.Insert(index, bezierKnot3);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005034 File Offset: 0x00003234
		public void RemoveAt(int index)
		{
			this.EnsureMetaDataValid();
			this.CacheKnotOperationCurves(index);
			this.m_Knots.RemoveAt(index);
			this.m_MetaData.RemoveAt(index);
			int num = Mathf.Clamp(index, 0, this.Count - 1);
			if (this.Count > 0)
			{
				this.ApplyTangentModeNoNotify(this.PreviousIndex(num), BezierTangent.Out);
				this.ApplyTangentModeNoNotify(num, BezierTangent.Out);
			}
			this.SetDirty(SplineModification.KnotRemoved, index);
		}

		// Token: 0x17000029 RID: 41
		public BezierKnot this[int index]
		{
			get
			{
				return this.m_Knots[index];
			}
			set
			{
				this.SetKnot(index, value, BezierTangent.Out);
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000050B6 File Offset: 0x000032B6
		public void SetKnot(int index, BezierKnot value, BezierTangent main = BezierTangent.Out)
		{
			this.CacheKnotOperationCurves(index);
			this.SetKnotNoNotify(index, value, main);
			this.SetDirty(SplineModification.KnotModified, index);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000050D0 File Offset: 0x000032D0
		public void SetKnotNoNotify(int index, BezierKnot value, BezierTangent main = BezierTangent.Out)
		{
			this.m_Knots[index] = value;
			this.ApplyTangentModeNoNotify(index, main);
			int num = this.PreviousIndex(index);
			int num2 = this.NextIndex(index);
			if (this.m_MetaData[num].Mode == TangentMode.AutoSmooth)
			{
				this.ApplyTangentModeNoNotify(num, main);
			}
			if (this.m_MetaData[num2].Mode == TangentMode.AutoSmooth)
			{
				this.ApplyTangentModeNoNotify(num2, main);
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005138 File Offset: 0x00003338
		public Spline()
		{
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000051A0 File Offset: 0x000033A0
		public Spline(int knotCapacity, bool closed = false)
		{
			this.m_Knots = new List<BezierKnot>(knotCapacity);
			this.m_Closed = closed;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000521C File Offset: 0x0000341C
		public Spline(IEnumerable<BezierKnot> knots, bool closed = false)
		{
			this.m_Knots = knots.ToList<BezierKnot>();
			this.m_Closed = closed;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005298 File Offset: 0x00003498
		public Spline(IEnumerable<float3> knotPositions, TangentMode tangentMode = TangentMode.AutoSmooth, bool closed = false)
		{
			this.InsertRangeNoNotify(this.Count, knotPositions, tangentMode, false);
			this.m_Closed = closed;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005318 File Offset: 0x00003518
		public Spline(Spline spline)
		{
			this.m_Knots = spline.Knots.ToList<BezierKnot>();
			this.m_Closed = spline.Closed;
			foreach (SplineDataKeyValuePair<int> splineDataKeyValuePair in spline.m_IntData)
			{
				this.m_IntData[splineDataKeyValuePair.Key] = splineDataKeyValuePair.Value;
			}
			foreach (SplineDataKeyValuePair<float> splineDataKeyValuePair2 in spline.m_FloatData)
			{
				this.m_FloatData[splineDataKeyValuePair2.Key] = splineDataKeyValuePair2.Value;
			}
			foreach (SplineDataKeyValuePair<float4> splineDataKeyValuePair3 in spline.m_Float4Data)
			{
				this.m_Float4Data[splineDataKeyValuePair3.Key] = splineDataKeyValuePair3.Value;
			}
			foreach (SplineDataKeyValuePair<Object> splineDataKeyValuePair4 in spline.m_ObjectData)
			{
				this.m_ObjectData[splineDataKeyValuePair4.Key] = splineDataKeyValuePair4.Value;
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000054E0 File Offset: 0x000036E0
		public BezierCurve GetCurve(int index)
		{
			int num = (this.m_Closed ? ((index + 1) % this.m_Knots.Count) : math.min(index + 1, this.m_Knots.Count - 1));
			return new BezierCurve(this.m_Knots[index], this.m_Knots[num]);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000553C File Offset: 0x0000373C
		public float GetCurveLength(int index)
		{
			this.EnsureMetaDataValid();
			DistanceToInterpolation[] distanceToInterpolation = this.m_MetaData[index].DistanceToInterpolation;
			if (distanceToInterpolation[0].Distance < 0f)
			{
				CurveUtility.CalculateCurveLengths(this.GetCurve(index), distanceToInterpolation);
			}
			if (distanceToInterpolation.Length == 0)
			{
				return 0f;
			}
			return distanceToInterpolation[distanceToInterpolation.Length - 1].Distance;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000559C File Offset: 0x0000379C
		public float GetLength()
		{
			if (this.m_Length < 0f)
			{
				this.m_Length = 0f;
				int i = 0;
				int num = (this.Closed ? this.Count : (this.Count - 1));
				while (i < num)
				{
					this.m_Length += this.GetCurveLength(i);
					i++;
				}
			}
			return this.m_Length;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005600 File Offset: 0x00003800
		private DistanceToInterpolation[] GetCurveDistanceLut(int index)
		{
			if (this.m_MetaData[index].DistanceToInterpolation[0].Distance < 0f)
			{
				CurveUtility.CalculateCurveLengths(this.GetCurve(index), this.m_MetaData[index].DistanceToInterpolation);
			}
			return this.m_MetaData[index].DistanceToInterpolation;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000565E File Offset: 0x0000385E
		public float GetCurveInterpolation(int curveIndex, float curveDistance)
		{
			return CurveUtility.GetDistanceToInterpolation<DistanceToInterpolation[]>(this.GetCurveDistanceLut(curveIndex), curveDistance);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005670 File Offset: 0x00003870
		private void WarmUpCurveUps()
		{
			this.EnsureMetaDataValid();
			int i = 0;
			int num = (this.Closed ? this.Count : (this.Count - 1));
			while (i < num)
			{
				this.EvaluateUpVectorsForCurve(i, this.m_MetaData[i].UpVectors);
				i++;
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000056C0 File Offset: 0x000038C0
		public float3 GetCurveUpVector(int index, float t)
		{
			this.EnsureMetaDataValid();
			float3[] upVectors = this.m_MetaData[index].UpVectors;
			if (math.all(upVectors[0] == float3.zero))
			{
				this.EvaluateUpVectorsForCurve(index, upVectors);
			}
			float num = 1f / (float)(upVectors.Length - 1);
			float num2 = 0f;
			for (int i = 0; i < upVectors.Length; i++)
			{
				if (t <= num2 + num)
				{
					return Vector3.Lerp(upVectors[i], upVectors[i + 1], (t - num2) / num);
				}
				num2 += num;
			}
			return upVectors[upVectors.Length - 1];
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005765 File Offset: 0x00003965
		public void Warmup()
		{
			this.GetLength();
			this.WarmUpCurveUps();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005774 File Offset: 0x00003974
		public void Resize(int newSize)
		{
			int count = this.Count;
			newSize = math.max(0, newSize);
			if (newSize == count)
			{
				return;
			}
			if (newSize > count)
			{
				while (this.m_Knots.Count < newSize)
				{
					this.Add(default(BezierKnot));
				}
				return;
			}
			if (newSize < count)
			{
				while (newSize < this.Count)
				{
					this.RemoveAt(this.Count - 1);
				}
				int num = newSize - 1;
				if (num > -1 && num < this.m_Knots.Count)
				{
					this.ApplyTangentModeNoNotify(num, BezierTangent.Out);
				}
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000057F5 File Offset: 0x000039F5
		public BezierKnot[] ToArray()
		{
			return this.m_Knots.ToArray();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005804 File Offset: 0x00003A04
		public void Copy(Spline copyFrom)
		{
			if (copyFrom == this)
			{
				return;
			}
			this.m_Closed = copyFrom.Closed;
			this.m_Knots.Clear();
			this.m_Knots.AddRange(copyFrom.m_Knots);
			this.m_MetaData.Clear();
			for (int i = 0; i < copyFrom.m_MetaData.Count; i++)
			{
				this.m_MetaData.Add(new Spline.MetaData(copyFrom.m_MetaData[i]));
			}
			this.SetDirty(SplineModification.Default, -1);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005883 File Offset: 0x00003A83
		public IEnumerator<BezierKnot> GetEnumerator()
		{
			return this.m_Knots.GetEnumerator();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005895 File Offset: 0x00003A95
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.m_Knots.GetEnumerator();
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000058A7 File Offset: 0x00003AA7
		public void Add(BezierKnot item)
		{
			this.Add(item, TangentMode.Broken);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000058B1 File Offset: 0x00003AB1
		public void Add(float3 knotPosition, TangentMode tangentMode = TangentMode.AutoSmooth)
		{
			this.Insert(this.Count, knotPosition, tangentMode);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000058C1 File Offset: 0x00003AC1
		public void AddRange(IEnumerable<float3> knotPositions, TangentMode tangentMode = TangentMode.AutoSmooth)
		{
			this.InsertRange(this.Count, knotPositions, tangentMode);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000058D1 File Offset: 0x00003AD1
		public void Insert(int index, float3 knotPosition, TangentMode tangentMode = TangentMode.AutoSmooth)
		{
			if (tangentMode == TangentMode.AutoSmooth)
			{
				this.Insert(index, new BezierKnot(knotPosition), tangentMode);
				return;
			}
			this.CacheKnotOperationCurves(index);
			this.InsertNoNotify(index, new BezierKnot(knotPosition), TangentMode.AutoSmooth, 0.33333334f);
			this.SetTangentModeNoNotify(index, tangentMode, BezierTangent.Out);
			this.SetDirty(SplineModification.KnotInserted, index);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005910 File Offset: 0x00003B10
		public void InsertRange(int index, IEnumerable<float3> knotPositions, TangentMode tangentMode = TangentMode.AutoSmooth)
		{
			this.InsertRangeNoNotify(index, knotPositions, tangentMode, true);
			this.SetDirty(SplineModification.KnotInserted, -1);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005924 File Offset: 0x00003B24
		private void InsertRangeNoNotify(int index, IEnumerable<float3> knotPositions, TangentMode tangentMode = TangentMode.AutoSmooth, bool cacheCurves = false)
		{
			int num = 0;
			foreach (float3 @float in knotPositions)
			{
				int num2 = index + num;
				if (cacheCurves)
				{
					this.CacheKnotOperationCurves(num2);
				}
				this.InsertNoNotify(num2, new BezierKnot(@float), TangentMode.AutoSmooth, 0.33333334f);
				num++;
			}
			if (tangentMode != TangentMode.AutoSmooth)
			{
				num = 0;
				foreach (float3 float2 in knotPositions)
				{
					int num3 = index + num;
					this.SetTangentModeNoNotify(num3, tangentMode, BezierTangent.Out);
					num++;
				}
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000059D8 File Offset: 0x00003BD8
		public void Add(BezierKnot item, TangentMode mode)
		{
			this.Insert(this.Count, item, mode);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000059E8 File Offset: 0x00003BE8
		public void Add(BezierKnot item, TangentMode mode, float tension)
		{
			this.Insert(this.Count, item, mode, tension);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000059FC File Offset: 0x00003BFC
		public void Add(Spline spline)
		{
			for (int i = 0; i < spline.Count; i++)
			{
				this.Insert(this.Count, spline[i], spline.GetTangentMode(i), spline.GetAutoSmoothTension(i));
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005A3B File Offset: 0x00003C3B
		public void Clear()
		{
			this.m_Knots.Clear();
			this.m_MetaData.Clear();
			this.SetDirty(SplineModification.KnotRemoved, -1);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005A5B File Offset: 0x00003C5B
		public bool Contains(BezierKnot item)
		{
			return this.m_Knots.Contains(item);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005A69 File Offset: 0x00003C69
		public void CopyTo(BezierKnot[] array, int arrayIndex)
		{
			this.m_Knots.CopyTo(array, arrayIndex);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005A78 File Offset: 0x00003C78
		public bool Remove(BezierKnot item)
		{
			int num = this.m_Knots.IndexOf(item);
			if (num >= 0)
			{
				this.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005AA0 File Offset: 0x00003CA0
		internal void RemoveUnusedSplineData()
		{
			this.m_FloatData.RemoveEmpty();
			this.m_Float4Data.RemoveEmpty();
			this.m_IntData.RemoveEmpty();
			this.m_ObjectData.RemoveEmpty();
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005ACE File Offset: 0x00003CCE
		internal void CacheKnotOperationCurves(int index)
		{
			if (this.Count <= 1)
			{
				return;
			}
			this.m_LastKnotChangeCurveLengths.Item1 = this.GetCurveLength(this.PreviousIndex(index));
			if (index < this.Count)
			{
				this.m_LastKnotChangeCurveLengths.Item2 = this.GetCurveLength(index);
			}
		}

		// Token: 0x04000044 RID: 68
		private const TangentMode k_DefaultTangentMode = TangentMode.Broken;

		// Token: 0x04000045 RID: 69
		private const BezierTangent k_DefaultMainTangent = BezierTangent.Out;

		// Token: 0x04000046 RID: 70
		private const int k_BatchModification = -1;

		// Token: 0x04000047 RID: 71
		private const int k_CurveDistanceLutResolution = 30;

		// Token: 0x04000048 RID: 72
		[SerializeField]
		[Obsolete]
		[HideInInspector]
		private SplineType m_EditModeType = SplineType.Bezier;

		// Token: 0x04000049 RID: 73
		[SerializeField]
		private List<BezierKnot> m_Knots = new List<BezierKnot>();

		// Token: 0x0400004A RID: 74
		private float m_Length = -1f;

		// Token: 0x0400004B RID: 75
		[SerializeField]
		[HideInInspector]
		private List<Spline.MetaData> m_MetaData = new List<Spline.MetaData>();

		// Token: 0x0400004C RID: 76
		[SerializeField]
		private bool m_Closed;

		// Token: 0x0400004D RID: 77
		[SerializeField]
		private SplineDataDictionary<int> m_IntData = new SplineDataDictionary<int>();

		// Token: 0x0400004E RID: 78
		[SerializeField]
		private SplineDataDictionary<float> m_FloatData = new SplineDataDictionary<float>();

		// Token: 0x0400004F RID: 79
		[SerializeField]
		private SplineDataDictionary<float4> m_Float4Data = new SplineDataDictionary<float4>();

		// Token: 0x04000050 RID: 80
		[SerializeField]
		private SplineDataDictionary<Object> m_ObjectData = new SplineDataDictionary<Object>();

		// Token: 0x04000053 RID: 83
		[TupleElementNames(new string[] { "curve0", "curve1" })]
		private ValueTuple<float, float> m_LastKnotChangeCurveLengths;

		// Token: 0x02000057 RID: 87
		[Serializable]
		private sealed class MetaData
		{
			// Token: 0x170000B9 RID: 185
			// (get) Token: 0x06000342 RID: 834 RVA: 0x0001055E File Offset: 0x0000E75E
			public DistanceToInterpolation[] DistanceToInterpolation
			{
				get
				{
					if (this.m_DistanceToInterpolation == null || this.m_DistanceToInterpolation.Length != 30)
					{
						this.m_DistanceToInterpolation = new DistanceToInterpolation[30];
						this.InvalidateCache();
					}
					return this.m_DistanceToInterpolation;
				}
			}

			// Token: 0x170000BA RID: 186
			// (get) Token: 0x06000343 RID: 835 RVA: 0x0001058D File Offset: 0x0000E78D
			public float3[] UpVectors
			{
				get
				{
					if (this.m_UpVectors == null || this.m_UpVectors.Length != 30)
					{
						this.m_UpVectors = new float3[30];
						this.InvalidateCache();
					}
					return this.m_UpVectors;
				}
			}

			// Token: 0x06000344 RID: 836 RVA: 0x000105BC File Offset: 0x0000E7BC
			public MetaData()
			{
				this.Mode = TangentMode.Broken;
				this.Tension = 0.5f;
				this.InvalidateCache();
			}

			// Token: 0x06000345 RID: 837 RVA: 0x000105F8 File Offset: 0x0000E7F8
			public MetaData(Spline.MetaData toCopy)
			{
				this.Mode = toCopy.Mode;
				this.Tension = toCopy.Tension;
				Array.Copy(toCopy.DistanceToInterpolation, this.DistanceToInterpolation, this.DistanceToInterpolation.Length);
				Array.Copy(toCopy.UpVectors, this.UpVectors, this.UpVectors.Length);
			}

			// Token: 0x06000346 RID: 838 RVA: 0x0001066F File Offset: 0x0000E86F
			public void InvalidateCache()
			{
				this.DistanceToInterpolation[0] = UnityEngine.Splines.DistanceToInterpolation.Invalid;
				this.UpVectors[0] = Vector3.zero;
			}

			// Token: 0x04000114 RID: 276
			public TangentMode Mode;

			// Token: 0x04000115 RID: 277
			public float Tension;

			// Token: 0x04000116 RID: 278
			private DistanceToInterpolation[] m_DistanceToInterpolation = new DistanceToInterpolation[30];

			// Token: 0x04000117 RID: 279
			private float3[] m_UpVectors = new float3[30];
		}
	}
}
