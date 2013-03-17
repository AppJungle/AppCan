//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace StockTraderRI.Modules.Position.Tests.Mocks
{
    class MockUnityResolver : IUnityContainer
    {
        public readonly Dictionary<Type, object> Bag = new Dictionary<Type, object>();

        public T Resolve<T>()
        {
            return (T)Bag[typeof(T)];
        }

        public IUnityContainer RegisterType<TFrom, TTo>(LifetimeManager lifetimeManager) where TTo : TFrom
        {
            //ignore
            return this;
        }

        public IUnityContainer RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            //ignore
            return this;
        }
        
//        #region Desktop IUnityContainer Members
//#if !SILVERLIGHT
//        IUnityContainer IUnityContainer.RegisterType<TFrom, TTo>(string name)
//        {
//            throw new System.NotImplementedException();
//        }

//        IUnityContainer IUnityContainer.RegisterType<TFrom, TTo>(string name, LifetimeManager lifetimeManager)
//        {
//            throw new System.NotImplementedException();
//        }

//        IUnityContainer IUnityContainer.RegisterType<T>(LifetimeManager lifetimeManager)
//        {
//            throw new System.NotImplementedException();
//        }

//        IUnityContainer IUnityContainer.RegisterType<T>(string name, LifetimeManager lifetimeManager)
//        {
//            throw new System.NotImplementedException();
//        }

//        IUnityContainer IUnityContainer.RegisterType(Type from, Type to)
//        {
//            throw new System.NotImplementedException();
//        }

//        IUnityContainer IUnityContainer.RegisterType(Type from, Type to, string name)
//        {
//            throw new System.NotImplementedException();
//        }

//        IUnityContainer IUnityContainer.RegisterType(Type from, Type to, LifetimeManager lifetimeManager)
//        {
//            throw new System.NotImplementedException();
//        }

//        IUnityContainer IUnityContainer.RegisterType(Type t, LifetimeManager lifetimeManager)
//        {
//            throw new System.NotImplementedException();
//        }

//        IUnityContainer IUnityContainer.RegisterType(Type t, string name, LifetimeManager lifetimeManager)
//        {
//            throw new System.NotImplementedException();
//        }

//        IUnityContainer IUnityContainer.RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager)
//        {
//            throw new System.NotImplementedException();
//        }
//#endif
//        #endregion
        
        #region Silverlight IUnityContainer Members
        

        
        #endregion

        #region Common IUnityContainer Members
       
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion


        public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ContainerRegistration> Registrations
        {
            get { throw new NotImplementedException(); }
        }

        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            throw new NotImplementedException();
        }
    }
}