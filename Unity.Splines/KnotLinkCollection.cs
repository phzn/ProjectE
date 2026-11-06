using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.Splines
{
	// Token: 0x02000015 RID: 21
	[Serializable]
	public sealed class KnotLinkCollection
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003967 File Offset: 0x00001B67
		public int Count
		{
			get
			{
				return this.m_KnotsLink.Length;
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003974 File Offset: 0x00001B74
		private KnotLinkCollection.KnotLink GetKnotLinksInternal(SplineKnotIndex index)
		{
			foreach (KnotLinkCollection.KnotLink knotLink in this.m_KnotsLink)
			{
				if (Array.IndexOf<SplineKnotIndex>(knotLink.Knots, index) >= 0)
				{
					return knotLink;
				}
			}
			return null;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000039AC File Offset: 0x00001BAC
		public bool TryGetKnotLinks(SplineKnotIndex knotIndex, out IReadOnlyList<SplineKnotIndex> linkedKnots)
		{
			linkedKnots = this.GetKnotLinksInternal(knotIndex);
			return linkedKnots != null;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000039BC File Offset: 0x00001BBC
		public IReadOnlyList<SplineKnotIndex> GetKnotLinks(SplineKnotIndex knotIndex)
		{
			IReadOnlyList<SplineKnotIndex> readOnlyList;
			if (this.TryGetKnotLinks(knotIndex, out readOnlyList))
			{
				return readOnlyList;
			}
			return new KnotLinkCollection.KnotLink
			{
				Knots = new SplineKnotIndex[] { knotIndex }
			};
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000039F1 File Offset: 0x00001BF1
		public void Clear()
		{
			this.m_KnotsLink = new KnotLinkCollection.KnotLink[0];
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003A00 File Offset: 0x00001C00
		public void Link(SplineKnotIndex knotA, SplineKnotIndex knotB)
		{
			if (knotA.Equals(knotB))
			{
				return;
			}
			KnotLinkCollection.KnotLink knotLinksInternal = this.GetKnotLinksInternal(knotA);
			KnotLinkCollection.KnotLink knotLinksInternal2 = this.GetKnotLinksInternal(knotB);
			if (knotLinksInternal != null && knotLinksInternal2 != null)
			{
				if (knotLinksInternal.Equals(knotLinksInternal2))
				{
					return;
				}
				SplineKnotIndex[] array = new SplineKnotIndex[knotLinksInternal.Knots.Length + knotLinksInternal2.Knots.Length];
				Array.Copy(knotLinksInternal.Knots, array, knotLinksInternal.Knots.Length);
				Array.Copy(knotLinksInternal2.Knots, 0, array, knotLinksInternal.Knots.Length, knotLinksInternal2.Knots.Length);
				knotLinksInternal.Knots = array;
				ArrayUtility.Remove<KnotLinkCollection.KnotLink>(ref this.m_KnotsLink, knotLinksInternal2);
				return;
			}
			else
			{
				if (knotLinksInternal2 != null)
				{
					SplineKnotIndex[] knots = knotLinksInternal2.Knots;
					ArrayUtility.Add<SplineKnotIndex>(ref knots, knotA);
					knotLinksInternal2.Knots = knots;
					return;
				}
				if (knotLinksInternal != null)
				{
					SplineKnotIndex[] knots2 = knotLinksInternal.Knots;
					ArrayUtility.Add<SplineKnotIndex>(ref knots2, knotB);
					knotLinksInternal.Knots = knots2;
					return;
				}
				KnotLinkCollection.KnotLink knotLink = new KnotLinkCollection.KnotLink
				{
					Knots = new SplineKnotIndex[] { knotA, knotB }
				};
				ArrayUtility.Add<KnotLinkCollection.KnotLink>(ref this.m_KnotsLink, knotLink);
				return;
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003B00 File Offset: 0x00001D00
		public void Unlink(SplineKnotIndex knot)
		{
			KnotLinkCollection.KnotLink knotLinksInternal = this.GetKnotLinksInternal(knot);
			if (knotLinksInternal == null)
			{
				return;
			}
			SplineKnotIndex[] knots = knotLinksInternal.Knots;
			ArrayUtility.Remove<SplineKnotIndex>(ref knots, knot);
			knotLinksInternal.Knots = knots;
			if (knotLinksInternal.Knots.Length < 2)
			{
				ArrayUtility.Remove<KnotLinkCollection.KnotLink>(ref this.m_KnotsLink, knotLinksInternal);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003B48 File Offset: 0x00001D48
		public void SplineRemoved(int splineIndex)
		{
			List<int> list = new List<int>(1);
			for (int i = this.m_KnotsLink.Length - 1; i >= 0; i--)
			{
				KnotLinkCollection.KnotLink knotLink = this.m_KnotsLink[i];
				list.Clear();
				for (int j = 0; j < knotLink.Knots.Length; j++)
				{
					if (knotLink.Knots[j].Spline == splineIndex)
					{
						list.Add(j);
					}
				}
				if (knotLink.Knots.Length - list.Count < 2)
				{
					ArrayUtility.RemoveAt<KnotLinkCollection.KnotLink>(ref this.m_KnotsLink, i);
				}
				else
				{
					SplineKnotIndex[] knots = knotLink.Knots;
					ArrayUtility.SortedRemoveAt<SplineKnotIndex>(ref knots, list);
					knotLink.Knots = knots;
				}
				for (int k = 0; k < knotLink.Knots.Length; k++)
				{
					SplineKnotIndex splineKnotIndex = knotLink.Knots[k];
					if (splineKnotIndex.Spline > splineIndex)
					{
						knotLink.Knots[k] = new SplineKnotIndex(splineKnotIndex.Spline - 1, splineKnotIndex.Knot);
					}
				}
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003C40 File Offset: 0x00001E40
		public void SplineIndexChanged(int previousIndex, int newIndex)
		{
			for (int i = this.m_KnotsLink.Length - 1; i >= 0; i--)
			{
				KnotLinkCollection.KnotLink knotLink = this.m_KnotsLink[i];
				for (int j = 0; j < knotLink.Knots.Length; j++)
				{
					SplineKnotIndex splineKnotIndex = knotLink.Knots[j];
					if (splineKnotIndex.Spline == previousIndex)
					{
						knotLink.Knots[j] = new SplineKnotIndex(newIndex, splineKnotIndex.Knot);
					}
					else if (splineKnotIndex.Spline > previousIndex && splineKnotIndex.Spline <= newIndex)
					{
						knotLink.Knots[j] = new SplineKnotIndex(splineKnotIndex.Spline - 1, splineKnotIndex.Knot);
					}
					else if (splineKnotIndex.Spline < previousIndex && splineKnotIndex.Spline >= newIndex)
					{
						knotLink.Knots[j] = new SplineKnotIndex(splineKnotIndex.Spline + 1, splineKnotIndex.Knot);
					}
				}
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003D1E File Offset: 0x00001F1E
		public void KnotIndexChanged(int splineIndex, int previousKnotIndex, int newKnotIndex)
		{
			this.KnotIndexChanged(new SplineKnotIndex(splineIndex, previousKnotIndex), new SplineKnotIndex(splineIndex, newKnotIndex));
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003D34 File Offset: 0x00001F34
		public void KnotIndexChanged(SplineKnotIndex previousIndex, SplineKnotIndex newIndex)
		{
			if (previousIndex.Knot > newIndex.Knot)
			{
				previousIndex.Knot++;
			}
			else
			{
				newIndex.Knot++;
			}
			this.KnotInserted(newIndex);
			this.Link(previousIndex, newIndex);
			this.KnotRemoved(previousIndex);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003D74 File Offset: 0x00001F74
		public void KnotRemoved(int splineIndex, int knotIndex)
		{
			this.KnotRemoved(new SplineKnotIndex(splineIndex, knotIndex));
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003D83 File Offset: 0x00001F83
		public void KnotRemoved(SplineKnotIndex index)
		{
			this.Unlink(index);
			this.ShiftKnotIndices(index, -1);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003D94 File Offset: 0x00001F94
		public void KnotInserted(int splineIndex, int knotIndex)
		{
			this.KnotInserted(new SplineKnotIndex(splineIndex, knotIndex));
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003DA3 File Offset: 0x00001FA3
		public void KnotInserted(SplineKnotIndex index)
		{
			this.ShiftKnotIndices(index, 1);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003DB0 File Offset: 0x00001FB0
		public void ShiftKnotIndices(SplineKnotIndex index, int offset)
		{
			foreach (KnotLinkCollection.KnotLink knotLink in this.m_KnotsLink)
			{
				for (int j = 0; j < knotLink.Knots.Length; j++)
				{
					SplineKnotIndex splineKnotIndex = knotLink.Knots[j];
					if (splineKnotIndex.Spline == index.Spline && splineKnotIndex.Knot >= index.Knot)
					{
						knotLink.Knots[j] = new SplineKnotIndex(splineKnotIndex.Spline, splineKnotIndex.Knot + offset);
					}
				}
			}
		}

		// Token: 0x04000030 RID: 48
		[SerializeField]
		private KnotLinkCollection.KnotLink[] m_KnotsLink = new KnotLinkCollection.KnotLink[0];

		// Token: 0x02000054 RID: 84
		[Serializable]
		private sealed class KnotLink : IReadOnlyList<SplineKnotIndex>, IEnumerable<SplineKnotIndex>, IEnumerable, IReadOnlyCollection<SplineKnotIndex>
		{
			// Token: 0x06000335 RID: 821 RVA: 0x0001049C File Offset: 0x0000E69C
			public IEnumerator<SplineKnotIndex> GetEnumerator()
			{
				return ((IEnumerable<SplineKnotIndex>)this.Knots).GetEnumerator();
			}

			// Token: 0x06000336 RID: 822 RVA: 0x000104A9 File Offset: 0x0000E6A9
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.Knots.GetEnumerator();
			}

			// Token: 0x170000B4 RID: 180
			// (get) Token: 0x06000337 RID: 823 RVA: 0x000104B6 File Offset: 0x0000E6B6
			public int Count
			{
				get
				{
					return this.Knots.Length;
				}
			}

			// Token: 0x170000B5 RID: 181
			public SplineKnotIndex this[int index]
			{
				get
				{
					return this.Knots[index];
				}
			}

			// Token: 0x04000110 RID: 272
			public SplineKnotIndex[] Knots;
		}
	}
}
