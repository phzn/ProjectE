using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x020006F2 RID: 1778
	internal sealed class BicycleDiagnosticSessionWriter : IDisposable
	{
		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06002D37 RID: 11575 RVA: 0x000F9888 File Offset: 0x000F7A88
		// (set) Token: 0x06002D38 RID: 11576 RVA: 0x000F9890 File Offset: 0x000F7A90
		public string CurrentPath { get; private set; } = string.Empty;

		// Token: 0x06002D39 RID: 11577 RVA: 0x000F989C File Offset: 0x000F7A9C
		public BicycleDiagnosticSessionWriter(string directory, string session, long maximumPartBytes)
		{
			if (string.IsNullOrWhiteSpace(directory))
			{
				throw new ArgumentException("A log directory is required.", "directory");
			}
			if (string.IsNullOrWhiteSpace(session))
			{
				throw new ArgumentException("A session ID is required.", "session");
			}
			if (maximumPartBytes < 1L)
			{
				throw new ArgumentOutOfRangeException("maximumPartBytes");
			}
			this.directory = directory;
			this.session = session;
			this.maximumPartBytes = maximumPartBytes;
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x000F9910 File Offset: 0x000F7B10
		public void WriteJsonLine(string json)
		{
			if (json == null)
			{
				throw new ArgumentNullException("json");
			}
			long num = (long)BicycleDiagnosticSessionWriter.Utf8.GetByteCount(json) + 1L;
			if (this.streamWriter != null && this.currentBytes > 0L && this.currentBytes + num > this.maximumPartBytes)
			{
				this.ClosePart();
			}
			if (this.streamWriter == null)
			{
				this.OpenPart();
			}
			this.streamWriter.WriteLine(json);
			this.currentBytes += num;
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x000F998B File Offset: 0x000F7B8B
		public void Flush()
		{
			StreamWriter streamWriter = this.streamWriter;
			if (streamWriter == null)
			{
				return;
			}
			streamWriter.Flush();
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x000F999D File Offset: 0x000F7B9D
		public void Dispose()
		{
			this.ClosePart();
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000F99A8 File Offset: 0x000F7BA8
		private void OpenPart()
		{
			Directory.CreateDirectory(this.directory);
			string text;
			do
			{
				this.part++;
				text = Path.Combine(this.directory, string.Concat(new string[]
				{
					"bicycle_",
					this.session,
					"_part",
					this.part.ToString("D3", CultureInfo.InvariantCulture),
					".jsonl"
				}));
			}
			while (File.Exists(text));
			FileStream fileStream = new FileStream(text, FileMode.CreateNew, FileAccess.Write, FileShare.Read, 65536);
			try
			{
				this.streamWriter = new StreamWriter(fileStream, BicycleDiagnosticSessionWriter.Utf8, 65536)
				{
					NewLine = "\n"
				};
			}
			catch
			{
				fileStream.Dispose();
				throw;
			}
			this.CurrentPath = text;
			this.currentBytes = 0L;
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x000F9A84 File Offset: 0x000F7C84
		private void ClosePart()
		{
			StreamWriter streamWriter = this.streamWriter;
			this.streamWriter = null;
			if (streamWriter == null)
			{
				return;
			}
			streamWriter.Dispose();
		}

		// Token: 0x04002C27 RID: 11303
		private static readonly UTF8Encoding Utf8 = new UTF8Encoding(false);

		// Token: 0x04002C28 RID: 11304
		private readonly string directory;

		// Token: 0x04002C29 RID: 11305
		private readonly string session;

		// Token: 0x04002C2A RID: 11306
		private readonly long maximumPartBytes;

		// Token: 0x04002C2B RID: 11307
		private StreamWriter streamWriter;

		// Token: 0x04002C2C RID: 11308
		private long currentBytes;

		// Token: 0x04002C2D RID: 11309
		private int part;
	}
}
