using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Xml
{
	// Token: 0x02000049 RID: 73
	internal class ReadContentAsBinaryHelper
	{
		// Token: 0x06000261 RID: 609 RVA: 0x0000DCBA File Offset: 0x0000BEBA
		internal ReadContentAsBinaryHelper(XmlReader reader)
		{
			this.reader = reader;
			this.canReadValueChunk = reader.CanReadValueChunk;
			if (this.canReadValueChunk)
			{
				this.valueChunk = new char[256];
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000DCED File Offset: 0x0000BEED
		internal static ReadContentAsBinaryHelper CreateOrReset(ReadContentAsBinaryHelper helper, XmlReader reader)
		{
			if (helper == null)
			{
				return new ReadContentAsBinaryHelper(reader);
			}
			helper.Reset();
			return helper;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000DD00 File Offset: 0x0000BF00
		internal int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			switch (this.state)
			{
			case ReadContentAsBinaryHelper.State.None:
				if (!this.reader.CanReadContentAs())
				{
					throw this.reader.CreateReadContentAsException("ReadContentAsBase64");
				}
				if (!this.Init())
				{
					return 0;
				}
				break;
			case ReadContentAsBinaryHelper.State.InReadContent:
				if (this.decoder == this.base64Decoder)
				{
					return this.ReadContentAsBinary(buffer, index, count);
				}
				break;
			case ReadContentAsBinaryHelper.State.InReadElementContent:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			default:
				return 0;
			}
			this.InitBase64Decoder();
			return this.ReadContentAsBinary(buffer, index, count);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000DDC8 File Offset: 0x0000BFC8
		internal int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			switch (this.state)
			{
			case ReadContentAsBinaryHelper.State.None:
				if (!this.reader.CanReadContentAs())
				{
					throw this.reader.CreateReadContentAsException("ReadContentAsBinHex");
				}
				if (!this.Init())
				{
					return 0;
				}
				break;
			case ReadContentAsBinaryHelper.State.InReadContent:
				if (this.decoder == this.binHexDecoder)
				{
					return this.ReadContentAsBinary(buffer, index, count);
				}
				break;
			case ReadContentAsBinaryHelper.State.InReadElementContent:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			default:
				return 0;
			}
			this.InitBinHexDecoder();
			return this.ReadContentAsBinary(buffer, index, count);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000DE90 File Offset: 0x0000C090
		internal int ReadElementContentAsBase64(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			switch (this.state)
			{
			case ReadContentAsBinaryHelper.State.None:
				if (this.reader.NodeType != XmlNodeType.Element)
				{
					throw this.reader.CreateReadElementContentAsException("ReadElementContentAsBase64");
				}
				if (!this.InitOnElement())
				{
					return 0;
				}
				break;
			case ReadContentAsBinaryHelper.State.InReadContent:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			case ReadContentAsBinaryHelper.State.InReadElementContent:
				if (this.decoder == this.base64Decoder)
				{
					return this.ReadElementContentAsBinary(buffer, index, count);
				}
				break;
			default:
				return 0;
			}
			this.InitBase64Decoder();
			return this.ReadElementContentAsBinary(buffer, index, count);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000DF5C File Offset: 0x0000C15C
		internal int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			switch (this.state)
			{
			case ReadContentAsBinaryHelper.State.None:
				if (this.reader.NodeType != XmlNodeType.Element)
				{
					throw this.reader.CreateReadElementContentAsException("ReadElementContentAsBinHex");
				}
				if (!this.InitOnElement())
				{
					return 0;
				}
				break;
			case ReadContentAsBinaryHelper.State.InReadContent:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			case ReadContentAsBinaryHelper.State.InReadElementContent:
				if (this.decoder == this.binHexDecoder)
				{
					return this.ReadElementContentAsBinary(buffer, index, count);
				}
				break;
			default:
				return 0;
			}
			this.InitBinHexDecoder();
			return this.ReadElementContentAsBinary(buffer, index, count);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000E028 File Offset: 0x0000C228
		internal void Finish()
		{
			if (this.state != ReadContentAsBinaryHelper.State.None)
			{
				while (this.MoveToNextContentNode(true))
				{
				}
				if (this.state == ReadContentAsBinaryHelper.State.InReadElementContent)
				{
					if (this.reader.NodeType != XmlNodeType.EndElement)
					{
						throw new XmlException("'{0}' is an invalid XmlNodeType.", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
					}
					this.reader.Read();
				}
			}
			this.Reset();
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000E09F File Offset: 0x0000C29F
		internal void Reset()
		{
			this.state = ReadContentAsBinaryHelper.State.None;
			this.isEnd = false;
			this.valueOffset = 0;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000E0B6 File Offset: 0x0000C2B6
		private bool Init()
		{
			if (!this.MoveToNextContentNode(false))
			{
				return false;
			}
			this.state = ReadContentAsBinaryHelper.State.InReadContent;
			this.isEnd = false;
			return true;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000E0D4 File Offset: 0x0000C2D4
		private bool InitOnElement()
		{
			bool isEmptyElement = this.reader.IsEmptyElement;
			this.reader.Read();
			if (isEmptyElement)
			{
				return false;
			}
			if (this.MoveToNextContentNode(false))
			{
				this.state = ReadContentAsBinaryHelper.State.InReadElementContent;
				this.isEnd = false;
				return true;
			}
			if (this.reader.NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("'{0}' is an invalid XmlNodeType.", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
			}
			this.reader.Read();
			return false;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000E160 File Offset: 0x0000C360
		private void InitBase64Decoder()
		{
			if (this.base64Decoder == null)
			{
				this.base64Decoder = new Base64Decoder();
			}
			else
			{
				this.base64Decoder.Reset();
			}
			this.decoder = this.base64Decoder;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000E18E File Offset: 0x0000C38E
		private void InitBinHexDecoder()
		{
			if (this.binHexDecoder == null)
			{
				this.binHexDecoder = new BinHexDecoder();
			}
			else
			{
				this.binHexDecoder.Reset();
			}
			this.decoder = this.binHexDecoder;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000E1BC File Offset: 0x0000C3BC
		private int ReadContentAsBinary(byte[] buffer, int index, int count)
		{
			if (this.isEnd)
			{
				this.Reset();
				return 0;
			}
			this.decoder.SetNextOutputBuffer(buffer, index, count);
			for (;;)
			{
				if (this.canReadValueChunk)
				{
					for (;;)
					{
						if (this.valueOffset < this.valueChunkLength)
						{
							int num = this.decoder.Decode(this.valueChunk, this.valueOffset, this.valueChunkLength - this.valueOffset);
							this.valueOffset += num;
						}
						if (this.decoder.IsFull)
						{
							goto Block_3;
						}
						if ((this.valueChunkLength = this.reader.ReadValueChunk(this.valueChunk, 0, 256)) == 0)
						{
							break;
						}
						this.valueOffset = 0;
					}
				}
				else
				{
					string value = this.reader.Value;
					int num2 = this.decoder.Decode(value, this.valueOffset, value.Length - this.valueOffset);
					this.valueOffset += num2;
					if (this.decoder.IsFull)
					{
						goto Block_5;
					}
				}
				this.valueOffset = 0;
				if (!this.MoveToNextContentNode(true))
				{
					goto Block_6;
				}
			}
			Block_3:
			return this.decoder.DecodedCount;
			Block_5:
			return this.decoder.DecodedCount;
			Block_6:
			this.isEnd = true;
			return this.decoder.DecodedCount;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000E2F4 File Offset: 0x0000C4F4
		private int ReadElementContentAsBinary(byte[] buffer, int index, int count)
		{
			if (count == 0)
			{
				return 0;
			}
			int num = this.ReadContentAsBinary(buffer, index, count);
			if (num > 0)
			{
				return num;
			}
			if (this.reader.NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("'{0}' is an invalid XmlNodeType.", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
			}
			this.reader.Read();
			this.state = ReadContentAsBinaryHelper.State.None;
			return 0;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000E368 File Offset: 0x0000C568
		private bool MoveToNextContentNode(bool moveIfOnContentNode)
		{
			for (;;)
			{
				switch (this.reader.NodeType)
				{
				case XmlNodeType.Attribute:
					goto IL_0052;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					if (!moveIfOnContentNode)
					{
						return true;
					}
					goto IL_0078;
				case XmlNodeType.EntityReference:
					if (this.reader.CanResolveEntity)
					{
						this.reader.ResolveEntity();
						goto IL_0078;
					}
					break;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Comment:
				case XmlNodeType.EndEntity:
					goto IL_0078;
				}
				break;
				IL_0078:
				moveIfOnContentNode = false;
				if (!this.reader.Read())
				{
					return false;
				}
			}
			return false;
			IL_0052:
			return !moveIfOnContentNode;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000E404 File Offset: 0x0000C604
		internal async Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			switch (this.state)
			{
			case ReadContentAsBinaryHelper.State.None:
			{
				if (!this.reader.CanReadContentAs())
				{
					throw this.reader.CreateReadContentAsException("ReadContentAsBase64");
				}
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.InitAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					return 0;
				}
				break;
			}
			case ReadContentAsBinaryHelper.State.InReadContent:
				if (this.decoder == this.base64Decoder)
				{
					return await this.ReadContentAsBinaryAsync(buffer, index, count).ConfigureAwait(false);
				}
				break;
			case ReadContentAsBinaryHelper.State.InReadElementContent:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			default:
				return 0;
			}
			this.InitBase64Decoder();
			return await this.ReadContentAsBinaryAsync(buffer, index, count).ConfigureAwait(false);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000E460 File Offset: 0x0000C660
		internal async Task<int> ReadContentAsBinHexAsync(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			switch (this.state)
			{
			case ReadContentAsBinaryHelper.State.None:
			{
				if (!this.reader.CanReadContentAs())
				{
					throw this.reader.CreateReadContentAsException("ReadContentAsBinHex");
				}
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.InitAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					return 0;
				}
				break;
			}
			case ReadContentAsBinaryHelper.State.InReadContent:
				if (this.decoder == this.binHexDecoder)
				{
					return await this.ReadContentAsBinaryAsync(buffer, index, count).ConfigureAwait(false);
				}
				break;
			case ReadContentAsBinaryHelper.State.InReadElementContent:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			default:
				return 0;
			}
			this.InitBinHexDecoder();
			return await this.ReadContentAsBinaryAsync(buffer, index, count).ConfigureAwait(false);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000E4BC File Offset: 0x0000C6BC
		internal async Task<int> ReadElementContentAsBase64Async(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			switch (this.state)
			{
			case ReadContentAsBinaryHelper.State.None:
			{
				if (this.reader.NodeType != XmlNodeType.Element)
				{
					throw this.reader.CreateReadElementContentAsException("ReadElementContentAsBase64");
				}
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.InitOnElementAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					return 0;
				}
				break;
			}
			case ReadContentAsBinaryHelper.State.InReadContent:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			case ReadContentAsBinaryHelper.State.InReadElementContent:
				if (this.decoder == this.base64Decoder)
				{
					return await this.ReadElementContentAsBinaryAsync(buffer, index, count).ConfigureAwait(false);
				}
				break;
			default:
				return 0;
			}
			this.InitBase64Decoder();
			return await this.ReadElementContentAsBinaryAsync(buffer, index, count).ConfigureAwait(false);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000E518 File Offset: 0x0000C718
		internal async Task<int> ReadElementContentAsBinHexAsync(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			switch (this.state)
			{
			case ReadContentAsBinaryHelper.State.None:
			{
				if (this.reader.NodeType != XmlNodeType.Element)
				{
					throw this.reader.CreateReadElementContentAsException("ReadElementContentAsBinHex");
				}
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.InitOnElementAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					return 0;
				}
				break;
			}
			case ReadContentAsBinaryHelper.State.InReadContent:
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			case ReadContentAsBinaryHelper.State.InReadElementContent:
				if (this.decoder == this.binHexDecoder)
				{
					return await this.ReadElementContentAsBinaryAsync(buffer, index, count).ConfigureAwait(false);
				}
				break;
			default:
				return 0;
			}
			this.InitBinHexDecoder();
			return await this.ReadElementContentAsBinaryAsync(buffer, index, count).ConfigureAwait(false);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000E574 File Offset: 0x0000C774
		internal async Task FinishAsync()
		{
			if (this.state != ReadContentAsBinaryHelper.State.None)
			{
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter;
				do
				{
					configuredTaskAwaiter = this.MoveToNextContentNodeAsync(true).ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
					}
				}
				while (configuredTaskAwaiter.GetResult());
				if (this.state == ReadContentAsBinaryHelper.State.InReadElementContent)
				{
					if (this.reader.NodeType != XmlNodeType.EndElement)
					{
						throw new XmlException("'{0}' is an invalid XmlNodeType.", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
					}
					await this.reader.ReadAsync().ConfigureAwait(false);
				}
			}
			this.Reset();
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000E5B8 File Offset: 0x0000C7B8
		private async Task<bool> InitAsync()
		{
			ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.MoveToNextContentNodeAsync(false).ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
			}
			bool flag;
			if (!configuredTaskAwaiter.GetResult())
			{
				flag = false;
			}
			else
			{
				this.state = ReadContentAsBinaryHelper.State.InReadContent;
				this.isEnd = false;
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000E5FC File Offset: 0x0000C7FC
		private async Task<bool> InitOnElementAsync()
		{
			bool isEmpty = this.reader.IsEmptyElement;
			await this.reader.ReadAsync().ConfigureAwait(false);
			bool flag;
			if (isEmpty)
			{
				flag = false;
			}
			else
			{
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.MoveToNextContentNodeAsync(false).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					if (this.reader.NodeType != XmlNodeType.EndElement)
					{
						throw new XmlException("'{0}' is an invalid XmlNodeType.", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
					}
					await this.reader.ReadAsync().ConfigureAwait(false);
					flag = false;
				}
				else
				{
					this.state = ReadContentAsBinaryHelper.State.InReadElementContent;
					this.isEnd = false;
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000E640 File Offset: 0x0000C840
		private async Task<int> ReadContentAsBinaryAsync(byte[] buffer, int index, int count)
		{
			int num;
			if (this.isEnd)
			{
				this.Reset();
				num = 0;
			}
			else
			{
				this.decoder.SetNextOutputBuffer(buffer, index, count);
				for (;;)
				{
					if (this.canReadValueChunk)
					{
						for (;;)
						{
							if (this.valueOffset < this.valueChunkLength)
							{
								int num2 = this.decoder.Decode(this.valueChunk, this.valueOffset, this.valueChunkLength - this.valueOffset);
								this.valueOffset += num2;
							}
							if (this.decoder.IsFull)
							{
								goto Block_3;
							}
							int num3 = await this.reader.ReadValueChunkAsync(this.valueChunk, 0, 256).ConfigureAwait(false);
							int num4 = num3;
							this.valueChunkLength = num4;
							if (num4 == 0)
							{
								break;
							}
							this.valueOffset = 0;
						}
					}
					else
					{
						string text = await this.reader.GetValueAsync().ConfigureAwait(false);
						int num5 = this.decoder.Decode(text, this.valueOffset, text.Length - this.valueOffset);
						this.valueOffset += num5;
						if (this.decoder.IsFull)
						{
							goto Block_5;
						}
					}
					this.valueOffset = 0;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.MoveToNextContentNodeAsync(true).ConfigureAwait(false).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						await configuredTaskAwaiter;
						ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
					}
					if (!configuredTaskAwaiter.GetResult())
					{
						goto Block_7;
					}
				}
				Block_3:
				return this.decoder.DecodedCount;
				Block_5:
				return this.decoder.DecodedCount;
				Block_7:
				this.isEnd = true;
				num = this.decoder.DecodedCount;
			}
			return num;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000E69C File Offset: 0x0000C89C
		private async Task<int> ReadElementContentAsBinaryAsync(byte[] buffer, int index, int count)
		{
			int num;
			if (count == 0)
			{
				num = 0;
			}
			else
			{
				int num2 = await this.ReadContentAsBinaryAsync(buffer, index, count).ConfigureAwait(false);
				if (num2 > 0)
				{
					num = num2;
				}
				else
				{
					if (this.reader.NodeType != XmlNodeType.EndElement)
					{
						throw new XmlException("'{0}' is an invalid XmlNodeType.", this.reader.NodeType.ToString(), this.reader as IXmlLineInfo);
					}
					await this.reader.ReadAsync().ConfigureAwait(false);
					this.state = ReadContentAsBinaryHelper.State.None;
					num = 0;
				}
			}
			return num;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000E6F8 File Offset: 0x0000C8F8
		private async Task<bool> MoveToNextContentNodeAsync(bool moveIfOnContentNode)
		{
			for (;;)
			{
				switch (this.reader.NodeType)
				{
				case XmlNodeType.Attribute:
					goto IL_0066;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					if (!moveIfOnContentNode)
					{
						goto Block_1;
					}
					goto IL_00A5;
				case XmlNodeType.EntityReference:
					if (this.reader.CanResolveEntity)
					{
						this.reader.ResolveEntity();
						goto IL_00A5;
					}
					break;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Comment:
				case XmlNodeType.EndEntity:
					goto IL_00A5;
				}
				break;
				IL_00A5:
				moveIfOnContentNode = false;
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.reader.ReadAsync().ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					goto Block_4;
				}
			}
			goto IL_009E;
			IL_0066:
			return !moveIfOnContentNode;
			Block_1:
			return true;
			IL_009E:
			return false;
			Block_4:
			return false;
		}

		// Token: 0x04000629 RID: 1577
		private XmlReader reader;

		// Token: 0x0400062A RID: 1578
		private ReadContentAsBinaryHelper.State state;

		// Token: 0x0400062B RID: 1579
		private int valueOffset;

		// Token: 0x0400062C RID: 1580
		private bool isEnd;

		// Token: 0x0400062D RID: 1581
		private bool canReadValueChunk;

		// Token: 0x0400062E RID: 1582
		private char[] valueChunk;

		// Token: 0x0400062F RID: 1583
		private int valueChunkLength;

		// Token: 0x04000630 RID: 1584
		private IncrementalReadDecoder decoder;

		// Token: 0x04000631 RID: 1585
		private Base64Decoder base64Decoder;

		// Token: 0x04000632 RID: 1586
		private BinHexDecoder binHexDecoder;

		// Token: 0x04000633 RID: 1587
		private const int ChunkSize = 256;

		// Token: 0x0200004A RID: 74
		private enum State
		{
			// Token: 0x04000635 RID: 1589
			None,
			// Token: 0x04000636 RID: 1590
			InReadContent,
			// Token: 0x04000637 RID: 1591
			InReadElementContent
		}
	}
}
