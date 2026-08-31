namespace Blazor.Components.Pages
{
    public partial class Counter
    {
        private int currentCount = 0;
        int increment = 1;
        /*поля в классе по умолчанию private*/

        private void IncrementCount()
        {
            currentCount += increment;
        }
        void Reset()
        {
            currentCount = 0;
        }
    }
}
