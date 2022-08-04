using UniViewV3.Messaging.Chaining;

namespace UniViewV3
{
    public interface IView
    {
        
    }

    public interface IView<T>
    {
        
    }

    public class ApiTester
    {
        public void Test(IViewSetup<ApiTesterModel> setup)
        {
            setup.Content("First Name", x => x.FirstName);

            setup.ModelConstructor<string, string, int>(
                "First Name", "Last Name", "Age", (firstName, lastName, age) => new ApiTesterModel(firstName, lastName, age));
        }
    }

    public struct ApiTesterModel
    {
        public string FirstName;
        public string LastName;
        public int Age;

        public ApiTesterModel(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }
    }
}