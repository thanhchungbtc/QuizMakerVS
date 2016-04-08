using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Model {
    public class Question {
        public int Id { get; set; }
        public string Description { get; set; }
        
        public List<Option> Options { get; set; }      
        public Question() {
            this.Options = new List<Option>();
        }

        public override string ToString() {
            return this.Description;
        }

        public Question Clone() {
            Question clone = new Question {
                Id = this.Id,
                Description = this.Description,
                Options = this.Options.ConvertAll<Option>(o => {
                    Option cloneO = new Option {
                        Description = o.Description,
                        IsCorrectAnswer = o.IsCorrectAnswer
                    };
                    return cloneO;
                })
            };
            return clone;
        }
    }
}
