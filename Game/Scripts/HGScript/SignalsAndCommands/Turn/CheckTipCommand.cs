using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.command.impl;

public class CheckTipCommand:Command
{
    [Inject]
    public GameInfo gameInfo { get; set; }

    public override void Execute()
    {
        switch (gameInfo.gametype_id)
        {
            case 1:
                switch (gameInfo.progress_id)
                {
                    case 1:

                        break;
                }
                break;
            case 2:
                break;
        }
    }
}
