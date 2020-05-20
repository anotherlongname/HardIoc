using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using HardIoCTests.Models;
using Xunit;

namespace HardIoCTests
{
    public class UnitTests
    {
        [Fact]
        public void SingletonRegistrations_ShouldBeEquivalent()
        {
            var container = new TestContainer();
            var singleton1 = container.CreateISingletonClass();
            var singleton2 = container.CreateISingletonClass();

            singleton1.Should().Be(singleton2);
        }

        [Fact]
        public void TransientRegistrations_ShouldNotBeEquivalent()
        {
            var container = new TestContainer();
            var transient1 = container.CreateITransientClass();
            var transient2 = container.CreateITransientClass();

            transient1.Should().NotBe(transient2);
        }
    }
}
