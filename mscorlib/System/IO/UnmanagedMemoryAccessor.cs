using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.IO
{
	/// <summary>Provides random access to unmanaged blocks of memory from managed code.</summary>
	// Token: 0x02000B28 RID: 2856
	public class UnmanagedMemoryAccessor : IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryAccessor" /> class.</summary>
		// Token: 0x0600666B RID: 26219 RVA: 0x0015D892 File Offset: 0x0015BA92
		protected UnmanagedMemoryAccessor()
		{
			this._isOpen = false;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryAccessor" /> class with a specified buffer, offset, and capacity.</summary>
		/// <param name="buffer">The buffer to contain the accessor.</param>
		/// <param name="offset">The byte at which to start the accessor.</param>
		/// <param name="capacity">The size, in bytes, of memory to allocate.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" /> plus <paramref name="capacity" /> is greater than <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="capacity" /> is less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="offset" /> plus <paramref name="capacity" /> would wrap around the high end of the address space.</exception>
		// Token: 0x0600666C RID: 26220 RVA: 0x0015D8A1 File Offset: 0x0015BAA1
		public UnmanagedMemoryAccessor(SafeBuffer buffer, long offset, long capacity)
		{
			this.Initialize(buffer, offset, capacity, FileAccess.Read);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryAccessor" /> class with a specified buffer, offset, capacity, and access right.</summary>
		/// <param name="buffer">The buffer to contain the accessor.</param>
		/// <param name="offset">The byte at which to start the accessor.</param>
		/// <param name="capacity">The size, in bytes, of memory to allocate.</param>
		/// <param name="access">The type of access allowed to the memory. The default is <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.ReadWrite" />.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" /> plus <paramref name="capacity" /> is greater than <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="capacity" /> is less than zero.  
		/// -or-  
		/// <paramref name="access" /> is not a valid <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFileAccess" /> enumeration value.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="offset" /> plus <paramref name="capacity" /> would wrap around the high end of the address space.</exception>
		// Token: 0x0600666D RID: 26221 RVA: 0x0015D8B3 File Offset: 0x0015BAB3
		public UnmanagedMemoryAccessor(SafeBuffer buffer, long offset, long capacity, FileAccess access)
		{
			this.Initialize(buffer, offset, capacity, access);
		}

		/// <summary>Sets the initial values for the accessor.</summary>
		/// <param name="buffer">The buffer to contain the accessor.</param>
		/// <param name="offset">The byte at which to start the accessor.</param>
		/// <param name="capacity">The size, in bytes, of memory to allocate.</param>
		/// <param name="access">The type of access allowed to the memory. The default is <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.ReadWrite" />.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" /> plus <paramref name="capacity" /> is greater than <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="capacity" /> is less than zero.  
		/// -or-  
		/// <paramref name="access" /> is not a valid <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFileAccess" /> enumeration value.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="offset" /> plus <paramref name="capacity" /> would wrap around the high end of the address space.</exception>
		// Token: 0x0600666E RID: 26222 RVA: 0x0015D8C8 File Offset: 0x0015BAC8
		protected unsafe void Initialize(SafeBuffer buffer, long offset, long capacity, FileAccess access)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0L)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (capacity < 0L)
			{
				throw new ArgumentOutOfRangeException("capacity", "Non-negative number required.");
			}
			if (buffer.ByteLength < (ulong)(offset + capacity))
			{
				throw new ArgumentException("Offset and capacity were greater than the size of the view.");
			}
			if (access < FileAccess.Read || access > FileAccess.ReadWrite)
			{
				throw new ArgumentOutOfRangeException("access");
			}
			if (this._isOpen)
			{
				throw new InvalidOperationException("The method cannot be called twice on the same instance.");
			}
			byte* ptr = null;
			try
			{
				buffer.AcquirePointer(ref ptr);
				if (ptr + offset + capacity < ptr)
				{
					throw new ArgumentException("The UnmanagedMemoryAccessor capacity and offset would wrap around the high end of the address space.");
				}
			}
			finally
			{
				if (ptr != null)
				{
					buffer.ReleasePointer();
				}
			}
			this._offset = offset;
			this._buffer = buffer;
			this._capacity = capacity;
			this._access = access;
			this._isOpen = true;
			this._canRead = (this._access & FileAccess.Read) > (FileAccess)0;
			this._canWrite = (this._access & FileAccess.Write) > (FileAccess)0;
		}

		/// <summary>Gets the capacity of the accessor.</summary>
		/// <returns>The capacity of the accessor.</returns>
		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x0600666F RID: 26223 RVA: 0x0015D9D0 File Offset: 0x0015BBD0
		public long Capacity
		{
			get
			{
				return this._capacity;
			}
		}

		/// <summary>Determines whether the accessor is readable.</summary>
		/// <returns>
		///   <see langword="true" /> if the accessor is readable; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x06006670 RID: 26224 RVA: 0x0015D9D8 File Offset: 0x0015BBD8
		public bool CanRead
		{
			get
			{
				return this._isOpen && this._canRead;
			}
		}

		/// <summary>Determines whether the accessory is writable.</summary>
		/// <returns>
		///   <see langword="true" /> if the accessor is writable; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x06006671 RID: 26225 RVA: 0x0015D9EA File Offset: 0x0015BBEA
		public bool CanWrite
		{
			get
			{
				return this._isOpen && this._canWrite;
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.UnmanagedMemoryAccessor" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x06006672 RID: 26226 RVA: 0x0015D9FC File Offset: 0x0015BBFC
		protected virtual void Dispose(bool disposing)
		{
			this._isOpen = false;
		}

		/// <summary>Releases all resources used by the <see cref="T:System.IO.UnmanagedMemoryAccessor" />.</summary>
		// Token: 0x06006673 RID: 26227 RVA: 0x0015DA05 File Offset: 0x0015BC05
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Determines whether the accessor is currently open by a process.</summary>
		/// <returns>
		///   <see langword="true" /> if the accessor is open; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x06006674 RID: 26228 RVA: 0x0015DA14 File Offset: 0x0015BC14
		protected bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
		}

		/// <summary>Reads a Boolean value from the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
		/// <returns>
		///   <see langword="true" /> or <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006675 RID: 26229 RVA: 0x0015DA1C File Offset: 0x0015BC1C
		public bool ReadBoolean(long position)
		{
			return this.ReadByte(position) > 0;
		}

		/// <summary>Reads a byte value from the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
		/// <returns>The value that was read.</returns>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006676 RID: 26230 RVA: 0x0015DA28 File Offset: 0x0015BC28
		public unsafe byte ReadByte(long position)
		{
			this.EnsureSafeToRead(position, 1);
			byte* ptr = null;
			byte b;
			try
			{
				this._buffer.AcquirePointer(ref ptr);
				b = (ptr + this._offset)[position];
			}
			finally
			{
				if (ptr != null)
				{
					this._buffer.ReleasePointer();
				}
			}
			return b;
		}

		/// <summary>Reads a character from the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
		/// <returns>The value that was read.</returns>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006677 RID: 26231 RVA: 0x0015DA80 File Offset: 0x0015BC80
		public char ReadChar(long position)
		{
			return (char)this.ReadInt16(position);
		}

		/// <summary>Reads a 16-bit integer from the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
		/// <returns>The value that was read.</returns>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006678 RID: 26232 RVA: 0x0015DA8C File Offset: 0x0015BC8C
		public unsafe short ReadInt16(long position)
		{
			this.EnsureSafeToRead(position, 2);
			byte* ptr = null;
			short num;
			try
			{
				this._buffer.AcquirePointer(ref ptr);
				num = Unsafe.ReadUnaligned<short>((void*)(ptr + this._offset + position));
			}
			finally
			{
				if (ptr != null)
				{
					this._buffer.ReleasePointer();
				}
			}
			return num;
		}

		/// <summary>Reads a 32-bit integer from the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
		/// <returns>The value that was read.</returns>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006679 RID: 26233 RVA: 0x0015DAE8 File Offset: 0x0015BCE8
		public unsafe int ReadInt32(long position)
		{
			this.EnsureSafeToRead(position, 4);
			byte* ptr = null;
			int num;
			try
			{
				this._buffer.AcquirePointer(ref ptr);
				num = Unsafe.ReadUnaligned<int>((void*)(ptr + this._offset + position));
			}
			finally
			{
				if (ptr != null)
				{
					this._buffer.ReleasePointer();
				}
			}
			return num;
		}

		/// <summary>Reads a 64-bit integer from the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
		/// <returns>The value that was read.</returns>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x0600667A RID: 26234 RVA: 0x0015DB44 File Offset: 0x0015BD44
		public unsafe long ReadInt64(long position)
		{
			this.EnsureSafeToRead(position, 8);
			byte* ptr = null;
			long num;
			try
			{
				this._buffer.AcquirePointer(ref ptr);
				num = Unsafe.ReadUnaligned<long>((void*)(ptr + this._offset + position));
			}
			finally
			{
				if (ptr != null)
				{
					this._buffer.ReleasePointer();
				}
			}
			return num;
		}

		/// <summary>Reads a decimal value from the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
		/// <returns>The value that was read.</returns>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.  
		///  -or-  
		///  The decimal to read is invalid.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x0600667B RID: 26235 RVA: 0x0015DBA0 File Offset: 0x0015BDA0
		public unsafe decimal ReadDecimal(long position)
		{
			this.EnsureSafeToRead(position, 16);
			byte* ptr = null;
			int num;
			int num2;
			int num3;
			int num4;
			try
			{
				this._buffer.AcquirePointer(ref ptr);
				ptr += this._offset + position;
				num = Unsafe.ReadUnaligned<int>((void*)ptr);
				num2 = Unsafe.ReadUnaligned<int>((void*)(ptr + 4));
				num3 = Unsafe.ReadUnaligned<int>((void*)(ptr + 8));
				num4 = Unsafe.ReadUnaligned<int>((void*)(ptr + 12));
			}
			finally
			{
				if (ptr != null)
				{
					this._buffer.ReleasePointer();
				}
			}
			if ((num4 & 2130771967) != 0 || (num4 & 16711680) > 1835008)
			{
				throw new ArgumentException("Read an invalid decimal value from the buffer.");
			}
			bool flag = (num4 & int.MinValue) != 0;
			byte b = (byte)(num4 >> 16);
			return new decimal(num, num2, num3, flag, b);
		}

		/// <summary>Reads a single-precision floating-point value from the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
		/// <returns>The value that was read.</returns>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x0600667C RID: 26236 RVA: 0x0015DC64 File Offset: 0x0015BE64
		public float ReadSingle(long position)
		{
			return BitConverter.Int32BitsToSingle(this.ReadInt32(position));
		}

		/// <summary>Reads a double-precision floating-point value from the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
		/// <returns>The value that was read.</returns>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x0600667D RID: 26237 RVA: 0x0015DC72 File Offset: 0x0015BE72
		public double ReadDouble(long position)
		{
			return BitConverter.Int64BitsToDouble(this.ReadInt64(position));
		}

		/// <summary>Reads an 8-bit signed integer from the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
		/// <returns>The value that was read.</returns>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x0600667E RID: 26238 RVA: 0x0015DC80 File Offset: 0x0015BE80
		[CLSCompliant(false)]
		public sbyte ReadSByte(long position)
		{
			return (sbyte)this.ReadByte(position);
		}

		/// <summary>Reads an unsigned 16-bit integer from the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
		/// <returns>The value that was read.</returns>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x0600667F RID: 26239 RVA: 0x0015DA80 File Offset: 0x0015BC80
		[CLSCompliant(false)]
		public ushort ReadUInt16(long position)
		{
			return (ushort)this.ReadInt16(position);
		}

		/// <summary>Reads an unsigned 32-bit integer from the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
		/// <returns>The value that was read.</returns>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006680 RID: 26240 RVA: 0x0015DC8A File Offset: 0x0015BE8A
		[CLSCompliant(false)]
		public uint ReadUInt32(long position)
		{
			return (uint)this.ReadInt32(position);
		}

		/// <summary>Reads an unsigned 64-bit integer from the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
		/// <returns>The value that was read.</returns>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006681 RID: 26241 RVA: 0x0015DC93 File Offset: 0x0015BE93
		[CLSCompliant(false)]
		public ulong ReadUInt64(long position)
		{
			return (ulong)this.ReadInt64(position);
		}

		/// <summary>Reads a structure of type <paramref name="T" /> from the accessor into a provided reference.</summary>
		/// <param name="position">The position in the accessor at which to begin reading.</param>
		/// <param name="structure">The structure to contain the read data.</param>
		/// <typeparam name="T">The type of structure.</typeparam>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read in a structure of type <paramref name="T" />.  
		///  -or-  
		///  <see langword="T" /> is a value type that contains one or more reference types.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006682 RID: 26242 RVA: 0x0015DC9C File Offset: 0x0015BE9C
		public void Read<T>(long position, out T structure) where T : struct
		{
			if (position < 0L)
			{
				throw new ArgumentOutOfRangeException("position", "Non-negative number required.");
			}
			if (!this._isOpen)
			{
				throw new ObjectDisposedException("UnmanagedMemoryAccessor", "Cannot access a closed accessor.");
			}
			if (!this._canRead)
			{
				throw new NotSupportedException("Accessor does not support reading.");
			}
			uint num = SafeBuffer.SizeOf<T>();
			if (position <= this._capacity - (long)((ulong)num))
			{
				structure = this._buffer.Read<T>((ulong)(this._offset + position));
				return;
			}
			if (position >= this._capacity)
			{
				throw new ArgumentOutOfRangeException("position", "The position may not be greater or equal to the capacity of the accessor.");
			}
			throw new ArgumentException(SR.Format("There are not enough bytes remaining in the accessor to read at this position.", typeof(T)), "position");
		}

		/// <summary>Reads structures of type <paramref name="T" /> from the accessor into an array of type <paramref name="T" />.</summary>
		/// <param name="position">The number of bytes in the accessor at which to begin reading.</param>
		/// <param name="array">The array to contain the structures read from the accessor.</param>
		/// <param name="offset">The index in <paramref name="array" /> in which to place the first copied structure.</param>
		/// <param name="count">The number of structures of type T to read from the accessor.</param>
		/// <typeparam name="T">The type of structure.</typeparam>
		/// <returns>The number of structures read into <paramref name="array" />. This value can be less than <paramref name="count" /> if there are fewer structures available, or zero if the end of the accessor is reached.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is not large enough to contain <paramref name="count" /> of structures (starting from <paramref name="position" />).</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006683 RID: 26243 RVA: 0x0015DD4C File Offset: 0x0015BF4C
		public int ReadArray<T>(long position, T[] array, int offset, int count) where T : struct
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", "Buffer cannot be null.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (!this._isOpen)
			{
				throw new ObjectDisposedException("UnmanagedMemoryAccessor", "Cannot access a closed accessor.");
			}
			if (!this._canRead)
			{
				throw new NotSupportedException("Accessor does not support reading.");
			}
			if (position < 0L)
			{
				throw new ArgumentOutOfRangeException("position", "Non-negative number required.");
			}
			uint num = SafeBuffer.AlignedSizeOf<T>();
			if (position >= this._capacity)
			{
				throw new ArgumentOutOfRangeException("position", "The position may not be greater or equal to the capacity of the accessor.");
			}
			int num2 = count;
			long num3 = this._capacity - position;
			if (num3 < 0L)
			{
				num2 = 0;
			}
			else
			{
				ulong num4 = (ulong)num * (ulong)((long)count);
				if (num3 < (long)num4)
				{
					num2 = (int)(num3 / (long)((ulong)num));
				}
			}
			this._buffer.ReadArray<T>((ulong)(this._offset + position), array, offset, num2);
			return num2;
		}

		/// <summary>Writes a Boolean value into the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006684 RID: 26244 RVA: 0x0015DE45 File Offset: 0x0015C045
		public void Write(long position, bool value)
		{
			this.Write(position, value ? 1 : 0);
		}

		/// <summary>Writes a byte value into the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006685 RID: 26245 RVA: 0x0015DE58 File Offset: 0x0015C058
		public unsafe void Write(long position, byte value)
		{
			this.EnsureSafeToWrite(position, 1);
			byte* ptr = null;
			try
			{
				this._buffer.AcquirePointer(ref ptr);
				(ptr + this._offset)[position] = value;
			}
			finally
			{
				if (ptr != null)
				{
					this._buffer.ReleasePointer();
				}
			}
		}

		/// <summary>Writes a character into the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006686 RID: 26246 RVA: 0x0015DEB0 File Offset: 0x0015C0B0
		public void Write(long position, char value)
		{
			this.Write(position, (short)value);
		}

		/// <summary>Writes a 16-bit integer into the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006687 RID: 26247 RVA: 0x0015DEBC File Offset: 0x0015C0BC
		public unsafe void Write(long position, short value)
		{
			this.EnsureSafeToWrite(position, 2);
			byte* ptr = null;
			try
			{
				this._buffer.AcquirePointer(ref ptr);
				Unsafe.WriteUnaligned<short>((void*)(ptr + this._offset + position), value);
			}
			finally
			{
				if (ptr != null)
				{
					this._buffer.ReleasePointer();
				}
			}
		}

		/// <summary>Writes a 32-bit integer into the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006688 RID: 26248 RVA: 0x0015DF18 File Offset: 0x0015C118
		public unsafe void Write(long position, int value)
		{
			this.EnsureSafeToWrite(position, 4);
			byte* ptr = null;
			try
			{
				this._buffer.AcquirePointer(ref ptr);
				Unsafe.WriteUnaligned<int>((void*)(ptr + this._offset + position), value);
			}
			finally
			{
				if (ptr != null)
				{
					this._buffer.ReleasePointer();
				}
			}
		}

		/// <summary>Writes a 64-bit integer into the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after position to write a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006689 RID: 26249 RVA: 0x0015DF74 File Offset: 0x0015C174
		public unsafe void Write(long position, long value)
		{
			this.EnsureSafeToWrite(position, 8);
			byte* ptr = null;
			try
			{
				this._buffer.AcquirePointer(ref ptr);
				Unsafe.WriteUnaligned<long>((void*)(ptr + this._offset + position), value);
			}
			finally
			{
				if (ptr != null)
				{
					this._buffer.ReleasePointer();
				}
			}
		}

		/// <summary>Writes a decimal value into the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.  
		///  -or-  
		///  The decimal is invalid.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x0600668A RID: 26250 RVA: 0x0015DFD0 File Offset: 0x0015C1D0
		public unsafe void Write(long position, decimal value)
		{
			this.EnsureSafeToWrite(position, 16);
			int* ptr = (int*)(&value);
			int num = *ptr;
			int num2 = ptr[1];
			int num3 = ptr[2];
			int num4 = ptr[3];
			byte* ptr2 = null;
			try
			{
				this._buffer.AcquirePointer(ref ptr2);
				ptr2 += this._offset + position;
				Unsafe.WriteUnaligned<int>((void*)ptr2, num3);
				Unsafe.WriteUnaligned<int>((void*)(ptr2 + 4), num4);
				Unsafe.WriteUnaligned<int>((void*)(ptr2 + 8), num2);
				Unsafe.WriteUnaligned<int>((void*)(ptr2 + 12), num);
			}
			finally
			{
				if (ptr2 != null)
				{
					this._buffer.ReleasePointer();
				}
			}
		}

		/// <summary>Writes a <see langword="Single" /> into the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x0600668B RID: 26251 RVA: 0x0015E070 File Offset: 0x0015C270
		public void Write(long position, float value)
		{
			this.Write(position, BitConverter.SingleToInt32Bits(value));
		}

		/// <summary>Writes a <see langword="Double" /> value into the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x0600668C RID: 26252 RVA: 0x0015E07F File Offset: 0x0015C27F
		public void Write(long position, double value)
		{
			this.Write(position, BitConverter.DoubleToInt64Bits(value));
		}

		/// <summary>Writes an 8-bit integer into the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x0600668D RID: 26253 RVA: 0x0015E08E File Offset: 0x0015C28E
		[CLSCompliant(false)]
		public void Write(long position, sbyte value)
		{
			this.Write(position, (byte)value);
		}

		/// <summary>Writes an unsigned 16-bit integer into the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x0600668E RID: 26254 RVA: 0x0015DEB0 File Offset: 0x0015C0B0
		[CLSCompliant(false)]
		public void Write(long position, ushort value)
		{
			this.Write(position, (short)value);
		}

		/// <summary>Writes an unsigned 32-bit integer into the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x0600668F RID: 26255 RVA: 0x0015E099 File Offset: 0x0015C299
		[CLSCompliant(false)]
		public void Write(long position, uint value)
		{
			this.Write(position, (int)value);
		}

		/// <summary>Writes an unsigned 64-bit integer into the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006690 RID: 26256 RVA: 0x0015E0A3 File Offset: 0x0015C2A3
		[CLSCompliant(false)]
		public void Write(long position, ulong value)
		{
			this.Write(position, (long)value);
		}

		/// <summary>Writes a structure into the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
		/// <param name="structure">The structure to write.</param>
		/// <typeparam name="T">The type of structure.</typeparam>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes in the accessor after <paramref name="position" /> to write a structure of type <paramref name="T" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006691 RID: 26257 RVA: 0x0015E0B0 File Offset: 0x0015C2B0
		public void Write<T>(long position, ref T structure) where T : struct
		{
			if (position < 0L)
			{
				throw new ArgumentOutOfRangeException("position", "Non-negative number required.");
			}
			if (!this._isOpen)
			{
				throw new ObjectDisposedException("UnmanagedMemoryAccessor", "Cannot access a closed accessor.");
			}
			if (!this._canWrite)
			{
				throw new NotSupportedException("Accessor does not support writing.");
			}
			uint num = SafeBuffer.SizeOf<T>();
			if (position <= this._capacity - (long)((ulong)num))
			{
				this._buffer.Write<T>((ulong)(this._offset + position), structure);
				return;
			}
			if (position >= this._capacity)
			{
				throw new ArgumentOutOfRangeException("position", "The position may not be greater or equal to the capacity of the accessor.");
			}
			throw new ArgumentException(SR.Format("There are not enough bytes remaining in the accessor to write at this position.", typeof(T)), "position");
		}

		/// <summary>Writes structures from an array of type <paramref name="T" /> into the accessor.</summary>
		/// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
		/// <param name="array">The array to write into the accessor.</param>
		/// <param name="offset">The index in <paramref name="array" /> to start writing from.</param>
		/// <param name="count">The number of structures in <paramref name="array" /> to write.</param>
		/// <typeparam name="T">The type of structure.</typeparam>
		/// <exception cref="T:System.ArgumentException">There are not enough bytes in the accessor after <paramref name="position" /> to write the number of structures specified by <paramref name="count" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than zero or greater than the capacity of the accessor.  
		/// -or-  
		/// <paramref name="offset" /> or <paramref name="count" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
		// Token: 0x06006692 RID: 26258 RVA: 0x0015E160 File Offset: 0x0015C360
		public void WriteArray<T>(long position, T[] array, int offset, int count) where T : struct
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", "Buffer cannot be null.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (position < 0L)
			{
				throw new ArgumentOutOfRangeException("position", "Non-negative number required.");
			}
			if (position >= this.Capacity)
			{
				throw new ArgumentOutOfRangeException("position", "The position may not be greater or equal to the capacity of the accessor.");
			}
			if (!this._isOpen)
			{
				throw new ObjectDisposedException("UnmanagedMemoryAccessor", "Cannot access a closed accessor.");
			}
			if (!this._canWrite)
			{
				throw new NotSupportedException("Accessor does not support writing.");
			}
			this._buffer.WriteArray<T>((ulong)(this._offset + position), array, offset, count);
		}

		// Token: 0x06006693 RID: 26259 RVA: 0x0015E230 File Offset: 0x0015C430
		private void EnsureSafeToRead(long position, int sizeOfType)
		{
			if (!this._isOpen)
			{
				throw new ObjectDisposedException("UnmanagedMemoryAccessor", "Cannot access a closed accessor.");
			}
			if (!this._canRead)
			{
				throw new NotSupportedException("Accessor does not support reading.");
			}
			if (position < 0L)
			{
				throw new ArgumentOutOfRangeException("position", "Non-negative number required.");
			}
			if (position <= this._capacity - (long)sizeOfType)
			{
				return;
			}
			if (position >= this._capacity)
			{
				throw new ArgumentOutOfRangeException("position", "The position may not be greater or equal to the capacity of the accessor.");
			}
			throw new ArgumentException("There are not enough bytes remaining in the accessor to read at this position.", "position");
		}

		// Token: 0x06006694 RID: 26260 RVA: 0x0015E2B4 File Offset: 0x0015C4B4
		private void EnsureSafeToWrite(long position, int sizeOfType)
		{
			if (!this._isOpen)
			{
				throw new ObjectDisposedException("UnmanagedMemoryAccessor", "Cannot access a closed accessor.");
			}
			if (!this._canWrite)
			{
				throw new NotSupportedException("Accessor does not support writing.");
			}
			if (position < 0L)
			{
				throw new ArgumentOutOfRangeException("position", "Non-negative number required.");
			}
			if (position <= this._capacity - (long)sizeOfType)
			{
				return;
			}
			if (position >= this._capacity)
			{
				throw new ArgumentOutOfRangeException("position", "The position may not be greater or equal to the capacity of the accessor.");
			}
			throw new ArgumentException("There are not enough bytes remaining in the accessor to write at this position.", "position");
		}

		// Token: 0x04003C0D RID: 15373
		private SafeBuffer _buffer;

		// Token: 0x04003C0E RID: 15374
		private long _offset;

		// Token: 0x04003C0F RID: 15375
		private long _capacity;

		// Token: 0x04003C10 RID: 15376
		private FileAccess _access;

		// Token: 0x04003C11 RID: 15377
		private bool _isOpen;

		// Token: 0x04003C12 RID: 15378
		private bool _canRead;

		// Token: 0x04003C13 RID: 15379
		private bool _canWrite;
	}
}
