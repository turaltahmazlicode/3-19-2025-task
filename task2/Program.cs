using System.Text;

namespace task2
{
    internal class Program
    {

        static string Repeat(string str, int count)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append(str);
            }
            sb.AppendLine();
            return sb.ToString();
        }
        static int Avg(ref int[] arr)
        {
            int avg = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                avg += arr[i];
            }
            return avg / arr.Length;
        }
        static int MinIndex(ref int[] arr)
        {
            int min = arr[0], minIndex = 0;
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] < min)
                {
                    min = arr[i];
                    minIndex = i;
                }
            }
            return minIndex;
        }
        static void BubbleSort(ref int[] arr)
        {
            bool isSorted;
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                isSorted = true;
                for (int j = 0; j < i; j++)
                    if (arr[j] > arr[j + 1])
                    {
                        (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                        isSorted = false;
                    }
                if (isSorted)
                    break;
            }

        }
        static void Main(string[] args)
        {
            int[] arr = { 0, 50, -11, 23, 18 };
            foreach (var item in arr)
            {
                Console.WriteLine(item + " ");
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(Avg(ref arr));
            Console.WriteLine(MinIndex(ref arr));
            Console.WriteLine();

            BubbleSort(ref arr);
            foreach (var item in arr)
            {
                Console.WriteLine(item + " ");
            }
            Console.WriteLine();
            Console.WriteLine(Repeat("imanigga", 5));

        }
    }
}
