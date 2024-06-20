using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;
using Practice.Enum;

namespace Practice.Model
{
    public partial class EmployeeModel : ObservableValidator
    {
        [NotMapped]
        private bool isChecked = false;
        [NotMapped]
        public bool IsChecked
        {
            get => isChecked;
            set => SetProperty(ref isChecked, value);
        }

        [Key]
        [ObservableProperty]
        private int? id;

        [Required(ErrorMessage = "이름은 필수 입력값입니다.")]
        [StringLength(10, ErrorMessage = "이름은 10글자이내여야 합니다.")]
        [ObservableProperty]
        private string? name;
        
        [Required(ErrorMessage = "직위는 필수 입력값입니다.")]
        [StringLength(10, ErrorMessage = "직위는 10글자이내여야 합니다.")]
        [ObservableProperty]
        private string? position;

        [Required(ErrorMessage = "성별은 필수 입력값입니다.")]
        [Range(0, 1, ErrorMessage = "유효한 성별값이 아닙니다.")]
        [ObservableProperty]
        private Gender? gender;

        internal bool IsValid()
        {
            ClearErrors();
            ValidateAllProperties();
            return !HasErrors;
        }
    }
}
