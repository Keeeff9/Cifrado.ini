using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace _UnitTests
{
    public class CryptoTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CryptoTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Must_Encrypt_OK()
        {
            string keyBase64 = MTICrypto.Toolkit.GetKey();
            string dataToTest = "Texto de prueba para cifrado aes";
            MTICrypto.AESEncrypt aESEncrypt = new MTICrypto.AESEncrypt(dataToTest, keyBase64);

            string outOne = Convert.ToBase64String(aESEncrypt.Encrypt());
            string outTwo = Convert.ToBase64String(aESEncrypt.Encrypt());

            _testOutputHelper.WriteLine($"uno=[{outOne}]");
            _testOutputHelper.WriteLine($"dos=[{outTwo}]");

            Assert.False(outOne.Equals(outTwo));
        }

        [Fact]
        public void Must_Decrypt_OK()
        {
            string keyBase64 = MTICrypto.Toolkit.GetKey();
            string dataToTest =
                "Server=127.0.0.1;Port=5432;Database=myDataBase;User Id=myUsername;Password=myPassword;";
            MTICrypto.AESEncrypt aESEncrypt = new MTICrypto.AESEncrypt(dataToTest, keyBase64);

            string outOne = Convert.ToBase64String(aESEncrypt.Encrypt());
            string outTwo = Convert.ToBase64String(aESEncrypt.Encrypt());

            _testOutputHelper.WriteLine($"uno=[{outOne}]");
            _testOutputHelper.WriteLine($"dos=[{outTwo}]");

            MTICrypto.AESDecrypt aESDecrypt = new MTICrypto.AESDecrypt(outOne, keyBase64);
            string plainOne = MTICrypto.Toolkit.GetStringFromCipherBytes(aESDecrypt.Decrypt());

            aESDecrypt = new MTICrypto.AESDecrypt(outTwo, keyBase64);
            string plainTwo = MTICrypto.Toolkit.GetStringFromCipherBytes(aESDecrypt.Decrypt());

            _testOutputHelper.WriteLine($"uno=[{plainOne}]");
            _testOutputHelper.WriteLine($"dos=[{plainTwo}]");

            Assert.True(plainOne.Equals(plainTwo) && plainOne.Equals(dataToTest));
        }

        [Fact]
        public void Must_Decrypt_From_Value_OK()
        {
            string keyBase64 = "7MrKAciFH3rv40XgNab5VaauPSjax79MSU1mIqC9xTQ=";

            string outOne = "PsEYZp6xmiAVOkxKcVkMDbkEG+etS6MXmZ49FcMYD67d/7G3OANbA2bvlwLQRgey3m0zBUf1lLH2TfSx7UIpN6xP+8nJA+PRSQVEYEQCCasRLiio4vtb0UcZpbpPg4fNRxL1AixVzDzVQ9YUfOJe7ocDBv5quBrlesfmDH9d+CUx3n7NFlfB9Mv2Er5DWo+xWtoyL0FXwTHQxGrpjVLTag==";

            _testOutputHelper.WriteLine($"uno=[{outOne}]");

            MTICrypto.AESDecrypt aESDecrypt = new MTICrypto.AESDecrypt(outOne, keyBase64);
            string plainOne = MTICrypto.Toolkit.GetStringFromCipherBytes(aESDecrypt.Decrypt());

            _testOutputHelper.WriteLine($"uno=[{plainOne}]");

            Assert.Contains("qadtorres", plainOne.ToLower());
        }

        [Fact]
        public void Must_Encrypt_From_Value_OK()
        {
            string keyBase64 = "7MrKAciFH3rv40XgNab5VaauPSjax79MSU1mIqC9xTQ=";

            string plainOne = "Server=10.10.253.19; Database=registraduria; User Id=QADTORRES; Password=Test1974; SSl Mode=Require; Trust Server Certificate=true;";

            _testOutputHelper.WriteLine($"uno=[{plainOne}]");

            MTICrypto.AESEncrypt aESEncrypt = new MTICrypto.AESEncrypt(plainOne, keyBase64);
            string outOne = Convert.ToBase64String(aESEncrypt.Encrypt());
            string outTwo = Convert.ToBase64String(aESEncrypt.Encrypt());

            _testOutputHelper.WriteLine($"uno=[{outOne}]");
            _testOutputHelper.WriteLine($"uno=[{outTwo}]");

            Assert.False(outOne.Equals(outTwo));
        }


        [Fact]
        public void Must_Encrypt_File_Value_OK()
        {
            string keyBase64 = "7MrKAciFH3rv40XgNab5VaauPSjax79MSU1mIqC9xTQ=";

            string plainOne = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "File", "configuracionTest.ini"));

            _testOutputHelper.WriteLine($"uno=[{plainOne}]");

            MTICrypto.AESEncrypt aESEncrypt = new MTICrypto.AESEncrypt(plainOne, keyBase64);
            string outOne = Convert.ToBase64String(aESEncrypt.Encrypt());
            string outTwo = Convert.ToBase64String(aESEncrypt.Encrypt());

            _testOutputHelper.WriteLine($"uno=[{outOne}]");
            _testOutputHelper.WriteLine($"uno=[{outTwo}]");

            Assert.False(outOne.Equals(outTwo));
        }

        [Fact]
        public void Must_Decrypt_File_Value_OK()
        {
            string keyBase64 = "7MrKAciFH3rv40XgNab5VaauPSjax79MSU1mIqC9xTQ=";

            string outOne = "tGtj3dWuDtLhx06+vvMkwm8Q+5tDPmN86Z8z5v41jwODMbcW0bHz/iUd6le96pJ86uEenusdaXrgpY0FwrYRsxhWuN8KRkWNL3qiVZwKowV2+X5aVM6MGWkPzVSk/Wsm57aFsSffK/7/MsT4pYANL8c4biOFAg9YpuvaeKoWOukLNs8CcmWpv2365WhegaUpKWbQSgNwofxP/vgyy2zaa5YwX5/SP9O7VvMcpUsFw3UFoiUkYpfrN0l5P6cfbR48FdX8G1dySrxPXV//9YPFbQxbMd6UmL7w6NsDHPoT+2OhDy0Xc0m5TotQ1U01egElv3yMMWFYfD4SAbC0aWpp4wkeBG9CMzZyGiMAnxuMzhI4q8ljFFme+wo8LLlnnyBEX4gdnslX7mXv2lU8FhH9xCMJFZlbeTVIyNKNmCBOyKE4Kl4hqEGLlQtpP+gBzi9xFfFteQg8vHz0mVaNlxbJc1Hpcn8+2dPlnr1wSRrEt2ws1pcRolmrPnP968FCdnS7bjsnQoQ7xpGAq/0eu4OiTskVRJQW/ubFNII8gNdkEF488WdWz2yjIDQ1lsYc4uLpiyUAj7KiqYZ5NI2ZUJVFaJUNVzj6z6Ijjwd2AMf+/X4Mpf16BPkX1XzDIWZdvYlNtmxll4ph7eXW3Kon31JlwlzH2ZfIZjDP3mCpD0d3ToZKFX96dKqqkoXGFDrrmYPnuzgRUripJqpkVuVll03Rd98w5Jua5pGdNsh2sfO5H2IyLxJfdQc8hXIru6walGHm+8/obl/TT4aZz+AkYNT0m+LFczfVVg1WcZL6zbqZ+dP+s5ne+/DMC3oxatMrDuHS5rl6fpAG1s7r6B2zjizNhDRkkFECtUZxaaIVw05nGc0Up7yazs5QYOWUQisYirlDGOIK8eKopZhIe5NJytdaCCo/SX4NICqmljxsCm7kc8LmD6AqlPXFfl9ccxmOYItUfn4XHeOUHtL+aiWcYsAzMXzuCSrJkbLMWqzPncs9rtRQldL8wdrPOgmiBctj8VUeZyeKSERtxrwgxu3FKO12TDRoLCcvnh86Ipe0xBIhv3k=";

            _testOutputHelper.WriteLine($"uno=[{outOne}]");

            MTICrypto.AESDecrypt aESDecrypt = new MTICrypto.AESDecrypt(outOne, keyBase64);
            string plainOne = MTICrypto.Toolkit.GetStringFromCipherBytes(aESDecrypt.Decrypt());

            _testOutputHelper.WriteLine($"uno=[{plainOne}]");

            Assert.Contains("918\thoy", plainOne.ToLower());
        }
    }
}
