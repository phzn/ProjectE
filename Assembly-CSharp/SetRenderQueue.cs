using System;
using UnityEngine;

// Token: 0x02000147 RID: 327
[AddComponentMenu("Rendering/SetRenderQueue")]
public class SetRenderQueue : MonoBehaviour
{
	// Token: 0x06000A6B RID: 2667 RVA: 0x000473C0 File Offset: 0x000455C0
	protected void Awake()
	{
		Material[] materials = base.GetComponent<Renderer>().materials;
		int num = 0;
		while (num < materials.Length && num < this.m_queues.Length)
		{
			materials[num].renderQueue = this.m_queues[num];
			num++;
		}
	}

	// Token: 0x040009A2 RID: 2466
	[SerializeField]
	protected int[] m_queues = new int[] { 3000 };
}
