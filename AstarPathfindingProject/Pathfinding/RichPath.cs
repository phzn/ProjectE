using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200001F RID: 31
	public class RichPath
	{
		// Token: 0x06000205 RID: 517 RVA: 0x00009162 File Offset: 0x00007362
		public RichPath()
		{
			this.Clear();
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000917B File Offset: 0x0000737B
		public void Clear()
		{
			this.parts.Clear();
			this.currentPart = 0;
			this.Endpoint = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x000091AC File Offset: 0x000073AC
		public void Initialize(Seeker seeker, Path path, bool mergePartEndpoints, bool simplificationMode)
		{
			if (path.error)
			{
				throw new ArgumentException("Path has an error");
			}
			List<GraphNode> path2 = path.path;
			if (path2.Count == 0)
			{
				throw new ArgumentException("Path traverses no nodes");
			}
			this.seeker = seeker;
			for (int i = 0; i < this.parts.Count; i++)
			{
				RichFunnel richFunnel = this.parts[i] as RichFunnel;
				RichSpecial richSpecial = this.parts[i] as RichSpecial;
				if (richFunnel != null)
				{
					ObjectPool<RichFunnel>.Release(ref richFunnel);
				}
				else if (richSpecial != null)
				{
					ObjectPool<RichSpecial>.Release(ref richSpecial);
				}
			}
			this.Clear();
			this.Endpoint = path.vectorPath[path.vectorPath.Count - 1];
			for (int j = 0; j < path2.Count; j++)
			{
				if (path2[j] is TriangleMeshNode)
				{
					NavmeshBase navmeshBase = AstarData.GetGraph(path2[j]) as NavmeshBase;
					if (navmeshBase == null)
					{
						throw new Exception("Found a TriangleMeshNode that was not in a NavmeshBase graph");
					}
					RichFunnel richFunnel2 = ObjectPool<RichFunnel>.Claim().Initialize(this, navmeshBase);
					richFunnel2.funnelSimplification = simplificationMode;
					int num = j;
					uint graphIndex = path2[num].GraphIndex;
					while (j < path2.Count && (path2[j].GraphIndex == graphIndex || path2[j] is NodeLink3Node))
					{
						j++;
					}
					j--;
					if (num == 0)
					{
						richFunnel2.exactStart = path.vectorPath[0];
					}
					else
					{
						richFunnel2.exactStart = (Vector3)path2[mergePartEndpoints ? (num - 1) : num].position;
					}
					if (j == path2.Count - 1)
					{
						richFunnel2.exactEnd = path.vectorPath[path.vectorPath.Count - 1];
					}
					else
					{
						richFunnel2.exactEnd = (Vector3)path2[mergePartEndpoints ? (j + 1) : j].position;
					}
					richFunnel2.BuildFunnelCorridor(path2, num, j);
					this.parts.Add(richFunnel2);
				}
				else
				{
					LinkNode linkNode = path2[j] as LinkNode;
					if (linkNode != null)
					{
						int num2 = j;
						uint graphIndex2 = path2[num2].GraphIndex;
						while (j < path2.Count && path2[j].GraphIndex == graphIndex2)
						{
							j++;
						}
						j--;
						if (j - num2 > 1)
						{
							throw new Exception("NodeLink2 path length greater than two (2) nodes. " + (j - num2).ToString());
						}
						if (j - num2 != 0)
						{
							RichSpecial richSpecial2 = ObjectPool<RichSpecial>.Claim().Initialize(linkNode.linkConcrete.GetTracer(linkNode));
							this.parts.Add(richSpecial2);
						}
					}
					else if (!(path2[j] is PointNode))
					{
						throw new InvalidOperationException("The RichAI movment script can only be used on recast/navmesh graphs. A node of type " + path2[j].GetType().Name + " was in the path.");
					}
				}
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000949A File Offset: 0x0000769A
		// (set) Token: 0x06000209 RID: 521 RVA: 0x000094A2 File Offset: 0x000076A2
		public Vector3 Endpoint { get; private set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600020A RID: 522 RVA: 0x000094AB File Offset: 0x000076AB
		public bool CompletedAllParts
		{
			get
			{
				return this.currentPart >= this.parts.Count;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600020B RID: 523 RVA: 0x000094C3 File Offset: 0x000076C3
		public bool IsLastPart
		{
			get
			{
				return this.currentPart >= this.parts.Count - 1;
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x000094DD File Offset: 0x000076DD
		public void NextPart()
		{
			this.currentPart = Mathf.Min(this.currentPart + 1, this.parts.Count);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00009500 File Offset: 0x00007700
		public RichPathPart GetCurrentPart()
		{
			if (this.parts.Count == 0)
			{
				return null;
			}
			if (this.currentPart >= this.parts.Count)
			{
				return this.parts[this.parts.Count - 1];
			}
			return this.parts[this.currentPart];
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000955C File Offset: 0x0000775C
		public void GetRemainingPath(List<Vector3> buffer, List<PathPartWithLinkInfo> partsBuffer, Vector3 currentPosition, out bool requiresRepath)
		{
			buffer.Clear();
			buffer.Add(currentPosition);
			requiresRepath = false;
			for (int i = this.currentPart; i < this.parts.Count; i++)
			{
				RichPathPart richPathPart = this.parts[i];
				RichFunnel richFunnel = richPathPart as RichFunnel;
				if (richFunnel != null)
				{
					int count = buffer.Count;
					if (i != 0)
					{
						buffer.Add(richFunnel.exactStart);
					}
					bool flag;
					richFunnel.Update((i == 0) ? currentPosition : richFunnel.exactStart, buffer, int.MaxValue, out flag, out requiresRepath);
					if (partsBuffer != null)
					{
						partsBuffer.Add(new PathPartWithLinkInfo(count, buffer.Count - 1, default(OffMeshLinks.OffMeshLinkTracer)));
					}
					if (requiresRepath)
					{
						return;
					}
				}
				else
				{
					RichSpecial richSpecial = richPathPart as RichSpecial;
					if (richSpecial != null && partsBuffer != null)
					{
						partsBuffer.Add(new PathPartWithLinkInfo(buffer.Count - 1, buffer.Count, richSpecial.nodeLink));
					}
				}
			}
		}

		// Token: 0x0400010D RID: 269
		private int currentPart;

		// Token: 0x0400010E RID: 270
		private readonly List<RichPathPart> parts = new List<RichPathPart>();

		// Token: 0x0400010F RID: 271
		public Seeker seeker;

		// Token: 0x04000110 RID: 272
		public ITransform transform;
	}
}
