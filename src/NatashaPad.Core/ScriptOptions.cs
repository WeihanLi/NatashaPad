﻿// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

namespace NatashaPad;

public class NScriptOptions
{
    public HashSet<string> UsingList { get; } = new()
    {
        "System",
        "System.Collections.Generic",
        "System.Diagnostics",
        "System.Linq",
        "System.Linq.Expressions",
        "System.IO",
        "System.Reflection",
        "System.Text",
        "System.Text.RegularExpressions",
        "System.Threading",
        "System.Threading.Tasks",
    };

    public HashSet<IReference> References { get; } = new();
}
