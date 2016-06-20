﻿using System;
using Cake.Vagrant.Settings;
using FluentAssertions;
using Xunit;

namespace Cake.Vagrant.Tests
{
    public class PowerShellTests
    {
        [Theory]
        [InlineData("echo echo")]
        [InlineData("cat")]
        public void Should_Set_Command_When_Provided(string command)
        {
            var fixture = new VagrantFixture(r => r.PowerShell(s => s.RunCommand(command)));
            var result = fixture.Run();
            result.Args.Should().Be($"powershell --command \"{command}\"");
        }

        [Fact]
        public void Should_Fail_When_Command_Not_Provided()
        {
            var fixture = new VagrantFixture(r => r.PowerShell(s => s.RunCommand(null)));
            var result = Record.Exception(() => fixture.Run());
            result.Should().BeOfType<ArgumentOutOfRangeException>();
        }
    }
}