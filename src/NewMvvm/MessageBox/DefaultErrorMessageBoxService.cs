﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NewMvvm.MessageBox
{
    internal class DefaultErrorMessageBoxService : IErrorMessageBoxService
    {
        public void ShowError(string title, string content)
        {
            System.Windows.MessageBox.Show(content, title);
        }
    }
}
