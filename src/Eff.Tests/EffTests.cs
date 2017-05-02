﻿using Eff.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Eff.Tests
{
    public class EffTests
    {
        [Fact]
        public void SimpleReturn()
        {
            async Eff<int> Foo(int x)
            {
                return x + 1;
            }

            Assert.Equal(2, Foo(1).Result);
        }

        [Fact]
        public void AwaitEff()
        {
            async Eff<int> Bar(int x)
            {
                return x + 1;
            }
            async Eff<int> Foo(int x)
            {
                var y = await Bar(x);
                return y + 1;
            }

            Assert.Equal(3, Foo(1).Result);
        }

    }
}
