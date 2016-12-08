using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;

public class AddCountySkillSignal:Signal<AddCountySkillSignal.Param>
{
    public class Param
    {
        public int player_id;
        public int role_did;
        public int skill_id;

        public Param(int player_id, int role_did, int skill_id)
        {
            this.player_id = player_id;
            this.role_did = role_did;
            this.skill_id = skill_id;
        }
    }
}

