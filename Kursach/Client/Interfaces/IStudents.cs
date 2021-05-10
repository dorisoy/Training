using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerMethods;

namespace ISTraining_Part.Client.Interfaces
{

    interface IStudents : IStudentsHub
    {

        event OnChanged<Student> OnChanged;


        event StudentsImported Imported;
    }
}
