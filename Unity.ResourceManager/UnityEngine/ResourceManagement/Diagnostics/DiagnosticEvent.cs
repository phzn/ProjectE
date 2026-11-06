using System;
using System.Text;

namespace UnityEngine.ResourceManagement.Diagnostics
{
	// Token: 0x02000053 RID: 83
	[Serializable]
	public struct DiagnosticEvent
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00007E4B File Offset: 0x0000604B
		public string Graph
		{
			get
			{
				return this.m_Graph;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00007E53 File Offset: 0x00006053
		public int ObjectId
		{
			get
			{
				return this.m_ObjectId;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00007E5B File Offset: 0x0000605B
		public string DisplayName
		{
			get
			{
				return this.m_DisplayName;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00007E63 File Offset: 0x00006063
		public int[] Dependencies
		{
			get
			{
				return this.m_Dependencies;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00007E6B File Offset: 0x0000606B
		public int Stream
		{
			get
			{
				return this.m_Stream;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00007E73 File Offset: 0x00006073
		public int Frame
		{
			get
			{
				return this.m_Frame;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00007E7B File Offset: 0x0000607B
		public int Value
		{
			get
			{
				return this.m_Value;
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00007E83 File Offset: 0x00006083
		public DiagnosticEvent(string graph, string name, int id, int stream, int frame, int value, int[] deps)
		{
			this.m_Graph = graph;
			this.m_DisplayName = name;
			this.m_ObjectId = id;
			this.m_Stream = stream;
			this.m_Frame = frame;
			this.m_Value = value;
			this.m_Dependencies = deps;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00007EBA File Offset: 0x000060BA
		internal byte[] Serialize()
		{
			return Encoding.ASCII.GetBytes(JsonUtility.ToJson(this));
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00007ED6 File Offset: 0x000060D6
		public static DiagnosticEvent Deserialize(byte[] data)
		{
			return JsonUtility.FromJson<DiagnosticEvent>(Encoding.ASCII.GetString(data));
		}

		// Token: 0x040000DA RID: 218
		[SerializeField]
		private string m_Graph;

		// Token: 0x040000DB RID: 219
		[SerializeField]
		private int[] m_Dependencies;

		// Token: 0x040000DC RID: 220
		[SerializeField]
		private int m_ObjectId;

		// Token: 0x040000DD RID: 221
		[SerializeField]
		private string m_DisplayName;

		// Token: 0x040000DE RID: 222
		[SerializeField]
		private int m_Stream;

		// Token: 0x040000DF RID: 223
		[SerializeField]
		private int m_Frame;

		// Token: 0x040000E0 RID: 224
		[SerializeField]
		private int m_Value;
	}
}
