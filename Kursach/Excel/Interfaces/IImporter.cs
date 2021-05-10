using System.Threading.Tasks;

namespace ISTraining_Part.Excel.Interfaces
{

    interface IAsyncImporter<T>
    {

        Task<T> Import();
    }


    interface IImporter<T>
    {

        T Import();
    }
}
