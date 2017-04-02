using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJson;

public class DCookSkill
{
    public int cook_skill_id;
    //public int inteligent_need;
    public string name;
    //public int activity_need;
    //public List<int> method_need_list;
}

public class DCookSkillCollection
{
    public Dictionary<int, DCookSkill> dCookSkillDic = new Dictionary<int, DCookSkill>();

    public void InitFromStr(string jsstr)
    {

        JsonObject jsobj = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsstr);
        dCookSkillDic.Clear();
        foreach (JsonObject jo in jsobj.Values)
        {

            DCookSkill dCookSkillValue = SimpleJson.SimpleJson.DeserializeObject<DCookSkill>(jo.ToString());
            dCookSkillDic.Add(dCookSkillValue.cook_skill_id, dCookSkillValue);
        }
    }
}