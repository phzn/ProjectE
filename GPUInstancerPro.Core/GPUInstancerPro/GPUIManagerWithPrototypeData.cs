using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200000B RID: 11
	public abstract class GPUIManagerWithPrototypeData<T> : GPUIManager where T : GPUIPrototypeData, new()
	{
		// Token: 0x0600005C RID: 92 RVA: 0x000030DA File Offset: 0x000012DA
		public override void Initialize()
		{
			base.Initialize();
			this.OnPrototypePropertiesModified();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000030E8 File Offset: 0x000012E8
		protected override bool RegisterRenderer(int prototypeIndex)
		{
			GPUIPrototype gpuiprototype = this._prototypes[prototypeIndex];
			T t = this._prototypeDataArray[prototypeIndex];
			if (t.Initialize(gpuiprototype))
			{
				if (base.RegisterRenderer(prototypeIndex))
				{
					return true;
				}
				t.Dispose();
			}
			return false;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003130 File Offset: 0x00001330
		protected override void DisposeRenderer(int prototypeIndex)
		{
			base.DisposeRenderer(prototypeIndex);
			if (this._prototypeDataArray == null || this._prototypeDataArray.Length <= prototypeIndex)
			{
				return;
			}
			T t = this._prototypeDataArray[prototypeIndex];
			if (t == null || !t.IsInitialized)
			{
				return;
			}
			t.Dispose();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003186 File Offset: 0x00001386
		public override void OnPrototypeEnabledStatusChanged(int prototypeIndex, bool isEnabled)
		{
			base.OnPrototypeEnabledStatusChanged(prototypeIndex, isEnabled);
			this.OnPrototypePropertiesModified();
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003196 File Offset: 0x00001396
		protected override void ClearNullPrototypes()
		{
			base.ClearNullPrototypes();
			if (this._prototypeDataArray == null)
			{
				this._prototypeDataArray = new T[0];
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000031B4 File Offset: 0x000013B4
		protected override void SynchronizeData()
		{
			base.SynchronizeData();
			int num = this._prototypes.Length;
			if (this._prototypeDataArray == null)
			{
				this._prototypeDataArray = new T[num];
			}
			else if (this._prototypeDataArray.Length != num)
			{
				Array.Resize<T>(ref this._prototypeDataArray, num);
			}
			for (int i = 0; i < num; i++)
			{
				if (this._prototypeDataArray[i] == null)
				{
					this._prototypeDataArray[i] = new T();
					this.OnNewPrototypeDataCreated(i);
				}
				this._prototypeDataArray[i].FillRequiredFields();
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000024BC File Offset: 0x000006BC
		protected virtual void OnNewPrototypeDataCreated(int prototypeIndex)
		{
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000324C File Offset: 0x0000144C
		public override void OnPrototypePropertiesModified()
		{
			base.OnPrototypePropertiesModified();
			if (!base.IsInitialized)
			{
				return;
			}
			for (int i = 0; i < this._prototypes.Length; i++)
			{
				if (this._runtimeRenderKeys[i] != 0)
				{
					this._prototypeDataArray[i].SetParameterBufferData();
				}
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000329B File Offset: 0x0000149B
		public override void RemovePrototypeAtIndex(int index)
		{
			this._prototypeDataArray = this._prototypeDataArray.RemoveAtAndReturn<T>(index);
			base.RemovePrototypeAtIndex(index);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000032B6 File Offset: 0x000014B6
		public override void RemoveAllPrototypes()
		{
			base.RemoveAllPrototypes();
			this._prototypeDataArray = new T[0];
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000032CC File Offset: 0x000014CC
		public T GetPrototypeData(int prototypeIndex)
		{
			if (prototypeIndex < 0 || this._prototypeDataArray == null || prototypeIndex >= this._prototypeDataArray.Length)
			{
				return default(T);
			}
			return this._prototypeDataArray[prototypeIndex];
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003306 File Offset: 0x00001506
		public override GPUIPrototypeData GetPrototypeDataGeneric(int prototypeIndex)
		{
			return this.GetPrototypeData(prototypeIndex);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003314 File Offset: 0x00001514
		public T GetPrototypeDataWithRenderKey(int renderKey)
		{
			if (this._runtimeRenderKeys != null)
			{
				for (int i = 0; i < this._runtimeRenderKeys.Length; i++)
				{
					if (this._runtimeRenderKeys[i] == renderKey)
					{
						return this._prototypeDataArray[i];
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003360 File Offset: 0x00001560
		public bool TryGetPrototypeDataWithRenderKey(int renderKey, out T prototypeData)
		{
			prototypeData = default(T);
			if (this._runtimeRenderKeys != null)
			{
				for (int i = 0; i < this._runtimeRenderKeys.Length; i++)
				{
					if (this._runtimeRenderKeys[i] == renderKey)
					{
						prototypeData = this._prototypeDataArray[i];
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04000018 RID: 24
		[SerializeField]
		protected T[] _prototypeDataArray;
	}
}
