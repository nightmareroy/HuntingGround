using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class IconSpritesService
{
    [Inject]
    public ResourceService resourceService { get; set; }


    IconSpritesView view;


    public IconSpritesView GetView()
    {
        if (view == null)
        {
            view=resourceService.Spawn("iconsprites/IconSprites").GetComponent<IconSpritesView>();
        }
        return view;
    }
}
