﻿using System;
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

        [Fact]
        public void DelegateRegistrations_ConstructorValuesAreAccessable()
        {
            var value = "Test Value";
            var container = new TestContainer(value);
            var stringDelegate = container.CreateStringDelegate();

            stringDelegate().Should().Be(value);
        }

        [Fact]
        public void MultipleConstructors_ConstructorWithMostSatisfiableDependenciesShouldBeSelected()
        {
            var container = new TestContainer();
            var multiConstructor = container.CreateMultiConstructorClass();

            multiConstructor.Value.Should().Be(MultiConstructorClass.GoodValue);
        }

        [Fact]
        public void FactoryWorks()
        {
            var container = new TestContainer();
            var factory = container.CreateIExampleFactory();
            var createdClass = factory.Create();
        }
 
    }
}
