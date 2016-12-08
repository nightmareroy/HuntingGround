using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class LoginView : View {

    //Button singleBtn;
    //Button multiBtn;

    //total
    public Action onRegisterClickedDelegate;

    public Action onLoginClickedDelegate;

    public InputField loginAccount;
    public InputField loginPwd;
    public InputField loginYzm;

    public InputField registerAccount;
    public InputField registerPwd;
    public InputField registerPwd2;

    //public Button loadGameBtn;

    //public GameObject mainMenu;
    //
    public GameObject loginMenu;
    public GameObject registerMenu;
    public GameObject gameMenu;
    public GameObject newGameSettingMenu;
    //public GameObject serverSelectMenu;

    //public GameObject serverItemTpl;

    public enum PlayMode
    {
        single,
        multi
    }
    //public Signal<PlayMode> playModeSignal;

    //single
    public enum MapSize
    {
        small,
        big
    }
    //public Signal<MapSize> singleMapSizeSignal;
    //public Signal singleStartSignal;

    public Action<string> viewClick;
    

    //multi

    //public Button loginBtn;
    //public Button registerBtn;



	// Use this for initialization
    public void Init()
	{
        //base.Start();
        //singleBtn = this.transform.FindChild("Background/Menu/SinglePlay").GetComponent<Button>();
        //multiBtn = this.transform.FindChild("Background/Menu/MultiPlay").GetComponent<Button>();

        ////total
        //playModeSignal = new Signal<PlayMode>();

        ////single
        //singleMapSizeSignal = new Signal<MapSize>();
        //singleStartSignal = new Signal();

        //multi


        //singleBtn.onClick.AddListener(() =>
        //{
        //    playModelSelect.Dispatch(PlayMode.single);
        //});

        //multiBtn.onClick.AddListener(() =>
        //{
        //    playModelSelect.Dispatch(PlayMode.multi);
        //});

	}



    //public void onSingleClicked()
    //{
    //    //Debug.Log("onSingleClicked");
    //    //playModeSignal.Dispatch(PlayMode.single);
    //    if (viewClick != null)
    //    {
    //        viewClick("Single");
    //    }
    //}

    public void OnSmallClicked()
    {
        //Debug.Log("onSingleSmallClicked");
        //singleMapSizeSignal.Dispatch(MapSize.small);
        if (viewClick != null)
        {
            viewClick("Small");
        }
    }

    public void OnBigClicked()
    {
        //Debug.Log("onSingleBigClicked");
        //singleMapSizeSignal.Dispatch(MapSize.big);
        if (viewClick != null)
        {
            viewClick("Big");
        }
    }

    public void OnEnterStartNewGamePageClicked()
    {
        //singleStartSignal.Dispatch();
        if (viewClick != null)
        {
            viewClick("EnterStartNewGamePage");
        }
    }

    public void OnStartNewGameClicked()
    {
        //singleStartSignal.Dispatch();
        if (viewClick != null)
        {
            viewClick("StartNewGame");
        }
    }

    public void OnLoadGameClicked()
    {
        if (viewClick != null)
        {
            viewClick("LoadGame");
        }
    }

    public void OnFriendClicked()
    {
        if (viewClick != null)
        {
            viewClick("Friend");
        }
    }


    public void OnRegisterClicked()
    {
        //if (onRegisterClickedDelegate != null)
        //{
        //    onRegisterClickedDelegate();
        //}
        if (viewClick != null)
        {
            viewClick("Register");
        }
    }

    public void OnConfirmRegisterClicked()
    {
        if (viewClick != null)
        {
            viewClick("ConfirmRegister");
        }
    }

    public void OnLoginClicked()
    {
        //if (onLoginClickedDelegate != null)
        //{
        //    onLoginClickedDelegate();
        //}
        if (viewClick != null)
        {
            viewClick("Login");
        }
    }

    

    public void OnReturnClicked()
    {
        if (viewClick != null)
        {
            viewClick("Return");
        }
    }

    //public void SetLoadGameEnable(bool enable)
    //{
    //    loadGameBtn.enabled = enable;
    //}

    //public void enterGameMenu()
    //{
    //    loginMenu.SetActive(false);
    //    gameMenu.SetActive(true);
    //}
    public void SetLoadGameVisible(bool visible)
    {
        gameMenu.transform.FindChild("LoadBtn").GetComponent<Button>().interactable = visible;
    }

}
