using System;
using System.Collections.Generic;
using System.Reflection;
using FubuMVC.Core.Runtime;

namespace FubuMVC.Core.Models
{
    public interface IBindingContext
    {
        IList<ConvertProblem> Problems { get; }
        object PropertyValue { get; }

        PropertyInfo Property { get; }
        object Object { get; }
        T Service<T>();
        IBindingContext PrefixWith(string prefix);
        void ForProperty(PropertyInfo property, Action action);
        void LogProblem(Exception ex);
        void LogProblem(string exceptionText);

        void StartObject(object @object);
        void FinishObject();
        
    }
}