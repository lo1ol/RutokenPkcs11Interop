﻿using System;
using System.Runtime.InteropServices;
using Net.Pkcs11Interop.Common;
using RutokenPkcs11Interop.LowLevelAPI41;
using HLA41 = Net.Pkcs11Interop.HighLevelAPI41;

namespace RutokenPkcs11Interop.HighLevelAPI41
{
    public static class SlotExtensions
    {
        public static TokenInfoExtended GetTokenInfoExtended(this HLA41.Slot slot)
        {
            var tokenInfo = new CK_TOKEN_INFO_EXTENDED
            {
                SizeofThisStructure = Convert.ToUInt32(Marshal.SizeOf(typeof(CK_TOKEN_INFO_EXTENDED)))
            };

            CKR rv = slot.LowLevelPkcs11.C_EX_GetTokenInfoExtended(slot.SlotId, ref tokenInfo);
            if (rv != CKR.CKR_OK)
                throw new Pkcs11Exception("C_EX_GetTokenInfoExtended", rv);

            return new TokenInfoExtended(tokenInfo);
        }

        public static void InitTokenExtended(this HLA41.Slot slot, string pin, RutokenInitParam initParam)
        {
            if (pin == null)
                throw new ArgumentNullException("pin");

            if (initParam == null)
                throw new ArgumentNullException("initParam");

            CK_RUTOKEN_INIT_PARAM ckInitParam = initParam.CkRutokenInitParam;

            byte[] pinArray = ConvertUtils.Utf8StringToBytes(pin);
            CKR rv = slot.LowLevelPkcs11.C_EX_InitToken(slot.SlotId, pinArray, (uint)pin.Length, ref ckInitParam);
            if (rv != CKR.CKR_OK)
                throw new Pkcs11Exception("C_EX_InitToken", rv);
        }

        public static byte[] GetJournal(this HLA41.Slot slot)
        {
            uint journalLength = 0;
            CKR rv = slot.LowLevelPkcs11.C_EX_GetJournal(slot.SlotId, null, ref journalLength);
            if (rv != CKR.CKR_OK)
                throw new Pkcs11Exception("C_EX_GetJournal", rv);

            byte[] journal = new byte[journalLength];

            rv = slot.LowLevelPkcs11.C_EX_GetJournal(slot.SlotId, journal, ref journalLength);
            if (rv != CKR.CKR_OK)
                throw new Pkcs11Exception("C_EX_GetJournal", rv);

            return journal;
        }
    }
}
