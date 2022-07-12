using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;
using System.Text;

using YTS.Log;
using YTS.ConsolePrint;

using DocumentDealWithCommand.Logic.Implementation;
using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Test
{
    [TestClass]
    public class TestMain_Rename_Whole
    {
        private ILog log;
        [TestInitialize]
        public void Init()
        {
            var logFile = ILogExtend.GetLogFilePath("TestMain_Rename_Whole");
            log = new FilePrintLog(logFile, Encoding.UTF8)
                .Connect(new BasicJSONConsolePrintLog());
        }

        [TestCleanup]
        public void Clean()
        {
        }

        private static ParamRenameWhole GetDefalutParam()
        {
            return new ParamRenameWhole()
            {
                RootDire = new DirectoryInfo("C:\\"),
                Config = null,
                ConsoleType = EConsoleType.WindowGitBash,

                IsPreview = true,
                NamingRules = "*",
                StartedOnIndex = 1,
                Increment = 1,
                Digit = 1,
                IsInsufficientSupplementBit = true,
                IsUseLetter = false,
                UseLetterFormat = ERenameLetterFormat.Lower,
                IsChangeExtension = false,
                ChangeExtensionValue = null,
                IsAutomaticallyResolveRenameConflicts = true,
            };
        }

        [TestMethod]
        public void TestToNewName_CalcNumberNo()
        {
            var main = new Main_Rename_Whole(log);
            ParamRenameWhole param = GetDefalutParam();
            param.NamingRules = "*";
            main.SetParam(param);

            Assert.AreEqual("test.jpg", main.ToNewName("test.jpg", 0));
            Assert.AreEqual("test.jpg", main.ToNewName("test.jpg", 1));
            Assert.AreEqual("test.jpg", main.ToNewName("test.jpg", 2));

            Assert.AreEqual("test.jpg", main.ToNewName("\\test.jpg", 2));
            Assert.AreEqual("test.jpg", main.ToNewName("D:\\test.jpg", 2));
            Assert.AreEqual("test.jpg", main.ToNewName("D:\\File\\test.jpg", 2));
            Assert.AreEqual("test.jpg", main.ToNewName("/test.jpg", 2));
            Assert.AreEqual("test.jpg", main.ToNewName("/file/test.jpg", 2));
            Assert.AreEqual("test.jpg", main.ToNewName("/var/File/test.jpg", 2));

            param.NamingRules = "*_#";

            param.StartedOnIndex = 1;
            param.Increment = 1;
            param.Digit = 1;
            Assert.AreEqual("test_1.jpg", main.ToNewName("test.jpg", 0));
            Assert.AreEqual("test_2.jpg", main.ToNewName("test.jpg", 1));
            Assert.AreEqual("test_3.jpg", main.ToNewName("test.jpg", 2));
            Assert.AreEqual("test_23.jpg", main.ToNewName("test.jpg", 22));
            Assert.AreEqual("test_416.jpg", main.ToNewName("test.jpg", 415));

            param.StartedOnIndex = 3;
            param.Increment = 1;
            param.Digit = 1;
            Assert.AreEqual("test_3.jpg", main.ToNewName("test.jpg", 0));
            Assert.AreEqual("test_4.jpg", main.ToNewName("test.jpg", 1));
            Assert.AreEqual("test_5.jpg", main.ToNewName("test.jpg", 2));
            Assert.AreEqual("test_25.jpg", main.ToNewName("test.jpg", 22));
            Assert.AreEqual("test_418.jpg", main.ToNewName("test.jpg", 415));

            param.StartedOnIndex = 1;
            param.Increment = 2;
            param.Digit = 1;
            Assert.AreEqual("test_1.jpg", main.ToNewName("test.jpg", 0));
            Assert.AreEqual("test_3.jpg", main.ToNewName("test.jpg", 1));
            Assert.AreEqual("test_5.jpg", main.ToNewName("test.jpg", 2));
            Assert.AreEqual("test_7.jpg", main.ToNewName("test.jpg", 3));
            Assert.AreEqual("test_9.jpg", main.ToNewName("test.jpg", 4));
            Assert.AreEqual("test_11.jpg", main.ToNewName("test.jpg", 5));
            Assert.AreEqual("test_13.jpg", main.ToNewName("test.jpg", 6));
            Assert.AreEqual("test_15.jpg", main.ToNewName("test.jpg", 7));
            Assert.AreEqual("test_17.jpg", main.ToNewName("test.jpg", 8));
            Assert.AreEqual("test_19.jpg", main.ToNewName("test.jpg", 9));
            Assert.AreEqual("test_21.jpg", main.ToNewName("test.jpg", 10));
            Assert.AreEqual("test_23.jpg", main.ToNewName("test.jpg", 11));

            param.StartedOnIndex = 0;
            param.Increment = 2;
            param.Digit = 1;
            Assert.AreEqual("test_0.jpg", main.ToNewName("test.jpg", 0));
            Assert.AreEqual("test_2.jpg", main.ToNewName("test.jpg", 1));
            Assert.AreEqual("test_4.jpg", main.ToNewName("test.jpg", 2));
            Assert.AreEqual("test_6.jpg", main.ToNewName("test.jpg", 3));
            Assert.AreEqual("test_8.jpg", main.ToNewName("test.jpg", 4));
            Assert.AreEqual("test_10.jpg", main.ToNewName("test.jpg", 5));
            Assert.AreEqual("test_12.jpg", main.ToNewName("test.jpg", 6));
            Assert.AreEqual("test_14.jpg", main.ToNewName("test.jpg", 7));
            Assert.AreEqual("test_16.jpg", main.ToNewName("test.jpg", 8));
            Assert.AreEqual("test_18.jpg", main.ToNewName("test.jpg", 9));
            Assert.AreEqual("test_20.jpg", main.ToNewName("test.jpg", 10));
            Assert.AreEqual("test_22.jpg", main.ToNewName("test.jpg", 11));
            Assert.AreEqual("test_24.jpg", main.ToNewName("test.jpg", 12));
            Assert.AreEqual("test_26.jpg", main.ToNewName("test.jpg", 13));
            Assert.AreEqual("test_28.jpg", main.ToNewName("test.jpg", 14));
            Assert.AreEqual("test_30.jpg", main.ToNewName("test.jpg", 15));

            param.StartedOnIndex = 1;
            param.Increment = 1;
            param.Digit = 3;
            Assert.AreEqual("test_001.jpg", main.ToNewName("test.jpg", 0));
            Assert.AreEqual("test_002.jpg", main.ToNewName("test.jpg", 1));
            Assert.AreEqual("test_003.jpg", main.ToNewName("test.jpg", 2));
            Assert.AreEqual("test_023.jpg", main.ToNewName("test.jpg", 22));
            Assert.AreEqual("test_416.jpg", main.ToNewName("test.jpg", 415));
        }

        [TestMethod]
        public void TestToNewName_CalcLetter()
        {
            var main = new Main_Rename_Whole(log);
            ParamRenameWhole param = GetDefalutParam();

            param.NamingRules = "*_#";
            param.IsUseLetter = true;
            param.UseLetterFormat = ERenameLetterFormat.Lower;
            param.StartedOnIndex = 1;
            param.Increment = 1;
            param.Digit = 1;
            main.SetParam(param);
            Assert.AreEqual("test_a.jpg", main.ToNewName("test.jpg", 0));
            Assert.AreEqual("test_b.jpg", main.ToNewName("test.jpg", 1));
            Assert.AreEqual("test_c.jpg", main.ToNewName("test.jpg", 2));
            Assert.AreEqual("test_d.jpg", main.ToNewName("test.jpg", 3));
            Assert.AreEqual("test_e.jpg", main.ToNewName("test.jpg", 4));
            Assert.AreEqual("test_f.jpg", main.ToNewName("test.jpg", 5));
            Assert.AreEqual("test_g.jpg", main.ToNewName("test.jpg", 6));
            Assert.AreEqual("test_h.jpg", main.ToNewName("test.jpg", 7));
            Assert.AreEqual("test_i.jpg", main.ToNewName("test.jpg", 8));
            Assert.AreEqual("test_j.jpg", main.ToNewName("test.jpg", 9));

            Assert.AreEqual("test_k.jpg", main.ToNewName("test.jpg", 10));
            Assert.AreEqual("test_l.jpg", main.ToNewName("test.jpg", 11));
            Assert.AreEqual("test_m.jpg", main.ToNewName("test.jpg", 12));
            Assert.AreEqual("test_n.jpg", main.ToNewName("test.jpg", 13));
            Assert.AreEqual("test_o.jpg", main.ToNewName("test.jpg", 14));
            Assert.AreEqual("test_p.jpg", main.ToNewName("test.jpg", 15));
            Assert.AreEqual("test_q.jpg", main.ToNewName("test.jpg", 16));
            Assert.AreEqual("test_r.jpg", main.ToNewName("test.jpg", 17));
            Assert.AreEqual("test_s.jpg", main.ToNewName("test.jpg", 18));
            Assert.AreEqual("test_t.jpg", main.ToNewName("test.jpg", 19));

            Assert.AreEqual("test_u.jpg", main.ToNewName("test.jpg", 20));
            Assert.AreEqual("test_v.jpg", main.ToNewName("test.jpg", 21));
            Assert.AreEqual("test_w.jpg", main.ToNewName("test.jpg", 22));
            Assert.AreEqual("test_x.jpg", main.ToNewName("test.jpg", 23));
            Assert.AreEqual("test_y.jpg", main.ToNewName("test.jpg", 24));
            Assert.AreEqual("test_z.jpg", main.ToNewName("test.jpg", 25));
            Assert.AreEqual("test_aa.jpg", main.ToNewName("test.jpg", 26));
            Assert.AreEqual("test_ab.jpg", main.ToNewName("test.jpg", 27));
            Assert.AreEqual("test_ac.jpg", main.ToNewName("test.jpg", 28));
            Assert.AreEqual("test_ad.jpg", main.ToNewName("test.jpg", 29));

            Assert.AreEqual("test_ae.jpg", main.ToNewName("test.jpg", 30));
            Assert.AreEqual("test_af.jpg", main.ToNewName("test.jpg", 31));
            Assert.AreEqual("test_ag.jpg", main.ToNewName("test.jpg", 32));
            Assert.AreEqual("test_ah.jpg", main.ToNewName("test.jpg", 33));
            Assert.AreEqual("test_ai.jpg", main.ToNewName("test.jpg", 34));
            Assert.AreEqual("test_aj.jpg", main.ToNewName("test.jpg", 35));
            Assert.AreEqual("test_ak.jpg", main.ToNewName("test.jpg", 36));
            Assert.AreEqual("test_al.jpg", main.ToNewName("test.jpg", 37));
            Assert.AreEqual("test_am.jpg", main.ToNewName("test.jpg", 38));
            Assert.AreEqual("test_an.jpg", main.ToNewName("test.jpg", 39));

            Assert.AreEqual("test_ao.jpg", main.ToNewName("test.jpg", 40));
            Assert.AreEqual("test_ap.jpg", main.ToNewName("test.jpg", 41));
            Assert.AreEqual("test_aq.jpg", main.ToNewName("test.jpg", 42));
            Assert.AreEqual("test_ar.jpg", main.ToNewName("test.jpg", 43));
            Assert.AreEqual("test_as.jpg", main.ToNewName("test.jpg", 44));
            Assert.AreEqual("test_at.jpg", main.ToNewName("test.jpg", 45));
            Assert.AreEqual("test_au.jpg", main.ToNewName("test.jpg", 46));
            Assert.AreEqual("test_av.jpg", main.ToNewName("test.jpg", 47));
            Assert.AreEqual("test_aw.jpg", main.ToNewName("test.jpg", 48));
            Assert.AreEqual("test_ax.jpg", main.ToNewName("test.jpg", 49));

            Assert.AreEqual("test_ay.jpg", main.ToNewName("test.jpg", 50));
            Assert.AreEqual("test_az.jpg", main.ToNewName("test.jpg", 51));
            Assert.AreEqual("test_ba.jpg", main.ToNewName("test.jpg", 52));
            Assert.AreEqual("test_bb.jpg", main.ToNewName("test.jpg", 53));
            Assert.AreEqual("test_bc.jpg", main.ToNewName("test.jpg", 54));
            Assert.AreEqual("test_bd.jpg", main.ToNewName("test.jpg", 55));
            Assert.AreEqual("test_be.jpg", main.ToNewName("test.jpg", 56));
            Assert.AreEqual("test_bf.jpg", main.ToNewName("test.jpg", 57));
            Assert.AreEqual("test_bg.jpg", main.ToNewName("test.jpg", 58));
            Assert.AreEqual("test_bh.jpg", main.ToNewName("test.jpg", 59));

            param.UseLetterFormat = ERenameLetterFormat.Upper;
            Assert.AreEqual("test_AU.jpg", main.ToNewName("test.jpg", 46));
            Assert.AreEqual("test_AY.jpg", main.ToNewName("test.jpg", 50));
            Assert.AreEqual("test_AZ.jpg", main.ToNewName("test.jpg", 51));
            Assert.AreEqual("test_BA.jpg", main.ToNewName("test.jpg", 52));

            param.UseLetterFormat = ERenameLetterFormat.LowerAndUpper;
            Assert.AreEqual("test_U.jpg", main.ToNewName("test.jpg", 46));
            Assert.AreEqual("test_Y.jpg", main.ToNewName("test.jpg", 50));
            Assert.AreEqual("test_Z.jpg", main.ToNewName("test.jpg", 51));
            Assert.AreEqual("test_aa.jpg", main.ToNewName("test.jpg", 52));

            param.UseLetterFormat = ERenameLetterFormat.UpperAndLower;
            Assert.AreEqual("test_u.jpg", main.ToNewName("test.jpg", 46));
            Assert.AreEqual("test_y.jpg", main.ToNewName("test.jpg", 50));
            Assert.AreEqual("test_z.jpg", main.ToNewName("test.jpg", 51));
            Assert.AreEqual("test_AA.jpg", main.ToNewName("test.jpg", 52));

            param.UseLetterFormat = ERenameLetterFormat.Lower;
            param.StartedOnIndex = 3;
            param.Increment = 1;
            param.Digit = 1;
            Assert.AreEqual("test_c.jpg", main.ToNewName("test.jpg", 0));
            Assert.AreEqual("test_d.jpg", main.ToNewName("test.jpg", 1));
            Assert.AreEqual("test_e.jpg", main.ToNewName("test.jpg", 2));

            param.StartedOnIndex = 1;
            param.Increment = 2;
            param.Digit = 1;
            Assert.AreEqual("test_a.jpg", main.ToNewName("test.jpg", 0));
            Assert.AreEqual("test_c.jpg", main.ToNewName("test.jpg", 1));
            Assert.AreEqual("test_e.jpg", main.ToNewName("test.jpg", 2));
            Assert.AreEqual("test_g.jpg", main.ToNewName("test.jpg", 3));

            param.StartedOnIndex = 0;
            param.Increment = 1;
            param.Digit = 1;
            Assert.AreEqual("test_0.jpg", main.ToNewName("test.jpg", 0));
            Assert.AreEqual("test_a.jpg", main.ToNewName("test.jpg", 1));
            Assert.AreEqual("test_b.jpg", main.ToNewName("test.jpg", 2));

            param.StartedOnIndex = 1;
            param.Increment = 1;
            param.Digit = 3;
            Assert.AreEqual("test_0bd.jpg", main.ToNewName("test.jpg", 55));
            Assert.AreEqual("test_0be.jpg", main.ToNewName("test.jpg", 56));
            Assert.AreEqual("test_0bf.jpg", main.ToNewName("test.jpg", 57));
            Assert.AreEqual("test_0bg.jpg", main.ToNewName("test.jpg", 58));
            Assert.AreEqual("test_0bh.jpg", main.ToNewName("test.jpg", 59));
        }

        [TestMethod]
        public void TestToNewName_ChangeExtension()
        {
            var main = new Main_Rename_Whole(log);
            ParamRenameWhole param = GetDefalutParam();
            param.NamingRules = "*_#";

            param.IsChangeExtension = true;
            param.ChangeExtensionValue = ".png";
            main.SetParam(param);
            Assert.AreEqual("test_1.png", main.ToNewName("test.jpg", 0));
            Assert.AreEqual("test_2.png", main.ToNewName("test.jpg", 1));
            Assert.AreEqual("test_3.png", main.ToNewName("test.jpg", 2));

            param.ChangeExtensionValue = "png";
            main.SetParam(param);
            Assert.AreEqual("test_1png", main.ToNewName("test.jpg", 0));
            Assert.AreEqual("test_2png", main.ToNewName("test.jpg", 1));
            Assert.AreEqual("test_3png", main.ToNewName("test.jpg", 2));
        }
    }
}
