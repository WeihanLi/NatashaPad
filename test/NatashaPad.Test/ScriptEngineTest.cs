// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Microsoft.Extensions.DependencyInjection;
using ReferenceResolver;
using WeihanLi.Extensions;

namespace NatashaPad.Test;

public class ScriptEngineTest : IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly INScriptEngine _scriptEngine;
    private readonly ServiceProvider _serviceProvider;

    public ScriptEngineTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _serviceProvider = new ServiceCollection()
            .AddReferenceResolvers()
            .BuildServiceProvider();
        _scriptEngine = new CSharpScriptEngine(new ReferenceResolverFactory(_serviceProvider));
    }

    [Fact]
    public async Task ExecuteTest()
    {
        DumpOutHelper.OutputAction += Output;
        try
        {
            await _scriptEngine.Execute("\"Hello NatashaPad\".Dump();", new NScriptOptions(), TestContext.Current.CancellationToken);

            await _scriptEngine.Execute("Console.WriteLine(\"Hello NatashaPad\");", new NScriptOptions(), TestContext.Current.CancellationToken);
        }
        catch (NatashaException ex)
        {
            _testOutputHelper.WriteLine(ex.Diagnostics.Select(d => d.ToString())
                .StringJoin(Environment.NewLine));
            throw;
        }
        finally
        {
            DumpOutHelper.OutputAction -= Output;
        }
    }

    [Fact]
    public async Task ExecuteTestWithReference()
    {
        DumpOutHelper.OutputAction += Output;
        try
        {
            var options = new NScriptOptions();
            options.References.Add(new NuGetReference("WeihanLi.Npoi", "3.2.0"));
            options.UsingList.Add("WeihanLi.Npoi");
            await _scriptEngine.Execute("CsvHelper.GetCsvText(new[]{1,2,3}).Dump();", options, TestContext.Current.CancellationToken);
        }
        catch (NatashaException ex)
        {
            _testOutputHelper.WriteLine(ex.Diagnostics.Select(d => d.ToString()).StringJoin(Environment.NewLine));
            throw;
        }
        finally
        {
            DumpOutHelper.OutputAction -= Output;
        }
    }

    [Fact]
    public async Task EvalTest()
    {
        var result = await _scriptEngine.Eval("1+1", new NScriptOptions(), TestContext.Current.CancellationToken);
        Assert.Equal(2, result);

        result = await _scriptEngine.Eval("\"Hello \" + \"NatashaPad\"", new NScriptOptions(), TestContext.Current.CancellationToken);
        Assert.Equal("Hello NatashaPad", result);

        result = await _scriptEngine.Eval("DateTime.Today.ToString(\"yyyyMMdd\")", new NScriptOptions(), TestContext.Current.CancellationToken);
        Assert.Equal(DateTime.Today.ToString("yyyyMMdd"), result);
    }

    [Fact]
    public async Task EvalTestWithReference()
    {
        var options = new NScriptOptions();
        options.References.Add(new NuGetReference("WeihanLi.Npoi", "3.2.0"));
        options.UsingList.Add("WeihanLi.Npoi");
        var result = await _scriptEngine.Eval("CsvHelper.GetCsvText(Enumerable.Range(1, 3))", options, TestContext.Current.CancellationToken);
        Assert.NotNull(result);
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }
    
    private void Output(string msg)
    {
        _testOutputHelper.WriteLine(msg);
    }

}
