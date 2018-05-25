using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutofacLab
{
    class Program
    {
        static void Main(string[] args)
        {
            BooModel boo = new BooModel(); //故意不給Name
            ShowValidationResult(boo);

            boo.Name = "Jeffrey Lee"; //故意讓Name過長
            boo.Score = 65535; //故意讓數字超出範圍
            ShowValidationResult(boo);

            boo.Name = "Jeffrey";
            boo.Score = 32767;
            ShowValidationResult(boo);

            Console.Read();
        }

        static void ShowValidationResult(BooModel boo)
        {
            ValidationContext vldCtx =
                new ValidationContext(boo, null, null);
            List<ValidationResult> errors =
                new List<ValidationResult>(); //檢核錯誤會被放入集合
            //注意第四個參數，要填true，才會檢核Required以外的ValidationAttribute
            //參數名稱(validateAllProperties)有誤導之嫌
            //已有網友在Connect反應: http://goo.gl/zllLw
            bool succ = Validator.TryValidateObject(boo, vldCtx, errors, true);
            Console.WriteLine("Boo {{ Name:{0} Score:{1} }}",
                boo.Name, boo.Score);
            Console.WriteLine("Pass? {0}", succ);
            foreach (ValidationResult r in errors)
                Console.WriteLine(" - Error: {0}", r.ErrorMessage);
        }
    }

    class BooModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(8, ErrorMessage = "Max length = 8")]
        public string Name { get; set; }

        [Range(0, 32767, ErrorMessage = "Range = 0-32767")]
        public int Score { get; set; }
    }
}
