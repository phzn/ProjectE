using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System
{
	/// <summary>Represents the standard input, output, and error streams for console applications. This class cannot be inherited.</summary>
	// Token: 0x0200022A RID: 554
	public static class Console
	{
		// Token: 0x060018D9 RID: 6361 RVA: 0x0005E1E8 File Offset: 0x0005C3E8
		static Console()
		{
			if (Environment.IsRunningOnWindows)
			{
				try
				{
					Console.inputEncoding = Encoding.GetEncoding(Console.WindowsConsole.GetInputCodePage());
					Console.outputEncoding = Encoding.GetEncoding(Console.WindowsConsole.GetOutputCodePage());
					goto IL_009B;
				}
				catch
				{
					Console.inputEncoding = (Console.outputEncoding = Encoding.Default);
					goto IL_009B;
				}
			}
			int num = 0;
			EncodingHelper.InternalCodePage(ref num);
			if (num != -1 && ((num & 268435455) == 3 || (num & 268435456) != 0))
			{
				Console.inputEncoding = (Console.outputEncoding = EncodingHelper.UTF8Unmarked);
			}
			else
			{
				Console.inputEncoding = (Console.outputEncoding = Encoding.Default);
			}
			IL_009B:
			Console.SetupStreams(Console.inputEncoding, Console.outputEncoding);
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x0005E2B0 File Offset: 0x0005C4B0
		private static void SetupStreams(Encoding inputEncoding, Encoding outputEncoding)
		{
			if (!Environment.IsRunningOnWindows && ConsoleDriver.IsConsole)
			{
				Console.stdin = new CStreamReader(Console.OpenStandardInput(0), inputEncoding);
				Console.stdout = TextWriter.Synchronized(new CStreamWriter(Console.OpenStandardOutput(0), outputEncoding, true)
				{
					AutoFlush = true
				});
				Console.stderr = TextWriter.Synchronized(new CStreamWriter(Console.OpenStandardError(0), outputEncoding, true)
				{
					AutoFlush = true
				});
			}
			else
			{
				Console.stdin = TextReader.Synchronized(new UnexceptionalStreamReader(Console.OpenStandardInput(0), inputEncoding));
				Console.stdout = TextWriter.Synchronized(new UnexceptionalStreamWriter(Console.OpenStandardOutput(0), outputEncoding)
				{
					AutoFlush = true
				});
				Console.stderr = TextWriter.Synchronized(new UnexceptionalStreamWriter(Console.OpenStandardError(0), outputEncoding)
				{
					AutoFlush = true
				});
			}
			GC.SuppressFinalize(Console.stdout);
			GC.SuppressFinalize(Console.stderr);
			GC.SuppressFinalize(Console.stdin);
		}

		/// <summary>Gets the standard error output stream.</summary>
		/// <returns>A <see cref="T:System.IO.TextWriter" /> that represents the standard error output stream.</returns>
		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x060018DB RID: 6363 RVA: 0x0005E388 File Offset: 0x0005C588
		public static TextWriter Error
		{
			get
			{
				return Console.stderr;
			}
		}

		/// <summary>Gets the standard output stream.</summary>
		/// <returns>A <see cref="T:System.IO.TextWriter" /> that represents the standard output stream.</returns>
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060018DC RID: 6364 RVA: 0x0005E38F File Offset: 0x0005C58F
		public static TextWriter Out
		{
			get
			{
				return Console.stdout;
			}
		}

		/// <summary>Gets the standard input stream.</summary>
		/// <returns>A <see cref="T:System.IO.TextReader" /> that represents the standard input stream.</returns>
		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060018DD RID: 6365 RVA: 0x0005E396 File Offset: 0x0005C596
		public static TextReader In
		{
			get
			{
				return Console.stdin;
			}
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x0005E3A0 File Offset: 0x0005C5A0
		private static Stream Open(IntPtr handle, FileAccess access, int bufferSize)
		{
			Stream stream;
			try
			{
				FileStream fileStream = new FileStream(handle, access, false, bufferSize, false, true);
				GC.SuppressFinalize(fileStream);
				stream = fileStream;
			}
			catch (IOException)
			{
				stream = Stream.Null;
			}
			return stream;
		}

		/// <summary>Acquires the standard error stream.</summary>
		/// <returns>The standard error stream.</returns>
		// Token: 0x060018DF RID: 6367 RVA: 0x0005E3DC File Offset: 0x0005C5DC
		public static Stream OpenStandardError()
		{
			return Console.OpenStandardError(0);
		}

		/// <summary>Acquires the standard error stream, which is set to a specified buffer size.</summary>
		/// <param name="bufferSize">The internal stream buffer size.</param>
		/// <returns>The standard error stream.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bufferSize" /> is less than or equal to zero.</exception>
		// Token: 0x060018E0 RID: 6368 RVA: 0x0005E3E4 File Offset: 0x0005C5E4
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		public static Stream OpenStandardError(int bufferSize)
		{
			return Console.Open(MonoIO.ConsoleError, FileAccess.Write, bufferSize);
		}

		/// <summary>Acquires the standard input stream.</summary>
		/// <returns>The standard input stream.</returns>
		// Token: 0x060018E1 RID: 6369 RVA: 0x0005E3F2 File Offset: 0x0005C5F2
		public static Stream OpenStandardInput()
		{
			return Console.OpenStandardInput(0);
		}

		/// <summary>Acquires the standard input stream, which is set to a specified buffer size.</summary>
		/// <param name="bufferSize">The internal stream buffer size.</param>
		/// <returns>The standard input stream.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bufferSize" /> is less than or equal to zero.</exception>
		// Token: 0x060018E2 RID: 6370 RVA: 0x0005E3FA File Offset: 0x0005C5FA
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		public static Stream OpenStandardInput(int bufferSize)
		{
			return Console.Open(MonoIO.ConsoleInput, FileAccess.Read, bufferSize);
		}

		/// <summary>Acquires the standard output stream.</summary>
		/// <returns>The standard output stream.</returns>
		// Token: 0x060018E3 RID: 6371 RVA: 0x0005E408 File Offset: 0x0005C608
		public static Stream OpenStandardOutput()
		{
			return Console.OpenStandardOutput(0);
		}

		/// <summary>Acquires the standard output stream, which is set to a specified buffer size.</summary>
		/// <param name="bufferSize">The internal stream buffer size.</param>
		/// <returns>The standard output stream.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bufferSize" /> is less than or equal to zero.</exception>
		// Token: 0x060018E4 RID: 6372 RVA: 0x0005E410 File Offset: 0x0005C610
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		public static Stream OpenStandardOutput(int bufferSize)
		{
			return Console.Open(MonoIO.ConsoleOutput, FileAccess.Write, bufferSize);
		}

		/// <summary>Sets the <see cref="P:System.Console.Error" /> property to the specified <see cref="T:System.IO.TextWriter" /> object.</summary>
		/// <param name="newError">A stream that is the new standard error output.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="newError" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x060018E5 RID: 6373 RVA: 0x0005E41E File Offset: 0x0005C61E
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public static void SetError(TextWriter newError)
		{
			if (newError == null)
			{
				throw new ArgumentNullException("newError");
			}
			Console.stderr = TextWriter.Synchronized(newError);
		}

		/// <summary>Sets the <see cref="P:System.Console.In" /> property to the specified <see cref="T:System.IO.TextReader" /> object.</summary>
		/// <param name="newIn">A stream that is the new standard input.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="newIn" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x060018E6 RID: 6374 RVA: 0x0005E439 File Offset: 0x0005C639
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public static void SetIn(TextReader newIn)
		{
			if (newIn == null)
			{
				throw new ArgumentNullException("newIn");
			}
			Console.stdin = TextReader.Synchronized(newIn);
		}

		/// <summary>Sets the <see cref="P:System.Console.Out" /> property to the specified <see cref="T:System.IO.TextWriter" /> object.</summary>
		/// <param name="newOut">A stream that is the new standard output.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="newOut" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x060018E7 RID: 6375 RVA: 0x0005E454 File Offset: 0x0005C654
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public static void SetOut(TextWriter newOut)
		{
			if (newOut == null)
			{
				throw new ArgumentNullException("newOut");
			}
			Console.stdout = TextWriter.Synchronized(newOut);
		}

		/// <summary>Writes the text representation of the specified Boolean value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018E8 RID: 6376 RVA: 0x0005E46F File Offset: 0x0005C66F
		public static void Write(bool value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the specified Unicode character value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018E9 RID: 6377 RVA: 0x0005E47C File Offset: 0x0005C67C
		public static void Write(char value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the specified array of Unicode characters to the standard output stream.</summary>
		/// <param name="buffer">A Unicode character array.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018EA RID: 6378 RVA: 0x0005E489 File Offset: 0x0005C689
		public static void Write(char[] buffer)
		{
			Console.stdout.Write(buffer);
		}

		/// <summary>Writes the text representation of the specified <see cref="T:System.Decimal" /> value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018EB RID: 6379 RVA: 0x0005E496 File Offset: 0x0005C696
		public static void Write(decimal value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified double-precision floating-point value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018EC RID: 6380 RVA: 0x0005E4A3 File Offset: 0x0005C6A3
		public static void Write(double value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified 32-bit signed integer value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018ED RID: 6381 RVA: 0x0005E4B0 File Offset: 0x0005C6B0
		public static void Write(int value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified 64-bit signed integer value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018EE RID: 6382 RVA: 0x0005E4BD File Offset: 0x0005C6BD
		public static void Write(long value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified object to the standard output stream.</summary>
		/// <param name="value">The value to write, or <see langword="null" />.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018EF RID: 6383 RVA: 0x0005E4CA File Offset: 0x0005C6CA
		public static void Write(object value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified single-precision floating-point value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018F0 RID: 6384 RVA: 0x0005E4D7 File Offset: 0x0005C6D7
		public static void Write(float value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the specified string value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018F1 RID: 6385 RVA: 0x0005E4E4 File Offset: 0x0005C6E4
		public static void Write(string value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified 32-bit unsigned integer value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018F2 RID: 6386 RVA: 0x0005E4F1 File Offset: 0x0005C6F1
		[CLSCompliant(false)]
		public static void Write(uint value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified 64-bit unsigned integer value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018F3 RID: 6387 RVA: 0x0005E4FE File Offset: 0x0005C6FE
		[CLSCompliant(false)]
		public static void Write(ulong value)
		{
			Console.stdout.Write(value);
		}

		/// <summary>Writes the text representation of the specified object to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">An object to write using <paramref name="format" />.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid.</exception>
		// Token: 0x060018F4 RID: 6388 RVA: 0x0005E50B File Offset: 0x0005C70B
		public static void Write(string format, object arg0)
		{
			Console.stdout.Write(format, arg0);
		}

		/// <summary>Writes the text representation of the specified array of objects to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg">An array of objects to write using <paramref name="format" />.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> or <paramref name="arg" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid.</exception>
		// Token: 0x060018F5 RID: 6389 RVA: 0x0005E519 File Offset: 0x0005C719
		public static void Write(string format, params object[] arg)
		{
			if (arg == null)
			{
				Console.stdout.Write(format);
				return;
			}
			Console.stdout.Write(format, arg);
		}

		/// <summary>Writes the specified subarray of Unicode characters to the standard output stream.</summary>
		/// <param name="buffer">An array of Unicode characters.</param>
		/// <param name="index">The starting position in <paramref name="buffer" />.</param>
		/// <param name="count">The number of characters to write.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> plus <paramref name="count" /> specify a position that is not within <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018F6 RID: 6390 RVA: 0x0005E536 File Offset: 0x0005C736
		public static void Write(char[] buffer, int index, int count)
		{
			Console.stdout.Write(buffer, index, count);
		}

		/// <summary>Writes the text representation of the specified objects to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to write using <paramref name="format" />.</param>
		/// <param name="arg1">The second object to write using <paramref name="format" />.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid.</exception>
		// Token: 0x060018F7 RID: 6391 RVA: 0x0005E545 File Offset: 0x0005C745
		public static void Write(string format, object arg0, object arg1)
		{
			Console.stdout.Write(format, arg0, arg1);
		}

		/// <summary>Writes the text representation of the specified objects to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to write using <paramref name="format" />.</param>
		/// <param name="arg1">The second object to write using <paramref name="format" />.</param>
		/// <param name="arg2">The third object to write using <paramref name="format" />.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid.</exception>
		// Token: 0x060018F8 RID: 6392 RVA: 0x0005E554 File Offset: 0x0005C754
		public static void Write(string format, object arg0, object arg1, object arg2)
		{
			Console.stdout.Write(format, arg0, arg1, arg2);
		}

		/// <summary>Writes the text representation of the specified objects and variable-length parameter list to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to write using <paramref name="format" />.</param>
		/// <param name="arg1">The second object to write using <paramref name="format" />.</param>
		/// <param name="arg2">The third object to write using <paramref name="format" />.</param>
		/// <param name="arg3">The fourth object to write using <paramref name="format" />.</param>
		/// <param name="…">A comma-delimited list of one or more additional objects to write using <paramref name="format" />. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid.</exception>
		// Token: 0x060018F9 RID: 6393 RVA: 0x0005E564 File Offset: 0x0005C764
		[CLSCompliant(false)]
		public static void Write(string format, object arg0, object arg1, object arg2, object arg3, __arglist)
		{
			ArgIterator argIterator = new ArgIterator(__arglist);
			int remainingCount = argIterator.GetRemainingCount();
			object[] array = new object[remainingCount + 4];
			array[0] = arg0;
			array[1] = arg1;
			array[2] = arg2;
			array[3] = arg3;
			for (int i = 0; i < remainingCount; i++)
			{
				TypedReference nextArg = argIterator.GetNextArg();
				array[i + 4] = TypedReference.ToObject(nextArg);
			}
			Console.stdout.Write(string.Format(format, array));
		}

		/// <summary>Writes the current line terminator to the standard output stream.</summary>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018FA RID: 6394 RVA: 0x0005E5CE File Offset: 0x0005C7CE
		public static void WriteLine()
		{
			Console.stdout.WriteLine();
		}

		/// <summary>Writes the text representation of the specified Boolean value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018FB RID: 6395 RVA: 0x0005E5DA File Offset: 0x0005C7DA
		public static void WriteLine(bool value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the specified Unicode character, followed by the current line terminator, value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018FC RID: 6396 RVA: 0x0005E5E7 File Offset: 0x0005C7E7
		public static void WriteLine(char value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the specified array of Unicode characters, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="buffer">A Unicode character array.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018FD RID: 6397 RVA: 0x0005E5F4 File Offset: 0x0005C7F4
		public static void WriteLine(char[] buffer)
		{
			Console.stdout.WriteLine(buffer);
		}

		/// <summary>Writes the text representation of the specified <see cref="T:System.Decimal" /> value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018FE RID: 6398 RVA: 0x0005E601 File Offset: 0x0005C801
		public static void WriteLine(decimal value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified double-precision floating-point value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x060018FF RID: 6399 RVA: 0x0005E60E File Offset: 0x0005C80E
		public static void WriteLine(double value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified 32-bit signed integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x06001900 RID: 6400 RVA: 0x0005E61B File Offset: 0x0005C81B
		public static void WriteLine(int value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified 64-bit signed integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x06001901 RID: 6401 RVA: 0x0005E628 File Offset: 0x0005C828
		public static void WriteLine(long value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x06001902 RID: 6402 RVA: 0x0005E635 File Offset: 0x0005C835
		public static void WriteLine(object value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified single-precision floating-point value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x06001903 RID: 6403 RVA: 0x0005E642 File Offset: 0x0005C842
		public static void WriteLine(float value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the specified string value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x06001904 RID: 6404 RVA: 0x0005E64F File Offset: 0x0005C84F
		public static void WriteLine(string value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified 32-bit unsigned integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x06001905 RID: 6405 RVA: 0x0005E65C File Offset: 0x0005C85C
		[CLSCompliant(false)]
		public static void WriteLine(uint value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified 64-bit unsigned integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x06001906 RID: 6406 RVA: 0x0005E669 File Offset: 0x0005C869
		[CLSCompliant(false)]
		public static void WriteLine(ulong value)
		{
			Console.stdout.WriteLine(value);
		}

		/// <summary>Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">An object to write using <paramref name="format" />.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid.</exception>
		// Token: 0x06001907 RID: 6407 RVA: 0x0005E676 File Offset: 0x0005C876
		public static void WriteLine(string format, object arg0)
		{
			Console.stdout.WriteLine(format, arg0);
		}

		/// <summary>Writes the text representation of the specified array of objects, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg">An array of objects to write using <paramref name="format" />.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> or <paramref name="arg" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid.</exception>
		// Token: 0x06001908 RID: 6408 RVA: 0x0005E684 File Offset: 0x0005C884
		public static void WriteLine(string format, params object[] arg)
		{
			if (arg == null)
			{
				Console.stdout.WriteLine(format);
				return;
			}
			Console.stdout.WriteLine(format, arg);
		}

		/// <summary>Writes the specified subarray of Unicode characters, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="buffer">An array of Unicode characters.</param>
		/// <param name="index">The starting position in <paramref name="buffer" />.</param>
		/// <param name="count">The number of characters to write.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> plus <paramref name="count" /> specify a position that is not within <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x06001909 RID: 6409 RVA: 0x0005E6A1 File Offset: 0x0005C8A1
		public static void WriteLine(char[] buffer, int index, int count)
		{
			Console.stdout.WriteLine(buffer, index, count);
		}

		/// <summary>Writes the text representation of the specified objects, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to write using <paramref name="format" />.</param>
		/// <param name="arg1">The second object to write using <paramref name="format" />.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid.</exception>
		// Token: 0x0600190A RID: 6410 RVA: 0x0005E6B0 File Offset: 0x0005C8B0
		public static void WriteLine(string format, object arg0, object arg1)
		{
			Console.stdout.WriteLine(format, arg0, arg1);
		}

		/// <summary>Writes the text representation of the specified objects, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to write using <paramref name="format" />.</param>
		/// <param name="arg1">The second object to write using <paramref name="format" />.</param>
		/// <param name="arg2">The third object to write using <paramref name="format" />.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid.</exception>
		// Token: 0x0600190B RID: 6411 RVA: 0x0005E6BF File Offset: 0x0005C8BF
		public static void WriteLine(string format, object arg0, object arg1, object arg2)
		{
			Console.stdout.WriteLine(format, arg0, arg1, arg2);
		}

		/// <summary>Writes the text representation of the specified objects and variable-length parameter list, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to write using <paramref name="format" />.</param>
		/// <param name="arg1">The second object to write using <paramref name="format" />.</param>
		/// <param name="arg2">The third object to write using <paramref name="format" />.</param>
		/// <param name="arg3">The fourth object to write using <paramref name="format" />.</param>
		/// <param name="…">A comma-delimited list of one or more additional objects to write using <paramref name="format" />. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid.</exception>
		// Token: 0x0600190C RID: 6412 RVA: 0x0005E6D0 File Offset: 0x0005C8D0
		[CLSCompliant(false)]
		public static void WriteLine(string format, object arg0, object arg1, object arg2, object arg3, __arglist)
		{
			ArgIterator argIterator = new ArgIterator(__arglist);
			int remainingCount = argIterator.GetRemainingCount();
			object[] array = new object[remainingCount + 4];
			array[0] = arg0;
			array[1] = arg1;
			array[2] = arg2;
			array[3] = arg3;
			for (int i = 0; i < remainingCount; i++)
			{
				TypedReference nextArg = argIterator.GetNextArg();
				array[i + 4] = TypedReference.ToObject(nextArg);
			}
			Console.stdout.WriteLine(string.Format(format, array));
		}

		/// <summary>Reads the next character from the standard input stream.</summary>
		/// <returns>The next character from the input stream, or negative one (-1) if there are currently no more characters to be read.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x0600190D RID: 6413 RVA: 0x0005E73A File Offset: 0x0005C93A
		public static int Read()
		{
			if (Console.stdin is CStreamReader && ConsoleDriver.IsConsole)
			{
				return ConsoleDriver.Read();
			}
			return Console.stdin.Read();
		}

		/// <summary>Reads the next line of characters from the standard input stream.</summary>
		/// <returns>The next line of characters from the input stream, or <see langword="null" /> if no more lines are available.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters in the next line of characters is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		// Token: 0x0600190E RID: 6414 RVA: 0x0005E75F File Offset: 0x0005C95F
		public static string ReadLine()
		{
			if (Console.stdin is CStreamReader && ConsoleDriver.IsConsole)
			{
				return ConsoleDriver.ReadLine();
			}
			return Console.stdin.ReadLine();
		}

		/// <summary>Gets or sets the encoding the console uses to read input.</summary>
		/// <returns>The encoding used to read console input.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property value in a set operation is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred during the execution of this operation.</exception>
		/// <exception cref="T:System.Security.SecurityException">Your application does not have permission to perform this operation.</exception>
		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x0600190F RID: 6415 RVA: 0x0005E784 File Offset: 0x0005C984
		// (set) Token: 0x06001910 RID: 6416 RVA: 0x0005E78B File Offset: 0x0005C98B
		public static Encoding InputEncoding
		{
			get
			{
				return Console.inputEncoding;
			}
			set
			{
				Console.inputEncoding = value;
				Console.SetupStreams(Console.inputEncoding, Console.outputEncoding);
			}
		}

		/// <summary>Gets or sets the encoding the console uses to write output.</summary>
		/// <returns>The encoding used to write console output.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property value in a set operation is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred during the execution of this operation.</exception>
		/// <exception cref="T:System.Security.SecurityException">Your application does not have permission to perform this operation.</exception>
		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06001911 RID: 6417 RVA: 0x0005E7A2 File Offset: 0x0005C9A2
		// (set) Token: 0x06001912 RID: 6418 RVA: 0x0005E7A9 File Offset: 0x0005C9A9
		public static Encoding OutputEncoding
		{
			get
			{
				return Console.outputEncoding;
			}
			set
			{
				Console.outputEncoding = value;
				Console.SetupStreams(Console.inputEncoding, Console.outputEncoding);
			}
		}

		/// <summary>Gets or sets the background color of the console.</summary>
		/// <returns>A value that specifies the background color of the console; that is, the color that appears behind each character. The default is black.</returns>
		/// <exception cref="T:System.ArgumentException">The color specified in a set operation is not a valid member of <see cref="T:System.ConsoleColor" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06001913 RID: 6419 RVA: 0x0005E7C0 File Offset: 0x0005C9C0
		// (set) Token: 0x06001914 RID: 6420 RVA: 0x0005E7C7 File Offset: 0x0005C9C7
		public static ConsoleColor BackgroundColor
		{
			get
			{
				return ConsoleDriver.BackgroundColor;
			}
			set
			{
				ConsoleDriver.BackgroundColor = value;
			}
		}

		/// <summary>Gets or sets the height of the buffer area.</summary>
		/// <returns>The current height, in rows, of the buffer area.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value in a set operation is less than or equal to zero.  
		///  -or-  
		///  The value in a set operation is greater than or equal to <see cref="F:System.Int16.MaxValue" />.  
		///  -or-  
		///  The value in a set operation is less than <see cref="P:System.Console.WindowTop" /> + <see cref="P:System.Console.WindowHeight" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06001915 RID: 6421 RVA: 0x0005E7CF File Offset: 0x0005C9CF
		// (set) Token: 0x06001916 RID: 6422 RVA: 0x0005E7D6 File Offset: 0x0005C9D6
		public static int BufferHeight
		{
			get
			{
				return ConsoleDriver.BufferHeight;
			}
			[MonoLimitation("Implemented only on Windows")]
			set
			{
				ConsoleDriver.BufferHeight = value;
			}
		}

		/// <summary>Gets or sets the width of the buffer area.</summary>
		/// <returns>The current width, in columns, of the buffer area.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value in a set operation is less than or equal to zero.  
		///  -or-  
		///  The value in a set operation is greater than or equal to <see cref="F:System.Int16.MaxValue" />.  
		///  -or-  
		///  The value in a set operation is less than <see cref="P:System.Console.WindowLeft" /> + <see cref="P:System.Console.WindowWidth" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06001917 RID: 6423 RVA: 0x0005E7DE File Offset: 0x0005C9DE
		// (set) Token: 0x06001918 RID: 6424 RVA: 0x0005E7E5 File Offset: 0x0005C9E5
		public static int BufferWidth
		{
			get
			{
				return ConsoleDriver.BufferWidth;
			}
			[MonoLimitation("Implemented only on Windows")]
			set
			{
				ConsoleDriver.BufferWidth = value;
			}
		}

		/// <summary>Gets a value indicating whether the CAPS LOCK keyboard toggle is turned on or turned off.</summary>
		/// <returns>
		///   <see langword="true" /> if CAPS LOCK is turned on; <see langword="false" /> if CAPS LOCK is turned off.</returns>
		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06001919 RID: 6425 RVA: 0x0005E7ED File Offset: 0x0005C9ED
		[MonoLimitation("Implemented only on Windows")]
		public static bool CapsLock
		{
			get
			{
				return ConsoleDriver.CapsLock;
			}
		}

		/// <summary>Gets or sets the column position of the cursor within the buffer area.</summary>
		/// <returns>The current position, in columns, of the cursor.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value in a set operation is less than zero.  
		///  -or-  
		///  The value in a set operation is greater than or equal to <see cref="P:System.Console.BufferWidth" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600191A RID: 6426 RVA: 0x0005E7F4 File Offset: 0x0005C9F4
		// (set) Token: 0x0600191B RID: 6427 RVA: 0x0005E7FB File Offset: 0x0005C9FB
		public static int CursorLeft
		{
			get
			{
				return ConsoleDriver.CursorLeft;
			}
			set
			{
				ConsoleDriver.CursorLeft = value;
			}
		}

		/// <summary>Gets or sets the row position of the cursor within the buffer area.</summary>
		/// <returns>The current position, in rows, of the cursor.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value in a set operation is less than zero.  
		///  -or-  
		///  The value in a set operation is greater than or equal to <see cref="P:System.Console.BufferHeight" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x170002AE RID: 686
		// (get) Token: 0x0600191C RID: 6428 RVA: 0x0005E803 File Offset: 0x0005CA03
		// (set) Token: 0x0600191D RID: 6429 RVA: 0x0005E80A File Offset: 0x0005CA0A
		public static int CursorTop
		{
			get
			{
				return ConsoleDriver.CursorTop;
			}
			set
			{
				ConsoleDriver.CursorTop = value;
			}
		}

		/// <summary>Gets or sets the height of the cursor within a character cell.</summary>
		/// <returns>The size of the cursor expressed as a percentage of the height of a character cell. The property value ranges from 1 to 100.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified in a set operation is less than 1 or greater than 100.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x0600191E RID: 6430 RVA: 0x0005E812 File Offset: 0x0005CA12
		// (set) Token: 0x0600191F RID: 6431 RVA: 0x0005E819 File Offset: 0x0005CA19
		public static int CursorSize
		{
			get
			{
				return ConsoleDriver.CursorSize;
			}
			set
			{
				ConsoleDriver.CursorSize = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the cursor is visible.</summary>
		/// <returns>
		///   <see langword="true" /> if the cursor is visible; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06001920 RID: 6432 RVA: 0x0005E821 File Offset: 0x0005CA21
		// (set) Token: 0x06001921 RID: 6433 RVA: 0x0005E828 File Offset: 0x0005CA28
		public static bool CursorVisible
		{
			get
			{
				return ConsoleDriver.CursorVisible;
			}
			set
			{
				ConsoleDriver.CursorVisible = value;
			}
		}

		/// <summary>Gets or sets the foreground color of the console.</summary>
		/// <returns>A <see cref="T:System.ConsoleColor" /> that specifies the foreground color of the console; that is, the color of each character that is displayed. The default is gray.</returns>
		/// <exception cref="T:System.ArgumentException">The color specified in a set operation is not a valid member of <see cref="T:System.ConsoleColor" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06001922 RID: 6434 RVA: 0x0005E830 File Offset: 0x0005CA30
		// (set) Token: 0x06001923 RID: 6435 RVA: 0x0005E837 File Offset: 0x0005CA37
		public static ConsoleColor ForegroundColor
		{
			get
			{
				return ConsoleDriver.ForegroundColor;
			}
			set
			{
				ConsoleDriver.ForegroundColor = value;
			}
		}

		/// <summary>Gets a value indicating whether a key press is available in the input stream.</summary>
		/// <returns>
		///   <see langword="true" /> if a key press is available; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.InvalidOperationException">Standard input is redirected to a file instead of the keyboard.</exception>
		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06001924 RID: 6436 RVA: 0x0005E83F File Offset: 0x0005CA3F
		public static bool KeyAvailable
		{
			get
			{
				return ConsoleDriver.KeyAvailable;
			}
		}

		/// <summary>Gets the largest possible number of console window rows, based on the current font and screen resolution.</summary>
		/// <returns>The height of the largest possible console window measured in rows.</returns>
		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06001925 RID: 6437 RVA: 0x0005E846 File Offset: 0x0005CA46
		public static int LargestWindowHeight
		{
			get
			{
				return ConsoleDriver.LargestWindowHeight;
			}
		}

		/// <summary>Gets the largest possible number of console window columns, based on the current font and screen resolution.</summary>
		/// <returns>The width of the largest possible console window measured in columns.</returns>
		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06001926 RID: 6438 RVA: 0x0005E84D File Offset: 0x0005CA4D
		public static int LargestWindowWidth
		{
			get
			{
				return ConsoleDriver.LargestWindowWidth;
			}
		}

		/// <summary>Gets a value indicating whether the NUM LOCK keyboard toggle is turned on or turned off.</summary>
		/// <returns>
		///   <see langword="true" /> if NUM LOCK is turned on; <see langword="false" /> if NUM LOCK is turned off.</returns>
		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06001927 RID: 6439 RVA: 0x0005E854 File Offset: 0x0005CA54
		public static bool NumberLock
		{
			get
			{
				return ConsoleDriver.NumberLock;
			}
		}

		/// <summary>Gets or sets the title to display in the console title bar.</summary>
		/// <returns>The string to be displayed in the title bar of the console. The maximum length of the title string is 24500 characters.</returns>
		/// <exception cref="T:System.InvalidOperationException">In a get operation, the retrieved title is longer than 24500 characters.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">In a set operation, the specified title is longer than 24500 characters.</exception>
		/// <exception cref="T:System.ArgumentNullException">In a set operation, the specified title is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06001928 RID: 6440 RVA: 0x0005E85B File Offset: 0x0005CA5B
		// (set) Token: 0x06001929 RID: 6441 RVA: 0x0005E862 File Offset: 0x0005CA62
		public static string Title
		{
			get
			{
				return ConsoleDriver.Title;
			}
			set
			{
				ConsoleDriver.Title = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the combination of the <see cref="F:System.ConsoleModifiers.Control" /> modifier key and <see cref="F:System.ConsoleKey.C" /> console key (Ctrl+C) is treated as ordinary input or as an interruption that is handled by the operating system.</summary>
		/// <returns>
		///   <see langword="true" /> if Ctrl+C is treated as ordinary input; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.IO.IOException">Unable to get or set the input mode of the console input buffer.</exception>
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x0600192A RID: 6442 RVA: 0x0005E86A File Offset: 0x0005CA6A
		// (set) Token: 0x0600192B RID: 6443 RVA: 0x0005E871 File Offset: 0x0005CA71
		public static bool TreatControlCAsInput
		{
			get
			{
				return ConsoleDriver.TreatControlCAsInput;
			}
			set
			{
				ConsoleDriver.TreatControlCAsInput = value;
			}
		}

		/// <summary>Gets or sets the height of the console window area.</summary>
		/// <returns>The height of the console window measured in rows.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <see cref="P:System.Console.WindowWidth" /> property or the value of the <see cref="P:System.Console.WindowHeight" /> property is less than or equal to 0.  
		///  -or-  
		///  The value of the <see cref="P:System.Console.WindowHeight" /> property plus the value of the <see cref="P:System.Console.WindowTop" /> property is greater than or equal to <see cref="F:System.Int16.MaxValue" />.  
		///  -or-  
		///  The value of the <see cref="P:System.Console.WindowWidth" /> property or the value of the <see cref="P:System.Console.WindowHeight" /> property is greater than the largest possible window width or height for the current screen resolution and console font.</exception>
		/// <exception cref="T:System.IO.IOException">Error reading or writing information.</exception>
		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x0600192C RID: 6444 RVA: 0x0005E879 File Offset: 0x0005CA79
		// (set) Token: 0x0600192D RID: 6445 RVA: 0x0005E880 File Offset: 0x0005CA80
		public static int WindowHeight
		{
			get
			{
				return ConsoleDriver.WindowHeight;
			}
			set
			{
				ConsoleDriver.WindowHeight = value;
			}
		}

		/// <summary>Gets or sets the leftmost position of the console window area relative to the screen buffer.</summary>
		/// <returns>The leftmost console window position measured in columns.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">In a set operation, the value to be assigned is less than zero.  
		///  -or-  
		///  As a result of the assignment, <see cref="P:System.Console.WindowLeft" /> plus <see cref="P:System.Console.WindowWidth" /> would exceed <see cref="P:System.Console.BufferWidth" />.</exception>
		/// <exception cref="T:System.IO.IOException">Error reading or writing information.</exception>
		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x0600192E RID: 6446 RVA: 0x0005E888 File Offset: 0x0005CA88
		// (set) Token: 0x0600192F RID: 6447 RVA: 0x0005E88F File Offset: 0x0005CA8F
		public static int WindowLeft
		{
			get
			{
				return ConsoleDriver.WindowLeft;
			}
			set
			{
				ConsoleDriver.WindowLeft = value;
			}
		}

		/// <summary>Gets or sets the top position of the console window area relative to the screen buffer.</summary>
		/// <returns>The uppermost console window position measured in rows.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">In a set operation, the value to be assigned is less than zero.  
		///  -or-  
		///  As a result of the assignment, <see cref="P:System.Console.WindowTop" /> plus <see cref="P:System.Console.WindowHeight" /> would exceed <see cref="P:System.Console.BufferHeight" />.</exception>
		/// <exception cref="T:System.IO.IOException">Error reading or writing information.</exception>
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06001930 RID: 6448 RVA: 0x0005E897 File Offset: 0x0005CA97
		// (set) Token: 0x06001931 RID: 6449 RVA: 0x0005E89E File Offset: 0x0005CA9E
		public static int WindowTop
		{
			get
			{
				return ConsoleDriver.WindowTop;
			}
			set
			{
				ConsoleDriver.WindowTop = value;
			}
		}

		/// <summary>Gets or sets the width of the console window.</summary>
		/// <returns>The width of the console window measured in columns.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <see cref="P:System.Console.WindowWidth" /> property or the value of the <see cref="P:System.Console.WindowHeight" /> property is less than or equal to 0.  
		///  -or-  
		///  The value of the <see cref="P:System.Console.WindowHeight" /> property plus the value of the <see cref="P:System.Console.WindowTop" /> property is greater than or equal to <see cref="F:System.Int16.MaxValue" />.  
		///  -or-  
		///  The value of the <see cref="P:System.Console.WindowWidth" /> property or the value of the <see cref="P:System.Console.WindowHeight" /> property is greater than the largest possible window width or height for the current screen resolution and console font.</exception>
		/// <exception cref="T:System.IO.IOException">Error reading or writing information.</exception>
		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06001932 RID: 6450 RVA: 0x0005E8A6 File Offset: 0x0005CAA6
		// (set) Token: 0x06001933 RID: 6451 RVA: 0x0005E8AD File Offset: 0x0005CAAD
		public static int WindowWidth
		{
			get
			{
				return ConsoleDriver.WindowWidth;
			}
			set
			{
				ConsoleDriver.WindowWidth = value;
			}
		}

		/// <summary>Gets a value that indicates whether the error output stream has been redirected from the standard error stream.</summary>
		/// <returns>
		///   <see langword="true" /> if error output is redirected; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06001934 RID: 6452 RVA: 0x0005E8B5 File Offset: 0x0005CAB5
		public static bool IsErrorRedirected
		{
			get
			{
				return ConsoleDriver.IsErrorRedirected;
			}
		}

		/// <summary>Gets a value that indicates whether output has been redirected from the standard output stream.</summary>
		/// <returns>
		///   <see langword="true" /> if output is redirected; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06001935 RID: 6453 RVA: 0x0005E8BC File Offset: 0x0005CABC
		public static bool IsOutputRedirected
		{
			get
			{
				return ConsoleDriver.IsOutputRedirected;
			}
		}

		/// <summary>Gets a value that indicates whether input has been redirected from the standard input stream.</summary>
		/// <returns>
		///   <see langword="true" /> if input is redirected; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06001936 RID: 6454 RVA: 0x0005E8C3 File Offset: 0x0005CAC3
		public static bool IsInputRedirected
		{
			get
			{
				return ConsoleDriver.IsInputRedirected;
			}
		}

		/// <summary>Plays the sound of a beep through the console speaker.</summary>
		/// <exception cref="T:System.Security.HostProtectionException">This method was executed on a server, such as SQL Server, that does not permit access to a user interface.</exception>
		// Token: 0x06001937 RID: 6455 RVA: 0x0005E8CA File Offset: 0x0005CACA
		public static void Beep()
		{
			Console.Beep(1000, 500);
		}

		/// <summary>Plays the sound of a beep of a specified frequency and duration through the console speaker.</summary>
		/// <param name="frequency">The frequency of the beep, ranging from 37 to 32767 hertz.</param>
		/// <param name="duration">The duration of the beep measured in milliseconds.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="frequency" /> is less than 37 or more than 32767 hertz.  
		/// -or-  
		/// <paramref name="duration" /> is less than or equal to zero.</exception>
		/// <exception cref="T:System.Security.HostProtectionException">This method was executed on a server, such as SQL Server, that does not permit access to the console.</exception>
		// Token: 0x06001938 RID: 6456 RVA: 0x0005E8DB File Offset: 0x0005CADB
		public static void Beep(int frequency, int duration)
		{
			if (frequency < 37 || frequency > 32767)
			{
				throw new ArgumentOutOfRangeException("frequency");
			}
			if (duration <= 0)
			{
				throw new ArgumentOutOfRangeException("duration");
			}
			ConsoleDriver.Beep(frequency, duration);
		}

		/// <summary>Clears the console buffer and corresponding console window of display information.</summary>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x06001939 RID: 6457 RVA: 0x0005E90B File Offset: 0x0005CB0B
		public static void Clear()
		{
			ConsoleDriver.Clear();
		}

		/// <summary>Copies a specified source area of the screen buffer to a specified destination area.</summary>
		/// <param name="sourceLeft">The leftmost column of the source area.</param>
		/// <param name="sourceTop">The topmost row of the source area.</param>
		/// <param name="sourceWidth">The number of columns in the source area.</param>
		/// <param name="sourceHeight">The number of rows in the source area.</param>
		/// <param name="targetLeft">The leftmost column of the destination area.</param>
		/// <param name="targetTop">The topmost row of the destination area.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">One or more of the parameters is less than zero.  
		///  -or-  
		///  <paramref name="sourceLeft" /> or <paramref name="targetLeft" /> is greater than or equal to <see cref="P:System.Console.BufferWidth" />.  
		///  -or-  
		///  <paramref name="sourceTop" /> or <paramref name="targetTop" /> is greater than or equal to <see cref="P:System.Console.BufferHeight" />.  
		///  -or-  
		///  <paramref name="sourceTop" /> + <paramref name="sourceHeight" /> is greater than or equal to <see cref="P:System.Console.BufferHeight" />.  
		///  -or-  
		///  <paramref name="sourceLeft" /> + <paramref name="sourceWidth" /> is greater than or equal to <see cref="P:System.Console.BufferWidth" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x0600193A RID: 6458 RVA: 0x0005E912 File Offset: 0x0005CB12
		[MonoLimitation("Implemented only on Windows")]
		public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
		{
			ConsoleDriver.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);
		}

		/// <summary>Copies a specified source area of the screen buffer to a specified destination area.</summary>
		/// <param name="sourceLeft">The leftmost column of the source area.</param>
		/// <param name="sourceTop">The topmost row of the source area.</param>
		/// <param name="sourceWidth">The number of columns in the source area.</param>
		/// <param name="sourceHeight">The number of rows in the source area.</param>
		/// <param name="targetLeft">The leftmost column of the destination area.</param>
		/// <param name="targetTop">The topmost row of the destination area.</param>
		/// <param name="sourceChar">The character used to fill the source area.</param>
		/// <param name="sourceForeColor">The foreground color used to fill the source area.</param>
		/// <param name="sourceBackColor">The background color used to fill the source area.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">One or more of the parameters is less than zero.  
		///  -or-  
		///  <paramref name="sourceLeft" /> or <paramref name="targetLeft" /> is greater than or equal to <see cref="P:System.Console.BufferWidth" />.  
		///  -or-  
		///  <paramref name="sourceTop" /> or <paramref name="targetTop" /> is greater than or equal to <see cref="P:System.Console.BufferHeight" />.  
		///  -or-  
		///  <paramref name="sourceTop" /> + <paramref name="sourceHeight" /> is greater than or equal to <see cref="P:System.Console.BufferHeight" />.  
		///  -or-  
		///  <paramref name="sourceLeft" /> + <paramref name="sourceWidth" /> is greater than or equal to <see cref="P:System.Console.BufferWidth" />.</exception>
		/// <exception cref="T:System.ArgumentException">One or both of the color parameters is not a member of the <see cref="T:System.ConsoleColor" /> enumeration.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x0600193B RID: 6459 RVA: 0x0005E924 File Offset: 0x0005CB24
		[MonoLimitation("Implemented only on Windows")]
		public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
		{
			ConsoleDriver.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, sourceChar, sourceForeColor, sourceBackColor);
		}

		/// <summary>Obtains the next character or function key pressed by the user. The pressed key is displayed in the console window.</summary>
		/// <returns>An object that describes the <see cref="T:System.ConsoleKey" /> constant and Unicode character, if any, that correspond to the pressed console key. The <see cref="T:System.ConsoleKeyInfo" /> object also describes, in a bitwise combination of <see cref="T:System.ConsoleModifiers" /> values, whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously with the console key.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Console.In" /> property is redirected from some stream other than the console.</exception>
		// Token: 0x0600193C RID: 6460 RVA: 0x0005E944 File Offset: 0x0005CB44
		public static ConsoleKeyInfo ReadKey()
		{
			return Console.ReadKey(false);
		}

		/// <summary>Obtains the next character or function key pressed by the user. The pressed key is optionally displayed in the console window.</summary>
		/// <param name="intercept">Determines whether to display the pressed key in the console window. <see langword="true" /> to not display the pressed key; otherwise, <see langword="false" />.</param>
		/// <returns>An object that describes the <see cref="T:System.ConsoleKey" /> constant and Unicode character, if any, that correspond to the pressed console key. The <see cref="T:System.ConsoleKeyInfo" /> object also describes, in a bitwise combination of <see cref="T:System.ConsoleModifiers" /> values, whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously with the console key.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Console.In" /> property is redirected from some stream other than the console.</exception>
		// Token: 0x0600193D RID: 6461 RVA: 0x0005E94C File Offset: 0x0005CB4C
		public static ConsoleKeyInfo ReadKey(bool intercept)
		{
			return ConsoleDriver.ReadKey(intercept);
		}

		/// <summary>Sets the foreground and background console colors to their defaults.</summary>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x0600193E RID: 6462 RVA: 0x0005E954 File Offset: 0x0005CB54
		public static void ResetColor()
		{
			ConsoleDriver.ResetColor();
		}

		/// <summary>Sets the height and width of the screen buffer area to the specified values.</summary>
		/// <param name="width">The width of the buffer area measured in columns.</param>
		/// <param name="height">The height of the buffer area measured in rows.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="height" /> or <paramref name="width" /> is less than or equal to zero.  
		/// -or-  
		/// <paramref name="height" /> or <paramref name="width" /> is greater than or equal to <see cref="F:System.Int16.MaxValue" />.  
		/// -or-  
		/// <paramref name="width" /> is less than <see cref="P:System.Console.WindowLeft" /> + <see cref="P:System.Console.WindowWidth" />.  
		/// -or-  
		/// <paramref name="height" /> is less than <see cref="P:System.Console.WindowTop" /> + <see cref="P:System.Console.WindowHeight" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x0600193F RID: 6463 RVA: 0x0005E95B File Offset: 0x0005CB5B
		[MonoLimitation("Only works on windows")]
		public static void SetBufferSize(int width, int height)
		{
			ConsoleDriver.SetBufferSize(width, height);
		}

		/// <summary>Sets the position of the cursor.</summary>
		/// <param name="left">The column position of the cursor. Columns are numbered from left to right starting at 0.</param>
		/// <param name="top">The row position of the cursor. Rows are numbered from top to bottom starting at 0.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="left" /> or <paramref name="top" /> is less than zero.  
		/// -or-  
		/// <paramref name="left" /> is greater than or equal to <see cref="P:System.Console.BufferWidth" />.  
		/// -or-  
		/// <paramref name="top" /> is greater than or equal to <see cref="P:System.Console.BufferHeight" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x06001940 RID: 6464 RVA: 0x0005E964 File Offset: 0x0005CB64
		public static void SetCursorPosition(int left, int top)
		{
			ConsoleDriver.SetCursorPosition(left, top);
		}

		/// <summary>Sets the position of the console window relative to the screen buffer.</summary>
		/// <param name="left">The column position of the upper left  corner of the console window.</param>
		/// <param name="top">The row position of the upper left corner of the console window.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="left" /> or <paramref name="top" /> is less than zero.  
		/// -or-  
		/// <paramref name="left" /> + <see cref="P:System.Console.WindowWidth" /> is greater than <see cref="P:System.Console.BufferWidth" />.  
		/// -or-  
		/// <paramref name="top" /> + <see cref="P:System.Console.WindowHeight" /> is greater than <see cref="P:System.Console.BufferHeight" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x06001941 RID: 6465 RVA: 0x0005E96D File Offset: 0x0005CB6D
		public static void SetWindowPosition(int left, int top)
		{
			ConsoleDriver.SetWindowPosition(left, top);
		}

		/// <summary>Sets the height and width of the console window to the specified values.</summary>
		/// <param name="width">The width of the console window measured in columns.</param>
		/// <param name="height">The height of the console window measured in rows.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="width" /> or <paramref name="height" /> is less than or equal to zero.  
		/// -or-  
		/// <paramref name="width" /> plus <see cref="P:System.Console.WindowLeft" /> or <paramref name="height" /> plus <see cref="P:System.Console.WindowTop" /> is greater than or equal to <see cref="F:System.Int16.MaxValue" />.  
		/// -or-  
		/// <paramref name="width" /> or <paramref name="height" /> is greater than the largest possible window width or height for the current screen resolution and console font.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x06001942 RID: 6466 RVA: 0x0005E976 File Offset: 0x0005CB76
		public static void SetWindowSize(int width, int height)
		{
			ConsoleDriver.SetWindowSize(width, height);
		}

		/// <summary>Occurs when the <see cref="F:System.ConsoleModifiers.Control" /> modifier key (Ctrl) and either the <see cref="F:System.ConsoleKey.C" /> console key (C) or the Break key are pressed simultaneously (Ctrl+C or Ctrl+Break).</summary>
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06001943 RID: 6467 RVA: 0x0005E97F File Offset: 0x0005CB7F
		// (remove) Token: 0x06001944 RID: 6468 RVA: 0x0005E9B5 File Offset: 0x0005CBB5
		public static event ConsoleCancelEventHandler CancelKeyPress
		{
			add
			{
				if (!ConsoleDriver.Initialized)
				{
					ConsoleDriver.Init();
				}
				Console.cancel_event = (ConsoleCancelEventHandler)Delegate.Combine(Console.cancel_event, value);
				if (Environment.IsRunningOnWindows && !Console.WindowsConsole.ctrlHandlerAdded)
				{
					Console.WindowsConsole.AddCtrlHandler();
				}
			}
			remove
			{
				if (!ConsoleDriver.Initialized)
				{
					ConsoleDriver.Init();
				}
				Console.cancel_event = (ConsoleCancelEventHandler)Delegate.Remove(Console.cancel_event, value);
				if (Console.cancel_event == null && Environment.IsRunningOnWindows && Console.WindowsConsole.ctrlHandlerAdded)
				{
					Console.WindowsConsole.RemoveCtrlHandler();
				}
			}
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x0005E9F2 File Offset: 0x0005CBF2
		private static void DoConsoleCancelEventInBackground()
		{
			ThreadPool.UnsafeQueueUserWorkItem(delegate(object _)
			{
				Console.DoConsoleCancelEvent();
			}, null);
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x0005EA1C File Offset: 0x0005CC1C
		private static void DoConsoleCancelEvent()
		{
			bool flag = true;
			if (Console.cancel_event != null)
			{
				ConsoleCancelEventArgs e = new ConsoleCancelEventArgs(ConsoleSpecialKey.ControlC);
				foreach (ConsoleCancelEventHandler consoleCancelEventHandler in Console.cancel_event.GetInvocationList())
				{
					try
					{
						consoleCancelEventHandler(null, e);
					}
					catch
					{
					}
				}
				flag = !e.Cancel;
			}
			if (flag)
			{
				Environment.Exit(58);
			}
		}

		// Token: 0x040016DB RID: 5851
		internal static TextWriter stdout;

		// Token: 0x040016DC RID: 5852
		private static TextWriter stderr;

		// Token: 0x040016DD RID: 5853
		private static TextReader stdin;

		// Token: 0x040016DE RID: 5854
		private const string LibLog = "/system/lib/liblog.so";

		// Token: 0x040016DF RID: 5855
		private const string LibLog64 = "/system/lib64/liblog.so";

		// Token: 0x040016E0 RID: 5856
		internal static bool IsRunningOnAndroid = File.Exists("/system/lib/liblog.so") || File.Exists("/system/lib64/liblog.so");

		// Token: 0x040016E1 RID: 5857
		private static Encoding inputEncoding;

		// Token: 0x040016E2 RID: 5858
		private static Encoding outputEncoding;

		// Token: 0x040016E3 RID: 5859
		private static ConsoleCancelEventHandler cancel_event;

		// Token: 0x0200022B RID: 555
		private class WindowsConsole
		{
			// Token: 0x06001947 RID: 6471
			[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			private static extern int GetConsoleCP();

			// Token: 0x06001948 RID: 6472
			[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			private static extern int GetConsoleOutputCP();

			// Token: 0x06001949 RID: 6473
			[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			private static extern bool SetConsoleCtrlHandler(Console.WindowsConsole.WindowsCancelHandler handler, bool addHandler);

			// Token: 0x0600194A RID: 6474 RVA: 0x0005EA90 File Offset: 0x0005CC90
			private static bool DoWindowsConsoleCancelEvent(int keyCode)
			{
				if (keyCode == 0)
				{
					Console.DoConsoleCancelEvent();
				}
				return keyCode == 0;
			}

			// Token: 0x0600194B RID: 6475 RVA: 0x0005EA9E File Offset: 0x0005CC9E
			[MethodImpl(MethodImplOptions.NoInlining)]
			public static int GetInputCodePage()
			{
				return Console.WindowsConsole.GetConsoleCP();
			}

			// Token: 0x0600194C RID: 6476 RVA: 0x0005EAA5 File Offset: 0x0005CCA5
			[MethodImpl(MethodImplOptions.NoInlining)]
			public static int GetOutputCodePage()
			{
				return Console.WindowsConsole.GetConsoleOutputCP();
			}

			// Token: 0x0600194D RID: 6477 RVA: 0x0005EAAC File Offset: 0x0005CCAC
			public static void AddCtrlHandler()
			{
				Console.WindowsConsole.SetConsoleCtrlHandler(Console.WindowsConsole.cancelHandler, true);
				Console.WindowsConsole.ctrlHandlerAdded = true;
			}

			// Token: 0x0600194E RID: 6478 RVA: 0x0005EAC0 File Offset: 0x0005CCC0
			public static void RemoveCtrlHandler()
			{
				Console.WindowsConsole.SetConsoleCtrlHandler(Console.WindowsConsole.cancelHandler, false);
				Console.WindowsConsole.ctrlHandlerAdded = false;
			}

			// Token: 0x040016E4 RID: 5860
			public static bool ctrlHandlerAdded = false;

			// Token: 0x040016E5 RID: 5861
			private static Console.WindowsConsole.WindowsCancelHandler cancelHandler = new Console.WindowsConsole.WindowsCancelHandler(Console.WindowsConsole.DoWindowsConsoleCancelEvent);

			// Token: 0x0200022C RID: 556
			// (Invoke) Token: 0x06001952 RID: 6482
			private delegate bool WindowsCancelHandler(int keyCode);
		}
	}
}
