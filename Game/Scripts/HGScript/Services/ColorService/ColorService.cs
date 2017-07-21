using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ColorService
{
    Color[] colors={Color.red,Color.blue,Color.yellow,Color.green,Color.cyan,Color.gray,Color.grey,Color.magenta};

    //public ColorService()
    //{
    //    colors.
    //}
    public Color getColor(int index)
    {
        return colors[index];
    }
}
