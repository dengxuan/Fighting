using System.Text;

namespace Fighting.Caching
{
    public class CachingOptions
    {
        public string ConnectionString { get; set; }

        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }
}
