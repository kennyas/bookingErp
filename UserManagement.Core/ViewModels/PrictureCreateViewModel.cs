﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace UserManagement.Core.ViewModels
{
    public class PictureCreateViewModel:IValidatableObject
    {
        public IFormFile File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validExt = new[] { ".png", ".gif", ".jpg" };

            if (File is null || File.Length <= 0 || string.IsNullOrWhiteSpace(File.FileName))
            {
                yield return new ValidationResult("Please upload a valid file.");
            }
            else
            {
                var extension = Path.GetExtension(File.FileName).ToLower();

                if (!validExt.Any(x => x.Equals(extension)))
                    yield return new ValidationResult("Invalid file type. Please select a valid file.");
            }
        }
    }
}