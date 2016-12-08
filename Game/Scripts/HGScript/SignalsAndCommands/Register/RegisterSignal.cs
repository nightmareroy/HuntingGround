using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;

public class RegisterSignal:Signal<RegisterSignal.Param>
{
    public class Param
    {
        public string account;
        public string pwd;
        public Param(string account, string pwd)
        {
            this.account = account;
            this.pwd = pwd;
        }
    }
}

