using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using CannyFastMath;

namespace Fizix {

  public readonly partial struct BoxF {

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool FiniteNaive(in BoxF r)
      => float.IsFinite(MathF.FusedMultiplyAdd(r.X2, r.Y2, r.X1 * r.Y1));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool FiniteSse(in BoxF r) {
      var r1 = r;
      var r2 = Sse.Add(r1, Vector128.Create(1f));
      var r3 = Sse.Add(r2, r2);
      var x = Sse.CompareEqual(r1, r3);
      var m = Sse.MoveMask(x);
      return m == 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Finite(in BoxF r)
      => Sse.IsSupported
        ? FiniteSse(r)
        : FiniteNaive(r);

  }

}