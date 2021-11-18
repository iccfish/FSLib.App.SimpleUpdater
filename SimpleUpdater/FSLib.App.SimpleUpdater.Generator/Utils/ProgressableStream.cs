using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator.Utils
{
	using System.IO;
	using System.Runtime.Remoting;
	using System.Threading;

	class ProgressStream : Stream
	{
		private Stream _stream;

		public ProgressStream(Stream stream) { _stream = stream; }

		/// <inheritdoc />
		public override void Flush() { _stream.Flush(); }

		/// <inheritdoc />
		public override long Seek(long offset, SeekOrigin origin)
		{
			var pos = _stream.Seek(offset, origin);
			OnPositionChanged();
			return pos;
		}

		/// <inheritdoc />
		public override void SetLength(long value)
		{
			_stream.SetLength(value);
			OnPositionChanged();
		}

		/// <inheritdoc />
		public override int Read(byte[] buffer, int offset, int count)
		{
			var actual = _stream.Read(buffer, offset, count);
			if (actual > 0)
				OnPositionChanged();

			return actual;
		}

		/// <inheritdoc />
		public override void Write(byte[] buffer, int offset, int count)
		{
			_stream.Write(buffer, offset, count);
			OnPositionChanged();

		}

		/// <inheritdoc />
		public override bool CanRead => _stream.CanRead;

		/// <inheritdoc />
		public override bool CanSeek => _stream.CanSeek;

		/// <inheritdoc />
		public override bool CanWrite => _stream.CanWrite;

		/// <inheritdoc />
		public override long Length => _stream.Length;

		/// <inheritdoc />
		public override long Position
		{
			get => _stream.Position;
			set => _stream.Position = value;
		}

		/// <summary>
		/// 当前流的进度发生变更
		/// </summary>
		public event EventHandler PositionChanged;

		/// <summary>
		/// 引发 <see cref="PositionChanged"/> 事件
		/// </summary>
		protected virtual void OnPositionChanged() { PositionChanged?.Invoke(this, EventArgs.Empty); }

		/// <inheritdoc />
		public override void Close() => _stream.Close();

		/// <inheritdoc />
		protected override void Dispose(bool disposing) => _stream.Dispose();


		/// <inheritdoc />
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state) => _stream.BeginRead(buffer, offset, count, callback, state);

		/// <inheritdoc />
		public override int EndRead(IAsyncResult asyncResult)
		{
			var ret = base.EndRead(asyncResult);
			OnPositionChanged();
			return ret;
		}

		/// <inheritdoc />
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) => _stream.BeginWrite(buffer, offset, count, callback, state);

		/// <inheritdoc />
		public override void EndWrite(IAsyncResult asyncResult)
		{
			_stream.EndWrite(asyncResult);
			OnPositionChanged();
		}

		/// <inheritdoc />
		public override int ReadByte()
		{
			var ret = _stream.ReadByte();
			OnPositionChanged();
			return ret;
		}

		/// <inheritdoc />
		public override void WriteByte(byte value)
		{
			_stream.WriteByte(value);
			OnPositionChanged();
		}

		/// <inheritdoc />
		public override bool CanTimeout => _stream.CanTimeout;

		/// <inheritdoc />
		public override int ReadTimeout
		{
			get => _stream.ReadTimeout;
			set => _stream.ReadTimeout = value;
		}

		/// <inheritdoc />
		public override int WriteTimeout
		{
			get => _stream.WriteTimeout;
			set => _stream.WriteTimeout = value;
		}
	}
}
