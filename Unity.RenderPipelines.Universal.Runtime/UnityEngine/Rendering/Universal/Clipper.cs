using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000022 RID: 34
	internal class Clipper : ClipperBase
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00004FA0 File Offset: 0x000031A0
		public Clipper(int InitOptions = 0)
		{
			this.m_Scanbeam = null;
			this.m_Maxima = null;
			this.m_ActiveEdges = null;
			this.m_SortedEdges = null;
			this.m_IntersectList = new List<IntersectNode>();
			this.m_IntersectNodeComparer = new MyIntersectNodeSort();
			this.m_ExecuteLocked = false;
			this.m_UsingPolyTree = false;
			this.m_PolyOuts = new List<OutRec>();
			this.m_Joins = new List<Join>();
			this.m_GhostJoins = new List<Join>();
			this.ReverseSolution = (1 & InitOptions) != 0;
			this.StrictlySimple = (2 & InitOptions) != 0;
			base.PreserveCollinear = (4 & InitOptions) != 0;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005038 File Offset: 0x00003238
		private void InsertMaxima(long X)
		{
			Maxima maxima = new Maxima();
			maxima.X = X;
			if (this.m_Maxima == null)
			{
				this.m_Maxima = maxima;
				this.m_Maxima.Next = null;
				this.m_Maxima.Prev = null;
				return;
			}
			if (X < this.m_Maxima.X)
			{
				maxima.Next = this.m_Maxima;
				maxima.Prev = null;
				this.m_Maxima = maxima;
				return;
			}
			Maxima maxima2 = this.m_Maxima;
			while (maxima2.Next != null && X >= maxima2.Next.X)
			{
				maxima2 = maxima2.Next;
			}
			if (X == maxima2.X)
			{
				return;
			}
			maxima.Next = maxima2.Next;
			maxima.Prev = maxima2;
			if (maxima2.Next != null)
			{
				maxima2.Next.Prev = maxima;
			}
			maxima2.Next = maxima;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00005101 File Offset: 0x00003301
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00005109 File Offset: 0x00003309
		public int LastIndex { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00005112 File Offset: 0x00003312
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x0000511A File Offset: 0x0000331A
		public bool ReverseSolution { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00005123 File Offset: 0x00003323
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x0000512B File Offset: 0x0000332B
		public bool StrictlySimple { get; set; }

		// Token: 0x060000A9 RID: 169 RVA: 0x00005134 File Offset: 0x00003334
		public bool Execute(ClipType clipType, List<List<IntPoint>> solution, PolyFillType FillType = PolyFillType.pftEvenOdd)
		{
			return this.Execute(clipType, solution, FillType, FillType);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005140 File Offset: 0x00003340
		public bool Execute(ClipType clipType, PolyTree polytree, PolyFillType FillType = PolyFillType.pftEvenOdd)
		{
			return this.Execute(clipType, polytree, FillType, FillType);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000514C File Offset: 0x0000334C
		public bool Execute(ClipType clipType, List<List<IntPoint>> solution, PolyFillType subjFillType, PolyFillType clipFillType)
		{
			if (this.m_ExecuteLocked)
			{
				return false;
			}
			if (this.m_HasOpenPaths)
			{
				throw new ClipperException("Error: PolyTree struct is needed for open path clipping.");
			}
			this.m_ExecuteLocked = true;
			solution.Clear();
			this.m_SubjFillType = subjFillType;
			this.m_ClipFillType = clipFillType;
			this.m_ClipType = clipType;
			this.m_UsingPolyTree = false;
			bool flag;
			try
			{
				flag = this.ExecuteInternal();
				if (flag)
				{
					this.BuildResult(solution);
				}
			}
			finally
			{
				this.DisposeAllPolyPts();
				this.m_ExecuteLocked = false;
			}
			return flag;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000051D4 File Offset: 0x000033D4
		public bool Execute(ClipType clipType, PolyTree polytree, PolyFillType subjFillType, PolyFillType clipFillType)
		{
			if (this.m_ExecuteLocked)
			{
				return false;
			}
			this.m_ExecuteLocked = true;
			this.m_SubjFillType = subjFillType;
			this.m_ClipFillType = clipFillType;
			this.m_ClipType = clipType;
			this.m_UsingPolyTree = true;
			bool flag;
			try
			{
				flag = this.ExecuteInternal();
				if (flag)
				{
					this.BuildResult2(polytree);
				}
			}
			finally
			{
				this.DisposeAllPolyPts();
				this.m_ExecuteLocked = false;
			}
			return flag;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005244 File Offset: 0x00003444
		internal void FixHoleLinkage(OutRec outRec)
		{
			if (outRec.FirstLeft == null || (outRec.IsHole != outRec.FirstLeft.IsHole && outRec.FirstLeft.Pts != null))
			{
				return;
			}
			OutRec outRec2 = outRec.FirstLeft;
			while (outRec2 != null && (outRec2.IsHole == outRec.IsHole || outRec2.Pts == null))
			{
				outRec2 = outRec2.FirstLeft;
			}
			outRec.FirstLeft = outRec2;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000052AC File Offset: 0x000034AC
		private bool ExecuteInternal()
		{
			bool flag;
			try
			{
				this.Reset();
				this.m_SortedEdges = null;
				this.m_Maxima = null;
				long num;
				if (!base.PopScanbeam(out num))
				{
					flag = false;
				}
				else
				{
					this.InsertLocalMinimaIntoAEL(num);
					long num2;
					while (base.PopScanbeam(out num2) || base.LocalMinimaPending())
					{
						this.ProcessHorizontals();
						this.m_GhostJoins.Clear();
						if (!this.ProcessIntersections(num2))
						{
							return false;
						}
						this.ProcessEdgesAtTopOfScanbeam(num2);
						num = num2;
						this.InsertLocalMinimaIntoAEL(num);
					}
					foreach (OutRec outRec in this.m_PolyOuts)
					{
						if (outRec.Pts != null && !outRec.IsOpen && (outRec.IsHole ^ this.ReverseSolution) == this.Area(outRec) > 0.0)
						{
							this.ReversePolyPtLinks(outRec.Pts);
						}
					}
					this.JoinCommonEdges();
					foreach (OutRec outRec2 in this.m_PolyOuts)
					{
						if (outRec2.Pts != null)
						{
							if (outRec2.IsOpen)
							{
								this.FixupOutPolyline(outRec2);
							}
							else
							{
								this.FixupOutPolygon(outRec2);
							}
						}
					}
					if (this.StrictlySimple)
					{
						this.DoSimplePolygons();
					}
					flag = true;
				}
			}
			finally
			{
				this.m_Joins.Clear();
				this.m_GhostJoins.Clear();
			}
			return flag;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000546C File Offset: 0x0000366C
		private void DisposeAllPolyPts()
		{
			for (int i = 0; i < this.m_PolyOuts.Count; i++)
			{
				base.DisposeOutRec(i);
			}
			this.m_PolyOuts.Clear();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000054A4 File Offset: 0x000036A4
		private void AddJoin(OutPt Op1, OutPt Op2, IntPoint OffPt)
		{
			Join join = new Join();
			join.OutPt1 = Op1;
			join.OutPt2 = Op2;
			join.OffPt = OffPt;
			this.m_Joins.Add(join);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000054D8 File Offset: 0x000036D8
		private void AddGhostJoin(OutPt Op, IntPoint OffPt)
		{
			Join join = new Join();
			join.OutPt1 = Op;
			join.OffPt = OffPt;
			this.m_GhostJoins.Add(join);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005508 File Offset: 0x00003708
		private void InsertLocalMinimaIntoAEL(long botY)
		{
			LocalMinima localMinima;
			while (base.PopLocalMinima(botY, out localMinima))
			{
				TEdge leftBound = localMinima.LeftBound;
				TEdge rightBound = localMinima.RightBound;
				OutPt outPt = null;
				if (leftBound == null)
				{
					this.InsertEdgeIntoAEL(rightBound, null);
					this.SetWindingCount(rightBound);
					if (this.IsContributing(rightBound))
					{
						outPt = this.AddOutPt(rightBound, rightBound.Bot);
					}
				}
				else if (rightBound == null)
				{
					this.InsertEdgeIntoAEL(leftBound, null);
					this.SetWindingCount(leftBound);
					if (this.IsContributing(leftBound))
					{
						outPt = this.AddOutPt(leftBound, leftBound.Bot);
					}
					base.InsertScanbeam(leftBound.Top.Y);
				}
				else
				{
					this.InsertEdgeIntoAEL(leftBound, null);
					this.InsertEdgeIntoAEL(rightBound, leftBound);
					this.SetWindingCount(leftBound);
					rightBound.WindCnt = leftBound.WindCnt;
					rightBound.WindCnt2 = leftBound.WindCnt2;
					if (this.IsContributing(leftBound))
					{
						outPt = this.AddLocalMinPoly(leftBound, rightBound, leftBound.Bot);
					}
					base.InsertScanbeam(leftBound.Top.Y);
				}
				if (rightBound != null)
				{
					if (ClipperBase.IsHorizontal(rightBound))
					{
						if (rightBound.NextInLML != null)
						{
							base.InsertScanbeam(rightBound.NextInLML.Top.Y);
						}
						this.AddEdgeToSEL(rightBound);
					}
					else
					{
						base.InsertScanbeam(rightBound.Top.Y);
					}
				}
				if (leftBound != null && rightBound != null)
				{
					if (outPt != null && ClipperBase.IsHorizontal(rightBound) && this.m_GhostJoins.Count > 0 && rightBound.WindDelta != 0)
					{
						for (int i = 0; i < this.m_GhostJoins.Count; i++)
						{
							Join join = this.m_GhostJoins[i];
							if (this.HorzSegmentsOverlap(join.OutPt1.Pt.X, join.OffPt.X, rightBound.Bot.X, rightBound.Top.X))
							{
								this.AddJoin(join.OutPt1, outPt, join.OffPt);
							}
						}
					}
					if (leftBound.OutIdx >= 0 && leftBound.PrevInAEL != null && leftBound.PrevInAEL.Curr.X == leftBound.Bot.X && leftBound.PrevInAEL.OutIdx >= 0 && ClipperBase.SlopesEqual(leftBound.PrevInAEL.Curr, leftBound.PrevInAEL.Top, leftBound.Curr, leftBound.Top, this.m_UseFullRange) && leftBound.WindDelta != 0 && leftBound.PrevInAEL.WindDelta != 0)
					{
						OutPt outPt2 = this.AddOutPt(leftBound.PrevInAEL, leftBound.Bot);
						this.AddJoin(outPt, outPt2, leftBound.Top);
					}
					if (leftBound.NextInAEL != rightBound)
					{
						if (rightBound.OutIdx >= 0 && rightBound.PrevInAEL.OutIdx >= 0 && ClipperBase.SlopesEqual(rightBound.PrevInAEL.Curr, rightBound.PrevInAEL.Top, rightBound.Curr, rightBound.Top, this.m_UseFullRange) && rightBound.WindDelta != 0 && rightBound.PrevInAEL.WindDelta != 0)
						{
							OutPt outPt3 = this.AddOutPt(rightBound.PrevInAEL, rightBound.Bot);
							this.AddJoin(outPt, outPt3, rightBound.Top);
						}
						TEdge tedge = leftBound.NextInAEL;
						if (tedge != null)
						{
							while (tedge != rightBound)
							{
								this.IntersectEdges(rightBound, tedge, leftBound.Curr);
								tedge = tedge.NextInAEL;
							}
						}
					}
				}
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005850 File Offset: 0x00003A50
		private void InsertEdgeIntoAEL(TEdge edge, TEdge startEdge)
		{
			if (this.m_ActiveEdges == null)
			{
				edge.PrevInAEL = null;
				edge.NextInAEL = null;
				this.m_ActiveEdges = edge;
				return;
			}
			if (startEdge == null && this.E2InsertsBeforeE1(this.m_ActiveEdges, edge))
			{
				edge.PrevInAEL = null;
				edge.NextInAEL = this.m_ActiveEdges;
				this.m_ActiveEdges.PrevInAEL = edge;
				this.m_ActiveEdges = edge;
				return;
			}
			if (startEdge == null)
			{
				startEdge = this.m_ActiveEdges;
			}
			while (startEdge.NextInAEL != null && !this.E2InsertsBeforeE1(startEdge.NextInAEL, edge))
			{
				startEdge = startEdge.NextInAEL;
			}
			edge.NextInAEL = startEdge.NextInAEL;
			if (startEdge.NextInAEL != null)
			{
				startEdge.NextInAEL.PrevInAEL = edge;
			}
			edge.PrevInAEL = startEdge;
			startEdge.NextInAEL = edge;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005910 File Offset: 0x00003B10
		private bool E2InsertsBeforeE1(TEdge e1, TEdge e2)
		{
			if (e2.Curr.X != e1.Curr.X)
			{
				return e2.Curr.X < e1.Curr.X;
			}
			if (e2.Top.Y > e1.Top.Y)
			{
				return e2.Top.X < Clipper.TopX(e1, e2.Top.Y);
			}
			return e1.Top.X > Clipper.TopX(e2, e1.Top.Y);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000059A3 File Offset: 0x00003BA3
		private bool IsEvenOddFillType(TEdge edge)
		{
			if (edge.PolyTyp == PolyType.ptSubject)
			{
				return this.m_SubjFillType == PolyFillType.pftEvenOdd;
			}
			return this.m_ClipFillType == PolyFillType.pftEvenOdd;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000059C0 File Offset: 0x00003BC0
		private bool IsEvenOddAltFillType(TEdge edge)
		{
			if (edge.PolyTyp == PolyType.ptSubject)
			{
				return this.m_ClipFillType == PolyFillType.pftEvenOdd;
			}
			return this.m_SubjFillType == PolyFillType.pftEvenOdd;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000059E0 File Offset: 0x00003BE0
		private bool IsContributing(TEdge edge)
		{
			PolyFillType polyFillType;
			PolyFillType polyFillType2;
			if (edge.PolyTyp == PolyType.ptSubject)
			{
				polyFillType = this.m_SubjFillType;
				polyFillType2 = this.m_ClipFillType;
			}
			else
			{
				polyFillType = this.m_ClipFillType;
				polyFillType2 = this.m_SubjFillType;
			}
			switch (polyFillType)
			{
			case PolyFillType.pftEvenOdd:
				if (edge.WindDelta == 0 && edge.WindCnt != 1)
				{
					return false;
				}
				break;
			case PolyFillType.pftNonZero:
				if (Math.Abs(edge.WindCnt) != 1)
				{
					return false;
				}
				break;
			case PolyFillType.pftPositive:
				if (edge.WindCnt != 1)
				{
					return false;
				}
				break;
			default:
				if (edge.WindCnt != -1)
				{
					return false;
				}
				break;
			}
			switch (this.m_ClipType)
			{
			case ClipType.ctIntersection:
				if (polyFillType2 <= PolyFillType.pftNonZero)
				{
					return edge.WindCnt2 != 0;
				}
				if (polyFillType2 != PolyFillType.pftPositive)
				{
					return edge.WindCnt2 < 0;
				}
				return edge.WindCnt2 > 0;
			case ClipType.ctUnion:
				if (polyFillType2 <= PolyFillType.pftNonZero)
				{
					return edge.WindCnt2 == 0;
				}
				if (polyFillType2 != PolyFillType.pftPositive)
				{
					return edge.WindCnt2 >= 0;
				}
				return edge.WindCnt2 <= 0;
			case ClipType.ctDifference:
				if (edge.PolyTyp == PolyType.ptSubject)
				{
					if (polyFillType2 <= PolyFillType.pftNonZero)
					{
						return edge.WindCnt2 == 0;
					}
					if (polyFillType2 != PolyFillType.pftPositive)
					{
						return edge.WindCnt2 >= 0;
					}
					return edge.WindCnt2 <= 0;
				}
				else
				{
					if (polyFillType2 <= PolyFillType.pftNonZero)
					{
						return edge.WindCnt2 != 0;
					}
					if (polyFillType2 != PolyFillType.pftPositive)
					{
						return edge.WindCnt2 < 0;
					}
					return edge.WindCnt2 > 0;
				}
				break;
			case ClipType.ctXor:
				if (edge.WindDelta != 0)
				{
					return true;
				}
				if (polyFillType2 <= PolyFillType.pftNonZero)
				{
					return edge.WindCnt2 == 0;
				}
				if (polyFillType2 != PolyFillType.pftPositive)
				{
					return edge.WindCnt2 >= 0;
				}
				return edge.WindCnt2 <= 0;
			default:
				return true;
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005B70 File Offset: 0x00003D70
		private void SetWindingCount(TEdge edge)
		{
			TEdge tedge = edge.PrevInAEL;
			while (tedge != null && (tedge.PolyTyp != edge.PolyTyp || tedge.WindDelta == 0))
			{
				tedge = tedge.PrevInAEL;
			}
			if (tedge == null)
			{
				PolyFillType polyFillType = ((edge.PolyTyp == PolyType.ptSubject) ? this.m_SubjFillType : this.m_ClipFillType);
				if (edge.WindDelta == 0)
				{
					edge.WindCnt = ((polyFillType == PolyFillType.pftNegative) ? (-1) : 1);
				}
				else
				{
					edge.WindCnt = edge.WindDelta;
				}
				edge.WindCnt2 = 0;
				tedge = this.m_ActiveEdges;
			}
			else if (edge.WindDelta == 0 && this.m_ClipType != ClipType.ctUnion)
			{
				edge.WindCnt = 1;
				edge.WindCnt2 = tedge.WindCnt2;
				tedge = tedge.NextInAEL;
			}
			else if (this.IsEvenOddFillType(edge))
			{
				if (edge.WindDelta == 0)
				{
					bool flag = true;
					for (TEdge tedge2 = tedge.PrevInAEL; tedge2 != null; tedge2 = tedge2.PrevInAEL)
					{
						if (tedge2.PolyTyp == tedge.PolyTyp && tedge2.WindDelta != 0)
						{
							flag = !flag;
						}
					}
					edge.WindCnt = (flag ? 0 : 1);
				}
				else
				{
					edge.WindCnt = edge.WindDelta;
				}
				edge.WindCnt2 = tedge.WindCnt2;
				tedge = tedge.NextInAEL;
			}
			else
			{
				if (tedge.WindCnt * tedge.WindDelta < 0)
				{
					if (Math.Abs(tedge.WindCnt) > 1)
					{
						if (tedge.WindDelta * edge.WindDelta < 0)
						{
							edge.WindCnt = tedge.WindCnt;
						}
						else
						{
							edge.WindCnt = tedge.WindCnt + edge.WindDelta;
						}
					}
					else
					{
						edge.WindCnt = ((edge.WindDelta == 0) ? 1 : edge.WindDelta);
					}
				}
				else if (edge.WindDelta == 0)
				{
					edge.WindCnt = ((tedge.WindCnt < 0) ? (tedge.WindCnt - 1) : (tedge.WindCnt + 1));
				}
				else if (tedge.WindDelta * edge.WindDelta < 0)
				{
					edge.WindCnt = tedge.WindCnt;
				}
				else
				{
					edge.WindCnt = tedge.WindCnt + edge.WindDelta;
				}
				edge.WindCnt2 = tedge.WindCnt2;
				tedge = tedge.NextInAEL;
			}
			if (this.IsEvenOddAltFillType(edge))
			{
				while (tedge != edge)
				{
					if (tedge.WindDelta != 0)
					{
						edge.WindCnt2 = ((edge.WindCnt2 == 0) ? 1 : 0);
					}
					tedge = tedge.NextInAEL;
				}
				return;
			}
			while (tedge != edge)
			{
				edge.WindCnt2 += tedge.WindDelta;
				tedge = tedge.NextInAEL;
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005DC8 File Offset: 0x00003FC8
		private void AddEdgeToSEL(TEdge edge)
		{
			if (this.m_SortedEdges == null)
			{
				this.m_SortedEdges = edge;
				edge.PrevInSEL = null;
				edge.NextInSEL = null;
				return;
			}
			edge.NextInSEL = this.m_SortedEdges;
			edge.PrevInSEL = null;
			this.m_SortedEdges.PrevInSEL = edge;
			this.m_SortedEdges = edge;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005E1C File Offset: 0x0000401C
		internal bool PopEdgeFromSEL(out TEdge e)
		{
			e = this.m_SortedEdges;
			if (e == null)
			{
				return false;
			}
			TEdge tedge = e;
			this.m_SortedEdges = e.NextInSEL;
			if (this.m_SortedEdges != null)
			{
				this.m_SortedEdges.PrevInSEL = null;
			}
			tedge.NextInSEL = null;
			tedge.PrevInSEL = null;
			return true;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005E68 File Offset: 0x00004068
		private void CopyAELToSEL()
		{
			TEdge tedge = this.m_ActiveEdges;
			this.m_SortedEdges = tedge;
			while (tedge != null)
			{
				tedge.PrevInSEL = tedge.PrevInAEL;
				tedge.NextInSEL = tedge.NextInAEL;
				tedge = tedge.NextInAEL;
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005EA8 File Offset: 0x000040A8
		private void SwapPositionsInSEL(TEdge edge1, TEdge edge2)
		{
			if (edge1.NextInSEL == null && edge1.PrevInSEL == null)
			{
				return;
			}
			if (edge2.NextInSEL == null && edge2.PrevInSEL == null)
			{
				return;
			}
			if (edge1.NextInSEL == edge2)
			{
				TEdge nextInSEL = edge2.NextInSEL;
				if (nextInSEL != null)
				{
					nextInSEL.PrevInSEL = edge1;
				}
				TEdge prevInSEL = edge1.PrevInSEL;
				if (prevInSEL != null)
				{
					prevInSEL.NextInSEL = edge2;
				}
				edge2.PrevInSEL = prevInSEL;
				edge2.NextInSEL = edge1;
				edge1.PrevInSEL = edge2;
				edge1.NextInSEL = nextInSEL;
			}
			else if (edge2.NextInSEL == edge1)
			{
				TEdge nextInSEL2 = edge1.NextInSEL;
				if (nextInSEL2 != null)
				{
					nextInSEL2.PrevInSEL = edge2;
				}
				TEdge prevInSEL2 = edge2.PrevInSEL;
				if (prevInSEL2 != null)
				{
					prevInSEL2.NextInSEL = edge1;
				}
				edge1.PrevInSEL = prevInSEL2;
				edge1.NextInSEL = edge2;
				edge2.PrevInSEL = edge1;
				edge2.NextInSEL = nextInSEL2;
			}
			else
			{
				TEdge nextInSEL3 = edge1.NextInSEL;
				TEdge prevInSEL3 = edge1.PrevInSEL;
				edge1.NextInSEL = edge2.NextInSEL;
				if (edge1.NextInSEL != null)
				{
					edge1.NextInSEL.PrevInSEL = edge1;
				}
				edge1.PrevInSEL = edge2.PrevInSEL;
				if (edge1.PrevInSEL != null)
				{
					edge1.PrevInSEL.NextInSEL = edge1;
				}
				edge2.NextInSEL = nextInSEL3;
				if (edge2.NextInSEL != null)
				{
					edge2.NextInSEL.PrevInSEL = edge2;
				}
				edge2.PrevInSEL = prevInSEL3;
				if (edge2.PrevInSEL != null)
				{
					edge2.PrevInSEL.NextInSEL = edge2;
				}
			}
			if (edge1.PrevInSEL == null)
			{
				this.m_SortedEdges = edge1;
				return;
			}
			if (edge2.PrevInSEL == null)
			{
				this.m_SortedEdges = edge2;
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00006018 File Offset: 0x00004218
		private void AddLocalMaxPoly(TEdge e1, TEdge e2, IntPoint pt)
		{
			this.AddOutPt(e1, pt);
			if (e2.WindDelta == 0)
			{
				this.AddOutPt(e2, pt);
			}
			if (e1.OutIdx == e2.OutIdx)
			{
				e1.OutIdx = -1;
				e2.OutIdx = -1;
				return;
			}
			if (e1.OutIdx < e2.OutIdx)
			{
				this.AppendPolygon(e1, e2);
				return;
			}
			this.AppendPolygon(e2, e1);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000607C File Offset: 0x0000427C
		private OutPt AddLocalMinPoly(TEdge e1, TEdge e2, IntPoint pt)
		{
			OutPt outPt;
			TEdge tedge;
			TEdge tedge2;
			if (ClipperBase.IsHorizontal(e2) || e1.Dx > e2.Dx)
			{
				outPt = this.AddOutPt(e1, pt);
				e2.OutIdx = e1.OutIdx;
				e1.Side = EdgeSide.esLeft;
				e2.Side = EdgeSide.esRight;
				tedge = e1;
				if (tedge.PrevInAEL == e2)
				{
					tedge2 = e2.PrevInAEL;
				}
				else
				{
					tedge2 = tedge.PrevInAEL;
				}
			}
			else
			{
				outPt = this.AddOutPt(e2, pt);
				e1.OutIdx = e2.OutIdx;
				e1.Side = EdgeSide.esRight;
				e2.Side = EdgeSide.esLeft;
				tedge = e2;
				if (tedge.PrevInAEL == e1)
				{
					tedge2 = e1.PrevInAEL;
				}
				else
				{
					tedge2 = tedge.PrevInAEL;
				}
			}
			if (tedge2 != null && tedge2.OutIdx >= 0 && tedge2.Top.Y < pt.Y && tedge.Top.Y < pt.Y)
			{
				long num = Clipper.TopX(tedge2, pt.Y);
				long num2 = Clipper.TopX(tedge, pt.Y);
				if (num == num2 && tedge.WindDelta != 0 && tedge2.WindDelta != 0 && ClipperBase.SlopesEqual(new IntPoint(num, pt.Y), tedge2.Top, new IntPoint(num2, pt.Y), tedge.Top, this.m_UseFullRange))
				{
					OutPt outPt2 = this.AddOutPt(tedge2, pt);
					this.AddJoin(outPt, outPt2, tedge.Top);
				}
			}
			return outPt;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000061D4 File Offset: 0x000043D4
		private OutPt AddOutPt(TEdge e, IntPoint pt)
		{
			if (e.OutIdx < 0)
			{
				OutRec outRec = base.CreateOutRec();
				outRec.IsOpen = e.WindDelta == 0;
				OutPt outPt = new OutPt();
				outRec.Pts = outPt;
				outPt.Idx = outRec.Idx;
				outPt.Pt = pt;
				outPt.Next = outPt;
				outPt.Prev = outPt;
				if (!outRec.IsOpen)
				{
					this.SetHoleState(e, outRec);
				}
				e.OutIdx = outRec.Idx;
				return outPt;
			}
			OutRec outRec2 = this.m_PolyOuts[e.OutIdx];
			OutPt pts = outRec2.Pts;
			bool flag = e.Side == EdgeSide.esLeft;
			if (flag && pt == pts.Pt)
			{
				return pts;
			}
			if (!flag && pt == pts.Prev.Pt)
			{
				return pts.Prev;
			}
			OutPt outPt2 = new OutPt();
			outPt2.Idx = outRec2.Idx;
			outPt2.Pt = pt;
			outPt2.Next = pts;
			outPt2.Prev = pts.Prev;
			outPt2.Prev.Next = outPt2;
			pts.Prev = outPt2;
			if (flag)
			{
				outRec2.Pts = outPt2;
			}
			return outPt2;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000062F8 File Offset: 0x000044F8
		private OutPt GetLastOutPt(TEdge e)
		{
			OutRec outRec = this.m_PolyOuts[e.OutIdx];
			if (e.Side == EdgeSide.esLeft)
			{
				return outRec.Pts;
			}
			return outRec.Pts.Prev;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00006334 File Offset: 0x00004534
		internal void SwapPoints(ref IntPoint pt1, ref IntPoint pt2)
		{
			IntPoint intPoint = new IntPoint(pt1);
			pt1 = pt2;
			pt2 = intPoint;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006361 File Offset: 0x00004561
		private bool HorzSegmentsOverlap(long seg1a, long seg1b, long seg2a, long seg2b)
		{
			if (seg1a > seg1b)
			{
				base.Swap(ref seg1a, ref seg1b);
			}
			if (seg2a > seg2b)
			{
				base.Swap(ref seg2a, ref seg2b);
			}
			return seg1a < seg2b && seg2a < seg1b;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000638C File Offset: 0x0000458C
		private void SetHoleState(TEdge e, OutRec outRec)
		{
			TEdge tedge = e.PrevInAEL;
			TEdge tedge2 = null;
			while (tedge != null)
			{
				if (tedge.OutIdx >= 0 && tedge.WindDelta != 0)
				{
					if (tedge2 == null)
					{
						tedge2 = tedge;
					}
					else if (tedge2.OutIdx == tedge.OutIdx)
					{
						tedge2 = null;
					}
				}
				tedge = tedge.PrevInAEL;
			}
			if (tedge2 == null)
			{
				outRec.FirstLeft = null;
				outRec.IsHole = false;
				return;
			}
			outRec.FirstLeft = this.m_PolyOuts[tedge2.OutIdx];
			outRec.IsHole = !outRec.FirstLeft.IsHole;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006413 File Offset: 0x00004613
		private double GetDx(IntPoint pt1, IntPoint pt2)
		{
			if (pt1.Y == pt2.Y)
			{
				return -3.4E+38;
			}
			return (double)(pt2.X - pt1.X) / (double)(pt2.Y - pt1.Y);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000644C File Offset: 0x0000464C
		private bool FirstIsBottomPt(OutPt btmPt1, OutPt btmPt2)
		{
			OutPt outPt = btmPt1.Prev;
			while (outPt.Pt == btmPt1.Pt && outPt != btmPt1)
			{
				outPt = outPt.Prev;
			}
			double num = Math.Abs(this.GetDx(btmPt1.Pt, outPt.Pt));
			outPt = btmPt1.Next;
			while (outPt.Pt == btmPt1.Pt && outPt != btmPt1)
			{
				outPt = outPt.Next;
			}
			double num2 = Math.Abs(this.GetDx(btmPt1.Pt, outPt.Pt));
			outPt = btmPt2.Prev;
			while (outPt.Pt == btmPt2.Pt && outPt != btmPt2)
			{
				outPt = outPt.Prev;
			}
			double num3 = Math.Abs(this.GetDx(btmPt2.Pt, outPt.Pt));
			outPt = btmPt2.Next;
			while (outPt.Pt == btmPt2.Pt && outPt != btmPt2)
			{
				outPt = outPt.Next;
			}
			double num4 = Math.Abs(this.GetDx(btmPt2.Pt, outPt.Pt));
			if (Math.Max(num, num2) == Math.Max(num3, num4) && Math.Min(num, num2) == Math.Min(num3, num4))
			{
				return this.Area(btmPt1) > 0.0;
			}
			return (num >= num3 && num >= num4) || (num2 >= num3 && num2 >= num4);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000065A4 File Offset: 0x000047A4
		private OutPt GetBottomPt(OutPt pp)
		{
			OutPt outPt = null;
			OutPt outPt2;
			for (outPt2 = pp.Next; outPt2 != pp; outPt2 = outPt2.Next)
			{
				if (outPt2.Pt.Y > pp.Pt.Y)
				{
					pp = outPt2;
					outPt = null;
				}
				else if (outPt2.Pt.Y == pp.Pt.Y && outPt2.Pt.X <= pp.Pt.X)
				{
					if (outPt2.Pt.X < pp.Pt.X)
					{
						outPt = null;
						pp = outPt2;
					}
					else if (outPt2.Next != pp && outPt2.Prev != pp)
					{
						outPt = outPt2;
					}
				}
			}
			if (outPt != null)
			{
				while (outPt != outPt2)
				{
					if (!this.FirstIsBottomPt(outPt2, outPt))
					{
						pp = outPt;
					}
					outPt = outPt.Next;
					while (outPt.Pt != pp.Pt)
					{
						outPt = outPt.Next;
					}
				}
			}
			return pp;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000668C File Offset: 0x0000488C
		private OutRec GetLowermostRec(OutRec outRec1, OutRec outRec2)
		{
			if (outRec1.BottomPt == null)
			{
				outRec1.BottomPt = this.GetBottomPt(outRec1.Pts);
			}
			if (outRec2.BottomPt == null)
			{
				outRec2.BottomPt = this.GetBottomPt(outRec2.Pts);
			}
			OutPt bottomPt = outRec1.BottomPt;
			OutPt bottomPt2 = outRec2.BottomPt;
			if (bottomPt.Pt.Y > bottomPt2.Pt.Y)
			{
				return outRec1;
			}
			if (bottomPt.Pt.Y < bottomPt2.Pt.Y)
			{
				return outRec2;
			}
			if (bottomPt.Pt.X < bottomPt2.Pt.X)
			{
				return outRec1;
			}
			if (bottomPt.Pt.X > bottomPt2.Pt.X)
			{
				return outRec2;
			}
			if (bottomPt.Next == bottomPt)
			{
				return outRec2;
			}
			if (bottomPt2.Next == bottomPt2)
			{
				return outRec1;
			}
			if (this.FirstIsBottomPt(bottomPt, bottomPt2))
			{
				return outRec1;
			}
			return outRec2;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00006766 File Offset: 0x00004966
		private bool OutRec1RightOfOutRec2(OutRec outRec1, OutRec outRec2)
		{
			for (;;)
			{
				outRec1 = outRec1.FirstLeft;
				if (outRec1 == outRec2)
				{
					break;
				}
				if (outRec1 == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000677C File Offset: 0x0000497C
		private OutRec GetOutRec(int idx)
		{
			OutRec outRec;
			for (outRec = this.m_PolyOuts[idx]; outRec != this.m_PolyOuts[outRec.Idx]; outRec = this.m_PolyOuts[outRec.Idx])
			{
			}
			return outRec;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000067C0 File Offset: 0x000049C0
		private void AppendPolygon(TEdge e1, TEdge e2)
		{
			OutRec outRec = this.m_PolyOuts[e1.OutIdx];
			OutRec outRec2 = this.m_PolyOuts[e2.OutIdx];
			OutRec outRec3;
			if (this.OutRec1RightOfOutRec2(outRec, outRec2))
			{
				outRec3 = outRec2;
			}
			else if (this.OutRec1RightOfOutRec2(outRec2, outRec))
			{
				outRec3 = outRec;
			}
			else
			{
				outRec3 = this.GetLowermostRec(outRec, outRec2);
			}
			OutPt pts = outRec.Pts;
			OutPt prev = pts.Prev;
			OutPt pts2 = outRec2.Pts;
			OutPt prev2 = pts2.Prev;
			if (e1.Side == EdgeSide.esLeft)
			{
				if (e2.Side == EdgeSide.esLeft)
				{
					this.ReversePolyPtLinks(pts2);
					pts2.Next = pts;
					pts.Prev = pts2;
					prev.Next = prev2;
					prev2.Prev = prev;
					outRec.Pts = prev2;
				}
				else
				{
					prev2.Next = pts;
					pts.Prev = prev2;
					pts2.Prev = prev;
					prev.Next = pts2;
					outRec.Pts = pts2;
				}
			}
			else if (e2.Side == EdgeSide.esRight)
			{
				this.ReversePolyPtLinks(pts2);
				prev.Next = prev2;
				prev2.Prev = prev;
				pts2.Next = pts;
				pts.Prev = pts2;
			}
			else
			{
				prev.Next = pts2;
				pts2.Prev = prev;
				pts.Prev = prev2;
				prev2.Next = pts;
			}
			outRec.BottomPt = null;
			if (outRec3 == outRec2)
			{
				if (outRec2.FirstLeft != outRec)
				{
					outRec.FirstLeft = outRec2.FirstLeft;
				}
				outRec.IsHole = outRec2.IsHole;
			}
			outRec2.Pts = null;
			outRec2.BottomPt = null;
			outRec2.FirstLeft = outRec;
			int outIdx = e1.OutIdx;
			int outIdx2 = e2.OutIdx;
			e1.OutIdx = -1;
			e2.OutIdx = -1;
			for (TEdge tedge = this.m_ActiveEdges; tedge != null; tedge = tedge.NextInAEL)
			{
				if (tedge.OutIdx == outIdx2)
				{
					tedge.OutIdx = outIdx;
					tedge.Side = e1.Side;
					break;
				}
			}
			outRec2.Idx = outRec.Idx;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000069A8 File Offset: 0x00004BA8
		private void ReversePolyPtLinks(OutPt pp)
		{
			if (pp == null)
			{
				return;
			}
			OutPt outPt = pp;
			do
			{
				OutPt next = outPt.Next;
				outPt.Next = outPt.Prev;
				outPt.Prev = next;
				outPt = next;
			}
			while (outPt != pp);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000069DC File Offset: 0x00004BDC
		private static void SwapSides(TEdge edge1, TEdge edge2)
		{
			EdgeSide side = edge1.Side;
			edge1.Side = edge2.Side;
			edge2.Side = side;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006A04 File Offset: 0x00004C04
		private static void SwapPolyIndexes(TEdge edge1, TEdge edge2)
		{
			int outIdx = edge1.OutIdx;
			edge1.OutIdx = edge2.OutIdx;
			edge2.OutIdx = outIdx;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00006A2C File Offset: 0x00004C2C
		private void IntersectEdges(TEdge e1, TEdge e2, IntPoint pt)
		{
			bool flag = e1.OutIdx >= 0;
			bool flag2 = e2.OutIdx >= 0;
			if (e1.WindDelta == 0 || e2.WindDelta == 0)
			{
				if (e1.WindDelta == 0 && e2.WindDelta == 0)
				{
					return;
				}
				if (e1.PolyTyp == e2.PolyTyp && e1.WindDelta != e2.WindDelta && this.m_ClipType == ClipType.ctUnion)
				{
					if (e1.WindDelta == 0)
					{
						if (flag2)
						{
							this.AddOutPt(e1, pt);
							if (flag)
							{
								e1.OutIdx = -1;
								return;
							}
						}
					}
					else if (flag)
					{
						this.AddOutPt(e2, pt);
						if (flag2)
						{
							e2.OutIdx = -1;
							return;
						}
					}
				}
				else if (e1.PolyTyp != e2.PolyTyp)
				{
					if (e1.WindDelta == 0 && Math.Abs(e2.WindCnt) == 1 && (this.m_ClipType != ClipType.ctUnion || e2.WindCnt2 == 0))
					{
						this.AddOutPt(e1, pt);
						if (flag)
						{
							e1.OutIdx = -1;
							return;
						}
					}
					else if (e2.WindDelta == 0 && Math.Abs(e1.WindCnt) == 1 && (this.m_ClipType != ClipType.ctUnion || e1.WindCnt2 == 0))
					{
						this.AddOutPt(e2, pt);
						if (flag2)
						{
							e2.OutIdx = -1;
						}
					}
				}
				return;
			}
			else
			{
				if (e1.PolyTyp == e2.PolyTyp)
				{
					if (this.IsEvenOddFillType(e1))
					{
						int windCnt = e1.WindCnt;
						e1.WindCnt = e2.WindCnt;
						e2.WindCnt = windCnt;
					}
					else
					{
						if (e1.WindCnt + e2.WindDelta == 0)
						{
							e1.WindCnt = -e1.WindCnt;
						}
						else
						{
							e1.WindCnt += e2.WindDelta;
						}
						if (e2.WindCnt - e1.WindDelta == 0)
						{
							e2.WindCnt = -e2.WindCnt;
						}
						else
						{
							e2.WindCnt -= e1.WindDelta;
						}
					}
				}
				else
				{
					if (!this.IsEvenOddFillType(e2))
					{
						e1.WindCnt2 += e2.WindDelta;
					}
					else
					{
						e1.WindCnt2 = ((e1.WindCnt2 == 0) ? 1 : 0);
					}
					if (!this.IsEvenOddFillType(e1))
					{
						e2.WindCnt2 -= e1.WindDelta;
					}
					else
					{
						e2.WindCnt2 = ((e2.WindCnt2 == 0) ? 1 : 0);
					}
				}
				PolyFillType polyFillType;
				PolyFillType polyFillType2;
				if (e1.PolyTyp == PolyType.ptSubject)
				{
					polyFillType = this.m_SubjFillType;
					polyFillType2 = this.m_ClipFillType;
				}
				else
				{
					polyFillType = this.m_ClipFillType;
					polyFillType2 = this.m_SubjFillType;
				}
				PolyFillType polyFillType3;
				PolyFillType polyFillType4;
				if (e2.PolyTyp == PolyType.ptSubject)
				{
					polyFillType3 = this.m_SubjFillType;
					polyFillType4 = this.m_ClipFillType;
				}
				else
				{
					polyFillType3 = this.m_ClipFillType;
					polyFillType4 = this.m_SubjFillType;
				}
				int num;
				if (polyFillType != PolyFillType.pftPositive)
				{
					if (polyFillType != PolyFillType.pftNegative)
					{
						num = Math.Abs(e1.WindCnt);
					}
					else
					{
						num = -e1.WindCnt;
					}
				}
				else
				{
					num = e1.WindCnt;
				}
				int num2;
				if (polyFillType3 != PolyFillType.pftPositive)
				{
					if (polyFillType3 != PolyFillType.pftNegative)
					{
						num2 = Math.Abs(e2.WindCnt);
					}
					else
					{
						num2 = -e2.WindCnt;
					}
				}
				else
				{
					num2 = e2.WindCnt;
				}
				if (!flag || !flag2)
				{
					if (flag)
					{
						if (num2 == 0 || num2 == 1)
						{
							this.AddOutPt(e1, pt);
							Clipper.SwapSides(e1, e2);
							Clipper.SwapPolyIndexes(e1, e2);
							return;
						}
					}
					else if (flag2)
					{
						if (num == 0 || num == 1)
						{
							this.AddOutPt(e2, pt);
							Clipper.SwapSides(e1, e2);
							Clipper.SwapPolyIndexes(e1, e2);
							return;
						}
					}
					else if ((num == 0 || num == 1) && (num2 == 0 || num2 == 1))
					{
						long num3;
						if (polyFillType2 != PolyFillType.pftPositive)
						{
							if (polyFillType2 != PolyFillType.pftNegative)
							{
								num3 = (long)Math.Abs(e1.WindCnt2);
							}
							else
							{
								num3 = (long)(-(long)e1.WindCnt2);
							}
						}
						else
						{
							num3 = (long)e1.WindCnt2;
						}
						long num4;
						if (polyFillType4 != PolyFillType.pftPositive)
						{
							if (polyFillType4 != PolyFillType.pftNegative)
							{
								num4 = (long)Math.Abs(e2.WindCnt2);
							}
							else
							{
								num4 = (long)(-(long)e2.WindCnt2);
							}
						}
						else
						{
							num4 = (long)e2.WindCnt2;
						}
						if (e1.PolyTyp != e2.PolyTyp)
						{
							this.AddLocalMinPoly(e1, e2, pt);
							return;
						}
						if (num == 1 && num2 == 1)
						{
							switch (this.m_ClipType)
							{
							case ClipType.ctIntersection:
								if (num3 > 0L && num4 > 0L)
								{
									this.AddLocalMinPoly(e1, e2, pt);
									return;
								}
								break;
							case ClipType.ctUnion:
								if (num3 <= 0L && num4 <= 0L)
								{
									this.AddLocalMinPoly(e1, e2, pt);
									return;
								}
								break;
							case ClipType.ctDifference:
								if ((e1.PolyTyp == PolyType.ptClip && num3 > 0L && num4 > 0L) || (e1.PolyTyp == PolyType.ptSubject && num3 <= 0L && num4 <= 0L))
								{
									this.AddLocalMinPoly(e1, e2, pt);
									return;
								}
								break;
							case ClipType.ctXor:
								this.AddLocalMinPoly(e1, e2, pt);
								return;
							default:
								return;
							}
						}
						else
						{
							Clipper.SwapSides(e1, e2);
						}
					}
					return;
				}
				if ((num != 0 && num != 1) || (num2 != 0 && num2 != 1) || (e1.PolyTyp != e2.PolyTyp && this.m_ClipType != ClipType.ctXor))
				{
					this.AddLocalMaxPoly(e1, e2, pt);
					return;
				}
				this.AddOutPt(e1, pt);
				this.AddOutPt(e2, pt);
				Clipper.SwapSides(e1, e2);
				Clipper.SwapPolyIndexes(e1, e2);
				return;
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00006EE8 File Offset: 0x000050E8
		private void DeleteFromSEL(TEdge e)
		{
			TEdge prevInSEL = e.PrevInSEL;
			TEdge nextInSEL = e.NextInSEL;
			if (prevInSEL == null && nextInSEL == null && e != this.m_SortedEdges)
			{
				return;
			}
			if (prevInSEL != null)
			{
				prevInSEL.NextInSEL = nextInSEL;
			}
			else
			{
				this.m_SortedEdges = nextInSEL;
			}
			if (nextInSEL != null)
			{
				nextInSEL.PrevInSEL = prevInSEL;
			}
			e.NextInSEL = null;
			e.PrevInSEL = null;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006F40 File Offset: 0x00005140
		private void ProcessHorizontals()
		{
			TEdge tedge;
			while (this.PopEdgeFromSEL(out tedge))
			{
				this.ProcessHorizontal(tedge);
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00006F60 File Offset: 0x00005160
		private void GetHorzDirection(TEdge HorzEdge, out Direction Dir, out long Left, out long Right)
		{
			if (HorzEdge.Bot.X < HorzEdge.Top.X)
			{
				Left = HorzEdge.Bot.X;
				Right = HorzEdge.Top.X;
				Dir = Direction.dLeftToRight;
				return;
			}
			Left = HorzEdge.Top.X;
			Right = HorzEdge.Bot.X;
			Dir = Direction.dRightToLeft;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00006FC4 File Offset: 0x000051C4
		private void ProcessHorizontal(TEdge horzEdge)
		{
			bool flag = horzEdge.WindDelta == 0;
			Direction direction;
			long num;
			long num2;
			this.GetHorzDirection(horzEdge, out direction, out num, out num2);
			TEdge tedge = horzEdge;
			TEdge tedge2 = null;
			while (tedge.NextInLML != null && ClipperBase.IsHorizontal(tedge.NextInLML))
			{
				tedge = tedge.NextInLML;
			}
			if (tedge.NextInLML == null)
			{
				tedge2 = this.GetMaximaPair(tedge);
			}
			Maxima maxima = this.m_Maxima;
			if (maxima != null)
			{
				if (direction == Direction.dLeftToRight)
				{
					while (maxima != null && maxima.X <= horzEdge.Bot.X)
					{
						maxima = maxima.Next;
					}
					if (maxima != null && maxima.X >= tedge.Top.X)
					{
						maxima = null;
					}
				}
				else
				{
					while (maxima.Next != null && maxima.Next.X < horzEdge.Bot.X)
					{
						maxima = maxima.Next;
					}
					if (maxima.X <= tedge.Top.X)
					{
						maxima = null;
					}
				}
			}
			OutPt outPt = null;
			for (;;)
			{
				bool flag2 = horzEdge == tedge;
				TEdge nextInAEL;
				for (TEdge tedge3 = this.GetNextInAEL(horzEdge, direction); tedge3 != null; tedge3 = nextInAEL)
				{
					if (maxima != null)
					{
						if (direction == Direction.dLeftToRight)
						{
							while (maxima != null)
							{
								if (maxima.X >= tedge3.Curr.X)
								{
									break;
								}
								if (horzEdge.OutIdx >= 0 && !flag)
								{
									this.AddOutPt(horzEdge, new IntPoint(maxima.X, horzEdge.Bot.Y));
								}
								maxima = maxima.Next;
							}
						}
						else
						{
							while (maxima != null && maxima.X > tedge3.Curr.X)
							{
								if (horzEdge.OutIdx >= 0 && !flag)
								{
									this.AddOutPt(horzEdge, new IntPoint(maxima.X, horzEdge.Bot.Y));
								}
								maxima = maxima.Prev;
							}
						}
					}
					if ((direction == Direction.dLeftToRight && tedge3.Curr.X > num2) || (direction == Direction.dRightToLeft && tedge3.Curr.X < num) || (tedge3.Curr.X == horzEdge.Top.X && horzEdge.NextInLML != null && tedge3.Dx < horzEdge.NextInLML.Dx))
					{
						break;
					}
					if (horzEdge.OutIdx >= 0 && !flag)
					{
						outPt = this.AddOutPt(horzEdge, tedge3.Curr);
						for (TEdge tedge4 = this.m_SortedEdges; tedge4 != null; tedge4 = tedge4.NextInSEL)
						{
							if (tedge4.OutIdx >= 0 && this.HorzSegmentsOverlap(horzEdge.Bot.X, horzEdge.Top.X, tedge4.Bot.X, tedge4.Top.X))
							{
								OutPt lastOutPt = this.GetLastOutPt(tedge4);
								this.AddJoin(lastOutPt, outPt, tedge4.Top);
							}
						}
						this.AddGhostJoin(outPt, horzEdge.Bot);
					}
					if (tedge3 == tedge2 && flag2)
					{
						goto Block_28;
					}
					if (direction == Direction.dLeftToRight)
					{
						IntPoint intPoint = new IntPoint(tedge3.Curr.X, horzEdge.Curr.Y);
						this.IntersectEdges(horzEdge, tedge3, intPoint);
					}
					else
					{
						IntPoint intPoint2 = new IntPoint(tedge3.Curr.X, horzEdge.Curr.Y);
						this.IntersectEdges(tedge3, horzEdge, intPoint2);
					}
					nextInAEL = this.GetNextInAEL(tedge3, direction);
					base.SwapPositionsInAEL(horzEdge, tedge3);
				}
				if (horzEdge.NextInLML == null || !ClipperBase.IsHorizontal(horzEdge.NextInLML))
				{
					goto IL_039F;
				}
				base.UpdateEdgeIntoAEL(ref horzEdge);
				if (horzEdge.OutIdx >= 0)
				{
					this.AddOutPt(horzEdge, horzEdge.Bot);
				}
				this.GetHorzDirection(horzEdge, out direction, out num, out num2);
			}
			Block_28:
			if (horzEdge.OutIdx >= 0)
			{
				this.AddLocalMaxPoly(horzEdge, tedge2, horzEdge.Top);
			}
			base.DeleteFromAEL(horzEdge);
			base.DeleteFromAEL(tedge2);
			return;
			IL_039F:
			if (horzEdge.OutIdx >= 0 && outPt == null)
			{
				outPt = this.GetLastOutPt(horzEdge);
				for (TEdge tedge5 = this.m_SortedEdges; tedge5 != null; tedge5 = tedge5.NextInSEL)
				{
					if (tedge5.OutIdx >= 0 && this.HorzSegmentsOverlap(horzEdge.Bot.X, horzEdge.Top.X, tedge5.Bot.X, tedge5.Top.X))
					{
						OutPt lastOutPt2 = this.GetLastOutPt(tedge5);
						this.AddJoin(lastOutPt2, outPt, tedge5.Top);
					}
				}
				this.AddGhostJoin(outPt, horzEdge.Top);
			}
			if (horzEdge.NextInLML != null)
			{
				if (horzEdge.OutIdx < 0)
				{
					base.UpdateEdgeIntoAEL(ref horzEdge);
					return;
				}
				outPt = this.AddOutPt(horzEdge, horzEdge.Top);
				base.UpdateEdgeIntoAEL(ref horzEdge);
				if (horzEdge.WindDelta == 0)
				{
					return;
				}
				TEdge prevInAEL = horzEdge.PrevInAEL;
				TEdge nextInAEL2 = horzEdge.NextInAEL;
				if (prevInAEL != null && prevInAEL.Curr.X == horzEdge.Bot.X && prevInAEL.Curr.Y == horzEdge.Bot.Y && prevInAEL.WindDelta != 0 && prevInAEL.OutIdx >= 0 && prevInAEL.Curr.Y > prevInAEL.Top.Y && ClipperBase.SlopesEqual(horzEdge, prevInAEL, this.m_UseFullRange))
				{
					OutPt outPt2 = this.AddOutPt(prevInAEL, horzEdge.Bot);
					this.AddJoin(outPt, outPt2, horzEdge.Top);
					return;
				}
				if (nextInAEL2 != null && nextInAEL2.Curr.X == horzEdge.Bot.X && nextInAEL2.Curr.Y == horzEdge.Bot.Y && nextInAEL2.WindDelta != 0 && nextInAEL2.OutIdx >= 0 && nextInAEL2.Curr.Y > nextInAEL2.Top.Y && ClipperBase.SlopesEqual(horzEdge, nextInAEL2, this.m_UseFullRange))
				{
					OutPt outPt3 = this.AddOutPt(nextInAEL2, horzEdge.Bot);
					this.AddJoin(outPt, outPt3, horzEdge.Top);
					return;
				}
			}
			else
			{
				if (horzEdge.OutIdx >= 0)
				{
					this.AddOutPt(horzEdge, horzEdge.Top);
				}
				base.DeleteFromAEL(horzEdge);
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000075AE File Offset: 0x000057AE
		private TEdge GetNextInAEL(TEdge e, Direction Direction)
		{
			if (Direction != Direction.dLeftToRight)
			{
				return e.PrevInAEL;
			}
			return e.NextInAEL;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000075C1 File Offset: 0x000057C1
		private bool IsMinima(TEdge e)
		{
			return e != null && e.Prev.NextInLML != e && e.Next.NextInLML != e;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000075E7 File Offset: 0x000057E7
		private bool IsMaxima(TEdge e, double Y)
		{
			return e != null && (double)e.Top.Y == Y && e.NextInLML == null;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00007606 File Offset: 0x00005806
		private bool IsIntermediate(TEdge e, double Y)
		{
			return (double)e.Top.Y == Y && e.NextInLML != null;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00007624 File Offset: 0x00005824
		internal TEdge GetMaximaPair(TEdge e)
		{
			if (e.Next.Top == e.Top && e.Next.NextInLML == null)
			{
				return e.Next;
			}
			if (e.Prev.Top == e.Top && e.Prev.NextInLML == null)
			{
				return e.Prev;
			}
			return null;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000768C File Offset: 0x0000588C
		internal TEdge GetMaximaPairEx(TEdge e)
		{
			TEdge maximaPair = this.GetMaximaPair(e);
			if (maximaPair == null || maximaPair.OutIdx == -2 || (maximaPair.NextInAEL == maximaPair.PrevInAEL && !ClipperBase.IsHorizontal(maximaPair)))
			{
				return null;
			}
			return maximaPair;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000076C8 File Offset: 0x000058C8
		private bool ProcessIntersections(long topY)
		{
			if (this.m_ActiveEdges == null)
			{
				return true;
			}
			try
			{
				this.BuildIntersectList(topY);
				if (this.m_IntersectList.Count == 0)
				{
					return true;
				}
				if (this.m_IntersectList.Count != 1 && !this.FixupIntersectionOrder())
				{
					return false;
				}
				this.ProcessIntersectList();
			}
			catch
			{
				this.m_SortedEdges = null;
				this.m_IntersectList.Clear();
				throw new ClipperException("ProcessIntersections error");
			}
			this.m_SortedEdges = null;
			return true;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00007754 File Offset: 0x00005954
		private void BuildIntersectList(long topY)
		{
			if (this.m_ActiveEdges == null)
			{
				return;
			}
			TEdge tedge = this.m_ActiveEdges;
			this.m_SortedEdges = tedge;
			while (tedge != null)
			{
				tedge.PrevInSEL = tedge.PrevInAEL;
				tedge.NextInSEL = tedge.NextInAEL;
				tedge.Curr.X = Clipper.TopX(tedge, topY);
				tedge = tedge.NextInAEL;
			}
			bool flag = true;
			while (flag && this.m_SortedEdges != null)
			{
				flag = false;
				tedge = this.m_SortedEdges;
				while (tedge.NextInSEL != null)
				{
					TEdge nextInSEL = tedge.NextInSEL;
					if (tedge.Curr.X > nextInSEL.Curr.X)
					{
						IntPoint intPoint;
						this.IntersectPoint(tedge, nextInSEL, out intPoint);
						if (intPoint.Y < topY)
						{
							intPoint = new IntPoint(Clipper.TopX(tedge, topY), topY);
						}
						IntersectNode intersectNode = new IntersectNode();
						intersectNode.Edge1 = tedge;
						intersectNode.Edge2 = nextInSEL;
						intersectNode.Pt = intPoint;
						this.m_IntersectList.Add(intersectNode);
						this.SwapPositionsInSEL(tedge, nextInSEL);
						flag = true;
					}
					else
					{
						tedge = nextInSEL;
					}
				}
				if (tedge.PrevInSEL == null)
				{
					break;
				}
				tedge.PrevInSEL.NextInSEL = null;
			}
			this.m_SortedEdges = null;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000786F File Offset: 0x00005A6F
		private bool EdgesAdjacent(IntersectNode inode)
		{
			return inode.Edge1.NextInSEL == inode.Edge2 || inode.Edge1.PrevInSEL == inode.Edge2;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00007899 File Offset: 0x00005A99
		private static int IntersectNodeSort(IntersectNode node1, IntersectNode node2)
		{
			return (int)(node2.Pt.Y - node1.Pt.Y);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000078B4 File Offset: 0x00005AB4
		private bool FixupIntersectionOrder()
		{
			this.m_IntersectList.Sort(this.m_IntersectNodeComparer);
			this.CopyAELToSEL();
			int count = this.m_IntersectList.Count;
			for (int i = 0; i < count; i++)
			{
				if (!this.EdgesAdjacent(this.m_IntersectList[i]))
				{
					int num = i + 1;
					while (num < count && !this.EdgesAdjacent(this.m_IntersectList[num]))
					{
						num++;
					}
					if (num == count)
					{
						return false;
					}
					IntersectNode intersectNode = this.m_IntersectList[i];
					this.m_IntersectList[i] = this.m_IntersectList[num];
					this.m_IntersectList[num] = intersectNode;
				}
				this.SwapPositionsInSEL(this.m_IntersectList[i].Edge1, this.m_IntersectList[i].Edge2);
			}
			return true;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00007990 File Offset: 0x00005B90
		private void ProcessIntersectList()
		{
			for (int i = 0; i < this.m_IntersectList.Count; i++)
			{
				IntersectNode intersectNode = this.m_IntersectList[i];
				this.IntersectEdges(intersectNode.Edge1, intersectNode.Edge2, intersectNode.Pt);
				base.SwapPositionsInAEL(intersectNode.Edge1, intersectNode.Edge2);
			}
			this.m_IntersectList.Clear();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000079F5 File Offset: 0x00005BF5
		internal static long Round(double value)
		{
			if (value >= 0.0)
			{
				return (long)(value + 0.5);
			}
			return (long)(value - 0.5);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00007A1C File Offset: 0x00005C1C
		private static long TopX(TEdge edge, long currentY)
		{
			if (currentY == edge.Top.Y)
			{
				return edge.Top.X;
			}
			return edge.Bot.X + Clipper.Round(edge.Dx * (double)(currentY - edge.Bot.Y));
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00007A6C File Offset: 0x00005C6C
		private void IntersectPoint(TEdge edge1, TEdge edge2, out IntPoint ip)
		{
			ip = default(IntPoint);
			long num = -1L;
			bool flag = edge2.Curr.N > 0L && edge2.Curr.N < (long)this.LastIndex && edge1.Curr.N > 0L && edge1.Curr.N < (long)this.LastIndex;
			if (edge1.Curr.N > edge2.Curr.N)
			{
				if (edge2.Curr.N != -1L)
				{
					if (flag)
					{
						num = ((edge1.Curr.N > 0L) ? (edge1.Curr.N - 1L) : 0L);
					}
				}
				else
				{
					num = edge1.Curr.N;
				}
			}
			else if (edge1.Curr.N != -1L)
			{
				if (flag)
				{
					num = edge2.Curr.N;
				}
			}
			else
			{
				num = ((edge2.Curr.N > 0L) ? (edge2.Curr.N - 1L) : 0L);
			}
			ip.D = 2L;
			ip.N = (flag ? num : (-1L));
			if (edge1.Dx == edge2.Dx)
			{
				ip.Y = edge1.Curr.Y;
				ip.X = Clipper.TopX(edge1, ip.Y);
				return;
			}
			if (edge1.Delta.X == 0L)
			{
				ip.X = edge1.Bot.X;
				if (ClipperBase.IsHorizontal(edge2))
				{
					ip.Y = edge2.Bot.Y;
				}
				else
				{
					double num2 = (double)edge2.Bot.Y - (double)edge2.Bot.X / edge2.Dx;
					ip.Y = Clipper.Round((double)ip.X / edge2.Dx + num2);
				}
			}
			else if (edge2.Delta.X == 0L)
			{
				ip.X = edge2.Bot.X;
				if (ClipperBase.IsHorizontal(edge1))
				{
					ip.Y = edge1.Bot.Y;
				}
				else
				{
					double num3 = (double)edge1.Bot.Y - (double)edge1.Bot.X / edge1.Dx;
					ip.Y = Clipper.Round((double)ip.X / edge1.Dx + num3);
				}
			}
			else
			{
				double num3 = (double)edge1.Bot.X - (double)edge1.Bot.Y * edge1.Dx;
				double num2 = (double)edge2.Bot.X - (double)edge2.Bot.Y * edge2.Dx;
				double num4 = (num2 - num3) / (edge1.Dx - edge2.Dx);
				ip.Y = Clipper.Round(num4);
				if (Math.Abs(edge1.Dx) < Math.Abs(edge2.Dx))
				{
					ip.X = Clipper.Round(edge1.Dx * num4 + num3);
				}
				else
				{
					ip.X = Clipper.Round(edge2.Dx * num4 + num2);
				}
			}
			if (ip.Y < edge1.Top.Y || ip.Y < edge2.Top.Y)
			{
				if (edge1.Top.Y > edge2.Top.Y)
				{
					ip.Y = edge1.Top.Y;
				}
				else
				{
					ip.Y = edge2.Top.Y;
				}
				if (Math.Abs(edge1.Dx) < Math.Abs(edge2.Dx))
				{
					ip.X = Clipper.TopX(edge1, ip.Y);
				}
				else
				{
					ip.X = Clipper.TopX(edge2, ip.Y);
				}
			}
			if (ip.Y > edge1.Curr.Y)
			{
				ip.Y = edge1.Curr.Y;
				if (Math.Abs(edge1.Dx) > Math.Abs(edge2.Dx))
				{
					ip.X = Clipper.TopX(edge2, ip.Y);
					return;
				}
				ip.X = Clipper.TopX(edge1, ip.Y);
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00007E5C File Offset: 0x0000605C
		private void ProcessEdgesAtTopOfScanbeam(long topY)
		{
			TEdge tedge = this.m_ActiveEdges;
			while (tedge != null)
			{
				bool flag = this.IsMaxima(tedge, (double)topY);
				if (flag)
				{
					TEdge maximaPairEx = this.GetMaximaPairEx(tedge);
					flag = maximaPairEx == null || !ClipperBase.IsHorizontal(maximaPairEx);
				}
				if (flag)
				{
					if (this.StrictlySimple)
					{
						this.InsertMaxima(tedge.Top.X);
					}
					TEdge prevInAEL = tedge.PrevInAEL;
					this.DoMaxima(tedge);
					if (prevInAEL == null)
					{
						tedge = this.m_ActiveEdges;
					}
					else
					{
						tedge = prevInAEL.NextInAEL;
					}
				}
				else
				{
					if (this.IsIntermediate(tedge, (double)topY) && ClipperBase.IsHorizontal(tedge.NextInLML))
					{
						base.UpdateEdgeIntoAEL(ref tedge);
						if (tedge.OutIdx >= 0)
						{
							this.AddOutPt(tedge, tedge.Bot);
						}
						this.AddEdgeToSEL(tedge);
					}
					else
					{
						tedge.Curr.X = Clipper.TopX(tedge, topY);
						tedge.Curr.Y = topY;
					}
					if (this.StrictlySimple)
					{
						TEdge prevInAEL2 = tedge.PrevInAEL;
						if (tedge.OutIdx >= 0 && tedge.WindDelta != 0 && prevInAEL2 != null && prevInAEL2.OutIdx >= 0 && prevInAEL2.Curr.X == tedge.Curr.X && prevInAEL2.WindDelta != 0)
						{
							IntPoint intPoint = new IntPoint(tedge.Curr);
							OutPt outPt = this.AddOutPt(prevInAEL2, intPoint);
							OutPt outPt2 = this.AddOutPt(tedge, intPoint);
							this.AddJoin(outPt, outPt2, intPoint);
						}
					}
					tedge = tedge.NextInAEL;
				}
			}
			this.ProcessHorizontals();
			this.m_Maxima = null;
			for (tedge = this.m_ActiveEdges; tedge != null; tedge = tedge.NextInAEL)
			{
				if (this.IsIntermediate(tedge, (double)topY))
				{
					OutPt outPt3 = null;
					if (tedge.OutIdx >= 0)
					{
						outPt3 = this.AddOutPt(tedge, tedge.Top);
					}
					base.UpdateEdgeIntoAEL(ref tedge);
					TEdge prevInAEL3 = tedge.PrevInAEL;
					TEdge nextInAEL = tedge.NextInAEL;
					if (prevInAEL3 != null && prevInAEL3.Curr.X == tedge.Bot.X && prevInAEL3.Curr.Y == tedge.Bot.Y && outPt3 != null && prevInAEL3.OutIdx >= 0 && prevInAEL3.Curr.Y > prevInAEL3.Top.Y && ClipperBase.SlopesEqual(tedge.Curr, tedge.Top, prevInAEL3.Curr, prevInAEL3.Top, this.m_UseFullRange) && tedge.WindDelta != 0 && prevInAEL3.WindDelta != 0)
					{
						OutPt outPt4 = this.AddOutPt(prevInAEL3, tedge.Bot);
						this.AddJoin(outPt3, outPt4, tedge.Top);
					}
					else if (nextInAEL != null && nextInAEL.Curr.X == tedge.Bot.X && nextInAEL.Curr.Y == tedge.Bot.Y && outPt3 != null && nextInAEL.OutIdx >= 0 && nextInAEL.Curr.Y > nextInAEL.Top.Y && ClipperBase.SlopesEqual(tedge.Curr, tedge.Top, nextInAEL.Curr, nextInAEL.Top, this.m_UseFullRange) && tedge.WindDelta != 0 && nextInAEL.WindDelta != 0)
					{
						OutPt outPt5 = this.AddOutPt(nextInAEL, tedge.Bot);
						this.AddJoin(outPt3, outPt5, tedge.Top);
					}
				}
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000081B8 File Offset: 0x000063B8
		private void DoMaxima(TEdge e)
		{
			TEdge maximaPairEx = this.GetMaximaPairEx(e);
			if (maximaPairEx == null)
			{
				if (e.OutIdx >= 0)
				{
					this.AddOutPt(e, e.Top);
				}
				base.DeleteFromAEL(e);
				return;
			}
			TEdge tedge = e.NextInAEL;
			while (tedge != null && tedge != maximaPairEx)
			{
				this.IntersectEdges(e, tedge, e.Top);
				base.SwapPositionsInAEL(e, tedge);
				tedge = e.NextInAEL;
			}
			if (e.OutIdx == -1 && maximaPairEx.OutIdx == -1)
			{
				base.DeleteFromAEL(e);
				base.DeleteFromAEL(maximaPairEx);
				return;
			}
			if (e.OutIdx >= 0 && maximaPairEx.OutIdx >= 0)
			{
				if (e.OutIdx >= 0)
				{
					this.AddLocalMaxPoly(e, maximaPairEx, e.Top);
				}
				base.DeleteFromAEL(e);
				base.DeleteFromAEL(maximaPairEx);
				return;
			}
			if (e.WindDelta == 0)
			{
				if (e.OutIdx >= 0)
				{
					this.AddOutPt(e, e.Top);
					e.OutIdx = -1;
				}
				base.DeleteFromAEL(e);
				if (maximaPairEx.OutIdx >= 0)
				{
					this.AddOutPt(maximaPairEx, e.Top);
					maximaPairEx.OutIdx = -1;
				}
				base.DeleteFromAEL(maximaPairEx);
				return;
			}
			throw new ClipperException("DoMaxima error");
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000082D4 File Offset: 0x000064D4
		public static void ReversePaths(List<List<IntPoint>> polys)
		{
			foreach (List<IntPoint> list in polys)
			{
				list.Reverse();
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00008320 File Offset: 0x00006520
		public static bool Orientation(List<IntPoint> poly)
		{
			return Clipper.Area(poly) >= 0.0;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00008338 File Offset: 0x00006538
		private int PointCount(OutPt pts)
		{
			if (pts == null)
			{
				return 0;
			}
			int num = 0;
			OutPt outPt = pts;
			do
			{
				num++;
				outPt = outPt.Next;
			}
			while (outPt != pts);
			return num;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00008360 File Offset: 0x00006560
		private void BuildResult(List<List<IntPoint>> polyg)
		{
			polyg.Clear();
			polyg.Capacity = this.m_PolyOuts.Count;
			for (int i = 0; i < this.m_PolyOuts.Count; i++)
			{
				OutRec outRec = this.m_PolyOuts[i];
				if (outRec.Pts != null)
				{
					OutPt outPt = outRec.Pts.Prev;
					int num = this.PointCount(outPt);
					if (num >= 2)
					{
						List<IntPoint> list = new List<IntPoint>(num);
						for (int j = 0; j < num; j++)
						{
							list.Add(outPt.Pt);
							outPt = outPt.Prev;
						}
						polyg.Add(list);
					}
				}
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000083FC File Offset: 0x000065FC
		private void BuildResult2(PolyTree polytree)
		{
			polytree.Clear();
			polytree.m_AllPolys.Capacity = this.m_PolyOuts.Count;
			for (int i = 0; i < this.m_PolyOuts.Count; i++)
			{
				OutRec outRec = this.m_PolyOuts[i];
				int num = this.PointCount(outRec.Pts);
				if ((!outRec.IsOpen || num >= 2) && (outRec.IsOpen || num >= 3))
				{
					this.FixHoleLinkage(outRec);
					PolyNode polyNode = new PolyNode();
					polytree.m_AllPolys.Add(polyNode);
					outRec.PolyNode = polyNode;
					polyNode.m_polygon.Capacity = num;
					OutPt outPt = outRec.Pts.Prev;
					for (int j = 0; j < num; j++)
					{
						polyNode.m_polygon.Add(outPt.Pt);
						outPt = outPt.Prev;
					}
				}
			}
			polytree.m_Childs.Capacity = this.m_PolyOuts.Count;
			for (int k = 0; k < this.m_PolyOuts.Count; k++)
			{
				OutRec outRec2 = this.m_PolyOuts[k];
				if (outRec2.PolyNode != null)
				{
					if (outRec2.IsOpen)
					{
						outRec2.PolyNode.IsOpen = true;
						polytree.AddChild(outRec2.PolyNode);
					}
					else if (outRec2.FirstLeft != null && outRec2.FirstLeft.PolyNode != null)
					{
						outRec2.FirstLeft.PolyNode.AddChild(outRec2.PolyNode);
					}
					else
					{
						polytree.AddChild(outRec2.PolyNode);
					}
				}
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00008588 File Offset: 0x00006788
		private void FixupOutPolyline(OutRec outrec)
		{
			OutPt outPt = outrec.Pts;
			OutPt outPt2 = outPt.Prev;
			while (outPt != outPt2)
			{
				outPt = outPt.Next;
				if (outPt.Pt == outPt.Prev.Pt)
				{
					if (outPt == outPt2)
					{
						outPt2 = outPt.Prev;
					}
					OutPt prev = outPt.Prev;
					prev.Next = outPt.Next;
					outPt.Next.Prev = prev;
					outPt = prev;
				}
			}
			if (outPt == outPt.Prev)
			{
				outrec.Pts = null;
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00008604 File Offset: 0x00006804
		private void FixupOutPolygon(OutRec outRec)
		{
			OutPt outPt = null;
			outRec.BottomPt = null;
			OutPt outPt2 = outRec.Pts;
			bool flag = base.PreserveCollinear || this.StrictlySimple;
			while (outPt2.Prev != outPt2 && outPt2.Prev != outPt2.Next)
			{
				if (outPt2.Pt == outPt2.Next.Pt || outPt2.Pt == outPt2.Prev.Pt || (ClipperBase.SlopesEqual(outPt2.Prev.Pt, outPt2.Pt, outPt2.Next.Pt, this.m_UseFullRange) && (!flag || !base.Pt2IsBetweenPt1AndPt3(outPt2.Prev.Pt, outPt2.Pt, outPt2.Next.Pt))))
				{
					outPt = null;
					outPt2.Prev.Next = outPt2.Next;
					outPt2.Next.Prev = outPt2.Prev;
					outPt2 = outPt2.Prev;
				}
				else
				{
					if (outPt2 == outPt)
					{
						outRec.Pts = outPt2;
						return;
					}
					if (outPt == null)
					{
						outPt = outPt2;
					}
					outPt2 = outPt2.Next;
				}
			}
			outRec.Pts = null;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00008720 File Offset: 0x00006920
		private OutPt DupOutPt(OutPt outPt, bool InsertAfter)
		{
			OutPt outPt2 = new OutPt();
			outPt2.Pt = outPt.Pt;
			outPt2.Idx = outPt.Idx;
			if (InsertAfter)
			{
				outPt2.Next = outPt.Next;
				outPt2.Prev = outPt;
				outPt.Next.Prev = outPt2;
				outPt.Next = outPt2;
			}
			else
			{
				outPt2.Prev = outPt.Prev;
				outPt2.Next = outPt;
				outPt.Prev.Next = outPt2;
				outPt.Prev = outPt2;
			}
			return outPt2;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000087A0 File Offset: 0x000069A0
		private bool GetOverlap(long a1, long a2, long b1, long b2, out long Left, out long Right)
		{
			if (a1 < a2)
			{
				if (b1 < b2)
				{
					Left = Math.Max(a1, b1);
					Right = Math.Min(a2, b2);
				}
				else
				{
					Left = Math.Max(a1, b2);
					Right = Math.Min(a2, b1);
				}
			}
			else if (b1 < b2)
			{
				Left = Math.Max(a2, b1);
				Right = Math.Min(a1, b2);
			}
			else
			{
				Left = Math.Max(a2, b2);
				Right = Math.Min(a1, b1);
			}
			return Left < Right;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00008820 File Offset: 0x00006A20
		private bool JoinHorz(OutPt op1, OutPt op1b, OutPt op2, OutPt op2b, IntPoint Pt, bool DiscardLeft)
		{
			Direction direction = ((op1.Pt.X > op1b.Pt.X) ? Direction.dRightToLeft : Direction.dLeftToRight);
			Direction direction2 = ((op2.Pt.X > op2b.Pt.X) ? Direction.dRightToLeft : Direction.dLeftToRight);
			if (direction == direction2)
			{
				return false;
			}
			if (direction == Direction.dLeftToRight)
			{
				while (op1.Next.Pt.X <= Pt.X && op1.Next.Pt.X >= op1.Pt.X && op1.Next.Pt.Y == Pt.Y)
				{
					op1 = op1.Next;
				}
				if (DiscardLeft && op1.Pt.X != Pt.X)
				{
					op1 = op1.Next;
				}
				op1b = this.DupOutPt(op1, !DiscardLeft);
				if (op1b.Pt != Pt)
				{
					op1 = op1b;
					op1.Pt = Pt;
					op1b = this.DupOutPt(op1, !DiscardLeft);
				}
			}
			else
			{
				while (op1.Next.Pt.X >= Pt.X && op1.Next.Pt.X <= op1.Pt.X && op1.Next.Pt.Y == Pt.Y)
				{
					op1 = op1.Next;
				}
				if (!DiscardLeft && op1.Pt.X != Pt.X)
				{
					op1 = op1.Next;
				}
				op1b = this.DupOutPt(op1, DiscardLeft);
				if (op1b.Pt != Pt)
				{
					op1 = op1b;
					op1.Pt = Pt;
					op1b = this.DupOutPt(op1, DiscardLeft);
				}
			}
			if (direction2 == Direction.dLeftToRight)
			{
				while (op2.Next.Pt.X <= Pt.X && op2.Next.Pt.X >= op2.Pt.X && op2.Next.Pt.Y == Pt.Y)
				{
					op2 = op2.Next;
				}
				if (DiscardLeft && op2.Pt.X != Pt.X)
				{
					op2 = op2.Next;
				}
				op2b = this.DupOutPt(op2, !DiscardLeft);
				if (op2b.Pt != Pt)
				{
					op2 = op2b;
					op2.Pt = Pt;
					op2b = this.DupOutPt(op2, !DiscardLeft);
				}
			}
			else
			{
				while (op2.Next.Pt.X >= Pt.X && op2.Next.Pt.X <= op2.Pt.X && op2.Next.Pt.Y == Pt.Y)
				{
					op2 = op2.Next;
				}
				if (!DiscardLeft && op2.Pt.X != Pt.X)
				{
					op2 = op2.Next;
				}
				op2b = this.DupOutPt(op2, DiscardLeft);
				if (op2b.Pt != Pt)
				{
					op2 = op2b;
					op2.Pt = Pt;
					op2b = this.DupOutPt(op2, DiscardLeft);
				}
			}
			if (direction == Direction.dLeftToRight == DiscardLeft)
			{
				op1.Prev = op2;
				op2.Next = op1;
				op1b.Next = op2b;
				op2b.Prev = op1b;
			}
			else
			{
				op1.Next = op2;
				op2.Prev = op1;
				op1b.Prev = op2b;
				op2b.Next = op1b;
			}
			return true;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00008B84 File Offset: 0x00006D84
		private bool JoinPoints(Join j, OutRec outRec1, OutRec outRec2)
		{
			OutPt outPt = j.OutPt1;
			OutPt outPt2 = j.OutPt2;
			bool flag = j.OutPt1.Pt.Y == j.OffPt.Y;
			if (flag && j.OffPt == j.OutPt1.Pt && j.OffPt == j.OutPt2.Pt)
			{
				if (outRec1 != outRec2)
				{
					return false;
				}
				OutPt outPt3 = j.OutPt1.Next;
				while (outPt3 != outPt && outPt3.Pt == j.OffPt)
				{
					outPt3 = outPt3.Next;
				}
				bool flag2 = outPt3.Pt.Y > j.OffPt.Y;
				OutPt outPt4 = j.OutPt2.Next;
				while (outPt4 != outPt2 && outPt4.Pt == j.OffPt)
				{
					outPt4 = outPt4.Next;
				}
				bool flag3 = outPt4.Pt.Y > j.OffPt.Y;
				if (flag2 == flag3)
				{
					return false;
				}
				if (flag2)
				{
					outPt3 = this.DupOutPt(outPt, false);
					outPt4 = this.DupOutPt(outPt2, true);
					outPt.Prev = outPt2;
					outPt2.Next = outPt;
					outPt3.Next = outPt4;
					outPt4.Prev = outPt3;
					j.OutPt1 = outPt;
					j.OutPt2 = outPt3;
					return true;
				}
				outPt3 = this.DupOutPt(outPt, true);
				outPt4 = this.DupOutPt(outPt2, false);
				outPt.Next = outPt2;
				outPt2.Prev = outPt;
				outPt3.Prev = outPt4;
				outPt4.Next = outPt3;
				j.OutPt1 = outPt;
				j.OutPt2 = outPt3;
				return true;
			}
			else if (flag)
			{
				OutPt outPt3 = outPt;
				while (outPt.Prev.Pt.Y == outPt.Pt.Y && outPt.Prev != outPt3)
				{
					if (outPt.Prev == outPt2)
					{
						break;
					}
					outPt = outPt.Prev;
				}
				while (outPt3.Next.Pt.Y == outPt3.Pt.Y && outPt3.Next != outPt && outPt3.Next != outPt2)
				{
					outPt3 = outPt3.Next;
				}
				if (outPt3.Next == outPt || outPt3.Next == outPt2)
				{
					return false;
				}
				OutPt outPt4 = outPt2;
				while (outPt2.Prev.Pt.Y == outPt2.Pt.Y && outPt2.Prev != outPt4)
				{
					if (outPt2.Prev == outPt3)
					{
						break;
					}
					outPt2 = outPt2.Prev;
				}
				while (outPt4.Next.Pt.Y == outPt4.Pt.Y && outPt4.Next != outPt2 && outPt4.Next != outPt)
				{
					outPt4 = outPt4.Next;
				}
				if (outPt4.Next == outPt2 || outPt4.Next == outPt)
				{
					return false;
				}
				long num;
				long num2;
				if (!this.GetOverlap(outPt.Pt.X, outPt3.Pt.X, outPt2.Pt.X, outPt4.Pt.X, out num, out num2))
				{
					return false;
				}
				IntPoint intPoint;
				bool flag4;
				if (outPt.Pt.X >= num && outPt.Pt.X <= num2)
				{
					intPoint = outPt.Pt;
					flag4 = outPt.Pt.X > outPt3.Pt.X;
				}
				else if (outPt2.Pt.X >= num && outPt2.Pt.X <= num2)
				{
					intPoint = outPt2.Pt;
					flag4 = outPt2.Pt.X > outPt4.Pt.X;
				}
				else if (outPt3.Pt.X >= num && outPt3.Pt.X <= num2)
				{
					intPoint = outPt3.Pt;
					flag4 = outPt3.Pt.X > outPt.Pt.X;
				}
				else
				{
					intPoint = outPt4.Pt;
					flag4 = outPt4.Pt.X > outPt2.Pt.X;
				}
				j.OutPt1 = outPt;
				j.OutPt2 = outPt2;
				return this.JoinHorz(outPt, outPt3, outPt2, outPt4, intPoint, flag4);
			}
			else
			{
				OutPt outPt3 = outPt.Next;
				while (outPt3.Pt == outPt.Pt && outPt3 != outPt)
				{
					outPt3 = outPt3.Next;
				}
				bool flag5 = outPt3.Pt.Y > outPt.Pt.Y || !ClipperBase.SlopesEqual(outPt.Pt, outPt3.Pt, j.OffPt, this.m_UseFullRange);
				if (flag5)
				{
					outPt3 = outPt.Prev;
					while (outPt3.Pt == outPt.Pt && outPt3 != outPt)
					{
						outPt3 = outPt3.Prev;
					}
					if (outPt3.Pt.Y > outPt.Pt.Y || !ClipperBase.SlopesEqual(outPt.Pt, outPt3.Pt, j.OffPt, this.m_UseFullRange))
					{
						return false;
					}
				}
				OutPt outPt4 = outPt2.Next;
				while (outPt4.Pt == outPt2.Pt && outPt4 != outPt2)
				{
					outPt4 = outPt4.Next;
				}
				bool flag6 = outPt4.Pt.Y > outPt2.Pt.Y || !ClipperBase.SlopesEqual(outPt2.Pt, outPt4.Pt, j.OffPt, this.m_UseFullRange);
				if (flag6)
				{
					outPt4 = outPt2.Prev;
					while (outPt4.Pt == outPt2.Pt && outPt4 != outPt2)
					{
						outPt4 = outPt4.Prev;
					}
					if (outPt4.Pt.Y > outPt2.Pt.Y || !ClipperBase.SlopesEqual(outPt2.Pt, outPt4.Pt, j.OffPt, this.m_UseFullRange))
					{
						return false;
					}
				}
				if (outPt3 == outPt || outPt4 == outPt2 || outPt3 == outPt4 || (outRec1 == outRec2 && flag5 == flag6))
				{
					return false;
				}
				if (flag5)
				{
					outPt3 = this.DupOutPt(outPt, false);
					outPt4 = this.DupOutPt(outPt2, true);
					outPt.Prev = outPt2;
					outPt2.Next = outPt;
					outPt3.Next = outPt4;
					outPt4.Prev = outPt3;
					j.OutPt1 = outPt;
					j.OutPt2 = outPt3;
					return true;
				}
				outPt3 = this.DupOutPt(outPt, true);
				outPt4 = this.DupOutPt(outPt2, false);
				outPt.Next = outPt2;
				outPt2.Prev = outPt;
				outPt3.Prev = outPt4;
				outPt4.Next = outPt3;
				j.OutPt1 = outPt;
				j.OutPt2 = outPt3;
				return true;
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00009194 File Offset: 0x00007394
		public static int PointInPolygon(IntPoint pt, List<IntPoint> path)
		{
			int num = 0;
			int count = path.Count;
			if (count < 3)
			{
				return 0;
			}
			IntPoint intPoint = path[0];
			for (int i = 1; i <= count; i++)
			{
				IntPoint intPoint2 = ((i == count) ? path[0] : path[i]);
				if (intPoint2.Y == pt.Y && (intPoint2.X == pt.X || (intPoint.Y == pt.Y && intPoint2.X > pt.X == intPoint.X < pt.X)))
				{
					return -1;
				}
				if (intPoint.Y < pt.Y != intPoint2.Y < pt.Y)
				{
					if (intPoint.X >= pt.X)
					{
						if (intPoint2.X > pt.X)
						{
							num = 1 - num;
						}
						else
						{
							double num2 = (double)(intPoint.X - pt.X) * (double)(intPoint2.Y - pt.Y) - (double)(intPoint2.X - pt.X) * (double)(intPoint.Y - pt.Y);
							if (num2 == 0.0)
							{
								return -1;
							}
							if (num2 > 0.0 == intPoint2.Y > intPoint.Y)
							{
								num = 1 - num;
							}
						}
					}
					else if (intPoint2.X > pt.X)
					{
						double num3 = (double)(intPoint.X - pt.X) * (double)(intPoint2.Y - pt.Y) - (double)(intPoint2.X - pt.X) * (double)(intPoint.Y - pt.Y);
						if (num3 == 0.0)
						{
							return -1;
						}
						if (num3 > 0.0 == intPoint2.Y > intPoint.Y)
						{
							num = 1 - num;
						}
					}
				}
				intPoint = intPoint2;
			}
			return num;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00009370 File Offset: 0x00007570
		private static int PointInPolygon(IntPoint pt, OutPt op)
		{
			int num = 0;
			OutPt outPt = op;
			long x = pt.X;
			long y = pt.Y;
			long num2 = op.Pt.X;
			long num3 = op.Pt.Y;
			for (;;)
			{
				op = op.Next;
				long x2 = op.Pt.X;
				long y2 = op.Pt.Y;
				if (y2 == y && (x2 == x || (num3 == y && x2 > x == num2 < x)))
				{
					break;
				}
				if (num3 < y != y2 < y)
				{
					if (num2 >= x)
					{
						if (x2 > x)
						{
							num = 1 - num;
						}
						else
						{
							double num4 = (double)(num2 - x) * (double)(y2 - y) - (double)(x2 - x) * (double)(num3 - y);
							if (num4 == 0.0)
							{
								return -1;
							}
							if (num4 > 0.0 == y2 > num3)
							{
								num = 1 - num;
							}
						}
					}
					else if (x2 > x)
					{
						double num5 = (double)(num2 - x) * (double)(y2 - y) - (double)(x2 - x) * (double)(num3 - y);
						if (num5 == 0.0)
						{
							return -1;
						}
						if (num5 > 0.0 == y2 > num3)
						{
							num = 1 - num;
						}
					}
				}
				num2 = x2;
				num3 = y2;
				if (outPt == op)
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000094A4 File Offset: 0x000076A4
		private static bool Poly2ContainsPoly1(OutPt outPt1, OutPt outPt2)
		{
			OutPt outPt3 = outPt1;
			int num;
			for (;;)
			{
				num = Clipper.PointInPolygon(outPt3.Pt, outPt2);
				if (num >= 0)
				{
					break;
				}
				outPt3 = outPt3.Next;
				if (outPt3 == outPt1)
				{
					return true;
				}
			}
			return num > 0;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000094D8 File Offset: 0x000076D8
		private void FixupFirstLefts1(OutRec OldOutRec, OutRec NewOutRec)
		{
			foreach (OutRec outRec in this.m_PolyOuts)
			{
				OutRec outRec2 = Clipper.ParseFirstLeft(outRec.FirstLeft);
				if (outRec.Pts != null && outRec2 == OldOutRec && Clipper.Poly2ContainsPoly1(outRec.Pts, NewOutRec.Pts))
				{
					outRec.FirstLeft = NewOutRec;
				}
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00009558 File Offset: 0x00007758
		private void FixupFirstLefts2(OutRec innerOutRec, OutRec outerOutRec)
		{
			OutRec firstLeft = outerOutRec.FirstLeft;
			foreach (OutRec outRec in this.m_PolyOuts)
			{
				if (outRec.Pts != null && outRec != outerOutRec && outRec != innerOutRec)
				{
					OutRec outRec2 = Clipper.ParseFirstLeft(outRec.FirstLeft);
					if (outRec2 == firstLeft || outRec2 == innerOutRec || outRec2 == outerOutRec)
					{
						if (Clipper.Poly2ContainsPoly1(outRec.Pts, innerOutRec.Pts))
						{
							outRec.FirstLeft = innerOutRec;
						}
						else if (Clipper.Poly2ContainsPoly1(outRec.Pts, outerOutRec.Pts))
						{
							outRec.FirstLeft = outerOutRec;
						}
						else if (outRec.FirstLeft == innerOutRec || outRec.FirstLeft == outerOutRec)
						{
							outRec.FirstLeft = firstLeft;
						}
					}
				}
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000962C File Offset: 0x0000782C
		private void FixupFirstLefts3(OutRec OldOutRec, OutRec NewOutRec)
		{
			foreach (OutRec outRec in this.m_PolyOuts)
			{
				OutRec outRec2 = Clipper.ParseFirstLeft(outRec.FirstLeft);
				if (outRec.Pts != null && outRec2 == OldOutRec)
				{
					outRec.FirstLeft = NewOutRec;
				}
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00009698 File Offset: 0x00007898
		private static OutRec ParseFirstLeft(OutRec FirstLeft)
		{
			while (FirstLeft != null && FirstLeft.Pts == null)
			{
				FirstLeft = FirstLeft.FirstLeft;
			}
			return FirstLeft;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000096B0 File Offset: 0x000078B0
		private void JoinCommonEdges()
		{
			for (int i = 0; i < this.m_Joins.Count; i++)
			{
				Join join = this.m_Joins[i];
				OutRec outRec = this.GetOutRec(join.OutPt1.Idx);
				OutRec outRec2 = this.GetOutRec(join.OutPt2.Idx);
				if (outRec.Pts != null && outRec2.Pts != null && !outRec.IsOpen && !outRec2.IsOpen)
				{
					OutRec outRec3;
					if (outRec == outRec2)
					{
						outRec3 = outRec;
					}
					else if (this.OutRec1RightOfOutRec2(outRec, outRec2))
					{
						outRec3 = outRec2;
					}
					else if (this.OutRec1RightOfOutRec2(outRec2, outRec))
					{
						outRec3 = outRec;
					}
					else
					{
						outRec3 = this.GetLowermostRec(outRec, outRec2);
					}
					if (this.JoinPoints(join, outRec, outRec2))
					{
						if (outRec == outRec2)
						{
							outRec.Pts = join.OutPt1;
							outRec.BottomPt = null;
							outRec2 = base.CreateOutRec();
							outRec2.Pts = join.OutPt2;
							this.UpdateOutPtIdxs(outRec2);
							if (Clipper.Poly2ContainsPoly1(outRec2.Pts, outRec.Pts))
							{
								outRec2.IsHole = !outRec.IsHole;
								outRec2.FirstLeft = outRec;
								if (this.m_UsingPolyTree)
								{
									this.FixupFirstLefts2(outRec2, outRec);
								}
								if ((outRec2.IsHole ^ this.ReverseSolution) == this.Area(outRec2) > 0.0)
								{
									this.ReversePolyPtLinks(outRec2.Pts);
								}
							}
							else if (Clipper.Poly2ContainsPoly1(outRec.Pts, outRec2.Pts))
							{
								outRec2.IsHole = outRec.IsHole;
								outRec.IsHole = !outRec2.IsHole;
								outRec2.FirstLeft = outRec.FirstLeft;
								outRec.FirstLeft = outRec2;
								if (this.m_UsingPolyTree)
								{
									this.FixupFirstLefts2(outRec, outRec2);
								}
								if ((outRec.IsHole ^ this.ReverseSolution) == this.Area(outRec) > 0.0)
								{
									this.ReversePolyPtLinks(outRec.Pts);
								}
							}
							else
							{
								outRec2.IsHole = outRec.IsHole;
								outRec2.FirstLeft = outRec.FirstLeft;
								if (this.m_UsingPolyTree)
								{
									this.FixupFirstLefts1(outRec, outRec2);
								}
							}
						}
						else
						{
							outRec2.Pts = null;
							outRec2.BottomPt = null;
							outRec2.Idx = outRec.Idx;
							outRec.IsHole = outRec3.IsHole;
							if (outRec3 == outRec2)
							{
								outRec.FirstLeft = outRec2.FirstLeft;
							}
							outRec2.FirstLeft = outRec;
							if (this.m_UsingPolyTree)
							{
								this.FixupFirstLefts3(outRec2, outRec);
							}
						}
					}
				}
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00009914 File Offset: 0x00007B14
		private void UpdateOutPtIdxs(OutRec outrec)
		{
			OutPt outPt = outrec.Pts;
			do
			{
				outPt.Idx = outrec.Idx;
				outPt = outPt.Prev;
			}
			while (outPt != outrec.Pts);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00009944 File Offset: 0x00007B44
		private void DoSimplePolygons()
		{
			int i = 0;
			while (i < this.m_PolyOuts.Count)
			{
				OutRec outRec = this.m_PolyOuts[i++];
				OutPt outPt = outRec.Pts;
				if (outPt != null && !outRec.IsOpen)
				{
					do
					{
						for (OutPt outPt2 = outPt.Next; outPt2 != outRec.Pts; outPt2 = outPt2.Next)
						{
							if (outPt.Pt == outPt2.Pt && outPt2.Next != outPt && outPt2.Prev != outPt)
							{
								OutPt prev = outPt.Prev;
								OutPt prev2 = outPt2.Prev;
								outPt.Prev = prev2;
								prev2.Next = outPt;
								outPt2.Prev = prev;
								prev.Next = outPt2;
								outRec.Pts = outPt;
								OutRec outRec2 = base.CreateOutRec();
								outRec2.Pts = outPt2;
								this.UpdateOutPtIdxs(outRec2);
								if (Clipper.Poly2ContainsPoly1(outRec2.Pts, outRec.Pts))
								{
									outRec2.IsHole = !outRec.IsHole;
									outRec2.FirstLeft = outRec;
									if (this.m_UsingPolyTree)
									{
										this.FixupFirstLefts2(outRec2, outRec);
									}
								}
								else if (Clipper.Poly2ContainsPoly1(outRec.Pts, outRec2.Pts))
								{
									outRec2.IsHole = outRec.IsHole;
									outRec.IsHole = !outRec2.IsHole;
									outRec2.FirstLeft = outRec.FirstLeft;
									outRec.FirstLeft = outRec2;
									if (this.m_UsingPolyTree)
									{
										this.FixupFirstLefts2(outRec, outRec2);
									}
								}
								else
								{
									outRec2.IsHole = outRec.IsHole;
									outRec2.FirstLeft = outRec.FirstLeft;
									if (this.m_UsingPolyTree)
									{
										this.FixupFirstLefts1(outRec, outRec2);
									}
								}
								outPt2 = outPt;
							}
						}
						outPt = outPt.Next;
					}
					while (outPt != outRec.Pts);
				}
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00009B0C File Offset: 0x00007D0C
		public static double Area(List<IntPoint> poly)
		{
			int count = poly.Count;
			if (count < 3)
			{
				return 0.0;
			}
			double num = 0.0;
			int i = 0;
			int num2 = count - 1;
			while (i < count)
			{
				num += ((double)poly[num2].X + (double)poly[i].X) * ((double)poly[num2].Y - (double)poly[i].Y);
				num2 = i;
				i++;
			}
			return -num * 0.5;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00009B90 File Offset: 0x00007D90
		internal double Area(OutRec outRec)
		{
			return this.Area(outRec.Pts);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00009BA0 File Offset: 0x00007DA0
		internal double Area(OutPt op)
		{
			OutPt outPt = op;
			if (op == null)
			{
				return 0.0;
			}
			double num = 0.0;
			do
			{
				num += (double)(op.Prev.Pt.X + op.Pt.X) * (double)(op.Prev.Pt.Y - op.Pt.Y);
				op = op.Next;
			}
			while (op != outPt);
			return num * 0.5;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00009C1C File Offset: 0x00007E1C
		public static List<List<IntPoint>> SimplifyPolygon(List<IntPoint> poly, PolyFillType fillType = PolyFillType.pftEvenOdd)
		{
			List<List<IntPoint>> list = new List<List<IntPoint>>();
			Clipper clipper = new Clipper(0);
			clipper.StrictlySimple = true;
			clipper.AddPath(poly, PolyType.ptSubject, true);
			clipper.Execute(ClipType.ctUnion, list, fillType, fillType);
			return list;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00009C54 File Offset: 0x00007E54
		public static List<List<IntPoint>> SimplifyPolygons(List<List<IntPoint>> polys, PolyFillType fillType = PolyFillType.pftEvenOdd)
		{
			List<List<IntPoint>> list = new List<List<IntPoint>>();
			Clipper clipper = new Clipper(0);
			clipper.StrictlySimple = true;
			clipper.AddPaths(polys, PolyType.ptSubject, true);
			clipper.Execute(ClipType.ctUnion, list, fillType, fillType);
			return list;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00009C8C File Offset: 0x00007E8C
		private static double DistanceSqrd(IntPoint pt1, IntPoint pt2)
		{
			double num = (double)pt1.X - (double)pt2.X;
			double num2 = (double)pt1.Y - (double)pt2.Y;
			return num * num + num2 * num2;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00009CC0 File Offset: 0x00007EC0
		private static double DistanceFromLineSqrd(IntPoint pt, IntPoint ln1, IntPoint ln2)
		{
			double num = (double)(ln1.Y - ln2.Y);
			double num2 = (double)(ln2.X - ln1.X);
			double num3 = num * (double)ln1.X + num2 * (double)ln1.Y;
			num3 = num * (double)pt.X + num2 * (double)pt.Y - num3;
			return num3 * num3 / (num * num + num2 * num2);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00009D20 File Offset: 0x00007F20
		private static bool SlopesNearCollinear(IntPoint pt1, IntPoint pt2, IntPoint pt3, double distSqrd)
		{
			if (Math.Abs(pt1.X - pt2.X) > Math.Abs(pt1.Y - pt2.Y))
			{
				if (pt1.X > pt2.X == pt1.X < pt3.X)
				{
					return Clipper.DistanceFromLineSqrd(pt1, pt2, pt3) < distSqrd;
				}
				if (pt2.X > pt1.X == pt2.X < pt3.X)
				{
					return Clipper.DistanceFromLineSqrd(pt2, pt1, pt3) < distSqrd;
				}
				return Clipper.DistanceFromLineSqrd(pt3, pt1, pt2) < distSqrd;
			}
			else
			{
				if (pt1.Y > pt2.Y == pt1.Y < pt3.Y)
				{
					return Clipper.DistanceFromLineSqrd(pt1, pt2, pt3) < distSqrd;
				}
				if (pt2.Y > pt1.Y == pt2.Y < pt3.Y)
				{
					return Clipper.DistanceFromLineSqrd(pt2, pt1, pt3) < distSqrd;
				}
				return Clipper.DistanceFromLineSqrd(pt3, pt1, pt2) < distSqrd;
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00009E14 File Offset: 0x00008014
		private static bool PointsAreClose(IntPoint pt1, IntPoint pt2, double distSqrd)
		{
			double num = (double)pt1.X - (double)pt2.X;
			double num2 = (double)pt1.Y - (double)pt2.Y;
			return num * num + num2 * num2 <= distSqrd;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00009E4C File Offset: 0x0000804C
		private static OutPt ExcludeOp(OutPt op)
		{
			OutPt prev = op.Prev;
			prev.Next = op.Next;
			op.Next.Prev = prev;
			prev.Idx = 0;
			return prev;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00009E80 File Offset: 0x00008080
		public static List<IntPoint> CleanPolygon(List<IntPoint> path, double distance = 1.415)
		{
			int num = path.Count;
			if (num == 0)
			{
				return new List<IntPoint>();
			}
			OutPt[] array = new OutPt[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new OutPt();
			}
			for (int j = 0; j < num; j++)
			{
				array[j].Pt = path[j];
				array[j].Next = array[(j + 1) % num];
				array[j].Next.Prev = array[j];
				array[j].Idx = 0;
			}
			double num2 = distance * distance;
			OutPt outPt = array[0];
			while (outPt.Idx == 0 && outPt.Next != outPt.Prev)
			{
				if (Clipper.PointsAreClose(outPt.Pt, outPt.Prev.Pt, num2))
				{
					outPt = Clipper.ExcludeOp(outPt);
					num--;
				}
				else if (Clipper.PointsAreClose(outPt.Prev.Pt, outPt.Next.Pt, num2))
				{
					Clipper.ExcludeOp(outPt.Next);
					outPt = Clipper.ExcludeOp(outPt);
					num -= 2;
				}
				else if (Clipper.SlopesNearCollinear(outPt.Prev.Pt, outPt.Pt, outPt.Next.Pt, num2))
				{
					outPt = Clipper.ExcludeOp(outPt);
					num--;
				}
				else
				{
					outPt.Idx = 1;
					outPt = outPt.Next;
				}
			}
			if (num < 3)
			{
				num = 0;
			}
			List<IntPoint> list = new List<IntPoint>(num);
			for (int k = 0; k < num; k++)
			{
				list.Add(outPt.Pt);
				outPt = outPt.Next;
			}
			return list;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000A004 File Offset: 0x00008204
		public static List<List<IntPoint>> CleanPolygons(List<List<IntPoint>> polys, double distance = 1.415)
		{
			List<List<IntPoint>> list = new List<List<IntPoint>>(polys.Count);
			for (int i = 0; i < polys.Count; i++)
			{
				list.Add(Clipper.CleanPolygon(polys[i], distance));
			}
			return list;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000A044 File Offset: 0x00008244
		internal static List<List<IntPoint>> Minkowski(List<IntPoint> pattern, List<IntPoint> path, bool IsSum, bool IsClosed)
		{
			int num = (IsClosed ? 1 : 0);
			int count = pattern.Count;
			int count2 = path.Count;
			List<List<IntPoint>> list = new List<List<IntPoint>>(count2);
			if (IsSum)
			{
				for (int i = 0; i < count2; i++)
				{
					List<IntPoint> list2 = new List<IntPoint>(count);
					foreach (IntPoint intPoint in pattern)
					{
						list2.Add(new IntPoint(path[i].X + intPoint.X, path[i].Y + intPoint.Y));
					}
					list.Add(list2);
				}
			}
			else
			{
				for (int j = 0; j < count2; j++)
				{
					List<IntPoint> list3 = new List<IntPoint>(count);
					foreach (IntPoint intPoint2 in pattern)
					{
						list3.Add(new IntPoint(path[j].X - intPoint2.X, path[j].Y - intPoint2.Y));
					}
					list.Add(list3);
				}
			}
			List<List<IntPoint>> list4 = new List<List<IntPoint>>((count2 + num) * (count + 1));
			for (int k = 0; k < count2 - 1 + num; k++)
			{
				for (int l = 0; l < count; l++)
				{
					List<IntPoint> list5 = new List<IntPoint>(4);
					list5.Add(list[k % count2][l % count]);
					list5.Add(list[(k + 1) % count2][l % count]);
					list5.Add(list[(k + 1) % count2][(l + 1) % count]);
					list5.Add(list[k % count2][(l + 1) % count]);
					if (!Clipper.Orientation(list5))
					{
						list5.Reverse();
					}
					list4.Add(list5);
				}
			}
			return list4;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000A26C File Offset: 0x0000846C
		public static List<List<IntPoint>> MinkowskiSum(List<IntPoint> pattern, List<IntPoint> path, bool pathIsClosed)
		{
			List<List<IntPoint>> list = Clipper.Minkowski(pattern, path, true, pathIsClosed);
			Clipper clipper = new Clipper(0);
			clipper.AddPaths(list, PolyType.ptSubject, true);
			clipper.Execute(ClipType.ctUnion, list, PolyFillType.pftNonZero, PolyFillType.pftNonZero);
			return list;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000A2A0 File Offset: 0x000084A0
		private static List<IntPoint> TranslatePath(List<IntPoint> path, IntPoint delta)
		{
			List<IntPoint> list = new List<IntPoint>(path.Count);
			for (int i = 0; i < path.Count; i++)
			{
				list.Add(new IntPoint(path[i].X + delta.X, path[i].Y + delta.Y));
			}
			return list;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000A2FC File Offset: 0x000084FC
		public static List<List<IntPoint>> MinkowskiSum(List<IntPoint> pattern, List<List<IntPoint>> paths, bool pathIsClosed)
		{
			List<List<IntPoint>> list = new List<List<IntPoint>>();
			Clipper clipper = new Clipper(0);
			for (int i = 0; i < paths.Count; i++)
			{
				List<List<IntPoint>> list2 = Clipper.Minkowski(pattern, paths[i], true, pathIsClosed);
				clipper.AddPaths(list2, PolyType.ptSubject, true);
				if (pathIsClosed)
				{
					List<IntPoint> list3 = Clipper.TranslatePath(paths[i], pattern[0]);
					clipper.AddPath(list3, PolyType.ptClip, true);
				}
			}
			clipper.Execute(ClipType.ctUnion, list, PolyFillType.pftNonZero, PolyFillType.pftNonZero);
			return list;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000A370 File Offset: 0x00008570
		public static List<List<IntPoint>> MinkowskiDiff(List<IntPoint> poly1, List<IntPoint> poly2)
		{
			List<List<IntPoint>> list = Clipper.Minkowski(poly1, poly2, false, true);
			Clipper clipper = new Clipper(0);
			clipper.AddPaths(list, PolyType.ptSubject, true);
			clipper.Execute(ClipType.ctUnion, list, PolyFillType.pftNonZero, PolyFillType.pftNonZero);
			return list;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000A3A4 File Offset: 0x000085A4
		public static List<List<IntPoint>> PolyTreeToPaths(PolyTree polytree)
		{
			List<List<IntPoint>> list = new List<List<IntPoint>>();
			list.Capacity = polytree.Total;
			Clipper.AddPolyNodeToPaths(polytree, Clipper.NodeType.ntAny, list);
			return list;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000A3CC File Offset: 0x000085CC
		internal static void AddPolyNodeToPaths(PolyNode polynode, Clipper.NodeType nt, List<List<IntPoint>> paths)
		{
			bool flag = true;
			if (nt != Clipper.NodeType.ntOpen)
			{
				if (nt == Clipper.NodeType.ntClosed)
				{
					flag = !polynode.IsOpen;
				}
				if (polynode.m_polygon.Count > 0 && flag)
				{
					paths.Add(polynode.m_polygon);
				}
				foreach (PolyNode polyNode in polynode.Childs)
				{
					Clipper.AddPolyNodeToPaths(polyNode, nt, paths);
				}
				return;
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000A454 File Offset: 0x00008654
		public static List<List<IntPoint>> OpenPathsFromPolyTree(PolyTree polytree)
		{
			List<List<IntPoint>> list = new List<List<IntPoint>>();
			list.Capacity = polytree.ChildCount;
			for (int i = 0; i < polytree.ChildCount; i++)
			{
				if (polytree.Childs[i].IsOpen)
				{
					list.Add(polytree.Childs[i].m_polygon);
				}
			}
			return list;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000A4B0 File Offset: 0x000086B0
		public static List<List<IntPoint>> ClosedPathsFromPolyTree(PolyTree polytree)
		{
			List<List<IntPoint>> list = new List<List<IntPoint>>();
			list.Capacity = polytree.Total;
			Clipper.AddPolyNodeToPaths(polytree, Clipper.NodeType.ntClosed, list);
			return list;
		}

		// Token: 0x0400008C RID: 140
		public const int ioReverseSolution = 1;

		// Token: 0x0400008D RID: 141
		public const int ioStrictlySimple = 2;

		// Token: 0x0400008E RID: 142
		public const int ioPreserveCollinear = 4;

		// Token: 0x0400008F RID: 143
		private ClipType m_ClipType;

		// Token: 0x04000090 RID: 144
		private Maxima m_Maxima;

		// Token: 0x04000091 RID: 145
		private TEdge m_SortedEdges;

		// Token: 0x04000092 RID: 146
		private List<IntersectNode> m_IntersectList;

		// Token: 0x04000093 RID: 147
		private IComparer<IntersectNode> m_IntersectNodeComparer;

		// Token: 0x04000094 RID: 148
		private bool m_ExecuteLocked;

		// Token: 0x04000095 RID: 149
		private PolyFillType m_ClipFillType;

		// Token: 0x04000096 RID: 150
		private PolyFillType m_SubjFillType;

		// Token: 0x04000097 RID: 151
		private List<Join> m_Joins;

		// Token: 0x04000098 RID: 152
		private List<Join> m_GhostJoins;

		// Token: 0x04000099 RID: 153
		private bool m_UsingPolyTree;

		// Token: 0x0200014F RID: 335
		internal enum NodeType
		{
			// Token: 0x04000967 RID: 2407
			ntAny,
			// Token: 0x04000968 RID: 2408
			ntOpen,
			// Token: 0x04000969 RID: 2409
			ntClosed
		}
	}
}
