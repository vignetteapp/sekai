// Copyright (c) The Vignette Authors
// Licensed under MIT. See LICENSE for details.

using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sekai.Framework.Entities;

namespace Sekai.Framework.Testing;

public class TestScene : Component
{
    private HeadlessHost? host;
    private TestSceneGame? game;
    private Task? runTask;

    [OneTimeSetUp]
    public void OneTimeSetUpFromRunner()
    {
        if (!TestUtils.IsNUnit)
            return;

        host = new HeadlessHost();
        game = new TestSceneGame();
        game.Services.Cache(this);

        runTask = Task.Factory.StartNew(() => host.Run(game), TaskCreationOptions.LongRunning);

        while (!IsLoaded)
        {
            checkForErrors();
            Thread.Sleep(10);
        }
    }

    [TearDown]
    public void TearDownFromRunner()
    {
        if (!TestUtils.IsNUnit)
            return;

        checkForErrors();
    }

    [OneTimeTearDown]
    public void OneTimeTearDownFromRunner()
    {
        if (!TestUtils.IsNUnit)
            return;

        try
        {
            game?.Dispose();
            host?.Dispose();
        }
        catch
        {
        }
    }

    private void checkForErrors()
    {
        if (runTask?.Exception != null)
            throw runTask.Exception;
    }
}
