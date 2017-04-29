using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

public class FoodPanelView:View
{
    [Inject]
    public DGameDataCollection dGameDataCollection { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public SPlayerInfo sPlayerInfo { get; set; }

    public GameObject rootPanel;

    public Transform topPanelRootT;
    public Transform topPanel2RootT;
    public Transform leftPanelRootT;
    public Transform foodPanelRootT;

    public Transform topToggleTplT;
    public Transform leftToggleTplT;
    public Transform foodToggleTplT;

    string role_id;

    List<int> current_selected_cook_skill_needed_list;
    int current_selected_skill_type;
    int current_selected_food_id;
    List<int> actived_food_ids;

    public Button confirmBtn;


    public Action<string, int> onConfirm;

    public void Init()
    {
        Debug.Log(this.GetHashCode());
    }

    public void Init(string role_id, List<int> cook_skill_id_list, List<int> actived_food_ids)
    {
        this.role_id = role_id;
        this.actived_food_ids = actived_food_ids;
        current_selected_cook_skill_needed_list = new List<int>();
        current_selected_skill_type = -1;

        
        InitTopPanel(cook_skill_id_list);
        InitLeftPanel();
        UpdateContentPanel();

        rootPanel.SetActive(true);

    }

    public void Confirm()
    {
        if (onConfirm != null)
            onConfirm(role_id,current_selected_food_id);
        rootPanel.SetActive(false);
    }


    void InitTopPanel(List<int> cook_skill_id_list)
    {
        //Debug.Log(gameObject);
        Tools.ClearChildren(topPanelRootT);

        foreach (int j in cook_skill_id_list)
        {
            int i = j;
            GameObject item = GameObject.Instantiate<GameObject>(topToggleTplT.gameObject);
            item.transform.SetParent(topPanelRootT);
            item.SetActive(true);
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.identity;

            item.transform.FindChild("Label").GetComponent<Text>().text = dGameDataCollection.dCookSkillCollection.dCookSkillDic[i].name;

            item.name = i.ToString();

            item.GetComponent<Toggle>().onValueChanged.AddListener((bool value) => {
                if (value)
                {
                    current_selected_cook_skill_needed_list.Add(i);

                }
                else
                {
                    current_selected_cook_skill_needed_list.Remove(i);
                }

                UpdateContentPanel();
            });
        }


        
    }

    void InitLeftPanel()
    {
        Tools.ClearChildren(topPanel2RootT);


        for (int j = -1; j < 3; j++)
        {
            int i = j;
            GameObject item = GameObject.Instantiate<GameObject>(leftToggleTplT.gameObject);
            item.transform.SetParent(topPanel2RootT);
            item.SetActive(true);
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.identity;

            Text text = item.transform.FindChild("Label").GetComponent<Text>();
            Toggle toggle=item.GetComponent<Toggle>();
            switch (i)
            {
                case -1:
                    text.text = "全部";
                    toggle.isOn = true;
                    break;
                case 0:
                    text.text = "无";
                    toggle.isOn = false;
                    break;
                case 1:
                    text.text = "战斗\n技能";
                    toggle.isOn = false;
                    break;
                case 2:
                    text.text = "料理\n技能";
                    toggle.isOn = false;
                    break;
            }

            item.name = i.ToString();

            item.GetComponent<Toggle>().onValueChanged.AddListener((bool value) =>
            {
                if (value)
                {
                    current_selected_skill_type=i;
                    UpdateContentPanel();
                }
            });

        }


    }

    void UpdateContentPanel()
    {
        Tools.ClearChildren(foodPanelRootT);

        
        foreach (int food_id in actived_food_ids)
        {
            DFood dFood = dGameDataCollection.dFoodCollection.dFoodDic[food_id];
            if (dFood.inspire_skill_type == current_selected_skill_type || current_selected_skill_type==-1)
            {


                bool have;
                if (current_selected_cook_skill_needed_list.Count == 0)
                {
                    have = true;
                }
                
                else if (current_selected_cook_skill_needed_list.Count == dFood.cook_skills_need.Count)
                {
                    have = true;
                    foreach (int needed_cook_skill_id in dFood.cook_skills_need)
                    {
                        if (!current_selected_cook_skill_needed_list.Contains(needed_cook_skill_id))
                        {
                            have = false;
                            break;
                        }
                    }
                }
                else
                {
                    have = false;
                }

                if (have)
                {
                    //
                    GameObject item = GameObject.Instantiate<GameObject>(foodToggleTplT.gameObject);
                    item.transform.SetParent(foodPanelRootT);
                    item.SetActive(true);
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localScale = Vector3.one;
                    item.transform.localRotation = Quaternion.identity;
                    
                    item.transform.FindChild("Foreground/Name").GetComponent<Text>().text = dFood.name;

                    item.name = dFood.food_id.ToString();

                    item.GetComponent<Toggle>().onValueChanged.AddListener((bool value) =>
                    {
                        if (value)
                        {
                            current_selected_food_id = dFood.food_id;
                            confirmBtn.interactable = true;
                        }
                    });

                    item.transform.FindChild("Foreground/Nutritions/carbohydrate/value").GetComponent<Text>().text = dFood.carbohydrate.ToString();
                    item.transform.FindChild("Foreground/Nutritions/fat/value").GetComponent<Text>().text = dFood.fat.ToString();
                    item.transform.FindChild("Foreground/Nutritions/protein/value").GetComponent<Text>().text = dFood.protein.ToString();
                    item.transform.FindChild("Foreground/Nutritions/minerals/value").GetComponent<Text>().text = dFood.minerals.ToString();
                    item.transform.FindChild("Foreground/Nutritions/dietary_fiber/value").GetComponent<Text>().text = dFood.dietary_fiber.ToString();
                    item.transform.FindChild("Foreground/Nutritions/vitamin/value").GetComponent<Text>().text = dFood.vitamin.ToString();
                    switch(dFood.inspire_skill_type)
                    {
                        case 0:
                            item.transform.FindChild("Foreground/Skill/skill").gameObject.SetActive(false);
                            break;
                        case 1:
                            item.transform.FindChild("Foreground/Skill/skill").gameObject.SetActive(true);
                            item.transform.FindChild("Foreground/Skill/skill/key").GetComponent<Text>().text = "战斗技能";
                            item.transform.FindChild("Foreground/Skill/skill/value").GetComponent<Text>().text = dGameDataCollection.dSkillCollection.dSkillDic[dFood.inspire_skill_id].name;
                            break;
                        case 2:
                            item.transform.FindChild("Foreground/Skill/skill").gameObject.SetActive(true);
                            item.transform.FindChild("Foreground/Skill/skill/key").GetComponent<Text>().text = "料理方法";
                            item.transform.FindChild("Foreground/Skill/skill/value").GetComponent<Text>().text = dGameDataCollection.dCookSkillCollection.dCookSkillDic[dFood.inspire_skill_id].name;
                            break;
                    }


                    item.transform.FindChild("Foreground/Need/banana/value").GetComponent<Text>().text = dFood.banana.ToString();
                    item.transform.FindChild("Foreground/Need/meat/value").GetComponent<Text>().text = dFood.meat.ToString();
                    //if (dFood.cook_skills_need.Count == 0)
                    //{
                    //    item.transform.FindChild("Foreground/Need/cook_skill").gameObject.SetActive(false);
                    //}
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < dFood.cook_skills_need.Count;i++ )
                    {
                        sb.Append(dGameDataCollection.dCookSkillCollection.dCookSkillDic[dFood.cook_skills_need[i]].name);
                        if (i < dFood.cook_skills_need.Count - 1)
                        {
                            sb.Append(" ");
                        }
                    }
                    item.transform.FindChild("Foreground/Need/cook_skill/value").GetComponent<Text>().text = sb.ToString();



                    if (gameInfo.allplayers_dic[sPlayerInfo.uid].banana < dFood.banana || gameInfo.allplayers_dic[sPlayerInfo.uid].meat < dFood.meat)
                    {
                        item.GetComponent<Toggle>().interactable = false;
                    }
                }

            }
        }
    }

    void OnDestroy()
    {

    }

}
