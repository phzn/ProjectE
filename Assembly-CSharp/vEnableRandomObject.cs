using System;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class vEnableRandomObject : MonoBehaviour
{
	// Token: 0x060009AD RID: 2477 RVA: 0x000448F6 File Offset: 0x00042AF6
	protected void Awake()
	{
		this.random = new Random(base.GetInstanceID());
		if (this.enableOnStart)
		{
			this.EnableObject();
		}
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x00044918 File Offset: 0x00042B18
	public virtual void EnableObject()
	{
		int num = this.random.Next(0, this.objects.Length);
		for (int i = 0; i < this.objects.Length; i++)
		{
			this.objects[i].SetActive(i == num);
		}
	}

	// Token: 0x04000900 RID: 2304
	public GameObject[] objects;

	// Token: 0x04000901 RID: 2305
	public bool enableOnStart;

	// Token: 0x04000902 RID: 2306
	private Random random;
}
