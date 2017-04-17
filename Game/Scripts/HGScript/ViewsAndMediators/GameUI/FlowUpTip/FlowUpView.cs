using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class FlowUpView:MonoBehaviour
{
    



    public Image image;
    public Text text;

    //public Sprite[] sprites;

    ResourceService resourceService;

    Animator animator;

    DGameDataCollection dGameDataCollection;


    public void Init(FlowUpTipSignal.Param param, IconSpritesService iconSpritesService,ResourceService resourceService,DGameDataCollection dGameDataCollection)
    {
        this.resourceService = resourceService;
        this.dGameDataCollection=dGameDataCollection;
        animator = GetComponent<Animator>();

        transform.SetParent(param.parent);
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;


        if (param.value > 0)
        {
            text.text = "+" + param.value.ToString();
        }
        else if (param.value < 0)
        {
            text.text = param.value.ToString(); //"-" + (-1 * param.value).ToString();
        }


        switch (param.type)
        {
            
            case FlowUpTipSignal.Type.blood:
                image.sprite = iconSpritesService.GetView().ball3;
                text.color = Color.red;
                break;
            case FlowUpTipSignal.Type.blood_max:
            case FlowUpTipSignal.Type.muscle:
            case FlowUpTipSignal.Type.fat:
            case FlowUpTipSignal.Type.inteligent:
            case FlowUpTipSignal.Type.amino_acid:
            case FlowUpTipSignal.Type.breath:
            case FlowUpTipSignal.Type.digest:
            case FlowUpTipSignal.Type.courage:
            case FlowUpTipSignal.Type.life:
                image.sprite = iconSpritesService.GetView().ball3;
                text.color = Color.blue;
                break;
            case FlowUpTipSignal.Type.skill:
                image.sprite = iconSpritesService.GetView().ball3;
                text.color = Color.blue;
                text.text = dGameDataCollection.dSkillCollection.dSkillDic[param.value].name;
                break;
            case FlowUpTipSignal.Type.cook_skill:
                image.sprite = iconSpritesService.GetView().ball3;
                text.color = Color.blue;
                text.text=dGameDataCollection.dCookSkillCollection.dCookSkillDic[param.value].name;
                break;

            case FlowUpTipSignal.Type.banana:
                image.sprite = iconSpritesService.GetView().ball1;
                text.color = Color.yellow;
                break;
            case FlowUpTipSignal.Type.meat:
                image.sprite = iconSpritesService.GetView().ball2;
                text.color = Color.magenta;
                break;
            case FlowUpTipSignal.Type.branch:
                image.sprite = iconSpritesService.GetView().ball3;
                text.color = Color.gray;
                break;
        }

        //animator.SetTrigger("flowup");

        //Debug.Log(iconSpritesService);
        
    }

    public void DestroyItself()
    {
        //Destroy(gameObject);
        
        resourceService.Despawn("flowuptip/FlowUpTip",gameObject);
    }

    public void OnDestroy()
    {

    }
}
