using System.Threading.Tasks;

namespace ISTraining_Part.Excel.Interfaces
{

    interface IAsyncExporter<T>
    {

        Task<bool> Export(T arg);
    }


    interface IExporter<T>
    {

        bool Export(T arg);
    }
}
