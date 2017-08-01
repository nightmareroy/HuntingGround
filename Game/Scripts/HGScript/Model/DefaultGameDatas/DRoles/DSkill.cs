using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJson;


public class DSkill
{
    public int skill_id;
    public string name;
    public List<string> keys;
    public List<int> values;
    public string desc;

//    public DSkill(int skillid,string name,string property,int param,string desc)
//    {
//        this.skill_did = skillid;
//        this.name = name;
//        this.property = property;
//        this.param = param;
//        this.desc = desc;
//    }
}

//[Serializable]
public class DSkillCollection
{
//    public List<DSkill> dSkillList = new List<DSkill>();
    public Dictionary<int, DSkill> dSkillDic = new Dictionary<int, DSkill>();

    public void InitFromStr(string jsstr)
    {
        JsonObject jsobj = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsstr);
        dSkillDic.Clear();
        foreach (JsonObject jo in jsobj.Values)
        {
//            int skillid = int.Parse(jo["skillid"].ToString());
//            string name = jo["name"].ToString();
//            string property = jo["property"].ToString();
//            int param=int.Parse(jo["param"].ToString());
//            string desc = jo["desc"].ToString();



            DSkill dSkill = SimpleJson.SimpleJson.DeserializeObject<DSkill>(jo.ToString());
                //new DSkill(skillid,name,property,param,desc);
            dSkillDic.Add(dSkill.skill_id,dSkill);
        }
    }
}