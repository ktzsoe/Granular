﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using Granular.Extensions;

namespace System.Windows
{
    public class ResourceReferenceExpressionProvider : IExpressionProvider
    {
        private object resourceKey;

        public ResourceReferenceExpressionProvider(object resourceKey)
        {
            this.resourceKey = resourceKey;
        }

        public IExpression CreateExpression(DependencyObject dependencyObject, DependencyProperty dependencyProperty)
        {
            IResourceContainer resourceContainer = dependencyObject as IResourceContainer;

            if (resourceContainer == null)
            {
                throw new Granular.Exception("ResourceReferenceExpression cannot be attached to \"{0}\" as it does not implement \"{1}\"", dependencyObject.GetType().Name, typeof(IResourceContainer).Name);
            }

            return new ResourceReferenceExpression(resourceContainer, resourceKey);
        }
    }

    public class ResourceReferenceExpression : IExpression, IDisposable
    {
        public event EventHandler<ObservableValueChangedArgs> ValueChanged;

        public object Value { get { return observableValue.Value; } }

        private object resourceKey;
        private ObservableValue observableValue;
        private IResourceContainer resourceContainer;

        private IValueProvider currentValueProvider;
        private object currentValue;

        public ResourceReferenceExpression(IResourceContainer resourceContainer, object resourceKey)
        {
            this.resourceContainer = resourceContainer;
            this.resourceKey = resourceKey;

            observableValue = new ObservableValue();
            observableValue.ValueChanged += (sender, e) => ValueChanged.Raise(this, e);
            observableValue.Value = GetResourceValue();

            resourceContainer.ResourcesChanged += OnResourcesChanged;
        }

        public void Dispose()
        {
            observableValue.Value = ObservableValue.UnsetValue;
            resourceContainer.ResourcesChanged -= OnResourcesChanged;
        }

        private void OnResourcesChanged(object sender, ResourcesChangedEventArgs e)
        {
            if (e.Contains(resourceKey))
            {
                observableValue.Value = GetResourceValue();
            }
        }

        public bool SetValue(object value)
        {
            return false;
        }

        private object GetResourceValue()
        {
            object value;

            if (!resourceContainer.TryGetResource(resourceKey, out value))
            {
                return ObservableValue.UnsetValue;
            }

            if (!(value is IValueProvider))
            {
                return value;
            }

            if (currentValueProvider != value)
            {
                currentValueProvider = (IValueProvider)value;
                currentValue = currentValueProvider.ProvideValue();
            }

            return currentValue;
        }
    }
}