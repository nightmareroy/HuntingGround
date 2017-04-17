using System;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using SimpleJson;

public class TopPanelView:View
{
    public Toggle toggle;

    public Text bananaValueText;
    public Text bananaModifyText;

    public Text meatValueText;
    public Text meatModifyText;

    public Text branchValueText;
    public Text branchModifyText;


    public Text averageBloodSugarText;
    public Text averageMuscleText;
    public Text averageFatText;
    public Text averageInteligentText;
    public Text averageAminoAcidText;
    public Text averageBreathText;
    public Text averageDigestText;
    public Text averageCourageText;
    public Text averageYoungerMaxText;
    public Text averageGrowupMaxText;

    public Text differenceBloodSugarText;
    public Text differenceMuscleText;
    public Text differenceFatText;
    public Text differenceInteligentText;
    public Text differenceAminoAcidText;
    public Text differenceBreathText;
    public Text differenceDigestText;
    public Text differenceCourageText;
    public Text differenceYoungerMaxText;
    public Text differenceGrowupMaxText;


    public void UpdateBanana(PlayerInfo playerInfo)
    {
        bananaValueText.text = playerInfo.banana.ToString();
        bananaModifyText.text = "("+playerInfo.bananaModify.ToString()+")";

        meatValueText.text = playerInfo.meat.ToString();
        meatModifyText.text = "(" + playerInfo.meatModify.ToString()+")";

        branchValueText.text = playerInfo.branch.ToString();
        branchModifyText.text = "(" + playerInfo.branchModify.ToString() + ")";

    }

    public void UpdateGroup(JsonObject groupJO)
    {
        JsonObject bloodSugarJO = groupJO["blood_sugar_max"] as JsonObject;
        JsonObject muscleJO = groupJO["muscle"] as JsonObject;
        JsonObject fatJO = groupJO["fat"] as JsonObject;
        JsonObject inteligentJO = groupJO["inteligent"] as JsonObject;
        JsonObject aminoAcidJO = groupJO["amino_acid"] as JsonObject;
        JsonObject breathJO = groupJO["breath"] as JsonObject;
        JsonObject digestJO = groupJO["digest"] as JsonObject;
        JsonObject courageJO = groupJO["courage"] as JsonObject;
        JsonObject youngerMaxJO = groupJO["younger_left_max"] as JsonObject;
        JsonObject growupMaxJO = groupJO["growup_left_max"] as JsonObject;

        //int averageBloodSugar = int.Parse(bloodSugarJO["quality"].ToString());
        //int differenceBloodSugar = int.Parse(bloodSugarJO["difference"].ToString());

        //int averageMuscle = int.Parse(muscleJO["quality"].ToString());
        //int differenceMuscle = int.Parse(muscleJO["difference"].ToString());

        //int averageFatSugar = int.Parse(fatJO["quality"].ToString());
        //int differenceFatSugar = int.Parse(fatJO["difference"].ToString());

        //int averageInteligent = int.Parse(inteligentJO["quality"].ToString());
        //int differenceInteligent = int.Parse(inteligentJO["difference"].ToString());

        //int averageAminoAcid = int.Parse(aminoAcidJO["quality"].ToString());
        //int differenceAminoAcid = int.Parse(aminoAcidJO["difference"].ToString());

        //int averageBreath = int.Parse(breathJO["quality"].ToString());
        //int differenceBreath = int.Parse(breathJO["difference"].ToString());

        //int averageDigest = int.Parse(digestJO["quality"].ToString());
        //int differenceDigest = int.Parse(digestJO["difference"].ToString());

        //int averageCourage = int.Parse(courageJO["quality"].ToString());
        //int differenceCourage = int.Parse(courageJO["difference"].ToString());

        //int averageYoungerMax = int.Parse(youngerMaxJO["quality"].ToString());
        //int differenceYoungerMax = int.Parse(youngerMaxJO["difference"].ToString());

        //int averageGrowupMax = int.Parse(growupMaxJO["quality"].ToString());
        //int differenceGrowupMax = int.Parse(growupMaxJO["difference"].ToString());

        averageBloodSugarText.text = bloodSugarJO["quality"].ToString();
        differenceBloodSugarText.text = bloodSugarJO["difference"].ToString();

        averageMuscleText.text = muscleJO["quality"].ToString();
        differenceMuscleText.text = muscleJO["difference"].ToString();

        averageFatText.text = fatJO["quality"].ToString();
        differenceFatText.text = fatJO["difference"].ToString();

        averageInteligentText.text = inteligentJO["quality"].ToString();
        differenceInteligentText.text = inteligentJO["difference"].ToString();

        averageAminoAcidText.text = aminoAcidJO["quality"].ToString();
        differenceAminoAcidText.text = aminoAcidJO["difference"].ToString();

        averageBreathText.text = breathJO["quality"].ToString();
        differenceBreathText.text = breathJO["difference"].ToString();

        averageDigestText.text = digestJO["quality"].ToString();
        differenceDigestText.text = digestJO["difference"].ToString();

        averageCourageText.text = courageJO["quality"].ToString();
        differenceCourageText.text = courageJO["difference"].ToString();

        int averageYoungerMax = int.Parse(youngerMaxJO["quality"].ToString());
        int differenceYoungerMax = int.Parse(youngerMaxJO["difference"].ToString());
        averageYoungerMaxText.text = ((float)averageYoungerMax/100f).ToString();
        differenceYoungerMaxText.text = ((float)differenceYoungerMax / 100f).ToString();

        int averageGrowupMax = int.Parse(growupMaxJO["quality"].ToString());
        int differenceGrowupMax = int.Parse(growupMaxJO["difference"].ToString());
        averageGrowupMaxText.text = ((float)averageGrowupMax / 100f).ToString();
        differenceGrowupMaxText.text = ((float)differenceGrowupMax / 100f).ToString();
    }
}

