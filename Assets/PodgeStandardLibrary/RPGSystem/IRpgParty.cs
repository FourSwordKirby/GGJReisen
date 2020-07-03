using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.PodgeStandardLibrary.RPGSystem
{
    public interface IRpgParty
    {
        List<IRpgPartyMember> GetPartyMembers();
        List<IRpgPartyMember> GetActivePartyMembers();
        List<IRpgPartyMember> GetReservePartyMembers();
        List<IRpgPartyMember> GetAbsentPartyMembers();

        Inventory GetBattleInventory();
        Inventory GetInventory();
    }
}
