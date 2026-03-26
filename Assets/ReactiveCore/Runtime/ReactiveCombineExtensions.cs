using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ReactiveCore.Runtime
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ReactiveCombineExtensions
    {
        public static IReactiveStream<bool[]> Combine(this ReactiveValue<bool> first, params ReactiveValue<bool>[] others)
        {
            return new ReactiveStream<bool[]>(observer =>
            {
                var count = 1 + others.Length;
                var values = new bool[count];

                values[0] = first.Value;
                for (var i = 0; i < others.Length; i++)
                {
                    values[i + 1] = others[i].Value;
                }

                void Emit()
                {
                    observer(values);
                }

                Emit();

                var subs = new List<IDisposable>();

                subs.Add(first.SkipValueOnSubscribe(v =>
                {
                    values[0] = v;
                    Emit();
                }));

                for (var i = 0; i < others.Length; i++)
                {
                    var index = i + 1;
                    var source = others[i];

                    subs.Add(source.SkipValueOnSubscribe(v =>
                    {
                        values[index] = v;
                        Emit();
                    }));
                }

                return new CompositeSubscription(subs);
            });
        }

        public static IReactiveStream<bool> CombineUsingAnd(this ReactiveValue<bool> first, params ReactiveValue<bool>[] others)
        {
            return first
                .Combine(others)
                .Map(values =>
                {
                    var result = true;
                    for (var i = 0; i < values.Length; i++)
                    {
                        result &= values[i];
                    }

                    return result;
                });
        }

        public static IReactiveStream<bool> CombineUsingOr(this ReactiveValue<bool> first, params ReactiveValue<bool>[] others)
        {
            return first
                .Combine(others)
                .Map(values =>
                {
                    var result = false;
                    for (var i = 0; i < values.Length; i++)
                    {
                        result |= values[i];
                    }

                    return result;
                });
        }

        public static IReactiveStream<(T1, T2)> Combine<T1, T2>(this ReactiveValue<T1> first, ReactiveValue<T2> second)
        {
            return new ReactiveStream<(T1, T2)>(observer =>
            {
                var v1 = first.Value;
                var v2 = second.Value;

                void Emit()
                {
                    observer((v1, v2));
                }

                Emit();

                var s1 = first.SkipValueOnSubscribe(x =>
                {
                    v1 = x;
                    Emit();
                });

                var s2 = second.SkipValueOnSubscribe(x =>
                {
                    v2 = x;
                    Emit();
                });

                return new CompositeSubscription(new List<IDisposable> { s1, s2 });
            });
        }

        public static IReactiveStream<(T1, T2, T3)> Combine<T1, T2, T3>(this ReactiveValue<T1> first, ReactiveValue<T2> second, ReactiveValue<T3> third)
        {
            return new ReactiveStream<(T1, T2, T3)>(observer =>
            {
                var v1 = first.Value;
                var v2 = second.Value;
                var v3 = third.Value;

                void Emit()
                {
                    observer((v1, v2, v3));
                }

                Emit();

                var s1 = first.SkipValueOnSubscribe(x =>
                {
                    v1 = x;
                    Emit();
                });

                var s2 = second.SkipValueOnSubscribe(x =>
                {
                    v2 = x;
                    Emit();
                });

                var s3 = third.SkipValueOnSubscribe(x =>
                {
                    v3 = x;
                    Emit();
                });

                return new CompositeSubscription(new List<IDisposable> { s1, s2, s3 });
            });
        }
    }
}