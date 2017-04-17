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
        banana,
        meat,
        branch
    }

    public class Param
    {
        public Transform parent;
        public Type type;
        public int value;

        public Param(Transform parent,Type type,int value)
        {
            this.parent = parent;
            this.type = type;
            this.value = value;
        }
        
    }
}
