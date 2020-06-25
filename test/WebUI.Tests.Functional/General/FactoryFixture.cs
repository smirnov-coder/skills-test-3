using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebUI.Tests.Functional
{
    public class FactoryFixture : IDisposable
    {
        // 4 разные фабрики нужны, чтобя хоть немного сократить время на выполнение тестов.
        public CustomWebApplicationFactory ForRead { get; private set; }
        public CustomWebApplicationFactory ForAdd { get; private set; }
        public CustomWebApplicationFactory ForUpdate { get; private set; }
        public CustomWebApplicationFactory ForDelete { get; private set; }

        public FactoryFixture()
        {
            ForRead = new CustomWebApplicationFactory();
            ForAdd = new CustomWebApplicationFactory();
            ForUpdate = new CustomWebApplicationFactory();
            ForDelete = new CustomWebApplicationFactory();
        }
        
        public void Dispose()
        {
            ForRead?.Dispose();
            ForAdd?.Dispose();
            ForUpdate?.Dispose();
            ForDelete?.Dispose();
        }
    }

    [CollectionDefinition(nameof(FactoryCollection))]
    public class FactoryCollection : ICollectionFixture<FactoryFixture>
    {
    }
}
