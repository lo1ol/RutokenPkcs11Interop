﻿using System;
using NUnit.Framework;
using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.LowLevelAPI41;
using RutokenPkcs11Interop.Common;
using RutokenPkcs11Interop.LowLevelAPI41;

namespace RutokenPkcs11InteropTests.LowLevelAPI41
{
    [TestFixture()]
    public class _LL41_34_ManageEntityTest
    {
        [Test()]
        public void _LL41_34_01_ManageSlotTest()
        {
            if (Platform.UnmanagedLongSize != 4 || Platform.StructPackingSize != 1)
                Assert.Inconclusive("Test cannot be executed on this platform");

            CKR rv = CKR.CKR_OK;

            using (var pkcs11 = new Pkcs11(Settings.Pkcs11LibraryPath))
            {
                // Инициализация библиотеки
                rv = pkcs11.C_Initialize(Settings.InitArgs41);
                if ((rv != CKR.CKR_OK) && (rv != CKR.CKR_CRYPTOKI_ALREADY_INITIALIZED))
                    Assert.Fail(rv.ToString());

                // Установление соединения с Рутокен в первом доступном слоте
                uint slotId = Helpers.GetUsableSlot(pkcs11);

                // TODO: актуализировать тест с реальными значениями

                rv = pkcs11.C_EX_SlotManage(slotId, 0, IntPtr.Zero);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

                // Завершение сессии
                rv = pkcs11.C_Finalize(IntPtr.Zero);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());
            }
        }

        [Test()]
        public void _LL41_34_02_ManageTokenTest()
        {
            if (Platform.UnmanagedLongSize != 4 || Platform.StructPackingSize != 1)
                Assert.Inconclusive("Test cannot be executed on this platform");

            CKR rv = CKR.CKR_OK;

            using (var pkcs11 = new Pkcs11(Settings.Pkcs11LibraryPath))
            {
                // Инициализация библиотеки
                rv = pkcs11.C_Initialize(Settings.InitArgs41);
                if ((rv != CKR.CKR_OK) && (rv != CKR.CKR_CRYPTOKI_ALREADY_INITIALIZED))
                    Assert.Fail(rv.ToString());

                // Установление соединения с Рутокен в первом доступном слоте
                uint slotId = Helpers.GetUsableSlot(pkcs11);

                // Открытие RW сессии
                uint session = CK.CK_INVALID_HANDLE;
                rv = pkcs11.C_OpenSession(slotId, (CKF.CKF_SERIAL_SESSION | CKF.CKF_RW_SESSION), IntPtr.Zero,
                    IntPtr.Zero, ref session);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

                // TODO: актуализировать тест с реальными значениями

                rv = pkcs11.C_EX_TokenManage(session, (uint)TokenManageMode.ChannelTypeBluetooth, IntPtr.Zero);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

                // Завершение сессии
                rv = pkcs11.C_CloseSession(session);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

                rv = pkcs11.C_Finalize(IntPtr.Zero);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());
            }
        }
    }
}
