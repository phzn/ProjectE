using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;

namespace GPUInstancerPro
{
	// Token: 0x0200001B RID: 27
	public class GPUIDataBuffer<T> : IGPUIDisposable, IDisposable where T : struct
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x000047A4 File Offset: 0x000029A4
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x000047AC File Offset: 0x000029AC
		public GraphicsBuffer Buffer { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x000047B5 File Offset: 0x000029B5
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x000047BD File Offset: 0x000029BD
		public int Length { get; private set; }

		// Token: 0x060000B5 RID: 181 RVA: 0x000047C8 File Offset: 0x000029C8
		public GPUIDataBuffer(string name, int length = 0, GraphicsBuffer.Target target = GraphicsBuffer.Target.Structured)
		{
			if (length < 0)
			{
				length = 0;
			}
			this.Length = length;
			this._name = name;
			this._target = target;
			if (length > 0)
			{
				this._data = new NativeArray<T>(length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this._requireUpdate = true;
			}
			this._stride = Marshal.SizeOf(typeof(T));
			this._requestCallbackInternal = new Action<AsyncGPUReadbackRequest>(this.OnDataRequestCompleted);
			this._requestCallbacksExternal = new List<Action<GPUIDataBuffer<T>>>();
		}

		// Token: 0x17000012 RID: 18
		public T this[int index]
		{
			get
			{
				return this._data[index];
			}
			set
			{
				if (this._isDataRequested && this._writeToDataAfterReadback)
				{
					this._readbackRequest.WaitForCompletion();
				}
				this._data[index] = value;
				this._requireUpdate = true;
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004884 File Offset: 0x00002A84
		public void Add(T element)
		{
			if (this._isDataRequested && this._writeToDataAfterReadback)
			{
				this._readbackRequest.WaitForCompletion();
			}
			int length = this.Length;
			int length2 = this.Length;
			this.Length = length2 + 1;
			(ref this._data).ResizeNativeArray<T>(this.Length, Allocator.Persistent);
			this._data[length] = element;
			this._requireUpdate = true;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000048EC File Offset: 0x00002AEC
		public void Add(params T[] elements)
		{
			if (this._isDataRequested && this._writeToDataAfterReadback)
			{
				this._readbackRequest.WaitForCompletion();
			}
			int length = this.Length;
			this.Length += elements.Length;
			(ref this._data).ResizeNativeArray<T>(this.Length, Allocator.Persistent);
			for (int i = 0; i < elements.Length; i++)
			{
				this._data[length + i] = elements[i];
			}
			this._requireUpdate = true;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004968 File Offset: 0x00002B68
		public void Add(List<T> elements)
		{
			if (this._isDataRequested && this._writeToDataAfterReadback)
			{
				this._readbackRequest.WaitForCompletion();
			}
			int length = this.Length;
			this.Length += elements.Count;
			(ref this._data).ResizeNativeArray<T>(this.Length, Allocator.Persistent);
			for (int i = 0; i < elements.Count; i++)
			{
				this._data[length + i] = elements[i];
			}
			this._requireUpdate = true;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000049EC File Offset: 0x00002BEC
		public void AddOrSet(int index, T element)
		{
			if (this._isDataRequested && this._writeToDataAfterReadback)
			{
				this._readbackRequest.WaitForCompletion();
			}
			if (!this._data.IsCreated)
			{
				this.Length = index + 1;
				this._data = new NativeArray<T>(this.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
			else if (index >= this.Length)
			{
				this.Length = index + 1;
				(ref this._data).ResizeNativeArray<T>(this.Length, Allocator.Persistent);
			}
			this._data[index] = element;
			this._requireUpdate = true;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004A77 File Offset: 0x00002C77
		public void SetData(int bufferStartIndex, int arrayStartIndex, int count, T[] array)
		{
			if (count <= 0)
			{
				return;
			}
			if (this._isDataRequested && this._writeToDataAfterReadback)
			{
				this._readbackRequest.WaitForCompletion();
			}
			NativeArray<T>.Copy(array, arrayStartIndex, this._data, bufferStartIndex, count);
			this._requireUpdate = true;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004AB0 File Offset: 0x00002CB0
		public void SetData(int bufferStartIndex, int count, T newValue)
		{
			if (this._isDataRequested && this._writeToDataAfterReadback)
			{
				this._readbackRequest.WaitForCompletion();
			}
			int num = count + bufferStartIndex;
			for (int i = bufferStartIndex; i < num; i++)
			{
				this._data[i] = newValue;
			}
			this._requireUpdate = true;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004AFC File Offset: 0x00002CFC
		public void Resize(int newSize)
		{
			if (this._isDataRequested && this._writeToDataAfterReadback)
			{
				this._readbackRequest.WaitForCompletion();
			}
			this.Length = newSize;
			if (newSize <= 0)
			{
				this.ReleaseBuffers();
				return;
			}
			(ref this._data).ResizeNativeArray<T>(this.Length, Allocator.Persistent);
			this._requireUpdate = true;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004B4F File Offset: 0x00002D4F
		public void ResizeWithoutCopy(int newSize)
		{
			this.ReleaseBuffers();
			this.Length = newSize;
			this._data = new NativeArray<T>(newSize, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this._requireUpdate = true;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004B73 File Offset: 0x00002D73
		public bool UpdateBufferData(bool forceUpdate = false)
		{
			if (forceUpdate || this._requireUpdate)
			{
				this.SetBufferData(0, 0);
				return true;
			}
			return false;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004B8C File Offset: 0x00002D8C
		public void SetBufferData(int startIndex = 0, int count = 0)
		{
			int length = this.Length;
			if (startIndex >= length || startIndex < 0)
			{
				return;
			}
			if (count <= 0 || count > length)
			{
				count = length;
			}
			if (this._isDataRequested)
			{
				this._readbackRequest.WaitForCompletion();
			}
			this._requireUpdate = false;
			if (length == 0 || !this._data.IsCreated)
			{
				this.ReleaseBuffers();
				return;
			}
			if (this.Buffer != null && this.Buffer.count != length)
			{
				this.Buffer.Release();
				this.Buffer = null;
			}
			if (this.Buffer == null)
			{
				this.Buffer = new GraphicsBuffer(this._target, length, this._stride);
			}
			this.SetBufferDataInternal(startIndex, count);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004C35 File Offset: 0x00002E35
		protected virtual void SetBufferDataInternal(int startIndex, int count)
		{
			this.Buffer.SetData<T>(this._data, startIndex, startIndex, count);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004C4C File Offset: 0x00002E4C
		public virtual void ReleaseBuffers()
		{
			if (this._isDataRequested)
			{
				this._isRequestedDataInvalid = true;
				this._readbackRequest.WaitForCompletion();
			}
			if (this.Buffer != null)
			{
				this.Buffer.Release();
				this.Buffer = null;
			}
			if (this._data.IsCreated)
			{
				this._data.Dispose();
			}
			this.Length = 0;
			if (!this._isDataRequested && this._requestedData.IsCreated)
			{
				this._requestedData.Dispose();
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004CCC File Offset: 0x00002ECC
		public void Dispose()
		{
			this.ReleaseBuffers();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004CD4 File Offset: 0x00002ED4
		public T[] GetBufferData()
		{
			this.UpdateBufferData(false);
			T[] array = new T[this.Length];
			if (this.Buffer != null)
			{
				this.Buffer.GetData(array);
			}
			this._data.CopyFrom(array);
			return array;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004D16 File Offset: 0x00002F16
		public NativeArray<T> GetNativeData(bool isReadFromGPU = false)
		{
			if (isReadFromGPU)
			{
				this.AsyncDataRequest(null, true);
				if (this._isDataRequested)
				{
					this._readbackRequest.WaitForCompletion();
				}
			}
			return this._data;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004D40 File Offset: 0x00002F40
		public bool AsyncDataRequest(Action<GPUIDataBuffer<T>> callback, bool writeToDataAfterReadback)
		{
			if (this.Buffer == null)
			{
				return false;
			}
			if (!this._isDataRequested)
			{
				this._writeToDataAfterReadback = writeToDataAfterReadback;
				if (!this._requestedData.IsCreated || this._requestedData.Length != this.Length)
				{
					if (this._requestedData.IsCreated)
					{
						this._requestedData.Dispose();
					}
					this._requestedData = new NativeArray<T>(this.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				}
				this._readbackRequest = AsyncGPUReadback.RequestIntoNativeArray<T>(ref this._requestedData, this.Buffer, this._requestCallbackInternal);
				this._isDataRequested = true;
			}
			else if (writeToDataAfterReadback != this._writeToDataAfterReadback)
			{
				Debug.LogWarning(GPUIConstants.LOG_PREFIX + "There is another AsyncDataRequest pending results.");
				return false;
			}
			this._requestCallbacksExternal.Add(callback);
			return true;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004E04 File Offset: 0x00003004
		private void OnDataRequestCompleted(AsyncGPUReadbackRequest obj)
		{
			this._isDataRequested = false;
			if (obj.hasError)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Async data request has encountered an error. Data buffer name: " + this._name);
				return;
			}
			if (this._isRequestedDataInvalid)
			{
				this._isRequestedDataInvalid = false;
				if (this._requestedData.IsCreated)
				{
					this._requestedData.Dispose();
				}
				return;
			}
			if (this._writeToDataAfterReadback)
			{
				if (!this._data.IsCreated)
				{
					return;
				}
				if (this._data.Length != this._requestedData.Length)
				{
					if (Application.isPlaying)
					{
						Debug.LogError(string.Concat(new string[]
						{
							GPUIConstants.LOG_PREFIX,
							"GPUI async data request size mismatch. Data length: ",
							this._data.Length.ToString(),
							" requested length: ",
							this._requestedData.Length.ToString(),
							" name: ",
							this._name
						}));
					}
					return;
				}
				this._data.CopyFrom(this._requestedData);
			}
			foreach (Action<GPUIDataBuffer<T>> action in this._requestCallbacksExternal)
			{
				if (action != null)
				{
					action(this);
				}
			}
			this._requestCallbacksExternal.Clear();
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004F68 File Offset: 0x00003168
		public bool IsDataRequested()
		{
			return this._isDataRequested;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004F70 File Offset: 0x00003170
		public NativeArray<T> GetRequestedData()
		{
			if (this._isDataRequested)
			{
				return default(NativeArray<T>);
			}
			return this._requestedData;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004F95 File Offset: 0x00003195
		public void WaitForReadbackCompletion()
		{
			if (this._isDataRequested)
			{
				this._readbackRequest.WaitForCompletion();
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004FAA File Offset: 0x000031AA
		public void RequireUpdate()
		{
			this._requireUpdate = true;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004FB3 File Offset: 0x000031B3
		public unsafe void* GetUnsafeNativeArrayPtr()
		{
			if (this._isDataRequested && this._writeToDataAfterReadback)
			{
				this._readbackRequest.WaitForCompletion();
			}
			this._requireUpdate = true;
			return this._data.GetUnsafePtr<T>();
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004FE2 File Offset: 0x000031E2
		public unsafe void* GetUnsafeReadOnlyNativeArrayPtr()
		{
			if (this._isDataRequested && this._writeToDataAfterReadback)
			{
				this._readbackRequest.WaitForCompletion();
			}
			return this._data.GetUnsafeReadOnlyPtr<T>();
		}

		// Token: 0x04000062 RID: 98
		private readonly string _name;

		// Token: 0x04000063 RID: 99
		private readonly GraphicsBuffer.Target _target;

		// Token: 0x04000064 RID: 100
		private readonly int _stride;

		// Token: 0x04000065 RID: 101
		protected NativeArray<T> _data;

		// Token: 0x04000066 RID: 102
		protected bool _requireUpdate;

		// Token: 0x04000067 RID: 103
		private bool _isDataRequested;

		// Token: 0x04000068 RID: 104
		private bool _isRequestedDataInvalid;

		// Token: 0x04000069 RID: 105
		private AsyncGPUReadbackRequest _readbackRequest;

		// Token: 0x0400006A RID: 106
		private readonly Action<AsyncGPUReadbackRequest> _requestCallbackInternal;

		// Token: 0x0400006B RID: 107
		private List<Action<GPUIDataBuffer<T>>> _requestCallbacksExternal;

		// Token: 0x0400006C RID: 108
		private NativeArray<T> _requestedData;

		// Token: 0x0400006D RID: 109
		private bool _writeToDataAfterReadback;
	}
}
