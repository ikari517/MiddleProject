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

        [Required(ErrorMessage = "Name is required.")]
        [ObservableProperty]
        private string? name;
        
        [Required(ErrorMessage = "Position is required.")]
        [ObservableProperty]
        private string? position;

        [Required(ErrorMessage = "Gender is required.")]
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
