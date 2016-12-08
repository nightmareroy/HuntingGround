using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[Serializable]
public class DRole
{
    public int id;

    public string name;

    public List<int> original_skills;

}

[Serializable]
public class DRoleCollection
{
    public List<DRole> dRoleList = new List<DRole>();
    public Dictionary<int, DRole> dRoleDic = new Dictionary<int, DRole>();
}

