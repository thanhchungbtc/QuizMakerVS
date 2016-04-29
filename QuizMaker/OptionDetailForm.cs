using QuizMaker.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuizMaker {
    public partial class OptionDetailForm : Form {        

        public Option Option { get; set; }
        public IOptionDetailFormDelegate formDelegate;
        public OptionDetailForm(Option option) {
            InitializeComponent();
            Option = option;
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (Option == null) {
                Option = new Option {
                    Description = txtDescription.Text.Trim(),
                    IsCorrectAnswer = chkIsCorrectOption.Checked
                };
                formDelegate.submitOptionWithMode(Option, false);
                return;
            }
            formDelegate.submitOptionWithMode(Option, true);
        }
    }

    public interface IOptionDetailFormDelegate {
        void submitOptionWithMode(Option option, bool isEditMode);
    }
}
