using System.Net;
using System.Text;

namespace De.HDBW.Apollo.Data.Tests.Remote
{
    public class FakeHttpContent : HttpContent
    {
        private readonly string _content;

        public FakeHttpContent(string content)
        {
            _content = content;
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            var buffer = Encoding.UTF8.GetBytes(_content);
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = Encoding.UTF8.GetByteCount(_content);
            return true;
        }
    }
}
