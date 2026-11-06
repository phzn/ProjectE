using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
	/// <summary>Implements a <see cref="T:System.IO.TextReader" /> that reads characters from a byte stream in a particular encoding.</summary>
	// Token: 0x02000B12 RID: 2834
	[Serializable]
	public class StreamReader : TextReader
	{
		// Token: 0x06006537 RID: 25911 RVA: 0x00158657 File Offset: 0x00156857
		private void CheckAsyncTaskInProgress()
		{
			if (!this._asyncReadTask.IsCompleted)
			{
				StreamReader.ThrowAsyncIOInProgress();
			}
		}

		// Token: 0x06006538 RID: 25912 RVA: 0x0015866B File Offset: 0x0015686B
		private static void ThrowAsyncIOInProgress()
		{
			throw new InvalidOperationException("The stream is currently in use by a previous operation on the stream.");
		}

		// Token: 0x06006539 RID: 25913 RVA: 0x00158677 File Offset: 0x00156877
		internal StreamReader()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified stream.</summary>
		/// <param name="stream">The stream to be read.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="stream" /> does not support reading.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> is <see langword="null" />.</exception>
		// Token: 0x0600653A RID: 25914 RVA: 0x0015868A File Offset: 0x0015688A
		public StreamReader(Stream stream)
			: this(stream, true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified stream, with the specified byte order mark detection option.</summary>
		/// <param name="stream">The stream to be read.</param>
		/// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="stream" /> does not support reading.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> is <see langword="null" />.</exception>
		// Token: 0x0600653B RID: 25915 RVA: 0x00158694 File Offset: 0x00156894
		public StreamReader(Stream stream, bool detectEncodingFromByteOrderMarks)
			: this(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks, 1024, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified stream, with the specified character encoding.</summary>
		/// <param name="stream">The stream to be read.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="stream" /> does not support reading.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
		// Token: 0x0600653C RID: 25916 RVA: 0x001586A9 File Offset: 0x001568A9
		public StreamReader(Stream stream, Encoding encoding)
			: this(stream, encoding, true, 1024, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified stream, with the specified character encoding and byte order mark detection option.</summary>
		/// <param name="stream">The stream to be read.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="stream" /> does not support reading.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
		// Token: 0x0600653D RID: 25917 RVA: 0x001586BA File Offset: 0x001568BA
		public StreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
			: this(stream, encoding, detectEncodingFromByteOrderMarks, 1024, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified stream, with the specified character encoding, byte order mark detection option, and buffer size.</summary>
		/// <param name="stream">The stream to be read.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
		/// <param name="bufferSize">The minimum buffer size.</param>
		/// <exception cref="T:System.ArgumentException">The stream does not support reading.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bufferSize" /> is less than or equal to zero.</exception>
		// Token: 0x0600653E RID: 25918 RVA: 0x001586CB File Offset: 0x001568CB
		public StreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
			: this(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified stream based on the specified character encoding, byte order mark detection option, and buffer size, and optionally leaves the stream open.</summary>
		/// <param name="stream">The stream to read.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="detectEncodingFromByteOrderMarks">
		///   <see langword="true" /> to look for byte order marks at the beginning of the file; otherwise, <see langword="false" />.</param>
		/// <param name="bufferSize">The minimum buffer size.</param>
		/// <param name="leaveOpen">
		///   <see langword="true" /> to leave the stream open after the <see cref="T:System.IO.StreamReader" /> object is disposed; otherwise, <see langword="false" />.</param>
		// Token: 0x0600653F RID: 25919 RVA: 0x001586DC File Offset: 0x001568DC
		public StreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool leaveOpen)
		{
			if (stream == null || encoding == null)
			{
				throw new ArgumentNullException((stream == null) ? "stream" : "encoding");
			}
			if (!stream.CanRead)
			{
				throw new ArgumentException("Stream was not readable.");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", "Positive number required.");
			}
			this.Init(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize, leaveOpen);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified file name.</summary>
		/// <param name="path">The complete file path to be read.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is an empty string ("").</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
		// Token: 0x06006540 RID: 25920 RVA: 0x0015874A File Offset: 0x0015694A
		public StreamReader(string path)
			: this(path, true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified file name, with the specified byte order mark detection option.</summary>
		/// <param name="path">The complete file path to be read.</param>
		/// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is an empty string ("").</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
		// Token: 0x06006541 RID: 25921 RVA: 0x00158754 File Offset: 0x00156954
		public StreamReader(string path, bool detectEncodingFromByteOrderMarks)
			: this(path, Encoding.UTF8, detectEncodingFromByteOrderMarks, 1024)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified file name, with the specified character encoding.</summary>
		/// <param name="path">The complete file path to be read.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is an empty string ("").</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
		// Token: 0x06006542 RID: 25922 RVA: 0x00158768 File Offset: 0x00156968
		public StreamReader(string path, Encoding encoding)
			: this(path, encoding, true, 1024)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified file name, with the specified character encoding and byte order mark detection option.</summary>
		/// <param name="path">The complete file path to be read.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is an empty string ("").</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
		// Token: 0x06006543 RID: 25923 RVA: 0x00158778 File Offset: 0x00156978
		public StreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
			: this(path, encoding, detectEncodingFromByteOrderMarks, 1024)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified file name, with the specified character encoding, byte order mark detection option, and buffer size.</summary>
		/// <param name="path">The complete file path to be read.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
		/// <param name="bufferSize">The minimum buffer size, in number of 16-bit characters.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is an empty string ("").</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="buffersize" /> is less than or equal to zero.</exception>
		// Token: 0x06006544 RID: 25924 RVA: 0x00158788 File Offset: 0x00156988
		public StreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", "Positive number required.");
			}
			Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan);
			this.Init(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize, false);
		}

		// Token: 0x06006545 RID: 25925 RVA: 0x0015880C File Offset: 0x00156A0C
		private void Init(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool leaveOpen)
		{
			this._stream = stream;
			this._encoding = encoding;
			this._decoder = encoding.GetDecoder();
			if (bufferSize < 128)
			{
				bufferSize = 128;
			}
			this._byteBuffer = new byte[bufferSize];
			this._maxCharsPerBuffer = encoding.GetMaxCharCount(bufferSize);
			this._charBuffer = new char[this._maxCharsPerBuffer];
			this._byteLen = 0;
			this._bytePos = 0;
			this._detectEncoding = detectEncodingFromByteOrderMarks;
			this._checkPreamble = encoding.Preamble.Length > 0;
			this._isBlocked = false;
			this._closable = !leaveOpen;
		}

		// Token: 0x06006546 RID: 25926 RVA: 0x001588AD File Offset: 0x00156AAD
		internal void Init(Stream stream)
		{
			this._stream = stream;
			this._closable = true;
		}

		/// <summary>Closes the <see cref="T:System.IO.StreamReader" /> object and the underlying stream, and releases any system resources associated with the reader.</summary>
		// Token: 0x06006547 RID: 25927 RVA: 0x001588BD File Offset: 0x00156ABD
		public override void Close()
		{
			this.Dispose(true);
		}

		/// <summary>Closes the underlying stream, releases the unmanaged resources used by the <see cref="T:System.IO.StreamReader" />, and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x06006548 RID: 25928 RVA: 0x001588C8 File Offset: 0x00156AC8
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this.LeaveOpen && disposing && this._stream != null)
				{
					this._stream.Close();
				}
			}
			finally
			{
				if (!this.LeaveOpen && this._stream != null)
				{
					this._stream = null;
					this._encoding = null;
					this._decoder = null;
					this._byteBuffer = null;
					this._charBuffer = null;
					this._charPos = 0;
					this._charLen = 0;
					base.Dispose(disposing);
				}
			}
		}

		/// <summary>Gets the current character encoding that the current <see cref="T:System.IO.StreamReader" /> object is using.</summary>
		/// <returns>The current character encoding used by the current reader. The value can be different after the first call to any <see cref="Overload:System.IO.StreamReader.Read" /> method of <see cref="T:System.IO.StreamReader" />, since encoding autodetection is not done until the first call to a <see cref="Overload:System.IO.StreamReader.Read" /> method.</returns>
		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x06006549 RID: 25929 RVA: 0x00158950 File Offset: 0x00156B50
		public virtual Encoding CurrentEncoding
		{
			get
			{
				return this._encoding;
			}
		}

		/// <summary>Returns the underlying stream.</summary>
		/// <returns>The underlying stream.</returns>
		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x0600654A RID: 25930 RVA: 0x00158958 File Offset: 0x00156B58
		public virtual Stream BaseStream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x0600654B RID: 25931 RVA: 0x00158960 File Offset: 0x00156B60
		internal bool LeaveOpen
		{
			get
			{
				return !this._closable;
			}
		}

		/// <summary>Clears the internal buffer.</summary>
		// Token: 0x0600654C RID: 25932 RVA: 0x0015896B File Offset: 0x00156B6B
		public void DiscardBufferedData()
		{
			this.CheckAsyncTaskInProgress();
			this._byteLen = 0;
			this._charLen = 0;
			this._charPos = 0;
			if (this._encoding != null)
			{
				this._decoder = this._encoding.GetDecoder();
			}
			this._isBlocked = false;
		}

		/// <summary>Gets a value that indicates whether the current stream position is at the end of the stream.</summary>
		/// <returns>
		///   <see langword="true" /> if the current stream position is at the end of the stream; otherwise <see langword="false" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The underlying stream has been disposed.</exception>
		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x0600654D RID: 25933 RVA: 0x001589A8 File Offset: 0x00156BA8
		public bool EndOfStream
		{
			get
			{
				if (this._stream == null)
				{
					throw new ObjectDisposedException(null, "Cannot read from a closed TextReader.");
				}
				this.CheckAsyncTaskInProgress();
				return this._charPos >= this._charLen && this.ReadBuffer() == 0;
			}
		}

		/// <summary>Returns the next available character but does not consume it.</summary>
		/// <returns>An integer representing the next character to be read, or -1 if there are no characters to be read or if the stream does not support seeking.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x0600654E RID: 25934 RVA: 0x001589E0 File Offset: 0x00156BE0
		public override int Peek()
		{
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Cannot read from a closed TextReader.");
			}
			this.CheckAsyncTaskInProgress();
			if (this._charPos == this._charLen && (this._isBlocked || this.ReadBuffer() == 0))
			{
				return -1;
			}
			return (int)this._charBuffer[this._charPos];
		}

		/// <summary>Reads the next character from the input stream and advances the character position by one character.</summary>
		/// <returns>The next character from the input stream represented as an <see cref="T:System.Int32" /> object, or -1 if no more characters are available.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x0600654F RID: 25935 RVA: 0x00158A34 File Offset: 0x00156C34
		public override int Read()
		{
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Cannot read from a closed TextReader.");
			}
			this.CheckAsyncTaskInProgress();
			if (this._charPos == this._charLen && this.ReadBuffer() == 0)
			{
				return -1;
			}
			int num = (int)this._charBuffer[this._charPos];
			this._charPos++;
			return num;
		}

		/// <summary>Reads a specified maximum of characters from the current stream into a buffer, beginning at the specified index.</summary>
		/// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (index + count - 1) replaced by the characters read from the current source.</param>
		/// <param name="index">The index of <paramref name="buffer" /> at which to begin writing.</param>
		/// <param name="count">The maximum number of characters to read.</param>
		/// <returns>The number of characters that have been read, or 0 if at the end of the stream and no data was read. The number will be less than or equal to the <paramref name="count" /> parameter, depending on whether the data is available within the stream.</returns>
		/// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs, such as the stream is closed.</exception>
		// Token: 0x06006550 RID: 25936 RVA: 0x00158A90 File Offset: 0x00156C90
		public override int Read(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", "Non-negative number required.");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			return this.ReadSpan(new Span<char>(buffer, index, count));
		}

		// Token: 0x06006551 RID: 25937 RVA: 0x00158AF4 File Offset: 0x00156CF4
		public override int Read(Span<char> buffer)
		{
			if (!(base.GetType() == typeof(StreamReader)))
			{
				return base.Read(buffer);
			}
			return this.ReadSpan(buffer);
		}

		// Token: 0x06006552 RID: 25938 RVA: 0x00158B1C File Offset: 0x00156D1C
		private int ReadSpan(Span<char> buffer)
		{
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Cannot read from a closed TextReader.");
			}
			this.CheckAsyncTaskInProgress();
			int num = 0;
			bool flag = false;
			int i = buffer.Length;
			while (i > 0)
			{
				int num2 = this._charLen - this._charPos;
				if (num2 == 0)
				{
					num2 = this.ReadBuffer(buffer.Slice(num), out flag);
				}
				if (num2 == 0)
				{
					break;
				}
				if (num2 > i)
				{
					num2 = i;
				}
				if (!flag)
				{
					new Span<char>(this._charBuffer, this._charPos, num2).CopyTo(buffer.Slice(num));
					this._charPos += num2;
				}
				num += num2;
				i -= num2;
				if (this._isBlocked)
				{
					break;
				}
			}
			return num;
		}

		/// <summary>Reads all characters from the current position to the end of the stream.</summary>
		/// <returns>The rest of the stream as a string, from the current position to the end. If the current position is at the end of the stream, returns an empty string ("").</returns>
		/// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x06006553 RID: 25939 RVA: 0x00158BC8 File Offset: 0x00156DC8
		public override string ReadToEnd()
		{
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Cannot read from a closed TextReader.");
			}
			this.CheckAsyncTaskInProgress();
			StringBuilder stringBuilder = new StringBuilder(this._charLen - this._charPos);
			do
			{
				stringBuilder.Append(this._charBuffer, this._charPos, this._charLen - this._charPos);
				this._charPos = this._charLen;
				this.ReadBuffer();
			}
			while (this._charLen > 0);
			return stringBuilder.ToString();
		}

		/// <summary>Reads a specified maximum number of characters from the current stream and writes the data to a buffer, beginning at the specified index.</summary>
		/// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (index + count - 1) replaced by the characters read from the current source.</param>
		/// <param name="index">The position in <paramref name="buffer" /> at which to begin writing.</param>
		/// <param name="count">The maximum number of characters to read.</param>
		/// <returns>The number of characters that have been read. The number will be less than or equal to <paramref name="count" />, depending on whether all input characters have been read.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.StreamReader" /> is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x06006554 RID: 25940 RVA: 0x00158C44 File Offset: 0x00156E44
		public override int ReadBlock(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", "Non-negative number required.");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Cannot read from a closed TextReader.");
			}
			this.CheckAsyncTaskInProgress();
			return base.ReadBlock(buffer, index, count);
		}

		// Token: 0x06006555 RID: 25941 RVA: 0x00158CC0 File Offset: 0x00156EC0
		public override int ReadBlock(Span<char> buffer)
		{
			if (base.GetType() != typeof(StreamReader))
			{
				return base.ReadBlock(buffer);
			}
			int num = 0;
			int num2;
			do
			{
				num2 = this.ReadSpan(buffer.Slice(num));
				num += num2;
			}
			while (num2 > 0 && num < buffer.Length);
			return num;
		}

		// Token: 0x06006556 RID: 25942 RVA: 0x00158D10 File Offset: 0x00156F10
		private void CompressBuffer(int n)
		{
			Buffer.BlockCopy(this._byteBuffer, n, this._byteBuffer, 0, this._byteLen - n);
			this._byteLen -= n;
		}

		// Token: 0x06006557 RID: 25943 RVA: 0x00158D3C File Offset: 0x00156F3C
		private void DetectEncoding()
		{
			if (this._byteLen < 2)
			{
				return;
			}
			this._detectEncoding = false;
			bool flag = false;
			if (this._byteBuffer[0] == 254 && this._byteBuffer[1] == 255)
			{
				this._encoding = Encoding.BigEndianUnicode;
				this.CompressBuffer(2);
				flag = true;
			}
			else if (this._byteBuffer[0] == 255 && this._byteBuffer[1] == 254)
			{
				if (this._byteLen < 4 || this._byteBuffer[2] != 0 || this._byteBuffer[3] != 0)
				{
					this._encoding = Encoding.Unicode;
					this.CompressBuffer(2);
					flag = true;
				}
				else
				{
					this._encoding = Encoding.UTF32;
					this.CompressBuffer(4);
					flag = true;
				}
			}
			else if (this._byteLen >= 3 && this._byteBuffer[0] == 239 && this._byteBuffer[1] == 187 && this._byteBuffer[2] == 191)
			{
				this._encoding = Encoding.UTF8;
				this.CompressBuffer(3);
				flag = true;
			}
			else if (this._byteLen >= 4 && this._byteBuffer[0] == 0 && this._byteBuffer[1] == 0 && this._byteBuffer[2] == 254 && this._byteBuffer[3] == 255)
			{
				this._encoding = new UTF32Encoding(true, true);
				this.CompressBuffer(4);
				flag = true;
			}
			else if (this._byteLen == 2)
			{
				this._detectEncoding = true;
			}
			if (flag)
			{
				this._decoder = this._encoding.GetDecoder();
				int maxCharCount = this._encoding.GetMaxCharCount(this._byteBuffer.Length);
				if (maxCharCount > this._maxCharsPerBuffer)
				{
					this._charBuffer = new char[maxCharCount];
				}
				this._maxCharsPerBuffer = maxCharCount;
			}
		}

		// Token: 0x06006558 RID: 25944 RVA: 0x00158EF4 File Offset: 0x001570F4
		private unsafe bool IsPreamble()
		{
			if (!this._checkPreamble)
			{
				return this._checkPreamble;
			}
			ReadOnlySpan<byte> preamble = this._encoding.Preamble;
			int num = ((this._byteLen >= preamble.Length) ? (preamble.Length - this._bytePos) : (this._byteLen - this._bytePos));
			int i = 0;
			while (i < num)
			{
				if (this._byteBuffer[this._bytePos] != *preamble[this._bytePos])
				{
					this._bytePos = 0;
					this._checkPreamble = false;
					break;
				}
				i++;
				this._bytePos++;
			}
			if (this._checkPreamble && this._bytePos == preamble.Length)
			{
				this.CompressBuffer(preamble.Length);
				this._bytePos = 0;
				this._checkPreamble = false;
				this._detectEncoding = false;
			}
			return this._checkPreamble;
		}

		// Token: 0x06006559 RID: 25945 RVA: 0x00158FD0 File Offset: 0x001571D0
		internal virtual int ReadBuffer()
		{
			this._charLen = 0;
			this._charPos = 0;
			if (!this._checkPreamble)
			{
				this._byteLen = 0;
			}
			for (;;)
			{
				if (this._checkPreamble)
				{
					int num = this._stream.Read(this._byteBuffer, this._bytePos, this._byteBuffer.Length - this._bytePos);
					if (num == 0)
					{
						break;
					}
					this._byteLen += num;
				}
				else
				{
					this._byteLen = this._stream.Read(this._byteBuffer, 0, this._byteBuffer.Length);
					if (this._byteLen == 0)
					{
						goto Block_5;
					}
				}
				this._isBlocked = this._byteLen < this._byteBuffer.Length;
				if (!this.IsPreamble())
				{
					if (this._detectEncoding && this._byteLen >= 2)
					{
						this.DetectEncoding();
					}
					this._charLen += this._decoder.GetChars(this._byteBuffer, 0, this._byteLen, this._charBuffer, this._charLen);
				}
				if (this._charLen != 0)
				{
					goto Block_9;
				}
			}
			if (this._byteLen > 0)
			{
				this._charLen += this._decoder.GetChars(this._byteBuffer, 0, this._byteLen, this._charBuffer, this._charLen);
				this._bytePos = (this._byteLen = 0);
			}
			return this._charLen;
			Block_5:
			return this._charLen;
			Block_9:
			return this._charLen;
		}

		// Token: 0x0600655A RID: 25946 RVA: 0x00159138 File Offset: 0x00157338
		private int ReadBuffer(Span<char> userBuffer, out bool readToUserBuffer)
		{
			this._charLen = 0;
			this._charPos = 0;
			if (!this._checkPreamble)
			{
				this._byteLen = 0;
			}
			int num = 0;
			readToUserBuffer = userBuffer.Length >= this._maxCharsPerBuffer;
			for (;;)
			{
				if (this._checkPreamble)
				{
					int num2 = this._stream.Read(this._byteBuffer, this._bytePos, this._byteBuffer.Length - this._bytePos);
					if (num2 == 0)
					{
						break;
					}
					this._byteLen += num2;
				}
				else
				{
					this._byteLen = this._stream.Read(this._byteBuffer, 0, this._byteBuffer.Length);
					if (this._byteLen == 0)
					{
						goto IL_01CD;
					}
				}
				this._isBlocked = this._byteLen < this._byteBuffer.Length;
				if (!this.IsPreamble())
				{
					if (this._detectEncoding && this._byteLen >= 2)
					{
						this.DetectEncoding();
						readToUserBuffer = userBuffer.Length >= this._maxCharsPerBuffer;
					}
					this._charPos = 0;
					if (readToUserBuffer)
					{
						num += this._decoder.GetChars(new ReadOnlySpan<byte>(this._byteBuffer, 0, this._byteLen), userBuffer.Slice(num), false);
						this._charLen = 0;
					}
					else
					{
						num = this._decoder.GetChars(this._byteBuffer, 0, this._byteLen, this._charBuffer, num);
						this._charLen += num;
					}
				}
				if (num != 0)
				{
					goto IL_01CD;
				}
			}
			if (this._byteLen > 0)
			{
				if (readToUserBuffer)
				{
					num = this._decoder.GetChars(new ReadOnlySpan<byte>(this._byteBuffer, 0, this._byteLen), userBuffer.Slice(num), false);
					this._charLen = 0;
				}
				else
				{
					num = this._decoder.GetChars(this._byteBuffer, 0, this._byteLen, this._charBuffer, num);
					this._charLen += num;
				}
			}
			return num;
			IL_01CD:
			this._isBlocked &= num < userBuffer.Length;
			return num;
		}

		/// <summary>Reads a line of characters from the current stream and returns the data as a string.</summary>
		/// <returns>The next line from the input stream, or <see langword="null" /> if the end of the input stream is reached.</returns>
		/// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x0600655B RID: 25947 RVA: 0x0015932C File Offset: 0x0015752C
		public override string ReadLine()
		{
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Cannot read from a closed TextReader.");
			}
			this.CheckAsyncTaskInProgress();
			if (this._charPos == this._charLen && this.ReadBuffer() == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = null;
			int num;
			char c;
			for (;;)
			{
				num = this._charPos;
				do
				{
					c = this._charBuffer[num];
					if (c == '\r' || c == '\n')
					{
						goto IL_0051;
					}
					num++;
				}
				while (num < this._charLen);
				num = this._charLen - this._charPos;
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(num + 80);
				}
				stringBuilder.Append(this._charBuffer, this._charPos, num);
				if (this.ReadBuffer() <= 0)
				{
					goto Block_11;
				}
			}
			IL_0051:
			string text;
			if (stringBuilder != null)
			{
				stringBuilder.Append(this._charBuffer, this._charPos, num - this._charPos);
				text = stringBuilder.ToString();
			}
			else
			{
				text = new string(this._charBuffer, this._charPos, num - this._charPos);
			}
			this._charPos = num + 1;
			if (c == '\r' && (this._charPos < this._charLen || this.ReadBuffer() > 0) && this._charBuffer[this._charPos] == '\n')
			{
				this._charPos++;
			}
			return text;
			Block_11:
			return stringBuilder.ToString();
		}

		/// <summary>Reads a line of characters asynchronously from the current stream and returns the data as a string.</summary>
		/// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the next line from the stream, or is <see langword="null" /> if all the characters have been read.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters in the next line is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
		// Token: 0x0600655C RID: 25948 RVA: 0x00159464 File Offset: 0x00157664
		public override Task<string> ReadLineAsync()
		{
			if (base.GetType() != typeof(StreamReader))
			{
				return base.ReadLineAsync();
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Cannot read from a closed TextReader.");
			}
			this.CheckAsyncTaskInProgress();
			Task<string> task = this.ReadLineAsyncInternal();
			this._asyncReadTask = task;
			return task;
		}

		// Token: 0x0600655D RID: 25949 RVA: 0x001594B8 File Offset: 0x001576B8
		private async Task<string> ReadLineAsyncInternal()
		{
			bool flag = this._charPos == this._charLen;
			ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
			if (flag)
			{
				ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadBufferAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
				}
				flag = configuredTaskAwaiter.GetResult() == 0;
			}
			string text;
			if (flag)
			{
				text = null;
			}
			else
			{
				StringBuilder sb = null;
				char[] charBuffer;
				int charLen;
				int num2;
				int num;
				char c;
				for (;;)
				{
					charBuffer = this._charBuffer;
					charLen = this._charLen;
					num = (num2 = this._charPos);
					do
					{
						c = charBuffer[num2];
						if (c == '\r' || c == '\n')
						{
							goto IL_00E1;
						}
						num2++;
					}
					while (num2 < charLen);
					num2 = charLen - num;
					if (sb == null)
					{
						sb = new StringBuilder(num2 + 80);
					}
					sb.Append(charBuffer, num, num2);
					ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadBufferAsync().ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
					}
					if (configuredTaskAwaiter.GetResult() <= 0)
					{
						goto Block_14;
					}
				}
				IL_00E1:
				string s;
				if (sb != null)
				{
					sb.Append(charBuffer, num, num2 - num);
					s = sb.ToString();
				}
				else
				{
					s = new string(charBuffer, num, num2 - num);
				}
				int num3 = num2 + 1;
				this._charPos = num3;
				flag = c == '\r';
				if (flag)
				{
					bool flag2 = num3 < charLen;
					if (!flag2)
					{
						ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadBufferAsync().ConfigureAwait(false).GetAwaiter();
						if (!configuredTaskAwaiter.IsCompleted)
						{
							await configuredTaskAwaiter;
							configuredTaskAwaiter = configuredTaskAwaiter2;
							configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
						}
						flag2 = configuredTaskAwaiter.GetResult() > 0;
					}
					flag = flag2;
				}
				if (flag)
				{
					num = this._charPos;
					if (this._charBuffer[num] == '\n')
					{
						this._charPos = num + 1;
					}
				}
				return s;
				Block_14:
				text = sb.ToString();
			}
			return text;
		}

		/// <summary>Reads all characters from the current position to the end of the stream asynchronously and returns them as one string.</summary>
		/// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains a string with the characters from the current position to the end of the stream.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
		// Token: 0x0600655E RID: 25950 RVA: 0x001594FC File Offset: 0x001576FC
		public override Task<string> ReadToEndAsync()
		{
			if (base.GetType() != typeof(StreamReader))
			{
				return base.ReadToEndAsync();
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Cannot read from a closed TextReader.");
			}
			this.CheckAsyncTaskInProgress();
			Task<string> task = this.ReadToEndAsyncInternal();
			this._asyncReadTask = task;
			return task;
		}

		// Token: 0x0600655F RID: 25951 RVA: 0x00159550 File Offset: 0x00157750
		private async Task<string> ReadToEndAsyncInternal()
		{
			StringBuilder sb = new StringBuilder(this._charLen - this._charPos);
			do
			{
				int charPos = this._charPos;
				sb.Append(this._charBuffer, charPos, this._charLen - charPos);
				this._charPos = this._charLen;
				await this.ReadBufferAsync().ConfigureAwait(false);
			}
			while (this._charLen > 0);
			return sb.ToString();
		}

		/// <summary>Reads a specified maximum number of characters from the current stream asynchronously and writes the data to a buffer, beginning at the specified index.</summary>
		/// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
		/// <param name="index">The position in <paramref name="buffer" /> at which to begin writing.</param>
		/// <param name="count">The maximum number of characters to read. If the end of the stream is reached before the specified number of characters is written into the buffer, the current method returns.</param>
		/// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of characters read into the buffer. The result value can be less than the number of characters requested if the number of characters currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ArgumentException">The sum of <paramref name="index" /> and <paramref name="count" /> is larger than the buffer length.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
		// Token: 0x06006560 RID: 25952 RVA: 0x00159594 File Offset: 0x00157794
		public override Task<int> ReadAsync(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", "Non-negative number required.");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (base.GetType() != typeof(StreamReader))
			{
				return base.ReadAsync(buffer, index, count);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Cannot read from a closed TextReader.");
			}
			this.CheckAsyncTaskInProgress();
			Task<int> task = this.ReadAsyncInternal(new Memory<char>(buffer, index, count), default(CancellationToken)).AsTask();
			this._asyncReadTask = task;
			return task;
		}

		// Token: 0x06006561 RID: 25953 RVA: 0x00159650 File Offset: 0x00157850
		public override ValueTask<int> ReadAsync(Memory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (base.GetType() != typeof(StreamReader))
			{
				return base.ReadAsync(buffer, cancellationToken);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Cannot read from a closed TextReader.");
			}
			this.CheckAsyncTaskInProgress();
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
			}
			return this.ReadAsyncInternal(buffer, cancellationToken);
		}

		// Token: 0x06006562 RID: 25954 RVA: 0x001596B4 File Offset: 0x001578B4
		internal override async ValueTask<int> ReadAsyncInternal(Memory<char> buffer, CancellationToken cancellationToken)
		{
			bool flag = this._charPos == this._charLen;
			if (flag)
			{
				ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.ReadBufferAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter);
				}
				flag = configuredTaskAwaiter.GetResult() == 0;
			}
			int num;
			if (flag)
			{
				num = 0;
			}
			else
			{
				int charsRead = 0;
				bool readToUserBuffer = false;
				byte[] tmpByteBuffer = this._byteBuffer;
				Stream tmpStream = this._stream;
				int count = buffer.Length;
				while (count > 0)
				{
					int i = this._charLen - this._charPos;
					if (i == 0)
					{
						this._charLen = 0;
						this._charPos = 0;
						if (!this._checkPreamble)
						{
							this._byteLen = 0;
						}
						readToUserBuffer = count >= this._maxCharsPerBuffer;
						do
						{
							if (this._checkPreamble)
							{
								int bytePos = this._bytePos;
								int num2 = await tmpStream.ReadAsync(new Memory<byte>(tmpByteBuffer, bytePos, tmpByteBuffer.Length - bytePos), cancellationToken).ConfigureAwait(false);
								if (num2 == 0)
								{
									goto Block_7;
								}
								this._byteLen += num2;
							}
							else
							{
								this._byteLen = await tmpStream.ReadAsync(new Memory<byte>(tmpByteBuffer), cancellationToken).ConfigureAwait(false);
								if (this._byteLen == 0)
								{
									goto Block_10;
								}
							}
							this._isBlocked = this._byteLen < tmpByteBuffer.Length;
							if (!this.IsPreamble())
							{
								if (this._detectEncoding && this._byteLen >= 2)
								{
									this.DetectEncoding();
									readToUserBuffer = count >= this._maxCharsPerBuffer;
								}
								this._charPos = 0;
								if (readToUserBuffer)
								{
									i += this._decoder.GetChars(new ReadOnlySpan<byte>(tmpByteBuffer, 0, this._byteLen), buffer.Span.Slice(charsRead), false);
									this._charLen = 0;
								}
								else
								{
									i = this._decoder.GetChars(tmpByteBuffer, 0, this._byteLen, this._charBuffer, 0);
									this._charLen += i;
								}
							}
						}
						while (i == 0);
						IL_0423:
						if (i != 0)
						{
							goto IL_042E;
						}
						break;
						Block_10:
						this._isBlocked = true;
						goto IL_0423;
						Block_7:
						if (this._byteLen > 0)
						{
							if (readToUserBuffer)
							{
								i = this._decoder.GetChars(new ReadOnlySpan<byte>(tmpByteBuffer, 0, this._byteLen), buffer.Span.Slice(charsRead), false);
								this._charLen = 0;
							}
							else
							{
								i = this._decoder.GetChars(tmpByteBuffer, 0, this._byteLen, this._charBuffer, 0);
								this._charLen += i;
							}
						}
						this._isBlocked = true;
						goto IL_0423;
					}
					IL_042E:
					if (i > count)
					{
						i = count;
					}
					if (!readToUserBuffer)
					{
						new Span<char>(this._charBuffer, this._charPos, i).CopyTo(buffer.Span.Slice(charsRead));
						this._charPos += i;
					}
					charsRead += i;
					count -= i;
					if (this._isBlocked)
					{
						break;
					}
				}
				num = charsRead;
			}
			return num;
		}

		/// <summary>Reads a specified maximum number of characters from the current stream asynchronously and writes the data to a buffer, beginning at the specified index.</summary>
		/// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
		/// <param name="index">The position in <paramref name="buffer" /> at which to begin writing.</param>
		/// <param name="count">The maximum number of characters to read. If the end of the stream is reached before the specified number of characters is written into the buffer, the method returns.</param>
		/// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of characters read into the buffer. The result value can be less than the number of characters requested if the number of characters currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ArgumentException">The sum of <paramref name="index" /> and <paramref name="count" /> is larger than the buffer length.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
		// Token: 0x06006563 RID: 25955 RVA: 0x00159708 File Offset: 0x00157908
		public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", "Non-negative number required.");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (base.GetType() != typeof(StreamReader))
			{
				return base.ReadBlockAsync(buffer, index, count);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Cannot read from a closed TextReader.");
			}
			this.CheckAsyncTaskInProgress();
			Task<int> task = base.ReadBlockAsync(buffer, index, count);
			this._asyncReadTask = task;
			return task;
		}

		// Token: 0x06006564 RID: 25956 RVA: 0x001597AC File Offset: 0x001579AC
		public override ValueTask<int> ReadBlockAsync(Memory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (base.GetType() != typeof(StreamReader))
			{
				return base.ReadBlockAsync(buffer, cancellationToken);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Cannot read from a closed TextReader.");
			}
			this.CheckAsyncTaskInProgress();
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
			}
			ValueTask<int> valueTask = base.ReadBlockAsyncInternal(buffer, cancellationToken);
			if (valueTask.IsCompletedSuccessfully)
			{
				return valueTask;
			}
			Task<int> task = valueTask.AsTask();
			this._asyncReadTask = task;
			return new ValueTask<int>(task);
		}

		// Token: 0x06006565 RID: 25957 RVA: 0x00159834 File Offset: 0x00157A34
		private async Task<int> ReadBufferAsync()
		{
			this._charLen = 0;
			this._charPos = 0;
			byte[] tmpByteBuffer = this._byteBuffer;
			Stream tmpStream = this._stream;
			if (!this._checkPreamble)
			{
				this._byteLen = 0;
			}
			for (;;)
			{
				if (this._checkPreamble)
				{
					int bytePos = this._bytePos;
					int num = await tmpStream.ReadAsync(new Memory<byte>(tmpByteBuffer, bytePos, tmpByteBuffer.Length - bytePos), default(CancellationToken)).ConfigureAwait(false);
					if (num == 0)
					{
						break;
					}
					this._byteLen += num;
				}
				else
				{
					this._byteLen = await tmpStream.ReadAsync(new Memory<byte>(tmpByteBuffer), default(CancellationToken)).ConfigureAwait(false);
					if (this._byteLen == 0)
					{
						goto Block_5;
					}
				}
				this._isBlocked = this._byteLen < tmpByteBuffer.Length;
				if (!this.IsPreamble())
				{
					if (this._detectEncoding && this._byteLen >= 2)
					{
						this.DetectEncoding();
					}
					this._charLen += this._decoder.GetChars(tmpByteBuffer, 0, this._byteLen, this._charBuffer, this._charLen);
				}
				if (this._charLen != 0)
				{
					goto Block_9;
				}
			}
			if (this._byteLen > 0)
			{
				this._charLen += this._decoder.GetChars(tmpByteBuffer, 0, this._byteLen, this._charBuffer, this._charLen);
				this._bytePos = 0;
				this._byteLen = 0;
			}
			return this._charLen;
			Block_5:
			return this._charLen;
			Block_9:
			return this._charLen;
		}

		// Token: 0x06006566 RID: 25958 RVA: 0x00159877 File Offset: 0x00157A77
		internal bool DataAvailable()
		{
			return this._charPos < this._charLen;
		}

		/// <summary>A <see cref="T:System.IO.StreamReader" /> object around an empty stream.</summary>
		// Token: 0x04003B6D RID: 15213
		public new static readonly StreamReader Null = new StreamReader.NullStreamReader();

		// Token: 0x04003B6E RID: 15214
		private const int DefaultBufferSize = 1024;

		// Token: 0x04003B6F RID: 15215
		private const int DefaultFileStreamBufferSize = 4096;

		// Token: 0x04003B70 RID: 15216
		private const int MinBufferSize = 128;

		// Token: 0x04003B71 RID: 15217
		private Stream _stream;

		// Token: 0x04003B72 RID: 15218
		private Encoding _encoding;

		// Token: 0x04003B73 RID: 15219
		private Decoder _decoder;

		// Token: 0x04003B74 RID: 15220
		private byte[] _byteBuffer;

		// Token: 0x04003B75 RID: 15221
		private char[] _charBuffer;

		// Token: 0x04003B76 RID: 15222
		private int _charPos;

		// Token: 0x04003B77 RID: 15223
		private int _charLen;

		// Token: 0x04003B78 RID: 15224
		private int _byteLen;

		// Token: 0x04003B79 RID: 15225
		private int _bytePos;

		// Token: 0x04003B7A RID: 15226
		private int _maxCharsPerBuffer;

		// Token: 0x04003B7B RID: 15227
		private bool _detectEncoding;

		// Token: 0x04003B7C RID: 15228
		private bool _checkPreamble;

		// Token: 0x04003B7D RID: 15229
		private bool _isBlocked;

		// Token: 0x04003B7E RID: 15230
		private bool _closable;

		// Token: 0x04003B7F RID: 15231
		private Task _asyncReadTask = Task.CompletedTask;

		// Token: 0x02000B13 RID: 2835
		private class NullStreamReader : StreamReader
		{
			// Token: 0x06006568 RID: 25960 RVA: 0x00159893 File Offset: 0x00157A93
			internal NullStreamReader()
			{
				base.Init(Stream.Null);
			}

			// Token: 0x170011BD RID: 4541
			// (get) Token: 0x06006569 RID: 25961 RVA: 0x001598A6 File Offset: 0x00157AA6
			public override Stream BaseStream
			{
				get
				{
					return Stream.Null;
				}
			}

			// Token: 0x170011BE RID: 4542
			// (get) Token: 0x0600656A RID: 25962 RVA: 0x001598AD File Offset: 0x00157AAD
			public override Encoding CurrentEncoding
			{
				get
				{
					return Encoding.Unicode;
				}
			}

			// Token: 0x0600656B RID: 25963 RVA: 0x00004BF9 File Offset: 0x00002DF9
			protected override void Dispose(bool disposing)
			{
			}

			// Token: 0x0600656C RID: 25964 RVA: 0x0012276A File Offset: 0x0012096A
			public override int Peek()
			{
				return -1;
			}

			// Token: 0x0600656D RID: 25965 RVA: 0x0012276A File Offset: 0x0012096A
			public override int Read()
			{
				return -1;
			}

			// Token: 0x0600656E RID: 25966 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
			public override int Read(char[] buffer, int index, int count)
			{
				return 0;
			}

			// Token: 0x0600656F RID: 25967 RVA: 0x0000AF5E File Offset: 0x0000915E
			public override string ReadLine()
			{
				return null;
			}

			// Token: 0x06006570 RID: 25968 RVA: 0x000258DF File Offset: 0x00023ADF
			public override string ReadToEnd()
			{
				return string.Empty;
			}

			// Token: 0x06006571 RID: 25969 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
			internal override int ReadBuffer()
			{
				return 0;
			}
		}
	}
}
