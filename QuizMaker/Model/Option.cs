using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Model {
    public class Option {
        public string Description { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public string ImageUrl { get; set; }

        public override string ToString() {
            return Description;
        }
        public Option Clone() {
            Option clone = new Option {             
                Description = this.Description,
                ImageUrl = this.ImageUrl,
                IsCorrectAnswer = this.IsCorrectAnswer        
            };
            return clone;
        }
    }
}
