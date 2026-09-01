namespace Blazor.Components.Pages
{
    public partial class Power
    {
        int number = 0;
        int degree = 0;

        double result = 0;
        void Exponentiation()
        {
            if (degree == 0)
            {
                result = 1;
            }
            else
            {
                result = 1;
                for(var i = 0; i < degree; i++)
			    {
                    result *= number;
                }
                for(var i = degree; i < 0; i++)
			    {
                    result /= number;
                }
                /*Отрицательная степень тогда мы делим одно число на другое.*/
            }
        }
    }
}
