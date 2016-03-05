/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 14/April/2014
 * Date Moddified :- 25/November/2015
 * </Copyright>
 */

using System.Collections.Generic;

using Newtonsoft.Json;

namespace VaultTech.Network
{
    public static class Packet
    {
        public static string OutBuffer = "";
        public static string InBuffer = "";

        public static void SerializePacket(this List<object> Objects)
        {
            OutBuffer = "";
            OutBuffer = JsonConvert.SerializeObject(Objects);
        }

        public static void SerializePacket(this string _string)
        {
            OutBuffer = "";
            OutBuffer = JsonConvert.SerializeObject(_string);
        }

        public static List<object> DeserializePacket(this string RecivedPacket)
        {
            return JsonConvert.DeserializeObject<List<object>>(RecivedPacket);
        }
    }
}