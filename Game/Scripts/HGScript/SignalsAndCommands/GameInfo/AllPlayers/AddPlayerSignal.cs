using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;


public class AddPlayerSignal : Signal<AddPlayerSignal.Param>
{
    public class Param
    {
        public PlayerInfo playerInfo;// = new GamePlayerInfo();

        public Param(PlayerInfo playerInfo)
        {
            this.playerInfo = playerInfo;
        }

        //public Action dataCallback;
    }
}

