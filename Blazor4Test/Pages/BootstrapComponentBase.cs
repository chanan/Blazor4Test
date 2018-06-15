using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace Blazor4Test.Pages
{
    public abstract class BootstrapComponentBase : BlazorComponent
    {
        /// <summary>
        /// A dictionary holding any parameter name/value pairs that do not match
        /// properties declared on the component.
        /// </summary>
        protected IDictionary<string, object> UnknownParameters { get; }
            = new Dictionary<string, object>();

        public Action<UIMouseEventArgs> OnClick { get; set; }

        /// <inheritdoc />
        public override void SetParameters(ParameterCollection parameters)
        {
            UnknownParameters.Clear();

            foreach (var param in parameters)
            {
                if (TryGetPropertyInfo(param.Name, out var declaredPropertyInfo))
                {
                    declaredPropertyInfo.SetValue(this, param.Value);
                }
                else
                {
                    UnknownParameters[param.Name] = param.Value;
                }
            }

            StateHasChanged();
        }

        private bool TryGetPropertyInfo(string propertyName, out PropertyInfo result)
        {
            result = GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            return result != null;
        }
    }
}
