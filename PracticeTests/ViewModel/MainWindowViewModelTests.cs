using Practice.Enum;
using Practice.Model;

namespace Practice.ViewModel.Tests
{
    [TestClass()]
    public class MainWindowViewModelTests
    {
        MainWindowViewModel viewModel;

        [TestInitialize]
        public void init()
        {
            viewModel = new MainWindowViewModel();
        }

        [TestMethod()]
        public void SelectEmployeeListTestFail()
        {
            Gender searchGender = Gender.전체; // 조회할 성별
            Gender expectGender = Gender.여자; // 조회된 성별
            
            // 조회할 성별: "전체"
            EmployeeModel condition = new EmployeeModel() { Gender = searchGender };

            List<EmployeeModel> empList = viewModel.SelectEmployeeList(condition).ToList();

            // 조회한 사원의 성별이 모두 "여자" 인지
            Assert.IsTrue(empList.All(emp => emp.Gender == expectGender));
        }

        [TestMethod()]
        public void SelectEmployeeListTestSuccess()
        {
            Gender searchGender = Gender.여자; // 조회할 성별
            Gender expectGender = Gender.여자; // 조회된 성별

            // 조회할 성별: "여자"
            EmployeeModel condition = new EmployeeModel() { Gender = searchGender };

            List<EmployeeModel> empList = viewModel.SelectEmployeeList(condition).ToList();

            // 조회한 사원의 성별이 모두 "여자" 인지
            Assert.IsTrue(empList.All(emp => emp.Gender == expectGender));
        }
    }
}