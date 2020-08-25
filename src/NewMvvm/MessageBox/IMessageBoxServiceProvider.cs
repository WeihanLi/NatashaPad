﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NewMvvm.MessageBox
{
    public interface IInfoMessageBoxService
    {
        void ShowInfo(string title, string content);
    }

    public interface IWarnMessageBoxService
    {
        void ShowWarn(string title, string content);
    }

    public interface IErrorMessageBoxService
    {
        void ShowError(string title, string content);
    }

    public enum AskResponse
    {
        Option1,
        Option2,
        Option3,
        Option4,
        Option5
    }

    public interface IAskMessageBoxService
    {
        AskResponse ShowAsk(string title, string content);
    }

    public interface IConfirmMessageBoxService
    {
        bool ShowConfirm(string title, string content);
    }
}
