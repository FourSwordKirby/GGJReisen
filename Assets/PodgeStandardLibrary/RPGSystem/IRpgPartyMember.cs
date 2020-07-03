using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.PodgeStandardLibrary.RPGSystem
{
    public interface IRpgPartyMember
    {
        IRpgParty GetParty();
        //PartyMemberStats GetStatusInParty();

        string GetMemberName();
        string GetMemberTitle();
        int GetMemberTotalExp();
        int GetMemberLevel();
        int GetMaxHp();
        int GetHp();
    }

    public enum PartyMemberStatus
    {
        Active,
        Reserve,
        Absent
    }
}
