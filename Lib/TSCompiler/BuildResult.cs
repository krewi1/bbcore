﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BTDB.Collections;
using Lib.Utils;
using Njsast.SourceMap;

namespace Lib.TSCompiler
{
    public class ResolveResult
    {
        public string FileName;
        public Njsast.StructList<string> NegativeChecks;
        public int IterationId;
        public string FileNameJs;

        internal string FileNameWithPreference(bool preferDts)
        {
            if (preferDts) return FileName;
            return FileNameJs ?? FileName;
        }
    }

    public class BuildResult
    {
        readonly MainBuildResult _mainBuildResult;

        public BuildResult(MainBuildResult mainBuildResult, ProjectOptions options)
        {
            _mainBuildResult = mainBuildResult;
            BundleJsUrl = _mainBuildResult.AllocateName(options.GetDefaultBundleJsName(), options.Variant != "serviceworker");
        }

        public readonly Dictionary<(string From, string Name), ResolveResult> ResolveCache =
            new Dictionary<(string From, string Name), ResolveResult>();

        public readonly Dictionary<string, TsFileAdditionalInfo> Path2FileInfo = new Dictionary<string, TsFileAdditionalInfo>();
        public readonly HashSet<TsFileAdditionalInfo> RecompiledIncrementaly = new HashSet<TsFileAdditionalInfo>();
        public Njsast.StructList<TsFileAdditionalInfo> JavaScriptAssets;
        public readonly Dictionary<string, TSProject> Modules = new Dictionary<string, TSProject>();
        public bool HasError;
        public bool Incremental;
        public Task<List<Diagnostic>> TaskForSemanticCheck;
        public readonly string BundleJsUrl;
        public RefDictionary<string, BuildResult> SubBuildResults;

        public string ToOutputUrl(string fileName)
        {
            Path2FileInfo.TryGetValue(fileName, out var info);
            if (info == null)
            {
                Debug.Assert(false);
                return fileName;
            }

            if (info.OutputUrl == null)
                info.OutputUrl =
                    _mainBuildResult.AllocateName(PathUtils.Subtract(fileName, _mainBuildResult.CommonSourceDirectory));
            return info.OutputUrl;
        }


        internal string ToOutputUrl(TsFileAdditionalInfo source)
        {
            if (source.OutputUrl == null)
            {
                return ToOutputUrl(source.Owner.FullPath);
            }

            return source.OutputUrl;
        }
    }
}
