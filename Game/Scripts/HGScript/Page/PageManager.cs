using System;
using System.Collections.Generic;
using UnityEngine;

public class PageManager
{
    public Page activePage;
    public PageManager(Page activePage)
    {
        this.activePage = activePage;
        activePage.SetActive(true);
    }

    public bool JumpDown(Page childPage,Action initFunc=null)
    {
        if (!activePage.childList.Contains(childPage))
        {
            return false;
        }
        else
        {
            if (initFunc != null)
            {
                initFunc();
            }

            activePage.SetActive(false);
            childPage.SetActive(true);
            activePage = childPage;
            return true;
        }
    }
    public bool JumpUp(Action initFunc=null)
    {
        if (activePage.parent == null)
        {
            return false;
        }
        else
        {
            if (initFunc != null)
            {
                initFunc();
            }

            activePage.SetActive(false);
            activePage.parent.SetActive(true);
            activePage = activePage.parent;
            return true;
        }
    }

    public bool JumpTo(Page page,Action initFunc=null)
    {
        if (initFunc != null)
        {
            initFunc();
        }
        activePage.SetActive(false);
        page.SetActive(true);
        activePage = page;
        return true;
    }
}

