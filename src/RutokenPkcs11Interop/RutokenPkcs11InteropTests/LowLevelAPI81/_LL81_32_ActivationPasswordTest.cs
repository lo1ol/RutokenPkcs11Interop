﻿using System;
using NUnit.Framework;
using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.LowLevelAPI81;
using RutokenPkcs11Interop.Common;
using RutokenPkcs11Interop.LowLevelAPI81;

namespace RutokenPkcs11InteropTests.LowLevelAPI81
{
    [TestFixture()]
    public class _LL81_32_ActivationPasswordTest
    {
        [Test()]
        public void _LL81_32_01_ActivationPasswordTest()
        {
            if (Platform.UnmanagedLongSize != 8 || Platform.StructPackingSize != 1)
                Assert.Inconclusive("Test cannot be executed on this platform");

            CKR rv = CKR.CKR_OK;

            using (var pkcs11 = new Pkcs11(Settings.Pkcs11LibraryPath))
            {
                rv = pkcs11.C_Initialize(Settings.InitArgs81);
                if ((rv != CKR.CKR_OK) && (rv != CKR.CKR_CRYPTOKI_ALREADY_INITIALIZED))
                    Assert.Fail(rv.ToString());

                // Установление соединения с Рутокен в первом доступном слоте
                ulong slotId = Helpers.GetUsableSlot(pkcs11);

                // Открытие RW сессии
                ulong session = CK.CK_INVALID_HANDLE;
                rv = pkcs11.C_OpenSession(slotId, (CKF.CKF_SERIAL_SESSION | CKF.CKF_RW_SESSION), IntPtr.Zero, IntPtr.Zero, ref session);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

                // Выполнение аутентификации администратора
                rv = pkcs11.C_Login(session, CKU.CKU_SO, Settings.SecurityOfficerPinArray,
                    Convert.ToUInt64(Settings.SecurityOfficerPinArray.Length));
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

                // TODO: сделать вызов функций активации в правильном порядке
                // и с правильными данными

                // Загрузка ключа активации
                byte[] activationKey = new byte[32];
                rv = pkcs11.C_EX_LoadActivationKey(session, activationKey);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

                // Получение длины пароля активации
                ulong passwordLength = 0;

                rv = pkcs11.C_EX_GenerateActivationPassword(session, (ulong)ActivationPasswordNumber.GenerateNextPassword,
                    null, ref passwordLength, (ulong)ActivationPasswordCharacterSet.CapsAndDigits);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

                Assert.IsTrue(passwordLength > 0);

                // Генерация пароля активации
                byte[] password = new byte[passwordLength];
                rv = pkcs11.C_EX_GenerateActivationPassword(session, (ulong)ActivationPasswordNumber.GenerateNextPassword,
                    password, ref passwordLength, (ulong)ActivationPasswordCharacterSet.CapsAndDigits);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

                // Установка пароля активации
                rv = pkcs11.C_EX_SetActivationPassword(slotId, password);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

                rv = pkcs11.C_Logout(session);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

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
