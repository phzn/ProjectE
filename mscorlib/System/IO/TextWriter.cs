using System;
using System.Buffers;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
	/// <summary>Represents a writer that can write a sequential series of characters. This class is abstract.</summary>
	// Token: 0x02000B24 RID: 2852
	[Serializable]
	public abstract class TextWriter : MarshalByRefObject, IDisposable, IAsyncDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.IO.TextWriter" /> class.</summary>
		// Token: 0x060065E8 RID: 26088 RVA: 0x0015CC26 File Offset: 0x0015AE26
		protected TextWriter()
		{
			this._internalFormatProvider = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.TextWriter" /> class with the specified format provider.</summary>
		/// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that controls formatting.</param>
		// Token: 0x060065E9 RID: 26089 RVA: 0x0015CC4B File Offset: 0x0015AE4B
		protected TextWriter(IFormatProvider formatProvider)
		{
			this._internalFormatProvider = formatProvider;
		}

		/// <summary>Gets an object that controls formatting.</summary>
		/// <returns>An <see cref="T:System.IFormatProvider" /> object for a specific culture, or the formatting of the current culture if no other culture is specified.</returns>
		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x060065EA RID: 26090 RVA: 0x0015CC70 File Offset: 0x0015AE70
		public virtual IFormatProvider FormatProvider
		{
			get
			{
				if (this._internalFormatProvider == null)
				{
					return CultureInfo.CurrentCulture;
				}
				return this._internalFormatProvider;
			}
		}

		/// <summary>Closes the current writer and releases any system resources associated with the writer.</summary>
		// Token: 0x060065EB RID: 26091 RVA: 0x0015A797 File Offset: 0x00158997
		public virtual void Close()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.TextWriter" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x060065EC RID: 26092 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void Dispose(bool disposing)
		{
		}

		/// <summary>Releases all resources used by the <see cref="T:System.IO.TextWriter" /> object.</summary>
		// Token: 0x060065ED RID: 26093 RVA: 0x0015A797 File Offset: 0x00158997
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060065EE RID: 26094 RVA: 0x0015CC88 File Offset: 0x0015AE88
		public virtual ValueTask DisposeAsync()
		{
			ValueTask valueTask;
			try
			{
				this.Dispose();
				valueTask = default(ValueTask);
				valueTask = valueTask;
			}
			catch (Exception ex)
			{
				valueTask = new ValueTask(Task.FromException(ex));
			}
			return valueTask;
		}

		/// <summary>Clears all buffers for the current writer and causes any buffered data to be written to the underlying device.</summary>
		// Token: 0x060065EF RID: 26095 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public virtual void Flush()
		{
		}

		/// <summary>When overridden in a derived class, returns the character encoding in which the output is written.</summary>
		/// <returns>The character encoding in which the output is written.</returns>
		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x060065F0 RID: 26096
		public abstract Encoding Encoding { get; }

		/// <summary>Gets or sets the line terminator string used by the current <see langword="TextWriter" />.</summary>
		/// <returns>The line terminator string for the current <see langword="TextWriter" />.</returns>
		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x060065F1 RID: 26097 RVA: 0x0015CCC8 File Offset: 0x0015AEC8
		// (set) Token: 0x060065F2 RID: 26098 RVA: 0x0015CCD0 File Offset: 0x0015AED0
		public virtual string NewLine
		{
			get
			{
				return this.CoreNewLineStr;
			}
			set
			{
				if (value == null)
				{
					value = Environment.NewLine;
				}
				this.CoreNewLineStr = value;
				this.CoreNewLine = value.ToCharArray();
			}
		}

		/// <summary>Writes a character to the text string or stream.</summary>
		/// <param name="value">The character to write to the text stream.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x060065F3 RID: 26099 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public virtual void Write(char value)
		{
		}

		/// <summary>Writes a character array to the text string or stream.</summary>
		/// <param name="buffer">The character array to write to the text stream.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x060065F4 RID: 26100 RVA: 0x0015CCEF File Offset: 0x0015AEEF
		public virtual void Write(char[] buffer)
		{
			if (buffer != null)
			{
				this.Write(buffer, 0, buffer.Length);
			}
		}

		/// <summary>Writes a subarray of characters to the text string or stream.</summary>
		/// <param name="buffer">The character array to write data from.</param>
		/// <param name="index">The character position in the buffer at which to start retrieving data.</param>
		/// <param name="count">The number of characters to write.</param>
		/// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x060065F5 RID: 26101 RVA: 0x0015CD00 File Offset: 0x0015AF00
		public virtual void Write(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			for (int i = 0; i < count; i++)
			{
				this.Write(buffer[index + i]);
			}
		}

		// Token: 0x060065F6 RID: 26102 RVA: 0x0015CD74 File Offset: 0x0015AF74
		public virtual void Write(ReadOnlySpan<char> buffer)
		{
			char[] array = ArrayPool<char>.Shared.Rent(buffer.Length);
			try
			{
				buffer.CopyTo(new Span<char>(array));
				this.Write(array, 0, buffer.Length);
			}
			finally
			{
				ArrayPool<char>.Shared.Return(array, false);
			}
		}

		/// <summary>Writes the text representation of a <see langword="Boolean" /> value to the text string or stream.</summary>
		/// <param name="value">The <see langword="Boolean" /> value to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x060065F7 RID: 26103 RVA: 0x0015CDD0 File Offset: 0x0015AFD0
		public virtual void Write(bool value)
		{
			this.Write(value ? "True" : "False");
		}

		/// <summary>Writes the text representation of a 4-byte signed integer to the text string or stream.</summary>
		/// <param name="value">The 4-byte signed integer to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x060065F8 RID: 26104 RVA: 0x0015CDE7 File Offset: 0x0015AFE7
		public virtual void Write(int value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		/// <summary>Writes the text representation of a 4-byte unsigned integer to the text string or stream.</summary>
		/// <param name="value">The 4-byte unsigned integer to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x060065F9 RID: 26105 RVA: 0x0015CDFC File Offset: 0x0015AFFC
		[CLSCompliant(false)]
		public virtual void Write(uint value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		/// <summary>Writes the text representation of an 8-byte signed integer to the text string or stream.</summary>
		/// <param name="value">The 8-byte signed integer to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x060065FA RID: 26106 RVA: 0x0015CE11 File Offset: 0x0015B011
		public virtual void Write(long value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		/// <summary>Writes the text representation of an 8-byte unsigned integer to the text string or stream.</summary>
		/// <param name="value">The 8-byte unsigned integer to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x060065FB RID: 26107 RVA: 0x0015CE26 File Offset: 0x0015B026
		[CLSCompliant(false)]
		public virtual void Write(ulong value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		/// <summary>Writes the text representation of a 4-byte floating-point value to the text string or stream.</summary>
		/// <param name="value">The 4-byte floating-point value to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x060065FC RID: 26108 RVA: 0x0015CE3B File Offset: 0x0015B03B
		public virtual void Write(float value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		/// <summary>Writes the text representation of an 8-byte floating-point value to the text string or stream.</summary>
		/// <param name="value">The 8-byte floating-point value to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x060065FD RID: 26109 RVA: 0x0015CE50 File Offset: 0x0015B050
		public virtual void Write(double value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		/// <summary>Writes the text representation of a decimal value to the text string or stream.</summary>
		/// <param name="value">The decimal value to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x060065FE RID: 26110 RVA: 0x0015CE65 File Offset: 0x0015B065
		public virtual void Write(decimal value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		/// <summary>Writes a string to the text string or stream.</summary>
		/// <param name="value">The string to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x060065FF RID: 26111 RVA: 0x0015CE7A File Offset: 0x0015B07A
		public virtual void Write(string value)
		{
			if (value != null)
			{
				this.Write(value.ToCharArray());
			}
		}

		/// <summary>Writes the text representation of an object to the text string or stream by calling the <see langword="ToString" /> method on that object.</summary>
		/// <param name="value">The object to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x06006600 RID: 26112 RVA: 0x0015CE8C File Offset: 0x0015B08C
		public virtual void Write(object value)
		{
			if (value != null)
			{
				IFormattable formattable = value as IFormattable;
				if (formattable != null)
				{
					this.Write(formattable.ToString(null, this.FormatProvider));
					return;
				}
				this.Write(value.ToString());
			}
		}

		/// <summary>Writes a formatted string to the text string or stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object)" /> method.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The object to format and write.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is not a valid composite format string.  
		/// -or-  
		/// The index of a format item is less than 0 (zero), or greater than or equal to the number of objects to be formatted (which, for this method overload, is one).</exception>
		// Token: 0x06006601 RID: 26113 RVA: 0x0015CEC6 File Offset: 0x0015B0C6
		public virtual void Write(string format, object arg0)
		{
			this.Write(string.Format(this.FormatProvider, format, arg0));
		}

		/// <summary>Writes a formatted string to the text string or stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object,System.Object)" /> method.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to format and write.</param>
		/// <param name="arg1">The second object to format and write.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is not a valid composite format string.  
		/// -or-  
		/// The index of a format item is less than 0 (zero) or greater than or equal to the number of objects to be formatted (which, for this method overload, is two).</exception>
		// Token: 0x06006602 RID: 26114 RVA: 0x0015CEDB File Offset: 0x0015B0DB
		public virtual void Write(string format, object arg0, object arg1)
		{
			this.Write(string.Format(this.FormatProvider, format, arg0, arg1));
		}

		/// <summary>Writes a formatted string to the text string or stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object,System.Object,System.Object)" /> method.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to format and write.</param>
		/// <param name="arg1">The second object to format and write.</param>
		/// <param name="arg2">The third object to format and write.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is not a valid composite format string.  
		/// -or-  
		/// The index of a format item is less than 0 (zero), or greater than or equal to the number of objects to be formatted (which, for this method overload, is three).</exception>
		// Token: 0x06006603 RID: 26115 RVA: 0x0015CEF1 File Offset: 0x0015B0F1
		public virtual void Write(string format, object arg0, object arg1, object arg2)
		{
			this.Write(string.Format(this.FormatProvider, format, arg0, arg1, arg2));
		}

		/// <summary>Writes a formatted string to the text string or stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object[])" /> method.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg">An object array that contains zero or more objects to format and write.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> or <paramref name="arg" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is not a valid composite format string.  
		/// -or-  
		/// The index of a format item is less than 0 (zero), or greater than or equal to the length of the <paramref name="arg" /> array.</exception>
		// Token: 0x06006604 RID: 26116 RVA: 0x0015CF09 File Offset: 0x0015B109
		public virtual void Write(string format, params object[] arg)
		{
			this.Write(string.Format(this.FormatProvider, format, arg));
		}

		/// <summary>Writes a line terminator to the text string or stream.</summary>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x06006605 RID: 26117 RVA: 0x0015CF1E File Offset: 0x0015B11E
		public virtual void WriteLine()
		{
			this.Write(this.CoreNewLine);
		}

		/// <summary>Writes a character followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The character to write to the text stream.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x06006606 RID: 26118 RVA: 0x0015CF2C File Offset: 0x0015B12C
		public virtual void WriteLine(char value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes an array of characters followed by a line terminator to the text string or stream.</summary>
		/// <param name="buffer">The character array from which data is read.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x06006607 RID: 26119 RVA: 0x0015CF3B File Offset: 0x0015B13B
		public virtual void WriteLine(char[] buffer)
		{
			this.Write(buffer);
			this.WriteLine();
		}

		/// <summary>Writes a subarray of characters followed by a line terminator to the text string or stream.</summary>
		/// <param name="buffer">The character array from which data is read.</param>
		/// <param name="index">The character position in <paramref name="buffer" /> at which to start reading data.</param>
		/// <param name="count">The maximum number of characters to write.</param>
		/// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x06006608 RID: 26120 RVA: 0x0015CF4A File Offset: 0x0015B14A
		public virtual void WriteLine(char[] buffer, int index, int count)
		{
			this.Write(buffer, index, count);
			this.WriteLine();
		}

		// Token: 0x06006609 RID: 26121 RVA: 0x0015CF5C File Offset: 0x0015B15C
		public virtual void WriteLine(ReadOnlySpan<char> buffer)
		{
			char[] array = ArrayPool<char>.Shared.Rent(buffer.Length);
			try
			{
				buffer.CopyTo(new Span<char>(array));
				this.WriteLine(array, 0, buffer.Length);
			}
			finally
			{
				ArrayPool<char>.Shared.Return(array, false);
			}
		}

		/// <summary>Writes the text representation of a <see langword="Boolean" /> value followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The <see langword="Boolean" /> value to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x0600660A RID: 26122 RVA: 0x0015CFB8 File Offset: 0x0015B1B8
		public virtual void WriteLine(bool value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of a 4-byte signed integer followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The 4-byte signed integer to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x0600660B RID: 26123 RVA: 0x0015CFC7 File Offset: 0x0015B1C7
		public virtual void WriteLine(int value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of a 4-byte unsigned integer followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The 4-byte unsigned integer to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x0600660C RID: 26124 RVA: 0x0015CFD6 File Offset: 0x0015B1D6
		[CLSCompliant(false)]
		public virtual void WriteLine(uint value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of an 8-byte signed integer followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The 8-byte signed integer to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x0600660D RID: 26125 RVA: 0x0015CFE5 File Offset: 0x0015B1E5
		public virtual void WriteLine(long value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of an 8-byte unsigned integer followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The 8-byte unsigned integer to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x0600660E RID: 26126 RVA: 0x0015CFF4 File Offset: 0x0015B1F4
		[CLSCompliant(false)]
		public virtual void WriteLine(ulong value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of a 4-byte floating-point value followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The 4-byte floating-point value to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x0600660F RID: 26127 RVA: 0x0015D003 File Offset: 0x0015B203
		public virtual void WriteLine(float value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of a 8-byte floating-point value followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The 8-byte floating-point value to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x06006610 RID: 26128 RVA: 0x0015D012 File Offset: 0x0015B212
		public virtual void WriteLine(double value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes the text representation of a decimal value followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The decimal value to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x06006611 RID: 26129 RVA: 0x0015D021 File Offset: 0x0015B221
		public virtual void WriteLine(decimal value)
		{
			this.Write(value);
			this.WriteLine();
		}

		/// <summary>Writes a string followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The string to write. If <paramref name="value" /> is <see langword="null" />, only the line terminator is written.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x06006612 RID: 26130 RVA: 0x0015D030 File Offset: 0x0015B230
		public virtual void WriteLine(string value)
		{
			if (value != null)
			{
				this.Write(value);
			}
			this.Write(this.CoreNewLineStr);
		}

		/// <summary>Writes the text representation of an object by calling the <see langword="ToString" /> method on that object, followed by a line terminator to the text string or stream.</summary>
		/// <param name="value">The object to write. If <paramref name="value" /> is <see langword="null" />, only the line terminator is written.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x06006613 RID: 26131 RVA: 0x0015D048 File Offset: 0x0015B248
		public virtual void WriteLine(object value)
		{
			if (value == null)
			{
				this.WriteLine();
				return;
			}
			IFormattable formattable = value as IFormattable;
			if (formattable != null)
			{
				this.WriteLine(formattable.ToString(null, this.FormatProvider));
				return;
			}
			this.WriteLine(value.ToString());
		}

		/// <summary>Writes a formatted string and a new line to the text string or stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object)" /> method.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The object to format and write.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is not a valid composite format string.  
		/// -or-  
		/// The index of a format item is less than 0 (zero), or greater than or equal to the number of objects to be formatted (which, for this method overload, is one).</exception>
		// Token: 0x06006614 RID: 26132 RVA: 0x0015D089 File Offset: 0x0015B289
		public virtual void WriteLine(string format, object arg0)
		{
			this.WriteLine(string.Format(this.FormatProvider, format, arg0));
		}

		/// <summary>Writes a formatted string and a new line to the text string or stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object,System.Object)" /> method.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to format and write.</param>
		/// <param name="arg1">The second object to format and write.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is not a valid composite format string.  
		/// -or-  
		/// The index of a format item is less than 0 (zero), or greater than or equal to the number of objects to be formatted (which, for this method overload, is two).</exception>
		// Token: 0x06006615 RID: 26133 RVA: 0x0015D09E File Offset: 0x0015B29E
		public virtual void WriteLine(string format, object arg0, object arg1)
		{
			this.WriteLine(string.Format(this.FormatProvider, format, arg0, arg1));
		}

		/// <summary>Writes out a formatted string and a new line, using the same semantics as <see cref="M:System.String.Format(System.String,System.Object)" />.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to format and write.</param>
		/// <param name="arg1">The second object to format and write.</param>
		/// <param name="arg2">The third object to format and write.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is not a valid composite format string.  
		/// -or-  
		/// The index of a format item is less than 0 (zero), or greater than or equal to the number of objects to be formatted (which, for this method overload, is three).</exception>
		// Token: 0x06006616 RID: 26134 RVA: 0x0015D0B4 File Offset: 0x0015B2B4
		public virtual void WriteLine(string format, object arg0, object arg1, object arg2)
		{
			this.WriteLine(string.Format(this.FormatProvider, format, arg0, arg1, arg2));
		}

		/// <summary>Writes out a formatted string and a new line, using the same semantics as <see cref="M:System.String.Format(System.String,System.Object)" />.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg">An object array that contains zero or more objects to format and write.</param>
		/// <exception cref="T:System.ArgumentNullException">A string or object is passed in as <see langword="null" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is not a valid composite format string.  
		/// -or-  
		/// The index of a format item is less than 0 (zero), or greater than or equal to the length of the <paramref name="arg" /> array.</exception>
		// Token: 0x06006617 RID: 26135 RVA: 0x0015D0CC File Offset: 0x0015B2CC
		public virtual void WriteLine(string format, params object[] arg)
		{
			this.WriteLine(string.Format(this.FormatProvider, format, arg));
		}

		/// <summary>Writes a character to the text string or stream asynchronously.</summary>
		/// <param name="value">The character to write to the text stream.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The text writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The text writer is currently in use by a previous write operation.</exception>
		// Token: 0x06006618 RID: 26136 RVA: 0x0015D0E4 File Offset: 0x0015B2E4
		public virtual Task WriteAsync(char value)
		{
			Tuple<TextWriter, char> tuple = new Tuple<TextWriter, char>(this, value);
			return Task.Factory.StartNew(delegate(object state)
			{
				Tuple<TextWriter, char> tuple2 = (Tuple<TextWriter, char>)state;
				tuple2.Item1.Write(tuple2.Item2);
			}, tuple, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		/// <summary>Writes a string to the text string or stream asynchronously.</summary>
		/// <param name="value">The string to write. If <paramref name="value" /> is <see langword="null" />, nothing is written to the text stream.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The text writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The text writer is currently in use by a previous write operation.</exception>
		// Token: 0x06006619 RID: 26137 RVA: 0x0015D130 File Offset: 0x0015B330
		public virtual Task WriteAsync(string value)
		{
			Tuple<TextWriter, string> tuple = new Tuple<TextWriter, string>(this, value);
			return Task.Factory.StartNew(delegate(object state)
			{
				Tuple<TextWriter, string> tuple2 = (Tuple<TextWriter, string>)state;
				tuple2.Item1.Write(tuple2.Item2);
			}, tuple, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		/// <summary>Writes a character array to the text string or stream asynchronously.</summary>
		/// <param name="buffer">The character array to write to the text stream. If <paramref name="buffer" /> is <see langword="null" />, nothing is written.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The text writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The text writer is currently in use by a previous write operation.</exception>
		// Token: 0x0600661A RID: 26138 RVA: 0x0015D17A File Offset: 0x0015B37A
		public Task WriteAsync(char[] buffer)
		{
			if (buffer == null)
			{
				return Task.CompletedTask;
			}
			return this.WriteAsync(buffer, 0, buffer.Length);
		}

		/// <summary>Writes a subarray of characters to the text string or stream asynchronously.</summary>
		/// <param name="buffer">The character array to write data from.</param>
		/// <param name="index">The character position in the buffer at which to start retrieving data.</param>
		/// <param name="count">The number of characters to write.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="index" /> plus <paramref name="count" /> is greater than the buffer length.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The text writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The text writer is currently in use by a previous write operation.</exception>
		// Token: 0x0600661B RID: 26139 RVA: 0x0015D190 File Offset: 0x0015B390
		public virtual Task WriteAsync(char[] buffer, int index, int count)
		{
			Tuple<TextWriter, char[], int, int> tuple = new Tuple<TextWriter, char[], int, int>(this, buffer, index, count);
			return Task.Factory.StartNew(delegate(object state)
			{
				Tuple<TextWriter, char[], int, int> tuple2 = (Tuple<TextWriter, char[], int, int>)state;
				tuple2.Item1.Write(tuple2.Item2, tuple2.Item3, tuple2.Item4);
			}, tuple, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		// Token: 0x0600661C RID: 26140 RVA: 0x0015D1DC File Offset: 0x0015B3DC
		public virtual Task WriteAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			ArraySegment<char> arraySegment;
			if (!MemoryMarshal.TryGetArray<char>(buffer, out arraySegment))
			{
				return Task.Factory.StartNew(delegate(object state)
				{
					Tuple<TextWriter, ReadOnlyMemory<char>> tuple = (Tuple<TextWriter, ReadOnlyMemory<char>>)state;
					tuple.Item1.Write(tuple.Item2.Span);
				}, Tuple.Create<TextWriter, ReadOnlyMemory<char>>(this, buffer), cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
			}
			return this.WriteAsync(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
		}

		/// <summary>Writes a character followed by a line terminator asynchronously to the text string or stream.</summary>
		/// <param name="value">The character to write to the text stream.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The text writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The text writer is currently in use by a previous write operation.</exception>
		// Token: 0x0600661D RID: 26141 RVA: 0x0015D248 File Offset: 0x0015B448
		public virtual Task WriteLineAsync(char value)
		{
			Tuple<TextWriter, char> tuple = new Tuple<TextWriter, char>(this, value);
			return Task.Factory.StartNew(delegate(object state)
			{
				Tuple<TextWriter, char> tuple2 = (Tuple<TextWriter, char>)state;
				tuple2.Item1.WriteLine(tuple2.Item2);
			}, tuple, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		/// <summary>Writes a string followed by a line terminator asynchronously to the text string or stream.</summary>
		/// <param name="value">The string to write. If the value is <see langword="null" />, only a line terminator is written.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The text writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The text writer is currently in use by a previous write operation.</exception>
		// Token: 0x0600661E RID: 26142 RVA: 0x0015D294 File Offset: 0x0015B494
		public virtual Task WriteLineAsync(string value)
		{
			Tuple<TextWriter, string> tuple = new Tuple<TextWriter, string>(this, value);
			return Task.Factory.StartNew(delegate(object state)
			{
				Tuple<TextWriter, string> tuple2 = (Tuple<TextWriter, string>)state;
				tuple2.Item1.WriteLine(tuple2.Item2);
			}, tuple, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		/// <summary>Writes an array of characters followed by a line terminator asynchronously to the text string or stream.</summary>
		/// <param name="buffer">The character array to write to the text stream. If the character array is <see langword="null" />, only the line terminator is written.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The text writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The text writer is currently in use by a previous write operation.</exception>
		// Token: 0x0600661F RID: 26143 RVA: 0x0015D2DE File Offset: 0x0015B4DE
		public Task WriteLineAsync(char[] buffer)
		{
			if (buffer == null)
			{
				return this.WriteLineAsync();
			}
			return this.WriteLineAsync(buffer, 0, buffer.Length);
		}

		/// <summary>Writes a subarray of characters followed by a line terminator asynchronously to the text string or stream.</summary>
		/// <param name="buffer">The character array to write data from.</param>
		/// <param name="index">The character position in the buffer at which to start retrieving data.</param>
		/// <param name="count">The number of characters to write.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="index" /> plus <paramref name="count" /> is greater than the buffer length.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The text writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The text writer is currently in use by a previous write operation.</exception>
		// Token: 0x06006620 RID: 26144 RVA: 0x0015D2F8 File Offset: 0x0015B4F8
		public virtual Task WriteLineAsync(char[] buffer, int index, int count)
		{
			Tuple<TextWriter, char[], int, int> tuple = new Tuple<TextWriter, char[], int, int>(this, buffer, index, count);
			return Task.Factory.StartNew(delegate(object state)
			{
				Tuple<TextWriter, char[], int, int> tuple2 = (Tuple<TextWriter, char[], int, int>)state;
				tuple2.Item1.WriteLine(tuple2.Item2, tuple2.Item3, tuple2.Item4);
			}, tuple, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		// Token: 0x06006621 RID: 26145 RVA: 0x0015D344 File Offset: 0x0015B544
		public virtual Task WriteLineAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			ArraySegment<char> arraySegment;
			if (!MemoryMarshal.TryGetArray<char>(buffer, out arraySegment))
			{
				return Task.Factory.StartNew(delegate(object state)
				{
					Tuple<TextWriter, ReadOnlyMemory<char>> tuple = (Tuple<TextWriter, ReadOnlyMemory<char>>)state;
					tuple.Item1.WriteLine(tuple.Item2.Span);
				}, Tuple.Create<TextWriter, ReadOnlyMemory<char>>(this, buffer), cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
			}
			return this.WriteLineAsync(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
		}

		/// <summary>Writes a line terminator asynchronously to the text string or stream.</summary>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The text writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The text writer is currently in use by a previous write operation.</exception>
		// Token: 0x06006622 RID: 26146 RVA: 0x0015D3AE File Offset: 0x0015B5AE
		public virtual Task WriteLineAsync()
		{
			return this.WriteAsync(this.CoreNewLine);
		}

		/// <summary>Asynchronously clears all buffers for the current writer and causes any buffered data to be written to the underlying device.</summary>
		/// <returns>A task that represents the asynchronous flush operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The text writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The writer is currently in use by a previous write operation.</exception>
		// Token: 0x06006623 RID: 26147 RVA: 0x0015D3BC File Offset: 0x0015B5BC
		public virtual Task FlushAsync()
		{
			return Task.Factory.StartNew(delegate(object state)
			{
				((TextWriter)state).Flush();
			}, this, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		/// <summary>Creates a thread-safe wrapper around the specified <see langword="TextWriter" />.</summary>
		/// <param name="writer">The <see langword="TextWriter" /> to synchronize.</param>
		/// <returns>A thread-safe wrapper.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="writer" /> is <see langword="null" />.</exception>
		// Token: 0x06006624 RID: 26148 RVA: 0x0015D3F3 File Offset: 0x0015B5F3
		public static TextWriter Synchronized(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (!(writer is TextWriter.SyncTextWriter))
			{
				return new TextWriter.SyncTextWriter(writer);
			}
			return writer;
		}

		/// <summary>Provides a <see langword="TextWriter" /> with no backing store that can be written to, but not read from.</summary>
		// Token: 0x04003BFD RID: 15357
		public static readonly TextWriter Null = new TextWriter.NullTextWriter();

		// Token: 0x04003BFE RID: 15358
		private static readonly char[] s_coreNewLine = Environment.NewLine.ToCharArray();

		/// <summary>Stores the newline characters used for this <see langword="TextWriter" />.</summary>
		// Token: 0x04003BFF RID: 15359
		protected char[] CoreNewLine = TextWriter.s_coreNewLine;

		// Token: 0x04003C00 RID: 15360
		private string CoreNewLineStr = Environment.NewLine;

		// Token: 0x04003C01 RID: 15361
		private IFormatProvider _internalFormatProvider;

		// Token: 0x02000B25 RID: 2853
		[Serializable]
		private sealed class NullTextWriter : TextWriter
		{
			// Token: 0x06006626 RID: 26150 RVA: 0x0015D42E File Offset: 0x0015B62E
			internal NullTextWriter()
				: base(CultureInfo.InvariantCulture)
			{
			}

			// Token: 0x170011CA RID: 4554
			// (get) Token: 0x06006627 RID: 26151 RVA: 0x001598AD File Offset: 0x00157AAD
			public override Encoding Encoding
			{
				get
				{
					return Encoding.Unicode;
				}
			}

			// Token: 0x06006628 RID: 26152 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public override void Write(char[] buffer, int index, int count)
			{
			}

			// Token: 0x06006629 RID: 26153 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public override void Write(string value)
			{
			}

			// Token: 0x0600662A RID: 26154 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public override void WriteLine()
			{
			}

			// Token: 0x0600662B RID: 26155 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public override void WriteLine(string value)
			{
			}

			// Token: 0x0600662C RID: 26156 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public override void WriteLine(object value)
			{
			}

			// Token: 0x0600662D RID: 26157 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public override void Write(char value)
			{
			}
		}

		// Token: 0x02000B26 RID: 2854
		[Serializable]
		internal sealed class SyncTextWriter : TextWriter, IDisposable
		{
			// Token: 0x0600662E RID: 26158 RVA: 0x0015D43B File Offset: 0x0015B63B
			internal SyncTextWriter(TextWriter t)
				: base(t.FormatProvider)
			{
				this._out = t;
			}

			// Token: 0x170011CB RID: 4555
			// (get) Token: 0x0600662F RID: 26159 RVA: 0x0015D450 File Offset: 0x0015B650
			public override Encoding Encoding
			{
				get
				{
					return this._out.Encoding;
				}
			}

			// Token: 0x170011CC RID: 4556
			// (get) Token: 0x06006630 RID: 26160 RVA: 0x0015D45D File Offset: 0x0015B65D
			public override IFormatProvider FormatProvider
			{
				get
				{
					return this._out.FormatProvider;
				}
			}

			// Token: 0x170011CD RID: 4557
			// (get) Token: 0x06006631 RID: 26161 RVA: 0x0015D46A File Offset: 0x0015B66A
			// (set) Token: 0x06006632 RID: 26162 RVA: 0x0015D477 File Offset: 0x0015B677
			public override string NewLine
			{
				[MethodImpl(MethodImplOptions.Synchronized)]
				get
				{
					return this._out.NewLine;
				}
				[MethodImpl(MethodImplOptions.Synchronized)]
				set
				{
					this._out.NewLine = value;
				}
			}

			// Token: 0x06006633 RID: 26163 RVA: 0x0015D485 File Offset: 0x0015B685
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Close()
			{
				this._out.Close();
			}

			// Token: 0x06006634 RID: 26164 RVA: 0x0015D492 File Offset: 0x0015B692
			[MethodImpl(MethodImplOptions.Synchronized)]
			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					((IDisposable)this._out).Dispose();
				}
			}

			// Token: 0x06006635 RID: 26165 RVA: 0x0015D4A2 File Offset: 0x0015B6A2
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Flush()
			{
				this._out.Flush();
			}

			// Token: 0x06006636 RID: 26166 RVA: 0x0015D4AF File Offset: 0x0015B6AF
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(char value)
			{
				this._out.Write(value);
			}

			// Token: 0x06006637 RID: 26167 RVA: 0x0015D4BD File Offset: 0x0015B6BD
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(char[] buffer)
			{
				this._out.Write(buffer);
			}

			// Token: 0x06006638 RID: 26168 RVA: 0x0015D4CB File Offset: 0x0015B6CB
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(char[] buffer, int index, int count)
			{
				this._out.Write(buffer, index, count);
			}

			// Token: 0x06006639 RID: 26169 RVA: 0x0015D4DB File Offset: 0x0015B6DB
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(bool value)
			{
				this._out.Write(value);
			}

			// Token: 0x0600663A RID: 26170 RVA: 0x0015D4E9 File Offset: 0x0015B6E9
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(int value)
			{
				this._out.Write(value);
			}

			// Token: 0x0600663B RID: 26171 RVA: 0x0015D4F7 File Offset: 0x0015B6F7
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(uint value)
			{
				this._out.Write(value);
			}

			// Token: 0x0600663C RID: 26172 RVA: 0x0015D505 File Offset: 0x0015B705
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(long value)
			{
				this._out.Write(value);
			}

			// Token: 0x0600663D RID: 26173 RVA: 0x0015D513 File Offset: 0x0015B713
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(ulong value)
			{
				this._out.Write(value);
			}

			// Token: 0x0600663E RID: 26174 RVA: 0x0015D521 File Offset: 0x0015B721
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(float value)
			{
				this._out.Write(value);
			}

			// Token: 0x0600663F RID: 26175 RVA: 0x0015D52F File Offset: 0x0015B72F
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(double value)
			{
				this._out.Write(value);
			}

			// Token: 0x06006640 RID: 26176 RVA: 0x0015D53D File Offset: 0x0015B73D
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(decimal value)
			{
				this._out.Write(value);
			}

			// Token: 0x06006641 RID: 26177 RVA: 0x0015D54B File Offset: 0x0015B74B
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(string value)
			{
				this._out.Write(value);
			}

			// Token: 0x06006642 RID: 26178 RVA: 0x0015D559 File Offset: 0x0015B759
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(object value)
			{
				this._out.Write(value);
			}

			// Token: 0x06006643 RID: 26179 RVA: 0x0015D567 File Offset: 0x0015B767
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(string format, object arg0)
			{
				this._out.Write(format, arg0);
			}

			// Token: 0x06006644 RID: 26180 RVA: 0x0015D576 File Offset: 0x0015B776
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(string format, object arg0, object arg1)
			{
				this._out.Write(format, arg0, arg1);
			}

			// Token: 0x06006645 RID: 26181 RVA: 0x0015D586 File Offset: 0x0015B786
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(string format, object arg0, object arg1, object arg2)
			{
				this._out.Write(format, arg0, arg1, arg2);
			}

			// Token: 0x06006646 RID: 26182 RVA: 0x0015D598 File Offset: 0x0015B798
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(string format, params object[] arg)
			{
				this._out.Write(format, arg);
			}

			// Token: 0x06006647 RID: 26183 RVA: 0x0015D5A7 File Offset: 0x0015B7A7
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine()
			{
				this._out.WriteLine();
			}

			// Token: 0x06006648 RID: 26184 RVA: 0x0015D5B4 File Offset: 0x0015B7B4
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(char value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06006649 RID: 26185 RVA: 0x0015D5C2 File Offset: 0x0015B7C2
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(decimal value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x0600664A RID: 26186 RVA: 0x0015D5D0 File Offset: 0x0015B7D0
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(char[] buffer)
			{
				this._out.WriteLine(buffer);
			}

			// Token: 0x0600664B RID: 26187 RVA: 0x0015D5DE File Offset: 0x0015B7DE
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(char[] buffer, int index, int count)
			{
				this._out.WriteLine(buffer, index, count);
			}

			// Token: 0x0600664C RID: 26188 RVA: 0x0015D5EE File Offset: 0x0015B7EE
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(bool value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x0600664D RID: 26189 RVA: 0x0015D5FC File Offset: 0x0015B7FC
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(int value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x0600664E RID: 26190 RVA: 0x0015D60A File Offset: 0x0015B80A
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(uint value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x0600664F RID: 26191 RVA: 0x0015D618 File Offset: 0x0015B818
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(long value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06006650 RID: 26192 RVA: 0x0015D626 File Offset: 0x0015B826
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(ulong value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06006651 RID: 26193 RVA: 0x0015D634 File Offset: 0x0015B834
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(float value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06006652 RID: 26194 RVA: 0x0015D642 File Offset: 0x0015B842
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(double value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06006653 RID: 26195 RVA: 0x0015D650 File Offset: 0x0015B850
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(string value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06006654 RID: 26196 RVA: 0x0015D65E File Offset: 0x0015B85E
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(object value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06006655 RID: 26197 RVA: 0x0015D66C File Offset: 0x0015B86C
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(string format, object arg0)
			{
				this._out.WriteLine(format, arg0);
			}

			// Token: 0x06006656 RID: 26198 RVA: 0x0015D67B File Offset: 0x0015B87B
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(string format, object arg0, object arg1)
			{
				this._out.WriteLine(format, arg0, arg1);
			}

			// Token: 0x06006657 RID: 26199 RVA: 0x0015D68B File Offset: 0x0015B88B
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(string format, object arg0, object arg1, object arg2)
			{
				this._out.WriteLine(format, arg0, arg1, arg2);
			}

			// Token: 0x06006658 RID: 26200 RVA: 0x0015D69D File Offset: 0x0015B89D
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(string format, params object[] arg)
			{
				this._out.WriteLine(format, arg);
			}

			// Token: 0x06006659 RID: 26201 RVA: 0x0015D6AC File Offset: 0x0015B8AC
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override Task WriteAsync(char value)
			{
				this.Write(value);
				return Task.CompletedTask;
			}

			// Token: 0x0600665A RID: 26202 RVA: 0x0015D6BA File Offset: 0x0015B8BA
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override Task WriteAsync(string value)
			{
				this.Write(value);
				return Task.CompletedTask;
			}

			// Token: 0x0600665B RID: 26203 RVA: 0x0015D6C8 File Offset: 0x0015B8C8
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override Task WriteAsync(char[] buffer, int index, int count)
			{
				this.Write(buffer, index, count);
				return Task.CompletedTask;
			}

			// Token: 0x0600665C RID: 26204 RVA: 0x0015D6D8 File Offset: 0x0015B8D8
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override Task WriteLineAsync(char value)
			{
				this.WriteLine(value);
				return Task.CompletedTask;
			}

			// Token: 0x0600665D RID: 26205 RVA: 0x0015D6E6 File Offset: 0x0015B8E6
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override Task WriteLineAsync(string value)
			{
				this.WriteLine(value);
				return Task.CompletedTask;
			}

			// Token: 0x0600665E RID: 26206 RVA: 0x0015D6F4 File Offset: 0x0015B8F4
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override Task WriteLineAsync(char[] buffer, int index, int count)
			{
				this.WriteLine(buffer, index, count);
				return Task.CompletedTask;
			}

			// Token: 0x0600665F RID: 26207 RVA: 0x0015D704 File Offset: 0x0015B904
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override Task FlushAsync()
			{
				this.Flush();
				return Task.CompletedTask;
			}

			// Token: 0x04003C02 RID: 15362
			private readonly TextWriter _out;
		}
	}
}
