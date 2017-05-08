# Eff
A library design for programming with effects and handlers in C#, inspired by the [Eff] programming language and the implementation of Algebraic Effects in [OCaml], [Eff Directly in OCaml].

``` csharp
// Effect example
async Eff<DateTime> Foo()
{
    var y = await Effect.Random();
    return y;
}
    
// Effect handler
public class EffectHandler : IEffectHandler
{
    private readonly Random random;
    public EffectHandler(Random random)
    {
        this.random = random;
    }

    public async ValueTask<ValueTuple> Handle<TResult>(IEffect<TResult> effect)
    {
        switch (effect)
        {
            case RandomEffect randomEffect:
                randomEffect.SetResult(random.Next());
                break;
        }

        return ValueTuple.Create();
    }
}

// Apply state effect and execute
EffectExecutionContext.Handler = new EffectHandler(new Random());
var x = Foo().Result;
```


[Eff]: http://math.andrej.com/wp-content/uploads/2012/03/eff.pdf
[OCaml]: http://www.lpw25.net/ocaml2015-abs2.pdf
[Eff Directly in OCaml]: http://kcsrk.info/papers/eff_ocaml_ml16.pdf
