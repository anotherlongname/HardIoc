using System;

namespace HardIoC.CodeGenerators.Models
{
    internal struct DependencyNode
    {
        private readonly DependencyNodeTypeEnum _dependencyNodeType;
        private readonly object _value;

        public DependencyNode(TransientDependencyNode transientDependencyNode)
        {
            _dependencyNodeType = DependencyNodeTypeEnum.Transient;
            _value = transientDependencyNode;
        }

        public DependencyNode(SingletonDependencyNode singletonDependencyNode)
        {
            _dependencyNodeType = DependencyNodeTypeEnum.Singleton;
            _value = singletonDependencyNode;
        }

        public DependencyNode(DelegateDependencyNode delegateDependencyNode)
        {
            _dependencyNodeType = DependencyNodeTypeEnum.Delegate;
            _value = delegateDependencyNode;
        }

        public DependencyNode(FactoryDependencyNode factoryDependencyNode)
        {
            _dependencyNodeType = DependencyNodeTypeEnum.Factory;
            _value = factoryDependencyNode;
        }

        public T Match<T>(
            Func<TransientDependencyNode, T> transientMatch,
            Func<SingletonDependencyNode, T> singletonMatch,
            Func<DelegateDependencyNode, T> delegateMatch,
            Func<FactoryDependencyNode, T> factoryMatch)
        {
            switch (_dependencyNodeType)
            {
                case DependencyNodeTypeEnum.Transient:
                    return transientMatch((TransientDependencyNode)_value);
                case DependencyNodeTypeEnum.Singleton:
                    return singletonMatch((SingletonDependencyNode)_value);
                case DependencyNodeTypeEnum.Delegate:
                    return delegateMatch((DelegateDependencyNode)_value);
                case DependencyNodeTypeEnum.Factory:
                    return factoryMatch((FactoryDependencyNode)_value);
                case DependencyNodeTypeEnum.Defaulted:
                    throw new Exception($"Default value not is invalid for {nameof(DependencyNode)} type");
                default:
                    throw new NotImplementedException($"Not implemented for type {_dependencyNodeType}");
            }
        }

        private enum DependencyNodeTypeEnum { Defaulted, Transient, Singleton, Delegate, Factory }
    }
}
