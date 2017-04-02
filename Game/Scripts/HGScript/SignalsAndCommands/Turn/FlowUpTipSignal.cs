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
        banana,
        meat,
        blood,
        blood_max,
        fat,
        inteligent,
        amino_acid,
        digest,
        skill,
        cook_skill
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
