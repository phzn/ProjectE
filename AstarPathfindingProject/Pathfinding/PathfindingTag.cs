using System;

namespace Pathfinding
{
	// Token: 0x02000092 RID: 146
	[Serializable]
	public struct PathfindingTag
	{
		// Token: 0x060004E2 RID: 1250 RVA: 0x00019149 File Offset: 0x00017349
		public PathfindingTag(uint value)
		{
			this.value = value;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00019152 File Offset: 0x00017352
		public static implicit operator uint(PathfindingTag tag)
		{
			return tag.value;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001915A File Offset: 0x0001735A
		public static implicit operator PathfindingTag(uint tag)
		{
			return new PathfindingTag(tag);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00019164 File Offset: 0x00017364
		public static PathfindingTag FromName(string tagName)
		{
			AstarPath.FindAstarPath();
			if (AstarPath.active == null)
			{
				throw new InvalidOperationException("There's no AstarPath component in the scene. Cannot get tag names.");
			}
			int num = Array.IndexOf<string>(AstarPath.active.GetTagNames(), tagName);
			if (num == -1)
			{
				throw new ArgumentException("There's no pathfinding tag with the name '" + tagName + "'");
			}
			return new PathfindingTag((uint)num);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000191BD File Offset: 0x000173BD
		public int ToMask()
		{
			return 1 << (int)this.value;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x000191CA File Offset: 0x000173CA
		public override string ToString()
		{
			return this.value.ToString();
		}

		// Token: 0x04000327 RID: 807
		public uint value;
	}
}
