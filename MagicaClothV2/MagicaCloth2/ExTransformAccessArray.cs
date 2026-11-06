using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

namespace MagicaCloth2
{
	// Token: 0x02000113 RID: 275
	public class ExTransformAccessArray : IDisposable
	{
		// Token: 0x06000586 RID: 1414 RVA: 0x0002F244 File Offset: 0x0002D444
		public ExTransformAccessArray(int capacity, int desiredJobCount = -1)
		{
			this.transformArray = new TransformAccessArray(capacity, desiredJobCount);
			this.nativeLength = this.transformArray.length;
			this.emptyStack = new Queue<int>(capacity);
			this.useIndexDict = new Dictionary<int, int>(capacity);
			this.indexDict = new Dictionary<int, int>(capacity);
			this.referenceDict = new Dictionary<int, int>(capacity);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0002F2A8 File Offset: 0x0002D4A8
		public void Dispose()
		{
			if (this.transformArray.isCreated)
			{
				this.transformArray.Dispose();
			}
			this.emptyStack.Clear();
			this.useIndexDict.Clear();
			this.indexDict.Clear();
			this.referenceDict.Clear();
			this.nativeLength = 0;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0002F300 File Offset: 0x0002D500
		public TransformAccessArray GetTransformAccessArray()
		{
			return this.transformArray;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0002F308 File Offset: 0x0002D508
		public int Add(Transform element)
		{
			int instanceID = element.GetInstanceID();
			if (this.referenceDict.ContainsKey(instanceID))
			{
				this.referenceDict[instanceID] = this.referenceDict[instanceID] + 1;
				return this.indexDict[instanceID];
			}
			int num;
			if (this.emptyStack.Count > 0)
			{
				num = this.emptyStack.Dequeue();
				this.transformArray[num] = element;
			}
			else
			{
				num = this.transformArray.length;
				this.transformArray.Add(element);
			}
			this.useIndexDict.Add(num, instanceID);
			this.indexDict.Add(instanceID, num);
			this.referenceDict.Add(instanceID, 1);
			this.nativeLength = this.transformArray.length;
			return num;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0002F3D0 File Offset: 0x0002D5D0
		public void Remove(int index)
		{
			if (this.useIndexDict.ContainsKey(index))
			{
				int num = this.useIndexDict[index];
				int num2 = this.referenceDict[num] - 1;
				if (num2 > 0)
				{
					this.referenceDict[num] = num2;
					return;
				}
				this.transformArray[index] = null;
				this.emptyStack.Enqueue(index);
				this.useIndexDict.Remove(index);
				this.indexDict.Remove(num);
				this.referenceDict.Remove(num);
				this.nativeLength = this.transformArray.length;
			}
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0002F46A File Offset: 0x0002D66A
		public bool Exist(int index)
		{
			return this.useIndexDict.ContainsKey(index);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0002F478 File Offset: 0x0002D678
		public bool Exist(Transform element)
		{
			return !(element == null) && this.indexDict.ContainsKey(element.GetInstanceID());
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x0002F496 File Offset: 0x0002D696
		public int Count
		{
			get
			{
				return this.useIndexDict.Count;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x0002F4A3 File Offset: 0x0002D6A3
		public int Length
		{
			get
			{
				return this.nativeLength;
			}
		}

		// Token: 0x17000097 RID: 151
		public Transform this[int index]
		{
			get
			{
				return this.transformArray[index];
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0002F4BC File Offset: 0x0002D6BC
		public int GetIndex(Transform element)
		{
			if (element == null)
			{
				return -1;
			}
			int instanceID = element.GetInstanceID();
			if (this.indexDict.ContainsKey(instanceID))
			{
				return this.indexDict[instanceID];
			}
			return -1;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0002F4F8 File Offset: 0x0002D6F8
		public void Clear()
		{
			foreach (int num in this.useIndexDict.Keys)
			{
				this.emptyStack.Enqueue(num);
			}
			this.useIndexDict.Clear();
			int i = 0;
			int length = this.Length;
			while (i < length)
			{
				this.transformArray[i] = null;
				i++;
			}
			this.indexDict.Clear();
			this.referenceDict.Clear();
			this.nativeLength = 0;
		}

		// Token: 0x040008AE RID: 2222
		private TransformAccessArray transformArray;

		// Token: 0x040008AF RID: 2223
		private int nativeLength;

		// Token: 0x040008B0 RID: 2224
		private Queue<int> emptyStack;

		// Token: 0x040008B1 RID: 2225
		private Dictionary<int, int> useIndexDict;

		// Token: 0x040008B2 RID: 2226
		private Dictionary<int, int> indexDict;

		// Token: 0x040008B3 RID: 2227
		private Dictionary<int, int> referenceDict;
	}
}
