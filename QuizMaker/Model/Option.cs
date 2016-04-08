using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Model {
    public class Option {
        public string Description { get; set; }
        public bool IsCorrectAnswer { get; set; }

        public override string ToString() {
            return Description;
        }
    }
}
