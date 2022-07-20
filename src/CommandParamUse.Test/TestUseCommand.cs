using System.Text.RegularExpressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandParamUse.Test
{
    [TestClass]
    public class TestUseCommand
    {
        [TestMethod]
        public void Test_Use()
        {
            string user_args = "rename --rule \"test_#\" --path \"D:\\Work\\YTS.ZRQ\\DocumentDealWith\\_test\\rename\"";
            string[] args = Regex.Split(user_args, @"\s+", RegexOptions.IgnoreCase | RegexOptions.ECMAScript);
        }
    }
}
