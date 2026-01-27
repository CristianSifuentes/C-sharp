using System.Threading;
using System.Threading.Tasks;
using MatrixMul;
using Xunit;

namespace Matrix.Test;

public class MatrixTest
{
    [Fact]
    public async Task MulConcurrent_SmallKnown()
    {
        var a = Matrix<double>.FromRowMajor(2, 3, new double[]
        {
            1, 2, 3, 
            4, 5, 6
        });

        var b = Matrix<double>.FromRowMajor(3, 2, new double[]
        {
            7, 8, 
            9, 10,
            11, 12    
        });

        var c = await MatrixMultiply.MulConcurrent(
            a, b, CancellationToken.None,
            MulOption.WithWorkers(2), 
            MulOption.WithBlockRows(1));

        Assert.Equal(58, c.At(0, 0));
        Assert.Equal(64, c.At(0, 1));
        Assert.Equal(139, c.At(1,0));
        Assert.Equal(154, c.At(1,1));
        
        
    }
}
