﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Vagrant.Settings;
using FluentAssertions;
using Xunit;

namespace Cake.Vagrant.Tests
{
    public class UpTests
    {
        [Fact]
        public void Should_Run_With_Defaults()
        {
            var fixture = new VagrantFixture(r => r.Up());
            var result = fixture.Run();
            result.Args.Should().Be("up");
        }

        [Fact]
        public void Should_Use_Name_When_Set()
        {
            var fixture = new VagrantFixture(r => r.Up("web"));
            var result = fixture.Run();
            result.Args.Should().Be("up web");
        }

        [Theory]
        [InlineData("web", "docker")]
        [InlineData("db", "hyperv")]
        public void Should_Use_Provider_When_Set(string imageName, string providerName)
        {
            var fixture = new VagrantFixture(r => r. Up(imageName, s => s.UseProvider(providerName)));
            var result = fixture.Run();
            result.Args.Should().Be($"up {imageName} --provider {providerName}");
        }

        [Fact]
        public void Should_Use_Provision_When_Explicitly_Set()
        {
            var fixture = new VagrantFixture(r => r.Up(s => s.RunProvisioners()));
            var result = fixture.Run();
            result.Args.Should().Be("up --provision");
        }

        [Fact]
        public void Should_Use_Provisioners()
        {
            var fixture = new VagrantFixture(r => r.Up(s => s.WithProvisioners("chef", "shell")));
            var result = fixture.Run();
            result.Args.Should().Be("up --provision --provision-with chef,shell");
        }

        [Fact]
        public void Should_Use_Parallel_When_Set()
        {
            var fixture = new VagrantFixture(r => r.Up(s => s.EnableParallel()));
            var result = fixture.Run();
            result.Args.Should().Be("up --parallel");
        }

        [Theory]
        [InlineData(true, "--destroy-on-error")]
        [InlineData(false, "--no-destroy-on-error")]
        public void Should_Use_Destroy_When_Set(bool enableDestroy, string flagValue)
        {
            var fixture = new VagrantFixture(r => r.Up(s => s.DestroyOnError(enableDestroy)));
            var result = fixture.Run();
            result.Args.Should().Be($"up {flagValue}");
        }

        [Theory]
        [InlineData(true, "--install-provider")]
        [InlineData(false, "--no-install-provider")]
        public void Should_Install_Provider_When_Set(bool enable, string flagValue)
        {
            var fixture = new VagrantFixture(r => r.Up(s => s.InstallProvider(enable)));
            var result = fixture.Run();
            result.Args.Should().Be($"up {flagValue}");
        }
    }
}
