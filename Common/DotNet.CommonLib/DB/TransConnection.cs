using System.Data.Common;

namespace DotNet.CommonLib.DB
{
    internal class TransConnection
    {
        public TransConnection()
        {
            this.Deeps = 0;
        }

        public DbTransaction DBTransaction { get; set; }

        public int Deeps { get; set; }
    }
}