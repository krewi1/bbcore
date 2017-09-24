﻿using System.Collections.Concurrent;
using Lib.ToolsDir;
using System;
using Lib.TSCompiler;
using Lib.CSSProcessor;

namespace Lib.Composition
{
    public class CompilerPool : ICompilerPool
    {
        public CompilerPool(IToolsDir toolsDir, int parallelCompilations = 1)
        {
            _toolsDir = toolsDir;
            _semaphore = new System.Threading.SemaphoreSlim(parallelCompilations);
            _semaphoreCss = new System.Threading.SemaphoreSlim(parallelCompilations);
        }

        System.Threading.SemaphoreSlim _semaphore;
        System.Threading.SemaphoreSlim _semaphoreCss;
        ConcurrentBag<ITSCompiler> pool = new ConcurrentBag<ITSCompiler>();
        ConcurrentBag<ICssProcessor> poolCss = new ConcurrentBag<ICssProcessor>();

        readonly IToolsDir _toolsDir;

        public ITSCompiler GetTs()
        {
            _semaphore.Wait();
            if (pool.TryTake(out var res))
                return res;
            return new TsCompiler(_toolsDir);
        }

        public void ReleaseTs(ITSCompiler value)
        {
            pool.Add(value);
            _semaphore.Release();
        }

        public ICssProcessor GetCss()
        {
            _semaphoreCss.Wait();
            if (poolCss.TryTake(out var res))
                return res;
            return new CssProcessor(_toolsDir);
        }

        public void ReleaseCss(ICssProcessor value)
        {
            poolCss.Add(value);
            _semaphoreCss.Release();
        }
    }
}
