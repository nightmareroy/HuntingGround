using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class DSkill
{
    public int id;
    public string name;
    public int directionid;
    public string desc;
}

[Serializable]
public class DSkillCollection
{
    public List<DSkill> dSkillList = new List<DSkill>();
    public Dictionary<int, DSkill> dSkillDic = new Dictionary<int, DSkill>();
}