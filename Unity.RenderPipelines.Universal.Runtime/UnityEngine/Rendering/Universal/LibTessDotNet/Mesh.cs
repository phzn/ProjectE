using System;
using System.Diagnostics;

namespace UnityEngine.Rendering.Universal.LibTessDotNet
{
	// Token: 0x0200010A RID: 266
	internal class Mesh : MeshUtils.Pooled<Mesh>
	{
		// Token: 0x06000861 RID: 2145 RVA: 0x00034CD4 File Offset: 0x00032ED4
		public Mesh()
		{
			MeshUtils.Vertex vertex = (this._vHead = MeshUtils.Pooled<MeshUtils.Vertex>.Create());
			MeshUtils.Face face = (this._fHead = MeshUtils.Pooled<MeshUtils.Face>.Create());
			MeshUtils.EdgePair edgePair = MeshUtils.EdgePair.Create();
			MeshUtils.Edge edge = (this._eHead = edgePair._e);
			MeshUtils.Edge edge2 = (this._eHeadSym = edgePair._eSym);
			MeshUtils.Vertex vertex2 = vertex;
			vertex._prev = vertex;
			vertex._next = vertex2;
			vertex._anEdge = null;
			face._next = (face._prev = face);
			face._anEdge = null;
			face._trail = null;
			face._marked = false;
			face._inside = false;
			edge._next = edge;
			edge._Sym = edge2;
			edge._Onext = null;
			edge._Lnext = null;
			edge._Org = null;
			edge._Lface = null;
			edge._winding = 0;
			edge._activeRegion = null;
			edge2._next = edge2;
			edge2._Sym = edge;
			edge2._Onext = null;
			edge2._Lnext = null;
			edge2._Org = null;
			edge2._Lface = null;
			edge2._winding = 0;
			edge2._activeRegion = null;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00034DE8 File Offset: 0x00032FE8
		public override void Reset()
		{
			this._vHead = null;
			this._fHead = null;
			this._eHead = (this._eHeadSym = null);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00034E14 File Offset: 0x00033014
		public override void OnFree()
		{
			MeshUtils.Face face = this._fHead._next;
			MeshUtils.Face face2 = this._fHead;
			while (face != this._fHead)
			{
				face2 = face._next;
				face.Free();
				face = face2;
			}
			MeshUtils.Vertex vertex = this._vHead._next;
			MeshUtils.Vertex vertex2 = this._vHead;
			while (vertex != this._vHead)
			{
				vertex2 = vertex._next;
				vertex.Free();
				vertex = vertex2;
			}
			MeshUtils.Edge edge = this._eHead._next;
			MeshUtils.Edge edge2 = this._eHead;
			while (edge != this._eHead)
			{
				edge2 = edge._next;
				edge.Free();
				edge = edge2;
			}
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00034EB0 File Offset: 0x000330B0
		public MeshUtils.Edge MakeEdge()
		{
			MeshUtils.Edge edge = MeshUtils.MakeEdge(this._eHead);
			MeshUtils.MakeVertex(edge, this._vHead);
			MeshUtils.MakeVertex(edge._Sym, this._vHead);
			MeshUtils.MakeFace(edge, this._fHead);
			return edge;
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x00034EE8 File Offset: 0x000330E8
		public void Splice(MeshUtils.Edge eOrg, MeshUtils.Edge eDst)
		{
			if (eOrg == eDst)
			{
				return;
			}
			bool flag = false;
			if (eDst._Org != eOrg._Org)
			{
				flag = true;
				MeshUtils.KillVertex(eDst._Org, eOrg._Org);
			}
			bool flag2 = false;
			if (eDst._Lface != eOrg._Lface)
			{
				flag2 = true;
				MeshUtils.KillFace(eDst._Lface, eOrg._Lface);
			}
			MeshUtils.Splice(eDst, eOrg);
			if (!flag)
			{
				MeshUtils.MakeVertex(eDst, eOrg._Org);
				eOrg._Org._anEdge = eOrg;
			}
			if (!flag2)
			{
				MeshUtils.MakeFace(eDst, eOrg._Lface);
				eOrg._Lface._anEdge = eOrg;
			}
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00034F80 File Offset: 0x00033180
		public void Delete(MeshUtils.Edge eDel)
		{
			MeshUtils.Edge sym = eDel._Sym;
			bool flag = false;
			if (eDel._Lface != eDel._Rface)
			{
				flag = true;
				MeshUtils.KillFace(eDel._Lface, eDel._Rface);
			}
			if (eDel._Onext == eDel)
			{
				MeshUtils.KillVertex(eDel._Org, null);
			}
			else
			{
				eDel._Rface._anEdge = eDel._Oprev;
				eDel._Org._anEdge = eDel._Onext;
				MeshUtils.Splice(eDel, eDel._Oprev);
				if (!flag)
				{
					MeshUtils.MakeFace(eDel, eDel._Lface);
				}
			}
			if (sym._Onext == sym)
			{
				MeshUtils.KillVertex(sym._Org, null);
				MeshUtils.KillFace(sym._Lface, null);
			}
			else
			{
				eDel._Lface._anEdge = sym._Oprev;
				sym._Org._anEdge = sym._Onext;
				MeshUtils.Splice(sym, sym._Oprev);
			}
			MeshUtils.KillEdge(eDel);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00035064 File Offset: 0x00033264
		public MeshUtils.Edge AddEdgeVertex(MeshUtils.Edge eOrg)
		{
			MeshUtils.Edge edge = MeshUtils.MakeEdge(eOrg);
			MeshUtils.Edge sym = edge._Sym;
			MeshUtils.Splice(edge, eOrg._Lnext);
			edge._Org = eOrg._Dst;
			MeshUtils.MakeVertex(sym, edge._Org);
			edge._Lface = (sym._Lface = eOrg._Lface);
			return edge;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x000350BC File Offset: 0x000332BC
		public MeshUtils.Edge SplitEdge(MeshUtils.Edge eOrg)
		{
			MeshUtils.Edge sym = this.AddEdgeVertex(eOrg)._Sym;
			MeshUtils.Splice(eOrg._Sym, eOrg._Sym._Oprev);
			MeshUtils.Splice(eOrg._Sym, sym);
			eOrg._Dst = sym._Org;
			sym._Dst._anEdge = sym._Sym;
			sym._Rface = eOrg._Rface;
			sym._winding = eOrg._winding;
			sym._Sym._winding = eOrg._Sym._winding;
			return sym;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00035144 File Offset: 0x00033344
		public MeshUtils.Edge Connect(MeshUtils.Edge eOrg, MeshUtils.Edge eDst)
		{
			MeshUtils.Edge edge = MeshUtils.MakeEdge(eOrg);
			MeshUtils.Edge sym = edge._Sym;
			bool flag = false;
			if (eDst._Lface != eOrg._Lface)
			{
				flag = true;
				MeshUtils.KillFace(eDst._Lface, eOrg._Lface);
			}
			MeshUtils.Splice(edge, eOrg._Lnext);
			MeshUtils.Splice(sym, eDst);
			edge._Org = eOrg._Dst;
			sym._Org = eDst._Org;
			edge._Lface = (sym._Lface = eOrg._Lface);
			eOrg._Lface._anEdge = sym;
			if (!flag)
			{
				MeshUtils.MakeFace(edge, eOrg._Lface);
			}
			return edge;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x000351E0 File Offset: 0x000333E0
		public void ZapFace(MeshUtils.Face fZap)
		{
			MeshUtils.Edge anEdge = fZap._anEdge;
			MeshUtils.Edge edge = anEdge._Lnext;
			MeshUtils.Edge edge2;
			do
			{
				edge2 = edge;
				edge = edge2._Lnext;
				edge2._Lface = null;
				if (edge2._Rface == null)
				{
					if (edge2._Onext == edge2)
					{
						MeshUtils.KillVertex(edge2._Org, null);
					}
					else
					{
						edge2._Org._anEdge = edge2._Onext;
						MeshUtils.Splice(edge2, edge2._Oprev);
					}
					MeshUtils.Edge sym = edge2._Sym;
					if (sym._Onext == sym)
					{
						MeshUtils.KillVertex(sym._Org, null);
					}
					else
					{
						sym._Org._anEdge = sym._Onext;
						MeshUtils.Splice(sym, sym._Oprev);
					}
					MeshUtils.KillEdge(edge2);
				}
			}
			while (edge2 != anEdge);
			MeshUtils.Face prev = fZap._prev;
			MeshUtils.Face next = fZap._next;
			next._prev = prev;
			prev._next = next;
			fZap.Free();
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x000352B8 File Offset: 0x000334B8
		public void MergeConvexFaces(int maxVertsPerFace)
		{
			for (MeshUtils.Face face = this._fHead._next; face != this._fHead; face = face._next)
			{
				if (face._inside)
				{
					MeshUtils.Edge edge = face._anEdge;
					MeshUtils.Vertex org = edge._Org;
					for (;;)
					{
						MeshUtils.Edge edge2 = edge._Lnext;
						MeshUtils.Edge sym = edge._Sym;
						if (sym != null && sym._Lface != null && sym._Lface._inside)
						{
							int vertsCount = face.VertsCount;
							int vertsCount2 = sym._Lface.VertsCount;
							if (vertsCount + vertsCount2 - 2 <= maxVertsPerFace && Geom.VertCCW(edge._Lprev._Org, edge._Org, sym._Lnext._Lnext._Org) && Geom.VertCCW(sym._Lprev._Org, sym._Org, edge._Lnext._Lnext._Org))
							{
								edge2 = sym._Lnext;
								this.Delete(sym);
								edge = null;
							}
						}
						if (edge != null && edge._Lnext._Org == org)
						{
							break;
						}
						edge = edge2;
					}
				}
			}
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x000353D0 File Offset: 0x000335D0
		[Conditional("DEBUG")]
		public void Check()
		{
			MeshUtils.Face face = this._fHead;
			face = this._fHead;
			MeshUtils.Face next;
			MeshUtils.Edge edge;
			while ((next = face._next) != this._fHead)
			{
				edge = next._anEdge;
				do
				{
					edge = edge._Lnext;
				}
				while (edge != next._anEdge);
				face = next;
			}
			MeshUtils.Vertex vertex = this._vHead;
			vertex = this._vHead;
			MeshUtils.Vertex next2;
			while ((next2 = vertex._next) != this._vHead)
			{
				edge = next2._anEdge;
				do
				{
					edge = edge._Onext;
				}
				while (edge != next2._anEdge);
				vertex = next2;
			}
			MeshUtils.Edge edge2 = this._eHead;
			edge2 = this._eHead;
			while ((edge = edge2._next) != this._eHead)
			{
				edge2 = edge;
			}
		}

		// Token: 0x040007B6 RID: 1974
		internal MeshUtils.Vertex _vHead;

		// Token: 0x040007B7 RID: 1975
		internal MeshUtils.Face _fHead;

		// Token: 0x040007B8 RID: 1976
		internal MeshUtils.Edge _eHead;

		// Token: 0x040007B9 RID: 1977
		internal MeshUtils.Edge _eHeadSym;
	}
}
