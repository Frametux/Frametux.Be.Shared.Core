using System.Linq.Expressions;
using Frametux.Shared.Core.Domain.ValueObjs.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frametux.Shared.Core.Driven.Persistence.EntityConfigs.Conversions;

public static class SinglePropValueObjConversion
{
    private static readonly Dictionary<Type, Delegate> ConstructorCache = new();
    private static readonly Lock Lock = new();

    /// <summary>
    /// Generic conversion for non-nullable value objects that implement ISinglePropValueObj
    /// </summary>
    /// <typeparam name="TValueObj">The value object type</typeparam>
    /// <typeparam name="TValue">The underlying value type</typeparam>
    public static PropertyBuilder<TValueObj> HasValueObjConversion<TValueObj, TValue>(
        this PropertyBuilder<TValueObj> builder)
        where TValueObj : ISinglePropValueObj<TValue>
    {
        var constructor = GetOrCreateConstructor<TValueObj, TValue>();
        
        return builder.HasConversion(
            valueObj => valueObj.Value,
            value => constructor(value)
        );
    }

    /// <summary>
    /// Generic conversion for nullable value objects that implement ISinglePropValueObj
    /// </summary>
    /// <typeparam name="TValueObj">The value object type</typeparam>
    /// <typeparam name="TValue">The underlying value type</typeparam>
    public static PropertyBuilder<TValueObj?> HasValueObjNullableConversion<TValueObj, TValue>(
        this PropertyBuilder<TValueObj?> builder)
        where TValueObj : ISinglePropValueObj<TValue>
    {
        var constructor = GetOrCreateConstructor<TValueObj, TValue>();
        
        return builder.HasConversion<TValue?>(
            valueObj => valueObj != null ? valueObj.Value : default,
            value => value != null ? constructor(value) : default
        );
    }

    private static Func<TValue, TValueObj> GetOrCreateConstructor<TValueObj, TValue>()
        where TValueObj : ISinglePropValueObj<TValue>
    {
        var key = typeof(TValueObj);
        
        // ReSharper disable once InconsistentlySynchronizedField
        if (ConstructorCache.TryGetValue(key, out var cached))
            return (Func<TValue, TValueObj>)cached;
        
        lock (Lock)
        {
            if (ConstructorCache.TryGetValue(key, out cached))
                return (Func<TValue, TValueObj>)cached;
        
            var valueObjType = typeof(TValueObj);
            var valueType = typeof(TValue);
            
            // Find the constructor that takes a single parameter of TValue
            var constructorInfo = valueObjType.GetConstructor([valueType]);
            
            if (constructorInfo == null)
            {
                throw new InvalidOperationException(
                    $"Type {valueObjType.Name} must have a constructor that accepts a single parameter of type {valueType.Name}");
            }
        
            // Build expression: value => new TValueObj(value)
            var parameter = Expression.Parameter(valueType, "value");
            var newExpression = Expression.New(constructorInfo, parameter);
            var lambda = Expression.Lambda<Func<TValue, TValueObj>>(newExpression, parameter);
            var compiled = lambda.Compile();
        
            ConstructorCache[key] = compiled;
            
            return compiled;
        }
    }
}