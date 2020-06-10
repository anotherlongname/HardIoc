using System;

namespace HardIoC.CodeGenerators.Models
{
    internal struct Registration
    {
        private readonly RegistrationTypeEnum _registrationType;
        private readonly object _value;

        public Registration(TransientRegistration transientRegistration)
        {
            _registrationType = RegistrationTypeEnum.Transient;
            _value = transientRegistration;
        }

        public Registration(SingletonRegistration singletonRegistration)
        {
            _registrationType = RegistrationTypeEnum.Singleton;
            _value = singletonRegistration;
        }

        public Registration(DelegateRegistration delegateRegistration)
        {
            _registrationType = RegistrationTypeEnum.Delegate;
            _value = delegateRegistration;
        }

        public Registration(FactoryRegistration factoryRegistration)
        {
            _registrationType = RegistrationTypeEnum.Factory;
            _value = factoryRegistration;
        }

        public T Match<T>(
            Func<TransientRegistration, T> transientMatch,
            Func<SingletonRegistration, T> singletonMatch,
            Func<DelegateRegistration, T> delegateMatch,
            Func<FactoryRegistration, T> factoryMatch)
        {
            switch (_registrationType)
            {
                case RegistrationTypeEnum.Transient:
                    return transientMatch((TransientRegistration)_value);
                case RegistrationTypeEnum.Singleton:
                    return singletonMatch((SingletonRegistration)_value);
                case RegistrationTypeEnum.Delegate:
                    return delegateMatch((DelegateRegistration)_value);
                case RegistrationTypeEnum.Factory:
                    return factoryMatch((FactoryRegistration)_value);
                case RegistrationTypeEnum.Defaulted:
                    throw new Exception($"Default value not is invalid for {nameof(Registration)} type");
                default:
                    throw new NotImplementedException($"Not implemented for type {_registrationType}");
            }
        }

        private enum RegistrationTypeEnum { Defaulted, Transient, Singleton, Delegate, Factory }
    }
}
