using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace Fizix {

  public readonly partial struct BoxF {

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static BoxF OverlapNaive(in BoxF a, in BoxF b)
      => new BoxF(
        Vector2.Min(a.TopLeft, b.TopLeft),
        Vector2.Max(a.BottomRight, b.BottomRight)
      );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static BoxF OverlapSse(in BoxF a, in BoxF b) {
      var min = Sse.Max(a, b);
      var max = Sse.Min(a, b);
      return Sse.Shuffle(min, max, 0b11_10_01_00);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoxF Overlap(in BoxF a, in BoxF b)
      => Sse.IsSupported
        ? OverlapSse(a, b)
        : OverlapNaive(a, b);

  }

}