﻿using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Vagrant.Settings;

namespace Cake.Vagrant
{
    public class VagrantRunner : Tool<VagrantSettings>
    {
        private VagrantSettings Settings { get; set; } = new VagrantSettings();
        public VagrantBoxRunner Box { get; set; }
        public VagrantRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner,
            IToolLocator tools, ICakeLog log) : base(fileSystem, environment, processRunner, tools)
        {
            Box = new VagrantBoxRunner(log, Run, Settings);
        }

        protected override string GetToolName() => "Vagrant by Hashicorp";

        protected override IEnumerable<FilePath> GetAlternativeToolPaths(VagrantSettings settings)
        {
            yield return "C:\\HashiCorp\\Vagrant\\bin\\vagrant.exe";
        }

        protected override IEnumerable<string> GetToolExecutableNames()
        {
            yield return "vagrant.exe";
        }

        public void Init(string name, string url = null)
        {
            Init(name, null, null);
        }

        public void Init(string name, Action<VagrantInitSettings> configure)
        {
            Init(name, null, configure);
        }

        public void Init(string name, string url, Action<VagrantInitSettings> configure)
        {
            var settings = new VagrantInitSettings();
            configure?.Invoke(settings);
            var args = new ProcessArgumentBuilder();
            args.Append("init");
            settings.GetToolArguments().Invoke(args);
            args.Append(name);
            if (url.HasValue()) args.Append(url);
            Run(new VagrantSettings(), args);
        }

        public void Up(string name = null)
        {
            Up((Action<VagrantUpSettings>) null);
        }

        public void Up(Action<VagrantUpSettings> configure)
        {
            Up(null, configure);
        }

        public void Up(string name, Action<VagrantUpSettings> configure)
        {
            var settings = new VagrantUpSettings();
            configure?.Invoke(settings);
            var args = new ProcessArgumentBuilder();
            args.Append("up");
            if (name.HasValue()) args.Append(name);
            settings.GetToolArguments().Invoke(args);
            Run(Settings, args);
        }


        public void SSH(string name = null)
        {
            SSH(name, null);
        }

        public void SSH(Action<VagrantSSHSettings> configure)
        {
            SSH(null, configure);
        }

        public void SSH(string name, Action<VagrantSSHSettings> configure)
        {
            var settings = new VagrantSSHSettings();
            configure?.Invoke(settings);
            var args = new ProcessArgumentBuilder();
            args.Append("ssh");
            if (name.HasValue()) args.Append(name);
            settings.GetToolArguments().Invoke(args);
            Run(Settings, args);
        }

        public void Destroy(string name = null, bool force = false)
        {
            var args = new ProcessArgumentBuilder();
            args.Append("destroy");
            if (name.HasValue()) args.Append(name);
            args.Append("--force");
        }

        public void Reload(string name = null)
        {
            Reload(name, null);
        }

        public void Reload(Action<VagrantReloadSettings> configure)
        {
            Reload(null, null);
        }

        public void Reload(string name, Action<VagrantReloadSettings> configure)
        {
            var settings = new VagrantReloadSettings();
            configure?.Invoke(settings);
            var args = new ProcessArgumentBuilder();
            args.Append("reload");
            if (name.HasValue()) args.Append(name);
            settings.GetToolArguments().Invoke(args);
            Run(Settings, args);
        }

        public void Share(Action<VagrantShareSettings> configure = null)
        {
            var settings = new VagrantShareSettings();
            configure?.Invoke(settings);
            var args = new ProcessArgumentBuilder();
            args.Append("share");
            settings.GetToolArguments().Invoke(args);
            Run(Settings, args);
        }

        //TODO: vagrant halt
        //TODO: vagrant suspend
    }
}