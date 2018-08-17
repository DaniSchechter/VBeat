using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VBeat.Models.Validations
{
    public class AudioPlayerPathAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string path = (string)value;
            Regex rx = new Regex(@".*?\.(Ogg|MP3|AAC|Wav)", RegexOptions.IgnoreCase);
            Match m = rx.Match(path);
            if (m.Success)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("File type is not supported by the audio player");
            }
        }
    }
}
