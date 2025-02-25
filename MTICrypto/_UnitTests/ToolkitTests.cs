using Xunit.Abstractions;

namespace _UnitTests
{
    public class ToolkitTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ToolkitTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Must_Get_Key()
        {
            string key = MTICrypto.Toolkit.GetKey();
            _testOutputHelper.WriteLine($"key=[{key}]");

            Assert.True(Convert.FromBase64String(key).Length == 32);
        }

        [Fact]
        public void Must_Get_Environment_Variable()
        {
            string mivalor = Environment.GetEnvironmentVariable("CLAVE_CIFRADO");

            _testOutputHelper.WriteLine($"key=[{mivalor}]");

            Assert.NotNull(mivalor);
        }
    }
}