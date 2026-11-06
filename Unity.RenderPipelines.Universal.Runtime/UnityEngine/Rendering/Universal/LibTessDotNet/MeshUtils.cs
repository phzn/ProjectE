using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal.LibTessDotNet
{
	// Token: 0x0200010C RID: 268
	internal static class MeshUtils
	{
		// Token: 0x06000876 RID: 2166 RVA: 0x00035644 File Offset: 0x00033844
		public static MeshUtils.Edge MakeEdge(MeshUtils.Edge eNext)
		{
			MeshUtils.EdgePair edgePair = MeshUtils.EdgePair.Create();
			MeshUtils.Edge e = edgePair._e;
			MeshUtils.Edge eSym = edgePair._eSym;
			MeshUtils.Edge.EnsureFirst(ref eNext);
			MeshUtils.Edge next = eNext._Sym._next;
			eSym._next = next;
			next._Sym._next = e;
			e._next = eNext;
			eNext._Sym._next = eSym;
			e._Sym = eSym;
			e._Onext = e;
			e._Lnext = eSym;
			e._Org = null;
			e._Lface = null;
			e._winding = 0;
			e._activeRegion = null;
			eSym._Sym = e;
			eSym._Onext = eSym;
			eSym._Lnext = e;
			eSym._Org = null;
			eSym._Lface = null;
			eSym._winding = 0;
			eSym._activeRegion = null;
			return e;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00035700 File Offset: 0x00033900
		public static void Splice(MeshUtils.Edge a, MeshUtils.Edge b)
		{
			MeshUtils.Edge onext = a._Onext;
			MeshUtils.Edge onext2 = b._Onext;
			onext._Sym._Lnext = b;
			onext2._Sym._Lnext = a;
			a._Onext = onext2;
			b._Onext = onext;
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00035744 File Offset: 0x00033944
		public static void MakeVertex(MeshUtils.Edge eOrig, MeshUtils.Vertex vNext)
		{
			MeshUtils.Vertex vertex = MeshUtils.Pooled<MeshUtils.Vertex>.Create();
			MeshUtils.Vertex prev = vNext._prev;
			vertex._prev = prev;
			prev._next = vertex;
			vertex._next = vNext;
			vNext._prev = vertex;
			vertex._anEdge = eOrig;
			MeshUtils.Edge edge = eOrig;
			do
			{
				edge._Org = vertex;
				edge = edge._Onext;
			}
			while (edge != eOrig);
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00035798 File Offset: 0x00033998
		public static void MakeFace(MeshUtils.Edge eOrig, MeshUtils.Face fNext)
		{
			MeshUtils.Face face = MeshUtils.Pooled<MeshUtils.Face>.Create();
			MeshUtils.Face prev = fNext._prev;
			face._prev = prev;
			prev._next = face;
			face._next = fNext;
			fNext._prev = face;
			face._anEdge = eOrig;
			face._trail = null;
			face._marked = false;
			face._inside = fNext._inside;
			MeshUtils.Edge edge = eOrig;
			do
			{
				edge._Lface = face;
				edge = edge._Lnext;
			}
			while (edge != eOrig);
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00035804 File Offset: 0x00033A04
		public static void KillEdge(MeshUtils.Edge eDel)
		{
			MeshUtils.Edge.EnsureFirst(ref eDel);
			MeshUtils.Edge next = eDel._next;
			MeshUtils.Edge next2 = eDel._Sym._next;
			next._Sym._next = next2;
			next2._Sym._next = next;
			eDel.Free();
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0003584C File Offset: 0x00033A4C
		public static void KillVertex(MeshUtils.Vertex vDel, MeshUtils.Vertex newOrg)
		{
			MeshUtils.Edge anEdge = vDel._anEdge;
			MeshUtils.Edge edge = anEdge;
			do
			{
				edge._Org = newOrg;
				edge = edge._Onext;
			}
			while (edge != anEdge);
			MeshUtils.Vertex prev = vDel._prev;
			MeshUtils.Vertex next = vDel._next;
			next._prev = prev;
			prev._next = next;
			vDel.Free();
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00035898 File Offset: 0x00033A98
		public static void KillFace(MeshUtils.Face fDel, MeshUtils.Face newLFace)
		{
			MeshUtils.Edge anEdge = fDel._anEdge;
			MeshUtils.Edge edge = anEdge;
			do
			{
				edge._Lface = newLFace;
				edge = edge._Lnext;
			}
			while (edge != anEdge);
			MeshUtils.Face prev = fDel._prev;
			MeshUtils.Face next = fDel._next;
			next._prev = prev;
			prev._next = next;
			fDel.Free();
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x000358E4 File Offset: 0x00033AE4
		public static float FaceArea(MeshUtils.Face f)
		{
			float num = 0f;
			MeshUtils.Edge edge = f._anEdge;
			do
			{
				num += (edge._Org._s - edge._Dst._s) * (edge._Org._t + edge._Dst._t);
				edge = edge._Lnext;
			}
			while (edge != f._anEdge);
			return num;
		}

		// Token: 0x040007BE RID: 1982
		public const int Undef = -1;

		// Token: 0x020001CD RID: 461
		public abstract class Pooled<T> where T : MeshUtils.Pooled<T>, new()
		{
			// Token: 0x06000B53 RID: 2899
			public abstract void Reset();

			// Token: 0x06000B54 RID: 2900 RVA: 0x0004BC48 File Offset: 0x00049E48
			public virtual void OnFree()
			{
			}

			// Token: 0x06000B55 RID: 2901 RVA: 0x0004BC4A File Offset: 0x00049E4A
			public static T Create()
			{
				if (MeshUtils.Pooled<T>._stack != null && MeshUtils.Pooled<T>._stack.Count > 0)
				{
					return MeshUtils.Pooled<T>._stack.Pop();
				}
				return new T();
			}

			// Token: 0x06000B56 RID: 2902 RVA: 0x0004BC70 File Offset: 0x00049E70
			public void Free()
			{
				this.OnFree();
				this.Reset();
				if (MeshUtils.Pooled<T>._stack == null)
				{
					MeshUtils.Pooled<T>._stack = new Stack<T>();
				}
				MeshUtils.Pooled<T>._stack.Push((T)((object)this));
			}

			// Token: 0x04000BE7 RID: 3047
			private static Stack<T> _stack;
		}

		// Token: 0x020001CE RID: 462
		public class Vertex : MeshUtils.Pooled<MeshUtils.Vertex>
		{
			// Token: 0x06000B58 RID: 2904 RVA: 0x0004BCA8 File Offset: 0x00049EA8
			public override void Reset()
			{
				this._prev = (this._next = null);
				this._anEdge = null;
				this._coords = Vec3.Zero;
				this._s = 0f;
				this._t = 0f;
				this._pqHandle = default(PQHandle);
				this._n = 0;
				this._data = null;
			}

			// Token: 0x04000BE8 RID: 3048
			internal MeshUtils.Vertex _prev;

			// Token: 0x04000BE9 RID: 3049
			internal MeshUtils.Vertex _next;

			// Token: 0x04000BEA RID: 3050
			internal MeshUtils.Edge _anEdge;

			// Token: 0x04000BEB RID: 3051
			internal Vec3 _coords;

			// Token: 0x04000BEC RID: 3052
			internal float _s;

			// Token: 0x04000BED RID: 3053
			internal float _t;

			// Token: 0x04000BEE RID: 3054
			internal PQHandle _pqHandle;

			// Token: 0x04000BEF RID: 3055
			internal int _n;

			// Token: 0x04000BF0 RID: 3056
			internal object _data;
		}

		// Token: 0x020001CF RID: 463
		public class Face : MeshUtils.Pooled<MeshUtils.Face>
		{
			// Token: 0x17000262 RID: 610
			// (get) Token: 0x06000B5A RID: 2906 RVA: 0x0004BD10 File Offset: 0x00049F10
			internal int VertsCount
			{
				get
				{
					int num = 0;
					MeshUtils.Edge edge = this._anEdge;
					do
					{
						num++;
						edge = edge._Lnext;
					}
					while (edge != this._anEdge);
					return num;
				}
			}

			// Token: 0x06000B5B RID: 2907 RVA: 0x0004BD3C File Offset: 0x00049F3C
			public override void Reset()
			{
				this._prev = (this._next = null);
				this._anEdge = null;
				this._trail = null;
				this._n = 0;
				this._marked = false;
				this._inside = false;
			}

			// Token: 0x04000BF1 RID: 3057
			internal MeshUtils.Face _prev;

			// Token: 0x04000BF2 RID: 3058
			internal MeshUtils.Face _next;

			// Token: 0x04000BF3 RID: 3059
			internal MeshUtils.Edge _anEdge;

			// Token: 0x04000BF4 RID: 3060
			internal MeshUtils.Face _trail;

			// Token: 0x04000BF5 RID: 3061
			internal int _n;

			// Token: 0x04000BF6 RID: 3062
			internal bool _marked;

			// Token: 0x04000BF7 RID: 3063
			internal bool _inside;
		}

		// Token: 0x020001D0 RID: 464
		public struct EdgePair
		{
			// Token: 0x06000B5D RID: 2909 RVA: 0x0004BD84 File Offset: 0x00049F84
			public static MeshUtils.EdgePair Create()
			{
				MeshUtils.EdgePair edgePair = default(MeshUtils.EdgePair);
				edgePair._e = MeshUtils.Pooled<MeshUtils.Edge>.Create();
				edgePair._e._pair = edgePair;
				edgePair._eSym = MeshUtils.Pooled<MeshUtils.Edge>.Create();
				edgePair._eSym._pair = edgePair;
				return edgePair;
			}

			// Token: 0x06000B5E RID: 2910 RVA: 0x0004BDCC File Offset: 0x00049FCC
			public void Reset()
			{
				this._e = (this._eSym = null);
			}

			// Token: 0x04000BF8 RID: 3064
			internal MeshUtils.Edge _e;

			// Token: 0x04000BF9 RID: 3065
			internal MeshUtils.Edge _eSym;
		}

		// Token: 0x020001D1 RID: 465
		public class Edge : MeshUtils.Pooled<MeshUtils.Edge>
		{
			// Token: 0x17000263 RID: 611
			// (get) Token: 0x06000B5F RID: 2911 RVA: 0x0004BDE9 File Offset: 0x00049FE9
			// (set) Token: 0x06000B60 RID: 2912 RVA: 0x0004BDF6 File Offset: 0x00049FF6
			internal MeshUtils.Face _Rface
			{
				get
				{
					return this._Sym._Lface;
				}
				set
				{
					this._Sym._Lface = value;
				}
			}

			// Token: 0x17000264 RID: 612
			// (get) Token: 0x06000B61 RID: 2913 RVA: 0x0004BE04 File Offset: 0x0004A004
			// (set) Token: 0x06000B62 RID: 2914 RVA: 0x0004BE11 File Offset: 0x0004A011
			internal MeshUtils.Vertex _Dst
			{
				get
				{
					return this._Sym._Org;
				}
				set
				{
					this._Sym._Org = value;
				}
			}

			// Token: 0x17000265 RID: 613
			// (get) Token: 0x06000B63 RID: 2915 RVA: 0x0004BE1F File Offset: 0x0004A01F
			// (set) Token: 0x06000B64 RID: 2916 RVA: 0x0004BE2C File Offset: 0x0004A02C
			internal MeshUtils.Edge _Oprev
			{
				get
				{
					return this._Sym._Lnext;
				}
				set
				{
					this._Sym._Lnext = value;
				}
			}

			// Token: 0x17000266 RID: 614
			// (get) Token: 0x06000B65 RID: 2917 RVA: 0x0004BE3A File Offset: 0x0004A03A
			// (set) Token: 0x06000B66 RID: 2918 RVA: 0x0004BE47 File Offset: 0x0004A047
			internal MeshUtils.Edge _Lprev
			{
				get
				{
					return this._Onext._Sym;
				}
				set
				{
					this._Onext._Sym = value;
				}
			}

			// Token: 0x17000267 RID: 615
			// (get) Token: 0x06000B67 RID: 2919 RVA: 0x0004BE55 File Offset: 0x0004A055
			// (set) Token: 0x06000B68 RID: 2920 RVA: 0x0004BE62 File Offset: 0x0004A062
			internal MeshUtils.Edge _Dprev
			{
				get
				{
					return this._Lnext._Sym;
				}
				set
				{
					this._Lnext._Sym = value;
				}
			}

			// Token: 0x17000268 RID: 616
			// (get) Token: 0x06000B69 RID: 2921 RVA: 0x0004BE70 File Offset: 0x0004A070
			// (set) Token: 0x06000B6A RID: 2922 RVA: 0x0004BE7D File Offset: 0x0004A07D
			internal MeshUtils.Edge _Rprev
			{
				get
				{
					return this._Sym._Onext;
				}
				set
				{
					this._Sym._Onext = value;
				}
			}

			// Token: 0x17000269 RID: 617
			// (get) Token: 0x06000B6B RID: 2923 RVA: 0x0004BE8B File Offset: 0x0004A08B
			// (set) Token: 0x06000B6C RID: 2924 RVA: 0x0004BE98 File Offset: 0x0004A098
			internal MeshUtils.Edge _Dnext
			{
				get
				{
					return this._Rprev._Sym;
				}
				set
				{
					this._Rprev._Sym = value;
				}
			}

			// Token: 0x1700026A RID: 618
			// (get) Token: 0x06000B6D RID: 2925 RVA: 0x0004BEA6 File Offset: 0x0004A0A6
			// (set) Token: 0x06000B6E RID: 2926 RVA: 0x0004BEB3 File Offset: 0x0004A0B3
			internal MeshUtils.Edge _Rnext
			{
				get
				{
					return this._Oprev._Sym;
				}
				set
				{
					this._Oprev._Sym = value;
				}
			}

			// Token: 0x06000B6F RID: 2927 RVA: 0x0004BEC1 File Offset: 0x0004A0C1
			internal static void EnsureFirst(ref MeshUtils.Edge e)
			{
				if (e == e._pair._eSym)
				{
					e = e._Sym;
				}
			}

			// Token: 0x06000B70 RID: 2928 RVA: 0x0004BEDC File Offset: 0x0004A0DC
			public override void Reset()
			{
				this._pair.Reset();
				this._next = (this._Sym = (this._Onext = (this._Lnext = null)));
				this._Org = null;
				this._Lface = null;
				this._activeRegion = null;
				this._winding = 0;
			}

			// Token: 0x04000BFA RID: 3066
			internal MeshUtils.EdgePair _pair;

			// Token: 0x04000BFB RID: 3067
			internal MeshUtils.Edge _next;

			// Token: 0x04000BFC RID: 3068
			internal MeshUtils.Edge _Sym;

			// Token: 0x04000BFD RID: 3069
			internal MeshUtils.Edge _Onext;

			// Token: 0x04000BFE RID: 3070
			internal MeshUtils.Edge _Lnext;

			// Token: 0x04000BFF RID: 3071
			internal MeshUtils.Vertex _Org;

			// Token: 0x04000C00 RID: 3072
			internal MeshUtils.Face _Lface;

			// Token: 0x04000C01 RID: 3073
			internal Tess.ActiveRegion _activeRegion;

			// Token: 0x04000C02 RID: 3074
			internal int _winding;
		}
	}
}
