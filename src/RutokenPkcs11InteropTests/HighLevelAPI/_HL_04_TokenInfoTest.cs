﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Pkcs11Interop.HighLevelAPI;
using RutokenPkcs11Interop;
using RutokenPkcs11Interop.HighLevelAPI;

namespace RutokenPkcs11InteropTests.HighLevelAPI
{
    [TestClass]
    public class _HL_04_TokenInfoTest
    {
        /// <summary>
        /// Basic C_GetTokenInfo test.
        /// </summary>
        [TestMethod()]
        public void _HL_04_01_TokenInfoTest()
        {
            using (var pkcs11 = new Pkcs11(Settings.Pkcs11LibraryPath, Settings.UseOsLocking))
            {
                // Find first slot with token present
                Slot slot = Helpers.GetUsableSlot(pkcs11);

                // Get token info
                TokenInfo tokenInfo = slot.GetTokenInfo();

                // Do something interesting with token info
                Assert.IsFalse(String.IsNullOrEmpty(tokenInfo.ManufacturerId));
            }
        }

        /// <summary>
        /// C_EX_GetTokenInfoExtended test.
        /// </summary>
        [TestMethod()]
        public void _HL_04_02_TokenInfoExtendedTest()
        {
            using (var pkcs11 = new Pkcs11(Settings.Pkcs11LibraryPath, Settings.UseOsLocking))
            {
                // Find first slot with token present
                Slot slot = Helpers.GetUsableSlot(pkcs11);

                // Get token info
                TokenInfoExtended tokenInfo = slot.GetTokenInfoExtended();

                // Do something interesting with token info
                Assert.IsFalse(String.IsNullOrEmpty(tokenInfo.SerialNumber));
            }
        }
    }
}