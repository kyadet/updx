//------------------------------------------------------------------------------
// <copyright file="updx.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2022 FurtherSystem Co.,Ltd. All rights reserved.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Generater - updater generator
// </summary>
//------------------------------------------------------------------------------
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using System.Collections.Generic;
using MiniJSON;

namespace Com.FurtherSystems.updx.admin
{
    public class Generator
    {
        // constants and definitions.
        private const int OK = 0;
        private const int NG = 1;
        private const string CodeType = "CSharp";
        private const string TemplateSuffix = ".cs.tmpl";
        private const string ReplacePrefix = "UPDX_STATIC_REPLACE_KEYWORD::";
        private enum UpdaterTemplate {
            typical,
            //offline|selfhosted|service
        }

        // variables.
        private static string Label = "[init]";

        // methods.
        private static void Log(params string[] logs)
        {
            var log = Label;
            foreach (var s in logs) log += s + " ";
            Console.WriteLine(log);
        }
        private static void Step(string str)
        {
            Label = "[" + str + "] ";
            Log();
        }
        private static void SafeQuit(params string[] logs)
        {
            Log(logs);
            Environment.Exit(OK);
        }
        private static void FailQuit(params string[] logs)
        {
            Log(logs);
            Environment.Exit(NG);
        }

        // main routine.
        public static void Main(string[] args)
        {
            Step("check arguments");

            var filename = args[0];
            var outpath = args[1];
            var respath = args[2]; 

            if (args.Length < 2) FailQuit("invalid arguments failed.");

            Step("load settings");

            var json = "";
            try
            {
                using (var reader = new StreamReader(filename, Encoding.UTF8))
                {
                    json = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                FailQuit("exception:", e.Message, e.StackTrace);
            }

            var settings = Json.Deserialize(json) as Dictionary<string,object>;
            var name = settings["name"];
            var rootEndpoint = settings["root_endpoint"];
            var mode = (string)settings["mode"];
            var failbackPolicy = settings["failback_policy"];
            var admin = (Dictionary<string,object>)settings["admin"];
            var adminRequireVersion = admin["require_version"];

            var adminSource = (Dictionary<string,object>)admin["source"];
            var adminSourceWindows = (Dictionary<string,object>)adminSource["windows"];
            var adminSourceWindowsDir = adminSourceWindows["directory"];
            var adminSourceWindowsIcon = adminSourceWindows["icon"];
            var adminSourceWindowsAssembly = adminSourceWindows["assembly"];
            var adminSourceWindowsArguments = adminSourceWindows["arguments"];
            var adminSourceWindowsUpdater = (string)adminSourceWindows["updater"];

            var adminDestination = (Dictionary<string,object>)admin["destination"];
            var adminDestinationUrl = adminDestination["url"];
            var adminDestinationType = adminDestination["type"];
            var adminDestinationAuth = adminDestination["auth"];
            var adminDestinationAuthUser = adminDestination["auth_user"];
            var adminDestinationAuthPass = adminDestination["auth_pass"];

            Step("load source template");

            var source = "";
            var sourceFilename = "";
            switch (mode) {
                case "typical":
                    sourceFilename = UpdaterTemplate.typical.ToString() + TemplateSuffix;
                    break;
                default:
                    Log("invalid mode:", mode);
                    return;
            }
            var sourceFilepath = Path.Combine(respath, sourceFilename);
            try
            {
                using (var reader = new StreamReader(sourceFilepath, Encoding.UTF8))
                {
                    source = reader.ReadToEnd();

                }
            }
            catch (Exception e)
            {
                FailQuit("exception:", e.Message, e.StackTrace);
            }

            Step("replace source template");

            // foreach(var keyPair in settings)
            // source = source.Replace(ReplacePrefix + "name", name);

            Step("compiling process");

            var provider = CodeDomProvider.CreateProvider(CodeType);
            var compiler = new CompilerParameters();
            compiler.GenerateExecutable = true;
            compiler.OutputAssembly = Path.Combine(outpath, adminSourceWindowsUpdater);
            compiler.GenerateInMemory = false;
            compiler.TreatWarningsAsErrors = true;
            compiler.CompilerOptions = "/optimize";
            compiler.ReferencedAssemblies.Add("System.dll");
            var compilerResults = provider.CompileAssemblyFromSource(compiler, source);
            if (compilerResults.Errors.Count > 0)
            {
                foreach (var ce in compilerResults.Errors)
                {
                    Log("error:", ce.ToString());
                }
                FailQuit("comple failed.");
            }

            SafeQuit("updater created.");
        }
    }
}