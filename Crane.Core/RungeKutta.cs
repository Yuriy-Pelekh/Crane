namespace Crane.Core
{
  public class RungeKutta : SolverBase
  {
    private DoubleVectorExtended k1;
    private DoubleVectorExtended k2;
    private DoubleVectorExtended k3;
    private DoubleVectorExtended k4;
    private const double H = 0.0078;

    public RungeKutta(Function function)
      : base(function)
    {

    }

    private DoubleVectorExtended K1(double um)
    {
      return (k1 = H*F(W, um));
    }

    private DoubleVectorExtended K2(double um)
    {
      return (k2 = H*F(W + 0.5*k1, um));
    }

    private DoubleVectorExtended K3(double um)
    {
      return (k3 = H*F(W + 0.5*k2, um));
    }

    private DoubleVectorExtended K4(double um)
    {
      return (k4 = H*F(W + k3, um));
    }

    public override DoubleVectorExtended Execute(double um)
    {
      //t += H;
      return W + 1.0/6.0*(K1(um) + 2*K2(um) + 2*K3(um) + K4(um));
    }
  }
}
