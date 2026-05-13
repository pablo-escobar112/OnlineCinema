using ZaytsevG.Services;

namespace ZaytserG.test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            AuthService authService = new AuthService();
            var result = authService.TryAuth("d.kozlov@globus-tech.ru", "4p6q8r");
            Assert.NotNull(result);
        }

        [Fact]
        public void Test2()
        {
            AuthService authService = new AuthService();
            var result = authService.TryAuth("d.kozlov@globus-tech.ru", "");
            Assert.Null(result);
        }
    }
}