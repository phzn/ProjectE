using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;

namespace System.Threading
{
	/// <summary>Provides thread-local storage of data.</summary>
	/// <typeparam name="T">Specifies the type of data stored per-thread.</typeparam>
	// Token: 0x020002C0 RID: 704
	[DebuggerTypeProxy(typeof(SystemThreading_ThreadLocalDebugView<>))]
	[DebuggerDisplay("IsValueCreated={IsValueCreated}, Value={ValueForDebugDisplay}, Count={ValuesCountForDebugDisplay}")]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class ThreadLocal<T> : IDisposable
	{
		/// <summary>Initializes the <see cref="T:System.Threading.ThreadLocal`1" /> instance.</summary>
		// Token: 0x06001EAF RID: 7855 RVA: 0x00071EFF File Offset: 0x000700FF
		public ThreadLocal()
		{
			this.Initialize(null, false);
		}

		/// <summary>Initializes the <see cref="T:System.Threading.ThreadLocal`1" /> instance and specifies whether all values are accessible from any thread.</summary>
		/// <param name="trackAllValues">
		///   <see langword="true" /> to track all values set on the instance and expose them through the <see cref="P:System.Threading.ThreadLocal`1.Values" /> property; <see langword="false" /> otherwise.</param>
		// Token: 0x06001EB0 RID: 7856 RVA: 0x00071F1B File Offset: 0x0007011B
		public ThreadLocal(bool trackAllValues)
		{
			this.Initialize(null, trackAllValues);
		}

		/// <summary>Initializes the <see cref="T:System.Threading.ThreadLocal`1" /> instance with the specified <paramref name="valueFactory" /> function.</summary>
		/// <param name="valueFactory">The  <see cref="T:System.Func`1" /> invoked to produce a lazily-initialized value when an attempt is made to retrieve <see cref="P:System.Threading.ThreadLocal`1.Value" /> without it having been previously initialized.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="valueFactory" /> is a null reference (Nothing in Visual Basic).</exception>
		// Token: 0x06001EB1 RID: 7857 RVA: 0x00071F37 File Offset: 0x00070137
		public ThreadLocal(Func<T> valueFactory)
		{
			if (valueFactory == null)
			{
				throw new ArgumentNullException("valueFactory");
			}
			this.Initialize(valueFactory, false);
		}

		/// <summary>Initializes the <see cref="T:System.Threading.ThreadLocal`1" /> instance with the specified <paramref name="valueFactory" /> function and a flag that indicates whether all values are accessible from any thread.</summary>
		/// <param name="valueFactory">The <see cref="T:System.Func`1" /> invoked to produce a lazily-initialized value when an attempt is made to retrieve <see cref="P:System.Threading.ThreadLocal`1.Value" /> without it having been previously initialized.</param>
		/// <param name="trackAllValues">
		///   <see langword="true" /> to track all values set on the instance and expose them through the <see cref="P:System.Threading.ThreadLocal`1.Values" /> property; <see langword="false" /> otherwise.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="valueFactory" /> is a <see langword="null" /> reference (<see langword="Nothing" /> in Visual Basic).</exception>
		// Token: 0x06001EB2 RID: 7858 RVA: 0x00071F61 File Offset: 0x00070161
		public ThreadLocal(Func<T> valueFactory, bool trackAllValues)
		{
			if (valueFactory == null)
			{
				throw new ArgumentNullException("valueFactory");
			}
			this.Initialize(valueFactory, trackAllValues);
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x00071F8C File Offset: 0x0007018C
		private void Initialize(Func<T> valueFactory, bool trackAllValues)
		{
			this.m_valueFactory = valueFactory;
			this.m_trackAllValues = trackAllValues;
			try
			{
			}
			finally
			{
				this.m_idComplement = ~ThreadLocal<T>.s_idManager.GetId();
				this.m_initialized = true;
			}
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x00071FD4 File Offset: 0x000701D4
		~ThreadLocal()
		{
			this.Dispose(false);
		}

		/// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.ThreadLocal`1" /> class.</summary>
		// Token: 0x06001EB5 RID: 7861 RVA: 0x00072004 File Offset: 0x00070204
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the resources used by this <see cref="T:System.Threading.ThreadLocal`1" /> instance.</summary>
		/// <param name="disposing">A Boolean value that indicates whether this method is being called due to a call to <see cref="M:System.Threading.ThreadLocal`1.Dispose" />.</param>
		// Token: 0x06001EB6 RID: 7862 RVA: 0x00072014 File Offset: 0x00070214
		protected virtual void Dispose(bool disposing)
		{
			ThreadLocal<T>.IdManager idManager = ThreadLocal<T>.s_idManager;
			int num;
			lock (idManager)
			{
				num = ~this.m_idComplement;
				this.m_idComplement = 0;
				if (num < 0 || !this.m_initialized)
				{
					return;
				}
				this.m_initialized = false;
				for (ThreadLocal<T>.LinkedSlot linkedSlot = this.m_linkedSlot.Next; linkedSlot != null; linkedSlot = linkedSlot.Next)
				{
					ThreadLocal<T>.LinkedSlotVolatile[] slotArray = linkedSlot.SlotArray;
					if (slotArray != null)
					{
						linkedSlot.SlotArray = null;
						slotArray[num].Value.Value = default(T);
						slotArray[num].Value = null;
					}
				}
			}
			this.m_linkedSlot = null;
			ThreadLocal<T>.s_idManager.ReturnId(num);
		}

		/// <summary>Creates and returns a string representation of this instance for the current thread.</summary>
		/// <returns>The result of calling <see cref="M:System.Object.ToString" /> on the <see cref="P:System.Threading.ThreadLocal`1.Value" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ThreadLocal`1" /> instance has been disposed.</exception>
		/// <exception cref="T:System.NullReferenceException">The <see cref="P:System.Threading.ThreadLocal`1.Value" /> for the current thread is a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="T:System.InvalidOperationException">The initialization function attempted to reference <see cref="P:System.Threading.ThreadLocal`1.Value" /> recursively.</exception>
		/// <exception cref="T:System.MissingMemberException">No default constructor is provided and no value factory is supplied.</exception>
		// Token: 0x06001EB7 RID: 7863 RVA: 0x000720E8 File Offset: 0x000702E8
		public override string ToString()
		{
			T value = this.Value;
			return value.ToString();
		}

		/// <summary>Gets or sets the value of this instance for the current thread.</summary>
		/// <returns>Returns an instance of the object that this ThreadLocal is responsible for initializing.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ThreadLocal`1" /> instance has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The initialization function attempted to reference <see cref="P:System.Threading.ThreadLocal`1.Value" /> recursively.</exception>
		/// <exception cref="T:System.MissingMemberException">No default constructor is provided and no value factory is supplied.</exception>
		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x0007210C File Offset: 0x0007030C
		// (set) Token: 0x06001EB9 RID: 7865 RVA: 0x00072160 File Offset: 0x00070360
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public T Value
		{
			get
			{
				ThreadLocal<T>.LinkedSlotVolatile[] array = ThreadLocal<T>.ts_slotArray;
				int num = ~this.m_idComplement;
				ThreadLocal<T>.LinkedSlot value;
				if (array != null && num >= 0 && num < array.Length && (value = array[num].Value) != null && this.m_initialized)
				{
					return value.Value;
				}
				return this.GetValueSlow();
			}
			set
			{
				ThreadLocal<T>.LinkedSlotVolatile[] array = ThreadLocal<T>.ts_slotArray;
				int num = ~this.m_idComplement;
				ThreadLocal<T>.LinkedSlot value2;
				if (array != null && num >= 0 && num < array.Length && (value2 = array[num].Value) != null && this.m_initialized)
				{
					value2.Value = value;
					return;
				}
				this.SetValueSlow(value, array);
			}
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x000721B4 File Offset: 0x000703B4
		private T GetValueSlow()
		{
			if (~this.m_idComplement < 0)
			{
				throw new ObjectDisposedException(Environment.GetResourceString("The ThreadLocal object has been disposed."));
			}
			Debugger.NotifyOfCrossThreadDependency();
			T t;
			if (this.m_valueFactory == null)
			{
				t = default(T);
			}
			else
			{
				t = this.m_valueFactory();
				if (this.IsValueCreated)
				{
					throw new InvalidOperationException(Environment.GetResourceString("ValueFactory attempted to access the Value property of this instance."));
				}
			}
			this.Value = t;
			return t;
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x00072220 File Offset: 0x00070420
		private void SetValueSlow(T value, ThreadLocal<T>.LinkedSlotVolatile[] slotArray)
		{
			int num = ~this.m_idComplement;
			if (num < 0)
			{
				throw new ObjectDisposedException(Environment.GetResourceString("The ThreadLocal object has been disposed."));
			}
			if (slotArray == null)
			{
				slotArray = new ThreadLocal<T>.LinkedSlotVolatile[ThreadLocal<T>.GetNewTableSize(num + 1)];
				ThreadLocal<T>.ts_finalizationHelper = new ThreadLocal<T>.FinalizationHelper(slotArray, this.m_trackAllValues);
				ThreadLocal<T>.ts_slotArray = slotArray;
			}
			if (num >= slotArray.Length)
			{
				this.GrowTable(ref slotArray, num + 1);
				ThreadLocal<T>.ts_finalizationHelper.SlotArray = slotArray;
				ThreadLocal<T>.ts_slotArray = slotArray;
			}
			if (slotArray[num].Value == null)
			{
				this.CreateLinkedSlot(slotArray, num, value);
				return;
			}
			ThreadLocal<T>.LinkedSlot value2 = slotArray[num].Value;
			if (!this.m_initialized)
			{
				throw new ObjectDisposedException(Environment.GetResourceString("The ThreadLocal object has been disposed."));
			}
			value2.Value = value;
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x000722DC File Offset: 0x000704DC
		private void CreateLinkedSlot(ThreadLocal<T>.LinkedSlotVolatile[] slotArray, int id, T value)
		{
			ThreadLocal<T>.LinkedSlot linkedSlot = new ThreadLocal<T>.LinkedSlot(slotArray);
			ThreadLocal<T>.IdManager idManager = ThreadLocal<T>.s_idManager;
			lock (idManager)
			{
				if (!this.m_initialized)
				{
					throw new ObjectDisposedException(Environment.GetResourceString("The ThreadLocal object has been disposed."));
				}
				ThreadLocal<T>.LinkedSlot next = this.m_linkedSlot.Next;
				linkedSlot.Next = next;
				linkedSlot.Previous = this.m_linkedSlot;
				linkedSlot.Value = value;
				if (next != null)
				{
					next.Previous = linkedSlot;
				}
				this.m_linkedSlot.Next = linkedSlot;
				slotArray[id].Value = linkedSlot;
			}
		}

		/// <summary>Gets a list for all of the values currently stored by all of the threads that have accessed this instance.</summary>
		/// <returns>A list for all of the values currently stored by all of the threads that have accessed this instance.</returns>
		/// <exception cref="T:System.InvalidOperationException">Values stored by all threads are not available because this instance was initialized with the <paramref name="trackAllValues" /> argument set to <see langword="false" /> in the call to a class constructor.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ThreadLocal`1" /> instance has been disposed.</exception>
		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001EBD RID: 7869 RVA: 0x0007238C File Offset: 0x0007058C
		public IList<T> Values
		{
			get
			{
				if (!this.m_trackAllValues)
				{
					throw new InvalidOperationException(Environment.GetResourceString("The ThreadLocal object is not tracking values. To use the Values property, use a ThreadLocal constructor that accepts the trackAllValues parameter and set the parameter to true."));
				}
				List<T> valuesAsList = this.GetValuesAsList();
				if (valuesAsList == null)
				{
					throw new ObjectDisposedException(Environment.GetResourceString("The ThreadLocal object has been disposed."));
				}
				return valuesAsList;
			}
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x000723C0 File Offset: 0x000705C0
		private List<T> GetValuesAsList()
		{
			List<T> list = new List<T>();
			if (~this.m_idComplement == -1)
			{
				return null;
			}
			for (ThreadLocal<T>.LinkedSlot linkedSlot = this.m_linkedSlot.Next; linkedSlot != null; linkedSlot = linkedSlot.Next)
			{
				list.Add(linkedSlot.Value);
			}
			return list;
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001EBF RID: 7871 RVA: 0x00072408 File Offset: 0x00070608
		private int ValuesCountForDebugDisplay
		{
			get
			{
				int num = 0;
				for (ThreadLocal<T>.LinkedSlot linkedSlot = this.m_linkedSlot.Next; linkedSlot != null; linkedSlot = linkedSlot.Next)
				{
					num++;
				}
				return num;
			}
		}

		/// <summary>Gets whether <see cref="P:System.Threading.ThreadLocal`1.Value" /> is initialized on the current thread.</summary>
		/// <returns>true if <see cref="P:System.Threading.ThreadLocal`1.Value" /> is initialized on the current thread; otherwise false.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ThreadLocal`1" /> instance has been disposed.</exception>
		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001EC0 RID: 7872 RVA: 0x00072438 File Offset: 0x00070638
		public bool IsValueCreated
		{
			get
			{
				int num = ~this.m_idComplement;
				if (num < 0)
				{
					throw new ObjectDisposedException(Environment.GetResourceString("The ThreadLocal object has been disposed."));
				}
				ThreadLocal<T>.LinkedSlotVolatile[] array = ThreadLocal<T>.ts_slotArray;
				return array != null && num < array.Length && array[num].Value != null;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001EC1 RID: 7873 RVA: 0x00072484 File Offset: 0x00070684
		internal T ValueForDebugDisplay
		{
			get
			{
				ThreadLocal<T>.LinkedSlotVolatile[] array = ThreadLocal<T>.ts_slotArray;
				int num = ~this.m_idComplement;
				ThreadLocal<T>.LinkedSlot value;
				if (array == null || num >= array.Length || (value = array[num].Value) == null || !this.m_initialized)
				{
					return default(T);
				}
				return value.Value;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001EC2 RID: 7874 RVA: 0x000724D4 File Offset: 0x000706D4
		internal List<T> ValuesForDebugDisplay
		{
			get
			{
				return this.GetValuesAsList();
			}
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x000724DC File Offset: 0x000706DC
		private void GrowTable(ref ThreadLocal<T>.LinkedSlotVolatile[] table, int minLength)
		{
			ThreadLocal<T>.LinkedSlotVolatile[] array = new ThreadLocal<T>.LinkedSlotVolatile[ThreadLocal<T>.GetNewTableSize(minLength)];
			ThreadLocal<T>.IdManager idManager = ThreadLocal<T>.s_idManager;
			lock (idManager)
			{
				for (int i = 0; i < table.Length; i++)
				{
					ThreadLocal<T>.LinkedSlot value = table[i].Value;
					if (value != null && value.SlotArray != null)
					{
						value.SlotArray = array;
						array[i] = table[i];
					}
				}
			}
			table = array;
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x0007256C File Offset: 0x0007076C
		private static int GetNewTableSize(int minSize)
		{
			if (minSize > 2146435071)
			{
				return int.MaxValue;
			}
			int num = minSize - 1;
			num |= num >> 1;
			num |= num >> 2;
			num |= num >> 4;
			num |= num >> 8;
			num |= num >> 16;
			num++;
			if (num > 2146435071)
			{
				num = 2146435071;
			}
			return num;
		}

		// Token: 0x04001ADC RID: 6876
		private Func<T> m_valueFactory;

		// Token: 0x04001ADD RID: 6877
		[ThreadStatic]
		private static ThreadLocal<T>.LinkedSlotVolatile[] ts_slotArray;

		// Token: 0x04001ADE RID: 6878
		[ThreadStatic]
		private static ThreadLocal<T>.FinalizationHelper ts_finalizationHelper;

		// Token: 0x04001ADF RID: 6879
		private int m_idComplement;

		// Token: 0x04001AE0 RID: 6880
		private volatile bool m_initialized;

		// Token: 0x04001AE1 RID: 6881
		private static ThreadLocal<T>.IdManager s_idManager = new ThreadLocal<T>.IdManager();

		// Token: 0x04001AE2 RID: 6882
		private ThreadLocal<T>.LinkedSlot m_linkedSlot = new ThreadLocal<T>.LinkedSlot(null);

		// Token: 0x04001AE3 RID: 6883
		private bool m_trackAllValues;

		// Token: 0x020002C1 RID: 705
		private struct LinkedSlotVolatile
		{
			// Token: 0x04001AE4 RID: 6884
			internal volatile ThreadLocal<T>.LinkedSlot Value;
		}

		// Token: 0x020002C2 RID: 706
		private sealed class LinkedSlot
		{
			// Token: 0x06001EC6 RID: 7878 RVA: 0x000725CB File Offset: 0x000707CB
			internal LinkedSlot(ThreadLocal<T>.LinkedSlotVolatile[] slotArray)
			{
				this.SlotArray = slotArray;
			}

			// Token: 0x04001AE5 RID: 6885
			internal volatile ThreadLocal<T>.LinkedSlot Next;

			// Token: 0x04001AE6 RID: 6886
			internal volatile ThreadLocal<T>.LinkedSlot Previous;

			// Token: 0x04001AE7 RID: 6887
			internal volatile ThreadLocal<T>.LinkedSlotVolatile[] SlotArray;

			// Token: 0x04001AE8 RID: 6888
			internal T Value;
		}

		// Token: 0x020002C3 RID: 707
		private class IdManager
		{
			// Token: 0x06001EC7 RID: 7879 RVA: 0x000725DC File Offset: 0x000707DC
			internal int GetId()
			{
				List<bool> freeIds = this.m_freeIds;
				int num2;
				lock (freeIds)
				{
					int num = this.m_nextIdToTry;
					while (num < this.m_freeIds.Count && !this.m_freeIds[num])
					{
						num++;
					}
					if (num == this.m_freeIds.Count)
					{
						this.m_freeIds.Add(false);
					}
					else
					{
						this.m_freeIds[num] = false;
					}
					this.m_nextIdToTry = num + 1;
					num2 = num;
				}
				return num2;
			}

			// Token: 0x06001EC8 RID: 7880 RVA: 0x00072674 File Offset: 0x00070874
			internal void ReturnId(int id)
			{
				List<bool> freeIds = this.m_freeIds;
				lock (freeIds)
				{
					this.m_freeIds[id] = true;
					if (id < this.m_nextIdToTry)
					{
						this.m_nextIdToTry = id;
					}
				}
			}

			// Token: 0x04001AE9 RID: 6889
			private int m_nextIdToTry;

			// Token: 0x04001AEA RID: 6890
			private List<bool> m_freeIds = new List<bool>();
		}

		// Token: 0x020002C4 RID: 708
		private class FinalizationHelper
		{
			// Token: 0x06001ECA RID: 7882 RVA: 0x000726DF File Offset: 0x000708DF
			internal FinalizationHelper(ThreadLocal<T>.LinkedSlotVolatile[] slotArray, bool trackAllValues)
			{
				this.SlotArray = slotArray;
				this.m_trackAllValues = trackAllValues;
			}

			// Token: 0x06001ECB RID: 7883 RVA: 0x000726F8 File Offset: 0x000708F8
			protected override void Finalize()
			{
				try
				{
					ThreadLocal<T>.LinkedSlotVolatile[] slotArray = this.SlotArray;
					for (int i = 0; i < slotArray.Length; i++)
					{
						ThreadLocal<T>.LinkedSlot value = slotArray[i].Value;
						if (value != null)
						{
							if (this.m_trackAllValues)
							{
								value.SlotArray = null;
							}
							else
							{
								ThreadLocal<T>.IdManager s_idManager = ThreadLocal<T>.s_idManager;
								lock (s_idManager)
								{
									if (value.Next != null)
									{
										value.Next.Previous = value.Previous;
									}
									value.Previous.Next = value.Next;
								}
							}
						}
					}
				}
				finally
				{
					base.Finalize();
				}
			}

			// Token: 0x04001AEB RID: 6891
			internal ThreadLocal<T>.LinkedSlotVolatile[] SlotArray;

			// Token: 0x04001AEC RID: 6892
			private bool m_trackAllValues;
		}
	}
}
