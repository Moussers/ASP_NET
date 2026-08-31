namespace Blazor.Components.Pages
{
    public partial class Fibonacci
    {
        int a = 1;
        int b = 1;
        int c = 1;
        string res = "1 ";
        void fibonacciFunction()
        {
            while (a >= c)
            {
                int tmp = b + c;
                //сумма двух предыдущих чисел
                b = c;
                c = tmp;
                res += b.ToString() + " ";
                //складывается в массив
            }
        }
    }
}
