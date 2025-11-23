// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

namespace NatashaPad.Mvvm.MessageBox;

public interface IErrorMessageBoxService
{
    void ShowError(string title, string content);
}
