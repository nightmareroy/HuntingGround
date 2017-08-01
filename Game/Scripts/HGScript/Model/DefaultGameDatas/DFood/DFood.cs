using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJson;

public class DFood
{
    public int food_id;
    public string name;
    public List<int> cook_skills_need;
    public int meat;
    public int banana;
    public int ant;
    public int egg;
    public int honey;
    public int protein;
    public int minerals;
    public int fat;
    public int carbohydrate;
    public int dietary_fiber;
    public int vitamin;
    public int inspire_skill_type;
    public int inspire_skill_id;
    public List<int> inspire_skill_properties;
    public List<int> inspire_skill_values;
}

public class DFoodCollection
{
    public Dictionary<int, DFood> dFoodDic = new Dictionary<int, DFood>();

    public void InitFromStr(string jsstr)
    {

        JsonObject jsobj = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsstr);
        dFoodDic.Clear();
        foreach (JsonObject jo in jsobj.Values)
        {

            DFood dFoodValue = SimpleJson.SimpleJson.DeserializeObject<DFood>(jo.ToString());
            dFoodDic.Add(dFoodValue.food_id, dFoodValue);
        }
    }
}
