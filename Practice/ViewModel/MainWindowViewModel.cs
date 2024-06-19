using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.IdentityModel.Tokens;
using Practice.DataSource;
using Practice.Enum;
using Practice.Extensions;
using Practice.Model;

namespace Practice.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        /// <summary>
        /// init
        /// </summary>
        public MainWindowViewModel()
        {
            MyDB = new MyDBContext();

            NewEmployee = new EmployeeModel() { Gender = Gender.남자 };
            SearchCondition = new EmployeeModel() { Gender = Gender.전체 };

            EmployeeList = SelectEmployeeList(SearchCondition);
        }

        /// <summary>
        /// [Data Source] 
        /// DB Context
        /// </summary>
        private readonly MyDBContext MyDB;

        /// <summary>
        /// [Binding param] 
        /// 사원 목록 (Not Mapped)
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<EmployeeModel>? employeeList;

        /// <summary>
        /// [Binding param] 
        /// 전체 체크
        /// </summary>
        [ObservableProperty]
        private bool allChecked = false;

        /// <summary>
        /// [Binding param] 
        /// 추가할 사원 
        /// </summary>
        [ObservableProperty]
        private EmployeeModel? newEmployee;
        /// <summary>
        /// [Binding param] 
        /// 검색 조건
        /// </summary>
        [ObservableProperty]
        private EmployeeModel? searchCondition;

        /// <summary>
        /// [Binding param] 
        /// 성별 목록
        /// </summary>
        [ObservableProperty]
        private IList<Gender>? genderTypeList = new List<Gender>() { Gender.남자, Gender.여자 };
        /// <summary>
        /// [Binding param] 
        /// 검색조건 성별 목록
        /// </summary>
        [ObservableProperty]
        private IList<Gender>? seachGenderTypeList = new List<Gender>() { Gender.전체, Gender.남자, Gender.여자 };

        /// <summary>
        /// [Binding Command] 
        /// 전체 체크 클릭
        /// </summary>
        [RelayCommand]
        public void AllCheck_Click()
        {
            foreach (EmployeeModel emp in EmployeeList)
            {
                emp.IsChecked = allChecked;
            }
        }

        /// <summary>
        /// [Binding Command] 
        /// 사원 개별 체크 클릭
        /// </summary
        [RelayCommand]
        public void Check_Click()
        {
            if (EmployeeList.All(i => i.IsChecked))
            {
                AllChecked = true;
            }
            else if (EmployeeList.Any(i => !i.IsChecked))
            {
                AllChecked = false;
            }
        }

        /// <summary>
        ///  [Binding Command] 
        ///  검색 버튼 클릭
        /// </summary>
        [RelayCommand]
        private void SearchButton_Click()
        {
            try
            {
                EmployeeList = SelectEmployeeList(SearchCondition);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// [Binding Command] 
        /// 추가 버튼 클릭
        /// </summary>
        [RelayCommand]
        private void AddButton_Click()
        {
            try
            {
                if (!(MessageBox.Show("사원을 추가하시겠습니까?", "사원 추가", MessageBoxButton.YesNo) == MessageBoxResult.Yes)) return;

                // 추가
                bool isCreateSucces = CreateEmployee(NewEmployee);

                // 이후
                if (isCreateSucces)
                {
                    NewEmployee = new EmployeeModel() { Gender = 0 };

                    EmployeeList = SelectEmployeeList(SearchCondition);
                    MessageBox.Show("추가 완료");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// [Binding Command] 
        /// 수정 버튼 클릭
        /// </summary>
        [RelayCommand]
        private void ModifyButton_Click()
        {
            try
            {
                if (!(MessageBox.Show("사원목록을 수정하시겠습니까?", "사원 수정", MessageBoxButton.YesNo) == MessageBoxResult.Yes)) return;

                bool isUpdateSucces = UpdateEmployeeList(EmployeeList);

                if (isUpdateSucces)
                {
                    EmployeeList = SelectEmployeeList(SearchCondition);
                    MessageBox.Show("수정 완료");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// [Binding Command] 
        /// 삭제 버튼 클릭
        /// </summary>
        [RelayCommand]
        private void DeleteButton_Click()
        {
            try
            {
                if (!(MessageBox.Show("체크된 사원목록을 삭제하시겠습니까?", "사원 삭제", MessageBoxButton.YesNo) == MessageBoxResult.Yes)) return;

                bool isDeleteSucces = DeleteEmployeeList(EmployeeList);

                if (isDeleteSucces)
                {
                    EmployeeList = SelectEmployeeList(SearchCondition);
                    MessageBox.Show("삭제 완료");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// [Business Logic] 
        /// 사원 목록 조회
        /// </summary>
        /// <param name="condition">조회 조건</param>
        /// <returns></returns>
        public ObservableCollection<EmployeeModel> SelectEmployeeList(EmployeeModel condition)
        {
            ObservableCollection<EmployeeModel> UIEmployeeList = new ObservableCollection<EmployeeModel>();

            if (condition == null) return UIEmployeeList;

            // ORM List 조회
            var ORMEmployeeList = MyDB.tb_employee.Where(emp => (string.IsNullOrWhiteSpace(condition.Name) || emp.Name.Contains(condition.Name))
                                                                && (string.IsNullOrWhiteSpace(condition.Position) || emp.Position.Contains(condition.Position))
                                                                && (condition.Gender == Gender.전체 || emp.Gender == condition.Gender))
                                                  .Select(emp => new EmployeeModel()
                                                  {
                                                      Id = emp.Id,
                                                      Name = emp.Name,
                                                      Position = emp.Position, 
                                                      Gender = emp.Gender, 
                                                  });

            // 객체 깊은복사 (ORM List -> UI List)
            foreach (EmployeeModel ORMEmployee in ORMEmployeeList)
            {
                EmployeeModel UIEmployee = ORMEmployee.DeepCopy();
                UIEmployeeList.Add(UIEmployee);
            };

            return UIEmployeeList;
        }

        /// <summary>
        /// [Business Logic] 
        /// 사원 추가
        /// </summary>
        /// <param name="newEmployee">추가할 사원</param>
        /// <returns></returns>
        private bool CreateEmployee(EmployeeModel newEmployee)
        {
            // Validate
            if (newEmployee == null) return false;
            if (!newEmployee.IsValid())
            {
                throw new InvalidDataException("입력정보가 유효하지 않습니다.\n" 
                                            + newEmployee.GetErrors().First().ErrorMessage);
            }

            // Business Logic
            MyDB.tb_employee.Add(newEmployee);
            MyDB.SaveChanges();
            
            return true;
        }

        /// <summary>
        /// [Business Logic] 
        /// 사원 목록 수정
        /// </summary>
        /// <param name="UIEmployeeList">수정된 UI 목록</param>
        /// <returns></returns>
        private bool UpdateEmployeeList(ObservableCollection<EmployeeModel> UIEmployeeList)
        {
            // Validate
            if (UIEmployeeList.IsNullOrEmpty()) return false;
            foreach(EmployeeModel UIEmployee in UIEmployeeList)
            {
                if (!UIEmployee.IsValid())
                {
                    throw new InvalidDataException($"사원(Id: {UIEmployee.Id})의 입력정보가 유효하지 않습니다.\n"
                                                + UIEmployee.GetErrors().First().ErrorMessage);
                }
            }

            // 변경점 반영 (UI List -> ORM List)
            foreach (EmployeeModel ORMEmployee in MyDB.tb_employee)
            {
                foreach(EmployeeModel UIEmployee in UIEmployeeList)
                {
                    if (ORMEmployee.Id == UIEmployee.Id)
                    {
                        ORMEmployee.PropertyCopy(UIEmployee);
                        break;
                    }
                }
            }

            MyDB.SaveChanges();

            return true;
        }

        /// <summary>
        /// [Business Logic] 
        /// 사원 목록 삭제
        /// </summary>
        /// <param name="UIEmployeeList">체크된(삭제할) UI 목록</param>
        /// <returns></returns>
        private bool DeleteEmployeeList(ObservableCollection<EmployeeModel> UIEmployeeList)
        {
            if (UIEmployeeList.IsNullOrEmpty()) return false;
            
            // CheckedIdList - 체크된(삭제할) Id(pk) 리스트
            List<int> CheckedIdList = new List<int>();
            foreach (EmployeeModel UIEmployee in UIEmployeeList.Where(emp => emp.IsChecked))
            {
                CheckedIdList.Add((int)UIEmployee.Id);
            }
            if (CheckedIdList.IsNullOrEmpty()) throw new Exception("삭제할 사원을 선택해주세요.");

            // deleteList - 삭제할 Id(pk)를 가진 ORM List
            var deleteEmployeeList = MyDB.tb_employee.Where(emp => CheckedIdList.Contains((int)emp.Id)).ToList();

            MyDB.tb_employee.RemoveRange(deleteEmployeeList);
            MyDB.SaveChanges();

            return true;
        }
    }
}
