namespace Crane.Core
{
  public class Euler : SolverBase
  {
    public Euler(Function function)
      : base(function)
    {

    }

    public override DoubleVectorExtended Execute(double um)
    {
      const double step = 0.0078;

      var temp = F(W, um);
      W[0] += temp[0]*step;
      W[1] += temp[1]*step;
      W[2] += temp[2]*step;
      W[3] += temp[3]*step;

      return W;// X0 => W[0], A0 => W[1];
    }
  }
}
