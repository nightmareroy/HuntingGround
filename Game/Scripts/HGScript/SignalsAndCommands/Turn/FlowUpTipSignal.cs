using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using strange.extensions.signal.impl;

public class FlowUpTipSignal:Signal<FlowUpTipSignal.Param>
{

    public enum Type
    {
        common,

        blood,
        blood_max,
        muscle,
        fat,
        inteligent,
        amino_acid,
        breath,
        digest,
        courage,
        life,
        skill,
        cook_skill,
        meat,
        banana,
        ant,
        egg,
        honey
    }

    public class Param
    {
        MainContext mainContext;

        public Transform parent;
        public Type type;
        public int value;

        public string content;

        public Param(Transform parent,Type type,int value)
        {
            this.parent = parent;
            this.type = type;
            this.value = value;
        }

        public Param( string content)
        {
            this.parent = null;
            this.type = Type.common;
            this.value = 0;
            this.content = content;
        }
        
    }
}
