using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000121 RID: 289
[Serializable]
public class vTagMask : IList<string>, ICollection<string>, IEnumerable<string>, IEnumerable
{
	// Token: 0x060009D4 RID: 2516 RVA: 0x00045390 File Offset: 0x00043590
	public vTagMask()
	{
		this.tags = new List<string>();
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x000453AE File Offset: 0x000435AE
	public vTagMask(List<string> tags)
	{
		this.tags = tags;
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x000453C8 File Offset: 0x000435C8
	public vTagMask(params string[] arg)
	{
		this.tags = new List<string>(arg);
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x000453E7 File Offset: 0x000435E7
	public bool Contains(string tag)
	{
		return this.tags.Contains(tag);
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x000453F5 File Offset: 0x000435F5
	public void Add(string tag)
	{
		if (!this.tags.Contains(tag))
		{
			this.tags.Add(tag);
		}
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x00045411 File Offset: 0x00043611
	public void Remove(string tag)
	{
		if (this.tags.Contains(tag))
		{
			this.tags.Remove(tag);
		}
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x0004542E File Offset: 0x0004362E
	public void Clear()
	{
		this.tags.Clear();
	}

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x060009DB RID: 2523 RVA: 0x0004543B File Offset: 0x0004363B
	public int Count
	{
		get
		{
			return this.tags.Count;
		}
	}

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x060009DC RID: 2524 RVA: 0x0000D91D File Offset: 0x0000BB1D
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000216 RID: 534
	public string this[int index]
	{
		get
		{
			return this.tags[index];
		}
		set
		{
			if (!this.tags.Contains(value))
			{
				this.tags[index] = value;
			}
		}
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x00045473 File Offset: 0x00043673
	IEnumerator<string> IEnumerable<string>.GetEnumerator()
	{
		return this.tags.GetEnumerator();
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x00045473 File Offset: 0x00043673
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.tags.GetEnumerator();
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x00045485 File Offset: 0x00043685
	public int IndexOf(string item)
	{
		return this.tags.IndexOf(item);
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x00045493 File Offset: 0x00043693
	public void Insert(int index, string item)
	{
		if (!this.tags.Contains(item))
		{
			this.tags.Insert(index, item);
		}
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x000454B0 File Offset: 0x000436B0
	public void RemoveAt(int index)
	{
		if (index >= 0 && index < this.tags.Count)
		{
			this.tags.RemoveAt(index);
		}
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x000454D0 File Offset: 0x000436D0
	public void CopyTo(string[] array, int arrayIndex)
	{
		this.tags.CopyTo(array, arrayIndex);
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x000454DF File Offset: 0x000436DF
	bool ICollection<string>.Remove(string item)
	{
		return this.tags.Contains(item) && this.tags.Remove(item);
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x000454FD File Offset: 0x000436FD
	public static implicit operator List<string>(vTagMask t)
	{
		return t.tags;
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x00045505 File Offset: 0x00043705
	public static implicit operator string[](vTagMask t)
	{
		return t.tags.ToArray();
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x00045512 File Offset: 0x00043712
	public static implicit operator vTagMask(List<string> l)
	{
		return new vTagMask(l);
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x0004551A File Offset: 0x0004371A
	public static implicit operator vTagMask(string[] l)
	{
		return new vTagMask(l);
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x00045524 File Offset: 0x00043724
	public static vTagMask operator +(vTagMask a, vTagMask b)
	{
		for (int i = 0; i < b.tags.Count; i++)
		{
			if (!a.Contains(b.tags[i]))
			{
				a.Add(b.tags[i]);
			}
		}
		return a.tags;
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x00045578 File Offset: 0x00043778
	public static vTagMask operator -(vTagMask a, vTagMask b)
	{
		for (int i = 0; i < b.tags.Count; i++)
		{
			if (a.Contains(b.tags[i]))
			{
				a.Remove(b.tags[i]);
			}
		}
		return a.tags;
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x000455CC File Offset: 0x000437CC
	public static vTagMask operator +(vTagMask a, List<string> b)
	{
		for (int i = 0; i < b.Count; i++)
		{
			if (!a.Contains(b[i]))
			{
				a.Add(b[i]);
			}
		}
		return a.tags;
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x00045614 File Offset: 0x00043814
	public static vTagMask operator -(vTagMask a, List<string> b)
	{
		for (int i = 0; i < b.Count; i++)
		{
			if (a.Contains(b[i]))
			{
				a.Remove(b[i]);
			}
		}
		return a.tags;
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x0004565C File Offset: 0x0004385C
	public static vTagMask operator +(vTagMask a, string[] b)
	{
		for (int i = 0; i < b.Length; i++)
		{
			if (!a.Contains(b[i]))
			{
				a.Add(b[i]);
			}
		}
		return a.tags;
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x00045698 File Offset: 0x00043898
	public static vTagMask operator -(vTagMask a, string[] b)
	{
		for (int i = 0; i < b.Length; i++)
		{
			if (a.Contains(b[i]))
			{
				a.Remove(b[i]);
			}
		}
		return a.tags;
	}

	// Token: 0x0400092E RID: 2350
	[SerializeField]
	private List<string> tags = new List<string>();
}
