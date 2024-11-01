using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Api.Extensions;

public sealed class SetPropertyBuilder<TSource>
{
    public Expression<Func<SetPropertyCalls<TSource>, SetPropertyCalls<TSource>>> SetPropertyCalls { get; private set; } = static b => b;


    public SetPropertyBuilder<TSource> SetProperty<TProperty>(Expression<Func<TSource, TProperty>> propertyExpression, TProperty value)
        => SetProperty(propertyExpression, _ => value);

    public SetPropertyBuilder<TSource> SetProperty<TProperty>(Expression<Func<TSource, TProperty>> propertyExpression, Expression<Func<TSource, TProperty>> valueExpression)
    {
        SetPropertyCalls = SetPropertyCalls.Update(
            body: Expression.Call(
                instance: SetPropertyCalls.Body,
                methodName: nameof(SetPropertyCalls<TSource>.SetProperty),
                typeArguments: new[] { typeof(TProperty) },
                arguments: new Expression[] {
                propertyExpression,
                valueExpression
                }
            ),
            parameters: SetPropertyCalls.Parameters
        );

        return this;
    }
}
