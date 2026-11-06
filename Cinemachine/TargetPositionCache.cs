using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200004D RID: 77
	internal class TargetPositionCache
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000350 RID: 848 RVA: 0x00014DEE File Offset: 0x00012FEE
		// (set) Token: 0x06000351 RID: 849 RVA: 0x00014DF5 File Offset: 0x00012FF5
		public static TargetPositionCache.Mode CacheMode
		{
			get
			{
				return TargetPositionCache.m_CacheMode;
			}
			set
			{
				if (value == TargetPositionCache.m_CacheMode)
				{
					return;
				}
				TargetPositionCache.m_CacheMode = value;
				switch (value)
				{
				default:
					TargetPositionCache.ClearCache();
					return;
				case TargetPositionCache.Mode.Record:
					TargetPositionCache.ClearCache();
					return;
				case TargetPositionCache.Mode.Playback:
					TargetPositionCache.CreatePlaybackCurves();
					return;
				}
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000352 RID: 850 RVA: 0x00014E29 File Offset: 0x00013029
		public static bool IsRecording
		{
			get
			{
				return TargetPositionCache.UseCache && TargetPositionCache.m_CacheMode == TargetPositionCache.Mode.Record;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000353 RID: 851 RVA: 0x00014E3C File Offset: 0x0001303C
		public static bool CurrentPlaybackTimeValid
		{
			get
			{
				return TargetPositionCache.UseCache && TargetPositionCache.m_CacheMode == TargetPositionCache.Mode.Playback && TargetPositionCache.HasCurrentTime;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000354 RID: 852 RVA: 0x00014E54 File Offset: 0x00013054
		public static bool IsEmpty
		{
			get
			{
				return TargetPositionCache.CacheTimeRange.IsEmpty;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000355 RID: 853 RVA: 0x00014E6E File Offset: 0x0001306E
		public static TargetPositionCache.TimeRange CacheTimeRange
		{
			get
			{
				return TargetPositionCache.m_CacheTimeRange;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000356 RID: 854 RVA: 0x00014E75 File Offset: 0x00013075
		public static bool HasCurrentTime
		{
			get
			{
				return TargetPositionCache.m_CacheTimeRange.Contains(TargetPositionCache.CurrentTime);
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00014E86 File Offset: 0x00013086
		public static void ClearCache()
		{
			TargetPositionCache.m_Cache = ((TargetPositionCache.CacheMode == TargetPositionCache.Mode.Disabled) ? null : new Dictionary<Transform, TargetPositionCache.CacheEntry>());
			TargetPositionCache.m_CacheTimeRange = TargetPositionCache.TimeRange.Empty;
			TargetPositionCache.CurrentTime = 0f;
			TargetPositionCache.CurrentFrame = 0;
			TargetPositionCache.IsCameraCut = false;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00014EBC File Offset: 0x000130BC
		private static void CreatePlaybackCurves()
		{
			if (TargetPositionCache.m_Cache == null)
			{
				TargetPositionCache.m_Cache = new Dictionary<Transform, TargetPositionCache.CacheEntry>();
			}
			foreach (KeyValuePair<Transform, TargetPositionCache.CacheEntry> keyValuePair in TargetPositionCache.m_Cache)
			{
				keyValuePair.Value.CreateCurves();
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00014F04 File Offset: 0x00013104
		public static Vector3 GetTargetPosition(Transform target)
		{
			if (!TargetPositionCache.UseCache || TargetPositionCache.CacheMode == TargetPositionCache.Mode.Disabled)
			{
				return target.position;
			}
			if (TargetPositionCache.CacheMode == TargetPositionCache.Mode.Record && !TargetPositionCache.m_CacheTimeRange.IsEmpty && TargetPositionCache.CurrentTime < TargetPositionCache.m_CacheTimeRange.Start - 0.1f)
			{
				TargetPositionCache.ClearCache();
			}
			if (TargetPositionCache.CacheMode == TargetPositionCache.Mode.Playback && !TargetPositionCache.HasCurrentTime)
			{
				return target.position;
			}
			TargetPositionCache.CacheEntry cacheEntry;
			if (!TargetPositionCache.m_Cache.TryGetValue(target, out cacheEntry))
			{
				if (TargetPositionCache.CacheMode != TargetPositionCache.Mode.Record)
				{
					return target.position;
				}
				cacheEntry = new TargetPositionCache.CacheEntry();
				TargetPositionCache.m_Cache.Add(target, cacheEntry);
			}
			if (TargetPositionCache.CacheMode == TargetPositionCache.Mode.Record)
			{
				cacheEntry.AddRawItem(TargetPositionCache.CurrentTime, TargetPositionCache.IsCameraCut, target);
				TargetPositionCache.m_CacheTimeRange.Include(TargetPositionCache.CurrentTime);
				return target.position;
			}
			if (cacheEntry.Curve == null)
			{
				return target.position;
			}
			return cacheEntry.Curve.Evaluate(TargetPositionCache.CurrentTime).Pos;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00014FF0 File Offset: 0x000131F0
		public static Quaternion GetTargetRotation(Transform target)
		{
			if (TargetPositionCache.CacheMode == TargetPositionCache.Mode.Disabled)
			{
				return target.rotation;
			}
			if (TargetPositionCache.CacheMode == TargetPositionCache.Mode.Record && !TargetPositionCache.m_CacheTimeRange.IsEmpty && TargetPositionCache.CurrentTime < TargetPositionCache.m_CacheTimeRange.Start - 0.1f)
			{
				TargetPositionCache.ClearCache();
			}
			if (TargetPositionCache.CacheMode == TargetPositionCache.Mode.Playback && !TargetPositionCache.HasCurrentTime)
			{
				return target.rotation;
			}
			TargetPositionCache.CacheEntry cacheEntry;
			if (!TargetPositionCache.m_Cache.TryGetValue(target, out cacheEntry))
			{
				if (TargetPositionCache.CacheMode != TargetPositionCache.Mode.Record)
				{
					return target.rotation;
				}
				cacheEntry = new TargetPositionCache.CacheEntry();
				TargetPositionCache.m_Cache.Add(target, cacheEntry);
			}
			if (TargetPositionCache.CacheMode == TargetPositionCache.Mode.Record)
			{
				if (TargetPositionCache.m_CacheTimeRange.End <= TargetPositionCache.CurrentTime)
				{
					cacheEntry.AddRawItem(TargetPositionCache.CurrentTime, TargetPositionCache.IsCameraCut, target);
					TargetPositionCache.m_CacheTimeRange.Include(TargetPositionCache.CurrentTime);
				}
				return target.rotation;
			}
			return cacheEntry.Curve.Evaluate(TargetPositionCache.CurrentTime).Rot;
		}

		// Token: 0x04000230 RID: 560
		public static bool UseCache;

		// Token: 0x04000231 RID: 561
		public const float CacheStepSize = 0.016666668f;

		// Token: 0x04000232 RID: 562
		private static TargetPositionCache.Mode m_CacheMode;

		// Token: 0x04000233 RID: 563
		public static float CurrentTime;

		// Token: 0x04000234 RID: 564
		public static int CurrentFrame;

		// Token: 0x04000235 RID: 565
		public static bool IsCameraCut;

		// Token: 0x04000236 RID: 566
		private static Dictionary<Transform, TargetPositionCache.CacheEntry> m_Cache;

		// Token: 0x04000237 RID: 567
		private static TargetPositionCache.TimeRange m_CacheTimeRange;

		// Token: 0x04000238 RID: 568
		private const float kWraparoundSlush = 0.1f;

		// Token: 0x020000BC RID: 188
		public enum Mode
		{
			// Token: 0x040003C2 RID: 962
			Disabled,
			// Token: 0x040003C3 RID: 963
			Record,
			// Token: 0x040003C4 RID: 964
			Playback
		}

		// Token: 0x020000BD RID: 189
		private class CacheCurve
		{
			// Token: 0x170000E7 RID: 231
			// (get) Token: 0x06000472 RID: 1138 RVA: 0x00019B35 File Offset: 0x00017D35
			public int Count
			{
				get
				{
					return this.m_Cache.Count;
				}
			}

			// Token: 0x06000473 RID: 1139 RVA: 0x00019B42 File Offset: 0x00017D42
			public CacheCurve(float startTime, float endTime, float stepSize)
			{
				this.StepSize = stepSize;
				this.StartTime = startTime;
				this.m_Cache = new List<TargetPositionCache.CacheCurve.Item>(Mathf.CeilToInt((this.StepSize * 0.5f + endTime - startTime) / this.StepSize));
			}

			// Token: 0x06000474 RID: 1140 RVA: 0x00019B7F File Offset: 0x00017D7F
			public void Add(TargetPositionCache.CacheCurve.Item item)
			{
				this.m_Cache.Add(item);
			}

			// Token: 0x06000475 RID: 1141 RVA: 0x00019B90 File Offset: 0x00017D90
			public void AddUntil(TargetPositionCache.CacheCurve.Item item, float time, bool isCut)
			{
				int num = this.m_Cache.Count - 1;
				float num2 = (float)num * this.StepSize;
				float num3 = time - this.StartTime - num2;
				if (isCut)
				{
					for (float num4 = this.StepSize; num4 <= num3; num4 += this.StepSize)
					{
						this.Add(item);
					}
					return;
				}
				TargetPositionCache.CacheCurve.Item item2 = this.m_Cache[num];
				for (float num5 = this.StepSize; num5 <= num3; num5 += this.StepSize)
				{
					this.Add(TargetPositionCache.CacheCurve.Item.Lerp(item2, item, num5 / num3));
				}
			}

			// Token: 0x06000476 RID: 1142 RVA: 0x00019C1C File Offset: 0x00017E1C
			public TargetPositionCache.CacheCurve.Item Evaluate(float time)
			{
				int count = this.m_Cache.Count;
				if (count == 0)
				{
					return TargetPositionCache.CacheCurve.Item.Empty;
				}
				float num = time - this.StartTime;
				int num2 = Mathf.Clamp(Mathf.FloorToInt(num / this.StepSize), 0, count - 1);
				TargetPositionCache.CacheCurve.Item item = this.m_Cache[num2];
				if (num2 == count - 1)
				{
					return item;
				}
				return TargetPositionCache.CacheCurve.Item.Lerp(item, this.m_Cache[num2 + 1], (num - (float)num2 * this.StepSize) / this.StepSize);
			}

			// Token: 0x040003C5 RID: 965
			public float StartTime;

			// Token: 0x040003C6 RID: 966
			public float StepSize;

			// Token: 0x040003C7 RID: 967
			private List<TargetPositionCache.CacheCurve.Item> m_Cache;

			// Token: 0x020000EF RID: 239
			public struct Item
			{
				// Token: 0x0600057B RID: 1403 RVA: 0x000237A4 File Offset: 0x000219A4
				public static TargetPositionCache.CacheCurve.Item Lerp(TargetPositionCache.CacheCurve.Item a, TargetPositionCache.CacheCurve.Item b, float t)
				{
					return new TargetPositionCache.CacheCurve.Item
					{
						Pos = Vector3.LerpUnclamped(a.Pos, b.Pos, t),
						Rot = Quaternion.SlerpUnclamped(a.Rot, b.Rot, t)
					};
				}

				// Token: 0x170000FB RID: 251
				// (get) Token: 0x0600057C RID: 1404 RVA: 0x000237EC File Offset: 0x000219EC
				public static TargetPositionCache.CacheCurve.Item Empty
				{
					get
					{
						return new TargetPositionCache.CacheCurve.Item
						{
							Rot = Quaternion.identity
						};
					}
				}

				// Token: 0x040004A6 RID: 1190
				public Vector3 Pos;

				// Token: 0x040004A7 RID: 1191
				public Quaternion Rot;
			}
		}

		// Token: 0x020000BE RID: 190
		private class CacheEntry
		{
			// Token: 0x06000477 RID: 1143 RVA: 0x00019C9C File Offset: 0x00017E9C
			public void AddRawItem(float time, bool isCut, Transform target)
			{
				float num = time - 0.016666668f;
				int num2 = this.RawItems.Count - 1;
				int num3 = num2;
				while (num3 >= 0 && this.RawItems[num3].Time > num)
				{
					num3--;
				}
				if (num3 == num2)
				{
					this.RawItems.Add(new TargetPositionCache.CacheEntry.RecordingItem
					{
						Time = time,
						IsCut = isCut,
						Item = new TargetPositionCache.CacheCurve.Item
						{
							Pos = target.position,
							Rot = target.rotation
						}
					});
					return;
				}
				int num4 = num3 + 2;
				if (num4 <= num2)
				{
					this.RawItems.RemoveRange(num4, this.RawItems.Count - num4);
				}
				this.RawItems[num3 + 1] = new TargetPositionCache.CacheEntry.RecordingItem
				{
					Time = time,
					IsCut = isCut,
					Item = new TargetPositionCache.CacheCurve.Item
					{
						Pos = target.position,
						Rot = target.rotation
					}
				};
			}

			// Token: 0x06000478 RID: 1144 RVA: 0x00019DAC File Offset: 0x00017FAC
			public void CreateCurves()
			{
				int num = this.RawItems.Count - 1;
				float num2 = ((num < 0) ? 0f : this.RawItems[0].Time);
				float num3 = ((num < 0) ? 0f : this.RawItems[num].Time);
				this.Curve = new TargetPositionCache.CacheCurve(num2, num3, 0.016666668f);
				this.Curve.Add((num < 0) ? TargetPositionCache.CacheCurve.Item.Empty : this.RawItems[0].Item);
				for (int i = 1; i <= num; i++)
				{
					this.Curve.AddUntil(this.RawItems[i].Item, this.RawItems[i].Time, this.RawItems[i].IsCut);
				}
				this.RawItems.Clear();
			}

			// Token: 0x040003C8 RID: 968
			public TargetPositionCache.CacheCurve Curve;

			// Token: 0x040003C9 RID: 969
			private List<TargetPositionCache.CacheEntry.RecordingItem> RawItems = new List<TargetPositionCache.CacheEntry.RecordingItem>();

			// Token: 0x020000F0 RID: 240
			private struct RecordingItem
			{
				// Token: 0x040004A8 RID: 1192
				public float Time;

				// Token: 0x040004A9 RID: 1193
				public bool IsCut;

				// Token: 0x040004AA RID: 1194
				public TargetPositionCache.CacheCurve.Item Item;
			}
		}

		// Token: 0x020000BF RID: 191
		public struct TimeRange
		{
			// Token: 0x170000E8 RID: 232
			// (get) Token: 0x0600047A RID: 1146 RVA: 0x00019EA2 File Offset: 0x000180A2
			public bool IsEmpty
			{
				get
				{
					return this.End < this.Start;
				}
			}

			// Token: 0x0600047B RID: 1147 RVA: 0x00019EB2 File Offset: 0x000180B2
			public bool Contains(float time)
			{
				return time >= this.Start && time <= this.End;
			}

			// Token: 0x170000E9 RID: 233
			// (get) Token: 0x0600047C RID: 1148 RVA: 0x00019ECC File Offset: 0x000180CC
			public static TargetPositionCache.TimeRange Empty
			{
				get
				{
					return new TargetPositionCache.TimeRange
					{
						Start = float.MaxValue,
						End = float.MinValue
					};
				}
			}

			// Token: 0x0600047D RID: 1149 RVA: 0x00019EFA File Offset: 0x000180FA
			public void Include(float time)
			{
				this.Start = Mathf.Min(this.Start, time);
				this.End = Mathf.Max(this.End, time);
			}

			// Token: 0x040003CA RID: 970
			public float Start;

			// Token: 0x040003CB RID: 971
			public float End;
		}
	}
}
