﻿using Cake.Core;
using Cake.Core.IO;

namespace Cake.Vagrant
{
    internal static class CoreExtensions
    {
        internal static bool HasValue(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        internal static void Add(this ProcessArgumentBuilder args, bool trigger, string template,
            bool enableNegation = false)
        {
            args.Append(
                enableNegation
                    ? $"{(template.StartsWith("-") ? string.Empty : "--")}{(trigger ? string.Empty : "no-")}{template}"
                    : $"{(template.StartsWith("-") ? string.Empty : "--")}{template}");
        }

        internal static void Add(this ProcessArgumentBuilder args, bool? trigger, string template,
            bool enableNegation = false)
        {
            if (trigger.HasValue)
            {
                Add(args, trigger, template, enableNegation);
            }
        }

        internal static void Add(this ProcessArgumentBuilder args, string value, string key = null)
        {
            if (!value.HasValue()) return;
            if (key == null)
            {
                args.Append(value);
            }
            else
            {
                args.AppendSwitch(key.StartsWith("-") ? key : $"--{key}", value);
            }
        }

        internal static void Add(this ProcessArgumentBuilder args, int? value, string key = null)
        {
            if (!value.HasValue) return;
            if (key == null)
            {
                args.Append(value.Value.ToString());
            }
            else
            {
                args.AppendSwitch(key.StartsWith("-") ? key : $"--{key}", value.Value.ToString());
            }
        }

        internal static void AppendAll(this ProcessArgumentBuilder args, params string[] arguments)
        {
            foreach (var arg in arguments)
            {
                args.Append(arg);
            }
        }
    }
}